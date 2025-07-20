using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class SpecificationEvaluator<TEntity>
    where TEntity : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> specifications)
    {
        var query = inputQuery;

        if (specifications.Criteria is not null)
        {
            query = query.Where(specifications.Criteria);
        }

        if (specifications.OrderBy != null)
        {
            query = query.OrderBy(specifications.OrderBy);
        }

        else if (specifications.OrderByDescending != null)
        {
            query = query.OrderByDescending(specifications.OrderByDescending);
        }

        if (specifications.Includes != null && specifications.Includes.Count > 0)
        {
            foreach (var include in specifications.Includes)
            {
                query = query.Include(include);
            }
        }

        if (specifications.IsPaginated)
        {
            query = query.Skip(specifications.Skip).Take(specifications.Take);
        }

        return query;
    }
}
