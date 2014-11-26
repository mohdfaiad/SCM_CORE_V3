using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Drawing;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class BillingInvoiceCollection : System.Web.UI.Page
    {
        BillingAWBFileInvoiceBAL objBillBAL = new BillingAWBFileInvoiceBAL();
        BALCollectionDetails objBAL = new BALCollectionDetails();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BALAgentCredit ObjTrans = new BALAgentCredit();
        DateTime dtCurrentDate = DateTime.Now;
        LoginBL objBL = new LoginBL();

        string strfromdate, strtodate, strchequedate, strdepositdate;
        protected void Page_Load(object sender, EventArgs e)
        {
            dtCurrentDate = (DateTime)Session["IT"];
            if (!IsPostBack)
            {
                try
                {
                    //Added on 21-08-2014 - Vijay
                    btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnPrintPDF.Visible = false;
                    //lblStatus.Text = "";
                    txtInvoiceFrom.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                    txtInvoiceTo.Text = dtCurrentDate.ToString("dd/MM/yyyy");

                    LoadAgentDropdown(); //AgentName 
                    LoadAgentCodeDropdown(); //AgentCode
                    LoadInvoiceTypes(); //Billing invoice types

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

                    //Agent authorization
                    string AgentCode = Convert.ToString(Session["AgentCode"]);

                    if (AgentCode != "")
                    {
                        ddlAgentName.SelectedValue = AgentCode;
                        ddlAgentName_SelectedIndexChanged(null, null);
                        ddlAgentCode.Enabled = false;
                        ddlAgentName.Enabled = false;

                    }

                    //By default Cash is selected so disable bank textbox
                    txtChequeDdNo.Enabled = false;
                    txtChequeDate.Enabled = false;
                    txtBankName.Enabled = false;
                    TXTWreason.Text = "";
                    TXTWAmount.Text = "";
                    TXTInvoiceNo.Text = "";
                    TXTIssueBy.Text = "";
                    //if (Request.QueryString["INVNO"] != null && Request.QueryString["INVNO"] != "")
                    if (Request.QueryString["Type"] == "WalkIn" || Request.QueryString["Type"] == "Dest")
                    {
                        DateTime CurrentDt = (DateTime)Session["IT"];
                        txtInvoiceNumber.Text = Request.QueryString["INVNO"].ToString().Trim();
                        txtInvoiceNumber.Enabled = false;

                        txtInvoiceFrom.Enabled = false;
                        btnInvoiceFrom.Enabled = false;
                        //txtInvoiceFrom.Text = CurrentDt.AddDays(-1).ToString("dd/MM/yyyy");
                        txtInvoiceFrom.Text = CurrentDt.AddDays(-30).ToString("dd/MM/yyyy");

                        txtInvoiceTo.Enabled = false;
                        btnInvoiceTo.Enabled = false;
                        txtInvoiceTo.Text = CurrentDt.AddDays(1).ToString("dd/MM/yyyy");

                        ddlAgentCode.Enabled = false;
                        ddlAgentName.Enabled = false;
                        //txtAWBPrefix.Enabled = false;
                        txtAWBPrefix.Text = Request.QueryString["AWBPrefix"].ToString().Trim();
                        //txtAWBNumber.Enabled = false;
                        txtAWBNumber.Text = Request.QueryString["AWBNo"].ToString().Trim();
                        //txtOrigin.Enabled = false;
                        txtOrigin.Text = Request.QueryString["Origin"].ToString().Trim();
                        ddlBillType.Enabled = false;
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                        btnSaveDCM.Enabled = false;

                        //If Invoice type is walkin - Type querystring coming from quick booking
                        if (Request.QueryString["Type"] == "WalkIn")
                        {
                            ddlInvoiceType.SelectedValue = "2";
                            ddlInvoiceType.Enabled = false;
                            btnList_Click(null, null);
                        }

                        //If Invoice type is Destination - Type querystring coming from quick booking
                        if (Request.QueryString["Type"] == "Dest")
                        {
                            ddlInvoiceType.SelectedValue = "3";
                            ddlInvoiceType.Enabled = false;
                            btnList_Click(null, null);
                        }

                        if (grdInvoiceList.Rows.Count > 0)
                        {
                            //((RadioButton)grdInvoiceList.Rows[0].FindControl("rbSelect")).Checked = true;
                            if (ddlInvoiceType.SelectedValue == "2")
                                ((CheckBox)grdInvoiceList.Rows[0].FindControl("chkSelect")).Checked = true;
                            else
                                ((RadioButton)grdInvoiceList.Rows[0].FindControl("rbSelect")).Checked = true;

                            txtCollectedAmount.Text = ((Label)grdInvoiceList.Rows[0].FindControl("lblPendingAmount")).Text.Trim();
                            txtPPRemarks.Text = ((Label)grdInvoiceList.Rows[0].FindControl("lblPPRemarks")).Text;
                            txtTDS.Text = "0";
                            txtVAT.Text = "0";
                            txt194C.Text = "0";
                            HdnBooking.Value = "1";
                            hdPendingAmt.Value = txtCollectedAmount.Text;
                        }
                    }

                    //get awb level collection applicable or not
                    string AWBCollection = "";
                    AWBCollection = objBAL.getAWBLevelCollectionValue();
                    if (AWBCollection == "true")
                        Session["AWBCollection"] = AWBCollection;
                    else
                        Session["AWBCollection"] = "";

                }
                catch (Exception)
                {
                }
            }

            //outside ispostback
            //get VAT in collections visible or not - true for CEBU
            string VATVisible = "";
            if (Session["VATVisible"] == null)
            {
                VATVisible = objBL.GetMasterConfiguration("ShowCollectionVAT");
                Session["VATVisible"] = VATVisible;
            }

            if (Session["VATVisible"].ToString() != "true") //hide VAT details in screen
            {
                hideVATDetails();
            }

            //get Deposit Date in collections visible or not - true for CEBU
            string DepositDateVisible = "";
            if (Session["DepositDateVisible"] == null)
            {
                DepositDateVisible = objBL.GetMasterConfiguration("ShowCollectionDepositDate");
                Session["DepositDateVisible"] = DepositDateVisible;
            }

            if (Session["DepositDateVisible"].ToString() != "true") //hide deposit date details in screen
            {
                hideDepositDateDetails();
            }

            //To get PP and CC visible in invoice details
            string PPCCVisible = "";
            if (Session["PPCCVisible"] == null)
            {
                PPCCVisible = objBL.GetMasterConfiguration("ShowPrepaidAndCollectInInvoice");
                Session["PPCCVisible"] = PPCCVisible;
            }

            if (Session["PPCCVisible"].ToString() != "true") //hide CC columns in Invoice details
            {
                hideCCDetails();
            }

            //to check if Partial collection allowed for PP and CC invoices
            if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "PartialCollectionAllowedForPP")))
            {
                hdnPartialPP.Value = "Y";
            }
            else
            {
                hdnPartialPP.Value = "N";
            }
            if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "PartialCollectionAllowedForCC")))
            {
                hdnPartialCC.Value = "Y";
            }
            else
            {
                hdnPartialCC.Value = "N";
            }

        }

        #region hide VAT details
        protected void hideVATDetails()
        {
            LabelVAT.Visible = false;
            txtVAT.Visible = false;
            grdInvoiceList.Columns[11].Visible = false; //Hide VAT column from invoice list
            grdAWBCollect.Columns[9].Visible = false; //Hide VAT column from AWB Collection list
        }
        #endregion hide VAT details

        #region hide deposit date details
        protected void hideDepositDateDetails()
        {
            LabelDepositDate.Visible = false;
            txtDepositDate.Visible = false;
            grdAWBCollect.Columns[19].Visible = false; //Hide Deposit date column from AWB collection grid
        }
        #endregion hide deposit date details

        #region hide CC details
        protected void hideCCDetails()
        {
            grdInvoiceDetails.Columns[9].Visible = false; //Hide CC freight column
            grdInvoiceDetails.Columns[10].Visible = false; //Hide CC OCDC column
            grdInvoiceDetails.Columns[11].Visible = false; //Hide CC OCDA column
        }
        #endregion hide CC details


        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            DataSet ds = new DataSet("BillingInvoiceCollection_1");
               ds = objBillBAL.GetAllAgents();
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
            DataSet ds = new DataSet("BillingInvoiceCollection_2");
                ds = objBillBAL.GetAllAgents();
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

        #region Load Payment Types
        public void LoadPaymentTypes()
        {
            DataSet ds = new DataSet("BillingInvoiceCollection_3");
                ds = objBAL.GetAllPaymentTypes();
            if (ds != null)
            {
                ddlPaymentType.DataSource = ds;
                ddlPaymentType.DataMember = ds.Tables[0].TableName;
                ddlPaymentType.DataTextField = "PaymentType";
                ddlPaymentType.DataValueField = "PaymentType";
                ddlPaymentType.DataBind();
            }
        }
        #endregion Load Payment Types

        #region Load Invoice Types
        public void LoadInvoiceTypes()
        {
            DataSet ds = new DataSet("BillingInvoiceCollection_5");
                ds = objBAL.GetAllBillingInvoiceTypes();
            if (ds != null)
            {
                ddlInvoiceType.DataSource = ds;
                ddlInvoiceType.DataMember = ds.Tables[0].TableName;
                ddlInvoiceType.DataTextField = "InvoiceTypeName";
                ddlInvoiceType.DataValueField = "InvoiceTypeID";
                ddlInvoiceType.DataBind();
            }
        }
        #endregion Load Payment Types

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
            Session["AWBCollectData"] = null;
            PopulateUserOpeningBalance(); //Get User Opening Balance onLoad.


            bindCollectionDetails();

            //Default values when click on list
            txtDepositDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
            txtCollectedAmount.Text = "0";
            txtTDS.Text = "0";
            txtVAT.Text = "0";
            txt194C.Text = "0";

            txtPPRemarks.Text = "";
            txtTINNumber.Text = "";
            txtORNumber.Text = "";
            setDefaultDDL();// Pawan Jadhav CS-961
            ddlPaymentType_SelectedIndexChanged(sender, e);// Pawan Jadhav CS-961

        }
        void setDefaultDDL()// Pawan Jadhav CS-961
        {
            if (ddlInvoiceType.SelectedIndex == 0)// Agent
            {
                ddlPaymentType.Text = "DD";
            }
            else
            {
                //if walkin Or Dest
                ddlPaymentType.Text = "Cash";
            }
        }
        protected void rbSelect_CheckedChanged(object sender, System.EventArgs e)
        {
            //Clear the existing selected row 
            foreach (GridViewRow oldrow in grdInvoiceList.Rows)
            {
                ((RadioButton)oldrow.FindControl("rbSelect")).Checked = false;
                ((CheckBox)oldrow.FindControl("chkSelect")).Checked = false;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("rbSelect")).Checked = true;

            //Get the values in textboxes from selected row
            txtCollectedAmount.Text = ((Label)row.FindControl("lblCollectedAmount")).Text;
            txtTDS.Text = ((Label)row.FindControl("lblTDSAmount")).Text;
            txtVAT.Text = ((Label)row.FindControl("lblVATAmount")).Text;
            if (((Label)row.FindControl("lblPaymentType")).Text != "")
            {
                ddlPaymentType.SelectedValue = ((Label)row.FindControl("lblPaymentType")).Text;
            }
            txtChequeDdNo.Text = ((Label)row.FindControl("lblChequeDdNo")).Text;
            txtChequeDate.Text = ((Label)row.FindControl("lblChequeDate")).Text;
            txtBankName.Text = ((Label)row.FindControl("lblBankName")).Text;
            txt194C.Text = ((Label)row.FindControl("lbl194CAmount")).Text;
            txtPPRemarks.Text = ((Label)row.FindControl("lblPPRemarks")).Text;

            txtTINNumber.Text = ((Label)row.FindControl("lblTINNumber")).Text;

            if (((Label)row.FindControl("lblDepositDate")).Text != "")
                txtDepositDate.Text = ((Label)row.FindControl("lblDepositDate")).Text;
            else
                txtDepositDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");

            hdPendingAmt.Value = ((Label)row.FindControl("lblPendingAmount")).Text;
            hdTransactionId.Value = "";
            ddlPaymentType_SelectedIndexChanged(null, null);
        }

        protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCreditPopup.Enabled = false;

            if (ddlPaymentType.SelectedValue == "Cash")
            {
                txtChequeDdNo.Enabled = false;
                txtChequeDate.Enabled = false;
                txtBankName.Enabled = false;
            }
            else if (ddlPaymentType.SelectedValue == "Cheque")
            {
                txtChequeDdNo.Enabled = true;
                txtChequeDate.Enabled = true;
                txtBankName.Enabled = true;
            }
            else
            {
                txtChequeDdNo.Enabled = true;
                txtChequeDate.Enabled = false;
                txtBankName.Enabled = false;
                txtChequeDate.Text = "";
                txtBankName.Text = "";
            }

            if (ddlPaymentType.SelectedValue == "Card")
            {
                btnCreditPopup.Enabled = true;
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        protected void grdInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Session["dsInvoiceData"]
            DataSet dst = new DataSet("Collection_grdInvoiceList_dst");
            dst = (DataSet)Session["dsInvoiceData"];
            grdInvoiceList.PageIndex = e.NewPageIndex;
            grdInvoiceList.DataSource = dst.Tables[0];
            grdInvoiceList.DataBind();
            editCollectionGridInvoiceAmt();
            editCollectionGridPendingAmt();
            if (dst != null)
                dst.Dispose();
        }

        #region RowCommand
        protected void grdInvoiceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblStatus.Text = "";
            Session["AWBCollectData"] = null;
            BALCollectionDetails BALCol = new BALCollectionDetails();

            try
            {
                #region Invoice details
                if (e.CommandName == "Detail")
                {
                    string InvoiceNumber = "";
                    InvoiceNumber = e.CommandArgument.ToString();
                    try
                    {
                        string strInvType = "";
                        if (ddlInvoiceType.SelectedIndex == 0)
                            strInvType = "Regular";
                        else if (ddlInvoiceType.SelectedIndex == 1)
                            strInvType = "WalkIn";
                        else if (ddlInvoiceType.SelectedIndex == 2)
                            strInvType = "Dest";

                        
                        if (InvoiceNumber != "")
                        {
                            hfInvoiceNos.Value = InvoiceNumber;
                            hfInvoiceType.Value = strInvType;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateInvoicesExcel();", true);

                        }
                        else
                        {
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }
                #endregion Invoice details


                #region ORReceipt
                try
                {
                    if (e.CommandName == "ORNumber")
                    {
                        string ORNumber = e.CommandArgument.ToString();
                        string Station = Session["Station"].ToString();
                        DateTime IssuingDate = (DateTime)Session["IT"];
                        Session["ORReceiptDownload"] = null;
                        Session["ORReceiptDownloadCalc"] = null;
                        Session["ORReceiptName"] = null;
                        Session["REPRINT"] = null;
                        DataSet ds = new DataSet("BillingInvoiceCollection_7");
                        DataSet dsCalc = new DataSet("BillingInvoiceCollection_8");

                        if (ddlInvoiceType.SelectedValue == "1")
                        {
                            ds = BALCol.GetORDetailsRegularCollection(ORNumber, Station, IssuingDate, "");
                            dsCalc = BALCol.GetORDetailsRegularCollectionCalculation(ORNumber, Station, IssuingDate, "");
                        }
                        else if (ddlInvoiceType.SelectedValue == "2")
                        {
                            ds = BALCol.GetORDetailsRegularCollection(ORNumber, Station, IssuingDate, "Walkin");
                            dsCalc = BALCol.GetORDetailsRegularCollectionCalculation(ORNumber, Station, IssuingDate, "Walkin");
                        }
                        else if (ddlInvoiceType.SelectedValue == "3")
                        {
                            ds = BALCol.GetORDetailsRegularCollection(ORNumber, Station, IssuingDate, "Destination");
                            dsCalc = BALCol.GetORDetailsRegularCollectionCalculation(ORNumber, Station, IssuingDate, "Destination");
                        }

                        System.IO.MemoryStream Logo = null;
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }

                        if (ds.Tables[0].Columns.Contains("Logo") == false)
                        {
                            DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            col1.DefaultValue = Logo.ToArray();
                            ds.Tables[0].Columns.Add(col1);
                        }

                        if (ddlInvoiceType.SelectedValue == "1") //Agent
                            Session["ORReceiptName"] = "AgentInvoiceCollection";
                        else if (ddlInvoiceType.SelectedValue == "2") //WalkIn
                            Session["ORReceiptName"] = "WalkInInvoiceCollection";
                        else if (ddlInvoiceType.SelectedValue == "3") //Destination
                            Session["ORReceiptName"] = "DestinationInvoiceCollection";

                        Session["ORReceiptDownload"] = ds;


                        //To get OR calculations in single Datatable
                        DataSet dsCalcNew = new DataSet("BillingInvoiceCollection_9");
                        DataTable dtCalc = new DataTable("BillingInvoiceCollection_25");
                        dtCalc.Columns.Add("Txt");
                        dtCalc.Columns.Add("Value");

                        for (int tbl = 0; tbl < dsCalc.Tables.Count; tbl++)
                        {
                            if (dsCalc.Tables[tbl].Rows.Count > 0)
                            {
                                DataRow drCalc = dtCalc.NewRow();
                                drCalc["Txt"] = dsCalc.Tables[tbl].Rows[0][0].ToString();//Txt
                                drCalc["Value"] = dsCalc.Tables[tbl].Rows[0][1].ToString();//Value
                                dtCalc.Rows.Add(drCalc);
                            }
                        }

                        dsCalcNew.Tables.Add(dtCalc.Copy());

                        Session["ORReceiptDownloadCalc"] = dsCalcNew;


                        #region Commented Code
                        //ReportViewer rptLoadPlanReport = new ReportViewer();
                        //ReportDataSource rds1 = new ReportDataSource();
                        //rptLoadPlanReport.ProcessingMode = ProcessingMode.Local;

                        //LocalReport rep1 = rptLoadPlanReport.LocalReport;

                        //rep1.ReportPath = Server.MapPath("/Reports/rptORReceipt.rdlc");

                        //rds1.Name = "dsORReceipt_dtORReceipt";
                        //// rds1.Value = dtTable1; //ULD Section Table

                        //rds1.Value = ds.Tables[0];
                        //rep1.DataSources.Add(rds1);

                        ////rptLoadPlanReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                        //try
                        //{
                        //    string reportType = "PDF";
                        //    //string mimeType;
                        //    //string encoding;
                        //    string fileNameExtension;
                        //    string deviceInfo = "<DeviceInfo><PageHeight>40cm</PageHeight><PageWidth>50cm</PageWidth></DeviceInfo>";

                        //    //"<DeviceInfo>" +

                        //    //"  <OutputFormat>PDF</OutputFormat>" +

                        //    //"</DeviceInfo>";

                        //    Warning[] warnings;
                        //    string[] streamIds;
                        //    string mimeType; //= string.Empty;
                        //    string encoding;//= string.Empty;
                        //    string extension;//= string.Empty;

                        //    //Render the report
                        //    // send it to the client to download
                        //    byte[] bytes = rptLoadPlanReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        //    Response.Buffer = true;
                        //    Response.Clear();
                        //    Response.ContentType = mimeType;
                        //    Response.AddHeader("content-disposition", "attachment; filename=" + "InvoiceCollection" +"."+ ".pdf");
                        //    Response.BinaryWrite(bytes); // create the file
                        //    Response.Flush();

                        //    //Response.Clear();
                        //    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //    //Response.ContentType = "text/plain";
                        //    //Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceCollection"  + ".pdf");
                        //    //Response.BinaryWrite(bytes);
                        //    //Response.Flush();
                        //    //Response.End();
                        //}


                        //catch (Exception ex)
                        //{
                        //}
                        #endregion

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download();</SCRIPT>", false);
                        Session["REPRINT"] = "ORNumber";
                    }
                }
                catch (Exception ex)
                { }
                #endregion ORReceipt

                #region reprint

                try
                {
                    if (e.CommandName == "RePrint")
                    {
                        string ORNumber = e.CommandArgument.ToString();
                        string Station = Session["Station"].ToString();
                        DateTime IssuingDate = (DateTime)Session["IT"];
                        Session["ORReceiptDownload"] = null;
                        Session["ORReceiptDownloadCalc"] = null;
                        Session["ORReceiptName"] = null;
                        Session["REPRINT"] = null;
                        DataSet ds = new DataSet("BillingInvoiceCollection_9");
                        DataSet dsCalc = new DataSet("BillingInvoiceCollection_10");

                        if (ddlInvoiceType.SelectedValue == "1")
                        {
                            ds = BALCol.GetORDetailsRegularCollection(ORNumber, Station, IssuingDate, "");
                            dsCalc = BALCol.GetORDetailsRegularCollectionCalculation(ORNumber, Station, IssuingDate, "");
                        }
                        else if (ddlInvoiceType.SelectedValue == "2")
                        {
                            ds = BALCol.GetORDetailsRegularCollection(ORNumber, Station, IssuingDate, "Walkin");
                            dsCalc = BALCol.GetORDetailsRegularCollectionCalculation(ORNumber, Station, IssuingDate, "Walkin");
                        }
                        else if (ddlInvoiceType.SelectedValue == "3")
                        {
                            ds = BALCol.GetORDetailsRegularCollection(ORNumber, Station, IssuingDate, "Destination");
                            dsCalc = BALCol.GetORDetailsRegularCollectionCalculation(ORNumber, Station, IssuingDate, "Destination");
                        }

                        System.IO.MemoryStream Logo = null;
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
                            if (ds.Tables[0].Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                ds.Tables[0].Columns.Add(col1);
                            }



                            if (ddlInvoiceType.SelectedValue == "1") //Agent
                                Session["ORReceiptName"] = "AgentInvoiceCollection";
                            else if (ddlInvoiceType.SelectedValue == "2") //WalkIn
                                Session["ORReceiptName"] = "WalkInInvoiceCollection";
                            else if (ddlInvoiceType.SelectedValue == "3") //Destination
                                Session["ORReceiptName"] = "DestinationInvoiceCollection";

                            Session["ORReceiptDownload"] = ds;


                            //To get OR calculations in single Datatable
                            DataSet dsCalcNew = new DataSet("BillingInvoiceCollection_11");
                            DataTable dtCalc = new DataTable("BillingInvoiceCollection_26");
                            dtCalc.Columns.Add("Txt");
                            dtCalc.Columns.Add("Value");

                            for (int tbl = 0; tbl < dsCalc.Tables.Count; tbl++)
                            {
                                if (dsCalc.Tables[tbl].Rows.Count > 0)
                                {
                                    DataRow drCalc = dtCalc.NewRow();
                                    drCalc["Txt"] = dsCalc.Tables[tbl].Rows[0][0].ToString();//Txt
                                    drCalc["Value"] = dsCalc.Tables[tbl].Rows[0][1].ToString();//Value
                                    dtCalc.Rows.Add(drCalc);
                                }
                            }

                            dsCalcNew.Tables.Add(dtCalc.Copy());

                            Session["ORReceiptDownloadCalc"] = dsCalcNew;




                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download();</SCRIPT>", false);
                            Session["REPRINT"] = "RePrint";
                        }
                        catch (Exception ex)
                        { lblStatus.Text = "OR Number Not available to Reprint"; }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "OR Number Not available to Reprint";
                }



                #endregion reprint
                #region AWB Collection
                try
                {
                    double runningCollectionAmount; runningCollectionAmount = 0.0;
                    lblRunningCollectionAmt.Text = "";

                    if (e.CommandName == "AWBCollect" && ddlInvoiceType.SelectedValue == "1")
                    {
                        Session["AWBCollectData"] = null;
                        lblCollectAWBStatus.Text = "";
                        lblAWBCollectInvoiceNumber.Text = "";
                        lblAWBCollectCurrency.Text = "";
                        string InvoiceNumber = e.CommandArgument.ToString();

                        DataSet ds = new DataSet("BillingInvoiceCollection_12");
                        ds = db.SelectRecords("SP_GetAgentAWBCollectData", "InvoiceNumber", InvoiceNumber, SqlDbType.VarChar);

                        if (InvoiceNumber != "")
                        {
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 2)
                                {
                                    DataTable dtForLoop = new DataTable("BillingInvoiceCollection_27");
                                        dtForLoop = ds.Tables[2].Copy();
                                        DataTable dt = new DataTable("BillingInvoiceCollection_28");
                                        dt = ds.Tables[2].Copy();
                                    int CountOfAdded = 0;
                                    for (int index = 0; index < dtForLoop.Rows.Count; index++)
                                    {

                                        string strTotalAWBCharg = dtForLoop.Rows[index]["TotalAWBCharges"].ToString();
                                        strTotalAWBCharg = strTotalAWBCharg == "" ? "0" : strTotalAWBCharg;
                                        string strPending = dtForLoop.Rows[index]["PendingAmount"].ToString();
                                        string strCurrAWBNumber = dtForLoop.Rows[index]["AWBNumber"].ToString();
                                        string strNextAWBNumber = "";
                                        if (index + 1 < dtForLoop.Rows.Count)
                                            strNextAWBNumber = dtForLoop.Rows[index + 1]["AWBNumber"].ToString();

                                        strPending = strPending == "" ? "0" : strPending;
                                        if (Convert.ToDouble(strTotalAWBCharg) > Convert.ToDouble(strPending) &&
                                           Convert.ToDouble(strPending) != 0 && strNextAWBNumber != strCurrAWBNumber)
                                        {
                                            DataRow dr = dt.NewRow();
                                            dr["SrNo"] = 0;
                                            dr["AWBNumber"] = dtForLoop.Rows[index]["AWBNumber"].ToString();
                                            dr["TotalAWBCharges"] = dtForLoop.Rows[index]["TotalAWBCharges"].ToString();
                                            dr["AgentCode"] = dtForLoop.Rows[index]["AgentCode"].ToString();
                                            dr["CollectedAmount"] = 0;
                                            dr["TDSAmount"] = 0;
                                            dr["PaymentType"] = "";
                                            dr["DCMAmount"] = 0;
                                            dr["DCMType"] = "";
                                            dr["ChequeDdNo"] = "";
                                            dr["ChequeDate"] = "";
                                            dr["BankName"] = "";
                                            dr["PaymentDate"] = "";
                                            dr["PendingAmount"] = dtForLoop.Rows[index]["PendingAmount"].ToString();
                                            dr["Remarks"] = "";
                                            dr["Amt194C"] = 0;
                                            dr["ORRecieptNo"] = "";
                                            dr["DepositDate"] = "";
                                            dr["VATAmount"] = 0;
                                            dt.Rows.InsertAt(dr, CountOfAdded + index + 1);
                                            CountOfAdded++;
                                        }
                                        // DataTable[] dtArr = { ds.Tables[0].Copy(), ds.Tables[1].Copy(), dt, ds.Tables[3].Copy() };
                                        List<DataTable> dtArr = new List<DataTable>();
                                        foreach (DataTable dtItem in ds.Tables)
                                        {
                                            dtArr.Add(dtItem);
                                        }
                                        for (int i = ds.Tables.Count - 1; i >= 0; i--)
                                        {
                                            ds.Tables.RemoveAt(i);
                                        }
                                        for (int tblIndex = 0; tblIndex < dtArr.Count; tblIndex++)
                                        {
                                            DataTable dtTemp = new DataTable("InvcColl_dt" + tblIndex);
                                            if (tblIndex == 2)
                                            {
                                                ds.Tables.Add(dt.Copy());
                                            }
                                            else
                                            {
                                                dtTemp = dtArr[tblIndex];
                                                ds.Tables.Add(dtTemp);
                                            }
                                        }
                                    }
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        Session["AWBCollectData"] = ds;
                                        grdAWBCollect.DataSource = ds.Tables[2];
                                        grdAWBCollect.DataBind();
                                        lblAWBCollectInvoiceNumber.Text = "Invoice# : " + InvoiceNumber;
                                        lblAWBCollectCurrency.Text = "Currency : " + ds.Tables[1].Rows[0]["RateLineCurrency"].ToString();
                                        for (int i = 0; i < grdAWBCollect.Rows.Count; i++)
                                        {
                                            if (ds.Tables[2].Rows[i]["PaymentType"].ToString() != "")
                                            {
                                                ((DropDownList)grdAWBCollect.Rows[i].FindControl("ddlPaymentType")).SelectedValue = ds.Tables[2].Rows[i]["PaymentType"].ToString();
                                            }
                                            if (ds.Tables[2].Rows[i]["DCMType"].ToString() != "")
                                            {
                                                //((DropDownList)grdAWBCollect.Rows[i].FindControl("ddlDCMType")).SelectedValue = ds.Tables[2].Rows[i]["DCMType"].ToString();
                                            }
                                            //Put default todays date for deposit date if blank
                                            if (ds.Tables[2].Rows[i]["DepositDate"].ToString() == "")
                                            {
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtDepositDate")).Text = dtCurrentDate.ToString("dd/MM/yyyy");
                                            }
                                            //Disable AWBcollection if OR is already generated
                                            if (((Label)grdAWBCollect.Rows[i].FindControl("lblORNumber")).Text.Trim() != "")
                                            {
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtCollectedAmount")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtTDSAmount")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txt194CAmount")).Enabled = false;
                                                ((DropDownList)grdAWBCollect.Rows[i].FindControl("ddlPaymentType")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtDCMAmount")).Enabled = false;
                                                ((DropDownList)grdAWBCollect.Rows[i].FindControl("ddlDCMType")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtChequeDDNo")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtChequeDate")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtBankName")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtPaymentDate")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtRemarks")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtVATAmount")).Enabled = false;
                                                ((TextBox)grdAWBCollect.Rows[i].FindControl("txtDepositDate")).Enabled = false;

                                                if (lblRunningCollectionAmt.Text == "")
                                                    lblRunningCollectionAmt.Text = "0";
                                            }
                                            else //If OR is not generate, populate fields on AWB list popup entered in main screen
                                            {
                                                ((DropDownList)grdAWBCollect.Rows[i].FindControl("ddlPaymentType")).SelectedValue = ddlPaymentType.SelectedValue;
                                                if (txtChequeDdNo.Text != "")
                                                    ((TextBox)grdAWBCollect.Rows[i].FindControl("txtChequeDDNo")).Text = txtChequeDdNo.Text;
                                                if (txtChequeDate.Text != "")
                                                    ((TextBox)grdAWBCollect.Rows[i].FindControl("txtChequeDate")).Text = txtChequeDate.Text;
                                                if (txtBankName.Text != "")
                                                    ((TextBox)grdAWBCollect.Rows[i].FindControl("txtBankName")).Text = txtBankName.Text;
                                                if (txtPPRemarks.Text != "")
                                                    ((TextBox)grdAWBCollect.Rows[i].FindControl("txtRemarks")).Text = txtPPRemarks.Text;
                                                if (txtDepositDate.Text != "")
                                                    ((TextBox)grdAWBCollect.Rows[i].FindControl("txtDepositDate")).Text = txtDepositDate.Text;

                                                //Add default collected amount as runningCollectionAmount
                                                runningCollectionAmount = runningCollectionAmount + Convert.ToDouble(ds.Tables[2].Rows[i]["CollectedAmount"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[i]["TDSAmount"].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[i]["VATAmount"].ToString());
                                                lblRunningCollectionAmt.Text = Convert.ToString(runningCollectionAmount);
                                            }
                                        }
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelAWBCollect();</script>", false);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                #endregion AWB Collection

            }
            catch
            { }
        }
        #endregion

        protected DataTable fillgrdAWBCollectPaymentType()
        {
            DataTable dtPayType = new DataTable("BillingInvoiceCollection_30");
            try
            {
                DataSet ds = new DataSet("BillingInvoiceCollection_13");
                    ds= objBAL.GetAllPaymentTypes();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dtPayType = ds.Tables[0];
                }
            }
            catch
            {
                dtPayType.Columns.Add("PaymentType");
                dtPayType.Rows.Add("Select");
            }

            return dtPayType;
        }


        protected void grdInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton hyp = (LinkButton)e.Row.FindControl("lblORNumber");
                    if (hyp.Text.StartsWith("CR"))
                    {
                        hyp.Enabled = false;
                    }
                }
            }
            catch { }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            //    chkSelect.Attributes.Add("onclick", "javascript:return DoPostBackWithRowIndex('" + e.Row.RowIndex + "');");
            //    //TxtCollAmt.Attributes.Add("onkeydown", "javascript:return DoPostBackWithRowIndex('" + e.Row.RowIndex + "');");
            //}
            DataSet dsTotal = new DataSet("BillingInvoiceCollection_14");
                dsTotal = (DataSet)Session["dsInvoiceData"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblGTotalText = (Label)e.Row.FindControl("lblGTotalText");
                Label lblGTotalInvoiceAmt = (Label)e.Row.FindControl("lblGTotalInvoiceAmt");
                Label lblGTotalCollectedAmt = (Label)e.Row.FindControl("lblGTotalCollectedAmt");
                Label lblGTotal194CAmt = (Label)e.Row.FindControl("lblGTotal194CAmt");
                Label lblGTotalPendingAmt = (Label)e.Row.FindControl("lblGTotalPendingAmt");

                lblGTotalText.Text = "Grand Total";
                lblGTotalInvoiceAmt.Text = dsTotal.Tables[1].Rows[0]["InvoiceAmount"].ToString();
                lblGTotalCollectedAmt.Text = dsTotal.Tables[1].Rows[0]["CollectedAmount"].ToString();
                lblGTotal194CAmt.Text = dsTotal.Tables[1].Rows[0]["Amt194C"].ToString();
                lblGTotalPendingAmt.Text = dsTotal.Tables[1].Rows[0]["PendingAmount"].ToString();

            }
            //disable hyperlink on invoice number for walkin and dest
            if (ddlInvoiceType.SelectedValue != "1")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lnkInv = (LinkButton)e.Row.FindControl("lnkAWBCollect");
                    lnkInv.Enabled = false;
                }
            }

        }

        protected void bindCollectionDetails()
        {
            try
            {
                lblStatus.Visible = false;
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
                catch
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


                DataSet DSInvoicedata = new DataSet("BillingInvoiceCollection_15");

                if (ddlInvoiceType.SelectedValue == "1") //Agent
                    DSInvoicedata = objBAL.GetCollectionMasterData(ddlAgentName.SelectedValue, ddlBillType.SelectedValue, txtOrigin.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), txtInvoiceNumber.Text.Trim(), txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim(), ddlcollection.SelectedValue);
                else if (ddlInvoiceType.SelectedValue == "2") //Walk-In
                    DSInvoicedata = objBAL.GetCollectionWalkInMasterData(ddlAgentName.SelectedValue, ddlBillType.SelectedValue, txtOrigin.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), txtInvoiceNumber.Text.Trim(), txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim(), ddlcollection.SelectedValue);
                else if (ddlInvoiceType.SelectedValue == "3") // Destination
                    DSInvoicedata = objBAL.GetCollectionDestMasterData(ddlAgentName.SelectedValue, ddlBillType.SelectedValue, txtOrigin.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), txtInvoiceNumber.Text.Trim(), txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim(), ddlcollection.SelectedValue);

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    Session["dsInvoiceData"] = DSInvoicedata;
                    grdInvoiceList.DataSource = DSInvoicedata.Tables[0];
                    grdInvoiceList.DataBind();

                    //If Invoice Type is walk in, keep checkbox column visible and radio button column invisible
                    if (ddlInvoiceType.SelectedValue == "2") //Walk-In
                    {
                        grdInvoiceList.Columns[1].Visible = true; //Checkbox
                        grdInvoiceList.Columns[2].Visible = false; //RadioButton
                    }
                    else
                    {
                        grdInvoiceList.Columns[1].Visible = false; //Checkbox
                        grdInvoiceList.Columns[2].Visible = true; //RadioButton
                    }

                    //If AWB level collection is not activated, disable Invoice Number link
                    if (Convert.ToString(Session["AWBCollection"]) == "")
                    {
                        for (int ro = 0; ro < grdInvoiceList.Rows.Count; ro++)
                        {
                            ((LinkButton)grdInvoiceList.Rows[ro].FindControl("lnkAWBCollect")).Enabled = false;
                        }
                    }


                    LoadPaymentTypes();

                    grdInvoiceList.Visible = true;
                    pnlAddCollection.Visible = true;
                    pnlDCMAdjustment.Visible = true;
                    pnlchargeswaveoff.Visible = true;
                    btnExport.Visible = true;
                    btnPrintPDF.Visible = true;
                    lblStatus.Visible = false;
                    editCollectionGridInvoiceAmt();
                    editCollectionGridPendingAmt();
                    txtCollectedAmount.Text = "";
                    txtTDS.Text = "";
                    txtChequeDdNo.Text = "";
                    txtChequeDate.Text = "";
                    txtBankName.Text = "";
                    txtDCMNumber.Text = "";
                    txtDCMAmount.Text = "";
                    txtReason.Text = "";
                    txt194C.Text = "";
                    txtVAT.Text = "";
                    txtDepositDate.Text = "";

                    btnPosttoTreasury.Visible = true;

                    //Agent authorization
                    if (Convert.ToString(Session["AgentCode"]) != "")
                    {
                        disableForAgent();
                    }
                    for (int rowindex = 0; rowindex < grdInvoiceList.Rows.Count; rowindex++)
                    {
                        LinkButton lbtn = (LinkButton)grdInvoiceList.Rows[rowindex].FindControl("lblORNumber");
                        if (lbtn.Text == "")
                        {
                            LinkButton lbtnprint = (LinkButton)grdInvoiceList.Rows[rowindex].FindControl("lblRePrint");
                            lbtnprint.Visible = false;
                        }
                    }

                }
                else
                {
                    grdInvoiceList.Visible = false;
                    pnlAddCollection.Visible = false;
                    pnlDCMAdjustment.Visible = false;
                    pnlchargeswaveoff.Visible = false;
                    btnExport.Visible = false;
                    btnPrintPDF.Visible = false;

                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Blue;
                }
            }
            catch
            {
            }
        }

        protected void disableForAgent()
        {
            pnlAddCollection.Visible = false;
            pnlDCMAdjustment.Visible = false;
            pnlchargeswaveoff.Visible = false;
        }

        private void PopulateUserOpeningBalance()
        {
            string UserName = Convert.ToString(Session["UserName"]);
            BALCollectionDetails objBAL = new BALCollectionDetails();
            decimal OpeningBalance = 0;
            string DataIds = string.Empty;

            try
            {
                //if(ddlInvoiceType.SelectedValue == "1") //Agent
                //    objBAL.GetUserOpeningBalance(UserName, "A", ref OpeningBalance, ref DataIds);
                //else if (ddlInvoiceType.SelectedValue == "2") //Walk-In
                //    objBAL.GetUserOpeningBalance(UserName, "W", ref OpeningBalance, ref DataIds);
                //else if (ddlInvoiceType.SelectedValue == "3") //Destination
                //    objBAL.GetUserOpeningBalance(UserName, "D", ref OpeningBalance, ref DataIds);

                objBAL.GetUserOpeningBalance(UserName, "", ref OpeningBalance, ref DataIds);


                lblOpeningBalance.Text = OpeningBalance.ToString();
                hdnSerialIds.Value = DataIds;
            }
            catch
            {
                lblOpeningBalance.Text = "0";
                hdnSerialIds.Value = "";
            }
            finally
            {
                objBAL = null;
            }
        }

        protected void editCollectionGridInvoiceAmt()
        {
            string OldInvNo = "", NewInvNo = "";
            int FirstLoop = 0;
            for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
            {
                if (FirstLoop == 0)
                    OldInvNo = ((Label)grdInvoiceList.Rows[0].FindControl("lblInvoiceNumber")).Text;
                else
                    OldInvNo = ((Label)grdInvoiceList.Rows[y - 1].FindControl("lblInvoiceNumber")).Text;

                if (grdInvoiceList.Rows.Count > y)
                {
                    NewInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                }
                if (NewInvNo == OldInvNo && y != 0)
                {
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceAmount")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblCentralAgent")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblLocalAgent")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lbl194CAmount")).Visible = false;

                }
                else
                {
                    FirstLoop = 1;
                    OldInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                }
            }
        }

        protected void editCollectionGridPendingAmt()
        {
            string OldInvNo = "", NewInvNo = "";
            int FirstLoop = 0;
            for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
            {
                if (FirstLoop == 0)
                    OldInvNo = ((Label)grdInvoiceList.Rows[0].FindControl("lblInvoiceNumber")).Text;
                else
                    OldInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;

                if (grdInvoiceList.Rows.Count - 1 > y)
                {
                    NewInvNo = ((Label)grdInvoiceList.Rows[y + 1].FindControl("lblInvoiceNumber")).Text;
                    if (NewInvNo == OldInvNo)
                    {
                        //((CheckBox)grdInvoiceList.Rows[y].FindControl("chkSelect")).Checked = false;
                        //((CheckBox)grdInvoiceList.Rows[y].FindControl("chkSelect")).Enabled = false;
                        //((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Visible = false;
                        ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Attributes.Add("style", "display:none");
                    }
                    else
                    {
                        FirstLoop = 1;
                        OldInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                    }
                }
            }
        }

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/BillingInvoiceCollection.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                invNo = "";
                string AgentName = "";
                string PendingAmt = "";
                string TransactionType = "";
                string PaymentType = ddlPaymentType.SelectedItem.Text;
                double InvoiceAmount = 0.0;
                double totalPendingAmt = 0.0;

                string CollectedAmt = txtCollectedAmount.Text.Trim();
                string TDSAmount = txtTDS.Text.Trim();
                string VATAmount = txtVAT.Text.Trim();
                string Amt194C = txt194C.Text.Trim();

                int AWBSelected = 0;
                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    //Loop for walk-In
                    if (ddlInvoiceType.SelectedValue == "2")
                    {
                        if (((CheckBox)grdInvoiceList.Rows[y].FindControl("chkSelect")).Checked)
                        {
                            AWBSelected = AWBSelected + 1;
                            invNo = invNo + ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                            totalPendingAmt = totalPendingAmt + Convert.ToDouble(((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Text);
                            //AgentName = ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Text;
                            //PendingAmt = ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Text;
                            //InvoiceAmount = Convert.ToDouble(((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceAmount")).Text);
                        }

                    }
                    else
                    {
                        if (((RadioButton)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                        {
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                            AgentName = ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Text;
                            PendingAmt = ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Text;
                            InvoiceAmount = Convert.ToDouble(((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceAmount")).Text);
                        }
                    }

                }


                //To check if collected amount is less than pending amount
                if (AWBSelected > 1)
                {
                    if (Math.Round(Convert.ToDouble(CollectedAmt) + Convert.ToDouble(TDSAmount) + Convert.ToDouble(VATAmount) + Convert.ToDouble(Amt194C),2) < Math.Round(totalPendingAmt,2))
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Partial collection can not be done on multiple invoices";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    if (Math.Round(Convert.ToDouble(CollectedAmt) + Convert.ToDouble(TDSAmount) + Convert.ToDouble(VATAmount) + Convert.ToDouble(Amt194C),2) > Math.Round(totalPendingAmt,2))
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Collected amount exceeding Pending amount";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }

                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                if (!Validation())
                    return;

                try
                {
                    //Commented as Done in Javascript
                    //To check if collected amount is greeatteer than pending amount
                    //if (Convert.ToDouble(CollectedAmt) + Convert.ToDouble(TDSAmount) + Convert.ToDouble(VATAmount) + Convert.ToDouble(Amt194C) > Convert.ToDouble(PendingAmt))
                    //{
                    //    lblStatus.Visible = true;
                    //    lblStatus.Text = "Collected amount can not be greater than pending amount";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}


                    //Commented on 11-08-2014 (Partial collection allowed on invoice. AWBs falling in Total amount will be updated)
                    ////to check if AWB level collection is active or not
                    //if (Convert.ToString(Session["AWBCollection"]) != "" && ddlInvoiceType.SelectedValue == "1")
                    //{
                    //    //Commented by Vijay 27-07-2014 - Over collection allowed for agents and Dest invoices
                    //    //if (Convert.ToDouble(PendingAmt) != Convert.ToDouble(CollectedAmt) + Convert.ToDouble(TDSAmount) + Convert.ToDouble(VATAmount) + Convert.ToDouble(Amt194C))
                    //    if (Convert.ToDouble(PendingAmt) > Convert.ToDouble(CollectedAmt) + Convert.ToDouble(TDSAmount) + Convert.ToDouble(VATAmount) + Convert.ToDouble(Amt194C))
                    //    {
                    //        lblStatus.Visible = true;
                    //        lblStatus.Text = "Please collect on AWB for partial collection";
                    //        lblStatus.ForeColor = Color.Red;
                    //        return;
                    //    }
                    //}
                }
                catch (Exception ex)
                {

                }


                //to check if Collected amount is equal to Pending amount..else do AWB level collection
                //For Collections other than walk-in invoice
                #region non walk-in collection
                if (ddlInvoiceType.SelectedValue != "2")
                {
                    #region Prepare Parameters
                    object[] RateCardInfo = new object[16];
                    int i = 0;

                    RateCardInfo.SetValue(invNo, i);
                    i++;

                    RateCardInfo.SetValue(Convert.ToDouble(txtCollectedAmount.Text.Trim()), i);
                    i++;

                    RateCardInfo.SetValue(Convert.ToDouble(txtTDS.Text.Trim()), i);
                    i++;

                    RateCardInfo.SetValue(ddlPaymentType.SelectedValue, i);
                    i++;

                    RateCardInfo.SetValue(txtChequeDdNo.Text.Trim(), i);
                    i++;

                    if (txtChequeDate.Text.Trim() != "")
                        RateCardInfo.SetValue(Convert.ToDateTime(strchequedate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                    else
                        RateCardInfo.SetValue(txtChequeDate.Text.Trim(), i);
                    i++;

                    RateCardInfo.SetValue(txtBankName.Text.Trim(), i);
                    i++;

                    RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                    i++;

                    RateCardInfo.SetValue(txtPPRemarks.Text.Trim(), i);
                    i++;

                    RateCardInfo.SetValue(Convert.ToDouble(txt194C.Text.Trim()), i);
                    i++;

                    RateCardInfo.SetValue(hdTransactionId.Value.Trim(), i);
                    i++;

                    RateCardInfo.SetValue(Convert.ToDateTime(Session["IT"]), i);
                    i++;

                    //new added
                    if (txtDepositDate.Text.Trim() != "")
                        RateCardInfo.SetValue(Convert.ToDateTime(strdepositdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                    else
                        RateCardInfo.SetValue(txtDepositDate.Text.Trim(), i);

                    i++;
                    RateCardInfo.SetValue(Convert.ToDouble(txtVAT.Text.Trim()), i);

                    i++;
                    RateCardInfo.SetValue(txtTINNumber.Text.Trim(), i);
                    i++;

                    RateCardInfo.SetValue(txtORNumber.Text.Trim(), i);

                    #endregion Prepare Parameters

                    string res = "";

                    if (ddlInvoiceType.SelectedValue == "1")
                        res = objBAL.AddInvoiceCollectionDetails(RateCardInfo);
                    else if (ddlInvoiceType.SelectedValue == "2")
                        res = objBAL.AddWalkInInvoiceCollectionDetails(RateCardInfo);
                    else if (ddlInvoiceType.SelectedValue == "3")
                        res = objBAL.AddDestInvoiceCollectionDetails(RateCardInfo);

                    if (res != "error")
                    {
                        //bindCollectionDetails();
                        btnList_Click(null, null);

                        lblStatus.Visible = true;
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                        try
                        {

                            if (CollectedAmt == "")
                            { CollectedAmt = "0"; }

                            TransactionType = "Credit";

                            //Commented by Vijay 10-08-2014
                            //if (ObjTrans.SaveTransacation(AgentName, "", PaymentType, TransactionType, 0, 0, Convert.ToDouble(CollectedAmt), txtPPRemarks.Text, Session["UserName"].ToString(), "", invNo))
                            //{
                            //}

                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = ex.Message;
                            lblStatus.ForeColor = Color.Red;
                        }
                        PopulateUserOpeningBalance();
                    }
                    else
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                #endregion non walk-in collection
                //Collections for multiple AWB at one time - get OR number
                #region walk-in collection
                else
                {
                    string res = "";
                    invNo = "";
                    string ORNumber = "", oldORNumber = "";
                    if (txtORNumber.Text.Length > 0)
                        ORNumber = txtORNumber.Text.Trim();
                    else if (Convert.ToDouble(txtCollectedAmount.Text.Trim()) < 0)
                        ORNumber = objBAL.GetNextORNumber("CR", 0, Session["UserName"].ToString());
                    else
                        ORNumber = objBAL.GetNextORNumber("OR", 0, Session["UserName"].ToString());


                    for (int r = 0; r < grdInvoiceList.Rows.Count; r++)
                    {
                        if (((CheckBox)grdInvoiceList.Rows[r].FindControl("chkSelect")).Checked)
                        {
                            invNo = ((Label)grdInvoiceList.Rows[r].FindControl("lblInvoiceNumber")).Text;
                            oldORNumber = ((LinkButton)grdInvoiceList.Rows[r].FindControl("lblORNumber")).Text;

                            if (invNo != "")
                            {
                                #region Prepare Parameters
                                object[] RateCardInfo = new object[16];
                                int i = 0;


                                RateCardInfo.SetValue(ORNumber, i);
                                i++;


                                RateCardInfo.SetValue(invNo, i);
                                i++;

                                //for single AWB, collected amount =  collected amount entered
                                if (AWBSelected == 1)
                                {
                                    RateCardInfo.SetValue(Convert.ToDouble(txtCollectedAmount.Text.Trim()), i);
                                    i++;
                                }
                                else//for multiple AWB collection, collected amount = total of all select AWBs pending amount
                                {
                                    RateCardInfo.SetValue(Convert.ToDouble(((Label)grdInvoiceList.Rows[r].FindControl("lblPendingAmount")).Text.Trim()), i);
                                    i++;
                                }


                                RateCardInfo.SetValue(Convert.ToDouble(txtTDS.Text.Trim()), i);
                                i++;

                                RateCardInfo.SetValue(ddlPaymentType.SelectedValue, i);
                                i++;

                                RateCardInfo.SetValue(txtChequeDdNo.Text.Trim(), i);
                                i++;

                                if (txtChequeDate.Text.Trim() != "")
                                    RateCardInfo.SetValue(Convert.ToDateTime(strchequedate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                                else
                                    RateCardInfo.SetValue(txtChequeDate.Text.Trim(), i);
                                i++;

                                RateCardInfo.SetValue(txtBankName.Text.Trim(), i);
                                i++;

                                RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                                i++;

                                RateCardInfo.SetValue(txtPPRemarks.Text.Trim(), i);
                                i++;

                                RateCardInfo.SetValue(Convert.ToDouble(txt194C.Text.Trim()), i);
                                i++;

                                RateCardInfo.SetValue(hdTransactionId.Value.Trim(), i);
                                i++;

                                RateCardInfo.SetValue(Convert.ToDateTime(Session["IT"]), i);
                                i++;

                                //new added
                                if (txtDepositDate.Text.Trim() != "")
                                    RateCardInfo.SetValue(Convert.ToDateTime(strdepositdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                                else
                                    RateCardInfo.SetValue(txtDepositDate.Text.Trim(), i);

                                i++;
                                RateCardInfo.SetValue(Convert.ToDouble(txtVAT.Text.Trim()), i);

                                i++;
                                RateCardInfo.SetValue(txtTINNumber.Text.Trim(), i);

                                #endregion Prepare Parameters



                                res = objBAL.AddMultipleWalkInInvoiceCollectionDetails(RateCardInfo);

                                if (res != "error" && res != "")
                                {
                                    try
                                    {
                                        if (CollectedAmt == "")
                                        { CollectedAmt = "0"; }

                                        TransactionType = "Credit";

                                        //Commented by Vijay 10-08-2014
                                        //if (ObjTrans.SaveTransacation(AgentName, "", PaymentType, TransactionType, 0, 0, Convert.ToDouble(CollectedAmt), txtPPRemarks.Text, Session["UserName"].ToString(), "", invNo))
                                        //{
                                        //}

                                    }
                                    catch (Exception ex)
                                    {
                                        lblStatus.Visible = true;
                                        lblStatus.Text = ex.Message;
                                        lblStatus.ForeColor = Color.Red;
                                    }
                                }
                                else
                                {
                                    //bindCollectionDetails();
                                    btnList_Click(null, null);

                                    lblStatus.Visible = true;
                                    lblStatus.Text = "Collections failed";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }
                            }
                        }

                    }

                    PopulateUserOpeningBalance();

                    if (res != "error" && res != "")
                    {
                        //bindCollectionDetails();
                        btnList_Click(null, null);

                        lblStatus.Visible = true;
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                    }
                }
                #endregion walk-in collection


                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "CallPopulateClick();", true);
            }
            catch
            {
            }
        }

        protected bool Validation()
        {
            if (txtCollectedAmount.Text.Trim() == "" && txtTDS.Text.Trim() == "" && txtVAT.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter Collected amount/TDS/VAT ";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (txtCollectedAmount.Text.Trim() == "")
                txtCollectedAmount.Text = "0";

            if (txtTDS.Text.Trim() == "")
                txtTDS.Text = "0";

            if (txtVAT.Text.Trim() == "")
                txtVAT.Text = "0";

            if (txt194C.Text.Trim() == "")
                txt194C.Text = "0";

            if (txtCollectedAmount.Text.Trim() == "0" && txtTDS.Text.Trim() == "0" && txtVAT.Text.Trim() == "0" && txt194C.Text.Trim() == "0")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter Collected amount/TDS/VAT ";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (ddlPaymentType.SelectedValue == "Cheque" || ddlPaymentType.SelectedValue == "DD" || ddlPaymentType.SelectedValue == "RTGS")
            {
                if (txtChequeDdNo.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please enter Cheque/DD/RTGS/Card Number ";
                    lblStatus.ForeColor = Color.Blue;
                    return false;
                }
            }
            if (ddlPaymentType.SelectedValue == "Cheque" && (txtChequeDate.Text.Trim() == "" || txtBankName.Text.Trim() == ""))
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter Cheque date and Bank name ";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (txtChequeDate.Text.Trim() != "")
            {
                DateTime dt;

                try
                {
                    string day = txtChequeDate.Text.Substring(0, 2);
                    string mon = txtChequeDate.Text.Substring(3, 2);
                    string yr = txtChequeDate.Text.Substring(6, 4);
                    strchequedate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strchequedate);
                }
                catch
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Cheque date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }

            if (txtDepositDate.Text.Trim() != "")
            {
                DateTime dt;

                try
                {
                    string day = txtDepositDate.Text.Substring(0, 2);
                    string mon = txtDepositDate.Text.Substring(3, 2);
                    string yr = txtDepositDate.Text.Substring(6, 4);
                    strdepositdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strdepositdate);
                }
                catch
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Deposit date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }


            return true;
        }

        protected bool ValidationAWBCollection()
        {
            double CollectedAmount = 0.0, TDSAmount = 0.0, Amt194CAmount = 0.0, VATAmount = 0.0, PendingAmount = 0.0;
            for (int row = 0; row < grdAWBCollect.Rows.Count; row++)
            {
                string ORNumber = ((Label)grdAWBCollect.Rows[row].FindControl("lblORNumber")).Text;

                if (ORNumber == "")
                {
                    CollectedAmount = Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtCollectedAmount")).Text.Trim());
                    TDSAmount = Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtTDSAmount")).Text.Trim());
                    Amt194CAmount = Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txt194CAmount")).Text.Trim());
                    VATAmount = Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtVATAmount")).Text.Trim());
                    PendingAmount = Convert.ToDouble(((Label)grdAWBCollect.Rows[row].FindControl("lblPendingAmount")).Text.Trim());

                    if (CollectedAmount + TDSAmount + Amt194CAmount + VATAmount > PendingAmount)
                    {
                        //Commented by Vijay - 27-07-2014 - Over collection allowed for agents
                        //lblCollectAWBStatus.Text = "Collected amount can not be greater than Pending amount";
                        //lblCollectAWBStatus.ForeColor = Color.Red;
                        //return false;
                    }
                }

            }
            return true;

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;

                int SrNo; SrNo = 0;
                invNo = "";
                string AgentName = "";
                string PendingAmt = "";
                string TransactionType = "";
                string CollectedAmt = txtCollectedAmount.Text.Trim();
                string PaymentType = ddlPaymentType.SelectedItem.Text;
                string CollAmt2 = "";

                int AWBSelected = 0;
                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    //Loop for walk-In
                    if (ddlInvoiceType.SelectedValue == "2")
                    {
                        if (((CheckBox)grdInvoiceList.Rows[y].FindControl("chkSelect")).Checked)
                        {
                            AWBSelected = AWBSelected + 1;
                            SrNo = Convert.ToInt32(((Label)grdInvoiceList.Rows[y].FindControl("lblSrNo")).Text);
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                        }
                    }
                    else
                    {
                        if (((RadioButton)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                        {
                            SrNo = Convert.ToInt32(((Label)grdInvoiceList.Rows[y].FindControl("lblSrNo")).Text);
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                            AgentName = ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Text;
                            PendingAmt = ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Text;
                            CollAmt2 = ((Label)grdInvoiceList.Rows[y].FindControl("lblCollectedAmount")).Text;

                        }
                    }

                }


                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (AWBSelected > 1) //To check if single Invoice selected for delete for walkin
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select single Invoice for udpate";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                #region Prepare Parameters
                object[] RateCardInfo = new object[13];
                int i = 0;

                RateCardInfo.SetValue(invNo, i);
                i++;

                RateCardInfo.SetValue(SrNo, i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtCollectedAmount.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtTDS.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(ddlPaymentType.SelectedValue, i);
                i++;

                RateCardInfo.SetValue(txtChequeDdNo.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(txtChequeDate.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(txtBankName.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                i++;

                RateCardInfo.SetValue(txtPPRemarks.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txt194C.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(txtDepositDate.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtVAT.Text.Trim()), i);


                #endregion Prepare Parameters

                string res = "";
                string resAWB = "";
                if (ddlInvoiceType.SelectedValue == "1")
                {
                    //To check if AWB level collection exists - if exists , invoice level can not be updated. Update awb level collection
                    resAWB = objBAL.CheckAWBCollectionForInvoice(invNo);

                    if (resAWB == "1") //if AWB collection exists
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Please update collections at AWB level";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else if (resAWB == "0")
                    {
                        res = objBAL.EditInvoiceCollectionDetails(RateCardInfo);
                    }
                }
                else if (ddlInvoiceType.SelectedValue == "2")
                {
                    //res = objBAL.EditWalkInInvoiceCollectionDetails(RateCardInfo);
                    res = objBAL.EditMultipleWalkInInvoiceCollectionDetails(RateCardInfo);
                }
                else if (ddlInvoiceType.SelectedValue == "3")
                    res = objBAL.EditDestInvoiceCollectionDetails(RateCardInfo);

                if (res != "error")
                {
                    bindCollectionDetails();
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    try
                    {
                        if (CollectedAmt == "")
                        { CollectedAmt = "0"; }

                        double ColAmt = Convert.ToDouble(CollAmt2) - Convert.ToDouble(CollectedAmt);
                        if (Convert.ToDouble(ColAmt) > 0)
                        {
                            TransactionType = "Debit";
                        }
                        else
                        {
                            TransactionType = "Credit";
                            ColAmt = -ColAmt;
                        }

                        //Commented by Vijay 10-08-2014
                        //if (ObjTrans.SaveTransacation(AgentName, "", PaymentType, TransactionType, 0, 0, ColAmt, txtPPRemarks.Text, Session["UserName"].ToString(), "", invNo))
                        //{
                        //}
                    }
                    catch (Exception ex)
                    { }
                    PopulateUserOpeningBalance();
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                invNo = "";
                string ORNo;
                ORNo = "";
                int SrNo; SrNo = 0;
                string Amount = "";
                string PaymentType = ddlPaymentType.SelectedItem.Text;
                string AgentName = "";

                int AWBSelected = 0;
                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    //Loop for walk-In
                    if (ddlInvoiceType.SelectedValue == "2")
                    {
                        if (((CheckBox)grdInvoiceList.Rows[y].FindControl("chkSelect")).Checked)
                        {
                            AWBSelected = AWBSelected + 1;
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                            ORNo = ((LinkButton)grdInvoiceList.Rows[y].FindControl("lblORNumber")).Text;
                            SrNo = Convert.ToInt32(((Label)grdInvoiceList.Rows[y].FindControl("lblSrNo")).Text);
                        }
                    }
                    else
                    {
                        if (((RadioButton)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                        {
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                            ORNo = ((LinkButton)grdInvoiceList.Rows[y].FindControl("lblORNumber")).Text;
                            SrNo = Convert.ToInt32(((Label)grdInvoiceList.Rows[y].FindControl("lblSrNo")).Text);
                            Amount = ((Label)grdInvoiceList.Rows[y].FindControl("lblCollectedAmount")).Text;
                            AgentName = ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Text;
                        }
                    }

                }

                if (invNo == "") //To check if Invoice number is selected
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (AWBSelected > 1) //To check if single Invoice selected for delete for walkin
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select single Invoice for delete";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                #region Prepare Parameters
                object[] RateCardInfo = new object[8];
                int i = 0;

                RateCardInfo.SetValue(invNo, i);
                i++;

                RateCardInfo.SetValue(SrNo, i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtCollectedAmount.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(ddlPaymentType.SelectedValue, i);
                i++;

                RateCardInfo.SetValue(txtChequeDdNo.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                i++;

                RateCardInfo.SetValue(Convert.ToDateTime(Session["IT"]), i);
                i++;

                RateCardInfo.SetValue(ORNo, i);


                #endregion Prepare Parameters

                string res = "";
                if (ddlInvoiceType.SelectedValue == "1")
                    res = objBAL.DeleteInvoiceCollectionDetails(RateCardInfo);
                else if (ddlInvoiceType.SelectedValue == "2")
                {
                    //res = objBAL.DeleteWalkInInvoiceCollectionDetails(RateCardInfo);
                    res = objBAL.DeleteMultipleWalkInInvoiceCollectionDetails(RateCardInfo);
                }
                else if (ddlInvoiceType.SelectedValue == "3")
                    res = objBAL.DeleteDestInvoiceCollectionDetails(RateCardInfo);

                if (res != "error")
                {
                    bindCollectionDetails();
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    try
                    {

                        //Commented by Vijay 10-08-2014
                        //if (ObjTrans.SaveTransacation(AgentName, "", PaymentType, "Debit", 0, 0, Convert.ToDouble(Amount), "Remarks", Session["UserName"].ToString(), "", invNo))
                        //{
                        //    // lblStatus.Text = "True";
                        //}
                    }
                    catch (Exception ex)
                    { }
                    PopulateUserOpeningBalance();
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsInvoiceData = new DataSet("BillingInvoiceCollection_16");
                dsInvoiceData = (DataSet)Session["dsInvoiceData"];
            DataRow dr = dsInvoiceData.Tables[0].NewRow();
            dr = dsInvoiceData.Tables[1].Rows[0];
            dsInvoiceData.Tables[0].ImportRow(dr);

            if (dsInvoiceData != null)
            {
                if (dsInvoiceData.Tables != null)
                {
                    if (dsInvoiceData.Tables.Count > 0)
                    {
                        if (dsInvoiceData.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["Filters"] = "";

                                DataTable DTFilters = new DataTable("BillingInvoiceCollection_31");

                                DTFilters.Columns.Add("AgentName");
                                DTFilters.Columns.Add("AgentCode");
                                DTFilters.Columns.Add("BillType");
                                DTFilters.Columns.Add("FromDate");
                                DTFilters.Columns.Add("ToDate");
                                DTFilters.Columns.Add("Origin");

                                DTFilters.Rows.Add(
                                    ddlAgentName.SelectedItem.Text,
                                    ddlAgentCode.SelectedItem.Text,
                                    ddlBillType.SelectedItem.Text,
                                    //Convert.ToDateTime(txtInvoiceFrom.Text).ToShortDateString(),
                                    //Convert.ToDateTime(txtInvoiceTo.Text).ToShortDateString(),
                                    txtInvoiceFrom.Text,
                                    txtInvoiceTo.Text,
                                    txtOrigin.Text);


                                Session["Filters"] = DTFilters;

                                Session["CollData"] = "";
                                DataTable DTCollData = new DataTable("BillingInvoiceCollection_32");
                                DTCollData = dsInvoiceData.Tables[0];
                                Session["CollData"] = DTCollData;

                                //to remove unwanted columns from export
                                if (DTCollData.Columns.Contains("SrNo"))
                                    DTCollData.Columns.Remove("SrNo");
                                if (DTCollData.Columns.Contains("CentralAgent"))
                                    DTCollData.Columns.Remove("CentralAgent");
                                if (DTCollData.Columns.Contains("IsActive"))
                                    DTCollData.Columns.Remove("IsActive");
                                if (DTCollData.Columns.Contains("BillType"))
                                    DTCollData.Columns.Remove("BillType");
                                if (DTCollData.Columns.Contains("Amt194C"))
                                    DTCollData.Columns.Remove("Amt194C");
                                if (DTCollData.Columns.Contains("RatelineCurrency"))
                                    DTCollData.Columns.Remove("RatelineCurrency");
                                if (DTCollData.Columns.Contains("ExecutionDate"))
                                    DTCollData.Columns.Remove("ExecutionDate");

                                ExportToExcel(DTCollData, "CollectionExport.xls");

                                //Response.Write("<script>");
                                //Response.Write("window.open('ShowCollectionDetailsReport.aspx','_blank')");
                                //Response.Write("</script>");
                                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('ShowCollectionDetailsReport.aspx','_blank')", true);


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

        protected void btnSaveDCM_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                invNo = "";

                int AWBSelected = 0;
                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    //Loop for walk-In
                    if (ddlInvoiceType.SelectedValue == "2")
                    {
                        if (((CheckBox)grdInvoiceList.Rows[y].FindControl("chkSelect")).Checked)
                        {
                            AWBSelected = AWBSelected + 1;
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                        }
                    }
                    if (((RadioButton)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                    {
                        invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                    }
                }

                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else if (AWBSelected > 1) //To check if single Invoice selected for DCM for walkin
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select single Invoice to save DCM";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                if (!DCMValidation(invNo))
                    return;

                #region Prepare Parameters
                object[] RateCardInfo = new object[10];
                int j = 0;

                RateCardInfo.SetValue(invNo, j);
                j++;

                RateCardInfo.SetValue(txtDCMNumber.Text.Trim(), j);
                j++;

                RateCardInfo.SetValue(txtDCMAmount.Text.Trim(), j);
                j++;

                //If DCMType.SelectedValue = 0 -> Collection/TDS amount
                //If DCMType.SelectedValue = 1 -> DCM debit amount
                //If DCMType.SelectedValue = 2 -> DCM credit amount
                RateCardInfo.SetValue(ddlDCMType.SelectedValue, j);
                j++;

                RateCardInfo.SetValue(txtReason.Text.Trim(), j);
                j++;

                RateCardInfo.SetValue(Session["UserName"].ToString(), j);
                j++;

                RateCardInfo.SetValue("", j); //result
                j++;

                RateCardInfo.SetValue(Convert.ToDateTime(Session["IT"]), j); //Updatedon


                #endregion Prepare Parameters

                string res = "";
                if (ddlInvoiceType.SelectedValue == "1") //Agent
                    res = objBAL.AddInvoiceDCMCollectionDetails(RateCardInfo);
                else if (ddlInvoiceType.SelectedValue == "2") //Walk-IN
                    res = objBAL.AddWalkInInvoiceDCMCollectionDetails(RateCardInfo);
                else if (ddlInvoiceType.SelectedValue == "3") //Destination
                    res = objBAL.AddDestInvoiceDCMCollectionDetails(RateCardInfo);

                if (res != "error")
                {
                    //bindCollectionDetails();
                    btnList_Click(null, null);

                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected bool DCMValidation(string InvoiceNumber)
        {
            if (txtDCMNumber.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter DCM number";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (!objBAL.ValidateDCMNumber(txtDCMNumber.Text.Trim(), InvoiceNumber))
            {
                lblStatus.Visible = true;
                lblStatus.Text = "DCM number not valid";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (txtDCMAmount.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter DCM amount";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }


            if (txtReason.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter reason";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            return true;
        }

        protected void btnPosttoTreasury_Click(object sender, EventArgs e)
        {
            string UserName = Convert.ToString(Session["UserName"]);
            DateTime TimeStamp = Convert.ToDateTime(Session["IT"]);
            string strDataIds = hdnSerialIds.Value;
            BALCollectionDetails objBAL = new BALCollectionDetails();
            bool blnResult = false;

            try
            {
                //if(ddlInvoiceType.SelectedValue == "1") //Agn
                //    blnResult = objBAL.RecordPostingAmount(strDataIds, UserName, TimeStamp, "A");
                //if (ddlInvoiceType.SelectedValue == "2") //Walkn
                //    blnResult = objBAL.RecordPostingAmount(strDataIds, UserName, TimeStamp, "W");
                //if (ddlInvoiceType.SelectedValue == "3") //Ds
                //    blnResult = objBAL.RecordPostingAmount(strDataIds, UserName, TimeStamp, "D");

                blnResult = objBAL.RecordPostingAmount(strDataIds, UserName, TimeStamp, "");

                if (blnResult)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Amount posted successfully.";
                    PopulateUserOpeningBalance();
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Amount posted failed.";
                }
            }
            catch
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Amount posted failed.";
            }
        }

        protected void btnPostTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo = string.Empty, AgentName = string.Empty, AgentCode = string.Empty, Description = string.Empty, TranError = string.Empty;
                decimal PendingAmt = 0;
                decimal TDSAmount = 0;
                decimal VATAmount = 0;
                decimal Amt194C = 0;
                decimal CollectedAmt = 0;
                int awbCollectIndex = 0;
                BALCardTransaction BalTransaction = new BALCardTransaction();
                bool blnTranResult = false;

                //For Invoice level collection
                if (Session["AWBCollectData"] == null)
                {
                    for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                    {
                        if (((CheckBox)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                        {
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text.Trim();
                            AgentCode = ((Label)grdInvoiceList.Rows[y].FindControl("lblLocalAgent")).Text.Trim();
                            PendingAmt = Convert.ToDecimal(txtCollectedAmount.Text.Trim());
                            Description = txtPPRemarks.Text.Trim();
                            break;
                        }
                    }

                    hdTransactionId.Value = "";
                    string TransactionId = string.Empty;

                    if (invNo != "" && PendingAmt > 0)
                    {
                        blnTranResult = BalTransaction.ProcessCardPayment("", txtCardNumber.Text.Trim(), ddlMonth.SelectedValue + ddlYear.SelectedValue, PendingAmt, Description,
                            txtCVV.Text.Trim(), invNo, AgentCode, txtCardholdername.Text.Trim(), "", "", "", "", "", "", "", "", "", "", ref TranError, ref TransactionId);

                        if (blnTranResult)
                        {
                            lblError.Text = "";
                            hdTransactionId.Value = TransactionId;

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CloseWindow();</SCRIPT>", false);

                            btnSave_Click(null, null);
                        }
                        else
                        {
                            lblError.ForeColor = Color.Red;
                            if (TranError == "")
                                lblError.Text = "Card payment process failed.";
                            else
                                lblError.Text = TranError;

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CreditCardPopup();</SCRIPT>", false);
                        }
                    }
                }
                else //For AWB level collection
                {
                    //to get credit card details adding to which row in AWB collection - Set in credit card button click in javascript.
                    if (hfAWBCollectRowIndex.Value == null || hfAWBCollectRowIndex.Value == "")
                    {
                        return;
                    }
                    else
                    {
                        awbCollectIndex = Convert.ToInt32(hfAWBCollectRowIndex.Value);
                    }

                    DataSet dsTran = new DataSet("BillingInvoiceCollection_17");
                        dsTran = (DataSet)Session["AWBCollectData"];

                    if (dsTran != null && dsTran.Tables.Count > 2 && dsTran.Tables[1].Rows.Count > 0)
                    {
                        invNo = dsTran.Tables[1].Rows[0]["InvoiceNumber"].ToString();
                        AgentCode = dsTran.Tables[1].Rows[0]["AgentCode"].ToString();

                        CollectedAmt = Convert.ToDecimal(((TextBox)grdAWBCollect.Rows[awbCollectIndex - 1].FindControl("txtCollectedAmount")).Text);
                        TDSAmount = Convert.ToDecimal(((TextBox)grdAWBCollect.Rows[awbCollectIndex - 1].FindControl("txtTDSAmount")).Text);
                        VATAmount = Convert.ToDecimal(((TextBox)grdAWBCollect.Rows[awbCollectIndex - 1].FindControl("txtVATAmount")).Text);
                        Amt194C = Convert.ToDecimal(((TextBox)grdAWBCollect.Rows[awbCollectIndex - 1].FindControl("txt194CAmount")).Text);

                        PendingAmt = CollectedAmt + TDSAmount + VATAmount + Amt194C;

                        Description = ((TextBox)grdAWBCollect.Rows[awbCollectIndex - 1].FindControl("txtRemarks")).Text;
                    }

                    hdTransactionId.Value = "";

                    string TransactionId = string.Empty;

                    if (invNo != "" && PendingAmt > 0)
                    {
                        blnTranResult = BalTransaction.ProcessCardPayment("", txtCardNumber.Text.Trim(), ddlMonth.SelectedValue + ddlYear.SelectedValue, PendingAmt, Description,
                            txtCVV.Text.Trim(), invNo, AgentCode, txtCardholdername.Text.Trim(), "", "", "", "", "", "", "", "", "", "", ref TranError, ref TransactionId);

                        if (blnTranResult)
                        {
                            lblError.Text = "";
                            hdTransactionId.Value = TransactionId;

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CloseWindow();</SCRIPT>", false);

                            btnSaveAWBCollect_Click(null, null);
                            hfAWBCollectRowIndex.Value = "";
                        }
                        else
                        {
                            lblError.ForeColor = Color.Red;
                            if (TranError == "")
                                lblError.Text = "Card payment process failed.";
                            else
                                lblError.Text = TranError;

                            ImageButton IB = grdAWBCollect.Rows[awbCollectIndex - 1].FindControl("btnAWBCollectCreditPopup") as ImageButton;

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>AWBCollectCreditCardPopup(" + IB.ClientID + ");</SCRIPT>", false);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        protected void btnSaveAWBCollect_Click(object sender, EventArgs e)
        {
            //grdAWBCollect
            //for (int i = 0; i < grdAWBCollect.Rows.Count; i++)
            //{
            //    ((DropDownList)grdAWBCollect.Rows[i].FindControl("ddlPaymentType")).se;
            //}

            try
            {
                string invNo;
                string AWBPrf = "", AWBNo = "";
                string res = "";
                invNo = "";
                int Srno = 0;
                string AgentName = "", AgentCode = "";
                string TransactionType = "";


                DataSet dsAWBCollectData = new DataSet("BillingInvoiceCollection_18");
                    dsAWBCollectData = (DataSet)Session["AWBCollectData"];

                if (dsAWBCollectData != null && dsAWBCollectData.Tables.Count > 2 && dsAWBCollectData.Tables[1].Rows.Count > 0)
                {
                    invNo = dsAWBCollectData.Tables[1].Rows[0]["InvoiceNumber"].ToString();
                    AgentName = dsAWBCollectData.Tables[1].Rows[0]["AgentName"].ToString();
                    AgentCode = dsAWBCollectData.Tables[1].Rows[0]["AgentCode"].ToString();

                    if (invNo == "")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Please select Invoice Number";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                    else
                    {
                        lblStatus.Visible = false;
                        lblStatus.Text = "";
                    }

                    if (!ValidationAWBCollection())
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelAWBCollect();</script>", false);
                        return;
                    }


                    //DataSet dsres = null;
                    string NewORNumber = "", OldORNumber = "";

                    if (txtORNumber.Text.Length > 0)
                        NewORNumber = txtORNumber.Text.Trim();
                    else
                        NewORNumber = objBAL.GetNextORNumber("OR", 0, Session["UserName"].ToString());

                    double totalCollectedAmount = 0.0;

                    if (NewORNumber != "")
                    {
                        for (int row = 0; row < grdAWBCollect.Rows.Count; row++)
                        {
                            //PendingAmt = ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Text;

                            OldORNumber = ((Label)grdAWBCollect.Rows[row].FindControl("lblORNumber")).Text;

                            Srno = Convert.ToInt32(dsAWBCollectData.Tables[2].Rows[row]["SrNo"].ToString());

                            #region Prepare Parameters
                            object[] RateCardInfo = new object[18];
                            int i = 0;

                            RateCardInfo.SetValue(Srno, i);
                            i++;

                            RateCardInfo.SetValue(invNo, i);
                            i++;

                            RateCardInfo.SetValue(((Label)grdAWBCollect.Rows[row].FindControl("lblAWBNo")).Text, i);
                            AWBNo = ((Label)grdAWBCollect.Rows[row].FindControl("lblAWBNo")).Text;

                            i++;

                            RateCardInfo.SetValue(Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtCollectedAmount")).Text.Trim()), i);
                            i++;

                            RateCardInfo.SetValue(Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtTDSAmount")).Text.Trim()), i);
                            i++;

                            RateCardInfo.SetValue(((DropDownList)grdAWBCollect.Rows[row].FindControl("ddlPaymentType")).SelectedValue, i);
                            i++;

                            RateCardInfo.SetValue(((TextBox)grdAWBCollect.Rows[row].FindControl("txtChequeDDNo")).Text.Trim(), i); //txtChequeDDNo
                            i++;

                            //if (txtChequeDate.Text.Trim() != "")
                            //    RateCardInfo.SetValue(Convert.ToDateTime(strchequedate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                            //else
                            RateCardInfo.SetValue(((TextBox)grdAWBCollect.Rows[row].FindControl("txtChequeDate")).Text.Trim(), i); //txtChequeDate
                            i++;

                            RateCardInfo.SetValue(((TextBox)grdAWBCollect.Rows[row].FindControl("txtBankName")).Text.Trim(), i); //txtBankName
                            i++;

                            RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                            i++;

                            RateCardInfo.SetValue(((TextBox)grdAWBCollect.Rows[row].FindControl("txtRemarks")).Text.Trim(), i); //txtRemarks
                            i++;

                            RateCardInfo.SetValue(Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txt194CAmount")).Text.Trim()), i); //txt194CAmount
                            //RateCardInfo.SetValue(Convert.ToDouble(0), i); //txt194CAmount
                            i++;

                            RateCardInfo.SetValue(hdTransactionId.Value.Trim(), i);
                            i++;

                            RateCardInfo.SetValue(Convert.ToDateTime(Session["IT"]), i);
                            i++;

                            RateCardInfo.SetValue(NewORNumber, i);
                            i++;

                            //if (txtDepositDate.Text.Trim() != "")
                            //    RateCardInfo.SetValue(Convert.ToDateTime(strdepositdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                            //else
                            RateCardInfo.SetValue(((TextBox)grdAWBCollect.Rows[row].FindControl("txtDepositDate")).Text.Trim(), i);

                            i++;
                            RateCardInfo.SetValue(Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtVATAmount")).Text.Trim()), i);

                            i++;
                            RateCardInfo.SetValue(txtTINNumber.Text.Trim(), i);



                            #endregion Prepare Parameters

                            totalCollectedAmount = Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtCollectedAmount")).Text.Trim()) +
                                Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtTDSAmount")).Text.Trim()) +
                                Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txt194CAmount")).Text.Trim()) +
                                Convert.ToDouble(((TextBox)grdAWBCollect.Rows[row].FindControl("txtVATAmount")).Text.Trim());


                            //Commented 21-08-2014 - Update collection
                            //if (ddlInvoiceType.SelectedValue == "1" && OldORNumber == "" && totalCollectedAmount > 0.0)
                            if (ddlInvoiceType.SelectedValue == "1" && OldORNumber == "" && totalCollectedAmount != 0.0)
                                res = objBAL.AddAWBCollectionDetails(RateCardInfo);

                            if (res != "error" && res != "")
                            {
                                try
                                {
                                    //Collected amount = new amount entered in textbox - old amount in labels
                                    //string CollectedAmt = txtCollectedAmount.Text.Trim();
                                    //string PaymentType = ddlPaymentType.SelectedItem.Text;
                                    double CollectedAmt = 0.0;
                                    string PaymentType = ((DropDownList)grdAWBCollect.Rows[row].FindControl("ddlPaymentType")).SelectedValue;
                                    string Remarks = ((TextBox)grdAWBCollect.Rows[row].FindControl("txtRemarks")).Text;

                                    string oldCollectedAmt = ((Label)grdAWBCollect.Rows[row].FindControl("lblCollectedAmount")).Text;
                                    string oldTDSAmt = ((Label)grdAWBCollect.Rows[row].FindControl("lblTDSAmount")).Text;
                                    string oldVATAmt = ((Label)grdAWBCollect.Rows[row].FindControl("lblVATAmount")).Text;
                                    string old194CAmt = ((Label)grdAWBCollect.Rows[row].FindControl("lbl194CAmount")).Text;

                                    string newCollectedAmt = ((TextBox)grdAWBCollect.Rows[row].FindControl("txtCollectedAmount")).Text;
                                    string newTDSAmt = ((TextBox)grdAWBCollect.Rows[row].FindControl("txtTDSAmount")).Text;
                                    string newVATAmt = ((TextBox)grdAWBCollect.Rows[row].FindControl("txtVATAmount")).Text;
                                    string new194CAmt = ((TextBox)grdAWBCollect.Rows[row].FindControl("txt194CAmount")).Text;

                                    if (oldCollectedAmt == "")
                                    { oldCollectedAmt = "0"; }
                                    if (oldTDSAmt == "")
                                    { oldTDSAmt = "0"; }
                                    if (oldVATAmt == "")
                                    { oldVATAmt = "0"; }
                                    if (old194CAmt == "")
                                    { old194CAmt = "0"; }

                                    double oldTotalCollectedAmt = Convert.ToDouble(oldCollectedAmt) + Convert.ToDouble(oldTDSAmt) + Convert.ToDouble(oldVATAmt) + Convert.ToDouble(old194CAmt);
                                    double newTotalCollectedAmt = Convert.ToDouble(newCollectedAmt) + Convert.ToDouble(newTDSAmt) + Convert.ToDouble(newVATAmt) + Convert.ToDouble(new194CAmt);

                                    if (newCollectedAmt == "")
                                    { newCollectedAmt = "0"; }
                                    if (newTDSAmt == "")
                                    { newTDSAmt = "0"; }
                                    if (newVATAmt == "")
                                    { newVATAmt = "0"; }
                                    if (new194CAmt == "")
                                    { new194CAmt = "0"; }


                                    CollectedAmt = newTotalCollectedAmt - oldTotalCollectedAmt;

                                    TransactionType = "Credit";
                                    if (CollectedAmt != 0)
                                    {
                                        //Commented by Vijay 10-08-2014
                                        //if (ObjTrans.SaveTransacation(AgentName, AgentCode, PaymentType, TransactionType, 0, 0, Convert.ToDouble(CollectedAmt), txtPPRemarks.Text, Session["UserName"].ToString(), AWBNo, invNo))
                                        //{
                                        //}
                                    }

                                }
                                catch (Exception ex)
                                {
                                    lblStatus.Text = ex.Message;
                                    lblStatus.ForeColor = Color.Red;
                                }

                                //PopulateUserOpeningBalance();
                            }
                            else if (res == "error")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelAWBCollect();</script>", false);
                                Session["AWBCollectData"] = null;

                                //bindCollectionDetails();
                                btnList_Click(null, null);

                                lblStatus.Visible = true;
                                lblStatus.Text = "Collections not saved";
                                lblStatus.ForeColor = Color.Red;

                                return;
                            }

                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "CallPopulateClick();", true);
                        }
                    }

                    if (NewORNumber != "")
                    {
                        if (res != "error" && res != "")
                        {
                            #region Prepare Parameters
                            object[] InvoiceInfo = new object[4];
                            int i = 0;

                            InvoiceInfo.SetValue(invNo, i);
                            i++;

                            InvoiceInfo.SetValue(Session["UserName"].ToString(), i);
                            i++;

                            InvoiceInfo.SetValue(Convert.ToDateTime(Session["IT"]), i);
                            i++;

                            InvoiceInfo.SetValue(NewORNumber, i);


                            #endregion Prepare Parameters


                            res = objBAL.UpdateAgentInvoiceCollectionDetails(InvoiceInfo);

                            if (res != "error" && res != "")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelAWBCollect();</script>", false);
                                Session["AWBCollectData"] = null;

                                //bindCollectionDetails();
                                btnList_Click(null, null);

                                //PopulateUserOpeningBalance();

                                lblStatus.Visible = true;
                                lblStatus.Text = "Collections saved successfully";
                                lblStatus.ForeColor = Color.Green;
                            }
                            else if (res == "error")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelAWBCollect();</script>", false);
                                //bindCollectionDetails();
                                btnList_Click(null, null);

                                lblStatus.Visible = true;
                                lblStatus.Text = "Collections not saved";
                                lblStatus.ForeColor = Color.Red;

                                return;
                            }

                        }
                    }

                }


            }
            catch (Exception ex)
            {
            }
        }

        protected void grdAWBCollect_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddNewRow")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                DataSet dsNew = new DataSet("BillingInvoiceCollection_19");
                DataTable dtNew = new DataTable("BillingInvoiceCollection_20");
                dtNew = ((DataSet)Session["AWBCollectData"]).Tables[2];
                dtNew.Rows.Clear();

                for (int i = 0; i < grdAWBCollect.Rows.Count; i++)
                {
                    DataRow drow = dtNew.NewRow();
                    drow["SrNo"] = ((Label)grdAWBCollect.Rows[i].FindControl("lblSrNo")).Text;
                    drow["AWBNumber"] = ((Label)grdAWBCollect.Rows[i].FindControl("lblAWBNo")).Text;
                    drow["TotalAWBCharges"] = ((Label)grdAWBCollect.Rows[i].FindControl("lblTotal")).Text;
                    drow["AgentCode"] = ((Label)grdAWBCollect.Rows[i].FindControl("lblAgentCode")).Text;
                    drow["CollectedAmount"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtCollectedAmount")).Text;
                    drow["TDSAmount"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtTDSAmount")).Text;
                    drow["PaymentType"] = ((DropDownList)grdAWBCollect.Rows[i].FindControl("ddlPaymentType")).SelectedValue;
                    drow["DCMAmount"] = 0;
                    drow["DCMType"] = "";
                    drow["ChequeDdNo"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtChequeDDNo")).Text;
                    drow["ChequeDate"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtChequeDate")).Text;
                    drow["BankName"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtBankName")).Text;
                    drow["PaymentDate"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtPaymentDate")).Text;
                    drow["PendingAmount"] = ((Label)grdAWBCollect.Rows[i].FindControl("lblPendingAmount")).Text;
                    drow["Remarks"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtRemarks")).Text;
                    drow["Amt194C"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txt194CAmount")).Text;
                    drow["ORRecieptNo"] = ((Label)grdAWBCollect.Rows[i].FindControl("lblORNumber")).Text;
                    drow["DepositDate"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtDepositDate")).Text;
                    drow["VATAmount"] = ((TextBox)grdAWBCollect.Rows[i].FindControl("txtVATAmount")).Text;

                    dtNew.Rows.Add(drow);



                }

                DataTable dt = new DataTable("BillingInvoiceCollection_21");
                    dt = ((DataSet)Session["AWBCollectData"]).Tables[2];
                dt = dtNew.Copy();

                DataRow dr = null;
                dr = dt.NewRow();
                dr["SrNo"] = 0;
                dr["AWBNumber"] = dt.Rows[index]["AWBNumber"].ToString();
                dr["TotalAWBCharges"] = dt.Rows[index]["TotalAWBCharges"].ToString();
                dr["AgentCode"] = dt.Rows[index]["AgentCode"].ToString();
                dr["CollectedAmount"] = 0;
                dr["TDSAmount"] = 0;
                dr["PaymentType"] = "";
                dr["DCMAmount"] = 0;
                dr["DCMType"] = "";
                dr["ChequeDdNo"] = "";
                dr["ChequeDate"] = "";
                dr["BankName"] = "";
                dr["PaymentDate"] = "";
                dr["PendingAmount"] = dt.Rows[index]["PendingAmount"].ToString();
                dr["Remarks"] = "";
                dr["Amt194C"] = 0;
                dr["ORRecieptNo"] = "";
                dr["DepositDate"] = "";
                dr["VATAmount"] = 0;

                dt.Rows.InsertAt(dr, index + 1);

                grdAWBCollect.DataSource = dt;
                grdAWBCollect.DataBind();


                for (int j = 0; j < grdAWBCollect.Rows.Count; j++)
                {
                    //Disable AWBcollection if OR is already generated
                    if (((Label)grdAWBCollect.Rows[j].FindControl("lblORNumber")).Text.Trim() != "")
                    {
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtCollectedAmount")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtTDSAmount")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txt194CAmount")).Enabled = false;
                        ((DropDownList)grdAWBCollect.Rows[j].FindControl("ddlPaymentType")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtDCMAmount")).Enabled = false;
                        ((DropDownList)grdAWBCollect.Rows[j].FindControl("ddlDCMType")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtChequeDDNo")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtChequeDate")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtBankName")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtPaymentDate")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtRemarks")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtDepositDate")).Enabled = false;
                        ((TextBox)grdAWBCollect.Rows[j].FindControl("txtVATAmount")).Enabled = false;
                    }
                }

                dsNew.Tables.Add(((DataSet)Session["AWBCollectData"]).Tables[0].Copy());
                dsNew.Tables.Add(((DataSet)Session["AWBCollectData"]).Tables[1].Copy());
                dsNew.Tables.Add(dt.Copy());

                Session["AWBCollectData"] = dsNew;


                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelAWBCollect();</script>", false);
            }
        }

        protected void grdAWBCollect_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet dsTotal = new DataSet("BillingInvoiceCollection_22");
                dsTotal = (DataSet)Session["AWBCollectData"];

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblAWBCollectTotal = (Label)e.Row.FindControl("lblAWBCollectTotal");
                Label lblAWBCollectCollectedAmount = (Label)e.Row.FindControl("lblAWBCollectCollectedAmount");
                Label lblAWBCollectPendingAmount = (Label)e.Row.FindControl("lblAWBCollectPendingAmount");

                if (dsTotal != null && dsTotal.Tables.Count > 2 && dsTotal.Tables[3].Rows.Count > 0)
                {
                    lblAWBCollectTotal.Text = dsTotal.Tables[3].Rows[0]["TotalAWBCharges"].ToString();
                    lblAWBCollectCollectedAmount.Text = dsTotal.Tables[3].Rows[0]["CollectedAmount"].ToString();
                    lblAWBCollectPendingAmount.Text = dsTotal.Tables[3].Rows[0]["PendingAmount"].ToString();
                }
                else
                {
                    lblAWBCollectTotal.Text = "0";
                    lblAWBCollectCollectedAmount.Text = "0";
                    lblAWBCollectPendingAmount.Text = "0";
                }

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton IB = e.Row.FindControl("btnAWBCollectCreditPopup") as ImageButton;
                IB.Attributes.Add("onclick", "AWBCollectCreditCardPopup(this);return false;");

                TextBox txtCollAmt = e.Row.FindControl("txtCollectedAmount") as TextBox;
                txtCollAmt.Attributes.Add("onchange", "UpdateRunningCollectionAmount(this);return false;");
                TextBox txtTDSAmt = e.Row.FindControl("txtTDSAmount") as TextBox;
                txtTDSAmt.Attributes.Add("onchange", "UpdateRunningCollectionAmount(this);return false;");
                TextBox txtVATAmt = e.Row.FindControl("txtVATAmount") as TextBox;
                txtVATAmt.Attributes.Add("onchange", "UpdateRunningCollectionAmount(this);return false;");

            }
        }

        public static string TruncateString(string longString, int maxLength)
        {
            if (string.IsNullOrEmpty(longString) || longString.Length <= maxLength)
            {
                return longString;
            }
            else
            {
                return longString.Substring(0, maxLength);
            }
        }

        protected void btnSaveWaveOFF_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                invNo = "";

                int AWBSelected = 0;
                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    //Loop for walk-In
                    if (ddlInvoiceType.SelectedValue == "2")
                    {
                        if (((CheckBox)grdInvoiceList.Rows[y].FindControl("chkSelect")).Checked)
                        {
                            AWBSelected = AWBSelected + 1;
                            invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                        }
                    }
                    if (((RadioButton)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                    {
                        invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                    }
                }

                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else if (AWBSelected > 1) //To check if single Invoice selected for DCM for walkin
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select single Invoice to save DCM";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                if (!WaveOffValidation())
                    return;

                string InvoiceType = "Agent";
                if (ddlInvoiceType.SelectedValue == "1") //Agent
                    InvoiceType = "Agent";
                else if (ddlInvoiceType.SelectedValue == "2") //Walk-IN
                    InvoiceType = "Walkin";
                else if (ddlInvoiceType.SelectedValue == "3") //Destination
                    InvoiceType = "Dest";


                #region Prepare Parameters
                object[] RateCardInfo = new object[6];
                int j = 0;

                RateCardInfo.SetValue(invNo, j);
                j++;
                RateCardInfo.SetValue(TXTWAmount.Text.Trim(), j);
                j++;
                RateCardInfo.SetValue(TXTIssueBy.Text.Trim(), j);
                j++;
                RateCardInfo.SetValue(TXTWreason.Text.Trim(), j);
                j++;
                RateCardInfo.SetValue(Session["UserName"].ToString(), j);
                j++;
                RateCardInfo.SetValue(InvoiceType, j);

                #endregion Prepare Parameters

                string res = "";
                //Destination 
                res = objBAL.AddDestInvoiceWaivedOffDetails(RateCardInfo);

                if (res != "error")
                {
                    //bindCollectionDetails();
                    btnList_Click(null, null);

                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    TXTWreason.Text = "";
                    TXTWAmount.Text = "";
                    TXTInvoiceNo.Text = "";
                    TXTIssueBy.Text = "";
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected bool WaveOffValidation()
        {
            if (TXTWAmount.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter Waive off amount";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }
            if (TXTInvoiceNo.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter Invoice number";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (TXTWreason.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter reason";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            return true;
        }

        protected void btnPrintPDF_Click(object sender, EventArgs e)
        {
            string strInvNumber = "";
            string strInvType = "";
            if (ddlInvoiceType.SelectedIndex == 0)
                strInvType = "Regular";
            else if (ddlInvoiceType.SelectedIndex == 1)
                strInvType = "WalkIn";
            else if (ddlInvoiceType.SelectedIndex == 2)
                strInvType = "Dest";


            //for WalkIn invoice listing
            if (ddlInvoiceType.SelectedValue == "2")
            {
                for (int intCount1 = 0; intCount1 < grdInvoiceList.Rows.Count; intCount1++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[intCount1].FindControl("chkSelect")).Checked)
                    {
                        if (strInvNumber == "")
                        {
                            strInvNumber = strInvNumber + ((Label)grdInvoiceList.Rows[intCount1].FindControl("lblInvoiceNumber")).Text;
                        }
                        else
                        {
                            strInvNumber = strInvNumber + "," + ((Label)grdInvoiceList.Rows[intCount1].FindControl("lblInvoiceNumber")).Text;
                        }
                    }
                }
               
            }
            else
            {
                for (int intCount2 = 0; intCount2 < grdInvoiceList.Rows.Count; intCount2++)
                {
                    if (((RadioButton)grdInvoiceList.Rows[intCount2].FindControl("rbSelect")).Checked)
                    {
                        strInvNumber = ((Label)grdInvoiceList.Rows[intCount2].FindControl("lblInvoiceNumber")).Text;
                    }
                }
               
            }

            if (strInvNumber == "")
            { return; }

            if (strInvNumber != "")
            {
                hfInvoiceNos.Value = strInvNumber;
                hfInvoiceType.Value = strInvType;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateInvoicesPDF();", true);

            }
            else
            {
                lblStatus.Visible = true;
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please Select Atleast One Invoice.";
            }
            
        }

        protected void grdInvoiceList_DataBound(object sender, EventArgs e)
        {
            for (int i = grdInvoiceList.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = grdInvoiceList.Rows[i];
                string str = row.RowType.ToString();
                GridViewRow previousRow = grdInvoiceList.Rows[i - 1];
            }
        }

    }
}
