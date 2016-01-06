using System.Collections.Generic;

namespace Club.Models
{
    public interface IClubRepository
    {
        IEnumerable<Event> GetAllEvents();
    }
}