using Domain.Enums;
using AccidentType = API.Contracts.Enums.AccidentType;
using ParticipantsNominalCount = API.Contracts.Enums.ParticipantsNominalCount;
using ParticipantsStatus = API.Contracts.Enums.ParticipantsStatus;

namespace API.Contracts;

public class TrafficAccidentRequest
{
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