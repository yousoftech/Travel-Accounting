using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;

/// <summary>
/// Summary description for SendEmail
/// </summary>
public class SendEmail
{
    public SendEmail()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void EmailSend(string to,string subj,string body)
    {

        try
        {


            

            MailMessage message = new System.Net.Mail.MailMessage("kiwihortltd@gmail.com",to.Trim(), subj, body);

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";

            smtp.Port = 587;
            System.Net.NetworkCredential HT = new System.Net.NetworkCredential("kiwihortltd@gmail.com", "Hypernova123");
            smtp.Credentials = HT;

            smtp.EnableSsl = true;

            message.IsBodyHtml = true;

            smtp.Send(message);

        }

        catch (Exception ex)
        {

        }
    }
}