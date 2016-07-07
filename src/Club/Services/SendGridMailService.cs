using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace Club.Services
{
    public class SendGridMailService : IMailService
    {

        public async Task<bool> SendMail(string to, string @from, string subject, string body)
        {
            var mailAccount = Startup.Configuration["Mail:SendGrid"].Split('|');

            var myMessage = new SendGridMessage();
            myMessage.From = new System.Net.Mail.MailAddress(mailAccount[0]);
            myMessage.AddTo(to);
            myMessage.Subject = subject;
            myMessage.Text = body;
            myMessage.Html = body;

            var credentials = new NetworkCredential(mailAccount[1],mailAccount[2]);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            try
            {
                await transportWeb.DeliverAsync(myMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return true;
        }
    }
}
