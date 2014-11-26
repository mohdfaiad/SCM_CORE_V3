using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;


namespace BAL
{
    public class BALAuditTrail
    {
        //SQLServer db = new SQLServer("");
        string constr = "";
        //DataSet res;
        //DataSet ds;

        public BALAuditTrail()
        {
            constr = Global.GetConnectionString();

        }

        #region GetAuditTrail
        public DataSet GetAuditTrail(string AWBNumber,string AWBPrefix)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = null;
            try
            {

                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "AWBNumber";
                Pname[1] = "AWBPrefix";

                Pvalue[0] = AWBNumber;
                Pvalue[1] = AWBPrefix;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;

                res = da.SelectRecords("Sp_getAuditTrail", Pname, Pvalue, Ptype);
                Pname = null;
                Ptype = null;
                Pvalue = null;
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
                return null;
            }
            finally 
            {
                da = null;
            }
            return res;
        }
        #endregion GetAuditTrail

        #region GetManifestuditTrail
        public DataSet GetAuditTrailManifest(string FlightDate,string FlightNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = new DataSet();
            try
            {
                string[] Pname = new string[2];
                object[] Pvalue = new object[2];
                SqlDbType[] Ptype = new SqlDbType[2];

                Pname[0] = "FlightDate";
                Pname[1] = "FlightNumber";


                Pvalue[0] = FlightDate;
                Pvalue[1] = FlightNumber;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;

                res = da.SelectRecords("SpGetAuditTrailManifest", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
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
            finally 
            {
                da = null;
            }
            return res;
        }
        #endregion GetManifestuditTrail

    }
}
