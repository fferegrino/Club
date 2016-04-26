using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class ProblemViewModel
    {
        public int Id { get; set; }
        public string Topic{ get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Difficulty { get; set; }
        public string Notes { get; set; }
    }
}
