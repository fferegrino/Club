using System;
using System.Collections.Generic;
using Club.Common;
using Club.Common.Security;
using Club.Models.Entities;
using System.Linq;
using Microsoft.Data.Entity;

namespace Club.Models.Repositories
{
    public interface ISubmissionsRepository
    {
        void AddOrUpdateSubmission(Submission s, bool update);
        void ReviewSubmission(int problemId, string userId, bool accept);

        IEnumerable<Submission> GetAllSubmissionsForUser(string userId = null);
        IEnumerable<Submission> GetAllRecentSubmissions();
        int GetAllPendingSubmissionsCount();
        Submission GetSubmissionForProblem(int problemId, string userId = null);

        bool SaveAll();
    }

    public class SubmissionsRepository : ISubmissionsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly IUserSession _user;

        public SubmissionsRepository(ClubContext context, IDateTime date, IUserSession user)
        {
            _context = context;
            _date = date;
            _user = user;
        }

        public void AddOrUpdateSubmission(Submission s, bool update)
        {

            if (update)
            {
                var ss = GetSubmissionForProblem(s.ProblemId, _user.Id);
                ss.GistUrl = s.GistUrl;
                ss.LastAttemptDate = _date.UtcNow;
                ss.Accepted = null;
                ss.Attempts++;
                _context.Update(ss);
            }
            else
            {
                var user = _context.Users.First(u => u.Id == _user.Id);
                s.User = user;

                var problem = _context.Problems.First(p => p.Id == s.ProblemId);
                s.Problem = problem;
                s.LastAttemptDate = _date.UtcNow;
                s.Accepted = null;
                s.Attempts++;
                _context.Update(s);
            }
        }

        public void ReviewSubmission(int problemId, string userId, bool accept)
        {
            var ss = GetSubmissionForProblem(problemId, userId);
            ss.Accepted = accept;
            _context.Update(ss);
        }

        public IEnumerable<Submission> GetAllSubmissionsForUser(string userId = null)
        {
            var user = _context.Users.First(r => r.UserName == userId);
            var problem =
                _context.Submissions
                .Include(s => s.User)
                .Include(s => s.Problem)
                .Where(pr =>  pr.UserId == user.Id && pr.Accepted.HasValue && pr.Accepted.Value);
            return problem;
        }

        public Submission GetSubmissionForProblem(int problemId, string userId = null)
        {
            var problem =
                _context.Submissions
                .Include(s => s.User)
                .Include(s => s.Problem)
                .FirstOrDefault(
                    pr => pr.ProblemId == problemId && pr.UserId == (userId ?? _user.Id));

            return problem;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Submission> GetAllRecentSubmissions()
        {
            var problem =
                _context.Submissions
                .Include(s => s.User)
                .Include(s => s.Problem)
                .Where(pr => pr.LastAttemptDate >= _date.UtcNow.AddMonths(-6));

            return problem;
        }

        public int GetAllPendingSubmissionsCount()
        {
            var problem =
                _context.Submissions
                .Where(s=> s.Accepted == null)
                .Where(pr => pr.LastAttemptDate >= _date.UtcNow.AddMonths(-6));

            var rcount = problem.Count();

            return rcount;
        }
    }
}