using System;

namespace Club.Models.Entities
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string ClubUserCreatorId { get; set; }
        public ClubUser ClubUserCreator { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
