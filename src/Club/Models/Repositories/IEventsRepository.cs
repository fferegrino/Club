using System.Collections.Generic;
using Club.Models.Entities;


namespace Club.Models.Repositories
{
    public interface IEventsRepository
    {
        void AddEvent(Event item);
        QueryResult<Event> GetPagedEventsAttendedByUsername(PagedDataRequest request, string username);

        IEnumerable<Event> GetAllEvents();

        IEnumerable<Event> GetEventsForMonth(int year, int month, bool showPrivate);

        Event GetNextEvent();

        Event GetEventById(int eventId);

        Event GetEventByEventCode(string eventCode);
        void DeleteById(int id);

        bool SaveAll();
    }
}