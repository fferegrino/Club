using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Entities
{
    public class Problem
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Difficulty { get; set; }
        public string Notes { get; set; }
        public string ClubUserCreatorId { get; set; }
        public ClubUser ClubUserCreator { get; set; }
        public DateTime AddedOn { get; set; }

        [NotMapped]
        public bool Attempted { get; set; }

        [NotMapped]
        public bool? Accepted { get; set; }

        public ICollection<Submission> Submissions { get; set; }

        public Problem()
        {
            Submissions = new HashSet<Submission>();
        }

    }
}
