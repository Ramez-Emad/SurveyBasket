using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Answers;
public record AnswerResponse(
    int Id,
    string Content
);