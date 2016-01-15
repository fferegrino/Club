using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models
{
    public class QueryResult<T>
    {
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages 
            => ResultsPagingUtility.CalculatePageCount(TotalItems, PageSize);
        public IEnumerable<T> Items { get; }

        public QueryResult(int pageSize,
            int totalItems,
            IEnumerable<T> items)
        {
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items;
        }
    }
}
