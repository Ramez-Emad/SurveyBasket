namespace Shared.Contracts.Users;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);