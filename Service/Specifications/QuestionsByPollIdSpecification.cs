using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications;
public class QuestionsByPollIdSpecification : BaseSpecification<Question>
{
    public QuestionsByPollIdSpecification(int pollId)
    {
        Criteria = q => q.PollId == pollId;
        AddInclude(q => q.Answers);
    }
}