using CosmoVerse.Models.Domain;
using CosmoVerse.Models.Dto;
using CosmoVerse.Services.Results;

namespace CosmoVerse.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<AuthResult> LoginAsync(UserLoginDto request);
        Task<UserInfoDto?> GetUserAsync(Guid Id);
        Task<bool> UpdateUser(User user, UpdateProfileDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
        Task<bool> ResetPasswordAsync(PasswordResetDto request);
    }
}
