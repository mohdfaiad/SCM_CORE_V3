using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using QID.DataAccess;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class RptPendingInvoiceCollection : System.Web.UI.Page
    {
        #region Variables
        BillingAWBFileInvoiceBAL objBillBAL = new BillingAWBFileInvoiceBAL();
        BALCollectionDetails objBAL = new BALCollectionDetails();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BALAgentCredit ObjTrans = new BALAgentCredit();
        DateTime dtCurrentDate = DateTime.Now;

        string strfromdate, strtodate, strchequedate;
        DataTable DTFilters = new DataTable();
        DataTable dtTable1 = new DataTable();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptPendingInvCollection.Visible = false;

                txtInvoiceFrom.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                txtInvoiceTo.Text = dtCurrentDate.ToString("dd/MM/yyyy");

                LoadAgentDropdown(); //AgentName 
                LoadAgentCodeDropdown(); //AgentCode

                //Agent authorization
                string AgentCode = Convert.ToString(Session["AgentCode"]);

                if (AgentCode != "")
                {
                    ddlAgentName.SelectedValue = AgentCode;
                    ddlAgentName_SelectedIndexChanged(null, null);
                    ddlAgentCode.Enabled = false;
                    ddlAgentName.Enabled = false;

                }
            }
        }

        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            DataSet ds = objBillBAL.GetAllAgents();
            if (ds != null)
            {
                ddlAgentName.DataSource = ds;
                ddlAgentName.DataMember = ds.Tables[0].TableName;
                ddlAgentName.DataTextField = "AgentName";
                ddlAgentName.DataValueField = "AgentCode";
                ddlAgentName.DataBind();
                ddlAgentName.Items.Insert(0, new ListItem("All", ""));
                ddlAgentName.SelectedIndex = -1;
            }
        }
        #endregion LoadAgentDropdown

        #region Load Agent Code Dropdown
        public void LoadAgentCodeDropdown()
        {
            DataSet ds = objBillBAL.GetAllAgents();
            if (ds != null)
            {
                ddlAgentCode.DataSource = ds;
                ddlAgentCode.DataMember = ds.Tables[0].TableName;
                ddlAgentCode.DataTextField = "AgentCode";
                ddlAgentCode.DataValueField = "AgentName";
                ddlAgentCode.DataBind();
                ddlAgentCode.Items.Insert(0, new ListItem("All", ""));
                ddlAgentCode.SelectedIndex = -1;
            }
        }
        #endregion LoadAgentCodeDropdown

        protected void ddlAgentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentName.SelectedIndex = ddlAgentCode.SelectedIndex;
        }

        protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentCode.SelectedIndex = ddlAgentName.SelectedIndex;
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                //Validation for From date
                if (txtInvoiceFrom.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter From Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DateTime dt;

                try
                {
                    //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                    //Change 03082012
                    string day = txtInvoiceFrom.Text.Substring(0, 2);
                    string mon = txtInvoiceFrom.Text.Substring(3, 2);
                    string yr = txtInvoiceFrom.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date Format Invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Validation for To date
                if (txtInvoiceTo.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter To Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                    //Change 03082012
                    string day = txtInvoiceTo.Text.Substring(0, 2);
                    string mon = txtInvoiceTo.Text.Substring(3, 2);
                    string yr = txtInvoiceTo.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dt)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;

                    return;
                }

                ReportBAL objBal = new ReportBAL();
                string strResult = string.Empty;

                try
                {
                    strResult = objBal.GetReportInterval(DateTime.ParseExact(txtInvoiceFrom.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtInvoiceTo.Text.Trim(), "dd/MM/yyyy", null));
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
                    return ;
                }

                #region params
                string [] Pname =new string[7];
                object[] Pvalue = new object[7];
                SqlDbType[] Ptype = new SqlDbType[7];

                Pname[0] = "AgentCode";
                Pvalue[0] = ddlAgentName.SelectedValue;
                Ptype[0] = SqlDbType.VarChar;

                Pname[1] = "BillingType";
                Pvalue[1] = ddlBillType.SelectedValue;
                Ptype[1] = SqlDbType.VarChar;

                Pname[2] = "Origin";
                Pvalue[2] = txtOrigin.Text;
                Ptype[2] = SqlDbType.VarChar;

                Pname[3] = "FromDate";
                Pvalue[3] = Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss");
                Ptype[3] = SqlDbType.VarChar;

                Pname[4] = "ToDate";
                Pvalue[4] = Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                Ptype[4] = SqlDbType.VarChar;

                Pname[5] = "InvoiceNumber";
                Pvalue[5] = txtInvoiceNumber.Text.Trim();
                Ptype[5] = SqlDbType.VarChar;

                Pname[6] = "AWBNumber";
                Pvalue[6] = txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim();
                Ptype[6] = SqlDbType.VarChar;

                #endregion 

                DataSet DSInvoicedata = db.SelectRecords("SP_GetOutstandngInvoices", Pname, Pvalue, Ptype);

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    Session["dsRptPendingInvCollection"] = DSInvoicedata;
                    rptPendingInvCollection.Visible = true;
                    dtTable1 = DSInvoicedata.Tables[0];

                    DTFilters.Columns.Add("AgentName");
                    DTFilters.Columns.Add("AgentCode");
                    DTFilters.Columns.Add("BillType");
                    DTFilters.Columns.Add("FromDate");
                    DTFilters.Columns.Add("ToDate");
                    DTFilters.Columns.Add("Origin");
                    DTFilters.Columns.Add("AWBNo");
                    DTFilters.Columns.Add("InvNo");

                    DTFilters.Rows.Add(
                        ddlAgentName.SelectedItem.Text,
                        ddlAgentCode.SelectedItem.Text,
                        ddlBillType.SelectedItem.Text,
                        txtInvoiceFrom.Text,
                        txtInvoiceTo.Text,
                        txtOrigin.Text,
                        txtAWBNumber.Text.Trim(),
                        txtInvoiceNumber.Text.Trim());

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
                    if (dtTable1.Columns.Contains("Logo") == false)
                    {
                        DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                        col1.DefaultValue = Logo.ToArray();
                        dtTable1.Columns.Add(col1);
                    }

                    ReportDataSource rds1 = new ReportDataSource();
                    rptPendingInvCollection.Visible = true;
                    rptPendingInvCollection.Reset();
                    rptPendingInvCollection.ProcessingMode = ProcessingMode.Local;
                    LocalReport rep1 = rptPendingInvCollection.LocalReport;
                    rep1.ReportPath = Server.MapPath("~/Reports/PendingInvoice.rdlc");
                    rds1.Name = "dsPendingInvoice_dtPendingInvoice";
                    rds1.Value = dtTable1;
                    rep1.DataSources.Add(rds1);

                    rptPendingInvCollection.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                    SaveUserActivityLog("");
                }
                else
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                    Session["dsRptPendingInvCollection"] = null;
                    rptPendingInvCollection.Visible = false;
                    SaveUserActivityLog(lblStatus.Text);
                }

            }
            catch (Exception ex)
            { SaveUserActivityLog(ex.Message); }
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsPendingInvoice_dtPendingInvoiceSub", DTFilters));

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            Session["dsRptPendingInvCollection"] = null;
            rptPendingInvCollection.Visible = false;

            txtInvoiceFrom.Text = dtCurrentDate.ToString("dd/MM/yyyy");
            txtInvoiceTo.Text = dtCurrentDate.ToString("dd/MM/yyyy");

            txtOrigin.Text = string.Empty;

            txtInvoiceNumber.Text = string.Empty;

            ddlAgentCode.SelectedIndex = 0;
            ddlAgentName.SelectedIndex = 0;

            ddlBillType.SelectedIndex = 0;

            txtAWBNumber.Text = string.Empty;
            txtAWBPrefix.Text = string.Empty;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            lblStatus.Text = string.Empty;
            //Session["dsRptPendingInvCollection"] = null;
            try
            {
                if (Session["dsRptPendingInvCollection"] == null)
                GetData();

                dsExp = (DataSet)Session["dsRptPendingInvCollection"];

                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                    dt = (DataTable)dsExp.Tables[0];

                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }

                #region Remove Columns For Excel

                if (dt.Columns.Contains("SrNo"))
                {
                    dt.Columns.Remove("SrNo");
                }
                if (dt.Columns.Contains("Logo"))
                {
                    dt.Columns.Remove("Logo");
                }
                if (dt.Columns.Contains("TDSAmount"))
                {
                    dt.Columns.Remove("TDSAmount");
                }
                if (dt.Columns.Contains("DCMAmount"))
                {
                    dt.Columns.Remove("DCMType");
                }
                if (dt.Columns.Contains("ChequeDate"))
                {
                    dt.Columns.Remove("ChequeDate");
                }
                if (dt.Columns.Contains("BankName"))
                {
                    dt.Columns.Remove("BankName");
                }
                if (dt.Columns.Contains("IsActive"))
                {
                    dt.Columns.Remove("IsActive");
                }
                if (dt.Columns.Contains("BillType"))
                {
                    dt.Columns.Remove("BillType");
                }
                if (dt.Columns.Contains("Amt194C"))
                {
                    dt.Columns.Remove("Amt194C");
                }
                #endregion

                string attachment = "attachment; filename=Pending Invoices.xls";
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
            }
            catch (Exception ex)
            { }
            finally
            {
                dsExp = null;
                dt = null;
            }
        }

        private void SaveUserActivityLog(string ErrorLog)
        {
            ReportBAL objBAL = new ReportBAL();
            // taking all parameters as user selected in report in one variable - "Param"
            string Param = "From:" + txtInvoiceFrom.Text.Trim() + ", To:" + txtInvoiceTo.Text.Trim() + ", Agent:" + ddlAgentCode.SelectedItem.Text + "(" + ddlAgentName.SelectedItem.Text + ")" + ", BillType:" + ddlBillType.SelectedItem.Text + ", Org:" + txtOrigin.Text.Trim();
            objBAL.SaveUserActivityLog(Convert.ToString(Session["IpAddress"]), Session["UserName"].ToString(), "Pending Invoices", Convert.ToDateTime(Session["IT"]), Param, ErrorLog, Session["Station"].ToString());
        }

        protected void GetData() {
            try
            {
                lblStatus.Text = string.Empty;

                //Validation for From date
                if (txtInvoiceFrom.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter From Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DateTime dt;

                try
                {
                    //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                    //Change 03082012
                    string day = txtInvoiceFrom.Text.Substring(0, 2);
                    string mon = txtInvoiceFrom.Text.Substring(3, 2);
                    string yr = txtInvoiceFrom.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date Format Invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Validation for To date
                if (txtInvoiceTo.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter To Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                    //Change 03082012
                    string day = txtInvoiceTo.Text.Substring(0, 2);
                    string mon = txtInvoiceTo.Text.Substring(3, 2);
                    string yr = txtInvoiceTo.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dt)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;

                    return;
                }

                #region params
                string[] Pname = new string[7];
                object[] Pvalue = new object[7];
                SqlDbType[] Ptype = new SqlDbType[7];

                Pname[0] = "AgentCode";
                Pvalue[0] = ddlAgentName.SelectedValue;
                Ptype[0] = SqlDbType.VarChar;

                Pname[1] = "BillingType";
                Pvalue[1] = ddlBillType.SelectedValue;
                Ptype[1] = SqlDbType.VarChar;

                Pname[2] = "Origin";
                Pvalue[2] = txtOrigin.Text;
                Ptype[2] = SqlDbType.VarChar;

                Pname[3] = "FromDate";
                Pvalue[3] = Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss");
                Ptype[3] = SqlDbType.VarChar;

                Pname[4] = "ToDate";
                Pvalue[4] = Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                Ptype[4] = SqlDbType.VarChar;

                Pname[5] = "InvoiceNumber";
                Pvalue[5] = txtInvoiceNumber.Text.Trim();
                Ptype[5] = SqlDbType.VarChar;

                Pname[6] = "AWBNumber";
                Pvalue[6] = txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim();
                Ptype[6] = SqlDbType.VarChar;

                #endregion

                DataSet DSInvoicedata = db.SelectRecords("SP_GetOutstandngInvoices", Pname, Pvalue, Ptype);

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    Session["dsRptPendingInvCollection"] = DSInvoicedata;
                    SaveUserActivityLog("");
                }

                else
                {
                    Session["dsRptPendingInvCollection"] = null;
                    rptPendingInvCollection.Visible = false;
                    SaveUserActivityLog("No Records to Export");
                }

            }
            catch (Exception ex)
            {
                SaveUserActivityLog(ex.Message);
            }
        }
    }
}