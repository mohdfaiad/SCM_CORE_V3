using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;
using System.Configuration;

namespace BAL
{
    public class ShipperBAL
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion

        #region GetShipperAccountInfo
        public DataSet GetShipperAccountInfo(string AccountCode,DateTime TimeStamp)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            string[] paramname = new string[2];
            paramname[0] = "AccountCode";
            paramname[1] = "TimeStamp";
            object[] paramvalue = new object[2];
            paramvalue[0] = AccountCode;
            paramvalue[1] = TimeStamp;
            SqlDbType[] paramtype = new SqlDbType[2];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.DateTime;
            ds = da.SelectRecords("sp_GetShipConsDetailsByAcctNum", paramname, paramvalue, paramtype);
            return ds;
        }
        #endregion
    }
}
