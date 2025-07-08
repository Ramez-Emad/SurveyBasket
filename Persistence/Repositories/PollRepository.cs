using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;

namespace Persistence.Repositories;
public class PollRepository(ApplicationDbContext _dbContext) : GenericRepository<Poll>(_dbContext), IPollRepository
{

}
