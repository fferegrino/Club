using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Club.Models.Entities
{
    public class ClubRole : IdentityRole
    {
        public ClubRole(string roleName)
            :base(roleName)
        {
            
        }
    }
}
