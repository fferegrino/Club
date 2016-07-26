using System;
using Club.Common.Security;
using Microsoft.AspNet.Mvc;
using Club.ViewModels;
using Club.Services;
using Microsoft.Extensions.Localization;
using Microsoft.AspNet.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class HomeController : Controller
    {
        readonly IMailService _mailService;
        private readonly IWebUserSession _userSession;
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(IMailService mailService, IWebUserSession userSession, IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
            _mailService = mailService;
            _userSession = userSession;
        }

        public IActionResult Index()
        {
            string user = "";
            ViewBag.Hello = _localizer["Hello"];

            if (User.Identity.IsAuthenticated)
                user = _userSession.Id;
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetCulture(string culture)
        {

            HttpContext.Response.Cookies.Append(Startup.CultureCookieName, culture, new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = false,

            });
            return RedirectToAction("Index");
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
