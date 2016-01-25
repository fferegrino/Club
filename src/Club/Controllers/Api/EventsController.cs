using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.ApiModels;
using Club.Common.Extensions;
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
        private readonly IPagedDataRequestFactory _requestFactory;
        private readonly IAutoMapper _mapper;

        public EventsController(IEventsRepository eventsRepository,
            IAutoMapper mapper, IPagedDataRequestFactory requestFactory)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
            _requestFactory = requestFactory;
        }

        public JsonResult Get()
        {
            var eventsModels = _eventsRepository.GetAllEvents();
            var eventsViewModels = _mapper.Map<IEnumerable<ViewModels.EventViewModel>>(eventsModels);
            return Json(eventsViewModels);
        }

        [HttpGet("attended/{*username}")]
        public JsonResult GetAttendedEvents(string username)
        {
            var request = _requestFactory.Create(Request.ToUri());
            var eventsModels = _eventsRepository.GetPagedEventsAttendedByUsername(request, username);
            var eventsViewModels = _mapper.Map<PagedDataResponse<ApiModels.EventApiModel>>(eventsModels);
            return Json(eventsViewModels);
        }
    }
}
