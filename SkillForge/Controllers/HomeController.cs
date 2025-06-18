using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkillForge.Models;
using SkillForge.BusinessLogic.Interfaces;
using SkillForge.DAL;

namespace SkillForge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ApplicationDbContext _dbContext;

        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
            _courseService = new BusinessLogic.Services.CourseService(_dbContext);
        }

        public HomeController(ICourseService courseService, ApplicationDbContext dbContext)
        {
            _courseService = courseService;
            _dbContext = dbContext;
        }

        // GET: Home
        public ActionResult Index()
        {
            try
            {
                var viewModel = new AdminDashboardViewModel
                {
                    TotalCourses = _dbContext.Courses.Count(),
                    TotalUsers = _dbContext.Users.Count(),
                    RecentCourses = _dbContext.Courses.OrderByDescending(c => c.CreatedAt).Take(5).ToList(),
                    FeaturedCourses = _dbContext.Courses.Where(c => c.IsActive).OrderByDescending(c => c.CreatedAt).Take(3).ToList()
                };

                // Get cart information
                var cartItems = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
                viewModel.TotalCartItems = cartItems.Sum(i => i.Quantity);
                viewModel.TotalCartValue = cartItems.Sum(i => i.Course.Price * i.Quantity);
                viewModel.RecentCartItems = cartItems.OrderByDescending(i => i.AddedAt).Take(5).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error in Home/Index: {ex.Message}");
                
                // Return a view with empty data
                return View(new AdminDashboardViewModel
                {
                    TotalCourses = 0,
                    TotalUsers = 0,
                    RecentCourses = new List<Course>(),
                    FeaturedCourses = new List<Course>(),
                    TotalCartItems = 0,
                    TotalCartValue = 0,
                    RecentCartItems = new List<CartItem>()
                });
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult NewCourses()
        {
            try
            {
                var courses = _courseService.GetAllCoursesAsync().Result;
                System.Diagnostics.Debug.WriteLine($"Retrieved {(courses != null ? courses.Count() : 0)} courses from database");
                return View(courses);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error in Home/NewCourses: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return View(new List<Course>());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext?.Dispose();
                if (_courseService is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}