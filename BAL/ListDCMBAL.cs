using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;


namespace BAL
{
    public class ListDCMBAL
    {
         string constr = "";
        public ListDCMBAL()
        {
            constr = Global.GetConnectionString();
        }


        # region GetDCMList
        public DataSet GetDCMList(object[] AcEq)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ParamNames = new string[] { "AWBNumber", "DCMNumber" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("spGetDCMList", ParamNames, AcEq, ParamTypes);
                //DataSet ds = da.SelectRecords("spGetAircraftEquipment", "Manufacturer", AcInfo, SqlDbType.VarChar);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return (ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return (null);


        }
        #endregion GetDCMList
    }
}