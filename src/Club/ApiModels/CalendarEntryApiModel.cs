using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ApiModels
{
    public class CalendarEntryApiModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string ClassName { get; set; }
        public string Color { get; set; }
        public string Url { get; set; }
    }
}
