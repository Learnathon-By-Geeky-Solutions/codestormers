using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Dto
{
    public class PasswordResetDto
    {
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Token { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
