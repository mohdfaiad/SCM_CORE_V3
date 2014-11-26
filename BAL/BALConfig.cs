using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;

namespace BAL
{ 
    
    public class BALConfig
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALConfig()
        {
            constr = Global.GetConnectionString();
        }
        # region Get Currency Code
        public DataSet GetCurrencyCodes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
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

        # endregion Get Currency Codes

        #region Get Desig Code
        public DataSet GetDesigCodes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetDesigCodes");
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

        # region Add Prefix
        public int AddPrefix(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
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

                //2
                ColumnNames.SetValue("currency", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddPrefixCode", ColumnNames, DataType, Values))
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
        #endregion Get Exchange Rate Type

        #region Add Exch Rate Type
        public int AddExchRate(object[] ExchRateInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //0
                ColumnNames.SetValue("currtype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ExchRateInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddExchRate", ColumnNames, DataType, Values))
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
        #endregion

        #region Remove Exch rate
        public int RemoveExchRate(object[] ExchRateInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //0
                ColumnNames.SetValue("currtype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(ExchRateInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spRemoveExchRate", ColumnNames, DataType, Values))
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
        #endregion

        #region Add Cnote
        public int AddCnote(object[] CnoteInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                //0
                ColumnNames.SetValue("cnoteType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CnoteInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("cnoteVaild", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CnoteInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spAddCnote", ColumnNames, DataType, Values))
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
        #endregion


    }
   
    
}
