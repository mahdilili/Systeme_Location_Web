using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using SystemeLocation.Context;
using SystemeLocation.Entities;
using SystemeLocation.Models.Utilisateur;
using SystemeLocation.Utilities;

namespace SystemeLocation.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class UtilisateurController : Controller
    {
        private readonly SystemeLocationContext _context;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UtilisateurController(SystemeLocationContext context, UserManager<Utilisateur> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public IActionResult Manage()
        {
            var vm = _userManager.Users.Select(utilisateur => new UtilisateurManageVM
            {
                Id = utilisateur.Id,
                UserName = utilisateur.UserName
            });

            return View(vm);
        }

        public IActionResult Create()
        {
            var passwordGenerated = PasswordGenerator.GeneratePassword();

            int nbUsers = _context.Users.Count();
            nbUsers++;

            return View(new UtilisateurCreateVM()
            {
                UserName = "User" + nbUsers.ToString(),
                Password = passwordGenerated,
                PasswordConfirmation = passwordGenerated
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(UtilisateurCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            bool userAlreadyExists = _context.Users.Any(u => u.UserName == vm.UserName);

            if (userAlreadyExists)
            {
                ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                return View(vm);
            }

            var role = await _roleManager.FindByIdAsync(vm.RoleId.ToString());

            var newUser = new Utilisateur(vm.UserName);
            var result = await _userManager.CreateAsync(newUser, vm.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Unable to add the user.");
                return View(vm);
            }

            result = await _userManager.AddToRoleAsync(newUser, role.Name);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"Unable to add the user to the role {role.Name}.");
                return View(vm);
            }

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult EditPassword(Guid id)
        {
            var passwordGenerated = PasswordGenerator.GeneratePassword();

            UtilisateurEditPasswordVM vm = new UtilisateurEditPasswordVM()
            {
                Id = id,
                Password = passwordGenerated,
                PasswordConfirmation = passwordGenerated
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(UtilisateurEditPasswordVM vm, Guid id)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var toModify = await _userManager.FindByIdAsync(vm.Id.ToString());

            if (toModify is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(toModify);

            var result = await _userManager.ResetPasswordAsync(toModify, resetToken, vm.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Unable to reset the password of the user.");
                return View(vm);
            }

            return RedirectToAction(nameof(Manage));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var toDelete = await _userManager.FindByIdAsync(id.ToString());

            // Prevents the access if you enter the url manually
            if (User.Identity.Name == toDelete.UserName)
                return RedirectToAction(nameof(Manage));

            if (toDelete is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            var result = await _userManager.DeleteAsync(toDelete!);

            if (!result.Succeeded)
                throw new Exception("Unable to remove the user.");

            return RedirectToAction(nameof(Manage));
        }
    }
}