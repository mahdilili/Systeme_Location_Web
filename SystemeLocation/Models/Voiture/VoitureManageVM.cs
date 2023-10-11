using System.ComponentModel.DataAnnotations;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Voiture
{
    public class VoitureManageVM
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

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

        [Display(Name = "Status")]
        public StatutVoiture Status { get; set; }
    }
}
