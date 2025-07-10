using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction;
public interface IJwtProvider
{
    (string token ,int expiresIn) GenerateToken(ApplicationUser applicationUser);
    string? ValidateToken(string token);
}
