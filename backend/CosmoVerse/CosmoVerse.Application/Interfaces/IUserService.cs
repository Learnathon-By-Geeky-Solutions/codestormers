using CosmoVerse.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Application.Services
{
    public interface IUserService
    {
        Task<User> GetUserFromCookieAsync();
    }
}
