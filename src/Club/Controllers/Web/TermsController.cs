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
using Club.Common;

namespace Club.Controllers.Web
{
    public class TermsController : Controller
    {
        private readonly ITermsRepository _termsRepository;
        private readonly IAutoMapper _mapper;
        private readonly IDateTime _dateTime;



        public TermsController(ITermsRepository termsRepository,
            IAutoMapper mapper,
            IDateTime dateTime)
        {
            _dateTime = dateTime;
            _termsRepository = termsRepository;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            ViewBag.Now = _dateTime.Now;
            var modelAnnouncements = _termsRepository.GetCurrentAndNextTerms();
            var vmAnnouncements = _mapper.Map<IEnumerable<ViewModels.TermViewModel>>(modelAnnouncements);
            return View(vmAnnouncements);
        }

        public IActionResult Details(int id)
        {
            var tt = _termsRepository.GetTermById(id);
            var t = _mapper.Map<TermViewModel>(tt);
            return View(t);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var tt = _termsRepository.GetTermById(id);
            if (_dateTime.Now < tt.End)
            {
                ViewBag.CanEditStartDate = _dateTime.Now < tt.Start;
                var t = _mapper.Map<TermViewModel>(tt);
                return View(t);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(TermViewModel viewModel)
        {
            var t = _mapper.Map<Models.Entities.Term>(viewModel);
            ViewBag.CanEditStartDate = _dateTime.Now < t.Start;
            if (ModelState.IsValid)
            {
                _termsRepository.UpdateTerm(t);
                _termsRepository.SaveAll();
                return RedirectToAction("index");
            }
            return View(t);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _termsRepository.DeleteTerm(id);
            _termsRepository.SaveAll();
            return RedirectToAction("Index");
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
