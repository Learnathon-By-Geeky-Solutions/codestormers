using CosmoVerse.Domain.Entities;
using CosmoVerse.Application.DTOs;
using CosmoVerse.Application.Interfaces.Results;

namespace CosmoVerse.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<AuthResult> LoginAsync(UserLoginDto request);
        Task<UserInfoDto?> GetUserAsync(Guid Id);
        Task<bool> UpdateUser(User user, UpdateProfileDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
        Task<bool> ResetPasswordAsync(PasswordResetDto request);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<List<object>> GetAllUsersAsync();
    }
}
