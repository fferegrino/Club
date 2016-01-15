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
        private readonly IEventsRepository _eventsRepository;
        private readonly IClubUsersRepository _clubUsersRepository;
        private readonly IAutoMapper _mapper;

        public DashboardController(IClubUsersRepository clubUsersRepository,
            IAutoMapper mapper, IEventsRepository eventsRepository)
        {
            _clubUsersRepository = clubUsersRepository;
            _mapper = mapper;
            _eventsRepository = eventsRepository;
        }

        public IActionResult Index()
        {
            var userDashboard = new DashboardViewModel();

            var n = _clubUsersRepository.GetUnapprovedUsers(5);
            userDashboard.UsersAwaitingApproval = _mapper.Map<IEnumerable<ViewModels.SimpleUserViewModel>>(n);
            userDashboard.UsersAwaitingApprovalCount = _clubUsersRepository.CountUnaccepted();

            userDashboard.NextEvent = _mapper.Map<ViewModels.EventViewModel>(_eventsRepository.GetNextEvent());

            return View(userDashboard);
        }
    }
}
