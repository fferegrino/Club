using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Club.Common.Extensions;
using Club.Common.TypeMapping;
using Club.Models;
using Club.Models.Entities;
using Club.Models.Repositories;
using Club.Services;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Logging;
using Microsoft.Owin.Security.DataProtection;

namespace Club.Controllers.Web
{
    public class AccountController : Controller
    {
        private const string UsernameKey = "Username";

        private readonly IAutoMapper _mapper;
        private readonly SignInManager<ClubUser> _signInManager;
        private readonly UserManager<ClubUser> _userManager;
        private readonly IClubUsersRepository _clubUsersRepository;
        private readonly IMailService _mailService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<ClubUser> signInManager,
            IAutoMapper mapper,
            UserManager<ClubUser> userManager,
            RoleManager<IdentityRole> roleManager, IClubUsersRepository clubUsersRepository, IMailService mailService)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _clubUsersRepository = clubUsersRepository;
            _mailService = mailService;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if (!ModelState.IsValid) return View();
            
            var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (signInResult.Succeeded)
            {
                var loggedInUser = _clubUsersRepository.GetUserByUserName(vm.Username);

                if (!loggedInUser.Approved)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("pendingapproval", new { username = loggedInUser.UserName });
                }
                var claims = new System.Security.Claims.ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.UserData, loggedInUser.UserLevelId.ToString()));
                User.AddIdentity(new ClaimsIdentity(claims));
                if (!String.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);

                return RedirectToAction("index", User.IsInRole("admin") ? "dashboard" : "home");

            }
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            var userModel = _mapper.Map<Models.Entities.ClubUser>(viewModel);
            userModel.UserLevelId = 1;
            var result = await _userManager.CreateAsync(userModel, viewModel.Password);
            if (result.Succeeded)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(userModel);
                var callbackUrl = Url.Action("ConfirmEmail", "Account",
                        new { userId = userModel.Id, code = code },
                                protocol: Request.ToUri().Scheme);


                await
                    _mailService.SendMail(userModel.Email, "mail@hola.com", "Confirmación de email",
                        "Confirma tu correo " + "<a href =\""
                        + callbackUrl + "\">link</a>");
                return RedirectToAction("pendingapproval", new { username = userModel.UserName });
            }
            ModelState.AddModelError("", result.ToString());
            return View(viewModel);
        }

        public IActionResult PendingApproval(string username)
        {
            var model = _clubUsersRepository.GetUserByUserName(username);
            var viewModel = _mapper.Map<ViewModels.SimpleUserViewModel>(model);
            ViewBag.Approved = model.Approved;
            ViewBag.EmailConfirmed = model.EmailConfirmed;
            return View(viewModel);
        }

        public IActionResult Forgotten()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Forgotten(ForgottenViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgottenConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                new { UserId = user.Id, code = code }, protocol: Request.ToUri().Scheme);
                await _mailService.SendMail(user.Email, "mail@hola.com", "Reset Password",
                "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return View("ForgottenConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> ForgottenConfirmation()
        {
            return View();
        }


        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            return View(new ResetPasswordViewModel() { UserId = userId, Code = code });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null) return View(vm);

            var result = await _userManager.ResetPasswordAsync(user, vm.Code, vm.Password);


            if (result.Succeeded)
                return RedirectToAction("Login");
            else
                return View(vm);
        }

        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = _clubUsersRepository.GetUserById(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            return View();
        }
    }
}
