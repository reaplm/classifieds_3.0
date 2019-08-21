using Classifieds.Service.Impl;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Service
{
    /// <summary>
    /// Service for sending emails to users
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        /// <summary>
        /// Email settings are defined in appsettings.json
        /// </summary>
        /// <param name="emailSettings"></param>
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">Recepient</param>
        /// <param name="subject">Email subject</param>
        /// <param name="message">Html message</param>
        /// <returns>Task</returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                //credentials
                var credentials = new NetworkCredential(emailSettings.Sender, 
                    emailSettings.Password);

                var mail = new MailMessage()
                {
                    From = new MailAddress(emailSettings.Sender, emailSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email));

                //smtp client
                var client = new SmtpClient
                {
                    Port = emailSettings.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = emailSettings.MailServer,
                    EnableSsl = emailSettings.EnableSsl,
                    Credentials = credentials,
                };

                await client.SendMailAsync(mail);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

        }
        
    }
}
