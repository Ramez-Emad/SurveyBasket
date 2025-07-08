using ServiceAbstraction.Contracts.Requests;
using ServiceAbstraction.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction;
public interface IPollService
{
    Task<IEnumerable<PollResponse>> GetAllPollsAsync(CancellationToken cancellationToken = default);

    Task<PollResponse?> GetPollByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PollResponse> CreatePollAsync(PollRequest request, CancellationToken cancellationToken = default);

    Task<PollResponse> UpdatePollAsync(int id, PollRequest request, CancellationToken cancellationToken = default);

    Task<PollResponse> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> DeletePollAsync(int id, CancellationToken cancellationToken = default);
}
