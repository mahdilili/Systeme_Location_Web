using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemeLocation.Entities
{
    #region Enum
    public enum StatutVoiture { Désactivé, Activé, Archivé };
    public enum Disponible { Faux, Vrai };
    public enum Etat { Usagé, Neuf };
    #endregion

    public class Voiture
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public StatutVoiture Status { get; set; } = StatutVoiture.Activé;

        public Disponible Available { get; set; } = Disponible.Vrai;

        public Etat State { get; set; } = Etat.Neuf;

        public string? SerialNumber { get; set; }

        public string? RegistrationNumber { get; set; }

        public string? Brand { get; set; }

        public string? Model { get; set; }

        public int? Year { get; set; }

        public string? Color { get; set; }

        public int? Mileage { get; set; }

        public decimal? EstimatePrice { get; set; }

        public Guid? SuccursaleId { get; set; }

        // Navigation Properties
        public List<Location>? Locations { get; set; } = new();

        public List<Note>? Notes { get; set; } = new();

        [ForeignKey(nameof(SuccursaleId))]
        public virtual Succursale? Succursale { get; set; }
    }
}
