﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Questions;

public record QuestionRequest(
    string Content,
    List<string> Answers
);