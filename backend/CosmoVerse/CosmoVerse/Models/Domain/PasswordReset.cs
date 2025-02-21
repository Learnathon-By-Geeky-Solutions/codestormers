using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class PasswordReset
    {
        [Key]
        public Guid Id{ get; set; }
        public string Email { get; set; } = string.Empty;
        public int Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
