namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Mailer repository
    /// </summary>
    public interface IMailerRepository
    {
        void SendMail(string subject, string message, string email);
    }
}