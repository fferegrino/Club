using System;
using System.Collections.Generic;
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
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.Razor.TagHelpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class EventsController : Controller
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IClubUsersRepository _usersRepository;
        private readonly ITermsRepository _termsRepository;
        private readonly IEventCodeGenerator _eventCodeGenerator;
        private readonly IWebUserSession _webSession;
        private readonly IQrCodeApi _qrCodeApi;
        private readonly IAutoMapper _mapper;


        public EventsController(IEventsRepository eventsRepository,
            IAutoMapper mapper,
            IEventCodeGenerator eventCodeGenerator,
            IClubUsersRepository usersRepository,
            IQrCodeApi qrCodeApi, IWebUserSession webSession, ITermsRepository termsRepository)
        {
            _mapper = mapper;
            _eventsRepository = eventsRepository;
            _eventCodeGenerator = eventCodeGenerator;
            _usersRepository = usersRepository;
            _qrCodeApi = qrCodeApi;
            _webSession = webSession;
            _termsRepository = termsRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("index", "calendar");
        }

        public IActionResult Detail(int id)
        {


            var queriedEvent = _eventsRepository.GetEventById(id);
            if (queriedEvent == null 
                || (queriedEvent.IsPrivate
                && !User.Identity.IsAuthenticated))
            {
                return new HttpNotFoundResult();
            }

            var eventViewModel = _mapper.Map<ViewModels.EventViewModel>(queriedEvent);
            string attendanceUrl = Url.Action("attend", new { eventCode = eventViewModel.EventCode });
            var requestUri = Request.ToUri();
            var uri = new UriBuilder
            {
                Scheme = Request.Scheme,
                Host =requestUri.Host,
                Path = Request.PathBase + attendanceUrl,
                Port = requestUri.Port
            };

            eventViewModel.EventCodeUrl = User.IsInRole("Admin") ? 
                _qrCodeApi.GetQrUrl(uri.ToString(), 500) 
                : "/img/defaults/eventcode.png";


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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var term = _mapper.Map<TermViewModel>(_termsRepository.GetCurrentTerm());
            ViewBag.Term = term;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(EventViewModel viewModel)
        {
            var eventEntity = _mapper.Map<Models.Entities.Event>(viewModel);

            eventEntity.EventCode = _eventCodeGenerator.GetCode();
            _eventsRepository.AddEvent(eventEntity);
            
            if (viewModel.Repeat && viewModel.RepeatUntil.HasValue)
            {
                var eventDuration = eventEntity.End - eventEntity.Start;
                for (var start = eventEntity.Start.AddDays(7); start < viewModel.RepeatUntil; start = start.AddDays(7))
                {
                    var repeatedEvent=  _mapper.Map<Models.Entities.Event>(viewModel);
                    repeatedEvent.Start = start;
                    repeatedEvent.End = start + eventDuration;
                    repeatedEvent.EventCode = _eventCodeGenerator.GetCode();
                    _eventsRepository.AddEvent(repeatedEvent);
                }
            }

            _eventsRepository.SaveAll();

            return RedirectToAction("detail", new { id = eventEntity.Id });
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {

            var queriedEvent = _eventsRepository.GetEventById(id);
            var eventViewModel = _mapper.Map<ViewModels.EventViewModel>(queriedEvent);
            return View(eventViewModel);
        }

        // POST: dummy/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _eventsRepository.DeleteById(id);
                _eventsRepository.SaveAll();
                return RedirectToAction("index","calendar");
            }
            catch
            {
                return View();
            }
        }
    }
}
