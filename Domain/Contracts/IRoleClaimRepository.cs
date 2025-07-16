using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;

public interface IRoleClaimRepository
{
    Task AddRangeAsync(IEnumerable<IdentityRoleClaim<string>> claims);
    Task<List<string>> GetPermissionsAsync(string roleId);
    Task DeletePermissionsAsync(string roleId, IEnumerable<string> claimValues);
}