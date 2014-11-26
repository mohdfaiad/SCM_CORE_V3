using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using BAL;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;

using System.Collections.Generic;
using Excel;
using ICSharpCode;
using System.Drawing;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class UploadRates : System.Web.UI.Page
    {
        string logPath = string.Empty;
        string logfilename = string.Empty;
        DateTime dtCurrentDate = new DateTime();
        string strUserName = "";
        excelUpload obj = new excelUpload();
        string path = ConfigurationManager.AppSettings["PaxLoadPath"].ToString();
        String FileExtension = string.Empty;
        String filePath = string.Empty;
        DataSet dsupload = new DataSet();
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer da = new SQLServer(Global.GetConnectionString().ToString());
        bool flag;
        string uploadsheet = "";
        UploadMastersBAL objUpload = new UploadMastersBAL();
        string strflag = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["IT"] == null)
                {
                    return;
                }
                dtCurrentDate = (DateTime)Session["IT"];
                strUserName = Convert.ToString(Session["UserName"]);
                dsupload = da.SelectRecords("Sp_GetUploadMaster");

                if (dsupload != null)
                {
                    if (dsupload.Tables != null)
                    {
                        if (dsupload.Tables.Count > 0)
                        {
                            if (dsupload.Tables[0].Rows.Count > 0)
                            {

                                for (int i = 0; i < dsupload.Tables[0].Rows.Count; i++)
                                {
                                    string loader = dsupload.Tables[0].Rows[i]["UplaodSheet"].ToString();

                                    string isload = "True";
                                    DataView dv = new DataView(dsupload.Tables[0]);
                                    dv.RowFilter = "Uplaodsheet = '" + loader + "'";
                                    DataTable dvv = new DataTable();
                                    dvv = dv.ToTable();
                                    if (loader == "Rates")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkRates.Checked = true;
                                    if (loader == "AirlineSchedule")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkAirschedule.Checked = true;
                                    if (loader == "ExchangeRete")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkExcRates.Checked = true;
                                    if (loader == "Agents")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkAgents.Checked = true;
                                    if (loader == "ShipperConsignee")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkshipCon.Checked = true;
                                    if (loader == "OC")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkOC.Checked = true;
                                    if (loader == "Airport")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkAirport.Checked = true;
                                    if (loader == "Partners")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkPartners.Checked = true;
                                    if (loader == "PartnersSchecule")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkPartnersch.Checked = true;
                                    if (loader == "GLAccount")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkGLAccount.Checked = true;
                                    if (loader == "Budget")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkBudget.Checked = true;

                                    chkAgents.Enabled = false;
                                    chkAirport.Enabled = false;
                                    chkAirschedule.Enabled = false;
                                    chkBudget.Enabled = false;
                                    chkExcRates.Enabled = false;
                                    chkGLAccount.Enabled = false;
                                    chkOC.Enabled = false;
                                    chkPartners.Enabled = false;
                                    chkPartnersch.Enabled = false;
                                    chkRates.Enabled = false;
                                    chkshipCon.Enabled = false;


                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnUploadBudget_Click(object sender, EventArgs e)
        {
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (BudgetFileUpload.HasFile)
                {
                    Session["WorkingFile"] = BudgetFileUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (BudgetFileUpload.FileName.Contains("-"))
                    {
                        lblError.Text = "Please remove - sign from file name";
                        return;
                    }
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please select appropriate file to upload.....";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    FileInfo fi = new FileInfo(Server.MapPath(path + BudgetFileUpload.FileName));

                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, BudgetFileUpload.FileName);
                    BudgetFileUpload.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                    //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + BudgetFileUpload.FileName);
                    if (!LoadBudgetExcelData(FileExtension, filePath))
                    {
                        if (strflag == "true")
                        {
                            lblError.Text = "File not Processed, Check log file for details.";
                            fi = new FileInfo(Server.MapPath(path + BudgetFileUpload.FileName));
                            fileExists = fi.Exists;
                            if (fileExists == true)
                            {
                                fi.Delete();
                            }
                        }
                        else
                        {
                            lblError.Text = "Please choose proper format";
                            lblError.ForeColor = System.Drawing.Color.Green;
                            fi.Delete();
                        }
                    }
                    else
                    {
                        lblError.Text = "File Uploaded Successfully.";
                        lblError.ForeColor = System.Drawing.Color.Green;
                        fi.Delete();

                    }
                    //btnUpload.Visible = false;
                    //btnDownload.Visible = true;
                    //FileExcelUpload.Enabled = false;
                }
                else
                {
                    lblError.Text = "Cannot accept files of this type.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    //btnUpload.Visible = true;
                    //btnDownload.Visible = false;
                    //FileExcelUpload.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }

        private bool LoadBudgetExcelData(string ext, string filepath)
        {
            string logPath = string.Empty;
            string logfilename = string.Empty;
            DateTime dtCurrentDate = new DateTime();
            string strUserName = "";
            //excelUpload obj = new excelUpload();


            FileInfo fics = new FileInfo(Server.MapPath(path + BudgetFileUpload.FileName + "_Log" + ".txt"));
            bool fileExists = fics.Exists;

            if (fileExists == true)
            {
                fics.Delete();
            }


            logfilename = BudgetFileUpload.FileName + "_Log" + ".txt";
            logPath = Server.MapPath(path + logfilename);
            Session["logpath"] = logPath;
            Session["logfilename"] = logfilename;
            string connString = string.Empty;

            FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = null;
            if (ext == ".xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //...
            }
            else if (ext == ".xlsx")
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
            }
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = new DataSet();
            result = excelReader.AsDataSet();

            //...
            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result1 = excelReader.AsDataSet();

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();

            bool IsInsert = false;

            try
            {

                DataSet dsPO = new DataSet();
                //oleda.Fill(dsPO, "Order");
                dsPO = result1.Copy();
                if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                    {
                        if (String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][0].ToString()))
                        {
                            continue;
                        }
                        string Region = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        string Market = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                        string FlightNo = (string)(dsPO.Tables[0].Rows[i][2].ToString());
                        string Origin = (string)(dsPO.Tables[0].Rows[i][3].ToString());
                        string Destination = (string)(dsPO.Tables[0].Rows[i][4].ToString());
                        string Year = (string)(dsPO.Tables[0].Rows[i][5].ToString());
                        string Month = (string)(dsPO.Tables[0].Rows[i][6].ToString());
                        string AC_Type = (string)(dsPO.Tables[0].Rows[i][7].ToString());
                        string FLT_INDIC = (string)(dsPO.Tables[0].Rows[i][8].ToString());
                        string POS = (string)(dsPO.Tables[0].Rows[i][9].ToString());
                        int NO_OF_FLIGHT = 0;
                        if (dsPO.Tables[0].Rows[i][10].ToString() != null || dsPO.Tables[0].Rows[i][10].ToString() != "")
                            NO_OF_FLIGHT = Convert.ToInt32(dsPO.Tables[0].Rows[i][10].ToString());
                        string UoM = (string)(dsPO.Tables[0].Rows[i][11].ToString());
                        string Currency = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                        int Cargo_per_Flight = 0;
                        if (dsPO.Tables[0].Rows[i][13].ToString() != "" || dsPO.Tables[0].Rows[i][13].ToString() != null)
                            Cargo_per_Flight = Convert.ToInt32((dsPO.Tables[0].Rows[i][13].ToString()));

                        int PO_Mail_per_Flight = 0;
                        if (dsPO.Tables[0].Rows[i][14].ToString() != null || dsPO.Tables[0].Rows[i][14].ToString() != "")
                            PO_Mail_per_Flight = Convert.ToInt32((dsPO.Tables[0].Rows[i][14].ToString()));

                        int Courier_per_Flight = 0;
                        if (dsPO.Tables[0].Rows[i][15].ToString() != null || dsPO.Tables[0].Rows[i][15].ToString() != "")
                            Courier_per_Flight = Convert.ToInt32((dsPO.Tables[0].Rows[i][15].ToString()));
                        decimal Cargo_Rate = 0;
                        if (dsPO.Tables[0].Rows[i][16].ToString() != null || dsPO.Tables[0].Rows[i][16].ToString() != "")
                            Cargo_Rate = Convert.ToDecimal((dsPO.Tables[0].Rows[i][16].ToString()));

                        int PO_Mail_rate = 0;
                        if (dsPO.Tables[0].Rows[i][17].ToString() != null || dsPO.Tables[0].Rows[i][17].ToString() != "")
                            PO_Mail_rate = Convert.ToInt32((dsPO.Tables[0].Rows[i][17].ToString()));

                        decimal Courier_Rate = 0;
                        if (dsPO.Tables[0].Rows[i][18].ToString() != null || dsPO.Tables[0].Rows[i][18].ToString() != "")
                            Courier_Rate = Convert.ToDecimal((dsPO.Tables[0].Rows[i][18].ToString()));


                        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                        SQLServer da = new SQLServer(constr);

                        try
                        {
                            IsInsert = da.InsertData("insert into FlightBudget(Region,Market,FlightNo,Origin,Destination,Year,Month,AC_Type,FLT_INDIC,POS,No_Of_Flights,UoM,Currency,Cargo_per_Flight,PO_Mail_per_Flight,Courier_per_Flight,Cargo_Rate,PO_Mail_rate,Courier_Rate) values('" + Region + "','" + Market + "','" + FlightNo + "','" + Origin + "','" + Destination + "'," + Year + ",'" + Month + "','" + AC_Type + "','" + FLT_INDIC + "','" + POS + "'," + NO_OF_FLIGHT + ",'" + UoM + "','" + Currency + "'," + Cargo_per_Flight + "," + PO_Mail_per_Flight + "," + Courier_per_Flight + "," + Cargo_Rate + "," + PO_Mail_rate + "," + Courier_Rate + ")");
                            //da.InsertData("insert into tblProrateMaster(OriginCode,DestCode,ProrateFactor,isActive) values('" + Origin + "','" + Dest + "'," + Prorate + ",'true')");
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                if (IsInsert == true)
                {

                    flag = true;
                    uploadsheet = "Budget";
                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                }
                strflag = "true";
                return IsInsert;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //Partner Excel
        protected void btnPartnerUpload_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (PartnerFileUpload.HasFile)
                {
                    Session["WorkingFile"] = PartnerFileUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (PartnerFileUpload.FileName.Contains("-"))
                    {
                        lblError.Text = "Please remove - sign from file name";
                        return;
                    }
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please select a proper file to upload..";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    FileInfo fi = new FileInfo(Server.MapPath(path + PartnerFileUpload.FileName));

                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, PartnerFileUpload.FileName);
                    PartnerFileUpload.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                    //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + PartnerFileUpload.FileName);
                    if (LoadPartnerExcelData(FileExtension, filePath) == true)
                    {
                        lblError.Text = "File Uploaded Successfully.";
                        lblError.ForeColor = System.Drawing.Color.Green;
                        fi.Delete();
                    }
                    else
                    {
                        if (lblError.Text == "")
                            lblError.Text = "File not Processed...Please check for valid data entry";
                        lblError.ForeColor = Color.Red;
                        fi = new FileInfo(Server.MapPath(path + PartnerFileUpload.FileName));
                        fileExists = fi.Exists;
                        if (fileExists == true)
                        {
                            fi.Delete();
                        }
                    }
                    //btnUpload.Visible = false;
                    //btnDownload.Visible = true;
                    //FileExcelUpload.Enabled = false;
                }
                else
                {
                    lblError.Text = "Cannot accept files of this type.";
                    //btnUpload.Visible = true;
                    //btnDownload.Visible = false;
                    //FileExcelUpload.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }
        private bool LoadPartnerExcelData(string ext, string filepath)
        {
            //AirlineMasterBAL airbal = new AirlineMasterBAL();
            UploadMastersBAL objUpload = new UploadMastersBAL();
            object[] QueryValues = new object[35];
            string logPath = string.Empty;
            string logfilename = string.Empty;
            DateTime dtCurrentDate = new DateTime();
            string strUserName = "";
            //excelUpload obj = new excelUpload();


            FileInfo fics = new FileInfo(Server.MapPath(path + PartnerFileUpload.FileName + "_Log" + ".txt"));
            bool fileExists = fics.Exists;

            if (fileExists == true)
            {
                fics.Delete();
            }


            logfilename = PartnerFileUpload.FileName + "_Log" + ".txt";
            logPath = Server.MapPath(path + logfilename);
            Session["logpath"] = logPath;
            Session["logfilename"] = logfilename;
            string connString = string.Empty;

            FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = null;
            if (ext == ".xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //...
            }
            else if (ext == ".xlsx")
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
            }
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = new DataSet();
            result = excelReader.AsDataSet();

            //...
            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result1 = excelReader.AsDataSet();

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();

            try
            {
                excelUpload objBAl = new excelUpload();
                bool IsInsert = false;
                DataSet dsPO = new DataSet();
                //oleda.Fill(dsPO, "Order");
                dsPO = result1.Copy();
                if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                    {
                        bool blnError = false;
                        bool blnInsert = true;

                        if (String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][0].ToString()))
                        {
                            continue;
                        }
                        string PartnerName = (string)(dsPO.Tables[0].Rows[i][7].ToString());
                        if (PartnerName == "")
                        {

                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[0] = PartnerName;

                        string PartnerLegalName = (string)(dsPO.Tables[0].Rows[i][20].ToString());
                        if (PartnerLegalName == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[1] = PartnerLegalName;

                        string PartnerPrefix = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        if (PartnerPrefix == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[2] = PartnerPrefix;

                        string DesignatorCode = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                        if (DesignatorCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[3] = DesignatorCode;

                        string PartnerLocationID = (string)(dsPO.Tables[0].Rows[i][3].ToString());
                        if (PartnerLocationID == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[4] = PartnerLocationID;

                        string PartnerAccountingCode = (string)(dsPO.Tables[0].Rows[i][6].ToString());
                        if (PartnerAccountingCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[5] = PartnerAccountingCode;

                        string RegistrastionID = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                        if (RegistrastionID == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[6] = RegistrastionID;

                        string DigitalSignature = (string)(dsPO.Tables[0].Rows[i][8].ToString());
                        // bool digsign = true;
                        if (DigitalSignature.ToLower() == "true" || DigitalSignature.ToLower() == "y" || DigitalSignature == "1")
                        {
                            // digsign =1;
                            QueryValues[7] = 1;
                        }
                        else if (DigitalSignature.ToLower() == "false" || DigitalSignature == "0" || DigitalSignature.ToLower() == "n")
                        {
                            //digsign = 0;
                            QueryValues[7] = 0;
                        }
                        else
                            QueryValues[7] = 0;

                        string Suspend = (string)(dsPO.Tables[0].Rows[i][9].ToString());
                        if (Suspend.ToLower() == "true" || Suspend.ToLower() == "y")
                            QueryValues[8] = 1;
                        if (Suspend.ToLower() == "false" || Suspend.ToLower() == "n")
                            QueryValues[8] = 0;

                        string President = (string)((dsPO.Tables[0].Rows[i][15].ToString()));
                        QueryValues[9] = President;

                        string CFO = (string)((dsPO.Tables[0].Rows[i][16].ToString()));
                        QueryValues[10] = CFO;

                        string CurrencyListing = (string)(dsPO.Tables[0].Rows[i][5].ToString());
                        if (CurrencyListing == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[11] = CurrencyListing;

                        string Language = (string)(dsPO.Tables[0].Rows[i][10].ToString());
                        if (Language == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[12] = Language;

                        string TaxRegistrationID = (string)(dsPO.Tables[0].Rows[i][13].ToString());
                        if (TaxRegistrationID == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[13] = TaxRegistrationID;

                        string AdditionalTaxRegistrationID = (string)(dsPO.Tables[0].Rows[i][14].ToString());
                        if (AdditionalTaxRegistrationID == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[14] = AdditionalTaxRegistrationID;


                        string SettlementMethod = (string)(dsPO.Tables[0].Rows[i][11].ToString());
                        bool valsm = SettlementMethod.Contains("/");
                        if (valsm == true)
                            return false;
                        else
                            QueryValues[15] = SettlementMethod;

                        string Address = (string)((dsPO.Tables[0].Rows[i][21].ToString()));
                        if (Address == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[16] = Address;

                        string Country = (string)((dsPO.Tables[0].Rows[i][17].ToString()));
                        if (Country == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[17] = Country;

                        //Check for Country Master

                        blnError = objBAl.CheckforCountryCode(Country);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(PartnerFileUpload.FileName, "AgentMaster", i + 2, Country + " - Country Code not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }

                        //End

                        string City = (string)((dsPO.Tables[0].Rows[i][18].ToString()));
                        if (City == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[18] = City;

                        string PostalCode = (string)((dsPO.Tables[0].Rows[i][19].ToString()));
                        if (PostalCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[19] = PostalCode;

                        string CurrencyBilling = (string)(dsPO.Tables[0].Rows[i][4].ToString());
                        if (CurrencyBilling == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[20] = CurrencyBilling;

                        //string getdate = (string)(dsPO.Tables[0].Rows[i][8].ToString());

                        string UpdatedBy = Session["Username"].ToString();// (string)(dsPO.Tables[0].Rows[i][48].ToString());
                        QueryValues[21] = UpdatedBy;


                        string PartnerType = (string)((dsPO.Tables[0].Rows[i][2].ToString()));
                        if (PartnerType == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        bool valpt = PartnerType.Contains("/");
                        if (valpt == true)
                            return false;
                        else
                            QueryValues[22] = PartnerType;

                        if ((dsPO.Tables[0].Rows[i][24].ToString()).ToString() == "" || (dsPO.Tables[0].Rows[i][25].ToString()).ToString() == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        //modified on 8/10/2014 
                        DateTime ValidFrom = new DateTime();
                        if (dsPO.Tables[0].Rows[i][24].ToString().Trim() != "")
                            ValidFrom = GetDateTimeFromExcelString(dsPO.Tables[0].Rows[i][24].ToString());
                        else
                            ValidFrom = Convert.ToDateTime(DateTime.Today);

                        QueryValues[23] = ValidFrom;

                        DateTime ValidTo = new DateTime();
                        if (dsPO.Tables[0].Rows[i][25].ToString().Trim() != "")
                            ValidTo = GetDateTimeFromExcelString(dsPO.Tables[0].Rows[i][25].ToString());
                        else
                            ValidTo = Convert.ToDateTime(DateTime.Today);

                        QueryValues[24] = ValidTo;

                        string IsScheduled = (string)((dsPO.Tables[0].Rows[i][26].ToString()));
                        QueryValues[25] = IsScheduled;

                        string SITAID = (string)((dsPO.Tables[0].Rows[i][22].ToString()));
                        QueryValues[26] = SITAID;

                        string EmailID = (string)((dsPO.Tables[0].Rows[i][23].ToString()));
                        QueryValues[27] = EmailID;

                        string PartialAcc = (string)((dsPO.Tables[0].Rows[i][27].ToString()));
                        QueryValues[28] = PartialAcc;

                        string tolerance = dsPO.Tables[0].Rows[i][30].ToString();
                        if (tolerance != "")
                            QueryValues[29] = Convert.ToDecimal(tolerance);
                        else
                            QueryValues[29] = 0;//accepted tolerance

                        string OCapplied = (string)((dsPO.Tables[0].Rows[i][28].ToString()));
                        if (OCapplied.ToLower() == "y" || OCapplied.ToLower() == "true" || OCapplied == "1")
                            QueryValues[30] = 1;
                        else if (OCapplied.ToLower() == "n" || OCapplied.ToLower() == "false" || OCapplied == "0")
                            QueryValues[30] = 0;
                        else
                            QueryValues[30] = 0;

                        string ISCustMsg = (string)((dsPO.Tables[0].Rows[i][29].ToString()));
                        if (ISCustMsg.ToLower() == "true" || ISCustMsg.ToLower() == "y" || ISCustMsg == "1")
                            QueryValues[31] = 1;
                        else if (ISCustMsg.ToLower() == "false" || ISCustMsg.ToLower() == "n" || ISCustMsg == "0")
                            QueryValues[31] = 0;
                        else
                            QueryValues[31] = 0;

                        string billEvent = dsPO.Tables[0].Rows[i][32].ToString();
                        bool valbe = billEvent.Contains("/");
                        if (valbe == true)
                            return false;
                        else
                            QueryValues[32] = billEvent;

                        string autogeninv = dsPO.Tables[0].Rows[i][31].ToString();
                        if (autogeninv.ToLower() == "true" || autogeninv.ToLower() == "y" || autogeninv == "1")
                            QueryValues[33] = 1;
                        else if (autogeninv.ToLower() == "false" || autogeninv.ToLower() == "n" || autogeninv == "0")
                            QueryValues[33] = 0;
                        else
                            QueryValues[33] = 0;

                        string billtype = dsPO.Tables[0].Rows[i][33].ToString();
                        bool valbt = billtype.Contains("/");
                        if (valbt == true)
                        {
                            return false;
                        }
                        else
                            QueryValues[34] = billtype;
                        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                        SQLServer da = new SQLServer(constr);

                        try
                        {
                            objUpload.SaveAirline(QueryValues);
                            IsInsert = true;
                            if (IsInsert == true)
                            {
                                flag = true;
                                uploadsheet = "Partners";
                                string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                            }
                            // IsInsert = da.InsertData("insert into tblPartnerMaster(PartnerName,LegalName,PartnerPrefix,PartnerCode,ZoneId,BillingCurrency,ListingCurrency,AccountCode,UpdatedOn,UpdatedBy,DigitalSignature,IsSuspended,[Language],SettlementMethod,RegistrationID,TaxRegistrationID,AdditionalTaxRegID,PartnerPresident,PartnerCFO,Country,City,PostalCode,Address,CreatedBy,CreatedOn,isActive,PartnerType,IsScheduled,ValidFrom,ValidTo,SITAiD,EmailiD) values('" + PartnerName + "','" + PartnerLegalName + "','" + PartnerPrefix + "','" + DesignatorCode + "','" + PartnerLocationID + "'," + CurrencyBilling + ",'" + CurrencyListing + "','" + PartnerAccountingCode + "','" + getdate + "','" + UpdatedBy + "'," + DigitalSignature + ",'" + Suspend + "','" + Language + "'," + SettlementMethod + "," + RegistrationID + "," + TaxRegistrationID + "," + AdditionalTaxRegistrationID + "," + President + "," + CFO + "," + Country + "," + City + "," + PostalCode + "," + Address + "," + UpdatedBy + "," + getdate + "," + 1 + "," + PartnerType + "," + IsScheduled + "," + ValidFrom + "," + ValidTo + "," + SITAID + "," + EmailID + ")");
                            //da.InsertData("insert into tblProrateMaster(OriginCode,DestCode,ProrateFactor,isActive) values('" + Origin + "','" + Dest + "'," + Prorate + ",'true')");
                        }
                        catch (Exception ex)
                        { }
                    }
                }

                return IsInsert;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Airport Excel
        protected void btnAirportUpload_Click(object sender, EventArgs e)
        {
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (AirportFileUpload.HasFile)
                {
                    Session["WorkingFile"] = AirportFileUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (AirportFileUpload.FileName.Contains("-"))
                    {
                        lblError.Text = "Please remove - sign from file name";
                        return;
                    }
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please select a proper file to upload";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    FileInfo fi = new FileInfo(Server.MapPath(path + AirportFileUpload.FileName));

                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, AirportFileUpload.FileName);
                    AirportFileUpload.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                    //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + AirportFileUpload.FileName);
                    if (LoadAirportExcelData(FileExtension, filePath) == true)
                    {
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = "File Uploaded Successfully.";

                        fi.Delete();
                    }
                    else
                    {
                        lblError.Text = "File not Processed...Please check for valid data entry";
                        lblError.ForeColor = System.Drawing.Color.Red;
                        //lblError.Text = "Field empty...";
                        fi = new FileInfo(Server.MapPath(path + AirportFileUpload.FileName));
                        fileExists = fi.Exists;
                        if (fileExists == true)
                        {
                            fi.Delete();
                        }
                    }
                    //btnUpload.Visible = false;
                    //btnDownload.Visible = true;
                    //FileExcelUpload.Enabled = false;
                }
                else
                {
                    lblError.Text = "Cannot accept files of this type.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    //btnUpload.Visible = true;
                    //btnDownload.Visible = false;
                    //FileExcelUpload.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }
        private bool LoadAirportExcelData(string ext, string filepath)
        {
            UploadMastersBAL objUpload = new UploadMastersBAL();
            object[] QueryValues = new object[46];
            string logPath = string.Empty;
            string logfilename = string.Empty;
            DateTime dtCurrentDate = new DateTime();
            string strUserName = "";
            //excelUpload obj = new excelUpload();


            FileInfo fics = new FileInfo(Server.MapPath(path + AirportFileUpload.FileName + "_Log" + ".txt"));
            bool fileExists = fics.Exists;

            if (fileExists == true)
            {
                fics.Delete();
            }


            logfilename = AirportFileUpload.FileName + "_Log" + ".txt";
            logPath = Server.MapPath(path + logfilename);
            Session["logpath"] = logPath;
            Session["logfilename"] = logfilename;
            string connString = string.Empty;

            FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = null;
            if (ext == ".xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //...
            }
            else if (ext == ".xlsx")
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
            }
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = new DataSet();
            result = excelReader.AsDataSet();

            //...
            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result1 = excelReader.AsDataSet();

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();

            bool IsInsert = false;

            try
            {

                DataSet dsPO = new DataSet();
                //oleda.Fill(dsPO, "Order");
                dsPO = result1.Copy();
                if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                    {
                        if (String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][0].ToString()))
                        {
                            continue;
                        }
                        string AirportCode = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        if (AirportCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[0] = AirportCode;

                        string AirportName = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                        if (AirportName == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[1] = AirportName;

                        string CountryCode = (string)(dsPO.Tables[0].Rows[i][2].ToString());
                        if (CountryCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[2] = CountryCode;

                        string RegionCode = (string)(dsPO.Tables[0].Rows[i][3].ToString());
                        if (RegionCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[3] = RegionCode;

                        string City = (string)(dsPO.Tables[0].Rows[i][4].ToString());
                        if (City == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[4] = City;

                        string IsActive = (string)(dsPO.Tables[0].Rows[i][5].ToString());
                        if (IsActive.ToLower() == "true" || IsActive == "1" || IsActive == "Y")
                            QueryValues[5] = 1;
                        else if (IsActive.ToLower() == "false" || IsActive == "0" || IsActive == "N")
                            QueryValues[5] = 0;
                        else QueryValues[5] = 0;

                        string StationMailId = (string)(dsPO.Tables[0].Rows[i][6].ToString());
                        //if (StationMailId == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        QueryValues[6] = StationMailId;

                        string ManagerName = (string)(dsPO.Tables[0].Rows[i][7].ToString());
                        //if (ManagerName == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        QueryValues[7] = ManagerName;

                        string ManagerEmailId = (string)(dsPO.Tables[0].Rows[i][8].ToString());
                        ////if (ManagerEmailId == "")
                        ////{
                        ////    lblError.Text = "Fields marked * in excel are mandatory";
                        ////    return false;
                        ////}
                        QueryValues[8] = ManagerEmailId;

                        string ShiftMobNo = (string)((dsPO.Tables[0].Rows[i][9].ToString()));
                        //if (ShiftMobNo == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        QueryValues[9] = ShiftMobNo;

                        string LandlineNo = (string)((dsPO.Tables[0].Rows[i][10].ToString()));
                        //if (LandlineNo == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        QueryValues[10] = LandlineNo;

                        string ManagerMobNo = (string)(dsPO.Tables[0].Rows[i][11].ToString());
                        //if (ManagerMobNo == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        QueryValues[11] = ManagerMobNo;

                        string Counter = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                        QueryValues[12] = Counter;

                        string GHAName = (string)(dsPO.Tables[0].Rows[i][13].ToString());
                        QueryValues[13] = GHAName;

                        string GHAAddress = (string)(dsPO.Tables[0].Rows[i][14].ToString());
                        QueryValues[14] = GHAAddress;


                        string GHAPhoneNo = (string)(dsPO.Tables[0].Rows[i][15].ToString());
                        QueryValues[15] = GHAPhoneNo;

                        string GHAMobileNo = (string)((dsPO.Tables[0].Rows[i][16].ToString()));
                        QueryValues[16] = GHAMobileNo;

                        string GHAFAXNo = (string)((dsPO.Tables[0].Rows[i][17].ToString()));
                        QueryValues[17] = GHAFAXNo;

                        string GHAEmailID = (string)((dsPO.Tables[0].Rows[i][18].ToString()));
                        QueryValues[18] = GHAEmailID;

                        string GSAName = (string)((dsPO.Tables[0].Rows[i][19].ToString()));
                        QueryValues[19] = GSAName;

                        string GSAAddress = (string)(dsPO.Tables[0].Rows[i][20].ToString());
                        QueryValues[20] = GSAAddress;

                        //string getdate = (string)(dsPO.Tables[0].Rows[i][8].ToString());

                        string GSAPhoneNo = (string)(dsPO.Tables[0].Rows[i][21].ToString());
                        QueryValues[21] = GSAPhoneNo;


                        string GSAMobileNo = (string)((dsPO.Tables[0].Rows[i][22].ToString()));
                        QueryValues[22] = GSAMobileNo;

                        string GSAFAXNo = (string)((dsPO.Tables[0].Rows[i][23].ToString()));
                        QueryValues[23] = GSAFAXNo;

                        string GSAEmailID = (string)((dsPO.Tables[0].Rows[i][24].ToString()));
                        QueryValues[24] = GSAEmailID;

                        string APMName = (string)((dsPO.Tables[0].Rows[i][25].ToString()));
                        QueryValues[25] = APMName;

                        string APMAddress = (string)((dsPO.Tables[0].Rows[i][26].ToString()));
                        QueryValues[26] = APMAddress;

                        string APMPhoneNo = (string)((dsPO.Tables[0].Rows[i][27].ToString()));
                        QueryValues[27] = APMPhoneNo;

                        string APMMobileNo = (string)((dsPO.Tables[0].Rows[i][28].ToString()));
                        QueryValues[28] = APMMobileNo;

                        string APMFAXNo = (string)((dsPO.Tables[0].Rows[i][29].ToString()));
                        QueryValues[29] = APMFAXNo;

                        string APMEmailID = (string)((dsPO.Tables[0].Rows[i][30].ToString()));
                        QueryValues[30] = APMEmailID;

                        string AdditionalInfo = (string)((dsPO.Tables[0].Rows[i][31].ToString()));
                        QueryValues[31] = AdditionalInfo;

                        string TransitTime = (string)((dsPO.Tables[0].Rows[i][32].ToString()));
                        QueryValues[32] = TransitTime;

                        string CutOffTime = (string)((dsPO.Tables[0].Rows[i][33].ToString()));
                        QueryValues[33] = CutOffTime;

                        string IsTaxExempted = (string)((dsPO.Tables[0].Rows[i][34].ToString()));
                        if (IsTaxExempted.ToLower() == "true" || IsTaxExempted == "1" || IsTaxExempted == "Y")
                            QueryValues[34] = 1;
                        else if (IsTaxExempted.ToLower() == "false" || IsTaxExempted == "0" || IsTaxExempted == "N")
                            QueryValues[34] = 0;
                        else QueryValues[34] = 0;

                        string BookingCurrency = (string)((dsPO.Tables[0].Rows[i][35].ToString()));
                        if (BookingCurrency == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[35] = BookingCurrency;

                        string BookingCurrencyType = (string)((dsPO.Tables[0].Rows[i][36].ToString()));
                        if (BookingCurrencyType == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[36] = BookingCurrencyType;

                        string InvoiceCurrency = (string)((dsPO.Tables[0].Rows[i][37].ToString()));
                        if (InvoiceCurrency == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[37] = InvoiceCurrency;

                        string InvoiceCurrencyType = (string)((dsPO.Tables[0].Rows[i][38].ToString()));
                        if (InvoiceCurrencyType == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[38] = InvoiceCurrencyType;

                        string citytype = (string)((dsPO.Tables[0].Rows[i][39].ToString()));
                        if (citytype == "")
                            QueryValues[39] = "Non-Metro";
                        else
                            QueryValues[39] = citytype;

                        string GLAccount = (string)((dsPO.Tables[0].Rows[i][40].ToString()));
                        //if (GLAccount == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        QueryValues[40] = GLAccount;

                        string TimeZone = (string)((dsPO.Tables[0].Rows[i][41].ToString()));
                        if (TimeZone == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[41] = TimeZone;

                        string Latitude = (string)((dsPO.Tables[0].Rows[i][42].ToString()));
                        QueryValues[42] = Latitude;

                        string Longitude = (string)((dsPO.Tables[0].Rows[i][43].ToString()));
                        QueryValues[43] = Longitude;

                        string IsULDEnabled = (string)((dsPO.Tables[0].Rows[i][44].ToString()));
                        if (IsULDEnabled.ToLower() == "true" || IsULDEnabled == "1" || IsULDEnabled == "Y")
                            QueryValues[44] = 1;
                        else if (IsULDEnabled.ToLower() == "false" || IsULDEnabled == "0" || IsULDEnabled == "N")
                            QueryValues[44] = 0;
                        else QueryValues[44] = 0;

                        QueryValues[45] = "";


                        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                        SQLServer da = new SQLServer(constr);

                        try
                        {
                            //code to add entries in master table
                            #region InsertCountrymaster
                            object isAct = 0;
                            DataSet ds;

                            ds = da.GetDataset("SELECT * FROM dbo.tblCountryMaster where CountryCode='" + CountryCode + "'");

                            if (ds == null || ds.Tables[0].Rows.Count == 0)
                            {

                                if (IsActive.ToLower() == "true" || IsActive == "1" || IsActive == "Y")
                                    isAct = 1;
                                else if (IsActive.ToLower() == "false" || IsActive == "0" || IsActive == "N")
                                    isAct = 0;
                                else isAct = 0;
                                da.InsertData("INSERT INTO tblCountryMaster(CountryCode,CountryName,CurrencyCode,IsActive)VALUES ('" + CountryCode + "','" + CountryCode + "','" + BookingCurrency + "','" + isAct + "')");


                            }


                            #endregion InsertCountrymaster

                            #region InsertRegionMaster


                            ds = da.GetDataset("SELECT * FROM dbo.RegionMaster where RegionCode='" + RegionCode + "'");

                            if (ds == null || ds.Tables[0].Rows.Count == 0)
                            {
                                if (IsActive.ToLower() == "true" || IsActive == "1" || IsActive == "Y")
                                    isAct = 1;
                                else if (IsActive.ToLower() == "false" || IsActive == "0" || IsActive == "N")
                                    isAct = 0;
                                else isAct = 0;
                                da.InsertData("INSERT INTO RegionMaster(RegionCode,RegionName,CountryCode,CountryName,IsActive)VALUES ('" + RegionCode + "','" + RegionCode + "','" + CountryCode + "','" + CountryCode + "','" + isAct + "')");


                            }



                            #endregion InsertRegionMaster

                            #region InsertCityMaster

                            ds = da.GetDataset("select * from CityMaster where CityCode='" + City + "'");

                            if (ds == null || ds.Tables[0].Rows.Count == 0)
                            {

                                da.InsertData("INSERT INTO CityMaster(CityCode,CityName,CountryCode,RegionCode)VALUES ('" + City + "','" + City + "','" + CountryCode + "','" + RegionCode + "')");


                            }


                            #endregion InsertCityMaster


                            ds = da.GetDataset("select * from AirportMaster where AirportCode='" + AirportCode + "'");
                            if (ds == null || ds.Tables[0].Rows.Count == 0)
                            {

                                objUpload.AddAirport(QueryValues);
                            }
                            IsInsert = true;
                            if (IsInsert == true)
                            {
                                flag = true;
                                uploadsheet = "Airport";
                                string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                            }
                            // IsInsert = da.InsertData("insert into tblPartnerMaster(PartnerName,LegalName,PartnerPrefix,PartnerCode,ZoneId,BillingCurrency,ListingCurrency,AccountCode,UpdatedOn,UpdatedBy,DigitalSignature,IsSuspended,[Language],SettlementMethod,RegistrationID,TaxRegistrationID,AdditionalTaxRegID,PartnerPresident,PartnerCFO,Country,City,PostalCode,Address,CreatedBy,CreatedOn,isActive,PartnerType,IsScheduled,ValidFrom,ValidTo,SITAiD,EmailiD) values('" + PartnerName + "','" + PartnerLegalName + "','" + PartnerPrefix + "','" + DesignatorCode + "','" + PartnerLocationID + "'," + CurrencyBilling + ",'" + CurrencyListing + "','" + PartnerAccountingCode + "','" + getdate + "','" + UpdatedBy + "'," + DigitalSignature + ",'" + Suspend + "','" + Language + "'," + SettlementMethod + "," + RegistrationID + "," + TaxRegistrationID + "," + AdditionalTaxRegistrationID + "," + President + "," + CFO + "," + Country + "," + City + "," + PostalCode + "," + Address + "," + UpdatedBy + "," + getdate + "," + 1 + "," + PartnerType + "," + IsScheduled + "," + ValidFrom + "," + ValidTo + "," + SITAID + "," + EmailID + ")");
                            //da.InsertData("insert into tblProrateMaster(OriginCode,DestCode,ProrateFactor,isActive) values('" + Origin + "','" + Dest + "'," + Prorate + ",'true')");
                        }

                        catch (Exception ex)
                        { }
                    }
                }

                return IsInsert;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Agent Excel 
        protected void btnAgentUpload_Click(object sender, EventArgs e)
        {
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (AgentFileUpload.HasFile)
                {
                    Session["WorkingFile"] = AgentFileUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (AgentFileUpload.FileName.Contains("-"))
                    {
                        lblError.Text = "Please remove - sign from file name";
                        return;
                    }
                    String[] allowedExtensions = { ".xls", ".xlsx", ".csv" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please select a proper file to upload";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    FileInfo fi = new FileInfo(Server.MapPath(path + AgentFileUpload.FileName));

                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, AgentFileUpload.FileName);
                    AgentFileUpload.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                    //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + AgentFileUpload.FileName);
                    if (LoadAgentExcelData(FileExtension, filePath) == true)
                    {
                        lblError.Text = "File Uploaded Successfully.";
                        lblError.ForeColor = System.Drawing.Color.Green;
                        fi.Delete();
                    }
                    else
                    {
                        lblError.Text = "File not Processed...Please check for valid data entry";
                        lblError.ForeColor = System.Drawing.Color.Red;
                        fi = new FileInfo(Server.MapPath(path + AgentFileUpload.FileName));
                        fileExists = fi.Exists;
                        if (fileExists == true)
                        {
                            fi.Delete();
                        }
                    }
                    //btnUpload.Visible = false;
                    //btnDownload.Visible = true;
                    //FileExcelUpload.Enabled = false;
                }
                else
                {
                    lblError.Text = "Cannot accept files of this type.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    //btnUpload.Visible = true;
                    //btnDownload.Visible = false;
                    //FileExcelUpload.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }
        private bool LoadAgentExcelData(string ext, string filepath)
        {
            UploadMastersBAL objUpload = new UploadMastersBAL();
            object[] QueryValues = new object[39];
            object[] AgentCredit = new object[17];
            object[] AgentDeals = new object[5];
            string logPath = string.Empty;
            string logfilename = string.Empty;
            DateTime dtCurrentDate = new DateTime();
            string strUserName = "";
            //excelUpload obj = new excelUpload();


            FileInfo fics = new FileInfo(Server.MapPath(path + AgentFileUpload.FileName + "_Log" + ".txt"));
            bool fileExists = fics.Exists;

            if (fileExists == true)
            {
                fics.Delete();
            }


            logfilename = AgentFileUpload.FileName + "_Log" + ".txt";
            logPath = Server.MapPath(path + logfilename);
            Session["logpath"] = logPath;
            Session["logfilename"] = logfilename;
            string connString = string.Empty;
            DataSet result = new DataSet();
            DataSet result1 = new DataSet();
            //code updated for .csv file upload by manjusha on 16th sept 2014
            if (ext != ".csv")
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                // DataSet result = new DataSet();
                result = excelReader.AsDataSet();

                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

            }
            else
            {
                StreamReader srCSV = new StreamReader(filepath);

                DataTable dt = new DataTable();
                try
                {
                    string str = srCSV.ReadLine();
                    string[] arrCsvElement = str.Split(',');

                    for (int i = 0; i < arrCsvElement.Length; i++)
                    {
                        try
                        {
                            dt.Columns.Add(new DataColumn(arrCsvElement[i], typeof(string)));
                        }
                        catch (Exception objEx)
                        {
                        }
                    }

                    while ((str = srCSV.ReadLine()) != null)
                    {
                        try
                        {
                            arrCsvElement = str.Split(',');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < arrCsvElement.Length; i++)
                            {
                                try
                                {
                                    dr[i] = arrCsvElement[i];
                                }
                                catch (Exception objEx)
                                {
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                        catch (Exception objEx)
                        {
                        }

                    }
                    result.Tables.Add(dt);
                    result1 = result;
                    srCSV.Close();
                    srCSV.Dispose();
                    srCSV = null;
                }
                catch (Exception objEx)
                {
                }
                // result.Tables.Add(dt);
                //result1 = result;
            }
            bool IsInsert = false;
            excelUpload objBAl = new excelUpload();

            try
            {
                DataSet dsPO = new DataSet();
                //oleda.Fill(dsPO, "Order");
                dsPO = result1.Copy();
                if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                {

                    for (int i = 1; i < dsPO.Tables[0].Rows.Count; i++)
                    {
                        bool blnError = false;
                        bool blnInsert = true;

                        if (String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][0].ToString()))
                        {
                            continue;
                        }
                        #region Insert Agent General Info

                        string AgentCode = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        if (AgentCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[0] = AgentCode;

                        string IATAAgentCode = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                        if (IATAAgentCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[1] = IATAAgentCode;

                        string AgentName = (string)(dsPO.Tables[0].Rows[i][2].ToString());
                        if (AgentName == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[2] = AgentName;

                        if (dsPO.Tables[0].Rows[i][3].ToString() == "" || dsPO.Tables[0].Rows[i][4].ToString() == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        //modified on 8/10/2014
                        DateTime FromDT = new DateTime();
                        if (dsPO.Tables[0].Rows[i][3].ToString().Trim() != "")
                            FromDT = GetDateTimeFromExcelString(dsPO.Tables[0].Rows[i][3].ToString());
                        else
                            FromDT = Convert.ToDateTime(DateTime.Today);
                        QueryValues[3] = FromDT;

                        DateTime ToDT = new DateTime();
                        if (dsPO.Tables[0].Rows[i][4].ToString().Trim() != "")
                            ToDT = GetDateTimeFromExcelString(dsPO.Tables[0].Rows[i][4].ToString());
                        else
                            ToDT = Convert.ToDateTime(DateTime.Today);
                        QueryValues[4] = ToDT;


                        string CustomerCode = (string)(dsPO.Tables[0].Rows[i][5].ToString());
                        if (CustomerCode == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[5] = CustomerCode;

                        string Station = (string)(dsPO.Tables[0].Rows[i][6].ToString());
                        if (Station == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[6] = Station;

                        //Check for Airport Master

                        blnError = objBAl.CheckforAirport(Station);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(AgentFileUpload.FileName, "AgentMaster", i + 2, Station + " - Airport not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }

                        //End

                        string Country = (string)(dsPO.Tables[0].Rows[i][7].ToString());
                        if (Country == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[7] = Country;

                        string City = (string)(dsPO.Tables[0].Rows[i][8].ToString());
                        QueryValues[8] = City;

                        string Adress = (string)((dsPO.Tables[0].Rows[i][9].ToString()));
                        QueryValues[9] = Adress;

                        string Email = (string)((dsPO.Tables[0].Rows[i][10].ToString()));
                        QueryValues[10] = Email;

                        string PersonContact = (string)(dsPO.Tables[0].Rows[i][11].ToString());
                        QueryValues[11] = PersonContact;

                        string MobileNumber = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                        if (MobileNumber == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[12] = MobileNumber;

                        string Remark = (string)(dsPO.Tables[0].Rows[i][13].ToString());
                        QueryValues[13] = Remark;

                        //string strFlag = (string)(dsPO.Tables[0].Rows[i][14].ToString());
                        QueryValues[14] = "";


                        string NormalComm = (string)(dsPO.Tables[0].Rows[i][14].ToString());
                        QueryValues[15] = NormalComm;

                        string ControllingLocator = (string)((dsPO.Tables[0].Rows[i][15].ToString()));
                        if (ControllingLocator == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[16] = ControllingLocator;

                        string AccountCode = (string)((dsPO.Tables[0].Rows[i][16].ToString()));
                        QueryValues[17] = AccountCode;

                        //string TDSOnCommision = (string)((dsPO.Tables[0].Rows[i][17].ToString()));
                        //if (TDSOnCommision == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        //QueryValues[18] = TDSOnCommision;

                        //string TDSOnFreight = (string)((dsPO.Tables[0].Rows[i][18].ToString()));
                        //if (TDSOnFreight == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        //QueryValues[19] = TDSOnFreight;

                        string ControllingLocatorCode = (string)(dsPO.Tables[0].Rows[i][17].ToString());
                        QueryValues[18] = ControllingLocatorCode;

                        string BillTo = (string)(dsPO.Tables[0].Rows[i][18].ToString());
                        QueryValues[19] = BillTo;

                        string AccountMail = (string)((dsPO.Tables[0].Rows[i][19].ToString()));
                        QueryValues[20] = AccountMail;

                        string SalesMail = (string)((dsPO.Tables[0].Rows[i][20].ToString()));
                        QueryValues[21] = SalesMail;

                        string AgentType = (string)((dsPO.Tables[0].Rows[i][21].ToString()));
                        QueryValues[22] = AgentType;

                        string BillType = (string)((dsPO.Tables[0].Rows[i][22].ToString()));
                        if (BillType == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        QueryValues[23] = BillType;

                        string PanCardNumber = (string)((dsPO.Tables[0].Rows[i][23].ToString()));
                        QueryValues[24] = PanCardNumber;

                        string ServiceTaxNumber = (string)((dsPO.Tables[0].Rows[i][24].ToString()));
                        QueryValues[25] = ServiceTaxNumber;

                        string ValidBG = (string)((dsPO.Tables[0].Rows[i][25].ToString()));
                        QueryValues[26] = ValidBG;

                        string CurrencyCode = (string)((dsPO.Tables[0].Rows[i][26].ToString()));
                        QueryValues[27] = CurrencyCode;

                        //Check for Currency Master

                        blnError = objBAl.CheckforCurrency(CurrencyCode);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(AgentFileUpload.FileName, "AgentMaster", i + 2, CurrencyCode + " - Currency not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }

                        //End

                        string IsFOC = (string)((dsPO.Tables[0].Rows[i][27].ToString()));
                        if (IsFOC.ToLower() == "true" || IsFOC.ToLower() == "y")
                            QueryValues[28] = 1;
                        else if (IsFOC.ToLower() == "false" || IsFOC.ToLower() == "n")
                            QueryValues[28] = 0;
                        else
                            QueryValues[28] = 0;

                        string threshold = (string)((dsPO.Tables[0].Rows[i][28].ToString()));
                        QueryValues[29] = threshold;

                        string AgentReferenceCode = (string)((dsPO.Tables[0].Rows[i][29].ToString()));
                        //if (AgentReferenceCode == "")
                        //{
                        //    lblError.Text = "Fields marked * in excel are mandatory";
                        //    return false;
                        //}
                        QueryValues[30] = AgentReferenceCode;

                        string RatePreference = (string)((dsPO.Tables[0].Rows[i][30].ToString()));
                        QueryValues[31] = RatePreference;

                        string AutoGenInv = dsPO.Tables[0].Rows[i][48].ToString();
                        if (AutoGenInv.ToLower() == "y" || AutoGenInv == "1" || AutoGenInv == "true")
                            QueryValues[32] = 1;
                        else if (AutoGenInv.ToLower() == "n" || AutoGenInv == "0" || AutoGenInv == "false")
                            QueryValues[32] = 0;
                        else
                            QueryValues[32] = 0;

                        QueryValues[33] = 0;    //chkstnlist

                        string IACCode = dsPO.Tables[0].Rows[i][46].ToString();

                        QueryValues[34] = IACCode;

                        string CCFcode = dsPO.Tables[0].Rows[i][47].ToString();
                        QueryValues[35] = CCFcode;

                        QueryValues[36] = "";
                        QueryValues[37] = "";
                        QueryValues[38] = 0;
                        #endregion Insert Agent General Info

                        #region Insert Agent Credit

                        string AgentCodeForCredit = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        AgentCredit[0] = AgentCodeForCredit;

                        string BankName = (string)(dsPO.Tables[0].Rows[i][31].ToString());
                        AgentCredit[1] = BankName;

                        string BankGuranteeNumber = (string)(dsPO.Tables[0].Rows[i][32].ToString());
                        AgentCredit[2] = BankGuranteeNumber;

                        if (!String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][33].ToString()))
                        {
                            float BankGuranteeAmount = float.Parse(dsPO.Tables[0].Rows[i][33].ToString());
                            AgentCredit[3] = BankGuranteeAmount;
                        }
                        else
                            AgentCredit[3] = 0.0;
                        // dt1 = DateTime.ParseExact(dsPO.Tables[0].Rows[i][36].ToString(), "dd/MM/yyyy hh:mm:ss", null);

                        DateTime CreditFromDT = new DateTime();
                        if (dsPO.Tables[0].Rows[i][34].ToString().Trim() != "")
                            CreditFromDT = GetDateTimeFromExcelString(dsPO.Tables[0].Rows[i][34].ToString());
                        else
                            CreditFromDT = Convert.ToDateTime(DateTime.Today);
                        AgentCredit[4] = CreditFromDT;

                        DateTime CreditToDT = new DateTime();
                        if (dsPO.Tables[0].Rows[i][35].ToString().Trim() != "")
                            CreditToDT = GetDateTimeFromExcelString(dsPO.Tables[0].Rows[i][35].ToString());
                        else
                            CreditToDT = Convert.ToDateTime(DateTime.Today);
                        AgentCredit[5] = CreditToDT;

                        string FinalAmt = (string)(dsPO.Tables[0].Rows[i][36].ToString());
                        AgentCredit[6] = FinalAmt;
                        if (!String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][37].ToString()))
                        {
                            float CreditAmount = (float.Parse)(dsPO.Tables[0].Rows[i][37].ToString());

                            AgentCredit[7] = CreditAmount;
                        }
                        else
                            AgentCredit[7] = 0.0;

                        if (!String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][38].ToString()))
                        {
                            float InvoiceBalance = (float.Parse)(dsPO.Tables[0].Rows[i][38].ToString());
                            AgentCredit[8] = InvoiceBalance;
                        }
                        else
                            AgentCredit[8] = 0.0;
                        if (!String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][39].ToString()))
                        {
                            float CreditRemaining = (float.Parse)((dsPO.Tables[0].Rows[i][39].ToString()));
                            AgentCredit[9] = CreditRemaining;
                        }
                        else
                            AgentCredit[9] = 0.0;

                        string Expired = (string)((dsPO.Tables[0].Rows[i][40].ToString()));
                        AgentCredit[10] = Expired;

                        string TresholdLimit = (string)(dsPO.Tables[0].Rows[i][41].ToString());
                        AgentCredit[11] = TresholdLimit;

                        string TransactionType = (string)(dsPO.Tables[0].Rows[i][42].ToString());
                        AgentCredit[12] = TransactionType;

                        string CreditDays = (string)(dsPO.Tables[0].Rows[i][43].ToString());
                        AgentCredit[13] = CreditDays;
                        if (!String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][44].ToString()))
                        {
                            int TresholdLimitDays = (int.Parse)(dsPO.Tables[0].Rows[i][44].ToString());
                            AgentCredit[14] = TresholdLimitDays;
                        }
                        else
                            AgentCredit[14] = 0;

                        string BankAddress = (string)(dsPO.Tables[0].Rows[i][45].ToString());
                        AgentCredit[15] = BankAddress;
                        AgentCredit[16] = 0;
                        #endregion Insert Agent Credit

                        #region Insert Agent Deals

                        string AgentCodeForDeal = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        AgentDeals[0] = AgentCodeForDeal;
                        //if (!String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][46].ToString()))
                        //{
                        //    int AgentFrom = (int.Parse)(dsPO.Tables[0].Rows[i][46].ToString());
                        //    AgentDeals[1] = AgentFrom;
                        //}
                        //else
                        AgentDeals[1] = 0;
                        //if (!String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][47].ToString()))
                        //{
                        //    int AgentTo = (int.Parse)(dsPO.Tables[0].Rows[i][47].ToString());
                        //    AgentDeals[2] = AgentTo;
                        //}
                        //else
                        AgentDeals[2] = 0;

                        // string percent = (string)(dsPO.Tables[0].Rows[i][50].ToString());
                        AgentDeals[3] = 0;

                        //string Value = (string)(dsPO.Tables[0].Rows[i][51].ToString());
                        AgentDeals[4] = 0;

                        #endregion Insert Agent Deals

                        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                        SQLServer da = new SQLServer(constr);

                        try
                        {
                            if (blnInsert)
                            {
                                bool creditres = objUpload.AddAgentCredits(AgentCredit);
                                bool ret = objUpload.AddAgent(QueryValues);
                                string dealres = objUpload.AddAgentDeal(AgentDeals);
                                IsInsert = true;
                                if (IsInsert == true)
                                {
                                    flag = true;
                                    uploadsheet = "Agents";
                                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                                }
                            }
                            // IsInsert = da.InsertData("insert into tblPartnerMaster(PartnerName,LegalName,PartnerPrefix,PartnerCode,ZoneId,BillingCurrency,ListingCurrency,AccountCode,UpdatedOn,UpdatedBy,DigitalSignature,IsSuspended,[Language],SettlementMethod,RegistrationID,TaxRegistrationID,AdditionalTaxRegID,PartnerPresident,PartnerCFO,Country,City,PostalCode,Address,CreatedBy,CreatedOn,isActive,PartnerType,IsScheduled,ValidFrom,ValidTo,SITAiD,EmailiD) values('" + PartnerName + "','" + PartnerLegalName + "','" + PartnerPrefix + "','" + DesignatorCode + "','" + PartnerLocationID + "'," + CurrencyBilling + ",'" + CurrencyListing + "','" + PartnerAccountingCode + "','" + getdate + "','" + UpdatedBy + "'," + DigitalSignature + ",'" + Suspend + "','" + Language + "'," + SettlementMethod + "," + RegistrationID + "," + TaxRegistrationID + "," + AdditionalTaxRegistrationID + "," + President + "," + CFO + "," + Country + "," + City + "," + PostalCode + "," + Address + "," + UpdatedBy + "," + getdate + "," + 1 + "," + PartnerType + "," + IsScheduled + "," + ValidFrom + "," + ValidTo + "," + SITAID + "," + EmailID + ")");
                            //da.InsertData("insert into tblProrateMaster(OriginCode,DestCode,ProrateFactor,isActive) values('" + Origin + "','" + Dest + "'," + Prorate + ",'true')");
                        }
                        catch (Exception ex)
                        { }
                    }
                }

                return IsInsert;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void btnUploadRates_Click(object sender, EventArgs e)
        {
            FileInfo fi = null;

            bool val = CheckFileExtension(RatesExcelUpload, ref fi);
            if (val == true)
            {

                filePath = Server.MapPath(path + RatesExcelUpload.FileName);

                if (!UploadRateExcel(FileExtension, filePath))
                {
                    lblError.Text = "File not Processed...Please check for valid data entry";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    fi.Delete();
                }
                else
                {
                    lblError.Text = "File Processed Successfully.";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    fi = new FileInfo(Server.MapPath(path + RatesExcelUpload.FileName));
                    //fileExists = fi.Exists;
                    if (fi.Exists == true)
                    {
                        fi.Delete();
                    }
                }
            }
            else
            {
                lblError.Text = "Please select appropriate file to upload.....";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected bool CheckFileExtension(FileUpload Uploadcontrol, ref FileInfo fi)
        {
            try
            {
                Boolean FileOK = false;

                if (Uploadcontrol.HasFile)
                {
                    Session["WorkingFile"] = Uploadcontrol.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (Uploadcontrol.FileName.Contains("-"))
                    {
                        lblError.Text = "Please remove - sign from file name";
                        return false;
                    }

                    String[] allowedExtensions = { ".xls", ".xlsx", ".csv" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }
                else
                {
                    //lblError.Text = "Please upload proper excel file....";
                    //lblError.ForeColor = System.Drawing.Color.Red;
                    return false;
                }
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    fi = new FileInfo(Server.MapPath(path + Uploadcontrol.FileName));

                    if (fi.Exists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return false;
                    }
                    #endregion

                    string filename = Path.Combine(path, Uploadcontrol.FileName);
                    Uploadcontrol.SaveAs(Server.MapPath(filename));

                    filePath = Server.MapPath(path + Uploadcontrol.FileName);
                }
                else
                {
                    lblError.Text = "Cannot accept files of this type.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        protected bool UploadRateExcel(string ext, string filepath)
        {
            UploadMastersBAL objUpload = new UploadMastersBAL();
            DataSet result = new DataSet();
            excelUpload objBAl = new excelUpload();

            try
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables

                result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();


                //Validate Dataset Schema

                /*
                bool IsRightFormat = ValidateExcel("Rates", result);

                if (IsRightFormat != true)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Please upload proper Rates Template";
                    return false;
                }                
                
                 */

                for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                {
                    bool blnError = false;
                    bool blnInsert = true;

                    if (String.IsNullOrEmpty(result.Tables[0].Rows[i][0].ToString()))
                        continue;

                    string RateCardId = Convert.ToString(result.Tables[0].Rows[i][0]);
                    String Origin;

                    Origin = Convert.ToString(result.Tables[0].Rows[i][1]);

                    if (Origin == null)
                    {
                        return false;
                    }
                    if (Origin == "")
                    {
                        return false;
                    }

                    //Check for Airport Master

                    blnError = objBAl.CheckforAirport(Origin);
                    if (blnError == false)
                    {
                        objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, Origin + " - Airport not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                        blnInsert = false;
                    }

                    //End

                    string Destination = Convert.ToString(result.Tables[0].Rows[i][2]);

                    blnError = objBAl.CheckforAirport(Destination);
                    if (blnError == false)
                    {
                        objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, Destination + " - Airport not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                        blnInsert = false;
                    }

                    //End

                    string commcode = Convert.ToString(result.Tables[0].Rows[i][3]);

                    if (commcode.Trim() != "")
                    {
                        blnError = objBAl.CheckforCommodityCode(commcode);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, commcode + " - Commodity not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }
                    }

                    //End

                    string ProductType = Convert.ToString(result.Tables[0].Rows[i][4]);

                    if (ProductType.Trim() != "")
                    {
                        blnError = objBAl.CheckforProductCode(ProductType);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, ProductType + " - Product Type not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }
                    }

                    string FlightNumber = Convert.ToString(result.Tables[0].Rows[i][5]);

                    if (FlightNumber.Trim() != "" && FlightNumber.IndexOf(",") == -1)
                    {
                        blnError = objBAl.CheckforFlightNumber(FlightNumber);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, FlightNumber + " - Flight No. not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }
                    }

                    string AgentCode = Convert.ToString(result.Tables[0].Rows[i][6]);

                    if (AgentCode.Trim() != "")
                    {
                        blnError = objBAl.CheckforAgengtCode(AgentCode);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, AgentCode + " - Agent Code not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }
                    }

                    string Currency = Convert.ToString(result.Tables[0].Rows[i][7]);

                    if (Currency.Trim() != "")
                    {
                        blnError = objBAl.CheckforCurrency(Currency);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, Currency + " - Currency Code not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }
                    }

                    decimal min = 0;
                    if (result.Tables[0].Rows[i][8].ToString().Trim() != "")
                        min = decimal.Parse(result.Tables[0].Rows[i][8].ToString());

                    decimal normal = 0;
                    if (result.Tables[0].Rows[i][9].ToString().Trim() != "")
                        normal = decimal.Parse(result.Tables[0].Rows[i][9].ToString().Trim());

                    decimal fourtyfive = 0;
                    if (result.Tables[0].Rows[i][10].ToString().Trim() != "")
                        fourtyfive = decimal.Parse(result.Tables[0].Rows[i][10].ToString().Trim());

                    decimal hundred = 0;
                    if (result.Tables[0].Rows[i][11].ToString().Trim() != "")
                        hundred = decimal.Parse(result.Tables[0].Rows[i][11].ToString().Trim());

                    decimal TwoFifty = 0;
                    if (result.Tables[0].Rows[i][12].ToString().Trim() != "")
                        TwoFifty = decimal.Parse(result.Tables[0].Rows[i][12].ToString().Trim());

                    decimal ThreeHundred = 0;
                    if (result.Tables[0].Rows[i][13].ToString().Trim() != "")
                        ThreeHundred = decimal.Parse(result.Tables[0].Rows[i][13].ToString().Trim());

                    decimal FiveHun = 0;
                    if (result.Tables[0].Rows[i][14].ToString().Trim() != "")
                        FiveHun = decimal.Parse(result.Tables[0].Rows[i][14].ToString().Trim());

                    decimal Thosand = 0;
                    if (result.Tables[0].Rows[i][15].ToString().Trim() != "")
                        Thosand = decimal.Parse(result.Tables[0].Rows[i][15].ToString().Trim());

                    decimal TwoThousand = 0;
                    if (result.Tables[0].Rows[i][16].ToString().Trim() != "")
                        TwoThousand = decimal.Parse(result.Tables[0].Rows[i][16].ToString().Trim());

                    decimal ThreeThousand = 0;
                    if (result.Tables[0].Rows[i][17].ToString().Trim() != "")
                        ThreeThousand = decimal.Parse(result.Tables[0].Rows[i][17].ToString().Trim());

                    decimal FiveThousand = 0;
                    if (result.Tables[0].Rows[i][18].ToString().Trim() != "")
                        FiveThousand = decimal.Parse(result.Tables[0].Rows[i][18].ToString().Trim());

                    //string strFrom = result.Tables[0].Rows[i][19].ToString().Trim();
                    //string strTo = result.Tables[0].Rows[i][20].ToString().Trim();

                    DateTime FromDT = Convert.ToDateTime(result.Tables[0].Rows[i][19]);
                    DateTime ToDT = Convert.ToDateTime(result.Tables[0].Rows[i][20]);

                    decimal AgentComm = 0;
                    if (result.Tables[0].Rows[i][21].ToString().Trim() != "")
                        AgentComm = decimal.Parse(result.Tables[0].Rows[i][21].ToString().Trim());

                    decimal Discount = 0;
                    if (result.Tables[0].Rows[i][22].ToString().Trim() != "")
                        Discount = decimal.Parse(result.Tables[0].Rows[i][22].ToString().Trim());

                    decimal Tax = 0;
                    if (result.Tables[0].Rows[i][23].ToString().Trim() != "")
                        Tax = decimal.Parse(result.Tables[0].Rows[i][23].ToString().Trim());

                    bool AllInRate = false;
                    if (result.Tables[0].Rows[i][24].ToString().Trim().ToLower() != "y" && result.Tables[0].Rows[i][24].ToString().Trim() != "1"
                        && result.Tables[0].Rows[i][24].ToString().Trim() != "")
                        AllInRate = true;
                    else
                        AllInRate = false;

                    bool IsTact = false;
                    if (result.Tables[0].Rows[i][25].ToString().Trim().ToLower() != "Y" && result.Tables[0].Rows[i][25].ToString().Trim() != "1"
                        && result.Tables[0].Rows[i][25].ToString().Trim() != "")
                        IsTact = true;
                    else
                        IsTact = false;

                    bool IsHeavy = false;
                    if (result.Tables[0].Rows[i][26].ToString().Trim().ToLower() != "y" && result.Tables[0].Rows[i][26].ToString().Trim() != "1"
                        && result.Tables[0].Rows[i][26].ToString().Trim() != "")
                        IsHeavy = true;
                    else
                        IsHeavy = false;

                    string IssueCarrier = string.Empty;
                    if (result.Tables[0].Rows[i][27].ToString().Trim() != "")
                        IssueCarrier = (string)result.Tables[0].Rows[i][27].ToString().Trim();

                    string FlightCarrier = string.Empty;
                    if (result.Tables[0].Rows[i][28].ToString().Trim() != "")
                        FlightCarrier = (string)result.Tables[0].Rows[i][28].ToString().Trim();

                    string SHC = string.Empty;
                    if (result.Tables[0].Rows[i][29].ToString().Trim() != "")
                        SHC = (string)result.Tables[0].Rows[i][29].ToString().Trim();

                    string DayOfWeek = string.Empty;
                    if (result.Tables[0].Rows[i][30].ToString().Trim() != "")
                        DayOfWeek = (string)result.Tables[0].Rows[i][30].ToString().Trim();

                    string DepIntFrom = string.Empty;
                    if (result.Tables[0].Rows[i][31].ToString().Trim() != "")
                        DepIntFrom = (string)result.Tables[0].Rows[i][31].ToString().Trim();

                    string DepIntTo = string.Empty;
                    if (result.Tables[0].Rows[i][32].ToString().Trim() != "")
                        DepIntTo = (string)result.Tables[0].Rows[i][32].ToString().Trim();

                    //Shipper Code 37
                    string ShipperCode = string.Empty;
                    if (result.Tables[0].Rows[i][37].ToString().Trim() != "")
                        ShipperCode = (string)result.Tables[0].Rows[i][37].ToString().Trim();

                    if (ShipperCode.Trim() != "")
                    {
                        blnError = objBAl.CheckforShipperode(ShipperCode);
                        if (blnError == false)
                        {
                            objBAl.CreateUploadError(RatesExcelUpload.FileName, "RatelineMaster", i + 2, ShipperCode + " - Shipper Code not Exist.", Convert.ToDateTime(Session["IT"]), Convert.ToString(Session["UserName"]));
                            blnInsert = false;
                        }
                    }

                    decimal Fifty = 0;
                    if (result.Tables[0].Rows[i][38].ToString().Trim() != "")
                        Fifty = decimal.Parse(result.Tables[0].Rows[i][38].ToString().Trim());
                    //Call Function to insert the data..

                    string errormessage = string.Empty;

                    if (blnInsert)
                    {
                        if (!objUpload.InsertAllTypesRateLines(RateCardId, Origin, Destination, commcode, ProductType, FlightNumber, AgentCode, Currency,
                            min, normal, fourtyfive, hundred, TwoFifty, ThreeHundred, FiveHun, Thosand, TwoThousand, ThreeThousand, FiveThousand,
                            FromDT, ToDT, AgentComm, Discount, Tax, AllInRate, IsTact, IsHeavy, IssueCarrier, FlightCarrier, SHC, DayOfWeek,
                            DepIntFrom, DepIntTo, ref errormessage, ShipperCode, Fifty))
                        {
                            //Log.WriteLog("Error : Line No(" + i + ")" + errormessage);
                        }
                    }

                    //Log.WriteLog("Inserted Successfully. Line No(" + i + ")");
                }
                flag = true;
                uploadsheet = "Rates";
                string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                lblError.Text = "File Uploaded Successfully.";
            }
            catch
            {
                lblError.Text = "Error while uploading file.";
                return false;
            }
            finally
            {
                objUpload = null;
                result = null;
            }

            return true;
        }

        protected void btnUploadShipCon_Click(object sender, EventArgs e)
        {
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (FileUploadShipCon.HasFile)
                {
                    Session["WorkingFile"] = FileUploadShipCon.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (FileUploadShipCon.FileName.Contains("-"))
                    {
                        lblError.Text = "Please remove - sign from file name";
                        return;
                    }
                    String[] allowedExtensions = { ".xls", ".xlsx", ".csv" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please select a proper file to upload";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    FileInfo fi = new FileInfo(Server.MapPath(path + FileUploadShipCon.FileName));

                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, FileUploadShipCon.FileName);
                    FileUploadShipCon.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                    //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + FileUploadShipCon.FileName);
                    if (LoadShipperConsigneeExcelData(FileExtension, filePath))
                    {
                        if (strflag == "true")
                        {
                            lblError.Text = "File Uploaded Successfully.";
                            lblError.ForeColor = System.Drawing.Color.Green;
                            fi.Delete();
                        }
                        else
                        {
                            lblError.Text = "please choose proper format";
                            lblError.ForeColor = System.Drawing.Color.Green;
                            fi.Delete();
                        }
                    }
                    else
                    {
                        lblError.Text = "File not Processed, Check log file for details.";
                        lblError.ForeColor = System.Drawing.Color.Red;
                        fi = new FileInfo(Server.MapPath(path + FileUploadShipCon.FileName));
                        fileExists = fi.Exists;
                        if (fileExists == true)
                        {
                            fi.Delete();
                        }
                    }
                    //btnUpload.Visible = false;
                    //btnDownload.Visible = true;
                    //FileExcelUpload.Enabled = false;
                }
                else
                {
                    lblError.Text = "Cannot accept files of this type.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    //btnUpload.Visible = true;
                    //btnDownload.Visible = false;
                    //FileExcelUpload.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }

        private bool LoadShipperConsigneeExcelData(string ext, string filepath)
        {
            string logPath = string.Empty;
            string logfilename = string.Empty;
            DateTime dtCurrentDate = new DateTime();
            string strUserName = "";
            //excelUpload obj = new excelUpload();


            FileInfo fics = new FileInfo(Server.MapPath(path + FileUploadShipCon.FileName + "_Log" + ".txt"));
            bool fileExists = fics.Exists;

            if (fileExists == true)
            {
                fics.Delete();
            }


            logfilename = FileUploadShipCon.FileName + "_Log" + ".txt";
            logPath = Server.MapPath(path + logfilename);
            Session["logpath"] = logPath;
            Session["logfilename"] = logfilename;
            string connString = string.Empty;


            DataSet result = new DataSet();
            DataSet result1 = new DataSet();
            //code updated for .csv file upload by manjusha on 16th sept 2014
            if (ext != ".csv")
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                // DataSet result = new DataSet();
                result = excelReader.AsDataSet();

                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

            }
            else
            {
                StreamReader srCSV = new StreamReader(filepath);

                DataTable dt = new DataTable();
                try
                {
                    string str = srCSV.ReadLine();
                    string[] arrCsvElement = str.Split(',');

                    for (int i = 0; i < arrCsvElement.Length; i++)
                    {
                        try
                        {
                            dt.Columns.Add(new DataColumn(arrCsvElement[i], typeof(string)));
                        }
                        catch (Exception objEx)
                        {
                        }
                    }

                    while ((str = srCSV.ReadLine()) != null)
                    {
                        try
                        {
                            arrCsvElement = str.Split(',');
                            DataRow dr = dt.NewRow();
                            for (int i = 0; i < arrCsvElement.Length; i++)
                            {
                                try
                                {
                                    dr[i] = arrCsvElement[i];
                                }
                                catch (Exception objEx)
                                {
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                        catch (Exception objEx)
                        {
                        }

                    }
                    result.Tables.Add(dt);
                    result1 = result;
                    srCSV.Close();
                    srCSV.Dispose();
                    srCSV = null;
                }
                catch (Exception objEx)
                {
                }
            }
            bool IsInsert = false;

            try
            {
                //int zipcode;
                DataSet dsPO = new DataSet();
                //oleda.Fill(dsPO, "Order");
                dsPO = result1.Copy();
                if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                    {
                        if (String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][0].ToString()))
                        {
                            continue;
                        }
                        string AccountCode = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        string AccountName = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                        if (AccountName == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        string Adress1 = (string)(dsPO.Tables[0].Rows[i][2].ToString());
                        if (Adress1 == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        string Adress2 = (string)(dsPO.Tables[0].Rows[i][3].ToString());
                        string City = (string)(dsPO.Tables[0].Rows[i][4].ToString());
                        string State = (string)(dsPO.Tables[0].Rows[i][5].ToString());
                        string Country = (string)(dsPO.Tables[0].Rows[i][6].ToString());
                        int Zipcode;
                        if (String.IsNullOrEmpty(dsPO.Tables[0].Rows[i][7].ToString()))
                        {
                            Zipcode = 0;
                        }
                        else
                            Zipcode = Convert.ToInt32((dsPO.Tables[0].Rows[i][7].ToString()));
                        string PhoneNumber = (string)(dsPO.Tables[0].Rows[i][8].ToString());
                        string MobileNumber = (string)(dsPO.Tables[0].Rows[i][9].ToString());
                        if (MobileNumber == "")
                        {
                            lblError.Text = "Fields marked * in excel are mandatory";
                            return false;
                        }
                        string Fax = (string)(dsPO.Tables[0].Rows[i][10].ToString());
                        string Email = (string)(dsPO.Tables[0].Rows[i][11].ToString());
                        string IATAAccountNo = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                        string ParticipationType = (string)(dsPO.Tables[0].Rows[i][13].ToString());
                        string AgentCode = (string)(dsPO.Tables[0].Rows[i][14].ToString());
                        string CCFcode = dsPO.Tables[0].Rows[i][15].ToString();
                        DateTime CreatedOn = Convert.ToDateTime(Session["IT"].ToString());
                        string CreatedBy = Session["UserName"].ToString();


                        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                        SQLServer da = new SQLServer(constr);

                        try
                        {
                            IsInsert = da.InsertData("insert into AccountMaster(AccountCode,AccountName,Adress1,Adress2,City,State,Country,ZipCode,PhoneNumber,MobileNumber,Fax,Email,IATAAccountNo,ParticipationType,AgentCode,CreatedOn,CreatedBy,IsActive,CCSFCode) values('" + AccountCode + "','" + AccountName + "','" + Adress1 + "','" + Adress2 + "','" + City + "','" + State + "','" + Country + "','" + Zipcode + "','" + PhoneNumber + "','" + MobileNumber + "','" + Fax + "','" + Email + "','" + IATAAccountNo + "','" + ParticipationType + "','" + AgentCode + "','" + CreatedOn + "','" + CreatedBy + "','" + 1 + "','" + CCFcode + "')");
                            //da.InsertData("insert into tblProrateMaster(OriginCode,DestCode,ProrateFactor,isActive) values('" + Origin + "','" + Dest + "'," + Prorate + ",'true')");
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                if (IsInsert == true)
                {
                    flag = true;
                    uploadsheet = "ShipperConsignee";
                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                }
                strflag = "true";
                return IsInsert;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool ValidateExcel(string ExcelType, DataSet ExcelDataSet)
        {
            if (ExcelType == "Rates")
            {
                if (ExcelDataSet != null && ExcelDataSet.Tables.Count > 0 && ExcelDataSet.Tables[0].Rows.Count > 0)
                {
                    if (ExcelDataSet.Tables[0].Rows[0][0].ToString().Trim() != "Rate Card Type")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][1].ToString().Trim() != "Origin")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][2].ToString().Trim() != "Destination")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][3].ToString().Trim() != "Commodity Code")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][4].ToString().Trim() != "Product Type")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][5].ToString().Trim() != "Flight#")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][6].ToString().Trim() != "Agent Code")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][7].ToString().Trim() != "Currency")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][8].ToString().Trim() != "MIN")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][9].ToString().Trim() != "N")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][10].ToString().Trim() != "45")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][11].ToString().Trim() != "100")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][12].ToString().Trim() != "250")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][13].ToString().Trim() != "300")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][14].ToString().Trim() != "500")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][15].ToString().Trim() != "1000")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][16].ToString().Trim() != "2000")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][17].ToString().Trim() != "3000")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][18].ToString().Trim() != "5000")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][19].ToString().Trim() != "Valid From")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][20].ToString().Trim() != "Valid To")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][21].ToString().Trim() != "Agent Comission")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][22].ToString().Trim() != "Discount")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][23].ToString().Trim() != "Tax%")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][24].ToString().Trim() != "All in rate flag")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][25].ToString().Trim() != "Tact Flag")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][26].ToString().Trim() != "Heavy Flag")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][27].ToString().Trim() != "Issue Carrier")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][28].ToString().Trim() != "Flight Carrier")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][29].ToString().Trim() != "SHC")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][30].ToString().Trim() != "Day of week")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][31].ToString().Trim() != "Departure Interval From")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][32].ToString().Trim() != "Departure Interval to")
                        return false;
                }
            }
            else if (ExcelType == "AirlineSchedule")
            {
                if (ExcelDataSet != null && ExcelDataSet.Tables.Count > 0 && ExcelDataSet.Tables[0].Rows.Count > 0)
                {
                    if (ExcelDataSet.Tables[0].Rows[0][0].ToString().Trim() != "From Date")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][1].ToString().Trim() != "To Date")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][2].ToString().Trim() != "EquipmentNo")
                        return false;


                    if (ExcelDataSet.Tables[0].Rows[0][3].ToString().Trim() != "Flight#")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][4].ToString().Trim() != "Source")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][5].ToString().Trim() != "Destination")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][6].ToString().Trim() != "DEPT-TIME")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][7].ToString().Trim() != "ARRIVAL-TIME")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][8].ToString().Trim() != "Frequency")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][9].ToString().Trim() != "UpdatedOn")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][10].ToString().Trim() != "UpdatedBy")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][11].ToString().Trim() != "CargoCapacity")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][12].ToString().Trim() != "ActSource")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][13].ToString().Trim() != "ActDestination")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][14].ToString().Trim() != "ActArrTime")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][15].ToString().Trim() != "ArrDay")
                        return false;

                }
            }
            else if (ExcelType == "PartnerSchedule")
            {
                if (ExcelDataSet != null && ExcelDataSet.Tables.Count > 0 && ExcelDataSet.Tables[0].Rows.Count > 0)
                {
                    if (ExcelDataSet.Tables[0].Rows[0][0].ToString().Trim() != "From Date")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][1].ToString().Trim() != "To Date")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][2].ToString().Trim() != "EquipmentNo")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][3].ToString().Trim() != "Flight#")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][4].ToString().Trim() != "Source")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][5].ToString().Trim() != "Destination")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][6].ToString().Trim() != "DEPT-TIME")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][7].ToString().Trim() != "ARRIVAL-TIME")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][8].ToString().Trim() != "Frequency")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][9].ToString().Trim() != "CargoCapacity")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][10].ToString().Trim() != "ActSource")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][11].ToString().Trim() != "ActDestination")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][12].ToString().Trim() != "ActDeptTime")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][13].ToString().Trim() != "ActArrTime")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][14].ToString().Trim() != "ArrDay")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][15].ToString().Trim() != "IsDomestic")
                        return false;


                    if (ExcelDataSet.Tables[0].Rows[0][16].ToString().Trim() != "PartnerCode")
                        return false;

                }
            }

            return true;
        }

        protected void BtnUploadOtherCharges_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fi = null;

                bool val = CheckFileExtension(uploadOtherCharges, ref fi);
                if (val == true)
                {
                    filePath = Server.MapPath(path + uploadOtherCharges.FileName);

                    if (UploadOtherChargesExcel(FileExtension, filePath))
                    {
                        lblError.Text = "File Uploaded Successfully.";
                        lblError.ForeColor = System.Drawing.Color.Green;
                        fi.Delete();
                    }
                    else
                    {
                        lblError.Text = "File not Processed, Check log file for details.";
                        fi = new FileInfo(Server.MapPath(path + RatesExcelUpload.FileName));
                        //fileExists = fi.Exists;
                        if (fi.Exists == true)
                        {
                            fi.Delete();
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please select appropriate file to upload.....";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex) { }
        }

        protected bool UploadOtherChargesExcel(string ext, string filepath)
        {
            UploadMastersBAL objUpload = new UploadMastersBAL();
            DataSet result = new DataSet();

            try
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables

                result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                //Validate Dataset Schema
                bool IsRightFormat = ValidateOtherChargesExcelHeader(result);

                if (IsRightFormat != true)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Please upload proper Rates Template";
                    FileInfo fi = null;
                    fi = new FileInfo(Server.MapPath(path + RatesExcelUpload.FileName));
                    //fileExists = fi.Exists;
                    if (fi.Exists == true)
                    {
                        fi.Delete();
                    }
                    return false;
                }
                int coloumnCount = result.Tables[0].Columns.Count;
                for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                {
                    int OCId=0;
                    if (Convert.ToString(result.Tables[0].Rows[i][1]) == "")
                        continue;
                    if (!String.IsNullOrEmpty(result.Tables[0].Rows[i][0].ToString()))
                        OCId = Convert.ToInt32(result.Tables[0].Rows[i][0]);
                    else
                        OCId = 0;
                    string ChargeHeadCode = Convert.ToString(result.Tables[0].Rows[i][1]);
                    string ChargeHeadName = (string)result.Tables[0].Rows[i][2];

                    //modified on 8/10/2014
                    DateTime FromDT = new DateTime();
                    try
                    {

                        if (result.Tables[0].Rows[i][3].ToString().Trim() != "")
                            FromDT = GetDateTimeFromExcelString(result.Tables[0].Rows[i][3].ToString());
                        else
                            FromDT = Convert.ToDateTime(DateTime.Today);

                    }
                    catch (Exception ex) { }

                    DateTime ToDT = new DateTime();
                    try
                    {
                        if (result.Tables[0].Rows[i][4].ToString().Trim() != "")
                            ToDT = GetDateTimeFromExcelString(result.Tables[0].Rows[i][4].ToString());
                        else
                            ToDT = Convert.ToDateTime(DateTime.Today);

                    }
                    catch (Exception ex) { }

                    string origin = Convert.ToString(result.Tables[0].Rows[i][5]);
                    string Destination = Convert.ToString(result.Tables[0].Rows[i][6]);
                    string Currency = Convert.ToString(result.Tables[0].Rows[i][7]);
                    string dueon = Convert.ToString(result.Tables[0].Rows[i][8]);
                    string paymentType = Convert.ToString(result.Tables[0].Rows[i][9]);
                    decimal AgentComm = 0;
                    try
                    {
                        if (result.Tables[0].Rows[i][10].ToString().Trim() != "" || result.Tables[0].Rows[i][10].ToString().Length < 1)
                            AgentComm = decimal.Parse(result.Tables[0].Rows[i][10].ToString().Trim());
                    }
                    catch (Exception ex) { }
                    decimal Discount = 0;
                    try
                    {
                        if (result.Tables[0].Rows[i][11].ToString().Trim() != "" || result.Tables[0].Rows[i][11].ToString().Trim() != "")
                            Discount = decimal.Parse(result.Tables[0].Rows[i][11].ToString().Trim());
                    }
                    catch (Exception ex) { }
                    decimal Tax = 0;
                    if (result.Tables[0].Rows[i][12].ToString().Trim() != "" || result.Tables[0].Rows[i][12].ToString().Trim() != "")
                        Tax = decimal.Parse(result.Tables[0].Rows[i][12].ToString().Trim());

                    decimal TDS = 0;
                    if (result.Tables[0].Rows[i][13].ToString().Trim() != "" || result.Tables[0].Rows[i][13].ToString().Trim() != "")
                        TDS = decimal.Parse(result.Tables[0].Rows[i][13].ToString().Trim());

                    string ChargeHeadBasis = Convert.ToString(result.Tables[0].Rows[i][14]);

                    decimal mincharge = 0;
                    try
                    {
                        if (!(String.IsNullOrEmpty(result.Tables[0].Rows[i][15].ToString())))
                            if (result.Tables[0].Rows[i][15].ToString().Length < 1)
                                mincharge = decimal.Parse(result.Tables[0].Rows[i][15].ToString());
                    }
                    catch (Exception ex) { }

                    decimal perunit = 0;
                    try
                    {
                        if (result.Tables[0].Rows[i][17].ToString().Trim() != " " || result.Tables[0].Rows[i][17].ToString().Length < 1)
                            perunit = decimal.Parse(result.Tables[0].Rows[i][17].ToString());
                    }
                    catch (Exception ex) { }

                    string WeightType = Convert.ToString(result.Tables[0].Rows[i][18]);
                    string ViaStation = Convert.ToString(result.Tables[0].Rows[i][19]);
                    string chargedAt = Convert.ToString(result.Tables[0].Rows[i][20]);

                    string Commodity = string.Empty;
                    if (result.Tables[0].Rows[i][21].ToString().Trim() != "")
                    {
                        Commodity = (string)result.Tables[0].Rows[i][21].ToString().Trim();
                        Commodity = Commodity.Trim(',');
                    }

                    string Product = string.Empty;
                    if (result.Tables[0].Rows[i][22].ToString().Trim() != "")
                        Product = (string)result.Tables[0].Rows[i][22].ToString().Trim();

                    string Flight = string.Empty;
                    if (result.Tables[0].Rows[i][23].ToString().Trim() != "")
                        Flight = (string)result.Tables[0].Rows[i][23].ToString().Trim();

                    string Agent = string.Empty;
                    if (result.Tables[0].Rows[i][24].ToString().Trim() != "")
                    {
                        Agent = (string)result.Tables[0].Rows[i][24].ToString().Trim();
                        Agent = Agent.Trim(',');
                    }

                    string SHC = string.Empty;
                    if (result.Tables[0].Rows[i][25].ToString().Trim() != "")
                        SHC = (string)result.Tables[0].Rows[i][25].ToString().Trim();

                    string DayOfWeek = string.Empty;
                    if (result.Tables[0].Rows[i][26].ToString().Trim() != "")
                        DayOfWeek = (string)result.Tables[0].Rows[i][26].ToString().Trim();

                    string Handler = string.Empty;
                    if (result.Tables[0].Rows[i][27].ToString().Trim() != "")
                        Handler = (string)result.Tables[0].Rows[i][27].ToString().Trim();

                    string Equipment = string.Empty;
                    if (result.Tables[0].Rows[i][28].ToString().Trim() != "")
                        Equipment = (string)result.Tables[0].Rows[i][28].ToString().Trim();

                    string GLAccount = "", IssueCarrier = "", FlightCarrier = "", AirlineCode = "", ShipperCode = "";

                    if (result.Tables[0].Rows[i][29].ToString().Trim() != "")
                        GLAccount = (string)result.Tables[0].Rows[i][29].ToString().Trim();

                    if (result.Tables[0].Rows[i][30].ToString().Trim() != "")
                        FlightCarrier = (string)result.Tables[0].Rows[i][30].ToString().Trim();

                    if (result.Tables[0].Rows[i][31].ToString().Trim() != "")
                        IssueCarrier = (string)result.Tables[0].Rows[i][31].ToString().Trim();

                    if (result.Tables[0].Rows[i][32].ToString().Trim() != "")
                        AirlineCode = (string)result.Tables[0].Rows[i][32].ToString().Trim();

                    if (result.Tables[0].Rows[i][33].ToString().Trim() != "")
                    {
                        ShipperCode = (string)result.Tables[0].Rows[i][33].ToString().Trim();
                        ShipperCode = ShipperCode.Trim(',');
                    }

                    decimal min = 0;
                    if (result.Tables[0].Rows[i][34].ToString().Trim() != "")
                        min = decimal.Parse(result.Tables[0].Rows[i][34].ToString());

                    decimal normal = 0;
                    if (result.Tables[0].Rows[i][35].ToString().Trim() != "")
                        normal = decimal.Parse(result.Tables[0].Rows[i][35].ToString().Trim());





                    //Call Function to insert the data..

                    string errormessage = string.Empty;
                    int srno = 0;
                    if (!objUpload.InsertAllTypesOtherCharges(ChargeHeadCode, ChargeHeadName, FromDT, ToDT, origin, Destination, Currency, dueon,
                        paymentType, AgentComm, Discount, Tax, TDS, ChargeHeadBasis, mincharge, perunit, WeightType, ViaStation, chargedAt, Commodity,
                        Product, Flight, Agent, SHC, DayOfWeek, Handler, Equipment,
                        ref errormessage, GLAccount, IssueCarrier, FlightCarrier, AirlineCode, ShipperCode, ref srno, OCId))
                    {
                        lblError.Text = errormessage.ToString();
                        return false;
                    }
                
#region weightslabs
                    

                  string[]  param = new string[] { "OthSrNo" };
                  SqlDbType[] dbtypes = new SqlDbType[] { SqlDbType.Int };
                  object[] values = new object[] { srno };

                       if (!da.ExecuteProcedure("SP_DeleteOthLineSlabs", param, dbtypes, values))
                       {
                           errormessage = "Error while deleting previous slabs(" + srno + ")";
                            return false;
                        }

                    param = new string[] { "OthSrNo", "SlabName", "Weight", "Charge", "Cost" };
                    dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float };
                    values = new object[] { srno, "M", Convert.ToDouble("0"), Convert.ToDouble("" + min), Convert.ToDouble("0") };
                    if (min > 0)
                    {
                        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab M(" + srno + ")";
                            return false;
                        }
                    }

                    if (normal > 0)
                    {
                        values = new object[] { srno, "N", Convert.ToDouble("0"), Convert.ToDouble("" + normal), Convert.ToDouble("0") };

                        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab N(" + srno + ")";
                            return false;
                        }
                    }



                    for (int k = 36; k < coloumnCount; k++)
                    {
                        //delete rate line slabs before insert or update for RateLineSrNo

                        decimal weight = 0, charge = 0;
                        if (result.Tables[0].Rows[0][k].ToString().Trim() != "")
                        {
                            weight = decimal.Parse(result.Tables[0].Rows[0][k].ToString().Trim());
                        }
                        else
                        {
                            weight = 0;
                        }
                        if (result.Tables[0].Rows[i][k].ToString().Trim() != "")
                        {
                            charge = decimal.Parse(result.Tables[0].Rows[i][k].ToString().Trim());
                        }
                        else
                        {
                            charge = 0;
                        }
                        if (weight > 0 && charge > 0)
                        {
                            values = new object[] { srno, "Q", Convert.ToDouble("" + weight), Convert.ToDouble("" + charge),Convert.ToDouble(0) };
                            if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                            {
                                errormessage = "Error while inserting slab Q(" + srno + ")";
                                return false;
                            }

                        }
                    }

#endregion weightslabs
                    
                }
                flag = true;
                uploadsheet = "OC";
                string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                lblError.Text = "File Uploaded Successfully.";
            }
            catch
            {
                lblError.Text = "Error while uploading file.";
                return false;
            }
            finally
            {
                objUpload = null;
                result = null;
            }

            return true;
        }

        protected bool ValidateOtherChargesExcelHeader(DataSet ExcelDataSet)
        {
            try
            {

                if (ExcelDataSet != null && ExcelDataSet.Tables.Count > 0 && ExcelDataSet.Tables[0].Rows.Count > 0)
                {
                    if (ExcelDataSet.Tables[0].Rows[0][0].ToString().Trim() != "OCID")
                        return false;
                    if (ExcelDataSet.Tables[0].Rows[0][1].ToString().Trim() != "ChargeHeadCode")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][2].ToString().Trim() != "ChargeHeadName")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][3].ToString().Trim() != "Valid From")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][4].ToString().Trim() != "Valid To")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][5].ToString().Trim() != "Origin")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][6].ToString().Trim() != "Destination")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][7].ToString().Trim() != "Currency")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][8].ToString().Trim() != "Due on")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][9].ToString().Trim() != "PaymentType")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][10].ToString().Trim() != "Agent Comission")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][11].ToString().Trim() != "Discount")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][12].ToString().Trim() != "Tax%")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][13].ToString().Trim() != "TDS %")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][14].ToString().Trim() != "ChargeHeadBasis")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][15].ToString().Trim() != "MinimumCharge")
                        return false;
                    if (ExcelDataSet.Tables[0].Rows[0][16].ToString().Trim() != "Max Charge")
                        return false;
                    if (ExcelDataSet.Tables[0].Rows[0][17].ToString().Trim() != "PerUnitCharge")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][18].ToString().Trim() != "WeightType")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][19].ToString().Trim() != "ViaStation")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][20].ToString().Trim() != "chargedAt")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][21].ToString().Trim() != "Commodity")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][22].ToString().Trim() != "Product Type")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][23].ToString().Trim() != "Flight#")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][24].ToString().Trim() != "Agent Code")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][25].ToString().Trim() != "SHC")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][26].ToString().Trim() != "Day of week")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][27].ToString().Trim() != "Handler")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][28].ToString().Trim() != "Equipment Type")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][29].ToString().Trim() != "GL Account") 
                    return false;

                    if (ExcelDataSet.Tables[0].Rows[0][30].ToString().Trim() != "Flight Carrier") 
                    return false;

                    if (ExcelDataSet.Tables[0].Rows[0][31].ToString().Trim() != "Issues Carrier") 
                    return false;

                    if (ExcelDataSet.Tables[0].Rows[0][32].ToString().Trim() != "Airline Code") 
                    return false;

                    if (ExcelDataSet.Tables[0].Rows[0][33].ToString().Trim() != "Shipper Code") 
                    return false;

                    if (ExcelDataSet.Tables[0].Rows[0][34].ToString().Trim() != "MIN")
                        return false;

                    if (ExcelDataSet.Tables[0].Rows[0][35].ToString().Trim() != "N")
                        return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][35].ToString().Trim() != "45")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][36].ToString().Trim() != "100")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][37].ToString().Trim() != "250")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][38].ToString().Trim() != "300")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][39].ToString().Trim() != "500")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][40].ToString().Trim() != "1000")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][41].ToString().Trim() != "2000")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][42].ToString().Trim() != "3000")
                    //    return false;

                    //if (ExcelDataSet.Tables[0].Rows[0][43].ToString().Trim() != "5000")
                    //    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected bool UploadScheduleExcel(string ext, string filepath)
        {
            AirlineScheduleBAL OBJasb = new AirlineScheduleBAL();
            DataSet result = new DataSet();

            try
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables

                result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                //Validate Dataset Schema
                bool IsRightFormat = ValidateExcel("AirlineSchedule", result);

                if (IsRightFormat != true)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Please upload proper Airline Schedule Template";
                    return false;
                }
                int IsRouteInsert = 0;

                for (int j = 1; j < result.Tables[0].Rows.Count; j++)
                {


                    if (Convert.ToString(result.Tables[0].Rows[j][0]) == "")
                        break;

                    //try
                    //{
                    if (result.Tables[0].Rows[j][0].ToString() == "" || result.Tables[0].Rows[j][0].ToString() == null)
                    {
                        break;
                    }

                    //DateTime dt1 = DateTime.ParseExact(result.Tables[0].Rows[j][0].ToString(), "dd/MM/yyyy", null);
                    //DateTime dt2 = DateTime.ParseExact(result.Tables[0].Rows[j][1].ToString(), "dd/MM/yyyy", null);

                    //modified on 8/10/2014
                    DateTime dt1 = new DateTime();
                    if (result.Tables[0].Rows[j][0].ToString().Trim() != "")
                        dt1 = GetDateTimeFromExcelString(result.Tables[0].Rows[j][0].ToString());
                    else
                        dt1 = Convert.ToDateTime(DateTime.Today);

                    DateTime dt2 = new DateTime();
                    if (result.Tables[0].Rows[j][1].ToString().Trim() != "")
                        dt2 = GetDateTimeFromExcelString(result.Tables[0].Rows[j][1].ToString());
                    else
                        dt2 = Convert.ToDateTime(DateTime.Today);


                    string arrTime = "";
                    string DeptTime = "";

                    #region Prepare Parameters Commented by Deepak 23/04/2014
                    if (result.Tables[0].Rows[j][10].ToString() != "" || result.Tables[0].Rows[j][11].ToString() != null || result.Tables[0].Rows[j][11].ToString() != "" || result.Tables[0].Rows[j][11].ToString() != null)
                    {
                        //object[] ScheduleInfo = new object[13];
                        //int k = 0;

                        ////0
                        //ScheduleInfo.SetValue(dt1, k);
                        //k++;


                        ////1
                        //ScheduleInfo.SetValue(dt2, k);
                        //k++;

                        ////2
                        //ScheduleInfo.SetValue(result.Tables[0].Rows[j][2].ToString(), k);
                        //k++;

                        ////3
                        //ScheduleInfo.SetValue(result.Tables[0].Rows[j][3].ToString(), k);
                        //k++;

                        //string ActSource = result.Tables[0].Rows[j][10].ToString();
                        //if (result.Tables[0].Rows[j][10].ToString() == "" || result.Tables[0].Rows[j][11].ToString() == null)
                        //{
                        //    ActSource = result.Tables[0].Rows[j][4].ToString();
                        //}
                        ////4
                        //ScheduleInfo.SetValue(ActSource.Trim(), k);
                        //k++;

                        //string Actdest = result.Tables[0].Rows[j][11].ToString();
                        //if (result.Tables[0].Rows[j][11].ToString() == "" || result.Tables[0].Rows[j][11].ToString() == null)
                        //{
                        //    Actdest = result.Tables[0].Rows[j][5].ToString();
                        //}

                        ////5
                        //ScheduleInfo.SetValue(Actdest.Trim(), k);
                        //k++;


                        ////6
                        //DeptTime = result.Tables[0].Rows[j][6].ToString();// 
                        //DeptTime = DeptTime.PadLeft(4, '0');
                        //DeptTime = ConverttoTime(DeptTime);

                        //DateTime dtdept = new DateTime();
                        //dtdept = Convert.ToDateTime(DeptTime);
                        //DeptTime = dtdept.ToString("H:mm");

                        //ScheduleInfo.SetValue(DeptTime, k);
                        //k++;

                        ////7

                        //arrTime = result.Tables[0].Rows[j][7].ToString();
                        //arrTime = arrTime.PadLeft(4, '0');
                        //arrTime = ConverttoTime(arrTime);


                        //DateTime dtarrTime = new DateTime();
                        //dtarrTime = Convert.ToDateTime(arrTime);

                        //arrTime = dtarrTime.ToString("H:mm");

                        ////8
                        //ScheduleInfo.SetValue(arrTime, k);
                        //k++;

                        ////9
                        //ScheduleInfo.SetValue(dtCurrentDate, k);
                        //k++;

                        ////9
                        //ScheduleInfo.SetValue(strUserName, k);
                        //k++;

                        ////10
                        //ScheduleInfo.SetValue(result.Tables[0].Rows[j][9].ToString(), k);
                        //k++;

                        ////11
                        //string strfrquency = "";
                        //strfrquency = result.Tables[0].Rows[j][8].ToString();

                        ////12
                        //ScheduleInfo.SetValue(strfrquency, k);
                        //k++;


                        ////13
                        //ScheduleInfo.SetValue(result.Tables[0].Rows[j][15].ToString(), k);
                        //k++;


                    #endregion Prepare Parameters

                        #region New Code Added From Schedule App Deepak 23/04/2014
                        string[] paramNames = new string[14];
                        SqlDbType[] dataTypes = new SqlDbType[14];
                        int i = 0;

                        //0
                        paramNames.SetValue("Fromdate", i);
                        dataTypes.SetValue(SqlDbType.DateTime, i);
                        i++;

                        //1
                        paramNames.SetValue("ToDate", i);
                        dataTypes.SetValue(SqlDbType.DateTime, i);
                        i++;

                        //2
                        paramNames.SetValue("EquipmentNo", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //3
                        paramNames.SetValue("FlightID", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //4
                        paramNames.SetValue("Source", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //5
                        paramNames.SetValue("Dest", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //6
                        paramNames.SetValue("ScheduleDepttime", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //7

                        paramNames.SetValue("SchArrtime", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //8
                        paramNames.SetValue("UpdatedOn", i);
                        dataTypes.SetValue(SqlDbType.DateTime, i);
                        i++;

                        //9
                        paramNames.SetValue("UpdatedBy", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //10
                        paramNames.SetValue("CargoCapacity", i);
                        dataTypes.SetValue(SqlDbType.Float, i);
                        i++;

                        //11
                        paramNames.SetValue("frequency", i);
                        dataTypes.SetValue(SqlDbType.VarChar, i);
                        i++;

                        //12
                        paramNames.SetValue("IsDomestic", i);
                        dataTypes.SetValue(SqlDbType.Bit, i);
                        i++;

                        //13
                        paramNames.SetValue("SchArrDay", i);
                        dataTypes.SetValue(SqlDbType.NVarChar, i);
                        i++;

                        object[] ScheduleInfo = new object[14];
                        int k = 0;

                        //0
                        ScheduleInfo.SetValue(dt1, k);
                        k++;

                        //1
                        ScheduleInfo.SetValue(dt2, k);
                        k++;

                        //2
                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][2], k);
                        k++;

                        //3
                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][3].ToString().Replace(" ", "").Trim(), k);
                        k++;
                        string ActSource = result.Tables[0].Rows[j][12].ToString();
                        if (result.Tables[0].Rows[j][12].ToString() == "" || result.Tables[0].Rows[j][12] == null)
                        {
                            ActSource = result.Tables[0].Rows[j][4].ToString();
                        }

                        //4
                        ScheduleInfo.SetValue(ActSource.Trim(), k);
                        k++;

                        string Actdest = result.Tables[0].Rows[j][13].ToString();
                        if (result.Tables[0].Rows[j][13].ToString() == "" || result.Tables[0].Rows[j][13] == null)
                        {
                            Actdest = result.Tables[0].Rows[j][5].ToString().Trim();
                        }

                        //5
                        ScheduleInfo.SetValue(Actdest, k);
                        k++;


                        //6
                        string schsept = result.Tables[0].Rows[j][6].ToString().Trim();
                        schsept = schsept.PadLeft(4, '0');
                        schsept = ConverttoTime(schsept);

                        DateTime dtdept = new DateTime();
                        dtdept = Convert.ToDateTime(schsept);

                        if (result.Tables[0].Rows[j][6].ToString() == "" || result.Tables[0].Rows[j][6].ToString() == null)
                        {
                            schsept = dtdept.ToString("H:mm");
                        }
                        else
                        {
                            schsept = dtdept.ToString("H:mm");
                        }

                        ScheduleInfo.SetValue(schsept, k);
                        k++;

                        //7
                        string Scharr = result.Tables[0].Rows[j][14].ToString();
                        if (Scharr.Length <= 3)
                            Scharr = Scharr.PadLeft(4, '0');
                        Scharr = ConverttoTime(Scharr);

                        DateTime dtarr = new DateTime();
                        dtarr = Convert.ToDateTime(Scharr);

                        if (result.Tables[0].Rows[j][14].ToString() == "" || result.Tables[0].Rows[j][14].ToString() == null)
                        {
                            // Scharr = result.Tables[0].Rows[j][7].ToString();
                            Scharr = dtarr.ToString("H:mm");
                        }
                        else
                        {
                            //  Scharr = Scharr.Substring(Scharr.Length - 11, 5);
                            Scharr = dtarr.ToString("H:mm");

                        }

                        ScheduleInfo.SetValue(Scharr, k);
                        k++;
                        // break;
                        //8
                        if (result.Tables[0].Rows[j][9].ToString() == "")
                        {
                            ScheduleInfo.SetValue(((DateTime)Session["IT"]).ToString(), k);
                        }
                        else
                        { ScheduleInfo.SetValue(result.Tables[0].Rows[j][9].ToString(), k); }
                        k++;

                        //9
                        if (result.Tables[0].Rows[j][10].ToString() == "")
                        {
                            ScheduleInfo.SetValue(Session["UserName"].ToString(), k);
                        }
                        else
                        { ScheduleInfo.SetValue(result.Tables[0].Rows[j][10].ToString(), k); }
                        k++;

                        //10
                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][11], k);
                        k++;

                        //11
                        string fequency = result.Tables[0].Rows[j][8].ToString().Trim();//ConvertToFrquency(result.Tables[0].Rows[j][8].ToString().Trim());
                        ScheduleInfo.SetValue(fequency, k);
                        k++;

                        //12
                        ScheduleInfo.SetValue(true, k);
                        k++;
                        string val = result.Tables[0].Rows[j][12].ToString();

                        //13
                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][15].ToString(), k);
                        k++;
                        #endregion

                        if (!da.InsertData("spSaveAirlineSchedule_UploadApp", paramNames, dataTypes, ScheduleInfo))
                        //Call SP to Save database.
                        {
                            lblError.Text = "Error Save Airline schedule. Please try again...";
                            lblError.ForeColor = Color.Red;
                            return false;
                        }
                        else
                        { IsRouteInsert++; }

                    }

                    #region Commented Code
                    //object[] RouteInfo = new object[17];
                    //int m = 0;

                    ////0
                    //RouteInfo.SetValue(dt1, m);
                    //m++;


                    ////1
                    //RouteInfo.SetValue(dt2, m);
                    //m++;

                    ////2
                    //RouteInfo.SetValue(result.Tables[0].Rows[j][3].ToString(), m);
                    //m++;
                    //string Source = "";
                    //if (result.Tables[0].Rows[j][10].ToString() == "")
                    //{
                    //}
                    //else
                    //{
                    //    Source = result.Tables[0].Rows[j][10].ToString();
                    //}


                    ////3
                    //RouteInfo.SetValue(Source.Trim(), m);
                    //m++;

                    //string Dest = "";
                    //if (result.Tables[0].Rows[j][11].ToString() == "")
                    //{
                    //}
                    //else
                    //{
                    //    Dest = result.Tables[0].Rows[j][11].ToString();
                    //}



                    ////4
                    //RouteInfo.SetValue(Dest.Trim(), m);
                    //m++;





                    ////5
                    //RouteInfo.SetValue(result.Tables[0].Rows[j][4].ToString(), m);
                    //m++;
                    ////6 ActDestination
                    //RouteInfo.SetValue(result.Tables[0].Rows[j][5].ToString(), m);
                    //m++;


                    ////7 Frequency
                    //RouteInfo.SetValue(result.Tables[0].Rows[j][8].ToString(), m);
                    //m++;



                    ////8 ActDeptTime

                    //RouteInfo.SetValue(DeptTime, m);
                    //m++;

                    ////10 ActArrTime
                    //RouteInfo.SetValue(arrTime, m);
                    //m++;


                    ////11
                    //RouteInfo.SetValue(dtCurrentDate, m);
                    //m++;

                    //string UserID = strUserName;
                    ////12
                    //RouteInfo.SetValue(UserID, m);
                    //m++;

                    ////13
                    ////Parameter Added on 20 april
                    //RouteInfo.SetValue("Active", m);

                    //m++;


                    ////14
                    ////parameter added on 3May12
                    //string strDeptDay = "1";
                    //RouteInfo.SetValue(strDeptDay, m);
                    //m++;

                    ////15 ArrDay

                    //RouteInfo.SetValue(result.Tables[0].Rows[j][14].ToString(), m);
                    //m++;

                    ////16 CargoCapacity
                    //string strCapacity = result.Tables[0].Rows[j][9].ToString();
                    //RouteInfo.SetValue(strCapacity, m);
                    //m++;

                    ////17 EquipmentNo
                    //string strAirCarType = result.Tables[0].Rows[j][2].ToString();
                    //RouteInfo.SetValue(strAirCarType, m);
                    ////i++;



                    //IsRouteInsert = OBJasb.SaveAirlineRouteDetails(RouteInfo);
                    #endregion

                    flag = true;
                    uploadsheet = "AirlineSchedule";
                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);


                }
                if (IsRouteInsert < 0)
                {
                    lblError.Text = "Error Save Route Details. Please try again...";
                    lblError.ForeColor = Color.Red;
                    return false;
                }
                else
                {
                    lblError.ForeColor = Color.Green;
                    lblError.Text = "Airline Schedule Save Successfully";
                }



                return true;

            }
            catch (Exception ex)
            {

                lblError.Text = "Error while uploading file.";
                lblError.ForeColor = Color.Red;
                return false;

            }
            finally
            {

            }


        }

        private string ConvertToFrquency(String strFrequency)
        {
            if (strFrequency.Contains(','))
                return strFrequency;

            string frequency = "";

            string s = strFrequency;
            for (int p = 1; p < s.Length; p += 2)
            {
                s = s.Insert(p, ",");
            }

            return s;
        }


        private string ConverttoTime(String strTime)
        {
            if (strTime.Contains(':'))
                return strTime;

            string hour = strTime.Substring(0, 2);
            string min = strTime.Substring(2, 2);
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(hour + ":" + min);

            return dt.ToString();


        }

        protected void ASchUpload_Click(object sender, EventArgs e)
        {
            FileInfo fi = null;

            CheckFileExtension(flieUploadAirlineschedule, ref fi);

            filePath = Server.MapPath(path + flieUploadAirlineschedule.FileName);

            if (!UploadScheduleExcel(FileExtension, filePath))
            {
                lblError.Text = "File Uploaded Successfully.";
                lblError.ForeColor = System.Drawing.Color.Green;
                fi.Delete();
            }
            else
            {
                lblError.Text = "File not Processed, Check log file for details.";
                fi = new FileInfo(Server.MapPath(path + flieUploadAirlineschedule.FileName));
                //fileExists = fi.Exists;
                if (fi.Exists == true)
                {
                    fi.Delete();
                }
            }

        }

        protected void btnAirlineSchedule_Click(object sender, EventArgs e)
        {
            FileInfo fi = null;

            bool val = CheckFileExtension(flieUploadAirlineschedule, ref fi);
            if (val == true)
            {
                filePath = Server.MapPath(path + flieUploadAirlineschedule.FileName);

                if (!UploadScheduleExcel(FileExtension, filePath))
                {
                    lblError.Text = "Please try again....";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    fi.Delete();
                }
                else
                {
                    lblError.Text = "File Uploaded Successfully....";
                    fi = new FileInfo(Server.MapPath(path + flieUploadAirlineschedule.FileName));
                    //fileExists = fi.Exists;
                    if (fi.Exists == true)
                    {
                        fi.Delete();
                    }
                }
            }
            else
            {
                lblError.Text = "Please select appropriate file to upload.....";
                lblError.ForeColor = System.Drawing.Color.Red;
            }



        }

        protected void btnPartnerSchedule_Click(object sender, EventArgs e)
        {
            FileInfo fi = null;

            bool val = CheckFileExtension(flieUploadPartnerschedule, ref fi);
            if (val == true)
            {
                filePath = Server.MapPath(path + flieUploadPartnerschedule.FileName);

                if (!UploadPartnerScheduleExcel(FileExtension, filePath))
                {
                    lblError.Text = "File not Processed...Please check for valid data entry";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    fi.Delete();
                }
                else
                {
                    lblError.Text = "File Uploaded Successfully....";
                    fi = new FileInfo(Server.MapPath(path + flieUploadPartnerschedule.FileName));
                    //fileExists = fi.Exists;
                    if (fi.Exists == true)
                    {
                        fi.Delete();
                    }
                }
            }
            else
            {
                lblError.Text = "Please select appropriate file to upload.....";
                lblError.ForeColor = System.Drawing.Color.Red;
            }


        }

        protected bool UploadPartnerScheduleExcel(string ext, string filepath)
        {
            AirlineScheduleBAL OBJasb = new AirlineScheduleBAL();
            DataSet result = new DataSet();

            try
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables

                result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                //Validate Dataset Schema
                bool IsRightFormat = ValidateExcel("PartnerSchedule", result);

                if (IsRightFormat != true)
                {
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Please upload proper Partner Schedule Template";
                    return false;
                }
                int IsRouteInsert = 0;

                for (int j = 1; j < result.Tables[0].Rows.Count; j++)
                {


                    if (String.IsNullOrEmpty(result.Tables[0].Rows[j][0].ToString()))
                        // break;
                        continue;

                    //try
                    //{
                    if (result.Tables[0].Rows[j][0].ToString() == "" || result.Tables[0].Rows[j][0].ToString() == null)
                    {
                        break;
                    }

                    //DateTime dt1 = DateTime.ParseExact(result.Tables[0].Rows[j][0].ToString(), "dd/MM/yyyy", null);
                    //DateTime dt2 = DateTime.ParseExact(result.Tables[0].Rows[j][1].ToString(), "dd/MM/yyyy", null);
                    //dates updated on 8/10/2014
                    DateTime dt1 = new DateTime();
                    if (result.Tables[0].Rows[j][0].ToString().Trim() != "")
                        dt1 = GetDateTimeFromExcelString(result.Tables[0].Rows[j][0].ToString());
                    else
                        dt1 = Convert.ToDateTime(DateTime.Today);

                    DateTime dt2 = new DateTime();
                    if (result.Tables[0].Rows[j][1].ToString().Trim() != "")
                        dt2 = GetDateTimeFromExcelString(result.Tables[0].Rows[j][1].ToString());
                    else
                        dt2 = Convert.ToDateTime(DateTime.Today);

                    string DeptTime = "";
                    string arrTime = "";
                    #region Prepare Parameters
                    if (result.Tables[0].Rows[j][10].ToString() != "" || result.Tables[0].Rows[j][11].ToString() != null || result.Tables[0].Rows[j][11].ToString() != "" || result.Tables[0].Rows[j][11].ToString() != null)
                    {
                        object[] ScheduleInfo = new object[14];
                        int k = 0;

                        //0
                        ScheduleInfo.SetValue(dt1, k);
                        k++;


                        //1
                        ScheduleInfo.SetValue(dt2, k);
                        k++;

                        //2
                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][2].ToString(), k);
                        k++;

                        //3
                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][3].ToString(), k);
                        k++;

                        string ActSource = result.Tables[0].Rows[j][10].ToString();
                        if (result.Tables[0].Rows[j][10].ToString() == "" || result.Tables[0].Rows[j][11].ToString() == null)
                        {
                            ActSource = result.Tables[0].Rows[j][4].ToString();
                        }
                        //4
                        ScheduleInfo.SetValue(ActSource.Trim(), k);
                        k++;

                        string Actdest = result.Tables[0].Rows[j][11].ToString();
                        if (result.Tables[0].Rows[j][11].ToString() == "" || result.Tables[0].Rows[j][11].ToString() == null)
                        {
                            Actdest = result.Tables[0].Rows[j][5].ToString();
                        }

                        //5
                        ScheduleInfo.SetValue(Actdest.Trim(), k);
                        k++;


                        //6
                        DeptTime = result.Tables[0].Rows[j][6].ToString();// 
                        DeptTime = DeptTime.PadLeft(4, '0');
                        DeptTime = ConverttoTime(DeptTime);

                        DateTime dtdept = new DateTime();
                        dtdept = Convert.ToDateTime(DeptTime);
                        DeptTime = dtdept.ToString("H:mm");

                        ScheduleInfo.SetValue(DeptTime, k);
                        k++;

                        //7

                        arrTime = result.Tables[0].Rows[j][7].ToString();
                        arrTime = arrTime.PadLeft(4, '0');
                        arrTime = ConverttoTime(arrTime);


                        DateTime dtarrTime = new DateTime();
                        dtarrTime = Convert.ToDateTime(arrTime);

                        arrTime = dtarrTime.ToString("H:mm");

                        //8
                        ScheduleInfo.SetValue(arrTime, k);
                        k++;

                        //9
                        ScheduleInfo.SetValue(dtCurrentDate, k);
                        k++;

                        //9
                        ScheduleInfo.SetValue(strUserName, k);
                        k++;

                        //10
                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][9].ToString(), k);
                        k++;

                        //11
                        string strfrquency = "";
                        strfrquency = result.Tables[0].Rows[j][8].ToString();

                        //12
                        ScheduleInfo.SetValue(strfrquency, k);
                        k++;


                        //13
                        string isdomestic = result.Tables[0].Rows[j][15].ToString();
                        if (isdomestic.ToLower() == "y" || isdomestic == "1")
                            ScheduleInfo.SetValue(1, k);
                        else
                            ScheduleInfo.SetValue(0, k);
                        //ScheduleInfo.SetValue(result.Tables[0].Rows[j][15].ToString(), k);
                        k++;

                        ScheduleInfo.SetValue(result.Tables[0].Rows[j][16].ToString(), k);

                    #endregion Prepare Parameters
                        int IsSchduleInsert = 0;

                        IsSchduleInsert = OBJasb.SavePartnerSchedule(ScheduleInfo);
                        //Call SP to Save database.
                        if (IsSchduleInsert < 0)
                        {

                            lblError.Text = "Error Save Airline schedule. Please try again...";
                            return false;
                        }
                    }
                    object[] RouteInfo = new object[19];
                    int i = 0;

                    //0
                    RouteInfo.SetValue(dt1, i);
                    i++;


                    //1
                    RouteInfo.SetValue(dt2, i);
                    i++;

                    //2
                    RouteInfo.SetValue(result.Tables[0].Rows[j][3].ToString(), i);
                    i++;
                    string Source = "";
                    if (result.Tables[0].Rows[j][10].ToString() == "")
                    {
                    }
                    else
                    {
                        Source = result.Tables[0].Rows[j][10].ToString();
                    }


                    //3
                    RouteInfo.SetValue(Source.Trim(), i);
                    i++;

                    string Dest = "";
                    if (result.Tables[0].Rows[j][11].ToString() == "")
                    {
                    }
                    else
                    {
                        Dest = result.Tables[0].Rows[j][11].ToString();
                    }



                    //4
                    RouteInfo.SetValue(Dest.Trim(), i);
                    i++;





                    //5
                    RouteInfo.SetValue(result.Tables[0].Rows[j][4].ToString(), i);
                    i++;
                    //6 ActDestination
                    RouteInfo.SetValue(result.Tables[0].Rows[j][5].ToString(), i);
                    i++;


                    //7 Frequency
                    RouteInfo.SetValue(result.Tables[0].Rows[j][8].ToString(), i);
                    i++;



                    //8 ActDeptTime

                    RouteInfo.SetValue(DeptTime, i);
                    i++;

                    //10 ActArrTime
                    RouteInfo.SetValue(arrTime, i);
                    i++;


                    //11
                    RouteInfo.SetValue(dtCurrentDate, i);
                    i++;

                    string UserID = strUserName;
                    //12
                    RouteInfo.SetValue(UserID, i);
                    i++;

                    //13
                    //Parameter Added on 20 april
                    RouteInfo.SetValue("Active", i);

                    i++;


                    //14
                    //parameter added on 3May12
                    string strDeptDay = "1";
                    RouteInfo.SetValue(strDeptDay, i);
                    i++;

                    //15 ArrDay

                    RouteInfo.SetValue(result.Tables[0].Rows[j][14].ToString(), i);
                    i++;

                    //16 CargoCapacity
                    string strCapacity = result.Tables[0].Rows[j][9].ToString();
                    RouteInfo.SetValue(strCapacity, i);
                    i++;

                    //17 EquipmentNo
                    string strAirCarType = result.Tables[0].Rows[j][2].ToString();
                    RouteInfo.SetValue(strAirCarType, i);
                    i++;

                    //18 PartnerCode
                    string PartnerCode = result.Tables[0].Rows[j][16].ToString();
                    RouteInfo.SetValue(PartnerCode, i);
                    i++;
                    //19 tail no
                    RouteInfo.SetValue("", i);
                    IsRouteInsert = OBJasb.SavePartnerRouteDetails(RouteInfo);
                    flag = true;
                    uploadsheet = "PartnersSchecule";
                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);


                }
                if (IsRouteInsert < 0)
                {
                    lblError.Text = "Error Save Route Details. Please try again...";
                    return false;
                }
                else
                {
                    lblError.ForeColor = Color.Green;
                    lblError.Text = "Airline Schedule Save Successfully";
                }



                return true;

            }
            catch (Exception ex)
            {

                lblError.Text = "Error while uploading file.";
                return false;

            }
            finally
            {

            }


        }


        protected bool UploadExchangeRatesExcel(string ext, string filepath)
        {
            UploadMastersBAL objUpload = new UploadMastersBAL();
            DataSet result = new DataSet();

            try
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables

                result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                bool IsInsert = false;

                for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(result.Tables[0].Rows[i][0]) == "")
                        continue;

                    string CurrencyCode = Convert.ToString(result.Tables[0].Rows[i][0]);

                    double CurrencyIATARate = Convert.ToDouble(result.Tables[0].Rows[i][1]);

                    string Type = Convert.ToString(result.Tables[0].Rows[i][2]);
                    DateTime dt1, dt2;

                    dt1 = GetDateTimeFromExcelString(result.Tables[0].Rows[i][3].ToString());
                    dt2 = GetDateTimeFromExcelString(result.Tables[0].Rows[i][4].ToString());
                    string ValidFromDT = dt1.ToString("MM/dd/yyyy");
                    string ValidToDT = dt2.ToString("MM/dd/yyyy");

                    //Call Function to insert the data..

                    // string errormessage = string.Empty;
                    // errormessage = objUpload.InsertExchangeRates(CurrencyCode, CurrencyIATARate, Type, ValidFromDT, ValidToDT);
                    string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

                    SQLServer da = new SQLServer(constr);
                    IsInsert = da.InsertData("insert into tbliatacurrencyrecords(CurrencyCode,CurrencyIATARate,Type,ValidFrom,ValidTo) values('" + CurrencyCode + "','" + CurrencyIATARate + "','" + Type + "','" + ValidFromDT + "','" + ValidToDT + "')");

                }
                if (IsInsert == true)
                {
                    //Log.WriteLog("Error : Line No(" + i + ")" + errormessage);
                    flag = true;
                    uploadsheet = "ExchangeRate";
                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                    lblError.Text = "File Uploaded Successfully.";

                    return true;
                }
                else
                {
                    lblError.Text = "Error while uploading file.";
                    return false;
                }

                //Log.WriteLog("Inserted Successfully. Line No(" + i + ")");



            }
            catch
            {

            }
            finally
            {
                objUpload = null;
                result = null;
            }

            return true;
        }


        protected bool UploadGLAccountsExcel(string ext, string filepath)
        {
            UploadMastersBAL objUpload = new UploadMastersBAL();
            DataSet result = new DataSet();

            try
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    //...
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                }
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables

                result = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result1 = excelReader.AsDataSet();

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                //bool IsInsert = false;                
                bool IsInsert = false;
                for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                {
                    if (String.IsNullOrEmpty(result.Tables[0].Rows[i][0].ToString()))
                        continue;

                    string GLCode = Convert.ToString(result.Tables[0].Rows[i][0]);

                    string GLDescription = Convert.ToString(result.Tables[0].Rows[i][1]);

                    //Call Function to insert the data..

                    //string errormessage = string.Empty;

                    //errormessage = objUpload.InsertGLAccounts(GLCode, GLDescription);
                    string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;

                    SQLServer da = new SQLServer(constr);
                    DateTime CreatedOn = Convert.ToDateTime(Session["IT"].ToString());
                    string CreatedBy = Session["UserName"].ToString();
                    try
                    {
                        IsInsert = da.InsertData("insert into GLAccountmaster(GLAccountCode,GLAccountDescription,CreatedOn,CreatedBy,GLAccountName) values('" + GLCode + "','" + GLDescription + "','" + CreatedOn + "','" + CreatedBy + "','" + GLCode + "')");
                    }
                    catch (Exception e)
                    {
                        string exc = e.Message;
                    }
                }
                if (IsInsert == true)
                {
                    //Log.WriteLog("Error : Line No(" + i + ")" + errormessage);
                    flag = true;
                    uploadsheet = "GLAccount";
                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                    lblError.Text = "File Uploaded Successfully.";

                    return true;
                }
                else
                {
                    lblError.Text = "Error while uploading file.";
                    return false;
                }

                //Log.WriteLog("Inserted Successfully. Line No(" + i + ")");
            }

            catch
            {
                lblError.Text = "Error while uploading file.";
                return false;
            }
            finally
            {
                objUpload = null;
                result = null;
            }

            return true;
        }

        protected void btnExchangeRatesUpload_Click1(object sender, EventArgs e)
        {
            FileInfo fi = null;

            bool val = CheckFileExtension(ExchangeRatesUpload, ref fi);
            if (val == true)
            {
                filePath = Server.MapPath(path + ExchangeRatesUpload.FileName);

                if (UploadExchangeRatesExcel(FileExtension, filePath))
                {
                    lblError.Text = "File Uploaded Successfully.";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    fi.Delete();
                }
                else
                {
                    lblError.Text = "File not Processed, Check log file for details.";
                    fi = new FileInfo(Server.MapPath(path + ExchangeRatesUpload.FileName));
                    //fileExists = fi.Exists;
                    if (fi.Exists == true)
                    {
                        fi.Delete();
                    }
                }
            }
            else
            {
                lblError.Text = "Please select appropriate file to upload.....";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnGLAccountsUpload_Click1(object sender, EventArgs e)
        {
            FileInfo fi = null;

            bool val = CheckFileExtension(GLAccountsUpload, ref fi);
            if (val == true)
            {
                filePath = Server.MapPath(path + GLAccountsUpload.FileName);

                if (UploadGLAccountsExcel(FileExtension, filePath))
                {
                    lblError.Text = "File Uploaded Successfully.";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    fi.Delete();
                }
                else
                {
                    lblError.Text = "File not Processed, Check log file for details.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    fi = new FileInfo(Server.MapPath(path + GLAccountsUpload.FileName));
                    //fileExists = fi.Exists;
                    if (fi.Exists == true)
                    {
                        fi.Delete();
                    }
                }
            }
            else
            {
                lblError.Text = "Please select appropriate file to upload.....";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnNewRateUpload_Click(object sender, EventArgs e)
        {

            FileInfo fi = null;

            bool val = CheckFileExtension(newRatesFileUpload, ref fi);
            if (val == true)
            {

                filePath = Server.MapPath(path + newRatesFileUpload.FileName);

                if (!UploadRateExcelNew(FileExtension, filePath))
                {
                    lblError.Text = "File not Processed...Please check for valid data entry";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    fi.Delete();
                }
                else
                {
                    lblError.Text = "File Processed Successfully.";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    fi = new FileInfo(Server.MapPath(path + newRatesFileUpload.FileName));
                    //fileExists = fi.Exists;
                    if (fi.Exists == true)
                    {
                        fi.Delete();
                    }
                }
            }
            else
            {
                lblError.Text = "Please select appropriate file to upload.....";
                lblError.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected bool UploadRateExcelNew(string ext, string filepath)
        {
            UploadMastersBAL objUpload = new UploadMastersBAL();
            DataSet result = new DataSet();
            excelUpload objBAl = new excelUpload();

            try
            {
                if (ext != ".csv")
                {
                    FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader = null;
                    if (ext == ".xls")
                    {
                        //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        //...
                    }
                    else if (ext == ".xlsx")
                    {
                        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        //...
                    }
                    //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                    // DataSet result = new DataSet();
                    result = excelReader.AsDataSet();

                    //...
                    //4. DataSet - Create column names from first row
                    excelReader.IsFirstRowAsColumnNames = true;
                    //result1 = excelReader.AsDataSet();

                    //6. Free resources (IExcelDataReader is IDisposable)
                    excelReader.Close();

                }
                else
                {
                    StreamReader srCSV = new StreamReader(filepath);

                    DataTable dt = new DataTable();
                    try
                    {
                        string str = srCSV.ReadLine();
                        string[] arrCsvElement = str.Split(',');

                        for (int i = 0; i < arrCsvElement.Length; i++)
                        {
                            try
                            {
                                dt.Columns.Add(new DataColumn(arrCsvElement[i], typeof(string)));
                            }
                            catch (Exception objEx)
                            {
                            }
                        }

                        while ((str = srCSV.ReadLine()) != null)
                        {
                            try
                            {
                                arrCsvElement = str.Split(',');
                                DataRow dr = dt.NewRow();
                                for (int i = 0; i < arrCsvElement.Length; i++)
                                {
                                    try
                                    {
                                        dr[i] = arrCsvElement[i];
                                    }
                                    catch (Exception objEx)
                                    {
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                            catch (Exception objEx)
                            {
                            }

                        }
                        result.Tables.Add(dt);
                        //result1 = result;
                        srCSV.Close();
                        srCSV.Dispose();
                        srCSV = null;
                    }
                    catch (Exception objEx)
                    {
                    }
                }
                int coloumnCount = result.Tables[0].Columns.Count;
                for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                {
                    bool blnError = false;
                    bool blnInsert = true;

                    if (String.IsNullOrEmpty(result.Tables[0].Rows[i][1].ToString()))
                        continue;

                    string rateIDnew = Convert.ToString(result.Tables[0].Rows[i][0]);
                    if (rateIDnew.Length == 0)
                    {
                        rateIDnew = Convert.ToString("0");
                    }
                    string RateCardId = Convert.ToString(result.Tables[0].Rows[i][1]);
                    String Origin;
                    Origin = Convert.ToString(result.Tables[0].Rows[i][2]);
                    if (Origin == null)
                    {
                        return false;
                    }
                    if (Origin == "")
                    {
                        return false;
                    }
                    string Destination = string.Empty;

                    if (result.Tables[0].Rows[i][3].ToString().Trim() != "")
                        Destination = Convert.ToString(result.Tables[0].Rows[i][3]);


                    string CommodityCode = string.Empty;

                    if (result.Tables[0].Rows[i][4].ToString().Trim() != "")
                    {
                        CommodityCode = Convert.ToString(result.Tables[0].Rows[i][4]);
                        CommodityCode = CommodityCode.Trim(',');
                    }

                    string ProductType = string.Empty;

                    if (result.Tables[0].Rows[i][5].ToString().Trim() != "")
                    {
                        ProductType = Convert.ToString(result.Tables[0].Rows[i][5]);
                        ProductType = ProductType.Trim(',');
                    }

                    string FlightNo = string.Empty;
                    if (result.Tables[0].Rows[i][6].ToString().Trim() != "")
                    {
                        FlightNo = Convert.ToString(result.Tables[0].Rows[i][6]);
                        FlightNo = FlightNo.Trim(',');
                    }

                    string AgentCode = string.Empty;
                    if (result.Tables[0].Rows[i][7].ToString().Trim() != "")
                    {
                        AgentCode = Convert.ToString(result.Tables[0].Rows[i][7]);
                        AgentCode = AgentCode.Trim(',');
                    }

                    string Currency = string.Empty;
                    if (result.Tables[0].Rows[i][8].ToString().Trim() != "")
                        Currency = Convert.ToString(result.Tables[0].Rows[i][8]);

                    DateTime FromDT = new DateTime();
                    if (result.Tables[0].Rows[i][9].ToString().Trim() != "")
                        FromDT = GetDateTimeFromExcelString(result.Tables[0].Rows[i][9].ToString());
                    else
                        FromDT = Convert.ToDateTime(DateTime.Today);


                    DateTime ToDT = new DateTime();
                    if (result.Tables[0].Rows[i][10].ToString().Trim() != "")
                        ToDT = GetDateTimeFromExcelString(result.Tables[0].Rows[i][10].ToString());
                    else
                        ToDT = Convert.ToDateTime(DateTime.Today);


                    decimal AgentComission = 0;
                    if (result.Tables[0].Rows[i][11].ToString().Trim() != "")
                        AgentComission = decimal.Parse(result.Tables[0].Rows[i][11].ToString().Trim());

                    decimal Discount = 0;
                    if (result.Tables[0].Rows[i][12].ToString().Trim() != "")
                        Discount = decimal.Parse(result.Tables[0].Rows[i][12].ToString().Trim());

                    decimal Tax = 0;
                    if (result.Tables[0].Rows[i][13].ToString().Trim() != "")
                        Tax = decimal.Parse(result.Tables[0].Rows[i][13].ToString().Trim());


                    bool AllInRate = false;
                    if ((result.Tables[0].Rows[i][14].ToString().Trim().ToLower() == "y" || result.Tables[0].Rows[i][14].ToString().Trim() == "1")
                        && result.Tables[0].Rows[i][14].ToString().Trim() != "")
                        AllInRate = true;
                    else
                        AllInRate = false;

                    bool IsTact = false;
                    if ((result.Tables[0].Rows[i][15].ToString().Trim().ToLower() == "y" || result.Tables[0].Rows[i][15].ToString().Trim() == "1")
                        && result.Tables[0].Rows[i][15].ToString().Trim() != "")
                        IsTact = true;
                    else
                        IsTact = false;

                    bool IsHeavy = false;
                    if ((result.Tables[0].Rows[i][16].ToString().Trim().ToLower() == "y" || result.Tables[0].Rows[i][16].ToString().Trim() == "1")
                        && result.Tables[0].Rows[i][16].ToString().Trim() != "")
                        IsHeavy = true;
                    else
                        IsHeavy = false;



                    string IssueCarrier = string.Empty;
                    if (result.Tables[0].Rows[i][17].ToString().Trim() != "")
                        IssueCarrier = (string)result.Tables[0].Rows[i][17].ToString().Trim();


                    string FlightCarrier = string.Empty;
                    if (result.Tables[0].Rows[i][18].ToString().Trim() != "")
                        FlightCarrier = Convert.ToString(result.Tables[0].Rows[i][18]);


                    string SHC = string.Empty;
                    if (result.Tables[0].Rows[i][19].ToString().Trim() != "")
                        SHC = Convert.ToString(result.Tables[0].Rows[i][19]);

                    string dayOfWeek = string.Empty;
                    if (result.Tables[0].Rows[i][20].ToString().Trim() != "")
                        dayOfWeek = Convert.ToString(result.Tables[0].Rows[i][20]);

                    string DepIntFrom = string.Empty;
                    if (result.Tables[0].Rows[i][21].ToString().Trim() != "")
                        DepIntFrom = Convert.ToString(result.Tables[0].Rows[i][21]);

                    string DepIntTo = string.Empty;
                    if (result.Tables[0].Rows[i][22].ToString().Trim() != "")
                        DepIntTo = Convert.ToString(result.Tables[0].Rows[i][22]);

                    string GLCode = string.Empty;
                    if (result.Tables[0].Rows[i][23].ToString().Trim() != "")
                        GLCode = Convert.ToString(result.Tables[0].Rows[i][23]);


                    string SPAMarkup = string.Empty;
                    if (result.Tables[0].Rows[i][24].ToString().Trim() != "")
                        SPAMarkup = Convert.ToString(result.Tables[0].Rows[i][24]);


                    string EquipmentType = string.Empty;
                    if (result.Tables[0].Rows[i][25].ToString().Trim() != "")
                        EquipmentType = Convert.ToString(result.Tables[0].Rows[i][25]);


                    string TransitStation = string.Empty;
                    if (result.Tables[0].Rows[i][26].ToString().Trim() != "")
                        TransitStation = Convert.ToString(result.Tables[0].Rows[i][26]);


                    string ShipperCode = string.Empty;
                    if (result.Tables[0].Rows[i][27].ToString().Trim() != "")
                    {
                        ShipperCode = Convert.ToString(result.Tables[0].Rows[i][27]);
                        ShipperCode = ShipperCode.Trim(',');
                    }

                    string RateBase = string.Empty;
                    if (result.Tables[0].Rows[i][32].ToString().Trim() != "")
                        RateBase = Convert.ToString(result.Tables[0].Rows[i][32]);

                    bool IsInsert = false;

                    //Call Function to insert the data..

                    string errormessage = string.Empty;
                    int RateID = 0;
                    int updateratelineID = 0;
                    if (objUpload.InsertAllTypesRateLinesNew(RateCardId, Origin, Destination, CommodityCode, ProductType, FlightNo,
                           AgentCode, Currency, FromDT, ToDT, AgentComission, Discount, Tax, AllInRate, IsTact, IsHeavy, IssueCarrier,
                           FlightCarrier, SHC, dayOfWeek, DepIntFrom, DepIntTo, GLCode, SPAMarkup, EquipmentType, TransitStation, ShipperCode, RateBase,
                           ref errormessage, ref RateID, rateIDnew))
                    {
                        #region Weight Slab
                        decimal MIN = 0;
                        if (result.Tables[0].Rows[i][33].ToString().Trim() != "")
                            MIN = decimal.Parse(result.Tables[0].Rows[i][33].ToString().Trim());


                        decimal normal = 0;
                        if (result.Tables[0].Rows[i][34].ToString().Trim() != "")
                            normal = decimal.Parse(result.Tables[0].Rows[i][34].ToString().Trim());

                        string[] param = new string[] { "RateLineSrNo" };
                        SqlDbType[] dbtypes = new SqlDbType[] { SqlDbType.Int };
                        object[] values = new object[] { RateID };

                        if (!da.ExecuteProcedure("SP_DeleteRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while deleting previous slabs(" + RateID + ")";
                            return false;
                        }

                        param = new string[] { "RateLineSrNo", "SlabName", "Weight", "Charge" };
                        dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float };
                        values = new object[] { RateID, "M", Convert.ToDouble("0"), Convert.ToDouble("" + MIN) };
                        if (MIN > 0)
                        {
                            if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                            {
                                errormessage = "Error while inserting slab M(" + RateID + ")";
                                return false;
                            }
                        }

                        if (normal > 0)
                        {
                            values = new object[] { RateID, "N", Convert.ToDouble("0"), Convert.ToDouble("" + normal) };

                            if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                            {
                                errormessage = "Error while inserting slab N(" + RateID + ")";
                                return false;
                            }
                        }



                        for (int k = 35; k < coloumnCount; k++)
                        {
                            //delete rate line slabs before insert or update for RateLineSrNo

                            decimal weight = 0, charge = 0;
                            if (result.Tables[0].Rows[0][k].ToString().Trim() != "")
                            {
                                weight = decimal.Parse(result.Tables[0].Rows[0][k].ToString().Trim());
                            }
                            else
                            {
                                weight = 0;
                            }
                            if (result.Tables[0].Rows[i][k].ToString().Trim() != "")
                            {
                                charge = decimal.Parse(result.Tables[0].Rows[i][k].ToString().Trim());
                            }
                            else
                            {
                                charge = 0;
                            }
                            if (weight > 0 && charge > 0)
                            {
                                values = new object[] { RateID, "Q", Convert.ToDouble("" + weight), Convert.ToDouble("" + charge) };
                                if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                                {
                                    errormessage = "Error while inserting slab Q(" + RateID + ")";
                                    return false;
                                }

                            }

                        }
                        #endregion

                        #region ULD Slabs

                        string ULDType = string.Empty;
                        if (result.Tables[0].Rows[i][28].ToString().Trim() != "")
                            ULDType = Convert.ToString(result.Tables[0].Rows[i][28]);
                        string[] ULD = ULDType.Split(',');

                        string MiminumCharge = string.Empty;
                        if (result.Tables[0].Rows[i][29].ToString().Trim() != "")
                            MiminumCharge = Convert.ToString(result.Tables[0].Rows[i][29]);
                        string[] MinCh = MiminumCharge.Split(',');


                        string pivotwt = string.Empty;
                        if (result.Tables[0].Rows[i][30].ToString().Trim() != "")
                            pivotwt = Convert.ToString(result.Tables[0].Rows[i][30]);
                        string[] pwt = pivotwt.Split(',');

                        string pivotrate = string.Empty;
                        if (result.Tables[0].Rows[i][31].ToString().Trim() != "")
                            pivotrate = Convert.ToString(result.Tables[0].Rows[i][31]);
                        string[] prate = pivotrate.Split(',');

                        string ULDtype = string.Empty;
                        string Slab = string.Empty;
                        decimal pweight = 0;
                        decimal pcharge = 0;


                        for (int m = 28; m <= 28; m++)
                        {
                            for (int p = 0; p < ULD.Length; p++)
                            {
                                if (result.Tables[0].Rows[i][m].ToString().Trim() != "")
                                {

                                    ULDtype = ULD[p];

                                }
                                else
                                {
                                    if (!objUpload.DeleteRecord(RateID)) { }
                                }
                                if (result.Tables[0].Rows[i][m + 1].ToString().Trim() != "")
                                {
                                    Slab = "M";

                                    if (Slab == "M")
                                    {//minimumcharge
                                        pweight = 0;
                                        pcharge = Convert.ToDecimal(MinCh[p]);


                                    }
                                    if (!objUpload.InsertAllTypesULDSlabs(RateID, ULDtype, Slab, pweight, pcharge, errormessage)) { }
                                }
                                else
                                {
                                    if (!objUpload.DeleteRecord(RateID)) { }
                                }

                                if (result.Tables[0].Rows[i][m + 2].ToString().Trim() != "")
                                {//overpivot weight
                                    Slab = "OverPivot";
                                    pweight = Convert.ToDecimal(pwt[p]);

                                }
                                else
                                {

                                }
                                if (result.Tables[0].Rows[i][m + 3].ToString().Trim() != "")
                                {//overpivotcharge

                                    pcharge = Convert.ToDecimal(prate[p]);

                                }
                                else
                                {
                                    if (!objUpload.DeleteRecord(RateID)) { }
                                }
                                if (!objUpload.InsertAllTypesULDSlabs(RateID, ULDtype, Slab, pweight, pcharge, errormessage)) { }
                                else
                                {
                                    // if (!objUpload.DeleteRecordULD(ULDtype)) { }
                                }
                            }
                        }




                        #endregion


                    }

                }
                flag = true;
                uploadsheet = "NewRates";
                string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                lblError.Text = "File Uploaded Successfully.";
            }

            catch
            {
                lblError.Text = "Error while uploading file.";
                return false;
            }
            finally
            {
                objUpload = null;
                result = null;
            }

            return true;
        }

        public static DateTime FromExcelSerialDate(int SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1899, 12, 31).AddDays(SerialDate);
        }

        public static DateTime GetDateTimeFromExcelString(string Date)
        {
            DateTime ExcelDate = DateTime.Today;
            try
            {
                //string Date1 = DateTime.ParseExact(Date, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
                if (DateTime.TryParse(Date, out ExcelDate))
                {
                    ExcelDate = Convert.ToDateTime(Date);
                }
                Int64 i = 0;
                if (Int64.TryParse(Date, out i))
                {
                    ExcelDate = FromExcelSerialDate(Convert.ToInt32(Date));
                }
            }
            catch (Exception ex)
            { }
            return ExcelDate;
        }


    }
}
