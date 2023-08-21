using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Specification;

public class TrafficAccidentByIdSpecification : Specification<TrafficAccidentDataModel>
{
    public TrafficAccidentByIdSpecification(Guid trafficAccidentId)
        : base(x => x.TrafficAccidentId == trafficAccidentId && !x.IsDeleted)
    {
    }
}