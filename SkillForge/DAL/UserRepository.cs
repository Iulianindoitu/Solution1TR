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
            return _context.Users
                .FirstOrDefault(u => u.Email == emailOrUsername || u.Username == emailOrUsername);
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

            _context.Users.Add(user);
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