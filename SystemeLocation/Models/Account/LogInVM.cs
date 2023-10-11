using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SystemeLocation.Models.Account
{
    public class LogInVM
    {
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; } = false;
    }
}
