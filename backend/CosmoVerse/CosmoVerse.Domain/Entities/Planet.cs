using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models
{
    public class Planet
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Planet name is required")]
        [StringLength(100)]
        public required string Name { get; set; }

        public double DistanceFromSun { get; set; }
        public double Diameter { get; set; }
        public double RotationPeriod { get; set; }
        public double OrbitalPeriod { get; set; }

        public required Dictionary<string, string> Description { get; set; }

        public required string MediaUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual List<Satellite> Satellites { get; set; } = new List<Satellite>();
    }
}