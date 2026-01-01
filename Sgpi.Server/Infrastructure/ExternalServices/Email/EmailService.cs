
using System.Net;
using System.Net.Mail;

namespace Sgpi.Server.Infrastructure.ExternalServices.Email
{
  public class EmailService : IEmailService
  {
    private IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
      var from = _configuration["EmailConfiguration:From"];
      var smtpServer = _configuration["EmailConfiguration:SmtpServer"];
      var port = int.Parse(_configuration["EmailConfiguration:Port"]!);
      var username = _configuration["EmailConfiguration:Username"];
      var password = _configuration["EmailConfiguration:Password"];

      var message = new MailMessage(from, toEmail, subject, body);
      message.IsBodyHtml = true;

      using var client = new SmtpClient(smtpServer, port)
      {
        Credentials = new NetworkCredential(username, password),
        EnableSsl = true
      };

      await client.SendMailAsync(message);
    }
  }
}
