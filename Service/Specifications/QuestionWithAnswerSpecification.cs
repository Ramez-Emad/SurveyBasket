using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications;
public sealed class QuestionWithAnswerSpecification : BaseSpecification<Question>
{
    public QuestionWithAnswerSpecification(int id)
    {
        Criteria = q => q.Id == id;

        Includes.Add(q => q.Answers);
    }
}
