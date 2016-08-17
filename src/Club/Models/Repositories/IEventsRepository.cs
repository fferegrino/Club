using System.Collections.Generic;
using Club.Models.Entities;
using System;

namespace Club.Models.Repositories
{
    public enum EventEditOption
    {
        All,
        Single,
        SingleAndNext
    }

    public interface IEventsRepository
    {
        void AddEvent(Event item);
        void UpdateEvent(Event item, EventEditOption option = EventEditOption.Single);
        QueryResult<Event> GetPagedEventsAttendedByUsername(PagedDataRequest request, string username);

        IEnumerable<Event> GetAllEvents();

        IEnumerable<Event> GetEventsForPeriod(DateTime start, DateTime end, bool showPrivate);

        IEnumerable<Event> GetEventsForMonth(int year, int month, bool showPrivate);

        Event GetNextEvent();

        Event GetEventById(int eventId);

        Event GetEventByEventCode(string eventCode);

        IEnumerable<EventAttendance> GetAllEventAttendance();

        void DeleteById(int id, EventEditOption option = EventEditOption.Single);

        bool SaveAll();
    }
}