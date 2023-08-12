using NetTopologySuite.Geometries;

namespace Infrastructure.Database.Entities;

public class SettlementDataModel
{
    public Guid SettlementId { get; set; }

    public string SettlementName { get; set; } = null!;

    public Geometry SettlementArea { get; set; } = null!;
}