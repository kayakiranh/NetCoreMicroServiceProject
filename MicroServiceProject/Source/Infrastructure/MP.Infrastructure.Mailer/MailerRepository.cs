using MP.Core.Application.Repositories;
using MP.Core.Domain.Enums;
using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace MP.Infrastructure.Mailer
{
    public class MailerRepository : IMailerRepository
    {
        private readonly ILoggerRepository _loggerRepository;

        public MailerRepository(ILoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }

        public void SendMail(string subject, string message, string email)
        {
            try
            {
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["fromAddress"], ConfigurationManager.AppSettings["fromName"]);
                MailAddress to = new MailAddress(email);

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSsl"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["useDefaultCredentials"]),
                    Credentials = new NetworkCredential(from.Address, ConfigurationManager.AppSettings["password"])
                };
                using (MailMessage mailMessage = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = message
                })
                {
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex) 
            {
                _loggerRepository.Insert(LogTypes.Critical, "MailerRepository Error", ex);
            }
        }
    }
}