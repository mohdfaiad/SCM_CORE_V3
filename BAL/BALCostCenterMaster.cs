using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALCostCenterMaster
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables

        #region Get Cost Center
        public DataSet ListCostCenter(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                
                string[] paramname = new string[3];
                SqlDbType[] paramtype = new SqlDbType[3];

                paramname[0] = "CostCenterName";
                paramname[1] = "CostCenterDescription";
                paramname[2] = "IsActive";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.Bit;
                
                DataSet result = da.SelectRecords("sp_ListCostCenter", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Add New Cost Center
        public DataSet InsertCostCenter(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[6];
                SqlDbType[] paramtype = new SqlDbType[6];

                paramname[0] = "CostCenterName";
                paramname[2] = "CostCenterID";
                paramname[4] = "UpdatedOn";
                paramname[3] = "UpdatedBy";
                paramname[1] = "CostCenterDescription";
                paramname[5] = "IsActive";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.DateTime;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.Bit;

                DataSet result = da.SelectRecords("sp_SaveCostCenter", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Delete
        public DataSet DeleteCostCenter(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[1];
                SqlDbType[] paramtype = new SqlDbType[1];

                paramname[0] = "CostCenterName";

                paramtype[0] = SqlDbType.VarChar;

                DataSet result = da.SelectRecords("sp_DeleteCostCenter", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        

    }
}
