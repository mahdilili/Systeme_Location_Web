using FluentValidation;

namespace SystemeLocation.Models.Voiture
{
    public class VoitureAddNoteVM
    {
        public string? Note { get; set; }

        public class Validator : AbstractValidator<VoitureAddNoteVM>
        {
            public Validator()
            {
                RuleFor(v => v.Note).NotEmpty().WithMessage("Note cannot be empty");
            }
        }
    }
}
