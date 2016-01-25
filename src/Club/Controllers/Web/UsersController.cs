using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class UsersController : Controller
    {
        private readonly IClubUsersRepository _usersRepository;
        private readonly IAutoMapper _mapper;

        public UsersController(IClubUsersRepository usersRepository, IAutoMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Unapproved()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string username)
        {
            var entity = _usersRepository.GetUserByUserName(username);
            var viewModel = _mapper.Map<ViewModels.ComplexUserViewModel>(entity);
            return View(viewModel);
        }
    }
}
