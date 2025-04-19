namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for User information.
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user's email is verified.
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// URL of the user's profile picture.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
