using NetTopologySuite.Geometries;

namespace Infrastructure.Database.Entities;

public class MunicipalityDataModel
{
    public Guid MunicipalityId { get; set; }

    public string MunicipalityName { get; set; } = null!;

    public Geometry MunicipalityArea { get; set; } = null!;
}