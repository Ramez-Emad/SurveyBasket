using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers;


[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class ResultsController(IResultService _resultService) : ControllerBase
{

    [HttpGet("row-data")]
    public async Task<IActionResult> PollVotes([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _resultService.GetPollVotesAsync(pollId, cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }


    [HttpGet("votes-per-day")]
    public async Task<IActionResult> VotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _resultService.GetVotesPerDayAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("votes-per-question")]
    public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _resultService.GetVotesPerQuestionAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


}
