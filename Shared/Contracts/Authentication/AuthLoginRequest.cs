namespace Shared.Contracts.Authentication;

public record AuthLoginRequest(
    string Email,
    string Password
);
