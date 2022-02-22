using Helperland.ViewModels;
using System;
using System.IO;
using System.Net.Mail;

namespace Helperland.Services
{
    public class EmailService
    {
        public static Boolean SendMail(ResetPasswordViewModel email)
        {
            try
            {
                MailMessage mm = new MailMessage()
                {
                    From = new MailAddress("helperland44@gmail.com")
                };
                mm.To.Add(email.To);
                mm.Subject = email.Subject;
                mm.Body = email.Body;
                mm.IsBodyHtml = email.IsHTML;
                if (email.Attachment != null)
                {

                    string fileName = Path.GetFileName(email.Attachment.FileName);
                    mm.Attachments.Add(new Attachment(email.Attachment.OpenReadStream(), fileName));
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("helperland44@gmail.com", "helperland@2001"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mm);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
