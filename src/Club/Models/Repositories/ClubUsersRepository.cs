using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Club.Common;
using Club.Common.Security;
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
        Task<bool> IsAdmin(string username, bool superAdmin = false);
        void UpdateApprovedStatus(string userId, bool approved);
        bool SaveAll();
        int CountUnapprovedUsers();
        void AttendEvent(string id, Event attendedEvent);
        Task ModifyUser(ClubUser entity, bool modifyNotes = false);
        List<ClubUser> GetAllActiveUsers();
    }

    public class ClubUsersRepository : IClubUsersRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly UserManager<ClubUser> _userManager;
        private readonly IUserSession _user;

        public ClubUsersRepository(ClubContext context, IDateTime date, UserManager<ClubUser> userManager, IUserSession user)
        {
            _context = context;
            _date = date;
            _userManager = userManager;
            _user = user;
        }

        public async Task<bool> IsAdmin(string username, bool superAdmin = false)
        {

            var rolesForUser = await _userManager.GetRolesAsync(GetFullUserByUserName(username));
            return rolesForUser.Contains(superAdmin ? "SuperAdmin" : "Admin");
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
                    AttendedOn = _date.Now
                };
                user.EventsAttended.Add(eventAttendance);
            }
        }

        public async Task ModifyUser(ClubUser entity, bool modifyNotes = false)
        {
            var realEntity = GetUserByUserName(entity.UserName);

            realEntity.FirstName = entity.FirstName;
            realEntity.LastName = entity.LastName;
            realEntity.Email = entity.Email;
            realEntity.UserLevel = _context.UserLevels.First(level => level.Id == entity.UserLevelId);
            realEntity.UserLevelId = entity.UserLevelId;

            var editedUserAdmin = await IsAdmin(entity.UserName, false);
            var editedUserSuperAdmin = await IsAdmin(entity.UserName, true);
            var editingUserIsAdmin = await IsAdmin(_user.Username, false);
            var editingUserIsSuperAdmin = await IsAdmin(_user.Username, true);
            var canModifyStatus = true;

            if (modifyNotes)
            {
                realEntity.Notes = entity.Notes;
            }

            if (editedUserSuperAdmin)
            {
                canModifyStatus = editingUserIsSuperAdmin;
            }

            if (canModifyStatus)
            {
                if (!entity.IsAdmin)
                {
                    var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                    if (adminUsers.Count > 1)
                    {
                        await _userManager.RemoveFromRoleAsync(realEntity, "Admin");
                    }
                }
                else if (entity.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(realEntity, "Admin");
                }
            }

            _context.Update(realEntity);
        }

        public List<ClubUser> GetAllActiveUsers()
        {
            var users = _context.Users.Where(user => user.Approved == true && user.EmailConfirmed)
                .Include(u => u.EventsAttended)
                .Include(u => u.Submissions);

            return users.ToList();
        }
    }
}
