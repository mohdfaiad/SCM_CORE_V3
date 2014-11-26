using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALDashboard
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        #region Agent Dashboard

        #region Get Total Flight Tonnage for Graph
        /// <summary>
        /// 
        /// </summary>
        /// <param name="QueryValues">
        ///     (FromDate in dd/MM/yyyy),
        ///     (ToDate in dd/MM/yyyy),
        ///     (AgentCode),
        ///     (Station)
        /// </param>
        /// <returns></returns>
        public DataSet GetFlightDataAgent(object[] QueryValues)
        {
            try
            {
                string[] QueryPlotNames = { "FromDate", "ToDate", "AgentCode", "Station" };
                SqlDbType[] QueryPlotType = { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("sp_DashBoard_BookedCargoFlight", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Get AWB Details as per flight

        public DataSet GetAWBDetailsPerFlight(object[] QueryValues)
        {
            try
            {
                string[] QueryN = { "ConfirmationStatus", "BookingStatus", "UserName", "Station", "AgentCode", "FlightNum", "FromDate", "ToDate" };
                SqlDbType[] QueryT = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetAWBStatus", QueryN, QueryValues, QueryT);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }

            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Flights As per Agent Date range selection

        public DataSet GettingAgentFlights(string AgentCode, DateTime FromDt, DateTime ToDt, string Station)
        {
            try
            {
                string[] QueryNames = { "AgentCode", "FromDt", "ToDt", "Station" };
                SqlDbType[] QueryType = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar };
                object[] QueryValues = { AgentCode, FromDt, ToDt, Station };
                DataSet ds = da.SelectRecords("sp_GetAgentWiseFlightList", QueryNames, QueryValues, QueryType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }

            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Invoice Details for Agent

        public DataSet GetInvoiceAgentListing(string AgentCode, string FromDate, string ToDate)
        {
            try
            {
                string[] QueryNames = { "AgentCode", "FromDate", "ToDate" };
                SqlDbType[] QueryType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] QueryValues = { AgentCode, FromDate, ToDate };
                DataSet ds = da.SelectRecords("sp_Dashboard_GetInvoiceListing", QueryNames, QueryValues, QueryType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }

            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Agent Quotes

        public DataSet GetAgentQuotes(string AgentCode, string Station, string FromDate, string ToDate)
        {
            try
            {
                string[] QueryPlotNames = { "AgentCode", "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryPlotType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] QueryValues = { AgentCode, Station, FromDate, ToDate };
                DataSet ds = da.SelectRecords("sp_Dashboard_GetAgentQuotes", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Agent Deals
        public DataSet GetAgentDeals(string AgentCode, string FromDate, string ToDate)
        {
            try
            {
                string[] QueryNames = { "AgentCode", "FromDate", "ToDate" };
                SqlDbType[] QueryType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] QueryValues = { AgentCode, FromDate, ToDate };
                DataSet ds = da.SelectRecords("sp_Dashboard_GetAgentDeals", QueryNames, QueryValues, QueryType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }

            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Agent Capacity Details
        public DataSet GetAgentCapacity(string AgentCode, string Station, string FromDate, string ToDate)
        {
            try
            {
                string[] QueryPlotNames = { "AgentCode", "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryPlotType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] QueryValues = { AgentCode, Station, FromDate, ToDate };
                DataSet ds = da.SelectRecords("sp_Dashboard_GetAgentCapacity", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Agent Claims
        public DataSet GetAgentClaims(string AgentCode, string Station, string FromDate, string ToDate)
        {
            try
            {
                string[] QueryPlotNames = { "AgentCode", "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryPlotType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] QueryValues = { AgentCode, Station, FromDate, ToDate };
                DataSet ds = da.SelectRecords("sp_Dashboard_GetAgentClaims", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #endregion

        #region IT-Admin Dashboard

        #region Getting External User details

        public DataSet GetExternalUsers(string Station, string UserType)
        {
            try
            {
                string[] PNames = new string[2];
                PNames[0] = "Station";
                PNames[1] = "UserType";


                object[] Pvalue = new object[2];
                Pvalue[0] = Station;
                Pvalue[1] = UserType;


                SqlDbType[] Ptype = new SqlDbType[2];
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;


                DataSet ds = da.SelectRecords("sp_DashBoard_ITLoggedInUsers", PNames, Pvalue, Ptype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }

            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Internal User Details
        public DataSet GetInternalUsers(string Station, string UserType)
        {
            try
            {
                string[] PNames = new string[2];
                PNames[0] = "Station";
                PNames[1] = "UserType";

                object[] Pvalue = new object[2];
                Pvalue[0] = Station;
                Pvalue[1] = UserType;

                SqlDbType[] Ptype = new SqlDbType[2];
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;

                DataSet ds = da.SelectRecords("sp_DashBoard_ITLoggedInUsers", PNames, Pvalue, Ptype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }

            }
            catch (Exception ex)
            { return null; }
        }

        #endregion

        #region Getting Report Audit Log
        public DataSet GetReportAuditLog(string FromDt, string ToDt, string User, string RptName)
        {
            try
            {
                string[] PNames = new string[4];
                PNames[0] = "FromDt";
                PNames[1] = "ToDt";
                PNames[2] = "User";
                PNames[3] = "RptName";

                object[] Pvalue = new object[4];
                Pvalue[0] = FromDt;
                Pvalue[1] = ToDt;
                Pvalue[2] = User;
                Pvalue[3] = RptName;

                SqlDbType[] Ptype = new SqlDbType[4];
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;

                DataSet ds = da.SelectRecords("sp_GetRptLogData", PNames, Pvalue, Ptype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region Operations Dashboard

        public DataSet GetExportWarehouseData(object[] QueryValues)
        {
            try
            {
                string[] QueryPlotNames = { "FromDate", "ToDate", "Station" };
                SqlDbType[] QueryPlotType = { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("sp_DashBoard_ExportWarehouse_New", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        public DataSet GetImportWarehouseData(object[] QueryValues)
        {
            try
            {
                string[] QueryPlotNames = { "FromDate", "ToDate", "Station" };
                SqlDbType[] QueryPlotType = { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("sp_DashBoard_ImportWarehouse_New", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        public DataSet GetOutGoingFlightsData(object[] QueryValues)
        {
            try
            {
                string[] QueryPlotNames = { "FromDate", "ToDate", "Station" };
                SqlDbType[] QueryPlotType = { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("sp_DashBoard_OutGoingFlights", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        public DataSet GetInComingFlightsData(object[] QueryValues)
        {
            try
            {
                string[] QueryPlotNames = { "FromDate", "ToDate", "Station" };
                SqlDbType[] QueryPlotType = { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("sp_DashBoard_InComingFlights_New", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        public bool GetAllAWBs(string source, string destination, string flight, string fromdate, string todate, string prefix, string status, string awbnumber, ref DataSet dsResult, ref string errormessage, string AgentCode)
        {
            try
            {
                errormessage = "";
                DateTime fromdt = System.DateTime.Now, todt = System.DateTime.Now;
                try
                {
                    fromdt = DateTime.ParseExact(fromdate, "dd/MM/yyyy", null);
                }
                catch (Exception ex) { }
                try
                {
                    todt = DateTime.ParseExact(todate, "dd/MM/yyyy", null);
                }
                catch (Exception ex) { }
                string[] param = { "Source", "Dest", "Flight", "fromdate", "todate", "Prefix", "Status", "AWBNumber", "AgentCode" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { source, destination, flight, fromdt, todt, prefix, status, awbnumber, AgentCode };

                dsResult = da.SelectRecords("sp_DashBoard_GetOutAWBs", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAllAWBs) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAllAWBs) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public bool GetAWBLevelDataRev(string FlightNo, string FlightDt, string d1, string d360, string flig, string origin, string dest, string agent, ref DataSet dsResult, ref string errormessage, string AWBPrefix, string AWBNo)
        {
            try
            {
                errormessage = "";
                string[] param = { "FlightNo", "FlightDt", "d1", "d360", "flig", "Origin", "Destination", "AgentCode", "AWBPrefix", "AWBNo" };
                SqlDbType[] sqldbtype = { SqlDbType.NVarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { FlightNo, FlightDt, d1, d360, flig, origin, dest, agent, AWBPrefix, AWBNo };
                dsResult = da.SelectRecords("sp_DashBoard_AWBLevelData", param, values, sqldbtype);

                //  dsResult = da.SelectRecords("spgetInternalDataofCapacityNewNew", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAllAWBs) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAllAWBs) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        #endregion

        #region Planner Dashboard

        public DataSet GetSpotRateData(object[] QueryValues)
        {
            try
            {
                string[] QueryPlotNames = { "FromDate", "ToDate", "Station" };
                SqlDbType[] QueryPlotType = { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("sp_DashBoard_SpotRates", QueryPlotNames, QueryValues, QueryPlotType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        public bool GetAllAWBsPlanner(string source, string destination, string flight, string fromdate, string todate, string prefix, string status, string awbnumber, ref DataSet dsResult, ref string errormessage, string AgentCode)
        {
            try
            {
                errormessage = "";
                DateTime fromdt = System.DateTime.Now, todt = System.DateTime.Now;
                try
                {
                    fromdt = DateTime.ParseExact(fromdate, "dd/MM/yyyy", null);
                }
                catch (Exception ex) { }
                try
                {
                    todt = DateTime.ParseExact(todate, "dd/MM/yyyy", null);
                }
                catch (Exception ex) { }
                string[] param = { "Source", "Dest", "Flight", "fromdate", "todate", "Prefix", "Status", "AWBNumber", "AgentCode" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { source, destination, flight, fromdt, todt, prefix, status, awbnumber, AgentCode };

                dsResult = da.SelectRecords("spgetAllMainDataforCapacityPlanning_PlannerDashboard", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAllAWBs) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAllAWBs) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

       public bool GetAWBLevelDataRevPlanner(string FlightNo, string FlightDt, string d1, string d360, string flig, string origin, string dest, string agent, ref DataSet dsResult, ref string errormessage, string AWBPrefix, string AWBNo)
        {
            try
            {
                errormessage = "";
                string[] param = { "FlightNo", "FlightDt", "d1", "d360", "flig", "Origin", "Destination", "AgentCode", "AWBPrefix", "AWBNo" };
                SqlDbType[] sqldbtype = { SqlDbType.NVarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { FlightNo, FlightDt, d1, d360, flig, origin, dest, agent, AWBPrefix, AWBNo };
                dsResult = da.SelectRecords("spgetInternalDataofCapacityRev_Planner", param, values, sqldbtype);

                //  dsResult = da.SelectRecords("spgetInternalDataofCapacityNewNew", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAllAWBs) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAllAWBs) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }
        #endregion

        #region Management Dashboard

        #region Getting Top Ten Flights
        public DataSet GetTopTenFlightsMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenFlights",QueryNames,QueryValues,QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Top Ten Agents
        public DataSet GetTopTenAgentsMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenAgents", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Getting Top Ten Booked Flights

        public DataSet GetTopTenBookedFlightsMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenBookedFlights", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        #endregion

        #region Getting Top Ten Locations

        public DataSet GetTopTenLocationsMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenLocationsFlights", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        #endregion

        #region Getting Top Ten Locations Yield

        public DataSet GetTopTenLocationsYieldMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenLocationsFlightsYield", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        #endregion

        #region Getting Top Ten Locations Revenue

        public DataSet GetTopTenLocationsRevenueMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenLocationsRevenue", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        #endregion

        #region Getting Top Ten Locations Volume

        public DataSet GetTopTenLocationsVolumeMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenLocationsVolume", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        #endregion

        #region Getting Top Ten Locations Shipper

        public DataSet GetTopTenLocationsShipperMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenLocationsShipper", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        #endregion

        #region Getting Top Ten Locations Commodity

        public DataSet GetTopTenLocationsCommodityMgtDashboard(string Station, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string[] QueryNames = { "Station", "FromDate", "ToDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime };
                object[] QueryValues = { Station, FromDate, ToDate };

                DataSet ds = da.SelectRecords("sp_Dashboard_GetTopTenLocationsCommodity", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }

        #endregion


        #endregion


        #region Dashboard Search Date Difference Validation
        public string GetDashBoardInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                string strOutput = objBL.GetMasterConfiguration("DashboardInterval");

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
                return "Records can be searched only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }
        #endregion
    }
}
