using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Entities
{
    public class UsersEventsAttendedCount
    {
        public int ClubUserId { get; set; }
        public int EventCount { get; set; }
        public int TotalTime { get; set; }
    }
}
