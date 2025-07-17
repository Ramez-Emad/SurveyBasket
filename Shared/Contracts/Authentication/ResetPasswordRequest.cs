﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Authentication;

public record ResetPasswordRequest(
    string Email,
    string Code,
    string NewPassword
);