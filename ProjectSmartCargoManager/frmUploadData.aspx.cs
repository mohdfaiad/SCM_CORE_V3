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
using BAL;
using System.Collections.Generic;
using Excel;
using ICSharpCode;
using System.Drawing;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmUploadData : System.Web.UI.Page
    {
        string logPath = string.Empty;
        string logfilename = string.Empty;
        DateTime dtCurrentDate = new DateTime();
        string strUserName = "";
        excelUpload obj=new excelUpload();        
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
                                  
                                    if (loader == "AWB")
                                        if (isload == dvv.Rows[0]["IsLoaded"].ToString())
                                            chkAWB.Checked = true;
                                  
                                    chkAWB.Enabled = false;
                                 

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

        //Partner Excel
        protected void btnAWBUpload_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (AWBFileUpload.HasFile)
                {
                    Session["WorkingFile"] = AWBFileUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (AWBFileUpload.FileName.Contains("-"))
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
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    FileInfo fi = new FileInfo(Server.MapPath(path + AWBFileUpload.FileName));

                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, AWBFileUpload.FileName);
                    AWBFileUpload.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                    //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + AWBFileUpload.FileName);
                    if (LoadPartnerExcelData(FileExtension, filePath) == true)
                    {
                        lblError.Text = "File Uploaded Successfully.";
                        lblError.ForeColor = System.Drawing.Color.Green;
                        fi.Delete();
                    }
                    else
                    {
                        if(lblError.Text=="")
                        lblError.Text = "File not Processed...";
                        lblError.ForeColor = Color.Red;
                        fi = new FileInfo(Server.MapPath(path + AWBFileUpload.FileName));
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
            object[] QueryValues = new object[10];
            string logPath = string.Empty;
            string logfilename = string.Empty;
            DateTime dtCurrentDate = new DateTime();
            string strUserName = "", Currency="", ProductType="", TotalAmount="";
            
            //excelUpload obj = new excelUpload();


            FileInfo fics = new FileInfo(Server.MapPath(path + AWBFileUpload.FileName + "_Log" + ".txt"));
            bool fileExists = fics.Exists;

            if (fileExists == true)
            {
                fics.Delete();
            }


            logfilename = AWBFileUpload.FileName + "_Log" + ".txt";
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
                dsPO = result1.Copy();
                DateTime Sysdate = DateTime.Parse(Session["IT"].ToString());
                string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                SQLServer da = new SQLServer(constr);
                string AWBPrefix="", AWBNumber="", Origin="", Destination="", FlightNo="", FlightDate="", Agentcode="",commodity="";
                int PCS=0;
                decimal Weight=0,Amount=0;
                             
                    //AWBPrefix,// dsPO.Tables[0].Rows[i][0].ToString().PadLeft(3,'0'),
                    //AWBNumber,// dsPO.Tables[0].Rows[i][1].ToString(),
                    //Origin,// dsPO.Tables[0].Rows[i][2].ToString(),
                    //Destination,// dsPO.Tables[0].Rows[i][3].ToString(),
                    //FlightNo,// dsPO.Tables[0].Rows[i][4].ToString(),
                    //FlightDate,// dsPO.Tables[0].Rows[i][5].ToString(),
                    //PCS,// Convert.ToInt32(dsPO.Tables[0].Rows[i][6].ToString().Replace(",",".")),
                    //Weight,// Convert.ToDecimal(dsPO.Tables[0].Rows[i][7].ToString().Replace(",",".")),
                    //Amount,// Convert.ToDecimal(dsPO.Tables[0].Rows[i][9].ToString().Replace(",",".")),
                    //Currency,// dsPO.Tables[0].Rows[i][8].ToString(),
                    //ProductType, dsPO.Tables[0].Rows[i][10].ToString(),
                    //Agentcode,// dsPO.Tables[0].Rows[i][11].ToString(),
                               
                if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                    {

                        if (!dsPO.Tables[0].Rows[i][0].ToString().Contains("-"))
                        {
                            #region Validations for Excel for AWBPrefix & AWBNumber Present Separately
                            AWBPrefix = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                            if (AWBPrefix == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }

                            AWBNumber = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                            if (AWBNumber == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            
                            Origin = (string)(dsPO.Tables[0].Rows[i][2].ToString());
                            if (Origin == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            
                            Destination = (string)(dsPO.Tables[0].Rows[i][3].ToString());
                            if (Destination == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            
                            FlightNo = (string)(dsPO.Tables[0].Rows[i][4].ToString());
                            if (FlightNo == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            
                            FlightDate = (string)(dsPO.Tables[0].Rows[i][5].ToString());
                            if (FlightDate == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            //DateTime dtFlight;
                            //if (!DateTime.TryParse(FlightDate, out dtFlight))
                            //{
                            //    lblError.Text = "Flight Date not in the correct format";
                            //    return false;
                            //}

                            string Pieces = (string)(dsPO.Tables[0].Rows[i][6].ToString());

                            if (Pieces == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            try
                            {
                                PCS= Convert.ToInt32(Pieces);
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "AWB pieces not in correct format";
                                return false;
                            }
                            QueryValues[5] = Pieces;

                            string wt = (string)(dsPO.Tables[0].Rows[i][7].ToString().Replace(",","."));
                            if (wt == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            try
                            {
                                Weight = Convert.ToDecimal(wt);
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "AWB Weight not in correct format";
                                return false;
                            }
                            

                            Currency = (string)(dsPO.Tables[0].Rows[i][8].ToString());
                            if (Currency == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }

                            string TotAMT = (string)(dsPO.Tables[0].Rows[i][9].ToString().Replace(",","."));
                            if (TotAMT == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            try
                            {
                                Amount = Convert.ToDecimal(TotAMT);
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "Total Amount not in correct format";
                                return false;
                            }

                            commodity = (string)(dsPO.Tables[0].Rows[i][10].ToString());
                            //if (CommodityCode == "")
                            //{
                            //    lblError.Text = "Fields marked * in excel are mandatory";
                            //    return false;
                            //}
                            
                            ProductType = (string)(dsPO.Tables[0].Rows[i][11].ToString());

                            
                            //string AgentCode = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                            //if (AgentCode == "")
                            //{
                            //    lblError.Text = "Fields marked * in excel are mandatory";
                            //    return false;
                            //}
                            //QueryValues[9] = AgentCode;

                            #endregion
                        }
                        else
                        {

                            #region Validations for Excel for AWBPrefix & AWBNumber Present combined with hyphen
                            string AWBNo = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                            if (AWBNo == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            if (AWBNo.Contains('-')) 
                            {
                                string[] str = AWBNo.Split('-');
                                AWBPrefix = str[0];
                                AWBNumber = str[1];
                            }

                            Origin = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                            if (Origin == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            
                            Destination = (string)(dsPO.Tables[0].Rows[i][2].ToString());
                            if (Destination == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            

                            FlightNo = (string)(dsPO.Tables[0].Rows[i][3].ToString());
                            if (FlightNo == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            
                            string FltDate = (string)(dsPO.Tables[0].Rows[i][4].ToString());
                            if (FltDate == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            DateTime dtFltDate;
                            if (!DateTime.TryParse(FltDate, out dtFltDate))
                            {
                                lblError.Text = "Flight Date not in the correct format";
                                return false;
                            }
                            else 
                            {
                                FlightDate = dtFltDate.ToString("dd/MM/yyyy");
                            }

                            string Pcs = (string)(dsPO.Tables[0].Rows[i][5].ToString().Replace(",","."));

                            if (Pcs == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            try
                            {
                                PCS = Convert.ToInt32(Pcs);
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "AWB pieces not in correct format";
                                return false;
                            }
                            
                            string Wgt = (string)(dsPO.Tables[0].Rows[i][6].ToString().Replace(",","."));
                            if (Wgt == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            try
                            {
                                Weight = Convert.ToDecimal(Wgt);
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "AWB Weight not in correct format";
                                return false;
                            }
                            

                            Currency = (string)(dsPO.Tables[0].Rows[i][7].ToString());
                            if (Currency == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }

                            TotalAmount = (string)(dsPO.Tables[0].Rows[i][8].ToString().Replace(",","."));
                            if (TotalAmount == "")
                            {
                                lblError.Text = "Fields marked * in excel are mandatory";
                                return false;
                            }
                            try
                            {
                                Amount = Convert.ToDecimal(TotalAmount);
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "Total Amount not in correct format";
                                return false;
                            }

                            
                            try
                            {
                                commodity = (string)(dsPO.Tables[0].Rows[i][10].ToString());
                            }
                            catch (Exception ex)
                            { }
                            

                            ProductType = string.Empty;
                            try
                            {
                                ProductType = (string)(dsPO.Tables[0].Rows[i][11].ToString());
                            }
                            catch (Exception ex)
                            { }
                            try
                            {
                                Agentcode = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                            }
                            catch (Exception ex) { }
                                //if (Agentcode == "")
                            //{
                            //    lblError.Text = "Fields marked * in excel are mandatory";
                            //    return false;
                            //}
                           

                            #endregion
                        }

                        try 
                        {
                            if (AWBPrefix.Length > 0 && AWBNumber.Length > 0)
                            {
                                string[] PName = new string[] 
                            { 
                                "AWBPrefix" ,
                                "AWBNumber" ,
                                "Origin" ,
                                "Destination" ,
                                "FlightNo" ,
                                "FlightDate" ,				  
                                "PCS" ,
                                "Weight" ,
                                "Amount" ,
                                "Currency" ,
                                "ProductType" ,
                                "AgentCode",
                                "UploadedBy" ,
                                "UploadedOn"
                            };
                                object[] PValue = new object[] 
                            {
                               AWBPrefix,// dsPO.Tables[0].Rows[i][0].ToString().PadLeft(3,'0'),
                               AWBNumber,// dsPO.Tables[0].Rows[i][1].ToString(),
                               Origin,// dsPO.Tables[0].Rows[i][2].ToString(),
                               Destination,// dsPO.Tables[0].Rows[i][3].ToString(),
                               FlightNo,// dsPO.Tables[0].Rows[i][4].ToString(),
                               FlightDate,// dsPO.Tables[0].Rows[i][5].ToString(),
                               PCS,// Convert.ToInt32(dsPO.Tables[0].Rows[i][6].ToString().Replace(",",".")),
                               Weight,// Convert.ToDecimal(dsPO.Tables[0].Rows[i][7].ToString().Replace(",",".")),
                               Amount,// Convert.ToDecimal(dsPO.Tables[0].Rows[i][9].ToString().Replace(",",".")),
                               Currency,// dsPO.Tables[0].Rows[i][8].ToString(),
                               ProductType,// dsPO.Tables[0].Rows[i][10].ToString(),
                               Agentcode,// dsPO.Tables[0].Rows[i][11].ToString(),
                                Session["UserName"].ToString(),
                                Sysdate
                                
                            };
                                SqlDbType[] PType = new SqlDbType[] 
                            {
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.Int,
                                SqlDbType.Decimal,
                                SqlDbType.Decimal,
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.VarChar,
                                SqlDbType.DateTime
                            };
                                IsInsert = da.InsertData("spUploadAWBData", PName, PType, PValue);
                                if (IsInsert)
                                {
                                    da.ExecuteProcedure("spProcessUploadedAWBData");
                                }

                            }
                        }
                        catch (Exception ex) { }

                    }
                    try
                    {
                        //Process Uploaded data
                        da.ExecuteProcedure("spProcessUploadedAWBData");
                    }
                    catch (Exception ex) { }
                }

                return IsInsert;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #region validateAndInsertFFMData
        /*
         *Updated on 5/8/2013
         *Data insert into ExportManifestTable
         */
        private bool validateAndInsertFFMData(object[] QueryValues)
        {
            try
            {

                string flightnum = QueryValues[3].ToString();
                string date = QueryValues[4].ToString();
                DateTime dtFlightDate = DateTime.Parse(date);
                date = dtFlightDate.ToString("dd/MM/yyyy");

                string source = "", dest = "";
                string[] PName = new string[] { "flightnum", "date" };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar };
                object[] PValue = new object[] { flightnum, date };
                DataSet ds = da.SelectRecords("spGetDestCodeForFFM", PName, PValue, PType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            source = ds.Tables[0].Rows[0]["source"].ToString();
                            dest = ds.Tables[0].Rows[0]["Dest"].ToString();
                        }
                    }
                }

                source = QueryValues[1].ToString();

                string origin = QueryValues[1].ToString();
                string awbdest = QueryValues[2].ToString();
                string awbnum = QueryValues[0].ToString();
                string unloadingairport = dest;
                string mftpcscnt = QueryValues[5].ToString();
                string mftweight = QueryValues[6].ToString();
                string SCC = QueryValues[7].ToString(); 
                string awbprefix = "";
                string ProductType = QueryValues[8].ToString();
                string AgentCode = QueryValues[9].ToString();
                if (ProductType == "")
                { ProductType = "JetPack"; }
                if (awbnum.Contains("-"))
                {
                    awbprefix = awbnum.Substring(0, awbnum.IndexOf("-"));
                }
                string awbnumwoprefix = "";
                if (awbnum.Contains("-"))
                {
                    awbnumwoprefix = awbnum.Substring(awbnum.IndexOf("-") + 1);
                }
                try
                {
                    int ManifestID = ExportManifestSummary(flightnum, source, unloadingairport, dtFlightDate);
                    if (ManifestID > 0)
                    {

                        #region Check Availabe ULD data if Present insert into ExportManifestULDAWB Association
                        string ULDNo = "Bulk";
                        try
                        {
                            if (ExportManifestULDAWBAssociation(ULDNo, source, unloadingairport, origin, awbdest, awbnumwoprefix,
                                SCC, "0", mftpcscnt, mftweight, "GEN", date, ManifestID, awbprefix, flightnum))
                            {
                                if (!ULDawbAssociation(flightnum, origin, unloadingairport, awbnumwoprefix,
                                    mftpcscnt, mftweight, dtFlightDate, ULDNo))
                                {
                                    lblError.Text = "Error [1] in FFM ULDawbAssociation Data:" + ULDNo;
                                    lblError.ForeColor = Color.Red;
                                    return (false);
                                }
                            }
                            else
                            {
                                lblError.Text = "Error [2] in generating Manifest ULD details: " + awbnum;
                                lblError.ForeColor = Color.Red;
                                return (false);
                            }
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = "Error [3] in generating Manifest Details & Summary for ULD: " + ex.Message;
                            lblError.ForeColor = Color.Red;
                            return (false);
                        }
                        #endregion

                        #region Add Consigment Details
                        try
                        {

                            #region Store in Manifest Tables
                            try
                            {
                                if (!ExportManifestDetails(ULDNo, source, unloadingairport, origin, awbdest,
                                    awbnumwoprefix, SCC, "0", mftpcscnt, mftweight, "GEN", dtFlightDate,
                                    ManifestID, flightnum, awbprefix))
                                {
                                    lblError.Text = "Error [4] in generating Manifest Details for AWB:" + awbnum;
                                    lblError.ForeColor = Color.Red;
                                    return (false);
                                }
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "Error [5] in generating Manifest Details: " + ex.Message;
                                lblError.ForeColor = Color.Red;
                                return (false);
                            }
                            #endregion

                            #region Status Message in Table
                            string[] PVName = new string[] { "AWBPrefix", "AWBNumber", "MType", "desc", "date", "time", "refno" };
                            object[] PValues = new object[] { awbprefix, awbnumwoprefix, "M ARR", origin + "-" + flightnum + "-" + date, "", "", 0 };
                            SqlDbType[] sqlType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.Int };
                            da.InsertData("spInsertAWBMessageStatus", PVName, sqlType, PValues);

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            lblError.Text = "Error in creating Manifest [6]: " + ex.Message;
                            lblError.ForeColor = Color.Red;
                            return (false);
                        }
                        #endregion

                        #region Add Consigment Details
                        try
                        {

                            #region Store in booking table

                            bool isAWBPresent = false;
                            #region Check AWB Present or Not
                            DataSet dsCheck = new DataSet();
                            string[] pname = new string[] { "AWBNumber", "AWBPrefix" };
                            object[] values = new object[] { awbnumwoprefix, awbprefix };
                            SqlDbType[] ptype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                            dsCheck = da.SelectRecords("sp_getawbdetails", pname, values, ptype);

                            if (dsCheck != null)
                            {
                                if (dsCheck.Tables.Count > 0)
                                {
                                    if (dsCheck.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsCheck.Tables[0].Rows[0]["AWBNumber"].ToString().Equals(awbnumwoprefix, StringComparison.OrdinalIgnoreCase))
                                        {
                                            isAWBPresent = true;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Add AWB details
                            if (!isAWBPresent)
                            {
                                string[] paramname = new string[] { "AirlinePrefix", "AWBNum", "Origin", "Dest", "PcsCount", 
                                    "Weight", "Volume","ComodityCode", "ComodityDesc", "CarrierCode", "FlightNum", 
                                    "FlightDate", "FlightOrigin", "FlightDest","ShipperName", "ShipperAddr", "ShipperPlace", 
                                    "ShipperState", "ShipperCountryCode", "ShipperContactNo", "ConsName", "ConsAddr", 
                                    "ConsPlace","ConsState", "ConsCountryCode", "ConsContactNo", "CustAccNo", 
                                    "IATACargoAgentCode", "CustName", "SystemDate", "MeasureUnit", "Length", "Breadth", 
                                    "Height", "PartnerStatus","REFNo","ProductType"};

                                object[] paramvalue = new object[] { awbprefix,awbnumwoprefix,origin,awbdest,mftpcscnt,
                                    mftweight,"",SCC,"", flightnum.Substring(0,2), flightnum, date, 
                                    source,dest,"","","","","","", "","", "","","","","", AgentCode, "", 
                                    Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd"),"", "", "", "", "" ,0,ProductType};

                                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                              SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                              SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.Int,SqlDbType.VarChar };

                                string procedure = "spInsertBookingDataFromFFR";
                                if (!da.InsertData(procedure, paramname, paramtype, paramvalue))
                                {
                                    lblError.Text = "Error [7] in generating AWB record for: " + awbnum;
                                    lblError.ForeColor = Color.Red;
                                }
                            }
                            #endregion

                            #region ProcessRateFunction
                            //try
                            //{
                            //    string[] CNname = new string[] { "AWBNumber", "AWBPrefix" };
                            //    SqlDbType[] CType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                            //    object[] CValues = new object[] { awbnumwoprefix, awbprefix };
                            //    //if (!dtb.ExecuteProcedure("sp_CalculateFreightChargesforMessage", "AWBNumber", SqlDbType.VarChar, awbnum))
                            //    if (!da.ExecuteProcedure("sp_CalculateFreightChargesforMessage", CNname, CType, CValues))
                            //    {
                            //        clsLog.WriteLog("Rates Not Calculated for:" + awbnum + Environment.NewLine + "Error: " + da.LastErrorDescription);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    clsLog.WriteLog("Rate Process Exception:" + ex.Message);
                            //}
                            #endregion
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            lblError.Text = "Error [8] in generating booking record: " + ex.Message;
                            lblError.ForeColor = Color.Red;
                            return (false);
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error [9] in StoreImportFFM_Summary Data: " + ex.Message;
                    lblError.ForeColor = Color.Red;
                    return (false);
                }
                return (true);
            }
            catch (Exception ex)
            {
                lblError.Text = ("Error [10] in generating manifest record in ValidateAndInsertFFM: " + ex.Message);
                lblError.ForeColor = Color.Red;
                return (false);
            }
        }

        public bool ULDawbAssociation(string FltNo, string POL, string POU, string AWBno, string PCS, string WGT, DateTime FltDate, string ULDNo)
        {
            bool res;
            try
            {
                string[] param = { "ULDtripid", "ULDNo", "AWBNumber", "POL", "POU", "FltNo", "Pcs", "Wgt", "AvlPcs", 
                                     "AvlWgt", "Updatedon", "Updatedby", "Status", "Manifested", "FltDate" };

                int _pcs = int.Parse(PCS);
                float _wgt = float.Parse(WGT);
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar
                                             , SqlDbType.Int, SqlDbType.Float, SqlDbType.Int, SqlDbType.Float,SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Bit,SqlDbType.Bit, SqlDbType.DateTime };
                object[] values = { "", ULDNo, AWBno, POL, POU, FltNo, 0, 0, _pcs, _wgt, DateTime.Now, "FFM", false, false, FltDate };


                //res = db.InsertData("SPImpManiSaveUldAwbAssociation", param, sqldbtypes, values);
                if (da.InsertData("SPImpManiSaveUldAwbAssociation", param, sqldbtypes, values))
                {
                    res = true;
                }
                else
                {
                    clsLog.WriteLog("ULDAWBAssociation Save Failed :" + AWBno + " Error: " + da.LastErrorDescription);
                    res = false;
                }

            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error in FFM ULDAWBAssociation:" + ex.Message);
                res = false;
            }
            return res;
        }

        #region FFM data to ExportManifest

        #region Export Manifest Summary Data Insert
        public int ExportManifestSummary(string FlightNo, string POL, string POU, DateTime FltDate)
        {
            int ID = 0;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] param = { "FLTno", "POL", "POU", "FLTDate" };
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
                object[] values = { FlightNo, POL, POU, FltDate };

                ID = db.GetIntegerByProcedure("spExpManifestSummaryFFM", param, values, sqldbtypes);
                if (ID < 1)
                {
                    clsLog.WriteLog("Error saving ExportFFM:" + db.LastErrorDescription);
                }
                //res = db.InsertData("SPImpManiSaveManifest", param, sqldbtypes, values);


            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error saving ExpFFM:" + ex.Message);
                ID = 0;

            }
            return ID;
        }
        #endregion

        #region ExportManifestDetails
        public bool ExportManifestDetails(string ULDNo, string POL, string POU, string ORG, string DES, string AWBno,
            string SCC, string VOL, string PCS, string WGT, string Desc, DateTime FltDate, int ManifestID,
            string FlightNo, string AWBPrefix)
        {
            bool res;
            try
            {
                string[] param = { "POL", "POU", "ORG", "DES", "AWBno", "SCC", "VOL", "PCS", "WGT", "Desc", "FLTDate", "ManifestID", "FlightNo", "AWBPrefix" };
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { POL, POU, ORG, DES, AWBno, SCC, VOL, PCS, WGT, Desc, FltDate, ManifestID, FlightNo, AWBPrefix };
                //res = db.InsertData("spExpManifestDetailsFFM", param, sqldbtypes, values);
                if (da.InsertData("spExpManifestDetailsFFM", param, sqldbtypes, values))
                {
                    res = true;
                }
                else
                {
                    clsLog.WriteLog("Failes ManifDetails Save:" + AWBno + " Error: " + da.LastErrorDescription);
                    res = false;
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error saving ImportFFM [" + DateTime.Now + "]");
                res = false;
            }
            return res;
        }
        #endregion

        #region ExportManifestULDAWBAssociation
        public bool ExportManifestULDAWBAssociation(string ULDNo, string POL, string POU, string ORG, string DES, string AWBno, string SCC, string VOL, string PCS, string WGT, string Desc, string FltDate, int ManifestID, string awbprefix, string flightno)
        {
            bool res;
            try
            {
                string[] param = { "ULDNo", "POL", "POU", "ORG", "DES", "AWBno", "SCC", "VOL", "PCS", "WGT", "Desc", "FLTDate", "ManifestID", "AWBPrefix", "FlightNo" };
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { ULDNo, POL, POU, ORG, DES, AWBno, SCC, VOL, PCS, WGT, Desc, FltDate, ManifestID, awbprefix, flightno };
                //res = db.InsertData("spExpManifestULDAWBFFM", param, sqldbtypes, values);
                if (da.InsertData("spExpManifestULDAWBFFM", param, sqldbtypes, values))
                {
                    res = true;
                }
                else
                {
                    clsLog.WriteLog("Failes ManifDetails Save:" + AWBno + " Error: " + da.LastErrorDescription);
                    res = false;
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error saving EXPULDAWB:" + ex.Message);
                res = false;
            }
            return res;
        }
        #endregion

        #endregion

        public bool UpdateAWBRateDetails(string AWBNumber,string CommodityCode,string PayMode,int Pieces,float Weight,float Total, string Currency)
        {
            try
            {
                SQLServer dtb = new SQLServer(Global.GetConnectionString());
                string[] AWBDetails = AWBNumber.Split('-');
                if (CommodityCode == "")
                {
                    CommodityCode = "GEN";
                }
                string[] param = new string[]
                {   "AWBNumber", 
                    "CommCode", 
                    "PayMode", 
                    "Pcs", 
                    "Wt", 
                    "FrIATA", 
                    "FrMKT", 
                    "ValCharge", 
                    "OcDueCar", 
                    "OcDueAgent", 
                    "SpotRate", 
                    "DynRate", 
                    "ServiceTax", 
                    "Total", 
                    "RatePerKg", 
                    "Currency",
                "AWBPrefix"};
                SqlDbType[] dbtypes = new SqlDbType[] 
                { SqlDbType.VarChar, 
                    SqlDbType.VarChar, 
                    SqlDbType.VarChar, 
                    SqlDbType.Int, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Float, 
                    SqlDbType.Decimal, 
                    SqlDbType.VarChar,
                SqlDbType.VarChar
                };
                object[] values = new object[] 
                {
                    AWBDetails[1],
                    CommodityCode,
                    PayMode,
                    Pieces,
                   Weight,
                    Total,
                    Total,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    Total,
                    Total/Weight,
                    Currency,
                    AWBDetails[0]
                };
                if (!dtb.UpdateData("SP_SaveAWBRatesviaMsg", param, dbtypes, values))
                {
                    clsLog.WriteLog("Error Saving FWB rates for:" + AWBNumber);
                    return false;
                }
                else
                { return true; }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion validateAndInsertFFMData

    
    
    }
}
