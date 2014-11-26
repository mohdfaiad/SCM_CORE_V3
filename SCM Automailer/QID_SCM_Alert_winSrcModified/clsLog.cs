using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace QID_SCM_Alert_winSrcModified
{
    class clsLog
    {
        public static void WriteLog(String Message)
        {
            string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\" + "Alert_Log.txt";
            StreamWriter sw1 = new StreamWriter(APP_PATH, true);
            sw1.WriteLine(Message);
            sw1.Close();
        }
    }
}
