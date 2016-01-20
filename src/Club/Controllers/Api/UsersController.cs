using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.ApiModels;
using Club.Common.Extensions;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Api
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IClubUsersRepository _clubUsersRepository;
        private readonly IPagedDataRequestFactory _requestFactory;
        private readonly IAutoMapper _mapper;

        public UsersController(IClubUsersRepository clubUsersRepository, 
            IPagedDataRequestFactory requestFactory, IAutoMapper mapper)
        {
            _clubUsersRepository = clubUsersRepository;
            _requestFactory = requestFactory;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public JsonResult GetUsers()
        {
            var request = _requestFactory.Create(Request.ToUri());
            var pagedQuery = _clubUsersRepository.GetPagedUsersWithAttendance(request);
            var resp = _mapper.Map<PagedDataResponse<ApiModels.ComplexUserApiModel>>(pagedQuery);
            return Json(resp);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("unapproved")]
        public JsonResult GetUnapprovedUsers()
        {
            var request = _requestFactory.Create(Request.ToUri());
            var pagedQuery = _clubUsersRepository.GetPagedUnapprovedUsers(request);
            var resp = _mapper.Map<PagedDataResponse<ViewModels.SimpleUserViewModel>>(pagedQuery);
            return Json(resp);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve")]
        public void Put([FromBody]ApprovedUserApiModel user)
        {
            _clubUsersRepository.UpdateApprovedStatus(user.Id, user.Approved);
            _clubUsersRepository.SaveAll();
        }
        
    }
}

