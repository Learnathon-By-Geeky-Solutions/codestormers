using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class PasswordReset
    {
        [Key]
        public Guid Id{ get; set; }
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
