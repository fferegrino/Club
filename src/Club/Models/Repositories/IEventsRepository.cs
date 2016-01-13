using System.Collections.Generic;

namespace Club.Models.Repositories
{
    public interface IEventsRepository
    {
        void AddEvent(Event item);
        IEnumerable<Event> GetAllEvents();
        Event GetEventById(int eventId);
        bool SaveAll();
    }
}