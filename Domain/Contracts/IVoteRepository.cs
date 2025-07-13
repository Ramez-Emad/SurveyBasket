using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;
public interface IVoteRepository : IGenericRepository<Vote>
{
    Task<bool> UserHasVotedAsync(string userId, int pollId, CancellationToken cancellationToken = default);
  
}
