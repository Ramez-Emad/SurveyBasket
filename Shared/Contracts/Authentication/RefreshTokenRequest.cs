using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Authentication;
public record RefreshTokenRequest
    (string token , string refreshToken);