using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using Presentation.Filters.Authentication;
using ServiceAbstraction;
using Shared.Contracts.Polls;
using Shared.Abstractions.Consts;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers;


[ApiController]
[Route("api/[controller]")]

public class PollsController(IPollService _pollService) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _pollService.GetAllPollsAsync();
        return Ok(polls);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _pollService.GetPollByIdAsync(id);

        return result.IsSuccess 
            ? Ok(result.Value)
             : result.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.AddPolls)]
    public async Task<IActionResult> CreatePoll([FromBody] PollRequest pollRequest , CancellationToken cancellationToken)
    {

        var errorResult = await this.ValidateAsync(pollRequest, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _pollService.CreatePollAsync(pollRequest, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _pollService.UpdatePollAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
             : result.ToProblem();
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeletePolls)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.DeletePollAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
             : result.ToProblem();
    }

    [HttpPut("{id}/togglePublish")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess
           ? NoContent()
           : result.ToProblem();
    }


    [HttpGet("current")]
    [Authorize(Roles = DefaultRoles.Member)]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var result = await _pollService.GetCurrentAsync(cancellationToken);

        return Ok(result.Value);
    }
}
