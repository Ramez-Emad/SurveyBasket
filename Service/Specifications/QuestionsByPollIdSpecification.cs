using Domain.Entities;
using Shared;

namespace Service.Specifications;
public class QuestionsByPollIdSpecification : BaseSpecification<Question>
{
    public QuestionsByPollIdSpecification(int pollId, QuestionQueryParams queryParams)
    {

        Criteria = q =>
                        q.PollId == pollId
                        && q.IsActive
                        && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || q.Content.ToLower().Contains(queryParams.SearchValue.ToLower()));


        switch (queryParams.SortingOption)
        {
            case QuestionSortingOptions.ContentAsc:
                AddOrderBy(p => p.Content);
                break;

            case QuestionSortingOptions.ContentDesc:
                AddOrderByDescending(p => p.Content);
                break;

            case QuestionSortingOptions.CreateOnAsc:
                AddOrderBy(p => p.CreatedOn);
                break;

            case QuestionSortingOptions.CreatedOnDesc:
                AddOrderByDescending(p => p.CreatedOn);
                break;

            default:
                break;
        }

        ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

        AddInclude(q => q.Answers);
    }


    public QuestionsByPollIdSpecification(int pollId)
    {

        Criteria = q =>
                        q.PollId == pollId
                        && q.IsActive;

        AddInclude(q => q.Answers);
    }
}