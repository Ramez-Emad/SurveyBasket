using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Threading;

namespace Persistence.Repositories;
public class UserRepository(ApplicationDbContext dbContext) : GenericRepository<ApplicationUser>(dbContext), IUserRepository
{
    public async Task<IEnumerable<string>> GetPermissionFromRoles(IEnumerable<string> roles , CancellationToken cancellationToken)
    {
        var userPermissions = await(from r in dbContext.Roles
                                    join p in dbContext.RoleClaims
                                    on r.Id equals p.RoleId
                                    where roles.Contains(r.Name!)
                                    select p.ClaimValue!)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);

        return userPermissions;
    }
}
