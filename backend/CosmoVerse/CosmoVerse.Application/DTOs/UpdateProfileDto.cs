using Microsoft.AspNetCore.Http;

namespace CosmoVerse.Application.DTOs
{
    public class UpdateProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? ProfilePicture { get; set; }
    }
}
