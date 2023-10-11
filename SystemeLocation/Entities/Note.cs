using System.ComponentModel.DataAnnotations.Schema;

namespace SystemeLocation.Entities
{
    public class Note
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }

        public Guid? VoitureId { get; set; }

        public Guid? LocationId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(VoitureId))]
        public virtual Voiture? Voiture { get; set; }

        [ForeignKey(nameof(LocationId))]
        public virtual Location? Location { get; set; }
    }
}
