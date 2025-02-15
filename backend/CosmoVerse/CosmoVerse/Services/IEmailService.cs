using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message);
        Task<bool> SentEmailForVerifyAsync(string toEmail);
        Task<bool> SaveEmailVerificationTokenAsync(string email, string token);
        Task<bool> VerifyEmailAsync(string email, string token);
    }
}
