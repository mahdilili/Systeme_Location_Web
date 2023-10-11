using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SystemeLocation.Entities;
using SystemeLocation.Models.Account;

namespace SystemeLocation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;

        public AccountController(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInVM vm, [BindNever] string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Log In Failed. Please try again.");
                    return View(vm);
                }

                var user = await _userManager.FindByNameAsync(vm.UserName);

                if (await _userManager.IsInRoleAsync(user, "Utilisateur"))
                    returnUrl = Url.Content("~/Home/Index");
                else
                    returnUrl = Url.Content("~/Succursale/Manage");

                return LocalRedirect(returnUrl);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
