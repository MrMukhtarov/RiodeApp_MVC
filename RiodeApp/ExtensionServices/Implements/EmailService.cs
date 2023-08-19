using System.Net.Mail;
using System.Net;

public class EmailService : IEmailService
{
    readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Send(string toMail, string subject, string message, bool isBodyHtml = true)
    {
        SmtpClient smtp = new SmtpClient();
        smtp.Port = Convert.ToInt32(_configuration["Email:Port"]);
        smtp.Host = _configuration["Email:Host"];
        smtp.EnableSsl = true;

        MailAddress from = new MailAddress(_configuration["Email:Username"], "Riode support");
        MailAddress to = new MailAddress(toMail);

        NetworkCredential credential = new NetworkCredential
            (
            _configuration["Email:Username"],
            _configuration["Email:Password"]
            );

        smtp.Credentials = credential;

        MailMessage mm = new MailMessage(from, to);
        mm.Subject = subject;
        mm.Body = message;
        mm.IsBodyHtml = isBodyHtml;
        smtp.Send(mm);
    }
}