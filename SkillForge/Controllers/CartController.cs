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
    public class CartController : Controller
    {
        private readonly ICourseService _courseService;

        public CartController()
        {
            // Create a new instance of ApplicationDbContext and CourseService
            var dbContext = new ApplicationDbContext();
            _courseService = new BusinessLogic.Services.CourseService(dbContext);
        }

        public CartController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        private List<CartItem> GetCartItems()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // GET: Cart/GetCart
        [ChildActionOnly]
        public ActionResult GetCart()
        {
            var cartItems = GetCartItems();
            return PartialView("_Cart", cartItems);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public ActionResult AddToCart(int courseId)
        {
            try
            {
                var course = _courseService.GetCourseByIdAsync(courseId).Result;
                if (course == null)
                {
                    return Json(new { success = false, message = "Course not found" });
                }

                var cartItems = GetCartItems();
                var existingItem = cartItems.FirstOrDefault(i => i.Course.Id == courseId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cartItems.Add(new CartItem
                    {
                        Course = course,
                        Quantity = 1
                    });
                }

                Session["Cart"] = cartItems;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Cart/RemoveFromCart
        [HttpPost]
        public ActionResult RemoveFromCart(int courseId)
        {
            try
            {
                var cartItems = GetCartItems();
                var itemToRemove = cartItems.FirstOrDefault(i => i.Course.Id == courseId);

                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                    Session["Cart"] = cartItems;
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Cart/Checkout
        public ActionResult Checkout()
        {
            var cartItems = GetCartItems();
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(cartItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_courseService is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
} 