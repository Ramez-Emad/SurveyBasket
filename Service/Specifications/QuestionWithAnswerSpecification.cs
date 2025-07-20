using Domain.Entities;

namespace Service.Specifications;
public sealed class QuestionWithAnswerSpecification : BaseSpecification<Question>
{
    public QuestionWithAnswerSpecification(int id)
    {
        Criteria = q => q.Id == id;

        Includes.Add(q => q.Answers);
    }
}
