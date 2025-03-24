using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Models.Dto
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
