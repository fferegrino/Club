using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Web
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsRepository _eventsRepository;
        private readonly IAutoMapper _mapper;

        public AnnouncementsController(IAutoMapper mapper, IAnnouncementsRepository eventsRepository)
        {
            _mapper = mapper;
            _eventsRepository = eventsRepository;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AnnouncementViewModel viewModel)
        {
            var model = _mapper.Map<Models.Announcement>(viewModel);
            _eventsRepository.AddAnnouncement(model);

            _eventsRepository.SaveAll();

            return RedirectToAction("detail", new { announcementId = model.Id });
        }
    }
}
