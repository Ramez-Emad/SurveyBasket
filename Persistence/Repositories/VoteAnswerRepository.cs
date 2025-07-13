using Domain.Entities;
using Domain.Contracts;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;
public class VoteAnswerRepository(ApplicationDbContext dbContext) : GenericRepository<VoteAnswer>(dbContext) , IVoteAnswerRepository
{
}
