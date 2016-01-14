using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Club.Models
{
    public class ClubContextSeedData
    {
        private readonly ClubContext _context;
        private readonly UserManager<ClubUser> _userManager;

        public ClubContextSeedData(ClubContext context, UserManager<ClubUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {
            if (await _userManager.FindByEmailAsync("antonio.feregrino@pokemon.com") == null)
            {
                var newUser = new ClubUser { UserName = "fferegrino", Accepted = true,Email = "antonio.feregrino@pokemon.com" };
                await _userManager.CreateAsync(newUser, "P@sword1");
            }

            if (!_context.Events.Any())
            {
                _context.Add(new Event()
                {
                    Name = "Concurso 1",
                    Host = "fferegrino",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddHours(2)
                });
                _context.Add(new Event()
                {
                    Name = "Concurso 2",
                    Host = "fferegrino",
                    Start = DateTime.Now.AddDays(3),
                    End = DateTime.Now.AddDays(3).AddHours(2)
                });

                _context.SaveChanges();
            }
        }
    }
}
