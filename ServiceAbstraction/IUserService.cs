using Domain.Entities;
using ServiceAbstraction.Contracts.Users;
using Shared.Abstractions;

namespace ServiceAbstraction;
public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);

    Task<(IEnumerable<string>, IEnumerable<string>)> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken);
    
}
