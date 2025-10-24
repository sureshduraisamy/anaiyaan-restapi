using System;
using System.Threading.Tasks;

namespace AutomatedEmailSender
{
    public abstract class EmailSender
    {
        public string FromAddress;
        public string ToAddress;
        public string CcAddress;
        public string BccAddress;
        public string Subject;
        public string Content;
        public string GmailAppPassword;


        public EmailSender(string fromaddress, string toaddress,string ccaddress,string bccaddress,  string subject, string content, string gmailAppPassword)
        {
            FromAddress = fromaddress;
            ToAddress = toaddress;
            CcAddress = ccaddress;
            BccAddress = bccaddress;
            Subject = subject;
            Content = content;
            GmailAppPassword = gmailAppPassword;
        }

        public abstract void SendEmail();
    }
}
