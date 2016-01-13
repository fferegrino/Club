using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Web
{
    public class BoardController : Controller
    {
        private readonly IClubUsersRepository _clubUsersRepository;
        private readonly IAutoMapper _mapper;

        public BoardController(IClubUsersRepository clubUsersRepository,
            IAutoMapper mapper)
        {
            _clubUsersRepository = clubUsersRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var n = _clubUsersRepository.GetAllUnAcceptedUsers();
            var notAcceptedUsers = _mapper.Map<IEnumerable<ViewModels.SimpleUserViewModel>>(n);
            return View(notAcceptedUsers);
        }
    }
}
