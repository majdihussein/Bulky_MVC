using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Bulky.Utility
{
    public class EmailSender : IEmailSender
    {
        // next 1.1 add these
        // public string SendGridSecret { get; set; }
        // public EmailSender(IConfiguration _config)
        // {
        //     SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        // }
        // next 1.2

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // logic to send email


            // next 1.2 // remove return down
            return Task.CompletedTask;
            // next 1.3 after remove 
            /*
                var client = new SendGridClient(SendGridSecret)
                var from = new EmailAddress("hello@dotnetmastery.com", "Bulky Book");
                var to = new EmailAddress(email);
                var message = MailHelper. CreateSingleEmail(from, to, subject, "", htmlMessage);
                return client.SendEmailAsync(message);
            */
            // next 1.3 opem CartControllers
        }
    }
}
