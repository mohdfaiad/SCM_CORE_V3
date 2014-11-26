using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class ACASBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DataSet ds;

        #region Update PRI Message
        public bool UpdatePRIMessage(object[] QueryValues)
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
                if (db.UpdateData("sp_GeneratePRIMessage", QueryNames, QueryTypes, QueryValues))
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

        #region Update PSN Message
        public bool UpdatePSNMessage(object[] QueryValues)
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
                if (db.UpdateData("sp_GeneratePSNMessage", QueryNames, QueryTypes, QueryValues))
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

        #region Update PER Message
        public bool UpdatePERMessage(object[] QueryValues)
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
                if (db.UpdateData("sp_GeneratePERMessage", QueryNames, QueryTypes, QueryValues))
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

        #region Validate ACAS Messages
        public bool ValidateACASMsg(object[] QueryValues, string MessageType)
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
                                #region Validate PRI Messages
                                if (MessageType == "PRI")
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

                                #region Validate PSN Messages
                                if (MessageType == "PSN")
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

                                #region Validate PSN Inbox Messages
                                if (MessageType == "PSNInbox")
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

        #region Check ACAS AWB Availability

        public DataSet CheckACASAWBAvailability(object[] QueryValues)
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

        #region Encoding PRI Message
        public StringBuilder EncodingPRIMessage(object[] QueryValues)
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
                            sb.AppendLine("PRI");


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

                return null;

            }
        }
        #endregion

        #region Encoding PSN Message
        public StringBuilder EncodingPSNMessage(object[] QueryValues)
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
                                        sb.AppendLine("PSN");

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
                                        sb.AppendLine("PSN");


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

        #region Encoding PER Message
        public StringBuilder EncodingPERMessage(object[] QueryValues)
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
                            sb.AppendLine("PER");

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

        #region Trigger point for PRI 
        public string ACASFRITriggerPointValidation()
        {
            try
            {
                LoginBL objLogin = new LoginBL();
                return objLogin.GetMasterConfiguration("ACASFRITriggerPoint");
            }
            catch (Exception ex)
            { return string.Empty; }
        }
        #endregion
    }
}
