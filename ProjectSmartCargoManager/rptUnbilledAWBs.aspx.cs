using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;

using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class rptUnbilledAWBs : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        ReportBAL objReport = new ReportBAL();
        public static string CurrTime = "";
        string strfromdate, strtodate;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dtNow = Convert.ToDateTime(Session["IT"]);
                txtFromdate.Text = txtTodate.Text = dtNow.ToString("dd/MM/yyyy");
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                //FileInfo info;
                //ReportRuntime runtime;
                //ReportDefinition definition;

                //Validate controls
                if (Validate() == false)
                    return;
                lblStatus.Text = string.Empty;
                RptUnbilledViewer.Visible = false;

                DataSet res = new DataSet("rptUnBill_ds1");
                res = objReport.GetUnbilledAWBList(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {

                        Dataset1 = res;

                        DataTable dtRes = new DataTable("rptUnBill_dt1");
                        dtRes = res.Tables[0];
                        Session["dsUnbilledAWB"] = res;

                        #region Old RDLX
                        //RptUnbilledViewer.Visible = true;

                        //info = new FileInfo(Server.MapPath("/Reports/rptUnbilledAWBs.rdlx"));
                        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStationWiseTonnageReport.rdlx");

                        //info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                        //RptUnbilledViewer.SetReport(runtime);
                        #endregion

                        #region RDLC

                        //Logo
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

                        RptUnbilledViewer.Visible = true;
                        RptUnbilledViewer.ProcessingMode = ProcessingMode.Local;
                        RptUnbilledViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/UnbilledAWB.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsUnbilledAWB_dtUnbilledAWB", res.Tables[0]);
                        RptUnbilledViewer.LocalReport.DataSources.Clear();
                        RptUnbilledViewer.LocalReport.DataSources.Add(datasource);
                        //RptUnbilledViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                        SaveUserActivityLog("");

                        #endregion
                        if (dtRes != null) 
                            dtRes.Dispose();
                    }
                    else
                    {
                        RptUnbilledViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFromdate.Focus();
                        SaveUserActivityLog(lblStatus.Text);
                        Session["dsUnbilledAWB"] = null;
                        return;
                    }
                }
                else
                {
                    RptUnbilledViewer.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not available for given search criteria";
                    txtFromdate.Focus();
                    SaveUserActivityLog(lblStatus.Text);
                    Session["dsUnbilledAWB"] = null;
                    return;

                }
                if (res != null)
                    res.Dispose();
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                DataSet ds = new DataSet("rptUnBill_ds2");
                DataTable dt = new DataTable("rptUnBill_dt2");

                if ((DataSet)Session["dsUnbilledAWB"] == null)
                { GetData(); }

                ds = (DataSet)Session["dsUnbilledAWB"];
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    dt = ds.Tables[0];
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
                ExportToExcel(dt, "Unbilled AWB Report.xls");

                if (ds != null)
                    ds.Dispose();
                if (dt != null)
                    dt.Dispose();
            }
            catch
            {
            }
        }

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
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                    this.EnableViewState = false;
                    Response.Write(tw.ToString());
                    Response.End();
                }
            }
            catch
            { }
        }
        #endregion

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

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        #endregion

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            try
            {
                // taking all parameters as user selected in report in one variable - "Param"
                string Param = "From:" + txtFromdate.Text.Trim() + ",To:" + txtTodate.Text.Trim();
                objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Unbilled AWB Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
            }
            finally
            {
                if (objBAL != null)
                    objBAL = null;
            }
        }

        protected void GetData()
        {
            try
            {
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;

                //Validate controls
                if (Validate() == false)
                    return;
                lblStatus.Text = string.Empty;
                DataSet res = new DataSet("rptUnBill_ds3");
                res = objReport.GetUnbilledAWBList(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        Dataset1 = res;
                        DataTable dtRes = new DataTable("rptUnBill_dt3");
                        dtRes = res.Tables[0];
                        Session["dsUnbilledAWB"] = res;
                        if (dtRes != null)
                            dtRes.Dispose();
                    }
                    else
                    {
                        RptUnbilledViewer.Visible = false;
                        SaveUserActivityLog(lblStatus.Text);
                        Session["dsUnbilledAWB"] = null;
                        return;
                    }
                }
                else
                {
                    RptUnbilledViewer.Visible = false;
                    SaveUserActivityLog(lblStatus.Text);
                    Session["dsUnbilledAWB"] = null;
                    return;
                }
                if (res != null)
                    res.Dispose();
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            DateTime dtNow = Convert.ToDateTime(Session["IT"]);
            txtFromdate.Text = txtTodate.Text = dtNow.ToString("dd/MM/yyyy");

            lblStatus.Text = string.Empty;

            RptUnbilledViewer.Visible = false;
        }
    }
}
