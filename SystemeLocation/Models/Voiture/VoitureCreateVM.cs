using FluentValidation;
using System.ComponentModel.DataAnnotations;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Voiture
{
    public class VoitureCreateVM
    {
        [Display(Name = "Succursale")]
        public Guid SuccursaleId { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Status")]
        public StatutVoiture Status { get; set; } = StatutVoiture.Activé;

        [Display(Name = "Available")]
        public Disponible Available { get; set; } = Disponible.Vrai;

        [Display(Name = "State")]
        public Etat State { get; set; }

        [Display(Name = "Serial Number")]
        public string? SerialNumber { get; set; }

        [Display(Name = "Registration Number")]
        public string? RegistrationNumber { get; set; }

        [Display(Name = "Brand")]
        public string? Brand { get; set; }

        [Display(Name = "Model")]
        public string? Model { get; set; }

        [Display(Name = "Year")]
        public int? Year { get; set; }

        [Display(Name = "Color")]
        public string? Color { get; set; }

        [Display(Name = "Mileage")]
        public int? Mileage { get; set; }

        [Display(Name = "Note")]
        public string? Note { get; set; }

        [Display(Name = "Estimated Price")]
        [DataType(DataType.Currency)]
        public decimal? EstimatePrice { get; set; }

        public class Validator : AbstractValidator<VoitureCreateVM>
        {
            private const int MIN_NAME_LENGTH = 5;
            private const int MAX_NAME_LENGTH = 20;
            private const int MIN_IMMATRICULATION_LENGTH = 6;
            private const int MAX_IMMATRICULATION_LENGTH = 7;
            private const int MIN_BRAND_MODEL_COLOR_LENGTH = 3;
            private const int MAX_BRAND_MODEL_COLOR_LENGTH = 20;

            private const string NOM_VOITURE_REGEX = "^[a-zA-Z0-9\\s$#-_]+$";
            private const string SERIAL_NUMBER_REGEX = "^[A-Z0-9]+$";
            private const string IMMATRICULATION_NUMBER_REGEX = "^[A-Z0-9]+$";
            private const string BRAND_MODEL_COLOR_REGEX = "^[A-Za-z]+$";
            public Validator()
            {
                RuleFor(v => v.Name)
                    .NotEmpty()
                    .WithMessage("Car name cannot be empty")
                        .Matches(NOM_VOITURE_REGEX)
                        .WithMessage("Please provide a valid name")
                            .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH)
                            .WithMessage($"Please provide a car name between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters.");

                RuleFor(v => v.SerialNumber)
                    .NotEmpty()
                    .WithMessage("Serial number cannot be empty")
                        .Matches(SERIAL_NUMBER_REGEX)
                        .WithMessage("Please provide a serial number that respect the NIV format");

                RuleFor(v => v.RegistrationNumber)
                    .NotEmpty()
                    .WithMessage("Please provide a registration number")
                        .Matches(IMMATRICULATION_NUMBER_REGEX)
                        .WithMessage("Please provide a valid registration number")
                            .Length(MIN_IMMATRICULATION_LENGTH, MAX_IMMATRICULATION_LENGTH)
                            .WithMessage($"Please provide a branch name over {MIN_IMMATRICULATION_LENGTH} and under {MAX_IMMATRICULATION_LENGTH} characters.");

                RuleFor(v => v.Brand)
                    .NotEmpty()
                    .WithMessage("Brand cannot be empty")
                        .Matches(BRAND_MODEL_COLOR_REGEX)
                        .WithMessage("Invalid brand name")
                            .Length(MIN_BRAND_MODEL_COLOR_LENGTH, MAX_BRAND_MODEL_COLOR_LENGTH)
                            .WithMessage($"Please provide a branch name over {MIN_BRAND_MODEL_COLOR_LENGTH} and under {MAX_BRAND_MODEL_COLOR_LENGTH} characters.");

                RuleFor(v => v.Model)
                    .NotEmpty()
                    .WithMessage("Model cannot be empty")
                        .Matches(BRAND_MODEL_COLOR_REGEX)
                        .WithMessage("Please provide a valid Model")
                            .Length(MIN_BRAND_MODEL_COLOR_LENGTH, MAX_BRAND_MODEL_COLOR_LENGTH)
                            .WithMessage($"Please provide a branch name over {MIN_BRAND_MODEL_COLOR_LENGTH} and under {MAX_BRAND_MODEL_COLOR_LENGTH} characters.");

                RuleFor(v => v.Year)
                    .NotEmpty()
                    .WithMessage("Please provide a year")
                        .GreaterThan(2000)
                        .WithMessage("Cannot be before 2000")
                            .LessThanOrEqualTo(DateTime.Now.Year)
                            .WithMessage("Cannot be later than 2023");

                RuleFor(v => v.Color)
                    .NotEmpty()
                    .WithMessage("Color cannot be empty")
                        .Matches(BRAND_MODEL_COLOR_REGEX)
                        .WithMessage("Please provide a valid color")
                            .Length(MIN_BRAND_MODEL_COLOR_LENGTH, MAX_BRAND_MODEL_COLOR_LENGTH)
                            .WithMessage($"Please provide a branch name over {MIN_BRAND_MODEL_COLOR_LENGTH} and under {MAX_BRAND_MODEL_COLOR_LENGTH} characters.");

                RuleFor(v => v.Mileage)
                    .GreaterThanOrEqualTo(0);

                RuleFor(v => v.EstimatePrice)
                    .GreaterThan(0.00m);
            }
        }
    }
}
