using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
