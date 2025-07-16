using ServiceAbstraction.Contracts.Authentication;
using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction;
public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

    Task<Result> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken);

    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);

    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);

    Task<Result> SendResetPasswordCodeAsync(string email);

    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
}

