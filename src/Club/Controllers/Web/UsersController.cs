using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Entities;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.PlatformAbstractions;
using Novacode;
using Humanizer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class UsersController : Controller
    {
        private readonly IClubUsersRepository _usersRepository;
        private readonly IAutoMapper _mapper;
        private readonly IUserLevelsRepository _usersLevelRepo;
        private readonly IApplicationEnvironment _appEnv;
        private readonly IHostingEnvironment _hostEnv;

        public UsersController(IClubUsersRepository usersRepository,
            IHostingEnvironment hostEnv,
            IAutoMapper mapper, IApplicationEnvironment appEnv,
            IUserLevelsRepository usersLevelRepo)
        {
            _hostEnv = hostEnv;
            _usersRepository = usersRepository;
            _mapper = mapper;
            _appEnv = appEnv;
            _usersLevelRepo = usersLevelRepo;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Unapproved()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Letter(string id)
        {
            string cartaDoc = _hostEnv.MapPath("assets\\carta.docx");
            var temp = System.IO.Path.GetTempFileName();
            temp = System.IO.Path.ChangeExtension(temp, "docx");

            var usr = _usersRepository.GetFullUserByUserName(id);
            var letterVm = _mapper.Map<LetterViewModel>(usr);

            letterVm.Ano = DateTime.Now.Year;
            letterVm.Dias = "dia".ToQuantity(DateTime.Now.Day);
            letterVm.Mes = $"{DateTime.Now:MMMM}";

            using (var template = DocX.Load(cartaDoc))
            {
                template.ReplaceText("$$Ano$$", letterVm.Ano.ToString());
                template.ReplaceText("$$NombreAlumno$$", letterVm.NombreAlumno);
                template.ReplaceText("$$Periodos$$", letterVm.Periodos);
                template.ReplaceText("$$Dias$$", letterVm.Dias);
                template.ReplaceText("$$Horas$$", letterVm.Horas);
                template.ReplaceText("$$Mes$$", letterVm.Mes);

                template.SaveAs(temp);
            }
            var bytes = System.IO.File.ReadAllBytes(temp);
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "doc.docx");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Edit(string username)
        {
            if (User.IsInRole("Admin") || username.Equals(User.Identity.Name))
            {
                var entity = _usersRepository.GetFullUserByUserName(username);
                var vm = _mapper.Map<EditUserViewModel>(entity);
                vm.IsAdmin = await _usersRepository.IsAdmin(username);
                ViewBag.SelectUserLevels = GetAllUserLevelsSelectList(vm.LevelId);

                return View(vm);
            }
            return RedirectToAction("details", new { username });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserViewModel vm)
        {
            if (User.IsInRole("Admin") || vm.Username.Equals(User.Identity.Name))
            {
                //var entity = _usersRepository.GetFullUserByUserName(vm.Username);
                var entity = _mapper.Map<ClubUser>(vm);
                await _usersRepository.ModifyUser(entity, User.IsInRole("Admin"));
                _usersRepository.SaveAll();
                return RedirectToAction("details", new { vm.Username });

            }
            ViewBag.SelectUserLevels = GetAllUserLevelsSelectList(vm.LevelId);
            return View(vm);
        }

        public IActionResult Details(string username)
        {
            var entity = _usersRepository.GetUserByUserName(username);
            var viewModel = _mapper.Map<ViewModels.ComplexUserViewModel>(entity);
            return View(viewModel);
        }

        public IActionResult Top(int? id)
        {
            // id is count

            var entity = _usersRepository.GetMostActiveUsers(id ?? 10);
            var viewModel = _mapper.Map<IEnumerable<ViewModels.ComplexUserViewModel>>(entity);
            return View(viewModel);
        }

        public IEnumerable<SelectListItem> GetAllUserLevelsSelectList(int selectedTopicId = 0)
        {
            var selectTopics = _usersLevelRepo.GetAllUsersLevels().Select(t => new SelectListItem
            {
                Text = $"{t.Id} — {t.Level}",
                Value = t.Id.ToString(),
                Selected = t.Id == selectedTopicId
            });
            return selectTopics;
        }
    }
}
