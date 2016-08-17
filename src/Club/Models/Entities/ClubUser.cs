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
        public string StudentId { get; set; }
        public string Phone { get; set; }
        public string GitHubProfile { get; set; }
        public string TwitterProfile { get; set; }
        public string FacebookProfile { get; set; }
        public UserLevel UserLevel { get; set; }
        public int UserLevelId { get; set; }
        public ICollection<EventAttendance> EventsAttended { get; set; }
        public ICollection<Submission> Submissions { get; set; }

        [NotMapped]
        public int EventsAttendedCount { get; set; }

        [NotMapped]
        public bool IsAdmin { get; set; }

        [NotMapped]
        public bool IsSuperAdmin { get; set; }

        public string Notes { get; set; }

        public ClubUser()
        {
            EventsAttended = new HashSet<EventAttendance>();
            Submissions = new HashSet<Submission>();
        }
    }
}
