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
    public partial class rptMonthlyStats : System.Web.UI.Page
    {
        #region Variable
        private DataSet Dataset1;
        private DataSet Dataset2;
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
                    RptMonthStatsViewer.Visible = false;
                    txtFromdate.Text = txtTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    lblStatus.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet res = null;
            DataTable dtRes = null;
            try
            {
                Session["dsListDCM_monthlystats"] = null;
                RptMonthStatsViewer.Visible = false;
                lblStatus.Text = string.Empty;

              //  FileInfo info;
                //ReportRuntime runtime;
               // ReportDefinition definition;

                //Validate controls
                if (Validate() == false)
                {
                    Session["dsListDCM_monthlystats"] = null;
                    RptMonthStatsViewer.Visible = false;
                    return;
                }
                lblStatus.Text = "";


                res = objReport.GetMonthlyStats(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), Session["Station"].ToString());

                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        RptMonthStatsViewer.Visible = true;
                        lblStatus.Text = string.Empty;
                        Dataset1 = res;
                        dtRes = res.Tables[0];
                        Session["dsListDCM_monthlystats"] = dtRes;

                        #region Old RDLX
                        /////////////////Code to edit result dataset (hiding duplicate values for AWB)////////////////////
                        ////DataTable dtRes = res.Tables[0];
                        ////Session["dsListDCM_monthlystats"] = dtRes;
                        ////btnExport.Visible = true;


                        ////Dataset1 = new DataSet("Table");
                        ////Dataset1.Tables.Add(dtRes);
                        //////////////////////////////////////////////////////////////////////////////////////////////////

                        //RptViewerRevenue_Station.Visible = true;

                        //info = new FileInfo(Server.MapPath("/Reports/rptMonthlyStats.rdlx"));
                        ////info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "rptStationWiseTonnageReport.rdlx");

                        //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Report1.rdlx");

                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                        //RptViewerRevenue_Station.SetReport(runtime);
                        ////  oDs2 = LoadParamDataset();// just temperory used, not provide real value  Dataset2 = oDs2;                  runtime.LocateDataSource += WARCustWise_LocateDataSource;                 WARCustWise.SetReport(runtime);                 pnlMsg.Visible = false;              }             else             {                 oDs1 = null;                 WARCustWise.Visible = false;                 // lblMsg.Text = "No Data Available";                 pnlMsg.Visible = true;                 lblMsgBox.Text = "No Data Available";                 //Pic.Visible = true;               }
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

                        RptMonthStatsViewer.Visible = true;
                        RptMonthStatsViewer.ProcessingMode = ProcessingMode.Local;
                        RptMonthStatsViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/MonthStatsRpt.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsMonthStats_dtMonthStats", res.Tables[0]);
                        RptMonthStatsViewer.LocalReport.DataSources.Clear();
                        RptMonthStatsViewer.LocalReport.DataSources.Add(datasource);
                        //RptMonthStatsViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                        SaveUserActivityLog("");

                        #endregion
                    }
                    else
                    {
                        RptMonthStatsViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        Session["dsListDCM_monthlystats"] = null;
                        SaveUserActivityLog(lblStatus.Text);
                        txtFromdate.Focus();
                        return;
                    }
                }
                else
                {
                    RptMonthStatsViewer.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not available for given search criteria";
                    Session["dsListDCM_monthlystats"] = null;
                    SaveUserActivityLog(lblStatus.Text);
                    txtFromdate.Focus();
                    return;

                }
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
            finally
            {
                if (dtRes != null)
                    dtRes.Dispose();
                if (res != null)
                    res.Dispose();

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
            return true;
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            lblStatus.Text = string.Empty;
            Session["dsListDCM_monthlystats"] = null;
            RptMonthStatsViewer.Visible=false;
            try
            {
                lblStatus.Text = "";
                //DataSet ds = null;


            if ((Session["dsListDCM_monthlystats"] == null))
                    GetData();

                dt = (DataTable)Session["dsListDCM_monthlystats"];

                if (dt != null && dt.Rows.Count > 0)
                    ExportToExcel(dt, "MonthlyStatsReport.xls");
                else
                {
                    lblStatus.Text = "Data not found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    RptMonthStatsViewer.Visible = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
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
            catch (Exception ex)
            { }
        }
        #endregion

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "FrmDt:" + txtFromdate.Text.ToString() + ",ToDt:" + txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Monthly Statistics", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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

        protected void GetData()
        {
            DataSet res = null;
            DataTable dtRes = null;
            try
            {
               
                lblStatus.Text = string.Empty;

                //Validate controls
                if (Validate() == false)
                {
                    Session["dsListDCM_monthlystats"] = null;
                    RptMonthStatsViewer.Visible = false;
                    return;
                }
                    
                lblStatus.Text = "";

                res = objReport.GetMonthlyStats(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), Session["Station"].ToString());

                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        RptMonthStatsViewer.Visible = true;
                        lblStatus.Text = string.Empty;
                        Dataset1 = res;
                        dtRes = res.Tables[0];
                        Session["dsListDCM_monthlystats"] = dtRes;
                    }
                    else
                    {
                        RptMonthStatsViewer.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        Session["dsListDCM_monthlystats"] = null;
                        txtFromdate.Focus();
                        return;
                    }
                }
                else
                {
                    RptMonthStatsViewer.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not available for given search criteria";
                    Session["dsListDCM_monthlystats"] = null;
                    txtFromdate.Focus();
                    return;

                }
            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
            finally
            {
                if (dtRes != null)
                    dtRes.Dispose();
                if (res != null)
                    res.Dispose();

            }
        }
    }
}
