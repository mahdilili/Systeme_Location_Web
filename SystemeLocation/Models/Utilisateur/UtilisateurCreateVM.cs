using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using SystemeLocation.Validators;

namespace SystemeLocation.Models.Utilisateur
{
    public class UtilisateurCreateVM
    {
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Role")]
        public Guid RoleId { get; set; }

        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Password Confirmation")]
        public string? PasswordConfirmation { get; set; }

        public class Validator : AbstractValidator<UtilisateurCreateVM>
        {
            private const string USERNAME_REG = "^[a-zA-Z0-9_]+$"; //caractères alphanumériques (accepte le souligné (_) ) 


            public Validator()
            {
                RuleFor(u => u.UserName)
                    .NotEmpty().WithMessage("Username cannot be empty!").Length(5, 20).WithMessage("Username must be between 5 and 20 caracters")
                    .Matches(USERNAME_REG).WithMessage("Please provide a valid username");

                RuleFor(u => u.Password)
                    .NotEmpty().WithMessage("Password cannot be empty").SetValidator(new PasswordValidator());

                RuleFor(u => u.PasswordConfirmation)
                    .NotEmpty().WithMessage("Please confirm the password")
                    .Equal(vm => vm.Password).WithMessage("The password and confirmation password do not match");
            }
        }
    }
}