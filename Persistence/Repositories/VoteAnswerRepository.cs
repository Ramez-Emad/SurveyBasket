using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;
public class VoteAnswerRepository(ApplicationDbContext dbContext) : GenericRepository<VoteAnswer>(dbContext) , IVoteAnswerRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<TResult>> GetGroupedAsync<TKey, TResult>(ISpecifications<VoteAnswer> specification, Expression<Func<VoteAnswer, TKey>> groupBySelector, Expression<Func<IGrouping<TKey, VoteAnswer>, TResult>> resultSelector, CancellationToken cancellationToken = default)
    {

        var query = SpecificationEvaluator<VoteAnswer>.GetQuery(_dbContext.VoteAnswers.AsQueryable(), specification);

        return await query
            .GroupBy(groupBySelector)
            .Select(resultSelector)
            .ToListAsync(cancellationToken);
    }
}
