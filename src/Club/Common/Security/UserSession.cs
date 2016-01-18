using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Club.Common.Extensions;
using Microsoft.AspNet.Http;

namespace Club.Common.Security
{
    public class UserSession : IWebUserSession
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserSession(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Username => _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

        public string Id => _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public bool IsInRole(string roleName) => _contextAccessor.HttpContext.User.IsInRole(roleName);

        public Uri RequestUri => _contextAccessor.HttpContext.Request.ToUri();

        public string HttpRequestMethod => _contextAccessor.HttpContext.Request.Method;

    }
}
