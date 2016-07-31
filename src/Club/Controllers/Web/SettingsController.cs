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
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Http;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNet.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Club.Controllers.Web
{
    public class SettingsController : Controller
    {
        const string DateTimeFormat = "yyyy-MM-dd HH:mm";

        //IApplicationEnvironment _app;
        string _assetsFolder;
        private readonly ITermsRepository _termsRepository;
        private readonly IAnnouncementsRepository _announcementsRepo;
        private readonly IClubUsersRepository _usersRepo;
        private readonly IProblemsRepository _problemsRepo;
        private readonly IEventsRepository _eventsRepo;

        public SettingsController(IApplicationEnvironment appEnv,
            IHostingEnvironment hostEnv,
            ITermsRepository termsRepository,
            IAnnouncementsRepository announcementsRepo,
            IClubUsersRepository usersRepo,
            IProblemsRepository problemsRepo)
        {
            _assetsFolder = hostEnv.MapPath("assets");

            _problemsRepo = problemsRepo;
            _usersRepo = usersRepo;
            _announcementsRepo = announcementsRepo;
            _termsRepository = termsRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Carta()
        {
            var bytes = System.IO.File.ReadAllBytes(_assetsFolder + "\\carta.docx");
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "carta.docx");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Carta(ICollection<IFormFile> carta)
        {
            if (carta.Any())
            {
                carta.First().SaveAs(_assetsFolder + "\\carta.docx");
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Header()
        {
            var bytes = System.IO.File.ReadAllBytes(_assetsFolder + "\\headerbg.png");
            return File(bytes, "image/png", "headerbg.png");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Header(ICollection<IFormFile> header)
        {
            if (header.Any())
            {
                header.First().SaveAs(_assetsFolder + "\\headerbg.png");
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(Startup.Settings);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Export()
        {
            if(TempData["Delete"] != null)
            {
                ViewBag.Delete = (bool)TempData["Delete"];
            }
            else
            {
                ViewBag.Delete = false;
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ExportTermsInfo(int termId)
        {
            var tempFile = System.IO.Path.GetTempFileName();
            tempFile = System.IO.Path.ChangeExtension(tempFile, ".xlsx");
            FileInfo fi = new FileInfo(tempFile);
            //using (var fs = new FileStream(tempFile, FileMode.OpenOrCreate))
            using (var excel = new ExcelPackage())
            {
                var workbook = excel.Workbook;

#region Periodos
                var termsSheet = workbook.Worksheets.Add("Periodos");

                var terms = _termsRepository.GetAllTerms();
                termsSheet.Cells["A1"].Value = "ID";
                termsSheet.Cells["B1"].Value = "Nombre";
                termsSheet.Cells["C1"].Value = "Inicio";
                termsSheet.Column(3).Style.Numberformat.Format = DateTimeFormat;
                termsSheet.Cells["D1"].Value = "Fin";
                termsSheet.Column(4).Style.Numberformat.Format = DateTimeFormat;
                for (int i = 0; i < terms.Count; i++)
                {
                    var term = terms[i];
                    termsSheet.Cells[$"A{i + 2}"].Value = term.Id;
                    termsSheet.Cells[$"B{i + 2}"].Value = term.Name;
                    termsSheet.Cells[$"C{i + 2}"].Value = term.Start;
                    termsSheet.Cells[$"D{i + 2}"].Value = term.End;
                }
#endregion

#region Sesiones
                var sessionsSheet = workbook.Worksheets.Add("Sesiones");

                var sessions = terms.SelectMany(t => t.Events).ToList();

                sessionsSheet.Cells["A1"].Value = "ID periodo";
                sessionsSheet.Cells["B1"].Value = "ID";
                sessionsSheet.Cells["C1"].Value = "Nombre";
                sessionsSheet.Cells["D1"].Value = "Tipo";
                sessionsSheet.Cells["E1"].Value = "Inicio";
                sessionsSheet.Column(5).Style.Numberformat.Format = DateTimeFormat;
                sessionsSheet.Cells["F1"].Value = "Fin";
                sessionsSheet.Column(6).Style.Numberformat.Format = DateTimeFormat;
                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    sessionsSheet.Cells[$"A{i + 2}"].Value = session.TermId;
                    sessionsSheet.Cells[$"B{i + 2}"].Value = session.Id;
                    sessionsSheet.Cells[$"C{i + 2}"].Value = session.Name;
                    sessionsSheet.Cells[$"D{i + 2}"].Value = session.Type;
                    sessionsSheet.Cells[$"E{i + 2}"].Value = session.Start;
                    sessionsSheet.Cells[$"F{i + 2}"].Value = session.End;
                }
#endregion

#region Avisos
                var announcementsSheet = workbook.Worksheets.Add("Anuncios");

                var announcementsTuples = new List<Tuple<int, IEnumerable<Announcement>>>();
                foreach (var t in terms)
                {
                    var r = Tuple.Create(t.Id, _announcementsRepo.GetAnnouncementsByDate(t.Start, t.End));
                    announcementsTuples.Add(r);
                }

                announcementsSheet.Cells["A1"].Value = "ID periodo";
                announcementsSheet.Cells["B1"].Value = "Texto";
                announcementsSheet.Cells["C1"].Value = "Fecha";
                announcementsSheet.Column(3).Style.Numberformat.Format = DateTimeFormat;

                var row = 2;
                for (int i = 0; i < announcementsTuples.Count; i++)
                {
                    var tuple = announcementsTuples[i];
                    var annoucements = tuple.Item2;
                    foreach (var announcement in annoucements)
                    {
                        announcementsSheet.Cells[$"A{row}"].Value = tuple.Item1;
                        announcementsSheet.Cells[$"B{row}"].Value = announcement.Text;
                        announcementsSheet.Cells[$"C{row}"].Value = announcement.CreatedOn;
                        row++;
                    }
                }
#endregion

#region Problemas
                var problemsSheet = workbook.Worksheets.Add("Problemas");

                var problems = _problemsRepo.GetAllCurrentProblems();

                problemsSheet.Cells["A1"].Value = "ID problema";
                problemsSheet.Cells["B1"].Value = "Nombre";
                problemsSheet.Cells["C1"].Value = "Link";
                problemsSheet.Cells["D1"].Value = "Tema";
                problemsSheet.Cells["E1"].Value = "Dificultad";

                row = 2;
                foreach (var problem in problems)
                {
                    problemsSheet.Cells[$"A{row}"].Value = problem.Id;
                    problemsSheet.Cells[$"B{row}"].Value = problem.Name;
                    problemsSheet.Cells[$"C{row}"].Value = problem.Link;
                    problemsSheet.Cells[$"D{row}"].Value = problem.Topic.Name;
                    problemsSheet.Cells[$"E{row}"].Value = problem.Difficulty;
                    row++;
                }

#endregion

                excel.SaveAs(fi);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFile);

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Terms.xlsx");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ExportUsersInfo()
        {
            var tempFile = System.IO.Path.GetTempFileName();
            tempFile = System.IO.Path.ChangeExtension(tempFile, ".xlsx");
            FileInfo fi = new FileInfo(tempFile);
            //using (var fs = new FileStream(tempFile, FileMode.OpenOrCreate))
            using (var excel = new ExcelPackage())
            {
                var workbook = excel.Workbook;

#region Usuarios
                var usersSheet = workbook.Worksheets.Add("Usuarios");

                var users = _usersRepo.GetAllActiveUsers();
                usersSheet.Cells["A1"].Value = "ID";
                usersSheet.Cells["B1"].Value = "Username";
                usersSheet.Cells["C1"].Value = "Nombre";
                usersSheet.Cells["D1"].Value = "Email";
                usersSheet.Cells["E1"].Value = "Notas";
                for (int i = 0; i < users.Count; i++)
                {
                    var term = users[i];
                    usersSheet.Cells[$"A{i + 2}"].Value = term.Id;
                    usersSheet.Cells[$"B{i + 2}"].Value = term.UserName;
                    usersSheet.Cells[$"C{i + 2}"].Value = term.FirstName + " " + term.LastName;
                    usersSheet.Cells[$"D{i + 2}"].Value = term.Email;
                    usersSheet.Cells[$"E{i + 2}"].Value = term.Notes;
                }
#endregion

#region Problemas resueltos
                var problemsSolvedSheet = workbook.Worksheets.Add("Envíos");
                var submissions = users.SelectMany(u => u.Submissions).ToList();
                problemsSolvedSheet.Cells["A1"].Value = "ID problema";
                problemsSolvedSheet.Cells["B1"].Value = "ID usuario";
                problemsSolvedSheet.Cells["C1"].Value = "Estatus";
                problemsSolvedSheet.Cells["D1"].Value = "Intentos";
                problemsSolvedSheet.Cells["E1"].Value = "Fecha intento";
                problemsSolvedSheet.Column(5).Style.Numberformat.Format = DateTimeFormat;
                for (int i = 0; i < submissions.Count; i++)
                {
                    var submission = submissions[i];
                    problemsSolvedSheet.Cells[$"A{i + 2}"].Value = submission.ProblemId;
                    problemsSolvedSheet.Cells[$"B{i + 2}"].Value = submission.UserId;
                    problemsSolvedSheet.Cells[$"C{i + 2}"].Value = submission.Accepted;
                    problemsSolvedSheet.Cells[$"D{i + 2}"].Value = submission.Attempts;
                    problemsSolvedSheet.Cells[$"E{i + 2}"].Value = submission.LastAttemptDate;
                }
#endregion

#region Eventos asistidos
                var eventsAttendendSheet = workbook.Worksheets.Add("Asistencias");
                var attendance = users.SelectMany(u => u.EventsAttended).ToList();
                eventsAttendendSheet.Cells["A1"].Value = "ID evento";
                eventsAttendendSheet.Cells["B1"].Value = "ID usuario";
                for (int i = 0; i < attendance.Count; i++)
                {
                    var att = attendance[i];
                    eventsAttendendSheet.Cells[$"A{i + 2}"].Value = att.EventId;
                    eventsAttendendSheet.Cells[$"B{i + 2}"].Value = att.ClubUserId;
                }
#endregion

                excel.SaveAs(fi);
            }
            var bytes = System.IO.File.ReadAllBytes(tempFile);

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Usuarios.xlsx");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Index(SettingsViewModel settings)
        {
            Startup.Settings.HtmlFooter = settings.HtmlFooter;
            Startup.Settings.Theme = settings.Theme;
            return View(Startup.Settings);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAll(bool trueDelete)
        {
            if (trueDelete)
            {
                await _usersRepo.DropTheBomb();
                return RedirectToAction("Logout", "Account");
            }
            else
            {
                TempData["Delete"]= true;
                return RedirectToAction("Export");
            }
        }
    }
}
