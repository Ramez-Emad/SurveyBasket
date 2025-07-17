using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Results;
public record VoteResponse (
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> SelectedAnswers
    );