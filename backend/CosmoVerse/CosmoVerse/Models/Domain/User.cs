using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        // User information
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
        public string EmailVerifyToken { get; set; } = string.Empty;
        public DateTime? EmailVerifyTokenExpiryTime { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        [StringLength(500)]
        public string ProfilePictureUrl { get; set; } = string.Empty;

        // Timestamps for account tracking
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
