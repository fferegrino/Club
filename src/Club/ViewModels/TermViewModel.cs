using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Club.Enums;

namespace Club.ViewModels
{
    public class TermViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Inicio")]
        public DateTime Start { get; set; }

        [Display(Name = "Fin")]
        public DateTime End { get; set; }
    }
}
