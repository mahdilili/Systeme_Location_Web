using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using SystemeLocation.Entities;

namespace SystemeLocation.Models.Location
{
    public class LocationDetailsVM
    {
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Status")]
        public StatutLocation Status { get; set; }

        [Display(Name = "Opening Time")]
        [DataType(DataType.DateTime)]
        public DateTime? OpeningTime { get; set; }

        [Display(Name = "Planned Closing Time")]
        [DataType(DataType.DateTime)]
        public DateTime? PlannedClosingTime { get; set; }

        [Display(Name = "Closing Time")]
        [DataType(DataType.DateTime)]
        public DateTime? ClosingTime { get; set; }

        [Display(Name = "Car")]
        public string? VoitureName { get; set; }

        [Display(Name = "Driver's Name")]
        public string? Driver_Name { get; set; }

        [Display(Name = "Driver's Phone")]
        public string? Driver_Phone { get; set; }

        [Display(Name = "Driver's Email")]
        public string? Driver_Email { get; set; }

        [Display(Name = "Driver's License")]
        public string? Driver_DriverLicense { get; set; }

        [Display(Name = "Notes")]
        public List<Note>? Notes { get; set; }

        [Display(Name = "Civic Number")]
        public int? Address_CivicNumber { get; set; }

        [Display(Name = "Street")]
        public string? Address_Street { get; set; }

        [Display(Name = "City")]
        public string? Address_City { get; set; }

        [Display(Name = "Postal Code")]
        public string? Address_PostalCode { get; set; }

        [Display(Name = "Time since start")]
        public int? ElapsedTimeStart => (DateTime.Today - OpeningTime!.Value).Days;

        [Display(Name = "Time since over")]
        public int? ElapsedTimeEnd => (DateTime.Today - ClosingTime!.Value).Days;
    }
}
