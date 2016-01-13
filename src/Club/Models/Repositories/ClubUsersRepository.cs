using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models.Repositories
{
    public interface IClubUsersRepository
    {
        IEnumerable<ClubUser> GetAllUnAcceptedUsers();
    }

    public class ClubUsersRepository : IClubUsersRepository
    {
        private readonly ClubContext _context;

        public ClubUsersRepository(ClubContext context)
        {
            _context = context;
        }

        public IEnumerable<ClubUser> GetAllUnAcceptedUsers()
        {
            return _context.Users.Where(user => user.Accepted == false).ToList();
        }
    }
}
