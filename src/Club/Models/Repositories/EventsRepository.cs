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

        private ITermsRepository _termsRepo;

        public EventsRepository(ClubContext context, IDateTime date, IUserSession user, ITermsRepository termsRepo)
        {
            _context = context;
            _date = date;
            _user = user;
            _termsRepo = termsRepo;
        }

        public QueryResult<Event> GetPagedEventsAttendedByUsername(PagedDataRequest request, string username)
        {
            var user = _context.Users.First(u => u.UserName == username);
            var attendedEvents = (from ea in _context.EventAttendance
                                  join @event in _context.Events on ea.EventId equals @event.Id
                                  where ea.ClubUserId == user.Id
                                  orderby ea.AttendedOn descending
                                  select @event);

            var totalItemCount = attendedEvents.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(request.PageNumber, request.PageSize);
            var toReturnUsers = attendedEvents.Skip(startIndex).Take(request.PageSize).ToList();

            return new QueryResult<Event>(request.PageSize, totalItemCount, toReturnUsers);
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Events.ToList();
        }

        public IEnumerable<Event> GetEventsForMonth(int year, int month, bool showPrivate)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = start.AddMonths(1);

            var betweenBoundsEvents = _context.Events
                .Include(evt => evt.ClubUserHost)
                .Include(evt => evt.Term)
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
            return _context.Events
                .Include(evt => evt.ClubUserHost)
                .Include(evt => evt.Term)
                .FirstOrDefault(evt => evt.Start > _date.Now);
        }

        public Event GetEventById(int eventId)
        {
            return _context.Events
                .Include(evt => evt.ClubUserHost)
                .Include(evt => evt.Term)
                .FirstOrDefault(evnt => evnt.Id == eventId);
        }

        public Event GetEventByEventCode(string eventCode)
        {
            return _context.Events
                .FirstOrDefault(evnt => evnt.EventCode == eventCode);
        }

        public IEnumerable<EventAttendance> GetAllEventAttendance()
        {
            return _context.EventAttendance
                .Include(ev => ev.ClubUser)
                .Include(ev => ev.Event)
                .Include(ev => ev.Event.Term)
                .Include(ev => ev.Event.ClubUserHost);
        }

        public void DeleteById(int id)
        {
            var @event = _context.Events
                .FirstOrDefault(e => e.Id == id);
            if (@event != null)
                _context.Remove(@event);
        }

        public void AddEvent(Event item)
        {
            item.CreatedOn = _date.Now;
            item.ClubUserHostId = _user.Id;
            var t = _termsRepo.GetTermById(item.TermId);

            item.TermId = t.Id;
            item.Term = t;

            _context.Add(item);
        }

        public void UpdateEvent(Event item)
        {
            var oldEvent = _context.Events.FirstOrDefault(ev => ev.Id == item.Id);

            oldEvent.IsPrivate = item.IsPrivate;
            oldEvent.Name = item.Name;
            oldEvent.Location = item.Location;
            oldEvent.Description = item.Description;
            oldEvent.Type = item.Type;

            oldEvent.Start = item.Start;
            oldEvent.End = item.End;

            oldEvent.Term = _termsRepo.GetTermById(item.TermId);
            oldEvent.TermId = item.TermId;

            _context.Update(oldEvent);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
