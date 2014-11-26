using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class AircraftTonnageMasterBAL
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public AircraftTonnageMasterBAL()
        {
            constr = Global.GetConnectionString();
        }

        # region chkAircraftTonnageList
        public DataSet chkAircraftTonnageList(object[] CountrycodeInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;


                //0
                ColumnNames.SetValue("AircraftType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountrycodeInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ValidFrom", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(CountrycodeInfo.GetValue(i), i);
                i++;
                
                //2
                ColumnNames.SetValue("ValidTo", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(CountrycodeInfo.GetValue(i), i);
                i++;





                DataSet ds = new DataSet();
                ds = da.SelectRecords("SPCheckAircraftTonnageList", ColumnNames, Values, DataType);
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

        # endregion chkAircraftTonnageList

        # region AddAircraftTonnageRecords
        public int AddAircraftTonnageRecords(object[] CountryInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                //0
                ColumnNames.SetValue("AircraftType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("ValidFrom", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("ValidTo", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //5
                
                ColumnNames.SetValue("Weight", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;
                

                //6
                ColumnNames.SetValue("Volume", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("VolUnit", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("SpAddAircraftTonnageMasterRecords", ColumnNames, DataType, Values))
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
        # endregion AddAircraftTonnageRecords

        # region UpdateAircraftTonnageRecords
        public int UpdateAircraftTonnageRecords(object[] UpdateCountryInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[10];
                SqlDbType[] DataType = new SqlDbType[10];
                Object[] Values = new object[10];
                int i = 0;

                //0
                ColumnNames.SetValue("AircraftType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("ValidFrom", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("ValidTo", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("Weight", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("Volume", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //8 
                ColumnNames.SetValue("VolUnit", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(UpdateCountryInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("SpUpdateAircraftTypeTonnageRecords", ColumnNames, DataType, Values))
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
        # endregion UpdateAircraftTonnageRecords

        # region Get Aircraft Tonnage List
        public DataSet spGetAircraftTonnageList(object[] CountryListInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];
                int i = 0;

                //0
                ColumnNames.SetValue("AircraftType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("ValidFrom", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("ValidTo", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(CountryListInfo.GetValue(i), i);
                i++;



                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetAircraftTonnageList", ColumnNames, Values, DataType);
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

        # endregion Get Aircraft Tonnage List

        # region Get Origin Code
        public DataSet GetOriginCodeList(string OriginCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetOriginCodeList");
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

        # endregion Get Dest Code 

        # region Get Dest Code
        public DataSet GetDestCodeList(string DestCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetDestCodeList");
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

        # endregion Get Dest Code 

        # region GetAircraftType Code
        public DataSet GetAircraftType(string AirCraftList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetAircraftTypeList");
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

        # endregion GetAircrafteType Code

    }
}
