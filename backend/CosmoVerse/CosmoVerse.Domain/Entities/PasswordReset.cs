﻿using System.ComponentModel.DataAnnotations;

namespace CosmoVerse.Domain.Entities
{
    public class PasswordReset
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }
    }
}
