using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Authentication;
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
