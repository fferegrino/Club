﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Web
{
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsRepository _announcementsRepository;
        private readonly IAutoMapper _mapper;

        public AnnouncementsController(IAutoMapper mapper,
IAnnouncementsRepository announcementsRepository)
        {
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
        public IActionResult Create(AnnouncementViewModel viewModel)
        {
            var model = _mapper.Map<Models.Entities.Announcement>(viewModel);
            model.ClubUserCreatorId = User.Identity.Name;
            _announcementsRepository.AddAnnouncement(model);

            _announcementsRepository.SaveAll();

            return RedirectToAction("detail", new { id = model.Id });
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
