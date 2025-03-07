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
        public DbSet<PasswordReset> PasswordResets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.PasswordReset)
                .WithOne(pr => pr.User)
                .HasForeignKey<PasswordReset>(pr => pr.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.EmailVerification)
                .WithOne(ev => ev.User)
                .HasForeignKey<EmailVerification>(ev => ev.UserId);
        }
    }
}
