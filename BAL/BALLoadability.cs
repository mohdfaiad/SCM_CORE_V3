using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Data.SqlClient;


namespace BAL
{
    public class BALLoadability
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;
        public BALLoadability()
        { constr = Global.GetConnectionString(); }

        #region Aircraft Type List
        public DataSet GetAircraftTypeList()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetAirCraftType");
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

        # region Get Loadablity List
        public DataSet GetLoadablityList(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;

                //1
                ColumnNames.SetValue("type", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("TailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("Compartment", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("ContainerType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("FloorLamination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetLoadabilityList" ,ColumnNames, Values, DataType);
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
        # endregion Get Get Loadablity List

        # region Add Loadability Master
        public int AddLoadabilityDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[17];
                SqlDbType[] DataType = new SqlDbType[17];
                Object[] Values = new object[17];
                int i = 0;

                //1
                ColumnNames.SetValue("validfrm", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("validto", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("aircrafttype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("length", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("width", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("height", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("unit", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("TailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("Compartment", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("MaxWeight", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("MaxVolWeight", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("MaxContainers", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("ContainerType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("MaxPallets96", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("MaxPallets88", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("FloorLamination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddAircraftType", ColumnNames, DataType, Values))
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
        # endregion Add Loadability Master

        # region Update Loadability Master
        public int UpdateLoadabilityDetail(object[] PrefixInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[18];
                SqlDbType[] DataType = new SqlDbType[18];
                Object[] Values = new object[18];
                int i = 0;

                //0
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("validfrm", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("validto", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("aircrafttype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("length", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("width", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("height", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("unit", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("isact", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("TailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("Compartment", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("MaxWeight", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("MaxVolWeight", i);
                DataType.SetValue(SqlDbType.Decimal, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("MaxContainers", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("ContainerType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("MaxPallets96", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("MaxPallets88", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("FloorLamination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(PrefixInfo.GetValue(i), i);
                i++;


                if (!da.ExecuteProcedure("spUpdateAircraftType", ColumnNames, DataType, Values))
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
        # endregion Add Loadability Master

        # region Delete
        public int DeleteLoadDetail(object[] PrefixInfo)
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


                if (!da.ExecuteProcedure("spDeleteLoadablityDetail", ColumnNames, DataType, Values))
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
