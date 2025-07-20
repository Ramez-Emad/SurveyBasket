using Shared;
using Shared.Abstractions;
using Shared.Contracts.Questions;
using Shared.Shared;

namespace ServiceAbstraction;
public interface IQuestionService
{
    Task<Result<PaginatedResult<QuestionResponse>>> GetQuestionsAsync(int pollId, QuestionQueryParams queryParams, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<QuestionResponse>>> GetAvailableQuestionsAsync(int pollId, string userId, CancellationToken cancellationToken);


    Task<Result<QuestionResponse>> CreateQuestionAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);

    Task<Result<QuestionResponse>> GetQuestionByIdAsync(int pollId, int id, CancellationToken cancellationToken = default);

    Task<Result> UpdateQuestionAsync(int pollId, int id, QuestionRequest questionRequest, CancellationToken cancellationToken = default);

    Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default);
}
