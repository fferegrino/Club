using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Club.Common;
using Club.Models.Entities;
using Club.Models.Enums;
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

            if (!_context.UserLevels.Any())
            {
                AddUserLevels();
            }

            const string adminRoleName = "Admin";
            var adminRole = await _roleManager.FindByNameAsync(adminRoleName);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(adminRoleName);
                var roleresult = await _roleManager.CreateAsync(adminRole);
            }

            const string memberRoleName = "Member";
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
                AddAnnouncements(admin);
            }

            if (!_context.Events.Any())
            {
                AddEvents(admin);
            }

            if (!_context.EventAttendance.Any())
                AddUsersEventsAttendance();

            if (!_context.Topics.Any())
            {
                AddTopics();
            }

            if (!_context.Problems.Any())
            {
                AddProblems(admin);
            }


        }

        private UserLevel basicUserLevel,
            basicIntermediateUserLevel,
            intermediateUserLevel,
            advancedIntermediateUserLevel,
            advancedUserLevel;

        private void AddUserLevels()
        {
            basicUserLevel = new UserLevel { Level = "Basico" };
            basicIntermediateUserLevel = new UserLevel { Level = "Basico-Intermedio" };
            intermediateUserLevel = new UserLevel { Level = "Intermedio" };
            advancedIntermediateUserLevel = new UserLevel { Level = "Intermedio-Avanzado" };
            advancedUserLevel = new UserLevel { Level = "Avanzado" };
            _context.AddRange(basicUserLevel, basicIntermediateUserLevel, intermediateUserLevel, advancedIntermediateUserLevel, advancedUserLevel);
            _context.SaveChanges();
        }

        private Topic barridoDeslizante,
            barridoAcumulativo,
            teoriaNumeros,
            greedy,
            busquedaBinaria;
        private void AddTopics()
        {
            //barridoDeslizante = new Topic { Name = "Barrido deslizante", UserLevelId = basicUserLevel.Id };
            //barridoAcumulativo = new Topic { Name = "Barrido acumulativo", UserLevelId = basicIntermediateUserLevel.Id };
            //teoriaNumeros = new Topic { Name = "Teoría de números", UserLevelId = basicUserLevel.Id };
            //greedy = new Topic { Name = "Greedy", UserLevelId = basicUserLevel.Id };
            //busquedaBinaria = new Topic { Name = "Búsqeda binaria", UserLevelId = basicIntermediateUserLevel.Id };
            barridoDeslizante = new Topic { Name = "Barrido deslizante", UserLevelId = basicUserLevel.Id };
            barridoAcumulativo = new Topic { Name = "Barrido acumulativo", UserLevelId = basicIntermediateUserLevel.Id };
            busquedaBinaria = new Topic { Name = "Búsqeda binaria", UserLevelId = intermediateUserLevel.Id };
            greedy = new Topic { Name = "Greedy", UserLevelId = advancedIntermediateUserLevel.Id };
            teoriaNumeros = new Topic { Name = "Teoría de números", UserLevelId = advancedUserLevel.Id };
            _context.AddRange(barridoDeslizante, barridoAcumulativo, teoriaNumeros, greedy, busquedaBinaria);
            _context.SaveChanges();
        }

        #region Problemas
        private Problem[] problemasBarridoDeslizante = new Problem[]
        {
            new Problem {Name="Las cuentas del hippie", Link="https://omegaup.com/arena/problem/cuentas#problems", Difficulty=2},
            new Problem {Name="Writing", Link="https://omegaup.com/arena/problem/writing#problems", Difficulty=4},
            new Problem {Name="Jardin", Link="https://omegaup.com/arena/problem/Garden#problems", Difficulty=7},
            new Problem {Name="Buscando Oro", Link="https://omegaup.com/arena/problem/buscaoro#problems", Difficulty=5},
            new Problem {Name="Herd sums", Link="https://omegaup.com/arena/problem/herdsums#problems", Difficulty=2},
        };

        private Problem[] problemasBarridoAcumulativo = new Problem[]
        {
            new Problem {Name="Counting substhreengs", Link="https://icpcarchive.ecs.baylor.edu/index.php?option=com_onlinejudge&Itemid=8&category=658&page=show_problem&problem=4835", Difficulty=2},
new Problem {Name="Another Problem on Strings", Link="http://codeforces.com/problemset/problem/165/C", Difficulty=2},
new Problem {Name="ABC-String", Link="https://www.codechef.com/problems/ABCSTR", Difficulty=4},
new Problem {Name="Hyperdrome", Link="http://coj.uci.cu/24h/problem.xhtml?pid=2864", Difficulty=5},
new Problem {Name="Count Good Substrings", Link="http://codeforces.com/contest/451/problem/D", Difficulty=4},
        };

        private Problem[] problemasTeoriaNumeros = new Problem[]
        {
            new Problem {Name="Numeros libres", Link="https://omegaup.com/arena/problem/Numeros-Libres#problems", Difficulty=2},
new Problem {Name="Factovisor", Link="https://uva.onlinejudge.org/index.php?option=onlinejudge&page=show_problem&problem=1080", Difficulty=5},
new Problem {Name="LCM-SUM", Link="http://www.spoj.com/problems/LCMSUM/", Difficulty=8},
new Problem {Name="Count the Factors", Link="http://acm.tju.edu.cn/toj/showp1868.html", Difficulty=1},
new Problem {Name="Dreamoon and Sums", Link="http://codeforces.com/problemset/problem/476/C", Difficulty=5},
new Problem {Name="Vasya and Beatiful Arrays", Link="http://codeforces.com/contest/354/problem/C", Difficulty=7},
new Problem {Name="Reducing Fractions", Link="http://codeforces.com/problemset/problem/222/C", Difficulty=10},
new Problem {Name="Permutacion Hermosa", Link="https://omegaup.com/arena/problem/Permutacion-Hermosa#problems", Difficulty=3},

        };

        private Problem[] problemasGreedy = new Problem[]
        {
new Problem {Name="Pearls in a Row", Link="http://codeforces.com/problemset/problem/620/C", Difficulty=3},
new Problem {Name="Drangos and Princesses", Link="http://acm.sgu.ru/problem.php?contest=0&problem=548", Difficulty=5},
new Problem {Name="Little girl and Maximun Sum", Link="http://codeforces.com/problemset/problem/276/C", Difficulty=4},
new Problem {Name="White and Black Stones", Link="https://icpcarchive.ecs.baylor.edu/index.php?option=com_onlinejudge&Itemid=8&category=658&page=show_problem&problem=4834", Difficulty=6},
new Problem {Name="Fence the vegetables", Link="https://icpcarchive.ecs.baylor.edu/index.php?option=com_onlinejudge&Itemid=8&category=658&page=show_problem&problem=4838", Difficulty=8},
new Problem {Name="Watering Grass", Link="https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&page=show_problem&problem=1323", Difficulty=4},
new Problem {Name="Help Cupid", Link="https://icpcarchive.ecs.baylor.edu/index.php?option=com_onlinejudge&Itemid=8&category=658&page=show_problem&problem=4840", Difficulty=3},
new Problem {Name="Lopov", Link="https://omegaup.com/arena/problem/LOPOV-COCI#problems", Difficulty=5},
        };

        private Problem[] problemasBinaria = new Problem[]
        {
new Problem {Name="Superstitious Socks", Link="https://icpcarchive.ecs.baylor.edu/index.php?option=com_onlinejudge&Itemid=2&category=670&page=show_problem&problem=4801", Difficulty=5},
new Problem {Name="Funky Numbers", Link="http://codeforces.com/problemset/problem/192/A", Difficulty=2},
new Problem {Name="Sweets for Everyone!", Link="http://codeforces.com/problemset/problem/248/D", Difficulty=7},
new Problem {Name="Random Task", Link="http://codeforces.com/problemset/problem/431/D", Difficulty=7},
new Problem {Name="Doctor", Link="http://codeforces.com/problemset/problem/83/B", Difficulty=6},
new Problem {Name="Aerodrom", Link="https://omegaup.com/arena/problem/AERODROM#problems", Difficulty=3},
new Problem {Name="SUMO", Link="https://omegaup.com/arena/problem/SUMO#problems", Difficulty=4},
new Problem {Name="LJUBOMORA", Link="https://omegaup.com/arena/problem/LJUBOMORA#problems", Difficulty=4},
new Problem {Name="Multiplication Table", Link="http://codeforces.com/problemset/problem/448/D", Difficulty=5},
new Problem {Name="A Careful Approach", Link="https://icpcarchive.ecs.baylor.edu/index.php?option=com_onlinejudge&Itemid=8&page=show_problem&problem=2446", Difficulty=5},
new Problem {Name="Planetas", Link="https://omegaup.com/arena/problem/Planetas#problems", Difficulty=3},
new Problem {Name="Reconnaissance", Link="http://coj.uci.cu/contest/cproblem.xhtml?pid=2890&cid=1351", Difficulty=6},
new Problem {Name="Mecho", Link="http://www.spoj.com/problems/CTOI09_1/", Difficulty=7},

        };

        #endregion
        private void AddProblems(ClubUser creator)
        {
            foreach (var problem in problemasBarridoDeslizante)
            {
                problem.TopicId = barridoDeslizante.Id;
                problem.ClubUserCreatorId = creator.Id;
                problem.Year = DateTime.Today.Year;
                _context.Add(problem);
            }
            foreach (var problem in problemasBarridoAcumulativo)
            {
                problem.TopicId = barridoAcumulativo.Id;
                problem.ClubUserCreatorId = creator.Id;
                problem.Year = DateTime.Today.Year;
                _context.Add(problem);
            }
            foreach (var problem in problemasTeoriaNumeros)
            {
                problem.TopicId = teoriaNumeros.Id;
                problem.ClubUserCreatorId = creator.Id;
                problem.Year = DateTime.Today.Year;
                _context.Add(problem);
            }
            foreach (var problem in problemasGreedy)
            {
                problem.TopicId = greedy.Id;
                problem.ClubUserCreatorId = creator.Id;
                problem.Year = DateTime.Today.Year;
                _context.Add(problem);
            }
            foreach (var problem in problemasBinaria)
            {
                problem.TopicId = busquedaBinaria.Id;
                problem.ClubUserCreatorId = creator.Id;
                problem.Year = DateTime.Today.Year;
                _context.Add(problem);
            }
            _context.SaveChanges();
        }

        private void AddUsersEventsAttendance()
        {
            var activeUsers = _context.Users.Where(usr => usr.Approved).ToList();
            var registeredEvents = _context.Events.ToList();
            foreach (var user in activeUsers)
            {
                var firstRandom = Random.Next(0, 11);
                for (int i = 0; i < firstRandom; i++)
                {
                    var randomEventIx = Random.Next(0, registeredEvents.Count);
                    if (
                        !_context.EventAttendance.Any(
                            ea => ea.ClubUserId == user.Id && ea.EventId == registeredEvents[randomEventIx].Id))
                    {
                        _context.Add(new EventAttendance() { ClubUserId = user.Id, EventId = registeredEvents[randomEventIx].Id, AttendedOn = registeredEvents[randomEventIx].Start.AddMinutes(randomEventIx) });
                        _context.SaveChanges();
                    }
                }

            }

        }

        private void AddEvents(ClubUser creator)
        {

            foreach (var @event in SampleData.SampleEvents)
            {
                int r = (Random.Next(1, 65465465) % 270) + 45;
                @event.ClubUserHostId = creator.Id;
                @event.CreatedOn = DateTime.Now;
                @event.Type = (EventType)(r % 4);
                @event.EventCode = _eventCodeGenerator.GetCode();
                @event.End = @event.Start.AddMinutes(r);
                _context.Add(@event);
            }
            _context.SaveChanges();
        }

        private void AddAnnouncements(ClubUser creator)
        {
            foreach (var announcement in SampleData.SampleAnnouncements)
            {
                int r = Random.Next(1, 65465465) % 365;
                announcement.ClubUserCreatorId = creator.Id;
                announcement.Type = r % 2 == 0
                    ? Club.Models.Enums.AnnouncementType.Info
                    : announcement.IsPrivate ? Club.Models.Enums.AnnouncementType.Warning : Club.Models.Enums.AnnouncementType.Danger;
                announcement.CreatedOn = DateTime.Now.AddDays(r);
                announcement.DueDate = announcement.DueDate.AddDays(r);
                _context.Add(announcement);
            }
            _context.SaveChanges();
        }

        private async Task<ClubUser> GetOrCreateAdmin(IdentityRole adminRole)
        {

            var admin = await _userManager.FindByEmailAsync("antonio.feregrino@gmail.com");
            if (admin == null)
            {
                admin = new ClubUser
                {
                    UserName = "fferegrino",
                    FirstName = "Antonio",
                    LastName = "Feregrino",
                    Approved = true,
                    UserLevel = advancedUserLevel,
                    EmailConfirmed = true,
                    Email = "antonio.feregrino@gmail.com"
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
                sampleUser.EmailConfirmed = sampleUser.Approved;
                sampleUser.UserLevel = basicIntermediateUserLevel;
                await _userManager.CreateAsync(sampleUser, defaultPassword);
                await _userManager.AddToRoleAsync(sampleUser, memberRole.Name);
            }
        }
    }
}
