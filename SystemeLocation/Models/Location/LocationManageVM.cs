using System.ComponentModel.DataAnnotations;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Location
{
    public class LocationManageVM
    {
        [Display(Name = "Id")]
        public Guid LocationId { get; set; }

        [Display(Name = "Status")]
        public StatutLocation Status { get; set; }

        [Display(Name = "Opening Time")]
        public DateTime? OpeningTime { get; set; }

        [Display(Name = "Planned Closing Time")]
        public DateTime? PlannedClosingTime { get; set; }

        [Display(Name = "Closing Time")]
        public DateTime? ClosingTime { get; set; }

        [Display(Name = "Car")]
        public string? VoitureName { get; set; }

        [Display(Name = "Driver's Name")]
        public string? Driver_Name { get; set; }

        [Display(Name = "Time since start")]
        public int? ElapsedTimeStart => (DateTime.Today - OpeningTime!.Value).Days;

        [Display(Name = "Time since over")]
        public int? ElapsedTimeEnd => (DateTime.Today - ClosingTime!.Value).Days;
    }
}
