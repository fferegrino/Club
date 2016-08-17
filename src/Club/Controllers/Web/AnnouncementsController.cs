using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Club.Common;
using Microsoft.AspNet.Http;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using Microsoft.AspNet.Hosting;
using ImageProcessor.Imaging;

namespace Club.Controllers.Web
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsRepository _announcementsRepository;
        private readonly IAutoMapper _mapper;
        private readonly IDateTime _dateTime;
        private string _assetsFolder;

        public AnnouncementsController(IAutoMapper mapper,
            IAnnouncementsRepository announcementsRepository,
            IHostingEnvironment hostEnv,
            IDateTime dateTime)
        {
            _assetsFolder = hostEnv.MapPath("assets/a");
            _dateTime = dateTime;
            _mapper = mapper;
            _announcementsRepository = announcementsRepository;
        }

        public IActionResult Index()
        {
            var modelAnnouncements = _announcementsRepository.GetAllAnnouncements();
            var vmAnnouncements = _mapper.Map<IEnumerable<ViewModels.AnnouncementViewModel>>(modelAnnouncements);
            return View(vmAnnouncements);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AnnouncementViewModel viewModel, ICollection<IFormFile> announcementImage)
        {
            bool isAValidCarousel = announcementImage.Any();
            
            if (ModelState.IsValid )
            {
                var model = _mapper.Map<Models.Entities.Announcement>(viewModel);
                model.ClubUserCreatorId = User.Identity.Name;

                if(isAValidCarousel)
                {
                    var fileImage = Guid.NewGuid().ToString("N");
                    var realFileName = $"{viewModel.DueDate:yyyyMMdd}-" + fileImage.Substring(7);
                    string file = _assetsFolder + $"\\{realFileName}.png";
                    var image = announcementImage.First();
                    using (var inStream = image.OpenReadStream())
                    {
                        ISupportedImageFormat format = new PngFormat { Quality = 100 };
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                        {

                            imageFactory.Load(inStream)
                                        .Format(format)
                                        .Save(file);
                            model.ImageUrl = realFileName;
                        }
                    }
                }

                _announcementsRepository.AddAnnouncement(model);
                _announcementsRepository.SaveAll();

                return RedirectToAction("detail", new { id = model.Id });

            }
            return View(viewModel);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Edit(int id)
        {
            var queriedEvent = _announcementsRepository.GetAnnouncementById(id);
            if (queriedEvent != null
                && queriedEvent.IsPrivate
                && !User.Identity.IsAuthenticated)
            {
                return new HttpNotFoundResult();
            }


            var eventViewModel = _mapper.Map<ViewModels.AnnouncementViewModel>(queriedEvent);

            ViewBag.EditAllowed = _dateTime.Now < eventViewModel.DueDate;
            return View(eventViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(AnnouncementViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<Models.Entities.Announcement>(viewModel);
                model.ClubUserCreatorId = User.Identity.Name;
                _announcementsRepository.UpdateAnnoucement(model);

                _announcementsRepository.SaveAll();

                return RedirectToAction("detail", new { id = model.Id });

            }
            return View(viewModel);
        }

        public IActionResult Detail(int id)
        {
            var queriedEvent = _announcementsRepository.GetAnnouncementById(id);
            if (queriedEvent != null
                && queriedEvent.IsPrivate
                && !User.Identity.IsAuthenticated)
            {
                return new HttpNotFoundResult();
            }

            var eventViewModel = _mapper.Map<ViewModels.AnnouncementViewModel>(queriedEvent);


            ViewBag.EditAllowed = _dateTime.Now < eventViewModel.DueDate;

            return View(eventViewModel);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {

            var queriedEvent = _announcementsRepository.GetAnnouncementById(id);
            var eventViewModel = _mapper.Map<ViewModels.AnnouncementViewModel>(queriedEvent);
            return View(eventViewModel);
        }

        // POST: dummy/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _announcementsRepository.DeleteById(id);
                _announcementsRepository.SaveAll();
                return RedirectToAction("index", "calendar");
            }
            catch
            {
                return View();
            }
        }
    }
}
