using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Club.Common;
using Club.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Club.Models.Repositories
{
    public interface IClubUsersRepository
    {
        QueryResult<ClubUser> GetPagedUsersWithAttendance(PagedDataRequest request);
        QueryResult<ClubUser> GetPagedUnapprovedUsers(PagedDataRequest request);
        IEnumerable<ClubUser> GetMostActiveUsers(int count = 5);
        IEnumerable<ClubUser> GetUnapprovedUsers(int count = 0);
        ClubUser GetUserByUserName(string username);
        ClubUser GetFullUserByUserName(string username);
        ClubUser GetUserById(string id);
        bool IsAdmin(string username);
        void UpdateApprovedStatus(string userId, bool approved);
        bool SaveAll();
        int CountUnapprovedUsers();
        void AttendEvent(string id, Event attendedEvent);
    }

    public class ClubUsersRepository : IClubUsersRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly UserManager<ClubUser> _userManager;

        public ClubUsersRepository(ClubContext context, IDateTime date, UserManager<ClubUser> userManager)
        {
            _context = context;
            _date = date;
            _userManager = userManager;
        }

        public bool IsAdmin(string username)
        {

            var ttt = _userManager.GetRolesAsync(GetFullUserByUserName(username));
            ttt.Wait();
            var rolesForUser = ttt.Result;
            return rolesForUser.Contains("Admin");
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

        public int CountUnapprovedUsers()
        {
            return _context.Users.Count(usr => usr.Approved == false && usr.EmailConfirmed);
        }

        public QueryResult<ClubUser> GetPagedUsersWithAttendance(PagedDataRequest request)
        {

            var users = (from ea in _context.EventAttendance
                         group ea by ea.ClubUserId into u
                         join usr in _context.Users on u.Key equals usr.Id
                         orderby u.Count() descending
                         select usr);

            var totalItemCount = users.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(request.PageNumber, request.PageSize);
            var toReturnUsers = users.Skip(startIndex).Take(request.PageSize).ToList();

            // Ugly hack around not being able to load EventsAttendedCount at the previous query
            toReturnUsers.ForEach(u => u.EventsAttendedCount = _context.EventAttendance.Count(c => c.ClubUserId == u.Id));
            return new QueryResult<ClubUser>(request.PageSize, totalItemCount, toReturnUsers);
        }

        public QueryResult<ClubUser> GetPagedUnapprovedUsers(PagedDataRequest request)
        {
            var users = _context.Users.Where(user => user.Approved == false && user.EmailConfirmed);
            var totalItemCount = users.Count();
            var startIndex = ResultsPagingUtility.CalculateStartIndex(request.PageNumber, request.PageSize);
            var toReturnUsers = users.Skip(startIndex).Take(request.PageSize).ToList();
            return new QueryResult<ClubUser>(request.PageSize, totalItemCount, toReturnUsers);
        }

        public IEnumerable<ClubUser> GetMostActiveUsers(int count = 5)
        {
            var users = (from ea in _context.EventAttendance
                         group ea by ea.ClubUserId into u
                         join usr in _context.Users on u.Key equals usr.Id
                         orderby u.Count() descending
                         select usr);


            var mostActiveUsers = users.Take(5).ToList();
            // Ugly hack around not being able to load EventsAttendedCount at the previous query
            mostActiveUsers.ForEach(u => u.EventsAttendedCount = _context.EventAttendance.Count(c => c.ClubUserId == u.Id));
            return mostActiveUsers;
        }

        public IEnumerable<ClubUser> GetUnapprovedUsers(int count = 0)
        {
            var users = _context.Users.Where(user => user.Approved == false && user.EmailConfirmed);
            if (count == 0)
                return users.ToList();
            else
            {
                return users.Take(count).ToList();
            }
        }

        public ClubUser GetUserByUserName(string username)
        {
            return _context.Users
                .Include(u => u.UserLevel)
                .FirstOrDefault(user => user.UserName == username);
        }

        public ClubUser GetFullUserByUserName(string username)
        {
            return _context.Users
                   .Include(u => u.UserLevel)
                   .Include(u => u.EventsAttended)
                    .ThenInclude(a => a.Event)
                    .ThenInclude(a => a.Term)
                   //.Include(u => u.EventsAttended.Select(y => y.Event.Term))
                   .FirstOrDefault(user => user.UserName == username);
        }

        public ClubUser GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }

        public void AttendEvent(string username, Event attendedEvent)
        {
            var user = _context.Users.FirstOrDefault(usr => usr.UserName == username);
            var hasAttendance =
                _context.EventAttendance.Any(
                    attendance =>
                    attendance.ClubUserId == user.Id
                    && attendance.EventId == attendedEvent.Id);

            if (!hasAttendance)
            {
                var eventAttendance = new EventAttendance()
                {
                    EventId = attendedEvent.Id,
                    ClubUserId = user.Id,
                    AttendedOn = _date.UtcNow
                };
                user.EventsAttended.Add(eventAttendance);
            }
        }
    }
}
