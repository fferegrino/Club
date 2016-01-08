﻿using System;
using Microsoft.AspNet.Mvc;
using Club.ViewModels;
using Club.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class HomeController : Controller
    {
        IMailService _mailService;

        public HomeController(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            var email = Startup.Configuration["AppSettings:SiteEmailAddress"];
            if (String.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "ConfigurationError");
            }
            if (ModelState.IsValid)
            {
                _mailService.SendMail(email, email, "Email", model.Message);
                ViewBag.Message = "Sent";
                ModelState.Clear();
            }
            //else
            //{

            //}
            return View();
        }
    }
}