using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction.Contracts.Authentication;
public record class RegisterRequest
{
    public string Email { get; init; } 
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Password { get; init; }
}