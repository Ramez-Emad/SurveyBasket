using Shared.Contracts.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Results;
public record PollVotesResponse(
    string Title,
    IEnumerable<VoteResponse> Votes
    );
