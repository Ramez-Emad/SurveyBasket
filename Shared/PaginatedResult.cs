using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    namespace Shared
    {
        public class PaginatedResult<TEntity>(int pageIndex, int pageSize, int totalCount, IEnumerable<TEntity> data)
        {
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize;
            public int TotalPages { get; } = (int)Math.Ceiling(totalCount / (double)pageSize);
            public bool HasPreviousPage => PageIndex > 1;
            public bool HasNextPage => PageIndex < TotalPages;
            public IEnumerable<TEntity> Data { get; } = data;
        }
    }

}
