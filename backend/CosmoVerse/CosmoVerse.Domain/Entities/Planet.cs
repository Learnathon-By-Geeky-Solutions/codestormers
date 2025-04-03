using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class Planet
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Planet name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mass is required")]
        public double Mass { get; set; }

        [Required(ErrorMessage = "Radius is required")]
        public double Radius { get; set; }

        public int OrbitalPeriod { get; set; }

        public virtual List<Satellite> Satellites { get; set; } = new List<Satellite>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}