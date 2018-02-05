using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace TimeSlice
{
    internal class EmailService
    {   // ktlivesite@gmail.com
        MailMessage mail;
        SmtpClient smtpClient;
        const string senderMail = "livesitekt@gmail.com";
        const string senderPassword = "tekton0405";
        const string mailSubject = "KT 5G Time-Slice";

        const string filePath = @"C:\PhotoZone_Result\total.txt";


        public EmailService()
        {
            try
            {
                mail = new MailMessage();
                smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(senderMail, senderPassword);
                
                smtpClient.Timeout = 100000;
                mail.From = new MailAddress(senderMail, "KT 타임 슬라이스", System.Text.Encoding.UTF8);
                mail.Subject = mailSubject;
                mail.Body = "2018 KT Live Site - Ice Hockey Challenge";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool SendEmail(string receiverMail, string photoName)
        {
            bool retval = false;

            if (smtpClient == null)
                return retval;

            try
            {
                mail.To.Add(receiverMail);

                Attachment attachment = new Attachment(photoName); //@"D:\TektonSpace\포토존\UI\" + photoName + ".png"
                mail.Attachments.Add(attachment);

                smtpClient.Send(mail);

                //completeWindow = CompleteWindow.completeWindowInstance;
                //completeWindow.ShowPopupMessage(true);
                //MessageBox.Show("메일이 전송되었습니다.");

                return retval = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return retval = false;
            }
        }
    }
}
