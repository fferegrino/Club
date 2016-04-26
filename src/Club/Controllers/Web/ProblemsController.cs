using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Microsoft.AspNet.Mvc;

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
    }
}
