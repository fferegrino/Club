﻿using System;
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
        IEnumerable<Announcement> GetAllAnnouncements();
        Announcement GetAnnouncementById(int announcementId);
        void AddAnnouncement(Announcement item);
        bool SaveAll();
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
            return _context.Announcements.ToList();
        }

        public Announcement GetAnnouncementById(int announcementId)
        {
            return _context.Announcements.Include(evt => evt.ClubUserCreator).FirstOrDefault(ann => ann.Id == announcementId);
        }

        public void AddAnnouncement(Announcement item)
        {
            item.CreatedOn = _date.UtcNow;
            item.ClubUserCreatorId = _user.Id;
            _context.Add(item);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
