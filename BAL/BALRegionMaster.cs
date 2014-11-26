using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALRegionMaster
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALRegionMaster()
        {
            constr = Global.GetConnectionString();
        }

        # region AddRegion
        public int AddRegion(object[] RegionInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                //0
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("RegionName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("CountryName",i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionInfo.GetValue(i), i);
                i++;



                if (!da.ExecuteProcedure("SpAddRegionRecords", ColumnNames, DataType, Values))
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

        # region Get Country Code
        public DataSet GetCountryCodeList(string CountryCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("SP_GetAllStationCodeName", "level", "Country", SqlDbType.VarChar); //Old SP to get Country Code-GetCountryCodeList
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
        # endregion Get Country Code

        # region Get Country Name
        public DataSet GetCountryName(object [] CountryInfo)
        {
            try
            {

                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetCountryName", ColumnNames, Values, DataType);
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

        # endregion  Get Country Name

        # region Get Region List
        public DataSet GetRegionList(object[] RegionListInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //1
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionListInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("RegionName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionListInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionListInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionListInfo.GetValue(i), i);



                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetRegionList",ColumnNames,Values,DataType);
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

        # endregion Get Region List

        # region UpdateREgion
        public int UpdateRgion(object[] UpdateRegionInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                //0
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateRegionInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("RegionName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateRegionInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateRegionInfo.GetValue(i), i);
                i++;


                //2
                ColumnNames.SetValue("CountryName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateRegionInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateRegionInfo.GetValue(i), i);
                i++;



                if (!da.ExecuteProcedure("SpUpdateRegionRecords", ColumnNames, DataType, Values))
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
        # endregion UpdateRegion

        # region chkRegionWithCountry
        public DataSet chkRegionWithCountry(object[] RegionCountryInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                //1
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionCountryInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RegionCountryInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("SPCheckRegionWithCountry", ColumnNames, Values, DataType);
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
        # endregion chkRegionWithCountry

    }
}
