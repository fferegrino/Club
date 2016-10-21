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
using Octokit;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Api
{
    [Route("api/[controller]")]
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionsRepository _repository;
        private readonly IPagedDataRequestFactory _requestFactory;
        private readonly IClubUsersRepository _clubUsersRepo;
        private readonly IAutoMapper _mapper;

        public SubmissionsController(ISubmissionsRepository repo,
            IClubUsersRepository clubUsersRepo,
            IPagedDataRequestFactory requestFactory, IAutoMapper mapper)
        {
            _repository = repo;
            _requestFactory = requestFactory;
            _clubUsersRepo = clubUsersRepo;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("{*username}")]
        public JsonResult GetSubmissionsForUser(string username)
        {
            //var request = _requestFactory.Create(Request.ToUri());
            var pagedQuery = _repository.GetAllSubmissionsForUser(username);
            var resp = _mapper.Map<List<ApiModels.SubmissionApiModel>>(pagedQuery);
            return Json(resp);
        }


        [Authorize]
        [HttpGet("{username}/gists")]
        public async Task<JsonResult> GetGists(string username)
        {
            var entity = _clubUsersRepo.GetFullUserByUserName(username);
            if (!String.IsNullOrEmpty(entity.GitHubAccessToken))
            {
                var githubClient = new GitHubClient(new ProductHeaderValue("ElClub"));
                githubClient.Credentials = new Credentials(entity.GitHubAccessToken);
                var gists = await githubClient.Gist.GetAll();
                return Json(gists.Select(gist => new { name = gist.Description, url = gist.HtmlUrl }));
            }
            return Json(null);
        }

    }
}

