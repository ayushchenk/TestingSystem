using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MailSender
{
    public class MailService
    {
        //private int port;
        //private string host;
        //private string login;
        //private string password;

        //public MailService()
        //{
        //    port = 587;
        //    host = "smtp.gmail.com";
        //    login = "testing.system.yl@gmail.com";
        //    password = "testing.system.yl123";
        //}

        //public void SendComplexMessage(string To, string Subject, string Body, params string[] Path)
        //{
        //    try
        //    {
        //        SmtpClient client = new SmtpClient(this.host, this.port);
        //        client.Credentials = new NetworkCredential(this.login, this.password);
        //        client.EnableSsl = true;

        //        MailMessage msg = new MailMessage();
        //        msg.From = new MailAddress(this.login);
        //        msg.To.Add(new MailAddress(To));
        //        msg.Subject = Subject;
        //        msg.Body = Body;
        //        foreach (var str in Path)
        //            if (!String.IsNullOrWhiteSpace(str))
        //                msg.Attachments.Add(new Attachment(str, MediaTypeNames.Application.Octet));

        //        client.Send(msg);

        //        foreach (var i in msg.Attachments)
        //            i.Dispose();
        //    }
        //    catch { }
        //}

        //public Task SendComplexMessageAsync(string To, string Subject, string Body, params string[] Path)
        //{
        //    return Task.Run(() => SendComplexMessage(To, Subject, Body, Path));
        //}

        public async Task SendMessageAsync(string to, string subject, string body)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("testing.system.yl@gmail.com", "Testing System Application"),
                Subject = subject,
                PlainTextContent = body,
            };
            msg.AddTo(new EmailAddress(to));
            var response = await client.SendEmailAsync(msg);
        }
    }
}