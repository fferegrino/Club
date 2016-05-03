using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class ProblemsController : Controller
    {
        private IProblemsRepository _problemsRepository;
        private readonly IAutoMapper _mapper;

        public ProblemsController(IProblemsRepository problemsRepository, IAutoMapper mapper)
        {
            _problemsRepository = problemsRepository;
            _mapper = mapper;
        }


        public IActionResult Details(int id)
        {
            var queriedProblem = _problemsRepository.GetProblemById(id);
            var eventViewModel = _mapper.Map<ViewModels.ProblemViewModel>(queriedProblem);
            return View(eventViewModel);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var listTopics = _mapper.Map<List<TopicViewModel>>(_problemsRepository.GetTopics());
            var selectTopics = listTopics.Select(t => new SelectListItem { Text = $"{t.Name} — {t.UserLevel}", Value = t.Id.ToString() });
            ViewBag.SelectTopics = selectTopics;
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create(ProblemViewModel viewModel)
        {
            //var eventEntity = _mapper.Map<Models.Entities.Event>(viewModel);

            //eventEntity.EventCode = _eventCodeGenerator.GetCode();
            //_eventsRepository.AddEvent(eventEntity);

            //if (viewModel.Repeat && viewModel.RepeatUntil.HasValue)
            //{
            //    var eventDuration = eventEntity.End - eventEntity.Start;
            //    for (var start = eventEntity.Start.AddDays(7); start < viewModel.RepeatUntil; start = start.AddDays(7))
            //    {
            //        var repeatedEvent = _mapper.Map<Models.Entities.Event>(viewModel);
            //        repeatedEvent.Start = start;
            //        repeatedEvent.End = start + eventDuration;
            //        repeatedEvent.EventCode = _eventCodeGenerator.GetCode();
            //        _eventsRepository.AddEvent(repeatedEvent);
            //    }
            //}

            //_eventsRepository.SaveAll();

            //return RedirectToAction("detail", new { id = eventEntity.Id });
            var listTopics = _mapper.Map<List<TopicViewModel>>(_problemsRepository.GetTopics());
            var selectTopics = listTopics.Select(t => new SelectListItem { Text = $"{t.Name} — {t.UserLevel}", Value = t.Id.ToString() });
            ViewBag.SelectTopics = selectTopics;
            return View();
        }
    }
}
