using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;


namespace BAL
{
    

    public class BALFlightCapacity
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        public bool GetAWBLevelData(string FlightNo, string FlightDt,  string d1, string d360, string flig, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";
                string[] param = { "FlightNo", "FlightDt",  "d1", "d360", "flig" };
                SqlDbType[] sqldbtype = { SqlDbType.NVarChar,  SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { FlightNo, FlightDt,  d1, d360, flig };
                dsResult = da.SelectRecords("spgetInternalDataofCapacityTest", param, values, sqldbtype);

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
        public bool GetAllAWBs(string source, string destination, string flight, string fromdate, string todate, 
            string prefix, string status, string awbnumber, ref DataSet dsResult, ref string errormessage, 
            string AgentCode,bool ShowNilFlt)
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
                string[] param = { "Source", "Dest", "Flight", "fromdate", "todate", "Prefix", "Status", "AWBNumber", 
                                     "AgentCode","ShowNilFlt" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, 
                                            SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                            SqlDbType.VarChar,SqlDbType.Bit };
                object[] values = { source, destination, flight, fromdt, todt, prefix, status, awbnumber, AgentCode, ShowNilFlt };

                dsResult = da.SelectRecords("spgetAllMainDataforCapacityPlanning", param, values, sqldbtype);

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
        public bool GetAWBLevelDataRev(string FlightNo, string FlightDt, string d1, string d360, string flig,string origin,string dest,string agent, ref DataSet dsResult, ref string errormessage,string AWBPrefix,string AWBNo)
        {
            try
            {
                errormessage = "";
                string[] param = { "FlightNo", "FlightDt", "d1", "d360", "flig", "Origin", "Destination", "AgentCode", "AWBPrefix", "AWBNo" };
                SqlDbType[] sqldbtype = { SqlDbType.NVarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { FlightNo, FlightDt, d1, d360, flig, origin, dest, agent, AWBPrefix, AWBNo };
                dsResult = da.SelectRecords("spgetInternalDataofCapacityRev", param, values, sqldbtype);

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

        public bool ConfirmAWBNumber(string AWBNumber, string FlightNo, string FlightDate, string UserName, string TimeStamp)
        {
            try
            {
                string Query = string.Empty;

                if(FlightNo !="")
                    Query = "Update AWBRouteMaster Set Status = 'C', UpdatedBy='" + UserName + "',UpdatedOn='" + TimeStamp + "' where AWBNumber='" + AWBNumber + "' and FltNumber='" + FlightNo + "' and FltDate='" + FlightDate + "'";
                else
                    Query = "Update AWBRouteMaster Set Status = 'C', UpdatedBy='" + UserName + "',UpdatedOn='" + TimeStamp + "' where AWBNumber='" + AWBNumber + "'";

                bool blnResult = da.InsertData(Query);
                return blnResult;
            }
            catch (Exception ex)
            {   
                return false;
            }
        }
        public bool ConfirmAWBNumber(string AWBPrefix,string AWBNumber, string FlightNo, string FlightDate, string UserName, string TimeStamp)
        {
            try
            {
                string Query = string.Empty;
                string[] PName = new string[] 
                {
                    "AWBPrefix",
                    "AWBNumber",
                    "FlightNo",
                    "FlightDate",
                    "UserName",
                    "TimeStamp",
                };
                object[] PValue = new object[] 
                {
                    AWBPrefix,
                    AWBNumber,
                    FlightNo,
                    FlightDate,
                    UserName,
                    TimeStamp
                };
                SqlDbType[] PType = new SqlDbType[] 
                {
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar

                };


                //if (FlightNo != "")
                //    Query = "Update AWBRouteMaster Set Status = 'C', UpdatedBy='" + UserName + "',UpdatedOn='" + TimeStamp + "' where AWBPrefix = '"+AWBPrefix+"' and AWBNumber='" + AWBNumber + "' and FltNumber='" + FlightNo + "' and FltDate='" + FlightDate + "'";
                //else
                //    Query = "Update AWBRouteMaster Set Status = 'C', UpdatedBy='" + UserName + "',UpdatedOn='" + TimeStamp + "' where AWBNumber='" + AWBNumber + "'";

                bool blnResult = da.ExecuteProcedure("spUpdateConfirmFromCapacity", PName, PType, PValue);
                return blnResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveHistoricalCapacity(int RecordId, string Origin, string Destination, string FlightNo, string FlightDate, 
           string Commodity, string Category, string DayOfWeek, string Month, decimal AvailableCapacity, decimal LowerExpCapacity,
            decimal ExpectedCapacity, decimal UpperExpCapacity, bool IsActive, string UserName, DateTime TimeStamp, string ProductType)
        {
            try
            {
                string Query = string.Empty;
                string[] PName = new string[] 
                {
                    "RecordId",
                    "Origin",
                    "Destination",
                    "FlightNo",
                    "FlightDt",
                    "Commodity",
                    "Category",
                    "DayOfWeek",
                    "Month",
                    "AvailableCapacity",
                    "LowerExpCapacity",
                    "ExpextedCapacity",
                    "UpperExpCapacity",
                    "IsActive",
                    "UserName",
                    "TimeStamp",
                    "ProductType"
                };
                object[] PValue = new object[] 
                {
                    RecordId,
                    Origin,
                    Destination,
                    FlightNo,
                    FlightDate,
                    Commodity,
                    Category,
                    DayOfWeek,
                    Month,
                    AvailableCapacity,
                    LowerExpCapacity,
                    ExpectedCapacity,
                    UpperExpCapacity,
                    IsActive,
                    UserName,
                    TimeStamp,
                    ProductType
                };
                SqlDbType[] PType = new SqlDbType[] 
                {
                    SqlDbType.Int,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.Decimal,
                    SqlDbType.Decimal,
                    SqlDbType.Decimal,
                    SqlDbType.Decimal,
                    SqlDbType.Bit,
                    SqlDbType.VarChar,
                    SqlDbType.DateTime,
                    SqlDbType.VarChar
                };

                bool blnResult = da.ExecuteProcedure("sp_SaveHistoricalCapacityPlanning", PName, PType, PValue);
                return blnResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataSet GetHistoricalCapacity(string Origin, string Destination, string FlightNo, string FlightDate,
           string Commodity, string Category, string DayOfWeek, string Month, string ProductType)
        {
            DataSet blnResult = null;
            string[] PName = null;
            object[] PValue = null;
            SqlDbType[] PType = null;

            try
            {
                string Query = string.Empty;
                PName = new string[] 
                {
                    "Origin",
                    "Destination",
                    "FlightNo",
                    "FlightDt",
                    "Commodity",
                    "Category",
                    "DayOfWeek",
                    "Month",
                    "ProductType"
                };
                PValue = new object[] 
                {
                    Origin,
                    Destination,
                    FlightNo,
                    FlightDate,
                    Commodity,
                    Category,
                    DayOfWeek,
                    Month,
                    ProductType
                };
                PType = new SqlDbType[] 
                {
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar,
                    SqlDbType.VarChar
                };

                blnResult = da.SelectRecords("sp_GetHistoricalCapacityPlanning", PName, PValue, PType);
                return blnResult;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                PName = null;
                PValue = null;
                PType = null;
            }
        }
    }
}
