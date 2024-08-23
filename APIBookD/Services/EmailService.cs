using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string token);
}

public class EmailService : IEmailService
{
    private readonly string _smtpServer = "smtp.example.com"; // Your SMTP server
    private readonly string _smtpUser = "your-email@example.com"; // Your SMTP username
    private readonly string _smtpPass = "your-password"; // Your SMTP password

    public async Task SendVerificationEmailAsync(string email, string token)
    {   
        var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = 587,
            Credentials = new NetworkCredential(_smtpUser, _smtpPass),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("noreply@example.com"),
            Subject = "Please verify your email address",
            Body = $"Please verify your email by clicking this link: https://yourapp.com/api/user/verify?token={token}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
