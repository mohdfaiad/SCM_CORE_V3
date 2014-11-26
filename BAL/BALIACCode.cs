using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALIACCode 
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables

        #region Get Cost Center
        public DataSet ListIACDetails (object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                
                string[] paramname = new string[4];
                SqlDbType[] paramtype = new SqlDbType[4];

                paramname[0] = "IACCode";
                paramname[1] = "ApprovalNo";
                paramname[2] = "FromDate";
                paramname[3] = "ToDate";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.DateTime;
                paramtype[3] = SqlDbType.DateTime;
                
                DataSet result = da.SelectRecords("sp_ListIACMaster", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region
        public void IACMasterInsert(object[] values)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[3];
                SqlDbType[] paramtype = new SqlDbType[3];
                paramname[0] = "ApprovalNo";
                paramname[1] = "CreatedOn";
                paramname[2] = "CreatedBy";
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.DateTime;
                paramtype[2] = SqlDbType.VarChar;
                bool result = da.InsertData("sp_IACMasterInsertUserLog", paramname, paramtype, values);
                
            }
            catch(Exception ex)
            {
            }
        }
        #endregion
        #region
        public void IACMasterUpdate(object[] values)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[3];
                SqlDbType[] paramtype = new SqlDbType[3];
                paramname[0] = "ApprovalNo";
                paramname[1] = "UpdatedOn";
                paramname[2] = "UpdatedBy";
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.DateTime;
                paramtype[2] = SqlDbType.VarChar;
                bool result = da.InsertData("sp_IACMasterUpdateUserLog", paramname, paramtype, values);

            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Add New Cost Center
        public DataSet InsertIACCode(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[11];
                SqlDbType[] paramtype = new SqlDbType[11];

                paramname[0] = "IACCode";
                paramname[1] = "IACName";
                paramname[2] = "ApprovalNo";
                paramname[3] = "City";
                paramname[4] = "State";
                paramname[5] = "Country"; 
                paramname[6] = "Zip";
                paramname[7] = "IsActive";
                paramname[8] = "ExpDate";
                paramname[9] = "UpdatedOn";
                paramname[10] = "UpdatedBy";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;              
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.Bit;
                paramtype[8] = SqlDbType.DateTime;
                paramtype[9] = SqlDbType.DateTime;
                paramtype[10] = SqlDbType.VarChar;

                DataSet result = da.SelectRecords("sp_SaveIACCode", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Delete
        public DataSet DeleteIACCode(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[1];
                SqlDbType[] paramtype = new SqlDbType[1];

                paramname[0] = "ApprovalNo";

                paramtype[0] = SqlDbType.VarChar;

                DataSet result = da.SelectRecords("sp_DeleteIACCode", paramname, paramvalue, paramtype);
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
