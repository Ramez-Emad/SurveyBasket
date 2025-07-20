using Domain.Entities;

namespace Service.Specifications;
public class VoteAnswerForPollSpecification : BaseSpecification<VoteAnswer>
{
    public VoteAnswerForPollSpecification(int pollId)
    {
        Criteria = va => va.Vote.PollId == pollId;
    }
}
