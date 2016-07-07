using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Club.Controllers.Web
{
    public class TermsController : Controller
    {
        private readonly ITermsRepository _termsRepository;
        private readonly IAutoMapper _mapper;

        public TermsController(ITermsRepository termsRepository, 
            IAutoMapper mapper)
        {
            _termsRepository = termsRepository;
            _mapper = mapper;
        }

        public IActionResult Details(int id)
        {
            var tt = _termsRepository.GetTermById(id);
            var t = _mapper.Map<TermViewModel>(tt);
            return View(t);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.LastTerm = _mapper.Map<TermViewModel>(_termsRepository.GetLastTerm());
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(TermViewModel viewModel)
        {
            var eventEntity = _mapper.Map<Models.Entities.Term>(viewModel);

            _termsRepository.AddTerm(eventEntity);
            _termsRepository.SaveAll();

            return RedirectToAction("details", new { id = eventEntity.Id });
        }
    }
}
