using System.Collections.Generic;
using Club.Common.TypeMapping;
using Club.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Club.ViewModels;
using Club.Models.Repositories;
using Club.Common;
using Club.Models.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class EventsController : Controller
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IEventCodeGenerator _eventCodeGenerator;
        private readonly IAutoMapper _mapper;


        public EventsController(IEventsRepository eventsRepository,
            IAutoMapper mapper,
            IEventCodeGenerator eventCodeGenerator)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
            _eventCodeGenerator = eventCodeGenerator;
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
            return View(eventViewModel);
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
            eventEntity.ClubUserHostId = User.Identity.Name;
            _eventsRepository.AddEvent(eventEntity);

            _eventsRepository.SaveAll();

            return RedirectToAction("detail", new { id = eventEntity.Id });
        }
    }
}
