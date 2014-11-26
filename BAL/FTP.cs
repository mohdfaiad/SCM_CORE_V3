using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;

namespace BAL
{
    public static class FTP
    {
        #region Save Saveon72FTP
        /// <summary>
        /// Saves FTP message 
        /// </summary>
        /// <param name="Message">Message to be stored on FTP.</param>
        /// <param name="FileName">Name to be given to file on FTP.</param>
        /// <returns>True if File saved successfully.</returns>
        public static bool Saveon72FTP(string Message, string FileName)
        {
            try
            {   //Find FTP address to save file. Write code to fetch FTP address from database.
                //Save file.
                FtpWebRequest myFtpWebRequest;
                FtpWebResponse myFtpWebResponse;
                StreamWriter myStreamWriter;

                string UserName = "AAuser";
                string Password = "AAuser";
                string FTPPath = " ftp://72.167.41.153:8897/";
                myFtpWebRequest = (FtpWebRequest)WebRequest.Create(FTPPath + "/" + FileName + ".msg");
                //myFtpWebRequest = (FtpWebRequest)WebRequest.Create(FTPPath + "/TestFTP.msg");

                if (UserName != "")
                {
                    myFtpWebRequest.Credentials = new NetworkCredential(UserName, Password);
                }

                myFtpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                myFtpWebRequest.UseBinary = true;

                myStreamWriter = new StreamWriter(myFtpWebRequest.GetRequestStream());
                myStreamWriter.Write(Message);
                myStreamWriter.Close();

                myFtpWebResponse = (FtpWebResponse)myFtpWebRequest.GetResponse();


                myFtpWebResponse.Close();

            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error on FTP upload:" + ex.Message);
                return (false);
            }
            return (true);
        }
        #endregion Save FTP

        #region Save FTP
        /// <summary>
        /// Saves FTP message 
        /// </summary>
        /// <param name="Message">Message to be stored on FTP.</param>
        /// <param name="FileName">Name to be given to file on FTP.</param>
        /// <returns>True if File saved successfully.</returns>
        public static bool SaveFTP(string FTPPath, string UserName, string Password, string Message, string FileName)
        {
            try
            {   //Find FTP address to save file. Write code to fetch FTP address from database.


                //Save file.
                FtpWebRequest myFtpWebRequest;
                FtpWebResponse myFtpWebResponse;
                StreamWriter myStreamWriter;

                myFtpWebRequest = (FtpWebRequest)WebRequest.Create(FTPPath + "/" + FileName + ".msg");
                //myFtpWebRequest = (FtpWebRequest)WebRequest.Create(FTPPath + "/TestFTP.msg");

                if (UserName != "")
                {
                    myFtpWebRequest.Credentials = new NetworkCredential(UserName, Password);
                }

                myFtpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                myFtpWebRequest.UseBinary = true;

                myStreamWriter = new StreamWriter(myFtpWebRequest.GetRequestStream());
                myStreamWriter.Write(Message);
                myStreamWriter.Close();

                myFtpWebResponse = (FtpWebResponse)myFtpWebRequest.GetResponse();


                myFtpWebResponse.Close();

            }
            catch (Exception)
            {
                return (false);
            }
            return (true);
        }
        #endregion Save FTP

        public static bool SaveSITAMsg(string Message, string FileName)
        {
            try
            {   //Find FTP address to save file. Write code to fetch FTP address from database.
                //Save file.
                FtpWebRequest myFtpWebRequest;
                FtpWebResponse myFtpWebResponse;
                StreamWriter myStreamWriter;

                string UserName = "";
                string Password = "";
                string FTPPath = "ftp://72.167.39.192:8897/";// ftp://72.167.41.153:8897/";
                myFtpWebRequest = (FtpWebRequest)WebRequest.Create(FTPPath + "/" + FileName + ".snd");
                //myFtpWebRequest = (FtpWebRequest)WebRequest.Create(FTPPath + "/TestFTP.msg");

                if (UserName != "")
                {
                    myFtpWebRequest.Credentials = new NetworkCredential(UserName, Password);
                }

                myFtpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                myFtpWebRequest.UseBinary = true;

                myStreamWriter = new StreamWriter(myFtpWebRequest.GetRequestStream());
                myStreamWriter.Write(Message);
                myStreamWriter.Close();

                myFtpWebResponse = (FtpWebResponse)myFtpWebRequest.GetResponse();


                myFtpWebResponse.Close();

            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error on FTP upload:" + ex.Message);
                return (false);
            }
            return (true);
        }
    }
}
