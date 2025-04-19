using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// User Data to login a user.
    /// </summary>
    public class UserLoginDto
    {
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
    }
}
