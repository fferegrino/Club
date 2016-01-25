using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Enums;

namespace Club.ApiModels
{
    public class EventApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool IsPrivate { get; set; }
        public EventType Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Duration { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
