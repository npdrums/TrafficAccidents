using Domain.Enums;

using NetTopologySuite.Geometries;

namespace Domain.Models;

public class TrafficAccident
{
    public Guid TrafficAccidentId { get; set; }

    public required string CaseNumber { get; init; }

    public required string PoliceDepartment { get; init; }

    public required DateTime ReportedOn { get; init; }


    private readonly Point _accidentLocation = null!;

    public required Point AccidentLocation
    {
        get => _accidentLocation;
        init
        {
            _accidentLocation = value;
            _accidentLocation.SRID = Srid.Wgs84;
        }
    }

    public required ParticipantsStatus ParticipantsStatus { get; init; }

    public required ParticipantsNominalCount ParticipantsNominalCount { get; init; }

    public required AccidentType AccidentType { get; init; }

    public string? Description { get; set; }

    public string? MunicipalityName { get; set; }

    public string? SettlementName { get; set; }

    public string? CityName { get; set; }
}