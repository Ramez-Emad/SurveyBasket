using Domain.Contracts;
using Domain.Entities;
using Mapster;
using Service.Specifications;
using ServiceAbstraction;
using Shared.Contracts.Answers;
using Shared.Contracts.Questions;
using Shared.Contracts.Votes;
using Shared.Abstractions;
using Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public class VoteService(IUnitOfWork _unitOfWork) : IVoteService
{

  
    public  async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken)
    {

        var pollIsExists = await _unitOfWork.PollRepository.IsPollAvailable(pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);


        var userHasVoted = await _unitOfWork.VoteRepository.UserHasVotedAsync(userId, pollId, cancellationToken);

        if (userHasVoted)
            return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);

        var spec = new AvailableQuestionsSpecification(pollId);
        Expression<Func<Question, int>> selector = q => q.Id;

        var availableQuestions = await _unitOfWork.QuestionRepository.GetAllAsync(spec, selector, cancellationToken);


        if (!request.Answers.Select(x => x.QuestionId).SequenceEqual(availableQuestions))
            return Result.Failure(VoteErrors.InvalidQuestions);

        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
        };

        await _unitOfWork.VoteRepository.AddAsync(vote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        

        return Result.Success();

    }

}
