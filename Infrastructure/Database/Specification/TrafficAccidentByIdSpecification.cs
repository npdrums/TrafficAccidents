using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Specification;

public class TrafficAccidentByExternalIdSpecification : Specification<TrafficAccidentDataModel>
{
    public TrafficAccidentByExternalIdSpecification(Guid externalId)
        : base(x => x.ExternalTrafficAccidentId == externalId && !x.IsDeleted)
    {
    }
}