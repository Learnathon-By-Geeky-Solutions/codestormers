using CosmoVerse.Application.DTOs;
using CosmoVerse.Domain.Entities;
using CosmoVerse.Application.Interfaces.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CosmoVerse.Application.Interfaces;

namespace CosmoVerse.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User, Guid> _repository;
        private readonly IRepository<PasswordReset, Guid> _passwordResetRepository;
        private readonly IRepository<ProfilePhoto, Guid> _profilePhotoRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public AuthService(IConfiguration _configuration, IRepository<User, Guid> _repository, IRepository<PasswordReset, Guid> _passwordResetRepository, ICloudinaryService cloudinaryService, IRepository<ProfilePhoto, Guid> profilePhotoRepository)
        {
            this._configuration = _configuration;
            this._repository = _repository;
            this._passwordResetRepository = _passwordResetRepository;
            _profilePhotoRepository = profilePhotoRepository;
            _cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// Handles the login process, authenticating the user with their email and password,
        /// and issuing a JWT token for further authenticated requests.
        /// </summary>
        /// <param name="request">The login request model containing email and password</param>
        /// <returns>A token response if the credentials are valid, or throws an exception if authentication fails</returns>
        public async Task<AuthResult> LoginAsync(UserLoginDto request)
        {
            // Find the user by email
            var user = await _repository.FindAsync(u => u.Email == request.Email);

            // Check if user does not exist
            if (user is null || new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                // Return an error message if the user does not exist or the password is incorrect
                return AuthResult.Failure("Invalid email or password");
            }

            // Return a token response if the user is authenticated
            return AuthResult.SuccessResult(await CreateTokenResponse(user));
        }

        /// <summary>  
        /// Handle registration of a new user, creating a new user record in the database.
        /// </summary>
        /// <param name="request">The user registration request model containing user details</param>
        /// <returns>The newly created user record</returns>
        public async Task<User?> RegisterAsync(UserDto request)
        {
            bool existingEmail;
            try
            {
                // Check if the email already exists in the database
                existingEmail = await _repository.ExistsAsync(u => u.Email == request.Email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while checking the email.", ex);
            }
            if (existingEmail)
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Create a new user record
            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.Name = request.Name;
            user.PasswordHash = hashedPassword;
            user.Id = Guid.NewGuid();
            user.Email = request.Email;
            user.IsEmailVerified = false;
            user.Role = "User";
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            if (request.ProfilePicture is not null && request.ProfilePicture.Length > 0)
            {
                var acceptedTypes = new[] { ".jpeg", ".png", ".jpg" };
                if (!acceptedTypes.Contains(Path.GetExtension(request.ProfilePicture.FileName)))
                {
                    throw new InvalidOperationException("Invalid image file type");
                }
            }

            // Add the user to the database
            await _repository.AddAsync(user);

            if (request.ProfilePicture is not null && request.ProfilePicture.Length > 0)
            {
                var imageInfo = await UploadPhoto(request.ProfilePicture);

                if (imageInfo is not null)
                {
                    var profilePhoto = new ProfilePhoto
                    {
                        Id = Guid.NewGuid(),
                        Url = imageInfo.ImageUrl,
                        PublicId = imageInfo.PublicId,
                        CreatedAt = imageInfo.CreatedAt,
                        UserId = user.Id,
                        User = user
                    };

                    // Add the profile photo to the database
                    await _profilePhotoRepository.AddAsync(profilePhoto);
                }
            }

            // Return the newly created user record
            return user;
        }

        /// <summary>
        /// Updates the user's profile information, including name and profile picture.
        /// </summary>
        /// <param name="user">User data</param>
        /// <param name="request">New Update user data</param>
        /// <returns>True if updated successfully</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> UpdateUser(User user, UpdateProfileDto request)
        {
            if(request.Name is not null)
            {
                user.Name = request.Name;
            }

            if (request.ProfilePicture is not null && request.ProfilePicture.Length > 0)
            {
                var acceptedTypes = new[] { ".jpeg", ".png", ".jpg" };
                if (!acceptedTypes.Contains(Path.GetExtension(request.ProfilePicture.FileName)))
                {
                    throw new InvalidOperationException("Invalid image file type");
                }
            }

            try
            {
                // Check if a new profile picture is provided
                if (request.ProfilePicture is not null && request.ProfilePicture.Length > 0)
                {
                    if(user.ProfilePhoto is not null)
                    {
                        await DeletePhoto(user.ProfilePhoto.PublicId);
                        await _profilePhotoRepository.DeleteAsync(user.ProfilePhoto);
                    }

                    var imageInfo = await UploadPhoto(request.ProfilePicture);
                    if (imageInfo is not null)
                    {
                        var profilePhoto = new ProfilePhoto
                        {
                            Id = Guid.NewGuid(),
                            Url = imageInfo.ImageUrl,
                            PublicId = imageInfo.PublicId,
                            CreatedAt = imageInfo.CreatedAt,
                            UserId = user.Id,
                            User = user
                        };

                        // Add the profile photo to the database
                        await _profilePhotoRepository.AddAsync(profilePhoto);
                    }
                }

                await _repository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the profile.", ex);
            }
            return true;
        }

        /// <summary>
        /// Refreshes the user's access token using a valid refresh token.
        /// </summary>
        /// <param name="request">The refresh token request model containing the user ID and refresh token</param>
        /// <returns>A token response if the refresh token is valid, or null if the refresh token is invalid</returns>
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            // Validate the refresh token
            var user = await ValidateRefreshTokenAsync(request.RefreshToken);

            if (user is null)
            {
                return null;
            }

            // Return a token response containing new access token and refresh token if the refresh token is valid
            return await CreateTokenResponse(user);
        }

        /// <summary>
        /// Validates the refresh token for a given user.
        /// </summary>
        /// <param name="refreshToken">RefreshToken of the user</param>
        /// <returns>User if the refresh token valid otherwise null</returns>
        private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {

            var user = await _repository.FindAsync(u=> u.RefreshToken == refreshToken);

            // Check if the user exists and the refresh token is valid
            if (user is null || !SecureCompare(user.RefreshToken, refreshToken) || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            // Return the user if the refresh token is valid
            return user;
        }

        /// <summary>
        /// Generates a new token response for a given user.
        /// </summary>
        /// <param name="user">User for which the token response is generated</param>
        /// <returns>A token response containing access token and refresh token</returns>
        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        /// <summary>
        /// Generates a new refresh token for a given user and saves it to the database.
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>The newly generated refresh token</returns>
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            // Generate a new refresh token
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken; // Set the new refresh token
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set the expiry time for the refresh token

            // Update the user record with the new refresh token in the database
            await _repository.UpdateAsync(user);

            return refreshToken;
        }

        /// <summary>
        /// Generates a new refresh token.
        /// </summary>
        /// <returns>The newly generated refresh token</returns>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Creates a new JWT token for a given user.
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>The newly generated JWT token</returns>
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("EmailVerified", user.IsEmailVerified.ToString()) // Custom claim
                };

            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new InvalidOperationException("JWT_SECRET not configured");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)); 

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512); // Create signing credentials

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }


        /// <summary>
        /// Get user information by Id  
        /// </summary>
        /// <param name="Id">User Id</param>
        /// <returns>User information</returns>
        public async Task<UserInfoDto?> GetUserAsync(Guid Id)
        {
            var user = await _repository.FindAsync(u => u.Id == Id, u => u.ProfilePhoto);
            if (user is null)
            {
                return null;
            }
            var userInfo = new UserInfoDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsEmailVerified = user.IsEmailVerified,
                ProfilePictureUrl = user.ProfilePhoto?.Url ?? ""
            };
            return userInfo;
        }


        /// <summary>
        /// Reset the user password
        /// </summary> 
        /// <param name="request">Password reset request model containing email and token and new password</param>
        /// <returns>True if the password was reset successfully, or throws an exception if the password reset fails</returns>
        public async Task<bool> ResetPasswordAsync(PasswordResetDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                throw new ArgumentException("Email and password must not be empty.");
            }

            var user = await _repository.FindAsync(u => u.Email == request.Email);

            if (user is null)
            {
                throw new KeyNotFoundException("User not found.");
            }
           
            var tokenDetails = user.PasswordReset;

            if (tokenDetails is null) {
                throw new KeyNotFoundException("Invalid token.");
            }
            if (tokenDetails.ExpiryDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Token expired.");
            }

            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.NewPassword);
            user.PasswordHash = hashedPassword;

            try
            {
                // Update the user password
                await _repository.UpdateAsync(user);

                // Delete the password reset token
                await _passwordResetRepository.DeleteAsync(tokenDetails);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while resetting the password.", ex);
            }
        }

        /// <summary>
        /// Deletes a user by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if user is deleted</returns>
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _repository.FindAsync(u => u.Id == userId);
            if (user is null)
            {
                return false;
            }
            if (user.ProfilePhoto is not null)
            {
                await DeletePhoto(user.ProfilePhoto.PublicId);
                await _profilePhotoRepository.DeleteAsync(user.ProfilePhoto);
            }
            await _repository.DeleteAsync(user);
            return true;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns> All user list </returns>
        public async Task<List<object>> GetAllUsersAsync()
        {
            var users = await _repository.FindWithProjectionAsync(
                predicate: u => true,
                selector: u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.IsEmailVerified,
                    ProfilePictureUrl = u.ProfilePhoto.Url
                });
            return users.Cast<object>().ToList();
        }

        /// <summary>
        /// Uploads a photo to Cloudinary and returns the image information.
        /// </summary>
        /// <param name="file">Image file</param>
        /// <returns>Image info</returns>
        private async Task<ImageDto> UploadPhoto(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var imgInfo = await _cloudinaryService.UploadImageAsync(stream, file.FileName);
                return imgInfo;
            }
            return null;
        }

        /// <summary>
        /// Deletes a photo from Cloudinary using the public ID.
        /// </summary>
        /// <param name="publicId">Public id of the image</param>
        /// <returns></returns>
        private async Task<bool> DeletePhoto(string publicId)
        {
            if (publicId != null)
            {
                await _cloudinaryService.DeleteImageAsync(publicId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Performs a constant-time comparison of two strings to prevent timing attacks.
        /// </summary>
        /// <param name="a">The first string to compare.</param>
        /// <param name="b">The second string to compare.</param>
        /// <returns>
        /// True if the strings are equal; otherwise, false.
        /// </returns>
        private static bool SecureCompare(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return false;                
            }
            int result = 0;
            for(int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }
    }
}
