﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Club.Enums;
using Club.Validation;

namespace Club.ViewModels
{
    public class TermViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Inicio")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Display(Name = "Fin")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [IsDateAfterAttribute(nameof(Start), ErrorMessage = "Debe ser mayor a la fecha de inicio")]
        public DateTime End { get; set; }
    }
}
