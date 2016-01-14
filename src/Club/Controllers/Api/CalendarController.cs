using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Api
{
    [Route("api/[controller]")]
    public class CalendarController : Controller
    {

        private readonly IEventsRepository _eventsRepository;
        private readonly IAutoMapper _mapper;

        public CalendarController(IEventsRepository eventsRepository,
            IAutoMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _mapper = mapper;
        }

        public JsonResult Get(string start)
        {
            DateTime result = DateTime.Now;

            var dates = start?.Split('-');
            if (dates != null)
            {
                var year = Int32.Parse(dates[0]);
                var month = Int32.Parse(dates[1]);
                var day = Int32.Parse(dates[2]);
                result = new DateTime(year, month, 1)
                    .AddMonths(day == 1 ? 0 : 1);
            }
            var events = _eventsRepository.GetEventsForMonth(result.Year, result.Month, User.Identity.IsAuthenticated);
            var calendarEvents = _mapper.Map<IEnumerable<ViewModels.CalendarEventViewModel>>(events);

            return Json(calendarEvents);
        }
    }
}
