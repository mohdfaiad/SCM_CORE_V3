using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;

namespace BAL
{
    public class BALIRCodes
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALIRCodes()
        {
            constr = Global.GetConnectionString();
        }

        # region Add IR Code
        public int AddIRCode(object[] IRCodeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("ircode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(IRCodeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("irdes", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(IRCodeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(IRCodeInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("expimp", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(IRCodeInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spAddIRCode", ColumnNames, DataType, Values))
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
        # endregion AddRegion

        # region Update IR Code
        public int UpdateIRCode(object[] UpdateIRCodeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                //0
                ColumnNames.SetValue("irc", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateIRCodeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ird", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateIRCodeInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("isactive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateIRCodeInfo.GetValue(i), i);
                i++;



                //3
                ColumnNames.SetValue("ExpImp", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateIRCodeInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("serialno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(UpdateIRCodeInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spUpdateIRCode", ColumnNames, DataType, Values))
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
        # endregion UpdateCommodity

        # region Get IR Code List
        public DataSet GetIRCOdesList(object[] IRInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];

                int i = 0;

                ColumnNames.SetValue("ircode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(IRInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("expimp", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(IRInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("irDesc", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(IRInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(IRInfo.GetValue(i), i);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetIRCList", ColumnNames, Values, DataType);
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
