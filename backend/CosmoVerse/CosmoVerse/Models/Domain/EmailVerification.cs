using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class EmailVerification
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryTime { get; set; }
    }
}
