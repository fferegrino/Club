using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class DashboardViewModel
    {
        public int UpcomingEventsCount { get; set; }
        public EventViewModel NextEvent { get; set; }


        public int AnnouncementsCount { get; set; }
        public IEnumerable<AnnouncementViewModel> RecentAnnouncements { get; set; }

        public int UsersAwaitingApprovalCount { get; set; }

        [Display(Name = "Usuarios pendientes de aprobación")]
        public IEnumerable<SimpleUserViewModel> UsersAwaitingApproval { get; set; }



    }
}
