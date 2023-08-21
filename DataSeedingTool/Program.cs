using CsvHelper;
using CsvHelper.Configuration;

using DataSeedingTool.Configuration;
using DataSeedingTool.Extensions;
using DataSeedingTool.Mappers;

using Infrastructure.Database.Entities;
using Infrastructure.Database.Entities.Enums;

using Microsoft.Extensions.Configuration;

using NetTopologySuite.Features;
using NetTopologySuite.IO;

using Npgsql;

using NpgsqlTypes;

using PostgreSQLCopyHelper;

using System.Globalization;

var configuration = ConfigurationFactory.CreateConfiguration();
var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, eventArgs) =>
{
    Console.WriteLine("Canceling...");
    cancellationTokenSource.Cancel();
    eventArgs.Cancel = true;
};

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var connectionString = configuration.GetConnectionString("DefaultConnection");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.UseNetTopologySuite();
await using var dataSource = dataSourceBuilder.Build();
await using var connection = dataSource.CreateConnection();

try
{
    await connection.OpenAsync(cancellationTokenSource.Token);

    if (configuration.GetValue<bool>("SeedTrafficAccidentsData"))
    {
        #region Import Municipalities

        var municipalitiesFilePath = configuration.GetSection("Datasets:Municipalities").Get<string>()!;
        using var municipalitiesReader = new StreamReader(municipalitiesFilePath);
        var municipalitiesGeoJson = await municipalitiesReader.ReadToEndAsync(cancellationTokenSource.Token);

        var geoJsonReader = new GeoJsonReader();

        var municipalitiesFeatureCollection = geoJsonReader.Read<FeatureCollection>(municipalitiesGeoJson)
                                              ?? throw new InvalidOperationException(
                                                  "Provided Feature Collection is empty!");

        var municipalities = municipalitiesFeatureCollection.GetMunicipalities().ToList();

        var municipalitiesBulkCopyHelper = new PostgreSQLCopyHelper<MunicipalityDataModel>("public", "municipalities")
            .MapUUID("municipality_id", x => x.MunicipalityId)
            .MapText("municipality_name", x => x.MunicipalityName)
            .Map("municipality_area", x => x.MunicipalityArea, NpgsqlDbType.Geometry);

        var rowsAffected = await municipalitiesBulkCopyHelper.SaveAllAsync(connection, municipalities, cancellationTokenSource.Token);

        Console.WriteLine($"Imported {rowsAffected} municipalities.");

        #endregion Import Municipalities

        #region Import Settlements

        var settlementsFilePath = configuration.GetSection("Datasets:Settlements").Get<string>()!;
        using var settlementsReader = new StreamReader(settlementsFilePath);
        var settlementsGeoJson = await settlementsReader.ReadToEndAsync(cancellationTokenSource.Token);

        var settlementsFeatureCollection = geoJsonReader.Read<FeatureCollection>(settlementsGeoJson)
                                              ?? throw new InvalidOperationException(
                                                  "Provided Feature Collection is empty!");

        var settlements = settlementsFeatureCollection.GetSettlements().ToList();

        var settlementsBulkCopyHelper = new PostgreSQLCopyHelper<SettlementDataModel>("public", "settlements")
            .MapUUID("settlement_id", x => x.SettlementId)
            .MapText("settlement_name", x => x.SettlementName)
            .Map("settlement_area", x => x.SettlementArea, NpgsqlDbType.Geometry);

        rowsAffected = await settlementsBulkCopyHelper.SaveAllAsync(connection, settlements, cancellationTokenSource.Token);

        Console.WriteLine($"Imported {rowsAffected} settlements.");

        #endregion Import Settlements

        #region Import Cities

        var citiesFilePath = configuration.GetSection("Datasets:Cities").Get<string>()!;
        using var citiesReader = new StreamReader(citiesFilePath);
        var citiesGeoJson = await citiesReader.ReadToEndAsync(cancellationTokenSource.Token);

        var citiesFeatureCollection = geoJsonReader.Read<FeatureCollection>(citiesGeoJson)
                                      ?? throw new InvalidOperationException(
                                          "Provided Feature Collection is empty!");

        var cities = citiesFeatureCollection.GetCities().ToList();

        var citiesBulkCopyHelper = new PostgreSQLCopyHelper<CityDataModel>("public", "cities")
            .MapUUID("city_id", x => x.CityId)
            .MapText("city_name", x => x.CityName)
            .Map("city_area", x => x.CityArea, NpgsqlDbType.Geometry);

        rowsAffected = await citiesBulkCopyHelper.SaveAllAsync(connection, cities, cancellationTokenSource.Token);

        Console.WriteLine($"Imported {rowsAffected} cities.");

        #endregion Import Cities

        #region Import Traffic Accidents Data

        foreach (var path in configuration.GetSection("Datasets:TrafficAccidents").Get<IEnumerable<string>>()!)
        {
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ","
            });

            csv.Context.TypeConverterCache.AddConverter<DateTime>(new ReportedOnDateTimeConverter());
            csv.Context.TypeConverterCache.AddConverter<DataParticipantsStatus>(new ParticipantStatusEnumConverter());
            csv.Context.TypeConverterCache.AddConverter<DataParticipantsNominalCount>(
                new ParticipantsNominalCountEnumConverter());
            csv.Context.TypeConverterCache.AddConverter<DataAccidentType>(new AccidentTypeEnumConverter());

            csv.Context.RegisterClassMap<CsvRecordToTrafficAccidentDataModelMapper>();

            var records = csv.GetRecords<TrafficAccidentDataModel>();
            var trafficAccidents = records.ToList();

            foreach (var trafficAccident in trafficAccidents)
            {
                var municipality =
                    municipalities.FirstOrDefault(x => x.MunicipalityArea.Covers(trafficAccident.AccidentLocation))
                        ?? municipalities.FirstOrDefault(x => 
                        x.MunicipalityName.Contains(trafficAccident.Municipality!.MunicipalityName));

                if (municipality is not null) trafficAccident.MunicipalityId = municipality.MunicipalityId;
            }

            var trafficAccidentsBulkCopyHelper =
                new PostgreSQLCopyHelper<TrafficAccidentDataModel>("public", "traffic_accidents")
                    .MapUUID("traffic_accident_id", _ => Guid.NewGuid())
                    .MapText("case_number", x => x.CaseNumber)
                    .MapText("police_department", x => x.PoliceDepartment)
                    .MapTimeStampTz("reported_on", x => x.ReportedOn)
                    .Map("accident_location", x => x.AccidentLocation, NpgsqlDbType.Geometry)
                    .MapInteger("participants_status", x => (int)x.ParticipantsStatus)
                    .MapInteger("participants_nominal_count", x => (int)x.ParticipantsNominalCount)
                    .MapInteger("accident_type", x => (int)x.AccidentType)
                    .MapText("description", x => x.Description)
                    .MapUUID("municipality_id", x => x.MunicipalityId);

            rowsAffected = await trafficAccidentsBulkCopyHelper.SaveAllAsync(connection, trafficAccidents,
                cancellationTokenSource.Token);

            Console.WriteLine($"Imported {rowsAffected} traffic accidents.");
        }

        #endregion Import Traffic Accidents Data

        #region Update Foreign Keys

        await using var sqlCommand = connection.CreateCommand();
     
        sqlCommand.CommandText = """
            UPDATE public.traffic_accidents
            SET city_id=subquery.city_id
            FROM (SELECT city_id, city_area
            	 FROM public.cities) AS subquery
            WHERE ST_Covers(subquery.city_area, accident_location)
            """;

        rowsAffected = (ulong)await sqlCommand.ExecuteNonQueryAsync();

        Console.WriteLine($"Updated {rowsAffected} traffic accidents with cities.");

        sqlCommand.CommandText = """
            UPDATE public.traffic_accidents
            SET settlement_id=subquery.settlement_id
            FROM (SELECT settlement_id, settlement_area
            	 FROM public.settlements) AS subquery
            WHERE ST_Covers(subquery.settlement_area, accident_location)
            """;

        rowsAffected = (ulong)await sqlCommand.ExecuteNonQueryAsync();

        Console.WriteLine($"Updated {rowsAffected} traffic accidents with settlements.");

        #endregion
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
finally
{
    await connection.CloseAsync();
}