using System.Collections.Generic;
using Club.Models.Entities;

namespace Club.Models.Repositories
{
    public interface IProblemsRepository
    {
        void AddProblem(Problem problem);
        Problem GetProblemById(int problemId);
        List<Problem> GetProblemsForLevel(int userLevelId);
        List<Problem> GetAllCurrentProblems();
        List<Topic> GetTopics();
        bool SaveAll();
    }
}