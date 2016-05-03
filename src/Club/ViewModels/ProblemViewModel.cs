using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class ProblemViewModel
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public int TopicId { get; set; }
        public string Level { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Site { get; set; }
        [Range(1, 10)]
        public int Difficulty { get; set; }
        public string Notes { get; set; }
    }
}
