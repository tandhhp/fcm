using MailKit.Net.Smtp;
using MimeKit;

namespace Waffle.ExternalAPI;

public class Sender
{
    public static async Task SendAsync(string? recipientEmail, string subject, string body)
    {
        if (string.IsNullOrEmpty(recipientEmail)) return;
        string senderEmail = "";
        string password = "";

        // Mail message
        var mail = new MimeMessage();
        mail.From.Add(new MailboxAddress("No Reply", senderEmail));
        mail.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
        mail.Subject = subject;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        mail.Body = bodyBuilder.ToMessageBody();
        // SMTP client
        using var client = new SmtpClient();
        await client.ConnectAsync("mail.nuras.com.vn", 465, true);
        await client.AuthenticateAsync(senderEmail, password);

        try
        {
            // Send the email
            await client.SendAsync(mail);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email: " + ex.Message);
        }
    }
}
