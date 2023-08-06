using Infrastructure.Database.Entities.Enums;

using Point = NetTopologySuite.Geometries.Point;

namespace Infrastructure.Database.Entities;

public class TrafficAccidentDataModel
{
    public Guid TrafficAccidentId { get; set; }

    public string ExternalTrafficAccidentId { get; set; } = null!;

    public string PoliceDepartment { get; set; } = null!;

    public DateTime ReportedOn { get; set; }

    public Point AccidentLocation { get; set; } = null!;

    public DataParticipantsStatus ParticipantsStatus { get; set; }

    public DataParticipantsNominalCount ParticipantsNominalCount { get; set; }

    public DataAccidentType AccidentType { get; set; }

    public string Description { get; set; } = null!;
}