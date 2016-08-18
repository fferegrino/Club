using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ApiModels
{
    public class SubmissionApiModel
    {
        public string User { get; set; }
        public int ProblemId { get; set; }
        public string ProblemName { get; set; }
        //public string Status { get; set; }
        public bool? Accepted { get; set; }
        public DateTime LastAttemptDate { get; set; }
    }
}
