using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;


namespace BAL
{
    

    public class GHA_BALFlightCapacity
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        public bool GetAllAWBs(string source, string destination, string flight, string fromdate, string todate, string prefix, string status, string awbnumber, ref DataSet dsResult, ref string errormessage, string AgentCode)
        {
            try
            {
                errormessage = "";
                string[] param = { "Source", "Dest", "Flight", "fromdate", "todate", "Prefix", "Status", "AWBNumber", "AgentCode" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { source, destination, flight, fromdate, todate, prefix, status, awbnumber, AgentCode };

                dsResult = da.SelectRecords("spgetAllMainDataforCapacityPlanning_DY", param, values, sqldbtype);

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

        public bool ValidateLoadableWeight(string Station, string FlightNo, DateTime FlightDt, string Dimensions)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            string[] param = { "Station", "FlightNo", "FlightDt", "Dimensions" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.NText };
            object[] values = { Station, FlightNo, FlightDt, Dimensions };

            try
            {
                dsResult = da.SelectRecords("sp_ValidateLoadablePieces_New", param, values, sqldbtype);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    blnResult = Convert.ToBoolean(dsResult.Tables[0].Rows[0][0]);
                }
                else
                {
                    blnResult = false;
                }

                return blnResult;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                param = null;
                sqldbtype = null;
                values = null;
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        public bool ValidateFlightCapacity(string Origin, string Destination, string FlightNo, DateTime FlightDt, decimal AWBWeight)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            string[] param = { "FlightNo", "FlightDt", "Origin", "Destination", "AWBWeight" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal };
            object[] values = { FlightNo, FlightDt, Origin,Destination,AWBWeight };

            try
            {
                dsResult = da.SelectRecords("sp_ValidateFlightCapacity", param, values, sqldbtype);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    blnResult = Convert.ToBoolean(dsResult.Tables[0].Rows[0][0]);
                }
                else
                {
                    blnResult = false;
                }

                return blnResult;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                param = null;
                sqldbtype = null;
                values = null;
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }
    }
}
