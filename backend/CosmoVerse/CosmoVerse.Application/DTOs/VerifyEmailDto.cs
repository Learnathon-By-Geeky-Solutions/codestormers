using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for verifying user email.
    /// </summary>
    public class VerifyEmailDto
    {
        /// <summary>
        /// Email address of the user to verify.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Token which is sent to the user's email for verification.
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
