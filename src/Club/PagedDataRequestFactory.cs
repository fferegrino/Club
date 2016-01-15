using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Club.Common;
using Club.Common.Extensions;
using Club.Models;

namespace Club
{
    public interface IPagedDataRequestFactory
    {
        PagedDataRequest Create(Uri requestUri);
    }

    public class PagedDataRequestFactory : IPagedDataRequestFactory
    {
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 25;
        public PagedDataRequest Create(Uri requestUri)
        {
            // I keep feeling that this doesn't need to full uri, just the query string.
            int? pageNumber,
                    pageSize;

            try
            {
                var valueDicitionary = requestUri.ParseQueryString();
                pageNumber = PrimitiveTypeParser.Parse<int?>(valueDicitionary[Constants.ApiCommonParameterNames.PageNumber]);
                pageSize = PrimitiveTypeParser.Parse<int?>(valueDicitionary[Constants.ApiCommonParameterNames.PageSize]);
            }
            catch (Exception e)
            {
                // log 
                pageNumber = 0;
                pageSize = 0;
            }
            pageNumber = pageNumber.GetBoundedValue(Constants.ApiDefaultValues.DefaultPageNumber, Constants.ApiDefaultValues.MinPageNumber);
            pageSize = pageSize.GetBoundedValue(DefaultPageSize, Constants.ApiDefaultValues.MinPageSize, MaxPageSize);

            return new PagedDataRequest(pageNumber.Value, pageSize.Value);
        }

    }
}
