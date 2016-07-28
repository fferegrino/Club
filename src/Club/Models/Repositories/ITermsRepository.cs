using System.Collections.Generic;
using Club.Models.Entities;


namespace Club.Models.Repositories
{
    public interface ITermsRepository
    {
        void AddTerm(Term item);
        Term GetTermById(int termId);
        Term GetLastTerm();
        Term GetCurrentTerm();
        List<Term> GetCurrentAndNextTerms();
        List<Term> GetAllTerms();
        bool SaveAll();
    }
}