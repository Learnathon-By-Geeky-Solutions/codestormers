using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Password Reset operation.
    /// </summary>
    public class PasswordResetDto
    {
        /// <summary>
        /// Email address of the user requesting password reset.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Token for password reset that is sent to the user's email.
        ///</summary>
        [Required]
        [MaxLength(100)]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// New password to be set for the user.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
