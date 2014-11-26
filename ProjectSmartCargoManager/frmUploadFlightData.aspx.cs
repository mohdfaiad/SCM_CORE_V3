using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using BAL;
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class frmUploadFlightData : System.Web.UI.Page
    {
        string logPath = string.Empty;
        string logfilename = string.Empty;
        //UploadFlightDataBAL Ubl = new UploadFlightDataBAL();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region Load DataSet
        private bool LoadExcelData(string ext, string filepath)
        {
            try
            {
                FileInfo fics = new FileInfo(Server.MapPath("~/FlightData/" + FileExcelUpload.FileName + "_Log" + ".txt"));
                bool fileExists = fics.Exists;
                if (fileExists == true)
                {
                    fics.Delete();
                }


                logfilename = FileExcelUpload.FileName + "_Log" + ".txt";
                logPath = Server.MapPath("~/FlightData/" + logfilename);
                Session["logpath"] = logPath;
                Session["logfilename"] = logfilename;
                string connString = string.Empty;
                if (ext == ".xls")
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                }
                else if (ext == ".xlsx")
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
                }
                OleDbConnection oledbConn = new OleDbConnection(connString);
                clsLog.WriteLog("FilePath : " + filepath + " Read On : " + System.DateTime.Now.ToString(), logPath);
                try
                {
                    oledbConn.Open();
                    char[] charsToTrim = { '$', '[', ']', '\'' };
                    string ExcelName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString().Trim(charsToTrim);
                    string query = "select * from [" + ExcelName + "$]";
                    clsLog.WriteLog("Sheet Name : " + ExcelName + " at " + System.DateTime.Now.ToString(), logPath);
                    OleDbCommand cmd = new OleDbCommand(query, oledbConn);
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet dsPO = new DataSet();
                    oleda.Fill(dsPO, "Order");
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
                        if (dsPO.Tables[0].Columns[0].ColumnName == "FLTDATE")
                        {
                            chkheader = 0;
                        }
                        else if (dsPO.Tables[0].Rows[0][0].ToString() == "FLTDATE")
                        {
                            chkheader = 1;
                        }
                        #endregion
                        #region conforming from where header start, is it from first row or header itself
                        for (int j = 0; j < dsPO.Tables[0].Rows.Count; j++)
                        {
                            if (dsPO.Tables[0].Rows[j][0].ToString().Trim() != "FLTDATE" && dsPO.Tables[0].Columns[0].ColumnName != "FLTDATE")
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
                        if (!dsPO.Tables[0].Columns.Contains("FLTDATE") || !dsPO.Tables[0].Columns.Contains("ACFT") || !dsPO.Tables[0].Columns.Contains("FLT-NO.") || !dsPO.Tables[0].Columns.Contains("STD") || !dsPO.Tables[0].Columns.Contains("FROM") || !dsPO.Tables[0].Columns.Contains("TO") || !dsPO.Tables[0].Columns.Contains("STA") || !dsPO.Tables[0].Columns.Contains("PAX") || !dsPO.Tables[0].Columns.Contains("P1") || !dsPO.Tables[0].Columns.Contains("P2") || !dsPO.Tables[0].Columns.Contains("LOAD FACTOR"))
                        {
                            clsLog.WriteLog("---------------Error Start---------------", logPath);
                            clsLog.WriteLog("Not find the Required Columns : {FLTDATE,ACFT,FLT-NO.,STD,FROM,TO,STA,PAX,P1,P2,LOAD FACTOR} at " + System.DateTime.Now.ToString(), logPath);
                            clsLog.WriteLog("---------------End---------------", logPath);
                            return false;
                        }
                        #endregion
                        dsPO.Tables[0].Columns.Add("CreatedOn");
                        clsLog.WriteLog("Total Lines before validation : " + dsPO.Tables[0].Rows.Count + " at " + System.DateTime.Now.ToString(), logPath);
                        int totrowcnt = 0;
                        #region check validation
                        for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                        {
                            int isvalid = 0;
                            if (dsPO.Tables[0].Rows[i]["FLTDATE"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["ACFT"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["FLT-NO."].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["STD"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["FROM"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["TO"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["STA"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["PAX"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["P1"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["P2"].ToString().Trim() == "" || dsPO.Tables[0].Rows[i]["LOAD FACTOR"].ToString().Trim() == "")
                            {
                                clsLog.WriteLog("---------------Error Start---------------", logPath);
                                clsLog.WriteLog("Data is Empty in one of the column ,row no : " + totrowcnt + " at " + System.DateTime.Now.ToString(), logPath);
                                clsLog.WriteLog("---------------End---------------", logPath);
                                dsPO.Tables[0].Rows[i].Delete();
                                dsPO.Tables[0].AcceptChanges();
                                i = i - 1;
                                isvalid = 1;
                            }
                            else if (dsPO.Tables[0].Rows[i]["LOAD FACTOR"].ToString().Contains('%') || dsPO.Tables[0].Rows[i]["FLT-NO."].ToString().Contains('-') || dsPO.Tables[0].Rows[i]["ACFT"].ToString().Contains('-'))
                            {
                                dsPO.Tables[0].Rows[i]["LOAD FACTOR"] = dsPO.Tables[0].Rows[i]["LOAD FACTOR"].ToString().Replace("%", "").Trim();
                                dsPO.Tables[0].Rows[i]["FLT-NO."] = dsPO.Tables[0].Rows[i]["FLT-NO."].ToString().Replace("-", "");
                                dsPO.Tables[0].Rows[i]["ACFT"] = dsPO.Tables[0].Rows[i]["ACFT"].ToString().Replace("-", "");
                                dsPO.Tables[0].Rows[i]["CreatedOn"] = Session["IT"].ToString();
                            }
                            #region validatio of datatype
                            if (isvalid == 0)
                            {
                                try
                                {
                                    Convert.ToDateTime(dsPO.Tables[0].Rows[i]["FLTDATE"].ToString());
                                    Convert.ToInt32(dsPO.Tables[0].Rows[i]["PAX"].ToString());
                                    Convert.ToInt32(dsPO.Tables[0].Rows[i]["LOAD FACTOR"].ToString());
                                }
                                catch (Exception ex)
                                {
                                    clsLog.WriteLog("---------------Error Start---------------", logPath);
                                    clsLog.WriteLog("Data is Not Valid in one of the column(FLTDATE,PAX,LOAD FACTOR) ,row no : " + totrowcnt + " at " + System.DateTime.Now.ToString(), logPath);
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
                        SqlBulkCopy sqlBulk = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
                        sqlBulk.DestinationTableName = "FlightLoadFactor";
                        sqlBulk.ColumnMappings.Add("FLTDATE", "FlightDate");
                        sqlBulk.ColumnMappings.Add("ACFT", "FltRegNo");
                        sqlBulk.ColumnMappings.Add("FLT-NO.", "FlightNumber");
                        sqlBulk.ColumnMappings.Add("STD", "STDeparture");
                        sqlBulk.ColumnMappings.Add("FROM", "Origin");
                        sqlBulk.ColumnMappings.Add("TO", "Destination");
                        sqlBulk.ColumnMappings.Add("STA", "STArrival");
                        sqlBulk.ColumnMappings.Add("PAX", "PaxCount");
                        sqlBulk.ColumnMappings.Add("P1", "Pilot1");
                        sqlBulk.ColumnMappings.Add("P2", "Pilot2");
                        sqlBulk.ColumnMappings.Add("LOAD FACTOR", "LoadFactor");
                        sqlBulk.ColumnMappings.Add("CreatedOn", "CreatedOn");
                        sqlBulk.WriteToServer(dsPO.Tables[0]);
                    }
                    else
                    {
                        clsLog.WriteLog("---------------Error Start---------------", logPath);
                        clsLog.WriteLog("Excel File is Empty at " + System.DateTime.Now.ToString(), logPath);
                        clsLog.WriteLog("---------------End---------------", logPath);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    oledbConn.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

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
                    String[] allowedExtensions = { ".xls",".xlsx" };
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
                    if (!Directory.Exists(Server.MapPath("~/FlightData")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/FlightData"));
                    }
                    FileInfo fi = new FileInfo(Server.MapPath("~/FlightData/" + FileExcelUpload.FileName));
                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblError.Text = "File already Uploaded";
                        return;
                    } 
                    #endregion
                    FileExcelUpload.SaveAs(Server.MapPath("~/FlightData/" + FileExcelUpload.FileName));
                    filePath = Server.MapPath("~/FlightData/" + FileExcelUpload.FileName);
                    if (!LoadExcelData(FileExtension, filePath))
                    {
                        lblError.Text = "Error in data upload";
                        fi.Delete();
                    }
                    else
                    {
                        lblError.Text = "File Processed, Check log file for details.";
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
    }
}
