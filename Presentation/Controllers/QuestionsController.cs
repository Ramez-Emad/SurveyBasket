using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using Presentation.Filters.Authentication;
using ServiceAbstraction;
using Shared;
using Shared.Abstractions.Consts;
using Shared.Contracts.Questions;
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
public class QuestionsController(IQuestionService _questionService ) : ControllerBase
{

    [HttpGet("")]
    [HasPermission(Permissions.GetQuestions)]
    public async Task<IActionResult> GetAll(int pollId, [FromQuery] QuestionQueryParams queryParams , CancellationToken cancellationToken)
    {

        var result = await _questionService.GetQuestionsAsync(pollId, queryParams, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetQuestions)]
    public async Task<IActionResult> Get( [FromRoute]int pollId, [FromRoute] int id , CancellationToken cancellationToken)
    {
         var result = await _questionService.GetQuestionByIdAsync(pollId , id, cancellationToken );

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }


    [HttpPost("")]
    [HasPermission(Permissions.AddQuestions)]
    public async Task<IActionResult> Create([FromRoute] int pollId, [FromBody] QuestionRequest questionRequest, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(questionRequest, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _questionService.CreateQuestionAsync(pollId, questionRequest, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { pollId, id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }


    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateQuestions)]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest questionRequest, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(questionRequest, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _questionService.UpdateQuestionAsync(pollId , id, questionRequest, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }


    [HttpPut("{id}/toggleStatus")]
    [HasPermission(Permissions.UpdateQuestions)]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);

        return result.IsSuccess 
                ? NoContent() 
                : result.ToProblem();
    }

}
