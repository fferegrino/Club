using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Entities
{
    public class Term
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ICollection<Event> Events { get; set; }

        public Term()
        {
            Events = new HashSet<Event>();
        }
    }
}
