using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class CalendarEventViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Inicio")]
        public string Start { get; set; }

        [Display(Name = "Fin")]
        public string End { get; set; }
        public string ClassName { get; set; }
        public string Color { get; set; }
        public string Url { get; set; }
    }
}
