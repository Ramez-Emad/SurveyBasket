using Domain.Entities;

namespace ServiceAbstraction;
public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? ValidateToken(string token);
}
