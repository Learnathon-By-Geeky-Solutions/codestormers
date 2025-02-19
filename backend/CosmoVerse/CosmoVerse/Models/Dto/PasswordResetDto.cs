namespace CosmoVerse.Models.Dto
{
    public class PasswordResetDto
    {
        public string Email { get; set; } = string.Empty;
        public int Token { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
