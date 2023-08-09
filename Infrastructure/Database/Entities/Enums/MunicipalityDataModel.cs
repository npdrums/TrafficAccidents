using NetTopologySuite.Geometries;

namespace Infrastructure.Database.Entities.Enums;

public class MunicipalityDataModel
{
    public Guid MunicipalityId { get; set; }

    public string MunicipalityName { get; set; } = null!;

    public Geometry MunicipalityBorder { get; set; } = null!;
}