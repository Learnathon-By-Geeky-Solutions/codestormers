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
        private readonly IRepository<PasswordReset> passwordResetRepository;
        private readonly IRepository<EmailVerification> emailVerificationRepository;



        // Injecting IConfiguration and IRepository<User> and IRepository<EmailVerification> into the constructor
        public EmailService(IConfiguration Configuration, IRepository<User> repository, IRepository<EmailVerification> emailVerificationRepository, IRepository<PasswordReset> passwordResetRepository)
        {
            _configuration = Configuration;
            this.repository = repository;
            this.emailVerificationRepository = emailVerificationRepository;
            this.passwordResetRepository = passwordResetRepository;
        }


        /// <summary>
        /// Sends an email to the specified email address with the given subject and message.
        /// </summary>
        /// <param name="toEmail">The email address to send the email to</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="message">The message to send in the email</param>
        /// <returns>True if the email was sent successfully, or throws an exception if sending the email fails</returns>
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
        {
            // Read email settings from appsettings.json
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var portString = _configuration["EmailSettings:Port"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            // Check if email settings are configured properly
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
                // Send email
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send email");
            }
        }


        /// <summary>
        /// Saves the email verification token in the database for the specified email address.
        /// and deletes the existing token if it exists
        /// </summary>
        /// <param name="email">The email address to save the token for</param>
        /// <param name="token">The token to save</param>
        /// <returns>True if the token was saved successfully</returns>
        public async Task<bool> SaveEmailVerificationTokenAsync(string email, string token)
        {
            // Find the user by email
            var user = await repository.FindAsync(u => u.Email == email);

            // Throw an exception if the user does not exist
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Find the existing email verification record
            var existingEmailVerification = await emailVerificationRepository.FindAsync(e => e.Id == user.Id);

            // Delete the existing email verification record if it exists
            if (existingEmailVerification != null)
            {
                await emailVerificationRepository.DeleteAsync(existingEmailVerification.Id);
            }

            // Create a new email verification record
            var emailVerification = new EmailVerification
            {
                Id = user.Id,
                Email = email,
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddHours(24)
            };

            // Save the email verification record in the database
            await emailVerificationRepository.AddAsync(emailVerification);

            return true;
        }


        /// <summary>
        /// Sends an email to the specified email address with a verification link.
        /// </summary>
        /// <param name="toEmail">The email address to send the email to</param>
        /// <returns>True if the email was sent successfully, or throws an exception if sending the email fails</returns>
        public async Task<bool> SentEmailForVerifyAsync(string toEmail)
        {
            // Generate a new token
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

            // Send email
            var response = await SendEmailAsync(toEmail, subject, message);

            // Save token in database if email was sent successfully
            if (response)
            {
                // Save token in database
                await SaveEmailVerificationTokenAsync(toEmail, token);
                return true;
            }

            // Throw an exception if sending the email fails
            throw new Exception("Failed to send email");
        }


        /// <summary>
        /// Verifies the email address for the specified email and token.
        /// </summary>
        /// <param name="email">The email address to verify</param>
        /// <param name="token">The token to verify</param>
        /// <returns>True if the email was verified successfully, or throws an exception if verification fails</returns>
        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            // Find the email verification record by email and token
            var emailVerification = await emailVerificationRepository.FindAsync(e => e.Email == email && e.Token == token);

            // Throw an exception if the token is invalid or expired
            if (emailVerification == null)
            {
                throw new Exception("Invalid token");
            }
            if (emailVerification.ExpiryTime < DateTime.UtcNow)
            {
                throw new Exception("Token expired");
            }

            // Find the user by email
            var user = await repository.FindByIdAsync(emailVerification.Id);

            // Throw an exception if the user does not exist
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Update the user's email verification status
            user.IsEmailVerified = true;

            // Save the updated user record
            await repository.UpdateAsync(user);

            // Delete the email verification record
            await emailVerificationRepository.DeleteAsync(emailVerification.Id);

            return true;
        }


        /// <summary>
        /// Sends an email to the specified email address with a verification Token.
        /// for password reset
        /// </summary>
        /// <param name="toEmail">The email address to send the email to</param>
        /// <returns>True if the email was sent successfully, or false if sending the email fails</returns>
        public async Task<bool> SentPasswordResetEmailAsync(string toEmail)
        {
            // Generate a new token
            var token = new Random().Next(1000000, 10000000);

            // Email subject and message
            string subject = "Your single-use code";

            string message = $@"
                <h1>Your single-use code is:</h1>
                <h2>{token}</h2>
                <p>This code will expire in 10 minute.</p>";

            // Send the email verification email
            if (await SendEmailAsync(toEmail, subject, message))
            {
                // Find user by email
                var user = await repository.FindAsync(e => e.Email == toEmail);

                // If user not found then return false
                if (user is null)
                {
                    return false;
                }

                var isEmailExist = await passwordResetRepository.FindAsync(e => e.Id == user.Id);

                // If email does not exist in password reset table then add it
                if (isEmailExist is null)
                {
                    PasswordReset passwordResetData = new PasswordReset
                    {
                        Id = user.Id,
                        Email = toEmail,
                        Token = token,
                        ExpiryDate = DateTime.UtcNow.AddMinutes(10)
                    };
                    await passwordResetRepository.AddAsync(passwordResetData);
                }
                else
                {
                    // If email exist in password reset table then update the token and expiry date
                    isEmailExist.Token = token;
                    isEmailExist.ExpiryDate = DateTime.UtcNow.AddMinutes(10);
                    await passwordResetRepository.UpdateAsync(isEmailExist);
                }

                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
