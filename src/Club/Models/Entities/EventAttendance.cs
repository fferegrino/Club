using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Entities
{
    public class EventAttendance
    {
        public int EventId { get; set; }
        public Event Event { get; set; }

        public string ClubUserId { get; set; }
        public ClubUser ClubUser { get; set; }
        public DateTime AttendedOn { get; set; }
    }
}
