using System;
using System.Collections.Generic;
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
        public double Mass { get; set; } // Mass in kg

        [Required(ErrorMessage = "Radius is required")]
        public double Radius { get; set; } // Radius in km

        public int OrbitalPeriod { get; set; } // In days

        public Guid StarId { get; set; } // Foreign Key
        public virtual Star Star { get; set; } // Navigation Property

        public virtual List<Satellite> Satellites { get; set; } = new List<Satellite>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
