using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class BALBreakULD
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion

        #region Get ULD List
        public DataSet GetULDList(object[] QueryValues)
        {
            string[] QueryNames = new string[5];
            SqlDbType[] QueryTypes = new SqlDbType[5];
            DataSet ds = null;
            try
            {

                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNumber";
                QueryNames[2] = "FlightDate";
                QueryNames[3] = "ULDNumber";
                QueryNames[4] = "FlightDestination";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                ds = db.SelectRecords("sp_GetULDBreakList", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            finally
            {
                QueryNames = null;
                QueryTypes = null;
                QueryValues = null;
            }
            return null;
        }
        #endregion

        #region Update Breakup ULD Details
        public bool UpdateULDBreakupDetails(object[] QueryValues)
        {
            string[] QueryNames = new string[11];
            SqlDbType[] QueryTypes = new SqlDbType[11];
            try
            {
                QueryNames[0] = "ULDNo";
                QueryNames[1] = "AWBNumber";
                QueryNames[2] = "Pieces";
                QueryNames[3] = "GrossWeight";
                QueryNames[4] = "IsBreak";
                QueryNames[5] = "ULDBreakId";
                QueryNames[6] = "UpdatedOn";
                QueryNames[7] = "UpdatedBy";
                QueryNames[8] = "Location";
                QueryNames[9] = "FlightNo";
                QueryNames[10] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.Int;
                QueryTypes[3] = SqlDbType.Decimal;
                QueryTypes[4] = SqlDbType.Bit;
                QueryTypes[5] = SqlDbType.BigInt;
                QueryTypes[6] = SqlDbType.DateTime;
                QueryTypes[7] = SqlDbType.VarChar;
                QueryTypes[8] = SqlDbType.VarChar;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;

                if (db.UpdateData("sp_UpdateULDBreakUpInfo_V1", QueryNames, QueryTypes, QueryValues))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                QueryValues = null;
                QueryTypes = null;
                QueryNames = null;
            }
        }
        #endregion


    }
}
