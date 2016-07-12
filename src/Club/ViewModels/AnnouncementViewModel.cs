using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Enums;
using System.ComponentModel.DataAnnotations;

namespace Club.ViewModels
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Name { get; set; }

        [Display(Name = "Texto")]
        public string Text { get; set; }

        [Display(Name = "Creador")]
        public string Creator { get; set; }

        [Display(Name = "Tipo")]
        public AnnouncementType Type { get; set; }

        [Display(Name = "Privado")]
        public bool IsPrivate { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime DueDate { get; set; }
        public string HumanizedDueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
