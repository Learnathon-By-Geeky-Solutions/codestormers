using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CosmoVerse.Domain.Entities
{
    public class Satellite
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Satellite name is required")]
        [StringLength(256)]
        public  string Name { get; set; } = string.Empty;

        public double Size { get; set; }
        public double DistanceFromPlanet { get; set; }
        public double OrbitalPeriod { get; set; }
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid PlanetId { get; set; }
        [JsonIgnore]
        public virtual Planet Planet { get; set; }
    }
}