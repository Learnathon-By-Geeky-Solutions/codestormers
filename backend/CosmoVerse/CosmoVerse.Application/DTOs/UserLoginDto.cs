using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Application.DTOs
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
