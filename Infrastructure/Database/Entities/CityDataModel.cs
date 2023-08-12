using NetTopologySuite.Geometries;

namespace Infrastructure.Database.Entities;

public class CityDataModel
{
    public Guid CityId { get; set; }

    public string CityName { get; set; } = null!;

    public Geometry CityArea { get; set; } = null!;
}