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
    public interface ITopicsRepository
    {
        void AddTopic(Topic topic);

        IEnumerable<Topic> GetAllTopics();

        bool SaveAll();
    }

    public class TopicsRepository : ITopicsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly IUserSession _user;

        public TopicsRepository(ClubContext context, IDateTime date, IUserSession user)
        {
            _context = context;
            _date = date;
            _user = user;
        }

        public void AddTopic(Topic topic)
        {
            _context.Add(topic);
        }

        public IEnumerable<Topic> GetAllTopics()
        {
            return _context.Topics.Include(t => t.Level);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
