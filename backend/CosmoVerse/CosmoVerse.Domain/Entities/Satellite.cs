using System;
using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class Satellite
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Satellite name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mass is required")]
        public double Mass { get; set; }

        public double DistanceFromPlanet { get; set; } // Distance in km

        public Guid PlanetId { get; set; } // Foreign Key
        public virtual Planet Planet { get; set; } // Navigation Property

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
