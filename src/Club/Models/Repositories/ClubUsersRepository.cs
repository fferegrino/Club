using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common;
using Club.Models.Entities;

namespace Club.Models.Repositories
{
    public interface IClubUsersRepository
    {
        QueryResult<ClubUser> GetPagedUnapprovedUsers(PagedDataRequest request);
        IEnumerable<ClubUser> GetUnapprovedUsers(int count = 0);
        ClubUser GetUserByUserName(string username);
        void UpdateApprovedStatus(string userId, bool approved);
        bool SaveAll();
        int CountUnaccepted();
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

        public void UpdateApprovedStatus(string userId, bool approved)
        {
            var user = _context.Users.FirstOrDefault(usr => usr.Id == userId);
            user.Approved = approved;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public int CountUnaccepted()
        {
            return _context.Users.Count(usr => usr.Approved == false);
        }

        public QueryResult<ClubUser> GetPagedUnapprovedUsers(PagedDataRequest request)
        {
            var users = _context.Users.Where(user => user.Approved == false);
            var totalItemCount = users.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(request.PageNumber, request.PageSize);
            var toReturnUsers = users.Skip(startIndex).Take(request.PageSize).ToList();
            return new QueryResult<ClubUser>(request.PageSize,totalItemCount, toReturnUsers);
        }

        public IEnumerable<ClubUser> GetUnapprovedUsers(int count = 0)
        {
            var users = _context.Users.Where(user => user.Approved == false);
            if (count == 0)
                return users.ToList();
            else
            {
                return users.Take(count).ToList();
            }
        }

        public ClubUser GetUserByUserName(string username)
        {
            return _context.Users.FirstOrDefault(user => user.UserName == username);
        }
    }
}
