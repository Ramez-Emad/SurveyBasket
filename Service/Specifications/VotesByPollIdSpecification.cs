using Domain.Entities;

namespace Service.Specifications;
public class VotesByPollIdSpecification : BaseSpecification<Vote>
{

    public VotesByPollIdSpecification(int pollId)
    {
        Criteria = v => v.PollId == pollId;
    }
}
