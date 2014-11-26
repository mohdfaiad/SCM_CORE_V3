using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;


/// <summary>
/// Summary description for clsLog
/// </summary>
public class clsLog
{
   
        #region WriteLog
        public static void WriteLog(String Message)
        {
            try
            {
                string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\" + "CreateOrdrbyExcel_Log.txt";
                long length = 0;
                StreamWriter sw1;
                if (File.Exists(APP_PATH))
                {
                    FileInfo file = new FileInfo(APP_PATH);
                    length = file.Length;
                }
                if (length > 10000000)
                    sw1 = new StreamWriter(APP_PATH, false);
                else
                    sw1 = new StreamWriter(APP_PATH, true);
                sw1.WriteLine(Message);
                sw1.Close();
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region WriteLogwithParameter
        public static void WriteLog(String Message,string Logpath)
        {
            try
            {
               // string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\" + fileName+"_Log.txt";
                string APP_PATH = Logpath;

                long length = 0;
                StreamWriter sw1;
                if (File.Exists(APP_PATH))
                {
                    FileInfo file = new FileInfo(APP_PATH);
                    length = file.Length;
                }
                if (length > 10000000)
                    sw1 = new StreamWriter(APP_PATH, false);
                else
                    sw1 = new StreamWriter(APP_PATH, true);
                sw1.WriteLine(Message);
                sw1.Close();
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
