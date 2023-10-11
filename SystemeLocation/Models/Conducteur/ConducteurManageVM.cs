using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SystemeLocation.Models.Conducteur
{
    public class ConducteurManageVM
    {
        [Display(Name = "ID")]
        public Guid? Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Number of Locations")]
        public int? NbLocations { get; set; }

        [Display(Name = "Last Location")]
        public DateTime? LastLocation { get; set; }
    }
}
