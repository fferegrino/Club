using System.Collections.Generic;
using Club.Common;
using Club.Common.Security;
using Club.Models.Entities;


namespace Club.Models.Repositories
{
    public interface IUserLevelsRepository
    {
        void AddUserLevel(UserLevel level);

        IEnumerable<UserLevel> GetAllUsersLevels();

        bool SaveAll();
    }

    public class UserLevelsRepository : IUserLevelsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly IUserSession _user;

        public UserLevelsRepository(ClubContext context, IDateTime date, IUserSession user)
        {
            _context = context;
            _date = date;
            _user = user;
        }

        public void AddUserLevel(UserLevel level)
        {
            _context.Add(level);
        }

        public IEnumerable<UserLevel> GetAllUsersLevels()
        {
            return _context.UserLevels;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}