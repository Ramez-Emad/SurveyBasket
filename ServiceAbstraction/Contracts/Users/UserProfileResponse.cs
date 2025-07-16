namespace ServiceAbstraction.Contracts.Users;
public record UserProfileResponse(
    string Email,
    string FirstName,
    string LastName
    );