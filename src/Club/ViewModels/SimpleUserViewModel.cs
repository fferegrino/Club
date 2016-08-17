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

        [Display(Name = "Notas")]
        public string Notes { get; set; }

        [Display(Name = "Boleta")]
        public string StudentId { get; set; }

        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Display(Name = "GitHub")]
        public string GitHubProfile { get; set; }

        [Display(Name = "Twitter")]
        public string TwitterProfile { get; set; }

        [Display(Name = "Facebook")]
        public string FacebookProfile { get; set; }
    }
}
