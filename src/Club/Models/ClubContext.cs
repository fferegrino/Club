using System;
using Club.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            
            var connectionString = Startup.Configuration["Data:ClubContextConnection"];
#else
            var connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AzureClubContextConnection");
#endif
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

        }
    }
}
