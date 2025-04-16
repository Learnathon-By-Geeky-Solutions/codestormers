using CosmoVerse.Domain.Entities;

namespace CosmoVerse.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserFromCookieAsync();
    }
}
