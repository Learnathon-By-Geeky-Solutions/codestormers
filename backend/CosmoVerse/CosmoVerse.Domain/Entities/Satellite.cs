using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models
{
    public class Satellite
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PlanetId { get; set; }

        [Required(ErrorMessage = "Satellite name is required")]
        [StringLength(100)]
        public required string Name { get; set; }

        public double Size { get; set; }
        public double DistanceFromPlanet { get; set; }
        public double OrbitalPeriod { get; set; }

        public required Dictionary<string, string> Description { get; set; }

        public required string MediaUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual required Planet Planet { get; set; }
    }
}