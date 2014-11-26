using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
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


namespace ProjectSmartCargoManager
{
    public partial class frmInterlineInvoiceListing : System.Web.UI.Page
    {
        DataSet dsResult;
        BAL.BALInvoiceListing objBAL = new BAL.BALInvoiceListing();
        BAL.BALInterlineInvoiceListing objIntBAL = new BAL.BALInterlineInvoiceListing();
        BAL.BillingInterlineInvoiceBAL objIntInvBAL = new BAL.BillingInterlineInvoiceBAL();
        BillingAWBFileInvoiceBAL objBillBAL = new BillingAWBFileInvoiceBAL();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DataSet dsDCM;
        DataSet dset;
        BillingCASSEncode EncodeBilling = new BillingCASSEncode();
        HandOffFileCASSEncode EncodeHandOff = new HandOffFileCASSEncode();
        DateTime dtCurrentDate = DateTime.Now;

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtCurrentDate = (DateTime)Session["IT"];
                if (!IsPostBack)
                {
                    txtInvoiceFrom.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                    txtInvoiceTo.Text = dtCurrentDate.ToString("dd/MM/yyyy");

                    LoadAgentDropdown(); //AgentName 
                    LoadAgentCodeDropdown(); //AgentCode
                    LoadCarrierDropdown();

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

        #region Load Carrier Dropdown
        public void LoadCarrierDropdown()
        {
            DataSet ds = objIntInvBAL.GetAllCarriers();
            if (ds != null)
            {
                ddlCarrier.DataSource = ds;
                ddlCarrier.DataMember = ds.Tables[0].TableName;
                ddlCarrier.DataTextField = "PartnerName";
                ddlCarrier.DataValueField = "PartnerCode";
                ddlCarrier.DataBind();
                ddlCarrier.Items.Insert(0, new ListItem("All", ""));
                ddlCarrier.SelectedIndex = -1;
            }
        }
        #endregion LoadCarrierDropdown

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
                string strfromdate, strtodate;
                //lblStatus.Visible = false;
                lblStatus.Text = "";

                //Validation for From date
                if (txtInvoiceFrom.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
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


                DataSet DSInvoicedata = objIntBAL.GetInterlineInvoiceList(ddlInvoiceType.SelectedValue, ddlAgentName.SelectedValue, ddlBillType.SelectedValue, txtOrigin.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), txtInvoiceNumber.Text.Trim(), txtAWBNumber.Text.Trim(), ddlCarrier.SelectedValue);

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    Session["dsInvoice"] = DSInvoicedata;
                    grdInvoiceList.DataSource = DSInvoicedata.Tables[0];
                    grdInvoiceList.DataBind();
                    grdInvoiceList.Visible = true;
                    btnPrintInvoice.Visible = true;
                    btnPrintInvoicePDF.Visible = true;
                    btnInvoiceSummary.Visible = true;
                    btnCASS.Visible = false;
                    btnPrint.Visible = true;
                    btnCASSHandOffFile.Visible = false;
                    btnIDEC.Visible = true;
                    hf1.Value = ddlAgentName.SelectedItem.Text;
                    hf2.Value = ddlBillType.SelectedItem.Text;
                    hf3.Value = txtInvoiceFrom.Text;
                    hf4.Value = txtInvoiceTo.Text;
                    if (txtOrigin.Text.Trim() == "")
                        hf5.Value = "-";
                    else
                        hf5.Value = txtOrigin.Text;

                }
                else
                {
                    grdInvoiceList.Visible = false;
                    btnPrint.Visible = false;
                    btnPrintInvoice.Visible = false;
                    btnPrintInvoicePDF.Visible = false;
                    btnInvoiceSummary.Visible = false;
                    btnCASS.Visible = false;
                    btnIDEC.Visible = false;
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

            if (ddlInvoiceType.SelectedValue == "1") //Final Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateRegularInvoices();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "2") //Final Credit Notes
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateRegularCreditNotes();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "3") //Proforma Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateProformaInvoices();", true);
            }
            else //Proforma Credit Notes
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateProformaCreditNotes();", true);
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

            if (ddlInvoiceType.SelectedValue == "1") //Final Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateRegularInvoicesPDF();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "2") //Final Credit Notes
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateRegularCreditNotesPDF();", true);
            }
            else if (ddlInvoiceType.SelectedValue == "3") //Proforma Invoice
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateProformaInvoicesPDF();", true);
            }
            else //Proforma Credit Notes
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateProformaCreditNotesPDF();", true);
            }
        }

        protected void btnInvoiceSummary_Click(object sender, EventArgs e)
        {
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
                                Session["BulkData"] = "";
                                DataTable DTBulkData = new DataTable();
                                DTBulkData = dsRes.Tables[0];
                                Session["BulkData"] = DTBulkData;

                                Response.Write("<script>");
                                Response.Write("window.open('ShowInvoiceSummary.aspx','_blank')");
                                Response.Write("</script>");
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

        #region Grid InvoiceList RowCommand
        protected void grdInvoiceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //if (e.CommandName == "IDEC")
                //{
                //    string InvoiceNo = e.CommandArgument.ToString();
                //    DataSet ds = db.SelectRecords("", "InvoiceNo", InvoiceNo, SqlDbType.VarChar);
                //    IDECEncode.FileHeaderRecord FileHeader = new IDECEncode.FileHeaderRecord();

                //    //FileHeader.StandardMessageIdentifier
                    
                //}
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion


        #region Button IDEC
        protected void btnIDEC_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string InvoiceNo = "";
                int count = 0;
                for (int i = 0; i < grdInvoiceList.Rows.Count; i++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[i].FindControl("ChkSelect")).Checked)
                    {
                        count++;
                        InvoiceNo = ((Label)grdInvoiceList.Rows[i].FindControl("lblInvoiceNumber")).Text;
                        break;
                        
                    }
                }
                if (count > 0)
                {
                    DataSet ds = db.SelectRecords("SP_GetIDECData", "InvoiceNo", InvoiceNo, SqlDbType.VarChar);
                    IDECEncode.FileHeaderRecord FileHeaderRecord = new IDECEncode.FileHeaderRecord();
                    IDECEncode.InvoiceHeaderRecord InvoiceHeaderRecord = new IDECEncode.InvoiceHeaderRecord();
                    IDECEncode.AWBRecord[] AWBRecord = new IDECEncode.AWBRecord[0];
                    IDECEncode.BillingCodeRecord BillingCodeRecord = new IDECEncode.BillingCodeRecord();
                    IDECEncode.InvoiceTotalRecord InvoiceTotalRecord = new IDECEncode.InvoiceTotalRecord();
                    IDECEncode.FileTotalRecord FileTotalRecord = new IDECEncode.FileTotalRecord();
                    IDECEncode.RefDataPart1 RefDataPart1 = new IDECEncode.RefDataPart1();
                    IDECEncode.RefDataPart2 RefDataPart2 = new IDECEncode.RefDataPart2();
                    IDECEncode.InvoiceFooterRecord InvoiceFooterRecord = new IDECEncode.InvoiceFooterRecord();
                    StringBuilder sb = new StringBuilder();
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FileHeaderRecord.StandardMessageIdentifier = ds.Tables[0].Rows[0]["SMI"].ToString();
                                FileHeaderRecord.RecordSequenceNo = ds.Tables[0].Rows[0]["RecordSequenceNo"].ToString();
                                FileHeaderRecord.StandardFieldIdentifier = ds.Tables[0].Rows[0]["SFI"].ToString();
                                FileHeaderRecord.AirlineCode = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
                                FileHeaderRecord.VersionNumber = ds.Tables[0].Rows[0]["VersionNo"].ToString();
                                FileHeaderRecord.Filler = "";
                                sb.AppendLine(FileHeaderRecord.ToString());
                                
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                InvoiceHeaderRecord.StandardMessageIdentifier = ds.Tables[1].Rows[0]["SMI"].ToString();
                                InvoiceHeaderRecord.RecordSequenceNo = ds.Tables[1].Rows[0]["RecordSequenceNo"].ToString();
                                InvoiceHeaderRecord.BillingAirline = ds.Tables[1].Rows[0]["BillingAirline"].ToString();
                                InvoiceHeaderRecord.BilledAirline = ds.Tables[1].Rows[0]["BilledAirline"].ToString();
                                InvoiceHeaderRecord.Filler1 = "";
                                InvoiceHeaderRecord.InvoiceNumber = ds.Tables[1].Rows[0]["InvoiceNumber"].ToString();
                                InvoiceHeaderRecord.BatchSequenceNo = ds.Tables[1].Rows[0]["BatchSequenceNo"].ToString();
                                InvoiceHeaderRecord.RecordSequenceNoBatch = ds.Tables[1].Rows[0]["SequenceWithinBatch"].ToString();
                                InvoiceHeaderRecord.BillingMonth = ds.Tables[1].Rows[0]["BillingMonth"].ToString();
                                InvoiceHeaderRecord.CurrencyListing = ds.Tables[1].Rows[0]["CurrencyListing"].ToString();
                                InvoiceHeaderRecord.CurrencyBilling = ds.Tables[1].Rows[0]["CurrencyBilling"].ToString();
                                InvoiceHeaderRecord.PeriodNumber = ds.Tables[1].Rows[0]["PeriodNo"].ToString();
                                InvoiceHeaderRecord.SettlementMethodIndicator = ds.Tables[1].Rows[0]["SettlementMethod"].ToString();
                                InvoiceHeaderRecord.DigitalSignatureFlag = ds.Tables[1].Rows[0]["DigitalSignature"].ToString();
                                InvoiceHeaderRecord.InvoiceDate = ds.Tables[1].Rows[0]["InvoiceDate"].ToString();
                                InvoiceHeaderRecord.ListingBillingRate = ds.Tables[1].Rows[0]["BillingRate"].ToString();
                                InvoiceHeaderRecord.SuspendedFlag = ds.Tables[1].Rows[0]["SuspendedFlag"].ToString();
                                InvoiceHeaderRecord.BillingAirlineLocID = ds.Tables[1].Rows[0]["BillingAirlineLocID"].ToString();
                                InvoiceHeaderRecord.BilledAirlineLocID = ds.Tables[1].Rows[0]["BilledAirlineLocID"].ToString();
                                InvoiceHeaderRecord.InvoiceType = ds.Tables[1].Rows[0]["InvoiceType"].ToString();
                                InvoiceHeaderRecord.InvoiceTemplateLanguage = ds.Tables[1].Rows[0]["Language"].ToString();
                                InvoiceHeaderRecord.Filler2 = "";
                                InvoiceHeaderRecord.Filler3 = "";
                                InvoiceHeaderRecord.Filler4 = "";
                                InvoiceHeaderRecord.Filler5 = "";
                                InvoiceHeaderRecord.Filler6 = "";
                                InvoiceHeaderRecord.Filler7 = "";
                                InvoiceHeaderRecord.Filler8 = "";
                                InvoiceHeaderRecord.Filler9 = "";
                                InvoiceHeaderRecord.Filler10 = "";

                                sb.AppendLine(InvoiceHeaderRecord.ToString());
                            }

                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                RefDataPart1.RecordSequenceNo = ds.Tables[2].Rows[0]["RecordSequenceNo"].ToString();
                                RefDataPart1.SFI = ds.Tables[2].Rows[0]["SFI"].ToString();
                                RefDataPart1.BillingAirline = ds.Tables[2].Rows[0]["BillingAirline"].ToString();
                                RefDataPart1.BilledAirline = ds.Tables[2].Rows[0]["BilledAirline"].ToString();
                                RefDataPart1.InvoiceNumber = ds.Tables[2].Rows[0]["InvoiceNumber"].ToString();
                                RefDataPart1.RecordSerialNo = ds.Tables[2].Rows[0]["RecordSerialNo"].ToString();
                                RefDataPart1.CompanyLegalName = ds.Tables[2].Rows[0]["LegalName"].ToString();
                                RefDataPart1.TaxRegistrationID = ds.Tables[2].Rows[0]["TaxRegistrationID"].ToString();
                                RefDataPart1.AdditionalTaxRegistrationID = ds.Tables[2].Rows[0]["AdditionalTaxRegID"].ToString();
                                RefDataPart1.CompanyRegistrationID = ds.Tables[2].Rows[0]["RegistrationID"].ToString();
                                RefDataPart1.AddressLine1 = ds.Tables[2].Rows[0]["Address1"].ToString();
                                RefDataPart1.AddressLine2 = ds.Tables[2].Rows[0]["Address2"].ToString();
                                RefDataPart1.AddressLine3 = ds.Tables[2].Rows[0]["Address3"].ToString();
                                RefDataPart1.Filler1 = "";
                                RefDataPart1.Filler2 = "";
                                RefDataPart1.Filler3 = "";
                                

                                sb.AppendLine(RefDataPart1.ToString());
                            
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                RefDataPart2.RecordSequenceNo = ds.Tables[3].Rows[0]["RecordSequenceNo"].ToString();
                                RefDataPart2.SFI = ds.Tables[3].Rows[0]["SFI"].ToString();
                                RefDataPart2.BillingAirline = ds.Tables[3].Rows[0]["BillingAirline"].ToString();
                                RefDataPart2.BilledAirline = ds.Tables[3].Rows[0]["BilledAirline"].ToString();
                                RefDataPart2.InvoiceNumber = ds.Tables[3].Rows[0]["InvoiceNumber"].ToString();
                                RefDataPart2.RecordSerialNo = ds.Tables[3].Rows[0]["RecordSerialNo"].ToString();
                                RefDataPart2.CityName = ds.Tables[3].Rows[0]["City"].ToString();
                                RefDataPart2.SubDivisionCode = ds.Tables[3].Rows[0]["SubDivCode"].ToString();
                                RefDataPart2.SubDivisionName = ds.Tables[3].Rows[0]["SubDivName"].ToString();
                                RefDataPart2.CountryCode = ds.Tables[3].Rows[0]["CountryCode"].ToString();
                                RefDataPart2.CountryName = ds.Tables[3].Rows[0]["CountryName"].ToString();
                                RefDataPart2.PostalCode = ds.Tables[3].Rows[0]["PostalCode"].ToString();
                                RefDataPart2.Filler1 = "";
                                RefDataPart2.Filler2 = "";
                                RefDataPart2.Filler3 = "";

                                sb.AppendLine(RefDataPart2.ToString());

                            }

                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                                {
                                    Array.Resize(ref AWBRecord, AWBRecord.Length + 1);
                                    AWBRecord[AWBRecord.Length - 1] = new IDECEncode.AWBRecord();
                                    AWBRecord[AWBRecord.Length-1].RecordSequenceNo = ds.Tables[4].Rows[i]["RecordSequenceNo"].ToString();
                                    AWBRecord[AWBRecord.Length-1].BillingAirline = ds.Tables[4].Rows[i]["BillingAirline"].ToString();
                                    AWBRecord[AWBRecord.Length-1].BilledAirline = ds.Tables[4].Rows[i]["BilledAirline"].ToString();
                                    AWBRecord[AWBRecord.Length-1].BillingCode = ds.Tables[4].Rows[i]["PayMode"].ToString();
                                    AWBRecord[AWBRecord.Length-1].InvoiceNumber = ds.Tables[4].Rows[i]["InvoiceNumber"].ToString();
                                    AWBRecord[AWBRecord.Length-1].BatchSequenceNo = ds.Tables[4].Rows[i]["BatchSequenceNo"].ToString();
                                    AWBRecord[AWBRecord.Length-1].RecordSequenceNo = ds.Tables[4].Rows[i]["RecordSequenceWithinBatch"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AWBDate = ds.Tables[4].Rows[i]["AWBDate"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AWBIssuingAirline = ds.Tables[4].Rows[i]["AWBIssuingAirline"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AWBSerialNo = ds.Tables[4].Rows[i]["AWBNumber"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AWBCheckDigit = ds.Tables[4].Rows[i]["CheckDigit"].ToString();
                                    AWBRecord[AWBRecord.Length-1].Origin = ds.Tables[4].Rows[i]["Origin"].ToString();
                                    AWBRecord[AWBRecord.Length-1].Destination = ds.Tables[4].Rows[i]["Destination"].ToString();
                                    AWBRecord[AWBRecord.Length-1].From = ds.Tables[4].Rows[i]["From"].ToString();
                                    AWBRecord[AWBRecord.Length-1].To = ds.Tables[4].Rows[i]["To"].ToString();
                                    AWBRecord[AWBRecord.Length-1].DateOfCarriage = ds.Tables[4].Rows[i]["CarriageDate"].ToString();
                                    AWBRecord[AWBRecord.Length-1].WeightCharges = ds.Tables[4].Rows[i]["WeightCharges"].ToString();
                                    AWBRecord[AWBRecord.Length-1].OtherCharges = ds.Tables[4].Rows[i]["OtherCharges"].ToString();
                                    AWBRecord[AWBRecord.Length-1].InterlineServiceCharge = ds.Tables[4].Rows[i]["AmountSubjectInterlineServiceCharge"].ToString();
                                    AWBRecord[AWBRecord.Length-1].InterlineServiceChargePercent = ds.Tables[4].Rows[i]["InterlineServiceChargePercent"].ToString();
                                    AWBRecord[AWBRecord.Length-1].InterlineServiceChargeRateSign = ds.Tables[4].Rows[i]["InterlineServiceChargeRateSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].CurrencyAdjustmentIndicator = ds.Tables[4].Rows[i]["CurrencyAdjustmentIndicator"].ToString();
                                    AWBRecord[AWBRecord.Length-1].BilledWeight = ds.Tables[4].Rows[i]["BilledWeight"].ToString();
                                    AWBRecord[AWBRecord.Length-1].Prorate = ds.Tables[4].Rows[i]["ChargeType"].ToString();
                                    AWBRecord[AWBRecord.Length-1].ProratePercent = ds.Tables[4].Rows[i]["ProratePercent"].ToString();
                                    AWBRecord[AWBRecord.Length-1].PartShipmentIndicator = ds.Tables[4].Rows[i]["PartShipmentIndicator"].ToString();
                                    AWBRecord[AWBRecord.Length-1].WeightChargesSign = ds.Tables[4].Rows[i]["WeightChargesSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].OtherChargesSign = ds.Tables[4].Rows[i]["OtherChargesSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].ValuationCharges = ds.Tables[4].Rows[i]["ValuationCharges"].ToString();
                                    AWBRecord[AWBRecord.Length-1].ValuationChargesSign = ds.Tables[4].Rows[i]["ValuationChargesSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].WeightIndicator = ds.Tables[4].Rows[i]["WeightIndicator"].ToString();
                                    AWBRecord[AWBRecord.Length-1].VATAmount = ds.Tables[4].Rows[i]["VATAmount"].ToString();
                                    AWBRecord[AWBRecord.Length-1].VATAmountSign = ds.Tables[4].Rows[i]["VATAmountSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].InterlineServiceChargeAmt = ds.Tables[4].Rows[i]["InterlineServiceChargeAmount"].ToString();
                                    AWBRecord[AWBRecord.Length-1].InterlineServiceChargeAmtSign = ds.Tables[4].Rows[i]["InterlineServiceChargeAmountSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AWBTotalAmt = ds.Tables[4].Rows[i]["AWBTotalAmount"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AWBTotalAmtSign = ds.Tables[4].Rows[i]["AWBTotalAmountSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].CCAIndicator = ds.Tables[4].Rows[i]["CCAIndicator"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AttachmentIndicatorOriginal = ds.Tables[4].Rows[i]["AttachmentIndicatorOriginal"].ToString();
                                    AWBRecord[AWBRecord.Length-1].AttachmentIndicatorValidated = ds.Tables[4].Rows[i]["AttachmentIndicatorValidated"].ToString();
                                    AWBRecord[AWBRecord.Length-1].NoOfAttachments = ds.Tables[4].Rows[i]["NoOfAttachments"].ToString();
                                    AWBRecord[AWBRecord.Length-1].ValidationFlag = ds.Tables[4].Rows[i]["IsValidationFlag"].ToString();
                                    AWBRecord[AWBRecord.Length-1].InterlineServiceChargeSign = ds.Tables[4].Rows[i]["AmountSubjectInterlineServiceChargeSign"].ToString();
                                    AWBRecord[AWBRecord.Length-1].Filler1 = "";
                                    AWBRecord[AWBRecord.Length-1].Filler2 = "";
                                    AWBRecord[AWBRecord.Length-1].Filler3 = "";
                                    AWBRecord[AWBRecord.Length-1].Filler4 = "";
                                    AWBRecord[AWBRecord.Length-1].ReferenceField1 = "";
                                    AWBRecord[AWBRecord.Length-1].ReferenceField2 = "";
                                    AWBRecord[AWBRecord.Length-1].ReferenceField3 = "";
                                    AWBRecord[AWBRecord.Length-1].ReferenceField4 = "";
                                    AWBRecord[AWBRecord.Length-1].ReferenceField5 = "";
                                    AWBRecord[AWBRecord.Length - 1].FillingReference = "";
                                    AWBRecord[AWBRecord.Length - 1].ReasonCode = "";
                                    AWBRecord[AWBRecord.Length - 1].AirlineOwnUse = "";
                                    AWBRecord[AWBRecord.Length - 1].OurReference = "";

                                }

                                foreach(IDECEncode.AWBRecord s in AWBRecord)
                                {
                                    sb.AppendLine(s.ToString());
                                }
                            }

                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                BillingCodeRecord.RecordSequenceNo = ds.Tables[5].Rows[0]["RecordSequenceNo"].ToString();
                                BillingCodeRecord.BillingAirline = ds.Tables[5].Rows[0]["BillingAirline"].ToString();
                                BillingCodeRecord.BilledAirline = ds.Tables[5].Rows[0]["BilledAirline"].ToString();
                                BillingCodeRecord.BillingCode = ds.Tables[5].Rows[0]["PayMode"].ToString();
                                BillingCodeRecord.InvoiceNumber = ds.Tables[5].Rows[0]["InvoiceNumber"].ToString();
                                BillingCodeRecord.BatchSequenceNo = ds.Tables[5].Rows[0]["BatchSequenceNo"].ToString();
                                BillingCodeRecord.RecordSequenceNo = ds.Tables[5].Rows[0]["RecordSequenceWithinBatch"].ToString();
                                BillingCodeRecord.TotalWeightCharges = ds.Tables[5].Rows[0]["TotalWeightCharges"].ToString();
                                BillingCodeRecord.TotalOtherCharges = ds.Tables[5].Rows[0]["TotalOtherCharges"].ToString();
                                BillingCodeRecord.InterlineServiceChargeAmtSign = ds.Tables[5].Rows[0]["TotalInterlineServiceChargeAmtSign"].ToString();
                                BillingCodeRecord.TotalInterlineServiceCharge = ds.Tables[5].Rows[0]["TotalInterlineServiceCharge"].ToString();
                                BillingCodeRecord.BillingCodeSubTotal = ds.Tables[5].Rows[0]["BillingCodeSubTotal"].ToString();
                                BillingCodeRecord.TotalBillingRecords = ds.Tables[5].Rows[0]["TotalBillingRecords"].ToString();
                                BillingCodeRecord.TotalValuationCharges = ds.Tables[5].Rows[0]["TotalValuationCharges"].ToString();
                                BillingCodeRecord.TotalValuationChargesSign = ds.Tables[5].Rows[0]["TotalValuationChargesSign"].ToString();
                                BillingCodeRecord.TotalVATAmount = ds.Tables[5].Rows[0]["TotalVATAmount"].ToString();
                                BillingCodeRecord.TotalVATAmountSign = ds.Tables[5].Rows[0]["TotalVATAmountSign"].ToString();
                                BillingCodeRecord.TotalWeightChargesSign = ds.Tables[5].Rows[0]["TotalWeightChargesSign"].ToString();
                                BillingCodeRecord.TotalOtherChargesSign = ds.Tables[5].Rows[0]["TotalOtherChargesSign"].ToString();
                                BillingCodeRecord.BillingCodeSubTotalSign = ds.Tables[5].Rows[0]["TotalBillingCodeSubTotalSign"].ToString();
                                BillingCodeRecord.TotalNumberOfRecords = ds.Tables[5].Rows[0]["TotalRecords"].ToString();
                                BillingCodeRecord.BillingCodeSubTotalDesc = ds.Tables[5].Rows[0]["Description"].ToString();
                                BillingCodeRecord.Filler1 = "";
                                BillingCodeRecord.Filler2 = "";
                                BillingCodeRecord.Filler3 = "";
                                BillingCodeRecord.Filler4 = "";
                                BillingCodeRecord.Filler5 = "";
                                BillingCodeRecord.RecordSequenceBatch = "";
                                
                                sb.AppendLine(BillingCodeRecord.ToString());
                            }

                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                InvoiceTotalRecord.RecordSequenceNo = ds.Tables[6].Rows[0]["RecordSequenceNo"].ToString();
                                InvoiceTotalRecord.BillingAirline = ds.Tables[6].Rows[0]["BillingAirline"].ToString();
                                InvoiceTotalRecord.BilledAirline = ds.Tables[6].Rows[0]["BilledAirline"].ToString();
                                InvoiceTotalRecord.BillingCode = ds.Tables[6].Rows[0]["BillingCode"].ToString();
                                InvoiceTotalRecord.InvoiceNumber = ds.Tables[6].Rows[0]["InvoiceNumber"].ToString();
                                InvoiceTotalRecord.BatchSequenceNumber = ds.Tables[6].Rows[0]["BatchSequenceNo"].ToString();
                                InvoiceTotalRecord.RecordSequenceBatch = ds.Tables[6].Rows[0]["RecordSequenceWithinBatch"].ToString();
                                InvoiceTotalRecord.TotalWeightCharges = ds.Tables[6].Rows[0]["TotalWeightCharges"].ToString();
                                InvoiceTotalRecord.TotalOtherCharges = ds.Tables[6].Rows[0]["TotalOtherCharges"].ToString();
                                InvoiceTotalRecord.InterlineServiceChargeAmtSign = ds.Tables[6].Rows[0]["TotalInterlineServiceChargeAmtSign"].ToString();
                                InvoiceTotalRecord.InterlineServiceChargeAmt = ds.Tables[6].Rows[0]["TotalInterlineServiceCharge"].ToString();
                                InvoiceTotalRecord.NetInvoiceTotal = ds.Tables[6].Rows[0]["NetInvoiceTotal"].ToString();
                                InvoiceTotalRecord.NetInvoiceBillingTotal = ds.Tables[6].Rows[0]["NetInvoiceBillingTotal"].ToString();
                                InvoiceTotalRecord.TotalBillingRecords = ds.Tables[6].Rows[0]["TotalBillingRecords"].ToString();
                                InvoiceTotalRecord.TotalValuationCharges = ds.Tables[6].Rows[0]["TotalValuationCharges"].ToString();
                                InvoiceTotalRecord.TotalValuationChargesSign = ds.Tables[6].Rows[0]["TotalValuationChargesSign"].ToString();
                                InvoiceTotalRecord.TotalVATAmount = ds.Tables[6].Rows[0]["TotalVATAmount"].ToString();
                                InvoiceTotalRecord.TotalVATAmountSign = ds.Tables[6].Rows[0]["TotalVATAmountSign"].ToString();
                                InvoiceTotalRecord.TotalWeightChargesSign = ds.Tables[6].Rows[0]["TotalWeightChargesSign"].ToString();
                                InvoiceTotalRecord.TotalOtherChargesSign = ds.Tables[6].Rows[0]["TotalOtherChargesSign"].ToString();
                                InvoiceTotalRecord.NetInvoiceTotalSign = ds.Tables[6].Rows[0]["NetInvoiceTotalSign"].ToString();
                                InvoiceTotalRecord.NetInvoiceBillingTotalSign = ds.Tables[6].Rows[0]["NetInvoiceBillingTotalSign"].ToString();
                                InvoiceTotalRecord.TotalRecords = ds.Tables[6].Rows[0]["TotalRecords"].ToString();
                                InvoiceTotalRecord.TotalNetAmountWithoutVAT = ds.Tables[6].Rows[0]["TotalNetAmountWithoutVAT"].ToString();
                                InvoiceTotalRecord.TotalNetAmountWithoutVATSign = ds.Tables[6].Rows[0]["TotalNetAmountWithoutVATSign"].ToString();
                                InvoiceTotalRecord.Filler1 = "";
                                InvoiceTotalRecord.Filler2 = "";
                                InvoiceTotalRecord.Filler3 = "";
                                InvoiceTotalRecord.Filler4 = "";

                                sb.AppendLine(InvoiceTotalRecord.ToString());
                            }

                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                InvoiceFooterRecord.RecordSequenceNo = ds.Tables[7].Rows[0]["RecordSequenceNo"].ToString();
                                InvoiceFooterRecord.SFI = ds.Tables[7].Rows[0]["SFI"].ToString();
                                InvoiceFooterRecord.BillingAirline = ds.Tables[7].Rows[0]["BillingAirline"].ToString();
                                InvoiceFooterRecord.BilledAirline = ds.Tables[7].Rows[0]["BilledAirline"].ToString();
                                InvoiceFooterRecord.BillingCode = ds.Tables[7].Rows[0]["BillingCode"].ToString();
                                InvoiceFooterRecord.InvoiceNumber = ds.Tables[7].Rows[0]["InvoiceNumber"].ToString();
                                InvoiceFooterRecord.FooterSerialNo = ds.Tables[7].Rows[0]["FooterSerialNo"].ToString();
                                InvoiceFooterRecord.FooterDetails1 = ds.Tables[7].Rows[0]["FooterDetails1"].ToString();
                                InvoiceFooterRecord.FooterDetails2 = ds.Tables[7].Rows[0]["FooterDetails2"].ToString();
                                InvoiceFooterRecord.FooterDetails3 = ds.Tables[7].Rows[0]["FooterDetails3"].ToString();
                                InvoiceFooterRecord.FooterDetails4 = ds.Tables[7].Rows[0]["FooterDetails4"].ToString(); 
                                InvoiceFooterRecord.FooterDetails5 = ds.Tables[7].Rows[0]["FooterDetails5"].ToString();
                                InvoiceFooterRecord.Filler1 = "";
                                InvoiceFooterRecord.Filler2 = "";

                                sb.AppendLine(InvoiceFooterRecord.ToString());

                            }

                            if (ds.Tables[8].Rows.Count > 0)
                            {
                                FileTotalRecord.RecordSequenceNo = ds.Tables[8].Rows[0]["RecordSequenceNo"].ToString();
                                FileTotalRecord.BillingAirline = ds.Tables[8].Rows[0]["BillingAirline"].ToString();
                                FileTotalRecord.BilledAirline = ds.Tables[8].Rows[0]["BilledAirline"].ToString();
                                FileTotalRecord.InvoiceNumber = ds.Tables[8].Rows[0]["InvoiceNumber"].ToString();
                                FileTotalRecord.BatchSequenceNumber = ds.Tables[8].Rows[0]["BatchSequenceNo"].ToString();
                                FileTotalRecord.RecordSequenceBatch = ds.Tables[8].Rows[0]["RecordSequenceWithinBatch"].ToString();
                                FileTotalRecord.TotalWeightCharges = ds.Tables[8].Rows[0]["TotalWeightCharges"].ToString();
                                FileTotalRecord.TotalOtherCharges = ds.Tables[8].Rows[0]["TotalOtherCharges"].ToString();
                                FileTotalRecord.TotalInterlineServiceChargeAmt = ds.Tables[8].Rows[0]["TotalInterlineServiceCharge"].ToString();
                                FileTotalRecord.NetInvoiceTotal = ds.Tables[8].Rows[0]["NetInvoiceTotal"].ToString();
                                FileTotalRecord.NetInvoiceBillingTotal = ds.Tables[8].Rows[0]["NetInvoiceBillingTotal"].ToString();
                                FileTotalRecord.TotalBillingRecords = ds.Tables[8].Rows[0]["TotalBillingRecords"].ToString();
                                FileTotalRecord.TotalValuationCharges = ds.Tables[8].Rows[0]["TotalValuationCharges"].ToString();
                                FileTotalRecord.TotalVATAmount = ds.Tables[8].Rows[0]["TotalVATAmount"].ToString();
                                FileTotalRecord.TotalRecords = (ds.Tables[0].Rows.Count + ds.Tables[1].Rows.Count + ds.Tables[2].Rows.Count + ds.Tables[3].Rows.Count +
                                    ds.Tables[4].Rows.Count + ds.Tables[5].Rows.Count + ds.Tables[6].Rows.Count + ds.Tables[7].Rows.Count + ds.Tables[8].Rows.Count).ToString();
                                FileTotalRecord.Filler1 = "";
                                FileTotalRecord.Filler2 = "";
                                FileTotalRecord.Filler3 = "";
                                FileTotalRecord.Filler4 = "";
                                FileTotalRecord.Filler5 = "";
                                FileTotalRecord.Filler6 = "";
                                FileTotalRecord.Filler7 = "";


                                sb.AppendLine(FileTotalRecord.ToString());
                            }

                            if (sb != null)
                            {
                                string filename = "CIDECF-589" + ds.Tables[1].Rows[0]["InvoiceDate"].ToString() + ".dat";
                                Response.Clear();
                                Response.ContentType = "application/txt";
                                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                                Response.Write(sb.ToString());
                                Response.End();


                            }
                            else
                            {
                                lblStatus.Text = "IDEC input file  generation failed! Please try again...";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    lblStatus.Text = "Please select Invoice(s) for IDEC file generation !";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            } 
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/frmInterlineInvoiceListing.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click

    }
}
