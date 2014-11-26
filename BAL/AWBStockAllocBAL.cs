using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Data;
using QID.DataAccess;


namespace BAL
{
    public class AWBStockAllocBAL
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #endregion Variables

        public DataSet FillAWBStockAlloc_ALL()
       {
           try
           {
               DataSet ds = db.SelectRecords("SP_GetAWBStockAlloc_ALL");
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

           catch (Exception)
           {

           }
           return (null);
       }

        public DataSet FillAWBStockAlloc(object[] RateListInfo)
        {
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateListInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("CityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateListInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetAWBStockAlloc", ColumnNames, Values, DataType);
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

            catch (Exception)
            {

            }
            return (null);
        }


        # region AddAWBStocAllol
        public string AddAWBStockAlloc(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("CityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("AWBFrom", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("AWBTo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_InsertAWBStockAlloc", ColumnNames, Values, DataType);
                return res;

                //if (!db.ExecuteProcedure("SP_InsertRateCard", ColumnNames, DataType, Values))
                //    return (-1);
                //else
                //{
                //    return (0);
                //}


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion AddAWBStockAlloc

        #region Get Region
        public DataSet GetRegion()
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetRegionCodeName");
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
            catch (Exception)
            {
            }
            return (null);
        }
        #endregion Get Region

        #region Get City
        public DataSet GetCity()
        {
            try
            {
                //Get agent codes...

                //string[] ColumnNames = new string[1];
                //SqlDbType[] DataType = new SqlDbType[1];
                //Object[] Values = new object[1];
                //SQLServer da = new SQLServer(constr);

                //ColumnNames[0] = "RegionCode";
                //DataType[0] = SqlDbType.VarChar;
                //Values[0] = regionCode;

                //DataSet ds = da.SelectRecords("SP_GetCityCodeName", ColumnNames, Values, DataType);

                SQLServer da = new SQLServer(constr);

                DataSet ds = da.SelectRecords("SP_GetCityCodeName");
                
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
            catch (Exception)
            {
            }
            return (null);
        }
        #endregion Get City

        #region Get City Region wise
        public DataSet GetCityFiltered(string regionCode)
        {
            try
            {
                
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                SQLServer da = new SQLServer(constr);

                ColumnNames[0] = "RegionCode";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = regionCode;

                DataSet ds = da.SelectRecords("SP_GetCityFromRegion", ColumnNames, Values, DataType);

                //SQLServer da = new SQLServer(constr);

                //DataSet ds = da.SelectRecords("SP_GetCityCodeName");

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
            catch (Exception)
            {
            }
            return (null);
        }
        #endregion Get City
    }
}
