using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{


    public class Jquery
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;
        DataSet ds;

        public Jquery()
        {
            constr = Global.GetConnectionString();
        }

        #region Get JQPlot Details
        public DataSet GetTopAgent(DateTime dt1, DateTime dt2)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "dt1";
                Pname[1] = "dt2";

                Pvalue[0] = dt1;
                Pvalue[1] = dt2;

                Ptype[0] = SqlDbType.DateTime;
                Ptype[1] = SqlDbType.DateTime;


                res = da.SelectRecords("SP_GetopAgent", Pname, Pvalue, Ptype);
                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }
        #endregion GetAuditTrail

        #region getDataForFlightDashboard
        public DataSet getDataForFlightDashboard(string Location, DateTime FromDate, DateTime ToDate)
        {
            DataSet dsData = new DataSet();
            try
            {
                //DataSet dsData = new DataSet();
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                string procedure = "spgetDataforFlightDashBoard";
                string[] paramname = new string[] { "Station", "FromDt", "ToDt" };
                object[] paramvalue = new object[] { Location, FromDate, ToDate };
                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                dsData = objSQL.SelectRecords(procedure, paramname, paramvalue, paramtype);
            }
            catch (Exception ex)
            { }
            return dsData;
        }
        #endregion

        #region Get JQPlot Details
        public DataSet GetProduction(string Month1, string Year1, string FortNight1, string Month2, string Year2, string FortNight2, bool isRegion, string RegLocName)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[8];
                object[] Pvalue = new object[8];
                SqlDbType[] Ptype = new SqlDbType[8];

                Pname[0] = "Month1";
                Pname[1] = "Year1";
                Pname[2] = "FortNight1";
                Pname[3] = "Month2";
                Pname[4] = "Year2";
                Pname[5] = "FortNight2";
                Pname[6] = "isRegion";
                Pname[7] = "RegLocName";

                Pvalue[0] = Month1;
                Pvalue[1] = Year1;
                Pvalue[2] = FortNight1;
                Pvalue[3] = Month2;
                Pvalue[4] = Year2;
                Pvalue[5] = FortNight2;
                Pvalue[6] = isRegion;
                Pvalue[7] = RegLocName;


                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.Bit;
                Ptype[7] = SqlDbType.VarChar;


                res = da.SelectRecords("sp_GetLineJQData", Pname, Pvalue, Ptype);
                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }
        #endregion GetAuditTrail

    }

}
