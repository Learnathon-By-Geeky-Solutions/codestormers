using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class Star
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Star name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mass is required")]
        public double Mass { get; set; } // Mass in Solar masses

        [Required(ErrorMessage = "Radius is required")]
        public double Radius { get; set; } // Radius in km

        public double Luminosity { get; set; } // In Solar Luminosities

        public virtual List<Planet> Planets { get; set; } = new List<Planet>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
