using Domain.Enums;

using NetTopologySuite.Geometries;

namespace Domain.Models;

public class TrafficAccident
{
    private TrafficAccident() { }

    public TrafficAccident(string externalTrafficAccidentId, string policeDepartment, string description,
        AccidentType accidentType, ParticipantsNominalCount participantsNominalCount,
        ParticipantsStatus participantsStatus,
        Point accidentLocation, DateTime reportedOn,
        string? municipalityName = null,
        string? settlementName = null,
        string? cityName = null) : this()
    {
        ExternalTrafficAccidentId = externalTrafficAccidentId;
        PoliceDepartment = policeDepartment;
        Description = description;
        AccidentType = accidentType;
        ParticipantsNominalCount = participantsNominalCount;
        ParticipantsStatus = participantsStatus;
        AccidentLocation = accidentLocation;
        ReportedOn = reportedOn;
        MunicipalityName ??= municipalityName;
        SettlementName ??= settlementName;
        CityName ??= cityName;
    }

    public TrafficAccident(string externalTrafficAccidentId, string policeDepartment, string description,
        AccidentType accidentType, ParticipantsNominalCount participantsNominalCount, ParticipantsStatus participantsStatus,
        double longitude, double latitude, DateTime reportedOn,
        string? municipalityName = null,
        string? settlementName = null,
        string? cityName = null)
        : this(
            externalTrafficAccidentId, policeDepartment, description, accidentType, participantsNominalCount, participantsStatus, 
            new Point(longitude, latitude) { SRID = Srid.Wgs84 }, reportedOn, municipalityName, settlementName, cityName)
    {
    }

    public string ExternalTrafficAccidentId { get; } = null!;

    public string PoliceDepartment { get; } = null!;

    public DateTime ReportedOn { get; }

    public Point AccidentLocation { get; } = null!;

    public ParticipantsStatus ParticipantsStatus { get; }

    public ParticipantsNominalCount ParticipantsNominalCount { get; }

    public AccidentType AccidentType { get; }

    public string? Description { get; }

    public string? MunicipalityName { get; }

    public string? SettlementName { get; }

    public string? CityName { get; }
}