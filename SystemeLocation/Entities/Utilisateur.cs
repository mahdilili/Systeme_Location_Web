using Microsoft.AspNetCore.Identity;

namespace SystemeLocation.Entities
{
    public class Utilisateur : IdentityUser<Guid>
    {
        public Utilisateur(string userName) : base(userName) { }
    }
}