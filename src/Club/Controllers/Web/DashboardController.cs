using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Web
{
    public class DashboardController : Controller
    {
        private readonly IClubUsersRepository _clubUsersRepository;
        private readonly IAutoMapper _mapper;

        public DashboardController(IClubUsersRepository clubUsersRepository,
            IAutoMapper mapper)
        {
            _clubUsersRepository = clubUsersRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var userDashboard = new DashboardViewModel();
            var n = _clubUsersRepository.GetAllUnAcceptedUsers();
            userDashboard.UsersAwaitingApproval = _mapper.Map<IEnumerable<ViewModels.SimpleUserViewModel>>(n);
            return View(userDashboard);
        }
    }
}
