namespace Shared.Contracts.Authentication;
public record class RegisterRequest
{
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Password { get; init; } = default!;
}