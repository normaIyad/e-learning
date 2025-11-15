using System.Net;
using System.Net.Mail;

namespace Course.Bll.Service.GenralIService
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync (string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl=true,
                UseDefaultCredentials=false,
                Credentials=new NetworkCredential("normamohsan39@gmail.com", "jfbi mxiu mrhb odkh")
            };

            return client.SendMailAsync(
                new MailMessage(from: "your.email@live.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml=true });
        }
    }
}

