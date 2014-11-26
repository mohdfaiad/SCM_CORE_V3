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



namespace ProjectSmartCargoManager
{
    public partial class UploadPaxLoad : System.Web.UI.Page
    {
        string logPath = string.Empty;
        string logfilename = string.Empty;
        DateTime dtCurrentDate = new DateTime();
        string strUserName = "";
        excelUpload obj=new excelUpload();
        string path = ConfigurationManager.AppSettings["PaxLoadPath"].ToString();



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtCurrentDate = (DateTime)Session["IT"];
                strUserName = Convert.ToString(Session["UserName"]);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;
            
                if (FileExcelUpload.HasFile)
                {
                    Session["WorkingFile"] = FileExcelUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (FileExcelUpload.FileName.Contains("-"))
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
                  
                    FileInfo fi = new FileInfo(Server.MapPath(path+ FileExcelUpload.FileName));
                   
                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, FileExcelUpload.FileName);
                    FileExcelUpload.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                  //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + FileExcelUpload.FileName);
                    if (!LoadExcelData(FileExtension, filePath))
                    {
                        lblError.Text = "Error in data upload";
                        fi.Delete();
                    }
                    else
                    {
                        lblError.Text = "File Processed, Check log file for details.";
                        fi = new FileInfo(Server.MapPath(path + FileExcelUpload.FileName));
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

        #region Load DataSet
        private bool LoadExcelData(string ext, string filepath)
        {
            try
            {
                FileInfo fics = new FileInfo(Server.MapPath(path + FileExcelUpload.FileName + "_Log" + ".txt"));
                bool fileExists = fics.Exists;
               
                if (fileExists == true)
                {
                    fics.Delete();
                }


                logfilename = FileExcelUpload.FileName + "_Log" + ".txt";
                logPath = Server.MapPath(path + logfilename);
                Session["logpath"] = logPath;
                Session["logfilename"] = logfilename;
                string connString = string.Empty;
                ////if (ext == ".xls")
                ////{
                ////    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString()+"'";
                   
                ////}
                ////else if (ext == ".xlsx")
                ////{
                ////    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='" + Convert.ToChar(34).ToString() + @"Excel 8.0;Imex=1;HDR=Yes;" + Convert.ToChar(34).ToString() + "'";
                ////}
                ////OleDbConnection oledbConn = new OleDbConnection(connString);
                ////clsLog.WriteLog("FilePath : " + filepath + " Read On : " + System.DateTime.Now.ToString(), logPath);
                ////try
                ////{
                ////    oledbConn.Open();



                //if (ext == ".xls")
                //{
                //    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                //   // connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
                //}
                //else if (ext == ".xlsx")
                //{
                //    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
                //}



                //OleDbConnection oledbConn = new OleDbConnection(connString);
                //clsLog.WriteLog("FilePath : " + filepath + " Read On : " + System.DateTime.Now.ToString(), logPath);
                //try
                //{
                //    oledbConn.Open();

                //   char[] charsToTrim = { '$', '[', ']', '\'' };
                //    string ExcelName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString().Trim(charsToTrim);
                //    string query = "select * from [" + ExcelName + "$]";
                //    clsLog.WriteLog("Sheet Name : " + ExcelName + " at " + System.DateTime.Now.ToString(), logPath);
                //    OleDbCommand cmd = new OleDbCommand(query, oledbConn);
                //    OleDbDataAdapter oleda = new OleDbDataAdapter();
                //    oleda.SelectCommand = cmd;
                //    DataSet dsPO = new DataSet();
                //    oleda.Fill(dsPO, "PaxLoad");
                ////if (ext == ".xls")
                ////{
                ////    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                ////}
                ////else if (ext == ".xlsx")
                ////{
                ////    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
                ////}
                ////OleDbConnection oledbConn = new OleDbConnection(connString);
                ////clsLog.WriteLog("FilePath : " + filepath + " Read On : " + System.DateTime.Now.ToString(), logPath);


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

                //5. Data Reader methods
                //while (excelReader.Read())
                //{
                //    //excelReader.GetInt32(0);
                //}

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();



                try
                {
                    //oledbConn.Open();
                    //char[] charsToTrim = { '$', '[', ']', '\'' };
                    //string ExcelName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString().Trim(charsToTrim);
                    //string query = "select * from [" + ExcelName + "$]";
                    //clsLog.WriteLog("Sheet Name : " + ExcelName + " at " + System.DateTime.Now.ToString(), logPath);
                    //OleDbCommand cmd = new OleDbCommand(query, oledbConn);
                    //OleDbDataAdapter oleda = new OleDbDataAdapter();
                    //oleda.SelectCommand = cmd;
                    DataSet dsPO = new DataSet();
                    //oleda.Fill(dsPO, "Order");
                    dsPO = result1.Copy();
                    if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                    {
                        int chkheader = 0;
                        #region find first column as empty
                        for (int j = 0; j < dsPO.Tables[0].Rows.Count; j++)
                        {
                            if (dsPO.Tables[0].Rows[j][0].ToString().Trim() == "")
                            {
                                dsPO.Tables[0].Rows[j].Delete();
                                dsPO.Tables[0].AcceptChanges();
                                j = j - 1;
                            }
                        }
                        #endregion
                        #region checking from where column start as it is from header or first row
                        if (dsPO.Tables[0].Columns[0].ColumnName == "DepartureDate")
                        {
                            chkheader = 0;
                        }
                        else if (dsPO.Tables[0].Rows[0][1].ToString() == "DepartureDate")
                        {
                            chkheader = 1;
                        }
                        #endregion
                        #region conforming from where header start, is it from first row or header itself
                        for (int j = 0; j < dsPO.Tables[0].Rows.Count; j++)
                        {
                            if (dsPO.Tables[0].Rows[j][1].ToString().Trim() != "DepartureDate" && dsPO.Tables[0].Columns[1].ColumnName != "DepartureDate")
                            {
                                dsPO.Tables[0].Rows[j].Delete();
                                dsPO.Tables[0].AcceptChanges();
                                j = j - 1;
                            }
                            else
                            {
                                break;
                            }
                        }

                        for (int j = 0; j < dsPO.Tables[0].Columns.Count; j++)
                        {
                            if (chkheader == 0)
                            {
                                if (dsPO.Tables[0].Columns[j].ColumnName.Trim().ToUpper() == "")
                                {
                                    continue;
                                }
                                dsPO.Tables[0].Columns[j].ColumnName = dsPO.Tables[0].Columns[j].ColumnName.Trim().ToUpper();
                            }
                            else
                            {
                                if (dsPO.Tables[0].Rows[0][j].ToString().Trim().ToUpper() == "")
                                {
                                    continue;
                                }
                                dsPO.Tables[0].Columns[j].ColumnName = dsPO.Tables[0].Rows[0][j].ToString().Trim().ToUpper();
                            }
                        }
                        if (chkheader == 1)
                        {
                            dsPO.Tables[0].Rows[0].Delete();
                            dsPO.Tables[0].AcceptChanges();
                        }
                        #endregion
                        #region checking dat all require fields available or not
                        if (!dsPO.Tables[0].Columns.Contains("EquipmentType") || !dsPO.Tables[0].Columns.Contains("DepartureDate") || !dsPO.Tables[0].Columns.Contains("DepartureStation") || !dsPO.Tables[0].Columns.Contains("ArrivalStation") || !dsPO.Tables[0].Columns.Contains("CarrierCode") || !dsPO.Tables[0].Columns.Contains("FlightNumber") || !dsPO.Tables[0].Columns.Contains("OpSuffix") || !dsPO.Tables[0].Columns.Contains("LidOrCap") || !dsPO.Tables[0].Columns.Contains("Sold") || !dsPO.Tables[0].Columns.Contains("NonStopSold") || !dsPO.Tables[0].Columns.Contains("ThruSold") || !dsPO.Tables[0].Columns.Contains("CnxSold"))
                        {
                            clsLog.WriteLog("---------------Error Start---------------", logPath);
                            clsLog.WriteLog("Not find the Required Columns : {EquipmentType,DepartureDate,DepartureStation,ArrivalStation,CarrierCode,FlightNumber,OpSuffix,LidOrCap,Sold,NonStopSold,ThruSold,CnxSold} at " + System.DateTime.Now.ToString(), logPath);
                            clsLog.WriteLog("---------------End---------------", logPath);
                            return false;
                        }
                        #endregion
                        dsPO.Tables[0].Columns.Add("CreatedOn");
                        dsPO.Tables[0].Columns.Add("CreatedBy");
                        clsLog.WriteLog("Total Lines before validation : " + dsPO.Tables[0].Rows.Count + " at " + System.DateTime.Now.ToString(), logPath);
                        int totrowcnt = 0;
                        #region check validation
                        for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                        {
                            int isvalid = 0;
                            if (dsPO.Tables[0].Rows[i]["EquipmentType"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["DepartureDate"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["DepartureStation"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["ArrivalStation"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["CarrierCode"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["FlightNumber"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["LidOrCap"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["Sold"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["NonStopSold"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["ThruSold"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["CnxSold"].ToString().Trim() == "")
                            {
                                clsLog.WriteLog("---------------Error Start---------------", logPath);
                                clsLog.WriteLog("Data is Empty in one of the column ,row no : " + totrowcnt + " at " + System.DateTime.Now.ToString(), logPath);
                                clsLog.WriteLog("---------------End---------------", logPath);
                                dsPO.Tables[0].Rows[i].Delete();
                                dsPO.Tables[0].AcceptChanges();
                                i = i - 1;
                                isvalid = 1;
                            }
                            //else if (dsPO.Tables[0].Rows[i]["EquipmentType"].ToString().Contains('%') || dsPO.Tables[0].Rows[i]["FLT-NO."].ToString().Contains('-') || dsPO.Tables[0].Rows[i]["ACFT"].ToString().Contains('-'))
                            //{
                                //dsPO.Tables[0].Rows[i]["LOAD FACTOR"] = dsPO.Tables[0].Rows[i]["LOAD FACTOR"].ToString().Replace("%", "").Trim();
                                //dsPO.Tables[0].Rows[i]["FLT-NO."] = dsPO.Tables[0].Rows[i]["FLT-NO."].ToString().Replace("-", "");
                                //dsPO.Tables[0].Rows[i]["ACFT"] = dsPO.Tables[0].Rows[i]["ACFT"].ToString().Replace("-", "");
                            else 
                            {
                                dsPO.Tables[0].Rows[i]["CreatedOn"] = dtCurrentDate;
                                dsPO.Tables[0].Rows[i]["CreatedBy"] = strUserName;
                            }
                            //}
                            #region validatio of datatype
                            if (isvalid == 0)
                            {
                                try
                                {
                                    string str = dsPO.Tables[0].Rows[i]["DepartureDate"].ToString();
                                   // Convert.ToDateTime(DateTime.ParseExact(str, "dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"));
                                   // DateTime.ParseExact(dsPO.Tables[0].Rows[i]["DepartureDate"].ToString(),"MM/dd/yyyy", null ).ToString();
                                    try
                                    {
                                       str= DateTime.Parse(str).ToString("MM/dd/yyyy");
                                    }
                                    catch (Exception ex) 
                                    {
                                        string day = str.Substring(0, 2);
                                        string mon = str.Substring(3, 2);
                                        string yr = str.Substring(6, 4);
                                       string  frmDate = yr + "/" + mon + "/" + day;
                                       str = Convert.ToDateTime(frmDate).ToString("MM/dd/yyyy");
                                        
                                    }

                                    dsPO.Tables[0].Rows[i]["DepartureDate"] = str;


                                    Convert.ToInt32(dsPO.Tables[0].Rows[i]["LidOrCap"].ToString());
                                    Convert.ToInt32(dsPO.Tables[0].Rows[i]["Sold"].ToString());
                                    Convert.ToInt32(dsPO.Tables[0].Rows[i]["NonStopSold"].ToString());
                                    Convert.ToInt32(dsPO.Tables[0].Rows[i]["ThruSold"].ToString());
                                    Convert.ToInt32(dsPO.Tables[0].Rows[i]["CnxSold"].ToString());
                                    dsPO.Tables[0].AcceptChanges();

                                }
                                catch (Exception ex)
                                {
                                    clsLog.WriteLog("---------------Error Start---------------", logPath);
                                    clsLog.WriteLog("Data is Not Valid in one of the column(DepartureDate,LidOrCap,Sold, NonStopSold,ThruSold,CnxSold) ,row no : " + totrowcnt + " at " + System.DateTime.Now.ToString(), logPath);
                                    clsLog.WriteLog("---------------End---------------", logPath);
                                    dsPO.Tables[0].Rows[i].Delete();
                                    dsPO.Tables[0].AcceptChanges();
                                    i = i - 1;
                                }
                            }
                            #endregion
                            totrowcnt = totrowcnt + 1;
                        }
                        #endregion
                        clsLog.WriteLog("Total Lines after validation : " + dsPO.Tables[0].Rows.Count + " at " + System.DateTime.Now.ToString(), logPath);

                        for(int i=0;i<dsPO.Tables[0].Rows.Count;i++)
                        {
                                #region Prepare Parameters

                                object[] PaxInfoInfo = new object[14];
                                int k = 0;

                                //0
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][0].ToString(), k);
                                k++;


                                //1
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][1].ToString(), k);
                                k++;

                                //2
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][2].ToString(), k);
                                k++;

                                //3
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][3].ToString(), k);
                                k++;


                                //4
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][4].ToString(), k);
                                k++;

                                //5
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][5].ToString(), k);
                                k++;


                                //6
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][6].ToString(), k);
                                k++;

                               
                                //9
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][7].ToString(), k);
                                k++;

                                //8
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][8].ToString(), k);
                                k++;

                                //9
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][9].ToString(), k);
                                k++;

                                //10
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][10].ToString(), k);
                                k++;
                                            
                                 //11
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][11].ToString(), k);
                                k++;

                                //12
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][12].ToString(), k);
                                k++;

                                
                                //13
                                PaxInfoInfo.SetValue(dsPO.Tables[0].Rows[i][13].ToString(), k);
                                k++;

                                #endregion Prepare Parameters
                                bool IsInsert = false;
                                IsInsert = obj.UploadPaxLoad(PaxInfoInfo);
                                //Call SP to Save database.
                                //if (IsSchduleInsert < 0)
                                //{

                                //    lblStatus.Text = "Error Save Airline schedule. Please try again...";
                                //    return;
                                //}
                             #endregion

                            
                       }   
                            //SqlBulkCopy sqlBulk = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
                            //sqlBulk.DestinationTableName = "FlightPaxLoad";
                            //sqlBulk.ColumnMappings.Add("EquipmentType", "EquipmentType");
                            //sqlBulk.ColumnMappings.Add("DepartureDate", "DepartureDate");
                            //sqlBulk.ColumnMappings.Add("DepartureStation", "Origin");
                            //sqlBulk.ColumnMappings.Add("ArrivalStation", "Destination");
                            //sqlBulk.ColumnMappings.Add("CarrierCode", "CarrierCode");
                            //sqlBulk.ColumnMappings.Add("FlightNumber", "FlightID");
                            //sqlBulk.ColumnMappings.Add("OpSuffix", "OpSuffix");
                            //sqlBulk.ColumnMappings.Add("LidOrCap", "LidOrCap");
                            //sqlBulk.ColumnMappings.Add("Sold", "Sold");
                            //sqlBulk.ColumnMappings.Add("NonStopSold", "NonStopSold");
                            //sqlBulk.ColumnMappings.Add("ThruSold", "ThruSold");
                            //sqlBulk.ColumnMappings.Add("CnxSold", "CnxSold");
                            //sqlBulk.ColumnMappings.Add("CreatedOn", "CreatedOn");
                            //sqlBulk.ColumnMappings.Add("CreatedBy", "CreatedBy");
                            //sqlBulk.WriteToServer(dsPO.Tables[0]);
                        }
                    
                    else
                    {
                        clsLog.WriteLog("---------------Error Start---------------", logPath);
                        clsLog.WriteLog("Excel File is Empty at " + System.DateTime.Now.ToString(), logPath);
                        clsLog.WriteLog("---------------End---------------", logPath);
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    clsLog.WriteLog("Error:"+ex, logPath);
                    return false;
                }
                finally
                {
                  //  oledbConn.Close();



                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["logpath"] == null || Session["logfilename"] == null)
                {
                    lblError.Text = "Not able to download Log File";
                    return;
                }
                FileInfo imageFile = new FileInfo(Session["logpath"].ToString());
                bool fileExists = imageFile.Exists;
                if (fileExists == true)
                {
                    try
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";// "application/ms-word";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + Session["logfilename"].ToString());
                        Response.WriteFile(Session["logpath"].ToString());
                        Response.Flush();
                        Response.Close();
                        imageFile.Delete();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error in downloading Log file";
                    }
                }
                else
                {
                    lblError.Text = "Log File not available";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            string Flightfromdate = "", FlightToDate = "";
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            try
            {    lblError.Text="";
                lblError.ForeColor = Color.Green;
            try
            {
                if (txtFlightFromdate.Text.Trim() != "" || txtToDate.Text.Trim() != "")
                {
                     dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                     dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblError.ForeColor = Color.Red;
                        lblError.Text = "Please enter valid To date";
                        txtFlightFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblError.ForeColor = Color.Red;
                lblError.Text = "Please enter valid date format ex:dd/MM/yyyy";
                lblError.Focus();
                return;
            }


                if (txtFlightFromdate.Text != "")
                {
                       Flightfromdate = txtFlightFromdate.Text;
                     dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);
                    Flightfromdate = dt1.ToString("MM/dd/yyyy");
                }
                if (txtToDate.Text != "")
                {
                       dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
                    FlightToDate = dt2.ToString("MM/dd/yyyy");


                }
            }
            catch (Exception ex)
            {

            }


               DataSet ds = obj.paxLoadList(dt1, dt2);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdPaxLoad.DataSource = ds;
                                grdPaxLoad.DataBind();
                            }

                        }
                    }
                }
        }
    }
}
