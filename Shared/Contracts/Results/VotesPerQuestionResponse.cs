namespace Shared.Contracts.Results;
public record VotesPerQuestionResponse(
    string Question,
    IEnumerable<VotesPerAnswerResponse> SelectedAnswers
);