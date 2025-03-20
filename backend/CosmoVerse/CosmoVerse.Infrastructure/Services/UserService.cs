using CosmoVerse.Application.Services;
using CosmoVerse.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<User> GetUserFromCookieAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return null;
            }
            var Id = Guid.Parse(userIdClaim);
            var user = await _userRepository.FindByIdAsync(Id);
            return user;
        }
    }
}
