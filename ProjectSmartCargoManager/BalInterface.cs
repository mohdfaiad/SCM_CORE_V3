using System;
using System.Data;
using QID.DataAccess;
using System.Configuration;
using System.Collections.Generic;
using ProjectSmartCargoManager.ServiceReference4;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.IO;
using ProjectSmartCargoManager;
using BAL;
using System.Text;


namespace nsBalInterface
{
    public class BalInterface
    {
        #region Variable Declaration


        #endregion
        
        #region Public Methods

        public DataSet GetFlightDetails(DateTime objDate, String strFlightNo)
        {
            try
            {
                object[] QueryValues = new object[2];

                QueryValues[0] = strFlightNo;
                if (objDate.Year != 1)
                {
                    QueryValues[1] = objDate;
                    //QueryValues[1] = strFlightNo;
                }
                return fetchFlightDetails(QueryValues);

            }
            catch (Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }


       


        public DataSet GetFlightDetailsARMS(DateTime fromDate , DateTime toDate, String strFlightNo)
        {
            try
            {
                object[] QueryValues = new object[3];
                QueryValues[0] = fromDate;
                QueryValues[1] = toDate;
                QueryValues[2] = strFlightNo;

               
                return fetchFlightDetailsARMS(QueryValues);

            }
            catch (Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }


        public static string GetDateYYYMMDD(DateTime invoiceDate)
        {
    //        String dtDate = "";
            try
            {
                return invoiceDate.Year + "-" + (invoiceDate.Month > 9 ? invoiceDate.Month.ToString() : ("0" + invoiceDate.Month)) + "-" + (invoiceDate.Day > 9 ? invoiceDate.Day.ToString() : ("0" + invoiceDate.Day));
            }
            catch(Exception objEx)
            {
            }
            return null;
        }
        public CargoResponseSimple ExecuteCargoRequest(paramCargoInfo objCargoParam,DateTime Timestamp)
        {
            CargoResponseSimple response = null;
            try
            {
                CargoControlServiceClient client = new CargoControlServiceClient();
                CargoRequestSimple request = getCargoDetails( objCargoParam);

                string RequestXML = CommonUtility.GenerateSerializedXmlString(request, request.GetType(), "");

                response = client.UploadCargo(request);

                string Result = string.Empty;

                if (response.UploadMessage != null && response.UploadMessage != "")
                    Result = response.UploadMessage;
                else
                    Result = response.ErrorMessage;

                byte[] raw = Encoding.UTF8.GetBytes(RequestXML);
                Stream decoded = new MemoryStream(raw);

                string FileName = "FLYWARE_" + Timestamp.ToString("ddMMyyHHmmss") + ".xml";
                bool Uploaded = CommonUtility.UploadBlob(decoded,FileName);
                cls_BL.DumpInterfaceInformation("FLYWARE", FileName, Timestamp, "FLYWARE", Result, true, "FLYWARE", "FLYWARE",null,"");

                client.Close();
            }
            catch (Exception objex)
            {
            }
            return response;
        }

        public bool sendMail(string fromEmailId, string toEmailId, string password, string subject, string body, bool isBodyHTML, int outmailport, string CCEmailID, Stream[] Document, string[] DocumentName, string[] Extension)
        {


            #region New Code For Attached Emails Deepak
            bool flag = false;
            try
            {
                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress(fromEmailId);
                Mail.To.Add(toEmailId);
                if (CCEmailID.Length > 3)
                {
                    Mail.CC.Add(CCEmailID);
                }
                Mail.Subject = subject;
                Mail.IsBodyHtml = isBodyHTML;
                Mail.Body = body;
                SmtpClient smtp = new SmtpClient("smtp.1and1.com", outmailport);
                smtp.Credentials = new NetworkCredential(fromEmailId, password);
                Mail.Priority = MailPriority.High;
                for (int i = 0; i < Document.Length; i++)
                {
                    Mail.Attachments.Add(new Attachment(Document[i], DocumentName[i] + Extension[i]));
                }

                try
                {
                    smtp.Send(Mail);
                    flag = true;
                    clsLog.WriteLog("Mail Sent @ " + DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    clsLog.WriteLog("Exception while Sending Mail : " + ex.Message);
                    flag = true;
                }

            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Exception while collection Mail Info : " + ex.Message);
                flag = true;
            }
            return flag;
            #endregion
        }

        
        #endregion
        
        #region private Methods


        private DataSet fetchFlightDetailsARMS(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[3];
                SqlDbType[] QueryTypes = new SqlDbType[3];
                SQLServer db = new SQLServer(getConnectionString());
                DataSet objDataSet;

                QueryNames[0] = "FromDateTime";
                QueryNames[1] = "ToDateTime";
                QueryNames[2] = "FlightNo";

                QueryTypes[0] = SqlDbType.DateTime;
                QueryTypes[1] = SqlDbType.DateTime;
                QueryTypes[2] = SqlDbType.VarChar;


                objDataSet = db.SelectRecords("sp_GetFlightDetailsforARMS", QueryNames, QueryValues, QueryTypes);
                if (objDataSet != null)
                {
                    if (objDataSet.Tables.Count > 0)
                    {
                        return objDataSet;
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


        private DataSet fetchFlightDetails(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];
                SQLServer db = new SQLServer(getConnectionString());
                DataSet objDataSet;
                
                QueryNames[1] = "FlightDate";
                QueryNames[0] = "FlightNo";

               
                QueryTypes[1] = SqlDbType.DateTime;
                QueryTypes[0] = SqlDbType.VarChar;


                objDataSet = db.SelectRecords("sp_GetFlightDetailsforIAMCEB", QueryNames, QueryValues, QueryTypes);
                if (objDataSet != null)
                {
                    if (objDataSet.Tables.Count > 0)
                    {
                        return objDataSet;
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

        private static string getConnectionString()
        {
            try
            {
                string strConnectionString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                if (strConnectionString == null)
                {
                    strConnectionString = "";
                }
                return strConnectionString;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private CargoRequestSimple getCargoDetails(paramCargoInfo objParam)
        {
            CargoRequestSimple cargoRequestSimple = new CargoRequestSimple();
            try
            {
                cargoRequestSimple.AirlineIdentifier    = objParam.AirlineIdentifier ;
                cargoRequestSimple.Departure            = objParam.Departure ;
                cargoRequestSimple.Arrival              = objParam.Arrival; 
                cargoRequestSimple.FlightNumber         = objParam.FlightNumber ;
                cargoRequestSimple.User                 = objParam.User;
                cargoRequestSimple.Password             = objParam.Password;
                cargoRequestSimple.STD                  = objParam.STD;

                if (objParam.objCargoList != null && objParam.objCargoList.Count > 0)
                {                   
                   cargoRequestSimple.CargoItems =  (Cargo[])objParam.objCargoList.ToArray(typeof(Cargo));                    
                }               
            }
            catch(Exception objEx)
            {
            }
            return cargoRequestSimple;
        }


        Cargo getCargo()
        {
            try
            {                
                Cargo objCargo = new Cargo();
                objCargo.CargoType = CargoType.Pallet;
                objCargo.CheckWeight = 400;
                objCargo.Destination = "ASU";
                objCargo.Origin = "AUA";
                objCargo.LoadInfoCode = "C";
                objCargo.NrUnits = 1;
                objCargo.Remarks = "Pall1";
                objCargo.TypeCode = "PLA";
                objCargo.Identifier = "Pall1";
                objCargo.ULDNumber = "ULDPAL";
                objCargo.SpecialInfoCode = "AVI";
                objCargo.TareWeight = 123;
                return objCargo;

            }
            catch(Exception objex)
            {
            }
            return null;
        }

        #endregion
        
        public DataSet fetchISAPInvoiceDetails(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[1];
                SqlDbType[] QueryTypes = new SqlDbType[1];
                SQLServer db = new SQLServer(getConnectionString());
                DataSet objDataSet;
                QueryNames[0] = "invoiceDate";
                QueryTypes[0] = SqlDbType.DateTime;
                objDataSet = db.SelectRecords("sp_getSAPInvoiceDetails", QueryNames, QueryValues, QueryTypes);

                if (objDataSet != null)
                {
                    return objDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            { return null; }
        }

        public DataSet fetchISAPManifestDetails(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[1];
                SqlDbType[] QueryTypes = new SqlDbType[1];
                SQLServer db = new SQLServer(getConnectionString());
                DataSet objDataSet;
                QueryNames[0] = "manifestDate";
                QueryTypes[0] = SqlDbType.DateTime;
                objDataSet = db.SelectRecords("sp_getSAPManifestDetails", QueryNames, QueryValues, QueryTypes);

                if (objDataSet != null)
                {
                    return objDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            { return null; }
        }

        public DataSet fetchISAPCollectionDetails(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[1];
                SqlDbType[] QueryTypes = new SqlDbType[1];
                SQLServer db = new SQLServer(getConnectionString());
                DataSet objDataSet;
                QueryNames[0] = "entryDate";
                QueryTypes[0] = SqlDbType.DateTime;
                objDataSet = db.SelectRecords("sp_getSAPCollectionDetails", QueryNames, QueryValues, QueryTypes);

                if (objDataSet != null)
                {
                    return objDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            { return null; }
        }

        public DataSet fetchFlywareBodyType(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[3];
                SqlDbType[] QueryTypes = new SqlDbType[3];
                SQLServer db = new SQLServer(getConnectionString());
                DataSet objDataSet;

                QueryNames[0] = "FlightNo";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "FlightDate";
                QueryTypes[1] = SqlDbType.DateTime;

                QueryNames[2] = "Origin";
                QueryTypes[2] = SqlDbType.VarChar;

                objDataSet = db.SelectRecords("sp_getFlywareBodyType_V1", QueryNames, QueryValues, QueryTypes);

                if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0 && objDataSet.Tables[0].Rows[0]["DepTime"].ToString() != "")
                {
                    return objDataSet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            { return null; }
        }

        public string FetchEmailForLoadControl(string Station)
        {
            DataSet objDataSet = null;

            try
            {
                string strQuery = "SELECT GHAEmailID FROM dbo.AirportMaster WHERE AirportCode = '" + Station + "' AND IsActive = 1";
                SQLServer db = new SQLServer(getConnectionString());                

                objDataSet = db.GetDataset(strQuery);

                if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToString(objDataSet.Tables[0].Rows[0][0]);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            finally
            {
                objDataSet = null;
            }
        }
        public void UpdateNavitireData(object[] QueryValues)
        {
            try
            {
                string[] QueryNames = new string[16];
                SqlDbType[] QueryTypes = new SqlDbType[16];
                SQLServer db = new SQLServer(getConnectionString());
                DataSet objDataSet;

                QueryNames[0] = "FltNo";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "CarrierCode";
                QueryTypes[1] = SqlDbType.VarChar ;

                QueryNames[2] = "DepStation";
                QueryTypes[2] = SqlDbType.VarChar;

                QueryNames[3] = "ArrStation";
                QueryTypes[3] = SqlDbType.VarChar ;

                QueryNames[4] = "FlightDate";
                QueryTypes[4] = SqlDbType.Date ;

                QueryNames[5] = "BxPxCt";
                QueryTypes[5] = SqlDbType.Int ;

                QueryNames[6] = "BxPxWt";
                QueryTypes[6] = SqlDbType.Float ;

                QueryNames[7] = "CxPxCt";
                QueryTypes[7] = SqlDbType.Int ;

                QueryNames[8] = "CxPxWt";
                QueryTypes[8] = SqlDbType.Float ;

                QueryNames[9] = "CxPxbCt";
                QueryTypes[9] = SqlDbType.Int ;

                QueryNames[10] = "CxPxbWt";
                QueryTypes[10] = SqlDbType.Float;

                QueryNames[11] = "FltStatus";
                QueryTypes[11] = SqlDbType.VarChar ;

                QueryNames[12] = "FltDepInfo";
                QueryTypes[12] = SqlDbType.Date ;

                QueryNames[13] = "UpdCarCap";
                QueryTypes[13] = SqlDbType.Float;

                QueryNames[14] = "UpdatedBy";
                QueryTypes[14] = SqlDbType.VarChar;

                QueryNames[15] = "UpdatedOn";
                QueryTypes[15] = SqlDbType.DateTime;

                db.SelectRecords("SP_NavitireUpdateManifest", QueryNames, QueryValues, QueryTypes);
                db.SelectRecords("SP_NavitireUpdateFlightPaxLoad", QueryNames, QueryValues, QueryTypes);
                
            }
            catch (Exception ex)
            { }
        }

        internal DataSet RaedPaxCountWeight(object[] objQueryVal)
        {
            try
            {
                string[] QueryNames = new string[5];
                SqlDbType[] QueryTypes = new SqlDbType[5];
                SQLServer db = new SQLServer(getConnectionString());              

                QueryNames[0] = "Origin";
                QueryTypes[0] = SqlDbType.VarChar;

                QueryNames[1] = "Dest";
                QueryTypes[1] = SqlDbType.VarChar;

                QueryNames[2] = "CarierCode";
                QueryTypes[2] = SqlDbType.VarChar;

                QueryNames[3] = "fltId";
                QueryTypes[3] = SqlDbType.VarChar;

                QueryNames[4] = "dtFltDate";
                QueryTypes[4] = SqlDbType.Date;

                return db.SelectRecords("sp_NavitireGetPaxCapacity", QueryNames, objQueryVal, QueryTypes);              

            }
            catch (Exception ex)
            { }
            return null;
        }
    }

    public class paramCargoInfo
    {
        public String AirlineIdentifier = "5J";
        public String Departure         = "AUA";
        public String Arrival           = "ASU";
        public String FlightNumber      = "CTEST";
        public String User              = "cebu_agent";
        public String Password          = "1206tempauth";
        public DateTime STD             = new DateTime(2013, 9, 13, 11, 0, 09);
        public ArrayList objCargoList   = new ArrayList();
    }
}