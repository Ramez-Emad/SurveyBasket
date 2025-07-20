using System.Linq.Expressions;

namespace Domain.Contracts;
public interface ISpecifications<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>>? Criteria { get; }

    List<Expression<Func<TEntity, object>>> Includes { get; }

    Expression<Func<TEntity, object>>? OrderBy { get; }

    Expression<Func<TEntity, object>>? OrderByDescending { get; }

    int Take { get; }
    int Skip { get; }
    bool IsPaginated { get; }
}
