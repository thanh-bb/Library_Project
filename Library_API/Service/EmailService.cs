﻿using Library_API.Helpter;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace Library_API.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }
        public async Task SendEmailAsync(Mailrequest mailrequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();


            byte[] fileBytes;
            if (System.IO.File.Exists("Attachment/QuyDinhThuVien.pdf"))
            {
                FileStream file = new FileStream("Attachment/QuyDinhThuVien.pdf", FileMode.Open, FileAccess.Read);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                builder.Attachments.Add("QuyDinhThuVien.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
                //builder.Attachments.Add("attachment2.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            }

            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
