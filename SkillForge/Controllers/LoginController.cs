using System.Web.Mvc;
using SkillForge.Models;
using System.Security.Cryptography;
using System.Text;
using SkillForge.DAL;


namespace SkillForge.Controllers
{
    public class LoginController : Controller
    {
        private UserRepository userRepository = new UserRepository();

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
            var user = userRepository.GetUserByEmailOrUsername(email);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                // TODO: Set authentication cookie/session here
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("Auth");
            }

            var existingUser = userRepository.GetUserByEmailOrUsername(email) ?? userRepository.GetUserByEmailOrUsername(username);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Username or email already exists.");
                return View("Auth");
            }

            var passwordHash = HashPassword(password);
            var user = new User { Username = username, Email = email, PasswordHash = passwordHash };
            userRepository.RegisterUser(user);
            // Optionally, log the user in automatically here
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            // Add your forgot password logic here
            return RedirectToAction("Login", "Login");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return System.Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
