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
                if(!await emailService.SentEmailForVerifyAsync(user.Email))
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
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }

        [Authorize]
        [HttpGet("UserInfo")]
        public async Task<ActionResult<User>> GetUser(Guid Id)
        {
            var user = await authService.GetUserAsync(Id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost("refreshToken")]
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

        [HttpPost("SentEmailForVerify")]
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
                await emailService.SentEmailForVerifyAsync(toEmail);

                return Ok(new { message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send verification email to {Email}", toEmail);
                return StatusCode(500, new { message = "An error occurred while sending the email. Please try again later." });
            }
        }

        [HttpPost("verifyEmail")]
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
    }
}
