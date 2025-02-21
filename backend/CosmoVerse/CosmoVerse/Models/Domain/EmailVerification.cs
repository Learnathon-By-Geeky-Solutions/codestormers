using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class EmailVerification
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }
}
