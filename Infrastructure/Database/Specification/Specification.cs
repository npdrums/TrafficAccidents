using System.Linq.Expressions;

namespace Infrastructure.Database.Specification;

public abstract class Specification<T> where T : class
{
    public bool IsSplitQuery { get; protected set; }

    protected Specification(Expression<Func<T, bool>>? criteria) => Criteria = criteria;

    public Expression<Func<T, bool>>? Criteria { get; }

    public List<Expression<Func<T, object>>?> Includes { get; } = new();

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDesc { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;

    protected void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression) => OrderByDesc = orderByDescExpression;
}