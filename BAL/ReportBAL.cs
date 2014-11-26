using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;


namespace BAL
{
    public class ReportBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());

        # region Get flightUtilization using selecetd criteria
        public DataSet GetFlightUtilization(string Country, string Region, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            DataSet ds = null;
            try
            {

                
                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Region", i);
                values.SetValue(Source, i);
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


                ds = db.SelectRecords("spGetAirlineScheduleUtilization1", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
                
            }
            return (null);
        }

        # endregion 
        
        #region  Get AWB Accepted but Not Departed
        public DataSet GetAWBNotDeparted(string Origin, string Destination, string AgentCode, string dtFrom, string dtTo)
        {
            DataSet ds = null;
            try
            {


                string[] colNames = new string[5];
                string[] values = new string[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;

                colNames.SetValue("Origin", i);
                values.SetValue(Origin, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Destination", i);
                values.SetValue(Destination, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AgentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("dtFrom", i);
                values.SetValue(dtFrom, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("dtTo", i);
                values.SetValue(dtTo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
             


                ds = db.SelectRecords("spGetAWBNotDeparted", colNames, values, dataTypes);
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

        #region  Get AWB Arrived but Not Delivered
        public DataSet GetAWBNotDelivered(string Origin, string Destination, string AgentCode, string dtFrom, string dtTo)
        {
            DataSet ds = null;
            try
            {


                string[] colNames = new string[5];
                string[] values = new string[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;

                colNames.SetValue("Origin", i);
                values.SetValue(Origin, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Destination", i);
                values.SetValue(Destination, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AgentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("dtFrom", i);
                values.SetValue(dtFrom, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("dtTo", i);
                values.SetValue(dtTo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);



                ds = db.SelectRecords("spGetAWBNotDelivered", colNames, values, dataTypes);
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

        # region Get AWB Movement using selecetd criteria
        public DataSet GetAWBMovement(string Country, string Region, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic,string AWBNumber,string AgentCode,string AWBPrefix)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[13];
                string[] values = new string[13];
                SqlDbType[] dataTypes = new SqlDbType[13];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(Country , i);
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
                i++;

                colNames.SetValue("AWBNumber ", i);
                values.SetValue(AWBNumber, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AgentCode ", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AWBPrefix ", i);
                values.SetValue(AWBPrefix, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("spGetMovement", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        # region Get AWB Movement using selecetd criteria
        public DataSet GetAWBMovemenInOneLine(string Country, string Region, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic, string AWBNumber, string AgentCode,string AWBPrefix)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[13];
                string[] values = new string[13];
                SqlDbType[] dataTypes = new SqlDbType[13];
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
                i++;

                colNames.SetValue("AWBNumber ", i);
                values.SetValue(AWBNumber, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AgentCode ", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AWBPrefix ", i);
                values.SetValue(AWBPrefix, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                ds = db.SelectRecords("spGetMovementForOneLine", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 
        
        # region Get AWB Movement using selecetd criteria Active Analysis Report
        public DataSet GetAARAWBMovement(string Country, string Region, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic, string AWBNumber, string AgentCode)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[12];
                string[] values = new string[12];
                SqlDbType[] dataTypes = new SqlDbType[12];
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
                i++;

                colNames.SetValue("AWBNumber ", i);
                values.SetValue(AWBNumber, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AgentCode ", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("spAARGetMovement", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        # region Get Datewise AirlineSchedule using  selecetd criteria Active Analysis
        public DataSet GetDateWiseAirlineScheduleReport(string country,string Regioncode,string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(country, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Regioncode", i);
                values.SetValue(Regioncode, i);
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


                ds = db.SelectRecords("sprptDateWiseAirlineScheduleDetails", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        # region Get Datewise AirlineSchedule using  selecetd criteria Active Analysis
        public DataSet GetAARDateWiseAirlineScheduleReport(string country, string Regioncode, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(country, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Regioncode", i);
                values.SetValue(Regioncode, i);
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


                ds = db.SelectRecords("spAARrptDateWiseAirlineScheduleDetails", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        # region Get Country Code
        public DataSet GetPaymentCode()
        {
            DataSet ds = null;
            try
            {

                ds = db.SelectRecords("SP_GetPaymentType");
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);


        }
        #endregion

        # region Get Billing DCM details
        public DataSet GetBillingDCMData(string frmDate, string ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[2];
                object[] values = new object[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("FromDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                ds = db.SelectRecords("Sp_rptGetBillingDCMReportNew", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 
                
        # region Get Master AirlineSchedule using  selecetd criteria
        public DataSet GetMasterAirlineScheduleReport(string country, string Regioncode, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(country, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Regioncode", i);
                values.SetValue(Regioncode, i);
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


                ds = db.SelectRecords("sprptMasterAirlineScheduleDetails", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        # region Get Master AirlineSchedule using  selecetd criteria Active Analysis
        public DataSet GetAARMasterAirlineScheduleReport(string country, string Regioncode, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[10];
                string[] values = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                colNames.SetValue("Country", i);
                values.SetValue(country, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Regioncode", i);
                values.SetValue(Regioncode, i);
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


                ds = db.SelectRecords("spAARrptMasterAirlineScheduleDetails", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        #region Get Aircraft Type
        public DataSet GetAirCraftType()
        {
            DataSet ds = null;
            try
            {

                ds = db.SelectRecords("spGetArircraftType");
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        #endregion Get Aircraft Type
        
        #region Get Shipment Type Master
        public DataSet GetShipmentType()
        {
            DataSet ds = null;
            try
            {

                ds = db.SelectRecords("spGetShipmentTypes");
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }
        #endregion
        
        # region Get StationWise report using  selecetd criteria
        public DataSet GetStationwiseTonnageReport(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[7];
                string[] values = new string[7];
                SqlDbType[] dataTypes = new SqlDbType[7];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

               
                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("spListProductivityStationwise", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 
        
        # region Get AWB Summary report using  selecetd criteria
        public DataSet GetAWBBookingSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt,string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);
              
                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);
               
                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                ds = db.SelectRecords("Sp_rptGetAWBSummaryReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 
                
        # region Get Sector wisw  Summary report using  selecetd criteria
        public DataSet GetSectorWiseAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                //DataSet ds = db.SelectRecords("Sp_rptGetAWBSectorwiseReport", colNames, values, dataTypes);
                ds = db.SelectRecords("Sp_rptGetAWBSectorTonnage", colNames, values, dataTypes);

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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 
        
        # region Get Station wise  Summary report using  selecetd criteria
        public DataSet GetStationWiseAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus, bool Domestic, bool International, bool DomInt, bool IntDom, bool POMail, string AgentRefCode)
        {
            DataSet ds = new DataSet();
            try
            {

                string[] colNames = new string[14];
                object[] values = new object[14];
                SqlDbType[] dataTypes = new SqlDbType[14];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                i++;


                colNames.SetValue("Domestic", i);
                values.SetValue(Domestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("International", i);
                values.SetValue(International, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("DomInt", i);
                values.SetValue(DomInt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("IntDom", i);
                values.SetValue(IntDom, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;


                colNames.SetValue("POMail", i);
                values.SetValue(POMail, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AgentReferenceCode", i);
                values.SetValue(AgentRefCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                ds = db.SelectRecords("Sp_rptGetAWBStationwiseReport_New", colNames, values, dataTypes);
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
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return (null);
        }
        # endregion 
        
        # region Get Agent wise  Summary report using  selecetd criteria
        public DataSet GetAgentWiseAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                ds = db.SelectRecords("Sp_rptGetAWBAgentwiseReportTrial", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get Agent wise  Summary Acitive Analysis report using  selecetd criteria
        public DataSet GetAgentWiseAARAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                ds = db.SelectRecords("Sp_rptAARGetAWBAgentwiseReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get Station wise  Summary Acitive Analysis report using  selecetd criteria
        public DataSet GetStationWiseAARAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus, bool Domestic, bool International, bool DomInt, bool IntDom, bool POMail, string AgentRefCode)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[14];
                object[] values = new object[14];
                SqlDbType[] dataTypes = new SqlDbType[14];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                i++;


                colNames.SetValue("Domestic", i);
                values.SetValue(Domestic, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("International", i);
                values.SetValue(International, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("DomInt", i);
                values.SetValue(DomInt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("IntDom", i);
                values.SetValue(IntDom, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;


                colNames.SetValue("POMail", i);
                values.SetValue(POMail, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AgentReferenceCode", i);
                values.SetValue(AgentRefCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                ds = db.SelectRecords("Sp_rptAARGetAWBStationwiseReport_New", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }


        # endregion 

        # region Get Sector wise  Summary Acive Analysis report using  selecetd criteria
        public DataSet GetSectorWiseAARAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                //DataSet ds = db.SelectRecords("Sp_rptGetAWBSectorwiseReport", colNames, values, dataTypes);
                ds = db.SelectRecords("Sp_rptAARGetAWBSectorTonnage", colNames, values, dataTypes);

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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get Bank Garantee report using  selecetd criteria
        public DataSet GetBanGuranteeDetails(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                ds = db.SelectRecords("Sp_rptGetBankGuaranteeDetails", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 
        
        # region Get Flight Manifested report using  selecetd criteria
        public DataSet GetFlightManifestedDetails(DateTime frmDate, DateTime ToDt, string Flightstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[3];
                object[] values = new object[3];
                SqlDbType[] dataTypes = new SqlDbType[3];
                int i = 0;

               

                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("FlightStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(Flightstatus, i);

                ds = db.SelectRecords("Sp_rptGetFlightManifestedReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get Flight Manifested Active Analysis report using  selecetd criteria
        public DataSet GetAARFlightManifestedDetails(DateTime frmDate, DateTime ToDt, string Flightstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[3];
                object[] values = new object[3];
                SqlDbType[] dataTypes = new SqlDbType[3];
                int i = 0;



                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("FlightStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(Flightstatus, i);

                ds = db.SelectRecords("Sp_rptAARGetFlightManifestedReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get AWB Stock Control report using  selecetd criteria
        public DataSet GetStockControlDetails(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                ds = db.SelectRecords("Sp_rptGetAWBStockControlDetails", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get AWB Stock Control Active Analysis report using  selecetd criteria
        public DataSet GetAARStockControlDetails(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                ds = db.SelectRecords("Sp_rptAARGetAWBStockControlDetails", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get Flight wise  Summary report using  selecetd criteria
        public DataSet GetFlightWiseAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

               // DataSet ds = db.SelectRecords("Sp_rptGetAWBFlightwiseReport", colNames, values, dataTypes);
                ds = db.SelectRecords("Sp_rptGetAWBFlightTonnage", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 
        
        # region Get Flight wise  Summary Active Analysis report using  selecetd criteria
        public DataSet GetFlightWiseAARAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                // DataSet ds = db.SelectRecords("Sp_rptGetAWBFlightwiseReport", colNames, values, dataTypes);
                ds = db.SelectRecords("Sp_rptAARGetAWBFlightTonnage", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get AWB Manifested details report using  selecetd criteria
        public DataSet GetAWBManifestedAWBReport(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);
               
                ds = db.SelectRecords("Sp_rptGetDetailedFlightReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion

        # region Get AWB Manifested details Active Analysis report using  selecetd criteria
        public DataSet GetAWBManifestedAARAWBReport(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                ds = db.SelectRecords("Sp_rptAARGetDetailedFlightReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion

        # region Get Offload report using  selecetd criteria
        public DataSet GetOffloadAWBDetails(string FlightNo, string AWBNo, string Location, string frmDate, string ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[5];
                object[] values = new object[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;

                colNames.SetValue("FlightNo", i);
                values.SetValue(FlightNo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AWBNo", i);
                values.SetValue(AWBNo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Location", i);
                values.SetValue(Location, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

               


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ToDt, i);


                

                // DataSet ds = db.SelectRecords("Sp_rptGetAWBFlightwiseReport", colNames, values, dataTypes);
                ds = db.SelectRecords("Sp_rptGetOffloadReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get Offload Active Analysis report using  selecetd criteria
        public DataSet GetOffloadAARAWBDetails(string FlightNo, string AWBNo, string Location, string frmDate, string ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[5];
                object[] values = new object[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;

                colNames.SetValue("FlightNo", i);
                values.SetValue(FlightNo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AWBNo", i);
                values.SetValue(AWBNo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Location", i);
                values.SetValue(Location, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;




                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ToDt, i);




                // DataSet ds = db.SelectRecords("Sp_rptGetAWBFlightwiseReport", colNames, values, dataTypes);
                ds = db.SelectRecords("Sp_rptAARGetOffloadReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get AWB Billing Summary report using  selecetd criteria
        public DataSet GetAWBBilligSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[7];
                string[] values = new string[7];
                SqlDbType[] dataTypes = new SqlDbType[7];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("Sp_rptGetBillingSummaryReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # endregion 

        # region Get Country Code
        public DataSet GetCountryCode()
        {
            DataSet ds = null;
            try
            {
               
                ds = db.SelectRecords("SP_GetCountryCode");
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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);


        }
        #endregion

        # region Get Void AWB  selecetd criteria
        public DataSet GetAWBVoidData(string Source, string AgentCode, string FromDt, string ToDt, string AWBNumber)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[5];
                string[] values = new string[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;


                colNames.SetValue("station", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("agent", i);
                values.SetValue(AgentCode, i);
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

                colNames.SetValue("AwbNo", i);
                values.SetValue(AWBNumber, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);




                ds = db.SelectRecords("spGetVoidMovement24", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion

        # region Get Void AWB  selecetd criteria Active Analysis
        public DataSet GetAARAWBVoidData(string Source, string AgentCode, string FromDt, string ToDt, string AWBNumber)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[5];
                string[] values = new string[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;


                colNames.SetValue("station", i);
                values.SetValue(Source, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("agent", i);
                values.SetValue(AgentCode, i);
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

                colNames.SetValue("AwbNo", i);
                values.SetValue(AWBNumber, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);




                ds = db.SelectRecords("spAARGetVoidMovement24", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion
        
        # region Get Overview AWB data using selecetd criteria
        public DataSet GetOverviewAWB(string Country, string Region, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic, string AWBNumber, string AgentCode,string AWBprefix)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[13];
                string[] values = new string[13];
                SqlDbType[] dataTypes = new SqlDbType[13];
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
                i++;

                colNames.SetValue("AWBNumber ", i);
                values.SetValue(AWBNumber, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AgentCode ", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AWBPrefix ", i);
                values.SetValue(AWBprefix, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("spGetOverviewAWB_New22", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }
        public DataSet GetOverviewAWBFlightDate(string Country, string Region, string Source, string Dest, string FlightId, string AirCraftType, string FromDt, string ToDt, string Status, string IsDomestic, string AWBNumber, string AgentCode, string AWBprefix)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[13];
                string[] values = new string[13];
                SqlDbType[] dataTypes = new SqlDbType[13];
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
                i++;

                colNames.SetValue("AWBNumber ", i);
                values.SetValue(AWBNumber, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AgentCode ", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AWBPrefix ", i);
                values.SetValue(AWBprefix, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("spGetOverviewAWB_NewFlightdate", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }
        # endregion 

        # region Get AWB Summary report using  selecetd criteria
        public DataSet GetAWBBookingDetails(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBStatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AWBStatus", i);
                values.SetValue(AWBStatus, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("rptGetAWBDetailMIS", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        public DataSet GetAWBBookingDetails_FlightDateWise(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBStatus)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AWBStatus", i);
                values.SetValue(AWBStatus, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                ds = db.SelectRecords("rptGetAWBDetailMIS_FlightDate", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }
        # endregion 

        # region Get AWB Summary Active Analysis report using  selecetd criteria
        public DataSet GetAARAWBBookingDetails(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBStatus, bool Domestic, bool International, bool DomInt, bool IntDom, bool POMail, string AgentReferenceCode)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[14];
                object[] values = new object[14];
                SqlDbType[] dataTypes = new SqlDbType[14];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AWBStatus", i);
                values.SetValue(AWBStatus, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("Domestic", i);
                values.SetValue(Domestic, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("International", i);
                values.SetValue(International, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("DomInt", i);
                values.SetValue(DomInt, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("IntDom", i);
                values.SetValue(IntDom, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;


                colNames.SetValue("POMail", i);
                values.SetValue(POMail, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("AgentReferenceCode", i);
                values.SetValue(AgentReferenceCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                ds = db.SelectRecords("rptAARGetAWBDetailMIS_New", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        public DataSet GetAARAWBBookingDetails_FlightDateWise(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBStatus, bool Domestic, bool International, bool DomInt, bool IntDom, bool POMail, string AgentReferenceCode)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[14];
                object[] values = new object[14];
                SqlDbType[] dataTypes = new SqlDbType[14];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AWBStatus", i);
                values.SetValue(AWBStatus, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("Domestic", i);
                values.SetValue(Domestic, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("International", i);
                values.SetValue(International, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("DomInt", i);
                values.SetValue(DomInt, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("IntDom", i);
                values.SetValue(IntDom, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("POMail", i);
                values.SetValue(POMail, i);
                dataTypes.SetValue(SqlDbType.Bit, i);

                i++;

                colNames.SetValue("AgentReferenceCode", i);
                values.SetValue(AgentReferenceCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;


                ds = db.SelectRecords("rptAARGetAWBDetailMIS_FlightDate_New", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }
        # endregion 

        #region Get Messaging Details

        #region Outbox
        public DataSet GetMessagingDetails_Outbox(string frmDate, string toDate, string status, string process, string msgtype, string commtype)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[6];
                string[] values = new string[6];
                SqlDbType[] dataTypes = new SqlDbType[6];
                int i = 0;

                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Process", i);
                values.SetValue(process, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("type", i);
                values.SetValue(msgtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("comtype", i);
                values.SetValue(commtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                ds = db.SelectRecords("spGetMessagingDetails_Outbox_New", colNames, values, dataTypes);
                colNames = null;
                values = null;
                dataTypes = null;
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);



        }
        #endregion

        #region Inbox
        public DataSet GetMessagingDetails_Inbox(string frmDate, string toDate, string status, string process, string msgtype, string commtype)
        {
            DataSet ds = null;
            try
            {
                string[] colNames = new string[6];
                string[] values = new string[6];
                SqlDbType[] dataTypes = new SqlDbType[6];
                int i = 0;

                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Process", i);
                values.SetValue(process, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("type", i);
                values.SetValue(msgtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("comtype", i);
                values.SetValue(commtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                ds = db.SelectRecords("spGetMessagingDetails_Inbox_New", colNames, values, dataTypes);
                colNames = null;
                values=null;
                dataTypes = null;
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }
        #endregion

        #region Outbox
        public DataSet GetMessagingDetails_Outbox(string frmDate, string toDate, string status, string process, string msgtype, string commtype, string Criteria)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[7];
                string[] values = new string[7];
                SqlDbType[] dataTypes = new SqlDbType[7];
                int i = 0;

                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Process", i);
                values.SetValue(process, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("type", i);
                values.SetValue(msgtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("comtype", i);
                values.SetValue(commtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Criteria", i);
                values.SetValue(Criteria, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                ds = db.SelectRecords("spGetMessagingDetails_Outbox", colNames, values, dataTypes);
                colNames=null;
                values=null;
                dataTypes = null;
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);



        }
        #endregion

        #region Inbox
        public DataSet GetMessagingDetails_Inbox(string frmDate, string toDate, string status, string process, string msgtype, string commtype, string Criteria)
        {
            DataSet ds = null;
            try
            {
                string[] colNames = new string[7];
                string[] values = new string[7];
                SqlDbType[] dataTypes = new SqlDbType[7];
                int i = 0;

                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Status", i);
                values.SetValue(status, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Process", i);
                values.SetValue(process, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("type", i);
                values.SetValue(msgtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("comtype", i);
                values.SetValue(commtype, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Criteria", i);
                values.SetValue(Criteria, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                ds = db.SelectRecords("spGetMessagingDetails_Inbox", colNames, values, dataTypes);
                colNames = null;
                values = null;
                dataTypes = null;
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);

        }
        #endregion

        #endregion

        # region AddMailinOutbox
        public int AddMailinOutbox(object[] MailInfo)
        {
            try
            {

                string[] ColumnNames = new string[11];
                SqlDbType[] DataType = new SqlDbType[11];
                Object[] Values = new object[11];
                int i = 0;

                //0
                ColumnNames.SetValue("subject", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("body", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("FromiD", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("ToiD", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("RecievedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("SendOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("isProcessed", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("STATUS", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("Type", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("CreatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(MailInfo.GetValue(i), i);




                if (!db.ExecuteProcedure("SpAddMailInOutbox", ColumnNames, DataType, Values))
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
        # endregion AddMailinOutbox

        # region GetAWBOtherCharges
        public DataSet GetAWBOtherCharges(object[] chargesInfo)
        {
            DataSet ds = null;
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;


                ColumnNames.SetValue("frmDate", i);
                Values.SetValue(chargesInfo.GetValue(i), i);
                DataType.SetValue(SqlDbType.VarChar, i);
                i++;

                ColumnNames.SetValue("toDate", i);
                Values.SetValue(chargesInfo.GetValue(i), i);
                DataType.SetValue(SqlDbType.VarChar, i);
                i++;





                ds = db.SelectRecords("rptGetAWBDetailCharges_new", ColumnNames, Values, DataType);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion

        public DataSet GetSLAPerformanceDetails(string StationCode, string FlightNo, string FlightDt, string FromDt, string ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[5];
                object[] values = new object[5];
                SqlDbType[] dataTypes = new SqlDbType[5];
                int i = 0;

                colNames.SetValue("StationCode", i);
                values.SetValue(StationCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightNo", i);
                values.SetValue(FlightNo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FlightDt", i);
                values.SetValue(FlightDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;




                colNames.SetValue("FromDt", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(FromDt, i);

                i++;

                colNames.SetValue("ToDt", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ToDt, i);




                ds = db.SelectRecords("sp_rptSLAPerformanceReport", colNames, values, dataTypes);


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
            if (ds != null)
            {
                ds.Dispose();

            }
            return (null);
        }

        # region Get AWB Summary report with billing using  selecetd criteria
        public DataSet GetAWBBookingwithBillingDetails(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBStatus)
        {
            try
            {

                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AWBStatus", i);
                values.SetValue(AWBStatus, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("rptGetAWBDetailMISForBilling", colNames, values, dataTypes);
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

        # region Get Billing Tonnage report using  selecetd criteria
        public DataSet GetBillingTonnageDetails(DateTime  frmDate, DateTime ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

               
               
                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate.ToString(), i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(ToDt.ToString(), i);
                dataTypes.SetValue(SqlDbType.DateTime, i);


                ds = db.SelectRecords("sp_rptGetBillingTonnageReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        #region DeleteMessage
        public bool DeleteMessage(string srno, string mailbox)
        {
            bool flag = false;
            try
            {
                string[] PName = new string[] { "srno", "mailbox" };
                //object[] PValues = new object[] { Convert.ToInt16(srno), mailbox };
                object[] PValues = new object[] { Convert.ToInt32(srno), mailbox };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar };
                flag = db.ExecuteProcedure("spDeleteMessage", PName, PType, PValues);

            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region UpdateMessageStatus
        public bool UpdateMessageStatus(string srno, string status)
        {
            bool flag = false;
            try
            {
                string[] PName = new string[] { "srno", "status" };
                object[] PValues = new object[] { Convert.ToInt16(srno), status };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar };
                flag = db.ExecuteProcedure("spUpdateMessageStatus", PName, PType, PValues);

            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        public bool UpdateMessageStatus(string srno, string body, string status)
        {
            bool flag = false;
            try
            {
                string[] PName = new string[] { "srno", "status", "body" };
                object[] PValues = new object[] { Convert.ToInt16(srno), status, body };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                flag = db.ExecuteProcedure("spUpdateMessageStatus", PName, PType, PValues);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        # region GetLegWiseReport
        public DataSet GetLegWiseReport(object[] Params)
        {
            DataSet ds = new DataSet();
            try
            {

                string[] QueryNames = new string[4];
                SqlDbType[] QueryTypes = new SqlDbType[4];

                QueryNames[0] = "Origin";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "Dest";
                QueryTypes[1] = SqlDbType.VarChar;

                QueryNames[2] = "FrmDt";
                QueryTypes[2] = SqlDbType.VarChar;

                QueryNames[3] = "ToDt";
                QueryTypes[3] = SqlDbType.VarChar;

                ds = db.SelectRecords("sp_GetLegWiseRpt", QueryNames, Params, QueryTypes);
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
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return (null);
        }
        # endregion

        public DataSet GetDailyAWBSalesReport(DateTime frmDate, DateTime ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[2];
                object[] values = new object[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("FromExecutionDt", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                colNames.SetValue("ToExecutionDt", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);

                ds = db.SelectRecords("sp_rptDailyAWBSalesReport", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # region Get Monthly Stats
        public DataSet GetMonthlyStats(string frmDate, string ToDt, string POL)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[3];
                object[] values = new object[3];
                SqlDbType[] dataTypes = new SqlDbType[3];
                int i = 0;

                colNames.SetValue("FromDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);

                i++;

                colNames.SetValue("POL", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(POL, i);

                ds = db.SelectRecords("Sp_rptGetMonthlyStats", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion
        
        # region Get Unbilled AWB list
        public DataSet GetUnbilledAWBList(string frmDate, string ToDt)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[2];
                object[] values = new object[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("FromDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                ds = db.SelectRecords("SP_GetUnbilledAWBList", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        # endregion 

        # region User Activity Log

        public bool SaveUserActivityLog(string IPAddress, string UserName, string Page, DateTime AccessTime, string Parameters, string ErrorLog, string Station)
        {
            bool blnResult = false;

            try
            {
                string[] colNames = new string[7];
                object[] values = new object[7];
                SqlDbType[] dataTypes = new SqlDbType[7];
                int i = 0;

                colNames.SetValue("IP", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(IPAddress, i);

                i++;

                colNames.SetValue("UserName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(UserName, i);

                i++;

                colNames.SetValue("Page", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(Page, i);

                i++;

                colNames.SetValue("AccessTime", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(AccessTime, i);

                i++;

                colNames.SetValue("Parameters", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(Parameters, i);

                i++;

                colNames.SetValue("ErrorLog", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ErrorLog, i);
                i++;

                colNames.SetValue("Station", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(Station, i);

                blnResult = db.InsertData("SPUpdateReportActivitylog", colNames, dataTypes, values);
            }

            catch (Exception)
            {
                return false;
            }

            return blnResult;
        }

        # endregion 

        public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                string strOutput = objBL.GetMasterConfiguration("ReportInterval");

                if (strOutput != "")
                    DaysConfigured = Convert.ToDouble(strOutput);
                else
                    DaysConfigured = 0;
            }
            catch
            {
                DaysConfigured = 0;
            }
            finally
            {
                objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }

        # region Get AWB Summary report with billing using  selecetd criteria for AAR
        public DataSet GetAWBBookingwithBillingDetails_AAR(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, string frmDate, string ToDt, string AWBStatus)
        {
            try
            {

                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                values.SetValue(levelCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("toDate", i);
                values.SetValue(ToDt, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AWBStatus", i);
                values.SetValue(AWBStatus, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("rptGetAWBDetailMISForBilling_AAR", colNames, values, dataTypes);
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
        #region getAllCollectionDetails
        public DataSet GetAllCollectionDetails_AAR(string AgentCode, string BillingType, string Origin, string fromDate, string toDate,string InvoiceNo, string AWBPrefix, string AWBNo)
        {
            try
            {

                string[] colNames = new string[8];
                string[] values = new string[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("AgentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("BillingType", i);
                values.SetValue(BillingType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("Origin", i);
                values.SetValue(Origin, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("FROMDate", i);
                values.SetValue(fromDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("ToDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("InvoiceNumber", i);
                values.SetValue(InvoiceNo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("AWBPrefix", i);
                values.SetValue(AWBPrefix, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                i++;

                colNames.SetValue("AWBNumber", i);
                values.SetValue(AWBNo, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = db.SelectRecords("SP_AARGetALLCollectionData", colNames, values, dataTypes);
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

        #region Get End Of Shift Data
        public DataSet GetEndOfShiftData(string frmDate, string ToDt, string Employee)
        {
            DataSet ds = null;
            try
            {

                string[] colNames = new string[3];
                object[] values = new object[3];
                SqlDbType[] dataTypes = new SqlDbType[3];
                int i = 0;

                colNames.SetValue("FromDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("ToDate", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(ToDt, i);

                i++;

                colNames.SetValue("Employee", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(Employee, i);


                ds = db.SelectRecords("Sp_rptGetEndOfShiftData", colNames, values, dataTypes);
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
            if (ds != null)
            {
                ds.Dispose();
            }
            return (null);
        }

        #endregion 


        # region EditMessage
        public int EditMessage(object[] MailInfo)
        {
            try
            {

                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //0
                ColumnNames.SetValue("hlfSrNo", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("MsgType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("body", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("EmailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MailInfo.GetValue(i), i);
                i++;


                if (!db.ExecuteProcedure("SpEditMessage", ColumnNames, DataType, Values))
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
    }
}

       
