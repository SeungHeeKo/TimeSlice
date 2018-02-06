using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.ComponentModel;

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
        bool mailSent = false;
        
        public EmailService()
        {
            try
            {
                mail = new MailMessage();
                smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(senderMail, senderPassword);
                smtpClient.SendCompleted += SmtpClient_SendCompleted;
                //smtpClient.Timeout = 100000;
                mail.From = new MailAddress(senderMail, "KT 타임 슬라이스", System.Text.Encoding.UTF8);
                mail.Subject = mailSubject;
                mail.Body = "2018 KT Live Site - Ice Hockey Challenge";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            String token = (string)e.UserState;            

            if (e.Error == null)
            {
                // Send succeeded   
                string message = string.Format("Send Mail with subject [{0}]", token);
                mailSent = true;

                //mainWindow.ResetWindow();
                //MessageBox.Show(message);
            }
            else if (e.Cancelled == true)
            {
                string message = string.Format("Send canceld for mail with subject [{0}]", token);
                mailSent = false;
                //MessageBox.Show(message);
            }
            else
            {
                // log exception   
                string message = string.Format("Send Mail Fail - Error: [{0}]", e.Error.ToString());
                mailSent = false;
                //MessageBox.Show(message);
            }

            MainWindow mainWindow = MainWindow.mainWindowInstance;
            mainWindow.MailResult(mailSent);

        }
        
        //public bool SendEmail(string receiverMail, string photoName)
        //{
        //    bool retval = false;

        //    if (smtpClient == null)
        //        return retval;

        //    try
        //    {
        //        mail.To.Add(receiverMail);

        //        Attachment attachment = new Attachment(photoName); //@"D:\TektonSpace\포토존\UI\" + photoName + ".png"
        //        mail.Attachments.Add(attachment);

        //        //smtpClient.Send(mail);
        //        object userState = "test message1";
        //        smtpClient.SendAsync(mail, userState);



        //        //completeWindow = CompleteWindow.completeWindowInstance;
        //        //completeWindow.ShowPopupMessage(true);
        //        //MessageBox.Show("메일이 전송되었습니다.");

        //        return retval = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);

        //        if (mailSent == false)
        //        {
        //            smtpClient.SendAsyncCancel();
        //        }
        //        return retval = false;
        //    }
        //}

        public void SendEmail(string receiverMail, string photoName)
        {
            bool retval = false;

            //if (smtpClient == null)
            //    return retval;

            try
            {
                mail.To.Add(receiverMail);

                Attachment attachment = new Attachment(photoName); //@"D:\TektonSpace\포토존\UI\" + photoName + ".png"
                mail.Attachments.Add(attachment);

                //smtpClient.Send(mail);
                object userState = "test message1";
                smtpClient.SendAsync(mail, userState);



                //completeWindow = CompleteWindow.completeWindowInstance;
                //completeWindow.ShowPopupMessage(true);
                //MessageBox.Show("메일이 전송되었습니다.");

                //return retval = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (mailSent == false)
                {
                    smtpClient.SendAsyncCancel();
                }
                //return retval = false;
            }
        }
    }
}
