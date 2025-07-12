using Domain.Entities;
using Domain.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;
public class PollRepository(ApplicationDbContext dbContext) : GenericRepository<Poll>(dbContext), IPollRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<bool> TitleExistsAsync(string title, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Polls
            .AnyAsync(p => p.Title == title && (excludeId == null || p.Id != excludeId), cancellationToken);
    }
}
