using Domain.Entities;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;
public class QuestionRepository(ApplicationDbContext dbContext) : GenericRepository<Question>(dbContext), IQuestionRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;


    public Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Questions
            .AnyAsync(q => (q.Content == content) && (q.PollId == pollId) , cancellationToken);
    }

    public Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, int questionId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Questions
             .AnyAsync(q => (q.Content == content) && (q.PollId == pollId) && (q.Id != questionId), cancellationToken);
    }
}

