using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models
{
    public class ClubRepository : IClubRepository
    {
        private readonly ClubContext _context;

        public ClubRepository(ClubContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Events.ToList();
        }
    }
}
