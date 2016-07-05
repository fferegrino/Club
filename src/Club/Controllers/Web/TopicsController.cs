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
    public class TopicsController : Controller
    {
        private readonly IProblemsRepository _problemsRepository;
        private readonly ITopicsRepository _topicsRepo;
        private readonly IUserLevelsRepository _usersLevelRepo;
        private readonly IAutoMapper _mapper;
        private IWebUserSession _userSession;

        public TopicsController(IProblemsRepository problemsRepository,
            IAutoMapper mapper, IWebUserSession userSession, 
            ITopicsRepository topicsRepo, 
            IUserLevelsRepository usersLevelRepo)
        {
            _problemsRepository = problemsRepository;
            _mapper = mapper;
            _userSession = userSession;
            _topicsRepo = topicsRepo;
            _usersLevelRepo = usersLevelRepo;
        }


        //public IActionResult Details(int id)
        //{
        //    var queriedProblem = _problemsRepository.GetProblemById(id);
        //    var eventViewModel = _mapper.Map<ProblemViewModel>(queriedProblem);
        //    return View(eventViewModel);
        //}

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.SelectUserLevels = GetAllUserLevelsSelectList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(TopicViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var m = _mapper.Map<Models.Entities.Topic>(viewModel);
                _topicsRepo.AddTopic(m);
                _topicsRepo.SaveAll();
                return RedirectToAction("index");
            }
            ViewBag.SelectUserLevels= GetAllUserLevelsSelectList(viewModel.UserLevelId);
            return View(viewModel);
        }

        public ActionResult Index()
        {
            var vms = _mapper.Map<List<TopicViewModel>>(_topicsRepo.GetAllTopics());
            return View(vms);
        }

        public IEnumerable<SelectListItem> GetAllUserLevelsSelectList(int selectedTopicId = 0)
        {
            var selectTopics = _usersLevelRepo.GetAllUsersLevels().Select(t => new SelectListItem
            {
                Text = $"{t.Id} — {t.Level}",
                Value = t.Id.ToString(),
                Selected = t.Id == selectedTopicId
            });
            return selectTopics;
        }

    }
}
