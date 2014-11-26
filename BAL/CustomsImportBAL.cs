using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class CustomsImportBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DataSet ds;

        #region Update FRI Message
        public bool UpdateFRIMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[5];
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "IsPopup";
                QueryNames[2] = "FlightNo";
                QueryNames[3] = "FlightDate";
                QueryNames[4] = "Message";
                SqlDbType[] QueryTypes = new SqlDbType[5];
                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.Bit;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.Text;
                if (db.UpdateData("sp_GenerateFRIMessage", QueryNames, QueryTypes, QueryValues))
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

        #region Update FSN Message
        public bool UpdateFSNMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[4];
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNo";
                QueryNames[2] = "FlightDate";
                QueryNames[3] = "Message";
                SqlDbType[] QueryTypes = new SqlDbType[4];
                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                if (db.UpdateData("sp_GenerateFSNMessage", QueryNames, QueryTypes, QueryValues))
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

        #region Update FDM Message
        public bool UpdateFDMMessage(object[] QueryValues)
        {
            try
            {
                string[]QueryNames=new string[8];
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "Carrier";
                QueryNames[2] = "FlightNumber";
                QueryNames[3] = "UpdatedOn";
                QueryNames[4] = "UpdatedBy";
                QueryNames[5] = "AWBPrefix";
                QueryNames[6] = "FlightDate";
                QueryNames[7] = "Message";
                SqlDbType[] QueryTypes = new SqlDbType[8];
                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.DateTime;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.DateTime;
                QueryTypes[7] = SqlDbType.VarChar;
                if (db.UpdateData("sp_GenerateFDMMessage", QueryNames,QueryTypes, QueryValues))
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

        #region Update FRC Message
        public bool UpdateFRCMessageOffload(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[4];
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNo";
                QueryNames[2] = "FlightDate";
                QueryNames[3] = "Message";
                SqlDbType[] QueryTypes = new SqlDbType[4];
                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                if (db.UpdateData("sp_GenerateFRCMessage", QueryNames, QueryTypes, QueryValues))
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

        public bool UpdateFRCMessageReAssign(object QueryValues)
        {
            try
            {
                if (db.UpdateData("sp_GenerateFRCMessageReAssign", "AWBNumber", SqlDbType.VarChar, QueryValues))
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

        #region Update FRX Message
        public bool UpdateFRXMessageOffload(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[4];
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNo";
                QueryNames[2] = "FlightDate";
                QueryNames[3] = "Message";
                SqlDbType[] QueryTypes = new SqlDbType[4];
                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                if (db.UpdateData("sp_GenerateFRXMessageOffload", QueryNames, QueryTypes, QueryValues))
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

        public bool UpdateFRXMessageReAssign(object QueryValues)
        {
            try
            {
                if (db.UpdateData("sp_GenerateFRXMessageReAssign", "AWBNumber", SqlDbType.VarChar, QueryValues))
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

        #region Update FER Message
        public bool UpdateFERMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[4];
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNo";
                QueryNames[2] = "FlightDate";
                QueryNames[3] = "Message";
                SqlDbType[] QueryTypes = new SqlDbType[4];
                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                if (db.UpdateData("sp_GenerateFERMessage", QueryNames, QueryTypes, QueryValues))
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

        #region Listing Custom AWB's
        public DataSet GetCustomsAWBList(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[6];
                SqlDbType[] QueryTypes = new SqlDbType[6];

                QueryNames[0] = "AWBPrefix";
                QueryNames[1] = "AWBNumber";
                QueryNames[2] = "FlightNumber";
                QueryNames[3] = "FlightDate";
                QueryNames[4] = "ULD";
                QueryNames[5] = "DestAirport";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;

                ds = db.SelectRecords("SP_GetCustomsAWBList", QueryNames, QueryValues, QueryTypes);
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
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Updating Customs Messages

        public bool UpdateCustomsMessages(object[] QueryValues,string MessageType)
        {
            try
            {
                string[] QueryNames = new string[85];
                SqlDbType[] QueryTypes = new SqlDbType[85];
                int i = 0;
                QueryNames[i++] = "AWBPrefix";
                QueryNames[i++] = "AWBNumber";
                QueryNames[i++] = "MessageType";
                QueryNames[i++] = "HAWBNumber";
                QueryNames[i++] = "ConsolidationIdentifier";
                QueryNames[i++] = "PackageTrackingIdentifier";
                QueryNames[i++] = "AWBPartArrivalReference";
                QueryNames[i++] = "ArrivalAirport";
                QueryNames[i++] = "AirCarrier";
                QueryNames[i++] = "Origin";
                QueryNames[i++] = "DestinionCode";
                QueryNames[i++] = "WBLNumberOfPieces";
                QueryNames[i++] = "WBLWeightIndicator";
                QueryNames[i++] = "WBLWeight";
                QueryNames[i++] = "WBLCargoDescription";
                QueryNames[i++] = "ArrivalDate";
                QueryNames[i++] = "PartArrivalReference";
                QueryNames[i++] = "BoardedQuantityIdentifier";
                QueryNames[i++] = "BoardedPieceCount";
                QueryNames[i++] = "BoardedWeight";
                QueryNames[i++] = "ImportingCarrier";
                QueryNames[i++] = "FlightNumber";
                QueryNames[i++] = "ARRPartArrivalReference";
                QueryNames[i++] = "RequestType";
                QueryNames[i++] = "RequestExplanation";
                QueryNames[i++] = "EntryType";
                QueryNames[i++] = "EntryNumber";
                QueryNames[i++] = "AMSParticipantCode";
                QueryNames[i++] = "ShipperName";
                QueryNames[i++] = "ShipperAddress";
                QueryNames[i++] = "ShipperCity";
                QueryNames[i++] = "ShipperState";
                QueryNames[i++] = "ShipperCountry";
                QueryNames[i++] = "ShipperPostalCode";
                QueryNames[i++] = "ConsigneeName";
                QueryNames[i++] = "ConsigneeAddress";
                QueryNames[i++] = "ConsigneeCity";
                QueryNames[i++] = "ConsigneeState";
                QueryNames[i++] = "ConsigneeCountry";
                QueryNames[i++] = "ConsigneePostalCode";
                QueryNames[i++] = "TransferDestAirport";
                QueryNames[i++] = "DomesticIdentifier";
                QueryNames[i++] = "BondedCarrierID";
                QueryNames[i++] = "OnwardCarrier";
                QueryNames[i++] = "BondedPremisesIdentifier";
                QueryNames[i++] = "InBondControlNumber";
                QueryNames[i++] = "OriginOfGoods";
                QueryNames[i++] = "DeclaredValue";
                QueryNames[i++] = "CurrencyCode";
                QueryNames[i++] = "CommodityCode";
                QueryNames[i++] = "LineIdentifier";
                QueryNames[i++] = "AmendmentCode";
                QueryNames[i++] = "AmendmentExplanation";
                QueryNames[i++] = "DeptImportingCarrier";
                QueryNames[i++] = "DeptFlightNumber";
                QueryNames[i++] = "DeptScheduledArrivalDate";
                QueryNames[i++] = "LiftoffDate";
                QueryNames[i++] = "LiftoffTime";
                QueryNames[i++] = "DeptActualImportingCarrier";
                QueryNames[i++] = "DeptActualFlightNumber";
                QueryNames[i++] = "ASNStatusCode";
                QueryNames[i++] = "ASNActionExplanation";
                QueryNames[i++] = "CSNActionCode";
                QueryNames[i++] = "CSNPieces";
                QueryNames[i++] = "TransactionDate";
                QueryNames[i++] = "TransactionTime";
                QueryNames[i++] = "CSNEntryType";
                QueryNames[i++] = "CSNEntryNumber";
                QueryNames[i++] = "CSNRemarks";
                QueryNames[i++] = "ErrorCode";
                QueryNames[i++] = "ErrorMessage";
                QueryNames[i++] = "StatusRequestCode";
                QueryNames[i++] = "StatusAnswerCode";
                QueryNames[i++] = "Information";
                QueryNames[i++] = "ERFImportingCarrier";
                QueryNames[i++] = "ERFFlightNumber";
                QueryNames[i++] = "ERFDate";
                QueryNames[i++] = "Message";
                QueryNames[i++] = "UpdatedOn";
                QueryNames[i++] = "UpdatedBy";
                QueryNames[i++] = "CreatedOn";
                QueryNames[i++] = "CreatedBy";
                QueryNames[i++] = "FlightNo";
                QueryNames[i++] = "FlightDate";
                QueryNames[i++] = "ControlLocation";

                int j = 0;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.Int;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.Decimal;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.Int;
                QueryTypes[j++] = SqlDbType.Decimal;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.BigInt;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.DateTime;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.DateTime;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;
                QueryTypes[j++] = SqlDbType.VarChar;

                if (MessageType == "FRI" || MessageType == "FXI" || MessageType == "FRC" || MessageType == "FXC" || MessageType == "FRX" || MessageType == "FXX" || MessageType == "FDM" || MessageType == "FER" || MessageType == "FSQ" || MessageType == "FSN" || MessageType == "PSN" || MessageType == "PER" || MessageType == "PRI")
                {
                    if (db.InsertData("SP_UpdateOutboxCustomsMessage", QueryNames, QueryTypes, QueryValues))
                    { return true; }
                    else
                    { return false; }
                }
                else
                {
                    if (db.InsertData("SP_UpdateInboxCustomsMessage", QueryNames, QueryTypes, QueryValues))
                    { return true; }
                    else
                    { return false; }
                }

            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        #region Fetch Customs AMS Data AWBWise

        public DataSet FetchCustomsAWBDetails(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[4];
                //object[] QueryValues = new object[2];
                SqlDbType[] QueryTypes = new SqlDbType[4];

                QueryNames[0] = "AWBPrefix";
                QueryNames[1] = "AWBNumber";
                QueryNames[2] = "FlightNo";
                QueryNames[3] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;


                 ds = db.SelectRecords("SP_GetCustomsMessagingData", QueryNames, QueryValues, QueryTypes);
                 if (ds != null)
                 {
                     if (ds.Tables.Count > 0)
                     {
                         if (ds.Tables[2].Rows.Count > 0)
                         {
                             return ds;
                         }
                         else
                         { return ds; }
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

        #region Validate Customs Messages
        public bool ValidateCustomsMsg(object[] QueryValues,string MessageType)
        {
            try
            {
                string[] QueryNames = new string[4];
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNo";
                QueryNames[2] = "FlightDate";
                QueryNames[3] = "MessageType";
                
                SqlDbType[] QueryTypes = new SqlDbType[4];
                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;

                object[] QueryVal = new object[4];
                QueryVal[0] = QueryValues[0];
                QueryVal[1] = QueryValues[1];
                QueryVal[2] = QueryValues[2];
                QueryVal[3] = MessageType;



                DataSet ds = db.SelectRecords("SP_GetCustomsMessagingData_Validation", QueryNames, QueryVal, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {

                            foreach (DataRow row in ds.Tables[2].Rows)
                            {
                                #region Validate FRI Messages
                                if (MessageType == "FRI")
                                {
                                    if (row["AWBPrefix"].ToString() == "" || row["AWBNumber"].ToString() == "")
                                    {

                                        return false;
                                    }
                                    if (row["Origin"].ToString() == "" || row["WBLCargoDescription"].ToString() == "" || row["WBLNumberOfPieces"].ToString() == "" || row["WBLWeightIndicator"].ToString() == "" || row["WBLWeight"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["ImportingCarrier"].ToString() == "" || row["FlightNumber"].ToString() == "" || row["ArrivalDate"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["ShipperName"].ToString() == "" || row["ShipperCity"].ToString() == "" || row["ShipperCountry"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["ConsigneeName"].ToString() == "" || row["ConsigneeCity"].ToString() == "" || row["ConsigneeCountry"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["LineIdentifier"].ToString() == "")
                                    {
                                        return false;
                                    }

                                }
                                #endregion

                                #region Validate FDM Messages
                                if (MessageType == "FDM")
                                {

                                    if (row["DeptImportingCarrier"].ToString() == "" || row["DeptFlightNumber"].ToString() == "" || row["DeptScheduledArrivalDate"].ToString() == "" || row["DeptActualImportingCarrier"].ToString() == "" || row["DeptActualFlightNumber"].ToString() == "")
                                    {

                                        return false;
                                    }
                                }
                                #endregion

                                #region Validate FSN Messages
                                if (MessageType == "FSN")
                                {
                                    if (row["AWBPrefix"].ToString() == "" || row["AWBNumber"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["ImportingCarrier"].ToString() == "" || row["FlightNumber"].ToString() == "" || row["ArrivalDate"].ToString() == "")
                                    {

                                        return false;
                                    }

                                    if (row["ASNStatusCode"].ToString() == "")
                                    {
                                        return false;
                                    }


                                }
                                #endregion

                                #region Validate FSN Inbox Messages
                                if (MessageType == "FSNInbox")
                                {

                                    if (row["AWBPrefix"].ToString() == "" || row["AWBNumber"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["ImportingCarrier"].ToString() == "" || row["FlightNumber"].ToString() == "" || row["ArrivalDate"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["CSNActionCode"].ToString() == "" || row["CSNPieces"].ToString() == "" || row["TransactionDate"].ToString() == "" || row["TransactionTime"].ToString() == "")
                                    {
                                        return false;
                                    }


                                }
                                #endregion

                                #region Validate FSQ Messages
                                if (MessageType == "FSQ")
                                {
                                    if (row["AWBPrefix"].ToString() == "" || row["AWBNumber"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["StatusRequestCode"].ToString() == "")
                                    {
                                        return false;
                                    }
                                }
                                #endregion

                                #region Validate FRC Messages
                                if (MessageType == "FRC")
                                {


                                    if (row["AWBPrefix"].ToString() == "" || row["AWBNumber"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["Origin"].ToString() == "" || row["WBLNumberOfPieces"].ToString() == "" || row["WBLWeightIndicator"].ToString() == "" || row["WBLWeight"].ToString() == "" || row["WBLCargoDescription"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["ImportingCarrier"].ToString() == "" || row["FlightNumber"].ToString() == "" || row["ArrivalDate"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["ShipperName"].ToString() == "" || row["ShipperCity"].ToString() == "" || row["ShipperCountry"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["ConsigneeName"].ToString() == "" || row["ConsigneeCity"].ToString() == "" || row["ConsigneeCountry"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["LineIdentifier"].ToString() == "")
                                    {
                                        return false;
                                    }
                                    if (row["AmendmentCode"].ToString() == "")
                                    {
                                        return false;
                                    }


                                }
                                #endregion

                                //Start
                                #region Validate FSI Messages
                                if (MessageType == "FSI")
                                {
                                    if (row["AWBPrefix"].ToString() == "" || row["AWBNumber"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["ImportingCarrier"].ToString() == "" || row["FlightNumber"].ToString() == "" || row["ArrivalDate"].ToString() == "")
                                    {
                                        return false;
                                    }

                                    if (row["CSNActionCode"].ToString() == "" || row["CSNPieces"].ToString() == "" || row["TransactionDate"].ToString() == "" || row["TransactionTime"].ToString() == "")
                                    {

                                        return false;
                                    }
                                }
                                #endregion

                                //END


                            }
                        }
                        else { return false; }

                        return true;
                    }
                    else { return true; }

                }
                else
                    return false;
            }

            catch (Exception ex)
            { return false; }

        }    
        #endregion

        #region Check Customs AWB Availability

        public DataSet CheckCustomsAWBAvailability(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[3];
                //object[] QueryValues = new object[2];
                SqlDbType[] QueryTypes = new SqlDbType[3];

                //QueryNames[0] = "AWBPrefix";
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNo";
                QueryNames[2] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                //QueryTypes[3] = SqlDbType.VarChar;


                ds = db.SelectRecords("sp_CheckCustomsApplicability", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return ds; }
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

        #region Encoding Messages

        #region Encoding FRI Message
        public StringBuilder EncodingFRIMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = { "AWBNumber", "FlightNo", "FlightDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                StringBuilder sb = new StringBuilder();
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet Dset = db.SelectRecords("sp_GetFRIDataAutoMsg", QueryNames, QueryValues, QueryTypes);
                if (Dset != null)
                {

                    if (Dset.Tables.Count > 0)
                    {
                        if (Dset.Tables[0].Rows.Count > 0)
                        {
                            sb.AppendLine("FRI");


                            if (Dset.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["CCLocation"].ToString() != "")
                                    {
                                        sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                    }
                                }
                                foreach (DataRow row in Dset.Tables[1].Rows)
                                {
                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "" || row["HAWBNo"].ToString() != "")
                                    {

                                        sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString() + row["HAWBNo"].ToString());
                                    }
                                }
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["Origin"].ToString() != "" || row["WBLCargoDescription"].ToString() != "" || row["WBLNumberOfPieces"].ToString() != "" || row["WBLWeightIndicator"].ToString() != "" || row["WBLWeight"].ToString() != "")
                                    {
                                        sb.AppendLine("WBL" + "/" + row["Origin"].ToString() + "/" + "T"
                                        + row["WBLNumberOfPieces"].ToString() + "/" + row["WBLWeightIndicator"].ToString() + row["WBLWeight"].ToString()
                                        + "/" + row["WBLCargoDescription"].ToString());
                                    }
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5).ToUpper());
                                    }

                                }
                            }
                            else
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["CCLocation"].ToString() != "")
                                    {
                                        sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                    }
                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "")
                                    {
                                        if (row["ConsolidationIdentifier"].ToString() == "")
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString());
                                        }
                                        else
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString()); 
                                        }
                                    }
                                    foreach (DataRow Srow in Dset.Tables[0].Rows)
                                    {
                                        if (Srow["Origin"].ToString() != "" || Srow["WBLCargoDescription"].ToString() != "" || Srow["WBLNumberOfPieces"].ToString() != "" || Srow["WBLWeightIndicator"].ToString() != "" || Srow["WBLWeight"].ToString() != "")
                                        {
                                            sb.AppendLine("WBL" + "/" + Srow["Origin"].ToString() + "/" + "T"
                                            + Srow["WBLNumberOfPieces"].ToString() + "/" + Srow["WBLWeightIndicator"].ToString() + Srow["WBLWeight"].ToString()
                                            + "/" + Srow["WBLCargoDescription"].ToString());
                                        }

                                    }
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5).ToUpper());
                                    }


                                }
                            }
                            if (Dset.Tables[2].Rows.Count > 0)
                            {
                                foreach (DataRow Srow in Dset.Tables[2].Rows)
                                {
                                    if (Srow["ShipperName"].ToString() != "" || Srow["ShipperCity"].ToString() != "" || Srow["ShipperCountry"].ToString() != "")
                                    {
                                        sb.AppendLine("SHP" + "/" + Srow["ShipperName"].ToString());
                                        if (Srow["ShipperAddress"].ToString() != "")
                                        {
                                            sb.AppendLine("/" + Srow["ShipperAddress"].ToString());
                                        }
                                        sb.AppendLine("/" + Srow["ShipperCity"].ToString() + "/" + Srow["ShipperState"].ToString());
                                        sb.AppendLine("/" + Srow["ShipperCountry"].ToString() + "/" + Srow["ShipperPinCode"].ToString());
                                    }

                                    if (Srow["ConsigneeName"].ToString() != "" || Srow["ConsigneeCity"].ToString() != "" || Srow["ConsigneeCountry"].ToString() != "")
                                    {
                                        sb.AppendLine("SHP" + "/" + Srow["ConsigneeName"].ToString());
                                        if (Srow["ConsigneeAddress"].ToString() != "")
                                        {
                                            sb.AppendLine("/" + Srow["ConsigneeAddress"].ToString());
                                        }
                                        sb.AppendLine("/" + Srow["ConsigneeCity"].ToString() + "/" + Srow["ConsigneeState"].ToString());
                                        sb.AppendLine("/" + Srow["ConsigneeCountry"].ToString() + "/" + Srow["ConsigneePinCode"].ToString());
                                    }
                                }
                            }
                            if (Dset.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["FlightDestination"].ToString() != "")
                                    {
                                        if (row["DomesticIdentifier"].ToString() != "")
                                        {
                                            sb.AppendLine("TRN" + "/" + row["FlightDestination"].ToString() + "-" + row["DomesticIdentifier"].ToString());
                                        }
                                        else
                                        {
                                            sb.AppendLine("TRN" + "/" + row["FlightDestination"].ToString());

                                        }
                                    }
                                    if (row["LineIdentifier"].ToString() != "")
                                    {
                                        sb.AppendLine(row["LineIdentifier"].ToString());
                                    }
                                }
                            }
                            db = null;
                            Dset.Dispose();
                            return sb;
                        }
                        else
                        {
                            db = null;
                            Dset.Dispose();
                            return null;
                        }


                    }
                    else
                    {
                        db = null;
                        Dset.Dispose();
                        return null;
                    }
                }
                else
                {
                    db = null;
                    Dset.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                db = null;
                
                return null ;
 
            }
        }
        #endregion

        #region Encoding FDM Message
        public StringBuilder EncodingFDMMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = { "AWBNumber", "FlightNo", "FlightDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                StringBuilder sb = new StringBuilder();
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet Dset = db.SelectRecords("sp_GetFDMDataAutoMsg", QueryNames, QueryValues, QueryTypes);
                if (Dset != null)
                {

                    if (Dset.Tables.Count > 0)
                    {
                        if (Dset.Tables[0].Rows.Count > 0)
                        {
                            sb.AppendLine("FDM");
                            foreach (DataRow row in Dset.Tables[0].Rows)
                            {
                                if (row["ImportingCarrier"].ToString() != "" || row["AWBNumber"].ToString() != "" || row["HAWBNo"].ToString() != "")
                                {
                                    sb.AppendLine("DEP" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" + row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5).ToUpper() + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString());
                                }
                            }
                            
                            db = null;
                            Dset.Dispose();
                            return sb;
                        }
                        else
                        {
                            db = null;
                            Dset.Dispose();
                            return null;
                        }


                    }
                    else
                    {
                        db = null;
                        Dset.Dispose();
                        return null;
                    }
                }
                else
                {
                    db = null;
                    Dset.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                db = null;

                return null;

            }
        }
        #endregion

        #region Encoding FSN Message
        public StringBuilder EncodingFSNMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = { "AWBNumber", "FlightNo", "FlightDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                StringBuilder sb = new StringBuilder();
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet Dset = db.SelectRecords("sp_GetFSNDataAutoMsg", QueryNames, QueryValues, QueryTypes);
                if (Dset != null)
                {

                    if (Dset.Tables.Count > 0)
                    {
                        if (Dset.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow Drow in Dset.Tables[2].Rows)
                            {
                                if (Drow["ShipperName"].ToString() != "" || Drow["ShipperCity"].ToString() != "" || Drow["ShipperCountry"].ToString() != "" || Drow["ConsigneeName"].ToString() != "" || Drow["ConsigneeCity"].ToString() != "" || Drow["ConsigneeCountry"].ToString() != "")
                                {
                                    if (Dset.Tables[1].Rows.Count > 0)
                                    {
                                        sb.AppendLine("FSN");

                                        foreach (DataRow row in Dset.Tables[0].Rows)
                                        {
                                            if (row["CCLocation"].ToString() != "")
                                            {
                                                sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                            }
                                        }
                                        foreach (DataRow row in Dset.Tables[1].Rows)
                                        {
                                            if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "" || row["HAWBNo"].ToString() != "")
                                            {

                                                sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString() + row["HAWBNo"].ToString());
                                            }
                                        }
                                        foreach (DataRow row in Dset.Tables[0].Rows)
                                        {
                                            if (row["Origin"].ToString() != "" || row["WBLCargoDescription"].ToString() != "" || row["WBLNumberOfPieces"].ToString() != "" || row["WBLWeightIndicator"].ToString() != "" || row["WBLWeight"].ToString() != "")
                                            {
                                                sb.AppendLine("WBL" + "/" + row["Origin"].ToString() + "/" + "T"
                                                + row["WBLNumberOfPieces"].ToString() + "/" + row["WBLWeightIndicator"].ToString() + row["WBLWeight"].ToString()
                                                + "/" + row["WBLCargoDescription"].ToString());
                                            }
                                            if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                            {
                                                sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                                row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5).ToUpper());
                                            }
                                            if (row["ASNCode"].ToString() != "")
                                            {
                                                sb.AppendLine("ASN" + "/" + row["ASNCode"].ToString());
                                            }

                                        }
                                    }
                                    else
                                    {
                                        sb.AppendLine("FSN");


                                        foreach (DataRow row in Dset.Tables[0].Rows)
                                        {
                                            if (row["CCLocation"].ToString() != "")
                                            {
                                                sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                            }
                                            if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "")
                                            {
                                                if (row["ConsolidationIdentifier"].ToString() == "")
                                                {
                                                    sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString());
                                                }
                                                else
                                                {
                                                    sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString());
                                                }
                                            }
                                            foreach (DataRow Srow in Dset.Tables[0].Rows)
                                            {
                                                if (Srow["Origin"].ToString() != "" || Srow["WBLCargoDescription"].ToString() != "" || Srow["WBLNumberOfPieces"].ToString() != "" || Srow["WBLWeightIndicator"].ToString() != "" || Srow["WBLWeight"].ToString() != "")
                                                {
                                                    sb.AppendLine("WBL" + "/" + Srow["Origin"].ToString() + "/" + "T"
                                                    + Srow["WBLNumberOfPieces"].ToString() + "/" + Srow["WBLWeightIndicator"].ToString() + Srow["WBLWeight"].ToString()
                                                    + "/" + Srow["WBLCargoDescription"].ToString());
                                                }

                                            }
                                            if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                            {
                                                sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                                row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5).ToUpper());
                                            }
                                            if (row["ASNCode"].ToString() != "")
                                            {
                                                sb.AppendLine("ASN" + "/" + row["ASNCode"].ToString());
                                            }


                                        }

                                        db = null;
                                        Dset.Dispose();
                                        return sb;
                                    }
                                   
                                    
                                }
                                else
                                {
                                    db = null;
                                    Dset.Dispose();
                                    return null;
                                }

                               
                            }
                            db = null;
                            Dset.Dispose();
                            return sb;

                            
                        }
                        else
                        {
                            db = null;
                            Dset.Dispose();
                            return null;
                        }


                    }
                    else
                    {
                        db = null;
                        Dset.Dispose();
                        return null;
                    }
                }
                else
                {
                    db = null;
                    Dset.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                db = null;

                return null;

            }
        }
        #endregion

        #region Encoding FRC Message
        public StringBuilder EncodingFRCMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = { "AWBNumber", "FlightNo", "FlightDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                StringBuilder sb = new StringBuilder();
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet Dset = db.SelectRecords("sp_GetFRCDataAutoMsg", QueryNames, QueryValues, QueryTypes);
                if (Dset != null)
                {

                    if (Dset.Tables.Count > 0)
                    {
                        if (Dset.Tables[0].Rows.Count > 0)
                        {
                            sb.AppendLine("FRC");


                            if (Dset.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["CCLocation"].ToString() != "")
                                    {
                                        sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                    }
                                }
                                foreach (DataRow row in Dset.Tables[1].Rows)
                                {

                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "" || row["HAWBNo"].ToString() != "")
                                    {

                                        sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString() + row["HAWBNo"].ToString());
                                    }
                                }
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    
                                    if (row["Origin"].ToString() != "" || row["WBLCargoDescription"].ToString() != "" || row["WBLNumberOfPieces"].ToString() != "" || row["WBLWeightIndicator"].ToString() != "" || row["WBLWeight"].ToString() != "")
                                    {
                                        sb.AppendLine("WBL" + "/" + row["Origin"].ToString() + "/" + "T"
                                        + row["WBLNumberOfPieces"].ToString() + "/" + row["WBLWeightIndicator"].ToString() + row["WBLWeight"].ToString()
                                        + "/" + row["WBLCargoDescription"].ToString());
                                    }
                                    if (row["OffloadedOrigin"].ToString() != "" || row["OffloadedCommodity"].ToString() != "" || row["OffloadPcs"].ToString() != "" || row["WBLWeightIndicator"].ToString() != "" || row["Offloadweight"].ToString() != "")
                                    {
                                        sb.AppendLine("WBL" + "/" + row["OffloadedOrigin"].ToString() + "/" + "T"
                                        + row["OffloadPcs"].ToString() + "/" + row["WBLWeightIndicator"].ToString() + row["Offloadweight"].ToString()
                                        + "/" + row["WBLCargoDescription"].ToString());
                                    }
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5));
                                    }

                                }
                            }
                            else
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["CCLocation"].ToString() != "")
                                    {
                                        sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                    }
                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "")
                                    {

                                        if (row["ConsolidationIdentifier"].ToString() == "")
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString());
                                        }
                                        else
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString());
                                        }
                                    }
                                    foreach (DataRow Srow in Dset.Tables[0].Rows)
                                    {
                                        if (Srow["Origin"].ToString() != "" || Srow["WBLCargoDescription"].ToString() != "" || Srow["WBLNumberOfPieces"].ToString() != "" || Srow["WBLWeightIndicator"].ToString() != "" || Srow["WBLWeight"].ToString() != "")
                                        {
                                            sb.AppendLine("WBL" + "/" + Srow["Origin"].ToString() + "/" + "T"
                                            + Srow["WBLNumberOfPieces"].ToString() + "/" + Srow["WBLWeightIndicator"].ToString() + Srow["WBLWeight"].ToString()
                                            + "/" + Srow["WBLCargoDescription"].ToString());
                                        }
                                        if (Srow["OffloadedOrigin"].ToString() != "" || Srow["OffloadedCommodity"].ToString() != "" || Srow["OffloadPcs"].ToString() != "" || Srow["WBLWeightIndicator"].ToString() != "" || Srow["Offloadweight"].ToString() != "")
                                        {
                                            sb.AppendLine("WBL" + "/" + Srow["OffloadedOrigin"].ToString() + "/" + "T"
                                            + Srow["OffloadPcs"].ToString() + "/" + Srow["WBLWeightIndicator"].ToString() + Srow["Offloadweight"].ToString()
                                            + "/" + Srow["WBLCargoDescription"].ToString());
                                        }

                                    }
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5));
                                    }


                                }
                            }
                            if (Dset.Tables[2].Rows.Count > 0)
                            {
                                foreach (DataRow Srow in Dset.Tables[2].Rows)
                                {
                                    if (Srow["ShipperName"].ToString() != "" || Srow["ShipperCity"].ToString() != "" || Srow["ShipperCountry"].ToString() != "")
                                    {
                                        sb.AppendLine("SHP" + "/" + Srow["ShipperName"].ToString());
                                        if (Srow["ShipperAddress"].ToString() != "")
                                        {
                                            sb.AppendLine("/" + Srow["ShipperAddress"].ToString());
                                        }
                                        sb.AppendLine("/" + Srow["ShipperCity"].ToString() + "/" + Srow["ShipperState"].ToString());
                                        sb.AppendLine("/" + Srow["ShipperCountry"].ToString() + "/" + Srow["ShipperPinCode"].ToString());
                                    }

                                    if (Srow["ConsigneeName"].ToString() != "" || Srow["ConsigneeCity"].ToString() != "" || Srow["ConsigneeCountry"].ToString() != "")
                                    {
                                        sb.AppendLine("SHP" + "/" + Srow["ConsigneeName"].ToString());
                                        if (Srow["ConsigneeAddress"].ToString() != "")
                                        {
                                            sb.AppendLine("/" + Srow["ConsigneeAddress"].ToString());
                                        }
                                        sb.AppendLine("/" + Srow["ConsigneeCity"].ToString() + "/" + Srow["ConsigneeState"].ToString());
                                        sb.AppendLine("/" + Srow["ConsigneeCountry"].ToString() + "/" + Srow["ConsigneePinCode"].ToString());
                                    }
                                }
                            }
                            if (Dset.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["LineIdentifier"].ToString() != "")
                                    {
                                        sb.AppendLine(row["LineIdentifier"].ToString());
                                    }
                                    if (row["AmendmentCode"].ToString() != "")
                                    {
                                        sb.AppendLine("RFA" + "/" + row["AmendmentCode"].ToString());
                                    }
                                }
                            }
                            db = null;
                            Dset.Dispose();
                            return sb;
                        }
                        else
                        {
                            db = null;
                            Dset.Dispose();
                            return null;
                        }


                    }
                    else
                    {
                        db = null;
                        Dset.Dispose();
                        return null;
                    }
                }
                else
                {
                    db = null;
                    Dset.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                db = null;

                return null;

            }
        }
        #endregion

        #region Encoding FRX Message
        public StringBuilder EncodingFRXMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = { "AWBNumber", "FlightNo", "FlightDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                StringBuilder sb = new StringBuilder();
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet Dset = db.SelectRecords("sp_GetFRXDataAutoMsg", QueryNames, QueryValues, QueryTypes);
                if (Dset != null)
                {

                    if (Dset.Tables.Count > 0)
                    {
                        if (Dset.Tables[0].Rows.Count > 0)
                        {
                            sb.AppendLine("FRX");


                            if (Dset.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["CCLocation"].ToString() != "")
                                    {
                                        sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                    }
                                }
                                foreach (DataRow row in Dset.Tables[1].Rows)
                                {
                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "" || row["HAWBNo"].ToString() != "")
                                    {

                                        sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString() + row["HAWBNo"].ToString());
                                    }
                                    
                                }
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                   
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5));
                                    }

                                }

                            }
                            else
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["CCLocation"].ToString() != "")
                                    {
                                        sb.AppendLine(row["CCLocation"].ToString() + row["ImportingCarrier"].ToString());
                                    }
                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "")
                                    {
                                        if (row["ConsolidationIdentifier"].ToString() == "")
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString());
                                        }
                                        else
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString());
                                        }
                                    }
                                  
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine("ARR" + "/" + row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5));
                                    }


                                }
                            }
                            if (Dset.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["AmendmentCode"].ToString() != "")
                                    {
                                        sb.AppendLine("RFA" + "/" + row["AmendmentCode"].ToString());
                                    }
                                }
                            }
                            db = null;
                            Dset.Dispose();
                            return sb;
                        }
                        else
                        {
                            db = null;
                            Dset.Dispose();
                            return null;
                        }


                    }
                    else
                    {
                        db = null;
                        Dset.Dispose();
                        return null;
                    }
                }
                else
                {
                    db = null;
                    Dset.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                db = null;

                return null;

            }
        }
        #endregion

        #region Encoding FER Message
        public StringBuilder EncodingFERMessage(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = { "AWBNumber", "FlightNo", "FlightDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                StringBuilder sb = new StringBuilder();
                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet Dset = db.SelectRecords("sp_GetFERDataAutoMsg", QueryNames, QueryValues, QueryTypes);
                if (Dset != null)
                {

                    if (Dset.Tables.Count > 0)
                    {
                        if (Dset.Tables[0].Rows.Count > 0)
                        {
                            sb.AppendLine("FER");

                            if (Dset.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine(row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5).ToUpper());
                                    }
                                }
                                foreach (DataRow row in Dset.Tables[1].Rows)
                                {
                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "" || row["HAWBNo"].ToString() != "")
                                    {

                                        sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString() + row["HAWBNo"].ToString());
                                    }
                                }
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["ErrorCode"].ToString() != "" || row["ErrorDescription"].ToString() != "")
                                    {
                                        sb.AppendLine("ERR" + "/" + row["ErrorCode"].ToString() + row["ErrorDescription"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow row in Dset.Tables[0].Rows)
                                {
                                    if (row["ImportingCarrier"].ToString() != "" || row["FlightNumber"].ToString() != "" || row["ArrivalDate"].ToString() != "")
                                    {
                                        sb.AppendLine(row["ImportingCarrier"].ToString() + row["FlightNumber"].ToString() + "/" +
                                        row["ArrivalDate"].ToString().Replace("-", "").Substring(0, 5).ToUpper());
                                    }

                                    if (row["AWBPrefix"].ToString() != "" || row["AWBNumber"].ToString() != "")
                                    {
                                        if (row["ConsolidationIdentifier"].ToString() == "")
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString());
                                        }
                                        else
                                        {
                                            sb.AppendLine(row["AWBPrefix"].ToString() + "-" + row["AWBNumber"].ToString() + "-" + row["ConsolidationIdentifier"].ToString());
                                        }
                                    }
                                    if (row["ErrorCode"].ToString() != "" || row["ErrorDescription"].ToString() != "")
                                    {
                                        sb.AppendLine("ERR" + "/" + row["ErrorCode"].ToString() + row["ErrorDescription"].ToString());
                                    }

                                }
                            }

                            db = null;
                            Dset.Dispose();
                            return sb;
                        }
                        else
                        {
                            db = null;
                            Dset.Dispose();
                            return null;
                        }


                    }
                    else
                    {
                        db = null;
                        Dset.Dispose();
                        return null;
                    }
                }
                else
                {
                    db = null;
                    Dset.Dispose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                db = null;

                return null;

            }
        }
        #endregion

        #endregion

        #region Check FRX Message Validity after Offloading

        public DataSet CheckFRXValidityOffload(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[3];
                //object[] QueryValues = new object[2];
                SqlDbType[] QueryTypes = new SqlDbType[3];

                //QueryNames[0] = "AWBPrefix";
                QueryNames[0] = "AWBNumber";
                QueryNames[1] = "FlightNo";
                QueryNames[2] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                //QueryTypes[3] = SqlDbType.VarChar;


                ds = db.SelectRecords("sp_CheckFRXValidityOffload", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return ds; }
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

        #region Get Customs Messages Email ID's

        public DataSet GetCustomMessagesMailID(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = { "MessageType", "FlightNo", "FlightDate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = db.SelectRecords("sp_GetCustomsMessageEmailID", QueryNames, QueryValues, QueryTypes);
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

        #region Validating AWB for Delivery

        public bool ValidateAWBDelivery(string AWBNumber, string FlightNo, string FlightDate)
        {
            try
            {
                string[] QueryNames = { "AWBNumber", "FlightNo", "FlightDate" };
                object[] QueryValues = { AWBNumber, FlightNo, FlightDate };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                DataSet ds = db.SelectRecords("sp_ValidateAWBDeliveryCustoms", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return Convert.ToBoolean(ds.Tables[0].Rows[0]["Validate"].ToString());
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        #region Fetch Customs ACAS Data AWBWise

        public DataSet FetchCustomsACASAWBDetails(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[4];
                //object[] QueryValues = new object[2];
                SqlDbType[] QueryTypes = new SqlDbType[4];

                QueryNames[0] = "AWBPrefix";
                QueryNames[1] = "AWBNumber";
                QueryNames[2] = "FlightNo";
                QueryNames[3] = "FlightDate";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;


                ds = db.SelectRecords("SP_GetCustomsACASMessagingData", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return ds; }
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

    }

}
