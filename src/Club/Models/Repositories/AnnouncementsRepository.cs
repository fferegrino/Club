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
    public interface IAnnouncementsRepository
    {
        IEnumerable<Announcement> GetAnnouncementsForPeriod(DateTime start, DateTime end, bool showPrivate);
        IEnumerable<Announcement> GetAnnouncementsForMonth(int year, int month, bool showPrivate);
        IEnumerable<Announcement> GetAnnouncementsForCarousel(bool showPrivate);
        IEnumerable<Announcement> GetAllAnnouncements();
        Announcement GetAnnouncementById(int announcementId);
        void AddAnnouncement(Announcement item);
        void UpdateAnnoucement(Announcement item);
        bool SaveAll();
        void DeleteById(int id);
        IEnumerable<Announcement> GetAnnouncementsByDate(DateTime start, DateTime end);
    }

    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private readonly ClubContext _context;
        private readonly IDateTime _date;
        private readonly IUserSession _user;

        public AnnouncementsRepository(IDateTime date,
            ClubContext context, IUserSession user)
        {
            _date = date;
            _context = context;
            _user = user;
        }


        public IEnumerable<Announcement> GetAllAnnouncements()
        {
            return _context.Announcements.Include(evt => evt.ClubUserCreator).ToList();
        }

        public Announcement GetAnnouncementById(int announcementId)
        {
            return _context.Announcements.Include(evt => evt.ClubUserCreator).FirstOrDefault(ann => ann.Id == announcementId);
        }

        public IEnumerable<Announcement> GetAnnouncementsForCarousel(bool showPrivate)
        {
            var results = _context.Announcements.Include(evt => evt.ClubUserCreator).Where(ann => ann.IsCarousel);


            if (showPrivate)
                return results;

            return results.Where(a => a.IsPrivate == false);
        }

        public IEnumerable<Announcement> GetAnnouncementsForPeriod(DateTime start, DateTime end, bool showPrivate)
        {
            var betweenBoundsAnnouncements = _context.Announcements.Include(evt => evt.ClubUserCreator)
                .Where(
                    evnt => ((start < evnt.CreatedOn && evnt.CreatedOn < end) || (start < evnt.DueDate && evnt.DueDate < end))
                );

            if (showPrivate)
            {
                return betweenBoundsAnnouncements.ToList();
            }

            return betweenBoundsAnnouncements.Where(evt => evt.IsPrivate == false).ToList();
        }

        public IEnumerable<Announcement> GetAnnouncementsForMonth(int year, int month, bool showPrivate)
        {

            DateTime start = new DateTime(year, month, 1);
            DateTime end = start.AddMonths(1);
            return GetAnnouncementsForPeriod(start, end, showPrivate);
        }

        public IEnumerable<Announcement> GetAnnouncementsByDate(DateTime start, DateTime end)
        {
            var betweenBoundsAnnouncements = _context.Announcements.Where(a => start <= a.CreatedOn && a.CreatedOn < end);
            return betweenBoundsAnnouncements;
        }

        public void AddAnnouncement(Announcement item)
        {
            item.CreatedOn = _date.Now;
            item.ClubUserCreatorId = _user.Id;
            _context.Add(item);
        }

        public void UpdateAnnoucement(Announcement item)
        {
            var oldItem = _context.Announcements.First(it => it.Id == item.Id);
            oldItem.Name = item.Name;
            oldItem.IsPrivate = item.IsPrivate;
            oldItem.Text = item.Text;
            oldItem.DueDate = item.DueDate;

            _context.Update(oldItem);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void DeleteById(int id)
        {
            var an = _context.Announcements.FirstOrDefault(e => e.Id == id);
            if (an != null)
                _context.Remove(an);
        }
    }
}
