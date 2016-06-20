using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
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
    }
}
