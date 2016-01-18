using System;
using System.Linq;
using System.Threading.Tasks;
using Club.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Club.Models.Context
{
    public class ClubContextSeedData
    {
        private readonly ClubContext _context;
        private readonly UserManager<ClubUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClubContextSeedData(ClubContext context,
            UserManager<ClubUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task EnsureSeedData()
        {
            var adminRoleName = "Admin";
            var adminRole = await _roleManager.FindByNameAsync(adminRoleName);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(adminRoleName);
                var roleresult = await _roleManager.CreateAsync(adminRole);
            }

            var memberRoleName = "Member";
            var memberRole = await _roleManager.FindByNameAsync(memberRoleName);
            if (memberRole == null)
            {
                memberRole = new IdentityRole(memberRoleName);
                var roleresult = await _roleManager.CreateAsync(memberRole);
            }

            if (await _userManager.FindByEmailAsync("antonio.feregrino@pokemon.com") == null)
            {
                var newUser = new ClubUser { UserName = "fferegrino", Approved = true, Email = "antonio.feregrino@pokemon.com" };

                await _userManager.CreateAsync(newUser, "P@sword1");


                var rolesForUser = await _userManager.GetRolesAsync(newUser);
                if (!rolesForUser.Contains(adminRole.Name))
                {
                    var result = await _userManager.AddToRoleAsync(newUser, adminRoleName);
                }
            }


            if (!Queryable.Any<Event>(_context.Events))
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
