using MP.Core.Domain.Enums;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Mailer repository
    /// </summary>
    public interface IMailerRepository
    {
        /// <summary>
        /// Mail send method.
        /// If mail could not send, logger write critical error.
        /// </summary>
        /// <param name="subject">Mail subject : "Credit Card Apply Status"</param>
        /// <param name="content">Mail content : "Dear Hüseyin, your American Express apply could not accepted. Regards."</param>
        /// <param name="email">To email address : "kayakiranh@gmail.com"</param>
        /// <param name="fullName">To name : "Hüseyin Kayakıran"</param>
        bool SendMail(string subject, string content, string email, string fullName);

        /// <summary>
        /// System mails
        /// </summary>
        /// <param name="emailTemplate">EmailTemplates</param>
        /// <param name="model">Exception</param>
        void SendToAdmin(EmailTemplates emailTemplate, object model);
    }
}