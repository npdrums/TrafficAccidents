using Infrastructure.Database.Entities;

using NetTopologySuite.Geometries;

namespace Infrastructure.Database.Specification;

public class MunicipalityAreaCoversSpecification : Specification<MunicipalityDataModel>
{
    public MunicipalityAreaCoversSpecification(Geometry geometry) 
        : base(x => x.MunicipalityArea.Covers(geometry))
    {
    }
}

public class SettlementAreaCoversSpecification : Specification<SettlementDataModel>
{
    public SettlementAreaCoversSpecification(Geometry geometry) 
        : base(x => x.SettlementArea.Covers(geometry))
    {
    }
}

public class CityAreaCoversSpecification : Specification<CityDataModel>
{
    public CityAreaCoversSpecification(Geometry geometry) 
        : base(x => x.CityArea.Covers(geometry))
    {
    }
}