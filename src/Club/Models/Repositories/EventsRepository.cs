using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common;

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

        public Event GetEventById(int eventId)
        {
            return _context.Events.Where(evnt => evnt.Id == eventId).FirstOrDefault();
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
