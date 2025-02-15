using CosmoVerse.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CosmoVerse.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }

    }
}
