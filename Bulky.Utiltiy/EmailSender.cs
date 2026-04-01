using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utiltiy
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }

        public EmailSender(IConfiguration _config)
        {
            SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //logic to send email

            var client = new SendGridClient(SendGridSecret);

            var from = new EmailAddress("mohamedshoaib488@gmail.com", "Bulky Book");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            return client.SendEmailAsync(message);

        }

        //public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    var client = new SendGridClient(SendGridSecret);

        //    var from = new EmailAddress("mohamedshoaib488@gmail.com", "Bulky Book");
        //    var to = new EmailAddress(email);
        //    var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

        //    var response = await client.SendEmailAsync(message);

        //    var body = await response.Body.ReadAsStringAsync();

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"SendGrid Error: {response.StatusCode} - {body}");
        //    }
        
    }
}
