using CosmoVerse.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Domain.Entities
{
    public class ProfilePhoto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Url { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        [Required]
        public string PublicId { get; set; } = string.Empty;
    }
}
