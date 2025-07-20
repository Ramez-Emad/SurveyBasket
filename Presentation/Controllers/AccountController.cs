using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using Shared.Contracts.Users;

namespace Presentation.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IUserService _userService) : ControllerBase
{

    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        var result = await _userService.GetProfileAsync(User.GetUserId()!);

        return Ok(result.Value);
    }

    [HttpPut("info")]
    public async Task<IActionResult> Info([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var errorsResult = await this.ValidateAsync(request, cancellationToken);

        if (errorsResult is not null)
            return errorsResult;

        await _userService.UpdateProfileAsync(User.GetUserId()!, request);

        return NoContent();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var errorsResult = await this.ValidateAsync(request, cancellationToken);

        if (errorsResult is not null)
            return errorsResult;

        var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }



}