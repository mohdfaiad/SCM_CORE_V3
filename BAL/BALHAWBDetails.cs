using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;
using System.Configuration;

namespace BAL
{
    public class BALHAWBDetails
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion

        #region GetHAWBDetails
        public DataSet GetHAWBDetails(string MAWB,string MAWBPrefix)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            string[] paramname = new string[2];
            paramname[0] = "MAWBNo";
            paramname[1] = "MAWBPrefix";
            object[] paramvalue = new object[2];
            paramvalue[0] = MAWB;
            paramvalue[1] = MAWBPrefix;

            SqlDbType[] paramtype = new SqlDbType[2];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            ds = da.SelectRecords("SP_GetHAWBDetails", paramname, paramvalue, paramtype);
            return ds;
        }
        public bool PutHAWBDetails(string MAWBNo, string HAWBNo, int HAWBPcs, float HAWBWt, string Description, string CustID, string CustName,
          string CustAddress, string CustCity, string Zipcode, string Origin, string Destination, string SHC,
          string HAWBPrefix, string AWBPrefix, string FltOrigin, string FltDest, string ArrivalStatus, string FlightNo,
          string FlightDt)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);

            string[] paramname = new string[20];
            paramname[0] = "MAWBNo";
            paramname[1] = "HAWBNo";
            paramname[2] = "HAWBPcs";
            paramname[3] = "HAWBWt";
            paramname[4] = "Description";
            paramname[5] = "CustID";
            paramname[6] = "CustName";
            paramname[7] = "CustAddress";
            paramname[8] = "CustCity";
            paramname[9] = "Zipcode";
            paramname[10] = "Origin";
            paramname[11] = "Destination";
            paramname[12] = "SHC";
            paramname[13] = "HAWBPrefix";
            paramname[14] = "AWBPrefix";
            paramname[15] = "ArrivalStatus";
            paramname[16] = "FlightNo";
            paramname[17] = "FlightDt";
            paramname[18] = "FlightOrigin";
            paramname[19] = "flightDest";


            object[] paramvalue = new object[20];
            paramvalue[0] = MAWBNo;
            paramvalue[1] = HAWBNo;
            paramvalue[2] = HAWBPcs;
            paramvalue[3] = HAWBWt;
            paramvalue[4] = Description;
            paramvalue[5] = CustID;
            paramvalue[6] = CustName;
            paramvalue[7] = CustAddress;
            paramvalue[8] = CustCity;
            paramvalue[9] = Zipcode;
            paramvalue[10] = Origin;
            paramvalue[11] = Destination;
            paramvalue[12] = SHC;
            paramvalue[13] = HAWBPrefix;
            paramvalue[14] = AWBPrefix;
            paramvalue[15] = ArrivalStatus;
            paramvalue[16] = FlightNo;
            if (FlightDt == "")
            {
                FlightDt = DateTime.Now.ToString();
            }
            else
            {
                paramvalue[17] = FlightDt;
            }
            paramvalue[18] = FltOrigin;
            paramvalue[19] = FltDest;

            SqlDbType[] paramtype = new SqlDbType[20];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            paramtype[2] = SqlDbType.Int;
            paramtype[3] = SqlDbType.Int;
            paramtype[4] = SqlDbType.VarChar;
            paramtype[5] = SqlDbType.VarChar;
            paramtype[6] = SqlDbType.VarChar;
            paramtype[7] = SqlDbType.VarChar;
            paramtype[8] = SqlDbType.VarChar;
            paramtype[9] = SqlDbType.VarChar;
            paramtype[10] = SqlDbType.VarChar;
            paramtype[11] = SqlDbType.VarChar;
            paramtype[12] = SqlDbType.VarChar;
            paramtype[13] = SqlDbType.VarChar;
            paramtype[14] = SqlDbType.VarChar;
            paramtype[15] = SqlDbType.VarChar;
            paramtype[16] = SqlDbType.VarChar;
            paramtype[17] = SqlDbType.DateTime;
            paramtype[18] = SqlDbType.VarChar;
            paramtype[19] = SqlDbType.VarChar;

            if (da.ExecuteProcedure("SP_PutHAWBDetails", paramname, paramtype, paramvalue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PutHAWBDetails(string MAWBNo,string HAWBNo,int HAWBPcs,float HAWBWt, string Description,string CustID,string CustName,
            string CustAddress,string CustCity, string Zipcode, string Origin, string Destination, string SHC,
            string HAWBPrefix,string AWBPrefix,string FltOrigin,string FltDest, string ArrivalStatus, string FlightNo,
            string FlightDt,string ConsigneeName,string ConsigneeAddress,string ConsigneeCity,string ConsigneeState,string ConsigneeCountry,string ConsigneePostalCode,
            string CustState,string CustCountry,string UOM, string SLAC)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            
            string[] paramname = new string[30];
            paramname[0] = "MAWBNo";
            paramname[1] = "HAWBNo";
            paramname[2] = "HAWBPcs";
            paramname[3] = "HAWBWt";
            paramname[4] = "Description";
            paramname[5] = "CustID";
            paramname[6] = "CustName";
            paramname[7] = "CustAddress";
            paramname[8] = "CustCity";
            paramname[9] = "Zipcode";
            paramname[10] = "Origin";
            paramname[11] = "Destination";
            paramname[12] = "SHC";
            paramname[13] = "HAWBPrefix";
            paramname[14] = "AWBPrefix";
            paramname[15] = "ArrivalStatus";
            paramname[16] = "FlightNo";
            paramname[17] = "FlightDt";
            paramname[18] = "FlightOrigin";
            paramname[19] = "flightDest";
            paramname[20] = "ConsigneeName";
            paramname[21] = "ConsigneeAddress";
            paramname[22] = "ConsigneeCity";
            paramname[23] = "ConsigneeState";
            paramname[24] = "ConsigneeCountry";
            paramname[25] = "ConsigneePostalCode";
            paramname[26] = "CustState";
            paramname[27] = "CustCountry";
            paramname[28] = "UOM";
            paramname[29] = "SLAC";


            object[] paramvalue = new object[30];
            paramvalue[0] = MAWBNo;
            paramvalue[1] = HAWBNo;
            paramvalue[2] = HAWBPcs;
            paramvalue[3] = HAWBWt;
            paramvalue[4] = Description;
            paramvalue[5] = CustID;
            paramvalue[6] = CustName;
            paramvalue[7] = CustAddress;
            paramvalue[8] = CustCity;
            paramvalue[9] = Zipcode;
            paramvalue[10] = Origin;
            paramvalue[11] = Destination;
            paramvalue[12] = SHC;
            paramvalue[13] = HAWBPrefix;
            paramvalue[14] = AWBPrefix;
            paramvalue[15] = ArrivalStatus;
            paramvalue[16] = FlightNo;
            if (FlightDt == "")
            {
                FlightDt = DateTime.Now.ToString();
            }
            else
            {
                paramvalue[17] = FlightDt;
            }
            paramvalue[18] = FltOrigin;
            paramvalue[19] = FltDest;
            paramvalue[20] = ConsigneeName;
            paramvalue[21] = ConsigneeAddress;
            paramvalue[22] = ConsigneeCity;
            paramvalue[23] = ConsigneeState;
            paramvalue[24] = ConsigneeCountry;
            paramvalue[25] = ConsigneePostalCode;
            paramvalue[26] = CustState;
            paramvalue[27] = CustCountry;
            paramvalue[28] = UOM;
            paramvalue[29] = SLAC != string.Empty ? SLAC : "0";


            SqlDbType[] paramtype = new SqlDbType[30];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            paramtype[2] = SqlDbType.Int;
            paramtype[3] = SqlDbType.Int;
            paramtype[4] = SqlDbType.VarChar;
            paramtype[5] = SqlDbType.VarChar;
            paramtype[6] = SqlDbType.VarChar;
            paramtype[7] = SqlDbType.VarChar;
            paramtype[8] = SqlDbType.VarChar;
            paramtype[9] = SqlDbType.VarChar;
            paramtype[10] = SqlDbType.VarChar;
            paramtype[11] = SqlDbType.VarChar;
            paramtype[12] = SqlDbType.VarChar;
            paramtype[13] = SqlDbType.VarChar;
            paramtype[14] = SqlDbType.VarChar;
            paramtype[15] = SqlDbType.VarChar;
            paramtype[16] = SqlDbType.VarChar;
            paramtype[17] = SqlDbType.DateTime;
            paramtype[18] = SqlDbType.VarChar;
            paramtype[19] = SqlDbType.VarChar;
            paramtype[20] = SqlDbType.VarChar;
            paramtype[21] = SqlDbType.VarChar;
            paramtype[22] = SqlDbType.VarChar;
            paramtype[23] = SqlDbType.VarChar;
            paramtype[24] = SqlDbType.VarChar;
            paramtype[25] = SqlDbType.VarChar;
            paramtype[26] = SqlDbType.VarChar;
            paramtype[27] = SqlDbType.VarChar;
            paramtype[28] = SqlDbType.VarChar;
            paramtype[29] = SqlDbType.Int;

            if (da.ExecuteProcedure("SP_PutHAWBDetails_V2", paramname, paramtype, paramvalue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region HAWB Details Save
        public bool PutHAWBDetails(string MAWBNo, string HAWBNo, int HAWBPcs, float HAWBWt, string Description, string CustID, string CustName,
string CustAddress, string CustCity, string Zipcode, string Origin, string Destination, string SHC,
string HAWBPrefix, string AWBPrefix, string FltOrigin, string FltDest, string ArrivalStatus, string FlightNo,
string FlightDt, string ConsigneeName, string ConsigneeAddress, string ConsigneeCity, string ConsigneeState, string ConsigneeCountry, string ConsigneePostalCode,
string CustState, string CustCountry, string UOM, string SLAC, string ConsigneeID, string ShipperEmail, string ShipperTelephone, string ConsigneeEmail, string ConsigneeTelephone)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);

            string[] paramname = new string[35];
            paramname[0] = "MAWBNo";
            paramname[1] = "HAWBNo";
            paramname[2] = "HAWBPcs";
            paramname[3] = "HAWBWt";
            paramname[4] = "Description";
            paramname[5] = "CustID";
            paramname[6] = "CustName";
            paramname[7] = "CustAddress";
            paramname[8] = "CustCity";
            paramname[9] = "Zipcode";
            paramname[10] = "Origin";
            paramname[11] = "Destination";
            paramname[12] = "SHC";
            paramname[13] = "HAWBPrefix";
            paramname[14] = "AWBPrefix";
            paramname[15] = "ArrivalStatus";
            paramname[16] = "FlightNo";
            paramname[17] = "FlightDt";
            paramname[18] = "FlightOrigin";
            paramname[19] = "flightDest";
            paramname[20] = "ConsigneeName";
            paramname[21] = "ConsigneeAddress";
            paramname[22] = "ConsigneeCity";
            paramname[23] = "ConsigneeState";
            paramname[24] = "ConsigneeCountry";
            paramname[25] = "ConsigneePostalCode";
            paramname[26] = "CustState";
            paramname[27] = "CustCountry";
            paramname[28] = "UOM";
            paramname[29] = "SLAC";
            paramname[30] = "ConsigneeID";
            paramname[31] = "ShipperEmail";
            paramname[32] = "ShipperTelephone";
            paramname[33] = "ConsigneeEmail";
            paramname[34] = "ConsigneeTelephone";


            object[] paramvalue = new object[35];
            paramvalue[0] = MAWBNo;
            paramvalue[1] = HAWBNo;
            paramvalue[2] = HAWBPcs;
            paramvalue[3] = HAWBWt;
            paramvalue[4] = Description;
            paramvalue[5] = CustID;
            paramvalue[6] = CustName;
            paramvalue[7] = CustAddress;
            paramvalue[8] = CustCity;
            paramvalue[9] = Zipcode;
            paramvalue[10] = Origin;
            paramvalue[11] = Destination;
            paramvalue[12] = SHC;
            paramvalue[13] = HAWBPrefix;
            paramvalue[14] = AWBPrefix;
            paramvalue[15] = ArrivalStatus;
            paramvalue[16] = FlightNo;
            if (FlightDt == "")
            {
                FlightDt = DateTime.Now.ToString();
            }
            else
            {
                paramvalue[17] = FlightDt;
            }
            paramvalue[18] = FltOrigin;
            paramvalue[19] = FltDest;
            paramvalue[20] = ConsigneeName;
            paramvalue[21] = ConsigneeAddress;
            paramvalue[22] = ConsigneeCity;
            paramvalue[23] = ConsigneeState;
            paramvalue[24] = ConsigneeCountry;
            paramvalue[25] = ConsigneePostalCode;
            paramvalue[26] = CustState;
            paramvalue[27] = CustCountry;
            paramvalue[28] = UOM;
            paramvalue[29] = SLAC != string.Empty ? SLAC : "0";
            paramvalue[30] = ConsigneeID;
            paramvalue[31] = ShipperEmail;
            paramvalue[32] = ShipperTelephone;
            paramvalue[33] = ConsigneeEmail;
            paramvalue[34] = ConsigneeTelephone;


            SqlDbType[] paramtype = new SqlDbType[35];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            paramtype[2] = SqlDbType.Int;
            paramtype[3] = SqlDbType.Int;
            paramtype[4] = SqlDbType.VarChar;
            paramtype[5] = SqlDbType.VarChar;
            paramtype[6] = SqlDbType.VarChar;
            paramtype[7] = SqlDbType.VarChar;
            paramtype[8] = SqlDbType.VarChar;
            paramtype[9] = SqlDbType.VarChar;
            paramtype[10] = SqlDbType.VarChar;
            paramtype[11] = SqlDbType.VarChar;
            paramtype[12] = SqlDbType.VarChar;
            paramtype[13] = SqlDbType.VarChar;
            paramtype[14] = SqlDbType.VarChar;
            paramtype[15] = SqlDbType.VarChar;
            paramtype[16] = SqlDbType.VarChar;
            paramtype[17] = SqlDbType.DateTime;
            paramtype[18] = SqlDbType.VarChar;
            paramtype[19] = SqlDbType.VarChar;
            paramtype[20] = SqlDbType.VarChar;
            paramtype[21] = SqlDbType.VarChar;
            paramtype[22] = SqlDbType.VarChar;
            paramtype[23] = SqlDbType.VarChar;
            paramtype[24] = SqlDbType.VarChar;
            paramtype[25] = SqlDbType.VarChar;
            paramtype[26] = SqlDbType.VarChar;
            paramtype[27] = SqlDbType.VarChar;
            paramtype[28] = SqlDbType.VarChar;
            paramtype[29] = SqlDbType.Int;
            paramtype[30] = SqlDbType.VarChar;
            paramtype[31] = SqlDbType.VarChar;
            paramtype[32] = SqlDbType.VarChar;
            paramtype[33] = SqlDbType.VarChar;
            paramtype[34] = SqlDbType.VarChar;

            if (da.ExecuteProcedure("SP_PutHAWBDetails_V2", paramname, paramtype, paramvalue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        public bool UpdateHAWBArrival(string HAWBPrefix, string HAWBNumber, string AWBNumber, string ArrivalStatus,
            string UserName, string UpdatedOn)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] paramname = new string[6];
                paramname[0] = "AWBNumber";
                paramname[1] = "HAWBPrefix";
                paramname[2] = "HAWBNumber";
                paramname[3] = "ArrivalStatus";
                paramname[4] = "UserName";
                paramname[5] = "TimeStamp";

                object[] paramvalue = new object[6];
                paramvalue[0] = AWBNumber;
                paramvalue[1] = HAWBPrefix;
                paramvalue[2] = HAWBNumber;
                paramvalue[3] = ArrivalStatus;
                paramvalue[4] = UserName;
                paramvalue[5] = UpdatedOn;

                SqlDbType[] paramtype = new SqlDbType[6];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.DateTime;

                if (da.ExecuteProcedure("sp_UpdateHAWBArrivalStatus", paramname, paramtype, paramvalue))
                {
                    return true;
                }
                else
                {
                    return false;
                }  
            }
            catch (Exception)
            {
            }
            return (false);
        }
        #endregion
        
        #region GetHAWBDetailsArrival
        public DataSet GetHAWBDetailsArrival(string MAWB, string MAWBPrefix, string FltNo,
            string FltDt, string FltOrig, string FltDest)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            string[] paramname = new string[6];
            paramname[0] = "MAWBNo";
            paramname[1] = "MAWBPrefix";
            paramname[2] = "FltNo";
            paramname[3] = "FltDt";
            paramname[4] = "FltOrig";
            paramname[5] = "FltDest";

            object[] paramvalue = new object[6];
            paramvalue[0] = MAWB;
            paramvalue[1] = MAWBPrefix;
            paramvalue[2] = FltNo;
            paramvalue[3] = FltDt;
            paramvalue[4] = FltOrig;
            paramvalue[5] = FltDest;

            SqlDbType[] paramtype = new SqlDbType[6];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            paramtype[2] = SqlDbType.VarChar;
            paramtype[3] = SqlDbType.VarChar;
            paramtype[4] = SqlDbType.VarChar;
            paramtype[5] = SqlDbType.VarChar;
            ds = da.SelectRecords("SP_GetHAWBDetailsArrival", paramname, paramvalue, paramtype);
            return ds;
        }

        #endregion

        #region GetHAWBDetailsBooking
        public DataSet GetHAWBDetailsBooking(string MAWB)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            string[] paramname = new string[1];
            paramname[0] = "MAWBNo";
            object[] paramvalue = new object[1];
            paramvalue[0] = MAWB;
            SqlDbType[] paramtype = new SqlDbType[1];
            paramtype[0] = SqlDbType.VarChar;
            ds = da.SelectRecords("SP_GetHAWBDetailsBooking", paramname, paramvalue, paramtype);
            return ds;
        }

        public bool PutHAWBDetailsBooking(string MAWBNo, string HAWBNo, int HAWBPcs, int HAWBWt, string CustID, string CustName, string CustAddress, string CustCity, string Zipcode)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            string[] paramname = new string[9];
            paramname[0] = "MAWBNo";
            paramname[1] = "HAWBNo";
            paramname[2] = "HAWBPcs";
            paramname[3] = "HAWBWt";
            paramname[4] = "CustID";
            paramname[5] = "CustName";
            paramname[6] = "CustAddress";
            paramname[7] = "CustCity";
            paramname[8] = "Zipcode";

            object[] paramvalue = new object[9];
            paramvalue[0] = MAWBNo;
            paramvalue[1] = HAWBNo;
            paramvalue[2] = HAWBPcs;
            paramvalue[3] = HAWBWt;
            paramvalue[4] = CustID;
            paramvalue[5] = CustName;
            paramvalue[6] = CustAddress;
            paramvalue[7] = CustCity;
            paramvalue[8] = Zipcode;

            SqlDbType[] paramtype = new SqlDbType[9];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            paramtype[2] = SqlDbType.Int;
            paramtype[3] = SqlDbType.Int;
            paramtype[4] = SqlDbType.VarChar;
            paramtype[5] = SqlDbType.VarChar;
            paramtype[6] = SqlDbType.VarChar;
            paramtype[7] = SqlDbType.VarChar;
            paramtype[8] = SqlDbType.VarChar;

            if (da.ExecuteProcedure("SP_PutHAWBDetails", paramname, paramtype, paramvalue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region DeleteAllMAWBHABW 
        public bool Delete_All_MAWB_HAWB(string MAWBNo,string AWBPrefix)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(constr);
            string[] paramname = new string[2];
            paramname[0] = "MAWBNo";
            paramname[1] = "AWBPrefix";
            object[] paramvalue = new object[2];
            paramvalue[0] = MAWBNo;
            paramvalue[1] = AWBPrefix;
            SqlDbType[] paramtype = new SqlDbType[2];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            if (da.ExecuteProcedure("SP_DeleteAll_MAWB_HAWB", paramname, paramtype, paramvalue))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Check HAWB Availability
        public bool CheckHAWBValidity(string AWBPrefix, string AWBNumber, string HAWBPrefix, string HAWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryNames = { "AWBPrefix", "AWBNumber", "HAWBPrefix", "HAWBNumber" };
                object[] QueryValues = { AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                DataSet ds = da.SelectRecords("sp_CheckHAWBValidity", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return true;
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

        #region Save HAWB Documents
        public bool SaveHAWBDocuments(string AWBPrefix, string AWBNumber, string HAWBPrefix, string HAWBNumber, string DocumentName, string DocumentExtension, byte[] Document,string UpdatedBy,DateTime UpdatedOn)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryNames = { "AWBPrefix", "AWBNumber", "HAWBPrefix", "HAWBNumber", "DocumentName", "DocumentExtension", "Document", "UpdatedBy", "UpdatedOn" };
                object[] QueryValues = { AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber, DocumentName, DocumentExtension, Document, UpdatedBy, UpdatedOn };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarBinary, SqlDbType.VarChar, SqlDbType.DateTime };

                return da.UpdateData("sp_UploadHAWBDocuments", QueryNames, QueryTypes, QueryValues);
            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        #region Download HAWB Documents
        public DataSet DownloadHAWBDocument(string AWBPrefix, string AWBNumber, string HAWBPrefix, string HAWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryNames = { "AWBPrefix", "AWBNumber", "HAWBPrefix", "HAWBNumber" };
                object[] QueryValues = { AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                DataSet ds = da.SelectRecords("sp_DownloadHAWBDocuments", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }
                else
                    return null;

            }
            catch (Exception ex)
            { return null; }
        }
        #endregion
    }
}
