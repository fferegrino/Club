using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class EditUserViewModel : SimpleUserViewModel
    {

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nivel")]
        public int LevelId { get; set; }

        [Display(Name = "Es administrador")]
        public bool IsAdmin { get; set; }

    }
}
