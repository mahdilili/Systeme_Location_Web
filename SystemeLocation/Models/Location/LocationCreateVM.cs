using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Location
{
    public class LocationCreateVM
    {
        [Display(Name = "Id")]
        public Guid LocationId { get; set; }

        [Display(Name = "Status")]
        public StatutLocation Status { get; set; }

        [Display(Name = "Opening Time")]
        [DataType(DataType.DateTime)]
        public DateTime? OpeningTime { get; set; }

        [Display(Name = "Planned Closing Time")]
        [DataType(DataType.DateTime)]
        public DateTime? PlannedClosingTime { get; set; }

        [Display(Name = "Car")]
        public string? VoitureName { get; set; }

        [Display(Name = "Driver's First Name")]
        public string? Driver_FirstName { get; set; }
        [Display(Name = "Driver's Name")]
        public string? Driver_Name { get; set; }

        [Display(Name = "Driver's Phone")]
        public string? Driver_Phone { get; set; }

        [Display(Name = "Driver's Email")]
        public string? Driver_Email { get; set; }

        [Display(Name = "Driver's License")]
        public string? Driver_DriverLicense { get; set; }

        [Display(Name = "Note")]
        public string? Note { get; set; }

        [Display(Name = "Civic Number")]
        public int? Address_CivicNumber { get; set; }

        [Display(Name = "Street")]
        public string? Address_Street { get; set; }

        [Display(Name = "City")]
        public string? Address_City { get; set; }

        [Display(Name = "Postal Code")]
        public string? Address_PostalCode { get; set; }

        [Display(Name = "Identity confirmed?")]
        public bool IsIndentified { get; set; } = false;

        [Display(Name = "The client is an existing driver")]
        public bool IsExistingDriver { get; set; } = false;

        public class Validator : AbstractValidator<LocationCreateVM>
        {
            private const int MIN_NAME_LENGTH = 3;
            private const int MAX_NAME_LENGTH = 20;
            private const int MIN_ADD_LENGTH = 5;
            private const int MAX_ADD_LENGTH = 30;
            private const string NAME_REGEX = "^[A-Za-z-]+$";
            private const string ADD_NAME_REGEX = "^[A-Za-z-]+$";
            private const string POSTAL_CODE_REGEX = "[A-Z]\\d[A-Z][ ]\\d[A-Z]\\d|[A-Z]\\d[A-Z]\\d[A-Z]\\d";
            private const string PHONE_REGEX = "^[(][0-9]{3}[)][ ][0-9]{3}[ ][0-9]{4}|[0-9]{10}|[0-9]{3}[ ][0-9]{3}[ ][0-9]{4}$";
            //private const string LICENSE_REGEX = "^[A-Z]\\d{4}[0-3][0-9][0-1][0-9]\\d{4}$";

            public Validator()
            {
                #region Adress Validation
                RuleFor(vm => vm.Address_CivicNumber)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide a civic number for the rental.")
                        .Must(x => int.TryParse(x.ToString(), out var i) && i > 0)
                        .When(d => !d.IsExistingDriver)
                        .WithMessage("Please provide a positive numeric value for the civic number.");
                RuleFor(vm => vm.Address_Street)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide a street address for the rental.")
                        .Length(MIN_ADD_LENGTH, MAX_ADD_LENGTH)
                        .WithMessage($"Please provide a street name between {MIN_ADD_LENGTH} and " +
                        $"{MAX_ADD_LENGTH} characters.")
                            .Matches(ADD_NAME_REGEX)
                            .WithMessage("Please provide a valid street name");
                RuleFor(vm => vm.Address_City)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide a city name for the rental.")
                        .Length(MIN_ADD_LENGTH, MAX_ADD_LENGTH)
                        .WithMessage($"Please provide a city name between {MIN_ADD_LENGTH} and " +
                        $"{MAX_ADD_LENGTH} characters.")
                            .Matches(ADD_NAME_REGEX)
                            .WithMessage("Please provide a valid city name");
                RuleFor(vm => vm.Address_PostalCode)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide a postal code for the rental.")
                        .Matches(POSTAL_CODE_REGEX)
                        .WithMessage($"Please provide a valid postal code. (ex: A1A1A1 or A1A 1A1)");
                #endregion

                #region Driver validation
                RuleFor(vm => vm.Driver_FirstName)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide the driver's first name.")
                        .Matches(NAME_REGEX)
                        .WithMessage("Please provide a valid first name.")
                            .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH)
                            .WithMessage($"Please provide a first name between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} letters.");
                RuleFor(vm => vm.Driver_Name)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide the driver's last name.")
                        .Matches(NAME_REGEX)
                        .WithMessage("Please provide a valid last name.")
                            .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH)
                            .WithMessage($"Please provide a last name between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} letters.");
                RuleFor(vm => vm.Driver_Phone)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide a phone number for the rental.")
                        .Matches(PHONE_REGEX)
                        .WithMessage("Please provide a valid phone number with the following format : \"123 123 1234\"");
                RuleFor(vm => vm.Driver_Email)
                    .NotEmpty()
                    .When(d => !d.IsExistingDriver)
                    .WithMessage("Please provide an email address.")
                        .EmailAddress()
                        .WithMessage("Please provide a valid email. Exemple: exemple@mail.com");
                //RuleFor(vm => vm.Driver_DriverLicense)
                //    .NotEmpty()
                //    .WithMessage("A driver's license is required to rent a car.")
                //        .Matches(LICENSE_REGEX)
                //        .WithMessage("Please provide a valid license with the following format : <A####DDMMYY##>");
                RuleFor(vm => vm.IsIndentified)
                    .Must(x => x == true)
                    .WithMessage("Please confirm the identity of the driver.");
                #endregion

                #region SelectedTime
                RuleFor(vm => vm.OpeningTime)
                    .NotEmpty()
                    .WithMessage("Please select the rental time.")
                        .LessThan(DateTime.Today.AddYears(1))
                        .WithMessage("You cannot rent a car over 1 year ahead of now.");
                RuleFor(vm => vm.PlannedClosingTime)
                    .NotEmpty()
                    .WithMessage("Please select the time at which you will bring the car back in.")
                        .Must((vm, _) => vm.OpeningTime < vm.PlannedClosingTime)
                        .WithMessage("The planned closing time must be greater than the opening time.")
                            .GreaterThan(DateTime.Today.AddHours(12))
                            .WithMessage("You must rent the car for at least 12 hours.");
                #endregion
            }
        }
    }
}
