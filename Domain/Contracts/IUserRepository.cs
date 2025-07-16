using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;
public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<IEnumerable<string>> GetPermissionFromRoles(IEnumerable<string> roles, CancellationToken cancellationToken);
 }
