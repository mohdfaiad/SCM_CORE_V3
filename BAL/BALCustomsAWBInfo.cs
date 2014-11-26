using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class BALCustomsAWBInfo
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Save/Update Customs Goods List
        public bool SaveCustomsGoodsList(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[31];
                SqlDbType[] QueryTypes = new SqlDbType[31];


                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "HouseAWBNo";
                QueryNames[2] = "CountryCode";
                QueryNames[3] = "Origin";
                QueryNames[4] = "Destination";
                QueryNames[5] = "Customs";
                QueryNames[6] = "Pieces";
                QueryNames[7] = "Weight";
                QueryNames[8] = "Description";
                QueryNames[9] = "Consol";
                QueryNames[10] = "QueryStatus";
                QueryNames[11] = "Shipper";
                QueryNames[12] = "Consignee";
                QueryNames[13] = "CustomsValue";
                QueryNames[14] = "Currency";
                QueryNames[15] = "FDA";
                QueryNames[16] = "FlightNo";
                QueryNames[17] = "FlightDate";
                QueryNames[18] = "Part";
                QueryNames[19] = "FlightPcs";
                QueryNames[20] = "FlightWt";
                QueryNames[21] = "Arrival";
                QueryNames[22] = "Offload";
                QueryNames[23] = "Shed";
                QueryNames[24] = "Agent";
                QueryNames[25] = "TransitControl";
                QueryNames[26] = "OnwardCarrier";
                QueryNames[27] = "Bond";
                QueryNames[28] = "IsDeleted";
                QueryNames[29] = "AmmendedReason";
                QueryNames[30] = "UserName";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.Int;
                QueryTypes[7] = SqlDbType.Decimal;
                QueryTypes[8] = SqlDbType.VarChar;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;
                QueryTypes[11] = SqlDbType.VarChar;
                QueryTypes[12] = SqlDbType.VarChar;
                QueryTypes[13] = SqlDbType.VarChar;
                QueryTypes[14] = SqlDbType.VarChar;
                QueryTypes[15] = SqlDbType.VarChar;
                QueryTypes[16] = SqlDbType.VarChar;
                QueryTypes[17] = SqlDbType.DateTime;
                QueryTypes[18] = SqlDbType.VarChar;
                QueryTypes[19] = SqlDbType.Int;
                QueryTypes[20] = SqlDbType.Decimal;
                QueryTypes[21] = SqlDbType.VarChar;
                QueryTypes[22] = SqlDbType.VarChar;
                QueryTypes[23] = SqlDbType.VarChar;
                QueryTypes[24] = SqlDbType.VarChar;
                QueryTypes[25] = SqlDbType.VarChar;
                QueryTypes[26] = SqlDbType.VarChar;
                QueryTypes[27] = SqlDbType.VarChar;
                QueryTypes[28] = SqlDbType.Bit;
                QueryTypes[29] = SqlDbType.VarChar; ;
                QueryTypes[30] = SqlDbType.VarChar;

                if (db.InsertData("SP_SaveCustomsGoodsList", QueryNames, QueryTypes, ParamValues))
                {
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Retrieving Records as per selection
        public DataSet GetCustomRecords(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];

                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "TableName";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;

                DataSet ds = db.SelectRecords("SP_GetCustomsData", QueryNames, ParamValues, QueryTypes);

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 0)
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                return ds;
                //        }
                //    }
                //}

                
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Save/Update Customs AWB Acceptance
        public bool SaveCustomsAWBAcceptance(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[47];
                SqlDbType[] QueryTypes = new SqlDbType[47];

                QueryNames[0] = "AirwayBillNo";
                QueryNames[1] = "Origin";
                QueryNames[2] = "Destination";
                QueryNames[3] = "FromCarrier";
                QueryNames[4] = "ToCarrier";
                QueryNames[5] = "SurfaceOrigin";
                QueryNames[6] = "SurfaceDest";
                QueryNames[7] = "Description";
                QueryNames[8] = "Product";
                QueryNames[9] = "Priority";
                QueryNames[10] = "Status";
                QueryNames[11] = "Pad";
                QueryNames[12] = "EUCustoms";
                QueryNames[13] = "CustomsStation";
                QueryNames[14] = "AccpPieces";
                QueryNames[15] = "AccpWeight";
                QueryNames[16] = "WeightUnit";
                QueryNames[17] = "AccpVolume";
                QueryNames[18] = "SLULD";
                QueryNames[19] = "IsContour";
                QueryNames[20] = "ContourValue";
                QueryNames[21] = "IsPallets";
                QueryNames[22] = "IsLD2";
                QueryNames[23] = "SLULDVerified";
                QueryNames[24] = "SHC";
                QueryNames[25] = "CrossReference";
                QueryNames[26] = "ManifestGroup";
                QueryNames[27] = "AcceptanceDate";
                QueryNames[28] = "WHLocation";
                QueryNames[29] = "ContainerId";
                QueryNames[30] = "FlightNo";
                QueryNames[31] = "FlightDate";
                QueryNames[32] = "Offload";
                QueryNames[33] = "HandlingRemarks";
                QueryNames[34] = "Labels";
                QueryNames[35] = "IsCarrierHold";
                QueryNames[36] = "IsDocReceived";
                QueryNames[37] = "DropOffDate";
                QueryNames[38] = "DroppedBy";
                QueryNames[39] = "IsBonded";
                QueryNames[40] = "AirrivalPort";
                QueryNames[41] = "DestStation";
                QueryNames[42] = "ImportFlight";
                QueryNames[43] = "ImportFltDate";
                QueryNames[44] = "IsPartArrival";
                QueryNames[45] = "UserName";
                QueryNames[46] = "TimeStamp";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;
                QueryTypes[7] = SqlDbType.VarChar;
                QueryTypes[8] = SqlDbType.VarChar;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;
                QueryTypes[11] = SqlDbType.VarChar;
                QueryTypes[12] = SqlDbType.VarChar;
                QueryTypes[13] = SqlDbType.VarChar;
                QueryTypes[14] = SqlDbType.Int;
                QueryTypes[15] = SqlDbType.Decimal;
                QueryTypes[16] = SqlDbType.VarChar;
                QueryTypes[17] = SqlDbType.Decimal;
                QueryTypes[18] = SqlDbType.Bit;
                QueryTypes[19] = SqlDbType.Bit;
                QueryTypes[20] = SqlDbType.VarChar;
                QueryTypes[21] = SqlDbType.Bit;
                QueryTypes[22] = SqlDbType.Bit;
                QueryTypes[23] = SqlDbType.Bit;
                QueryTypes[24] = SqlDbType.VarChar;
                QueryTypes[25] = SqlDbType.VarChar;
                QueryTypes[26] = SqlDbType.VarChar;
                QueryTypes[27] = SqlDbType.DateTime;
                QueryTypes[28] = SqlDbType.VarChar;
                QueryTypes[29] = SqlDbType.VarChar;
                QueryTypes[30] = SqlDbType.VarChar;
                QueryTypes[31] = SqlDbType.DateTime;
                QueryTypes[32] = SqlDbType.VarChar;
                QueryTypes[33] = SqlDbType.VarChar;
                QueryTypes[34] = SqlDbType.VarChar;
                QueryTypes[35] = SqlDbType.Bit;
                QueryTypes[36] = SqlDbType.Bit;
                QueryTypes[37] = SqlDbType.DateTime;
                QueryTypes[38] = SqlDbType.VarChar;
                QueryTypes[39] = SqlDbType.Bit;
                QueryTypes[40] = SqlDbType.VarChar;
                QueryTypes[41] = SqlDbType.VarChar;
                QueryTypes[42] = SqlDbType.VarChar;
                QueryTypes[43] = SqlDbType.DateTime;
                QueryTypes[44] = SqlDbType.Bit;
                QueryTypes[45] = SqlDbType.VarChar;
                QueryTypes[46] = SqlDbType.DateTime;

                if (db.InsertData("sp_InsertCustomsAWBAcceptance", QueryNames, QueryTypes, ParamValues))
                {
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Save/Update Customs AWB Messages
        public bool SaveCustomsAWBMessages(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[39];
                SqlDbType[] QueryTypes = new SqlDbType[39];

                QueryNames[0] = "AirwayBillNo";
                QueryNames[1] = "HouseAWBNo";
                QueryNames[2] = "CountryCode";
                QueryNames[3] = "FlightNo";
                QueryNames[4] = "FlightDate";
                QueryNames[5] = "CustomsStatus";
                QueryNames[6] = "ReportedDate";
                QueryNames[7] = "Origin";
                QueryNames[8] = "Destination";
                QueryNames[9] = "Customs";
                QueryNames[10] = "Arrival";
                QueryNames[11] = "Offload";
                QueryNames[12] = "Part";
                QueryNames[13] = "Description";
                QueryNames[14] = "Consol";
                QueryNames[15] = "QueryStatus";
                QueryNames[16] = "Shipper";
                QueryNames[17] = "Consignee";
                QueryNames[18] = "CustomsValue";
                QueryNames[19] = "Currency";
                QueryNames[20] = "FDA";
                QueryNames[21] = "TotalPcs";
                QueryNames[22] = "TotalWt";
                QueryNames[23] = "ManifestedPcs";
                QueryNames[24] = "ManifestedWt";
                QueryNames[25] = "Released";
                QueryNames[26] = "Exam";
                QueryNames[27] = "USCS";
                QueryNames[28] = "USDA";
                QueryNames[29] = "Other";
                QueryNames[30] = "TransitOrigin";
                QueryNames[31] = "TransitDest";
                QueryNames[32] = "Shed";
                QueryNames[33] = "Agent";
                QueryNames[34] = "TransitControl";
                QueryNames[35] = "OnwardCarrier";
                QueryNames[36] = "Bond";
                QueryNames[37] = "UserName";
                QueryNames[38] = "TimeStamp";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.DateTime;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.DateTime;
                QueryTypes[7] = SqlDbType.VarChar;
                QueryTypes[8] = SqlDbType.VarChar;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;
                QueryTypes[11] = SqlDbType.VarChar;
                QueryTypes[12] = SqlDbType.VarChar;
                QueryTypes[13] = SqlDbType.VarChar;
                QueryTypes[14] = SqlDbType.VarChar;
                QueryTypes[15] = SqlDbType.VarChar;
                QueryTypes[16] = SqlDbType.VarChar;
                QueryTypes[17] = SqlDbType.VarChar;
                QueryTypes[18] = SqlDbType.VarChar;
                QueryTypes[19] = SqlDbType.VarChar;
                QueryTypes[20] = SqlDbType.VarChar;
                QueryTypes[21] = SqlDbType.Int;
                QueryTypes[22] = SqlDbType.Decimal;
                QueryTypes[23] = SqlDbType.Int;
                QueryTypes[24] = SqlDbType.Decimal;
                QueryTypes[25] = SqlDbType.Int;
                QueryTypes[26] = SqlDbType.Int;
                QueryTypes[27] = SqlDbType.Int;
                QueryTypes[28] = SqlDbType.Int;
                QueryTypes[29] = SqlDbType.Int;
                QueryTypes[30] = SqlDbType.VarChar;
                QueryTypes[31] = SqlDbType.VarChar;
                QueryTypes[32] = SqlDbType.VarChar;
                QueryTypes[33] = SqlDbType.VarChar;
                QueryTypes[34] = SqlDbType.VarChar;
                QueryTypes[35] = SqlDbType.VarChar;
                QueryTypes[36] = SqlDbType.VarChar;
                QueryTypes[37] = SqlDbType.VarChar;
                QueryTypes[38] = SqlDbType.DateTime;

                if (db.InsertData("sp_InsertCustomsAWBMessages", QueryNames, QueryTypes, ParamValues))
                {
                    return true;
                }
                else
                    return false;

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
