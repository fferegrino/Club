using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class SimpleUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        public string LastName { get; set; }
        public string Username { get; set; }

        [Display(Name = "Nivel")]
        public string Level { get; set; }
    }
}
