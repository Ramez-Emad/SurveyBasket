using Domain.Entities;
using Domain.RepositoriesContracts;
using Mapster;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Questions;
using Shared.Abstractions;
using Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public class QuestionService(IUnitOfWork _unitOfWork) : IQuestionService
{
    public  async Task<Result<IEnumerable<QuestionResponse>>> GetQuestionsAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollExists = await _unitOfWork.PollRepository.GetByIdAsync(pollId, cancellationToken);

        if (pollExists is null)
        {
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
        }

        var questions = await _unitOfWork.QuestionRepository.GetQuestionsByPollIdAsync(pollId, cancellationToken);

        return Result.Success(questions.Adapt<IEnumerable<QuestionResponse>>());
    }


    public async Task<Result<QuestionResponse>> CreateQuestionAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var existingPoll = await _unitOfWork.PollRepository.GetByIdAsync(pollId, cancellationToken);

        if (existingPoll is null)
        {
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
        }

        var questionIsExists = await _unitOfWork.QuestionRepository.ExistsAsync(request.Content, pollId,null, cancellationToken);

        if (questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

       
        var question = request.Adapt<Question>();
        question.PollId = pollId;

        await _unitOfWork.QuestionRepository.AddAsync(question, cancellationToken);
        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());

    }

    public async Task<Result<QuestionResponse>> GetQuestionByIdAsync( int pollId , int id, CancellationToken cancellationToken = default)
    {
        var question = await _unitOfWork.QuestionRepository.GetByIdAsync(id, cancellationToken ,
            q => q.Answers );

        if (question is null || question.PollId != pollId)
        {
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
        }

        return Result.Success(question.Adapt<QuestionResponse>());
    }


    public async Task<Result> UpdateQuestionAsync(int pollId , int id, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
    {
        var existingQuestion = await _unitOfWork.QuestionRepository.GetByIdAsync(id, cancellationToken , q=> q.Answers);

        if (existingQuestion is null || existingQuestion.PollId != pollId)
        {
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
        }

        var questionIsExists = await _unitOfWork.QuestionRepository.ExistsAsync(questionRequest.Content, existingQuestion.PollId, existingQuestion.Id,cancellationToken);

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
        return Result.Success();
    }
}
