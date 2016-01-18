using System.Collections.Generic;
using Club.Models.Entities;


namespace Club.Models.Repositories
{
    public interface IEventsRepository
    {
        void AddEvent(Event item);
        IEnumerable<Event> GetAllEvents();

        IEnumerable<Event> GetEventsForMonth(int year, int month, bool showPrivate);

        Event GetNextEvent();

        Event GetEventById(int eventId);

        Event GetEventByEventCode(string eventCode);

        bool SaveAll();
    }
}