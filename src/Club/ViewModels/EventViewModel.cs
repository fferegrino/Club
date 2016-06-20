using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Club.Enums;

namespace Club.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Host { get; set; }
        public string Location { get; set; }

        public bool IsPrivate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }
        public EventType Type { get; set; }

        public string EventCode { get; set; }

        public string EventCodeUrl { get; set; }

        public string Duration { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TermId { get; set; }
        public string TermName { get; set; }

        public bool Repeat { get; set; }
        public DateTime? RepeatUntil { get; set; }
    }
}
