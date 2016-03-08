using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RM.Common.DotNetEamil
{
    public class SMTPManager
    {
        public static string MailSending(string Dep_Email, string Mis_Name, string Mis_Describe, string File_Path)
        {
            string MailUser = ConfigurationManager.AppSettings["MailUser"].ToString();
            string MailName = ConfigurationManager.AppSettings["MailName"].ToString();
            string MailHost = ConfigurationManager.AppSettings["MailHost"].ToString();
            string MailPwd = ConfigurationManager.AppSettings["MailPwd"].ToString();
            MailAddress from = new MailAddress(MailUser, MailName);
            MailMessage mail = new MailMessage();
            mail.Subject = Mis_Name;
            mail.From = from;
            string[] mailNames = (Dep_Email + ";").Split(new char[]
			{
				';'
			});
            string[] array = mailNames;
            for (int i = 0; i < array.Length; i++)
            {
                string name = array[i];
                if (name != string.Empty)
                {
                    string displayName;
                    string address;
                    if (name.IndexOf('<') > 0)
                    {
                        displayName = name.Substring(0, name.IndexOf('<'));
                        address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                    }
                    else
                    {
                        displayName = string.Empty;
                        address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                    }
                    mail.To.Add(new MailAddress(address, displayName));
                }
            }
            mail.Body = Mis_Describe;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            if (File_Path != "")
            {
                mail.Attachments.Add(new Attachment(File_Path));
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            }
            SmtpClient client = new SmtpClient();
            client.Host = MailHost;
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(MailUser, MailPwd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mail);
            return mail.ToString();
        }
    }
}