using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALAirportMaster
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALAirportMaster()
        {
            constr = Global.GetConnectionString();
        }

        # region Get Country Code
        public DataSet GetCountryCodeList(string CountryCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("SP_GetAllStationCodeName","level","country",SqlDbType.VarChar);
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

        # region Get Region Code
        public DataSet GetRegionCodeList(object[] regioncodeinfo)
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
                Values.SetValue(regioncodeinfo.GetValue(i), i);
               


                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetRegionCodeList", ColumnNames, Values, DataType);
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
        # endregion Get Region Code

        # region AddAirport
        public int AddAirport(object[] AirportInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[46];
                SqlDbType[] DataType = new SqlDbType[46];
                Object[] Values = new object[46];
                int i = 0;

                //0
                ColumnNames.SetValue("AirportCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("AirportName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

             

                //3 
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("City", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("StationMailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("ManagerName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("ManagerEmailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("ShiftMobNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("LandlineNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("ManagerMobNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("Counter", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("GHAName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("GHAAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("GHAPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("GHAMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("GHAFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //18
                ColumnNames.SetValue("GHAEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //19
                ColumnNames.SetValue("GSAName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //20
                ColumnNames.SetValue("GSAAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //21
                ColumnNames.SetValue("GSAPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //22
                ColumnNames.SetValue("GSAMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //23
                ColumnNames.SetValue("GSAFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //24
                ColumnNames.SetValue("GSAEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //25
                ColumnNames.SetValue("APMName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //26
                ColumnNames.SetValue("APMAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //27
                ColumnNames.SetValue("APMPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //28
                ColumnNames.SetValue("APMMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //29
                ColumnNames.SetValue("APMFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //30
                ColumnNames.SetValue("APMEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //31
                ColumnNames.SetValue("AdditionalInfo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //32
                ColumnNames.SetValue("TransitTime", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //33
                ColumnNames.SetValue("CutOffTime", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;
                

                //34
                ColumnNames.SetValue("IsTaxExempted", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //35
                ColumnNames.SetValue("BookingCurrency", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //36
                ColumnNames.SetValue("BookingCurrencyType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //37
                ColumnNames.SetValue("InvoiceCurrency", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //38
                ColumnNames.SetValue("InvoiceCurrencyType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //39
                ColumnNames.SetValue("citytype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //40
                ColumnNames.SetValue("GLAccount", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;


                //41 for timezone
                ColumnNames.SetValue("TimeZones", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //42 for UTCTimeDIFF
                ColumnNames.SetValue("UTCTimeDIFF", i);
                DataType.SetValue(SqlDbType.VarChar,i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //43
                ColumnNames.SetValue("isULDEnabled", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //44
                ColumnNames.SetValue("Latitude", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //45
                ColumnNames.SetValue("Longitude", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);

              
                if (!da.ExecuteProcedure("SpAddAirportRecordsNew", ColumnNames, DataType, Values))
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
        # endregion AddAirport

        # region Get Airport List
        public DataSet GetAirportList(object[] AirportListInfo)
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
                Values.SetValue(AirportListInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportListInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("AirportCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportListInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportListInfo.GetValue(i), i);
                i++;

              

                //ColumnNames.SetValue("GLAccount", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(AirportListInfo.GetValue(i), i);


                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetAirportListNew", ColumnNames, Values, DataType);
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
        # endregion Get Airport List

        # region UpdateAirport
        public int UpdateAirport(object[] UpdateAirportInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[46];
                SqlDbType[] DataType = new SqlDbType[46];
                Object[] Values = new object[46];
                int i = 0;

                //0
                ColumnNames.SetValue("AirportCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("AirportName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                
                //2
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;


                //4
                ColumnNames.SetValue("City", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;


                //5
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("StationMailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("ManagerName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("ManagerEmailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("ShiftMobNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("LandlineNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("ManagerMobNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Counter", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("GHAName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("GHAAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("GHAPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("GHAMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("GHAFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("GHAEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //18
                ColumnNames.SetValue("GSAName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //19
                ColumnNames.SetValue("GSAAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //20
                ColumnNames.SetValue("GSAPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //21
                ColumnNames.SetValue("GSAMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //22
                ColumnNames.SetValue("GSAFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //23
                ColumnNames.SetValue("GSAEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //24
                ColumnNames.SetValue("APMName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //25
                ColumnNames.SetValue("APMAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //26
                ColumnNames.SetValue("APMPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //27
                ColumnNames.SetValue("APMMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //28
                ColumnNames.SetValue("APMFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //29
                ColumnNames.SetValue("APMEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //30
                ColumnNames.SetValue("AdditionalInfo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //31
                ColumnNames.SetValue("TransitTime", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //32
                ColumnNames.SetValue("CutOffTime", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //33
                ColumnNames.SetValue("IsTaxExempted", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;
                //35
                ColumnNames.SetValue("BookingCurrency", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //36
                ColumnNames.SetValue("BookingCurrencyType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //37
                ColumnNames.SetValue("InvoiceCurrency", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //38
                ColumnNames.SetValue("InvoiceCurrencyType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                 //39
                ColumnNames.SetValue("citytype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //40
                ColumnNames.SetValue("GLAccount", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //41 for timeZones
                ColumnNames.SetValue("TimeZones", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //42 for UTCTImeDIFF
                ColumnNames.SetValue("UTCTimeDIFF", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //43
                ColumnNames.SetValue("isULDEnabled", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //44
                ColumnNames.SetValue("Latitude", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);
                i++;

                //45
                ColumnNames.SetValue("Longitude", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdateAirportInfo.GetValue(i), i);


                if (!da.ExecuteProcedure("SpUpdateAirportRecordsNew", ColumnNames, DataType, Values))
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
        # endregion UpdateAirport

        # region Get Region
        public DataSet GetRegionCode()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("SP_GetRegionCodeName");
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

        # endregion Get Region

        //new added timezones
        # region GetTimezone
        public DataSet GetTimeZoneCode()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("SPGetTImeZones");
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

        # endregion GetTimeZone

        #region CheckDuplicate
        public bool CheckDuplicate(object[] values, ref bool IsDuplicate, ref string errormessage)
        {
            try
            {
                string[] param = {"AirportCode"};

                SqlDbType[] dbtypes = { SqlDbType.VarChar};

                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_CheckDuplicateAirportCode", param, values, dbtypes);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            IsDuplicate = bool.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                            return true;
                        }
                        else
                        {
                            errormessage = "Error : (CheckDuplicate) Code III";
                            return false;
                        }
                    }
                    else
                    {
                        errormessage = "Error : (CheckDuplicate) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : (CheckDuplicate) Code I";
                    return false;
                }

                return false;

            }
            catch (Exception ex)
            {
                errormessage = "Error :(CheckDuplicate)" + ex.Message;
                return false;
            }

        }
        #endregion

        #region CheckAirportMasterEntry
        public bool CheckAirportMasterEntry(string airport,string level)
        {
            DataSet dsResult = null;
            SQLServer da = null;

            try
            {
                string[] pname = { "Code", "level" };
                object[] pvalue = { airport, level };
                SqlDbType[] ptype = { SqlDbType.VarChar, SqlDbType.VarChar };
                da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_CheckValidateMasterRecords", pname, pvalue, ptype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            return bool.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                dsResult = null;                
                da = null;
            }

        }
        #endregion

    }
}
