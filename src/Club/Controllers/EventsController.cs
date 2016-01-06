using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Models;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers
{
    public class EventsController : Controller
    {
        private readonly IClubRepository _repository;

        public EventsController(IClubRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var events = _repository.GetAllEvents();
            return View(events);
        }
    }
}
