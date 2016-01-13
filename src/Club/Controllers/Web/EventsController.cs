using System.Collections.Generic;
using Club.Common.TypeMapping;
using Club.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Club.ViewModels;
using Club.Models.Repositories;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IAutoMapper _mapper;


        public EventsController(IEventsRepository eventsRepository, IAutoMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var events = _eventsRepository.GetAllEvents();
            var eventViewModels = _mapper.Map<IEnumerable<ViewModels.EventViewModel>>(events);
            return View(eventViewModels);
        }

        public IActionResult Detail(int eventId)
        {
            var queriedEvent = _eventsRepository.GetEventById(eventId);
            var eventViewModel = _mapper.Map<ViewModels.EventViewModel>(queriedEvent);
            return View(eventViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EventViewModel viewModel)
        {
            var eventEntity = _mapper.Map<Models.Event>(viewModel);
            _eventsRepository.AddEvent(eventEntity);

            _eventsRepository.SaveAll();
            
            return RedirectToAction("detail",new { eventId =  eventEntity.Id});
        }
    }
}
