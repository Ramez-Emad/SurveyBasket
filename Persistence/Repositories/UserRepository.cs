using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;

namespace Persistence.Repositories;
public class UserRepository(ApplicationDbContext dbContext) : GenericRepository<ApplicationUser>(dbContext) , IUserRepository
{

}
