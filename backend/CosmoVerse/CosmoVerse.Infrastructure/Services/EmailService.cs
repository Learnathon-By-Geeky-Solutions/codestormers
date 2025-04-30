using System.Net.Mail;
using System.Net;
using CosmoVerse.Domain.Entities;
using CosmoVerse.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace CosmoVerse.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRepository<User, Guid> _repository;
        private readonly IRepository<PasswordReset, Guid> _passwordResetRepository;
        private readonly IRepository<EmailVerification, Guid> _emailVerificationRepository;



        // Injecting IConfiguration and IRepository<User> and IRepository<EmailVerification> into the constructor
        public EmailService(IRepository<User, Guid> _repository, IRepository<EmailVerification, Guid> _emailVerificationRepository, IRepository<PasswordReset, Guid> _passwordResetRepository)
        {
            this._repository = _repository;
            this._emailVerificationRepository = _emailVerificationRepository;
            this._passwordResetRepository = _passwordResetRepository;
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
            var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
            var portString = Environment.GetEnvironmentVariable("PORT");
            var senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
            var senderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD");

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
            catch (SmtpException ex)
            {
                throw new InvalidOperationException("Failed to send email due to SMTP error", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Failed to send email due to invalid operation", ex);
            }
        }


        /// <summary>
        /// Saves the email verification token in the database for the specified email address.
        /// and deletes the existing token if it exists
        /// </summary>
        /// <param name="user">The user to save the token for</param>
        /// <param name="token">The token to save</param>
        /// <returns>True if the token was saved successfully</returns>
        public async Task<bool> SaveEmailVerificationTokenAsync(User user, string token)
        {
            // Throw an exception if the user does not exist
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Find the existing email verification record
            var existingEmailVerification = user.EmailVerification;

            // Delete the existing email verification record if it exists
            if (existingEmailVerification != null)
            {
                await _emailVerificationRepository.DeleteAsync(existingEmailVerification.Id);
            }

            // Create a new email verification record
            var emailVerification = new EmailVerification
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Email = user.Email,
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddHours(24)
            };

            // Save the email verification record in the database
            await _emailVerificationRepository.AddAsync(emailVerification);

            // Update the user record
            await _repository.UpdateAsync(user);

            return true;
        }


        /// <summary>
        /// Sends an email to the specified email address with a verification link.
        /// </summary>
        /// <param name="user">The user to send the email to</param>
        /// <returns>True if the email was sent successfully, or throws an exception if sending the email fails</returns>
        public async Task<bool> SendEmailForVerifyAsync(User user)
        {
            // Generate a new token
            var token = Guid.NewGuid().ToString();
            string subject = "Verify your email";
            string message = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
            <p>Hi there,</p>
            <p>Thank you for registering with us. Please click the button below to verify your email address:</p>
            <a href='{Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")}/verification-page?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}' 
            style='display: inline-block; background-color: #4CAF50; color: white; padding: 12px 24px; text-decoration: none; font-size: 16px; border-radius: 5px; margin: 10px 0;'>
            Verify Email
            </a>
            <p>If you didn't sign up for this account, please ignore this email.</p>
            <p>Regards,<br/>CosmoVerse</p>
            </body>
            </html>";


            // Send email
            var response = await SendEmailAsync(user.Email, subject, message);

            // Save token in database if email was sent successfully
            if (response)
            {
                // Save token in database
                await SaveEmailVerificationTokenAsync(user, token);
                return true;
            }

            // Throw an exception if sending the email fails
            throw new InvalidOperationException("Failed to send email");
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
            var emailVerification = await _emailVerificationRepository.FindAsync(e => e.Email == email && e.Token == token);

            // Throw an exception if the token is invalid or expired
            if (emailVerification == null)
            {
                throw new InvalidOperationException("Invalid token");
            }
            if (emailVerification.ExpiryTime < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Token expired");
            }

            // Find the user by email
            var user = await _repository.FindByIdAsync(emailVerification.UserId);

            // Throw an exception if the user does not exist
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Update the user's email verification status
            user.IsEmailVerified = true;

            // Save the updated user record
            await _repository.UpdateAsync(user);

            // Delete the email verification record
            await _emailVerificationRepository.DeleteAsync(emailVerification);

            return true;
        }


        /// <summary>
        /// Sends an email to the specified email address with a verification Token.
        /// for password reset
        /// </summary>
        /// <param name="toEmail">The email address to send the email to</param>
        /// <returns>True if the email was sent successfully, or false if sending the email fails</returns>
        public async Task<bool> SendPasswordResetEmailAsync(string toEmail)
        {
            // Generate a new token
            var token = GenerateToken(6);

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
                var user = await _repository.FindAsync(e => e.Email == toEmail, u => u.PasswordReset);

                // If user not found then return false
                if (user is null)
                {
                    return false;
                }

                var isEmailExist = user.PasswordReset;

                // If email does not exist in password reset table then add it
                if (isEmailExist is null)
                {
                    PasswordReset passwordResetData = new PasswordReset
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Email = toEmail,
                        Token = token,
                        ExpiryDate = DateTime.UtcNow.AddMinutes(10)
                    };
                    await _passwordResetRepository.AddAsync(passwordResetData);

                    // Update the user record
                    await _repository.UpdateAsync(user);
                }
                else
                {
                    // If email exist in password reset table then update the token and expiry date
                    isEmailExist.Token = token;
                    isEmailExist.ExpiryDate = DateTime.UtcNow.AddMinutes(10);
                    await _passwordResetRepository.UpdateAsync(isEmailExist);
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        
        private static string GenerateToken(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] token = new char[length];
            byte[] randomBytes = new byte[length];

            // Use RandomNumberGenerator for cryptographic security
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            for (int i = 0; i < length; i++)
            {
                token[i] = validChars[randomBytes[i] % validChars.Length];
            }

            return new string(token);
        }

    }
}
