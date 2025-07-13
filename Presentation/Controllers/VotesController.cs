using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Votes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers;

[ApiController]
[Route("api/polls/{pollId}/vote")]
[Authorize]
public class VotesController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> start([FromRoute] int pollId, CancellationToken cancellationToken)
    {

        var userId = User.GetUserId();

        var result = await _serviceManager.VoteService.GetQuestionsAsync(pollId, userId!, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var errorResult =  await this.ValidateAsync(request, cancellationToken);

        if (errorResult is not null)
            return errorResult;

        var result = await _serviceManager.VoteService.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
}
