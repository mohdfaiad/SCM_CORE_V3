using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;
using System.IO.Ports;
using System.Net;
using System.Data;
using System.Web;
using QID.DataAccess;
using System.IO;
using System.Configuration;

namespace BAL
{
    public class SMS
    {
        
        #region Variables
        SerialPort srPort = new SerialPort();
        #endregion Variables

        #region Constructor
        public SMS()
        {
            srPort.PortName = "COM8";
            srPort.BaudRate = 115200;
        }

        internal SMS(string Port, int BaudRate)
        {
            srPort.PortName = Port;
            srPort.BaudRate = BaudRate;
        }
        #endregion Constructor

        #region SendSMS
        /// <summary>
        /// Sends given Message as SMS to given Phone Number.
        /// </summary>
        /// <param name="MobileNumber">Recipient's mobile number.</param>
        /// <param name="Message">Message to be sent to recipient.</param>
        /// <returns>Result code of message sending operation.</returns>
        public int SendSMS(string MobileNumber, string Message)
        {
            try
            {
                if (!MobileNumber.Contains("+"))
                    return (1);

                if (Message.Length > 100)
                    return (7);

                if (!OpenPort())
                    return (6);

                //Set message format...
                SendCommand("AT+CMGF = 1", false);
                Delay(600);
                if (!ReadResponse().Contains("OK"))
                {
                    ClosePort();
                    return (2);
                }
                //Set validation period...
                SendCommand("AT+CSMP = 17,167,0,0", false);
                Delay(600);
                if (!ReadResponse().Contains("OK"))
                {
                    ClosePort();
                    return (3);
                }

                //Send destination number...
                SendCommand("AT+CMGS = " + MobileNumber, false);
                Delay(600);
                if (!ReadResponse().Contains("> "))
                {
                    ClosePort();
                    return (4);
                }

                //Send actual message...
                SendCommand(Message, true);
                Delay(600);
                string str = ReadResponse();
                //Console.WriteLine(str);
                if (!str.Contains(Message))
                {
                    ClosePort();
                    return (5);
                }

                ClosePort();
                return (0);

            }
            catch (Exception)
            {
                ClosePort();
                return (6);
            }
        }
        #endregion SendSMS

        #region Get Message List
        /// <summary>
        /// Gets list of messages present in SIM based on given status.
        /// </summary>
        /// <param name="status">Message status to be listed.</param>
        /// <returns>Messages as specified in GPRS_TELIT_AT_COMMAND_REFERENCE_GUIDE.</returns>
        public  string GetMessageList(int status)
        {
            try
            {
                if (!OpenPort())
                    return ("");

                //Set message format as Text.
                SendCommand("AT+CMGF=1", false);
                Delay(500);
                if (!ReadResponse().Contains("OK"))
                {
                    return ("");
                }

                //Get list of sms present...
                string strStatus = "ALL";
                switch (status)
                {
                    case 0: //Received and Unread.
                        strStatus = "REC UNREAD";
                        break;
                    case 1: //Received and Read.
                        strStatus = "REC READ";
                        break;
                    case 2: //Stored message not yet sent.
                        strStatus = "STO UNSENT";
                        break;
                    case 3: //Stored message already sent.
                        strStatus = "STO SENT";
                        break;
                    case 4: //All messages.
                        strStatus = "ALL";
                        break;
                    default:
                        break;
                }

                SendCommand("AT+CMGL=" + strStatus, false);
                string strMessageList = "";
                for (int i = 0; i < 5; i++)
                {
                    Delay(1000);
                    strMessageList += ReadResponse();
                    if (strMessageList.Contains("OK"))
                        break;
                }

                return (strMessageList);
            }
            catch (Exception)
            {
                return ("");
            }
        }
        #endregion Get Message List

        #region Read Message
        /// <summary>
        /// Gets message present in SIM based on given index.
        /// </summary>
        /// <returns>Index,Sender,,Message.</returns>
        public  string ReadMessage(string index)
        {
            try
            {
                if (!OpenPort())
                    return ("");
                //Set message format as Text.
                
                SendCommand("AT+CMGF=1", false);
                string responce = "";
                for (int i = 0; i < 5; i++)
                {
                    Delay(1000);
                    responce += ReadResponse();
                    if (responce.Contains("OK"))
                        break;
                }
                if (!responce.Contains("OK"))
                {
                    return ("");
                }
                SendCommand("AT+CMGR=" + index, false);

                //SendCommand("AT+CMGF=1", false);
                string strMessage = "";
                for (int i = 0; i < 5; i++)
                {
                    Delay(1000);
                    strMessage += ReadResponse();
                    if (strMessage.Contains("OK"))
                        break;
                }
                strMessage = strMessage.Remove(0, strMessage.IndexOf("+CMGR: ") + 7);
                string[] aryMessageSummary;
                aryMessageSummary = null;
                aryMessageSummary = strMessage.Split(',');
                if (aryMessageSummary != null)
                {
                    if (aryMessageSummary.Length > 3)
                    {
                        strMessage = aryMessageSummary.GetValue(1).ToString().Replace("\"", "");
                        //Reconstruct SMS if comma is present in the message...
                        string strSMS = "";
                        for (int i = 4; i < aryMessageSummary.Length; i++)
                        {
                            if (i > 4)
                                strSMS = strSMS + ",";
                            strSMS += aryMessageSummary.GetValue(i).ToString();
                        }
                        //Retrieve message from string...
                        string[] strmsg = strSMS.Split('\r');      //aryMessageSummary.GetValue(4).ToString().Split('\r');
                        //strMessage = strMessage + strmsg.GetValue(1).ToString().Replace("\n", "");
                        strMessage = strmsg.GetValue(1).ToString().Replace("\n", "");
                        strMessage = HexString2Ascii(strMessage);
                    }
                    else
                        strMessage = "";
                }
                ClosePort();
                return (strMessage);
                //return (HexString2Ascii(strMessage));
                
            }
            catch (Exception ex)
            {
                return ("");
                
            }
        }
        #region Conversion
        //private static string HexString2Ascii(string hexString)
        //{
        //    byte[] tmp;
        //    int j = 0;
        //    int t;
        //    tmp = new byte[(hexString.Length) / 2];
        //    int i;
        //    t = (hexString.Length);
        //    t = t - 2;
        //    for (i = 0; i <= t; i += 2)
        //    {
        //        tmp[j] = (byte)Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
        //        j++;
        //    }
        //    return Encoding.GetEncoding(1252).GetString(tmp, 0, tmp.Length);
        //}
        #endregion

        #endregion Read Message

        #region Conversion
        private static string HexString2Ascii(string hexdata)
        {
            //string hexdata = "0791198945900906040781350000F300000110718195322265D9775D0E9297E3F5F29C0E32BFE5A0795D3C1FCBD3707AFAED06D1DF20623ACC2ECB41D4BABB3C07CDCB727B7A5C06A5E7A0F0785C86D3CB641788FA069DCB74D0FCED3E83C66F72790E7ACB41F3F61C04225241C6A493E802";
            string dt1 = "";
            string strdata = "";
            int i = 0;
            while (i <= hexdata.Length - 2)
            {
                dt1 = hexdata.Substring(i, 2);
                if (dt1 != "00")
                strdata += Convert.ToChar(Convert.ToUInt32(dt1, 16)).ToString();
                i = i + 2;
            }
            return (strdata);
        }
        #endregion

        #region Delete Message
        /// <summary>
        /// Delets message present in SIM based on given index.
        /// </summary>
        /// <returns>Index.</returns>
        public  bool DeleteMessage(string index)
        {
            try
            {
                if (!OpenPort())
                    return (false);

                //Set message format as Text.
                SendCommand("AT+CMGD=" + index, false);
                string strMessage = "";
                for (int i = 0; i < 10; i++)
                {
                    Delay(500);
                    strMessage += ReadResponse();
                    if (strMessage.Contains("OK"))
                        break;
                }
                if (strMessage.Contains("OK"))
                    return (true);
                else
                    return (false);
            }
            catch (Exception)
            {
                return (false);
            }
        }
        #endregion Delete Message

        #region OpenPort
        private bool OpenPort()
        {
            try
            {
                if (!srPort.IsOpen)
                    srPort.Open();

                if (!srPort.IsOpen)
                    return (false);
            }
            catch (Exception)
            {
                return (false);
            }
            return (true);
        }
        #endregion OpenPort

        #region ClosePort
        /// <summary>
        /// Closes serial port opened for GSM operations.
        /// </summary>
        private void ClosePort()
        {
            try
            {
                if (srPort.IsOpen)
                {
                    srPort.Close();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion ClosePort

        #region Delay
        private void Delay(int milliSeconds)
        {
            DateTime dt = DateTime.Now.AddMilliseconds(milliSeconds);
            while (dt > DateTime.Now)
            {
                //System.Windows.Forms.Application.DoEvents();
            }
        }
        #endregion Delay

        #region Send Command
        private void SendCommand(string Command, bool IsSMSText)
        {
            if (!srPort.IsOpen)
            {
                return;
            }
            srPort.DiscardInBuffer();
            srPort.DiscardOutBuffer();
            if (!IsSMSText)
            {
                srPort.WriteLine(Command + "\r");
            }
            else
            {
                srPort.WriteLine(Command + Convert.ToChar(Convert.ToInt16("1A", 16)).ToString());
            }
        }
        #endregion Send Command

        #region Read Response
        private string ReadResponse()
        {
            string str = "";

            if (srPort.IsOpen)
            {
                str = srPort.ReadExisting();
            }

            return (str);

        }
        #endregion Delay

        #region Send SMS
        /// <summary>
        /// sending SMS through http link SMSCounty
        /// </summary>
        /// <param name="mobileno"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool sendSMS(string mobileno, string message)
        {
            bool flag = false;
            try
            {

                #region Constants
                StringBuilder sb = new StringBuilder();
                int count = 0;
                string tempstring = "";
                byte[] buf = new byte[8192];
                # endregion

                //clsLog.WriteLog("SMS Message = " + message + " @ " + System.DateTime.Now.ToString());
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://www.smscountry.com/SMSCwebservice.asp?User=" + ConfigurationManager.AppSettings["SMSUN"].ToString() + "&passwd=" + ConfigurationManager.AppSettings["SMSPASS"].ToString() + "&mobilenumber=" + mobileno + "&message=" + message + "&sid=QIDAlert&mtype=N&DR=Y");
                //HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://www.smscountry.com/SMSCwebservice.asp?User=" + ConfigurationManager.AppSettings["SMSUN"].ToString() + "&passwd=" + ConfigurationManager.AppSettings["SMSPASS"].ToString() + "&mobilenumber=" + "919224255512," + ",919767816454" + "&message=" + Message + "&sid=QIDAlert&mtype=N&DR=Y");

                HttpWebResponse Reponse = (HttpWebResponse)Request.GetResponse();
                Stream Response_Stream = Reponse.GetResponseStream();

                do
                {
                    count = Response_Stream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        tempstring = Encoding.ASCII.GetString(buf, 0, count);
                        sb.Append(tempstring);
                    }
                } while (count > 0);
                //clsLog.WriteLog("SMS sent @ "+ DateTime.Now.ToString());
                flag = true;
            }
            catch (Exception ex)
            {
                //clsLog.WriteLog("Exception while sending SMS : "+ ex.Message +" @ "+ DateTime.Now.ToString());
                flag = false;
            }
            return flag;
        }

        public bool sendSMS(string mobileno, string message, string username, string password)
        {
            bool flag = false;
            try
            {

                #region Constants
                StringBuilder sb = new StringBuilder();
                int count = 0;
                string tempstring = "";
                byte[] buf = new byte[8192];
                # endregion

                //clsLog.WriteLog("SMS Message = " + message + " @ " + System.DateTime.Now.ToString());
                string link = "http://www.smscountry.com/SMSCwebservice.asp?User=" + username + "&passwd=" + password + "&mobilenumber=" + mobileno + "&message=" + message + "&sid=QIDAlert&mtype=N&DR=Y";
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://www.smscountry.com/SMSCwebservice.asp?User=" + username + "&passwd=" + password + "&mobilenumber=" + mobileno + "&message=" + message + "&sid=QIDAlert&mtype=N&DR=Y");
                //HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://www.smscountry.com/SMSCwebservice.asp?User=" + ConfigurationManager.AppSettings["SMSUN"].ToString() + "&passwd=" + ConfigurationManager.AppSettings["SMSPASS"].ToString() + "&mobilenumber=" + "919224255512," + ",919767816454" + "&message=" + Message + "&sid=QIDAlert&mtype=N&DR=Y");

                HttpWebResponse Reponse = (HttpWebResponse)Request.GetResponse();
                Stream Response_Stream = Reponse.GetResponseStream();

                do
                {
                    count = Response_Stream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        tempstring = Encoding.ASCII.GetString(buf, 0, count);
                        sb.Append(tempstring);
                    }
                } while (count > 0);
                //clsLog.WriteLog("SMS sent @ "+ DateTime.Now.ToString());
                flag = true;
            }
            catch (Exception ex)
            {
                //clsLog.WriteLog("Exception while sending SMS : "+ ex.Message +" @ "+ DateTime.Now.ToString());
                flag = false;
            }
            return flag;
        }
        #endregion

    }
}
