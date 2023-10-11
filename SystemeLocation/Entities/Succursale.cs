using System.ComponentModel.DataAnnotations.Schema;

namespace SystemeLocation.Entities
{
    public enum StatutSuccursale { Activé, Désactivé }

    public class Succursale
    {
        public Guid Id { get; set; }

        public StatutSuccursale Status { get; set; } = StatutSuccursale.Activé;

        public string? Name { get; set; }

        public Guid? AdresseId { get; set; }

        // Navigation properties.
        [ForeignKey(nameof(AdresseId))]
        public virtual Adresse? Adresse { get; set; }

        public List<Voiture>? Voitures { get; set; } = new();

        public List<Location>? Locations { get; set; } = new();
    }
}