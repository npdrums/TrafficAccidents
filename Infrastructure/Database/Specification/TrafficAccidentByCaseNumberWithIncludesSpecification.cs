using Infrastructure.Database.Entities;

namespace Infrastructure.Database.Specification;

public class TrafficAccidentByCaseNumberWithIncludesSpecification : Specification<TrafficAccidentDataModel>
{
    public TrafficAccidentByCaseNumberWithIncludesSpecification(string caseNumber)
        : base(x => x.CaseNumber == caseNumber && !x.IsDeleted)
    {
        AddInclude(x => x.Municipality!);
        AddInclude(x => x.Settlement!);
        AddInclude(x => x.City!);

        IsSplitQuery = true;
    }
}