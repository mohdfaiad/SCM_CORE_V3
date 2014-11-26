using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class EpouchBAL
    {
        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion

        # region Updating Flight Document Details to DB
        public bool UpdateFlightDocumentDetails(string FlightNo, string FlightDate, string DocumentName, string UploadedBy, string DocumentNo, string FlightsExtension, byte[] Document, string DocumentFileName)
        {
            try
            {
                bool flag = false;
                string[] QueryNames = new string[7];
                object[] QueryValues = new object[7];
                SqlDbType[] QueryTypes = new SqlDbType[7];

                QueryNames[0] = "FlightNo";
                QueryNames[1] = "DocumentName";
                QueryNames[2] = "UploadedBy";
                QueryNames[3] = "DocumentNo";
                QueryNames[4] = "FlightsExtension";
                QueryNames[5] = "Document";
                QueryNames[6] = "FileName";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarBinary;
                QueryTypes[6] = SqlDbType.VarChar;


                QueryValues[0] = FlightNo;
                QueryValues[1] = DocumentName;
                QueryValues[2] = UploadedBy;
                QueryValues[3] = DocumentNo;
                QueryValues[4] = FlightsExtension;
                QueryValues[5] = Document;
                QueryValues[6] = DocumentFileName;

                flag = db.InsertData("SP_InsertUploadedDocuments", QueryNames, QueryTypes, QueryValues);
                return flag;

            }
            catch (Exception ex)
            { }
            return false;
        }
        #endregion

        #region Get Epouch Count for AWB
        public string GetEpouchAWBCount(string AWBPrefix, string AWBNumber)
        {
            try
            {
                string[] QueryNames = { "AWBPrefix", "AWBNumber" };
                object[] QueryValues = { AWBPrefix, AWBNumber };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = db.SelectRecords("sp_GetAWBePouchCount", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds.Tables[0].Rows[0]["Count"].ToString();
                        }
                        else
                            return "0";
                    }
                    else
                        return "0";
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        #endregion

        #region Get Epouch Count for Flights
        public string GetEpouchFlightsCount(string FlightNo, string FlightDate)
        {
            try
            {
                string[] QueryNames = { "FlightNo", "FlightDate" };
                object[] QueryValues = { FlightNo, FlightDate };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = db.SelectRecords("sp_GetFlightePouchCount", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds.Tables[0].Rows[0]["Count"].ToString();
                        }
                        else
                            return "0";
                    }
                    else
                        return "0";
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        #endregion

        #region Saving Document Type
        public bool SaveAWBEpouchDocumentType(string DocumentType,string UpdatedBy,DateTime UpdatedOn)
        {
            try
            {
                string[] QueryNames = { "DocumentType", "UpdatedBy","UpdatedOn" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.DateTime };
                object[] QueryValues = { DocumentType, UpdatedBy, UpdatedOn };
                if (db.UpdateData("sp_AddUpdateDocumentType", QueryNames, QueryTypes, QueryValues))
                { return true; }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Saving Document Type
        public bool SaveEpouchFlightsDocumentType(string DocumentType, string UpdatedBy, DateTime UpdatedOn)
        {
            try
            {
                string[] QueryNames = { "DocumentType", "UpdatedBy", "UpdatedOn" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
                object[] QueryValues = { DocumentType, UpdatedBy, UpdatedOn };
                if (db.UpdateData("sp_AddUpdateFlightDocumentType", QueryNames, QueryTypes, QueryValues))
                { return true; }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
