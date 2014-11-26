using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using SCM.Common.Struct;
using System.Data;

namespace BAL
{
    public class BALCommon
    {
        SQLServer db = new SQLServer("");
        string constr = "";

        public BALCommon()
        {
            constr = Global.GetConnectionString();
        }

        public bool SaveOperationalTimeStamp(List<clsOperationTimeStamp> ListOperationTimeStamp)
        {
            SQLServer da = new SQLServer(constr);
            string[] ColumnNames = new string[8];
            SqlDbType[] DataType = new SqlDbType[8];
            Object[] Values = new object[8];
            StringBuilder strAWBDetails = new StringBuilder();

            try
            {
                if (ListOperationTimeStamp != null && ListOperationTimeStamp.Count > 0)
                {
                    for (int intCount = 0; intCount < ListOperationTimeStamp.Count; intCount++)
                    {
                        strAWBDetails.Append("Insert into #temp (AWBNumber, AWBPrefix, ULDNumber, Pieces, Weight, FlightNo, FligtDt) Values('");
                        strAWBDetails.Append(ListOperationTimeStamp[intCount].AWBNumber);
                        strAWBDetails.Append("','");
                        strAWBDetails.Append(ListOperationTimeStamp[intCount].AWBPrefix);
                        strAWBDetails.Append("','");
                        strAWBDetails.Append(ListOperationTimeStamp[intCount].ULDNumber);
                        strAWBDetails.Append("',");
                        strAWBDetails.Append(ListOperationTimeStamp[intCount].Pieces);
                        strAWBDetails.Append(",");
                        strAWBDetails.Append(ListOperationTimeStamp[intCount].Weight);
                        strAWBDetails.Append(",'");
                        strAWBDetails.Append(ListOperationTimeStamp[intCount].FlightNo);

                        if (ListOperationTimeStamp[intCount].FlightNo != "")
                        {
                            strAWBDetails.Append("','");
                            strAWBDetails.Append(ListOperationTimeStamp[intCount].FlightDt);
                            strAWBDetails.Append("'); ");
                        }
                        else
                        {
                            strAWBDetails.Append("',");
                            strAWBDetails.Append("null");
                            strAWBDetails.Append("); ");
                        }
                    }
                }

                int i = 0;

                //0
                ColumnNames.SetValue("OperationalType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ListOperationTimeStamp[0].OperationalType, i);
                i++;

                //1
                ColumnNames.SetValue("OperationalStatus", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ListOperationTimeStamp[0].OperationalStatus, i);
                i++;

                //2
                ColumnNames.SetValue("Station", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ListOperationTimeStamp[0].Station, i);
                i++;

                //3
                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ListOperationTimeStamp[0].UpdatedBy, i);
                i++;

                //4
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(ListOperationTimeStamp[0].UpdatedOn, i);
                i++;

                //5
                ColumnNames.SetValue("AWBDetails", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(strAWBDetails.ToString(), i);
                i++;

                //6
                ColumnNames.SetValue("OperationDt", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ListOperationTimeStamp[0].OperationDate, i);
                i++;

                //7
                ColumnNames.SetValue("OperationTime", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ListOperationTimeStamp[0].OperationTime, i);
                //i++;
                ListOperationTimeStamp = null;
                if (!da.ExecuteProcedure("sp_SaveOperationalTimeStamp", ColumnNames, DataType, Values))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                da = null;
                ColumnNames = null;
                DataType = null;
                Values = null;
                strAWBDetails = null;
            }
        }

        public DateTime GetLastOperationalTimeStamp(List<clsOperationTimeStamp> ListOperationTimeStamp)
        {
            SQLServer da = new SQLServer(constr);
            string[] ColumnNames = new string[6];
            SqlDbType[] DataType = new SqlDbType[6];
            Object[] Values = new object[6];
            DateTime dt;
            try
            {

                dt = Convert.ToDateTime("01-JAN-1900");

                if (ListOperationTimeStamp != null && ListOperationTimeStamp.Count == 1)
                {

                    int i = 0;

                    //0
                    ColumnNames.SetValue("OperationalType", i);
                    DataType.SetValue(SqlDbType.VarChar, i);
                    Values.SetValue(ListOperationTimeStamp[0].OperationalType, i);
                    i++;

                    //1
                    ColumnNames.SetValue("OperationalStatus", i);
                    DataType.SetValue(SqlDbType.VarChar, i);
                    Values.SetValue(ListOperationTimeStamp[0].OperationalStatus, i);
                    i++;

                    //2
                    ColumnNames.SetValue("Station", i);
                    DataType.SetValue(SqlDbType.VarChar, i);
                    Values.SetValue(ListOperationTimeStamp[0].Station, i);
                    i++;

                    //5
                    ColumnNames.SetValue("AWBPrefix", i);
                    DataType.SetValue(SqlDbType.VarChar, i);
                    Values.SetValue(ListOperationTimeStamp[0].AWBPrefix, i);
                    i++;

                    //6
                    ColumnNames.SetValue("AWBNumber", i);
                    DataType.SetValue(SqlDbType.VarChar, i);
                    Values.SetValue(ListOperationTimeStamp[0].AWBNumber, i);
                    i++;

                    //7
                    ColumnNames.SetValue("ULDNumber", i);
                    DataType.SetValue(SqlDbType.VarChar, i);
                    Values.SetValue(ListOperationTimeStamp[0].ULDNumber, i);
                    i++;
                   
                    string strDt = "";
                    strDt = da.GetStringByProcedure("sp_GetOperationalTimeStamp", ColumnNames, Values, DataType);
                    ListOperationTimeStamp = null;
                    if (strDt != null && strDt != "")
                    {
                        if (DateTime.TryParseExact(strDt,"dd/MM/yyyy HH:mm",null, 
                            System.Globalization.DateTimeStyles.None, out dt))
                        {
                            return (dt);
                        }
                        else
                        {
                            return (Convert.ToDateTime("01-JAN-1900"));
                        }
                    }
                }
                dt = Convert.ToDateTime("01-JAN-1900");
            }
            catch (Exception ex)
            {
                dt = Convert.ToDateTime("01-JAN-1900");
            }
            finally
            {
                da = null;
                ColumnNames = null;
                DataType = null;
                Values = null;
            }
            return (dt);
        }

    }
}
