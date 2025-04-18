﻿using CosmoVerse.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        // User information
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; } = false;
        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Timestamps for account tracking
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public virtual EmailVerification EmailVerification { get; set; }

        // Navigation property
        public virtual PasswordReset PasswordReset { get; set; }

        // Profile picture
        public virtual ProfilePhoto ProfilePhoto { get; set; } = null!;
    }
}
