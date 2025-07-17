using Shared.Contracts.Votes;

namespace Shared.Contracts.Votes;


public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
);