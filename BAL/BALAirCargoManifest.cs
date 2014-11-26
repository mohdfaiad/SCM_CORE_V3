using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class BALAirCargoManifest
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Retrieving Records as per selection
        public DataSet GetAirCargoManifest(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];

                QueryNames[0] = "FltNo";
                QueryNames[1] = "FltDt";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.DateTime;

                DataSet ds = db.SelectRecords("spGetAirCargoManifest", QueryNames, ParamValues, QueryTypes);
                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Save/Update Air Cargo Manifest
        public bool SaveAirCargoManifest(object[] ParamValues)
        {
            try
            {
                
                string[] QueryNames = new string[8];
                SqlDbType[] QueryTypes = new SqlDbType[8];

                QueryNames[0] = "FltNo";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "FltDt";
                QueryTypes[1] = SqlDbType.DateTime;

                QueryNames[2] = "Operator";
                QueryTypes[2] = SqlDbType.VarChar;

                QueryNames[3] = "Nationality";
                QueryTypes[3] = SqlDbType.VarChar;

                QueryNames[4] = "LadingPort";
                QueryTypes[4] = SqlDbType.VarChar;

                QueryNames[5] = "UnLadingPort";
                QueryTypes[5] = SqlDbType.VarChar;

                QueryNames[6] = "Consolidator";
                QueryTypes[6] = SqlDbType.VarChar;

                QueryNames[7] = "DeConsolidator";
                QueryTypes[7] = SqlDbType.VarChar;


                bool res = db.InsertData("spAddAirCargoManifest", QueryNames, QueryTypes, ParamValues);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        # region Get Airport Codes
        public DataSet GetAirportCodes()
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
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
                SQLServer da = new SQLServer(Global.GetConnectionString());
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
    }

}
