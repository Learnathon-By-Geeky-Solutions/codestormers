using CosmoVerse.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CosmoVerse.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Planet> Planets { get; set; }
        public DbSet<Satellite> Satellites { get; set; }
        public DbSet<Star> Stars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Planet>()
                .HasOne(p => p.Star)
                .WithMany(s => s.Planets)
                .HasForeignKey(p => p.StarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Satellite>()
                .HasOne(s => s.Planet)
                .WithMany(p => p.Satellites)
                .HasForeignKey(s => s.PlanetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
