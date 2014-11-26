using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;

namespace ProjectSmartCargoManager
{
    public static class Global
    {

        public static string GetConnectionString()
        {
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                if (strcon == null)
                {
                    strcon = "";
                }
                return (strcon);
            }
            catch (Exception)
            {
                return ("") ;
            }
        }

    }
}
