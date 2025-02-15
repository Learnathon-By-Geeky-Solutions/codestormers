using System.Net.Mail;
using System.Net;
using CosmoVerse.Models.Domain;
using CosmoVerse.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> repository;
        private readonly IRepository<EmailVerification> emailVerificationRepository;
        public EmailService(IConfiguration Configuration, IRepository<User> repository, IRepository<EmailVerification> emailVerificationRepository)
        {
            _configuration = Configuration;
            this.repository = repository;
            this.emailVerificationRepository = emailVerificationRepository;
        }
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
        {
            // Read email settings from appsettings.json
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var portString = _configuration["EmailSettings:Port"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            if (smtpServer == null || portString == null || senderEmail == null || senderPassword == null)
            {
                throw new InvalidOperationException("Email settings are not configured properly.");
            }

            var port = int.Parse(portString);

            // Set up SMTP client
            SmtpClient client = new SmtpClient(smtpServer, port);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Set up email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(senderEmail);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message;
            try
            {
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<bool> SaveEmailVerificationToken(string email, string token)
        {
            var user = await repository.FindAsync(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var existingEmailVerification = await emailVerificationRepository.FindAsync(e => e.Id == user.Id);
            if(existingEmailVerification != null)
            {
                await emailVerificationRepository.DeleteAsync(existingEmailVerification.Id);
            }
            var emailVerification = new EmailVerification
            {
                Id = user.Id,
                Email = email,
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddHours(24)
            };
            await emailVerificationRepository.AddAsync(emailVerification);
            return true;
        }

        public async Task<bool> SentEmailForVerify(string toEmail)
        {
            var token = Guid.NewGuid().ToString();
            string subject = "Verify your email";
            string message = $@"
            <html>
            <body>
                <p>Hi there,</p>
                <p>Thank you for registering with us. Please click the button below to verify your email address.</p>
                <a href='http://localhost:7116/api/auth/verify-email?email={toEmail}&token={token}' style='background-color: #4CAF50; color: white; padding: 12px 24px; text-decoration: none; font-size: 16px; border-radius: 5px;'>Verify Email</a>
                <p>If you didn't sign up for this account, please ignore this email.</p>
            </body>
            </html>";
            var response = await SendEmailAsync(toEmail, subject, message);
            if (response)
            {
                // Save token in database
                await SaveEmailVerificationToken(toEmail, token);
                return true;
            }
            throw new Exception("Failed to send email");
        }

        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            var emailVerification = await emailVerificationRepository.FindAsync(e => e.Email == email && e.Token == token);
            if (emailVerification == null)
            {
                throw new Exception("Invalid token");
            }
            if (emailVerification.ExpiryTime < DateTime.UtcNow)
            {
                throw new Exception("Token expired");
            }
            var user = await repository.FindByIdAsync(emailVerification.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.IsEmailVerified = true;
            await repository.UpdateAsync(user);
            await emailVerificationRepository.DeleteAsync(emailVerification.Id);
            return true;
        }
    }
}
