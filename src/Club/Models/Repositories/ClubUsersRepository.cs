using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common;

namespace Club.Models.Repositories
{
    public interface IClubUsersRepository
    {
        IEnumerable<ClubUser> GetAllUnAcceptedUsers();
    }

    public class ClubUsersRepository : IClubUsersRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;

        public ClubUsersRepository(ClubContext context, IDateTime date)
        {
            _context = context;
            _date = date;
        }

        public IEnumerable<ClubUser> GetAllUnAcceptedUsers()
        {
            return _context.Users.Where(user => user.Accepted == false).ToList();
        }
    }
}
