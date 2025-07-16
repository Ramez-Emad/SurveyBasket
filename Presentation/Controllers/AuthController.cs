using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Authentication;
using Shared.Abstractions;
using System;



namespace Presentation.Controllers;


[ApiController]
[Route("[controller]")]

public class AuthController(IAuthService _authService ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] AuthLoginRequest request, CancellationToken cancellationToken)
    {

        var validationResult = await this.ValidateAsync(request, cancellationToken);

        if (validationResult is not null)
            return validationResult;

        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

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

        var authResult = await _authService.GetRefreshTokenAsync(request.token , request.refreshToken , cancellationToken);

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

        var result = await _authService.RevokeRefreshTokenAsync(request.token, request.refreshToken ,cancellationToken);


        return result.IsSuccess 
            ? Ok("Refresh token revoked successfully")
            : result.ToProblem();
    }


    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await this.ValidateAsync(request, cancellationToken);
        if (validationResult is not null)
            return validationResult;

        var result = await _authService.RegisterUserAsync(request, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await this.ValidateAsync(request, cancellationToken);
        if (validationResult is not null)
            return validationResult;

        var result = await _authService.ConfirmEmailAsync(request);

        return result.IsSuccess 
            ? Ok() 
            : result.ToProblem();
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await this.ValidateAsync(request, cancellationToken);
        if (validationResult is not null)
            return validationResult;


        var result = await _authService.ResendConfirmationEmailAsync(request);

        return result.IsSuccess 
            ? Ok() 
            : result.ToProblem();
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return result.IsSuccess 
            ? Ok() 
            : result.ToProblem();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        return result.IsSuccess 
            ? Ok() 
            : result.ToProblem();
    }


}
