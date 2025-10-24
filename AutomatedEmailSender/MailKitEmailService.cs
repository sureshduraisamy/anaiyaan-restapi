using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AutomatedEmailSender
{
    public class MailKitEmailService : EmailSender
    {
        

        public MailKitEmailService(string fromAddress, string toAddress, string ccAddress, string bccAddress, string subject, string content, string gmailAppPassword)
            : base(fromAddress, toAddress, ccAddress, bccAddress, subject, content, gmailAppPassword)
        {
           
        }

        public override void SendEmail()
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Sender", FromAddress));
            email.To.Add(new MailboxAddress("Recipient", ToAddress));
            email.Subject = Subject;
            email.Body = new TextPart("plain")
            {
                Text = Content
            };

            // Add CC if provided
            if (!string.IsNullOrWhiteSpace(CcAddress))
            {
                email.Cc.Add(new MailboxAddress("CC", CcAddress));
            }

            // Add BCC if provided
            if (!string.IsNullOrWhiteSpace(BccAddress))
            {
                email.Bcc.Add(new MailboxAddress("BCC", BccAddress));
            }

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    Console.WriteLine("Connecting to SMTP server...");
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    Console.WriteLine("Connected.");

                    smtp.Authenticate(FromAddress, GmailAppPassword);
                    smtp.Send(email);
                    smtp.Disconnect(true);

                    Console.WriteLine("Email sent successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}
