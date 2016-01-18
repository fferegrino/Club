﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class UsersController : Controller
    {

        [Authorize(Roles = "Admin")]
        public IActionResult Unapproved()
        {
            return View();
        }
    }
}
