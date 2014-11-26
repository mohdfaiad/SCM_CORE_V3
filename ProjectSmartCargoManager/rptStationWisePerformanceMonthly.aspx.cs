using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;

using System.IO;
using BAL;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using System.Drawing;


namespace ProjectSmartCargoManager
{
    public partial class rptStationWisePerformanceMonthly : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet ds = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                //LoadCarrierDropdown();
                ReportViewer1.Visible = false;
                //btnExportReport.Visible = false;
        
            }
        }

        # region Get Origin List

        private void GetAirportCode()
        {
            BALException objBAL = new BALException();
            DataSet ds = null;

            try
            {
                ds = objBAL.GetAirportCodeList(ddlAirport.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlAirport.DataSource = ds;
                            ddlAirport.DataMember = ds.Tables[0].TableName;
                            ddlAirport.DataValueField = "AirportCode";

                            ddlAirport.DataTextField = "Airport";
                            ddlAirport.DataBind();
                            ddlAirport.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                objBAL = null;
                if (ds != null)
                    ds.Dispose();
            }
        }

        # endregion GetOriginCode List

        protected void ddlStationType_SelectedIndexChanged(object sender, EventArgs e)
        {

            BALException objBAL = new BALException();
            DataSet ds = null;
            if (ddlStationType.SelectedItem.Value.ToString() == "All")
            {
               

                try
                {
                    ds = objBAL.GetAirportCodeList(ddlAirport.SelectedValue);
                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {

                                ddlAirport.DataSource = ds;
                                ddlAirport.DataMember = ds.Tables[0].TableName;
                                ddlAirport.DataValueField = "AirportCode";

                                ddlAirport.DataTextField = "Airport";
                                ddlAirport.DataBind();
                                ddlAirport.Items.Insert(0, "All");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    objBAL = null;
                    if (ds != null)
                        ds.Dispose();
                }
            }

            if (ddlStationType.SelectedItem.Value.ToString() == "DOM")
            {

            }

            if (ddlStationType.SelectedItem.Value.ToString() == "INT")
            {

            }
        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Station:" +ddlAirport.Text.ToString() + ", StationType:" +ddlStationType .Text.ToString() + ", Month:" + ddlMonth.SelectedItem.Text.ToString() + ", Year:" +ddlYear.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Station Performance Per Month", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }


        public void GetData()
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = null;
            try
            {
                Session["ds"] = null;
                ReportViewer1.Visible = false;


                ReportBAL objBal = new ReportBAL();


                System.IO.MemoryStream Logo = null;

                //string fromdate, todate;
                //fromdate = dt1.ToString("MM/dd/yyy");
                //todate = dt2.ToString("MM/dd/yyyy");
                object[] param = { ddlStationType.SelectedItem.Value.ToString(), ddlAirport.SelectedItem.Value.ToString(), ddlMonth.SelectedItem.Value.Trim(),ddlYear.Text.Trim() };
                string[] pname = { "StationType", "Station", "Month", "Year" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("SpStationWisePerformanceReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["rptStationWisePerformanceMonthly"] = ds.Tables[0];
                     
                        SaveUserActivityLog("");
                    }
                    else
                    {
                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        ReportViewer1.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (ds != null)
                    ds.Dispose();
                if (da != null)
                    da = null;
            }
        }
       
        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            
            try
            {
                Session["rptStationWisePerformanceMonthly"] = null;
                ReportViewer1.Visible = false;

                ReportBAL objBal = new ReportBAL();

                System.IO.MemoryStream Logo = null;

                object[] param = { ddlStationType.SelectedItem.Value.ToString(), ddlAirport.SelectedItem.Value.ToString(), ddlMonth.SelectedItem.Value.Trim(), ddlYear.Text.Trim() };
                string[] pname = { "StationType", "Station", "Month", "Year" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("SpStationWisePerformanceReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        //Session["rptStationWisePerformanceMonthly"] = ds.Tables[0];
                        //Logo
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                               }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }
                        try
                        {

                        }
                        catch (Exception ex)
                        { }
                        MasterBAL m=new MasterBAL();
                        string client = m.clientName();
                        dt = null;
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            dt = ds.Tables[0].Copy();
                            if (dt.Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                //col1.DefaultValue = Logo.ToArray();
                                dt.Columns.Add(col1);
                            }

                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("ClientName", System.Type.GetType("System.String"));
                                col1.DefaultValue = client;
                                dt.Columns.Add(col1);
                            }


                        }
                        ReportViewer1.Visible = true;
                       
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptStationPerformanceMonthly.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsStationPerformanceReportMonthly_DataTable1", dt);
                       
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                      
                        SaveUserActivityLog("");
                    }
                    else
                    {
                        lblStatus.Text = "No records found.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        ReportViewer1.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                SaveUserActivityLog(ErrorLog);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (ds != null)
                    ds.Dispose();
                if (da != null)
                    da = null;
            }
        }
    

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptStationWisePerformanceMonthly.aspx", false);
        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
           
            DataTable dt = null;
            Session["rptStationWisePerformanceMonthly"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataTable)Session["rptStationWisePerformanceMonthly"] == null)
                    GetData();
         
                dt = (DataTable)Session["rptStationWisePerformanceMonthly"];

                if (Session["rptStationWisePerformanceMonthly"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
                Session["rptStationWisePerformanceMonthly"] = null;
                string attachment = "attachment; filename=StationPerformanceReportMonthly.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
                Response.Flush();
                Response.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
              
                dt = null;
            }
        }

    }
}
