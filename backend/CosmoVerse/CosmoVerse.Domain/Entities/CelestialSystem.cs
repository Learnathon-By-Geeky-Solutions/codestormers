using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Domain.Entities
{
    public class CelestialSystem
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Structure { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual List<Planet> Planets { get; set; } = new List<Planet>();
    }
}
