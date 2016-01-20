using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public int EventsAttendedCount { get; set; }

        public ClubUser()
        {
            EventsAttended = new HashSet<EventAttendance>();
        }
    }
}
