using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;


namespace BAL
{
    public class AirlineScheduleBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #region Get Origin List
        public DataSet GetOriginList(string OriginList)
        {
            try
            {

                DataSet ds = db.SelectRecords("SP_GetOriginForListRateLine");
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

        #endregion Get Origin List

        #region Get Destination List
        public DataSet GetDestinationList(string DestinationList)
        {
            try
            {

                DataSet ds = db.SelectRecords("SP_GetDestinationForListRateLine");
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

        #endregion Get Destination List

        #region Get Flight(tail No) List
        /// <summary>
        /// Get list of all the tail no based on entered value.
        /// </summary>
        /// <returns>tail code list as Array.</returns>
        public DataSet GetFlightList(string Source)
        {
            try
            {
                string[] colNames = new string[1];
                string[] values = new string[1];
                SqlDbType[] dataTypes = new SqlDbType[1];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("[SP_SourceGetFlightID]",colNames, values, dataTypes);
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
        #endregion Get Flight(tail No) List

        #region Get Partner Flight(tail No) List
        /// <summary>
        /// Get list of all the tail no based on entered value.
        /// </summary>
        /// <returns>tail code list as Array.</returns>
        public DataSet GetPartnerFlightList(string Source)
        {
            try
            {
                string[] colNames = new string[1];
                string[] values = new string[1];
                SqlDbType[] dataTypes = new SqlDbType[1];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("[SP_PartnerSourceGetFlightID]", colNames, values, dataTypes);
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
        #endregion Get Flight(tail No) List

        #region Get Flight List source Destination wise
        /// <summary>
        /// Get list of all the tail no based on entered value.
        /// </summary>
        /// <returns>tail code list as Array.</returns>
        public DataSet GetFlightListAsPerSourceDest(string Source,string Dest)
        {
            try
            {
                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("[SP_SourceDestGetFlightID]", colNames, values, dataTypes);
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
        #endregion Get Flight(tail No) List

        #region Get Flight List source Destination wise
        /// <summary>
        /// Get list of all the tail no based on entered value.
        /// </summary>
        /// <returns>tail code list as Array.</returns>
        public DataSet GetPartnerFlightListAsPerSourceDest(string Source, string Dest)
        {
            try
            {
                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("[SP_PartnerSourceDestGetFlightID]", colNames, values, dataTypes);
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
        #endregion Get Flight(tail No) List

        #region Save Airline Schedule
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAirlineSchedule(object[] ScheduleInfo)
        {
            try
            {
                /*  @Fromdate varchar(12), @ToDate varchar(20),  @FlightID varchar(8),     
    @Source VARCHAR(5),  @Dest VARCHAR(5),  
    @FromSource varchar(5), @ToSource varchar(5),  
    @FromScheduleDepttime varchar(15),@ToSchArrtime varchar(15) ,@UpdatedOn datetime,@UpdatedBy varchar(10)    */
                //Prepare column names and datatypes...
                string[] paramNames = new string[13];
                SqlDbType[] dataTypes = new SqlDbType[13];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //2
                paramNames.SetValue("EquipmentNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("ScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
               
                //7
                paramNames.SetValue("SchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                 //8
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //9
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                 //10
                paramNames.SetValue("CargoCapacity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //11
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //12
                paramNames.SetValue("IsDomestic", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //Update AWB information.
               
                bool res = db.UpdateData("spSaveAirlineSchedule", paramNames, dataTypes, ScheduleInfo);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion Save airline

        #region Save Partner Schedule
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SavePartnerSchedule(object[] ScheduleInfo)
        {
            try
            {
                /*  @Fromdate varchar(12), @ToDate varchar(20),  @FlightID varchar(8),     
    @Source VARCHAR(5),  @Dest VARCHAR(5),  
    @FromSource varchar(5), @ToSource varchar(5),  
    @FromScheduleDepttime varchar(15),@ToSchArrtime varchar(15) ,@UpdatedOn datetime,@UpdatedBy varchar(10)    */
                //Prepare column names and datatypes...
                string[] paramNames = new string[14];
                SqlDbType[] dataTypes = new SqlDbType[14];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //2
                paramNames.SetValue("EquipmentNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("ScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("SchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //9
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("CargoCapacity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //11
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //12
                paramNames.SetValue("IsDomestic", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //13
                paramNames.SetValue("PartnerCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //Update AWB information.
                //bool res = db.UpdateData("spSavePartnerSchedule", paramNames, dataTypes, ScheduleInfo);

                //string res = db.UpdateData("spSavePartnerSchedule", paramNames, dataTypes, ScheduleInfo);
               // bool res = db.ExecuteProcedure("spSavePartnerSchedule", paramNames, dataTypes, ScheduleInfo);
                bool res = db.UpdateData("spSavePartnerSchedule", paramNames, dataTypes, ScheduleInfo);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion Save Partner

        #region Save Airline Route Schedule
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAirlineRouteDetails(object[] RouteInfo)
        {
            try
            {
                /*   @Fromdate varchar(12), @ToDate varchar(20), @EquipmentNo int, @FlightID float,   
    @Source VARCHAR(5), @Dest VARCHAR(5),  @ScheduleDepttime time,   
    @SchArrtime time, @UpdatedOn datetime,@UpdatedBy varchar(10), @FromSource varchar(5), @ToSource varchar(5),
    @FromScheduleDepttime time,@ToSchArrtime time  */
                //Prepare column names and datatypes...
                
                string[] paramNames = new string[18];
                SqlDbType[] dataTypes = new SqlDbType[18];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                
                //2
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                   //6
                paramNames.SetValue("FromSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("ToSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("FromScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("ToSchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //11
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //12
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //13
                paramNames.SetValue("status", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("SchDeptDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //15
                paramNames.SetValue("SchArrDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("CargoCapacity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //17
                paramNames.SetValue("AirCraftType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //18
                paramNames.SetValue("TailNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //Update AWB information.
                
                bool res = db.InsertData("spSaveAirlinerouteDetails", paramNames, dataTypes, RouteInfo);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion Save airline Route Details

        #region Save Partner Route Schedule
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SavePartnerRouteDetails(object[] RouteInfo)
        {
            try
            {
                /*   @Fromdate varchar(12), @ToDate varchar(20), @EquipmentNo int, @FlightID float,   
    @Source VARCHAR(5), @Dest VARCHAR(5),  @ScheduleDepttime time,   
    @SchArrtime time, @UpdatedOn datetime,@UpdatedBy varchar(10), @FromSource varchar(5), @ToSource varchar(5),
    @FromScheduleDepttime time,@ToSchArrtime time  */
                //Prepare column names and datatypes...

                string[] paramNames = new string[19];
                SqlDbType[] dataTypes = new SqlDbType[19];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;


                //2
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("FromSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("ToSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("FromScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("ToSchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //11
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //12
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //13
                paramNames.SetValue("status", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("SchDeptDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //15
                paramNames.SetValue("SchArrDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("CargoCapacity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //17
                paramNames.SetValue("AirCraftType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                 i++;
                //18
                 paramNames.SetValue("PartnerCode", i);
                 dataTypes.SetValue(SqlDbType.VarChar, i);
                 i++;

                //19
                 paramNames.SetValue("TailNo", i);
                 dataTypes.SetValue(SqlDbType.VarChar, i);

                //Update AWB information.

                bool res = db.InsertData("spSavePartnerrouteDetails", paramNames, dataTypes, RouteInfo);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion Save airline Route Details

        # region GetAirlineSchedule using selecetd criteria
        public DataSet GetAirlineSchedule(string Source,string Dest,string FlightId,string AirCraftType,string FromDt,string ToDt,string Status,string IsDomestic)
        {
            try
            {
                
                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AirCraftType", i);
                values.SetValue(AirCraftType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frmDate", i);
                values.SetValue(FromDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("IsDomestic", i);
                values.SetValue(IsDomestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(Status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                

                DataSet ds = db.SelectRecords("spAirlineScheduleDetails",colNames,values,dataTypes);
                if (ds != null)
                {
                    if (ds.Tables!= null)
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

        # endregion 
        #region ExportSchedule
        public DataSet GetExpAirlineSchedule(string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            try
            {

                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AirCraftType", i);
                values.SetValue(AirCraftType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frmDate", i);
                values.SetValue(FromDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("IsDomestic", i);
                values.SetValue(IsDomestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(Status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("spexportAirlineSchedule", colNames, values, dataTypes);
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
                return null;
            }
            return (null);
        }

        #endregion 

        # region GetPartnerSchedule using selecetd criteria
        public DataSet GetPartnerSchedule(string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt,
            string Status, bool IsDomestic, bool IsIntenational, string PartnerCode)
        {
            try
            {

                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AirCraftType", i);
                values.SetValue(AirCraftType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frmDate", i);
                values.SetValue(FromDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("IsDomestic", i);
                values.SetValue(IsDomestic.ToString(), i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                colNames.SetValue("IsInterNational", i);
                values.SetValue(IsIntenational.ToString(), i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(Status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("PartnerCode", i);
                values.SetValue(PartnerCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                DataSet ds = db.SelectRecords("spPartnerScheduleDetails_New", colNames, values, dataTypes);
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

        # endregion 

        # region Get flightUtilization using selecetd criteria
        public DataSet GetFlightUtilization(string Country,string Region,string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            try
            {

                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(Country, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Region", i);
                values.SetValue(Region, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AirCraftType", i);
                values.SetValue(AirCraftType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frmDate", i);
                values.SetValue(FromDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("IsDomestic", i);
                values.SetValue(IsDomestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(Status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("spGetAirlineScheduleUtilization1", colNames, values, dataTypes);
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

        # endregion 

        # region Get flightUtilization using selecetd criteria Active Analysis
        public DataSet GetAARFlightUtilization(string Country, string Region, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            try
            {

                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(Country, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Region", i);
                values.SetValue(Region, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AirCraftType", i);
                values.SetValue(AirCraftType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frmDate", i);
                values.SetValue(FromDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("IsDomestic", i);
                values.SetValue(IsDomestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(Status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("spAARGetAirlineScheduleUtilization1", colNames, values, dataTypes);
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

        # endregion 


        # region Get Datewise AirlineSchedule using  selecetd criteria
        public DataSet GetDateWiseAirlineSchedule(string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            try
            {

                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AirCraftType", i);
                values.SetValue(AirCraftType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frmDate", i);
                values.SetValue(FromDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("IsDomestic", i);
                values.SetValue(IsDomestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(Status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("spDateWiseAirlineScheduleDetails", colNames, values, dataTypes);
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

        # endregion 

        
        # region Get Datewise AirlineSchedule with Flight Status using  selecetd criteria
        public DataSet GetDateWiseAirlineScheduleWithFlightStatus(string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic, string FlightStatus)
        {
            try
            {

                string[] colNames = new string[9];
                string[] values = new string[9];
                SqlDbType[] dataTypes = new SqlDbType[9];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AirCraftType", i);
                values.SetValue(AirCraftType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frmDate", i);
                values.SetValue(FromDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("IsDomestic", i);
                values.SetValue(IsDomestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(Status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("FlightStatus", i);
                values.SetValue(FlightStatus, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("spDateWiseAirlineScheduleDetails", colNames, values, dataTypes);
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

        # endregion 

        # region GetAirlineSchedule using Source and flight
        public DataSet GetAirlineScheduleforflight(string Source, string FlightId, string frequency, DateTime fromDate,DateTime toDate)
        {
            try
            {

                string[] colNames = new string[5];
                object[] values = new object[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frequency", i);
                values.SetValue(frequency, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("fromDate", i);
                values.SetValue(fromDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                

                DataSet ds = db.SelectRecords("spcheckAirlineScheduleDetails", colNames, values, dataTypes);
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

        # endregion 

        # region GetPartnerSchedule using Source and flight
        public DataSet GetPartnerScheduleforflight(string Source, string FlightId, string frequency, DateTime fromDate, DateTime toDate)
        {
            try
            {

                string[] colNames = new string[5];
                object[] values = new object[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frequency", i);
                values.SetValue(frequency, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("fromDate", i);
                values.SetValue(fromDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);


                DataSet ds = db.SelectRecords("spcheckPartnerScheduleDetails", colNames, values, dataTypes);
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

        # endregion 

    
        # region GetAirlineSchedule using Source,Dest,Frequency and flightID
        public DataSet GetAirlineScheduleRouteforflight(string Source, string Dest, string FlightId, string frequency, DateTime fromDate, DateTime toDate)
        {
            try
            {

                string[] colNames = new string[6];
                object[] values = new object[6];
                SqlDbType[] dataTypes = new SqlDbType[6];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frequency", i);
                values.SetValue(frequency, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("fromDate", i);
                values.SetValue(fromDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);


                DataSet ds = db.SelectRecords("spcheckAirlineRouteSchedule", colNames, values, dataTypes);
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

        # endregion 

        # region GetPartnerSchedule using Source,Dest,Frequency and flightID
        public DataSet GetPartnerScheduleRouteforflight(string Source, string Dest, string FlightId, string frequency, DateTime fromDate, DateTime toDate)
        {
            try
            {

                string[] colNames = new string[6];
                object[] values = new object[6];
                SqlDbType[] dataTypes = new SqlDbType[6];
                int i = 0;

                colNames.SetValue("Source", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Dest", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("frequency", i);
                values.SetValue(frequency, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("fromDate", i);
                values.SetValue(fromDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);


                DataSet ds = db.SelectRecords("spcheckPartnerRouteSchedule", colNames, values, dataTypes);
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

        # endregion 

        
        # region check daily Schedule and flightID
        public int CheckDailyScheduleforflight(string FlightId, DateTime flightDate)
        {
            int id = 0;
            try
            {

                string[] colNames = new string[2];
                object[] values = new object[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightId, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightDate", i);
                values.SetValue(flightDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
               // i++;

                //colNames.SetValue("ScheduleID", i);
                //values.SetValue(ScheduleID, i);
                //dataTypes.SetValue(SqlDbType.BigInt, i);



                 id = db.GetIntegerByProcedure ("spDailyFlightScheduleCheck", colNames, values, dataTypes);
                
            }

            catch (Exception)
            {

            }
            return id;
        }

        # endregion 


        # region GetAirlineSchedule using RouteID
        public DataSet GetAirlineScheduleUsingRouteID(long RouteID)
        {
            try
            {

                string[] colNames = new string[1];
                object[] values = new object[1];
                SqlDbType[] dataTypes = new SqlDbType[1];
                int i = 0;

                colNames.SetValue("scheduleID", i);
                values.SetValue(RouteID, i);
                dataTypes.SetValue(SqlDbType.BigInt, i);

                DataSet ds = db.SelectRecords("spGetRouteDetailsUsingRouteID", colNames, values, dataTypes);
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

        # endregion 

        # region GetAirlineSchedule using RouteID
        public DataSet GetPartnerAirlineScheduleUsingRouteID(long RouteID)
        {
            try
            {

                string[] colNames = new string[1];
                object[] values = new object[1];
                SqlDbType[] dataTypes = new SqlDbType[1];
                int i = 0;

                colNames.SetValue("scheduleID", i);
                values.SetValue(RouteID, i);
                dataTypes.SetValue(SqlDbType.BigInt, i);

                DataSet ds = db.SelectRecords("spGetPartnerRouteDetailsUsingRouteID", colNames, values, dataTypes);
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

        # endregion 



        #region Update Airline Route Schedule
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int UpdateAirlineRouteDetails(object[] RouteInfo)
        {
            try
            {
                /*   @Fromdate varchar(12), @ToDate varchar(20), @EquipmentNo int, @FlightID float,   
    @Source VARCHAR(5), @Dest VARCHAR(5),  @ScheduleDepttime time,   
    @SchArrtime time, @UpdatedOn datetime,@UpdatedBy varchar(10), @FromSource varchar(5), @ToSource varchar(5),
    @FromScheduleDepttime time,@ToSchArrtime time  */
                //Prepare column names and datatypes...

                string[] paramNames = new string[22];
                SqlDbType[] dataTypes = new SqlDbType[22];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;


                //2
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("FromSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("ToSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("FromScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("ToSchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //11
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //12
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //13
                paramNames.SetValue("Status", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("DeptDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //15
                paramNames.SetValue("ArrDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("CargoCapcity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;
                

                     //17
                paramNames.SetValue("AirCraftType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //17
                paramNames.SetValue("IsDomestic", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;


                //18
                paramNames.SetValue("SchID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;


                //19
                paramNames.SetValue("Aircraft", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //20
                paramNames.SetValue("freeSell", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //21
                paramNames.SetValue("TailNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                

                //Update AWB information.

              //  bool res = db.InsertData("spUpdateAirlinerouteDetails", paramNames, dataTypes, RouteInfo);
                bool res = db.InsertData("spUpdateAirlineSchedule2", paramNames, dataTypes, RouteInfo);

               
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
    catch (Exception)
            {
            }
            return (-1);
        }
        #endregion

        #region Update Partner Route Schedule
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int UpdatePartnerRouteDetails(object[] RouteInfo)
        {
            try
            {
                /*   @Fromdate varchar(12), @ToDate varchar(20), @EquipmentNo int, @FlightID float,   
    @Source VARCHAR(5), @Dest VARCHAR(5),  @ScheduleDepttime time,   
    @SchArrtime time, @UpdatedOn datetime,@UpdatedBy varchar(10), @FromSource varchar(5), @ToSource varchar(5),
    @FromScheduleDepttime time,@ToSchArrtime time  */
                //Prepare column names and datatypes...

                string[] paramNames = new string[23];
                SqlDbType[] dataTypes = new SqlDbType[23];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;


                //2
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("FromSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("ToSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("FromScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("ToSchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //11
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //12
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //13
                paramNames.SetValue("Status", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("DeptDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //15
                paramNames.SetValue("ArrDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("CargoCapcity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;


                //17
                paramNames.SetValue("AirCraftType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //17
                paramNames.SetValue("IsDomestic", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;


                //18
                paramNames.SetValue("SchID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;


                //19
                paramNames.SetValue("Aircraft", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //20
                paramNames.SetValue("freeSell", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //21
                paramNames.SetValue("PartnerCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //22
                paramNames.SetValue("TailNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                


                //Update AWB information.

                //  bool res = db.InsertData("spUpdatePartnerSchedule2", paramNames, dataTypes, RouteInfo);
                bool res = db.InsertData("spUpdatePartnerSchedule2", paramNames, dataTypes, RouteInfo);


                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion

        #region Update Airline Route Schedule for Same date
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int UpdateAirlineRouteDetailsForSameDate(object[] RouteInfo)
        {
            try
            {
                /*   @Fromdate varchar(12), @ToDate varchar(20), @EquipmentNo int, @FlightID float,   
    @Source VARCHAR(5), @Dest VARCHAR(5),  @ScheduleDepttime time,   
    @SchArrtime time, @UpdatedOn datetime,@UpdatedBy varchar(10), @FromSource varchar(5), @ToSource varchar(5),
    @FromScheduleDepttime time,@ToSchArrtime time  */
                //Prepare column names and datatypes...

                string[] paramNames = new string[22];
                SqlDbType[] dataTypes = new SqlDbType[22];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;


                //2
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("FromSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("ToSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("FromScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("ToSchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //11
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //12
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //13
                paramNames.SetValue("Status", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("DeptDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //15
                paramNames.SetValue("ArrDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("CargoCapcity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;


                //17
                paramNames.SetValue("AirCraftType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //18
                paramNames.SetValue("IsDomestic", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;


                //19
                paramNames.SetValue("SchID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;

                //20
                paramNames.SetValue("Aircraft", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //21
                paramNames.SetValue("freeSell", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //22
                paramNames.SetValue("TailNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                //Update AWB information.

                //  bool res = db.InsertData("spUpdateAirlinerouteDetails", paramNames, dataTypes, RouteInfo);
                bool res = db.InsertData("spUpdateAirlineScheduleRoute2", paramNames, dataTypes, RouteInfo);


                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion

        #region Update Partner Route Schedule for Same date
        /// <summary>
        /// Saves information of Airline Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int UpdatePartnerRouteDetailsForSameDate(object[] RouteInfo)
        {
            try
            {
                /*   @Fromdate varchar(12), @ToDate varchar(20), @EquipmentNo int, @FlightID float,   
    @Source VARCHAR(5), @Dest VARCHAR(5),  @ScheduleDepttime time,   
    @SchArrtime time, @UpdatedOn datetime,@UpdatedBy varchar(10), @FromSource varchar(5), @ToSource varchar(5),
    @FromScheduleDepttime time,@ToSchArrtime time  */
                //Prepare column names and datatypes...

                string[] paramNames = new string[23];
                SqlDbType[] dataTypes = new SqlDbType[23];
                int i = 0;

                //0
                paramNames.SetValue("Fromdate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //1
                paramNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;


                //2
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("FromSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("ToSource", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("frequency", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("FromScheduleDepttime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //10
                paramNames.SetValue("ToSchArrtime", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;
                //11
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //12
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //13
                paramNames.SetValue("Status", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //14
                paramNames.SetValue("DeptDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //15
                paramNames.SetValue("ArrDay", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //16
                paramNames.SetValue("CargoCapcity", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;


                //17
                paramNames.SetValue("AirCraftType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //18
                paramNames.SetValue("IsDomestic", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;


                //19
                paramNames.SetValue("SchID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;

                //20
                paramNames.SetValue("Aircraft", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                //21
                paramNames.SetValue("freeSell", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //22
                paramNames.SetValue("PartnerCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //23
                paramNames.SetValue("TailNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                //Update AWB information.

                //  bool res = db.InsertData("spUpdateAirlinerouteDetails", paramNames, dataTypes, RouteInfo);
                bool res = db.InsertData("spUpdatePartnerScheduleRoute2", paramNames, dataTypes, RouteInfo);


                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion

        #region Get Aircraft Type
        public DataSet GetAirCraftType()
        {
            try
            {

                DataSet ds = db.SelectRecords("spGetArircraftType");
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

        #endregion Get Aircraft Type
        
        #region Get Cargo Capacity as per Aircraft Type
        public DataSet GetCargoCapacity(string strAircraftType)
        {
            try
            {

                DataSet ds = db.SelectRecords("spGetCargoCapacity", "AircraftType", strAircraftType, SqlDbType.NVarChar);
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

        #endregion Get Cargo Capacity


        //Code for Active Flight Screen
        #region Get active flight Data
        public DataSet GetActiveFlightData(string FlightID,string FlightDate)
        {
            try
            {

                string[] colNames = new string[2];
                object[] values = new object[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("FlightID", i);
                values.SetValue(FlightID, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                
                colNames.SetValue("FlightDate", i);
                values.SetValue(FlightDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);


                DataSet ds = db.SelectRecords("spGetDailyFlightSchedule", colNames, values, dataTypes);
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
        #endregion

        #region Save Daily Active Flight Details
        /// <summary>
        /// Saves information of Active Schedule.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveDailyActiveFlightDetails(object[] RouteInfo)
        {
            try
            {
                //Prepare column names and datatypes...

                string[] paramNames = new string[13];
                SqlDbType[] dataTypes = new SqlDbType[13];
                int i = 0;

                //0
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("FlightDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;


                //2
                paramNames.SetValue("Source", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("Dest", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("STD", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("ATD", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("STA", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("ATA", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //9
                paramNames.SetValue("ScheduleID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;

                //10
                paramNames.SetValue("AircraftType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //11
                paramNames.SetValue("RegistrationNo", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //12
                paramNames.SetValue("Remark", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //13
                paramNames.SetValue("CreatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

               
                //save Daily flight Movement Data

                bool res = db.InsertData("spDailyFlightScheduleSave", paramNames, dataTypes, RouteInfo);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion Save Active Flight details

        #region Get Origin List
        public DataSet GetOriginListforPartner()
        {
            try
            {

                DataSet ds = db.GetDataset("Select Distinct Source from (Select Source from PartnerSchedule Union Select Dest 'Source' from PartnerSchedule) X Order by Source");
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
        #endregion Get Origin List

        public DataSet GetPartnerDetailsOnType(string PartnerType)
        {
            try
            {
                DataSet ds = db.GetDataset("Exec sp_GetPartnerDetailsbyType '" + PartnerType + "'");
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

        #region Get Tail Number as per Aircraft Type
        public DataSet GetTailNumber(string strAircraftType)
        {
            DataSet dsTail = new DataSet();
            try
            {
                dsTail = db.SelectRecords("sp_GetTailNoByAircraftType", "AircraftType", strAircraftType, SqlDbType.VarChar);

                if (dsTail != null && dsTail.Tables.Count > 0 && dsTail.Tables[0].Rows.Count > 0)
                {
                    return dsTail;
                }
            }
            catch (Exception ex)
            {
               
            }
            return null;
         
        }

        #endregion
    }

}
