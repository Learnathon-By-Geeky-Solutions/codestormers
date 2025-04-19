using Microsoft.AspNetCore.Http;

namespace CosmoVerse.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating user profile information.
    /// </summary>
    public class UpdateProfileDto
    {
        /// <summary>
        /// New name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Profile picture file to be uploaded.
        /// </summary>
        public IFormFile? ProfilePicture { get; set; }
    }
}
