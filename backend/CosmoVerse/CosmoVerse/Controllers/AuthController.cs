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
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService, IEmailService emailService, IUserService userService)
        {
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
        /// Returns user information if the registration is successful and a verification email is sent.
        /// </returns>
        /// <response code="201">If the user is successfully registered and a verification email is sent.</response>
        /// <response code="400">If the provided input is invalid.</response>
        /// <response code="500">If an unexpected error occurs during registration or email verification.</response>
        /// <remarks>
        /// Upon successful registration, a verification email is sent to the user's email address.
        /// The user's ID is included in the location header of the created resource.
        /// </remarks>
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDto request)
        {
            if (request == null) {
                return BadRequest("Invalid request data.");
            }
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
            catch (InvalidOperationException ex) when (ex.Message == "Email already exists")
            {
                return Conflict(new { message = "Email already exists. Please use a different email address." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }   
        }

        /// <summary>
        /// Authenticates a user using their login credentials.
        /// </summary>
        /// <param name="request">
        /// An object containing the user's email and password for login.
        /// </param>
        /// <returns>
        /// Returns response with access and refresh tokens if authentication is successful and set in cookies.
        /// </returns>
        /// <response code="200">If authentication succeeds, returns the access and refresh tokens.</response>
        /// <response code="401">If authentication fails due to invalid credentials.</response>
        /// <response code="500">If an unexpected error occurs during authentication.</response>
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

                SetTokenInCookies(tokenResponse.Token.AccessToken, tokenResponse.Token.RefreshToken);

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
        /// Returns User information if the user is successfully authenticated and found.
        /// </returns>
        /// <response code="200">Returns the user information if the user is successfully authenticated and found.</response>
        /// <response code="401">Returns if the user ID claim is missing or invalid, indicating an unauthorized request.</response>
        /// <response code="404">Returns if no user is found with the given ID.</response>
        /// <remarks>
        /// This endpoint requires the user to be authenticated.
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
        /// </returns>
        /// <response code="200">The user's profile is successfully updated.</response>
        /// <response code="401">The user is not authenticated, or their ID claim is missing.</response>
        /// <response code="400">The request data is invalid.</response>
        /// <response code="500">An unexpected error occurs during the update process.</response>
        /// <remarks>
        /// This endpoint requires the user to be authenticated. The user's ID is extracted from the authentication token,
        /// and their profile is updated based on the provided <see cref="UpdateProfileDto"/>.
        /// </remarks>
        [Authorize]
        [HttpPut("update-user")]
        public async Task<ActionResult> UpdateUser([FromForm]UpdateProfileDto request)
        {
            if(request is null)
            {
                return BadRequest("Invalid request data.");
            }
            var user = await _userService.GetUserFromCookieAsync();
            if (user is null)
            {
                return Unauthorized("User not found.");
            }
            try
            {
                await _authService.UpdateUser(user, request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later."});
            }
        }

        /// <summary>
        /// Refreshes the user's access token using a valid refresh token from the cookies.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ActionResult{T}"/>: and new access and refresh tokens are generated and set in the cookies.
        /// </returns>
        /// <response code="200">If the refresh token is valid and the new tokens are generated successfully.</response>
        /// <response code="401">If no valid refresh token is found in the cookies or the token is expired.</response>
        /// <response code="400">If the refresh token is invalid or the tokens cannot be refreshed for any reason.</response>
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

            SetTokenInCookies(tokenResponse.AccessToken, tokenResponse.RefreshToken);

            return Ok();
        }

        /// <summary>
        /// Sends a verification email to the authenticated user.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/>:
        /// </returns>
        /// <response code="200">If the verification email is sent successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an error occurs during the email sending process.</response>
        /// <remarks>
        /// This endpoint requires the user to be authenticated. Upon successful authentication, a verification email is sent to the user's registered email address.
        /// </remarks>
        [HttpPost("Sent-verification-email")]
        public async Task<IActionResult> SentEmailForVerify()
        {
            try
            {
                var user = await _userService.GetUserFromCookieAsync();

                if (user is null)
                {
                    return Unauthorized("User is not authenticated.");
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
        /// An object containing the user's email and the verification token required to verify the user's email address.
        /// </param>
        /// <returns>
        /// Returns an <see cref="ActionResult"/>:
        /// </returns>
        /// <response code="200">If the email is successfully verified.</response>
        /// <response code="500">If there is an error during the email verification process.</response>
        /// <remarks>
        /// This endpoint allows the user to verify their email address by providing their email and associated verification token.
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
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later."});
            }
        }

        /// <summary>
        /// Initiates the password reset process by sending a password reset email to the provided email address.
        /// </summary>
        /// <param name="email">
        /// The email address associated with the user's account. A valid, non-empty email address is required to send the reset email.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/>:
        /// </returns>
        /// <response code="200">If the password reset email is sent successfully.</response>
        /// <response code="400">If the provided email address is invalid or empty.</response>
        /// <response code="500">If an error occurs during the email sending process.</response>
        /// <remarks>
        /// This endpoint is used to request a password reset. A reset email with a link to reset the password will be sent to the specified email address.
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
                return StatusCode(500, new { message = "An error occurred while sending the email. Please try again later." });
            }
        }

        /// <summary>
        /// Resets the user's password using a provided token and new password.
        /// </summary>
        /// <param name="request">
        /// The password reset request containing the user's email address, reset token, and new password.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/>:
        /// </returns>
        /// <response code="200">If the password is successfully reset.</response>
        /// <response code="400">If the provided email or reset token is invalid or expired.</response>
        /// <response code="404">If the email address is not found in the system.</response>
        /// <response code="500">If an unexpected error occurs during the password reset process.</response>
        /// <remarks>
        /// This endpoint allows the user to reset their password after providing a valid reset token.
        /// The token is typically sent via email when the user requests a password reset.
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
        /// Returns an <see cref="IActionResult"/>:
        /// </returns>
        /// <response code="200">If the user was logged out successfully.</response>
        /// <response code="500">If the logout request is malformed or the operation fails.</response>
        /// <remarks>
        /// This endpoint clears the user's authentication cookies (access token and refresh token),
        /// effectively logging the user out and ending their session. No further requests will be authorized
        /// until the user logs in again.
        /// </remarks>
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        // Helper method to set token cookies
        private void SetTokenInCookies(string AccessToken, string RefreshToken)
        {
            // Define cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.None, 
                Expires = DateTime.UtcNow.AddMinutes(30) // Cookie expiration
            };

            // Save AccessToken in cookies
            Response.Cookies.Append("AccessToken", AccessToken, cookieOptions);

            // Save RefreshToken in cookies
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access to the cookie
                Secure = true,   // Ensures the cookie is sent over HTTPS
                SameSite = SameSiteMode.None, 
                Expires = DateTime.UtcNow.AddDays(30) // Expiration for RefreshToken
            };

            Response.Cookies.Append("RefreshToken", RefreshToken, refreshTokenOptions);
        }

    }
}
