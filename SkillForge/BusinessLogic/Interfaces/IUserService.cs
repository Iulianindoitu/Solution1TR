using System.Collections.Generic;
using System.Threading.Tasks;
using SkillForge.Models;

namespace SkillForge.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<bool> UpdateUserRolesAsync(string userId, string[] selectedRoles);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> IsUserInRoleAsync(string userId, string role);
    }
} 