using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Application.DTOs
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
