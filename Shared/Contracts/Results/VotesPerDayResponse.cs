namespace Shared.Contracts.Results;
public record VotesPerDayResponse(
    DateOnly Date,
    int NumberOfVotes
    );