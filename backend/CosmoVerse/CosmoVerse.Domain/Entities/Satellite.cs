using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models
{
    public class Satellite
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Satellite name is required")]
        [StringLength(256)]
        public required string Name { get; set; }

        public double Size { get; set; }
        public double DistanceFromPlanet { get; set; }
        public double OrbitalPeriod { get; set; }
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid PlanetId { get; set; }
        public virtual required Planet Planet { get; set; }
    }
}