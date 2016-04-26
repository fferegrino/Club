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
    public interface IProblemsRepository
    {
        Problem GetProblemById(int problemId);
    }

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

        public Problem GetProblemById(int problemId)
        {
            return _context.Problems.Include(pr => pr.Topic).FirstOrDefault(pr => pr.Id == problemId);
        }
    }
}
