using Domain.Entities;
using Domain.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;
public class QuestionRepository(ApplicationDbContext dbContext) : GenericRepository<Question>(dbContext), IQuestionRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<bool> ExistsAsync(string content, int pollId, int? questionId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Questions
            .AnyAsync(q => (q.Content == content) && (q.PollId == pollId) && (questionId == null || q.Id != questionId), cancellationToken);

    }

    public async Task<IEnumerable<Question>> GetQuestionsByPollIdAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Questions.Where(x => x.PollId == pollId).Include(q => q.Answers);

        return await query.ToListAsync(cancellationToken);

    }
}

