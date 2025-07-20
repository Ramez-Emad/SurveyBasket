using Shared.Abstractions;
using Shared.Contracts.Votes;

namespace ServiceAbstraction;
public interface IVoteService
{

    Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken);
}
