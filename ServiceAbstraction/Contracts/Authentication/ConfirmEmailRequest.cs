﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Authentication;
public record ConfirmEmailRequest(
    string UserId,
    string Code
    );