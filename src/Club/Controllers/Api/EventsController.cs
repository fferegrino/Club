using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Club.Models.Repositories;
using Club.Common.TypeMapping;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Api
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IAutoMapper _mapper;

        public EventsController(IEventsRepository eventsRepository,
            IAutoMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }

        public JsonResult Get()
        {
            var eventsModels = _eventsRepository.GetAllEvents();
            var eventsViewModels = _mapper.Map<IEnumerable<ViewModels.EventViewModel>>(eventsModels);
            return Json(eventsViewModels);
        }
    }
}
