using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using System.Data;
using BAL;


namespace ProjectSmartCargoManager
{
    public partial class InvoiceMatching : System.Web.UI.Page
    {
        InvoiceMatchingBAL objBAL = new InvoiceMatchingBAL();
        DataSet ds, dsPO;
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlSheets.Visible = false;
            if (!IsPostBack)
            {
                ClearLabels();

                Session["DatasetExcel"]=Session["DatasetDB"]="";
            }
        }
        #region Load DataSet
        private bool LoadExcelData(string ext, string filename, string filepath)
        {

            string connString = string.Empty;
            if (ext == ".xls")
            {
                string path = Server.MapPath("~/Excel/") + filename.ToString();
                //   connString = ConfigurationManager.ConnectionStrings["xls"].ConnectionString;
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=Excel 8.0;";

                //connString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                //          @"Data Source=" + filename + ";" +
                //          @"Extended Properties=" + Convert.ToChar(34).ToString() +
                //          @"Excel 8.0;HDR=YES" + Convert.ToChar(34).ToString();


            }
            else if (ext == ".xlsx")
            {
                string path = Server.MapPath("~/Excel/") + filename.ToString();
                // connString = ConfigurationManager.ConnectionStrings["xlsx"].ConnectionString;
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0";

            }

            // Create the connection object
            OleDbConnection oledbConn = new OleDbConnection(connString);
          
          
          #region Dynamic Sheet

            //ddlSheets.Items.Clear();
            //oledbConn.Open();
            //ddlSheets.Items.Add(new ListItem("--Select Sheet--", ""));

            //ddlSheets.DataSource = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //ddlSheets.DataTextField = "TABLE_NAME";

            //ddlSheets.DataValueField = "TABLE_NAME";
            //char[] charsToTrim = { '$', '[', ']', '\'' };
            //ddlSheets.DataTextField.Trim(charsToTrim);
           
            //ddlSheets.DataBind();
           
            //ddlSheets.Items.RemoveAt(0);
            //for (int j = 0; j < ddlSheets.Items.Count; j++)
            //{

            //    //temp = ddlSheets.Items[j].Text.Replace("$", "");
            //  ddlSheets.Items[j].Text= ddlSheets.Items[j].Text.ToString().Trim(charsToTrim);
            //}

            
            //oledbConn.Close();
            ////string ExcelName = ddlSheets.SelectedValue.ToString();
#endregion
            try
            {

                // Open connection
                oledbConn.Open();
                char[] charsToTrim = { '$', '[', ']', '\'' };
                string ExcelName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString().Trim(charsToTrim);
            string query = "select * from [" + ExcelName + "$]";
            //string query = "select * from [" + ExcelName + "$]";
            
                

                // Create OleDbCommand object and select data from worksheet Sheet1
                OleDbCommand cmd = new OleDbCommand(query, oledbConn);

                // Create new OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Create a DataSet which will hold the data extracted from the worksheet.
               dsPO = new DataSet();

                // Fill the DataSet from the data extracted from the worksheet.
                oleda.Fill(dsPO, "Invoice");

                // Bind the data to the GridView


                GrdXl.DataSource = dsPO.Tables["Invoice"];
                GrdXl.DataBind();
                Session["DatasetExcel"] = dsPO;
            return true;
            }
            catch
            {
                return false;

            }
            finally
            {
                // Close connection
                oledbConn.Close();
            }

        }
        #endregion

       
       
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ClearLabels();
            #region Checking file extension

            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (FileExcelUpload.HasFile)
                {
                    Session["WorkingFile"] = FileExcelUpload.FileName;
                    FileExcelUpload.SaveAs(Server.MapPath("~/Excel/") + FileExcelUpload.FileName);
                   // FileExcelUpload.SaveAs(Server.MapPath("~/Excel/") + FileExcelUpload.FileName);
                   
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    filePath = Path.GetFullPath(Session["WorkingFile"].ToString()).ToLower();
                    filePath = FileExcelUpload.PostedFile.FileName;

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

                    if (!LoadExcelData(FileExtension, FileExcelUpload.FileName, filePath))
                    {
                        lblError.Text = "Error in data upload";
                    }
                    else
                    {
                        lblError.Text = "File Uploaded Successfully...";
                    }


                }
                else
                {
                    lblError.Text = "Cannot accept files of this type.";
                    return;
                }

            }
            catch (Exception ex)
            {

            }
            #endregion

        }

        protected void btnCompare_Click(object sender, EventArgs e)
        {
            ClearLabels();
            #region Fetching invoice from database
           ds = new DataSet();
            try
            {

                object[] InvInfo = new object[1];
                int i = 0;

                //search parameers

                InvInfo.SetValue(txtInvoiceNumber.Text.ToString(), i);


                ds = objBAL.InvoiceDetails(InvInfo);

                dGrd.DataSource = ds;
                dGrd.DataBind();
                Session["DatasetDB"] = ds;

            }
            catch (Exception)
            {

                //  throw;
            }
#endregion

#region Compare Invoice
            int DsLength = 0, DsPoLength = 0,valid=0;
            DataSet dbXl, dbTable;
            dbXl = new DataSet();
            dbTable = new DataSet();

            if (Session["DatasetExcel"] != "" )
            {
                dbXl = (DataSet)Session["DatasetExcel"];
                dbTable = (DataSet)Session["DatasetDB"];
            }
            else
            {
                dbXl = null;
                dbTable = null;
            
            }
                if (dbTable != null && dbTable.Tables != null && dbTable.Tables[0].Rows.Count>0)
            {
                DsPoLength = dbXl.Tables["Invoice"].Columns.Count;
                DsLength = dbTable.Tables[0].Columns.Count;
            }
                

                   
               

                if (DsLength == 0 || DsPoLength == 0)
                {
                    
                    valid = 1;
                }
                if (DsLength != DsPoLength)
                {
                   
                    valid = 1;
                }
                if (DsLength == DsPoLength && (DsLength != 0 && DsPoLength != 0))
                {
                    for (int j = 0; j < DsLength-1; j++)
                    {
                        if (dbXl.Tables["Invoice"].Rows[0][j].ToString() != dbTable.Tables[0].Rows[0][j].ToString())
                            valid = 1;
                    }

                }
                if (valid == 0)
                {
                    lblStatus.Text = "Invoice Matches ";
                }
                if (valid == 1)
                {
                    lblStatus.Text = "Invoice doesnt Matches ";
                }

#endregion

        }





        protected void ClearLabels()
        {
            lblError.Text = lblStatus.Text = ""; 
        }

    }
  

}
