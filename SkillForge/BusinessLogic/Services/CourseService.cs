using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SkillForge.Models;
using SkillForge.DAL;
using SkillForge.BusinessLogic.Interfaces;
using System.Data.Entity;

namespace SkillForge.BusinessLogic.Services
{
    public class CourseService : ICourseService, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed;

        public CourseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            try
            {
                return await _dbContext.Courses
                    .Include(c => c.Instructor)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Failed to retrieve courses", ex);
            }
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Courses
                    .Include(c => c.Instructor)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException($"Failed to retrieve course with ID: {id}", ex);
            }
        }

        public async Task<bool> CreateCourseAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (string.IsNullOrEmpty(course.Title))
                throw new ArgumentException("Course title cannot be null or empty", nameof(course));

            if (string.IsNullOrEmpty(course.InstructorId))
                throw new ArgumentException("Instructor ID cannot be null or empty", nameof(course));

            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        course.CreatedAt = DateTime.UtcNow;
                        _dbContext.Courses.Add(course);
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException("Failed to create course", ex);
            }
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (string.IsNullOrEmpty(course.Title))
                throw new ArgumentException("Course title cannot be null or empty", nameof(course));

            if (string.IsNullOrEmpty(course.InstructorId))
                throw new ArgumentException("Instructor ID cannot be null or empty", nameof(course));

            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var existingCourse = await _dbContext.Courses.FindAsync(course.Id);
                        if (existingCourse == null)
                            return false;

                        _dbContext.Entry(existingCourse).CurrentValues.SetValues(course);
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException($"Failed to update course: {course.Id}", ex);
            }
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var course = await _dbContext.Courses.FindAsync(id);
                        if (course == null)
                            return false;

                        _dbContext.Courses.Remove(course);
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException($"Failed to delete course: {id}", ex);
            }
        }

        public async Task<IEnumerable<Course>> GetRecentCoursesAsync(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero", nameof(count));

            try
            {
                return await _dbContext.Courses
                    .Include(c => c.Instructor)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new ApplicationException($"Failed to retrieve recent courses", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }
                _disposed = true;
            }
        }
    }
} 