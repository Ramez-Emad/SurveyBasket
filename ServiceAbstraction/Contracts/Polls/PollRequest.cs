﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Polls;
public record PollRequest(
    string Title,
    string Summary,
    DateOnly StartsAt,
    DateOnly EndsAt
    );
