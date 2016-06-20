using System.Linq;
using Club.Common;
using Club.Common.Security;
using Club.Models.Entities;

namespace Club.Models.Repositories
{
    public class TermsRepository : ITermsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly IUserSession _user;

        public TermsRepository(ClubContext context, 
            IDateTime date, 
            IUserSession user)
        {
            _context = context;
            _date = date;
            _user = user;
        }

        public void AddEvent(Term item)
        {
            throw new System.NotImplementedException();
        }

        public Term GetCurrentTerm()
        {
            var term = _context.Terms.FirstOrDefault(t => t.Start <= _date.UtcNow && _date.UtcNow <= t.End);
            return term;
        }

        public Term GetTermById(int termId)
        {
            var term = _context.Terms.First(t => t.Id == termId);
            return term;
        }
    }
}