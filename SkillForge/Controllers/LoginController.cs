using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkillForge.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Auth()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            // Add your login logic here
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register(string username, string email, string password, string confirmPassword)
        {
            // Add your registration logic here
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            // Add your forgot password logic here
            return RedirectToAction("Login", "Login");
        }
    }
}