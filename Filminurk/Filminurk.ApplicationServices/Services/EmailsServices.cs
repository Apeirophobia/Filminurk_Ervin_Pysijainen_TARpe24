using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Filminurk.ApplicationServices.Services
{
    public class EmailsServices : IEmailsServices
    {
        private readonly IConfiguration _configuration;
        private readonly FilminurkTARpe24Context _context;
        private readonly IEmailsServices _emailsServices;

        public EmailsServices(FilminurkTARpe24Context context, IEmailsServices emailsServices, IConfiguration configuration)
        {
            _context = context;
            _emailsServices = emailsServices;
            _configuration = configuration;
        }

        public void SendEmail(EmailDTO dto)
        {
            var email = new MimeMessage();
            _configuration.GetSection("EmailUserName").Value = "ervinpusijainen";
            _configuration.GetSection("EmailHost").Value = "smtp.gmail.com";
            _configuration.GetSection("EmailPassword").Value = "jimd icnu alwr vltb";

            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.SendToThisAddress));
            email.Subject = dto.EmailSubject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.EmailContent
            };

            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();

            smtp.Connect(_configuration.GetSection("EmailHost").Value = "", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
