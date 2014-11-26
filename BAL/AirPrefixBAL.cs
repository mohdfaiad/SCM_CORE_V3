using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;

namespace BAL
{
    public class AirPrefixBAL
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public AirPrefixBAL()
        {
            constr = Global.GetConnectionString();
        }

        # region Add Prefix
        public int AddPrefix(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                //0
                ColumnNames.SetValue("code", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("prefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddPrefix", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion

        # region Update Prefix
        public int UpdatePrefix(object[] UpdatePrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                //0
                ColumnNames.SetValue("Code", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatePrefixInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Prefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatePrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("SrNo", i);
                DataType.SetValue(SqlDbType.BigInt, i);
                Values.SetValue(UpdatePrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spUpdatePrefix", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion


        # region Get Product Type List
        public DataSet GetProductTypeList()
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetPrefixList");
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
            catch (Exception ex)
            {
            }
            return (null);
        }

        # endregion Get Product Type List

    }
}
