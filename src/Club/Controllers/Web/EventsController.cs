﻿using System.Collections.Generic;
using Club.Common.TypeMapping;
using Club.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Club.ViewModels;
using Club.Models.Repositories;
using Club.Common;
using Club.Common.Extensions;
using Club.Common.Security;
using Club.Models.Entities;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Razor.TagHelpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class EventsController : Controller
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IClubUsersRepository _usersRepository;
        private readonly IEventCodeGenerator _eventCodeGenerator;
        private readonly IWebUserSession _userSession;
        private readonly IAutoMapper _mapper;


        public EventsController(IEventsRepository eventsRepository,
            IAutoMapper mapper,
            IEventCodeGenerator eventCodeGenerator, IClubUsersRepository usersRepository, IWebUserSession userSession)
        {
            _mapper = mapper;
            _eventsRepository = eventsRepository;
            _eventCodeGenerator = eventCodeGenerator;
            _usersRepository = usersRepository;
            _userSession = userSession;
        }

        public IActionResult Index()
        {
            return RedirectToAction("index", "calendar");
        }

        public IActionResult Detail(int id)
        {


            var queriedEvent = _eventsRepository.GetEventById(id);
            if (queriedEvent != null
                && queriedEvent.IsPrivate
                && !User.Identity.IsAuthenticated)
            {
                return new HttpNotFoundResult();
            }

            var eventViewModel = _mapper.Map<ViewModels.EventViewModel>(queriedEvent);
            eventViewModel.EventCodeUrl = Url.Action("attend", new { eventCode = eventViewModel.EventCode });
            return View(eventViewModel);
        }

        [Authorize]
        public IActionResult Attend(string eventCode)
        {
            var attendedEvent = _eventsRepository.GetEventByEventCode(eventCode);
            _usersRepository.AttendEvent(User.Identity.Name, attendedEvent);

            _usersRepository.SaveAll();

            return RedirectToAction("detail", new { id = attendedEvent.Id });
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(EventViewModel viewModel)
        {
            var eventEntity = _mapper.Map<Models.Entities.Event>(viewModel);

            eventEntity.EventCode = _eventCodeGenerator.GetCode();
            _eventsRepository.AddEvent(eventEntity);

            _eventsRepository.SaveAll();

            return RedirectToAction("detail", new { id = eventEntity.Id });
        }
    }
}
