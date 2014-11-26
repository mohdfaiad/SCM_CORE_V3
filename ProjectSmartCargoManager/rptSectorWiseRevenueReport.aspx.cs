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
using System.Text;

namespace ProjectSmartCargoManager
{
    public partial class rptSectorWiseRevenueReport : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString()); 
        
        DataTable dtTableStation = new DataTable();
        DataTable dtTableDetail = new DataTable();
        DataTable dtTableSector = new DataTable();
        DataTable dtTableTail = new DataTable();
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                LoadCarrierDropdown();
                ReportViewer1.Visible = false;
                txtFromdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txtTodate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
            }
        }
        //# region Get Origin List

        //private void GetAirportCode()
        //{
        //    BALException objBAL = new BALException();
        //    DataSet ds = null;

        //    try
        //    {
        //        ds = objBAL.GetAirportCodeList(ddlStation.SelectedValue);
        //        if (ds != null)
        //        {
        //            if (ds.Tables != null)
        //            {
        //                if (ds.Tables.Count > 0)
        //                {

        //                    ddlStation.DataSource = ds;
        //                    ddlStation.DataMember = ds.Tables[0].TableName;
        //                    ddlStation.DataValueField = "AirportCode";

        //                    ddlStation.DataTextField = "Airport";
        //                    ddlStation.DataBind();
        //                    ddlStation.Items.Insert(0, "All");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        objBAL = null;
        //        if (ds != null)
        //            ds.Dispose();
        //    }
        //}

        //# endregion GetOriginCode List

        # region Get Origin List

        private void GetAirportCode()
        {
            BALException objBAL = new BALException();
            DataSet ds = null;

            try
            {
                ds = objBAL.GetAirportCodeList(ddlStation.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlStation.DataSource = ds;
                            ddlStation.DataMember = ds.Tables[0].TableName;
                            ddlStation.DataValueField = "AirportCode";

                            ddlStation.DataTextField = "Airport";
                            ddlStation.DataBind();
                            ddlStation.Items.Insert(0, "All");
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

        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                Session["RevenueTrackingReport"] = null;
                ReportViewer1.Visible = false;
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFromdate.Text.Trim() != "" || txtTodate.Text.Trim() != "")
                    {
                        dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        dt2 = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromdate.Focus();
                            return;
                        }

                    }

                }
                catch (Exception ex)
                { }

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
                    ReportViewer1.Visible = false;
                    txtFromdate.Focus();
                    return;
                }

                System.IO.MemoryStream Logo = null;

                string fromdate, todate;
                fromdate = dt1.ToString("dd/MM/yyy");
                todate = dt2.ToString("dd/MM/yyyy");
                object[] param = { fromdate, todate, ddlStation.SelectedItem.Value.ToString(), ddlStationType.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Value.Trim(),ddlRevenueType.Text.Trim() };
                string[] pname = { "fromdate", "todate", "Station", "StationType", "Carrier", "ReportType" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spSectorWiseRevenueReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["RevenueTrackingReport"] = ds.Tables[0];
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
                        MasterBAL m = new MasterBAL();
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
                                DataColumn col1 = new DataColumn("PreparedOn", System.Type.GetType("System.String"));
                                col1.DefaultValue = Session["IT"] != null ? ((DateTime)(Session["IT"])).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                                dt.Columns.Add(col1);
                            }
                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("PreparedBy", System.Type.GetType("System.String"));
                                col1.DefaultValue = client;
                                dt.Columns.Add(col1);
                            }

                        }
                        ReportViewer1.Visible = true;
                        ReportViewer1.Reset();
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportDataSource datasource = new ReportDataSource();
                        if (ddlRevenueType.Text == "ST")
                        {
                            datasource = new ReportDataSource();
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSectorWiseRevenueSubStation.rdlc");
                            datasource = new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubStation", dt);
                        }
                        if (ddlRevenueType.Text == "SC")
                        {
                            datasource = new ReportDataSource();
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSectorWiseRevenueSubSector.rdlc");
                            datasource = new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubOND", dt);
                        }
                        if (ddlRevenueType.Text == "FW")
                        {
                            datasource = new ReportDataSource();
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSectorWiseRevenueSubDetail.rdlc");
                            datasource = new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubDetail", dt);
                        }
                        if (ddlRevenueType.Text == "FE")
                        {
                            datasource = new ReportDataSource();
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSectorWiseRevenueSubTail.rdlc");
                            datasource = new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubTail", dt);
                        }
                        

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
        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Station:" + ddlStation.Text.ToString() + ", FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Cargo Revenue Tracking Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptSectorWiseRevenueReport.aspx",false);
        }

        public void GetData()
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                Session["RevenueTrackingReport"] = null;
                ReportViewer1.Visible = false;
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFromdate.Text.Trim() != "" || txtTodate.Text.Trim() != "")
                    {
                        dt1 = DateTime.ParseExact(txtFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        dt2 = DateTime.ParseExact(txtTodate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtFromdate.Focus();
                            return;
                        }

                    }

                }
                catch (Exception ex)
                { }

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
                    ReportViewer1.Visible = false;
                    txtFromdate.Focus();
                    return;
                }

                System.IO.MemoryStream Logo = null;

                string fromdate, todate;
                fromdate = dt1.ToString("dd/MM/yyyy");
                todate = dt2.ToString("dd/MM/yyyy");
                object[] param = { fromdate, todate, ddlStation.SelectedItem.Value.ToString(), ddlStationType.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Value.Trim(), ddlRevenueType.Text.Trim() };
                string[] pname = { "fromdate", "todate", "Station", "StationType", "Carrier", "ReportType" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spSectorWiseRevenueReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["RevenueTrackingReport"] = ds;//.Tables[0];
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
                        MasterBAL m = new MasterBAL();
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
                                DataColumn col1 = new DataColumn("PreparedOn", System.Type.GetType("System.String"));
                                col1.DefaultValue = Session["IT"] != null ? ((DateTime)(Session["IT"])).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                                dt.Columns.Add(col1);
                            }
                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("PreparedBy", System.Type.GetType("System.string"));
                                col1.DefaultValue = client;
                                dt.Columns.Add(col1);
                            }

                        }


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
        protected void btnExport_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            Session["RevenueTrackingReport"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataSet)Session["RevenueTrackingReport"] == null)
                    GetData();

                dt = ((DataSet)Session["RevenueTrackingReport"]).Tables[0];

                if (Session["RevenueTrackingReport"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
                string attachment = "attachment; filename=RevenueTracking.xls";
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

        #region Load Carrier Dropdown
        public void LoadCarrierDropdown()
        {
            DataSet ds = da.SelectRecords("spGetCarrierCodes");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCarrier.DataSource = ds;
                ddlCarrier.DataTextField = "CarrierName";
                ddlCarrier.DataValueField = "CarrierCode";
                ddlCarrier.DataBind();
                ddlCarrier.Items.Insert(0, "All");
            }
        }
        #endregion

        #region SubReport Processing
        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubStation", dtTableStation));
            e.DataSources.Add(new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubOND", dtTableSector));
            e.DataSources.Add(new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubDetail", dtTableDetail)); //Amount,PayMode Removed From AWB DO Subreport
            e.DataSources.Add(new ReportDataSource("rptSectorWiseRevenue_rptSectorWiseRevenueSubTail", dtTableTail)); //Amount,PayMode Removed From AWB DO Subreport
        }
        #endregion
    }
}
