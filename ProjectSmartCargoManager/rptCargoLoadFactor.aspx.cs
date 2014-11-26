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
    public partial class rptCargoLoadFactor : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet ds = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                LoadCarrierDropdown();
                ReportViewer1.Visible = false;
                btnExportReport.Visible = false;
                txtFromdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                txtTodate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
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
            string Param = "Station:" +ddlAirport.Text.ToString() + ", StationType:" +ddlStationType .Text.ToString() + ", FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" +txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Cargo Load Factor Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
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
                fromdate = dt1.ToString("MM/dd/yyy");
                todate = dt2.ToString("MM/dd/yyyy");
                object[] param = { fromdate, todate, ddlStationType.SelectedItem.Value.ToString(), ddlAirport.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Value.Trim() };
                string[] pname = { "fromdate", "todate", "StationType", "Station", "Carrier" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("SpCargoLoadSector", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["CargoLoadFactor"] = ds.Tables[0];
                     
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
                Session["CargoLoadFactor"] = null;
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
                catch(Exception ex)
                {}

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
                fromdate = dt1.ToString("MM/dd/yyy");
                todate = dt2.ToString("MM/dd/yyyy");
                object[] param = { fromdate, todate, ddlStationType.SelectedItem.Value.ToString(), ddlAirport.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Text.Trim() };
                string[] pname = { "fromdate", "todate", "StationType", "Station","Carrier" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("SpCargoLoadSector", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["CargoLoadFactor"] = ds.Tables[0];
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
                                col1.DefaultValue = Logo.ToArray();
                                dt.Columns.Add(col1);
                            }
                            if (dt.Columns.Contains("") == false)
                            {
                                //DataColumn col1 = new DataColumn("TIME", System.Type.GetType("System.DateTime"));
                                //col1.DefaultValue = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy HH:mm:ss");
                                DataColumn col1 = new DataColumn("TIME", System.Type.GetType("System.String"));
                                col1.DefaultValue = Session["IT"] != null ? ((DateTime)(Session["IT"])).ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;
                                
                                dt.Columns.Add(col1);
                            }
                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("ClientName", System.Type.GetType("System.String"));
                                col1.DefaultValue = client;
                                dt.Columns.Add(col1);
                            }

                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("UserName", System.Type.GetType("System.String"));
                                col1.DefaultValue = Session["UserName"].ToString();
                                dt.Columns.Add(col1);
                            }

                            if (dt.Columns.Contains("") == false)
                            {
                                DataColumn col1 = new DataColumn("StationName", System.Type.GetType("System.String"));
                                col1.DefaultValue = ddlAirport.SelectedItem.Value.ToString();
                                dt.Columns.Add(col1);
                            }

                        }
                        ReportViewer1.Visible = true;
                       
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptCargoLoadFactor.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsrptCargoLoadFactor_dtrptCargoLoadFactor", dt);
                       
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                      
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
        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsrptCargoLoadFactor_dtrptCargoLoadFactor1", ds.Tables[1]));
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptCargoLoadFactor.aspx", false);
        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
           
            DataTable dt = null;
            Session["CargoLoadFactor"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataTable)Session["CargoLoadFactor"] == null)
                    GetData();
         
                dt = (DataTable)Session["CargoLoadFactor"];

                if (Session["CargoLoadFactor"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
               
                string attachment = "attachment; filename=CargoLoadFactorReport.xls";
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
    }
}
