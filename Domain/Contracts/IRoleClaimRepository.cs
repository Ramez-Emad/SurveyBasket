using Microsoft.AspNetCore.Identity;

namespace Domain.Contracts;

public interface IRoleClaimRepository
{
    Task AddRangeAsync(IEnumerable<IdentityRoleClaim<string>> claims);
    Task<List<string>> GetPermissionsAsync(string roleId);
    Task DeletePermissionsAsync(string roleId, IEnumerable<string> claimValues);
}