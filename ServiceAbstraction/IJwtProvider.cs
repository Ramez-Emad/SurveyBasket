using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction;
public interface IJwtProvider
{
    (string token ,int expiresIn) GenerateToken(ApplicationUser applicationUser ,IEnumerable<string> roles , IEnumerable<string> permissions);
    string? ValidateToken(string token);
}
