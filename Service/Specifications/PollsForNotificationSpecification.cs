using Domain.Entities;

namespace Service.Specifications;
public class PollsForNotificationSpecification : BaseSpecification<Poll>
{
    public PollsForNotificationSpecification(int? pollId)
    {
        Criteria = poll =>
            poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow) &&
            poll.IsPublished &&
            (pollId == null || poll.Id == pollId.Value);
    }
}
