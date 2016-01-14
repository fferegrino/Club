using System;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models;
using Club.Models.Repositories;
using Club.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Web
{
    public class AccountController : Controller
    {
        private const string UsernameKey = "Username";

        private readonly IAutoMapper _mapper;
        private readonly SignInManager<ClubUser> _signInManager;
        private readonly UserManager<ClubUser> _userManager;
        private readonly IClubUsersRepository _clubUsersRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<ClubUser> signInManager,
            IAutoMapper mapper,
            UserManager<ClubUser> userManager,
            RoleManager<IdentityRole> roleManager, IClubUsersRepository clubUsersRepository)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _clubUsersRepository = clubUsersRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

                if (signInResult.Succeeded)
                {
                    var loggedInUser = _clubUsersRepository.GetUserByUserName(vm.Username);

                    if (!loggedInUser.Accepted)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("pendingapproval", new { username = loggedInUser.UserName });
                    }

                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("index", "home");
                    }
                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", "Invalid credentials");
            }
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
            var userModel = _mapper.Map<Models.ClubUser>(viewModel);

            var result = await _userManager.CreateAsync(userModel, viewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("pendingapproval", new { username = userModel.UserName });
            }
            ModelState.AddModelError("", result.ToString());
            return View(viewModel);
        }
        
        public IActionResult PendingApproval(string username)
        {
            var model = _clubUsersRepository.GetUserByUserName(username);
            var viewModel = _mapper.Map<ViewModels.SimpleUserViewModel>(model);
            ViewBag.Accepted = model.Accepted;
            return View(viewModel);
        }
    }
}
