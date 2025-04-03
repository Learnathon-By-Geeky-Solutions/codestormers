using CosmoVerse.Application.Services;
using CosmoVerse.Models.Domain;
using CosmoVerse.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CosmoVerse.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IRepository<User, Guid> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get user from cookie
        /// </summary>
        /// <returns>User</returns>
        public async Task<User> GetUserFromCookieAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return null;
            }
            try
            {
                var id = Guid.Parse(userIdClaim);
                var user = await _userRepository.FindByIdAsync(id);
                return user;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
