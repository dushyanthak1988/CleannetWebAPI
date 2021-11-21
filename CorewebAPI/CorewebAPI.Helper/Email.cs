using CorewebAPI.Entities.Model;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace CorewebAPI.Helper
{
    public interface IEmail
    {
        public string SendEmail(string ReciverEmail, string CCReciver, string EmailHeading, string Attachmentpath, string Body);
    }
    public class Email : IEmail
    {
        private readonly AppSettings _appSettings;

        public Email(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string SendEmail(string ReciverEmail, string CCReciver, string EmailHeading, string Attachmentpath, string Body)
        {

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(ReciverEmail));
            if (CCReciver != "")
                msg.CC.Add(new MailAddress(CCReciver));

            msg.From = new MailAddress(_appSettings.EmailSender, "Softlogic Life Insurance");
            msg.Subject = EmailHeading;
            msg.Body = Body;


            if (Attachmentpath != "")
            {
                Attachment attachment = new Attachment(Attachmentpath);
                msg.Attachments.Add(attachment);
            }

            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_appSettings.EmailSender, _appSettings.EmailPassword);
            client.Port = _appSettings.PortNumber;
            client.Host = _appSettings.SmtpHost;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = _appSettings.IsSSL;
            client.Send(msg);

            return "Email Sent";
        }

    }
}

