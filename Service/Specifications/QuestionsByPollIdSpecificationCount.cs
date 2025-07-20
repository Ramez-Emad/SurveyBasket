using Domain.Entities;
using Shared;

namespace Service.Specifications;
public class QuestionsByPollIdSpecificationCount : BaseSpecification<Question>
{
    public QuestionsByPollIdSpecificationCount(int pollId, QuestionQueryParams queryParams)
    {

        Criteria = q =>
                        q.PollId == pollId
                        && q.IsActive
                        && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || q.Content.ToLower().Contains(queryParams.SearchValue.ToLower()));

    }
}