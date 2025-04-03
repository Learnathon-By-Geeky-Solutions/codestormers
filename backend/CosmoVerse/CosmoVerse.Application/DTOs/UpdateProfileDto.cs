using Microsoft.AspNetCore.Http;

namespace CosmoVerse.Models.Dto
{
    public class UpdateProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? ProfilePicture { get; set; }
    }
}
