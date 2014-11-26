using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

/*

  2012-07-04 vinayak
  2012-07-06  vinayak Edit/View
*/


namespace BAL
{
    public class ListBookingBAL
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        public bool GetAllStaions(ref DataSet dsResult,ref string errormessage)
        {
            try
            {
                errormessage = "";
                dsResult=da.SelectRecords("SP_GetAllStations");

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            return true;
                        }
                        else
                        {
                            errormessage = "Error :(GetAllStaions) Code III";
                            return false;
                        }
                    }
                    else
                    {
                        errormessage = "Error :(GetAllStaions) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAllStaions) Code I";
                    return false;
                }                

            }catch(Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public bool GetAllFlights(string source,string destination,string date,ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";
                string[] param = { "Origin", "Dest", "Date" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { source, destination, date };

                dsResult = da.SelectRecords("spGetAllFlightListFromOrgToDest", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAllFlights) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAllFlights) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public bool GetAllAWBs(string source, string destination, string flight, string Fltfromdate, string Flttodate, string AWBFromDate, string AWBToDate, 
            string status, string AWBPrefix, string awbnumber, string IsFFRflag, ref DataSet dsResult, ref string errormessage, string AgentCode, 
            string CommodityCode, string ExecutedBy,bool ViaTemplate,string Shipper)
        {
            try
            {
                errormessage = "";
                string[] param = { "Source", "Dest", "Flight", "FltFromDate", "FltToDate", "AWBFromDate", "AWBToDate", "Prefix", "Status", 
                                     "AWBNumber", "IsFFR", "CommodityCode", "ExecutedBy", "AgentCode","ViaTemplate","ShipperCode" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.VarChar};
                object[] values = { source, destination, flight, Fltfromdate, Flttodate, AWBFromDate, AWBToDate, AWBPrefix, status, 
                                      awbnumber, IsFFRflag, CommodityCode, ExecutedBy,AgentCode,ViaTemplate,Shipper };

                dsResult = da.SelectRecords("SP_GetBookingDetails", param, values, sqldbtype);

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
            finally 
            {
            }

        }

        #region GetAllFlightsNew
        public bool GetAllFlightsNew(string source, string destination, string date, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";
                string[] param = { "Origin", "Dest", "Date" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { source, destination, date};

                dsResult = da.SelectRecords("spGetAllFlightListFromOrgToDestNew", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAllFlights) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAllFlights) Code I";
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

        #region GetAllFlightsNew
        public DataSet GetAllFlightsNewForQuotes(string prefix, string source, string dest, string date)
        {
            try
            {
                DataSet dsResult = null; ;
                string[] param = {"Prefix" , "Origin", "Dest", "Date"};
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = {prefix, source, dest, date};

                dsResult = da.SelectRecords("spGetAllFlightListFromOrgToDestNew", param, values, sqldbtype);

                if (dsResult != null)
                {
                    return dsResult;
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
        #endregion


        public bool GetBookingTemplate(string source, string destination, string flight, string Fltfromdate, string Flttodate, string AgentCode, string CommodityCode,string User, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";
                string[] param = { "Source", "Dest", "Flight", "FltFromDate", "FltToDate","CommodityCode", "AgentCode","User" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar};
                object[] values = { source, destination, flight, Fltfromdate, Flttodate,  CommodityCode, AgentCode,User };

                dsResult = da.SelectRecords("SP_GetTemplateDetails", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetBookingTemplate) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetBookingTemplate) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
            }
        }
    }
}
