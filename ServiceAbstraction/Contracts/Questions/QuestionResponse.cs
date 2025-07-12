using ServiceAbstraction.Contracts.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Questions;

public record QuestionResponse(
    int Id,
    string Content,
    IEnumerable<AnswerResponse> Answers
);