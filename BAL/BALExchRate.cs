using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;

namespace BAL
{
    public class BALExchRate
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALExchRate()
        {
            constr = Global.GetConnectionString();
        }

        # region Get Currency Code
        public DataSet GetCurrList()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                //string[] ColumnNames = new string[1];
                //SqlDbType[] DataType = new SqlDbType[1];
                //Object[] Values = new object[1];
                //int i = 0;

                ////1
                //ColumnNames.SetValue("participationtype", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(PrefixInfo.GetValue(i), i);
                //i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetCurrenyCode");
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

        # endregion Get Currency Code

        #region Get Exchange Rate Type
        public DataSet GetExchrate()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetExchRateTypes");
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

        #endregion
        
        # region Get Exch Rate List
        public DataSet GetExchList(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("cuurrCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("rateType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("validFrom", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("validTo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetExchRateList",ColumnNames,PrefixInfo,DataType);
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
        # endregion Get Exch Rate List

        # region Add Exch Rate
        public int AddExchRate(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;

                //1
                ColumnNames.SetValue("currcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("usrt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("dtfrm", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("dtto", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("uploadby", i);
                DataType.SetValue(SqlDbType.VarChar , i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("uploadon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("type", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spAddExchRates", ColumnNames, DataType, Values))
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

        # region Delete
        public int DeleteExchDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("id", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spDeleteExchDetail", ColumnNames, DataType, Values))
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
        # endregion Delete

        # region Update Exch Detail
        public int UpdateExchDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;

                //1
                ColumnNames.SetValue("id", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("currcode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("iatart", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("validfrm", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("validto", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("updateby", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("updateon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("type", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spUpdateExchDetail", ColumnNames, DataType, Values))
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
    }
}
