using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;

using System.IO;
using Microsoft.Reporting.WebForms;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class rptARPaymentSummary : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        AgentBAL objBLL = new AgentBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    DataSet ds = objBLL.GetAgentListNew(Convert.ToString(Session["AgentCode"]));
                    if (ds != null)
                    {
                        ddlAgent.DataSource = ds;
                        ddlAgent.DataMember = ds.Tables[0].TableName;
                        ddlAgent.DataTextField = "AgentCode";
                        ddlAgent.DataValueField = "AgentName";
                        ddlAgent.DataBind();
                        ddlAgent.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                { }
                finally
                {
                    if (objBLL != null)
                        objBLL = null;
                }

            }
        }
        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string ErrorLog = string.Empty;
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            DataTable dtt1 = new DataTable();
            DataTable dtt2 = new DataTable();
            DataSet ds = null;
            try
            {
                Session["dsARPaymentSummary"] = null;
                ReportViewer1.Visible = false;
                if (Validate() == false)
                {
                    Session["dsARPaymentSummary"] = null;
                    ReportViewer1.Visible = false;
                    return;
                }
                System.IO.MemoryStream Logo = null;

                string Agent, dest;
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                if (ddlAgent.Text.Trim() == "" || ddlAgent.Text.Trim() == "Select")
                {
                    Agent = "";
                }
                else
                {
                    Agent = ddlAgent.SelectedItem.Text.ToString();
                    string str = "- " + ddlAgent.SelectedItem.Value.ToString();
                    Agent = Agent.Replace(str, "");
                    Agent = Agent.Trim(' ');
                }
                object[] param = { Agent, ddlmonth.SelectedValue, ddlYear.SelectedValue };
                string[] pname = { "Agent", "Month", "Year" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                ReportBAL objBal = new ReportBAL();

                ds = da.SelectRecords("Sp_ARPaymentSummary", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReportViewer1.Visible = true;
                        Session["dsARPaymentSummary"] = ds.Tables[0];



                        //Logo
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                            //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }

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
                        }

                        //Agent
                        try
                        {
                            if (ddlAgent.Text.Trim() == "" || ddlAgent.Text.Trim() == "Select")
                            {
                                Agent = "All";
                            }
                            else
                            {
                                Agent = ddlAgent.SelectedItem.Text.Trim();
                            }
                        }
                        catch (Exception ex)
                        {
                            Agent = "All";
                        }

                        dtt = null;
                        if (dt.Rows.Count > 0)
                        {

                            dtt = dt.Copy();
                            if (dtt.Columns.Contains("Agent1") == false)
                            {
                                DataColumn col2 = new DataColumn("Agent1", System.Type.GetType("System.String"));
                                col2.DefaultValue = Agent;
                                dtt.Columns.Add(col2);
                            }
                        }



                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/rptARPaymentSummary.rdlc");
                        //Customers dsCustomers = GetData("select top 20 * from customers");
                        ReportDataSource datasource = new ReportDataSource("dsARPaymentSummary_dtPaymentSummary", dtt);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(datasource);
                        //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForSUBrpt);

                        //btnExport.Visible = true;
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
                if (dtt != null)
                    dtt = null;
                if (dtt1 != null)
                    dtt1 = null;
                if (dtt2 != null)
                    dtt2 = null;


            }
        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "Agent:" + ddlAgent.Text.ToString() + ", Month:" + ddlmonth.Text.ToString() + ", Year:" + ddlYear.Text.ToString();

            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "A/RPaymentSummaryReport", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }
        private bool Validate()
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            try
            {
                if (ddlmonth.SelectedItem.Text == "Select" && ddlYear.SelectedItem.Text == "Select")
                {
                    lblStatus.Text = "Please Select Month and Year.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return false;
                }
                ReportBAL objrpt = new ReportBAL();
                string strResult = string.Empty;
                try
                {
                    //strResult = objrpt.GetReportInterval(DateTime.ParseExact(.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txttodate.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    // txtfrmdate.Focus();
                    return false;
                }



            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtfrmdate.Focus();
                return false;
            }

            return true;
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/rptARPaymentSummary.aspx",false);
            }
            catch (Exception ex)
            { }
        }
    }
}
