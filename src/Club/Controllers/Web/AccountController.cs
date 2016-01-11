using System;
using System.Threading.Tasks;
using Club.Models;
using Club.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;

namespace Club.Controllers.Web
{
    public class AccountController : Controller
    {

        private readonly SignInManager<ClubUser> _signInManager;

        public AccountController(SignInManager<ClubUser> signInManager)
        {
            _signInManager = signInManager;
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
    }
}
