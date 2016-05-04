using System.Collections.Generic;
using Club.Models.Entities;

namespace Club.Models.Repositories
{
    public interface IProblemsRepository
    {
        void AddProblem(Problem problem);
        Problem GetProblemById(int problemId);
        List<Topic> GetTopics();
        bool SaveAll();
    }
}