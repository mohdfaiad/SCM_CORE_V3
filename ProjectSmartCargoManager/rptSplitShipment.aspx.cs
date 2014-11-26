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
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class rptSplitShipment : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                LoadCarrierDropdown();
                ReportViewer1.Visible = false;
                if (Request.QueryString["FromDate"] != null && Request.QueryString["ToDate"] != null)
                {
                    txtFromdate.Text = Request.QueryString["FromDate"].ToString();
                    txtTodate.Text = Request.QueryString["ToDate"].ToString();
                    ddlStation.Text = Request.QueryString["Station"] != null ? Request.QueryString["Station"].ToString().Trim() : "All";
                    
                    btnList_Click(null, null);
                }
                else
                {
                    //GetAirportCode();
                    //LoadCarrierDropdown();
                    //ReportViewer1.Visible = false;
                    txtFromdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                    txtTodate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");

                }

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
                Session["rptSplitShipment"] = null;
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
                object[] param = { fromdate, todate, ddlStation.SelectedItem.Value.ToString(), ddlStationType.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Value.Trim() };
                string[] pname = { "fromdate", "todate", "Station", "StationType", "Carrier" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spSplitShipmentReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["rptSplitShipment"] = ds.Tables[0];
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
                                DataColumn col1 = new DataColumn("ClientName", System.Type.GetType("System.String"));
                                col1.DefaultValue = client;
                                dt.Columns.Add(col1);
                            }

                        }
                        ReportViewer1.Visible = true;

                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSplitShipment.rdlc");
                        ReportDataSource datasource = new ReportDataSource("dsrptSplitShipment_DataTable1", dt);

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

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Split Shipment Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptSplitShipment.aspx", false);
        }

        public void GetData()
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                Session["rptSplitShipment"] = null;
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
                object[] param = { fromdate, todate, ddlStation.SelectedItem.Value.ToString(), ddlStationType.SelectedItem.Value.ToString(), ddlCarrier.SelectedItem.Value.Trim() };
                string[] pname = { "fromdate", "todate", "Station", "StationType", "Carrier" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spSplitShipmentReport", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["rptSplitShipment"] = ds.Tables[0];
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
                                DataColumn col1 = new DataColumn("ClientName", System.Type.GetType("System.string"));
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
            Session["rptSplitShipment"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataTable)Session["rptSplitShipment"] == null)
                    GetData();

                dt = (DataTable)Session["rptSplitShipment"];

                if (Session["rptSplitShipment"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
                Session["rptSplitShipment"] = null;
                string attachment = "attachment; filename=SplitShipmentReport.xls";
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetShipperCode(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            string strQuery = "SELECT Isnull(AccountCode,'') + '|' + Isnull(AccountName,'') + '|' + Isnull(PhoneNumber,'') + '|' ";
            strQuery = strQuery + "+ Isnull(Adress1,'') + '|' + Isnull(Country,'') + '|' + Isnull(Adress2,'') + '|' + Isnull(City,'')";
            strQuery = strQuery + "+ '|' + Isnull(State,'') + '|' +  CASE WHEN len(Isnull(Email,''))>30 THEN SUBSTRING(Isnull(Email,''),0,30) ELSE Isnull(Email,'') END + '|' + CONVERT(VARCHAR,Isnull(ZipCode,''))";
            strQuery = strQuery + " from AccountMaster where (AccountName like '" + prefixText + "%' or AccountCode like '" + prefixText + "%')";
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter(strQuery, con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());
            }

            if (ds != null)
                ds.Dispose();
            dad = null;
            return list.ToArray();
        }


    }
}
