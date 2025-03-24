namespace CosmoVerse.Models.Dto
{
    public class UserInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public string ProfilePictureUrl { get; set; } = string.Empty;
    }
}
