﻿using System;
using Club.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System.Linq;
using Microsoft.Data.Entity.Storage;

namespace Club.Models
{
    public class ClubContext : IdentityDbContext<ClubUser>
    {

        public ClubContext()
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttendance> EventAttendance { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Term> Terms { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Startup.Configuration["Data:ClubContextConnection"];
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventAttendance>()
                .HasKey(t => new { t.ClubUserId, t.EventId });

            modelBuilder.Entity<EventAttendance>()
                .HasOne(pt => pt.Event)
                .WithMany(p => p.UsersAttending)
                .HasForeignKey(pt => pt.EventId);

            modelBuilder.Entity<EventAttendance>()
                .HasOne(pt => pt.ClubUser)
                .WithMany(t => t.EventsAttended)
                .HasForeignKey(pt => pt.ClubUserId);


            modelBuilder.Entity<Submission>()
                .HasKey(t => new { t.UserId,t.ProblemId });

            modelBuilder.Entity<Submission>()
                .HasOne(pt => pt.Problem)
                .WithMany(pt => pt.Submissions)
                .HasForeignKey(pt => pt.ProblemId);

            modelBuilder.Entity<Submission>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.Submissions)
                .HasForeignKey(pt => pt.UserId);

            var entiddades = modelBuilder.Model.GetEntityTypes();
            var foreign = entiddades.SelectMany(e => e.GetForeignKeys());

            foreach (var mutableForeignKey in foreign)
            {

                mutableForeignKey.DeleteBehavior = Microsoft.Data.Entity.Metadata.DeleteBehavior.Restrict;
            }

            //modelBuilder.Entity<Problem>()
            //    .Property(p=> p.Id).UseSqlServerIdentityColumn();
            //modelBuilder.Entity<Problem>()
            //    .HasKey(p => new {p.Year, p.Id});


        }
    }
}
