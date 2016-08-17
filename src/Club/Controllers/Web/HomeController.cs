using System;
using Club.Common.Security;
using Microsoft.AspNet.Mvc;
using Club.ViewModels;
using Club.Services;
using Microsoft.Extensions.Localization;
using Microsoft.AspNet.Http;
using Club.Models.Repositories;
using Club.Common.TypeMapping;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class HomeController : Controller
    {
        readonly IMailService _mailService;
        private readonly IWebUserSession _userSession;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IAnnouncementsRepository _announcementsRepo;
        private readonly IAutoMapper _mapper;

        public HomeController(IMailService mailService, 
            IWebUserSession userSession, 
            IStringLocalizer<HomeController> localizer,
            IAnnouncementsRepository announcementsRepo,
            IAutoMapper mapper)
        {
            _mapper = mapper;
            _announcementsRepo = announcementsRepo;
            _localizer = localizer;
            _mailService = mailService;
            _userSession = userSession;
        }

        public IActionResult Index()
        {

            var announcements = _announcementsRepo.GetAnnouncementsForCarousel(User.Identity.IsAuthenticated);
            var a = _mapper.Map<List<AnnouncementCarouselViewModel>>(announcements);
            return View(a);
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
