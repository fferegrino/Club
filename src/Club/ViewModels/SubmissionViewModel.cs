using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class SubmissionViewModel
    {
        public string User { get; set; }
        public int ProblemId { get; set; }
        public string ProblemName { get; set; }
        public string GistUrl { get; set; }
        public string GistId { get; set; }
        public bool? Accepted { get; set; }
        public DateTime LastAttemptDate { get; set; }
        public int Attempts { get; set; }
        public string Comment { get; set; }
    }
}
