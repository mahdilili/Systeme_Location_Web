using FluentValidation;

namespace SystemeLocation.Models.Voiture
{
    public class LocationAddNoteVM
    {
        public string? Note { get; set; }

        public class Validator : AbstractValidator<LocationAddNoteVM>
        {
            public Validator()
            {
                RuleFor(v => v.Note)
                    .NotEmpty()
                    .WithMessage("Note cannot be empty");
            }
        }
    }
}
