using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Common
{
   public class MailHelper
    {
        public static bool SendMail(string toEmail, string subject, string content)
        {
            try
            {
                var host = ConfigHelper.GetByKey("SMTPHost");
                var port = int.Parse(ConfigHelper.GetByKey("SMTPPort"));
                var fromEmail = ConfigHelper.GetByKey("FromEmailAddress");
                var password = ConfigHelper.GetByKey("FromEmailPassword");
                var fromName = ConfigHelper.GetByKey("FromName");

                var smtpClient = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromEmail, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                };

                var mail = new MailMessage
                {
                    Body = content,
                    Subject = subject,
                    From = new MailAddress(fromEmail, fromName)
                };

                mail.To.Add(new MailAddress(toEmail));
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                smtpClient.Send(mail);

                return true;
            }
            catch (SmtpException smex)
            {
               
                return false;
            }
        }
    }
}
