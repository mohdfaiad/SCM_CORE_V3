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
using System.Data.SqlClient;

namespace ProjectSmartCargoManager
{
    public partial class rptSummaryOfCharges : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString()); 
        
        DataTable dtTableStation = new DataTable();
        DataTable dtTableDetail = new DataTable();
        DataTable dtTableSector = new DataTable();
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["rptSummaryOfCharges"] = null;
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

        

        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                Session["rptSummaryOfCharges"] = null;
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
                object[] param = { fromdate, todate };
                string[] pname = { "fromdate", "todate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spSummaryOfCharges", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        //Session["rptSummaryOfCharges"] = ds.Tables[0];
                       
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
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptSummaryOfCharges.rdlc");
                        ReportDataSource datasource = new ReportDataSource("rptSummaryOfCharges_DataTable1", dt);

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
            string Param = "FrmDt:" + txtFromdate.Text.ToString() + ", ToDt:" + txtTodate.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Summary Of Charges Report", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/rptSummaryOfCharges.aspx", false);
        }

        public void GetData()
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                Session["rptSummaryOfCharges"] = null;
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
                object[] param = { fromdate, todate };
                string[] pname = { "fromdate", "todate" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };



                ds = da.SelectRecords("spSummaryOfCharges", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["rptSummaryOfCharges"] = ds;//.Tables[0];
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
            Session["rptSummaryOfCharges"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataSet)Session["rptSummaryOfCharges"] == null)
                    GetData();

                dt = ((DataSet)Session["rptSummaryOfCharges"]).Tables[0];
              

                if (Session["rptSummaryOfCharges"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    SaveUserActivityLog(lblStatus.Text);
                    ReportViewer1.Visible = false;
                    return;
                }
                Session["rptSummaryOfCharges"] = null;
                string attachment = "attachment; filename=SummaryOfCharges.xls";
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

        

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCodesWithName(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode + '(' + AgentName + ')' from AgentMaster where (AgentName like '%" + prefixText + "%' or AgentCode like '%" + prefixText + "%')", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }
            dad = null;
            if (ds != null)
                ds.Dispose();

            return list.ToArray();
        }



       
    }
}
