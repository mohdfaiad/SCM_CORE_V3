using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALRoutes
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALRoutes()
        {
            constr = Global.GetConnectionString();
        }
        
        # region Get Airport Codes
        public DataSet GetAirportCodes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetAirportCodes");
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

        # endregion Get Airport Codes

        # region Get Selected Route
        public DataSet GetSelectedRoute(object[] RouteInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                //0
                ColumnNames.SetValue("source", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetRouteList", ColumnNames, Values, DataType);
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
        # endregion Get Selected Route

        #region Add Route
        public int AddRoute(object[] RouteInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;
                
                //0
                ColumnNames.SetValue("origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("dest", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("route", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddNewRoute", ColumnNames, DataType, Values))
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
        #endregion Add Route

        #region Update
        public int UpdateRoute(object[] RouteInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;

                //0
                ColumnNames.SetValue("origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("dest", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("route", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("SrNo", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RouteInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spUpdateRoute", ColumnNames, DataType, Values))
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

        # region Delete
        public int DeleteRouteDetail(object[] PrefixInfo)
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


                if (!da.ExecuteProcedure("spDeleteRouteDetail", ColumnNames, DataType, Values))
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

    }
}
