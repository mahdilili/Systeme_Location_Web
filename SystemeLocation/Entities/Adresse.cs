using System.ComponentModel.DataAnnotations.Schema;

namespace SystemeLocation.Entities
{
    public class Adresse
    {
        public Guid Id { get; set; }

        public int? CivicNumber { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }
    }
}
