using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications;
public class AvailableQuestionsSpecification : BaseSpecification<Question>
{
    public AvailableQuestionsSpecification(int pollId)
    {
        Criteria = q => q.PollId == pollId && q.IsActive;
    }
}
