using System.Net;
using System.Net.Mail;

namespace AirlineReservationSystem.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("maherkarema@gmail.com", "tmce jvjh yhby zpsw")
            };

            return client.SendMailAsync(
                new MailMessage(from: "maherkarema@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                {
                    IsBodyHtml = true
                }
                );
        }

    }
}
