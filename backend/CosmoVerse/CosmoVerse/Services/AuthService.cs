﻿using CosmoVerse.Models.Domain;
using CosmoVerse.Models.Dto;
using CosmoVerse.Repositories;
using CosmoVerse.Services.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CosmoVerse.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration Configuration;
        private readonly IRepository<User, Guid> repository;
        private readonly IRepository<PasswordReset, Guid> passwordResetRepository;
        private readonly IEmailService emailService;


        // Injecting IConfiguration and IRepository<User> into the constructor
        public AuthService(IConfiguration Configuration, IRepository<User, Guid> repository, IEmailService emailService, IRepository<PasswordReset, Guid> passwordResetRepository)
        {
            this.Configuration = Configuration;
            this.repository = repository;
            this.emailService = emailService;
            this.passwordResetRepository = passwordResetRepository;
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
            var user = await repository.FindAsync(u => u.Email == request.Email);

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
                existingEmail = await repository.ExistsAsync(u => u.Email == request.Email);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while checking the email.", ex);
            }
            if(existingEmail)
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

            // Add the user to the database
            await repository.AddAsync(user);

            // Return the newly created user record
            return user;
        }



        public async Task<bool> UpdateUser(User user, UpdateProfileDto request)
        {
            user.Name = request.Name;
            user.ProfilePictureUrl = request.ProfilePictureUrl;
            try
            {
                await repository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the profile.", ex);
            }
            return true;
        }





        /// <summary>
        /// Refreshes the user's access token using a valid refresh token.
        /// </summary>
        /// <param name="request">The refresh token request model containing the user ID and refresh token</param>
        /// <returns>A token response if the refresh token is valid, or throws an exception if the refresh token is invalid</returns>
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            // Validate the refresh token
            var user = await ValidateRefreshTokenAsync(request.RefreshToken);

            // Throw an exception if the refresh token is invalid
            if (user is null)
            {
                throw new Exception("Invalid refresh token");
            }

            // Return a token response containing new access token and refresh token if the refresh token is valid
            return await CreateTokenResponse(user);
        }


        /// <summary>
        /// Validates the refresh token for a given user.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="refreshToken">RefreshToken of the user</param>
        /// <returns>User if the refresh token valid otherwise null</returns>
        private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {

            var user = await repository.FindAsync(u=> u.RefreshToken == refreshToken);

            // Find the user by Id
            //var user = await repository.FindByIdAsync(userId);

            // Check if the user exists and the refresh token is valid
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
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
            await repository.UpdateAsync(user);

            return refreshToken;
        }


        /// <summary>
        /// Generates a new refresh token.
        /// </summary>
        /// <returns>The newly generated refresh token</returns>
        private string GenerateRefreshToken()
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

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration.GetValue<string>("AppSettings:Token")!)); // Get the secret key from appsettings.json

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512); // Create signing credentials

            var tokenDescriptor = new JwtSecurityToken(
                issuer: Configuration.GetValue<string>("AppSettings:Issuer"),
                audience: Configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
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
            var user = await repository.FindByIdAsync(Id);
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
                ProfilePictureUrl = user.ProfilePictureUrl
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

            var user = await repository.FindAsync(u => u.Email == request.Email);

            if (user is null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            //var tokenDetails = await passwordResetRepository.FindAsync(p => p.UserId == user.Id && p.Token == request.Token);
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
                await repository.UpdateAsync(user);

                // Delete the password reset token
                await passwordResetRepository.DeleteAsync(tokenDetails);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while resetting the password.", ex);
            }
        }
    }
}
