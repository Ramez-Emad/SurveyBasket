using Domain.Entities;

namespace Domain.Contracts;
public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, CancellationToken cancellationToken = default);

    Task<bool> IsQuestionContentDuplicateAsync(string content, int pollId, int questionId, CancellationToken cancellationToken = default);
}
