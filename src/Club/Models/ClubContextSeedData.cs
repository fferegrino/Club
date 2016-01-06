using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models
{
    public class ClubContextSeedData
    {
        private readonly ClubContext _context;

        public ClubContextSeedData(ClubContext context)
        {
            _context = context;
        }

        public void EnsureSeedData()
        {
            if (!_context.Events.Any())
            {
                _context.Add(new Event()
                {
                    Name = "Concurso 1",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddHours(2)
                });
                _context.Add(new Event()
                {
                    Name = "Concurso 2",
                    Start = DateTime.Now.AddDays(3),
                    End = DateTime.Now.AddDays(3).AddHours(2)
                });

                _context.SaveChanges();
            }
        }
    }
}
