using Domain.Entities;
using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Service.Email;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Authentication;
using Shared.Abstractions;
using Shared.Errors;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Service.Authentication;
public class AuthService(
        UserManager<ApplicationUser> _userManager,
        IJwtProvider _jwtProvider,
        ILogger<AuthService> logger,
        IHttpContextAccessor _httpContextAccessor,
        IEmailSender emailSender) : IAuthService
{
    private readonly int _refreshTokenExpiryDays = 14;

    // Login
    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (!await _userManager.CheckPasswordAsync(user, password))
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(
            Id: user.Id,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Token: token,
            ExpiresIn: expiresIn * 60,
            RefreshToken: refreshToken,
            RefreshTokenExpiration: refreshTokenExpiration
        );

        return Result.Success(response);
    }

    
    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {

        if (_jwtProvider.ValidateToken(token) is not { } userId)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);


        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);


        if (user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive) is not { } userRefreshToken)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);


        userRefreshToken.RevokedOn = DateTime.UtcNow;


        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);


        var response = new AuthResponse(
            Id: user.Id,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Token: newToken,
            ExpiresIn: expiresIn * 60,
            RefreshToken: newRefreshToken,
            RefreshTokenExpiration: refreshTokenExpiration
        );

        return Result.Success(response);


    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();

    }

    public async Task<Result> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
       
        // Check if user already exists

        if (await _userManager.FindByEmailAsync(request.Email) is { })
            return Result.Failure(UserErrors.DuplicatedEmail);

        // create new user
    
        var applicationUser = request.Adapt<ApplicationUser>();
   

        var result = await _userManager.CreateAsync(applicationUser, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(
                new Error(error.Code,
                           error.Description,
                           StatusCodes.Status400BadRequest
                   )
            );
        }

        // Generate confirmation token

        var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

        // Encode confirmation code to Base64Url because it may contain special characters
        confirmationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationCode));

        await SendConfirmationEmail(applicationUser, confirmationCode);

        logger.LogInformation("User {Email} registered successfully.UserId {userId} Confirmation code: {code}", request.Email, applicationUser.Id , confirmationCode);

        return Result.Success();
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        confirmationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationCode));

        logger.LogInformation("User {email} resend Confirmation code: {code}", request.Email, confirmationCode);

        await SendConfirmationEmail(user, confirmationCode);

        return Result.Success();
    }


    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {

        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
        templateModel: new Dictionary<string, string>
        {
            { "{{name}}", user.FirstName },
            { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
        }
        );

        BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(user.Email!, "✔ Survey Basket: Email Confirmation", emailBody));

        await Task.CompletedTask;
    }


    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<Result> SendResetPasswordCodeAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        logger.LogInformation("Reset code: {code}", code);

        await SendResetPasswordEmail(user, code);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }

    private async Task SendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
            }
        );

        BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(user.Email!, "✔ Survey Basket: Change Password", emailBody));

        await Task.CompletedTask;
    }
}

