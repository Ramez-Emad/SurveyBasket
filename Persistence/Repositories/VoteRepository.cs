using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServiceAbstraction.Contracts.Results;
using Shared.Abstractions;
using Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;
public class VoteRepository(ApplicationDbContext dbContext) : GenericRepository<Vote>(dbContext), IVoteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<TResult>> GetGroupedAsync<TKey, TResult>(
     ISpecifications<Vote> specification,
     Expression<Func<Vote, TKey>> groupBySelector,
     Expression<Func<IGrouping<TKey, Vote>, TResult>> resultSelector,
     CancellationToken cancellationToken = default)
    {

        var query = SpecificationEvaluator<Vote>.GetQuery(_dbContext.Votes.AsQueryable(), specification);

        return await query
            .GroupBy(groupBySelector)
            .Select(resultSelector)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UserHasVotedAsync(string userId, int pollId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Votes.AnyAsync(v => v.UserId == userId && v.PollId == pollId, cancellationToken);
    }
}
