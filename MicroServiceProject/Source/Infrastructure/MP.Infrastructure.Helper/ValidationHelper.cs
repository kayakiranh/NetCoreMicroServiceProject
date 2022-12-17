using Ganss.Xss;
using System.Net.Mail;

namespace MP.Infrastructure.Helper
{
    /// <summary>
    /// Validation helpers
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Email validation helper
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>"email address or string.empty"</returns>
        public static string MailValidation(this string email)
        {
            if (email.StringSecurityValidation() == "") return string.Empty;

            try
            {
                MailAddress emailAddress = new(email);
                return emailAddress.Address;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// String validation and security
        /// </summary>
        /// <param name="str">string value</param>
        /// <returns>safe string or string.empty</returns>
        public static string StringSecurityValidation(this string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) return string.Empty;

            HtmlSanitizer htmlSanitizer = new();
            str = htmlSanitizer.Sanitize(str);
            return str;
        }
    }
}