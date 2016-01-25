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
            
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("YourMail@gmail.com");
            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = body;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            // Move this to config section
            client.Credentials = new NetworkCredential("mail mail", "XXXXX");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
            finally
            {
                msg.Dispose();
            }

        }
    }
}
