using Shared.Abstractions;
using Shared.Contracts.Results;

namespace ServiceAbstraction;
public interface IResultService
{
    Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken);

    Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken);

    Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default);
}
