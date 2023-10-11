using System.ComponentModel.DataAnnotations;

namespace SystemeLocation.Models.Utilisateur
{
    public class UtilisateurManageVM
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }
    }
}