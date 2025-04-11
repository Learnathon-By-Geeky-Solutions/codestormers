using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models
{
    public class Planet
    {
        [Key]
        public Guid Id { get; set; }

        public string Introduction { get; set; } = string.Empty;
        public string Namesake { get; set; } = string.Empty;
        public string PotentialForLife { get; set; } = string.Empty;
        public string SizeAndDistance { get; set; } = string.Empty;
        public string OrbitAndRotation { get; set; } = string.Empty;
        public string Moons { get; set; } = string.Empty;
        public string Rings { get; set; } = string.Empty;
        public string Formation { get; set; } = string.Empty;
        public string Structure { get; set; } = string.Empty;
        public string Surface { get; set; } = string.Empty;
        public string Atmosphere { get; set; } = string.Empty;
        public string Magnetosphere { get; set; } = string.Empty;

        public virtual List<Satellite> Satellites { get; set; } = new List<Satellite>();
    }
}