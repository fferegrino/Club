using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.Security;
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
        private readonly IProblemsRepository _problemsRepository;
        private readonly IClubUsersRepository _usersRepo;
        private readonly IAutoMapper _mapper;
        private IWebUserSession _userSession;

        public ProblemsController(IProblemsRepository problemsRepository,
            IClubUsersRepository usersRepo,
            IAutoMapper mapper, IWebUserSession userSession)
        {
            _problemsRepository = problemsRepository;
            _usersRepo = usersRepo;
            _mapper = mapper;
            _userSession = userSession;
        }


        public IActionResult Details(int id)
        {
            var queriedProblem = _problemsRepository.GetProblemById(id);
            var eventViewModel = _mapper.Map<ProblemViewModel>(queriedProblem);
            return View(eventViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.SelectTopics = GetAllTopicsSelectList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProblemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var m = _mapper.Map<Models.Entities.Problem>(viewModel);
                _problemsRepository.AddProblem(m);
                _problemsRepository.SaveAll();
                return RedirectToAction("details", new { id = m.Id });
            }
            ViewBag.SelectTopics = GetAllTopicsSelectList(viewModel.TopicId);
            return View(viewModel);
        }

        public ActionResult Index()
        {
            var userLevelId = 1;
            if (User.Identity.IsAuthenticated)
            {
                var user = _usersRepo.GetUserById(User.Identity.Name);

            }
            var problemsRepo = _mapper.Map<List< ProblemViewModel>>( _problemsRepository.GetAllCurrentProblems());
            var split = problemsRepo.GroupBy(t => t.LevelId).OrderBy(gr => gr.Key);
            ViewBag.UserLevelId = userLevelId;
            return View(split);
        }

        public IEnumerable<SelectListItem> GetAllTopicsSelectList(int selectedTopicId = 0)
        {

            var listTopics = _mapper.Map<List<TopicViewModel>>(_problemsRepository.GetTopics());
            var selectTopics = listTopics.Select(t => new SelectListItem
            {
                Text = $"{t.Name} — {t.UserLevel}",
                Value = t.Id.ToString(),
                Selected = t.Id == selectedTopicId
            });
            return selectTopics;
        }

    }
}
