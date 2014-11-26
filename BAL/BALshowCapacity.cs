using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;
namespace BAL
{
   public class BALshowCapacity
    {
        string constr = "";
        public BALshowCapacity()
        {
           constr = Global.GetConnectionString();
        }

        # region showCapacity
        public DataSet ShowCapacity(object[] AWB)
        {
            try
            {

                string[] ParamNames = new string[] { "fltDt", "fltId", "Station" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spFlightCapacity", ParamNames, AWB, ParamTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables.Count > 0)
                    {
                        return (ds);
                    }

                }

            }
            catch (Exception)
            {
            }
            return (null);


        }
        #endregion

    }
}


