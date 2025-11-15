namespace Course.Bll.Service.GenralIService
{
    public interface IEmailSender
    {
        Task SendEmailAsync (string toEmail, string subject, string body);
    }
}
