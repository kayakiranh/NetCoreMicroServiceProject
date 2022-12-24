using MP.Core.Application.Repositories;
using MP.Core.Domain.Enums;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using MP.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MP.Infrastructure.Mailer
{
    /// <summary>
    /// Mail repository
    /// </summary>
    [Serializable]
    public class MailerRepository : IMailerRepository
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly IConfiguration _configuration;

        public MailerRepository(ILoggerRepository loggerRepository, IConfiguration configuration)
        {
            _loggerRepository = loggerRepository;
            _configuration = configuration;
        }

        public bool SendMail(string subject, string content, string email, string fullName)
        {
            if (
                subject.StringSecurityValidation() == "" ||
                content.StringSecurityValidation() == "" ||
                email.MailValidation() == "" ||
                fullName.StringSecurityValidation() == "" )
            {
                _loggerRepository.Insert(LogTypes.Warning, "MailerRepository Error");
                return false;
            }

            try
            {
                MailAddress from = new(_configuration.GetSection("EmailSettings:FromAddress").Value, _configuration.GetSection("EmailSettings:FromName").Value);
                MailAddress to = new(email);

                SmtpClient smtpClient = new()
                {
                    Host = _configuration.GetSection("EmailSettings:Host").Value,
                    Port = Convert.ToInt32(_configuration.GetSection("EmailSettings:Port").Value),
                    EnableSsl = Convert.ToBoolean(_configuration.GetSection("EmailSettings:EnableSsl").Value),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(_configuration.GetSection("EmailSettings:UseDefaultCredentials").Value),
                    Credentials = new NetworkCredential(from.Address, _configuration.GetSection("EmailSettings:Password").Value)
                };
                using (MailMessage mailMessage = new(from, to)
                {
                    Subject = subject,
                    Body = GenerateMailBody(content)
                })
                {
                    smtpClient.Send(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                _loggerRepository.Insert(LogTypes.Critical, "MailerRepository Error", ex, subject, subject, content, email, fullName);
                return false;
            }
        }

        public void SendToAdmin(EmailTemplates emailTemplate, object model)
        {
            switch (emailTemplate)
            {
                case EmailTemplates.FinancialApiError:
                    SendMail("Financial Api Error", JsonConvert.SerializeObject(model), _configuration.GetSection("EmailSettings:AdminMailAddress").Value, _configuration.GetSection("EmailSettings:AdminFullName").Value);
                    break;
                default:
                    break;
            }

        }

        private static string GenerateMailBody(string content)
        {
            StringBuilder stringBuilder = new();
            string header = @"<head>
                                <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
                                <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
                                <title>MicroService Project</title>
                                <style>
                                 body,table td{font-size:14px}.body,body{background-color:#f6f6f6}.container,.content{display:block;max-width:580px;padding:10px}body,h1,h2,h3,h4{line-height:1.4;font-family:sans-serif}body,h1,h2,h3,h4,ol,p,table td,ul{font-family:sans-serif}.btn a,.btn table td{background-color:#fff}.btn,.btn a,.content,.wrapper{box-sizing:border-box}.btn a,h1{text-transform:capitalize}.align-center,.btn table td,.footer,h1{text-align:center}.clear,.footer{clear:both}.btn a,.powered-by a{text-decoration:none}img{border:none;-ms-interpolation-mode:bicubic;max-width:100%}body{-webkit-font-smoothing:antialiased;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}table{border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%}table td{vertical-align:top}.body{width:100%}.container{margin:0 auto!important;width:580px}.btn,.footer,.main{width:100%}.content{margin:0 auto}.main{background:#fff;border-radius:3px}.wrapper{padding:20px}.content-block{padding-bottom:10px;padding-top:10px}.footer{margin-top:10px}.footer a,.footer p,.footer span,.footer td{color:#999;font-size:12px;text-align:center}h1,h2,h3,h4{color:#000;font-weight:400;margin:0 0 30px}.btn a,a{color:#3498db}h1{font-size:35px;font-weight:300}.btn a,ol,p,ul{font-size:14px}ol,p,ul{font-weight:400;margin:0 0 15px}ol li,p li,ul li{list-style-position:inside;margin-left:5px}a{text-decoration:underline}.btn>tbody>tr>td{padding-bottom:15px}.btn table{width:auto}.btn table td{border-radius:5px}.btn a{border:1px solid #3498db;border-radius:5px;cursor:pointer;display:inline-block;font-weight:700;margin:0;padding:12px 25px}.btn-primary a,.btn-primary table td{background-color:#3498db}.btn-primary a{border-color:#3498db;color:#fff}.last,.mb0{margin-bottom:0}.first,.mt0{margin-top:0}.align-right{text-align:right}.align-left{text-align:left}.preheader{color:transparent;display:none;height:0;max-height:0;max-width:0;opacity:0;overflow:hidden;mso-hide:all;visibility:hidden;width:0}hr{border:0;border-bottom:1px solid #f6f6f6;margin:20px 0}@media only screen and (max-width:620px){table.body h1{font-size:28px!important;margin-bottom:10px!important}table.body a,table.body ol,table.body p,table.body span,table.body td,table.body ul{font-size:16px!important}table.body .article,table.body .wrapper{padding:10px!important}table.body .content{padding:0!important}table.body .container{padding:0!important;width:100%!important}table.body .main{border-left-width:0!important;border-radius:0!important;border-right-width:0!important}table.body .btn a,table.body .btn table{width:100%!important}table.body .img-responsive{height:auto!important;max-width:100%!important;width:auto!important}}@media all{.btn-primary a:hover,.btn-primary table td:hover{background-color:#34495e!important}.ExternalClass{width:100%}.ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td{line-height:100%}.apple-link a{color:inherit!important;font-family:inherit!important;font-size:inherit!important;font-weight:inherit!important;line-height:inherit!important;text-decoration:none!important}#MessageViewBody a{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}.btn-primary a:hover{border-color:#34495e!important}}            
                                </style>
                              </head>";
            stringBuilder.Append(header);
            string body = @$"<!doctype html>
                            <html>
                              
                              <body>
                                <span class='preheader'>{content}</span>
                                <table role='presentation' border='0' cellpadding='0' cellspacing='0' class='body'>
                                  <tr>
                                    <td>&nbsp;</td>
                                    <td class='container'>
                                      <div class='content'>
                                        <table role='presentation' class='main'>
                                          <tr>
                                            <td class='wrapper'>
                                              <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
                                                <tr>
                                                  <td>
                                                    {content}
                                                  </td>
                                                </tr>
                                              </table>
                                            </td>
                                          </tr>
                                        </table>
                                        footewr
                                      </div>
                                    </td>
                                    <td>&nbsp;</td>
                                  </tr>
                                </table>
                              </body>
                            </html>";
            stringBuilder.Append(body);
            string footer = @"<div class='footer'>
                                <table role='presentation' border='0' cellpadding='0' cellspacing='0'>
                                  <tr>
                                    <td class='content-block'>
                                      <span class='apple-link'>Company Inc, 3 Abbey Road, San Francisco CA 94102</span>
                                      <br> Don't like these emails? <a href='http://i.imgur.com/CScmqnj.gif'>Unsubscribe</a>.
                                    </td>
                                  </tr>
                                  <tr>
                                    <td class='content-block powered-by'>
                                      Powered by <a href='http://htmlemail.io'>HTMLemail</a>.
                                    </td>
                                  </tr>
                                </table>
                              </div>";
            stringBuilder.Append(footer);
            return stringBuilder.ToString();
        }
    }
}