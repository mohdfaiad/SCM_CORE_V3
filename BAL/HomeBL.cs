using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QID.DataAccess;
using System.Data;
namespace BAL
{
    public class HomeBL
    {
        #region Variables
        string constr = "";
        #endregion Variables

        #region Constructor
        public HomeBL()
        {
           constr = Global.GetConnectionString();
        }
        #endregion Constructor

        #region Get User Roll Details
        public DataSet GetUserRollDetails(object[] LoginInfo)
        {
            try
            {
                //Prepare column names and datatypes for search parameters...
                string[] paramNames = new string[1];
                SqlDbType[] dataTypes = new SqlDbType[1];
                int i = 0;

                //0
                paramNames.SetValue("UserName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //Get user roll details...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetUserRollDetails", paramNames, LoginInfo, dataTypes);
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
        #endregion Get User Roll Details

        #region Get User Permissions
        public DataSet GetUserPermissions(string PageName, int RoleId)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);

            string[] QueryPname = new string[2];
            object[] QueryValue = new object[2];
            SqlDbType[] QueryType = new SqlDbType[2];

            try
            {
                QueryPname[0] = "strPageName";
                QueryType[0] = SqlDbType.VarChar;
                QueryValue[0] = PageName;

                QueryPname[1] = "intRoleId";
                QueryType[1] = SqlDbType.Int;
                QueryValue[1] = RoleId;

                ds = da.SelectRecords("spGetPermissionsforRole", QueryPname, QueryValue, QueryType);

                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                da = null;
                QueryPname = null;
                QueryValue = null;
                QueryType = null;
            }
        }
        #endregion Get User Details
    }
}
