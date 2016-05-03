using System.Collections.Generic;
using Club.Models.Entities;

namespace Club.Models.Repositories
{
    public interface IProblemsRepository
    {
        Problem GetProblemById(int problemId);
        List<Topic> GetTopics();
    }
}