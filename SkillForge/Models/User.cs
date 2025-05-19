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
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password hash cannot exceed 100 characters")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Salt is required")]
        [StringLength(50, ErrorMessage = "Salt cannot exceed 50 characters")]
        public string Salt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? LastLoginAt { get; set; }

        [StringLength(50)]
        public string ResetPasswordToken { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ResetPasswordTokenExpiry { get; set; }
    }
} 