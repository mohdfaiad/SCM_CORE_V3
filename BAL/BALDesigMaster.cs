using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;

namespace BAL
{
    public class BALDesigMaster
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALDesigMaster()
        {
            constr = Global.GetConnectionString();
        }

        # region Add Record
        public int AddDesigCode(object[] DesigCodeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                //0
                ColumnNames.SetValue("prefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(DesigCodeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("code", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(DesigCodeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(DesigCodeInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddDesigCode", ColumnNames, DataType, Values))
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

        # region Update Record
        public int UpdateDesigCode(object[] DesigCodeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("Prefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(DesigCodeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Code", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(DesigCodeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("IsAct", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(DesigCodeInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.BigInt, i);
                Values.SetValue(DesigCodeInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spUpdateDesigCode", ColumnNames, DataType, Values))
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


        # region Get Record List
        public DataSet GetDesigCodeList()
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                //string[] ColumnNames = new string[1];
                //SqlDbType[] DataType = new SqlDbType[1];
                //Object[] Values = new object[1];

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetDeisgCodeList");
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

        # endregion

    }
}
