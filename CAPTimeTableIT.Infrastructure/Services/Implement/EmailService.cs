using CAPTimeTableIT.Infrastructure.Services.Abstract;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Services.Implement
{
    public class CapstoneEmailService : ICapstoneEmailService
    {
        private string _email;
        private string _password;
        private string _webFolderUrl;
        //public CapstoneEmailService()
        //{
        //}
        public CapstoneEmailService(string email, string password, string webFolderUrl)
        {
            _email = email;
            _password = password;
            _webFolderUrl = webFolderUrl;
        }

        public void SendHtmlMail(string to, string subject, string content)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Capstone Team 2401", _email));
            mailMessage.To.Add(new MailboxAddress(to, to));
            mailMessage.Subject = subject;
            var logoRelativePath = "res\\vendors\\images\\logo2.png";
            //var logoTeamRelativePath = "res\\vendors\\images\\team2401.jpg";
            var builder = new BodyBuilder();
            //Logo 
            var imageLogoPath = Path.Combine(_webFolderUrl, logoRelativePath);
            var image = builder.LinkedResources.Add(imageLogoPath);
            image.ContentId = "logo";
            ////team logo
            //var imageLogoTeamPath = Path.Combine(_webFolderUrl, logoTeamRelativePath);
            //var imageTeam = builder.LinkedResources.Add(imageLogoTeamPath);
            //imageTeam.ContentId = "logoTeam";
            builder.HtmlBody = content;
            mailMessage.Body = builder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp-mail.outlook.com", 587, false);
                smtpClient.Authenticate(_email, _password);
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }

        public void SendMail(string to, string subject, string content)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Capstone Team 2401", _email));
            mailMessage.To.Add(new MailboxAddress(to, to));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("plain")
            {
                Text = content
            };

            using (var smtpClient = new SmtpClient())
            {
                //smtpClient.ServerCertificateValidationCallback = (l, j, c, m) => true;
                smtpClient.Connect("smtp-mail.outlook.com", 587, false);
                smtpClient.Authenticate(_email, _password);
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
