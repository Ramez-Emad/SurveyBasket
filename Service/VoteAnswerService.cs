using Domain.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public class VoteAnswerService(IUnitOfWork _unitOfWork) : IVoteAnswerService
{
}
