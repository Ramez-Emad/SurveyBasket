using FluentValidation;

namespace Shared.Contracts.Authentication;
public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.token)
            .NotEmpty()
            .WithMessage("Token is required");
        RuleFor(x => x.refreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}
