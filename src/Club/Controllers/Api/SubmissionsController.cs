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
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionsRepository _repository;
        private readonly IPagedDataRequestFactory _requestFactory;
        private readonly IAutoMapper _mapper;

        public SubmissionsController(ISubmissionsRepository repo, 
            IPagedDataRequestFactory requestFactory, IAutoMapper mapper)
        {
            _repository = repo;
            _requestFactory = requestFactory;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{*username}")]
        public JsonResult GetSubmissionsForUser(string username)
        {
            //var request = _requestFactory.Create(Request.ToUri());
            var pagedQuery = _repository.GetAllSubmissionsForUser(username);
            var resp = _mapper.Map<List<ApiModels.SubmissionApiModel>>(pagedQuery);
            System.Diagnostics.Debug.WriteLine(resp.Count);
            return Json(resp);
        }
        
        
    }
}

