using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class EditUserViewModel : SimpleUserViewModel
    {
        public string Email { get; set; }
        public int LevelId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }

    }
}
