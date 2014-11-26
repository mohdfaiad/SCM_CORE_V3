using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class BALOC
    {
        #region Variables
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet result;
        #endregion

        #region Costructor
        public BALOC()
        {
            constr = Global.GetConnectionString();
        }
        #endregion

        #region List Oc Details
        public DataSet dsOcDetails(string CurrCode, string CurrName)
        {
            SQLServer da = new SQLServer(constr);

            //if (CurrDescription == "") CurrDescription = null;
            //if (CurrCode == "") CurrCode = null;

            string[] ColumnNames = new string[2];
            SqlDbType[] DataType = new SqlDbType[2];
            Object[] Values = new object[2];
            int i = 0;
            ColumnNames.SetValue("OcCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);

            Values.SetValue(CurrCode, i);
            i++;

            ColumnNames.SetValue("OcDesc", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(CurrName, i);

            DataSet ds = da.SelectRecords("spListOcDetails", ColumnNames, Values, DataType);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            return null;
        }
        #endregion List Oc Details

        #region MODIFY OC
        public bool ModifyOc(string Type, string Code, string desc, DateTime now, string uname)
        {
            SQLServer da = new SQLServer(constr);

            //if (CurrDescription == "") CurrDescription = null;
            //if (CurrCode == "") CurrCode = null;

            string[] ColumnNames = new string[5];
            SqlDbType[] DataType = new SqlDbType[5];
            Object[] Values = new object[5];
            int i = 0;

            ColumnNames.SetValue("SPType", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            if (Type.Trim() == "insert")
                Values.SetValue("insert", i);
            else if (Type.Trim() == "delete")
                Values.SetValue("delete", i);
            i++;

            ColumnNames.SetValue("OcCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(Code, i);
            i++;

            ColumnNames.SetValue("OcDesc", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(desc, i);
            i++;

            ColumnNames.SetValue("createdon", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(now, i);
            i++;

            ColumnNames.SetValue("createdby", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(uname, i);
            i++;

            if (da.ExecuteProcedure("sp_InsertUpdateOc", ColumnNames, DataType, Values))
            {
                return true;
            }
            return false;
        }
        #endregion MODIFY OC
    }
}
