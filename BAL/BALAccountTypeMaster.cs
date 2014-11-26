using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALAccountTypeMaster
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables

        #region Get Account Type
        public DataSet ListAccountType(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                
                string[] paramname = new string[3];
                SqlDbType[] paramtype = new SqlDbType[3];

                paramname[0] = "AccountTypeName";
                paramname[1] = "AccountTypeID";
                paramname[2] = "IsActive";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.Bit;
                
                DataSet result = da.SelectRecords("sp_ListAccountType", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Add New Account Type
        public DataSet InsertAccountType(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[8];
                SqlDbType[] paramtype = new SqlDbType[8];

                paramname[0] = "AccountTypeName";
                paramname[1] = "AccountTypeDescription";
                paramname[2] = "AccountTypeID";
                paramname[3] = "UpdatedBy";
                paramname[4] = "UpdatedOn";
                paramname[5] = "IsActive";
                paramname[6] = "CategoryID";
                paramname[7] = "IsSystem";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.DateTime;
                paramtype[5] = SqlDbType.Bit;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.Bit;

                DataSet result = da.SelectRecords("sp_SaveAccountType", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Delete
        public DataSet DeleteAccountType(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[1];
                SqlDbType[] paramtype = new SqlDbType[1];

                paramname[0] = "AccountTypeID";

                paramtype[0] = SqlDbType.VarChar;

                DataSet result = da.SelectRecords("sp_DeleteAccountType", paramname, paramvalue, paramtype);
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
