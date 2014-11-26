using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
//using clsDataLib;
using QID.DataAccess;

/// <summary>
/// Summary description for BookingBAL
/// </summary>
public class WebSerBookingBAL
{

        #region Variables
        string constr = "";
        public static string OrgStation = "", DestStation = "";
        DataSet dsOtherCharges = null;

        #endregion Variables

        #region Constructor
        public WebSerBookingBAL()
        {
           //constr = constr;
            constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        }
        #endregion Constructor

        #region Get Location List
        /// <summary>
        /// Get information of AWB based on passed values.
        /// </summary>
        /// <returns>Information as list.</returns>
        public DataSet GetAWBInfo(object[] AWBSearch)
        {
            try
            {
                //Prepare column names and datatypes for search parameters...
                string[] paramNames = new string[4];
                SqlDbType[] dataTypes = new SqlDbType[4];
                int i = 0;

                //0
                paramNames.SetValue("DocumentType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("AWBPrefix", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //2
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("AgentCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //Get location names ...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetAWBInfo",paramNames, AWBSearch, dataTypes);
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
        #endregion Get Location List

        #region Validate AWB Number
        /// <summary>
        /// Checks if AWB number is already issued.
        /// </summary>
        /// <returns>True if AWB is not used.</returns>
        public bool ValidateAWB(string AWBNumber,out string HolderName)
        {
            HolderName = ""; 
            try
            {
                //Prepare column names and datatypes...
                string paramName = "AWBNumber";
                SqlDbType dataType = SqlDbType.VarChar;
               
                //Update AWB information.
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spValidateAWB", paramName, AWBNumber, dataType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int res = int.Parse(ds.Tables[0].Rows[0].ToString());
                            if (res > 0)
                            {   //AWB is used.
                                return false;
                            }
                            else
                            {   //AWB is not used.

                                return true;
                            }
                        }
                    }
                }
               
               
            }
            catch (Exception)
            {
            }
            return (true);
        }
        #endregion Save AWB Summary

        #region Save AWB Summary
        /// <summary>
        /// Saves information of AWB.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAWBSummary(object[] AWBInfo)
        {
            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[23];
                SqlDbType[] dataTypes = new SqlDbType[23];
                int i = 0;

                //0
                paramNames.SetValue("BookingID", i);
                dataTypes.SetValue(SqlDbType.BigInt, i);
                i++;

                //1
                paramNames.SetValue("DocType", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //2
                paramNames.SetValue("AWBPrefix", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //3
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //4
                paramNames.SetValue("PiecesCount", i);
                dataTypes.SetValue(SqlDbType.Int,i);
                i++;

                //5
                paramNames.SetValue("GrossWeight", i);
                dataTypes.SetValue(SqlDbType.Float,i);
                i++;
                
                //6
                paramNames.SetValue("VolumetricWeight", i);
                dataTypes.SetValue(SqlDbType.Float,i);
                i++;

                //7
                paramNames.SetValue("ChargedWeight", i);
                dataTypes.SetValue(SqlDbType.Float,i);
                i++;

                //8
                paramNames.SetValue("OriginCode", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //9
                paramNames.SetValue("DestinationCode", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //10
                paramNames.SetValue("AgentCode", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //11
                paramNames.SetValue("AgentName", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //12
                paramNames.SetValue("ServiceCargoClassId", i);
                dataTypes.SetValue(SqlDbType.Int,i);
                i++;

                //13
                paramNames.SetValue("HandlingInfo", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //14
                paramNames.SetValue("ExecutionDate", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //15
                paramNames.SetValue("ExecutedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //16
                paramNames.SetValue("ExecutedAt", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //17
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar,i);
                i++;

                //18
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime,i);
                i++;

                //19
                paramNames.SetValue("IsConsole", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //20
                paramNames.SetValue("IsBonded", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //21
                paramNames.SetValue("IsExport", i);
                dataTypes.SetValue(SqlDbType.Bit, i);
                i++;

                //21
                paramNames.SetValue("IsAgreed", i);
                dataTypes.SetValue(SqlDbType.Bit, i);                

                //Update AWB information.
                SQLServer da = new SQLServer(constr);
                bool res = da.UpdateData("spSaveAWBSummary", paramNames, dataTypes, AWBInfo);
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
        #endregion Save AWB Summary

        #region Delete AWB Material
        /// <summary>
        /// Delete information of Material & Route.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int DeleteAWBDetails(string AWBNumber)
        {
            try
            {                
                //Delete AWB Material information.
                SQLServer da = new SQLServer(constr);
                bool res = da.UpdateData("spDeleteAWBDetails", "AWBNumber", SqlDbType.VarChar, AWBNumber);
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
        #endregion Save AWB Material

        #region Save AWB Material
        /// <summary>
        /// Saves information of Material.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAWBMaterial(object[] MaterialInfo)
        {
            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[10];
                SqlDbType[] dataTypes = new SqlDbType[10];
                int i = 0;

                //0
                paramNames.SetValue("AWBNumber", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("CommodityCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                paramNames.SetValue("Pieces", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //3
                paramNames.SetValue("Weight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //4
                paramNames.SetValue("Dimensions", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("VolumetricWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //6
                paramNames.SetValue("ChargedWeight", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //7
                paramNames.SetValue("UpdatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //8
                paramNames.SetValue("UpdatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //9
                paramNames.SetValue("CommodityDesc", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                //Update AWB information.
                SQLServer da = new SQLServer(constr);
                bool res = da.UpdateData("spSaveAWBMaterial", paramNames, dataTypes, MaterialInfo);
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
        #endregion Save AWB Material

        #region Save AWB Rates

        public bool SaveAWBRates(object[] values)
        {
            try
            {

                string[] param = { "AWBNumber", "CommCode", "PayMode", "Pcs", "Wt", "FrIATA", "FrMKT", "ValCharge", "OcDueCar", "OcDueAgent", "SpotRate", "DynRate", "ServiceTax", "Total" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float };

                SQLServer db = new SQLServer(constr);
                return db.ExecuteProcedure("SP_SaveAWBRates", param, dbtypes, values);

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Save AWB Dimensions

        public bool SaveAWBDimensions(object[] values)
        {
            try
            {

                string[] param = { "AWBNumber", "RowIndex", "Length", "Breadth", "Height", "PcsCount", "MeasureUnit" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int,SqlDbType.VarChar};

                SQLServer db = new SQLServer(constr);
                return db.ExecuteProcedure("SP_SaveAWBDimensions", param, dbtypes, values);

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Save AWB Rates Details

        public bool SaveAWBRatesDetails(string type,object[] values)
        {
            try
            {
                if (type == "Freight")
                {
                    string[] param = { "AWBNumber", "RateLineSrNo", "Type", "DiscountPercent", 
                                       "CommPercent", "TaxPercent", "Discount", "Commission", "Tax","Charge","CommCode"};
                    SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, 
                                            SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar };

                    SQLServer db = new SQLServer(constr);
                    return db.ExecuteProcedure("SP_SaveAWBFrRatesDetails", param, dbtypes, values);
                }
                else
                {
                    string[] param = { "AWBNumber", "ChargeHeadCode", "ChargeType", "DiscountPercent", 
                                       "CommPercent", "TaxPercent", "Discount", "Comission", "Tax","Charge","CommCode"};
                    SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, 
                                            SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar};

                    SQLServer db = new SQLServer(constr);
                    return db.ExecuteProcedure("SP_SaveAWBOCRatesDetails", param, dbtypes, values);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Save AWB Route
        /// <summary>
        /// Saves information of Route.
        /// </summary>
        /// <returns>Result Code.</returns>
        public int SaveAWBRoute(object[] RouteInfo)
        {
            try
            {
                //Prepare column names and datatypes...
                string[] paramNames = new string[] { "AWBNumber", "FltOrigin", "FltDestination", "FltNumber", "FltDate", "Pcs", "Wt", "Status", "Accepted", "AcceptedPcs", "AcceptedWt", "UpdatedBy", "UpdatedOn", "ScheduleID", "ChrgWt" };
                SqlDbType[] dataTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.Int, SqlDbType.Float};


                //Update AWB information.
                SQLServer da = new SQLServer(constr);
                bool res = da.UpdateData("spSaveAWBRoute", paramNames, dataTypes, RouteInfo);
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
        #endregion Save AWB Route

        #region Save AWB Shipper/Consignee

        public bool SaveAWBShipperConsignee(object[] values)
        {
            try
            {

                string[] param = { "AWBNumber", "ShipperName", "ShipperAddress", "ShipperCountry", "ShipperTelephone", "ConsigneeName", "ConsigneeAddress", "ConsigneeCountry", "ConsigneeTelephone" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                SQLServer db = new SQLServer(constr);
                return db.ExecuteProcedure("SP_InsertAWBShipperConsigneeDetails", param, dbtypes, values);

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Get Location List
        /// <summary>
        /// Get list of all the locations based on entered value.
        /// </summary>
        /// <returns>Location list as Array.</returns>
        public DataSet GetLocationList(string LocCode)
        {
            try
            {
                //Get location names ...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetLocationsByPrefix", "StationCode", LocCode, SqlDbType.VarChar);
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
        #endregion Get Location List

        #region Get Destination List
        /// <summary>
        /// Get list of all the locations based on entered value.
        /// </summary>
        /// <returns>Location list as Array.</returns>
        public DataSet GetDestinationsForSource(string source)
        {
            try
            {
                //Get location names ...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetDestinations", "source", source, SqlDbType.VarChar);
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

        #region Get Agent List
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgentList(string agentCode)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetAgentByPrefix", "AgentCode", agentCode, SqlDbType.VarChar);
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
        #endregion Get Agent List

        #region Get Agent List
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgentListByStation(string station)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetAgentByStation", "Station", station, SqlDbType.VarChar);
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
        #endregion Get Agent List

        #region Get Commodity List
        /// <summary>
        /// Get list of all the commodities based on entered value.
        /// </summary>
        /// <returns>Commodity code list as Array.</returns>
        public DataSet GetCommodityList(string commodityCode)
        {
            try
            {
                //Get commodity codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetCommodityByPrefix", "CommodityCode", commodityCode, SqlDbType.VarChar);
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
        #endregion Get Commodity List

        #region Get Flight List
        /// <summary>
        /// Get list of all the flights based on entered value.
        /// </summary>
        /// <returns>Flight code list as Array.</returns>
        public DataSet GetFlightList(string flightCode)
        {
            try
            {
                //Get flight codes...
                SQLServer da = new SQLServer("");
                DataSet ds = da.SelectRecords("spGetFlightByPrefix", "FlightCode", flightCode, SqlDbType.VarChar);
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

        public bool GetFlightList(string origin,string destination,ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                //Get flight codes...
                SQLServer da = new SQLServer(constr);

                string[] param = { "Origin","Dest"};
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { origin, destination };

                dsResult = da.SelectRecords("spGetFlightListFromOrgToDest", param, values, sqldbtype);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            errormessage = "No record found";
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            
        }
     


        #endregion Get Flight List

        #region Get Flight List By Route
        /// <summary>
        /// Gets list of flights based on Origin and Destination of flight.
        /// </summary>
        /// <param name="Origin">Origin location for flight.</param>
        /// <param name="Dest">Destination location for flight.</param>
        /// <returns>Returns list as string separated by ';'.</returns>
        public string GetFlightListByRoute(string Origin, string Dest)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("Origin", i);
                values.SetValue(Origin, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Destination", i);
                values.SetValue(Dest, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                DataSet ds = da.SelectRecords("spGetFlightForRoute", colNames, values, dataTypes);
                string flightList = " Select";
                if (ds!= null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        flightList = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            flightList = flightList + dr["FlightNumber"].ToString() + ";";
                        }
                    }
                }

                return (flightList);

            }
            catch (Exception )
            {
                return ("");
            }

        }
        #endregion Get Flight List By Route

        #region Get Next AWB Number
        /// <summary>
        /// Checks if AWB number is already issued.
        /// </summary>
        /// <returns>True if AWB is not used.</returns>
        public bool GetNextAWB(string AgentCode, ref string AWBNumber, ref string errormessage)
        {

            try
            {
                //Prepare column names and datatypes...
                string paramName = "AgentCode";
                SqlDbType dataType = SqlDbType.VarChar;


                //Update AWB information.
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetNextAWBNumberForAgent", paramName, AgentCode, dataType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0].ToString().Trim() != "0" || ds.Tables[0].Rows[0].ToString().Trim() != "")
                            {   //AWB is used.
                                AWBNumber = ds.Tables[0].Rows[0][0].ToString();
                                return true;
                            }
                            else
                            {   //AWB is not used.
                                errormessage = "Stock not allocated.";
                                return false;
                            }
                        }
                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        #endregion Get Next AWB Number

        #region ProcessRates

        public DataSet ProcessRates(string AWBNo,string CommCode,decimal ChargedWt)
        {

            try
            {
                    
                // Result
                DataSet dsResult = new DataSet();
                dsResult.Tables.Add();
                dsResult.Tables[0].TableName = "Rates";
                dsResult.Tables[0].Columns.Add("FrIATA");
                dsResult.Tables[0].Columns.Add("FrMKT");
                dsResult.Tables[0].Columns.Add("OCDC");
                dsResult.Tables[0].Columns.Add("OCDA");
                dsResult.Tables[0].Columns.Add("ServTax");
                               


                // OC
                DataSet dsOCDetails = new DataSet();
                dsOCDetails.Tables.Add();
                dsOCDetails.Tables[0].TableName = "OCDA";
                dsOCDetails.Tables[0].Columns.Add("Commodity Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Head Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Type");
                dsOCDetails.Tables[0].Columns.Add("Charge");
                dsOCDetails.Tables[0].Columns.Add("TaxPercent");
                dsOCDetails.Tables[0].Columns.Add("Tax");
                dsOCDetails.Tables[0].Columns.Add("DiscountPercent");
                dsOCDetails.Tables[0].Columns.Add("Discount");
                dsOCDetails.Tables[0].Columns.Add("CommPercent");
                dsOCDetails.Tables[0].Columns.Add("Commission");



                //Rates
                DataTable dtRates = new DataTable();
                dtRates.Columns.Add("CommCode");
                dtRates.Columns.Add("Pcs");
                dtRates.Columns.Add("Weight");
                dtRates.Columns.Add("FrIATA");
                dtRates.Columns.Add("FrMKT");
                dtRates.Columns.Add("ValCharge");
                dtRates.Columns.Add("PayMode");
                dtRates.Columns.Add("OcDueCar");
                dtRates.Columns.Add("OcDueAgent");
                dtRates.Columns.Add("SpotRate");
                dtRates.Columns.Add("DynRate");
                dtRates.Columns.Add("ServTax");
                dtRates.Columns.Add("Total");


                // Get AWB Info
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetAWBInfoToProcessRates", "AWBNumber", AWBNo, SqlDbType.VarChar);

                string Orgin, Dest, BookDt, AgentCode, FlightNum, errormessage="";

                Orgin = ds.Tables[0].Rows[0][0].ToString();
                Dest = ds.Tables[0].Rows[0][1].ToString();
                BookDt = Convert.ToDateTime(ds.Tables[0].Rows[0][2].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                AgentCode = ds.Tables[0].Rows[0][3].ToString();
                FlightNum = ds.Tables[0].Rows[0][4].ToString();

                if (CommCode.Trim() != "" && ChargedWt > 0)
                {
                    ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage);
                }
                else
                {

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        CommCode = row[0].ToString();
                        ChargedWt = decimal.Parse(row[1].ToString());

                        ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage);
                    }
                }

                decimal friata, frmkt, ocda, ocdc, tax;
                friata = frmkt = ocda = ocdc = tax = 0;

                foreach (DataRow rw in dtRates.Rows)
                {
                    friata += decimal.Parse(rw["FrIATA"].ToString());
                    frmkt += decimal.Parse(rw["FrMKT"].ToString());
                    ocda += decimal.Parse(rw["OcDueAgent"].ToString());
                    ocdc += decimal.Parse(rw["OcDueCar"].ToString());
                    tax += decimal.Parse(rw["ServTax"].ToString());

                }

                DataRow dsResultRow = dsResult.Tables[0].NewRow();

                dsResultRow["FrIATA"]=friata;
                dsResultRow["FrMKT"]=frmkt;
                dsResultRow["OCDC"]=ocdc;
                dsResultRow["OCDA"]=ocda;
                dsResultRow["ServTax"] = tax;

                dsResult.Tables[0].Rows.Add(dsResultRow);

                return dsResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void ProcessRatesByCommodity(ref DataSet dsOCDetails, ref DataTable dtRates, string CommCode, decimal ChargedWt, string Origin, string Dest, string BookDt, string AgentCode, string FlightNum, ref string errormessage)
        {
            try
            {

                string  RateCardType, AirlineCode, HandlingCode, ShipperCode;
                decimal GrossWt;
                decimal IATACharge, MKTCharge, ServiceTax, OCDC, OCDA,
                        TotalDiscount, TotalCommission,

                        DiscountPercentIATA, DiscountPercentMKT,
                        CommissionPercentIATA, CommissionPercentMKT,
                        TaxPercentIATA, TaxPercentMKT,

                        DiscountIATA, DiscountMKT,
                        CommissionIATA, CommissionMKT,
                        ServiceTaxIATA, ServiceTaxMKT;

                string OCDADetails, OCDCDetails, RateLineIATA = "", RateLineMKT = "";


                OCDADetails = OCDCDetails = "";
                IATACharge = MKTCharge = ServiceTax = ServiceTaxIATA = ServiceTaxMKT = OCDC = OCDA = TotalDiscount = TotalCommission = DiscountPercentIATA = CommissionPercentIATA = DiscountPercentMKT = CommissionPercentMKT = TaxPercentIATA = TaxPercentMKT = 0;
                DiscountIATA = DiscountMKT = CommissionIATA = CommissionMKT = ServiceTaxIATA = ServiceTaxMKT = 0;


               // Origin = ddlOrg.Text;
               // Dest = ddlDest.Text;
               // BookDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
               // FlightNum = ((HiddenField)grdRouting.Rows[0].FindControl("hdnFltNum")).Value;
               // AgentCode = ddlAgtCode.SelectedItem.Text;
               // CommCode = ((DropDownList)grdMaterialDetails.Rows[rowindex].FindControl("ddlMaterialCommCode")).SelectedItem.Text;
                RateCardType = "IATA";
                AirlineCode = "SG";
                HandlingCode = "";
                ShipperCode = "";
               // GrossWt = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("txtCommGrossWt")).Text);
               // ChargedWt = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("txtCommChargedWt")).Text);

               //if (GrossWt < ChargedWt)
                 GrossWt = ChargedWt;


                string[] param = { "org", "dest", "BookDt", "FlightNum", "AgentCode", "CommCode", "RateCardType", "GrossWt", "HandlingCode", "ShipperCode", "AirlineCode" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { Origin, Dest, BookDt, FlightNum, AgentCode, CommCode, RateCardType, GrossWt, HandlingCode, ShipperCode, AirlineCode };

                SQLServer db = new SQLServer(constr);
                DataSet dsResultIATA = db.SelectRecords("SP_RateCalculation", param, values, dbtypes);
                values[6] = "Market";
                DataSet dsResultMKT = db.SelectRecords("SP_RateCalculation", param, values, dbtypes);

                // IATA

                if (dsResultIATA != null)
                {

                    if (dsResultIATA.Tables.Count != 0)
                    {

                        if (dsResultIATA.Tables[0].Rows.Count != 0)
                        {
                            // done

                            RateLineIATA = dsResultIATA.Tables[0].Rows[0]["RateLineID"].ToString();
                            IATACharge = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Charge"].ToString());

                            ServiceTax = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["ServiceTax"].ToString());
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Commission"].ToString());

                            DiscountPercentIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["DiscountPercent"].ToString());
                            CommissionPercentIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["CommissionPercent"].ToString());
                            TaxPercentIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["TaxPercent"].ToString());

                            DiscountIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Discount"].ToString());
                            CommissionIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["Commission"].ToString());
                            ServiceTaxIATA = Convert.ToDecimal(dsResultIATA.Tables[0].Rows[0]["ServiceTax"].ToString());


                        }
                        else
                            errormessage = "Error :Code-O03";

                    }
                    else
                        errormessage = "Error :Code-O02";
                }
                else
                    errormessage = "Error :Code-O01";


                // MKT

                if (dsResultMKT != null)
                {

                    if (dsResultMKT.Tables.Count != 0)
                    {

                        if (dsResultMKT.Tables[0].Rows.Count != 0)
                        {
                            // done

                            RateLineMKT = dsResultMKT.Tables[0].Rows[0]["RateLineID"].ToString();
                            MKTCharge = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Charge"].ToString());

                            ServiceTax = ServiceTax + Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["ServiceTax"].ToString());
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Commission"].ToString());

                            DiscountPercentMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["DiscountPercent"].ToString());
                            CommissionPercentMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["CommissionPercent"].ToString());
                            TaxPercentMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["TaxPercent"].ToString());

                            DiscountMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Discount"].ToString());
                            CommissionMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["Commission"].ToString());
                            ServiceTaxMKT = Convert.ToDecimal(dsResultMKT.Tables[0].Rows[0]["ServiceTax"].ToString());


                        }
                        else
                            errormessage = "Error :Code-O06";
                    }
                    else
                        errormessage = "Error :Code-O05";
                }
                else
                    errormessage = "Error :Code-O04";







                #region Other Charges

                param = new string[] { "org", "dest", "BookDt", "FlightNum", "AgentCode", "CommCode", "ChargeType", "GrossWt", "ChargedWt", "HandlingCode", "ShipperCode", "AirlineCode", "HAWB" };
                dbtypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int };
                values = new object[] { Origin, Dest, BookDt, FlightNum, AgentCode, CommCode, "DA", GrossWt, ChargedWt, HandlingCode, ShipperCode, AirlineCode, 0 };

                db = new SQLServer(constr);


                //OCDA

                DataSet dsResultOtherCharges = db.SelectRecords("SP_RateCalculationForOtherCharges", param, values, dbtypes);

                if (dsResultOtherCharges != null)
                {

                    if (dsResultOtherCharges.Tables.Count != 0)
                    {

                        if (dsResultOtherCharges.Tables[0].Rows.Count != 0)
                        {
                            // done
                            // TotalCharge Tax Discount Commission Details

                            OCDA = Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["TotalCharge"].ToString());
                            ServiceTax = ServiceTax + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Tax"].ToString());
                            OCDADetails = dsResultOtherCharges.Tables[0].Rows[0]["Details"].ToString();
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Commission"].ToString());


                        }
                        else
                            errormessage = "Error :Code-O06";
                    }
                    else
                        errormessage = "Error :Code-O05";
                }
                else
                    errormessage = "Error :Code-O04";




                //OCDC

                values[6] = "DC";
                dsResultOtherCharges = db.SelectRecords("SP_RateCalculationForOtherCharges", param, values, dbtypes);

                if (dsResultOtherCharges != null)
                {

                    if (dsResultOtherCharges.Tables.Count != 0)
                    {

                        if (dsResultOtherCharges.Tables[0].Rows.Count != 0)
                        {
                            // done
                            // TotalCharge Tax Discount Commission Details

                            OCDC = Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["TotalCharge"].ToString());
                            ServiceTax = ServiceTax + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Tax"].ToString());
                            OCDCDetails = dsResultOtherCharges.Tables[0].Rows[0]["Details"].ToString();
                            TotalDiscount = TotalDiscount + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Discount"].ToString());
                            TotalCommission = TotalCommission + Convert.ToDecimal(dsResultOtherCharges.Tables[0].Rows[0]["Commission"].ToString());

                        }
                        else
                            errormessage = "Error :Code-O06";
                    }
                    else
                        errormessage = "Error :Code-O05";
                }
                else
                    errormessage = "Error :Code-O04";



                // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission

                //DataSet dsDetails = (DataSet)Session["OCDetails"];
                //if (dsDetails != null && dsDetails.Tables.Count != 0)
                //    dsDetails.Tables[0].Rows.Clear();



                //DataSet dsDetails = (DataSet)Session["OCDetails"];
                DataSet dsDetails = dsOCDetails;
                //if (dsDetails != null && dsDetails.Tables.Count != 0)
                //    dsDetails.Tables[0].Rows.Clear();


                // OCDA
                string[] strOCDADetails = OCDADetails.Split(new char[] { ',' });

                for (int i = 0; i < strOCDADetails.Length; i++)
                {
                    string[] strDAdetails = strOCDADetails[i].Split(new char[] { ':' });
                    DataRow rowDA = dsDetails.Tables[0].NewRow();

                    rowDA["Commodity Code"] = CommCode;
                    rowDA["Charge Head Code"] = strDAdetails[0].ToString();
                    rowDA["Charge Type"] = "OCDA";
                    rowDA["Charge"] = strDAdetails[1].ToString();
                    rowDA["TaxPercent"] = strDAdetails[2].ToString();
                    rowDA["Tax"] = strDAdetails[3].ToString();
                    rowDA["DiscountPercent"] = strDAdetails[4].ToString();
                    rowDA["Discount"] = strDAdetails[5].ToString();
                    rowDA["CommPercent"] = strDAdetails[6].ToString();
                    rowDA["Commission"] = strDAdetails[7].ToString();

                    dsDetails.Tables[0].Rows.Add(rowDA);
                }


                // OCDC
                string[] strOCDCDetails = OCDCDetails.Split(new char[] { ',' });

                for (int i = 0; i < strOCDCDetails.Length; i++)
                {
                    string[] strDCdetails = strOCDCDetails[i].Split(new char[] { ':' });
                    DataRow rowDC = dsDetails.Tables[0].NewRow();

                    rowDC["Commodity Code"] = CommCode;
                    rowDC["Charge Head Code"] = strDCdetails[0].ToString();
                    rowDC["Charge Type"] = "OCDC";
                    rowDC["Charge"] = strDCdetails[1].ToString();
                    rowDC["TaxPercent"] = strDCdetails[2].ToString();
                    rowDC["Tax"] = strDCdetails[3].ToString();
                    rowDC["DiscountPercent"] = strDCdetails[4].ToString();
                    rowDC["Discount"] = strDCdetails[5].ToString();
                    rowDC["CommPercent"] = strDCdetails[6].ToString();
                    rowDC["Commission"] = strDCdetails[7].ToString();

                    dsDetails.Tables[0].Rows.Add(rowDC);
                }

                #endregion


                // ADD FrIATA details

                DataRow dsDetailsRow = dsDetails.Tables[0].NewRow();

                dsDetailsRow["Commodity Code"] = CommCode;
                dsDetailsRow["Charge Head Code"] = RateLineIATA;
                dsDetailsRow["Charge Type"] = "RateLineIATA";
                dsDetailsRow["Charge"] = IATACharge;
                dsDetailsRow["TaxPercent"] = TaxPercentIATA;
                dsDetailsRow["Tax"] = ServiceTaxIATA;
                dsDetailsRow["DiscountPercent"] = DiscountPercentIATA;
                dsDetailsRow["Discount"] = DiscountIATA;
                dsDetailsRow["CommPercent"] = CommissionPercentIATA;
                dsDetailsRow["Commission"] = CommissionIATA;

                dsDetails.Tables[0].Rows.Add(dsDetailsRow);


                // ADD FrMKT details 

                dsDetailsRow = dsDetails.Tables[0].NewRow();

                dsDetailsRow["Commodity Code"] = CommCode;
                dsDetailsRow["Charge Head Code"] = (RateLineMKT.Trim() == "" ? "0" : RateLineMKT);
                dsDetailsRow["Charge Type"] = "RateLineMKT";
                dsDetailsRow["Charge"] = MKTCharge;
                dsDetailsRow["TaxPercent"] = TaxPercentMKT;
                dsDetailsRow["Tax"] = ServiceTaxMKT;
                dsDetailsRow["DiscountPercent"] = DiscountPercentMKT;
                dsDetailsRow["Discount"] = DiscountMKT;
                dsDetailsRow["CommPercent"] = CommissionPercentMKT;
                dsDetailsRow["Commission"] = CommissionMKT;

                dsDetails.Tables[0].Rows.Add(dsDetailsRow);


                //Session["OCDetails"] = dsDetails.Copy();



                //DataTable dt = ((DataTable)Session["dtRates"]).Copy();
                DataTable dt = dtRates;
                //dt.Rows.Clear();

                DataRow row;
                row = dt.NewRow();
                row["CommCode"] = CommCode;// "TV";
                //row["Pcs"] = ((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("TXTPcs")).Text;
                row["Pcs"] = "";
                row["Weight"] = GrossWt;// "7KG";
                row["FrIATA"] = "" + IATACharge;// "IATA";
                row["FrMKT"] = "" + MKTCharge;// "IATA";
                row["ValCharge"] = "0";// "IATA";
                row["PayMode"] = "";// "COD";
                row["OcDueCar"] = "" + OCDC;// "200.00";
                row["OcDueAgent"] = "" + OCDA;// "200.00";
                row["SpotRate"] = "0";// "500.00";
                row["DynRate"] = "0";// "0";
                row["ServTax"] = "" + ServiceTax;// "0";
                row["Total"] = "" + (IATACharge + MKTCharge + ServiceTax + OCDA + OCDC);// "0";
                dt.Rows.Add(row);


                //Session["dtRates"] = dt.Copy();


                //GRDRates.DataSource = null;
                //GRDRates.DataSource = dt.Copy();
                //GRDRates.DataBind();


            }
            catch (Exception ex)
            {

            }

        }


        public DataSet ProcessRatesByFlightNo(string AWBNo, string CommCode, decimal ChargedWt, string FlightNo)
        {

            try
            {

                // Result
                DataSet dsResult = new DataSet();
                dsResult.Tables.Add();
                dsResult.Tables[0].TableName = "Rates";
                dsResult.Tables[0].Columns.Add("FrIATA");
                dsResult.Tables[0].Columns.Add("FrMKT");
                dsResult.Tables[0].Columns.Add("OCDC");
                dsResult.Tables[0].Columns.Add("OCDA");
                dsResult.Tables[0].Columns.Add("ServTax");



                // OC
                DataSet dsOCDetails = new DataSet();
                dsOCDetails.Tables.Add();
                dsOCDetails.Tables[0].TableName = "OCDA";
                dsOCDetails.Tables[0].Columns.Add("Commodity Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Head Code");
                dsOCDetails.Tables[0].Columns.Add("Charge Type");
                dsOCDetails.Tables[0].Columns.Add("Charge");
                dsOCDetails.Tables[0].Columns.Add("TaxPercent");
                dsOCDetails.Tables[0].Columns.Add("Tax");
                dsOCDetails.Tables[0].Columns.Add("DiscountPercent");
                dsOCDetails.Tables[0].Columns.Add("Discount");
                dsOCDetails.Tables[0].Columns.Add("CommPercent");
                dsOCDetails.Tables[0].Columns.Add("Commission");



                //Rates
                DataTable dtRates = new DataTable();
                dtRates.Columns.Add("CommCode");
                dtRates.Columns.Add("Pcs");
                dtRates.Columns.Add("Weight");
                dtRates.Columns.Add("FrIATA");
                dtRates.Columns.Add("FrMKT");
                dtRates.Columns.Add("ValCharge");
                dtRates.Columns.Add("PayMode");
                dtRates.Columns.Add("OcDueCar");
                dtRates.Columns.Add("OcDueAgent");
                dtRates.Columns.Add("SpotRate");
                dtRates.Columns.Add("DynRate");
                dtRates.Columns.Add("ServTax");
                dtRates.Columns.Add("Total");


                // Get AWB Info
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetAWBInfoToProcessRates", "AWBNumber", AWBNo, SqlDbType.VarChar);

                string Orgin, Dest, BookDt, AgentCode, FlightNum, errormessage = "";

                Orgin = ds.Tables[0].Rows[0][0].ToString();
                Dest = ds.Tables[0].Rows[0][1].ToString();
                BookDt = Convert.ToDateTime(ds.Tables[0].Rows[0][2].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                AgentCode = ds.Tables[0].Rows[0][3].ToString();
                FlightNum = FlightNo; // ds.Tables[0].Rows[0][4].ToString(); Based on Flight Number passed

                if (CommCode.Trim() != "" && ChargedWt > 0)
                {
                    ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage);
                }
                else
                {

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        CommCode = row[0].ToString();
                        ChargedWt = decimal.Parse(row[1].ToString());

                        ProcessRatesByCommodity(ref dsOCDetails, ref dtRates, CommCode, ChargedWt, Orgin, Dest, BookDt, AgentCode, FlightNum, ref errormessage);
                    }
                }

                decimal friata, frmkt, ocda, ocdc, tax;
                friata = frmkt = ocda = ocdc = tax = 0;

                foreach (DataRow rw in dtRates.Rows)
                {
                    friata += decimal.Parse(rw["FrIATA"].ToString());
                    frmkt += decimal.Parse(rw["FrMKT"].ToString());
                    ocda += decimal.Parse(rw["OcDueAgent"].ToString());
                    ocdc += decimal.Parse(rw["OcDueCar"].ToString());
                    tax += decimal.Parse(rw["ServTax"].ToString());

                }

                DataRow dsResultRow = dsResult.Tables[0].NewRow();

                dsResultRow["FrIATA"] = friata;
                dsResultRow["FrMKT"] = frmkt;
                dsResultRow["OCDC"] = ocdc;
                dsResultRow["OCDA"] = ocda;
                dsResultRow["ServTax"] = tax;

                dsResult.Tables[0].Rows.Add(dsResultRow);

                return dsResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion

        public bool GetAWBDetails(string AWBNumber,ref DataSet dsResult,ref string errormessage)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                dsResult = da.SelectRecords("SP_GetAWBDetails", "AWBNumber", AWBNumber, SqlDbType.VarChar);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAWBDetails) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAWBDetails) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "Error :(GetAWBDetails)" + ex.Message;
                return false;
            }
        }

        #region Get Special Commodities
        /// <summary>
        /// Get list of all the locations based on entered value.
        /// </summary>
        /// <returns>Location list as Array.</returns>
        public DataSet GetSpecialCommodities()
        {
            try
            {
                //Get location names ...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_GetSpecialCommodityCodes");
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

        #region CalculateRateForSpecialComm
        
        public bool CalculateRateForSpecialComm(string commodity, float wt, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                
                SQLServer da = new SQLServer(constr);

                string[] param = { "Commodity", "Wt" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.Float };
                object[] values = { commodity, wt };

                dsResult = da.SelectRecords("SP_CalculateRateForSpecialComm", param, values, sqldbtype);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            errormessage = "No record found";
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }
        
        #endregion 

        #region CalculateRateForSpecialCommV2

        public bool CalculateRateForSpecialCommV2(string commodity, float wt,int genericrateline, ref DataSet dsResult, ref string errormessage)
        {
            try
            {

                SQLServer da = new SQLServer(constr);

                string[] param = { "Commodity", "Wt","GenericRateLineID" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Int };
                object[] values = { commodity, wt, genericrateline };

                dsResult = da.SelectRecords("SP_CalculateRateForSpecialCommV2", param, values, sqldbtype);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            errormessage = "No record found";
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        #endregion 
        
        #region AWBStatus

        public bool SetAWBStatus(string AWBNumber,string Status, ref string errormessage)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] param = { "AWBNumber", "Status" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { AWBNumber, Status };

                DataSet dsResult = da.SelectRecords("SP_SetAWBStatus", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString() == "Y")
                                return true;
                            else
                            {
                                errormessage = "" + dsResult.Tables[0].Rows[0][1].ToString();
                                return false;
                            }
                        }
                        else
                        {
                            errormessage = "AWB Details not found.";
                            return false;
                        }

                    }
                    else
                    {
                        errormessage = "Error :(SetAWBStatus) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(SetAWBStatus) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }


        public bool GetAWBStatus(string AWBNumber,ref string Status, ref string errormessage)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] param = { "AWBNumber" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar };
                object[] values = { AWBNumber };

                DataSet ds = da.SelectRecords("SP_GetAWBStatus", param, values, sqldbtype);

                if (ds != null)
                {
                    if (ds.Tables.Count != 0)
                    {

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            Status = ds.Tables[0].Rows[0][0].ToString();
                            return true;
                        }
                        else
                        {
                            errormessage = "AWB Details not found.";
                            return false;
                        }

                    }
                    else
                    {
                        errormessage = "Error :(GetAWBStatus) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAWBStatus) Code I";
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

        # region GetAWBStock
        public DataSet GetAWBStock(object[] AWB)
        {
            try
            {

                string[] ParamNames = new string[] { "AWBNumber" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };

                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpGetAgentStockDetails", ParamNames, AWB, ParamTypes);
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

        public DataSet VerifyEmbargoCargo(DateTime Ondate, string Origin, string Destination, string Commodity, string PaymentType, string FlightNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[6];
                object[] Pvalue = new object[6];
                SqlDbType[] Ptype = new SqlDbType[6];

                Pname[0] = "dtOnDate";
                Pname[1] = "strOrigin";
                Pname[2] = "strDestination";
                Pname[3] = "strCommodity";
                Pname[4] = "strPaymentType";
                Pname[5] = "strFlightNo";

                Ptype[0] = SqlDbType.DateTime;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                Pvalue[0] = Ondate;
                Pvalue[1] = Origin;
                Pvalue[2] = Destination;
                Pvalue[3] = Commodity;
                Pvalue[4] = PaymentType;
                Pvalue[5] = FlightNo;

                DataSet objDS = da.SelectRecords("spValidateEMBARGOCargo", Pname, Pvalue ,Ptype );

                return objDS;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public bool GetScheduleId(string Origin, string Destination, string FlightNo, ref int ScheduleId)
        {
            try
            {
                ScheduleId = 0;

                SQLServer da = new SQLServer(constr);

                string strQuery = "Select top 1 ScheduleID from AirlineScheduleRoute where FlightID='" + FlightNo.Trim() + "' and Source='" + Origin.Trim() + "' and Dest='" + Destination + "' order by ScheduleID desc";

                DataSet ds = da.GetDataset(strQuery);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    return true;
                }
                else
                {
                    ScheduleId = 0;
                    return false;
                }

            }
            catch (Exception ex)
            {
                ScheduleId = 0;
                return false;
            }
        }

        public DataSet GetAWBDimensions(string AWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string strQuery = "select * from dbo.AWBDimensions where AWBNumber='" + AWBNumber.Trim() + "'";

                DataSet ds = da.GetDataset(strQuery);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {   
                    return null;
                }

            }
            catch (Exception ex)
            {   
                return null;
            }
        }

        #region "Calculate AWB Charges"

        public bool GetAWBCharges(string Origin, string Destination, string FlightNo, string Commodity, string AgentCode, string Carrier, decimal Weight, ref decimal IATACharge, ref decimal IATATax, ref decimal MKTRate, ref decimal MKTTax, ref float OCDC, ref float OCDA, ref float OCTax, ref DataSet dsOther, DateTime fltDate)
        {

            bool IsSpecial = false;
            IATACharge = 0; IATATax = 0; MKTRate = 0; MKTTax = 0;
            dsOther = null;
            dsOtherCharges = null;

            IsSpecial = IsSpecialComm(Commodity);

            if (IsSpecial == false)
            {
                GetChargeRate(Origin, Destination, FlightNo, Commodity, AgentCode, Carrier, "IATA", Weight, ref IATACharge, ref IATATax, fltDate);
                GetChargeRate(Origin, Destination, FlightNo, Commodity, AgentCode, Carrier, "MKT", Weight, ref MKTRate, ref MKTTax, fltDate);                
            }
            else
            {
                GetChargeRateForSpecial(Origin, Destination, FlightNo, Commodity, AgentCode, Carrier, "IATA", Weight, ref IATACharge, ref IATATax, fltDate);
                GetChargeRateForSpecial(Origin, Destination, FlightNo, Commodity, AgentCode, Carrier, "MKT", Weight, ref MKTRate, ref MKTTax, fltDate);                
            }

            dsOther = GetOtherCharges(Origin, Destination, FlightNo, Commodity, AgentCode, Carrier, Weight, IATACharge);
            dsOther = dsOtherCharges;

            OCDC = 0;OCDA =0; OCTax = 0;
            SetOtherChargesSummary(ref OCDC, ref OCDA, ref OCTax);

            return true;
        }

        public bool IsSpecialComm(string commodity)
        {
            try
            {                
                DataSet dsCommodities = GetSpecialCommodities();
                
                foreach (DataRow row in dsCommodities.Tables[0].Rows)
                {
                    if (row["CommodityCode"].ToString() == commodity)
                    {
                        return true;
                    }

                }

                return false;           

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool GetChargeRate(string Origin, string Destination, string FlightNo, string Commodity, string AgentCode, string Carrier, string RateType, decimal Weight, ref decimal Charge, ref decimal Tax, DateTime fltDate)
        {
            DataSet dsResult = null;
            string errormessage = string.Empty;
            decimal IATAcharge = 0;
            decimal IATAtax = 0;

            if (GetRateLineFor(Origin, Destination, RateType, FlightNo, AgentCode, Commodity, Carrier, Weight, ref dsResult, ref errormessage, fltDate))
            {
                IATAcharge = decimal.Parse(dsResult.Tables[0].Rows[0]["Charge"].ToString());
                IATAtax = decimal.Parse(dsResult.Tables[0].Rows[0]["ServiceTax"].ToString());                
            }

            Charge = IATAcharge;
            Tax = IATAtax;
            return true;            
        }

        public bool GetRateLineFor(string source, string dest, string mode, string flight, string agentcode, string commcode, string carrier, decimal wt, ref DataSet dsResult, ref string errormessage, DateTime fltDate)
        {
            try
            {
                errormessage = "";

                source = source.Trim();
                dest = dest.Trim();
                flight = flight.Trim();
                agentcode = agentcode.Trim();
                commcode = commcode.Trim();
                carrier = carrier.Trim();


                DataSet dsAllRateLines = new DataSet();
                DataSet dsPriorities = new DataSet();

                if (GetAllRateLinesFor(source, dest, mode, ref dsAllRateLines, ref errormessage, fltDate))
                {
                    // dsAllRateLines contains all rate lines
                    string regionSource, regionDest, countrySource, countryDest;
                    regionSource = regionDest = countrySource = countryDest = "";

                    regionSource = dsAllRateLines.Tables[3].Rows[0]["RegionSource"].ToString();
                    regionDest = dsAllRateLines.Tables[3].Rows[0]["RegionDest"].ToString();
                    countrySource = dsAllRateLines.Tables[3].Rows[0]["CountrySource"].ToString();
                    countryDest = dsAllRateLines.Tables[3].Rows[0]["CountryDest"].ToString();

                    if (GetPriority(ref dsPriorities, ref errormessage))
                    {
                        string GenericRateLine = "";
                        string MatchingRateLine = "";
                        foreach (DataRow row in dsPriorities.Tables[0].Rows)
                        {
                            string filter = "";
                            if (row["Level"].ToString().Trim() == "SS")
                            {
                                filter = "Origin='" + source + "' and Destination='" + dest + "'";
                            }
                            else if (row["Level"].ToString().Trim() == "SC")
                            {
                                filter = "Origin='" + source + "' and Destination='" + countryDest + "'";
                            }
                            else if (row["Level"].ToString().Trim() == "CS")
                            {
                                filter = "Origin='" + countrySource + "' and Destination='" + dest + "'";
                            }
                            else if (row["Level"].ToString().Trim() == "CC")
                            {
                                filter = "Origin='" + countrySource + "' and Destination='" + countryDest + "'";
                            }


                            DataView dv = new DataView(dsAllRateLines.Tables[0]);
                            dv.RowFilter = filter;

                            DataTable dtFilteredRateLines = dv.ToTable();


                            if (dtFilteredRateLines != null && dtFilteredRateLines.Rows.Count != 0)
                            {
                                if (dtFilteredRateLines.Rows.Count == 1)
                                {
                                    // one rateline -> calculate
                                    MatchingRateLine = dtFilteredRateLines.Rows[0]["SerialNumber"].ToString();
                                }
                                else
                                {
                                    // multiple ratelines -> check parameter count and priority
                                    // check for generic rateline first

                                    bool isGeneric = true;

                                    foreach (DataRow rw in dtFilteredRateLines.Rows)
                                    {
                                        string filterdetail = " RateLineSrNo=" + rw["SerialNumber"].ToString();

                                        DataView dvParam = new DataView(dsAllRateLines.Tables[1]);
                                        dvParam.RowFilter = filterdetail;

                                        DataTable dtParams = dvParam.ToTable();


                                        foreach (DataRow rwParam in dtParams.Rows)
                                        {
                                            if (rwParam["ParamValue"].ToString().Trim() != "")
                                            {
                                                isGeneric = false;
                                                break;
                                            }
                                        }

                                        if (isGeneric)
                                        {
                                            GenericRateLine = rw["SerialNumber"].ToString();
                                            break;
                                        }

                                    }


                                    // match rateline parameters count and priority
                                    int matchcount = 0;
                                    int prioritycount = 50;

                                    int currentmatchcount = 0;
                                    int currentprioritycount = 0;


                                    foreach (DataRow rw in dtFilteredRateLines.Rows)
                                    {
                                        string filterdetail = " RateLineSrNo=" + rw["SerialNumber"].ToString();

                                        DataView dvParam = new DataView(dsAllRateLines.Tables[1]);
                                        dvParam.RowFilter = filterdetail;

                                        DataTable dtParams = dvParam.ToTable();
                                        bool excluded = false;
                                        currentmatchcount = 0;
                                        currentprioritycount = 0;

                                        foreach (DataRow rwParam in dtParams.Rows)
                                        {
                                            if (rwParam["IsInclude"].ToString().Trim() == "True" && rwParam["ParamValue"].ToString().Trim() != "")
                                            {
                                                if (rwParam["ParamName"].ToString().Trim() == "FlightNum" && rwParam["ParamValue"].ToString().Trim().Contains(flight) && flight != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "FlightNum");
                                                }
                                                else if (rwParam["ParamName"].ToString().Trim() == "CommCode" && rwParam["ParamValue"].ToString().Trim().Contains(commcode) && commcode != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "CommCode");
                                                }
                                                else if (rwParam["ParamName"].ToString().Trim() == "AgentCode" && rwParam["ParamValue"].ToString().Trim().Contains(agentcode) && agentcode != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "AgentCode");
                                                }
                                                else if (rwParam["ParamName"].ToString().Trim() == "FlightCarrier" && rwParam["ParamValue"].ToString().Trim().Contains(carrier) && carrier != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "FlightCarrier");
                                                }
                                                else
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                            }
                                            else if (rwParam["IsInclude"].ToString().Trim() == "False" && rwParam["ParamValue"].ToString().Trim() != "")
                                            {
                                                if (rwParam["ParamName"].ToString().Trim() == "FlightNum" && rwParam["ParamValue"].ToString().Trim().Contains(flight) && flight != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                if (rwParam["ParamName"].ToString().Trim() == "CommCode" && rwParam["ParamValue"].ToString().Trim().Contains(commcode) && commcode != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                if (rwParam["ParamName"].ToString().Trim() == "AgentCode" && rwParam["ParamValue"].ToString().Trim().Contains(agentcode) && agentcode != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                if (rwParam["ParamName"].ToString().Trim() == "FlightCarrier" && rwParam["ParamValue"].ToString().Trim().Contains(carrier) && carrier != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], rwParam["ParamName"].ToString().Trim());
                                                }
                                            }
                                        }

                                        if (!excluded)
                                        {
                                            if (matchcount == 0 && currentmatchcount == 0)
                                            {
                                                matchcount = 0;
                                                prioritycount = 50;
                                                MatchingRateLine = rw["SerialNumber"].ToString();
                                            }
                                            else if (matchcount < currentmatchcount)
                                            {
                                                matchcount = currentmatchcount;
                                                prioritycount = currentprioritycount;
                                                MatchingRateLine = rw["SerialNumber"].ToString();
                                            }
                                            else if (matchcount == currentmatchcount && prioritycount > currentprioritycount)
                                            {
                                                prioritycount = currentprioritycount;
                                                MatchingRateLine = rw["SerialNumber"].ToString();
                                            }
                                        }

                                    }


                                }
                            }


                            if (MatchingRateLine != "" || GenericRateLine != "")
                            {
                                SetDetailsOfMatchingRateLine(ref dsResult, wt, MatchingRateLine.Trim() == "" ? GenericRateLine : MatchingRateLine,OrgStation);
                                return true;
                            }

                        }

                    }

                }
                else
                    return false;

                return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }

        public bool GetAllRateLinesFor(string source, string dest, string mode, ref DataSet dsResult, ref string errormessage,DateTime dtFlt)
        {
            try
            {
                errormessage = "";

                string[] param = { "Source", "Dest", "Mode","ExecutionDt" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Date };
                object[] values = { source, dest, mode, dtFlt };


                SQLServer da = new SQLServer(constr);
                dsResult = da.SelectRecords("SP_GetAllRateLinesFor", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error : Code II(GetAllRateLinesFor)";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : Code I(GetAllRateLinesFor)";
                    return false;
                }



            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }

        public bool GetPriority(ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";


                SQLServer da = new SQLServer(constr);
                dsResult = da.SelectRecords("SP_GetLevelPriorities");

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error : Code II(GetPriority)";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : Code I(GetPriority)";
                    return false;
                }



            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }

        public int GetPriorityOfParameter(DataTable dt, string param)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["ParameterName"].ToString().Trim() == param)
                {
                    return int.Parse(row["ParameterPriority"].ToString().Trim());
                }
            }

            return 20;
        }

        public void SetDetailsOfMatchingRateLine(ref DataSet dsResult, decimal wt, string SerialNumber,string Origin)
        {
            string[] param = { "SerialNumber", "Wt", "Origin" };
            SqlDbType[] sqldbtype = { SqlDbType.Int, SqlDbType.Float, SqlDbType.VarChar };
            object[] values = { int.Parse(SerialNumber), wt,Origin };

            SQLServer da = new SQLServer(constr);
            dsResult = da.SelectRecords("SP_CalculateRateForRateLine", param, values, sqldbtype);

        }

        private DataSet GetOtherCharges(string Origin, string Destination, string FlightNo, string Commodity, string AgentCode, string Carrier, decimal Weight, decimal OtherCharges)
        {
            DataSet dsResult = null;
            string errormessage = string.Empty;

            if (GetOtherChargesFor(Origin, Destination, FlightNo, AgentCode, Commodity, Carrier, Weight, ref dsResult, ref errormessage,OtherCharges))
            {

            }

            return dsResult;
        }

        public bool GetOtherChargesFor(string source, string dest, string flight, string agentcode, string commcode, string carrier, decimal wt, ref DataSet dsResult, ref string errormessage, decimal IATAcharge)
        {
            try
            {

                errormessage = "";

                DataSet dsAllOtherCharges = new DataSet();
                DataSet dsPriorities = new DataSet();

                if (GetAllOtherChargesFor(source, dest, ref dsAllOtherCharges, ref errormessage))
                {
                    // dsAllRateLines contains all rate lines
                    string regionSource, regionDest, countrySource, countryDest;
                    regionSource = regionDest = countrySource = countryDest = "";

                    regionSource = dsAllOtherCharges.Tables[3].Rows[0]["RegionSource"].ToString();
                    regionDest = dsAllOtherCharges.Tables[3].Rows[0]["RegionDest"].ToString();
                    countrySource = dsAllOtherCharges.Tables[3].Rows[0]["CountrySource"].ToString();
                    countryDest = dsAllOtherCharges.Tables[3].Rows[0]["CountryDest"].ToString();

                    foreach (DataRow drOtherCharges in dsAllOtherCharges.Tables[0].Rows)
                    {
                        string chargefilter = "ChargeHeadCode='" + drOtherCharges["ChargeHeadCode"].ToString() + "'";

                        DataView dv = new DataView(dsAllOtherCharges.Tables[1]);
                        dv.RowFilter = chargefilter;

                        DataTable dtFiltered = dv.ToTable();


                        // multiple entries for same charge
                        string GenericOtherCharge = "";
                        string MatchingOtherCharge = "";


                        if (GetPriority(ref dsPriorities, ref errormessage))
                        {
                            foreach (DataRow row in dsPriorities.Tables[0].Rows)
                            {
                                string filter = "";
                                if (row["Level"].ToString().Trim() == "SS")
                                {
                                    filter = "Origin='" + source + "' and Destination='" + dest + "'";
                                }
                                else if (row["Level"].ToString().Trim() == "SC")
                                {
                                    filter = "Origin='" + source + "' and Destination='" + countryDest + "'";
                                }
                                else if (row["Level"].ToString().Trim() == "CS")
                                {
                                    filter = "Origin='" + countrySource + "' and Destination='" + dest + "'";
                                }
                                else if (row["Level"].ToString().Trim() == "CC")
                                {
                                    filter = "Origin='" + countrySource + "' and Destination='" + countryDest + "'";
                                }


                                DataView dvInner = new DataView(dtFiltered);
                                dvInner.RowFilter = filter;

                                DataTable dtFilteredOtherCharge = dv.ToTable();


                                if (dtFilteredOtherCharge != null && dtFilteredOtherCharge.Rows.Count != 0)
                                {
                                    if (dtFilteredOtherCharge.Rows.Count == 1)
                                    {
                                        // one rateline -> calculate
                                        MatchingOtherCharge = dtFilteredOtherCharge.Rows[0]["SerialNumber"].ToString();
                                        break;
                                    }
                                    else
                                    {
                                        // multiple entries check paramaters count/priority
                                        // check for generic rateline first

                                        bool isGeneric = true;

                                        foreach (DataRow rw in dtFilteredOtherCharge.Rows)
                                        {
                                            string filterdetail = " ChargeHeadSrNo=" + rw["SerialNumber"].ToString();

                                            DataView dvParam = new DataView(dsAllOtherCharges.Tables[2]);
                                            dvParam.RowFilter = filterdetail;

                                            DataTable dtParams = dvParam.ToTable();


                                            foreach (DataRow rwParam in dtParams.Rows)
                                            {
                                                if (rwParam["ParamValue"].ToString().Trim() != "")
                                                {
                                                    isGeneric = false;
                                                    break;
                                                }
                                            }

                                            if (isGeneric)
                                            {
                                                GenericOtherCharge = rw["SerialNumber"].ToString();
                                                break;
                                            }

                                        }


                                        // match rateline parameters count and priority
                                        int matchcount = 0;
                                        int prioritycount = 50;

                                        int currentmatchcount = 0;
                                        int currentprioritycount = 0;


                                        foreach (DataRow rw in dtFilteredOtherCharge.Rows)
                                        {
                                            string filterdetail = " ChargeHeadSrNo=" + rw["SerialNumber"].ToString();

                                            DataView dvParam = new DataView(dsAllOtherCharges.Tables[2]);
                                            dvParam.RowFilter = filterdetail;

                                            DataTable dtParams = dvParam.ToTable();
                                            bool excluded = false;
                                            currentmatchcount = 0;
                                            currentprioritycount = 0;

                                            foreach (DataRow rwParam in dtParams.Rows)
                                            {
                                                if (rwParam["IsInclude"].ToString().Trim() == "True" && rwParam["ParamValue"].ToString().Trim() != "")
                                                {
                                                    if (rwParam["ParamName"].ToString().Trim() == "FlightNum" && rwParam["ParamValue"].ToString().Trim().Contains(flight) && flight != "")
                                                    {
                                                        currentmatchcount++;
                                                        currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "FlightNum");
                                                    }
                                                    else if (rwParam["ParamName"].ToString().Trim() == "CommCode" && rwParam["ParamValue"].ToString().Trim().Contains(commcode) && commcode != "")
                                                    {
                                                        currentmatchcount++;
                                                        currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "CommCode");
                                                    }
                                                    else if (rwParam["ParamName"].ToString().Trim() == "AgentCode" && rwParam["ParamValue"].ToString().Trim().Contains(agentcode) && agentcode != "")
                                                    {
                                                        currentmatchcount++;
                                                        currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "AgentCode");
                                                    }
                                                    else if (rwParam["ParamName"].ToString().Trim() == "FlightCarrier" && rwParam["ParamValue"].ToString().Trim().Contains(carrier) && carrier != "")
                                                    {
                                                        currentmatchcount++;
                                                        currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "FlightCarrier");
                                                    }
                                                    else
                                                    {
                                                        excluded = true;
                                                        break;
                                                    }
                                                }
                                                else if (rwParam["IsInclude"].ToString().Trim() == "False" && rwParam["ParamValue"].ToString().Trim() != "")
                                                {
                                                    if (rwParam["ParamName"].ToString().Trim() == "FlightNum" && rwParam["ParamValue"].ToString().Trim().Contains(flight) && flight != "")
                                                    {
                                                        excluded = true;
                                                        break;
                                                    }
                                                    if (rwParam["ParamName"].ToString().Trim() == "CommCode" && rwParam["ParamValue"].ToString().Trim().Contains(commcode) && commcode != "")
                                                    {
                                                        excluded = true;
                                                        break;
                                                    }
                                                    if (rwParam["ParamName"].ToString().Trim() == "AgentCode" && rwParam["ParamValue"].ToString().Trim().Contains(agentcode) && agentcode != "")
                                                    {
                                                        excluded = true;
                                                        break;
                                                    }
                                                    if (rwParam["ParamName"].ToString().Trim() == "FlightCarrier" && rwParam["ParamValue"].ToString().Trim().Contains(carrier) && carrier != "")
                                                    {
                                                        excluded = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        currentmatchcount++;
                                                        currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], rwParam["ParamName"].ToString().Trim());
                                                    }
                                                }
                                            }

                                            if (!excluded)
                                            {
                                                if (matchcount == 0 && currentmatchcount == 0)
                                                {
                                                    matchcount = 0;
                                                    prioritycount = 50;
                                                    MatchingOtherCharge = rw["SerialNumber"].ToString();
                                                }
                                                else if (matchcount < currentmatchcount)
                                                {
                                                    matchcount = currentmatchcount;
                                                    prioritycount = currentprioritycount;
                                                    MatchingOtherCharge = rw["SerialNumber"].ToString();
                                                }
                                                else if (matchcount == currentmatchcount && prioritycount > currentprioritycount)
                                                {
                                                    prioritycount = currentprioritycount;
                                                    MatchingOtherCharge = rw["SerialNumber"].ToString();
                                                }
                                            }

                                        }




                                        //-----------------------

                                    }
                                }
                            }
                        }




                        //-----------

                        if (MatchingOtherCharge != "" || GenericOtherCharge != "")
                        {
                            SetDetailsOfMatchingOtherCharge(ref dsResult, wt, wt, commcode, agentcode, MatchingOtherCharge.Trim() == "" ? GenericOtherCharge : MatchingOtherCharge, IATAcharge, OrgStation);
                            //SetDetailsOfMatchingOtherCharge(ref dsResult, wt, wt, commcode, agentcode, GenericOtherCharge.Trim() == "" ? MatchingOtherCharge : GenericOtherCharge);
                        }

                        //----------

                    }

                }
                else
                {
                    //lblStatus.Text = "Error : while fetching other charges details for (" + source + "-" + dest + ")";
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                //lblStatus.Text = "" + ex.Message;
                return false;
            }
        }

        public void SetDetailsOfMatchingOtherCharge(ref DataSet dsResult, decimal wt, decimal chwt, string commcode, string agentcode, string SerailNumber,decimal dcFrtAmount,string Origin)
        {

            string[] param = { "AgentCode", "GrossWt", "ChargedWt", "HAWB", "SerialNumber", "dcFrtAmount", "Origin" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.Int, SqlDbType.Int, SqlDbType.Decimal, SqlDbType.VarChar };
            object[] values = { agentcode, wt, chwt, 0, int.Parse(SerailNumber), dcFrtAmount, Origin };

            SQLServer da = new SQLServer(constr);
            dsResult = da.SelectRecords("SP_CalculateOCFor", param, values, sqldbtype);

            // ADD OC details

            if (dsOtherCharges == null)
                PrepareOtherCharges();

            //DataSet dsDetails = ((DataSet)Session["OCDetails"]);
            DataRow dsDetailsRow = dsOtherCharges.Tables[0].NewRow();

            dsDetailsRow["Commodity Code"] = commcode;
            dsDetailsRow["Charge Head Code"] = dsResult.Tables[0].Rows[0]["ChargeHeadCode"].ToString().Trim();
            dsDetailsRow["Charge Type"] = dsResult.Tables[0].Rows[0]["ChargeType"].ToString().Trim();  // "RateLineMKT"
            dsDetailsRow["Charge"] = dsResult.Tables[0].Rows[0]["TotalCharge"].ToString(); //IATACharge;
            dsDetailsRow["TaxPercent"] = dsResult.Tables[0].Rows[0]["ServiceTax"].ToString(); //TaxPercentIATA;
            dsDetailsRow["Tax"] = dsResult.Tables[0].Rows[0]["Tax"].ToString();//ServiceTaxIATA;
            dsDetailsRow["DiscountPercent"] = dsResult.Tables[0].Rows[0]["DiscountPercent"].ToString();//DiscountPercentIATA;
            dsDetailsRow["Discount"] = dsResult.Tables[0].Rows[0]["Discount"].ToString();//DiscountIATA;
            dsDetailsRow["CommPercent"] = dsResult.Tables[0].Rows[0]["CommPercent"].ToString();//CommissionPercentIATA;
            dsDetailsRow["Commission"] = dsResult.Tables[0].Rows[0]["Commission"].ToString();//CommissionIATA;

            dsOtherCharges.Tables[0].Rows.Add(dsDetailsRow);
        }

        public bool GetAllOtherChargesFor(string source, string dest, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";

                string[] param = { "Source", "Dest" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { source, dest };


                SQLServer da = new SQLServer(constr);
                dsResult = da.SelectRecords("SP_GetAllOtherChargesFor", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error : Code II(GetAllOtherChargesFor)";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : Code I(GetAllOtherChargesFor)";
                    return false;
                }



            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }

        private DataSet PrepareOtherCharges()
        {
            dsOtherCharges = new DataSet();
            dsOtherCharges.Tables.Add();
            dsOtherCharges.Tables[0].TableName = "OCDA";
            dsOtherCharges.Tables[0].Columns.Add("Commodity Code");
            dsOtherCharges.Tables[0].Columns.Add("Charge Head Code");
            dsOtherCharges.Tables[0].Columns.Add("Charge Type");
            dsOtherCharges.Tables[0].Columns.Add("Charge");
            dsOtherCharges.Tables[0].Columns.Add("TaxPercent");
            dsOtherCharges.Tables[0].Columns.Add("Tax");
            dsOtherCharges.Tables[0].Columns.Add("DiscountPercent");
            dsOtherCharges.Tables[0].Columns.Add("Discount");
            dsOtherCharges.Tables[0].Columns.Add("CommPercent");
            dsOtherCharges.Tables[0].Columns.Add("Commission");

            return dsOtherCharges;
        }

        public void SetOtherChargesSummary(ref float OCDC, ref float OCDA, ref float OCTax)
        {
            DataSet dsDetails = dsOtherCharges;
            DataSet dsDetailsFinal = dsDetails.Clone();

            System.Collections.ArrayList list = new System.Collections.ArrayList();

            foreach (DataRow row in dsDetails.Tables[0].Rows)
            {
                if (row["Charge Type"].ToString() == "DC" || row["Charge Type"].ToString() == "DA")
                {
                    if (!list.Contains(row["Charge Head Code"].ToString()))
                    {

                        DataRow newrow = dsDetailsFinal.Tables[0].NewRow();
                        for (int i = 0; i < dsDetailsFinal.Tables[0].Columns.Count; i++)
                        {
                            newrow[i] = row[i];
                        }

                        dsDetailsFinal.Tables[0].Rows.Add(newrow);
                        list.Add(row["Charge Head Code"].ToString());
                    }
                    else
                    {
                        foreach (DataRow rw in dsDetailsFinal.Tables[0].Rows)
                        {

                            if (rw["Charge Head Code"].ToString() == row["Charge Head Code"].ToString())
                            {
                                rw["Charge"] = "" + (float.Parse(rw["Charge"].ToString()) + float.Parse(rw["Charge"].ToString()));
                                rw["Tax"] = "" + (float.Parse(rw["Tax"].ToString()) + float.Parse(rw["Tax"].ToString()));
                                rw["Discount"] = "" + (float.Parse(rw["Discount"].ToString()) + float.Parse(rw["Discount"].ToString()));
                                rw["Commission"] = "" + (float.Parse(rw["Commission"].ToString()) + float.Parse(rw["Commission"].ToString()));

                            }
                        }

                    }

                }
                else
                {
                    DataRow newrow = dsDetailsFinal.Tables[0].NewRow();
                    for (int i = 0; i < dsDetailsFinal.Tables[0].Columns.Count; i++)
                    {
                        newrow[i] = row[i];
                    }

                    dsDetailsFinal.Tables[0].Rows.Add(newrow);
                }

            }


            foreach (DataRow dr in dsDetailsFinal.Tables[0].Rows)
            {
                if (dr["Charge Type"].ToString() == "DC")
                {
                    OCDC += float.Parse(dr["Charge"].ToString());
                    OCTax += float.Parse(dr["Tax"].ToString());

                }
                else if (dr["Charge Type"].ToString() == "DA")
                {
                    OCDA += float.Parse(dr["Charge"].ToString());
                    OCTax += float.Parse(dr["Tax"].ToString());
                }

            }

        }

        public bool GetRateLineForSpecial(string source, string dest, string mode, string flight, string agentcode, string commcode, string carrier, decimal wt, ref DataSet dsResult, ref string errormessage,DateTime dtFltdate)
        {
            try
            {
                errormessage = "";

                source = source.Trim();
                dest = dest.Trim();
                flight = flight.Trim();
                agentcode = agentcode.Trim();
                commcode = commcode.Trim();
                carrier = carrier.Trim();

                DataSet dsAllRateLines = new DataSet();
                DataSet dsPriorities = new DataSet();

                if (GetAllRateLinesFor(source, dest, mode, ref dsAllRateLines, ref errormessage, dtFltdate))
                {
                    // dsAllRateLines contains all rate lines
                    string regionSource, regionDest, countrySource, countryDest;
                    regionSource = regionDest = countrySource = countryDest = "";

                    regionSource = dsAllRateLines.Tables[3].Rows[0]["RegionSource"].ToString();
                    regionDest = dsAllRateLines.Tables[3].Rows[0]["RegionDest"].ToString();
                    countrySource = dsAllRateLines.Tables[3].Rows[0]["CountrySource"].ToString();
                    countryDest = dsAllRateLines.Tables[3].Rows[0]["CountryDest"].ToString();

                    if (GetPriority(ref dsPriorities, ref errormessage))
                    {
                        string GenericRateLine = "";
                        string MatchingRateLine = "";
                        foreach (DataRow row in dsPriorities.Tables[0].Rows)
                        {
                            string filter = "";
                            if (row["Level"].ToString().Trim() == "SS")
                            {
                                filter = "Origin='" + source + "' and Destination='" + dest + "'";
                            }
                            else if (row["Level"].ToString().Trim() == "SC")
                            {
                                filter = "Origin='" + source + "' and Destination='" + countryDest + "'";
                            }
                            else if (row["Level"].ToString().Trim() == "CS")
                            {
                                filter = "Origin='" + countrySource + "' and Destination='" + dest + "'";
                            }
                            else if (row["Level"].ToString().Trim() == "CC")
                            {
                                filter = "Origin='" + countrySource + "' and Destination='" + countryDest + "'";
                            }


                            DataView dv = new DataView(dsAllRateLines.Tables[0]);
                            dv.RowFilter = filter;

                            DataTable dtFilteredRateLines = dv.ToTable();


                            if (dtFilteredRateLines != null && dtFilteredRateLines.Rows.Count != 0)
                            {
                                if (dtFilteredRateLines.Rows.Count == 1)
                                {
                                    // one rateline -> calculate
                                    MatchingRateLine = dtFilteredRateLines.Rows[0]["SerialNumber"].ToString();
                                }
                                else
                                {
                                    // multiple ratelines -> check parameter count and priority
                                    // check for generic rateline first

                                    bool isGeneric = true;

                                    foreach (DataRow rw in dtFilteredRateLines.Rows)
                                    {
                                        string filterdetail = " RateLineSrNo=" + rw["SerialNumber"].ToString();

                                        DataView dvParam = new DataView(dsAllRateLines.Tables[1]);
                                        dvParam.RowFilter = filterdetail;

                                        DataTable dtParams = dvParam.ToTable();


                                        foreach (DataRow rwParam in dtParams.Rows)
                                        {
                                            if (rwParam["ParamValue"].ToString().Trim() != "")
                                            {
                                                isGeneric = false;
                                                break;
                                            }
                                        }

                                        if (isGeneric)
                                        {
                                            GenericRateLine = rw["SerialNumber"].ToString();
                                            break;
                                        }

                                    }

                                    isGeneric = true;

                                    foreach (DataRow rw in dtFilteredRateLines.Rows)
                                    {
                                        string filterdetail = " RateLineSrNo=" + rw["SerialNumber"].ToString();

                                        DataView dvParam = new DataView(dsAllRateLines.Tables[1]);
                                        dvParam.RowFilter = filterdetail;

                                        DataTable dtParams = dvParam.ToTable();


                                        foreach (DataRow rwParam in dtParams.Rows)
                                        {
                                            if (rwParam["ParamValue"].ToString().Trim() != "")
                                            {
                                                isGeneric = false;
                                                break;
                                            }
                                        }

                                        if (isGeneric)
                                        {
                                            GenericRateLine = rw["SerialNumber"].ToString();
                                            break;
                                        }

                                    }


                                    // match rateline parameters count and priority
                                    int matchcount = 0;
                                    int prioritycount = 50;

                                    int currentmatchcount = 0;
                                    int currentprioritycount = 0;


                                    foreach (DataRow rw in dtFilteredRateLines.Rows)
                                    {
                                        string filterdetail = " RateLineSrNo=" + rw["SerialNumber"].ToString();

                                        DataView dvParam = new DataView(dsAllRateLines.Tables[1]);
                                        dvParam.RowFilter = filterdetail;

                                        DataTable dtParams = dvParam.ToTable();
                                        bool excluded = false;
                                        currentmatchcount = 0;
                                        currentprioritycount = 0;

                                        foreach (DataRow rwParam in dtParams.Rows)
                                        {
                                            if (rwParam["IsInclude"].ToString().Trim() == "True" && rwParam["ParamValue"].ToString().Trim() != "")
                                            {
                                                if (rwParam["ParamName"].ToString().Trim() == "FlightNum" && rwParam["ParamValue"].ToString().Trim().Contains(flight) && flight != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "FlightNum");
                                                }
                                                else if (rwParam["ParamName"].ToString().Trim() == "CommCode" && rwParam["ParamValue"].ToString().Trim().Contains(commcode) && commcode != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "CommCode");
                                                }
                                                else if (rwParam["ParamName"].ToString().Trim() == "AgentCode" && rwParam["ParamValue"].ToString().Trim().Contains(agentcode) && agentcode != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "AgentCode");
                                                }
                                                else if (rwParam["ParamName"].ToString().Trim() == "FlightCarrier" && rwParam["ParamValue"].ToString().Trim().Contains(carrier) && carrier != "")
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], "FlightCarrier");
                                                }
                                                else
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                            }
                                            else if (rwParam["IsInclude"].ToString().Trim() == "False" && rwParam["ParamValue"].ToString().Trim() != "")
                                            {
                                                if (rwParam["ParamName"].ToString().Trim() == "FlightNum" && rwParam["ParamValue"].ToString().Trim().Contains(flight) && flight != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                if (rwParam["ParamName"].ToString().Trim() == "CommCode" && rwParam["ParamValue"].ToString().Trim().Contains(commcode) && commcode != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                if (rwParam["ParamName"].ToString().Trim() == "AgentCode" && rwParam["ParamValue"].ToString().Trim().Contains(agentcode) && agentcode != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                if (rwParam["ParamName"].ToString().Trim() == "FlightCarrier" && rwParam["ParamValue"].ToString().Trim().Contains(carrier) && carrier != "")
                                                {
                                                    excluded = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    currentmatchcount++;
                                                    currentprioritycount += GetPriorityOfParameter(dsPriorities.Tables[1], rwParam["ParamName"].ToString().Trim());
                                                }
                                            }
                                        }

                                        if (!excluded)
                                        {
                                            if (matchcount == 0 && currentmatchcount == 0)
                                            {
                                                matchcount = 0;
                                                prioritycount = 50;
                                                MatchingRateLine = rw["SerialNumber"].ToString();
                                            }
                                            else if (matchcount < currentmatchcount)
                                            {
                                                matchcount = currentmatchcount;
                                                prioritycount = currentprioritycount;
                                                MatchingRateLine = rw["SerialNumber"].ToString();
                                            }
                                            else if (matchcount == currentmatchcount && prioritycount > currentprioritycount)
                                            {
                                                prioritycount = currentprioritycount;
                                                MatchingRateLine = rw["SerialNumber"].ToString();
                                            }
                                        }

                                    }


                                }
                            }

                            //Added by poorna on 10-Aug-2012
                            if (MatchingRateLine != "")
                            {
                                GenericRateLine = MatchingRateLine;
                            }
                            //End

                            if (MatchingRateLine != "" && RateLineContainsCommodity(dsAllRateLines, MatchingRateLine, commcode))
                            {
                                SetDetailsOfMatchingRateLine(ref dsResult, wt, MatchingRateLine.Trim(), source);
                                return true;
                            }
                            else if (GenericRateLine != "")
                            {
                                // formula 
                                CalculateRateForSpecialCommV2(commcode, float.Parse("" + wt), int.Parse(GenericRateLine), ref dsResult, ref errormessage);
                                return true;
                            }
                        }
                    }
                }
                else
                    return false;

                return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }

        public bool RateLineContainsCommodity(DataSet dsAllRateLines, string ratelineid, string commcode)
        {
            string filterdetail = " RateLineSrNo=" + ratelineid;

            DataView dvParam = new DataView(dsAllRateLines.Tables[1]);
            dvParam.RowFilter = filterdetail;

            DataTable dtParams = dvParam.ToTable();

            foreach (DataRow rwParam in dtParams.Rows)
            {
                if (rwParam["ParamName"].ToString().Trim() == "CommCode")
                {
                    if (rwParam["ParamValue"].ToString().Trim().Contains(commcode) && commcode != "")
                        return true;
                    else
                        return false;

                }

            }

            return false;
        }

        private bool GetChargeRateForSpecial(string Origin, string Destination, string FlightNo, string Commodity, string AgentCode, string Carrier, string RateType, decimal Weight, ref decimal Charge, ref decimal Tax, DateTime fltDate)
        {
            DataSet dsResult = null;
            string errormessage = string.Empty;
            decimal IATAcharge = 0;
            decimal IATAtax = 0;

            if (GetRateLineForSpecial(Origin, Destination, RateType, FlightNo, AgentCode, Commodity, Carrier, Weight, ref dsResult, ref errormessage, fltDate))
            {
                IATAcharge = decimal.Parse(dsResult.Tables[0].Rows[0]["Charge"].ToString());
                IATAtax = decimal.Parse(dsResult.Tables[0].Rows[0]["ServiceTax"].ToString());            
            }

            Charge = IATAcharge;
            Tax = IATAtax;
            return true;
        }

        #endregion "Calculate AWB Charges"

    }
