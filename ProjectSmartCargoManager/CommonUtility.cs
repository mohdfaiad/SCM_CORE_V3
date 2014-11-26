using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace ProjectSmartCargoManager
{
    public class CommonUtility
    {

        #region "Global Variables"

        private static DataSet _dsPartnerTypeMaster = null;
        private static DataSet _dsPartnerMaster = null;
        private static DataSet _dsAWBPrefixMaster = null;
        private static string _strSmartKargoInstance = null;
        private static bool _blnFlightValidation = false;
        private static string _strShipperMandatoryDuring = null;
        private static string _strConsigneeMandatoryDuring = null;
        private static string _strValidateCCSFandIAC = string.Empty;
        private static bool _blnShowOperationTimeOnSave = false;
        private static bool _blnCombineSaveandExecute = false;
        private static string _strHideRatesAcceptance = null;
        private static string _strEmbargoFunctionality = null;
        private static bool _UseCookiesForValidation = false;

        #endregion

        public static Color ColorHighlightedGrid = Color.LightBlue;

        public static MemoryStream GetImageStream(HttpServerUtility Request)
        {
            string Path = Request.MapPath ("/Reports/ClientLogo.png");
            System.Net.WebClient client = new System.Net.WebClient();
            byte[] imageData = client.DownloadData(Path);
            System.IO.MemoryStream Logo = new System.IO.MemoryStream(imageData);

            return Logo;
        }
        public static MemoryStream getPartnerImage(HttpServerUtility Request)
        {
           
            string Path = Request.MapPath("/Reports/ClientLogo1.png");
            System.Net.WebClient client = new System.Net.WebClient();
            byte[] imageData = client.DownloadData(Path);
            System.IO.MemoryStream partnerlogo = new System.IO.MemoryStream(imageData);
            return partnerlogo;
        }

        public static MemoryStream GetImageStream(HttpServerUtility Request,string ClientPrefix)
        {
            byte[] imageData = null;
            System.IO.MemoryStream Logo = null;
            string Path = Request.MapPath("/Images/Partners/" + ClientPrefix + ".png");
            System.Net.WebClient client = new System.Net.WebClient();
            try
            {
                imageData = client.DownloadData(Path);
                Logo = new System.IO.MemoryStream(imageData);
            }
            catch
            {
                Logo = GetImageStream(Request);
            }           

            return Logo;
        }

        public static MemoryStream GetImageStream()
        {
            SqlConnection Conn = new SqlConnection(Global.GetConnectionString());
            SqlDataAdapter DA = new SqlDataAdapter("Select top 1 logo from ClientMaster", Conn);
            DataSet DS = new DataSet();
            DA.Fill(DS);

            Byte[] ImgBytes = (Byte[])DS.Tables[0].Rows[0][0];

            MemoryStream ms = new MemoryStream(ImgBytes, 0, ImgBytes.Length);
            Image img = Image.FromStream(ms);
            MemoryStream msMain = new MemoryStream();
            img.Save(msMain, System.Drawing.Imaging.ImageFormat.Png);

            return ms;
        }

        #region Get Months List
        /// <summary>
        /// Gets 3 char list of Months in string list.
        /// </summary>
        /// <returns>List of Strings.</returns>
        public static List<String> GetMonthsList()
        {
            List<String> lstMonths = new List<string>();
            try
            {
                lstMonths.Add("");
                lstMonths.Add("JAN");
                lstMonths.Add("FEB");
                lstMonths.Add("MAR");
                lstMonths.Add("APR");
                lstMonths.Add("MAY");
                lstMonths.Add("JUN");
                lstMonths.Add("JUL");
                lstMonths.Add("AUG");
                lstMonths.Add("SEP");
                lstMonths.Add("OCT");
                lstMonths.Add("NOV");
                lstMonths.Add("DEC");
            }
            catch (Exception)
            {
            }
            return (lstMonths);
        }
        #endregion Get Months List

        #region Get Day Of Week List
        /// <summary>
        /// Gets 3 char list of Days of Week in string list.
        /// </summary>
        /// <returns>List of Strings.</returns>
        public static List<String> GetDayOfWeekList()
        {
            List<String> lstDayOfWeek = new List<string>();
            try
            {
                lstDayOfWeek.Add("");
                lstDayOfWeek.Add("MON");
                lstDayOfWeek.Add("TUE");
                lstDayOfWeek.Add("WED");
                lstDayOfWeek.Add("THU");
                lstDayOfWeek.Add("FRI");
                lstDayOfWeek.Add("SAT");
                lstDayOfWeek.Add("SUN");
            }
            catch (Exception)
            {
            }
            return (lstDayOfWeek);
        }
        #endregion Get Day Of Week List

        public static string GetConfigurationValue(string ConfigXML, string Key)
        {
            string strValue = string.Empty;
            System.Xml.XmlDocument objDoc = new System.Xml.XmlDocument();

            try
            {
                objDoc.LoadXml(ConfigXML);

                for (int intCount = 0; intCount < objDoc.GetElementsByTagName("Parameter").Count; intCount++)
                {
                    if ((objDoc.GetElementsByTagName("Parameter")[intCount]).InnerText.ToUpper() == Key.ToUpper())
                    {
                        strValue = (objDoc.GetElementsByTagName("Value")[intCount]).InnerText;
                        break;
                    }
                }
            }
            catch { }
            finally
            {
                objDoc = null;
            }

            return strValue;
        }

        public static string GenerateSerializedXmlString(Object pi_objObjectInstance, Type pi_objObjectType, string pi_strDefaultNameSpace)
        {
            System.Xml.Serialization.XmlSerializer objSerializer = new System.Xml.Serialization.XmlSerializer(pi_objObjectType, pi_strDefaultNameSpace);
            System.IO.StringWriter objStringWriter = new System.IO.StringWriter();
            string strReturnValue;
            objSerializer.Serialize(objStringWriter, pi_objObjectInstance);
            strReturnValue = objStringWriter.ToString();
            objStringWriter.Close();
            return strReturnValue;
        }

        #region "Upload Or Download from Blob Storage"

        public static bool UploadBlob(Stream stream, string fileName)
        {
            try
            {
                string containerName = "blobstorage"; //container must be lowercase, no special characters
                
                StorageCredentialsAccountAndKey cred = new StorageCredentialsAccountAndKey("qidstorage", "NUro8/C7+kMqtwOwLbe6agUvA83s+8xSTBqrkMwSjPP6MAxVkdtsLDGjyfyEqQIPv6JHEEf5F5s4a+DFPsSQfg==");
                CloudStorageAccount storageAccount = new CloudStorageAccount(cred, false);
                CloudBlobClient blobClient = new CloudBlobClient(storageAccount.BlobEndpoint.AbsoluteUri, cred);
                CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
                blobContainer.CreateIfNotExist();
                CloudBlob blob = blobContainer.GetBlobReference(fileName);
                blob.Properties.ContentType = "";//Request.Files[0].ContentType;
                blob.UploadFromStream(stream);
                return true;
            }
            catch (Exception ex)
            { return false; }

        }

        public static byte[] DownloadBlob(string filename)
        {
            string containerName = "blobstorage"; //container must be lowercase, no special characters
            byte[] downloadStream = null;
            StorageCredentialsAccountAndKey cred = new StorageCredentialsAccountAndKey("qidstorage", "NUro8/C7+kMqtwOwLbe6agUvA83s+8xSTBqrkMwSjPP6MAxVkdtsLDGjyfyEqQIPv6JHEEf5F5s4a+DFPsSQfg==");
            CloudStorageAccount storageAccount = new CloudStorageAccount(cred, false);
            CloudBlobClient blobClient = new CloudBlobClient(storageAccount.BlobEndpoint.AbsoluteUri, cred);

            //get a reference to the blob
            CloudBlob blob = blobClient.GetBlobReference(string.Format("{0}/{1}", containerName, filename));

            //write the file to the http response
            //blob.DownloadToStream(downloadStream);
            downloadStream = blob.DownloadByteArray();
            //FetchAttributes();

            return downloadStream;
        }

        public static CloudBlob BlodProperties(string filename)
        {
            string containerName = "blobstorage"; //container must be lowercase, no special characters
            
            StorageCredentialsAccountAndKey cred = new StorageCredentialsAccountAndKey("qidstorage", "NUro8/C7+kMqtwOwLbe6agUvA83s+8xSTBqrkMwSjPP6MAxVkdtsLDGjyfyEqQIPv6JHEEf5F5s4a+DFPsSQfg==");
            CloudStorageAccount storageAccount = new CloudStorageAccount(cred, false);
            CloudBlobClient blobClient = new CloudBlobClient(storageAccount.BlobEndpoint.AbsoluteUri, cred);

            //get a reference to the blob
            CloudBlob blob = blobClient.GetBlobReference(string.Format("{0}/{1}", containerName, filename));

            return blob;
        }

        #endregion

        #region "Common properties to store Master data"

        public static DataSet PartnerTypeMaster
        {
            get
            {
                return _dsPartnerTypeMaster;
            }
            set
            {
                _dsPartnerTypeMaster = value;
            }
        }

        public static DataSet PartnerMaster
        {
            get
            {
                return _dsPartnerMaster;
            }
            set
            {
                _dsPartnerMaster = value;
            }
        }

        public static DataSet AWBPrefixMaster
        {
            get
            {
                return _dsAWBPrefixMaster;
            }
            set
            {
                _dsAWBPrefixMaster = value;
            }
        }

        public static string SmartKargoInstance
        {
            get
            {
                return _strSmartKargoInstance;
            }
            set
            {
                _strSmartKargoInstance = value;
            }
        }

        public static bool FlightValidation
        {
            get
            {
                return _blnFlightValidation;
            }
            set
            {
                _blnFlightValidation = value;
            }
        }

        public static string ShipperMandatoryDuring
        {
            get
            {
                return _strShipperMandatoryDuring;
            }
            set
            {
                _strShipperMandatoryDuring = value;
            }
        }

        public static string ConsigneeMandatoryDuring
        {
            get
            {
                return _strConsigneeMandatoryDuring;
            }
            set
            {
                _strConsigneeMandatoryDuring = value;
            }
        }

        public static string ValidateCCSFandIAC
        {
            get
            {
                return _strValidateCCSFandIAC;
            }
            set
            {
                _strValidateCCSFandIAC = value;
            }
        }

        public static bool ShowOperationTimeOnSave
        {
            get
            {
                return _blnShowOperationTimeOnSave;
            }
            set
            {
                _blnShowOperationTimeOnSave = value;
            }
        }

        public static bool CombineSaveandExecute
        {
            get
            {
                return _blnCombineSaveandExecute;
            }
            set
            {
                _blnCombineSaveandExecute = value;
            }
        }

        public static string HideRatesAcceptance
        {
            get
            {
                return _strHideRatesAcceptance;
            }
            set
            {
                _strHideRatesAcceptance = value;
            }
        }

        public static string EmbargoFunctionality
        {
            get
            {
                return _strEmbargoFunctionality;
            }
            set
            {
                _strEmbargoFunctionality = value;
            }
        }

        public static bool UseCookiesForValidation
        {
            get
            {
                return _UseCookiesForValidation;
            }
            set
            {
                _UseCookiesForValidation = value;
            }
        }

        #endregion
        
        #region Shownotes
        public static bool ShowNotes( string AWBPrefix,string AWBNumber, string FltNo,string FltDate)
        {
            QID.DataAccess.SQLServer da = new QID.DataAccess.SQLServer(Global.GetConnectionString());

            DataSet dsData = new DataSet();
            try
            {


                string result = string.Empty;


                string[] paramname = new string[] { "AWBPrefix", "AWBNumber", "FltNo", "FltDate" };

                object[] paramvalue = new object[] { AWBPrefix,AWBNumber,
                                                 FltNo,FltDate };

                SqlDbType[] paramtype = new SqlDbType[] {  SqlDbType.VarChar,SqlDbType.VarChar,
                                                      SqlDbType.VarChar,
                                                      SqlDbType.VarChar};
               result= da.GetStringByProcedure("Sp_ShowNoteExists", paramname, paramvalue, paramtype);

               if (result != null && result != "")
               {

                   return true;

               }
               else
               {
                   return false;
               }
               
            }
            catch (Exception ex)
            {
                //return dsData;
                return false;
            }
        }
        #endregion Shownotes
    }
}
