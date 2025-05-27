using System.Collections.Generic;
using System.Threading.Tasks;
using SkillForge.Models;

namespace SkillForge.BusinessLogic.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<bool> CreateCourseAsync(Course course);
        Task<bool> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int id);
        Task<IEnumerable<Course>> GetRecentCoursesAsync(int count);
    }
} 