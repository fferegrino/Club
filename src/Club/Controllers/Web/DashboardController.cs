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
        private readonly ITermsRepository _termsRepository;
        private readonly IAutoMapper _mapper;

        public DashboardController(IClubUsersRepository clubUsersRepository,
            IAutoMapper mapper, IEventsRepository eventsRepository, ITermsRepository termsRepository)
        {
            _clubUsersRepository = clubUsersRepository;
            _mapper = mapper;
            _eventsRepository = eventsRepository;
            _termsRepository = termsRepository;
        }

        public IActionResult Index()
        {
            var userDashboard = new DashboardViewModel();

            ViewBag.Term = _mapper.Map<ViewModels.TermViewModel>( _termsRepository.GetCurrentTerm());

            var n = _clubUsersRepository.GetUnapprovedUsers(5);
            userDashboard.UsersAwaitingApproval = _mapper.Map<IEnumerable<ViewModels.SimpleUserViewModel>>(n);
            userDashboard.UsersAwaitingApprovalCount = _clubUsersRepository.CountUnapprovedUsers();
            userDashboard.NextEvent = _mapper.Map<ViewModels.EventViewModel>(_eventsRepository.GetNextEvent());
            var mostActiveUsers = _clubUsersRepository.GetMostActiveUsers();
            userDashboard.MostActiveUsers = _mapper.Map<IEnumerable<ViewModels.ComplexUserViewModel>>(mostActiveUsers);

            return View(userDashboard);
        }
    }
}
