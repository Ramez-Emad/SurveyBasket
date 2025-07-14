using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications;
public class VoteAnswerForPollSpecification : BaseSpecification<VoteAnswer>
{
    public VoteAnswerForPollSpecification(int pollId)
    {
        Criteria = va => va.Vote.PollId == pollId;
    }
}
