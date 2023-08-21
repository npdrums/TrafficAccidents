using Infrastructure.Database.Entities.Enums;

using Point = NetTopologySuite.Geometries.Point;

namespace Infrastructure.Database.Entities;

public class TrafficAccidentDataModel
{
    public Guid TrafficAccidentId { get; set; }

    public string CaseNumber { get; set; } = null!;

    public string PoliceDepartment { get; set; } = null!;

    public DateTime ReportedOn { get; set; }

    public Point AccidentLocation { get; set; } = null!;

    public DataParticipantsStatus ParticipantsStatus { get; set; }

    public DataParticipantsNominalCount ParticipantsNominalCount { get; set; }

    public DataAccidentType AccidentType { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public Guid? MunicipalityId { get; set; }

    public virtual MunicipalityDataModel? Municipality { get; set; } = null!;

    public Guid? SettlementId { get; set; }

    public virtual SettlementDataModel? Settlement { get; set; } = null!; 
    
    public Guid? CityId { get; set; }

    public virtual CityDataModel? City { get; set; } = null!;
}