using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using Infrastructure.Database.Entities;
using Infrastructure.Database.Entities.Enums;

using System.Globalization;

using Point = NetTopologySuite.Geometries.Point;

namespace DataSeedingTool.Mappers;

public sealed class CsvRecordToTrafficAccidentDataModelMapper : ClassMap<TrafficAccidentDataModel>
{
    public CsvRecordToTrafficAccidentDataModelMapper()
    {
        Map(x => x.TrafficAccidentId).Ignore();
        Map(x => x.ExternalTrafficAccidentId).Ignore();
        Map(x => x.CaseNumber).Index(0);
        Map(x => x.PoliceDepartment).Index(1);
        Map(x => x.AccidentLocation).Convert(x =>
        {
            var latitudeRowValue = x.Row.GetField(5);
            var longitudeRowValue = x.Row.GetField(4);
            var isValidLatitude = double.TryParse(latitudeRowValue, out var latitude);
            var isValidLongitude = double.TryParse(longitudeRowValue, out var longitude);

            return isValidLongitude && isValidLatitude
                ? new Point(longitude, latitude) { SRID = 4326 }
                : throw new InvalidOperationException($"Given Coordinates are not correct (longitude: {longitudeRowValue}, latitude {latitudeRowValue} is not in valid format.");

        });
        Map(x => x.ReportedOn).Index(3).TypeConverter<ReportedOnDateTimeConverter>();
        Map(x => x.ParticipantsStatus).Index(6).TypeConverter<ParticipantStatusEnumConverter>();
        Map(x => x.ParticipantsNominalCount).Index(7).TypeConverter<ParticipantsNominalCountEnumConverter>();
        Map(x => x.AccidentType).Index(8).TypeConverter<AccidentTypeEnumConverter>();
        Map(x => x.Description).Index(8);
        Map(x => x.MunicipalityId).Ignore();
        Map(x => x.Municipality!.MunicipalityId).Ignore();
        Map(x => x.Municipality!.MunicipalityName).Index(2); // We temporarily map Municipality Name in case we are not able to determine it by later by geographical data.
        Map(x => x.Municipality!.MunicipalityArea).Ignore();
    }
}

internal class ReportedOnDateTimeConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        var isValidDateTime = DateTime.TryParseExact(text?.Trim(), "dd.MM.yyyy,HH:mm", CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal, out var result);

        return isValidDateTime ? result : throw new InvalidOperationException($"Given DateTime ({text}) is not in valid format.");
    }
}

internal class ParticipantStatusEnumConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return text switch
        {
            "Sa mat.stetom" => DataParticipantsStatus.MaterialDamage,
            "Sa povredjenim" => DataParticipantsStatus.Injured,
            "Sa poginulim" => DataParticipantsStatus.Killed,
            _ => DataParticipantsStatus.Unknown
        };
    }
}

internal class ParticipantsNominalCountEnumConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return text switch
        {
            "SN SA NAJMANjE DVA VOZILA – BEZ SKRETANjA" or "SN SA NAJMANjE DVA VOZILA – SKRETANjE ILI PRELAZAK" =>
                DataParticipantsNominalCount.AtLeastTwoVehicles,
            "SN SA PEŠACIMA" => DataParticipantsNominalCount.WithPedestrians,
            "SN SA JEDNIM VOZILOM" => DataParticipantsNominalCount.SingleVehicle,
            "SN SA PARKIRANIM VOZILIMA" => DataParticipantsNominalCount.ParkedVehicle,
            _ => DataParticipantsNominalCount.Unknown
        };
    }
}

internal class AccidentTypeEnumConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return text switch
        {
            // Could refactor this to load it from configuration? Potential string comparision and collections overhead?
            "Najmanje dva vozila koja se kreću istim putem u istom smeru uz skretanje, skretanje ulevo ispred drugog vozila"
                or "Najmanje dva vozila koja se kreću u istom smeru – uključivanje u saobraćaj"
                or "Najmanje dva vozila koja se kreću različitim putevima uz skretanje ulevo i uz nailazak vozila sleva"
                or "Najmanje dva vozila koja se kreću istim putem u istom smeru uz skretanje, skretanje udesno ispred drugog vozila"
                or "Najmanje dva vozila koja se kreću istim putem u istom smeru uz skretanje, polukružno okretanje ispred drugog vozila"
                or "Najmanje dva vozila koja se kreću različitim putevima uz prolazak kroz raskrsnicu, ili od kojih jedno prelazi preko kolovoza, bez skretanja"
                or "Najmanje dva vozila koja se kreću različitim putevima uz skretanje ulevo i uz nailazak vozila zdesna"
                or "Najmanje dva vozila koja se kreću različitim putevima uz skretanje oba vozila"
                or "Najmanje dva vozila koja se kreću istim putem u suprotnim smerovima uz polukružno okretanje ispred drugog vozila"
                => DataAccidentType.Angle,

            "Nezgoda sa jednim vozilom – silazak sa kolovoza u krivini"
                or "Nezgoda sa jednim vozilom – silazak udesno sa kolovoza na pravcu"
                or "Nezgoda sa jednim vozilom – silazak ulevo sa kolovoza na pravcu"
                => DataAccidentType.FallOfRoad,

            "Najmanje dva vozila koja se kreću različitim putevima uz skretanje udesno – čeoni sudar sa vozilom koje nailazi zdesna"
                or "Najmanje dva vozila – čeoni sudar"
                or "Najmanje dva vozila koja se kreću istim putem u suprotnim smerovima i vrše skretanje na isti put"
                or "Najmanje dva vozila koja se kreću istim putem u suprotnim smerovima uz skretanje ulevo ispred drugog vozila"
                or "Najmanje dva vozila koja se kreću različitim putevima uz skretanje udesno ispred vozila koje nailazi sleva"
                or "Najmanje dva vozila koja se kreću istim putem u suprotnim smerovima i vrše skretanje na naspramne puteve"
                => DataAccidentType.HeadOnCollision,

            "Najmanje dva vozila koja se kreću u istom smeru – sustizanje"
                or "Najmanje dva vozila koja se kreću istim putem u istom smeru uz skretanje, sudar u sustizanju"
                => DataAccidentType.RearEndCollision,

            "Najmanje dva vozila – suprotni smerovi bez skretanja – kretanje unazad"
                => DataAccidentType.RearToRearCollision,

            "Najmanje dva vozila koja se kreću u istom smeru – preticanje"
                or "Najmanje dva vozila koja se kreću u istom smeru – sudar pri uporednoj vožnji"
                => DataAccidentType.SideSwipe,

            "Nezgoda sa jednim vozilom i prevrtanjem"
                => DataAccidentType.TurnOver,

            "Nezgode sa učešćem jednog vozila i životinja"
                => DataAccidentType.WithAnimals,

            "Nezgode sa učešćem jednog vozila i preprekama na ili iznad kolovoza"
                or "Nezgode sa učešćem jednog vozila na mestu na kome se izvode radovi na putu"
                => DataAccidentType.WithObstacles,

            "Sudar sa parkiranim vozilom sa desne strane kolovoza"
                or "Ostali sudari sa parkiranim vozilom"
                or "Sudar sa parkiranim vozilom sa leve strane kolovoza"
                or "Sudar sa parkiranim vozilom pri otvaranju vrata"
                or "Sudar sa parkiranim vozilom – bilo sa leve ili sa desne strane kolovoza"
                => DataAccidentType.WithParkedVehicle,

            "Prelazak pešaka sleva, van raskrsnice , bez skretanja vozila"
                or "Prelazak pešaka sleva, sa skretanjem vozila ulevo, u raskrsnici"
                or "Prelazak pešaka zdesna, sa skretanjem vozila ulevo, u raskrsnici"
                or "Pešak – ostale situacije"
                or "Prelazak pešaka zdesna, van raskrsnice, bez skretanja vozila"
                or "Prelazak pešaka sleva, u raskrsnici, bez skretanja vozila"
                or "Pešak se kreće duž kolovoza"
                or "Prelazak pešaka preko kolovoza, van raskrsnice, bez skretanja vozila"
                or "Prelazak pešaka preko kolovoza sa skretanjem vozila ulevo, u raskrsnici"
                or "Pešak se kreće duž kolovoza u smeru kretanja vozila"
                or "Prelazak pešaka zdesna , u raskrsnici, bez skretanja vozila"
                or "Prelazak pešaka preko kolovoza, u raskrsnici, bez skretanja vozila"
                or "Prelazak pešaka preko kolovoza, u ili van raskrsnice, bez skretanja vozila"
                or "Pešak stoji na kolovozu"
                or "Pešak se kreće na trotoaru ili biciklističkoj stazi"
                or "Prelazak pešaka sleva, sa skretanjem vozila udesno, u raskrsnici"
                or "Pešak se kreće duž kolovoza suprotno od smera kretanja vozila"
                or "Pešak zaustavljen na trotoaru ili biciklističkoj stazi"
                or "Prelazak pešaka zdesna, sa skretanjem vozila udesno, u raskrsnici"
                or "Pešak se na trotoaru ili biciklističkoj stazi kreće u smeru kretanja vozila"
                or "Prelazak pešaka preko kolovoza sa skretanjem vozila udesno, u raskrsnici"
                or "Prelazak pešaka preko kolovoza (sleva ili zdesna), sa skretanjem vozila (levo ili desno), u raskrsnici"
                or "Pešak se kreće duž kolovoza ili stoji na kolovozu"
                or "Pešak se na trotoaru ili biciklističkoj stazi kreće suprotno od smera kretanja vozila"
                or "Pešak stoji ili se kreće, sa kretanjem vozila unazad"
                => DataAccidentType.WithPedestrians,

            "Nezgode sa učešćem šinskog i drumskog vozila"
                => DataAccidentType.WithRailVehicle,

            _ => DataAccidentType.Unknown
        };
    }
}