using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SkillForge.Models;
using SkillForge.DAL;

namespace SkillForge.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;

        public LoginController()
        {
            _userRepository = new UserRepository();
        }

        // GET: Login
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Auth()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Please enter both email and password.");
                return View();
            }

            try
            {
                var user = _userRepository.GetUserByEmailOrUsername(email);
                if (user != null && _userRepository.VerifyPassword(password, user.PasswordHash, user.Salt))
                {
                    // Create authentication ticket
                    var ticket = new FormsAuthenticationTicket(
                        1,
                        user.Username,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                        user.Id.ToString()
                    );

                    // Encrypt the ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    // Create the cookie
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                    // Update last login time
                    user.LastLoginAt = DateTime.UtcNow;
                    _userRepository.UpdateUser(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View("Auth");
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("Auth");
            }

            try
            {
                var existingUser = _userRepository.GetUserByEmailOrUsername(email) ?? 
                                 _userRepository.GetUserByEmailOrUsername(username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username or email already exists.");
                    return View("Auth");
                }

                var user = new User 
                { 
                    Username = username, 
                    Email = email, 
                    PasswordHash = password // Will be hashed in repository
                };

                _userRepository.RegisterUser(user);

                // Auto-login after registration
                var ticket = new FormsAuthenticationTicket(
                    1,
                    user.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    user.Id.ToString()
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View("Auth");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Please enter your email address.");
                return View();
            }

            var user = _userRepository.GetUserByEmailOrUsername(email);
            if (user != null)
            {
                // TODO: Implement password reset email functionality
                TempData["SuccessMessage"] = "If an account exists with that email, you will receive password reset instructions.";
            }
            else
            {
                // Don't reveal that the user doesn't exist
                TempData["SuccessMessage"] = "If an account exists with that email, you will receive password reset instructions.";
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
