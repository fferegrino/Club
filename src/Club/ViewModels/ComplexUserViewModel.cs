using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class ComplexUserViewModel : SimpleUserViewModel
    {
        public int EventsAttendedCount { get; set; }
        public int SubmissionsCount { get; set; }
    }
}
