using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Entities
{
    public class Submission
    {
        public string UserId { get; set; }
        public ClubUser User { get; set; }
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        public string GistUrl { get; set; }
        public bool? Accepted { get; set; }
        public DateTime LastAttemptDate { get; set; }
        public int Attempts { get; set; }
        public string Comment { get; set; }
    }
}
