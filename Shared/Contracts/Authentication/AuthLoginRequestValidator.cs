using FluentValidation;

namespace Shared.Contracts.Authentication;
public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
{
    public AuthLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
