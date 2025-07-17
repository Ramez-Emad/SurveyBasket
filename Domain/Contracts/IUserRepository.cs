using Domain.Entities;
using Shared.Contracts.Users;



namespace Domain.Contracts;
public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<IEnumerable<string>> GetPermissionFromRoles(IEnumerable<string> roles, CancellationToken cancellationToken);

    Task<IEnumerable<UserResponse>> GetUserWithRolesAsync(CancellationToken cancellationToken);

    Task DeleteAllRoles(string id, CancellationToken cancellationToken);

}
