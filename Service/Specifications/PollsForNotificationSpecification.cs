using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
