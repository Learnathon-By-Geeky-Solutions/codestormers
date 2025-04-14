using CosmoVerse.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Domain.Entities
{
    public class CelestialSystem
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string structure { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual List<Planet> Planets { get; set; } = new List<Planet>();
    }
}
