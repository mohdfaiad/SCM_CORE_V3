using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Configuration;
using System.Text;
using System.Reflection;

namespace BAL
{
    public class Log
    {
        private static string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

        // return : void
        public static void WriteLog(string message)
        {
            try
            {
                // open log file
                StreamWriter sw = new StreamWriter(APP_PATH + "\\" + ConfigurationSettings.AppSettings["LogFile"].ToString(), true);

                // StreamWriter sw = new StreamWriter(ConfigurationSettings.AppSettings["LogFile"].ToString(), true);
                sw.WriteLine(DateTime.Now.ToString() + " :" + message);
                sw.Flush();
                sw.Close();

                // delete file if its size is more than 10 mb
                FileInfo fileinfo = new FileInfo(APP_PATH + "\\" + ConfigurationSettings.AppSettings["LogFile"].ToString());
                int maxsize = (1024 * 1024 * 10);
                if (fileinfo.Length >= maxsize)
                {
                    File.Delete(APP_PATH + "\\" + ConfigurationSettings.AppSettings["LogFile"].ToString());
                }


            }
            catch (Exception ex)
            {

            }

        }

    }

}
