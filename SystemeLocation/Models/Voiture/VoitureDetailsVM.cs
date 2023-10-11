using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Voiture
{
    public class VoitureDetailsVM
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Status")]
        public StatutVoiture Status { get; set; }

        [Display(Name = "Available")]
        public Disponible Available { get; set; }

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

        [Display(Name = "Estimated Price")]
        [DataType(DataType.Currency)]
        public decimal? EstimatePrice { get; set; }

        [Display(Name = "Notes")]
        public List<Note>? Notes { get; set; }

        [Display(Name = "Associated Branch")]
        public string? SuccursaleName { get; set; }

        [Display(Name = "Associated Locations")]
        public List<Entities.Location>? AssociatedLocations { get; set; }
    }
}
