using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common
{
    public static class Constants
    {
        public static class ApiCommonParameterNames
        {
            public const string PageNumber = "pageNumber";
            public const string PageSize = "pageSize";
        }

        public static class ApiDefaultValues
        {
            public const int MinPageSize = 1;
            public const int MinPageNumber = 1;
            public const int DefaultPageNumber = 1;
            public const string PagedQueryStringFormat = "pageNumber={0}&pageSize={1}";

        }
    }
}
