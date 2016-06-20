using System.Collections.Generic;
using Club.Models.Entities;


namespace Club.Models.Repositories
{
    public interface ITermsRepository
    {
        void AddEvent(Term item);
        Term GetTermById(int termId);
        Term GetCurrentTerm();
    }
}