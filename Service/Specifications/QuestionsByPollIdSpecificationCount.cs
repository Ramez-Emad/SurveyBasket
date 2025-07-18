using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications;
public class QuestionsByPollIdSpecificationCount : BaseSpecification<Question>
{
    public QuestionsByPollIdSpecificationCount(int pollId , QuestionQueryParams queryParams)
    {

        Criteria = q =>
                        q.PollId == pollId
                        && q.IsActive
                        && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || q.Content.ToLower().Contains(queryParams.SearchValue.ToLower()));

    }
}