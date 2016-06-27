using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Club.Enums;

namespace Club.ApiModels
{
    public class EventAttendanceApiModel
    {
        public int Id { get; set; }

        [Required]
        public string EventName { get; set; }
        public string Description { get; set; }

        public string Host { get; set; }
        public string Location { get; set; }
        
        public DateTime Date{ get; set; }

        public EventType Type { get; set; }

        public double Duration { get; set; }
        public string UserId { get; set; }

        public string User { get; set; }
        

        public int TermId { get; set; }
        public string TermName { get; set; }
    }
}
