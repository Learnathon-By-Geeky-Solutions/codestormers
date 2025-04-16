using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CosmoVerse.Application.DTOs
{
    public class UserDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        public IFormFile? ProfilePicture { get; set; }
    }
}
