using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class BALHoliday
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALHoliday()
        { constr = Global.GetConnectionString(); }

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

        # region Get Country Codes
        public DataSet GetCountryCodes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetCoutryCode");
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

        # region Add Holiday
        public int AddHoliday(object[] HolidayInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[10];
                SqlDbType[] DataType = new SqlDbType[10];
                Object[] Values = new object[10];
                int i = 0;

                //0
                ColumnNames.SetValue("stn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("country", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("day", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //3
                //ColumnNames.SetValue("date", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(HolidayInfo.GetValue(i), i);
                //i++;

                //4
                ColumnNames.SetValue("holidaytype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("datefrm", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("dateto", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("updatedon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("updatedby", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("validfrm", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("validto", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spAddHolidays", ColumnNames, DataType, Values))
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

        # region Update Holiday
        public int UpdateHoliday(object[] HolidayInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[10];
                SqlDbType[] DataType = new SqlDbType[10];
                Object[] Values = new object[10];
                int i = 0;

                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //0
                ColumnNames.SetValue("stn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("country", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("day", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("date", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("holidaytype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("datefrm", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("dateto", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("updatedon", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("updatedby", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spUpdateHolidays", ColumnNames, DataType, Values))
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

        # region Get Holiday List
        public DataSet GetHolidayList(object[] HolidayInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("holidaytype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("country", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("stn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("day", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HolidayInfo.GetValue(i), i);
                i++;

                ////4
                //ColumnNames.SetValue("vaildfrm", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(HolidayInfo.GetValue(i), i);
                //i++;

                ////5
                //ColumnNames.SetValue("validto", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(HolidayInfo.GetValue(i), i);
                //i++;

                ////6
                //ColumnNames.SetValue("dtfrm", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(HolidayInfo.GetValue(i), i);
                //i++;

                ////7
                //ColumnNames.SetValue("dtto", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(HolidayInfo.GetValue(i), i);
                //i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetHolidayList",ColumnNames,Values,DataType);
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
        # endregion Get Product Type List

        # region Delete Record
        public int DeleteRecord(object[] RecordInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //0
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RecordInfo.GetValue(i), i);
                i++;

                if (!da.ExecuteProcedure("spDeleteHolidayRecord", ColumnNames, DataType, Values))
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
    }

}
