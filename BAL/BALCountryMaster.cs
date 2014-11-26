using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALCountryMaster
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALCountryMaster()
        {
            constr = Global.GetConnectionString();
        }

        # region AddCountry
        public int AddCountry(object[] CountryInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("CountryName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CurrencyCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

             

                if (!da.ExecuteProcedure("SpAddCountryRecords", ColumnNames, DataType, Values))
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
        # endregion AddCountry

        # region Get Currency Code 
        public DataSet GetCurrencyCodeList(string CurrencyCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetCurrenyCode"); //Old SP to get Currency list-[GetCurrencyCodeList]
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

        # region Get Country List 
        public DataSet GetCountryList(object[] CountryListInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //1
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("CountryName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("CurrencyCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;
                //4
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);


                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetCountryList", ColumnNames, Values, DataType);
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

        # endregion Get Country List

        # region UpdateCountry
        public int UpdateCountry(object[] UpdateCountryInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("CountryName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CurrencyCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;



                if (!da.ExecuteProcedure("SpUpdateCountryRecords", ColumnNames, DataType, Values))
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
        # endregion UpdateCountry

        # region chkCountryCode
        public DataSet chkCountryCode(object[] CountrycodeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

               
                //2
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountrycodeInfo.GetValue(i), i);
                i++;





                DataSet ds = new DataSet();
                ds = da.SelectRecords("SPCheckCountryCode", ColumnNames, Values, DataType);
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

        # endregion chkCountryCode

    }
}
