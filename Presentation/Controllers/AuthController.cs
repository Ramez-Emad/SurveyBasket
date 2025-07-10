using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Authentication;



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

        return authResult.IsSuccess 
            ? Ok(authResult.Value) :
            Problem(
                statusCode: StatusCodes.Status400BadRequest ,
                title: authResult.Error.Code,
                detail: authResult.Error.Description
                  );
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var errorResult = await this.ValidateAsync(request, cancellationToken);
        if (errorResult is not null)
            return errorResult;

        var authResult = await _serviceManager.AuthService.GetRefreshTokenAsync(request.token , request.refreshToken , cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value) :
            Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: authResult.Error.Code,
                detail: authResult.Error.Description
                  );
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
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: result.Error.Code,
                detail: result.Error.Description
                );
    }
}
