using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALCurrency
    {
        #region Variables
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet result;
        #endregion

        #region Costructor
        public BALCurrency()
        {
            constr = Global.GetConnectionString();
        }
        #endregion

        #region GetCurrency
        public DataSet GetCurrencyCodeList(string CurrencyCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetCurrencyCodeList");
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
                return (null);
            }
            return (null);
        }
        #endregion

        #region List Currency Details
        public DataSet dsCurrDetails(string CurrCode, string CurrName)
        {
            SQLServer da = new SQLServer(constr);

            //if (CurrDescription == "") CurrDescription = null;
            //if (CurrCode == "") CurrCode = null;

            string[] ColumnNames = new string[2];
            SqlDbType[] DataType = new SqlDbType[2];
            Object[] Values = new object[2];
            int i = 0;
            ColumnNames.SetValue("CurrCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);

            Values.SetValue(CurrCode, i);
            i++;

            ColumnNames.SetValue("CurrName", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(CurrName, i);

            DataSet ds = da.SelectRecords("spListCurrencyDetails", ColumnNames, Values, DataType);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            return null;
        }
        #endregion

        #region MODIFY Currency
        public bool ModifyCurrency(string Type, string CurrencyCode, string CurrencyName,DateTime now,string uname)
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
            if(Type.Trim() == "insert")
            Values.SetValue("insert", i);
            else if(Type.Trim() == "delete")
            Values.SetValue("delete", i);
            i++;

            ColumnNames.SetValue("CurrCode", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(CurrencyCode, i);
            i++;

            ColumnNames.SetValue("CurrName", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(CurrencyName, i);
            i++;

            ColumnNames.SetValue("createdon", i);
            DataType.SetValue(SqlDbType.DateTime, i);
            Values.SetValue(now, i);
            i++;

            ColumnNames.SetValue("createdby", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(uname, i);
            i++;

            if (da.ExecuteProcedure("sp_InsertUpdateCurrency", ColumnNames, DataType, Values))
            {
                return true;
            }
            return false;
        }
        #endregion

        //#region List Country
        //public DataTable dsListCountries()
        //{ 
        //    BALCountryMaster objBALCM = new BALCountryMaster();
        //    object[] Obj = new object[3];
        //    Obj.SetValue(null, 0); Obj.SetValue(null, 1); Obj.SetValue(null, 2);
        //    DataSet ds = new DataSet();
        //    ds = objBALCM.GetCountryList(Obj);
        //    if (ds != null)
        //    {
        //        if (ds.Tables != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                return ds.Tables[0].DefaultView.ToTable(true,"CountryCode");
        //            }
        //        }
        //    }
        //    return null;
        //}
        //#endregion

        //#region GetListCountryCode
        //public DataSet GetListCountryCode()
        //{
        //    try
        //    {
        //        // Get Partner Type Master
        //        SQLServer da = new SQLServer(constr);
        //        string Query = "select 'Select' as Code,'1' as CountryID union SELECT CountryCode as Code, CountryID from CountryMaster order by [CountryID]";
        //        DataSet ds = da.GetDataset(Query);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        //#endregion
    }
}
