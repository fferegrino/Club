using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.PlatformAbstractions;
using Novacode;
using Humanizer;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class UsersController : Controller
    {
        private readonly IClubUsersRepository _usersRepository;
        private readonly IAutoMapper _mapper;
        private readonly IApplicationEnvironment _appEnv;

        public UsersController(IClubUsersRepository usersRepository,
            IAutoMapper mapper, IApplicationEnvironment appEnv)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _appEnv = appEnv;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Unapproved()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Letter()
        {
            string cartaDoc = _appEnv.ApplicationBasePath + "\\assets\\carta.docx";
            var temp = System.IO.Path.GetTempFileName();
            temp = System.IO.Path.ChangeExtension(temp, "docx");

            var usr = _usersRepository.GetFullUserByUserName("epetersonl");
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

        public IActionResult Details(string username)
        {
            var entity = _usersRepository.GetUserByUserName(username);
            var viewModel = _mapper.Map<ViewModels.ComplexUserViewModel>(entity);
            return View(viewModel);
        }
    }
}
