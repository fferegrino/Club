using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common;
using Club.Common.Security;
using Club.Models.Entities;
using Microsoft.Data.Entity;

namespace Club.Models.Repositories
{
    public class ProblemsRepository : IProblemsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly IUserSession _user;

        public ProblemsRepository(ClubContext context, IDateTime date, IUserSession user)
        {
            _context = context;
            _date = date;
            _user = user;
        }

        public void AddProblem(Problem item)
        {
            item.AddedOn = _date.Now;
            item.Topic = _context.Topics.First(t => t.Id == item.TopicId);
            item.ClubUserCreatorId = _user.Id;

            _context.Add(item);
        }

        public void UpdateProblem(Problem item)
        {

            var oldItem = _context.Problems.First(pr => pr.Id == item.Id);
            oldItem.Topic = _context.Topics.First(t => t.Id == item.TopicId);

            oldItem.Name = item.Name;
            oldItem.Difficulty = item.Difficulty;
            oldItem.Link = item.Link;

            _context.Update(oldItem);
        }

        public Problem GetProblemById(int problemId)
        {
            var problem = _context.Problems
                .Include(pr => pr.Topic)
                .Include(pr => pr.Topic.Level)
                .FirstOrDefault(pr => pr.Id == problemId);

            if (!_user.IsAuthenticated) return problem;

            var submission = _context.Submissions.FirstOrDefault(s => s.ProblemId == problem.Id && s.UserId == _user.Id);
            if (submission != null)
            {
                problem.Attempted = true;
                problem.Accepted = submission.Accepted;
            }

            return problem;
        }

        public List<Problem> GetProblemsForLevel(int userLevelId)
        {
            var query = from problem in _context.Problems.Include(pr => pr.Topic).Include(p => p.Topic.Level)
                        select problem;

            var problems = query.ToList();

            if (!_user.IsAuthenticated) return problems;

            foreach (var problem in problems)
            {
                var submission = _context.Submissions.FirstOrDefault(s => s.ProblemId == problem.Id && s.UserId == _user.Id);
                if (submission != null)
                {
                    problem.Attempted = true;
                    problem.Accepted = submission.Accepted;
                }
            }

            return problems;
        }

        public List<Problem> GetAllCurrentProblems()
        {
            var query = from problem in _context.Problems.Include(pr => pr.Topic).Include(p => p.Topic.Level)
                        select problem;

            var problems = query.ToList();

            if (!_user.IsAuthenticated) return problems;

            foreach (var problem in problems)
            {
                var submission = _context.Submissions.FirstOrDefault(s => s.ProblemId == problem.Id && s.UserId == _user.Id);
                if (submission != null)
                {
                    problem.Attempted = true;
                    problem.Accepted = submission.Accepted;
                }
            }

            return problems;
        }

        public List<Topic> GetTopics()
        {
            return _context.Topics.Include(t => t.Level).ToList();
        }
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
