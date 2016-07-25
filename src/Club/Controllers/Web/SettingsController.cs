using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Entities;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class SettingsController : Controller
    {
        
        //IApplicationEnvironment _app;
        string _assetsFolder;
        public SettingsController(IApplicationEnvironment appEnv)
        {
            _assetsFolder = appEnv.ApplicationBasePath + "\\wwwroot\\assets\\";
        }


        // GET: /<controller>/
        public IActionResult Carta()
        {
            var bytes = System.IO.File.ReadAllBytes(_assetsFolder + "carta.docx");
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "carta.docx");
        }

        // GET: /<controller>/
        [HttpPost]
        public IActionResult Carta(ICollection<IFormFile> carta)
        {
            if (carta.Any())
            {
                carta.First().SaveAs(_assetsFolder + "carta.docx");
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: /<controller>/
        public IActionResult Header()
        {
            var bytes = System.IO.File.ReadAllBytes(_assetsFolder + "headerbg.png");
            return File(bytes, "image/png", "headerbg.png");
        }

        // GET: /<controller>/
        [HttpPost]
        public IActionResult Header(ICollection<IFormFile> header)
        {
            if (header.Any())
            {
                header.First().SaveAs(_assetsFolder + "headerbg.png");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(Startup.Settings);
        }

        // GET: /<controller>/
        [HttpPost]
        public IActionResult Index(SettingsViewModel settings)
        {
            Startup.Settings.HtmlFooter = settings.HtmlFooter;
            Startup.Settings.Theme = settings.Theme;
            return View(Startup.Settings);
        }
    }
}
