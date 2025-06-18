using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkillForge.Models;

namespace SkillForge.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalCartItems { get; set; }
        public decimal TotalCartValue { get; set; }
        public List<ApplicationUser> RecentUsers { get; set; }
        public List<Course> RecentCourses { get; set; }
        public List<CartItem> RecentCartItems { get; set; }
        public List<Course> FeaturedCourses { get; set; }
    }
} 