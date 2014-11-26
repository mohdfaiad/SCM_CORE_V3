using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data ;
using System.Drawing;
using BAL;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using QID.DataAccess;
using System.Globalization;
using System.Text;
//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
//using DataDynamics.Reports.Extensibility.Rendering.IO;

using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{

    public partial class FrmInvoiceListing : System.Web.UI.Page
    {
        DataSet dsResult;
        BAL.BALInvoiceListing objBAL = new BAL.BALInvoiceListing();
        BillingAWBFileInvoiceBAL objBillBAL = new BillingAWBFileInvoiceBAL();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DataSet dsDCM;
        DataSet dset;
        BillingCASSEncode EncodeBilling=new BillingCASSEncode();
        HandOffFileCASSEncode EncodeHandOff=new HandOffFileCASSEncode();
        DateTime dtCurrentDate = DateTime.Now;

        static decimal GTotalInvoiceAmount = 0, GTotalCollectionAmount = 0, GTotalInvoiceBalance = 0;

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                

                if (Session["awbPrefix"] != null)
                {
                    txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                }
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["awbPrefix"] = objBal.awbPrefix();
                    txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                }

                dtCurrentDate = (DateTime)Session["IT"];
                if (!IsPostBack)
                {
                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grdInvoiceList.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion

                    if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AcceptPartnerAWB") == null ? "false" : CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AcceptPartnerAWB")))
                    {
                        btnCASS.Visible = false;
                        btnCASSHandOffFile.Visible = false;
                        btnCASSStandardDocs.Visible = false;
                    }
                    else
                    {
                        btnCASS.Visible = true;
                        btnCASSHandOffFile.Visible = true;
                        btnCASSStandardDocs.Visible = true;
                    }

                    Session["ExportERP"] = null;
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
                        //disableForAgent();
                    }
                    //If invoice # received as query string.
                    if (Request.QueryString != null && Request.QueryString["invno"] != null &&
                        Request.QueryString["invno"] != "")
                    {
                        txtInvoiceNumber.Text = Request.QueryString["invno"];
                        //If inv type received as query string.
                        if (Request.QueryString["invtype"] != null && Request.QueryString["invtype"] != "")
                        {
                            ddlInvoiceType.SelectedValue = Request.QueryString["invtype"];
                        }
                        else
                        {
                            if (Request.QueryString["invno"].ToUpper().Contains("PROFORMA_"))
                            {   //Set invoice type as proforma invoice.
                                ddlInvoiceType.SelectedValue = "4";
                            }
                            else
                            {   //Set invoice type as regular invoice.
                                ddlInvoiceType.SelectedValue = "1";
                            }
                        }
                        txtInvoiceFrom.Text = "01/01/2012";
                        txtInvoiceTo.Text = Convert.ToDateTime(Session["IT"].ToString()).ToString("dd/MM/yyyy");
                        btnList_Click(this, new EventArgs());
                        txtInvoiceFrom.Text = "";
                        txtInvoiceTo.Text = "";
                    }
                    txtAWBNumber.Enabled = false;
                }

                if (ddlInvoiceType.SelectedValue == "1" || ddlInvoiceType.SelectedValue == "4")
                {
                    txtAWBNumber.Enabled = false;
                }
                else
                {
                    txtAWBNumber.Enabled = true;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion Load

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

        protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentCode.SelectedIndex = ddlAgentName.SelectedIndex;
        }

        protected void ddlAgentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentName.SelectedIndex = ddlAgentCode.SelectedIndex;
        }    

        protected void bindInvoiceList()
        {
            try
            {
                Session["dsInvoice"] = null;
                string strfromdate, strtodate;
                string AWBNumber = "";
                //lblStatus.Visible = false;
                lblStatus.Text = "";

                //Validation for From date
                if (txtInvoiceFrom.Text == "" && txtInvoiceNumber.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date or Enter Invoice Number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                }

                DateTime dt;

                try
                {
                    if (txtInvoiceFrom.Text != "")
                    {
                        //dt = Convert.ToDateTime(txtbillingfrom.Text);
                        //Change 03082012
                        string day = txtInvoiceFrom.Text.Substring(0, 2);
                        string mon = txtInvoiceFrom.Text.Substring(3, 2);
                        string yr = txtInvoiceFrom.Text.Substring(6, 4);
                        strfromdate = yr + "-" + mon + "-" + day;
                        dt = Convert.ToDateTime(strfromdate);
                    }
                    else
                    {
                        strfromdate = "2012" + "-" + "01" + "-" + "01";
                        dt = Convert.ToDateTime(strfromdate);
                    }
                   
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Validation for To date
                if (txtInvoiceTo.Text == "" && txtInvoiceNumber.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date or Enter Invoice Number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    if (txtInvoiceTo.Text != "")
                    {
                        string day = txtInvoiceTo.Text.Substring(0, 2);
                        string mon = txtInvoiceTo.Text.Substring(3, 2);
                        string yr = txtInvoiceTo.Text.Substring(6, 4);
                        strtodate = yr + "-" + mon + "-" + day;
                        dtto = Convert.ToDateTime(strtodate);
                    }
                    else
                    {
                        strtodate = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
                        dtto = Convert.ToDateTime(strtodate);
                    }
                    
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

                if (txtAWBNumber.Text.Trim() != "")
                {
                    AWBNumber = txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim();
                }

                DataSet DSInvoicedata = objBAL.GetInvoiceData(ddlInvoiceType.SelectedValue, ddlAgentName.SelectedValue, ddlBillType.SelectedValue, txtOrigin.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), txtInvoiceNumber.Text.Trim(), AWBNumber, ddlInvoiceStatus.SelectedValue);

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    GTotalInvoiceAmount = 0; GTotalCollectionAmount = 0; GTotalInvoiceBalance = 0;

                    for (int cnt = 0; cnt < DSInvoicedata.Tables[0].Rows.Count; cnt++)
                    {
                        GTotalInvoiceAmount += Convert.ToDecimal(DSInvoicedata.Tables[0].Rows[cnt]["InvoiceAmount"].ToString());
                        GTotalCollectionAmount += Convert.ToDecimal(DSInvoicedata.Tables[0].Rows[cnt]["CollectedAmount"].ToString());
                        GTotalInvoiceBalance += Convert.ToDecimal(DSInvoicedata.Tables[0].Rows[cnt]["PendingAmount"].ToString());
                    }

                    Session["dsInvoice"] = DSInvoicedata;
                    grdInvoiceList.DataSource = DSInvoicedata.Tables[0];
                    grdInvoiceList.DataBind();
                    grdInvoiceList.Visible = true;
                    btnPrintInvoice.Visible = true;
                    btnPrintInvoicePDF.Visible = true;
                    btnCloseInvoice.Visible = true;
                    btnInvoiceSummary.Visible = true;
                    btnExportERP.Visible = true;
                    //btnCASS.Visible = true;
                    btnPrint.Visible = true;
                    if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AcceptPartnerAWB") == null ? "false" : CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AcceptPartnerAWB")))
                    {
                        btnCASS.Visible = false;
                        btnCASSHandOffFile.Visible = false;
                        btnCASSStandardDocs.Visible = false;
                    }
                    else
                    {
                        btnCASS.Visible = true;
                        btnCASSHandOffFile.Visible = true;
                        btnCASSStandardDocs.Visible = true;
                    }
                    //btnCASSHandOffFile.Visible = true;
                    hf1.Value = ddlAgentName.SelectedItem.Text;
                    hf2.Value = ddlBillType.SelectedItem.Text;
                    hf3.Value = txtInvoiceFrom.Text;
                    hf4.Value = txtInvoiceTo.Text;
                    if(txtOrigin.Text.Trim() == "")
                        hf5.Value = "-";
                    else
                        hf5.Value = txtOrigin.Text;

                }
                else
                {
                    grdInvoiceList.Visible = false;
                    btnCASSHandOffFile.Visible = false;
                    btnPrint.Visible = false;
                    btnPrintInvoice.Visible = false;
                    btnPrintInvoicePDF.Visible = false;
                    btnCloseInvoice.Visible = false;
                    btnInvoiceSummary.Visible = false;
                    btnExportERP.Visible = false;
                    btnCASS.Visible = false;
                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Blue;
                }

            }
            catch (Exception ex)
            {

            }

        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            bindInvoiceList();
        }

        protected void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            InvoicePrint();

            //To check Invoice Type

            if (ddlInvoiceType.SelectedValue == "1") //Regular Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateRegularInvoices();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "2") //Walk-In Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateWalkInInvoices();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "3") //Destination Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateDestInvoices();", true);
            }
            else //if (ddlInvoiceType.SelectedValue == "1") //Proforma Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateProformaInvoices();", true);
            }
        }

        protected void InvoicePrint()
        {
            try
            {
                DataSet dsInvoice = (DataSet)Session["dsInvoice"];
                string InvoiceList = "";
                for (int j = 0; j < grdInvoiceList.Rows.Count; j++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[j].FindControl("ChkSelect")).Checked)
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsInvoice.Tables[0].Rows[j + (grdInvoiceList.PageIndex * grdInvoiceList.PageSize)]["InvoiceNumber"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (InvoiceList == "")
                        {
                            InvoiceList = InvoiceList + dsInvoice.Tables[0].Rows[j + (grdInvoiceList.PageIndex * grdInvoiceList.PageSize)]["InvoiceNumber"].ToString();
                        }
                        else
                        {
                            InvoiceList = InvoiceList + "," + dsInvoice.Tables[0].Rows[j + (grdInvoiceList.PageIndex * grdInvoiceList.PageSize)]["InvoiceNumber"].ToString();
                        }

                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }

                hfInvoiceNos.Value = InvoiceList;

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void GenerateAgentInvoice(object[] InvoiceNum)
        {
            dsResult = objBAL.GetInvoiceDataImport(InvoiceNum);

            if (dsResult != null)
            {
                if (dsResult.Tables != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["CurrentInvoiceNo"] = dsResult.Tables[0].Rows[0]["InvoiceNumber"].ToString();

                                Session["ShowExcel"] = "";

                                DataTable DTExport = new DataTable();

                                DTExport.Columns.Add("AgentName");
                                DTExport.Columns.Add("AgentAddress1");
                                DTExport.Columns.Add("AgentAddress2");
                                DTExport.Columns.Add("AgentAddress3");
                                DTExport.Columns.Add("InvoiceNumber");
                                DTExport.Columns.Add("InvoiceDate");
                                DTExport.Columns.Add("ServiceTaxNo");
                                DTExport.Columns.Add("PANNo");
                                DTExport.Columns.Add("BillMonthYr");
                                DTExport.Columns.Add("ChargeableWt");
                                DTExport.Columns.Add("Freight");
                                DTExport.Columns.Add("FSC");
                                DTExport.Columns.Add("AHC");
                                DTExport.Columns.Add("FACHC");
                                DTExport.Columns.Add("AWBDO");
                                DTExport.Columns.Add("DueAgent");
                                DTExport.Columns.Add("SubTotal");
                                DTExport.Columns.Add("ServiceTax");
                                DTExport.Columns.Add("EduCess");
                                DTExport.Columns.Add("HighEduCess");
                                DTExport.Columns.Add("TotalInvoiceValue");
                                DTExport.Columns.Add("TotalInvoiceWords");


                                DTExport.Rows.Add(
                                    dsResult.Tables[0].Rows[0]["AgentName"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AgentAddress1"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AgentAddress2"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AgentAddress3"].ToString(),
                                    dsResult.Tables[0].Rows[0]["InvoiceNumber"].ToString(),
                                    dsResult.Tables[0].Rows[0]["InvoiceDate"].ToString(),
                                    dsResult.Tables[0].Rows[0]["ServiceTaxNo"].ToString(),
                                    dsResult.Tables[0].Rows[0]["PANNo"].ToString(),
                                    dsResult.Tables[0].Rows[0]["BillMonthYr"].ToString(),
                                    dsResult.Tables[0].Rows[0]["ChargeableWt"].ToString(),
                                    dsResult.Tables[0].Rows[0]["Freight"].ToString(),
                                    dsResult.Tables[0].Rows[0]["FSC"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AHC"].ToString(),
                                    dsResult.Tables[0].Rows[0]["FACHC"].ToString(),
                                    dsResult.Tables[0].Rows[0]["AWBDO"].ToString(),
                                    dsResult.Tables[0].Rows[0]["DueAgent"].ToString(),
                                    dsResult.Tables[0].Rows[0]["SubTotal"].ToString(),
                                    dsResult.Tables[0].Rows[0]["ServiceTax"].ToString(),
                                    dsResult.Tables[0].Rows[0]["EduCess"].ToString(),
                                    dsResult.Tables[0].Rows[0]["HighEduCess"].ToString(),
                                    dsResult.Tables[0].Rows[0]["TotalInvoiceValue"].ToString(),
                                    dsResult.Tables[0].Rows[0]["TotalInvoiceWords"].ToString());


                                Session["ShowExcel"] = DTExport;

                                Session["AWBData"] = "";
                                DataTable DTAWBData = new DataTable();
                                DTAWBData = dsResult.Tables[1];
                                Session["AWBData"] = DTAWBData;

                                //Response.Write("<script>");
                                //Response.Write("window.open('ShowAgentInvoiceImport.aspx','" + Session["CurrentInvoiceNo"].ToString() + "')");
                                //Response.Write("</script>");

                                //Page.ClientScript.RegisterClientScriptBlock(GetType(), "myScript" + Session["CurrentInvoiceNo"].ToString(), "<script>alert('hello world');</script>");

                                //Page.ClientScript.RegisterStartupScript(this.GetType(), "client" + Session["CurrentInvoiceNo"].ToString(), "window.open('ShowAgentInvoiceImport.aspx','_new','Height=300px, Width=700px, menubar=No, toolbar=no, scrollbars=yes');", true);
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "client" + Session["CurrentInvoiceNo"].ToString(), "CheckGrid();", true);
                                
                              

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        protected void grdInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = (DataSet)Session["dsInvoice"];
            grdInvoiceList.PageIndex = e.NewPageIndex;
            grdInvoiceList.DataSource = dst.Tables[0];
            grdInvoiceList.DataBind();
        }

        protected void btnPrintInvoicePDF_Click(object sender, EventArgs e)
        {
            InvoicePrint();

            //To check Invoice Type

            if (ddlInvoiceType.SelectedValue == "1") //Regular Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateRegularInvoicesPDF();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "2") //Walk-In Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateWalkInInvoicesPDF();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "3") //Destination Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateDestInvoicesPDF();", true);
            }
            else //if (ddlInvoiceType.SelectedValue == "1") //Proforma Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateProformaInvoicesPDF();", true);
            }
        }

        protected void btnInvoiceSummary_Click(object sender, EventArgs e)
        {
            //btnList_Click(null, null);
                            lblStatus.Text = string.Empty;
                string InvoiceNo = string.Empty;
            try
            {

                int count = 0;
                for (int j = 0; j < grdInvoiceList.Rows.Count; j++)
                {
                    if (((CheckBox)(grdInvoiceList.Rows[j].FindControl("ChkSelect"))).Checked)
                    {

                        count++;
                        if (count == 1)
                            InvoiceNo = "'"+((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                        else
                            InvoiceNo += "','" + ((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                    }
                }
                if (count == 0)
                {
                    lblStatus.Text = "No Records Selected!";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

            }
            catch (Exception ex)
            { }

            DataSet dsRes = (DataSet)Session["dsInvoice"];
            if (dsRes != null)
            {
                if (dsRes.Tables != null)
                {
                    if (dsRes.Tables.Count > 0)
                    {
                        if (dsRes.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                if (dsRes != null)
                                {
                                    string attachment = "attachment; filename=InvoiceSummary.xls";
                                    Response.ClearContent();
                                    Response.AddHeader("content-disposition", attachment);
                                    Response.ContentType = "application/vnd.ms-excel";
                                    string tab = "";
                                    foreach (DataColumn dc in dsRes.Tables[0].Columns)
                                    {
                                        Response.Write(tab + dc.ColumnName);
                                        tab = "\t";
                                    }
                                    Response.Write("\n");
                                    int i;
                                    foreach (DataRow dr in dsRes.Tables[0].Rows)
                                    {
                                        tab = "";
                                        for (i = 0; i < dsRes.Tables[0].Columns.Count; i++)
                                        {
                                            Response.Write(tab + dr[i].ToString());
                                            tab = "\t";
                                        }
                                        Response.Write("\n");
                                    }
                                    Response.End();
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }

        #region Button CASS
        protected void btnCASS_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                string[] QueryPname = new string[8];
                object[] QueryValue = new object[8];
                SqlDbType[] QueryType = new SqlDbType[8];


                QueryPname[0] = "InvoiceType";
                QueryPname[1] = "AgentCode";
                QueryPname[2] = "BillingType";
                QueryPname[3] = "Origin";
                QueryPname[4] = "FromDate";
                QueryPname[5] = "ToDate";
                QueryPname[6] = "InvoiceNumber";
                QueryPname[7] = "AWBNumber";


                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.DateTime;
                QueryType[5] = SqlDbType.DateTime;
                QueryType[6] = SqlDbType.VarChar;
                QueryType[7] = SqlDbType.VarChar;

                QueryValue[0] = ddlInvoiceType.SelectedValue;
                QueryValue[1] = ddlAgentCode.SelectedValue;
                QueryValue[2] = ddlBillType.SelectedValue;
                QueryValue[3] = txtOrigin.Text;
                QueryValue[4] = DateTime.ParseExact(txtInvoiceFrom.Text.Trim(), "dd/MM/yyyy", null);
                QueryValue[5] = DateTime.ParseExact(txtInvoiceTo.Text.Trim(), "dd/MM/yyyy", null);
                QueryValue[6] = txtInvoiceNumber.Text;
                QueryValue[7] = txtAWBNumber.Text;
                string InvoiceNo = txtInvoiceNumber.Text;
                int count = 0;

                for (int j = 0; j < grdInvoiceList.Rows.Count; j++)
                {
                    if (((CheckBox)(grdInvoiceList.Rows[j].FindControl("ChkSelect"))).Checked)
                    {
                        
                        count++;
                        if (count == 1)
                            InvoiceNo = "$"+((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                        else
                            InvoiceNo += "$,$" + ((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                    }
                }
                if (count > 0)
                {
                    InvoiceNo = InvoiceNo + "$";
                }




                QueryValue[6] = InvoiceNo;
                DataSet ds = db.SelectRecords("SP_GetCASSData", QueryPname, QueryValue, QueryType);
                
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        StringBuilder sb = EncodeBilling.GettingBillingCASSRecords(ds);

                        if (sb != null)
                        {
                            string filename = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString() + "01" + "1" + "1." + ds.Tables[0].Rows[0]["CountryCode"].ToString() + "E.txt";
                            Response.Clear();
                            Response.ContentType = "application/txt";
                            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                            Response.Write(sb.ToString());
                            Response.End();


                            lblStatus.Text = "CASS Billing Participant Export generated successfully!";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "CASS generation failed!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    lblStatus.Text = "No Records Found!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }



                #region Commented Code
                //#region All records
                //if (count < 1)
                //{
                //    DataSet ds = db.SelectRecords("SP_GetCASSData_Test", QueryPname, QueryValue, QueryType);
                //    #region Calling SSIM
                //    //string path = ConfigurationManager.AppSettings["CASSPath"].ToString();
                //    //string filename = "CargoSpot SSIM_9W_Summer 2013";
                //    //string filePath = Path.Combine(path, filename);
                //    //GetSSIMData(filePath);
                //    #endregion

                //    if (ds != null && ds.Tables.Count > 0)
                //    {
                //        if (ds.Tables[1].Rows.Count > 0)
                //        {
                //            string filename = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString() + "01" + "1" + "1." + ds.Tables[0].Rows[0]["CountryCode"].ToString() + "E.txt";
                //            QueryValue[6] = InvoiceNo;
                //            #region Preparing Parameters for text file generation

                //            //if (File.Exists(Server.MapPath(filePath)))
                //            //{
                //            //    //sw = File.AppendText(Server.MapPath(filePath));
                //            //    sw = File.CreateText(Server.MapPath(filePath));
                //            //}
                //            //else
                //            //{
                //            //    sw = File.CreateText(Server.MapPath(filePath));
                //            //}
                //            #endregion

                //            #region Header Record

                //            HeaderRecord HeaderRecords = new HeaderRecord();
                //            HeaderRecords.RecordID = "AAA";
                //            HeaderRecords.CASSAreaCode = "IN";
                //            HeaderRecords.BranchOfficeIndicator = "A";
                //            HeaderRecords.Filler = "";
                //            HeaderRecords.AirlinePrefix = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString();
                //            HeaderRecords.DatePeriodStart = ds.Tables[0].Rows[0]["PeriodStart"].ToString();
                //            HeaderRecords.DatePeriodEnd = ds.Tables[0].Rows[0]["PeriodEnd"].ToString();
                //            HeaderRecords.DateOfBilling = ds.Tables[0].Rows[0]["BillingDate"].ToString();
                //            HeaderRecords.FileNo = "";
                //            HeaderRecords.ReservedSpace1 = "";
                //            HeaderRecords.ReservedSpace2 = "";

                //            #endregion

                //            #region Retrieving AWB Records from DB
                //            AWBRecord[] AWBRecords = new AWBRecord[ds.Tables[1].Rows.Count];
                //            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                //            {
                //                AWBRecords[i] = new AWBRecord();
                //                AWBRecords[i].RecordID = ds.Tables[1].Rows[i]["RecordID"].ToString();
                //                AWBRecords[i].AWBInvoiceIndicator = ds.Tables[1].Rows[i]["InvoiceIndicator"].ToString();
                //                AWBRecords[i].VATIndicator = ds.Tables[1].Rows[i]["VATIndicator"].ToString();
                //                AWBRecords[i].AgentCode = ds.Tables[1].Rows[i]["AgentCode"].ToString();
                //                AWBRecords[i].AirlinePrefix = ds.Tables[1].Rows[i]["AirlinePrefix"].ToString();
                //                AWBRecords[i].AWBSerialNo = ds.Tables[1].Rows[i]["AWBNumber"].ToString();
                //                AWBRecords[i].AWBModCheck = ds.Tables[1].Rows[i]["AWBModCheck"].ToString();
                //                AWBRecords[i].Filler = " ";
                //                AWBRecords[i].Origin = ds.Tables[1].Rows[i]["Origin"].ToString();
                //                AWBRecords[i].AWBUseIndicator = ds.Tables[1].Rows[i]["AWBIndicator"].ToString();
                //                AWBRecords[i].BranchOfficeIndicator = ds.Tables[1].Rows[i]["BranchOfficeIndicator"].ToString();
                //                AWBRecords[i].Destination = ds.Tables[1].Rows[i]["Destination"].ToString();
                //                AWBRecords[i].DateAWBExecution = ds.Tables[1].Rows[i]["ExecutionDate"].ToString();
                //                AWBRecords[i].GrossWeight = ds.Tables[1].Rows[i]["GrossWeight"].ToString();
                //                AWBRecords[i].WeightIndicator = ds.Tables[1].Rows[i]["WeightIndicator"].ToString();
                //                AWBRecords[i].CurrencyCode = ds.Tables[1].Rows[i]["Currency"].ToString();
                //                AWBRecords[i].ChargeWeightPP = ds.Tables[1].Rows[i]["ChargeWeightPP"].ToString();
                //                AWBRecords[i].ValuationChargePP = ds.Tables[1].Rows[i]["ValChargePP"].ToString();
                //                AWBRecords[i].ChargeDueCarPP = ds.Tables[1].Rows[i]["ChargeDueCarPP"].ToString();
                //                AWBRecords[i].ChargeDueAgentPP = ds.Tables[1].Rows[i]["ChargeDueAgentPP"].ToString();
                //                AWBRecords[i].ChargeWeightCC = ds.Tables[1].Rows[i]["ChargedWeightCC"].ToString();
                //                AWBRecords[i].ValuationChargeCC = ds.Tables[1].Rows[i]["ValChargeCC"].ToString();
                //                AWBRecords[i].OCDueCarCC = ds.Tables[1].Rows[i]["OCDueCarCC"].ToString();
                //                AWBRecords[i].OCDueAgentCC = ds.Tables[1].Rows[i]["OCDueAgentCC"].ToString();
                //                AWBRecords[i].CommissionPercentage = ds.Tables[1].Rows[i]["CommissionPercentage"].ToString();
                //                AWBRecords[i].Commission = ds.Tables[1].Rows[i]["Commission"].ToString();
                //                AWBRecords[i].CommissionIndicator = ds.Tables[1].Rows[i]["CommissionIndicator"].ToString();
                //                AWBRecords[i].Discount = ds.Tables[1].Rows[i]["Discount"].ToString();
                //                AWBRecords[i].AWBAcceptanceDate = ds.Tables[1].Rows[i]["AcceptanceDate"].ToString();
                //                AWBRecords[i].AgentRefData = ds.Tables[1].Rows[i]["AgentReferenceCode"].ToString();
                //                AWBRecords[i].RateOfExchange = ds.Tables[1].Rows[i]["RateOfExchange"].ToString();
                //                AWBRecords[i].TaxDueAirline = ds.Tables[1].Rows[i]["TaxDueAirline"].ToString();
                //                AWBRecords[i].TaxDueAgent = ds.Tables[1].Rows[i]["TaxDueAgent"].ToString();
                //                AWBRecords[i].ReservedSpace = "";
                //                AWBRecords[i].TaxDueAirlineIndicator = ds.Tables[1].Rows[i]["TaxDueAirlineIndicator"].ToString();
                //                AWBRecords[i].DiscountIndicator = ds.Tables[1].Rows[i]["DiscountIndicator"].ToString();



                //            }
                //            #endregion

                //            #region Retrieving CCR/DCR Records from DB
                //            DCRDCORecord[] DCRSpecRecords = new DCRDCORecord[ds.Tables[2].Rows.Count];
                //            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //            {
                //                DCRSpecRecords[i] = new DCRDCORecord();
                //                DCRSpecRecords[i].RecordID = ds.Tables[2].Rows[i]["RecordID"].ToString();
                //                DCRSpecRecords[i].BranchOfficeIndicator = ds.Tables[2].Rows[i]["BranchOfficeIndicator"].ToString();
                //                DCRSpecRecords[i].VATIndicator = ds.Tables[2].Rows[i]["VATIndicator"].ToString();
                //                DCRSpecRecords[i].AirlinePrefix = ds.Tables[2].Rows[i]["AirlinePrefix"].ToString();
                //                DCRSpecRecords[i].AWBSerialNo = ds.Tables[2].Rows[i]["AWBNumber"].ToString();
                //                DCRSpecRecords[i].AWBModCheck = ds.Tables[2].Rows[i]["AWBModCheck"].ToString();
                //                DCRSpecRecords[i].Filler = " ";
                //                DCRSpecRecords[i].Origin = ds.Tables[2].Rows[i]["Origin"].ToString();
                //                DCRSpecRecords[i].AgentCode = ds.Tables[2].Rows[i]["AgentCode"].ToString();
                //                if (ds.Tables[2].Rows[i]["DCMNumber"].ToString().Length > 6)
                //                {
                //                    DCRSpecRecords[i].DCMNo = ds.Tables[2].Rows[i]["DCMNumber"].ToString().Substring(0, 6);
                //                }
                //                else
                //                    DCRSpecRecords[i].DCMNo = ds.Tables[2].Rows[i]["DCMNumber"].ToString();
                //                DCRSpecRecords[i].CurrencyCode = ds.Tables[2].Rows[i]["Currency"].ToString();
                //                DCRSpecRecords[i].RateOfExchange = ds.Tables[2].Rows[i]["RateOfExchange"].ToString();
                //                DCRSpecRecords[i].DateAWBExecution = ds.Tables[2].Rows[i]["ExecutionDate"].ToString();
                //                DCRSpecRecords[i].PPCCIndicator = ds.Tables[2].Rows[i]["PayMode"].ToString();
                //                DCRSpecRecords[i].ChargeWeight = ds.Tables[2].Rows[i]["ChargedWeight"].ToString();
                //                DCRSpecRecords[i].ValuationCharge = ds.Tables[2].Rows[i]["ValCharge"].ToString();
                //                DCRSpecRecords[i].Taxes = ds.Tables[2].Rows[i]["Tax"].ToString();
                //                DCRSpecRecords[i].ChargeDueAgent = ds.Tables[2].Rows[i]["OCDueAgent"].ToString();
                //                DCRSpecRecords[i].ChargeDueCar = ds.Tables[2].Rows[i]["OCDueCar"].ToString();
                //                DCRSpecRecords[i].VATOnAWBCharges = ds.Tables[2].Rows[i]["VATAWBCharges"].ToString();
                //                DCRSpecRecords[i].Commission = ds.Tables[2].Rows[i]["Commission"].ToString();
                //                DCRSpecRecords[i].VATOnCommission = ds.Tables[2].Rows[i]["VATAWBCommission"].ToString();
                //                DCRSpecRecords[i].Discount = ds.Tables[2].Rows[i]["Discount"].ToString();
                //                DCRSpecRecords[i].DiscountIndicator = ds.Tables[2].Rows[i]["DiscountIndicator"].ToString();
                //                DCRSpecRecords[i].WeightIndicator = ds.Tables[2].Rows[i]["WeightIndicator"].ToString();
                //                DCRSpecRecords[i].GrossWeight = ds.Tables[2].Rows[i]["GrossWeight"].ToString();
                //                DCRSpecRecords[i].Destination = ds.Tables[2].Rows[i]["Destination"].ToString();
                //                DCRSpecRecords[i].ReservedSpace = "";
                //                DCRSpecRecords[i].ReasonAdjustment = "";


                //            }

                //            #endregion

                //            #region Retrieving CCO/DCO Records from DB
                //            DCRDCORecord[] DCOSpecRecords = new DCRDCORecord[ds.Tables[3].Rows.Count];
                //            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                //            {
                //                DCOSpecRecords[i] = new DCRDCORecord();
                //                DCOSpecRecords[i].RecordID = ds.Tables[3].Rows[i]["RecordID"].ToString();
                //                DCOSpecRecords[i].BranchOfficeIndicator = ds.Tables[3].Rows[i]["BranchOfficeIndicator"].ToString();
                //                DCOSpecRecords[i].VATIndicator = ds.Tables[3].Rows[i]["VATIndicator"].ToString();
                //                DCOSpecRecords[i].AirlinePrefix = ds.Tables[3].Rows[i]["AirlinePrefix"].ToString();
                //                DCOSpecRecords[i].AWBSerialNo = ds.Tables[3].Rows[i]["AWBNumber"].ToString();
                //                DCOSpecRecords[i].AWBModCheck = ds.Tables[3].Rows[i]["AWBModCheck"].ToString();
                //                DCOSpecRecords[i].Filler = " ";
                //                DCOSpecRecords[i].Origin = ds.Tables[3].Rows[i]["Origin"].ToString();
                //                DCOSpecRecords[i].AgentCode = ds.Tables[3].Rows[i]["AgentCode"].ToString();
                //                if (ds.Tables[3].Rows[i]["DCMNumber"].ToString().Length > 6)
                //                {
                //                    DCOSpecRecords[i].DCMNo = ds.Tables[3].Rows[i]["DCMNumber"].ToString().Substring(0, 6);
                //                }
                //                else
                //                    DCOSpecRecords[i].DCMNo = ds.Tables[3].Rows[i]["DCMNumber"].ToString();
                //                DCOSpecRecords[i].CurrencyCode = ds.Tables[3].Rows[i]["Currency"].ToString();
                //                DCOSpecRecords[i].RateOfExchange = ds.Tables[3].Rows[i]["RateOfExchange"].ToString();
                //                DCOSpecRecords[i].DateAWBExecution = ds.Tables[3].Rows[i]["ExecutionDate"].ToString();
                //                DCOSpecRecords[i].PPCCIndicator = ds.Tables[3].Rows[i]["PayMode"].ToString();
                //                DCOSpecRecords[i].ChargeWeight = ds.Tables[3].Rows[i]["ChargedWeight"].ToString();
                //                DCOSpecRecords[i].ValuationCharge = ds.Tables[3].Rows[i]["ValCharge"].ToString();
                //                DCOSpecRecords[i].Taxes = ds.Tables[3].Rows[i]["Tax"].ToString();
                //                DCOSpecRecords[i].ChargeDueAgent = ds.Tables[3].Rows[i]["OCDueAgent"].ToString();
                //                DCOSpecRecords[i].ChargeDueCar = ds.Tables[3].Rows[i]["OCDueCar"].ToString();
                //                DCOSpecRecords[i].VATOnAWBCharges = ds.Tables[3].Rows[i]["VATAWBCharges"].ToString();
                //                DCOSpecRecords[i].Commission = ds.Tables[3].Rows[i]["Commission"].ToString();
                //                DCOSpecRecords[i].VATOnCommission = ds.Tables[3].Rows[i]["VATAWBCommission"].ToString();
                //                DCOSpecRecords[i].Discount = ds.Tables[3].Rows[i]["Discount"].ToString();
                //                DCOSpecRecords[i].DiscountIndicator = ds.Tables[3].Rows[i]["DiscountIndicator"].ToString();
                //                DCOSpecRecords[i].WeightIndicator = ds.Tables[3].Rows[i]["WeightIndicator"].ToString();
                //                DCOSpecRecords[i].GrossWeight = ds.Tables[3].Rows[i]["GrossWeight"].ToString();
                //                DCOSpecRecords[i].Destination = ds.Tables[3].Rows[i]["Destination"].ToString();
                //                DCOSpecRecords[i].ReservedSpace = "";
                //                DCOSpecRecords[i].ReasonAdjustment = "";


                //            }





                //            #endregion

                //            #region Trailer Record

                //            TrailerRecord TrailerRecords = new TrailerRecord();
                //            TrailerRecords.RecordID = "TTT";
                //            TrailerRecords.CASSAreaCode = "IN";
                //            TrailerRecords.AirlinePrefix = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString();
                //            TrailerRecords.BranchOfficeIndicator = "A";
                //            TrailerRecords.Filler = "";
                //            TrailerRecords.NoOfAWB = (ds.Tables[1].Rows.Count).ToString();
                //            TrailerRecords.NoOfCCA = "";
                //            TrailerRecords.NoOfDCM = (ds.Tables[2].Rows.Count + ds.Tables[3].Rows.Count).ToString();
                //            TrailerRecords.TotalRecords = (ds.Tables[1].Rows.Count + ds.Tables[2].Rows.Count + ds.Tables[3].Rows.Count).ToString();
                //            TrailerRecords.HashTotal = "";
                //            TrailerRecords.NoOfAW1 = "";
                //            TrailerRecords.NoOfCC3 = "";
                //            TrailerRecords.NoOfDC3 = "";
                //            TrailerRecords.ReservedSpace1 = "";
                //            TrailerRecords.ReservedSpace2 = "";

                //            #endregion

                //            #region Writing Records to the ASCII File
                //            StringBuilder sb = new StringBuilder();
                //            //sw.WriteLine(HeaderRecords.ToString());
                //            sb.AppendLine(HeaderRecords.ToString());

                //            foreach (AWBRecord file in AWBRecords)
                //            {
                //                //sw.WriteLine(file.ToString());
                //                sb.AppendLine(file.ToString());
                //            }

                //            foreach (DCRDCORecord file in DCRSpecRecords)
                //            {
                //                //sw.WriteLine(file.ToString());
                //                sb.AppendLine(file.ToString());
                //            }

                //            foreach (DCRDCORecord file in DCOSpecRecords)
                //            {
                //                //sw.WriteLine(file.ToString());
                //                sb.AppendLine(file.ToString());
                //            }

                //            // sw.WriteLine(TrailerRecords.ToString());
                //            sb.AppendLine(TrailerRecords.ToString());



                //            //sw.Flush();
                //            //sw.Close();
                //            #endregion

                //            Response.Clear();
                //            Response.ContentType = "application/txt";
                //            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                //            Response.Write(sb.ToString());
                //            Response.End();


                //            lblStatus.Text = "CASS Billing Participant Export generated successfully!";
                //            lblStatus.ForeColor = Color.Green;

                //        }
                //        else
                //        {
                //            lblStatus.Text = "No Records Found for CASS generation!";
                //            lblStatus.ForeColor = Color.Red;
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        lblStatus.Text = "No Records Found for CASS generation!";
                //        lblStatus.ForeColor = Color.Red;
                //        return;
                //    }

                //}
                //#endregion
                #endregion

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;

            }

        }
        #endregion

        #region SSIM Export
        public void  GetSSIMData(string FilePath)
        {
            try
            {
                string[] DesignationCode = new string[0];
                string[] AirlinePrefix = new string[0];
                string[] ArrivalTimeZone = new string[0];
                string[] TailNo = new string[0];
                string[] Constant = new string[0];
                string[] Origin = new string[0];
                string[] Destination = new string[0];
                string[] AirCraftType = new string[0];
                string[] AirlineFrequency = new string[0];
                string[] DepartureTimeZone = new string[0];
                string[] Arrival = new string[0];
                string[] Departure = new string[0];
                string[] FilghtID = new string[0];
                string[] FlightNo = new string[0];
                string[] FromDate = new string[0];
                string[] ToDate = new string[0];
                string[] RowID = new string[0];
                string[] Frequency = new string[0];
                string[] FinalFrequency = new string[0];
                string[] AirFrequency = new string[0];
                string[] AirSchMasterFlightID = new string[0];
                string[] AirSchMasterOrigin = new string[0];
                string[] AirSchMasterDest = new string[0];
                string[] AirSchMasterArrivalTime = new string[0];
                string[] AirSchMasterDepartureTime = new string[0];
                string[] AirSchDeptTimeZone = new string[0];
                string[] AirSchArrTimeZone = new string[0];
                string[] AirSchFromDate = new string[0];
                string[] AirSchToDate = new string[0];
                string[] AirSchTailNo = new string[0];
                string[] AirSchAirCraftType = new string[0];
                string[] AirSchFrequency = new string[0];
                string[] AirSchAirlinePrefix = new string[0];

                if (File.Exists(Server.MapPath(FilePath+".ssim")))
                {
                    foreach (string s in File.ReadAllLines(Server.MapPath(FilePath + ".ssim")))
                    {
                        if (s.StartsWith("3"))
                        {
                            Array.Resize(ref RowID, RowID.Length + 1);
                            RowID[RowID.Length - 1] = s.Substring(0, 1);
                            Array.Resize(ref Constant, Constant.Length + 1);
                            Constant[Constant.Length - 1] = s.Substring(6, 8);
                            Array.Resize(ref FromDate, FromDate.Length + 1);
                            FromDate[FromDate.Length - 1] = s.Substring(14, 7);
                            FromDate[FromDate.Length - 1] = DateTime.ParseExact(FromDate[FromDate.Length - 1], "ddMMMyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None).ToString("MM/dd/yyyy");
                            Array.Resize(ref ToDate, ToDate.Length + 1);
                            ToDate[ToDate.Length - 1] = s.Substring(21, 7);
                            ToDate[ToDate.Length - 1] = DateTime.ParseExact(ToDate[ToDate.Length - 1], "ddMMMyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None).ToString("MM/dd/yyyy");
                            Array.Resize(ref DesignationCode, DesignationCode.Length + 1);
                            DesignationCode[DesignationCode.Length - 1] = s.Substring(2, 2);
                            Array.Resize(ref AirlineFrequency, AirlineFrequency.Length + 1);
                            AirlineFrequency[AirlineFrequency.Length - 1] = s.Substring(28, 7);
                            Array.Resize(ref Origin, Origin.Length + 1);
                            Origin[Origin.Length - 1] = s.Substring(36, 3);
                            Array.Resize(ref Departure, Departure.Length + 1);
                            Departure[Departure.Length - 1] = s.Substring(39, 4);
                            Array.Resize(ref DepartureTimeZone, DepartureTimeZone.Length + 1);
                            DepartureTimeZone[DepartureTimeZone.Length - 1] = s.Substring(43, 9);
                            Array.Resize(ref Arrival, Arrival.Length + 1);
                            Array.Resize(ref Destination, Destination.Length + 1);
                            Destination[Destination.Length - 1] = s.Substring(54, 3);
                            Arrival[Arrival.Length - 1] = s.Substring(57, 4);
                            Array.Resize(ref ArrivalTimeZone, ArrivalTimeZone.Length + 1);
                            ArrivalTimeZone[ArrivalTimeZone.Length - 1] = s.Substring(61, 9);
                            Array.Resize(ref AirCraftType, AirCraftType.Length + 1);
                            AirCraftType[AirCraftType.Length - 1] = s.Substring(72, 3);
                            Array.Resize(ref AirlinePrefix, AirlinePrefix.Length + 1);
                            AirlinePrefix[AirlinePrefix.Length - 1] = s.Substring(137, 2);
                            Array.Resize(ref FlightNo, FlightNo.Length + 1);
                            FlightNo[FlightNo.Length - 1] = s.Substring(140, 4);
                            Array.Resize(ref TailNo, TailNo.Length + 1);
                            TailNo[TailNo.Length - 1] = s.Substring(172, 13);
                            Array.Resize(ref FilghtID, FilghtID.Length + 1);
                            FilghtID[FilghtID.Length - 1] = s.Substring(137, 2) + s.Substring(140, 4);


                        }

                    }
                }
                for (int i = 0; i < AirlineFrequency.Length; i++)
                {
                    string zero = "";
                    if (AirlineFrequency[i].Contains<char>(' '))
                    {
                        zero = AirlineFrequency[i].Replace(" ", "0");
                    }
                    else
                        zero = AirlineFrequency[i];
                    Array.Resize(ref Frequency, Frequency.Length + 1);
                    Frequency[Frequency.Length - 1] = zero;
                }
                for (int i = 0; i < Frequency.Length; i++)
                {
                    char[] chararr = new char[0];

                    foreach (char ch in Frequency[i])
                    {


                        if (ch == '2' || ch == '3' || ch == '4' || ch == '5' || ch == '6' || ch == '7')
                        {
                            Array.Resize(ref chararr, chararr.Length + 1);
                            chararr[chararr.Length - 1] = '1';

                        }
                        else
                        {
                            Array.Resize(ref chararr, chararr.Length + 1);
                            chararr[chararr.Length - 1] = ch;
                        }


                    }
                    Array.Resize(ref FinalFrequency, FinalFrequency.Length + 1);
                    FinalFrequency[FinalFrequency.Length - 1] = new string(chararr);


                }
                char[] finalchar = new char[0];
                for (int j = 0; j < FinalFrequency.Length; j++)
                {

                    FinalFrequency[j] = FinalFrequency[j].Insert(1, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(3, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(5, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(7, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(9, ",");
                    FinalFrequency[j] = FinalFrequency[j].Insert(11, ",");

                }

                #region Commented for Airline Schedule
                //for (int i = FilghtID.Length - 1; i > 0; i--)
                //{
                //    if (FilghtID[i].Contains("      "))
                //    {
                //        if (i < FilghtID.Length - 1)
                //        {
                //            FilghtID[i] = FilghtID[i + 1];
                //        }
                //    }
                //}
                //for (int i = 0; i < FilghtID.Length; i++)
                //{


                //    if (i==0 && FlightNo[i] != "    ")
                //    {
                //        Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //        AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //        Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //        AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //        Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //        AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //        Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //        AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //        Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //        AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //        Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //        AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];
                //        Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //        AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //        Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //        AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //        Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //        AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //        Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //        AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //        Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //        AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //        Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //        AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //        Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //        AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];
 
                //    }
                //    if (i != 0 && i < FilghtID.Length - 1)
                //    {
                //        if ((FlightNo[i] != "    "))
                //        {
                //            Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //            AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //            Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //            AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //            Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //            AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //            Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //            AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //            Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //            AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //            Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //            AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];


                //        }

                //        if (FlightNo[i] == "    " && (FilghtID[i] == FilghtID[i + 1]) && (Frequency[i] != Frequency[i + 1] || Destination[i] != Origin[i + 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];



                //        }
                //        if (FlightNo[i] == "    " && (FilghtID[i] == FilghtID[i - 1]) && (Frequency[i] != Frequency[i - 1] || Origin[i] != Destination[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];


                //        }
                //        if ((FlightNo[i] == "    ") && (FilghtID[i] != FilghtID[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];

                //        }

                //        if ((FlightNo[i] != "    ") && (FilghtID[i] != FilghtID[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];


                //        }
                //        if ((FlightNo[i] != "    ") && (FlightNo[i - 1] != "    ") && (FilghtID[i] == FilghtID[i - 1]))
                //        {
                //            Array.Resize(ref AirSchMasterOrigin, AirSchMasterOrigin.Length + 1);
                //            AirSchMasterOrigin[AirSchMasterOrigin.Length - 1] = Origin[i];
                //            Array.Resize(ref AirSchFromDate, AirSchFromDate.Length + 1);
                //            AirSchFromDate[AirSchFromDate.Length - 1] = FromDate[i];
                //            Array.Resize(ref AirSchTailNo, AirSchTailNo.Length + 1);
                //            AirSchTailNo[AirSchTailNo.Length - 1] = TailNo[i];
                //            Array.Resize(ref AirSchFrequency, AirSchFrequency.Length + 1);
                //            AirSchFrequency[AirSchFrequency.Length - 1] = FinalFrequency[i];
                //            Array.Resize(ref AirSchMasterDepartureTime, AirSchMasterDepartureTime.Length + 1);
                //            AirSchMasterDepartureTime[AirSchMasterDepartureTime.Length - 1] = Departure[i];
                //            Array.Resize(ref AirSchDeptTimeZone, AirSchDeptTimeZone.Length + 1);
                //            AirSchDeptTimeZone[AirSchDeptTimeZone.Length - 1] = DepartureTimeZone[i];
                //            Array.Resize(ref AirSchAirCraftType, AirSchAirCraftType.Length + 1);
                //            AirSchAirCraftType[AirSchAirCraftType.Length - 1] = AirCraftType[i];


                //        }

                //        if ((FlightNo[i] == "    ") && (Frequency[i] != Frequency[i + 1] && FilghtID[i] == FilghtID[i + 1]))
                //        {
                //            Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //            AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //            Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //            AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //            Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //            AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //            Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //            AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //            Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //            AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //            Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //            AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];

                //        }


                //    }
                //    if ((FilghtID.Length == i + 1) && FlightNo[i] != "    ")
                //    {
                //        Array.Resize(ref AirSchMasterDest, AirSchMasterDest.Length + 1);
                //        AirSchMasterDest[AirSchMasterDest.Length - 1] = Destination[i];
                //        Array.Resize(ref AirSchMasterArrivalTime, AirSchMasterArrivalTime.Length + 1);
                //        AirSchMasterArrivalTime[AirSchMasterArrivalTime.Length - 1] = Arrival[i];
                //        Array.Resize(ref AirSchArrTimeZone, AirSchArrTimeZone.Length + 1);
                //        AirSchArrTimeZone[AirSchArrTimeZone.Length - 1] = ArrivalTimeZone[i];
                //        Array.Resize(ref AirSchMasterFlightID, AirSchMasterFlightID.Length + 1);
                //        AirSchMasterFlightID[AirSchMasterFlightID.Length - 1] = FilghtID[i];
                //        Array.Resize(ref AirSchToDate, AirSchToDate.Length + 1);
                //        AirSchToDate[AirSchToDate.Length - 1] = ToDate[i];
                //        Array.Resize(ref AirSchAirlinePrefix, AirSchAirlinePrefix.Length + 1);
                //        AirSchAirlinePrefix[AirSchAirlinePrefix.Length - 1] = AirlinePrefix[i];

                //    }
                //}
                #endregion


                #region Preparing Parameters for SSIM Export to Update Airline Schedule

                //string[] QueryNames = new string[12];
                //object[] QueryValues = new object[12];
                //SqlDbType[] QueryTypes = new SqlDbType[12];
                //string[] Names = new string[12];
                //object[] Values = new object[12];
                //SqlDbType[] Types = new SqlDbType[12];

                //for (int i = 0; i < AirSchMasterFlightID.Length; i++)
                //{
                //    QueryNames[0] = "FromDate";
                //    QueryNames[1] = "ToDate";
                //    QueryNames[2] = "FlightID";
                //    QueryNames[3] = "Source";
                //    QueryNames[4] = "Dest";
                //    QueryNames[5] = "ScheduleDepttime";
                //    QueryNames[6] = "SchArrtime";
                //    QueryNames[7] = "frequency";
                //    QueryNames[8] = "EquipmentNo";
                //    QueryNames[9] = "ArrTimeZone";
                //    QueryNames[10] = "DeptTimeZone";
                //    QueryNames[11] = "FlightPrefix";


                //    QueryValues[0] = AirSchFromDate[i];
                //    QueryValues[1] = AirSchToDate[i];
                //    QueryValues[2] = AirSchMasterFlightID[i];
                //    QueryValues[3] = AirSchMasterOrigin[i];
                //    QueryValues[4] = AirSchMasterDest[i];
                //    QueryValues[5] = AirSchMasterDepartureTime[i];
                //    QueryValues[6] = AirSchMasterArrivalTime[i];
                //    QueryValues[7] = AirSchFrequency[i];
                //    QueryValues[8] = AirSchTailNo[i];
                //    QueryValues[9] = AirSchArrTimeZone[i];
                //    QueryValues[10] = AirSchDeptTimeZone[i];
                //    QueryValues[11] = AirSchAirlinePrefix[i];
                  


                //    QueryTypes[0] = SqlDbType.DateTime;
                //    QueryTypes[1] = SqlDbType.DateTime;
                //    QueryTypes[2] = SqlDbType.VarChar;
                //    QueryTypes[3] = SqlDbType.VarChar;
                //    QueryTypes[4] = SqlDbType.VarChar;
                //    QueryTypes[5] = SqlDbType.VarChar;
                //    QueryTypes[6] = SqlDbType.VarChar;
                //    QueryTypes[7] = SqlDbType.VarChar;
                //    QueryTypes[8] = SqlDbType.VarChar;
                //    QueryTypes[9] = SqlDbType.VarChar;
                //    QueryTypes[10] = SqlDbType.VarChar;
                //    QueryTypes[11] = SqlDbType.VarChar;

                //    if (db.InsertData("spSaveAirlineSchedule_SSIM", QueryNames, QueryTypes, QueryValues))
                //    {
                //        lblStatus.Text = "Schedule Uploaded Successfully!";
                //        lblStatus.ForeColor = Color.Green;
                //    }
                //    else
                //    {
                //        lblStatus.Text = "Schedule Updation Failed!";
                //        lblStatus.ForeColor = Color.Red;
                //    }

                //}
                //for (int i = 0; i < RowID.Length; i++)
                //{
                //    Names[0] = "FromDate";
                //    Names[1] = "ToDate";
                //    Names[2] = "FlightID";
                //    Names[3] = "Source";
                //    Names[4] = "Dest";
                //    Names[5] = "ScheduleDepttime";
                //    Names[6] = "SchArrtime";
                //    Names[7] = "frequency";
                //    Names[8] = "ArrTimeZone";
                //    Names[9] = "DeptTimeZone";
                //    Names[10] = "AirCraftType";
                //    Names[11] = "FlightPrefix";
                //    //Names[12] = "AirSchSource";
                //    //Names[13] = "AirSchDest";
                   


                //    Values[0] = FromDate[i];
                //    Values[1] = ToDate[i];
                //    Values[2] = FilghtID[i];
                //    Values[3] = Origin[i];
                //    Values[4] = Destination[i];
                //    Values[5] = Departure[i];
                //    Values[6] = Arrival[i];
                //    Values[7] = FinalFrequency[i];
                //    Values[8] = ArrivalTimeZone[i];
                //    Values[9] = DepartureTimeZone[i];
                //    Values[10] = AirCraftType[i];
                //    Values[11] = AirlinePrefix[i];
                //    //for (int j = 0; j < AirSchMasterFlightID.Length; j++)
                //    //{
                        
                //    //        if (FilghtID[i] == AirSchMasterFlightID[j] && FromDate[i] == AirSchFromDate[j] && ToDate[i] == AirSchToDate[j] && FinalFrequency[i] == AirSchFrequency[j])// && Arrival[i] == AirSchMasterArrivalTime[j] && ArrivalTimeZone[i] == AirSchArrTimeZone[j])
                //    //        {
                //    //            Values[12] = AirSchMasterOrigin[j];
                //    //            Values[13] = AirSchMasterDest[j];
                //    //        }
                        

                //    //}

                //    Types[0] = SqlDbType.DateTime;
                //    Types[1] = SqlDbType.DateTime;
                //    Types[2] = SqlDbType.VarChar;
                //    Types[3] = SqlDbType.VarChar;
                //    Types[4] = SqlDbType.VarChar;
                //    Types[5] = SqlDbType.VarChar;
                //    Types[6] = SqlDbType.VarChar;
                //    Types[7] = SqlDbType.VarChar;
                //    Types[8] = SqlDbType.VarChar;
                //    Types[9] = SqlDbType.VarChar;
                //    Types[10] = SqlDbType.VarChar;
                //    Types[11] = SqlDbType.VarChar;
                //    //Types[12] = SqlDbType.VarChar;
                //    //Types[13] = SqlDbType.VarChar;

                //    if (db.InsertData("spSaveAirlinerouteDetails_SSIM", Names, Types, Values))
                //    {
                //        lblStatus.Text = "Schedule Updated Successfully!";
                //        lblStatus.ForeColor = Color.Green;
                //    }
                //    else
                //    {

                //        lblStatus.Text = "Schedule Updation Failed!";
                //        lblStatus.ForeColor = Color.Red;
                //        return;
                //    }

                //}



                #endregion


                #region Preparing Parameters for SSIM Export to Update Airline Schedule

                string[] QueryNames = new string[12];
                object[] QueryValues = new object[12];
                SqlDbType[] QueryTypes = new SqlDbType[12];
                string[] Names = new string[12];
                object[] Values = new object[12];
                SqlDbType[] Types = new SqlDbType[12];

                for (int i = 0; i < FilghtID.Length; i++)
                {
                    QueryNames[0] = "FromDate";
                    QueryNames[1] = "ToDate";
                    QueryNames[2] = "FlightID";
                    QueryNames[3] = "Source";
                    QueryNames[4] = "Dest";
                    QueryNames[5] = "ScheduleDepttime";
                    QueryNames[6] = "SchArrtime";
                    QueryNames[7] = "frequency";
                    QueryNames[8] = "EquipmentNo";
                    QueryNames[9] = "ArrTimeZone";
                    QueryNames[10] = "DeptTimeZone";
                    QueryNames[11] = "FlightPrefix";


                    QueryValues[0] = FromDate[i];
                    QueryValues[1] = ToDate[i];
                    QueryValues[2] = FilghtID[i];
                    QueryValues[3] = Origin[i];
                    QueryValues[4] = Destination[i];
                    QueryValues[5] = Departure[i];
                    QueryValues[6] = Arrival[i];
                    QueryValues[7] = FinalFrequency[i];
                    QueryValues[8] = TailNo[i];
                    QueryValues[9] = ArrivalTimeZone[i];
                    QueryValues[10] = DepartureTimeZone[i];
                    QueryValues[11] = AirlinePrefix[i];



                    QueryTypes[0] = SqlDbType.DateTime;
                    QueryTypes[1] = SqlDbType.DateTime;
                    QueryTypes[2] = SqlDbType.VarChar;
                    QueryTypes[3] = SqlDbType.VarChar;
                    QueryTypes[4] = SqlDbType.VarChar;
                    QueryTypes[5] = SqlDbType.VarChar;
                    QueryTypes[6] = SqlDbType.VarChar;
                    QueryTypes[7] = SqlDbType.VarChar;
                    QueryTypes[8] = SqlDbType.VarChar;
                    QueryTypes[9] = SqlDbType.VarChar;
                    QueryTypes[10] = SqlDbType.VarChar;
                    QueryTypes[11] = SqlDbType.VarChar;

                    if (db.InsertData("spSaveAirlineSchedule_SSIM", QueryNames, QueryTypes, QueryValues))
                    {
                        lblStatus.Text = "Schedule Uploaded Successfully!";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Schedule Updation Failed!";
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                for (int i = 0; i < RowID.Length; i++)
                {
                    Names[0] = "FromDate";
                    Names[1] = "ToDate";
                    Names[2] = "FlightID";
                    Names[3] = "Source";
                    Names[4] = "Dest";
                    Names[5] = "ScheduleDepttime";
                    Names[6] = "SchArrtime";
                    Names[7] = "frequency";
                    Names[8] = "ArrTimeZone";
                    Names[9] = "DeptTimeZone";
                    Names[10] = "AirCraftType";
                    Names[11] = "FlightPrefix";
                    //Names[12] = "AirSchSource";
                    //Names[13] = "AirSchDest";



                    Values[0] = FromDate[i];
                    Values[1] = ToDate[i];
                    Values[2] = FilghtID[i];
                    Values[3] = Origin[i];
                    Values[4] = Destination[i];
                    Values[5] = Departure[i];
                    Values[6] = Arrival[i];
                    Values[7] = FinalFrequency[i];
                    Values[8] = ArrivalTimeZone[i];
                    Values[9] = DepartureTimeZone[i];
                    Values[10] = AirCraftType[i];
                    Values[11] = AirlinePrefix[i];
                    //for (int j = 0; j < AirSchMasterFlightID.Length; j++)
                    //{

                    //        if (FilghtID[i] == AirSchMasterFlightID[j] && FromDate[i] == AirSchFromDate[j] && ToDate[i] == AirSchToDate[j] && FinalFrequency[i] == AirSchFrequency[j])// && Arrival[i] == AirSchMasterArrivalTime[j] && ArrivalTimeZone[i] == AirSchArrTimeZone[j])
                    //        {
                    //            Values[12] = AirSchMasterOrigin[j];
                    //            Values[13] = AirSchMasterDest[j];
                    //        }


                    //}

                    Types[0] = SqlDbType.DateTime;
                    Types[1] = SqlDbType.DateTime;
                    Types[2] = SqlDbType.VarChar;
                    Types[3] = SqlDbType.VarChar;
                    Types[4] = SqlDbType.VarChar;
                    Types[5] = SqlDbType.VarChar;
                    Types[6] = SqlDbType.VarChar;
                    Types[7] = SqlDbType.VarChar;
                    Types[8] = SqlDbType.VarChar;
                    Types[9] = SqlDbType.VarChar;
                    Types[10] = SqlDbType.VarChar;
                    Types[11] = SqlDbType.VarChar;
                    //Types[12] = SqlDbType.VarChar;
                    //Types[13] = SqlDbType.VarChar;

                    if (db.InsertData("spSaveAirlinerouteDetails_SSIM", Names, Types, Values))
                    {
                        lblStatus.Text = "Schedule Updated Successfully!";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {

                        lblStatus.Text = "Schedule Updation Failed!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                }



                #endregion

            }
            catch (Exception ex)
            { }
            
        }
        #endregion

        protected void btnSSIM_Click(object sender, EventArgs e)
        {
            try
            {
                #region Calling SSIM
                string path = ConfigurationManager.AppSettings["CASSPath"].ToString();
                string filename = ConfigurationManager.AppSettings["SSIMFileName"].ToString();
                string filePath = Path.Combine(path, filename);
                GetSSIMData(filePath);
                #endregion
            }
            catch (Exception ex)
            { }
        }

        #region CASS HandOffFile Export
        protected void btnCASSHandOffFile_Click(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                StringBuilder sbuild = new StringBuilder();

                string[] QueryPname = new string[8];
                object[] QueryValue = new object[8];
                SqlDbType[] QueryType = new SqlDbType[8];


                QueryPname[0] = "InvoiceType";
                QueryPname[1] = "AgentCode";
                QueryPname[2] = "BillingType";
                QueryPname[3] = "Origin";
                QueryPname[4] = "FromDate";
                QueryPname[5] = "ToDate";
                QueryPname[6] = "InvoiceNumber";
                QueryPname[7] = "AWBNumber";


                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.DateTime;
                QueryType[5] = SqlDbType.DateTime;
                QueryType[6] = SqlDbType.VarChar;
                QueryType[7] = SqlDbType.VarChar;

                QueryValue[0] = ddlInvoiceType.SelectedValue;
                QueryValue[1] = ddlAgentCode.SelectedValue;
                QueryValue[2] = ddlBillType.SelectedValue;
                QueryValue[3] = txtOrigin.Text;
                QueryValue[4] = DateTime.ParseExact(txtInvoiceFrom.Text.Trim(), "dd/MM/yyyy", null);
                QueryValue[5] = DateTime.ParseExact(txtInvoiceTo.Text.Trim(), "dd/MM/yyyy", null);
                QueryValue[6] = txtInvoiceNumber.Text;
                QueryValue[7] = txtAWBNumber.Text;

                string InvoiceNo = txtInvoiceNumber.Text;
                int count = 0;

                for (int j = 0; j < grdInvoiceList.Rows.Count; j++)
                {
                    if (((CheckBox)(grdInvoiceList.Rows[j].FindControl("ChkSelect"))).Checked)
                    {

                        count++;
                        if (count == 1)
                            InvoiceNo = "$" + ((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                        else
                            InvoiceNo += "$,$" + ((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                    }
                }
                if (count > 0)
                {
                    InvoiceNo = InvoiceNo + "$";
                }

                QueryValue[6] = InvoiceNo;

                DataSet ds = db.SelectRecords("SP_GetCASSData", QueryPname, QueryValue, QueryType);

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string Handfilename = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString() + "01" + "01." + ds.Tables[0].Rows[0]["CountryCode"].ToString() + "E.txt";
                        sbuild = EncodeHandOff.GettingHandOffFileCASSRecords(ds);

                        if (sbuild != null)
                        {

                            Response.Clear();
                            Response.ContentType = "application/txt";
                            Response.AddHeader("content-disposition", "attachment; filename=" + Handfilename);
                            Response.Write(sbuild.ToString());
                            Response.End();

                            lblStatus.Text = "CASS Hand Off Export File generated successfully!";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "CASS generation failed!";
                            lblStatus.ForeColor = Color.Red;
                            return;

                        }


                    }
                    else
                    {
                        lblStatus.Text = "No Records Found!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    lblStatus.Text = "No Records Found!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;

            }

        }
        #endregion

        #region CASS Standard Documents Export
        protected void btnCASSStandardDocs_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string[] QueryNames = new string[2];
                object[] QueryValues = new object[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];

                QueryNames[0] = "FromDate";
                QueryNames[1] = "ToDate";

                QueryTypes[0] = SqlDbType.DateTime;
                QueryTypes[1] = SqlDbType.DateTime;

                QueryValues[0] = DateTime.ParseExact(txtInvoiceFrom.Text.Trim(), "dd/MM/yyyy", null);
                QueryValues[1] = DateTime.ParseExact(txtInvoiceTo.Text.Trim(), "dd/MM/yyyy", null);

                dsDCM = db.SelectRecords("SP_GetCASSData",QueryNames,QueryValues,QueryTypes);
                if (dsDCM != null)
                {
                    if (dsDCM.Tables.Count > 0)
                    {
                        if (dsDCM.Tables[2].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsDCM.Tables[2].Rows.Count; i++)
                            {
                               
                                //DataRow dr = dsDCM.Tables[1].NewRow();
                                DataTable dt = new DataTable(); 
                                dt=dsDCM.Tables[2].Clone();
                                dt.ImportRow(dsDCM.Tables[2].Rows[i]);
                                dt.TableName = "tblDCM";
                                dset = new DataSet();
                                dset.Tables.Add(dt);

                                
                                ////DataDynamics.Reports.ReportDefinition _reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/CASS/DCM.rdlx")));
                                ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                                ////_reportRuntime.LocateDataSource += WARCustWise_LocateDataSource;
                                string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
                                System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                                System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                                settings.Add("hideToolbar", "True");
                                settings.Add("hideMenubar", "True");
                                settings.Add("hideWindowUI", "True");
                                settings.Add("MarginLeft", "0.1in");
                                settings.Add("MarginRight", "0.1in");
                                settings.Add("MarginTop", "0.1in");
                                settings.Add("MarginBottom", "0.1in");
                                settings.Add("PageWidth", "9in");
                                settings.Add("PageHeight", "13in");
                                settings.Add("FitWindow", "True");


                                ////PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                                ////FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                                ////_reportRuntime.Render(_renderingExtension, _provider, settings);
                                Response.Clear();
                                Response.ContentType = "application/pdf";
                                string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                                string FullName = filename.Replace(" ", "");
                                Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                                Response.BinaryWrite(File.ReadAllBytes(exportFile));
                                myFile.Delete();
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Setting Data to the DataDynamic PDF
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName;

        //        e.Data = dset;
        //}
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/FrmInvoiceListing.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click


        #region Button Export to ERP
        protected void btnExportERP_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                string InvoiceNo = string.Empty;
                Session["ExportERP"] = null;
                int count = 0;
                for (int j = 0; j < grdInvoiceList.Rows.Count; j++)
                {
                    if (((CheckBox)(grdInvoiceList.Rows[j].FindControl("ChkSelect"))).Checked)
                    {

                        count++;
                        if (count == 1)
                            InvoiceNo = ((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                        else
                            InvoiceNo += "," + ((Label)(grdInvoiceList.Rows[j].FindControl("lblInvoiceNumber"))).Text;
                    }
                }
                //if (count == 0)
                //{
                //    lblStatus.Text = "No Records Selected!";
                //    lblStatus.ForeColor = Color.Blue;
                //    return;
                //}
                DateTime dtFrm = new DateTime();
                DateTime dtTo = new DateTime();
                
                if (InvoiceNo.Trim() == "")
                {
                    lblStatus.Text = "";
                    if (txtInvoiceFrom.Text == "")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Please select Valid date";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    try
                    {
                        //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                        //Change 03082012
                        string day = txtInvoiceFrom.Text.Substring(0, 2);
                        string mon = txtInvoiceFrom.Text.Substring(3, 2);
                        string yr = txtInvoiceFrom.Text.Substring(6, 4);
                        string strfromdate = yr + "-" + mon + "-" + day;
                        dtFrm = Convert.ToDateTime(strfromdate);
                    }
                    catch 
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Date format invalid";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    //Validation for To date
                    if (txtInvoiceTo.Text == "")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Please select Valid date";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    try
                    {
                        //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                        //Change 03082012
                        string day = txtInvoiceTo.Text.Substring(0, 2);
                        string mon = txtInvoiceTo.Text.Substring(3, 2);
                        string yr = txtInvoiceTo.Text.Substring(6, 4);
                        string strtodate = yr + "-" + mon + "-" + day;
                        dtTo = Convert.ToDateTime(strtodate);
                    }
                    catch 
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Date format invalid";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (dtTo < dtFrm)
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "To date should be greater than From date";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }

                string[] paramname = new string[4];
                paramname[0] = "InvoiceNumber";
                paramname[1] = "InvoiceType";
                paramname[2] = "FromDate";
                paramname[3] = "ToDate";
                
                object[] paramvalue = new object[4];
                paramvalue[0] = InvoiceNo;
                paramvalue[1] = InvoiceNo;
                paramvalue[2] = (InvoiceNo == "") ? dtFrm.ToString("yyyy/MM/dd") : "";
                paramvalue[3] = (InvoiceNo == "") ? dtTo.ToString("yyyy/MM/dd") : "";

                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;

                DataSet dResult = objBAL.getExportERPData(paramname, paramvalue, paramtype);
                if (dResult != null)
                {
                    string attachment = "attachment; filename=ExportERP.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dResult.Tables[0].Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dResult.Tables[0].Rows)
                    {
                        tab = "";
                        for (i = 0; i < dResult.Tables[0].Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();
                }

            }
            catch 
            { }
        }
        #endregion

        protected void btnCloseInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dsInvoice = (DataSet)Session["dsInvoice"];
                string InvoiceClose = "", res = "";

                //Close invoice which are checked and with status "Open"
                for (int j = 0; j < grdInvoiceList.Rows.Count; j++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[j].FindControl("ChkSelect")).Checked && ((Label)grdInvoiceList.Rows[j].FindControl("lblInvoiceStatus")).Text == "Open")
                    {

                        #region Prepare Parameters
                        object[] RateCardInfo = new object[1];
                        int irow = 0;

                        RateCardInfo.SetValue(dsInvoice.Tables[0].Rows[j + (grdInvoiceList.PageIndex * grdInvoiceList.PageSize)]["InvoiceNumber"].ToString(), irow);

                        #endregion Prepare Parameters

                        if (InvoiceClose == "")
                        {
                            InvoiceClose = InvoiceClose + dsInvoice.Tables[0].Rows[j + (grdInvoiceList.PageIndex * grdInvoiceList.PageSize)]["InvoiceNumber"].ToString();
                        }
                        else
                        {
                            InvoiceClose = InvoiceClose + "," + dsInvoice.Tables[0].Rows[j + (grdInvoiceList.PageIndex * grdInvoiceList.PageSize)]["InvoiceNumber"].ToString();
                        }

                        //GenerateAgentInvoice(RateCardInfo);

                    }
                }

                //Call SP to close invoice which are open
                if (InvoiceClose == "")
                {
                    lblStatus.Text = "Please select Invoices to be closed";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    res = objBAL.SetInvoiceStatustoClose(InvoiceClose);

                    if (res == "Invoices closed successfully.")
                    {
                        bindInvoiceList();
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Invoices not closed. Please try again.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
            }
            catch
            {
                lblStatus.Text = "Invoices not closed. Please try again.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            
        }

        protected void grdInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblGTotalInvoiceAmount = (Label)e.Row.FindControl("lblGTotalInvoiceAmount");
                Label lblGTotalCollectionAmount = (Label)e.Row.FindControl("lblGTotalCollectionAmount");
                Label lblGTotalInvoiceBalance = (Label)e.Row.FindControl("lblGTotalInvoiceBalance");

                lblGTotalInvoiceAmount.Text = Math.Round(GTotalInvoiceAmount, 2).ToString();
                lblGTotalCollectionAmount.Text = Math.Round(GTotalCollectionAmount, 2).ToString();
                lblGTotalInvoiceBalance.Text = Math.Round(GTotalInvoiceBalance, 2).ToString();
            }
        }

    }
}
