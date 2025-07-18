using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using Shared.Contracts.Votes;
using Shared.Abstractions.Consts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/polls/{pollId}/vote")]
[Authorize(Roles = DefaultRoles.Member)]
public class VotesController(IVoteService _voteService , IQuestionService _questionService ) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
    {

        var userId = User.GetUserId();

        var result = await _questionService.GetAvailableQuestionsAsync(pollId, userId!, cancellationToken);

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

        var result = await _voteService.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
}
