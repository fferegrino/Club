using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly ClubContext _context;

        public EventsRepository(ClubContext context)
        {
            _context = context;
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
            _context.Add(item);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
