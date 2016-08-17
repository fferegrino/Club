using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        public string Username { get; set; }

        [Display(Name = "Boleta")]
        [Required]
        public string StudentId { get; set; }

        [Required]
        public string Email { get; set; }


        [Display(Name = "Nombre")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        //[RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,24}$", ErrorMessage = "La contraseña debe tener 1 letra mayúscula, 1 minúscula y un número")]
        public string Password { get; set; }

        [Display(Name = "Confirmación")]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
    }
}
