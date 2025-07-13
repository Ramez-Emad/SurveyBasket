using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Votes;


public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(x => x.Answers)
            .NotEmpty();


        RuleForEach(x => x.Answers)
            .SetInheritanceValidator(
                                    v => v.Add(new VoteAnswerRequestValidator())
                                     );
    }
}