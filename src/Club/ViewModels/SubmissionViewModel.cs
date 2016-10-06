using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class SubmissionViewModel
    {

        [Display(Name = "Username")]
        public string User { get; set; }

        public int ProblemId { get; set; }

        [Display(Name = "Nombre")]
        public string ProblemName { get; set; }

        [Url(ErrorMessage ="Debe ser una dirección web")]
        public string GistUrl { get; set; }

        public string GistId { get; set; }

        [Display(Name = "Aceptado")]
        public bool? Accepted { get; set; }

        [Display(Name = "Último intento")]
        public DateTime LastAttemptDate { get; set; }

        [Display(Name = "Intentos")]
        public int Attempts { get; set; }
        public string FileContent { get; set; }
        public string Comment { get; set; }
    }
}
