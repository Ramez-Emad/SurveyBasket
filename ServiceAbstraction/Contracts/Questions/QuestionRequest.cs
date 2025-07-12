using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Questions;

public record QuestionRequest(
    string Content,
    List<string> Answers
);