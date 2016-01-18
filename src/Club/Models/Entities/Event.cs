using System;

namespace Club.Models.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ClubUserHostId { get; set; }
        public ClubUser ClubUserHost { get; set; }
        public string Location { get; set; }
        public bool IsPrivate { get; set; }
        public string EventCode { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
