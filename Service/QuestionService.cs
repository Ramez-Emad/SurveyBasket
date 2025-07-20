using Domain.Contracts;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.Caching.Hybrid;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.Abstractions;
using Shared.Contracts.Answers;
using Shared.Contracts.Questions;
using Shared.Errors;
using Shared.Shared;
using System.Linq.Expressions;

namespace Service;
public class QuestionService(IUnitOfWork _unitOfWork, HybridCache hybridCache) : IQuestionService
{
    private readonly HybridCache _hybridCache = hybridCache;
    private const string _cachePrefix = "AvailableQuestions";

    public async Task<Result<PaginatedResult<QuestionResponse>>> GetQuestionsAsync(int pollId, QuestionQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        var pollExists = await _unitOfWork.PollRepository.GetByIdAsync(pollId, cancellationToken);

        if (pollExists is null)
        {
            return Result.Failure<PaginatedResult<QuestionResponse>>(PollErrors.PollNotFound);
        }


        var selector = CreateQuestionResponseSelector();

        var questionResponses = await _unitOfWork.QuestionRepository.GetAllAsync(new QuestionsByPollIdSpecification(pollId, queryParams), selector, cancellationToken);


        var totalCount = await _unitOfWork.QuestionRepository.CountAsync(new QuestionsByPollIdSpecificationCount(pollId, queryParams));


        var response = new PaginatedResult<QuestionResponse>(
                                                                queryParams.PageIndex,
                                                                queryParams.PageSize,
                                                                totalCount,
                                                                questionResponses);



        return Result.Success(response);
    }


    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableQuestionsAsync(int pollId, string userId, CancellationToken cancellationToken)
    {
        var pollIsExists = await _unitOfWork.PollRepository.IsPollAvailable(pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);


        var userHasVoted = await _unitOfWork.VoteRepository.UserHasVotedAsync(userId, pollId, cancellationToken);

        if (userHasVoted)
            return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);




        string cacheKey = $"{_cachePrefix}-{pollId}";

        var cachedQuestions = await _hybridCache.GetOrCreateAsync
                                    (cacheKey,
                                    async entry =>
                                    {
                                        var spec = new QuestionsByPollIdSpecification(pollId);


                                        Expression<Func<Question, QuestionResponse>> selector = q => new QuestionResponse(
                                            q.Id,
                                            q.Content,
                                            q.Answers.Where(a => a.IsActive).Select(a =>
                                                new AnswerResponse(a.Id, a.Content)
                                            )
                                        );

                                        return await _unitOfWork.QuestionRepository.GetAllAsync(spec, selector, cancellationToken);
                                    },
                                    cancellationToken: cancellationToken
                                    );

        return Result.Success(cachedQuestions);


    }

    public async Task<Result<QuestionResponse>> CreateQuestionAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var existingPoll = await _unitOfWork.PollRepository.GetByIdAsync(pollId, cancellationToken);

        if (existingPoll is null)
        {
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
        }

        var questionIsExists = await _unitOfWork.QuestionRepository.IsQuestionContentDuplicateAsync(request.Content, pollId, cancellationToken);

        if (questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);


        var question = request.Adapt<Question>();
        question.PollId = pollId;

        await _unitOfWork.QuestionRepository.AddAsync(question, cancellationToken);
        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());

    }

    public async Task<Result<QuestionResponse>> GetQuestionByIdAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var spec = new QuestionWithAnswerSpecification(id);
        var question = await _unitOfWork.QuestionRepository.GetAsync(spec, cancellationToken);

        if (question is null || question.PollId != pollId)
        {
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
        }

        return Result.Success(question.Adapt<QuestionResponse>());
    }


    public async Task<Result> UpdateQuestionAsync(int pollId, int id, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
    {
        var spec = new QuestionWithAnswerSpecification(id);
        var existingQuestion = await _unitOfWork.QuestionRepository.GetAsync(spec, cancellationToken);

        if (existingQuestion is null || existingQuestion.PollId != pollId)
        {
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
        }

        var questionIsExists = await _unitOfWork.QuestionRepository.IsQuestionContentDuplicateAsync(questionRequest.Content, existingQuestion.PollId, existingQuestion.Id, cancellationToken);

        if (questionIsExists)
        {
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);
        }

        existingQuestion.Content = questionRequest.Content;

        var newAnswers = questionRequest.Answers.Except(existingQuestion.Answers.Select(a => a.Content)).ToList();

        foreach (var answerContent in newAnswers)
        {
            existingQuestion.Answers.Add(new Answer { Content = answerContent });
        }

        foreach (var answerContent in existingQuestion.Answers)
        {
            answerContent.IsActive = questionRequest.Answers.Contains(answerContent.Content);
        }

        _unitOfWork.QuestionRepository.Update(existingQuestion);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);


        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _unitOfWork.QuestionRepository.GetByIdAsync(id, cancellationToken);

        if (question is null || question.PollId != pollId)
        {
            return Result.Failure(QuestionErrors.QuestionNotFound);
        }

        question.IsActive = !question.IsActive;
        _unitOfWork.QuestionRepository.Update(question);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        return Result.Success();
    }

    private static Expression<Func<Question, QuestionResponse>> CreateQuestionResponseSelector()
    {
        return q => new QuestionResponse
        (
            q.Id,
            q.Content,
            q.Answers.Select(a => new AnswerResponse(a.Id, a.Content)).ToList()
        );
    }

}
