using System;
using System.Linq;
using System.Collections.Generic;

namespace Club.Common.Extensions
{
    public static class UriExtensions
    {
        public static Uri GetBaseUri(this Uri uri)
        {
            var queryDelimiterIndex = uri.AbsoluteUri.IndexOf("?", StringComparison.Ordinal);
            return queryDelimiterIndex < 0
                ? uri
                : new Uri(uri.AbsoluteUri.Substring(0, queryDelimiterIndex));
        }

        public static string QueryWithoutLeadingQuestionMark(this Uri uri)
        {
            const int indexToSkipQueryDelimiter = 1;
            return uri.Query.Length > 1 ? uri.Query.Substring(indexToSkipQueryDelimiter) : String.Empty;
        }

        public static Dictionary<string, string> ParseQueryString(this Uri uri)
        {
            string query = uri.QueryWithoutLeadingQuestionMark();
            if (!String.IsNullOrEmpty(query))
            {
                Dictionary<string, string> dictionary =
                        query.Split('&')
                            .ToDictionary(c => c.Split('=')[0],
                               c => Uri.UnescapeDataString(c.Split('=')[1]));

                return dictionary;
            }
            return new Dictionary<string, string>();
        }
    }
}
