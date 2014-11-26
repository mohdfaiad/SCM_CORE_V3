using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    /// <summary>
    /// Summary description for QuickSearch
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class QuickSearch : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetFlightList(string Origin, string Dest)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            string[] colNames = new string[2];
            string[] values = new string[2];
            SqlDbType[] dataTypes = new SqlDbType[2];
            int i = 0;

            colNames.SetValue("Origin",i);         
            values.SetValue(Origin,i);
            dataTypes.SetValue(SqlDbType.VarChar,i);
            i++;

            colNames.SetValue("Destination",i);
            values.SetValue(Dest,i);
            dataTypes.SetValue(SqlDbType.VarChar,i);

            
            DataSet ds = da.SelectRecords("spGetFlightForRoute", colNames, values, dataTypes);
            string retVal = "";
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            retVal = retVal + ds.Tables[0].Rows[i]["FlightNumber"].ToString() + ";";

                        }
                        return retVal;
                    }
                }
            }
            return "";
        }        
    }
}
