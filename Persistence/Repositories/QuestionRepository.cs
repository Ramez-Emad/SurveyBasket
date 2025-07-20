using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;
public class QuestionRepository(ApplicationDbContext dbContext) : GenericRepository<Question>(dbContext), IQuestionRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;


    public Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Questions
            .AnyAsync(q => (q.Content == content) && (q.PollId == pollId), cancellationToken);
    }

    public Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, int questionId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Questions
             .AnyAsync(q => (q.Content == content) && (q.PollId == pollId) && (q.Id != questionId), cancellationToken);
    }
}

