using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace BAL
{

    #region EMAILOUT
    /// <summary>
    /// This class provides facility to sending mail.
    /// </summary>
    public class EMAILOUT
    {

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EMAILOUT()
        { }
        #endregion

        #region Send Mail
        /// <summary>
        /// provide facility to send mail to provided email through provided emailid with provided mail body
        /// </summary>
        /// <param name="fromEmailId"></param>
        /// <param name="toEmailId"></param>
        /// <param name="password"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHTML"></param>
        /// <returns></returns>
        public bool sendMail(string fromEmailId, string toEmailId, string password, string subject, string body, bool isBodyHTML)
        {
            bool flag = false;
            try
            {
                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress(fromEmailId);
                Mail.To.Add(toEmailId);
                Mail.Subject = subject;
                Mail.IsBodyHtml = isBodyHTML;
                Mail.Body = body;
                SmtpClient smtp = new SmtpClient("smtpout.secureserver.net",80);//previous 25
                smtp.Credentials = new NetworkCredential(fromEmailId, password);
                Mail.Priority = MailPriority.High;
                
                try
                {
                    smtp.Send(Mail);
                    flag = true;
                    //clsLog.WriteLog("Mail Sent @ "+DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    //clsLog.WriteLog("Exception while Sending Mail : "+ ex.Message );
                }

            }
            catch (Exception ex)
            {
                //clsLog.WriteLog("Exception while collection Mail Info : "+ ex.Message );
                flag = false;
            }
            return flag;
        }
        #endregion

    }
    #endregion

}
