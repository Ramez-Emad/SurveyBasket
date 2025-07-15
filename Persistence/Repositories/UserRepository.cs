using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;
public class UserRepository(ApplicationDbContext dbContext) : GenericRepository<ApplicationUser>(dbContext) , IUserRepository
{

}
