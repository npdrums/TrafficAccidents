using API.Contracts.Enums;

using System.Text.Json.Serialization;

namespace API.Contracts;

public class TrafficAccidentUpdateRequest
{
    public required Guid TrafficAccidentId { get; set; }

    public string CaseNumber { get; set; } = null!;

    public string PoliceDepartment { get; set; } = null!;

    public DateTime ReportedOn { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public ParticipantsStatus ParticipantsStatus { get; set; }

    public ParticipantsNominalCount ParticipantsNominalCount { get; set; }

    public AccidentType AccidentType { get; set; }

    public string? Description { get; set; }
}