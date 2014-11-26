using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;
using System.Configuration;


namespace BAL
{
    public class ReportsTracking
    {
        //SQLServer db = new SQLServer("");


        public bool ReportTrack(string Pagenm, string UserNm, string Location, string IP, string Para, DateTime loggedOn)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

            try
            {

                SQLServer db = new SQLServer(constr);
                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];

                Pname[0] = "Pagenm";
                Pname[1] = "UserNm";
                Pname[2] = "Location";
                Pname[3] = "IP";
                Pname[4] = "Para";
                Pname[5] = "loggedOn";

                Pvalue[0] = Pagenm;
                Pvalue[1] = UserNm;
                Pvalue[2] = Location;
                Pvalue[3] = IP;
                Pvalue[4] = Para;
                Pvalue[5] = loggedOn;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.NVarChar;
                Ptype[5] = SqlDbType.DateTime;

                bool res = db.InsertData("ReportTrack", Pname, Ptype, Pvalue);

                return res;
            }

            catch (Exception e)
            {
                return false;
            }
            
        }
    }
}
