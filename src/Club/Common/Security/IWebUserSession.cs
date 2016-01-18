using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common.Security
{
    public interface IWebUserSession : IUserSession
    {
        Uri RequestUri { get; }

        string HttpRequestMethod { get; }
    }
}
