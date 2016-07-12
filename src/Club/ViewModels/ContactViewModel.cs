using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class ContactViewModel
    {

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(200,MinimumLength =5)]
        public string Name { get; set; }


        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }


        [Display(Name = "Mensaje")]
        [Required]
        public string Message { get; set; }
    }
}
