using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using QID.DataAccess;

namespace BAL
{
    public class BALDriverMaster
    {
        #region Variables
        
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #endregion

        #region Get Driver List

        public DataSet GetDriverList(string DriverName, string LicenceNo, string VehicleNo, string Phone, bool IsActive)
        {
            DataSet ds = new DataSet();
            try
            {
                string[] Pname = new string[5];
                Pname[0] = "DriverName";
                Pname[1] = "LicenceNo";
                Pname[2] = "VehicleNo";
                Pname[3] = "Phone";
                Pname[4] = "IsActive";

                object[] Pvalue = new object[5];
                Pvalue[0] = DriverName;
                Pvalue[1] = LicenceNo;
                Pvalue[2] = VehicleNo;
                Pvalue[3] = Phone;
                Pvalue[4] = IsActive;

                SqlDbType[] Ptype = new SqlDbType[5];
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Bit;

                ds = db.SelectRecords("Sp_GetDriverDetails", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }

            }
            catch (Exception ex)
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                
            }
            return null;
        }

        #endregion

        #region Save Driver Details
        public DataSet SetDriverDetails(string DriverName, string LicenceNo, string VehicleNo, string Phone,string UpdatedBy, bool IsActive)
        {
            DataSet ds = new DataSet();
            
            try
            {
                string[] Pname = new string[6];
                Pname[0] = "DriverName";
                Pname[1] = "LicenceNo";
                Pname[2] = "VehicleNo";
                Pname[3] = "Phone";
                Pname[4] = "UpdatedBy";
                Pname[5] = "IsActive";

                object[] Pvalue = new object[6];
                Pvalue[0] = DriverName;
                Pvalue[1] = LicenceNo;
                Pvalue[2] = VehicleNo;
                Pvalue[3] = Phone;
                Pvalue[4] = UpdatedBy;
                Pvalue[5] = IsActive;


                SqlDbType[] Ptype = new SqlDbType[6];
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.Bit;

                ds = db.SelectRecords("Sp_SetDriverDetails",Pname,Pvalue,Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }

            }
            catch (Exception ex)
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return null;
        }

        #endregion
    }
}
