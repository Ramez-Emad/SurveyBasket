using Domain.Entities;

namespace Service.Specifications;
public class AvailableQuestionsSpecification : BaseSpecification<Question>
{
    public AvailableQuestionsSpecification(int pollId)
    {
        Criteria = q => q.PollId == pollId && q.IsActive;
    }
}
