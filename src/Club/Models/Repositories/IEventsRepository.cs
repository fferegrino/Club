﻿using System.Collections.Generic;

namespace Club.Models.Repositories
{
    public interface IEventsRepository
    {
        void AddEvent(Event item);
        IEnumerable<Event> GetAllEvents();

        IEnumerable<Event> GetEventsForMonth(int year, int month, bool showPrivate);

        Event GetEventById(int eventId);
        bool SaveAll();
    }
}