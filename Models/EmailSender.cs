using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoGraphic.Models.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AutoGraphic.Models
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
    public class EmailSender: IEmailSender
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailSender(string senderEmail, string senderPassword)
        {
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Farah Yacoub", "f.y.abusiam@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlMessage };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("f.y.abusiam@gmail.com", "Farah-AbuSiam-2000");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
