using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        [Index("IX_Users_Username", IsUnique = true)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Index("IX_Users_Email", IsUnique = true)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password hash cannot exceed 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Salt is required")]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "Salt must be exactly 32 characters")]
        [RegularExpression(@"^[A-Za-z0-9+/=]+$", ErrorMessage = "Invalid salt format")]
        public string Salt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? LastLoginAt { get; set; }

        [StringLength(64, MinimumLength = 64, ErrorMessage = "Reset token must be exactly 64 characters")]
        [RegularExpression(@"^[A-Za-z0-9+/=]+$", ErrorMessage = "Invalid reset token format")]
        public string ResetPasswordToken { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ResetPasswordTokenExpiry { get; set; }

        [Required]
        public int LoginAttempts { get; set; } = 0;

        [DataType(DataType.DateTime)]
        public DateTime? LockoutEnd { get; set; }
    }
} 