using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Refresh Token request.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// Refresh token used to obtain a new access token.
        /// </summary>
        [Required]
        public required string RefreshToken { get; set; }
    }
}
