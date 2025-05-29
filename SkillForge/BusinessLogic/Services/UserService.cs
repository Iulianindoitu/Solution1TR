using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SkillForge.Models;
using SkillForge.DAL;
using SkillForge.BusinessLogic.Interfaces;
using System.Data.Entity;
using Microsoft.Extensions.Options;

namespace SkillForge.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IOptions<IdentityConfig> _identityConfig;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, 
            IOptions<IdentityConfig> identityConfig)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _identityConfig = identityConfig ?? throw new ArgumentNullException(nameof(identityConfig));
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to retrieve users", ex);
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("User ID cannot be null or empty", nameof(id));

            try
            {
                return await _userManager.FindByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to retrieve user with ID: {id}", ex);
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            try
            {
                return await _userManager.GetRolesAsync(userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to retrieve roles for user: {userId}", ex);
            }
        }

        public async Task<bool> UpdateUserRolesAsync(string userId, string[] selectedRoles)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                var currentRoles = await _userManager.GetRolesAsync(userId);
                var removeResult = await _userManager.RemoveFromRolesAsync(userId, currentRoles.ToArray());
                
                if (!removeResult.Succeeded)
                    return false;

                if (selectedRoles != null && selectedRoles.Any())
                {
                    var addResult = await _userManager.AddToRolesAsync(userId, selectedRoles);
                    return addResult.Succeeded;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to update roles for user: {userId}", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to delete user: {userId}", ex);
            }
        }

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            try
            {
                var result = await _userManager.CreateAsync(user, password);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create user", ex);
            }
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to update user: {user.Id}", ex);
            }
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            if (string.IsNullOrEmpty(role))
                throw new ArgumentException("Role cannot be null or empty", nameof(role));

            try
            {
                return await _userManager.IsInRoleAsync(userId, role);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to check role for user: {userId}", ex);
            }
        }
    }
}
