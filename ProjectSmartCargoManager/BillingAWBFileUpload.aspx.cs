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
using BAL;
using System.Drawing;


namespace ProjectSmartCargoManager
{
    public partial class BillingAWBFileUpload : System.Web.UI.Page
    {
        BillingFileUploadBAL objBAL = new BillingFileUploadBAL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Load DataSet
        private bool LoadExcelData(string ext, string filename, string filepath)
        {

            string connString = string.Empty;
            
            if (ext == ".xls")
            {
                //   connString = ConfigurationManager.ConnectionStrings["xls"].ConnectionString;
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";

                //connString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                //          @"Data Source=" + filename + ";" +
                //          @"Extended Properties=" + Convert.ToChar(34).ToString() +
                //          @"Excel 8.0;HDR=YES" + Convert.ToChar(34).ToString();


            }
            else if (ext == ".xlsx")
            {
                // connString = ConfigurationManager.ConnectionStrings["xlsx"].ConnectionString;
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=Excel 12.0";

            }

            OleDbConnection oledbConn = new OleDbConnection(connString);
            oledbConn.Open();
            //string ExcelName = "Sheet1";
            char[] charsToTrim = { '$', '[', ']', '\'' };
            string ExcelName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString().Trim(charsToTrim);

            string query = "select * from [" + ExcelName + "$]";


            // Create the connection object
            
            try
            {
                // Open connection
                

                // Create OleDbCommand object and select data from worksheet Sheet1
                OleDbCommand cmd = new OleDbCommand(query, oledbConn);

                // Create new OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Create a DataSet which will hold the data extracted from the worksheet.
                DataSet dsPO = new DataSet();

                // Fill the DataSet from the data extracted from the worksheet.
                oleda.Fill(dsPO);

                // Upload data in database table

                bool res = loadFileDataInDB(dsPO);

                return res;

            }
            catch(Exception ex)
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

        #region load File data in Db
        protected bool loadFileDataInDB(DataSet dsData)
        {
            try
            {
                #region Prepare Parameters
                object[] RateCardInfo = new object[15];
                int i = 0;
                for (int row = 0; row < dsData.Tables[0].Rows.Count -1; row++)
                {
                    i = 0;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][0].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][1].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][2].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][3].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][4].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][5].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][6].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][7].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][8].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][9].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][10].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][11].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][12].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][13].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][14].ToString(), i);

                    //Commented Agent Code
                    //i++;
                    //RateCardInfo.SetValue(dsData.Tables[0].Rows[row][15].ToString(), i);


                #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.AddAWBFileData(RateCardInfo);
                    //RateCardID=objBAL.AddRateCard(RateCardInfo);

                    if (res != "")
                    {
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
        #endregion load File data in Db


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            #region Checking file extension

            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;
                String savePath = @"c:\SCMExcel\";

                if (FileExcelUpload.HasFile)
                {
                    //code to save the file on server
                    String fileName = FileExcelUpload.FileName;

                    // Append the name of the file to upload to the path.
                    savePath += fileName;


                    // Call the SaveAs method to save the 
                    // uploaded file to the specified path.
                    // This example does not perform all
                    // the necessary error checking.               
                    // If a file with the same name
                    // already exists in the specified path,  
                    // the uploaded file overwrites it.
                    FileExcelUpload.SaveAs(savePath);


                    Session["WorkingFile"] = FileExcelUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    //filePath = Path.GetFullPath(Session["WorkingFile"].ToString()).ToLower();
                    //filePath = "C:\\AWBInvoice\\01aprto04apr2012final.xls";
                    filePath = @"C:\SCMExcel\" + FileExcelUpload.FileName;
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
                        lblStatus.Text = "Error in data upload";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblStatus.Text = "File Uploaded Successfully...";
                        lblStatus.ForeColor = Color.Green;
                    }


                }
                else
                {
                    lblStatus.Text = "Cannot accept files of this type.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {

            }
            #endregion


        }

        protected void btnMilestone_Click(object sender, EventArgs e)
        {
            #region Prepare Parameters
            object[] MilestoneInfo = new object[4];
            int i = 0;

            MilestoneInfo.SetValue("SG98989898", i);
            i++;

            MilestoneInfo.SetValue(2, i);
            i++;

            MilestoneInfo.SetValue("SG218", i);
            i++;

            MilestoneInfo.SetValue("Booking", i);

            #endregion Prepare Parameters


            AWBMilestonesBAL mile = new AWBMilestonesBAL();
            mile.AddAWBMilestone(MilestoneInfo);
        }
    }
}
