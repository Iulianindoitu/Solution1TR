using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SkillForge.Models;
using SkillForge.DAL;
using System.Data.Entity;

namespace SkillForge.BusinessLogic.Services
{
     public class IdentityConfig
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityConfig(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Create default roles if they don't exist
                string[] roles = { "Admin", "Instructor", "Student" };
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create default admin user if it doesn't exist
                var adminEmail = "admin@skillforge.com";
                var adminUser = await _userManager.FindByEmailAsync(adminEmail);
                
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await _userManager.CreateAsync(adminUser, "Admin123!");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(adminUser.Id, "Admin");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Failed to initialize identity configuration", ex);
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetInstructorsAsync()
        {
            try
            {
                var allUsers = await _userManager.Users.ToListAsync();
                var instructors = new List<ApplicationUser>();

                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user.Id, "Instructor"))
                    {
                        instructors.Add(user);
                    }
                }

                return instructors;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Failed to retrieve instructors", ex);
            }
        }

        public void Dispose()
        {
            _userManager?.Dispose();
            _roleManager?.Dispose();
        }
    }
}