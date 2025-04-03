using CosmoVerse.Application.Services;
using CosmoVerse.Models.Domain;
using CosmoVerse.Models.Dto;
using CosmoVerse.Repositories;
using CosmoVerse.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmoVerse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService, IEmailService emailService, IUserService userService)
        {
            _logger = logger;
            _authService = authService;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromForm] UserDto request)
        {
            try
            {
                // Register user
                var user = await _authService.RegisterAsync(request);

                if(user is null)
                {
                    return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." }); 
                }

                // Send email for verification
                if(!await _emailService.SendEmailForVerifyAsync(user))
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
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] UserLoginDto request)
        {
            try
            {
                // Authenticate user
                var tokenResponse = await _authService.LoginAsync(request);

                // Return error if authentication fails
                if (tokenResponse is null || !tokenResponse.Success || tokenResponse.Token is null)
                {
                    return Unauthorized("Invalid email or password");
                }

                setTokenInCookies(tokenResponse.Token.AccessToken, tokenResponse.Token.RefreshToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        [Authorize]
        [HttpGet("User")]
        public async Task<ActionResult<User>> GetUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found.");
            }

            var Id = Guid.Parse(userIdClaim);
            var user = await _authService.GetUserAsync(Id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [Authorize]
        [HttpPut("update-user")]
        public async Task<ActionResult> UpdateUser([FromForm]UpdateProfileDto request)
        {
            var user = await _userService.GetUserFromCookieAsync();
            if (user is null)
            {
                return Unauthorized("User not found.");
            }
            try
            {
                await _authService.UpdateUser(user, request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
            return Ok();
        }





        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }

            var request = new RefreshTokenRequestDto
            {
                RefreshToken = refreshToken
            };

            var tokenResponse = await _authService.RefreshTokensAsync(request);
            if (tokenResponse is null || tokenResponse.AccessToken is null || tokenResponse.RefreshToken is null)
            {
                return BadRequest("Invalid refresh token.");
            }

            setTokenInCookies(tokenResponse.AccessToken, tokenResponse.RefreshToken);

            return Ok();
        }

        [HttpPost("Sent-verification-email")]
        public async Task<IActionResult> SentEmailForVerify()
        {
            try
            {
                var user = await _userService.GetUserFromCookieAsync();

                if (user is null)
                {
                    return Unauthorized("User not found.");
                }

                // Send email for verification
                await _emailService.SendEmailForVerifyAsync(user);

                return Ok(new { message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the email. Please try again later." });
            }
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult> VerifyEmail([FromBody] VerifyEmailDto verificationRequest)
        {
            try
            {
                // Verify email
                await _emailService.VerifyEmailAsync(verificationRequest.Email, verificationRequest.Token);
                return Ok("Email verified successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("forgot-password-code")]
        public async Task<IActionResult> ForgotPasswordCode([FromBody] string email)
        {
            try
            {
                // Validate the email
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest(new { message = "Invalid email address." });
                }

                // Send password reset email
                await _emailService.SendPasswordResetEmailAsync(email);
                return Ok(new { message = "Password reset email sent successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                return StatusCode(500, new { message = "An error occurred while sending the email. Please try again later." });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto request)
        {
            try
            {
                // Validate the email
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new { message = "Invalid email address." });
                }
                // Reset password
                await _authService.ResetPasswordAsync(request);
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


        [HttpPost("Logout")]
        public IActionResult Logout()
        {

            // Define cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddDays(-1) // Set cookie expiration
            };
            // Remove AccessToken from cookies
            Response.Cookies.Delete("AccessToken", cookieOptions);
            // Remove RefreshToken from cookies
            Response.Cookies.Delete("RefreshToken", cookieOptions);
            return Ok();
        }



        // Helper method to set token cookies
        private void setTokenInCookies(string AccessToken, string RefreshToken)
        {
            // Define cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddMinutes(30) // Cookie expiration
            };

            // Save AccessToken in cookies
            Response.Cookies.Append("AccessToken", AccessToken, cookieOptions);

            // Save RefreshToken in cookies
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddDays(30) // Expiration for RefreshToken
            };

            Response.Cookies.Append("RefreshToken", RefreshToken, refreshTokenOptions);
        }

    }
}
