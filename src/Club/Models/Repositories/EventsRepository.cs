using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common;
using Club.Common.Security;
using Club.Models.Entities;
using Microsoft.Data.Entity;

namespace Club.Models.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly IUserSession _user;

        public EventsRepository(ClubContext context, IDateTime date, IUserSession user)
        {
            _context = context;
            _date = date;
            _user = user;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Events.ToList();
        }

        public IEnumerable<Event> GetEventsForMonth(int year, int month, bool showPrivate)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = start.AddMonths(1);

            var betweenBoundsEvents = _context.Events.Include(evt => evt.ClubUserHost)
                .Where(
                    evnt => ((start < evnt.Start && evnt.Start < end) || (start < evnt.End && evnt.End < end))
                );


            if (showPrivate)
            {
                return betweenBoundsEvents.ToList();
            }

            return betweenBoundsEvents.Where(evt => evt.IsPrivate == false).ToList();

        }

        public Event GetNextEvent()
        {
            return _context.Events.Include(evt => evt.ClubUserHost).FirstOrDefault(evt => evt.Start > _date.UtcNow);
        }

        public Event GetEventById(int eventId)
        {
            return _context.Events.Include(evt => evt.ClubUserHost).FirstOrDefault(evnt => evnt.Id == eventId);
        }

        public Event GetEventByEventCode(string eventCode)
        {
            return _context.Events.FirstOrDefault(evnt => evnt.EventCode == eventCode);
        }

        public void AddEvent(Event item)
        {
            item.CreatedOn = _date.UtcNow;
            item.ClubUserHostId = _user.Id;
            
            _context.Add(item);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
