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
        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<SignUpRequest> SignUpRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Startup.Configuration["Data:ClubContextConnection"];
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
