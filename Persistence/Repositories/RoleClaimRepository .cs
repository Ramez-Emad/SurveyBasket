using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.Abstractions.Consts;

namespace Persistence.Repositories;

public class RoleClaimRepository(ApplicationDbContext _dbContext) : IRoleClaimRepository
{  
    public async Task AddRangeAsync(IEnumerable<IdentityRoleClaim<string>> claims)
    {
        await _dbContext.AddRangeAsync(claims);
    }

    public async Task DeletePermissionsAsync(string roleId, IEnumerable<string> claimValues)
    {
        await _dbContext.RoleClaims
            .Where(x => x.RoleId == roleId && claimValues.Contains(x.ClaimValue))
            .ExecuteDeleteAsync();
    }

    public async Task AddPermissionsAsync(IEnumerable<IdentityRoleClaim<string>> claims)
    {
        await _dbContext.AddRangeAsync(claims);
    }

    public async Task<List<string>> GetPermissionsAsync(string roleId)
    {
        var permissions = await _dbContext!.RoleClaims
           .Where(x => x.RoleId == roleId && x.ClaimType == Permissions.Type)
           .Select(x => x.ClaimValue)
           .ToListAsync();

        return permissions!;
    }
}