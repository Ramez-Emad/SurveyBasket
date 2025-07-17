using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Shared.Abstractions.Consts;
using Shared.Contracts.Users;
using Shared.Contracts.Users;
using System.Data;
using System.Linq.Expressions;
using System.Threading;

namespace Persistence.Repositories;
public class UserRepository(ApplicationDbContext dbContext) : GenericRepository<ApplicationUser>(dbContext), IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task DeleteAllRoles(string id , CancellationToken cancellationToken)
    {
        await _dbContext.UserRoles
               .Where(x => x.UserId == id)
               .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetPermissionFromRoles(IEnumerable<string> roles , CancellationToken cancellationToken)
    {
        var userPermissions = await(from r in _dbContext.Roles
                                    join p in _dbContext.RoleClaims
                                    on r.Id equals p.RoleId
                                    where roles.Contains(r.Name!)
                                    select p.ClaimValue!)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);

        return userPermissions;
    }

    public async Task<IEnumerable<UserResponse>> GetUserWithRolesAsync(CancellationToken cancellationToken)
    {
         var response = await (from u in _dbContext.Users
                         join ur in _dbContext.UserRoles
                         on u.Id equals ur.UserId
                         join r in _dbContext.Roles
                         on ur.RoleId equals r.Id into roles
                         where !roles.Any(r => r.Name == DefaultRoles.Member)
                         select new
                                 {
                                     u.Id,
                                     u.FirstName,
                                     u.LastName,
                                     u.Email,
                                     u.IsDisabled,
                                     roles = roles.Select(r => r.Name).ToList()
                                 }
                         ).GroupBy(u => new { u.Id,u.FirstName,u.LastName,u.IsDisabled,u.Email})
                         .Select(
                                g => new UserResponse
                                (
                                    g.Key.Id,
                                    g.Key.FirstName,
                                    g.Key.LastName,
                                    g.Key.Email,
                                    g.Key.IsDisabled,
                                    g.SelectMany(x => x.roles).Distinct().ToList()
                                )
                         ).ToListAsync(cancellationToken);

        return response;
    }
}
