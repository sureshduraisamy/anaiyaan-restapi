using System;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace AutomatedEmailSender
{
    public class BuiltinEmailService : EmailSender
    {
        
        // Constructor for SendAutomatedEmail, passing parameters to the base class
        public BuiltinEmailService(string fromAddress, string toAddress, string ccaddress, string bccaddress, string subject, string content, string gmailAppPassword)
            : base(fromAddress, toAddress, ccaddress, bccaddress, subject, content, gmailAppPassword)
        {
           
        }

        // Overriding the SendEmail method to send the email using Gmail SMTP
        public override void SendEmail()
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(FromAddress);  // using inherited 'fromAddress'
                    mail.To.Add(ToAddress);  // using inherited 'toAddress'
                    mail.Subject = Subject;  // using inherited 'subject'
                    mail.Body = Content;     // using inherited 'content'
                    mail.IsBodyHtml = true;
                    // Add CC if provided
                    if (!string.IsNullOrWhiteSpace(CcAddress))
                    {
                        mail.CC.Add(CcAddress);
                    }

                    // Add BCC if provided
                    if (!string.IsNullOrWhiteSpace(BccAddress))
                    {
                        mail.Bcc.Add(BccAddress);
                    }

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(FromAddress, GmailAppPassword);
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;

                        smtp.Send(mail);
                        Console.WriteLine("Email sent successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}
