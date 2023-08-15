using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Specification;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, Specification<T> specification)
    where T : class
    {
        var query = inputQuery;
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderBy is not null)
        {
            query = query.OrderBy(specification.OrderBy);
        } 
        else if (specification.OrderByDesc is not null)
        {
            query = query.OrderByDescending(specification.OrderByDesc);
        }

        if (specification.IsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        query = specification.Includes.Aggregate(query, (current, include)
            => current.Include(include!));

        return query;
    }
}