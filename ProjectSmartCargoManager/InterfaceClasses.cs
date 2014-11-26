
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BAL;
using QID.DataAccess;
//using DG.Square.ASP.NET.Ajax.ChatControl;
using System.Xml;
using System.Diagnostics;
using nsBalInterface;
using System.Net;
using System.Collections;




namespace ProjectSmartCargoManager
{


    #region IFLYWARE
public class cargoInfo
{
    #region Variable declaration

    public String FlightNo = null;
    public String FlightDate = null;
    public String Origin = null;
    public String Destination = null;
    public String AircraftType = null;
    public String BookedCargoPieces = null;
    public String AcceptedCargoPieces = null;
    public String BookedCargoWeight = null;
    public String AcceptedCargoWeight = null;


    #endregion

    #region public Methods
    public cargoInfo()
    {
    }

    #endregion
}

#endregion

    #region ISAP

interface ISAP
{
    String ToXML();
    String ToXMLNode(DateTime fromDate, DateTime ToDate);
}

public class SAP : ISAP
{
    DateTime invoiceDate;
    SAPInvoice objInvoice = null;
    SAPCollection objCollection = null;
    SAPManifest objManifest = null;
    BalInterface objBal = null;
    LoginBL objBL = new LoginBL(); 


    public SAP(DateTime invDate)
    {
        try
        {
            invoiceDate = invDate;
            objInvoice = new SAPInvoice();
            objBal = new BalInterface();
        }
        catch (Exception objEx)
        {
        }
    }


    #region ISAP Members

    public string ToXML()
    {
        String strXML = "";
        try
        {
            objInvoice = new SAPInvoice();
            objManifest = new SAPManifest();
            objCollection = new SAPCollection();

            object[] obj = { invoiceDate };
            object[] obj1 = { new DateTime(2014, 06, 23) };
            object[] obj2 = { new DateTime(2014, 07, 02) };//Jul  2 2014 12:00AM
            DataSet dtInvoice = objBal.fetchISAPInvoiceDetails(obj);
            DataSet dtManifest = objBal.fetchISAPManifestDetails(obj);
            DataSet dtCollect = objBal.fetchISAPCollectionDetails(obj);
            
            objInvoice.FillData(dtInvoice);
            objManifest.FillData(dtManifest);
            objCollection.FillData(dtCollect);

            strXML = "<SAPCDMInterfaceType>" + Environment.NewLine +
                            "<ProcessingDate>" + invoiceDate.ToString() + "</ProcessingDate>" + Environment.NewLine +
                            objInvoice.ToXML() + Environment.NewLine +
                            objManifest.ToXML() + Environment.NewLine +
                            objCollection.ToXML() + Environment.NewLine +
                        "</SAPCDMInterfaceType>";
            string strFileName = "sksap." + invoiceDate.ToString("yyyyMMdd") + ".xml";
            String strPath = HttpRuntime.AppDomainAppPath.ToString()+"\\" + strFileName;
            StreamWriter wr = new StreamWriter(strPath);
            wr.Write(strXML);
            wr.Close();
            Process.Start(strPath);

            if (strXML.Length > 0)
            {
                byte[] raw = Encoding.UTF8.GetBytes(strXML);
                Stream decoded = new MemoryStream(raw);

                //string FileName = "IAMCEB_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".xml";
                bool Uploaded = ProjectSmartCargoManager.CommonUtility.UploadBlob(decoded, strFileName);
                cls_BL.DumpInterfaceInformation("SAPXml", strFileName, DateTime.Now, "SAPXml", "", true, "SAPXml", "SAPXml",null,"");
            }
        }
        catch (Exception objEx)
        {
        }

        return strXML;
    }

    public string ToXMLNode(DateTime fromDate, DateTime ToDate)
    {
        String strXML = "";
        try
        {
            
            objInvoice = new SAPInvoice();
            objManifest = new SAPManifest();
            objCollection = new SAPCollection();

            for (int i = 0; fromDate.AddDays(i) <= ToDate; i++)
            {
                try
                {
                    object[] obj = { fromDate.AddDays(i) };
                    DataSet dtInvoice = objBal.fetchISAPInvoiceDetails(obj);
                    DataSet dtManifest = objBal.fetchISAPManifestDetails(obj);
                    DataSet dtCollect = objBal.fetchISAPCollectionDetails(obj);

                    objInvoice.FillData(dtInvoice);
                    objManifest.FillData(dtManifest);
                    objCollection.FillData(dtCollect);
                }
                catch (Exception objEx)
                {
                }
            }

            if (!Directory.Exists(HttpRuntime.AppDomainAppPath.ToString() + "\\SAP_XML"))
                Directory.CreateDirectory(HttpRuntime.AppDomainAppPath.ToString() + "\\SAP_XML");

            String strPath = HttpRuntime.AppDomainAppPath.ToString()+"\\SAP_XML\\" + "sksap." + DateTime.Now.ToString("yyyyMMdd") + ".xml";

            XmlTextWriter writer = new XmlTextWriter(strPath, System.Text.Encoding.UTF8);

            if (writer != null)
            {
                writer.WriteStartDocument(true);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("SAPCDMInterfaceType");
                writer.WriteStartElement("ProcessingDate");
                writer.WriteString(BalInterface.GetDateYYYMMDD(fromDate));
                writer.WriteEndElement();
                objInvoice.ToXML(writer);
                objManifest.ToXML(writer);
                objCollection.ToXML(writer);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
                
            }
            else
            {


                strXML = "<SAPCDMInterfaceType>" + Environment.NewLine +
                                "<ProcessingDate>" + invoiceDate.ToString() + "</ProcessingDate>" + Environment.NewLine +
                                objInvoice.ToXML() + Environment.NewLine +
                                objManifest.ToXML() + Environment.NewLine +
                                objCollection.ToXML() + Environment.NewLine +
                            "</SAPCDMInterfaceType>";
                // String strPath = @"C:\Users\ARPIT\Desktop\CEBU\" + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".xml";
              //  StreamWriter wr = new StreamWriter(@"C:\Users\ARPIT\Desktop\CEBU\" + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + ".xml");
                StreamWriter wr = new StreamWriter(strPath);
                wr.Write(strXML);
                wr.Close();
                Process.Start(strPath);
            }

            if (Convert.ToBoolean(objBL.GetMasterConfiguration("SAP_uploadXmlOnFTP")))
            {
               
               // uploadXmlOnFTP(strPath, "ftp://72.167.41.153", "QIDtech", "QID#tech#2014", "SAP_XML");

                uploadXmlOnFTP(strPath, objBL.GetMasterConfiguration("SAP_ftpLink"),
                                        objBL.GetMasterConfiguration("SAP_ftpUser"),
                                        objBL.GetMasterConfiguration("SAP_ftpPassword"),
                                        objBL.GetMasterConfiguration("SAP_ftpFolderPath"));

                File.Delete(strPath);
            }
            

            

            //Process.Start(strPath);
        }
        catch (Exception objEx)
        {
        }

        return strXML;
    }


    private void uploadXmlOnFTP(String filePath, String ftpLink, string ftpUser, string ftpPwd, String ftpFolder)
    {
        FtpWebRequest objFtpReq;
        try
        {
            String fileName = Path.GetFileName(filePath);

            Uri objUri = null;

            if (String.IsNullOrEmpty(ftpFolder))
            {
                objUri = new Uri(String.Format(@"{0}/{1}", ftpLink, fileName));
            }
            else
            {
                objUri= new Uri(String.Format(@"{0}/{1}/{2}", ftpLink, ftpFolder, fileName));
            }
            objFtpReq = WebRequest.Create(objUri) as FtpWebRequest;

            objFtpReq.Method = WebRequestMethods.Ftp.UploadFile;
            objFtpReq.UseBinary = true;
            objFtpReq.UsePassive = true ;
            objFtpReq.KeepAlive = true;
            objFtpReq.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            objFtpReq.ConnectionGroupName = "group";

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] buff = new byte[fileStream.Length];
                fileStream.Read(buff, 0, buff.Length);
                fileStream.Close();

                Stream ftpStream = objFtpReq.GetRequestStream();
                ftpStream.Write(buff, 0, buff.Length);
                ftpStream.Close();
                ftpStream.Flush();

            }
        }
        catch (Exception objEx)
        {
        }
    }


    #endregion
}

public class SAPInvoice
{
    #region Data declaration
    DateTime invoiceDate;
    int accountYear = 0;
    int accountMonth = 0;
    InvoiceAccount[] acccounts = new InvoiceAccount[0];

    #endregion

    #region property

    public DateTime InvoiceDate
    {
        set
        {
            this.invoiceDate = value;
        }
        get
        {
            return this.invoiceDate;
        }
    }

    public InvoiceAccount[] AcccountInfo
    {
        set
        {
            this.acccounts = value;
        }
        get
        {
            return this.acccounts;
        }

    }

    #endregion

    public void FillData(DataSet dsData)
    {
        try
        {
            int count = 0;
            foreach (DataRow dr in dsData.Tables[0].Rows)
            {

                //if (count > 5)
                //    break;
                count++;
                try
                {
                    bool fRecordExist = false;
                    if (acccounts.Length > 0)
                    {
                        for (int i = 0; i < acccounts.Length; i++)
                        {
                            try
                            {
                                if (acccounts[i].AccountNo.Trim().ToUpper() == dr["AgentCode"].ToString().ToUpper().Trim())
                                {
                                    acccounts[i].FillData(dr);
                                    fRecordExist = true;
                                    break;
                                }
                            }
                            catch (Exception objEx)
                            {
                            }
                        }
                    }
                    if (!fRecordExist)
                    {
                        InvoiceAccount objAcc = new InvoiceAccount();
                        objAcc.FillData(dr);
                        Array.Resize(ref acccounts, acccounts.Length + 1);
                        acccounts[acccounts.Length - 1] = objAcc;
                    }

                    InvoiceDate = Convert.ToDateTime(dr["invoiceDate"].ToString().Trim());
                   
                    accountMonth = InvoiceDate.Month;
                    //accountYear = Convert.ToInt16(InvoiceDate.ToString("yyyy"));
                     accountYear = Convert.ToInt16(InvoiceDate.ToString("yy"));

                    
                }
                catch (Exception objEx)
                {
                }
            }


        }
        catch (Exception objEx)
        {
        }

    }

    public String ToXML()
    {
        string strXML = "";
        try
        {
            strXML = "<SAPInvoiceInformation>" + Environment.NewLine +
                         "<InvoiceDate>" + InvoiceDate.ToString() + "</InvoiceDate>" + Environment.NewLine +
                         "<AccountingPeriods>" + Environment.NewLine +
                             "<AccountingPeriodType>" + Environment.NewLine +
                                 "<AccountingYear>" + accountYear + "</AccountingYear>" + Environment.NewLine +
                                 "<AccountingMonth>" + accountMonth + "</AccountingMonth>" + Environment.NewLine +
                                 "<Accounts>" + Environment.NewLine +
                                     getInvoiceAccountXML(acccounts) + Environment.NewLine +
                                 "</Accounts>" + Environment.NewLine +
                             "</AccountingPeriodType>" + Environment.NewLine +
                         "</AccountingPeriods>" + Environment.NewLine +
                     "</SAPInvoiceInformation>";
        }
        catch (Exception objex)
        {
        }
        return strXML;
    }

    public String ToXML(XmlTextWriter writer)
    {
        string strXML = "";
        try
        {
            writer.WriteStartElement("SAPInvoiceInformation");// +Environment.NewLine +

            writer.WriteStartElement("InvoiceDate");
            writer.WriteString(BalInterface.GetDateYYYMMDD(invoiceDate));//InvoiceDate.Year+"-"+invoiceDate.Month+"-"+invoiceDate.Day);
            writer.WriteEndElement();// +"</InvoiceDate>" + Environment.NewLine +
            writer.WriteStartElement("AccountingPeriods");
            writer.WriteStartElement("AccountingPeriodType");
            writer.WriteStartElement("AccountingYear");
            //writer.WriteString((accountYear < 100 ? "20" + accountYear.ToString() : accountYear.ToString()));
            writer.WriteString(accountYear.ToString());
            writer.WriteEndElement();//AccountingYear>" + Environment.NewLine +
            writer.WriteStartElement("AccountingMonth");
            writer.WriteString((accountMonth <= 9 ? "0" + accountMonth.ToString() : accountMonth.ToString()));
            writer.WriteEndElement();// +"</AccountingMonth>" + Environment.NewLine +
            writer.WriteStartElement("Accounts");
            getInvoiceAccountXML(acccounts, writer);// +Environment.NewLine +
            writer.WriteEndElement();// "</Accounts>" + Environment.NewLine +
            writer.WriteEndElement();// "</AccountingPeriodType>" + Environment.NewLine +
            writer.WriteEndElement(); //"</AccountingPeriods>" + Environment.NewLine +
            writer.WriteEndElement();// "</SAPInvoiceInformation>";
        }
        catch (Exception objex)
        {
        }
        return strXML;
    }

    private void getInvoiceAccountXML(InvoiceAccount[] acccounts, XmlTextWriter writer)
    {
        try
        {
            try
            {
                foreach (InvoiceAccount objAcc in acccounts)
                {
                    objAcc.ToXML(writer);
                }
            }
            catch (Exception objEx)
            {
            }
        }
        catch (Exception obJex)
        {
        }
    }

    private string getInvoiceAccountXML(InvoiceAccount[] acccounts)
    {
        string strXML = "";
        try
        {
            foreach (InvoiceAccount objAcc in acccounts)
            {
                strXML += objAcc.ToXML();
            }
        }
        catch (Exception objEx)
        {
        }
        return strXML;
    } 
  
}

public class InvoiceAccount
{
    String invoiceType = "N";
    String accountNo = "";
    int accountingYear = 0;
    int accountingMonth = 0;

    InvoiceOriginDestination[] arrOriginDes = new InvoiceOriginDestination[0];

    public void FillData(DataRow dtRow)
    {
        try
        {
            bool fRecordExist = false;
            if (arrOriginDes.Length > 0)
            {
                for (int i = 0; i < arrOriginDes.Length; i++)
                {
                    try
                    {
                        if (arrOriginDes[i].origin.ToUpper().Trim() == dtRow["Origin"].ToString().ToUpper().Trim() &&
                            arrOriginDes[i].destination.ToUpper().Trim() == dtRow["Destination"].ToString().ToUpper().Trim()
                            )
                        {
                            arrOriginDes[i].FillData(dtRow);
                            fRecordExist = true;
                            break;
                        }

                    }
                    catch (Exception objEx)
                    {
                    }
                }
            }
            if (!fRecordExist)
            {
                InvoiceOriginDestination tmpOriginDes = new InvoiceOriginDestination();
                tmpOriginDes.FillData(dtRow);
                Array.Resize(ref arrOriginDes, arrOriginDes.Length + 1);
                arrOriginDes[arrOriginDes.Length - 1] = tmpOriginDes;
            }
            if (!String.IsNullOrEmpty(dtRow["InvMatchFlag"].ToString()))
            {
                invoiceType = dtRow["InvMatchFlag"].ToString();
            }
            accountNo = dtRow["AgentCode"].ToString();
            accountingYear = Convert.ToDateTime(dtRow["InvoiceDate"].ToString()).Year;
            accountingMonth = Convert.ToDateTime(dtRow["InvoiceDate"].ToString()).Month;
        }
        catch (Exception objEx)
        {
        }

    }

    public String InvoiceType
    {
        get
        {
            return this.invoiceType;
        }
        set
        {
            this.invoiceType = value;
        }
    }

    public string AccountNo
    {
        get
        {
            return this.accountNo;
        }
        set
        {
            this.accountNo = value;
        }
    }

    public int AccountingYear
    {
        get
        {
            return this.accountingYear;
        }
        set
        {
            this.accountingYear = value;
        }
    }

    public int AccountingMonth
    {
        get
        {
            return this.accountingMonth;
        }
        set
        {
            this.accountingMonth = value;
        }
    }

    public String ToXML()
    {
        String strXML = "";
        try
        {
            strXML = "<AccountType>" + Environment.NewLine +
                        "<AccountNumber>" + AccountNo + "</AccountNumber>" + Environment.NewLine +
                        "<InvoiceTypes>" + Environment.NewLine +
                            "<InvoiceTypeType>" + Environment.NewLine +
                                "<InvoiceType>" + InvoiceType + "</InvoiceType>" + Environment.NewLine +
                                "<OriginDestinations>" + Environment.NewLine +
                                    getInvoiceOriginDestXML(arrOriginDes) + Environment.NewLine +
                                "</OriginDestinations>" + Environment.NewLine +
                            "</InvoiceTypeType>" + Environment.NewLine +
                        "</InvoiceTypes>" + Environment.NewLine +
                    "</AccountType>";


        }
        catch (Exception objEx)
        {
        }
        return strXML;
    }

    private string getInvoiceOriginDestXML(InvoiceOriginDestination[] arrOriginDes)
    {
        String strXML = "";

        try
        {
            foreach (InvoiceOriginDestination obj in arrOriginDes)
            {
                strXML += obj.ToXML();
            }
        }
        catch (Exception objEx)
        {
        }
        return strXML;
    }

    internal void ToXML(XmlTextWriter writer)
    {
        try
        {

            writer.WriteStartElement("AccountType");
            writer.WriteStartElement("AccountNumber");
            writer.WriteString(AccountNo.ToString());
            writer.WriteEndElement();// "</AccountNumber>" + Environment.NewLine +
            writer.WriteStartElement("InvoiceTypes");//; + Environment.NewLine +
            writer.WriteStartElement("InvoiceTypeType");// + Environment.NewLine +
            writer.WriteStartElement("InvoiceType");
            writer.WriteString(InvoiceType);
            writer.WriteEndElement();// "</InvoiceType>" + Environment.NewLine +
            writer.WriteStartElement("OriginDestinations");// +Environment.NewLine +
            getInvoiceOriginDestXML(arrOriginDes, writer);// + Environment.NewLine +
            writer.WriteEndElement();//"</OriginDestinations>" + Environment.NewLine +
            writer.WriteEndElement();//"</InvoiceTypeType>" + Environmnt.NewLine +
            writer.WriteEndElement();//"</InvoiceTypes>" + Environment.NewLine +
            writer.WriteEndElement();//"</AccountType>";

        }
        catch (Exception objEx)
        {
        }
    }

    private void getInvoiceOriginDestXML(InvoiceOriginDestination[] arrOriginDes, XmlTextWriter writer)
    {
        try
        {
            foreach (InvoiceOriginDestination obj in arrOriginDes)
            {
                obj.ToXML(writer);
            }
        }
        catch (Exception objEx)
        {
        }
    }
}

public class InvoiceOriginDestination
{
    public String origin = String.Empty;
    public String destination = String.Empty;
    public String chargedCode = String.Empty;
    public String currencyCode = String.Empty;
    public DateTime AWBDate = DateTime.Now;
    // public InvoiceFlightInfo[] objFlights = new InvoiceFlightInfo[0];
    public InvoiceAWBDate[] objAWBDates = new InvoiceAWBDate[0];

    public void FillData(DataRow dtRow)
    {
        try
        {
            bool fRecordExist = false;
            if (objAWBDates.Length > 0)
            {
                for (int i = 0; i < objAWBDates.Length; i++)
                {
                    try
                    {
                        DateTime awbdate = Convert.ToDateTime(dtRow["AWBDate"].ToString().Trim());
                        if (objAWBDates[i].AWBExecDate.Day == awbdate.Day && objAWBDates[i].AWBExecDate.Month == awbdate.Month && objAWBDates[i].AWBExecDate.Year == awbdate.Year)
                        {
                            objAWBDates[i].FillData(dtRow);
                            fRecordExist = true;
                            break;
                        }
                    }
                    catch (Exception objEx)
                    {
                    }
                }
            }
            if (!fRecordExist)
            {
                InvoiceAWBDate objAWBDt = new InvoiceAWBDate();
                objAWBDt.FillData(dtRow);
                Array.Resize(ref objAWBDates, objAWBDates.Length + 1);
                objAWBDates[objAWBDates.Length - 1] = objAWBDt;

            }
            origin = dtRow["Origin"].ToString().Trim();
            destination = dtRow["Destination"].ToString().Trim();
            chargedCode = dtRow["ChargeType"].ToString().Trim();
            AWBDate = Convert.ToDateTime(dtRow["AWBDate"].ToString().Trim());
            currencyCode = dtRow["Currency"].ToString().Trim();
        }
        catch (Exception objEx)
        {
        }

    }

    public String ToXML()
    {
        String strXML = "";
        try
        {
            strXML = "<OriginAndDestinationType>" + Environment.NewLine +
                        "<Origin>" + origin + "</Origin>" + Environment.NewLine +
                        "<Destination>" + destination + "</Destination>" + Environment.NewLine +
                        "<ChargeCodes>" + Environment.NewLine +
                            "<ChargeCodeType>" + Environment.NewLine +
                                "<ChargeCode>" + chargedCode + "</ChargeCode>" + Environment.NewLine +
                                "<AWBDates>" + Environment.NewLine +
                                    "<AWBDateType>" + Environment.NewLine +
                                        "<AWBDate>" + AWBDate.ToString() + "</AWBDate>" + Environment.NewLine +
                                        "<CurrencyCharges>" + Environment.NewLine +
                                            "<CurrencyChargesType>" + Environment.NewLine +
                                                "<CurrencyCode>" + currencyCode + "</CurrencyCode>" + Environment.NewLine +
                                                "<FlightCharges>" + Environment.NewLine +
                                                    getInvoiceFlightInfoXML(objAWBDates) + Environment.NewLine +
                                                "</FlightCharges>" + Environment.NewLine +
                                            "</CurrencyChargesType>" + Environment.NewLine +
                                        "</CurrencyCharges>" + Environment.NewLine +
                                    "</AWBDateType>" + Environment.NewLine +
                                "</AWBDates>" + Environment.NewLine +
                            "</ChargeCodeType>" + Environment.NewLine +
                        "</ChargeCodes>" + Environment.NewLine +
                    "</OriginAndDestinationType>";

        }
        catch (Exception objEx)
        {
        }
        return strXML;
    }

    private string getInvoiceFlightInfoXML(InvoiceAWBDate[] objAWBDt)
    {
        string strXML = "";
        try
        {
            foreach (InvoiceAWBDate objDt in objAWBDt)
            {
                try
                {
                    strXML += objDt.ToXML();
                }
                catch (Exception objEx)
                {
                }
            }
        }
        catch (Exception objEx)
        {
        }
        return strXML;
    }

    internal void ToXML(XmlTextWriter writer)
    {
        try
        {
            writer.WriteStartElement("OriginAndDestinationType");
            writer.WriteStartElement("Origin");
            writer.WriteString(origin);
            writer.WriteEndElement();// +"</Origin>" + Environment.NewLine +
            writer.WriteStartElement("Destination");
            writer.WriteString(destination);
            writer.WriteEndElement();//+"</Destination>" + Environment.NewLine +
            writer.WriteStartElement("ChargeCodes");
            writer.WriteStartElement("ChargeCodeType");
            writer.WriteStartElement("ChargeCode");
            writer.WriteString(chargedCode);
            writer.WriteEndElement();//+"</ChargeCode>" + Environment.NewLine +
            writer.WriteStartElement("AWBDates");
            getInvoiceAWBDateXML(objAWBDates, writer);
            writer.WriteEndElement();//"</AWBDates>" + Environment.NewLine +
            writer.WriteEndElement();//"</ChargeCodeType>" + Environment.NewLine +
            writer.WriteEndElement();//"</ChargeCodes>" + Environment.NewLine +
            writer.WriteEndElement();//"</OriginAndDestinationType>";

        }
        catch (Exception objEx)
        {
        }
    }

    private void getInvoiceAWBDateXML(InvoiceAWBDate[] objAWBDt, XmlTextWriter writer)
    {
        try
        {
            foreach (InvoiceAWBDate objFlt in objAWBDt)
            {
                try
                {
                    objFlt.ToXML(writer);
                }
                catch (Exception objEx)
                {
                }
            }
        }
        catch (Exception objEx)
        {
        }
    }
}

public class InvoiceAWBDate
{
    public String airlineCode = String.Empty;
    public String flightNo = String.Empty;
    public String currencyCode = "";
    public DateTime AWBExecDate = DateTime.Now;
    public InvoiceFlightInfo[] objFlights = new InvoiceFlightInfo[0];

    public void FillData(DataRow dtRow)
    {
        try
        {
            try
            {
                bool fRecordExist = false;
                if (objFlights.Length > 0)
                {
                    for (int i = 0; i < objFlights.Length; i++)
                    {
                        try
                        {
                            DateTime fltdate = Convert.ToDateTime(dtRow["FlightDate"].ToString());
                            if (objFlights[i].AirlineCode.Trim().ToUpper() == dtRow["FlightNo"].ToString().ToUpper().Substring(0, 2) &&
                                    objFlights[i].FlightNumber.Trim().ToUpper() == dtRow["FlightNo"].ToString().ToUpper().Substring(2) &&
                                    objFlights[i].FlightDate.Day == fltdate.Day && objFlights[i].FlightDate.Month == fltdate.Month && objFlights[i].FlightDate.Year == fltdate.Year
                                )
                            {
                                objFlights[i].FillData(dtRow);
                                fRecordExist = true;
                                break;
                            }
                        }
                        catch (Exception objEx)
                        {
                        }
                    }
                }
                if (!fRecordExist)
                {
                    InvoiceFlightInfo objFlt = new InvoiceFlightInfo();
                    objFlt.FillData(dtRow);
                    Array.Resize(ref objFlights, objFlights.Length + 1);
                    objFlights[objFlights.Length - 1] = objFlt;
                }
            }
            catch (Exception objEx)
            {
            }
            airlineCode = dtRow["FlightNo"].ToString().Substring(0, 2);
            flightNo = dtRow["FlightNo"].ToString().Substring(2);
            currencyCode = dtRow["Currency"].ToString();
            AWBExecDate = Convert.ToDateTime(dtRow["AWBDate"].ToString().Trim());

        }
        catch (Exception objEx)
        {
        }

    }

    public String ToXML()
    {
        String strXML = "";
        try
        {
            //strXML = "<OriginAndDestinationType>" + Environment.NewLine +
            //            "<Origin>" + origin + "</Origin>" + Environment.NewLine +
            //            "<Destination>" + destination + "</Destination>" + Environment.NewLine +
            //            "<ChargeCodes>" + Environment.NewLine +
            //                "<ChargeCodeType>" + Environment.NewLine +
            //                    "<ChargeCode>" + chargedCode + "</ChargeCode>" + Environment.NewLine +
            //                    "<AWBDates>" + Environment.NewLine +
            //                        "<AWBDateType>" + Environment.NewLine +
            //                            "<AWBDate>" + AWBDate.ToString() + "</AWBDate>" + Environment.NewLine +
            //                            "<CurrencyCharges>" + Environment.NewLine +
            //                                "<CurrencyChargesType>" + Environment.NewLine +
            //                                    "<CurrencyCode>" + currencyCode + "</CurrencyCode>" + Environment.NewLine +
            //                                    "<FlightCharges>" + Environment.NewLine +
            //                                        getInvoiceFlightInfoXML(objFlights) + Environment.NewLine +
            //                                    "</FlightCharges>" + Environment.NewLine +
            //                                "</CurrencyChargesType>" + Environment.NewLine +
            //                            "</CurrencyCharges>" + Environment.NewLine +
            //                        "</AWBDateType>" + Environment.NewLine +
            //                    "</AWBDates>" + Environment.NewLine +
            //                "</ChargeCodeType>" + Environment.NewLine +
            //            "</ChargeCodes>" + Environment.NewLine +
            //        "</OriginAndDestinationType>";

        }
        catch (Exception objEx)
        {
        }


        return strXML;
    }

    internal void ToXML(XmlTextWriter writer)
    {
        try
        {
            writer.WriteStartElement("AWBDateType");
            writer.WriteStartElement("AWBDate");
            writer.WriteString(BalInterface.GetDateYYYMMDD(AWBExecDate));//AWBDate.Year+"-"+AWBDate.Month+"-"+AWBDate.Day);
            writer.WriteEndElement();// "</AWBDate>" + Environment.NewLine +
            writer.WriteStartElement("CurrencyCharges");
            writer.WriteStartElement("CurrencyChargesType");
            writer.WriteStartElement("CurrencyCode");
            writer.WriteString(currencyCode);
            writer.WriteEndElement();// +"</CurrencyCode>" + Environment.NewLine +
            writer.WriteStartElement("FlightCharges");// + Environment.NewLine +
            getInvoiceFlightInfoXML(objFlights, writer);
            writer.WriteEndElement();//                                      "</FlightCharges>" + Environment.NewLine +
            writer.WriteEndElement();//       "</CurrencyChargesType>" + Environment.NewLine +
            writer.WriteEndElement();//"</CurrencyCharges>" + Environment.NewLine +              
            writer.WriteEndElement();//"</AWBDateType>" + Environment.NewLine +

        }
        catch (Exception objEx)
        {
        }
    }

    private void getInvoiceFlightInfoXML(InvoiceFlightInfo[] objFlights, XmlTextWriter writer)
    {
        try
        {
            foreach (InvoiceFlightInfo objFlt in objFlights)
            {
                try
                {
                    objFlt.ToXml(writer);
                }
                catch (Exception objEx)
                {
                }
            }
        }
        catch (Exception objEx)
        {
        }
    }

}

public class InvoiceFlightInfo
{

    String airlineCode = String.Empty;
    String flightNumber = String.Empty;
    DateTime flightDate;
    String aircaftType = String.Empty;
    String tailNumber = String.Empty;
    int noOfInvoices = 1;
    InvoiceFreight[] objFreight = new InvoiceFreight[0];

    public void FillData(DataRow dtRow)
    {
        try
        {
            flightNumber = dtRow["FlightNo"].ToString().Substring(2);
            aircaftType = dtRow["AirCraftType"].ToString();
            tailNumber = dtRow["tailNo"].ToString();
            try
            {
                InvoiceFreight objFrt = new InvoiceFreight();
                objFrt.FillData(dtRow);
                Array.Resize(ref objFreight, objFreight.Length + 1);
                objFreight[objFreight.Length - 1] = objFrt;

                airlineCode = dtRow["FlightNo"].ToString().Substring(0, 2);
                flightDate = Convert.ToDateTime(dtRow["FlightDate"].ToString());
            }
            catch (Exception objEx)
            {
            }
        }
        catch (Exception objEx)
        {
        }

    }

    #region Property
    public String AirlineCode
    {
        get
        {
            return this.airlineCode;
        }
        set
        {
            this.airlineCode = value;
        }
    }
    public String FlightNumber
    {
        get
        {
            return this.flightNumber;
        }
        set
        {
            this.flightNumber = value;
        }
    }

    public DateTime FlightDate
    {
        get
        {
            return this.flightDate;
        }
        set
        {
            this.flightDate = value;
        }
    }
    public String AircaftType
    {
        get
        {
            return this.aircaftType;
        }
        set
        {
            this.aircaftType = value;
        }
    }
    public String TailNumber
    {
        get
        {
            return this.tailNumber;
        }
        set
        {
            this.tailNumber = value;
        }
    }

    public int NumberOfInvoices
    {
        get
        {
            return this.noOfInvoices;
        }
        set
        {
            this.noOfInvoices = value;
        }
    }

    #endregion


    #region  methods

    public String ToXml()
    {
        String strXML = "";

        try
        {
            strXML = "<FlightChargeType>" + Environment.NewLine +
                        "<AirlineCode>" + AirlineCode + "</AirlineCode>" + Environment.NewLine +
                        "<FlightNumber>" + FlightNumber.Substring(2) + "</FlightNumber>" + Environment.NewLine +
                        "<FlightDate>" + FlightDate + "</FlightDate>" + Environment.NewLine +
                        "<AircaftType>" + AircaftType + "</AircaftType>" + Environment.NewLine +
                        "<TailNumber>" + TailNumber + "</TailNumber>" + Environment.NewLine +
                        "<InvoiceAmount>" + getInvoiceAmount(objFreight) + "</InvoiceAmount>" + Environment.NewLine +
                        "<WeightCharge>" + getWeightCharge(objFreight) + "</WeightCharge>" + Environment.NewLine +
                        "<SurCharges>0</SurCharges>" + Environment.NewLine +
                        "<XVTC>0</XVTC>" + Environment.NewLine +
                        "<VAT>0</VAT>" + Environment.NewLine +
                        "<OtherCharges>00</OtherCharges>" + Environment.NewLine +
                        "<AIF>0</AIF>" + Environment.NewLine +
                        "<DGF>0</DGF>" + Environment.NewLine +
                        "<SHF>0</SHF>" + Environment.NewLine +
                        "<LAF>00</LAF>" + Environment.NewLine +
                        "<PKG>0</PKG>" + Environment.NewLine +
                        "<CSF>0</CSF>   " + Environment.NewLine +
                        "<INS>0</INS>" + Environment.NewLine +
                        "<DOC>0</DOC>" + Environment.NewLine +
                        "<TSC>0</TSC>" + Environment.NewLine +
                        "<XSF>0</XSF>  " + Environment.NewLine +
                        "<TSF>0</TSF>" + Environment.NewLine +
                        "<GSF>0</GSF>" + Environment.NewLine +
                        "<CUS>0</CUS>" + Environment.NewLine +
                        "<IMP>0</IMP>" + Environment.NewLine +
                        "<NumberOfInvoices>" + NumberOfInvoices + "</NumberOfInvoices>" + Environment.NewLine +
                        "</FlightChargeType>";
        }
        catch (Exception objEx)
        {
        }

        return strXML;
    }

    #endregion


    internal void ToXml(XmlTextWriter writer)
    {

        writer.WriteStartElement("FlightChargeType");
        writer.WriteStartElement("AirlineCode");
        writer.WriteString(AirlineCode);
        writer.WriteEndElement();// +"</AirlineCode>" + Environment.NewLine +
        writer.WriteStartElement("FlightNumber");
        writer.WriteString(FlightNumber.ToString());
        writer.WriteEndElement();// +"</FlightNumber>" + Environment.NewLine +
        writer.WriteStartElement("FlightDate");
        writer.WriteString(BalInterface.GetDateYYYMMDD(FlightDate));
        writer.WriteEndElement();//+"</FlightDate>" + Environment.NewLine +
        writer.WriteStartElement("AircaftType");
        writer.WriteString(AircaftType);
        writer.WriteEndElement();// +"</AircaftType>" + Environment.NewLine +
        writer.WriteStartElement("TailNumber");
        writer.WriteString(TailNumber);
        writer.WriteEndElement();// "</TailNumber>" + Environment.NewLine +
        writer.WriteStartElement("InvoiceAmount");
        writer.WriteString(getInvoiceAmount(objFreight));
        writer.WriteEndElement();// +"</InvoiceAmount>" + Environment.NewLine +
        writer.WriteStartElement("WeightCharge");
        writer.WriteString(getWeightCharge(objFreight));
        writer.WriteEndElement();//+"</WeightCharge>" + Environment.NewLine +
        writer.WriteStartElement("SurCharges");
        writer.WriteString(getSurCharges(objFreight));
        writer.WriteEndElement();//</SurCharges>" + Environment.NewLine +
        writer.WriteStartElement("XVTC");
        writer.WriteString("0");
        writer.WriteEndElement();// 0</XVTC>" + Environment.NewLine +
        writer.WriteStartElement("VAT");
        writer.WriteString(getVAT(objFreight));
        writer.WriteEndElement();//>0</VAT>" + Environment.NewLine +
        writer.WriteStartElement("OtherCharges");       
        writer.WriteString(getOtherChargesWithTotal(objFreight));
        writer.WriteEndElement();//>00</OtherCharges>" + Environment.NewLine +

        ArrayList alDuplicate = new ArrayList();
        double totalAmt = -1;
        for (int i = 0; i < objFreight.Length; i++)
        {
            try
            {
                
                if (!String.IsNullOrEmpty(objFreight[i].chargeHeadCode))
                {
                    if (!alDuplicate.Contains(objFreight[i].chargeHeadCode.ToUpper().Trim()))
                    {
                         totalAmt = getOtherCharges(objFreight[i].chargeHeadCode.ToUpper().Trim(), objFreight);
                        if (totalAmt > 0)
                        {
                            writer.WriteStartElement(objFreight[i].chargeHeadCode.ToUpper().Trim());
                            writer.WriteString(totalAmt.ToString());
                            writer.WriteEndElement();//>0</AIF>" + Environment.NewLine +
                        }

                        alDuplicate.Add(objFreight[i].chargeHeadCode.ToUpper().Trim());
                    }
                }
            }
            catch (Exception objEx)
            {
            }
        }


        #region Comment 

        /***********************************************************

        double totalAmt = getOtherCharges("AIF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("AIF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</AIF>" + Environment.NewLine +
        }
        totalAmt = -1;
        totalAmt = getOtherCharges("VLC", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("VLC");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</AIF>" + Environment.NewLine +
        }
        totalAmt = -1;
        totalAmt = getOtherCharges("DGF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("DGF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</DGF>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("SHF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("SHF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</SHF>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("LAF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("LAF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>00</LAF>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("PKG", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("PKG");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</PKG>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("CSF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("CSF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</CSF>   " + Environment.NewLine +
        }
        totalAmt = -1;
        totalAmt = getOtherCharges("INS", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("INS");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</INS>" + Environment.NewLine +
        }
        totalAmt = -1;
        totalAmt = getOtherCharges("DOC", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("DOC");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</DOC>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("TSC", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("TSC");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</TSC>" + Environment.NewLine +
        }
        totalAmt = -1;
        totalAmt = getOtherCharges("XSF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("XSF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</XSF>  " + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("TSF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("TSF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</TSF>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("GSF", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("GSF");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</GSF>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("CUS", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("CUS");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</CUS>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("IMP", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("IMP");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</IMP>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("FSC", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("FSC");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</IMP>" + Environment.NewLine +
        }

        totalAmt = -1;
        totalAmt = getOtherCharges("JWL", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("JWL");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</IMP>" + Environment.NewLine +
        }
        totalAmt = -1;
        totalAmt = getOtherCharges("JWE", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("JWE");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</IMP>" + Environment.NewLine +
        }
        totalAmt = -1;
        totalAmt = getOtherCharges("HUM", objFreight);
        if (totalAmt > 0)
        {
            writer.WriteStartElement("HUM");
            writer.WriteString(totalAmt.ToString());
            writer.WriteEndElement();//>0</IMP>" + Environment.NewLine +
        }
         * *********************************************************************/
        
        #endregion

        writer.WriteStartElement("NumberOfInvoices");
        writer.WriteString(getNumberOfInvoices(objFreight));
        writer.WriteEndElement();// + "</NumberOfInvoices>" + Environment.NewLine +
        writer.WriteEndElement();                     // "</FlightChargeType>";
    }

    private string getNumberOfInvoices(InvoiceFreight[] objFreight)
    {
        try
        {
            ArrayList alDuplicate = new ArrayList();
            for (int i = 0; i < objFreight.Length; i++)
            {
                try
                {
                    if (!alDuplicate.Contains(objFreight[i].invoicesNumber))
                    {

                        alDuplicate.Add(objFreight[i].invoicesNumber);  // to remove duplicate records
                    }
                }
                catch (Exception objEx)
                {
                }
            }

            return alDuplicate.Count.ToString();
        }
        catch (Exception objEx)
        {
        }
        return "1";
    }

    private string getOtherChargesWithTotal(InvoiceFreight[] objFreight)
    {
        double dResult = 0;
        try
        {
            for (int i = 0; i < objFreight.Length; i++)
            {
                try
                {                    
                    if (objFreight[i].chargeHeadCode.ToUpper().Trim() != "INS")
                        dResult += objFreight[i].otherCharges;
                }
                catch (Exception objEx)
                {
                }
            }

            return Math.Round(dResult, 2).ToString();
        }
        catch (Exception objEx)
        {
        }
        return dResult.ToString();
    }

    private double getOtherCharges(string chargeCode, InvoiceFreight[] objFreight)
    {
        double dResult = 0;
        try
        {
            for (int i = 0; i < objFreight.Length; i++)
            {
                try
                {
                    if (objFreight[i].chargeHeadCode.ToUpper().Trim() == chargeCode.Trim().ToUpper())
                        dResult += objFreight[i].otherCharges;
                }
                catch (Exception objEx)
                {
                }
            }
            return Math.Round(dResult, 2);
        }
        catch (Exception objEx)
        {
        }
        return dResult;
    }

    private string getVAT(InvoiceFreight[] objFreight)
    {
        double dResult = 0;
        try
        {
            for (int i = 0; i < objFreight.Length; i++)
            {
                try
                {
                    dResult += Convert.ToDouble(objFreight[i].VAT);
                }
                catch (Exception objEx)
                {
                }
            }
            return Math.Round(dResult, 2).ToString();
        }
        catch (Exception objEx)
        {
        }
        return dResult.ToString();
    }

    private string getSurCharges(InvoiceFreight[] objFreight)
    {
        double dResult = 0;
        try
        {
            for (int i = 0; i < objFreight.Length; i++)
            {
                try
                {
                    dResult += Convert.ToDouble(objFreight[i].SurCharges);
                }
                catch (Exception objEx)
                {
                }
            }
            return Math.Round(dResult, 2).ToString();
        }
        catch (Exception objEx)
        {
        }
        return dResult.ToString();
    }

    private string getWeightCharge(InvoiceFreight[] objFreight)
    {
        double dResult = 0;
        try
        {
            for (int i = 0; i < objFreight.Length; i++)
            {
                try
                {
                    dResult += Convert.ToDouble(objFreight[i].weightCharge);
                }
                catch (Exception objEx)
                {
                }
            }
            return Math.Round(dResult, 2).ToString();
        }
        catch (Exception objEx)
        {
        }
        return dResult.ToString();
    }

    private string getInvoiceAmount(InvoiceFreight[] objFreight)
    {
        double dResult = 0;
        try
        {
            ArrayList alDuplicate = new ArrayList();
            for (int i = 0; i < objFreight.Length; i++)
            {
                try
                {
                    if (!alDuplicate.Contains(objFreight[i].AWBPrefix + objFreight[i].AWBNumber))
                    {
                        dResult += Convert.ToDouble(objFreight[i].invoiceAmount);
                        alDuplicate.Add(objFreight[i].AWBPrefix + objFreight[i].AWBNumber);  // to remove duplicate records
                    }
                }
                catch (Exception objEx)
                {
                }
            }

            return Math.Round(dResult, 2).ToString();
        }
        catch (Exception objEx)
        {
        }
        return dResult.ToString();
    }
}

public class InvoiceFreight
{
    public float invoiceAmount = 0.0f;
    public float weightCharge = 0.0f;
    public float otherChargesWithTotal = 0.0f;
    public float otherCharges = 0.0f;
    public String chargeHeadCode = "";
    public String SurCharges = "0";
    public String VAT = "0";
    public String AWBNumber = "0";
    public String AWBPrefix = "0";
    public String taxesWithCodes = "0";
    public int    noOfInvoices = 1;
    public string invoicesNumber = String.Empty;

    public void FillData(DataRow dtRow)
    {
        try
        {
            try
            {
                invoiceAmount = float.Parse(dtRow["FinalAmt"].ToString().Trim());
            }
            catch (Exception objEx)
            {
            }
            try
            {
                weightCharge = float.Parse(dtRow["Freight"].ToString().Trim());
            }
            catch (Exception objEx)
            {
            }
            try
            {
                SurCharges = dtRow["SurCharge"].ToString().Trim();
            }
            catch (Exception objEx)
            {
            }
            try
            {
                invoicesNumber = dtRow["InvoiceNumber"].ToString().Trim();
            }
            catch (Exception objEx)
            {
            }
            try
            {
                VAT = dtRow["Tax"].ToString().Trim();
            }
            catch(Exception objEx)
            {
            }
            try
            {               
                AWBNumber = dtRow["AWBNumber"].ToString().Trim();
                AWBPrefix = dtRow["AWBPrefix"].ToString().Trim();
            }
            catch (Exception objEx)
            {
            }
            try
            {
                //otherChargesWithTotal  = (float.Parse(dtRow["FinalAmt"].ToString().Trim()))-(float.Parse(dtRow["Total"].ToString().Trim()));
                otherCharges = otherChargesWithTotal = (float.Parse(dtRow["Charge"].ToString().Trim()));
                chargeHeadCode = dtRow["ChargeHeadCode"].ToString().Trim().Substring(0, 3);
            }
            catch (Exception objEx)
            {
            }
        }
        catch (Exception objEx)
        {
        }
    }
}

#region SAPCollection

public class SAPCollection
{
    #region DataDeclaratiion

    int accountYear = 0;
    int accountMonth = 0;
    DateTime entryDate = DateTime.Now;
    CollectionAccounts[] accounts = new CollectionAccounts[0];
    #endregion

    #region property Implementation
    public int AccountYear
    {
        set
        {
            this.accountYear = value;
        }
        get
        {
            return this.accountYear;
        }
    }

    public int AccountMonth
    {
        get
        {
            return this.accountMonth;
        }
        set
        {
            this.accountMonth = value;
        }
    }

    public DateTime EntryDate
    {
        get
        {
            return this.entryDate;
        }
        set
        {
            this.entryDate = value;
        }
    }

    public CollectionAccounts[] Accounts
    {
        get
        {
            return this.accounts;
        }
        set
        {
            this.accounts = value;
        }
    }
    #endregion

    #region public Methods


    public void FillData(DataSet dsData)
    {
        try
        {
           
            foreach (DataRow dr in dsData.Tables[0].Rows)
            {
                try
                {                  
                    
                   // depositDate = Convert.ToDateTime(dr["paymentDate"].ToString().Trim()

                       bool isDuplicate=false;
                    for(int i=0; i<accounts.Length ;i++)
                    {
                        try
                        {
                            if (accounts[i].AccountNO.ToUpper().Trim()          == dr["AgentCode"].ToString().Trim().ToUpper()  &&
                                accounts[i].PaymentTypeCode.Trim().ToUpper()    == dr["PaymentType"].ToString().Trim().ToUpper() &&
                                accounts[i].DepositDate.ToString("YYYYMMDD")    == Convert.ToDateTime(dr["paymentDate"].ToString().Trim()).ToString("YYYYMMDD"))
                            {
                                accounts[i].FillData(dr);
                                isDuplicate = true;
                                break;
                            }
                        }
                        catch(Exception objEx)
                        {
                        }
                    }

                    if (!isDuplicate)
                    {
                        CollectionAccounts tmpAcc = new CollectionAccounts();
                        tmpAcc.FillData(dr);
                        Array.Resize(ref accounts, accounts.Length + 1);
                        accounts[accounts.Length - 1] = tmpAcc;
                        EntryDate = Convert.ToDateTime(dr["PaymentDate"].ToString().Trim());
                        AccountMonth = EntryDate.Month;
                        AccountYear = Convert.ToInt16(EntryDate.ToString("yy"));
                    }
                }
                catch (Exception ObjEx)
                {
                    //ErrMdg="Error while assigning data to SAPCollection due to "+ObjEx.Message;
                }
            }
        }
        catch (Exception objEx)
        {
            //ErrMdg="Error while assigning data to SAPCollection due to "+ObjEx.Message;
        }
    }
    public String ToXML()
    {
        try
        {
            String strXML = "<SAPCollection>" + Environment.NewLine +
                            "<EntryDate>" + EntryDate.ToString() +
                            "</EntryDate>" + Environment.NewLine +
                            "<CollectionAccountingPeriods>" + Environment.NewLine +
                                "<CollectionAccountingPeriodType>" + Environment.NewLine +
                                    "<AccountingYear>" + AccountYear +
                                    "</AccountingYear>" + Environment.NewLine +
                                    "<AccountingMonth>" + accountMonth +
                                    "</AccountingMonth>" + Environment.NewLine +
                                    "<CollectionAccounts>" +
                                        getAccountsXML(accounts) +
                                    "</CollectionAccounts>" + Environment.NewLine +
                               "</CollectionAccountingPeriodType>" + Environment.NewLine +
                            "</CollectionAccountingPeriods>" + Environment.NewLine +
                            "</SAPCollection>";
            return strXML;
        }
        catch (Exception objEx)
        {
            // ErrMsg="Error while converting object into XML due to "+objEx..Message;
        }
        return null;
    }

    private string getAccountsXML(CollectionAccounts[] accounts)
    {
        try
        {
            String StrXMl = null;
            foreach (CollectionAccounts objAccount in accounts)
            {
                StrXMl += objAccount.ToXML() + Environment.NewLine;
            }
            return StrXMl;
        }
        catch (Exception objEx)
        {
            //ErrMsg="Error while geting XML string from collection due to :- "+objEx.message;
        }
        return null;
    }
    #endregion

    internal void ToXML(XmlTextWriter writer)
    {
        try
        {
            writer.WriteStartElement("SAPCollection");// +Environment.NewLine +
            writer.WriteStartElement("EntryDate");
            writer.WriteString(BalInterface.GetDateYYYMMDD(EntryDate));//.Year+"-"+EntryDate.Month+"-"+EntryDate.Day);
            writer.WriteEndElement();
            //"</EntryDate>" + Environment.NewLine +
            writer.WriteStartElement("CollectionAccountingPeriods");// +Environment.NewLine +
            writer.WriteStartElement("CollectionAccountingPeriodType");// +Environment.NewLine +
            writer.WriteStartElement("AccountingYear");
            //writer.WriteString((AccountYear<=99?"20"+AccountYear:AccountYear.ToString()));
            writer.WriteString(AccountYear.ToString());
            writer.WriteEndElement();// "</AccountingYear>" + Environment.NewLine +
            writer.WriteStartElement("AccountingMonth");
            writer.WriteString((accountMonth <= 9 ? "0" + accountMonth : accountMonth.ToString()));
            writer.WriteEndElement();// "</AccountingMonth>" + Environment.NewLine +
            writer.WriteStartElement("CollectionAccounts");
            getAccountsXML(accounts, writer);
            writer.WriteEndElement();// "</CollectionAccounts>"
            writer.WriteEndElement();// "</CollectionAccountingPeriodType>" + Environment.NewLine +
            writer.WriteEndElement();//"</CollectionAccountingPeriods>" + Environment.NewLine +
            writer.WriteEndElement();//"</SAPCollection>";
        }
        catch (Exception objEx)
        {
        }
    }

    private void getAccountsXML(CollectionAccounts[] accounts, XmlTextWriter writer)
    {
        try
        {
            foreach (CollectionAccounts objAccount in accounts)
            {
                objAccount.ToXML(writer);
            }
        }
        catch (Exception objEx)
        {
        }
    }
}

public class CollectionAccounts
{
    #region Data declaration
    String accountNo = "-";
    DateTime depositDate = DateTime.Now;
    String payementTypeCode = "0";
    string currencyCode = "0";
    double paymentAmount = 0.0;
    double creditAmount = 0.0;
    double adjustmentAmount = 0.0;
    double taxAmount = 0.0;
    #endregion

    #region Property
    public String AccountNO
    {
        get
        {
            return this.accountNo;
        }
        set
        {
            this.accountNo = value;
        }
    }
    public DateTime DepositDate
    {
        get
        {
            return this.depositDate;
        }
        set
        {
            this.depositDate = value;
        }
    }
    public string PaymentTypeCode
    {
        set
        {
            this.payementTypeCode = value;
        }
        get
        {
            return payementTypeCode;
        }
    }
    public string CurrencyCode
    {
        get
        {
            return this.currencyCode;
        }
        set
        {
            this.currencyCode = value;
        }
    }
    public double PaymentAmount
    {
        get
        {
            return this.paymentAmount;
        }
        set
        {
            this.paymentAmount = value;
        }
    }
    public double CreditAmount
    {
        get
        {
            return this.creditAmount;
        }
        set
        {
            this.creditAmount = value;
        }
    }
    public double AdjustmentAmount
    {
        get
        {
            return this.adjustmentAmount;
        }
        set
        {
            this.adjustmentAmount = value;
        }
    }
    public double TaxAmount
    {
        get
        {
            return this.taxAmount;
        }
        set
        {
            this.taxAmount = value;
        }
    }
    #endregion

    #region public Methods
    public String GetXML()
    {
        return ToXML();
    }
    public String ToXML()
    {
        try
        {
            String strXML = "<CollectionAccountType>" + Environment.NewLine +
                                "<AccountNumber>" + AccountNO +
                                "</AccountNumber>" + Environment.NewLine +
                                "<CollectionDepositDates> " + Environment.NewLine +
                                    "<CollectionDepositDateType> " + Environment.NewLine +
                                            "<DepositDate>" + DepositDate.ToShortDateString() +
                                            "</DepositDate>" + Environment.NewLine +
                                        "<CollectionPaymentTypes>" + Environment.NewLine +
                                            "<CollectionPaymentTypeType>" + Environment.NewLine +
                                                  "<PaymentTypeCode>" + PaymentTypeCode +
                                                  "</PaymentTypeCode>" + Environment.NewLine +
                                            "<CollectionCurrencies>" + Environment.NewLine +
                                                "<CollectionCurrencyType>" + Environment.NewLine +
                                                    "<CurrencyCode>" + CurrencyCode +
                                                    "</CurrencyCode> " + Environment.NewLine +
                                                    "<PaymentAmount>" + PaymentAmount +
                                                    "</PaymentAmount>" + Environment.NewLine +
                                                    "<CreditAmount>" + creditAmount +
                                                    "</CreditAmount> " + Environment.NewLine +
                                                    "<AdjustmentAmount>" + adjustmentAmount +
                                                    "</AdjustmentAmount>" + Environment.NewLine +
                                                    "<TaxAmount>" + TaxAmount +
                                                    "</TaxAmount> " + Environment.NewLine +
                                                 "</CollectionCurrencyType>" + Environment.NewLine +
                                           "</CollectionCurrencies>" + Environment.NewLine +
                                        "</CollectionPaymentTypeType>" + Environment.NewLine +
                                    "</CollectionPaymentTypes>" + Environment.NewLine +
                                "</CollectionDepositDateType> " + Environment.NewLine +
                            "</CollectionDepositDates>" + Environment.NewLine +
                   "</CollectionAccountType>";
            return strXML;
        }
        catch (Exception objEx)
        {
            //ErrMsg="Error while converting object into XML String due to :-"+objEx.Message;
        }
        return null;
    }
    public void FillData(DataRow dr)
    {
        try
        {
            accountNo = dr["AgentCode"].ToString().Trim();
            payementTypeCode = dr["PaymentType"].ToString().Trim();
            currencyCode = dr["Currency"].ToString().Trim();

            if (!String.IsNullOrEmpty(dr["PendingAmount"].ToString()))
                creditAmount += Convert.ToDouble(dr["PendingAmount"].ToString().Trim());
            if (!String.IsNullOrEmpty(dr["DCMAmount"].ToString()))
                adjustmentAmount += Convert.ToDouble(dr["DCMAmount"].ToString().Trim());
            if (!String.IsNullOrEmpty(dr["TDS"].ToString()))
                taxAmount += Convert.ToDouble(dr["TDS"].ToString().Trim());
            if (!String.IsNullOrEmpty(dr["CollectedAmount"].ToString()))
                paymentAmount += Convert.ToDouble(dr["CollectedAmount"].ToString().Trim());
            depositDate = Convert.ToDateTime(dr["paymentDate"].ToString().Trim());
        }
        catch (Exception objEx)
        {
        }
    }
    #endregion

    internal void ToXML(XmlTextWriter writer)
    {
        try
        {
            writer.WriteStartElement("CollectionAccountType");
            writer.WriteStartElement("AccountNumber");
            writer.WriteString(AccountNO);
            writer.WriteEndElement();                        //"</AccountNumber>" + Environment.NewLine +
            writer.WriteStartElement("CollectionDepositDates");
            writer.WriteStartElement("CollectionDepositDateType");
            writer.WriteStartElement("DepositDate");
            writer.WriteString(BalInterface.GetDateYYYMMDD(DepositDate));//.Year + "-" + DepositDate.Month + "-" + DepositDate.Day);
            writer.WriteEndElement();// "</DepositDate>" + Environment.NewLine +
            writer.WriteStartElement("CollectionPaymentTypes");
            writer.WriteStartElement("CollectionPaymentTypeType");
            writer.WriteStartElement("PaymentTypeCode");
            writer.WriteString(PaymentTypeCode);
            writer.WriteEndElement();//       "</PaymentTypeCode>" + Environment.NewLine +
            writer.WriteStartElement("CollectionCurrencies");
            writer.WriteStartElement("CollectionCurrencyType");// +Environment.NewLine +
            writer.WriteStartElement("CurrencyCode");
            writer.WriteString(CurrencyCode.ToString());
            writer.WriteEndElement();// "</CurrencyCode> " + Environment.NewLine +
            writer.WriteStartElement("PaymentAmount");
            writer.WriteString(PaymentAmount.ToString());
            writer.WriteEndElement();// +Environment.NewLine +
            writer.WriteStartElement("CreditAmount");
            writer.WriteString(creditAmount.ToString());
            writer.WriteEndElement();// "</CreditAmount> " + Environment.NewLine +
            writer.WriteStartElement("AdjustmentAmount");
            writer.WriteString(adjustmentAmount.ToString());
            writer.WriteEndElement();// "</AdjustmentAmount>" + Environment.NewLine +
            writer.WriteStartElement("TaxAmount");
            writer.WriteString(TaxAmount.ToString());
            writer.WriteEndElement();//("TaxAmount> " + Environment.NewLine +
            writer.WriteEndElement();//        "</CollectionCurrencyType>" + Environment.NewLine +
            writer.WriteEndElement();//"</CollectionCurrencies>" + Environment.NewLine +
            writer.WriteEndElement();//"</CollectionPaymentTypeType>" + Environment.NewLine +
            writer.WriteEndElement();//"</CollectionPaymentTypes>" + Environment.NewLine +
            writer.WriteEndElement();//"</CollectionDepositDateType> " + Environment.NewLine +
            writer.WriteEndElement();//"</CollectionDepositDates>" + Environment.NewLine +
            writer.WriteEndElement();//"</CollectionAccountType>";
        }
        catch (Exception objEx)
        {
        }
    }
}

#endregion

#region SAPFlightManifest

public class SAPManifest
{
    DateTime flightManifestDate = DateTime.Now;
    SAPFlightInfo[] flights = new SAPFlightInfo[0];

    #region Property

    public DateTime FlightManifestDate
    {
        get
        {
            return this.flightManifestDate;
        }
        set
        {
            this.flightManifestDate = value;
        }
    }
    public SAPFlightInfo[] Flights
    {
        get
        {
            return this.flights;
        }
        set
        {
            this.flights = value;
        }
    }
    #endregion

    #region public methods

    public String ToXML()
    {

        try
        {
            String strXML = "<SAPFlightManifest>" + Environment.NewLine +
                                 "<FlightManifestDate>" + FlightManifestDate + Environment.NewLine +
                                 "</FlightManifestDate>" + Environment.NewLine +
                                 "<Flights>" + getXMLFromflights(flights) + Environment.NewLine +
                                 "</Flights>" + Environment.NewLine +
                             "</SAPFlightManifest>";
            return strXML;
        }
        catch (Exception objEx)
        {
            //ErrMsg=" Error while converting itto XML due to "+objEx.message;
        }
        return null;
    }

    public void FillData(DataSet dsData)
    {
        try
        {            
            foreach (DataRow dr in dsData.Tables[0].Rows)
            {
                try
                {
                    bool isDUplicate=false;
                    for (int i = 0; i < flights.Length;i++ )
                    {
                        if (flights[i].FlightNumber == dr["FltNo"].ToString().Trim() &&
                            flights[i].FlightDate.ToString("YYYYMMDD") == Convert.ToDateTime(dr["Date"].ToString().Trim()).ToString("YYYYMMDD"))
                        {
                            isDUplicate = true;
                            break;
                        }
                    }
                    if (!isDUplicate)
                    {
                        SAPFlightInfo objAcc = new SAPFlightInfo();
                        objAcc.FillData(dr);
                        Array.Resize(ref flights, flights.Length + 1);
                        flights[flights.Length - 1] = objAcc;
                        FlightManifestDate = Convert.ToDateTime(dr["Date"].ToString().Trim());
                    }
                }
                catch (Exception objEx)
                {
                }
            }


        }
        catch (Exception objEx)
        {
        }

    }

    private string getXMLFromflights(SAPFlightInfo[] flights)
    {
        try
        {
            String strXML = "";
            foreach (SAPFlightInfo objInfo in flights)
            {
                strXML += objInfo.ToXML() + Environment.NewLine;
            }
            return strXML;
        }
        catch (Exception objEx)
        {
            //ErrMsg="Error while converting array to XML due to : "+objEx.Message;
        }
        return null;
    }
    #endregion

    internal void ToXML(XmlTextWriter writer)
    {
        try
        {
            writer.WriteStartElement("SAPFlightManifest");
            writer.WriteStartElement("FlightManifestDate");
            writer.WriteString(BalInterface.GetDateYYYMMDD(FlightManifestDate));//.Year + "-" + FlightManifestDate.Month + "-" + FlightManifestDate.Day);
            writer.WriteEndElement();                    //"</FlightManifestDate>" + Environment.NewLine +
            writer.WriteStartElement("Flights");
            getXMLFromflights(flights, writer);
            writer.WriteEndElement();// "</Flights>" + Environment.NewLine +
            writer.WriteEndElement();// "</SAPFlightManifest>";            
        }
        catch (Exception objEx)
        {
        }
    }

    private void getXMLFromflights(SAPFlightInfo[] flights, XmlTextWriter writer)
    {
        try
        {

            foreach (SAPFlightInfo objInfo in flights)
            {
                objInfo.ToXML(writer);
            }

        }
        catch (Exception objEx)
        {
            //ErrMsg="Error while converting array to XML due to : "+objEx.Message;
        }
    }
}

public class SAPFlightInfo
{
    #region Data Declaration
    String flightNumber = String.Empty;
   public  DateTime FlightDate ;
    String aircraftType = String.Empty;
    String tailNumber = String.Empty;

    // String flightWeightCurrencies   = String.Empty;
    // String flightWeightCurrencyType = String.Empty;
    String currencyCode = "0";
    double weight = 0.0;
    double weightCharge = 0.0;
    #endregion

    #region Property
    public String FlightNumber
    {
        get
        {
            return this.flightNumber;
        }
        set
        {
            this.flightNumber = value;
        }
    }

    public String AircraftType
    {
        get
        {
            return this.aircraftType;
        }
        set
        {
            this.aircraftType = value;
        }

    }

    public String TailNumber
    {
        get
        {
            return this.tailNumber;
        }
        set
        {
            this.tailNumber = value;
        }
    }

    public string CurrencyCode
    {
        get
        {
            return this.currencyCode;
        }
        set
        {
            this.currencyCode = value;
        }
    }

    public double Weight
    {
        get
        {
            return this.weight;
        }
        set
        {
            this.weight = value;
        }
    }

    public double WeightCharge
    {
        get
        {
            return this.weightCharge;
        }
        set
        {
            this.weightCharge = value;
        }
    }

    #endregion

    #region Public methods

    public String ToXML()
    {
        try
        {
            string AirlineCode = FlightNumber.Substring(0, 2);
            string FltNumber = FlightNumber.Replace(AirlineCode, "");

            String strXML = " <FlightManifestFlightype> " + Environment.NewLine +
                                    "<AirlineCode>" + AirlineCode +
                                    "</AirlineCode>" + Environment.NewLine +
                                    "<FlightNumber>" + FlightNumber +
                                    "</FlightNumber>" + Environment.NewLine +
                                    " <AircaftType>" + AircraftType +
                                    "</AircaftType> " + Environment.NewLine +
                                    "<Tailnumber>" + TailNumber +
                                    "</Tailnumber>" + Environment.NewLine +
                                    "<FlightWeightCurrencies>" + Environment.NewLine +
                                        "<FlightWeightCurrencyType>" + Environment.NewLine +
                                            "<CurrencyCode>" + CurrencyCode +
                                            "</CurrencyCode>" + Environment.NewLine +
                                            "<Weight>" + Weight +
                                            "</Weight>" + Environment.NewLine +
                                            "<WeightCharge>" + WeightCharge +
                                            "</WeightCharge>" + Environment.NewLine +
                                        "</FlightWeightCurrencyType>" + Environment.NewLine +
                                   "</FlightWeightCurrencies>" + Environment.NewLine +
                            "</FlightManifestFlightype>";
            return strXML;
        }
        catch (Exception objEx)
        {
            //ErrMsg="Error while converting in to XML due to :- "+objEx.Message;
        }
        return null;
    }

    public void FillData(DataRow dtRow)
    {
        try
        {
            FlightNumber = dtRow["FltNo"].ToString();
            AircraftType = dtRow["AirCraftType"].ToString();
            tailNumber = dtRow["TailNo"].ToString();
            CurrencyCode = dtRow["Currency"].ToString().Trim();

            try
            {
                Weight = Convert.ToDouble(dtRow["GrossWgt"].ToString().Trim());
                WeightCharge = Convert.ToDouble(dtRow["Freight"].ToString().Trim());
            }
            catch (Exception objEx)
            {
            }
            try
            {
                FlightDate = Convert.ToDateTime(dtRow["Date"].ToString().Trim());
            }
            catch(Exception objEx)
            {
            }
        }
        catch (Exception objEx)
        {
        }

    }

    #endregion

    internal void ToXML(XmlTextWriter writer)
    {
        try
        {
            string AirlineCode = FlightNumber.Substring(0, 2);
            string FltNumber = FlightNumber.Replace(AirlineCode, "");

            writer.WriteStartElement("FlightManifestFlightype");
            writer.WriteStartElement("AirlineCode");
            writer.WriteString(AirlineCode);
            writer.WriteEndElement();
            writer.WriteStartElement("FlightNumber");
            writer.WriteString(FltNumber);
            writer.WriteEndElement();// "</FlightNumber>" + Environment.NewLine +
            writer.WriteStartElement("AircaftType");
            writer.WriteString(AircraftType);
            writer.WriteEndElement();// "</AircaftType> " + Environment.NewLine +
            writer.WriteStartElement("Tailnumber");
            writer.WriteString(TailNumber);
            writer.WriteEndElement();// "</Tailnumber>" + Environment.NewLine +
            writer.WriteStartElement("FlightWeightCurrencies");// +Environment.NewLine +
            writer.WriteStartElement("FlightWeightCurrencyType");// +Environment.NewLine +
            writer.WriteStartElement("CurrencyCode");
            writer.WriteString(CurrencyCode);
            writer.WriteEndElement();
            // "</CurrencyCode>" + Environment.NewLine +
            writer.WriteStartElement("Weight");
            writer.WriteString(Weight.ToString());
            writer.WriteEndElement();// "</Weight>" + Environment.NewLine +
            writer.WriteStartElement("WeightCharge");
            writer.WriteString(WeightCharge.ToString());
            writer.WriteEndElement(); //         "</WeightCharge>" + Environment.NewLine +
            writer.WriteEndElement(); //     "</FlightWeightCurrencyType>" + Environment.NewLine +
            writer.WriteEndElement(); //"</FlightWeightCurrencies>" + Environment.NewLine +
            writer.WriteEndElement(); //"</FlightManifestFlightype>";

        }
        catch (Exception objex)
        {
        }
    }
}

class Constant
{
    public static String BODY_TYPE_WIDE = "WIDE";
    public static String BODY_TYPE_NARROW = "NARROW";
}

#endregion

#endregion

    #region Navitire

    public class Navitire
    {
        String gLogingUserName = "";
        const int PAX_WT = 75;
        const int PAX_COUNT = 200;
        const int CARGO_CAPACITY = 200;

        public Navitire()
        {
        }
        public Navitire(String LogingUserName)
        {
            gLogingUserName = LogingUserName;
        }

        #region public Methods

        public void UpdateData(NavitireParamInfo objParam)
        {
            try
            {
                //NavitireParamInfo objParam=new NavitireParamInfo();
                NavitireOutData objOutData = readData(objParam);
                updateDatabase(objOutData);
            }
            catch (Exception objEx)
            {
                //Err= "Error while Updating the data due to:- "+objEx.Message;
            }
        }


        public NavitireOutData UpdateData(NavitireParamInfo objParam, String XMLString)
        {
            NavitireOutData objOut = null;
            try
            {
                // NavitireParamInfo objParam = new NavitireParamInfo();
                objOut                  = readDataFromXML(XMLString, objParam);
                objOut.FlightNumber     = objParam.FlightNumber;
                objOut.CarrierCode      = objParam.CarrierCode;
                objOut.DepartureStation = objParam.DepartureStation;
                objOut.ArrivalStation   = objParam.ArrivalStation;
                objOut.DepartureDate    = objParam.DepartureDate;
                updateDatabase(objOut);
            }
            catch (Exception objEx)
            {
                //Err= "Error while Updating the data due to:- "+objEx.Message;
            }
            return objOut;
        }



        #endregion



        #region prtivate Methods

        private NavitireOutData readData(NavitireParamInfo objParam)
        {

            NavitireOutData objOut = new NavitireOutData();
            try
            {


                //         '114',--@FltNo varchar(20),            
                //'9W',--@CarrierCode varchar(5),         
                //'BOM',--@DepStation  varchar(30),
                //'DEL',--@ArrStation  varchar(30), 
                //'05/22/2013',--@FlightDate  date,
                //'10',--@BxPxCt int,
                //'10',--@BxPxWt float,
                //'10',--@CxPxCt int,
                //'10',--@CxPxWt float,
                //'10',--@CxPxbCt int,
                //'10',--@CxPxbWt float,
                //'10',--@FltStatus varchar(30),
                //'10/10/2013',--@FltDepInfo date,
                //'10'--@UpdCarCap float

                objOut.FlightNumber = objParam.FlightNumber;
                objOut.CarrierCode = objParam.CarrierCode;
                objOut.DepartureStation = objParam.DepartureStation;
                objOut.ArrivalStation = objParam.ArrivalStation;
                objOut.DepartureDate = objParam.DepartureDate;

                // objOut.BookedPaxCnt = "100";
                //objOut.BookedPaxWeight = BookedPaxCnt*PAX_WT;

                //GetPaxManifest getPaxManifest = new GetPaxManifest();
                //getPaxManifest.PaxManifestAuth = new PaxManifestHeader();
                //getPaxManifest.PaxManifestAuth.AuthUser = "paxmanifestUser";
                //getPaxManifest.PaxManifestAuth.SecurityKey = "A@th";
                //txtResult.Text = getPaxManifest.GetManifest(txtDepartureDate.Text, txtDepartureStation.Text, txtArrivalStation.Text, txtFlightNumber.Text);

            }
            catch (Exception objEx)
            {
                //Err="Error while readingData due to :-"+objEx.Message
            }
            return objOut;
        }


        private NavitireOutData readDataFromXML(string XMLString, NavitireParamInfo objParam)
        {
            NavitireOutData objOut = new NavitireOutData();
            try
            {
                //string myXML = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Manifest><MaleBooked>31</MaleBooked> <FemaleBooked>31</FemaleBooked> <ChildrenBooked>10</ChildrenBooked> <InfantRevBooked>0</InfantRevBooked><InfantNonRevBooked>0</InfantNonRevBooked> <ChildrenNonRevBooked>10</ChildrenNonRevBooked> <AdultNonRevBooked>0</AdultNonRevBooked> <TotalPaxCountBooked>72</TotalPaxCountBooked> <MaleCheckIn>30</MaleCheckIn> <FemaleCheckIn>30</FemaleCheckIn> <ChildrenCheckIn>10</ChildrenCheckIn> <InfantRevCheckIn>0</InfantRevCheckIn> <InfantNonRevCheckIn>0</InfantNonRevCheckIn> <ChildrenNonRevCheckIn>10</ChildrenNonRevCheckIn> <AdultNonRevCheckIn>0</AdultNonRevCheckIn> <TotalPaxCountCheckIn>0</TotalPaxCountCheckIn> <BaggagePcs>26</BaggagePcs> <BaggageWeight>279</BaggageWeight><ExcessBaggageWeight>0</ExcessBaggageWeight><FlightStatus>Closed</FlightStatus></Manifest>";
                //string myXML = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + XMLString;
                string myXML =  XMLString;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(myXML);

                try
                {
                    int BxPxCt = 0;
                    //   objOut.BookedPaxCnt = (int)((xmlDoc.SelectSingleNode("Manifest/MaleBooked")).Value) + (int)(xmlDoc.SelectSingleNode("Manifest/FemaleBooked")).Value(xmlDoc.SelectSingleNode(xpath)).Value + (int)(xmlDoc.SelectSingleNode("Manifest/ChildrenBooked")).Value;
                    objOut.BookedPaxCnt = Convert.ToInt16(((xmlDoc.SelectSingleNode("Manifest/TotalPaxCountBooked")).ChildNodes[0].Value));
                    objOut.BookedPaxWeight = objOut.BookedPaxCnt * PAX_WT;
                }
                catch (Exception objEx)
                {
                }
                try
                {
                    objOut.CheckInPaxCnt = Convert.ToInt16(((xmlDoc.SelectSingleNode("Manifest/TotalPaxCountCheckIn")).ChildNodes[0].Value));
                    objOut.CheckInPaxWeight = objOut.CheckInPaxCnt * PAX_WT;
                }
                catch (Exception objEx)
                {
                }


                try
                {
                    objOut.FltStatus = (((xmlDoc.SelectSingleNode("Manifest/FlightStatus")).ChildNodes[0].Value));
                }
                catch (Exception objEx)
                {
                }

                try
                {
                    objOut.BagsCnt = Convert.ToInt16(((xmlDoc.SelectSingleNode("Manifest/BaggagePcs")).ChildNodes[0].Value));
                }
                catch (Exception objEx)
                {
                }
                try
                {
                    objOut.BagsWeight = Convert.ToInt16(((xmlDoc.SelectSingleNode("Manifest/BaggageWeight")).ChildNodes[0].Value));
                }
                catch (Exception objEx)
                {
                }

                try
                {
                    

                    BalInterface objBalInterface = new BalInterface();
                    object[] objQueryVal = 
                                    {    objParam.DepartureStation, objParam.ArrivalStation, objParam.CarrierCode, objParam.FlightNumber, objParam.DepartureDate
                                   };
                    DataSet ds = objBalInterface.RaedPaxCountWeight(objQueryVal);

                    int   paxCnt = PAX_COUNT;
                    float cargoCapacity = CARGO_CAPACITY;
                    try
                    {
                        paxCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["PaxCount"].ToString());
                    }
                    catch(Exception objEx)
                    {
                    }
                    try
                    {
                        cargoCapacity = float.Parse(ds.Tables[0].Rows[0]["CargoCapacity"].ToString());
                    }
                    catch (Exception objEx)
                    {
                    }
                    objOut.UpdCarCap = (paxCnt - objOut.CheckInPaxCnt) * PAX_WT + cargoCapacity;  // Formula (PaxCount -  CxPxCt) * 75Kg

                }
                catch (Exception objEx)
                {
                }

            }
            catch (Exception objEx)
            {
            }
            return objOut;
        }

        private void updateDatabase(NavitireOutData objData)
        {
            try
            {
                BalInterface objBalInterface = new BalInterface();
                object[] objQueryVal = 
                                    {   objData.FlightNumber ,
                                        objData.CarrierCode,
                                        objData.DepartureStation   , 
                                        objData.ArrivalStation   ,    
                                        objData.DepartureDate   ,    
                                        objData.BookedPaxCnt   ,    
                                        objData.BookedPaxWeight   ,    
                                        objData.CheckInPaxCnt  ,    
                                        objData.CheckInPaxWeight  ,    
                                        objData.BagsCnt  ,    
                                        objData.BagsWeight  ,    
                                        objData.FltStatus ,    
                                        objData.FltDepInfo ,   
                                        objData.UpdCarCap ,
                                        gLogingUserName,
                                        DateTime.Now
                                   };
                objBalInterface.UpdateNavitireData(objQueryVal);
            }
            catch (Exception objEx)
            {
                //Err="Error while updating data due to :- "+objEx.Message;
            }
        }


        #endregion



    }


    public class NavitireParamInfo
    {
        public string DepartureDate = "";
        public string DepartureStation = "";
        public string ArrivalStation = "";
        public string FlightNumber = "";
        public string CarrierCode = "";
        public string UserName = "";
        public string Password = "";
    }

    public class NavitireOutData
    {

        public string DepartureDate = "";
        public string DepartureStation = "";
        public string ArrivalStation = "";
        public string FlightNumber = "";
        public string CarrierCode = "";
        public int BookedPaxCnt = 0;
        public double BookedPaxWeight = 0;
        public int CheckInPaxCnt = 0;
        public double CheckInPaxWeight = 0;
        public int BagsCnt = 0;
        public double BagsWeight = 0;
        public String FltStatus = "";
        public DateTime FltDepInfo;
        public float UpdCarCap = 0;
    }

    #endregion

}
