using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class EmailVerification
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }    
        public virtual User User { get; set; } = null!; 

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
