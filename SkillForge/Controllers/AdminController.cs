using System;
using System.Linq;
using System.Web.Mvc;
using SkillForge.Models;
using SkillForge.BusinessLogic.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Diagnostics;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace SkillForge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private static readonly string LogSource = "SkillForge.AdminController";

        public AdminController(IUserService userService, ICourseService courseService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
        }

        private void LogError(string action, Exception ex)
        {
            string errorMessage = $"Error in {action}: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $" Inner Exception: {ex.InnerException.Message}";
            }
            Debug.WriteLine(errorMessage);
            // TODO: Add proper logging implementation
        }

        private ActionResult HandleError(string action, Exception ex)
        {
            LogError(action, ex);
            ModelState.AddModelError("", "An error occurred. Please try again later.");
            return View("Error");
        }

        // GET: Admin/Dashboard
        public async Task<ActionResult> Dashboard()
        {
            try
            {
                var dashboardViewModel = new AdminDashboardViewModel
                {
                    TotalUsers = (await _userService.GetAllUsersAsync()).Count(),
                    TotalCourses = (await _courseService.GetAllCoursesAsync()).Count(),
                    RecentUsers = (await _userService.GetAllUsersAsync()).OrderByDescending(u => u.CreatedAt).Take(5).ToList(),
                    RecentCourses = (List<Course>)await _courseService.GetRecentCoursesAsync(5)
                };
                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                return HandleError("Dashboard", ex);
            }
        }

        // GET: Admin/Users
        public async Task<ActionResult> Users()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var userRoles = new Dictionary<string, IList<string>>();
                
                foreach (var user in users)
                {
                    userRoles[user.Id] = await _userService.GetUserRolesAsync(user.Id);
                }
                
                ViewBag.UserRoles = userRoles;
                return View(users);
            }
            catch (Exception ex)
            {
                return HandleError("Users", ex);
            }
        }

        // GET: Admin/Courses
        public async Task<ActionResult> Courses()
        {
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                return View(courses);
            }
            catch (Exception ex)
            {
                return HandleError("Courses", ex);
            }
        }

        // GET: Admin/UserDetails/5
        public async Task<ActionResult> UserDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound("User ID is required");
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound("User not found");
                }

                var userRoles = await _userService.GetUserRolesAsync(id);
                ViewBag.UserRoles = userRoles;
                ViewBag.AllRoles = new[] { "Admin", "Instructor", "Student" }; // Define available roles
                return View(user);
            }
            catch (Exception ex)
            {
                return HandleError("UserDetails", ex);
            }
        }

        // POST: Admin/UpdateUserRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUserRoles(string userId, string[] selectedRoles)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return HttpNotFound("User ID is required");
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return HttpNotFound("User not found");
                }

                var success = await _userService.UpdateUserRolesAsync(userId, selectedRoles);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update user roles.");
                    return View("Error");
                }

                return RedirectToAction("UserDetails", new { id = userId });
            }
            catch (Exception ex)
            {
                return HandleError("UpdateUserRoles", ex);
            }
        }

        // GET: Admin/CourseDetails/5
        public async Task<ActionResult> CourseDetails(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                if (course == null)
                {
                    return HttpNotFound("Course not found");
                }
                return View(course);
            }
            catch (Exception ex)
            {
                return HandleError("CourseDetails", ex);
            }
        }

        // POST: Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound("User ID is required");
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound("User not found");
                }

                var success = await _userService.DeleteUserAsync(id);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to delete user.");
                    return View("Error");
                }
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                return HandleError("DeleteUser", ex);
            }
        }

        // POST: Admin/DeleteCourse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                if (course == null)
                {
                    return HttpNotFound("Course not found");
                }

                var success = await _courseService.DeleteCourseAsync(id);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to delete course.");
                    return View("Error");
                }
                return RedirectToAction("Courses");
            }
            catch (Exception ex)
            {
                return HandleError("DeleteCourse", ex);
            }
        }

        // GET: Admin/CreateCourse
        public async Task<ActionResult> CreateCourse()
        {
            try
            {
                var allUsers = await _userService.GetAllUsersAsync();
                var instructors = new List<ApplicationUser>();
                
                foreach (var user in allUsers)
                {
                    var isInstructor = await _userService.IsUserInRoleAsync(user.Id, "Instructor");
                    if (isInstructor)
                    {
                        instructors.Add(user);
                    }
                }

                ViewBag.Instructors = new SelectList(instructors, "Id", "UserName");
                return View(new Course());
            }
            catch (Exception ex)
            {
                return HandleError("CreateCourse", ex);
            }
        }

        // POST: Admin/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                var allUsers = await _userService.GetAllUsersAsync();
                var instructors = new List<ApplicationUser>();
                
                foreach (var user in allUsers)
                {
                    var isInstructor = await _userService.IsUserInRoleAsync(user.Id, "Instructor");
                    if (isInstructor)
                    {
                        instructors.Add(user);
                    }
                }
                
                ViewBag.Instructors = new SelectList(instructors, "Id", "UserName");
                return View(course);
            }

            try
            {
                var success = await _courseService.CreateCourseAsync(course);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to create course.");
                    return View(course);
                }

                return RedirectToAction("Courses");
            }
            catch (Exception ex)
            {
                return HandleError("CreateCourse", ex);
            }
        }

        // GET: Admin/EditCourse/5
        public async Task<ActionResult> EditCourse(int id)
        {
            try
            {
                var course = await _courseService.GetCourseByIdAsync(id);
                if (course == null)
                {
                    return HttpNotFound("Course not found");
                }

                var allUsers = await _userService.GetAllUsersAsync();
                var instructors = new List<ApplicationUser>();
                
                foreach (var user in allUsers)
                {
                    var isInstructor = await _userService.IsUserInRoleAsync(user.Id, "Instructor");
                    if (isInstructor)
                    {
                        instructors.Add(user);
                    }
                }
                
                ViewBag.Instructors = new SelectList(instructors, "Id", "UserName");
                return View(course);
            }
            catch (Exception ex)
            {
                return HandleError("EditCourse", ex);
            }
        }

        // POST: Admin/EditCourse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                var allUsers = await _userService.GetAllUsersAsync();
                var instructors = new List<ApplicationUser>();
                
                foreach (var user in allUsers)
                {
                    var isInstructor = await _userService.IsUserInRoleAsync(user.Id, "Instructor");
                    if (isInstructor)
                    {
                        instructors.Add(user);
                    }
                }
                
                ViewBag.Instructors = new SelectList(instructors, "Id", "UserName");
                return View(course);
            }

            try
            {
                var success = await _courseService.UpdateCourseAsync(course);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update course.");
                    return View(course);
                }

                return RedirectToAction("Courses");
            }
            catch (Exception ex)
            {
                return HandleError("EditCourse", ex);
            }
        }
    }
} 