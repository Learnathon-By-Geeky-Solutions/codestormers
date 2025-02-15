using CosmoVerse.Models.Domain;
using CosmoVerse.Models.Dto;
using CosmoVerse.Services;
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
                var user = await authService.RegisterAsync(request);
                await emailService.SentEmailForVerify(user.Email);
                return Created($"/api/users/{user.Id}", null); 
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }   
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
        {
            var tokenResponse = await authService.LoginAsync(request);
            if (tokenResponse is null)
            {
                return BadRequest("Invalid email or password");
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
            Response.Cookies.Append("AccessToken", tokenResponse.AccessToken, cookieOptions);

            // Save RefreshToken in cookies
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF
                Expires = DateTime.UtcNow.AddDays(30) // Longer expiration for RefreshToken
            };

            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, refreshTokenOptions);

            // Return tokens in the response body (optional, depending on your use case)

            return Ok(tokenResponse);
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

            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, refreshTokenOptions);


            return Ok(tokenResponse);
        }

        [HttpPost("SentEmailForVerify")]
        public async Task<ActionResult> SentEmailForVerify(string toEmail)
        {
            try
            {
                await emailService.SentEmailForVerify(toEmail);
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult> VerifyEmail(string email, string token)
        {
            try
            {
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
