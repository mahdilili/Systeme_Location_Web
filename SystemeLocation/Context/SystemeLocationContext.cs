using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SystemeLocation.Data;
using SystemeLocation.Entities;

namespace SystemeLocation.Context
{
    public class SystemeLocationContext : IdentityDbContext<Utilisateur, IdentityRole<Guid>, Guid>
    {
        public DbSet<Succursale> Succursales { get; set; }
        public DbSet<Voiture> Voitures { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Adresse> Adresses { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Conducteur> Conducteurs { get; set; }

        public SystemeLocationContext(DbContextOptions<SystemeLocationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
