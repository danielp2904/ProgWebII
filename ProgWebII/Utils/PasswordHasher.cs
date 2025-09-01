using System;
using System.Security.Cryptography;
using System.Text;

namespace AcademicEvents.Utils
{
    public static class PasswordHasher
    {
        public static void CreatePassword(string email, string password, out string hash, out string salt)
        {
            salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            hash = Hash(email, password, salt);
        }

        public static bool Verify(string email, string password, string salt, string expectedHash)
            => Hash(email, password, salt) == expectedHash;

        private static string Hash(string email, string password, string salt)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes($"{email}:{password}:{salt}");
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
