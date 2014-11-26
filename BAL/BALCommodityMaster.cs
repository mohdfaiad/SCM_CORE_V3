using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALCommodityMaster
    {
         SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALCommodityMaster()
        {
            constr = Global.GetConnectionString();
        }

        # region AddCommodity
        public int AddCommodity(object[] CommodityInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                //0
                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("CommodityName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CommodityDescription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("CommCategory", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("IsNoTOC", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Priority", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SHCCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityInfo.GetValue(i), i);


                if (!da.ExecuteProcedure("SpAddCommodityRecords", ColumnNames, DataType, Values))
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

        # region UpdateCommodity
        public int UpdateCommodity(object[] UpdateCommodityInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[10];
                SqlDbType[] DataType = new SqlDbType[10];
                Object[] Values = new object[10];
                int i = 0;

                //0
                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("CommodityName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CommodityDescription", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("CommCategory", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("IsShipper", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("IsNoTOC", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Priority", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SHCCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCommodityInfo.GetValue(i), i);

                if (!da.ExecuteProcedure("SpUpdateCommodityRecords", ColumnNames, DataType, Values))
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

        # region Get Commodity List
        public DataSet GetCommodityList(object[] CommodityListInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;

                //0
                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;
               
                //1
                ColumnNames.SetValue("CategorySrNo", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;
                //2
                ColumnNames.SetValue("CommodityName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("IsNoTOC", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;
                //4
                ColumnNames.SetValue("SHCCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;
                //5
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;
                //6
                ColumnNames.SetValue("Priority", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("Description", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CommodityListInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetCommodityList_Filter", ColumnNames, Values, DataType);
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
        # endregion Get Commodity List
                
        # region Delete
        public int DeleteCommodityDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spDeleteCommodityDetail", ColumnNames, DataType, Values))
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

        #region SHCCode
        public DataSet GetSHCCode()
        { 
        try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetSpecialHandlingCode");
                
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return (ds);
                    }
                }
            }
        catch (Exception ex)
        {
        }
        return (null);
        }

        #endregion
        #region Commodity Category
        public DataSet GetCommodityCategory()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetCommodityCategory");
                
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return (ds);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return (null);
        }
        #endregion
    }
}
