using Shared.Contracts.Questions;
using Shared.Abstractions;
using Shared.Contracts.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction;
public interface IQuestionService
{
    Task<Result<IEnumerable<QuestionResponse>>> GetQuestionsAsync(int pollId, CancellationToken cancellationToken = default);

    Task<Result<QuestionResponse>> CreateQuestionAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);

    Task<Result<QuestionResponse>> GetQuestionByIdAsync(int pollId , int id, CancellationToken cancellationToken = default);

    Task<Result> UpdateQuestionAsync(int pollId ,int id ,  QuestionRequest questionRequest , CancellationToken cancellationToken = default);

    Task<Result> ToggleStatusAsync (int pollId, int id, CancellationToken cancellationToken = default);
}
