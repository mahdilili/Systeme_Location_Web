using FluentValidation;
using System.ComponentModel.DataAnnotations;
using SystemeLocation.Validators;

namespace SystemeLocation.Models.Utilisateur
{
    public class UtilisateurEditPasswordVM
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Password Confirmation")]
        public string? PasswordConfirmation { get; set; }
    }

    public class Validator : AbstractValidator<UtilisateurEditPasswordVM>
    {
        public Validator()
        {
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty")
                    .SetValidator(new PasswordValidator());

            RuleFor(u => u.PasswordConfirmation)
                .NotEmpty()
                .WithMessage("Please confirm the password")
                    .Equal(vm => vm.Password)
                    .WithMessage("The password and confirmation password do not match");
        }
    }
}