using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;
public class PollRepository(ApplicationDbContext dbContext) : GenericRepository<Poll>(dbContext), IPollRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<bool> IsPollAvailable(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _dbContext.Polls.AnyAsync(
                                                            x => x.Id == pollId
                                                            && x.IsPublished
                                                            && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                                                            && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

        return pollIsExists;
    }

    public async Task<bool> TitleExistsAsync(string title, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Polls
            .AnyAsync(p => p.Title == title && (excludeId == null || p.Id != excludeId), cancellationToken);
    }
}
