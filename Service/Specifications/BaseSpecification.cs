using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications;
public abstract class BaseSpecification<TEntity> : ISpecifications<TEntity> where TEntity : class
{

    public Expression<Func<TEntity, bool>>? Criteria { get; protected set; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}
