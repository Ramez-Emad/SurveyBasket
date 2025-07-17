using Domain.Contracts;
using Domain.Entities;
using Service.Specifications;
using ServiceAbstraction;
using Shared.Contracts.Results;
using Shared.Abstractions;
using Shared.Contracts.Results;
using Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public class ResultService(IUnitOfWork _unitOfWork) : IResultService
{
    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken)
    {
        var poll = await _unitOfWork.PollRepository.GetByIdAsync(pollId);

        if (poll is null)
            return Result.Failure<PollVotesResponse>(PollErrors.PollNotFound);


        var spec = new VotesByPollIdSpecification(pollId);

        Expression<Func<Vote, VoteResponse>> selector = v => new VoteResponse(
                                                                    v.User.FirstName + " " + v.User.LastName,
                                                                    v.SubmittedOn,
                                                                    v.VoteAnswers.Select(
                                                                        va => new QuestionAnswerResponse(
                                                                            va.Question.Content,
                                                                            va.Answer.Content)
                                                                            )
                                                                    );

        var voteResponses = await _unitOfWork.VoteRepository.GetAllAsync(spec, selector, cancellationToken);

        var response = new PollVotesResponse(poll.Title, voteResponses);

        return Result.Success(response);

    }

    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken)
    {
        var poll = await _unitOfWork.PollRepository.GetByIdAsync(pollId);

        if (poll is null)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);


        var spec = new VotesByPollIdSpecification(pollId);


        var response = await _unitOfWork.VoteRepository.GetGroupedAsync(
            spec,
            v => DateOnly.FromDateTime(v.SubmittedOn),
            g => new VotesPerDayResponse(
                g.Key,
                g.Count()
            ),
            cancellationToken
        );

        return Result.Success(response);
    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var poll = await _unitOfWork.PollRepository.GetByIdAsync(pollId);

        if (poll is null)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);

        var spec = new VoteAnswerForPollSpecification(pollId);

        var allAnswers = await _unitOfWork.VoteAnswerRepository.GetGroupedAsync(
                        spec,

                        groupBySelector: x => new { x.Question.Content, AnswerId = x.Answer.Id, AnswerContent = x.Answer.Content },

                        resultSelector: g => new {
                                                    g.Key.Content,
                                                    g.Key.AnswerContent,
                                                    Count =g.Count() },
                        cancellationToken
                    );

        var response = allAnswers.GroupBy(g=> g.Content)
                                 .Select(g => new VotesPerQuestionResponse(
                                     g.Key,
                                     g.Select(x => new VotesPerAnswerResponse(
                                         x.AnswerContent,
                                         x.Count
                                     ))
                                 ));

        return Result.Success(response);
    }
}
