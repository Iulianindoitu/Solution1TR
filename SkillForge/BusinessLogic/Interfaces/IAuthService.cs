using System.Threading.Tasks;
using SkillForge.Models;

namespace SkillForge.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateUserAsync(string email, string password);
        Task<bool> RegisterUserAsync(ApplicationUser user, string password);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> IsUserInRoleAsync(string userId, string role);
    }
} 