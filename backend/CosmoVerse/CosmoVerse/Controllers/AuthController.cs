using CosmoVerse.Application.Interfaces;
using CosmoVerse.Domain.Entities;
using CosmoVerse.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="request">
        /// An object containing the user's registration details, such as name, email, and password.
        /// </param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> of type <see cref="User"/>.
        /// - Returns an HTTP 201 Created response if the user is successfully registered.
        /// - Returns an HTTP 500 Internal Server Error if an unexpected error occurs during registration or email verification.
        /// - Returns an HTTP 400 Bad Request if an exception is thrown with an error message.
        /// </returns>
        /// <remarks>
        /// Upon successful registration, a verification email is sent to the user's email address.
        /// The user's ID is included in the location of the created resource.
        /// </remarks>
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

        /// <summary>
        /// Authenticates a user using their login credentials.
        /// </summary>
        /// <param name="request">
        /// An object containing the user's email and password for login.
        /// </param>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> containing a <see cref="TokenResponseDto"/> if authentication succeeds.
        /// If authentication fails, returns an HTTP 401 Unauthorized response with an error message.
        /// If an unexpected error occurs, returns an HTTP 500 Internal Server Error with a message.
        /// </returns>
        /// <remarks>
        /// This endpoint sets the access token and refresh token in the user's cookies upon successful login.
        /// </remarks>
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

        /// <summary>
        /// Retrieves the currently authenticated user's information.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/> of type <see cref="User"/>.
        /// - Returns an HTTP 200 OK response with the user information if the user is found.
        /// - Returns an HTTP 401 Unauthorized response if the user ID claim is missing or invalid.
        /// - Returns an HTTP 404 Not Found response if no user is found with the given ID.
        /// </returns>
        /// <remarks>
        /// This endpoint requires the user to be authenticated and authorized.
        /// The user's ID is extracted from the claims provided by the authentication token.
        /// </remarks>
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

        /// <summary>
        /// Updates the currently authenticated user's profile information.
        /// </summary>
        /// <param name="request">
        /// An object containing the updated user profile details, such as name, email, etc.
        /// </param>
        /// <returns>
        /// Returns an <see cref="ActionResult"/>:
        /// - HTTP 200 OK if the user's profile is successfully updated.
        /// - HTTP 401 Unauthorized if the user is not found or not authenticated.
        /// - HTTP 500 Internal Server Error if an unexpected error occurs during the update process.
        /// </returns>
        /// <remarks>
        /// This endpoint requires the user to be authenticated. The user's details are retrieved from cookies,
        /// and their profile is updated based on the provided <see cref="UpdateProfileDto"/>.
        /// </remarks>
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
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later."});
            }
            return Ok();
        }

        /// <summary>
        /// Refreshes the user's access token using a valid refresh token from the cookies.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/>:
        /// - HTTP 200 OK if the refresh token is valid and the new tokens are generated successfully.
        /// - HTTP 401 Unauthorized if no valid refresh token is found in the cookies.
        /// - HTTP 400 Bad Request if the refresh token is invalid or the tokens cannot be refreshed.
        /// </returns>
        /// <remarks>
        /// This endpoint requires the refresh token to be present in the user's cookies.
        /// If the refresh token is valid, new access and refresh tokens are generated and set in the cookies.
        /// </remarks>
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

        /// <summary>
        /// Sends a verification email to the authenticated user.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/>:
        /// - HTTP 200 OK if the verification email is sent successfully.
        /// - HTTP 401 Unauthorized if the user is not authenticated.
        /// - HTTP 500 Internal Server Error if an error occurs during the email sending process.
        /// </returns>
        /// <remarks>
        /// This endpoint requires the user to be authenticated. Upon successful authentication, it sends a verification email.
        /// </remarks>
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

        /// <summary>
        /// Verifies the user's email address using a verification token.
        /// </summary>
        /// <param name="verificationRequest">
        /// The email and verification token required to verify the user's email address.
        /// </param>
        /// <returns>
        /// Returns an <see cref="ActionResult"/>:
        /// - HTTP 200 OK if the email is successfully verified.
        /// - HTTP 400 Bad Request if there is an error during the email verification process.
        /// </returns>
        /// <remarks>
        /// This endpoint allows the user to verify their email address by providing the email and associated verification token.
        /// </remarks>
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

        /// <summary>
        /// Initiates the password reset process by sending a password reset email to the provided email address.
        /// </summary>
        /// <param name="email">
        /// The email address associated with the user's account. A valid email address is required to send the reset email.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/>:
        /// - HTTP 200 OK if the password reset email is sent successfully.
        /// - HTTP 400 Bad Request if the provided email address is invalid or empty.
        /// - HTTP 500 Internal Server Error if an error occurs during the email sending process.
        /// </returns>
        /// <remarks>
        /// This endpoint is used to request a password reset. A reset email with a reset link will be sent to the specified email address.
        /// </remarks>
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordCode([FromBody] string email)
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

        /// <summary>
        /// Resets the user's password using a provided token and new password.
        /// </summary>
        /// <param name="request">
        /// The password reset request containing the email address, reset token, and new password.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/>:
        /// - HTTP 200 OK if the password is successfully reset.
        /// - HTTP 400 Bad Request if the provided email or reset token is invalid.
        /// - HTTP 404 Not Found if the email address is not registered.
        /// - HTTP 500 Internal Server Error if an unexpected error occurs during the password reset process.
        /// </returns>
        /// <remarks>
        /// This endpoint is used to reset the user's password after they have provided a valid reset token.
        /// The token is sent via email after the user requests a password reset.
        /// </remarks>
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

        /// <summary>
        /// Logs the current user out by clearing the authentication tokens from the session.
        /// This operation removes the user's access and refresh tokens, effectively ending their session.
        /// </summary>
        /// <returns>
        /// An HTTP status code indicating the outcome of the logout operation:
        /// - 200 OK if the user was logged out successfully.
        /// - 400 Bad Request if the logout request was malformed or failed.
        /// </returns>
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
