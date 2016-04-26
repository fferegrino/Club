using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserLevelId { get; set; }
        public UserLevel Level { get; set; }


        public ICollection<Problem> Problems{ get; set; }

        public Topic()
        {
            Problems = new HashSet<Problem>();
        }
    }
}
