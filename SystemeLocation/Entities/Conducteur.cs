using System.ComponentModel.DataAnnotations.Schema;

namespace SystemeLocation.Entities
{
    public class Conducteur
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? Name { get; set; }

        public string? DriverLicense { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? AdresseId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(AdresseId))]
        public virtual Adresse Adresse { get; set; }

        public List<Location>? Locations { get; set; } = new();
    }
}
