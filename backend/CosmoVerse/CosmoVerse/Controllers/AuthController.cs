﻿using CosmoVerse.Models.Domain;
using CosmoVerse.Models.Dto;
using CosmoVerse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService authService;
        private readonly IEmailService emailService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService, IEmailService emailService)
        {
            _logger = logger;
            this.authService = authService;
            this.emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            try
            {
                // Register user
                var user = await authService.RegisterAsync(request);

                // Send email for verification
                if(!await emailService.SendEmailForVerifyAsync(user.Email))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error sending verification email.");
                }

                return Created($"/api/users/{user.Id}", null); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
        {
            try
            {
                // Authenticate user
                var tokenResponse = await authService.LoginAsync(request);

                // Return error if authentication fails
                if (tokenResponse is null || !tokenResponse.Success || tokenResponse.Token is null)
                {
                    return Unauthorized("Invalid email or password");
                }

                // Define cookie options
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Prevents JavaScript access to the cookie
                    Secure = true,   // Ensures the cookie is sent over HTTPS
                    SameSite = SameSiteMode.Strict, // Prevent CSRF
                    Expires = DateTime.UtcNow.AddMinutes(30) // Set cookie expiration
                };

                // Save AccessToken in cookies
                Response.Cookies.Append("AccessToken", tokenResponse.Token.AccessToken, cookieOptions);

                // Save RefreshToken in cookies
                var refreshTokenOptions = new CookieOptions
                {
                    HttpOnly = true, // Prevents JavaScript access to the cookie
                    Secure = true,   // Ensures the cookie is sent over HTTPS
                    SameSite = SameSiteMode.Strict, // Prevent CSRF
                    Expires = DateTime.UtcNow.AddDays(30) // Longer expiration for RefreshToken
                };

                Response.Cookies.Append("RefreshToken", tokenResponse.Token.RefreshToken, refreshTokenOptions);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        [Authorize]
        [HttpGet("User")]
        public async Task<ActionResult<User>> GetUser(Guid Id)
        {
            var user = await authService.GetUserAsync(Id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var tokenResponse = await authService.RefreshTokensAsync(request);
            if (tokenResponse is null || tokenResponse.AccessToken is null || tokenResponse.RefreshToken is null)
                return BadRequest("Invalid refresh token.");

            // Define cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddMinutes(30) // Set cookie expiration
            };

            // Save AccessToken in cookies
            Response.Cookies.Append("AccessToken", tokenResponse.AccessToken, cookieOptions);

            // Save RefreshToken in cookies
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddDays(30) // Longer expiration for RefreshToken
            };

            // Save RefreshToken in cookies
            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, refreshTokenOptions);


            return Ok();
        }

        [HttpPost("Sent-verification-email")]
        public async Task<IActionResult> SentEmailForVerify(string toEmail)
        {
            try
            {
                // Validate the email
                if (string.IsNullOrWhiteSpace(toEmail))
                {
                    return BadRequest(new { message = "Invalid email address." });
                }

                // Send email for verification
                await emailService.SendEmailForVerifyAsync(toEmail);

                return Ok(new { message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send verification email to {Email}", toEmail);
                return StatusCode(500, new { message = "An error occurred while sending the email. Please try again later." });
            }
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult> VerifyEmail(string email, string token)
        {
            try
            {
                // Verify email
                await emailService.VerifyEmailAsync(email, token);
                return Ok("Email verified successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("forgot-password-code")]
        public async Task<IActionResult> ForgotPasswordCode(string email)
        {
            try
            {
                // Validate the email
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest(new { message = "Invalid email address." });
                }

                // Send password reset email
                await emailService.SendPasswordResetEmailAsync(email);
                return Ok(new { message = "Password reset email sent successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                return StatusCode(500, new { message = "An error occurred while sending the email. Please try again later." });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordResetDto request)
        {
            try
            {
                // Validate the email
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new { message = "Invalid email address." });
                }
                // Reset password
                await authService.ResetPasswordAsync(request);
                return Ok(new { message = "Password reset successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Invalid email address." });
            }
            catch(InvalidOperationException)
            {
                return BadRequest(new { message = "Invalid token." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", ex});
            }
        }
    }
}
