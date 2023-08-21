using System.Linq.Expressions;
using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Specification;

public class TrafficAccidentByIdWithIncludesSpecification : Specification<TrafficAccidentDataModel>
{
    public TrafficAccidentByIdWithIncludesSpecification(Guid trafficAccidentId) 
        : base(x => x.TrafficAccidentId == trafficAccidentId && !x.IsDeleted)
    {
        AddInclude(x => x.Municipality!);
        AddInclude(x => x.Settlement!);
        AddInclude(x => x.City!);

        IsSplitQuery = true;
    }
}