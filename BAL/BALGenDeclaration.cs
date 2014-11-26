using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class BALGenDeclaration
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Retrieving Records as per selection
        public DataSet GetGenDeclaration(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];

                QueryNames[0] = "FlightNo";
                QueryNames[1] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;

                DataSet ds = db.SelectRecords("spGetCustomsGenDeclData", QueryNames, ParamValues, QueryTypes);
                return ds;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Save/Update Customs Declaration Summary
        public DataSet SaveCustomsDeclaration(object[] ParamValues)
        {
            try
            {
                DataSet ds = new DataSet();
                string[] QueryNames = new string[17];
                SqlDbType[] QueryTypes = new SqlDbType[17];

                QueryNames[0] = "Operator";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "NationalityMarks";
                QueryTypes[1] = SqlDbType.VarChar;

                QueryNames[2] = "FlightNo";
                QueryTypes[2] = SqlDbType.VarChar;

                QueryNames[3] = "FltDate";
                QueryTypes[3] = SqlDbType.VarChar;

                QueryNames[4] = "DepartureFrom";
                QueryTypes[4] = SqlDbType.VarChar;

                QueryNames[5] = "ArrivalAt";
                QueryTypes[5] = SqlDbType.VarChar;

                QueryNames[6] = "DepartureEmbarking";
                QueryTypes[6] = SqlDbType.VarChar;

                QueryNames[7] = "DepartureThrough";
                QueryTypes[7] = SqlDbType.VarChar;

                QueryNames[8] = "ArrivalDisembarking";
                QueryTypes[8] = SqlDbType.VarChar;

                QueryNames[9] = "ArrivalThrough";
                QueryTypes[9] = SqlDbType.VarChar;

                QueryNames[10] = "SEDs";
                QueryTypes[10] = SqlDbType.VarChar;

                QueryNames[11] = "AWBs";
                QueryTypes[11] = SqlDbType.VarChar;

                QueryNames[12] = "DeclarationHealth";
                QueryTypes[12] = SqlDbType.VarChar;

                QueryNames[13] = "OtherCondition";
                QueryTypes[13] = SqlDbType.VarChar;

                QueryNames[14] = "DetailsOfTreatment";
                QueryTypes[14] = SqlDbType.VarChar;

                QueryNames[15] = "UpdatedOn";
                QueryTypes[15] = SqlDbType.DateTime;

                QueryNames[16] = "UpdatedBy";
                QueryTypes[16] = SqlDbType.VarChar;

                ds = db.SelectRecords("spAddCustomsDeclarationSummary", QueryNames, ParamValues, QueryTypes);
                    return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Save/Update Customs Declaration Details..Place and Crew
        public DataSet SaveCustomsPlaceAndCrew(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[3];
                SqlDbType[] QueryTypes = new SqlDbType[3];

                QueryNames[0] = "SummarySrNo";
                QueryTypes[0] = SqlDbType.BigInt;

                QueryNames[1] = "Place";
                QueryTypes[1] = SqlDbType.VarChar;

                QueryNames[2] = "TotalCrew";
                QueryTypes[2] = SqlDbType.VarChar;

                DataSet ds = db.SelectRecords("spAddCustomsDeclarationDetails", QueryNames, ParamValues, QueryTypes);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
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
