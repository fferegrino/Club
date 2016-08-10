using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Common.Security
{
    public interface IUserSession
    {
        string Username { get; }

        string Id { get; }

        bool IsInRole(string roleName);

        bool IsAuthenticated { get; }
    }
}
