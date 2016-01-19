using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Club.Common;
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
        private readonly IDateTime _dateTime;
        private readonly IEventCodeGenerator _eventCodeGenerator;

        private readonly Random Random;

        public ClubContextSeedData(ClubContext context,
            UserManager<ClubUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEventCodeGenerator eventCodeGenerator,
            IDateTime dateTime)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _eventCodeGenerator = eventCodeGenerator;
            _dateTime = dateTime;
            Random = new Random();
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

            var admin = await GetOrCreateAdmin(adminRole);
            await CreateAditionalUsers(memberRole);


            if (!_context.Announcements.Any())
            {
                AddEvents(admin);
            }

            if (!_context.Events.Any())
            {
                _context.Add(new Event()
                {
                    Name = "Concurso 1",
                    ClubUserHostId = admin.Id,
                    CreatedOn = _dateTime.UtcNow,
                    EventCode = _eventCodeGenerator.GetCode(),
                    Start = DateTime.Now,
                    End = DateTime.Now.AddHours(2)
                });
                _context.Add(new Event()
                {
                    Name = "Concurso 2",
                    ClubUserHostId = admin.Id,
                    CreatedOn = _dateTime.UtcNow,
                    EventCode = _eventCodeGenerator.GetCode(),
                    Start = DateTime.Now.AddDays(3),
                    End = DateTime.Now.AddDays(3).AddHours(2)
                });

                _context.SaveChanges();
            }
        }

        private void AddEvents(ClubUser creator)
        {
            foreach (var announcement in SampleData.SampleAnnouncements)
            {
                int r = Random.Next(1, 65465465) % 365;
                announcement.ClubUserCreatorId = creator.Id;
                announcement.Type = (announcement.DueDate.Ticks % ((long)10) == ((long)0))
                    ? "info"
                    : announcement.IsPrivate ? "warning" : "danger";
                announcement.CreatedOn = DateTime.Now.AddDays(r);
                announcement.DueDate = announcement.DueDate.AddDays(r);
                _context.Add(announcement);
            }
            _context.SaveChanges();
        }

        private async Task<ClubUser> GetOrCreateAdmin(IdentityRole adminRole)
        {

            var admin = await _userManager.FindByEmailAsync("antonio.feregrino@pokemon.com");
            if (admin == null)
            {
                admin = new ClubUser
                {
                    UserName = "fferegrino",
                    FirstName = "Antonio",
                    LastName = "Feregrino",
                    Approved = true,
                    Email = "antonio.feregrino@pokemon.com"
                };

                await _userManager.CreateAsync(admin, "P@sword1");


                var rolesForUser = await _userManager.GetRolesAsync(admin);
                if (!rolesForUser.Contains(adminRole.Name))
                {
                    var result = await _userManager.AddToRoleAsync(admin, adminRole.Name);
                }
            }
            return admin;
        }

        private async Task CreateAditionalUsers(IdentityRole memberRole)
        {
            const string defaultPassword = "@Abc1234";
            foreach (var sampleUser in SampleData.SampleClubUsers)
            {
                await _userManager.CreateAsync(sampleUser, defaultPassword);
                await _userManager.AddToRoleAsync(sampleUser, memberRole.Name);
            }
        }
    }
}
