using Microsoft.Data.Entity;

namespace Club.Models
{
    public class ClubContext : DbContext
    {

        public ClubContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Event> Events { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Startup.Configuration["Data:ClubContextConnection"];
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
