namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Token response.
    /// </summary>
    public class TokenResponseDto
    {
        /// <summary>
        /// Access token used for authentication.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Refresh token used to obtain a new access token.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
