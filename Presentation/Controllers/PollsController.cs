using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Polls;

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
        var result = await _serviceManager.PollService.GetPollByIdAsync(id);

        return result.IsSuccess 
            ? Ok(result.Value)
             : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePoll([FromBody] PollRequest pollRequest , CancellationToken cancellationToken)
    {

        var errorResult = await this.ValidateAsync(pollRequest, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await  _serviceManager.PollService.CreatePollAsync(pollRequest, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _serviceManager.PollService.UpdatePollAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
             : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _serviceManager.PollService.DeletePollAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
             : result.ToProblem();
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _serviceManager.PollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess
           ? NoContent()
           : result.ToProblem();
    }

}
