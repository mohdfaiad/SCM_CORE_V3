using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALCategoryMaster
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables

        #region Get Category Code
        public DataSet GetComCategoryList(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                
                string[] paramname = new string[3];
                SqlDbType[] paramtype = new SqlDbType[3];

                paramname[0] = "CategoryName";
                paramname[1] = "CategoryDesc";
                paramname[2] = "IsActive";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.Bit;
                
                DataSet result = da.SelectRecords("sp_ListComCategory", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Add New Category
        public DataSet InsertCategory(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[3];
                SqlDbType[] paramtype = new SqlDbType[3];

                paramname[0] = "CategoryName";
                paramname[1] = "CategoryDesc";
                paramname[2] = "IsActive";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.Bit;

                DataSet result = da.SelectRecords("sp_SaveComCategory", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Delete
        public DataSet DeleteCategory(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[1];
                SqlDbType[] paramtype = new SqlDbType[1];

                paramname[0] = "CategoryName";

                paramtype[0] = SqlDbType.VarChar;

                DataSet result = da.SelectRecords("sp_DeleteComCat", paramname, paramvalue, paramtype);
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
