using Microsoft.EntityFrameworkCore;
using CosmoVerse.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CosmoVerse.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Satellite> Satellites { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dictionaryConverter = new ValueConverter<Dictionary<string, string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>()
            );

            modelBuilder.Entity<Planet>()
                .Property(p => p.Description)
                .HasConversion(dictionaryConverter);

            modelBuilder.Entity<Satellite>()
                .Property(s => s.Description)
                .HasConversion(dictionaryConverter);

            modelBuilder.Entity<Satellite>()
                .HasOne(s => s.Planet)
                .WithMany(p => p.Satellites)
                .HasForeignKey(s => s.PlanetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}