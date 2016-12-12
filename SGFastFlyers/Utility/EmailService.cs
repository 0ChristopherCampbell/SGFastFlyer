using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGFastFlyers.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SGFastFlyers.Utility
{
    public class EmailService
    {
        //Asynchronous method to send email based on ContactForm object 
        public async static Task SendContactForm(ContactModels contactForm)
        {
            //Contructing the email
            string body = "<p>The following is a message from {0} {1} ({2})</p><p>{3}</p>";
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress("contactus@sgfastflyers.com.au"));
            message.Subject = string.Format(contactForm.Subject);
            message.Body = string.Format(body, contactForm.FirstName, contactForm.LastName, contactForm.Email, contactForm.Comment);
            message.IsBodyHtml = true;

            //Attempting to send the email
            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.SendMailAsync(message);
            }
        }
    }
}