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
        private int port;
        private string host;
        private string login;
        private string password;

        public MailService()
        {
            port = 587;
            host = "smtp.gmail.com";
            login = "a.yushchenk@gmail.com";
            password = "rhjn1234rn32ahxg";
        }

        public void SendComplexMessage(string To, string Subject, string Body, params string[] Path)
        {
            SmtpClient client = new SmtpClient(this.host, this.port);
            client.Credentials = new NetworkCredential(this.login, this.password);
            client.EnableSsl = true;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(this.login);
            msg.To.Add(new MailAddress(To));
            msg.Subject = Subject;
            msg.Body = Body;
            foreach (var str in Path)
                if (!String.IsNullOrWhiteSpace(str))
                    msg.Attachments.Add(new Attachment(str, MediaTypeNames.Application.Octet));

            client.Send(msg);

            foreach (var i in msg.Attachments)
                i.Dispose();
        }
    }
}
