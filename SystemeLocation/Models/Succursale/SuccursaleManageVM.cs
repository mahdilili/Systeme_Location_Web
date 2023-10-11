using System.ComponentModel.DataAnnotations;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Succursale
{
    public class SuccursaleManageVM
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Status")]
        public StatutSuccursale Status { get; set; }

        [Display(Name = "Cars")]
        public int? CarCount { get; set; }

        [Display(Name = "Active Cars")]
        public int? ActiveCars { get; set; }

        [Display(Name = "Available Cars")]
        public int? AvailableCars { get; set; }
    }
}