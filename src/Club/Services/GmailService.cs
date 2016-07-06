using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Club.Services
{
    public class GmailService : IMailService
    {
        public Task<bool> SendMail(string to, string from, string subject, string body)
        {

            var mailAccount = Startup.Configuration["Mail:Gmail"].Split('|');
            System.Diagnostics.Debug.WriteLine(Startup.Configuration["Mail:Gmail"]);
            MailMessage msg = new MailMessage
            {
                From = new MailAddress(mailAccount[0])
            };

            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient
            {
                UseDefaultCredentials = true,
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(mailAccount[0], mailAccount[1]),
                Timeout = 20000
            };

            try
            {
                client.Send(msg);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Task.FromResult(false);
            }
            finally
            {
                msg.Dispose();
            }

        }
    }
}
