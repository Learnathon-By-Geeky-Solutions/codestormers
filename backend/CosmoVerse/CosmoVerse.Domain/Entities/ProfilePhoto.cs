using System.ComponentModel.DataAnnotations;

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
