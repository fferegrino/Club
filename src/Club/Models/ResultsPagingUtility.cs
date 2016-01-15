using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models
{
    public static class ResultsPagingUtility
    {
        private const string ValueLessThanOneErrorMessage = "Value may not be less than 1.";
        private const string ValueLessThanZeroErrorMessage = "Value may not be less than 0.";

        public static int CalculatePageSize(int requestdeVal, int maxVal)
        {
            if (requestdeVal < 1)
                throw new ArgumentOutOfRangeException(nameof(requestdeVal), requestdeVal, ValueLessThanOneErrorMessage);

            if (maxVal < 1)
                throw new ArgumentOutOfRangeException(nameof(maxVal), maxVal, ValueLessThanOneErrorMessage);

            return Math.Min(requestdeVal, maxVal);
        }

        public static int CalculateStartIndex(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, ValueLessThanOneErrorMessage);
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, ValueLessThanOneErrorMessage);
            var startIndex = (pageNumber - 1) * pageSize;
            return startIndex;
        }

        public static int CalculatePageCount(int totalItemCount, int pageSize)
        {
            if (totalItemCount < 0)
                throw new ArgumentOutOfRangeException(nameof(totalItemCount), totalItemCount, ValueLessThanZeroErrorMessage);
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, ValueLessThanOneErrorMessage);

            var totalPageCount = (totalItemCount + pageSize - 1) / pageSize;
            return totalPageCount;
        }
    }

}
