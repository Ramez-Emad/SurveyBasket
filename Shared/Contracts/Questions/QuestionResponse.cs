using Shared.Contracts.Answers;

namespace Shared.Contracts.Questions;

public record QuestionResponse(
    int Id,
    string Content,
    IEnumerable<AnswerResponse> Answers
);