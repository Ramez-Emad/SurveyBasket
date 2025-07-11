using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Authentication;
using Shared.Abstractions;
using System;



namespace Presentation.Controllers;


[ApiController]
[Route("[controller]")]

public class AuthController(IServiceManager _serviceManager ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] AuthLoginRequest request, CancellationToken cancellationToken)
    {

        throw new Exception("Dd");
        var validationResult = await this.ValidateAsync(request, cancellationToken);

        if (validationResult is not null)
            return validationResult;

        var authResult = await _serviceManager.AuthService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem();

    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var authResult = await _serviceManager.AuthService.GetRefreshTokenAsync(request.token , request.refreshToken , cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem();
    }



    [HttpPut("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var result = await _serviceManager.AuthService.RevokeRefreshTokenAsync(request.token, request.refreshToken ,cancellationToken);


        return result.IsSuccess 
            ? Ok("Refresh token revoked successfully")
            : result.ToProblem();
    }
}
