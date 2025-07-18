using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared;

public class QuestionQueryParams
{
    private const int defaultPageSize = 5;
    private const int maxPageSize = 10;
    public QuestionSortingOptions SortingOption { get; set; }
    public string? SearchValue { get; set; }
    public int PageIndex { get; set; } = 1;

    private int pageSize = defaultPageSize;

    public int PageSize
    {
        get { return pageSize; }
        set
        {
            if (value > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            else
            {
                pageSize = value;
            }
        }
    }
}