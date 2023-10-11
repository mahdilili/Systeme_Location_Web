using System.ComponentModel.DataAnnotations.Schema;

namespace SystemeLocation.Entities
{
    public enum StatutLocation { Ouvert, Fermé }

    public class Location
    {
        public Guid Id { get; set; }

        public StatutLocation Status { get; set; } = StatutLocation.Ouvert;

        public DateTime? OpeningTime { get; set; }

        public DateTime? PlannedClosingTime { get; set; }

        public DateTime? ClosingTime { get; set; }

        public Guid? VoitureId { get; set; }

        public Guid? ConducteurId { get; set; }

        public Guid? SuccursaleId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(VoitureId))]
        public virtual Voiture Voiture { get; set; }

        [ForeignKey(nameof(ConducteurId))]
        public virtual Conducteur Conducteur { get; set; }

        [ForeignKey(nameof(SuccursaleId))]
        public virtual Succursale Succursale { get; set; }

        public List<Note>? Notes { get; set; } = new ();
    }
}