using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers;

[ApiController]
[Route("api/polls/{pollId}/[controller]")]
[Authorize]
public class QuestionsController(IServiceManager _serviceManager) : ControllerBase
{

    [HttpGet("")]
    public async Task<IActionResult> GetAll(int pollId, CancellationToken cancellationToken)
    {
        var result = await _serviceManager.QuestionService.GetQuestionsAsync(pollId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get( [FromRoute]int pollId, [FromRoute] int id , CancellationToken cancellationToken)
    {
         var result = await _serviceManager.QuestionService.GetQuestionByIdAsync(pollId , id, cancellationToken );

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }


    [HttpPost("")]
    public async Task<IActionResult> Create([FromRoute] int pollId, [FromBody] QuestionRequest questionRequest, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(questionRequest, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _serviceManager.QuestionService.CreateQuestionAsync(pollId, questionRequest, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { pollId, id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest questionRequest, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(questionRequest, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _serviceManager.QuestionService.UpdateQuestionAsync(pollId , id, questionRequest, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }


    [HttpPut("{id}/toggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _serviceManager.QuestionService.ToggleStatusAsync(pollId, id, cancellationToken);

        return result.IsSuccess 
                ? NoContent() 
                : result.ToProblem();
    }

}
