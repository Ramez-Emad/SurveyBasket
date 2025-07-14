using FluentValidation;
using Shared.Abstractions.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Authentication;
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(req => req.Email)
			.NotEmpty()
			.EmailAddress();


		RuleFor(req => req.Password)
			.NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, Non Alphanumeric and Uppercase");


        RuleFor(req => req.FirstName)
			.NotEmpty()
			.Length(3, 100);



		RuleFor(req => req.LastName)
			.NotEmpty()
            .Length(3, 100);
    }
}
