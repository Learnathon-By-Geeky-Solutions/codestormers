using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class PasswordReset
    {
        [Key]
        [Required]
        public Guid Id{ get; set; }
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }
    }
}
