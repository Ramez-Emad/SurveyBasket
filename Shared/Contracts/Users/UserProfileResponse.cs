﻿namespace Shared.Contracts.Users;
public record UserProfileResponse(
    string Email,
    string FirstName,
    string LastName
    );