using FluentValidation;
using System.Text.RegularExpressions;

namespace SystemeLocation.Validators
{

    public class PasswordValidator : AbstractValidator<string?>
    {
        private const RegexOptions REGEX_OPTIONS = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        private const int LENGTH_MIN = 8;
        private const string REGEX_UPPERCASE = @"[A-Z]*";
        private const string REGEX_LOWERCASE = @"[a-z]*";
        private const string REGEX_DIGIT = @"[0-9]*";
        private const string REGEX_NOT_SPACE = @"[^\s]*";

        public PasswordValidator()
        {
            RuleFor(password => password)
                .NotEmpty()
                    .WithMessage("The Password cannot be empty.")
                .MinimumLength(LENGTH_MIN)
                    .WithMessage($"The Password must have at least {LENGTH_MIN} characters.")
                .Matches(REGEX_UPPERCASE)
                    .WithMessage("The Password must have at least one uppercase letter.")
                .Matches(REGEX_LOWERCASE)
                    .WithMessage("The Password must have at least one lowercase letter.")
                .Matches(REGEX_DIGIT)
                    .WithMessage("The Password must have at least one digit.")
                .Matches(REGEX_NOT_SPACE)
                    .WithMessage("The Password cannot contain spaces.");
        }
    }
}