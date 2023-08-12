using Domain.Models;

using Infrastructure.Database.Entities;

using NetTopologySuite.Features;

namespace DataSeedingTool.Extensions;

public static class FeatureCollectionExtensions
{
    public static IEnumerable<MunicipalityDataModel> GetMunicipalities(this FeatureCollection featureCollection)
    {
        return featureCollection.Select(feature =>
        {
            feature.Geometry.SRID = Srid.Wgs84Utm34N; // NTS ignores SRID when reading GeoJSON
            return new MunicipalityDataModel
            {
                MunicipalityId = Guid.NewGuid(), // Explicitly set Id here, because it will be used in memory later.
                MunicipalityName = feature.Attributes["opstina_imel"].ToString()!,
                MunicipalityArea = feature.Geometry.ProjectTo(Srid.Wgs84)
            };
        }).ToList();
    }

    public static IEnumerable<SettlementDataModel> GetSettlements(this FeatureCollection featureCollection)
    {
        return featureCollection.Select(feature =>
        {
            feature.Geometry.SRID = Srid.Wgs84Utm34N; // NTS ignores SRID when reading GeoJSON
            return new SettlementDataModel
            {
                SettlementId = Guid.NewGuid(), // Explicitly set Id here, because it will be used in memory later.
                SettlementName = feature.Attributes["mz_imel"].ToString()!,
                SettlementArea = feature.Geometry.ProjectTo(Srid.Wgs84)
            };
        });
    }

    public static IEnumerable<CityDataModel> GetCities(this FeatureCollection featureCollection)
    {
        return featureCollection.Select(feature =>
        {
            feature.Geometry.SRID = Srid.Wgs84Utm34N; // NTS ignores SRID when reading GeoJSON
            return new CityDataModel
            {
                CityId = Guid.NewGuid(), // Explicitly set Id here, because it will be used in memory later.
                CityName = feature.Attributes["grad_imel"].ToString()!,
                CityArea = feature.Geometry.ProjectTo(Srid.Wgs84)
            };
        });
    }
}