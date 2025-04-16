using CosmoVerse.Domain.Entities;

namespace CosmoVerse.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
        Task<bool> SendEmailForVerifyAsync(User user);
        Task<bool> SaveEmailVerificationTokenAsync(User user, string token);
        Task<bool> VerifyEmailAsync(string email, string token);
        Task<bool> SendPasswordResetEmailAsync(string toEmail);
    }
}
