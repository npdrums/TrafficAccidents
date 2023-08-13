using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Specification;

public class TrafficAccidentByExternalIdSpecification : Specification<TrafficAccidentDataModel>
{
    public TrafficAccidentByExternalIdSpecification(string externalId) : base(x => x.ExternalTrafficAccidentId == externalId)
    {
        AddInclude(x => x.Municipality!);
        AddInclude(x => x.Settlement!);
        AddInclude(x => x.City!);

        IsSplitQuery = true;
    }
}