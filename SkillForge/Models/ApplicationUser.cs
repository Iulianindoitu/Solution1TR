using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        // Add a computed property for full name
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
} 