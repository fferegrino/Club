using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Creator { get; set; }
        public string Type { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DueDate { get; set; }
        public string HumanizedDueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
