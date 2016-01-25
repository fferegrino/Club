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
            var myMessage = new SendGridMessage();
            myMessage.AddTo(to);
            myMessage.From = new System.Net.Mail.MailAddress(
                                "qwdqwd.qwdqwd@qwd84qw9d84qwd.com", "qwdqwd fqwfqweqwe");
            myMessage.Subject = subject;
            myMessage.Text = body;
            myMessage.Html = body;

            var credentials = new NetworkCredential(
                       "qdwqwdqw.swqsqw@4qwd49q48w9d4.es",
                       "654654wd6qw4d65qw");

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
