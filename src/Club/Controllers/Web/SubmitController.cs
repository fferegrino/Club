using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Club.ViewModels;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class SubmitController : Controller
    {
        private readonly IClubUsersRepository _usersRepository;
        private readonly ISubmissionsRepository _submissionsRepo;
        private readonly IProblemsRepository _problemsRepo;
        private readonly IAutoMapper _mapper;

        public SubmitController(IClubUsersRepository usersRepository,
            ISubmissionsRepository submissionsRepo,
            IProblemsRepository problemsRepo,
            IAutoMapper mapper)
        {
            _usersRepository = usersRepository;
            _problemsRepo = problemsRepo;
            _submissionsRepo = submissionsRepo;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Review(int problemId, string user, bool ok)
        {
            var usr = _usersRepository.GetUserByUserName(user);
            _submissionsRepo.ReviewSubmission(problemId, usr.Id, ok);
            _submissionsRepo.SaveAll();
            return RedirectToAction("details", new { problemId, user });
        }

        [Authorize]
        public IActionResult Details(int problemId, string user)
        {
            var usr = _usersRepository.GetUserByUserName(user);
            var problem = _mapper.Map<SubmissionViewModel>(_submissionsRepo.GetSubmissionForProblem(problemId, user == null ? null : usr.Id));
            if (problem == null)
            {
                return RedirectToAction("create", new { problemId });
            }
            ViewBag.Problem = _mapper.Map<ProblemViewModel>(_problemsRepo.GetProblemById(problemId));
            return View(problem);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var rrr = _submissionsRepo.GetAllRecentSubmissions();
            var problems = _mapper.Map<List<SubmissionViewModel>>(rrr);
            return View(problems);
        }


        [Authorize]
        public IActionResult Create(int problemId)
        {
            var problem = _mapper.Map<SubmissionViewModel>(_submissionsRepo.GetSubmissionForProblem(problemId));
            ViewBag.Problem = _mapper.Map<ProblemViewModel>(_problemsRepo.GetProblemById(problemId));
            return View(problem);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Create(int problemId, SubmissionViewModel viewModel)
        {
            var problem = _mapper.Map<SubmissionViewModel>(_submissionsRepo.GetSubmissionForProblem(problemId));

            if (ModelState.IsValid)
            {
                var m = _mapper.Map<Models.Entities.Submission>(viewModel);
                _submissionsRepo.AddOrUpdateSubmission(m, problem != null);
                _submissionsRepo.SaveAll();
                return RedirectToAction("details", new { problemId });
            }
            ViewBag.Problem = _mapper.Map<ProblemViewModel>(_problemsRepo.GetProblemById(problemId));
            return View(problem);
        }

    }
}
