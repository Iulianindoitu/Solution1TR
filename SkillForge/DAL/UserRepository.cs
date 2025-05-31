using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SkillForge.Models;

namespace SkillForge.DAL
{
    public class UserRepository : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;

        public UserRepository()
        {
            _context = new ApplicationDbContext();
        }

          public User GetUserByEmailOrUsername(string emailOrUsername)
          {
               var appUser = _context.Users
                   .FirstOrDefault(u => u.Email == emailOrUsername || u.UserName == emailOrUsername);

               if (appUser == null)
                    return null;

               // Map ApplicationUser to User
               return new User
               {
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    IsActive = appUser.IsActive,
                    CreatedAt = appUser.CreatedAt,
                    // The following properties are not available in ApplicationUser, so set to default/null
                    PasswordHash = null,
                    Salt = null,
                    LastLoginAt = null,
                    ResetPasswordToken = null,
                    ResetPasswordTokenExpiry = null,   
                    LoginAttempts = 0,
                    LockoutEnd = null
               };
          }

        public void RegisterUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.PasswordHash))
                throw new ArgumentException("Password cannot be empty", nameof(user));

            // Generate a random salt
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            user.Salt = Convert.ToBase64String(saltBytes);

            // Hash the password with the salt
            user.PasswordHash = HashPassword(user.PasswordHash, user.Salt);

            // Map User to ApplicationUser
            var applicationUser = new ApplicationUser
            {
                UserName = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            _context.Users.Add(applicationUser);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null)
                throw new ArgumentException("User not found", nameof(user));

            // Update only the fields that exist in ApplicationUser
            existingUser.IsActive = user.IsActive;
            existingUser.UserName = user.Username;
            existingUser.Email = user.Email;

            _context.SaveChanges();
        }

        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(salt))
                return false;

            string computedHash = HashPassword(password, salt);
            return computedHash == storedHash;
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var bytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
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
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
} 