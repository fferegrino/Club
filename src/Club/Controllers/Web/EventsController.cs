using System.Collections.Generic;
using Club.Common.TypeMapping;
using Club.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IClubRepository _repository;
        private readonly IAutoMapper _mapper;


        public EventsController(IClubRepository repository, IAutoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var events = _repository.GetAllEvents();
            var eventViewModels = _mapper.Map<IEnumerable<ViewModels.EventViewModel>>(events);
            return View(eventViewModels);
        }


        public IActionResult Create()
        {
            return View();
        }
    }
}
