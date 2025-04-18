using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// User Data to create a new user.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email of the user.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Profile picture of the user.
        /// </summary>
        public IFormFile? ProfilePicture { get; set; }
    }
}
