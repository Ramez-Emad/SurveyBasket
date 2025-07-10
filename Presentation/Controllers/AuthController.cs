using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Authentication;
using ServiceAbstraction.Contracts.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers;


[ApiController]
[Route("[controller]")]

public class AuthController(IServiceManager _serviceManager ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] AuthLoginRequest request, CancellationToken cancellationToken)
    {

        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var authResult = await _serviceManager.AuthService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult is null ? BadRequest("Invalid email/password") : Ok(authResult);
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var authResult = await _serviceManager.AuthService.GetRefreshTokenAsync(request.token , request.refreshToken , cancellationToken);

        return authResult is null ? BadRequest("Invalid Token") : Ok(authResult);
    }



    [HttpPut("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _serviceManager.AuthService.RevokeRefreshTokenAsync(request.token, request.refreshToken ,cancellationToken);

        return result ? Ok("Refresh token revoked successfully") : BadRequest("Failed to revoke refresh token");
    }
}
