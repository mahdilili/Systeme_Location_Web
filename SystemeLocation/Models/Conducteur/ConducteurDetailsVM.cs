using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SystemeLocation.Models.Conducteur
{
    public class ConducteurDetailsVM
    {
        [Display(Name = "ID")]
        public Guid? Id { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Driver's License")]
        public string? DriverLicense { get; set; }

        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Locations")]
        public List<Entities.Location>? Locations { get; set; }
    }
}
