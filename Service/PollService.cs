using Domain.Contracts;
using Domain.Entities;
using Hangfire;
using Mapster;
using Service.Specifications;
using ServiceAbstraction;
using Shared.Contracts.Polls;
using Shared.Abstractions;
using Shared.Errors;

public class PollService(IUnitOfWork unitOfWork, INotificationService _notificationService) : IPollService
{
    public async Task<IEnumerable<PollResponse>> GetAllPollsAsync(CancellationToken cancellationToken = default)
    {
        var polls = await unitOfWork.PollRepository.GetAllAsync(cancellationToken);
        return polls.Adapt<IEnumerable<PollResponse>>();
    }

    public async Task<Result<PollResponse>> GetPollByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        return poll is null
         ? Result.Failure<PollResponse>(PollErrors.PollNotFound)
         : Result.Success(poll.Adapt<PollResponse>());
    }

    public async Task<Result<PollResponse>> CreatePollAsync(PollRequest request, CancellationToken cancellationToken = default)
    {
        var isTitleExists = await unitOfWork.PollRepository.TitleExistsAsync(request.Title, cancellationToken: cancellationToken);

        if (isTitleExists)
            return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

        var pollEntity = request.Adapt<Poll>();
        await unitOfWork.PollRepository.AddAsync(pollEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);


        return Result.Success(pollEntity.Adapt<PollResponse>());
    }

    public async Task<Result> UpdatePollAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
    {
      
        var existingPoll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        if (existingPoll is null)
            return Result.Failure(PollErrors.PollNotFound);

        var isTitleExists = await unitOfWork.PollRepository.TitleExistsAsync(request.Title, id , cancellationToken: cancellationToken);

        if (isTitleExists)
            return Result.Failure(PollErrors.DuplicatedPollTitle);

        request.Adapt(existingPoll);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeletePollAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);


        unitOfWork.PollRepository.Delete(poll);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        poll.IsPublished = !poll.IsPublished;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (poll.IsPublished && poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
            BackgroundJob.Enqueue(() => _notificationService.SendNewPollsNotification(poll.Id));

        return Result.Success();
    }

    public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var spec = new CurrentPollsSpecification();

        var polls = await unitOfWork.PollRepository.GetAllAsync(spec, cancellationToken);

        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Result.Success(response);
    }

}
