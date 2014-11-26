using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;

using System.IO;
using System.Configuration;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptEndOfShift : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";
        string strfromdate, strtodate;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //RptViewerRevenue_Station.Visible = false;
                    btnExport.Visible = true;
                    lblStatus.Text = string.Empty;
                    ReportViewer1.Visible = false;
                    txtFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFromTimeHr.Text = Convert.ToDateTime(Session["IT"]).ToString("HH");
                    txtFromTimeMin.Text = Convert.ToDateTime(Session["IT"]).ToString("mm");

                    txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToTimeHr.Text = "23"; //Convert.ToDateTime(Session["IT"]).ToString("HH");
                    txtToTimeMin.Text = "59"; //Convert.ToDateTime(Session["IT"]).ToString("mm");

                    txtEmployee.Text = Session["UserName"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet res = new DataSet();
            Dataset1 = new DataSet("Table");
            try
            {
                //RptViewerRevenue_Station.Visible = false;
                lblStatus.Text = string.Empty;

                if (Validate() == false)
                {
                    ReportViewer1.Visible = false;
                    return;
                }
                lblStatus.Text = "";
                
                res = objReport.GetEndOfShiftData(Convert.ToDateTime(strfromdate + " " + txtFromTimeHr.Text + ":" + txtFromTimeMin.Text).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate + " " + txtToTimeHr.Text + ":" + txtToTimeMin.Text).AddDays(0).ToString("yyyy-MM-dd HH:mm:ss"), txtEmployee.Text.Trim());

                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        Dataset1 = res;
                        Session["dsListShift"] = res;

                        lblStatus.Text = string.Empty;
                        ReportViewer1.Visible = true;

                        btnExport.Visible = true;

                        //Adding Logo
                        System.IO.MemoryStream Logo = null;
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                            //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }

                        if (res.Tables[0].Columns.Contains("Logo") == false)
                        {
                            DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            col1.DefaultValue = Logo.ToArray();
                            res.Tables[0].Columns.Add(col1);
                        }


                        //Adding from datetime, to datetime and employee name 
                        string fromDateTime = "", toDateTime = "", employeeid="";
                        try
                        {
                            fromDateTime = txtFromdate.Text.Trim() + " " + txtFromTimeHr.Text + ":" + txtFromTimeMin.Text;
                            toDateTime = txtTodate.Text.Trim() + " " + txtToTimeHr.Text + ":" + txtToTimeMin.Text;
                            employeeid = txtEmployee.Text.Trim();
                        }
                        catch (Exception ex)
                        {
                            fromDateTime = "";
                            toDateTime = "";
                            employeeid = "";
                        }

                        if (res.Tables[0].Columns.Contains("StartDateTime") == false)
                        {
                            DataColumn StartDateTime = new DataColumn("StartDateTime", System.Type.GetType("System.String"));
                            StartDateTime.DefaultValue = fromDateTime;
                            res.Tables[0].Columns.Add(StartDateTime);
                        }

                        if (res.Tables[0].Columns.Contains("EndDateTime") == false)
                        {
                            DataColumn EndDateTime = new DataColumn("EndDateTime", System.Type.GetType("System.String"));
                            EndDateTime.DefaultValue = toDateTime;
                            res.Tables[0].Columns.Add(EndDateTime);
                        }

                        if (res.Tables[0].Columns.Contains("Employee") == false)
                        {
                            DataColumn Employee = new DataColumn("Employee", System.Type.GetType("System.String"));
                            Employee.DefaultValue = employeeid;
                            res.Tables[0].Columns.Add(Employee);
                        }


                        //RptViewerRevenue_Station.Visible = false;
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptEndOfShift.rdlc");
                        ReportDataSource datasource = new ReportDataSource("rptEndOfShift_dtEndOfShift", res.Tables[0]);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        SaveUserActivityLog("");

                    }
                    else
                    {
                        ReportViewer1.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFromdate.Focus();
                        return;

                    }
                }
                else
                {
                    ReportViewer1.Visible = false;
                    //RptViewerRevenue_Station.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not available for given search criteria";
                    txtFromdate.Focus();
                    return;

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                res = null;
                Dataset1 = null;
            }
        }

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "DataSet1")
        //    {
        //        e.Data = Dataset1;

        //    }
        //    else
        //    {
        //        //e.Data = Dataset2;
        //    }
        //}
        #endregion


        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "From Date:" + txtFromdate.Text.ToString() + "To Date:" + txtTodate.Text.ToString();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "End of Shift", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                string strOutput = objBL.GetMasterConfiguration("ReportInterval");

                if (strOutput != "")
                    DaysConfigured = Convert.ToDouble(strOutput);
                else
                    DaysConfigured = 0;
            }
            catch
            {
                DaysConfigured = 0;
            }
            finally
            {
                objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            else
                return "";
        }

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                DateTime dt;


                try
                {
                    //dt = Convert.ToDateTime(txtbillingfrom.Text);
                    //Change 03082012
                    string day = txtFromdate.Text.Substring(0, 2);
                    string mon = txtFromdate.Text.Substring(3, 2);
                    string yr = txtFromdate.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);

                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                //Validation for To date
                if (txtTodate.Text == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid date');</SCRIPT>");
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return false;
                }

                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtbillingto.Text);
                    //Change 03082012
                    string day = txtTodate.Text.Substring(0, 2);
                    string mon = txtTodate.Text.Substring(3, 2);
                    string yr = txtTodate.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (dtto < dt)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('To date should be greater than From date');</SCRIPT>");
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;
                    // MessageBox.Show("Please Enter FlightID's which is not Operated");

                    return false;
                }



            }
            catch (Exception ex)
            {


            }
            ReportBAL objBal = new ReportBAL();
            string strResult = string.Empty;

            try
            {
                strResult = objBal.GetReportInterval(DateTime.ParseExact(txtFromdate.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtTodate.Text.Trim(), "dd/MM/yyyy", null));
            }
            catch
            {
                strResult = "";
            }
            finally
            {
                objBal = null;
            }

            if (strResult != "")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = strResult;
                txtFromdate.Focus();
                return false;
            }

            return true;
        }
        #endregion

        
        #region Export to DataTable

        public void ExportToExcel(DataTable dt, string FileName)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    string filename = FileName;
                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    DataGrid dgGrid = new DataGrid();
                    dgGrid.DataSource = dt;
                    dgGrid.DataBind();

                    //Get the HTML for the control.
                    dgGrid.RenderControl(hw);
                    //Write the HTML back to the browser.
                    //Response.ContentType = application/vnd.ms-excel;
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                    this.EnableViewState = false;
                    Response.Write(tw.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFromdate.Text = txtTodate.Text = string.Empty;
            ReportViewer1.Visible = false;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //lblStatus.Text = "";
            //DataSet ds = null;
            //DataTable dt = null;

            //try
            //{
            //    ds = (DataSet)Session["dsListShift"];
            //    dt = (DataTable)ds.Tables[0];
            //    //dt = city.GetAllCity();//your datatable 
            //    string attachment = "attachment; filename=End of shift.pdf";
            //    Response.ClearContent();
            //    Response.AddHeader("content-disposition", attachment);
            //    Response.ContentType = "application/pdf";
            //    string tab = "";
            //    foreach (DataColumn dc in dt.Columns)
            //    {
            //        Response.Write(tab + dc.ColumnName);
            //        tab = "\t";
            //    }
            //    Response.Write("\n");
            //    int i;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        tab = "";
            //        for (i = 0; i < dt.Columns.Count; i++)
            //        {
            //            Response.Write(tab + dr[i].ToString());
            //            tab = "\t";
            //        }
            //        Response.Write("\n");
            //    }
            //    Response.End();
            //}
            //catch (Exception ex)
            //{ }
            //finally
            //{
            //    ds = null;
            //    dt = null;
            //}


            try
            {
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                LocalReport rep1 = ReportViewer1.LocalReport;

                string deviceInfo = "<DeviceInfo><PageHeight>35cm</PageHeight><PageWidth>40cm</PageWidth></DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                //Render the report

                renderedBytes = rep1.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                Response.Clear();

                Response.ContentType = mimeType;

                Response.AddHeader("content-disposition", "attachment; filename=EndOfShift.pdf");

                Response.BinaryWrite(renderedBytes);


                //Response.End();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
