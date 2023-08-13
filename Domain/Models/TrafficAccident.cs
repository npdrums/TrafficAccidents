using Domain.Enums;

using NetTopologySuite.Geometries;

namespace Domain.Models;

public class TrafficAccident
{
    public TrafficAccident(
        string externalTrafficAccidentId, string policeDepartment, string description,
        string? municipalityName, string? settlementName, string? cityName, AccidentType accidentType,
        ParticipantsNominalCount participantsNominalCount, ParticipantsStatus participantsStatus,
        Point accidentLocation, DateTime reportedOn)
    {
        ExternalTrafficAccidentId = externalTrafficAccidentId;
        PoliceDepartment = policeDepartment;
        Description = description;
        MunicipalityName = municipalityName;
        SettlementName = settlementName;
        CityName = cityName;
        AccidentType = accidentType;
        ParticipantsNominalCount = participantsNominalCount;
        ParticipantsStatus = participantsStatus;
        AccidentLocation = accidentLocation;
        ReportedOn = reportedOn;
    }

    public TrafficAccident(
        string externalTrafficAccidentId, string policeDepartment, string description,
        string? municipalityName, string? settlementName, string? cityName, AccidentType accidentType,
        ParticipantsNominalCount participantsNominalCount, ParticipantsStatus participantsStatus,
        double latitude, double longitude, DateTime reportedOn)
        : this(
            externalTrafficAccidentId, policeDepartment, description, municipalityName, settlementName, cityName,
            accidentType, participantsNominalCount, participantsStatus, new Point(latitude, longitude), reportedOn)
    {
    }

    public string ExternalTrafficAccidentId { get; }

    public string PoliceDepartment { get; }

    public DateTime ReportedOn { get; }

    public Point AccidentLocation { get; }

    public ParticipantsStatus ParticipantsStatus { get; }

    public ParticipantsNominalCount ParticipantsNominalCount { get; }

    public AccidentType AccidentType { get; }

    public string? Description { get; }

    public string? MunicipalityName { get; }

    public string? SettlementName { get; }

    public string? CityName { get; }
}