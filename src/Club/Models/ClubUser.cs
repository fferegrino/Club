﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Club.Models
{
    public class ClubUser : IdentityUser
    {
        public bool Accepted { get; set; }

    }
}
