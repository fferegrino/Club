﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Club.Enums;
using Club.Validation;

namespace Club.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Organizador")]
        public string Host { get; set; }

        [Display(Name = "Ubicación")]
        public string Location { get; set; }

        [Display(Name = "Es privado")]
        public bool IsPrivate { get; set; }

        [Display(Name = "Inicio")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Display(Name = "Fin")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [IsDateAfterAttribute(nameof(Start), ErrorMessage = "Debe ser mayor a la fecha de inicio")]
        public DateTime End { get; set; }

        [Display(Name = "Tipo")]
        public EventType Type { get; set; }

        public string EventCode { get; set; }

        public string EventCodeUrl { get; set; }

        public bool HasSimilar { get; set; }
        public string EditOpt { get; set; }

        [Display(Name = "Duración")]
        public string Duration { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = "Periodo")]
        public int TermId { get; set; }

        [Display(Name = "Periodo")]
        public string TermName { get; set; }

        [Display(Name = "Se repite")]
        public bool Repeat { get; set; }

        [Display(Name = "Repetir hasta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [IsDateAfterAttribute(nameof(End), ErrorMessage = "Debe ser mayor a la fecha de finalización")]
        public DateTime? RepeatUntil { get; set; }

        [Display(Name = "Estatus")]
        public EventStatus Status { get; set; }
    }

    public enum EventStatus
    {
        Past,
        Underway,
        Future
    }
}
