using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common;

namespace Club.Models.Repositories
{
    public interface IAnnouncementsRepository 
    {
        IEnumerable<Announcement> GetAllAnnouncements();
        Announcement GetAnnouncementById(int announcementId);
        void AddAnnouncement(Announcement item);
        bool SaveAll();
    }

    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;

        public AnnouncementsRepository(IDateTime date, ClubContext context)
        {
            _date = date;
            _context = context;
        }


        public IEnumerable<Announcement> GetAllAnnouncements()
        {
            return _context.Announcements.ToList();
        }

        public Announcement GetAnnouncementById(int announcementId)
        {
            return _context.Announcements.FirstOrDefault(ann => ann.Id == announcementId);
        }

        public void AddAnnouncement(Announcement item)
        {
            item.CreatedOn = _date.UtcNow;
            _context.Add(item);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
