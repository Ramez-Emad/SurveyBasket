using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Polls;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Presentation.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PollsController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var polls = await _serviceManager.PollService.GetAllPollsAsync();
        return Ok(polls);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var poll = await _serviceManager.PollService.GetPollByIdAsync(id);

        if (poll == null)
        {
            return NotFound();
        }
        return Ok(poll);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePoll([FromBody] PollRequest pollRequest , CancellationToken cancellationToken)
    {

        var errorResult = await this.ValidateAsync(pollRequest, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var newPoll = await  _serviceManager.PollService.CreatePollAsync(pollRequest, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var isUpdated = await _serviceManager.PollService.UpdatePollAsync(id, request, cancellationToken);

        if (isUpdated is null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _serviceManager.PollService.DeletePollAsync(id, cancellationToken);

        if (!isDeleted)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isUpdated = await _serviceManager.PollService.TogglePublishStatusAsync(id, cancellationToken);

        if (isUpdated is null)
            return NotFound();

        return NoContent();
    }

}
