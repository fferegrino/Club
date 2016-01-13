using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Repositories
{
    public class ClubRepository : IClubRepository
    {
        internal readonly ClubContext _context;

        public ClubRepository(ClubContext context)
        {
            _context = context;
        }
        
    }
}
