using Ganss.Xss;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MP.Infrastructure.Helper
{
    /// <summary>
    /// Password encyrpt helper
    /// </summary>
    [Serializable]
    public static class PasswordHelper
    {
        private static string ConvertSha256(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            StringBuilder stringBuilder = new();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] result = hash.ComputeHash(encoding.GetBytes(str));

                foreach (byte b in result)
                    stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string Encrypt(this string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            password = password.Trim();

            HtmlSanitizer htmlSanitizer = new();
            password = htmlSanitizer.Sanitize(password);
            if (string.IsNullOrEmpty(password)) return string.Empty;

            const string saltValue = "MicroServiceProject_";
            string salt = ConvertSha256(saltValue);
            string pass = ConvertSha256(password);
            return ConvertSha256(salt + pass);
        }
    }
}