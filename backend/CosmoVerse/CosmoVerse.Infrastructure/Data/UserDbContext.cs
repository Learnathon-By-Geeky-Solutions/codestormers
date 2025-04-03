using CosmoVerse.Domain.Entities;
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

            // Configure one-to-one relationship between User and EmailVerification
            modelBuilder.Entity<User>()
                .HasOne(u => u.EmailVerification)
                .WithOne(ev => ev.User)
                .HasForeignKey<EmailVerification>(ev => ev.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relationship between User and PasswordReset
            modelBuilder.Entity<User>()
                .HasOne(u => u.PasswordReset)
                .WithOne(pr => pr.User)
                .HasForeignKey<PasswordReset>(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relationship between User and ProfilePhoto
            modelBuilder.Entity<User>()
                .HasOne(u => u.ProfilePhoto)
                .WithOne(p => p.User)
                .HasForeignKey<ProfilePhoto>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
