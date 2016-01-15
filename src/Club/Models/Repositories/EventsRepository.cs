using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common;
using Club.Models.Entities;

namespace Club.Models.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;

        public EventsRepository(ClubContext context, IDateTime date)
        {
            _context = context;
            _date = date;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Events.ToList();
        }

        public IEnumerable<Event> GetEventsForMonth(int year, int month, bool showPrivate)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = start.AddMonths(1);

            var betweenBoundsEvent = _context.Events.Where(
                    evnt => ((start < evnt.Start && evnt.Start < end) || (start < evnt.End && evnt.End < end))
                );


            if (showPrivate)
            {
                return betweenBoundsEvent.ToList();
            }

            return betweenBoundsEvent.Where(evt => evt.IsPrivate == false).ToList();

        }

        public Event GetNextEvent()
        {
            return _context.Events.FirstOrDefault(evt => evt.Start > _date.UtcNow);
        }

        public Event GetEventById(int eventId)
        {
            return _context.Events.FirstOrDefault(evnt => evnt.Id == eventId);
        }

        public void AddEvent(Event item)
        {
            item.CreatedOn = _date.UtcNow;
            _context.Add(item);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
