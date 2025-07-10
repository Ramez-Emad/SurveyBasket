using ServiceAbstraction.Contracts.Polls;
using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction;
public interface IPollService
{
    Task<IEnumerable<PollResponse>> GetAllPollsAsync(CancellationToken cancellationToken = default);

    Task<Result<PollResponse>> GetPollByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PollResponse> CreatePollAsync(PollRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdatePollAsync(int id, PollRequest request, CancellationToken cancellationToken = default);

    Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> DeletePollAsync(int id, CancellationToken cancellationToken = default);
}
