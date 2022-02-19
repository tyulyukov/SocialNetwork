using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Helpers.Notify
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(String email, String subject, String text)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Gachigram", "makstyulyukov@ukr.net");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("", email);
            message.To.Add(to);

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = text;

            message.Subject = subject;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.ukr.net", 465, true);
                client.Authenticate("makstyulyukov@ukr.net", "w7Nl4jZmWTQxeS9S");

                client.Send(message);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
