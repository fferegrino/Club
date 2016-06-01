using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Entities
{
    public class Problem
    {
        public int Id { get; set; }
        //public string Ciclo { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Difficulty { get; set; }
        public string Notes { get; set; }
        public string ClubUserCreatorId { get; set; }
        public ClubUser ClubUserCreator { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
