using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Succursale
{
    public class SuccursaleCreateVM
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Status")]
        public StatutSuccursale Status { get; set; }

        [Display(Name = "Civic Number")]
        public int? Address_CivicNumber { get; set; }

        [Display(Name = "Street")]
        public string? Address_Street { get; set; }

        [Display(Name = "City")]
        public string? Address_City { get; set; }

        [Display(Name = "Postal Code")]
        public string? Address_PostalCode { get; set; }
    }

    public class Validator : AbstractValidator<SuccursaleCreateVM>
    {
        private const int MIN_NAME_LENGTH = 5;
        private const int MAX_NAME_LENGTH = 20;
        private const int MIN_ADD_LENGTH = 5;
        private const int MAX_ADD_LENGTH = 30;
        private const string NAME_REGEX = "^[a-zA-Z0-9\\s$#-]+$";
        private const string ADD_NAME_REGEX = "^[A-Za-z-]+$";
        private const string POSTAL_CODE_REGEX = "[A-Z]\\d[A-Z][ ]\\d[A-Z]\\d|[A-Z]\\d[A-Z]\\d[A-Z]\\d";

        public Validator()
        {
            RuleFor(vm => vm.Name)
                .NotEmpty()
                .WithMessage("Please provide a name for the new branch.")
                    .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH)
                    .WithMessage($"Please provide a branch name between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters.")
                        .Matches(NAME_REGEX)
                        .WithMessage("Please provide a valid name.");

            RuleFor(vm => vm.Address_CivicNumber)
                .NotEmpty()
                .When(vm => !string.IsNullOrEmpty(vm.Address_City) || !string.IsNullOrEmpty(vm.Address_Street) || !string.IsNullOrEmpty(vm.Address_PostalCode))
                .WithMessage("Please provide a civic number for the new branch.")
                    .Must(x => int.TryParse(x.ToString(), out var i) && i > 0)
                    .When(vm => !string.IsNullOrEmpty(vm.Address_City) || !string.IsNullOrEmpty(vm.Address_Street) || !string.IsNullOrEmpty(vm.Address_PostalCode))
                    .WithMessage("Please provide a positive numeric value for the civic number.");

            RuleFor(vm => vm.Address_Street)
                .NotEmpty()
                .When(vm => !string.IsNullOrEmpty(vm.Address_City) || !string.IsNullOrEmpty(vm.Address_PostalCode) || vm.Address_CivicNumber != null)
                .WithMessage("Please provide a street address for the new branch.")
                    .Length(MIN_ADD_LENGTH, MAX_ADD_LENGTH)
                    .WithMessage($"Please provide a street name between {MIN_ADD_LENGTH} and {MAX_ADD_LENGTH} characters.")
                        .Matches(ADD_NAME_REGEX)
                        .WithMessage("Please provide a valid street name");

            RuleFor(vm => vm.Address_City)
                .NotEmpty()
                .When(vm => !string.IsNullOrEmpty(vm.Address_PostalCode) || !string.IsNullOrEmpty(vm.Address_Street) || vm.Address_CivicNumber != null)
                .WithMessage("Please provide a city name for the new branch.")
                    .Length(MIN_ADD_LENGTH, MAX_ADD_LENGTH)
                    .WithMessage($"Please provide a city name between {MIN_ADD_LENGTH} and {MAX_ADD_LENGTH} characters.")
                        .Matches(ADD_NAME_REGEX)
                        .WithMessage("Please provide a valid city name");

            RuleFor(vm => vm.Address_PostalCode)
                .NotEmpty()
                .When(vm => !string.IsNullOrEmpty(vm.Address_City) || !string.IsNullOrEmpty(vm.Address_Street) || vm.Address_CivicNumber != null)
                .WithMessage("Please provide a postal code for the new branch.")
                    .Matches(POSTAL_CODE_REGEX)
                    .WithMessage($"Please provide a valid postal code. (ex: A1A1A1 or A1A 1A1)");
        }
    }
}