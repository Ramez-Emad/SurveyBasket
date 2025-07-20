using Domain.Contracts;
using System.Linq.Expressions;

namespace Service.Specifications;
public abstract class BaseSpecification<TEntity> : ISpecifications<TEntity> where TEntity : class
{

    public Expression<Func<TEntity, bool>>? Criteria { get; protected set; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }


    #region Sort    
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) => OrderBy = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression) => OrderByDescending = orderByDescExpression;

    #endregion


    #region Pagination


    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPaginated { get; set; }

    protected void ApplyPagination(int PageSize, int PageIndex)
    {
        IsPaginated = true;
        Take = PageSize;
        Skip = PageSize * (PageIndex - 1);
    }
    #endregion

}
