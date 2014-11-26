using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALCCSFMaster 
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables

        #region Get Cost Center
        public DataSet ListCCSFDetails(object[] paramvalue)
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
                
                DataSet result = da.SelectRecords("sp_ListCCSFMaster", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region
        public void CCSFMasterInsert(object[] values)
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
                bool result = da.InsertData("sp_CCSFMasterInsertUserLog", paramname, paramtype, values);
                
            }
            catch(Exception ex)
            {
            }
        }
        #endregion

        #region
        public void CCSFMasterUpdate(object[] values)
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
                bool result = da.InsertData("sp_CCSFMasterUpdateUserLog", paramname, paramtype, values);

            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Add New Cost Center
        public DataSet InsertCCSFCode (object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[14];
                SqlDbType[] paramtype = new SqlDbType[14];

                paramname[0] = "CCSFCode";
                paramname[1] = "CCSFName";
                paramname[2] = "IACCode";
                paramname[3] = "IACName";
                paramname[4] = "ApprovalNo";
                paramname[5] = "City";
                paramname[6] = "State";
                paramname[7] = "Country"; 
                paramname[8] = "Zip";
                paramname[9] = "IsActive";
                paramname[10] = "ExpDate";
                paramname[11] = "StreetAddress";
                paramname[12] = "UpdatedOn";
                paramname[13] = "UpdatedBy";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;              
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.Bit;
                paramtype[10] = SqlDbType.DateTime;
                paramtype[11] = SqlDbType.VarChar;
                paramtype[12] = SqlDbType.DateTime;
                paramtype[13] = SqlDbType.VarChar;

                DataSet result = da.SelectRecords("sp_SaveCCSFCode", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Delete
        public DataSet DeleteCCSFCode (object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[1];
                SqlDbType[] paramtype = new SqlDbType[1];

                paramname[0] = "ApprovalNo";

                paramtype[0] = SqlDbType.VarChar;

                DataSet result = da.SelectRecords("sp_DeleteCCSFCode", paramname, paramvalue, paramtype);
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
