using System;
using System.Threading.Tasks;
using Club.Common.TypeMapping;
using Club.Models;
using Club.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Web
{
    public class AccountController : Controller
    {

        private readonly SignInManager<ClubUser> _signInManager;
        private readonly IAutoMapper _mapper;
        private readonly UserManager<ClubUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<ClubUser> signInManager,
            IAutoMapper mapper,
            UserManager<ClubUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
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
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if (signInResult.Succeeded)
                {
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

            return View(viewModel);
        }
    }
}
