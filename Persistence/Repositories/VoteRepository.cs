using Domain.Entities;
using Domain.Contracts;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;
public class VoteRepository(ApplicationDbContext dbContext) : GenericRepository<Vote>(dbContext), IVoteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<bool> UserHasVotedAsync(string userId, int pollId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Votes.AnyAsync(v => v.UserId == userId && v.PollId == pollId, cancellationToken);
    }
}
