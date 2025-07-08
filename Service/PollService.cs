using Domain.Entities;
using Domain.Exceptions;
using Domain.RepositoriesContracts;
using Mapster;
using ServiceAbstraction;
using ServiceAbstraction.Contracts.Requests;
using ServiceAbstraction.Contracts.Responses;

public class PollService(IUnitOfWork unitOfWork) : IPollService
{
    public async Task<IEnumerable<PollResponse>> GetAllPollsAsync(CancellationToken cancellationToken = default)
    {
        var polls = await unitOfWork.PollRepository.GetAllAsync(cancellationToken);
        return polls.Adapt<IEnumerable<PollResponse>>();
    }

    public async Task<PollResponse?> GetPollByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        if (poll is null)
            throw new PollNotFoundException(id);

        return poll.Adapt<PollResponse>();
    }

    public async Task<PollResponse> CreatePollAsync(PollRequest request, CancellationToken cancellationToken = default)
    {
        var pollEntity = request.Adapt<Poll>();
        await unitOfWork.PollRepository.AddAsync(pollEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return pollEntity.Adapt<PollResponse>();
    }

    public async Task<PollResponse> UpdatePollAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
    {
        var existingPoll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        if (existingPoll is null)
            throw new PollNotFoundException(id);

        request.Adapt(existingPoll);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return existingPoll.Adapt<PollResponse>();
    }

    public async Task<bool> DeletePollAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        if (poll is null)
            throw new PollNotFoundException(id);

        unitOfWork.PollRepository.Delete(poll);
        return await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<PollResponse> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await unitOfWork.PollRepository.GetByIdAsync(id, cancellationToken);

        if (poll is null)
            throw new PollNotFoundException(id);

        poll.IsPublished = !poll.IsPublished;

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return poll.Adapt<PollResponse>();
    }
}
