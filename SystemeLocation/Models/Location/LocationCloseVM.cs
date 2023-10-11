using FluentValidation;
using System.ComponentModel.DataAnnotations;
using SystemeLocation.Models.Voiture;

namespace SystemeLocation.Models.Location
{
    public class LocationCloseVM
    {
        [Display(Name = "ID")]
        public Guid? Id { get; set; }

        [Display(Name = "Closing Time")]
        public DateTime? ClosingTime { get; set; }

        [Display(Name = "Final Note")]
        public string? Note { get; set; }

        public class Validator : AbstractValidator<LocationCloseVM>
        {
            public Validator()
            {
                RuleFor(vm => vm.ClosingTime)
                    .NotEmpty()
                    .WithMessage("Closing time cannot be empty")
                        .LessThanOrEqualTo(DateTime.Today.AddDays(1))
                        .WithMessage("Closing time cannot be after today");
            }
        }
    }
}
