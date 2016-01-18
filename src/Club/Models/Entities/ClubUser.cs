using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Club.Models.Entities
{
    public class ClubUser : IdentityUser
    {
        public bool Approved { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<EventAttendance> EventsAttended { get; set; }

        public ClubUser()
        {
            EventsAttended = new HashSet<EventAttendance>();
        }
    }
}
