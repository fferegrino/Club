using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        //[RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).{8,24}$", ErrorMessage = "La contraseña debe tener 1 letra mayúscula, 1 minúscula y un número")]
        public string Password { get; set; }


        [Display(Name = "Confirmación")]
        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }


        [Display(Name = "Usuario")]
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
