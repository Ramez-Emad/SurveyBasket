using Domain.Entities;

namespace Service.Specifications;
public sealed class CurrentPollsSpecification : BaseSpecification<Poll>
{
    public CurrentPollsSpecification()
    {
        Criteria = poll => poll.IsPublished &&
                    (poll.StartsAt <= DateOnly.FromDateTime(DateTime.Now)) &&
                    (poll.EndsAt >= DateOnly.FromDateTime(DateTime.Now));
    }
}
