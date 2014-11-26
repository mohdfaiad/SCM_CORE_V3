using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALTaxCode
    {
        #region Variables
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet result;
        #endregion

        #region Costructor
        public BALTaxCode()
        {
            constr = Global.GetConnectionString();
        }
        #endregion

        #region TaxCode List
        public DataSet dsTaxCodeDetails(string CurrCode, string CurrName)
        {
            SQLServer da = new SQLServer(constr);


            string[] ColumnNames = new string[2];
            SqlDbType[] DataType = new SqlDbType[2];
            Object[] Values = new object[2];

            int i = 0;
            ColumnNames.SetValue("TaxCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);

            Values.SetValue(CurrCode, i);
            i++;

            ColumnNames.SetValue("TaxCodeDesc", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(CurrName, i);

            DataSet ds = da.SelectRecords("spListTaxCodeDetails", ColumnNames, Values, DataType);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            return null;
        }
        #endregion

        #region Modify TaxCode
        public bool ModifyTaxCode(string Type, string TaxCode, string TaxCodeDescription, DateTime createdon, string createdby, DateTime UpdatedOn, string UpdatedBy)
        {
            SQLServer da = new SQLServer(constr);

            try
            {
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;

                ColumnNames.SetValue("SPType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                if (Type.Trim() == "insert")
                    Values.SetValue("insert", i);
                else if (Type.Trim() == "delete")
                    Values.SetValue("delete", i);
                i++;

                ColumnNames.SetValue("TaxCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(TaxCode, i);
                i++;

                ColumnNames.SetValue("TaxCodeDescription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(TaxCodeDescription, i);
                i++;

                ColumnNames.SetValue("createdon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(createdon, i);
                i++;

                ColumnNames.SetValue("createdby", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(createdby, i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UpdatedOn, i);
                i++;

                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatedBy, i);
                i++;

                if (da.ExecuteProcedure("sp_InsertUpdateTaxCode", ColumnNames, DataType, Values))
                {
                    return true;
                }
            }

            catch (Exception ex)
            { 
            
            }

            return false;
        }
        #endregion

    }
}
