using ServiceAbstraction.Contracts.Questions;
using ServiceAbstraction.Contracts.Votes;
using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction;
public interface IVoteService
{
    Task<Result<IEnumerable<QuestionResponse>>> GetQuestionsAsync(int pollId , string userId , CancellationToken cancellationToken);

    Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken);
}
