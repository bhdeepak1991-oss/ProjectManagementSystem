using System.Net;
using System.Net.Mail;

namespace PMS.Features.UserManagement.Services
{
    public class EmailService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public EmailService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public void RegistrationEmail(string toEmail, string subject, string userName, string password)
        {
            var registrationBody = _env.WebRootPath + "/EmailTemplate/RegistrationEmail.html";

            var loginUrl = _configuration.GetSection("PmsDetail:LoginUrl").Value;

            var emailBody = File.ReadAllText(registrationBody);

            emailBody = emailBody.Replace("{{User}}", userName);

            emailBody = emailBody.Replace("{{Password}}", password);

            emailBody = emailBody.Replace("{{LoginUrl}}", loginUrl);

            SendEmail(toEmail, subject, emailBody);
        }

        public void ForgetPasswordEmail(string toEmail, string subject, string userName, string password)
        {
            var registrationBody = _env.WebRootPath + "/EmailTemplate/ForgetPasswordEmail.html";

            var emailBody = File.ReadAllText(registrationBody);

            emailBody = emailBody.Replace("{{User}}", userName);

            emailBody = emailBody.Replace("{{Password}}", password);

            SendEmail(toEmail, subject, emailBody);
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "Bhdeepak1991@gmail.com";
            var appPassword = "gtgf bltc crlq rrou"; // 16-character app password

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, appPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("support@PMS.com", "PMS Support"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
    }
}
