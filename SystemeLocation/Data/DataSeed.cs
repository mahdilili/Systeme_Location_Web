using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SystemeLocation.Entities;

namespace SystemeLocation.Data
{
    public static class DataSeed
    {
        public static readonly PasswordHasher<Utilisateur> PASSWORD_HASHER = new();

        public static void Seed(this ModelBuilder builder)
        {
            var adminRole = AddRole(builder, "Administrateur");
            _ = AddRole(builder, "Gérant");
            _ = AddRole(builder, "Commis");
            _ = AddRole(builder, "Utilisateur");
            var adminUser = AddUser(builder, "mahdi", "Toto123!");
            addUserToRole(builder, adminUser, adminRole);
        }

        private static void addUserToRole(ModelBuilder builder, Utilisateur newUser, IdentityRole<Guid> adminRole)
        {
            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                UserId = newUser.Id,
                RoleId = adminRole.Id,
            });
        }

        private static IdentityRole<Guid> AddRole(ModelBuilder builder, string name)
        {
            var newRole = new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = name,
                NormalizedName = name.ToUpper()
            };
            builder.Entity<IdentityRole<Guid>>().HasData(newRole);

            return newRole;
        }

        private static Utilisateur AddUser(ModelBuilder builder,
            string userName, string password)
        {
            var newUser = new Utilisateur(userName)
            {
                Id = Guid.NewGuid(),
                NormalizedUserName = userName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
            builder.Entity<Utilisateur>().HasData(newUser);

            return newUser;
        }
    }
}