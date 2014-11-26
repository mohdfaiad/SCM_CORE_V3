using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.IO;
using System.Collections;
using BAL;
using System.Configuration;
using System.Data.OleDb;
using System.Drawing;
using System.Web.Services;

namespace ProjectSmartCargoManager
{
    public partial class BillingInterlineInvoice : System.Web.UI.Page
    {
        BillingInvoiceMatchingBAL objBAL = new BillingInvoiceMatchingBAL();
        BillingInterlineInvoiceBAL objIntBAL = new BillingInterlineInvoiceBAL();
        BookingBAL objBook = new BookingBAL();
        MasterBAL objBal = new MasterBAL();
        DataSet ds;
        DataSet dsCommodity;
        DataSet dsInvoices;
        DataSet dsAgent;
        static int rowind;
        static int commrowind;
        string AWBPrefix = string.Empty;
        double ST = 0.0;
        DateTime dtCurrentDate = DateTime.Now;

        decimal totalchargewt = 0, totalIATA = 0, totalocda = 0, totalocdc = 0, totaltotal = 0, totaldiscount = 0, totaltad = 0, totalrevised = 0, totaltax = 0, totalfinal = 0;
        static decimal Gtotalchargewt = 0, GtotalIATA = 0, Gtotalocda = 0, Gtotalocdc = 0, Gtotaltotal = 0, Gtotaldiscount = 0, Gtotaltad = 0, Gtotalrevised = 0, Gtotaltax = 0, Gtotalfinal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (Session["ST"] != null)
                ST = Convert.ToDouble(Session["ST"].ToString());
            else
            {
                Session["ST"] = objBal.getServiceTax();
                ST = Convert.ToDouble(Session["ST"].ToString());
            }

            dtCurrentDate = (DateTime)Session["IT"];
            if (!IsPostBack)
            {
                txtbillingfrom.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                txtbillingto.Text = dtCurrentDate.ToString("dd/MM/yyyy");

                if (Request.QueryString["id"] == "0")
                    lblHeader.Text = "Interline Invoice";
                else
                    lblHeader.Text = "Interline Credit Note";

                Panel1.Visible = false;
                LoadAgentDropdown(); //AgentName 
                LoadAgentCodeDropdown(); //AgentCode 
                GetPaymentMode();   //paymentMode
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

                string handlerPanelCalc = ClientScript.GetPostBackEventReference(this.btnPanelCalc, "");
                txtchargablewt.Attributes.Add("onblur", handlerPanelCalc);
                txtRatePerKg.Attributes.Add("onblur", handlerPanelCalc);
                txtspotrate.Attributes.Add("onblur", handlerPanelCalc);
                txtocda.Attributes.Add("onblur", handlerPanelCalc);
                txtocdc.Attributes.Add("onblur", handlerPanelCalc);
                txtDiscount.Attributes.Add("onblur", handlerPanelCalc);
                txtcommission.Attributes.Add("onblur", handlerPanelCalc);
                txtTDSCommPer.Attributes.Add("onblur", handlerPanelCalc);
                txtTDSFrtPer.Attributes.Add("onblur", handlerPanelCalc);

                string handlerSTChange = ClientScript.GetPostBackEventReference(this.btnSTChange, "");
                txtServiceTax.Attributes.Add("onblur", handlerSTChange);
            }

            //To load values from Session declared in BillingEditOCDCOCDA
            if (Session["OCTotal"] != null)
                txtocdc.Text = Session["OCTotal"].ToString();
            if (Session["OATotal"] != null)
                txtocda.Text = Session["OATotal"].ToString();


        }

        protected void disableForAgent()
        {
            //btnConfirm.Visible = false;
            //btnGenerateBill.Visible = false;
            //btnUndoFinalize.Visible = false;
            //btnProformaInvoice.Visible = false;
            //btnGenerateInvoice.Visible = false;
            //btnRouteDetails.Visible = false;
            //btnTrackAWB.Visible = false;
            pnlUpload.Visible = false;
        }

        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            DataSet ds = objBAL.GetAllAgents();
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
            DataSet ds = objBAL.GetAllAgents();
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

        #region LoadPaymentMode
        public void GetPaymentMode()
        {
            try
            {

                SQLServer db = new SQLServer(Global.GetConnectionString());

                DataSet dsPayMode = db.SelectRecords("spGetAgentPayMode");

                if (dsPayMode != null)
                {
                    if (dsPayMode.Tables.Count > 0)
                    {
                        if (dsPayMode.Tables[0].Rows.Count > 0)
                        {

                            ddlPayType.DataSource = dsPayMode;
                            ddlPayType.DataMember = dsPayMode.Tables[0].TableName;
                            //ddlPayType.DataValueField = dsPayMode.Tables[0].Columns["PayModeCode"].ColumnName;
                            ddlPayType.DataValueField = dsPayMode.Tables[0].Columns["PayModeText"].ColumnName;
                            ddlPayType.DataTextField = dsPayMode.Tables[0].Columns["PayModeText"].ColumnName;
                            ddlPayType.DataBind();
                            //ddlPayType.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception exObj)
            { }
        }

        #endregion loadPaymentMode

        #region Load Carrier Dropdown
        public void LoadCarrierDropdown()
        {
            //DataSet ds = objIntBAL.GetAllCarriers();
            //if (ds != null)
            //{
            //    ddlCarrier.DataSource = ds;
            //    ddlCarrier.DataMember = ds.Tables[0].TableName;
            //    ddlCarrier.DataTextField = "PartnerName";
            //    ddlCarrier.DataValueField = "PartnerCode";
            //    ddlCarrier.DataBind();
            //    ddlCarrier.Items.Insert(0, new ListItem("All", ""));
            //    ddlCarrier.SelectedIndex = -1;
            //}
        }
        #endregion LoadCarrierDropdown

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdBillingInfo.PageIndex = 0;
            bindGridView();
            //pnlUpload.Visible = true;
            Panel1.Visible = false;
        }

        #region BindGridView
        protected void bindGridView()
        {
            string strfromdate, strtodate;
            #region Prepare Parameters
            object[] AwbRateInfo = new object[10];
            int i = 0;

            //0
            AwbRateInfo.SetValue(ddlAgentName.SelectedValue, i);
            i++;


            //Validation for From date
            if (txtbillingfrom.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid date');</SCRIPT>");
                lblStatus.Text = "Please select Valid date";
                lblStatus.ForeColor = Color.Blue;
                return;
            }

            DateTime dt;

            try
            {
                //dt = Convert.ToDateTime(txtbillingfrom.Text);
                //Change 03082012
                string day = txtbillingfrom.Text.Substring(0, 2);
                string mon = txtbillingfrom.Text.Substring(3, 2);
                string yr = txtbillingfrom.Text.Substring(6, 4);
                strfromdate = yr + "-" + mon + "-" + day;
                dt = Convert.ToDateTime(strfromdate);

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                lblStatus.Text = "Selected Date format invalid";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            AwbRateInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;

            //Validation for To date
            if (txtbillingto.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid date');</SCRIPT>");
                lblStatus.Text = "Please select Valid date";
                lblStatus.ForeColor = Color.Blue;
                return;
            }

            DateTime dtto;

            try
            {
                //dtto = Convert.ToDateTime(txtbillingto.Text);
                //Change 03082012
                string day = txtbillingto.Text.Substring(0, 2);
                string mon = txtbillingto.Text.Substring(3, 2);
                string yr = txtbillingto.Text.Substring(6, 4);
                strtodate = yr + "-" + mon + "-" + day;
                dtto = Convert.ToDateTime(strtodate);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                lblStatus.Text = "Selected Date format invalid";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (dtto < dt)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('To date should be greater than From date');</SCRIPT>");
                lblStatus.Text = "To date should be greater than From date";
                lblStatus.ForeColor = Color.Red;
                // MessageBox.Show("Please Enter FlightID's which is not Operated");

                return;
            }



            AwbRateInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;
            //Ignore flight prefix is flight number is not entered.
            if (txtFlightNo.Text != "")
            {
                AwbRateInfo.SetValue(txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim(), i);
            }
            else
            {
                AwbRateInfo.SetValue("", i);
            }
            i++;

            AwbRateInfo.SetValue(txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(txtOrigin.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(txtDest.Text.Trim(), i);
            i++;

            AwbRateInfo.SetValue(ddlStatus.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(ddlSpotRate.SelectedValue, i);
            i++;

            AwbRateInfo.SetValue(ddlPayType.SelectedValue, i);

            #endregion Prepare Parameters

            //Code to save filter value in Hidden Fields for Export to excel
            hfAgentCode.Value = ddlAgentName.SelectedValue;
            hfAgentName.Value = ddlAgentCode.SelectedValue;
            hfFromDate.Value = Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss");
            hfToDate.Value = Convert.ToDateTime(strtodate).ToString("yyyy-MM-dd HH:mm:ss");
            if (txtFlightNo.Text != "")
            {
                hfFlightNo.Value = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
            }
            else
            {
                hfFlightNo.Value = "";
            }
            
            hfAWBNumber.Value = txtAWBNumber.Text.Trim();
            hfOrigin.Value = txtOrigin.Text.Trim();
            hfDestination.Value = txtDest.Text.Trim();
            hfStatus.Value = ddlStatus.SelectedValue;
            hfSPotRate.Value = ddlSpotRate.SelectedValue;
            hfPaymentMode.Value = ddlPayType.SelectedValue;
            ////////////////////////////////////////////////////////////////

            if (Request.QueryString["id"] == "0")
                ds = objIntBAL.GetInterlineInvoiceAWBList(AwbRateInfo);
            else
                ds = objIntBAL.GetInterlineCreditNoteAWBList(AwbRateInfo);


            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {

                                //Code to show Grant Total in Footer of Gridview.
                                Gtotalchargewt = 0; GtotalIATA = 0; Gtotalocda = 0; Gtotalocdc = 0; Gtotaltotal = 0; Gtotaldiscount = 0; Gtotaltad = 0; Gtotalrevised = 0; Gtotaltax = 0; Gtotalfinal = 0;
                                for (int cnt = 0; cnt < ds.Tables[0].Rows.Count; cnt++)
                                {
                                    Gtotalchargewt += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["ChargedWeight"].ToString());
                                    GtotalIATA += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["FreightRate"].ToString());
                                    Gtotalocda += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["OCDueAgent"].ToString());
                                    Gtotalocdc += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["OCDueCar"].ToString());
                                    Gtotaltotal += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["TotalT"].ToString());
                                    Gtotaldiscount += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["DiscountAmt"].ToString());
                                    Gtotaltad += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["TAD"].ToString());
                                    Gtotalrevised += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["RevisedTotal"].ToString());
                                    Gtotaltax += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["TDSAmt"].ToString());
                                    Gtotalfinal += Convert.ToDecimal(ds.Tables[0].Rows[cnt]["Final"].ToString());
                                }
                                Session["dsDetails"] = ds;
                                grdBillingInfo.DataSource = ds.Tables[0];
                                grdBillingInfo.DataBind();
                                grdBillingInfo.Visible = true;
                                pnlCommodityGrid.Visible = false;

                                //visible all the buttons 
                                btnConfirm.Visible = true;
                                btnGenerateBill.Visible = true;
                                btnUndoFinalize.Visible = true;
                                btnProformaInvoice.Visible = true;
                                btnGenerateInvoice.Visible = true;
                                btnTrackAWB.Visible = true;
                                btnPrint.Visible = true;
                                btnRouteDetails.Visible = false;
                                lblAWBCount.Visible = true;
                                pnlUpload.Visible = true;
                                lblStatus.Text = "";
                                lblAWBCount.Text = "Total Count: " + ds.Tables[0].Rows.Count.ToString();

                                //Agent Authorization
                                if (Convert.ToString(Session["AgentCode"]) != "")
                                {
                                    disableForAgent();
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
                            lblAWBCount.Text = "Total Count: " + "0";
                            lblStatus.ForeColor = Color.Blue;
                            btnConfirm.Visible = false;
                            btnGenerateBill.Visible = false;
                            btnUndoFinalize.Visible = false;
                            btnProformaInvoice.Visible = false;
                            btnGenerateInvoice.Visible = false;
                            btnTrackAWB.Visible = false;
                            btnPrint.Visible = false;
                            btnRouteDetails.Visible = false;
                            grdBillingInfo.Visible = false;
                            lblAWBCount.Visible = false;
                            Panel1.Visible = false;
                            pnlUpload.Visible = false;
                            Session["dsDetails"] = null;
                            //ClearPanel();
                            return;
                        }

                    }
                }
            }

        }
        #endregion BindGridView

        protected void ddlAgentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentName.SelectedIndex = ddlAgentCode.SelectedIndex;
        }

        protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentCode.SelectedIndex = ddlAgentName.SelectedIndex;
        }

        protected void grdBillingInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = (DataSet)Session["dsDetails"];
            grdBillingInfo.PageIndex = e.NewPageIndex;
            grdBillingInfo.DataSource = dst.Tables[0];
            grdBillingInfo.DataBind();
        }

        protected void grdBillingInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AWBClick")
            {
                //FieldValidation();
                rowind = Convert.ToInt32(e.CommandArgument) + (grdBillingInfo.PageIndex * grdBillingInfo.PageSize);
                bindCommGridOrPanel();
                Page.ClientScript.RegisterStartupScript(GetType(), "ScrollKey", "pageScroll1();", true);
            }
        }

        protected void bindCommGridOrPanel()
        {
            //making Session null declared in BillingEditOCDCOCDA
            Session["OCTotal"] = null;
            Session["OATotal"] = null;


            DataSet dsdetails = (DataSet)Session["dsDetails"];
            string strfrom, strto;
            #region Prepare Parameters
            object[] AwbRateInfo = new object[5];
            int i = 0;

            AwbRateInfo.SetValue(dsdetails.Tables[0].Rows[rowind]["AWBPrefix"].ToString() + dsdetails.Tables[0].Rows[rowind]["AWBNumber"].ToString(), i);
            i++;

            AwbRateInfo.SetValue(dsdetails.Tables[0].Rows[rowind]["AgentCode"].ToString(), i);
            i++;

            string dayfrom = txtbillingfrom.Text.Substring(0, 2);
            string monfrom = txtbillingfrom.Text.Substring(3, 2);
            string yrfrom = txtbillingfrom.Text.Substring(6, 4);
            strfrom = yrfrom + "-" + monfrom + "-" + dayfrom;
            AwbRateInfo.SetValue(Convert.ToDateTime(strfrom).ToString("yyyy-MM-dd HH:mm:ss"), i);
            i++;

            string dayto = txtbillingto.Text.Substring(0, 2);
            string monto = txtbillingto.Text.Substring(3, 2);
            string yrto = txtbillingto.Text.Substring(6, 4);
            strto = yrto + "-" + monto + "-" + dayto;
            AwbRateInfo.SetValue(Convert.ToDateTime(strto).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), i);

            i++;

            AwbRateInfo.SetValue(dsdetails.Tables[0].Rows[rowind]["PartnerCode"].ToString(), i);
            #endregion Prepare Parameters

            if (Request.QueryString["id"] == "0")
                dsCommodity = objIntBAL.GetInterlineInvoiceAWBCommodityList(AwbRateInfo);
            else
                dsCommodity = objIntBAL.GetInterlineCreditNoteAWBCommodityList(AwbRateInfo);


            if (dsCommodity != null)
            {
                if (dsCommodity.Tables != null)
                {
                    if (dsCommodity.Tables.Count > 0)
                    {
                        //Code to show only more than one commodities on click of AWB
                        //if (dsCommodity.Tables[0].Rows.Count > 1)

                        //Showing single as well as multiple commodities on click of AWB
                        if (dsCommodity.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["dsCommodity"] = dsCommodity;

                                grdCommodity.DataSource = dsCommodity.Tables[0];
                                grdCommodity.DataBind();

                                //visible all the buttons
                                //btnVerify.Visible = true;
                                //btnApproved.Visible = true;
                                //btnConfirmInvoice.Visible = true;
                                //btnGenerateInvoice.Visible = true;
                                //btnGenerateProforma.Visible = true;
                                lblStatus.Text = "";
                                pnlCommodityGrid.Visible = true;
                                grdCommodity.Visible = true;
                                Panel1.Visible = false;
                                ClearPanel();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            Panel1.Visible = true;
                            pnlCommodityGrid.Visible = false;
                            //fillBillingDetails(rowind);

                        }

                    }
                }
            }
        }

        protected void ClearPanel()
        {
            txtAWBPrefixPnl.Text = "";
            txtAWBNo.Text = "";
            txtOrgPnl.Text = "";
            txtDestPnl.Text = "";
            txtAgentName.Text = "";
            txtgrosswt.Text = "";
            txtRatePerKg.Text = "";
            txtchargablewt.Text = "";
            txtfreightrate.Text = "";
            txtspotrate.Text = "";
            txtocdc.Text = "";
            txtocda.Text = "";
            txtcommodity.Text = "";
            txtdimensions.Text = "";
            txtTotal.Text = "";
            txtServiceTax.Text = "";
            txtDiscount.Text = "";
            txtdiscamount.Text = "";
            txttotalafterdiscount.Text = "";
            txtTADST.Text = "";
            txtcommission.Text = "";
            txtcommissionamt.Text = "";
            txtSTOnComm.Text = "";
            txtTDSCommPer.Text = "";
            txtTDSCommAmt.Text = "";
            txtRevisedTotal.Text = "";
            txtTDSFrtPer.Text = "";
            txtTDSFrtAmt.Text = "";
            txttotalaftertax.Text = "";
            Panel1.Visible = false;

        }

        protected void grdBillingInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdCommodity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataSet dsCommodity = (DataSet)Session["dsCommodity"];
            Panel1.Visible = true;
            commrowind = Convert.ToInt32(e.CommandArgument);

            fillCommodityBillingDetails(commrowind);
            //Code to fetch OCDC OCDA breakup to show in Panel
            OCDCOCDABreakup();

            Session["OCTotal"] = null;
            Session["OATotal"] = null;

            Page.ClientScript.RegisterStartupScript(GetType(), "ScrollKey", "pageScroll2();", true);
        }

        protected void fillCommodityBillingDetails(int ind)
        {
            btnRouteDetails.Visible = true;
            DataSet dsdetails = (DataSet)Session["dsCommodity"];
            //if(dsdetails.Tables[0].Rows[ind]["Confirmed"].ToString() == "Invoiced")
            //{
            //    Panel1.Enabled = false;
            //    btnUndoFinalize.Enabled = false;
            //}
            //else if (dsdetails.Tables[0].Rows[ind]["Confirmed"].ToString() == "Final" || dsdetails.Tables[0].Rows[ind]["Confirmed"].ToString() == "Invoiced")
            //{
            //    Panel1.Enabled = false;
            //}
            //else
            //{
            //    Panel1.Enabled = true;
            //    btnUndoFinalize.Enabled = true;
            //}

            if (dsdetails.Tables[0].Rows[ind]["Confirmed"].ToString() == "Invoiced")
            {
                btnUndoFinalize.Enabled = false;
            }
            else if (dsdetails.Tables[0].Rows[ind]["Confirmed"].ToString() == "Final" || dsdetails.Tables[0].Rows[ind]["Confirmed"].ToString() == "Invoiced")
            {
                txtgrosswt.ReadOnly = true;
                txtRatePerKg.ReadOnly = true;
                txtchargablewt.ReadOnly = true;
                txtServiceTax.ReadOnly = true;
                txtDiscount.ReadOnly = true;
                txtcommission.ReadOnly = true;
                txtTDSCommPer.ReadOnly = true;
                btnOcDueAgent.Enabled = true;
                btnOcDueCar.Enabled = true;

            }
            else
            {
                Panel1.Enabled = true;
                btnUndoFinalize.Enabled = true;

                txtgrosswt.ReadOnly = false;
                txtRatePerKg.ReadOnly = false;
                txtchargablewt.ReadOnly = false;
                txtServiceTax.ReadOnly = false;
                txtDiscount.ReadOnly = false;
                txtcommission.ReadOnly = false;
                txtTDSCommPer.ReadOnly = false;

            }

            txtAWBPrefixPnl.Text = dsdetails.Tables[0].Rows[ind]["AWBPrefix"].ToString();
            txtAWBNo.Text = dsdetails.Tables[0].Rows[ind]["AWBNumber"].ToString();
            txtOrgPnl.Text = dsdetails.Tables[0].Rows[ind]["Origin"].ToString();
            txtDestPnl.Text = dsdetails.Tables[0].Rows[ind]["Destination"].ToString();
            txtAWBDatePnl.Text = dsdetails.Tables[0].Rows[ind]["AWBDate"].ToString();
            txtPaymentTypePnl.Text = dsdetails.Tables[0].Rows[ind]["PayMode"].ToString();
            txtFlightNoPnl.Text = dsdetails.Tables[0].Rows[ind]["FlightNumber"].ToString();
            txtFlightDatePnl.Text = dsdetails.Tables[0].Rows[ind]["FlightDate"].ToString();
            txtSpotRateID.Text = dsdetails.Tables[0].Rows[ind]["SpotRateID"].ToString();
            txtAgentName.Text = dsdetails.Tables[0].Rows[ind]["AgentName"].ToString();
            txtgrosswt.Text = dsdetails.Tables[0].Rows[ind]["GrossWeight"].ToString();
            txtchargablewt.Text = dsdetails.Tables[0].Rows[ind]["ChargedWeight"].ToString();
            txtRatePerKg.Text = dsdetails.Tables[0].Rows[ind]["RatePerKG"].ToString();
            txtfreightrate.Text = dsdetails.Tables[0].Rows[ind]["FreightRate"].ToString();
            txtspotrate.Text = dsdetails.Tables[0].Rows[ind]["SpotFreight"].ToString();

            if (Session["OATotal"] == null)
                txtocda.Text = dsdetails.Tables[0].Rows[ind]["OCDueAgent"].ToString();
            else
                txtocda.Text = Session["OATotal"].ToString();

            //To disable Imagebutton for BreakUp if OCDA is Zero
            if (txtocda.Text.Trim() == "0")
                btnOcDueAgent.Enabled = false;
            else
                btnOcDueAgent.Enabled = true;

            if (Session["OCTotal"] == null)
                txtocdc.Text = dsdetails.Tables[0].Rows[ind]["OCDueCar"].ToString();
            else
                txtocdc.Text = Session["OCTotal"].ToString();

            //To disable Imagebutton for BreakUp if OCDC is Zero
            if (txtocdc.Text.Trim() == "0")
                btnOcDueCar.Enabled = false;
            else
                btnOcDueCar.Enabled = true;

            txtcommodity.Text = dsdetails.Tables[0].Rows[ind]["CommodityCode"].ToString();
            txtdimensions.Text = dsdetails.Tables[0].Rows[ind]["Dimensions"].ToString();

            txtTotal.Text = dsdetails.Tables[0].Rows[ind]["TotalT"].ToString();
            txtDiscount.Text = dsdetails.Tables[0].Rows[ind]["Discount"].ToString();
            txtdiscamount.Text = dsdetails.Tables[0].Rows[ind]["DiscountAmt"].ToString();
            txttotalafterdiscount.Text = "";
            txtTADST.Text = "";
            txtcommission.Text = dsdetails.Tables[0].Rows[ind]["Commission"].ToString();
            txtcommissionamt.Text = dsdetails.Tables[0].Rows[ind]["CommissionAmt"].ToString();
            txtRevisedTotal.Text = "";

            //TDS on Freignt % and Amount
            txtTDSFrtPer.Text = dsdetails.Tables[0].Rows[ind]["TDS"].ToString();
            txtTDSFrtAmt.Text = dsdetails.Tables[0].Rows[ind]["TDSAmt"].ToString();

            //TDS on Commission % and Amount
            txtTDSCommPer.Text = dsdetails.Tables[0].Rows[ind]["TDSOnComm"].ToString();
            txtTDSCommAmt.Text = dsdetails.Tables[0].Rows[ind]["TDSOnCommAmt"].ToString();

            txtServiceTax.Text = dsdetails.Tables[0].Rows[ind]["ServiceTax"].ToString();
            txtSTOnComm.Text = dsdetails.Tables[0].Rows[ind]["STOnComm"].ToString();
            txttotalaftertax.Text = "";


            //calculation for Total amounts
            txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text.Trim()) - Convert.ToDouble(txtdiscamount.Text.Trim()), 2).ToString();
            txtTADST.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text.Trim()) + Convert.ToDouble(txtServiceTax.Text.Trim()), 2).ToString();
            txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtTADST.Text.Trim()) - (Convert.ToDouble(txtcommissionamt.Text.Trim()) + Convert.ToDouble(txtSTOnComm.Text.Trim())) + Convert.ToDouble(txtTDSCommAmt.Text.Trim()), 2).ToString();
            //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text.Trim()) - Convert.ToDouble(txtTDSFrtAmt.Text.Trim()), 2).ToString();
            txttotalaftertax.Text = txtRevisedTotal.Text.Trim();

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //Confirm Single AWB number
            if (Panel1.Visible == true)
            {
                try
                {
                    #region Validation
                    if (txtDiscount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount %";
                        lblStatus.ForeColor = Color.Blue;
                        txtDiscount.Focus();
                        return;
                    }

                    if (txtdiscamount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount amount";
                        lblStatus.ForeColor = Color.Blue;
                        txtdiscamount.Focus();
                        return;
                    }
                    if (txtTDSFrtPer.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                        lblStatus.Text = "Please enter TDS %";
                        lblStatus.ForeColor = Color.Blue;
                        txtTDSFrtPer.Focus();
                        return;
                    }
                    #endregion Validation



                    #region Prepare Parameters
                    object[] RateCardInfo = new object[24];
                    int i = 0;

                    //0
                    RateCardInfo.SetValue(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text, i);
                    i++;

                    //AgentCode and AWBDate calculation in SP

                    RateCardInfo.SetValue(txtcommodity.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdimensions.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtgrosswt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtchargablewt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtfreightrate.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtocdc.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtocda.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTotal.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtDiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdiscamount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommission.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommissionamt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtServiceTax.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalaftertax.Text, i);

                    i++;
                    string UserName = Session["UserName"].ToString();
                    RateCardInfo.SetValue(UserName, i);
                    i++;

                    RateCardInfo.SetValue(System.DateTime.Now, i);
                    i++;

                    RateCardInfo.SetValue(((DataSet)Session["dsCommodity"]).Tables[0].Rows[commrowind]["PartnerCode"].ToString(), i);

                    #endregion Prepare Parameters

                    string res = "";

                    if (Request.QueryString["id"] == "0")
                        res = objIntBAL.ConfirmSingleInterLineInvoiceAWB(RateCardInfo);
                    else
                        res = objIntBAL.ConfirmSingleInterLineCreditNoteAWB(RateCardInfo);


                    if (res != "error")
                    {
                        //Code to Calculate Spot Rate to update it in [BillingAWBInvoiceMatching] table
                        CalculateAndUpdateSpotRate(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text);

                        SaveOCDCOCDAChanges(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text);

                        ClearPanel();
                        bindGridView();

                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {

                }

            }
            else // Confirm checked AWB Number
            {
                string awbList = "";
                ds = (DataSet)Session["dsDetails"];
                for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[y].FindControl("ChkSelect")).Checked)
                    {
                        if (ds.Tables[0].Rows[y]["Confirmed"].ToString() == "Pending" || ds.Tables[0].Rows[y]["Confirmed"].ToString() == "New")
                        {
                            if (awbList == "")
                            {
                                awbList = awbList + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                            else
                            {
                                awbList = awbList + "," + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                        }
                        else
                        {
                            //lblStatus.Text = "AWB's with status 'Pending' can only be 'Confirmed'";
                            //lblStatus.ForeColor = Color.Blue;
                            //return;
                        }
                    }

                }

                if (awbList != "")
                {
                    for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                    {
                        if (((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked)
                        {
                            try
                            {
                                ds = (DataSet)Session["dsDetails"];

                                #region Prepare Parameters
                                object[] RateCardInfo = new object[24];
                                int irow = 0;

                                //0
                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["AWBPrefix"].ToString() + ds.Tables[0].Rows[i]["AWBNumber"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["CommodityCode"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Dimensions"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["GrossWeight"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["ChargedWeight"].ToString(), irow);
                                irow++;

                                //RateCardInfo.SetValue(ds.Tables[0].Rows[i]["FrtIATA"].ToString(), irow);
                                //irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["FreightRate"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["OCDueCar"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["OCDueAgent"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TotalT"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Discount"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["DiscountAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TAD"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Commission"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["CommissionAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["RevisedTotal"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDS"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDSAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDSOnComm"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["TDSOnCommAmt"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["ServiceTax"].ToString(), irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["Final"].ToString(), irow);

                                irow++;

                                string UserName = Session["UserName"].ToString();
                                RateCardInfo.SetValue(UserName, irow);
                                irow++;

                                RateCardInfo.SetValue(System.DateTime.Now, irow);
                                irow++;

                                RateCardInfo.SetValue(ds.Tables[0].Rows[i]["PartnerCode"].ToString(), irow);

                                #endregion Prepare Parameters

                                string res = "";

                                if (Request.QueryString["id"] == "0")
                                    res = objIntBAL.ConfirmSingleInterLineInvoiceAWB(RateCardInfo);
                                else
                                    res = objIntBAL.ConfirmSingleInterLineCreditNoteAWB(RateCardInfo);


                                if (res != "error")
                                {
                                    //Code to Calculate Spot Rate to update it in [BillingAWBInvoiceMatching] table
                                    CalculateAndUpdateSpotRate(ds.Tables[0].Rows[i]["AWBPrefix"].ToString() + ds.Tables[0].Rows[i]["AWBNumber"].ToString());

                                    OCDCOCDABreakup(ds.Tables[0].Rows[i]["AWBPrefix"].ToString() + ds.Tables[0].Rows[i]["AWBNumber"].ToString());

                                    SaveOCDCOCDAChanges(ds.Tables[0].Rows[i]["AWBPrefix"].ToString() + ds.Tables[0].Rows[i]["AWBNumber"].ToString());

                                    txtAWBPrefixPnl.Text = "";
                                    txtAWBNo.Text = "";
                                    txtOrgPnl.Text = "";
                                    txtDestPnl.Text = "";
                                    txtAgentName.Text = "";
                                    txtgrosswt.Text = "";
                                    txtchargablewt.Text = "";
                                    txtRatePerKg.Text = "";

                                    txtfreightrate.Text = "";
                                    txtocdc.Text = "";
                                    txtocda.Text = "";
                                    txtcommodity.Text = "";
                                    txtdimensions.Text = "";
                                    txtTotal.Text = "";
                                    txtDiscount.Text = "";
                                    txtdiscamount.Text = "";
                                    txttotalafterdiscount.Text = "";
                                    txtTADST.Text = "";
                                    txtcommission.Text = "";
                                    txtcommissionamt.Text = "";
                                    txtRevisedTotal.Text = "";
                                    txtTDSFrtPer.Text = "";
                                    txtTDSFrtAmt.Text = "";
                                    txtTDSCommPer.Text = "";
                                    txtTDSCommAmt.Text = "";
                                    txtServiceTax.Text = "";
                                    txtSTOnComm.Text = "";
                                    txttotalaftertax.Text = "";
                                    Panel1.Visible = false;


                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Visible = true;
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Blue;


                                }
                                else
                                {
                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Red;
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select AWB Numbers to generate invoice');</SCRIPT>");
                    lblStatus.Text = "Select AWB numbers with status 'Pending' or 'New' to Confirm";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                bindGridView();
            }
        }

        protected void btnGenerateBill_Click(object sender, EventArgs e)
        {
            //Code to Update AWB's selected to status Final and change Finalization date in DB
            if (Panel1.Visible == true)
            {
                try
                {
                    #region validation
                    if (txtDiscount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount % ');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount %";
                        lblStatus.ForeColor = Color.Blue;
                        txtDiscount.Focus();
                        return;
                    }

                    if (txtdiscamount.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Discount amounut');</SCRIPT>");
                        lblStatus.Text = "Please enter Discount amount";
                        lblStatus.ForeColor = Color.Blue;
                        txtdiscamount.Focus();
                        return;
                    }
                    if (txtTDSFrtPer.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter TDS % ');</SCRIPT>");
                        lblStatus.Text = "Please enter TDS %";
                        lblStatus.ForeColor = Color.Blue;
                        txtTDSFrtPer.Focus();
                        return;
                    }
                    #endregion validation


                    #region Prepare Parameters
                    object[] RateCardInfo = new object[23];
                    int i = 0;

                    //0
                    RateCardInfo.SetValue(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text.Trim(), i);
                    i++;

                    //AgentCode and AWBDate calculation in SP

                    RateCardInfo.SetValue(txtcommodity.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdimensions.Text, i);
                    i++;

                    //1
                    RateCardInfo.SetValue(txtgrosswt.Text, i);
                    i++;

                    //2
                    RateCardInfo.SetValue(txtchargablewt.Text, i);
                    i++;

                    //3
                    RateCardInfo.SetValue(txtfreightrate.Text, i);
                    i++;

                    //4
                    RateCardInfo.SetValue(txtocdc.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtocda.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTotal.Text, i);
                    i++;


                    RateCardInfo.SetValue(txtDiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtdiscamount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalafterdiscount.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommission.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtcommissionamt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtRevisedTotal.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSFrtAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommPer.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtTDSCommAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtServiceTax.Text, i);
                    i++;

                    RateCardInfo.SetValue(txttotalaftertax.Text, i);

                    i++;
                    string UserName = Session["UserName"].ToString();
                    RateCardInfo.SetValue(UserName, i);
                    i++;

                    RateCardInfo.SetValue(System.DateTime.Now, i);

                    #endregion Prepare Parameters

                    string res = "";
                    if (Request.QueryString["id"] == "0")
                        res = objIntBAL.FinalizeSingleInterLineInvoiceAWB(RateCardInfo);
                    else
                        res = objIntBAL.FinalizeSingleInterLineCreditNoteAWB(RateCardInfo);


                    if (res != "error")
                    {
                        //Code to Calculate Spot Rate to update it in [BillingAWBInvoiceMatching] table
                        CalculateAndUpdateSpotRate(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text);
                        SaveOCDCOCDAChanges(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text.Trim());

                        ClearPanel();
                        bindGridView();

                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;


                    }
                    else
                    {
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                string awbList = "";
                DataSet dsdetailsN = (DataSet)Session["dsDetails"];
                for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[y].FindControl("ChkSelect")).Checked)
                    {
                        if (dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Pending" || dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Confirmed" || dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Reopened")
                        {
                            if (awbList == "")
                            {
                                awbList = awbList + dsdetailsN.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                            else
                            {
                                awbList = awbList + "," + dsdetailsN.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                        }
                        else
                        {
                            //lblStatus.Text = "AWB's with status 'Confirmed' can only be 'Finalized'";
                            //lblStatus.ForeColor = Color.Blue;
                            //return;
                        }
                    }

                }

                if (awbList != "")
                {

                    try
                    {
                        #region Prepare Parameters
                        object[] AWBInfo = new object[3];
                        int irow = 0;

                        AWBInfo.SetValue(awbList, irow);

                        irow++;
                        string UserName = Session["UserName"].ToString();
                        AWBInfo.SetValue(UserName, irow);
                        irow++;

                        AWBInfo.SetValue(System.DateTime.Now, irow);



                        #endregion Prepare Parameters

                        string res = "";
                        if (Request.QueryString["id"] == "0")
                            res = objIntBAL.FinalizeSelectedInterLineInvoiceAWB(AWBInfo);
                        else
                            res = objIntBAL.FinalizeSelectedInterLineCreditNoteAWB(AWBInfo);


                        if (res != "error")
                        {
                            //Code to Calculate Spot Rate to update it in [BillingAWBInvoiceMatching] table
                            Array arrAWB = awbList.Split(',');
                            foreach (string AWBNum in arrAWB)
                            {
                                CalculateAndUpdateSpotRate(AWBNum);
                            }

                            bindGridView();
                            lblStatus.Text = res;
                            lblStatus.ForeColor = Color.Green;
                            return;

                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                            lblStatus.Text = res;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    //lblStatus.Text = "Select AWB numbers with status 'Confirmed' to Finalize";
                    lblStatus.Text = "AWBs with status New or Invoiced can not be finalized";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }
        }

        protected void CalculateAndUpdateSpotRate(string AWBNumber)
        {
            try
            {
                #region Prepare Parameters
                object[] AWBInfo = new object[1];
                int irow = 0;

                AWBInfo.SetValue(AWBNumber, irow);

                #endregion Prepare Parameters

                string res = "";
                res = objBAL.CalculateAndUpdateSpotRate(AWBInfo);


                //if (res != "error")
                //{
                //    bindGridView();
                //    lblStatus.Text = res;
                //    lblStatus.ForeColor = Color.Green;
                //    return;
                //}
                //else
                //{
                //    lblStatus.Text = res;
                //    lblStatus.ForeColor = Color.Red;
                //    return;
                //}
            }
            catch
            {
            }
        }

        protected void btnUndoFinalize_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == true)
            {
                try
                {

                    #region Prepare Parameters
                    object[] RateCardInfo = new object[3];
                    int irow = 0;

                    RateCardInfo.SetValue(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text.Trim(), irow);

                    irow++;
                    string UserName = Session["UserName"].ToString();
                    RateCardInfo.SetValue(UserName, irow);
                    irow++;

                    RateCardInfo.SetValue(System.DateTime.Now, irow);


                    #endregion Prepare Parameters

                    string res = "";
                    if (Request.QueryString["id"] == "0")
                        res = objIntBAL.UndoFinalizeSingleInterLineInvoiceAWB(RateCardInfo);
                        
                    else
                        res = objIntBAL.UndoFinalizeSingleInterLineCreditNoteAWB(RateCardInfo);
                    


                    if (res != "error")
                    {
                        ClearPanel();
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Blue;


                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {

                }


            }
            else
            {
                string awbList = "";
                ds = (DataSet)Session["dsDetails"];
                for (int y = 0; y < grdBillingInfo.Rows.Count; y++)
                {
                    if (((CheckBox)grdBillingInfo.Rows[y].FindControl("ChkSelect")).Checked)
                    {
                        if (ds.Tables[0].Rows[y]["Confirmed"].ToString() == "Final")
                        {
                            if (awbList == "")
                            {
                                awbList = awbList + ds.Tables[0].Rows[y]["AWBPrefix"].ToString() + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                            else
                            {
                                awbList = awbList + "," + ds.Tables[0].Rows[y]["AWBPrefix"].ToString() + ds.Tables[0].Rows[y]["AWBNumber"].ToString();
                            }
                        }
                        else
                        {
                            //lblStatus.Text = "AWB's with status 'Pending' can only be 'Confirmed'";
                            //lblStatus.ForeColor = Color.Blue;
                            //return;
                        }
                    }

                }

                if (awbList != "")
                {
                    for (int i = 0; i < grdBillingInfo.Rows.Count; i++)
                    {
                        if (((CheckBox)grdBillingInfo.Rows[i].FindControl("ChkSelect")).Checked)
                        {
                            try
                            {
                                ds = (DataSet)Session["dsDetails"];

                                #region Prepare Parameters
                                object[] RateCardInfo = new object[3];
                                int irow = 0;

                                //0
                                RateCardInfo.SetValue(awbList, irow);

                                irow++;
                                string UserName = Session["UserName"].ToString();
                                RateCardInfo.SetValue(UserName, irow);
                                irow++;

                                RateCardInfo.SetValue(System.DateTime.Now, irow);

                                #endregion Prepare Parameters

                                string res = "";
                                if (Request.QueryString["id"] == "0")
                                    res = objIntBAL.UndoFinalizeSelectedInterLineInvoiceAWB(RateCardInfo);

                                else
                                    res = objIntBAL.UndoFinalizeSelectedInterLineCreditNoteAWB(RateCardInfo);


                                if (res != "error")
                                {
                                    ClearPanel();


                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Blue;


                                }
                                else
                                {
                                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                                    lblStatus.Text = res;
                                    lblStatus.ForeColor = Color.Red;
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                else
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select AWB Numbers to generate invoice');</SCRIPT>");
                    lblStatus.Text = "Undo Finalization can be done only for AWB numbers with status 'Final'";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
            }

            bindGridView();
        }

        protected void btnGenerateInvoice_Click(object sender, EventArgs e)
        {
            generateInvoice();
        }

        protected void generateInvoice()
        {
            //Code to Generate Invoice for the AWB's with status Final.

            //Code to add AWB int AWBList with status Final.

            //string awbList = "";
            System.Text.StringBuilder awbList = new System.Text.StringBuilder();
            DataSet dsdetailsN = (DataSet)Session["dsDetails"];
            for (int y = 0; y < dsdetailsN.Tables[0].Rows.Count; y++)
            {
                if (dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Final")
                {
                    if (awbList.ToString() == "")
                    {
                        awbList.Append(dsdetailsN.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString());
                    }
                    else
                    {
                        awbList.Append("," + dsdetailsN.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString());
                    }
                }
                else
                {
                    //lblStatus.Text = "Invoice can be generated only for Finalized AWB's";
                    //lblStatus.ForeColor = Color.Blue;
                    //return;
                }

            }


            #region Prepare Parameters
            object[] AWBInfo = new object[3];
            int i = 0;

            AWBInfo.SetValue(awbList.ToString(), i);

            i++;
            string UserName = Session["UserName"].ToString();
            AWBInfo.SetValue(UserName, i);
            i++;

            AWBInfo.SetValue(System.DateTime.Now, i);


            #endregion Prepare Parameters

            if (awbList.ToString() != "")
            {
                string res = "";
                try
                {
                    if (Request.QueryString["id"] == "0")
                    {
                        res = objIntBAL.GenerateBunchInterlineInvoiceNumber(AWBInfo);
                        dsInvoices = objIntBAL.GetInterlineInvoiceNumber(AWBInfo);
                    }
                    else
                    {
                        res = objIntBAL.GenerateBunchInterlineCreditNoteNumber(AWBInfo);
                        dsInvoices = objIntBAL.GetInterlineCreditNoteNumber(AWBInfo);
                    }

                    if (dsInvoices != null)
                    {
                        if (dsInvoices.Tables != null)
                        {
                            if (dsInvoices.Tables.Count > 0)
                            {
                                if (dsInvoices.Tables[0].Rows.Count > 0)
                                {
                                    for (int invCnt = 0; invCnt < dsInvoices.Tables[0].Rows.Count; invCnt++)
                                    {
                                        #region Prepare Parameters
                                        object[] InvInfo = new object[3];
                                        int inv = 0;

                                        InvInfo.SetValue(dsInvoices.Tables[0].Rows[invCnt]["InvoiceNumber"].ToString(), inv);
                                        inv++;

                                        InvInfo.SetValue(System.DateTime.Now, inv);
                                        inv++;

                                        InvInfo.SetValue(UserName, inv);

                                        #endregion Prepare Parameters

                                        try
                                        {
                                            //Code to update BillingInvoiceImport with sum of fields in BillingAWBImport
                                            if (Request.QueryString["id"] == "0")
                                                res = objIntBAL.UpdateBillingInterlineInvoiceSummary(InvInfo);
                                            else
                                                res = objIntBAL.UpdateBillingInterlineCreditNoteSummary(InvInfo);
                                            
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Commented by Vijay on 20/12/2013
                    #region Update Invoice amount from CCA and DCM
                    //if (dsInvoices != null)
                    //{
                    //    if (dsInvoices.Tables != null)
                    //    {
                    //        if (dsInvoices.Tables.Count > 0)
                    //        {
                    //            if (dsInvoices.Tables[0].Rows.Count > 0)
                    //            {
                    //                for (int invCnt = 0; invCnt < dsInvoices.Tables[0].Rows.Count; invCnt++)
                    //                {
                    //                    #region Prepare Parameters
                    //                    object[] InvInfo = new object[1];
                    //                    int inv = 0;

                    //                    InvInfo.SetValue(dsInvoices.Tables[0].Rows[invCnt]["InvoiceNumber"].ToString(), inv);

                    //                    #endregion Prepare Parameters

                    //                    try
                    //                    {
                    //                        //Code to Change Invoice Amount depending on CCA/DCM
                    //                        res = objBAL.UpdateBillingInvoiceAmtFromCCADCM(InvInfo);
                    //                    }
                    //                    catch (Exception ex)
                    //                    {
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion Update Invoice amount from CCA and DCM

                    //------------------------------------------Added for Loop---------------------------------------
                    bindGridView(); //To get latest records

                    DataSet dsdetailsInv = (DataSet)Session["dsDetails"];
                    //Code to Check if any AWBs are not invoiced
                    System.Text.StringBuilder awbList1 = new System.Text.StringBuilder();
                    if (dsdetailsInv != null)
                    {
                        for (int y = 0; y < dsdetailsInv.Tables[0].Rows.Count; y++)
                        {
                            if (dsdetailsInv.Tables[0].Rows[y]["Confirmed"].ToString() == "Final" && (Convert.ToString(dsdetailsInv.Tables[0].Rows[y]["InvoiceNumber"]).Trim() == "" || Convert.ToString(dsdetailsInv.Tables[0].Rows[y]["InvoiceNumber"]).Trim().Substring(0, 8) == "Proforma"))
                            {
                                if (awbList1.ToString() == "")
                                {
                                    awbList1.Append(dsdetailsInv.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsInv.Tables[0].Rows[y]["AWBNumber"].ToString());
                                }
                                else
                                {
                                    awbList1.Append("," + dsdetailsInv.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsInv.Tables[0].Rows[y]["AWBNumber"].ToString());
                                }
                            }
                        }
                    }

                    if (awbList1.ToString().Trim() != "")
                    {
                        generateInvoice();
                    }

                    //------------------------------------------Added for Loop---------------------------------------



                }
                catch (Exception)
                {

                    throw;
                }

                if (res != "")
                {
                    bindGridView();
                    lblStatus.Text = "Bunched Final Invoices generated successfully";
                    lblStatus.ForeColor = Color.Green;
                    //GenerateInvoiceReport(awbList);


                }
                else
                {
                    lblStatus.Text = "Bunched Final Invoice generation failed";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                lblStatus.Text = "Select AWB numbers with status 'Final' to generate Final Invoice";
                lblStatus.ForeColor = Color.Blue;
            }
        }

        protected void btnRouteDetails_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == true)
            {
                Response.Write("<script>");
                Response.Write("window.open('ShowRouteDetails.aspx?AWBNumber=" + txtAWBNo.Text + "', 'window','HEIGHT=400,WIDTH=600,top=50,left=50,toolbar=yes,scrollbars=yes,resizable=yes')");
                Response.Write("</script>");
                //Response.Redirect("ShowRouteDetails?AWBNumber=" + txtAWBNo.Text);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            #region Prepare Parameters
            object[] AwbBulkInfo = new object[10];
            int i = 0;

            AwbBulkInfo.SetValue(hfAgentCode.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfFromDate.Value, i);
            i++;

            //Add one day to date
            string todate = Convert.ToDateTime(hfToDate.Value).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

            AwbBulkInfo.SetValue(todate, i);
            i++;

            AwbBulkInfo.SetValue(hfFlightNo.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfAWBNumber.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfOrigin.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfDestination.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfStatus.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfSPotRate.Value, i);
            i++;

            AwbBulkInfo.SetValue(hfPaymentMode.Value, i);

            #endregion Prepare Parameters

            DataSet dsRes = objBAL.GetAWBExportRateList(AwbBulkInfo, "", hfPaymentMode.Value, false);


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

                                //DataRow dr = dsRes.Tables[0].NewRow();
                                //dr = dsRes.Tables[1].Rows[0];
                                //dsRes.Tables[0].ImportRow(dr);

                                Session["Filters"] = "";

                                DataTable DTFilters = new DataTable();

                                DTFilters.Columns.Add("AgentName");
                                DTFilters.Columns.Add("AgentCode");
                                DTFilters.Columns.Add("FromDate");
                                DTFilters.Columns.Add("ToDate");
                                DTFilters.Columns.Add("FlightNo");
                                DTFilters.Columns.Add("AWBNumber");
                                DTFilters.Columns.Add("Origin");
                                DTFilters.Columns.Add("Destination");
                                DTFilters.Columns.Add("Status");
                                DTFilters.Columns.Add("SpotRate");
                                DTFilters.Columns.Add("PaymentMode");

                                DTFilters.Rows.Add(
                                    hfAgentName.Value,
                                    hfAgentCode.Value,
                                    //hfFromDate.Value,
                                    txtbillingfrom.Text,
                                    //hfToDate.Value,
                                    txtbillingto.Text,
                                    hfFlightNo.Value,
                                    hfAWBNumber.Value,
                                    hfOrigin.Value,
                                    hfDestination.Value,
                                    hfStatus.Value,
                                    hfSPotRate.Value,
                                    hfPaymentMode.Value);


                                Session["Filters"] = DTFilters;

                                Session["BulkData"] = "";
                                DataTable DTBulkData = new DataTable();
                                DTBulkData = dsRes.Tables[0];
                                Session["BulkData"] = DTBulkData;

                                Response.Write("<script>");
                                Response.Write("window.open('ShowBillingBulkDataImport.aspx','_blank')");
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            #region Checking file extension

            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;
                String savePath = @"c:\SCMExcel\";

                if (FileExcelUpload.HasFile)
                {
                    //code to save the file on server
                    String fileName = FileExcelUpload.FileName;

                    // Append the name of the file to upload to the path.
                    savePath += fileName;


                    // Call the SaveAs method to save the 
                    // uploaded file to the specified path.
                    // This example does not perform all
                    // the necessary error checking.               
                    // If a file with the same name
                    // already exists in the specified path,  
                    // the uploaded file overwrites it.
                    FileExcelUpload.SaveAs(savePath);


                    Session["WorkingFile"] = FileExcelUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    //filePath = Path.GetFullPath(Session["WorkingFile"].ToString()).ToLower();
                    //filePath = "C:\\AWBInvoice\\01aprto04apr2012final.xls";
                    filePath = @"C:\SCMExcel\" + FileExcelUpload.FileName;
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }

                if (FileOK)
                {

                    if (!LoadExcelData(FileExtension, FileExcelUpload.FileName, filePath))
                    {
                        lblStatus.Text = "Error in data upload";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblStatus.Text = "File Uploaded Successfully...";
                        lblStatus.ForeColor = Color.Green;
                    }


                }
                else
                {
                    lblStatus.Text = "Cannot accept files of this type.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {

            }
            #endregion
        }

        #region Load DataSet
        private bool LoadExcelData(string ext, string filename, string filepath)
        {

            string connString = string.Empty;

            if (ext == ".xls")
            {
                //   connString = ConfigurationManager.ConnectionStrings["xls"].ConnectionString;
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";

                //connString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                //          @"Data Source=" + filename + ";" +
                //          @"Extended Properties=" + Convert.ToChar(34).ToString() +
                //          @"Excel 8.0;HDR=YES" + Convert.ToChar(34).ToString();


            }
            else if (ext == ".xlsx")
            {
                // connString = ConfigurationManager.ConnectionStrings["xlsx"].ConnectionString;
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=Excel 12.0";

            }

            OleDbConnection oledbConn = new OleDbConnection(connString);
            oledbConn.Open();
            //string ExcelName = "Sheet1";
            char[] charsToTrim = { '$', '[', ']', '\'' };
            string ExcelName = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString().Trim(charsToTrim);

            string query = "select * from [" + ExcelName + "$]";


            // Create the connection object

            try
            {
                // Open connection


                // Create OleDbCommand object and select data from worksheet Sheet1
                OleDbCommand cmd = new OleDbCommand(query, oledbConn);

                // Create new OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Create a DataSet which will hold the data extracted from the worksheet.
                DataSet dsPO = new DataSet();

                // Fill the DataSet from the data extracted from the worksheet.
                oleda.Fill(dsPO);

                // Upload data in database table

                bool res = loadFileDataInDB(dsPO);

                return res;

            }
            catch (Exception ex)
            {
                return false;

            }
            finally
            {
                // Close connection
                oledbConn.Close();
            }
        }
        #endregion

        #region load File data in Db
        protected bool loadFileDataInDB(DataSet dsData)
        {
            try
            {
                #region Prepare Parameters
                object[] RateCardInfo = new object[18];
                int i = 0;
                for (int row = 0; row < dsData.Tables[0].Rows.Count - 1; row++)
                {
                    i = 0;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][0].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][1].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][2].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][3].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][4].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][5].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][6].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][7].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][8].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][9].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][10].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][11].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][12].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][13].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][14].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][15].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][16].ToString(), i);

                    i++;
                    RateCardInfo.SetValue(dsData.Tables[0].Rows[row][17].ToString(), i);


                #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.InsertAgentFileForInvoiceMatching(RateCardInfo);
                    //RateCardID=objBAL.AddRateCard(RateCardInfo);

                    if (res != "")
                    {
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = res;
                        lblStatus.ForeColor = Color.Red;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
        #endregion load File data in Db

        protected void btnAllInvMatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdBillingInfo.Rows.Count > 0)
                {
                    #region Prepare Parameters
                    object[] AWBInfo = new object[2];
                    int irow = 0;

                    //Match All Invoices Of Selected Agent.
                    AWBInfo.SetValue(ddlAgentName.SelectedValue, irow);

                    #endregion Prepare Parameters

                    dsAgent = objBAL.GetAllPendingInvOfAgent(AWBInfo);

                    if (dsAgent != null)
                    {
                        if (dsAgent.Tables != null)
                        {
                            if (dsAgent.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsAgent.Tables[0].Rows.Count; j++)
                                {
                                    string res = "";
                                    try
                                    {
                                        #region Prepare Parameters
                                        object[] RateCardInfo = new object[2];
                                        int irownum = 0;

                                        RateCardInfo.SetValue(dsAgent.Tables[0].Rows[j]["AWBNumber"].ToString(), irownum);
                                        irownum++;

                                        RateCardInfo.SetValue(Convert.ToDouble(dsAgent.Tables[0].Rows[j]["AWBTotalAmt"].ToString()), irownum);

                                        #endregion Prepare Parameters

                                        res = objBAL.MatchAllInvoicesOfAgent(RateCardInfo);

                                        ClearPanel();

                                    }
                                    catch (Exception)
                                    {
                                        lblStatus.Text = res;
                                        lblStatus.ForeColor = Color.Red;

                                    }

                                }

                                bindGridView();
                            }
                            else
                            {
                                lblStatus.Text = "No invoices with status 'New' are available for selected agent";
                                lblStatus.ForeColor = Color.Blue;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnPanelCalc_Click(object sender, EventArgs e)
        {
            CalcOnTextExit();
        }

        public void CalcOnTextExit()
        {
            try
            {
                //txtchargablewt.Attributes.Add("onblur", handlerPanelCalc);
                //txtrate.Attributes.Add("onblur", handlerPanelCalc);
                //txtocda.Attributes.Add("onblur", handlerPanelCalc);
                //txtocdc.Attributes.Add("onblur", handlerPanelCalc);
                //txtDiscount.Attributes.Add("onblur", handlerPanelCalc);
                //txtcommission.Attributes.Add("onblur", handlerPanelCalc);
                //txtTDSCommPer.Attributes.Add("onblur", handlerPanelCalc);
                //txtTDSFrtPer.Attributes.Add("onblur", handlerPanelCalc);
                if (Convert.ToDouble(txtchargablewt.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Chargable Weight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRatePerKg.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Rate";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtfreightrate.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtspotrate.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Spot Rate";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtocda.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtocdc.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtDiscount.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Discount";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtServiceTax.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Service Tax";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtcommission.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtTDSCommPer.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid TDS on Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtTDSFrtPer.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid TDS on Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //code to do calc if other calc depends on Charged Weight
                    DataSet dsCommodity = (DataSet)Session["dsCommodity"];

                    //Code to get Freight, OCDC and OCDA on change of Chargable weight

                    //DataSet dsRates = objBook.ProcessRates(dsCommodity.Tables[0].Rows[commrowind]["AWBNumber"].ToString(), txtcommodity.Text.Trim(), Convert.ToDecimal(txtchargablewt.Text.Trim()), AWBPrefix);
                    //txtrate.Text = dsRates.Tables[0].Rows[0]["FrMKT"].ToString();
                    //txtocda.Text = dsRates.Tables[0].Rows[0]["OCDA"].ToString();
                    //txtocdc.Text = dsRates.Tables[0].Rows[0]["OCDC"].ToString();

                    if (Convert.ToDouble(txtspotrate.Text) == 0)
                    {
                        txtfreightrate.Text = Math.Round(Convert.ToDouble(txtchargablewt.Text) * Convert.ToDouble(txtRatePerKg.Text), 2).ToString();
                        txtspotrate.Text = Math.Round(Convert.ToDouble(dsCommodity.Tables[0].Rows[commrowind]["SpotFreight"].ToString()), 2).ToString();
                    }
                    else
                    {
                        txtfreightrate.Text = Math.Round(Convert.ToDouble(dsCommodity.Tables[0].Rows[commrowind]["FreightRate"].ToString()), 2).ToString();
                        txtspotrate.Text = Math.Round(Convert.ToDouble(txtchargablewt.Text) * Convert.ToDouble(txtRatePerKg.Text), 2).ToString();
                    }

                    //txtTotal.Text = Math.Round(Convert.ToDouble(txtfreightrate.Text) + Convert.ToDouble(txtocda.Text) + Convert.ToDouble(txtocdc.Text), 2).ToString();

                    if (Convert.ToDouble(txtspotrate.Text) == 0)
                    {
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtfreightrate.Text) + Convert.ToDouble(txtocda.Text) + Convert.ToDouble(txtocdc.Text), 2).ToString();
                    }
                    else
                    {
                        txtTotal.Text = Math.Round(Convert.ToDouble(txtspotrate.Text) + Convert.ToDouble(txtocda.Text) + Convert.ToDouble(txtocdc.Text), 2).ToString();
                    }

                    txtdiscamount.Text = Math.Round((Convert.ToDouble(txtTotal.Text) * Convert.ToDouble(txtDiscount.Text)) / 100, 2).ToString();
                    txttotalafterdiscount.Text = Math.Round(Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(txtdiscamount.Text), 2).ToString();
                    //txtServiceTax.Text = Math.Round(Convert.ToDouble(txtTotal.Text) * ST / 100, 2).ToString();
                    txtServiceTax.Text = Math.Round((Convert.ToDouble(txtfreightrate.Text) + Convert.ToDouble(txtocda.Text) + Convert.ToDouble(txtocdc.Text)) * ST / 100, 2).ToString();
                    txtTADST.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text) + Convert.ToDouble(txtServiceTax.Text), 2).ToString();
                    if (Convert.ToDouble(txtspotrate.Text) == 0)
                    {
                        txtcommissionamt.Text = Math.Round((Convert.ToDouble(txtfreightrate.Text) * Convert.ToDouble(txtcommission.Text)) / 100, 2).ToString();
                    }
                    else
                    {
                        txtcommissionamt.Text = Math.Round((Convert.ToDouble(txtspotrate.Text) * Convert.ToDouble(txtcommission.Text)) / 100, 2).ToString();
                    }
                    txtSTOnComm.Text = Math.Round(Convert.ToDouble(txtcommissionamt.Text) * ST / 100, 2).ToString();
                    txtTDSCommAmt.Text = Math.Round((Convert.ToDouble(txtcommissionamt.Text) + Convert.ToDouble(txtSTOnComm.Text)) * Convert.ToDouble(txtTDSCommPer.Text) / 100, 2).ToString();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtTADST.Text) + Convert.ToDouble(txtTDSCommAmt.Text) - (Convert.ToDouble(txtcommissionamt.Text) + Convert.ToDouble(txtSTOnComm.Text)), 2).ToString();
                    txtTDSFrtAmt.Text = Math.Round((Convert.ToDouble(txtTADST.Text) * Convert.ToDouble(txtTDSFrtPer.Text)) / 100, 2).ToString();
                    //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text) - Convert.ToDouble(txtTDSFrtAmt.Text), 2).ToString();
                    txttotalaftertax.Text = txtRevisedTotal.Text.Trim();
                }
            }
            catch
            {

                throw;
            }
        }

        protected void btnInvoiceMatching_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdBillingInfo.Rows.Count > 0)
                {
                    #region Prepare Parameters
                    object[] AWBInfo = new object[2];
                    int irow = 0;

                    //Match All Invoices Of Selected Agent.
                    AWBInfo.SetValue(ddlAgentName.SelectedValue, irow);

                    #endregion Prepare Parameters

                    dsAgent = objBAL.GetInvoicedAWBForInvoiceMatching(AWBInfo);

                    if (dsAgent != null)
                    {
                        if (dsAgent.Tables != null)
                        {
                            if (dsAgent.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsAgent.Tables[0].Rows.Count; j++)
                                {
                                    string res = "";
                                    try
                                    {
                                        #region Prepare Parameters
                                        object[] RateCardInfo = new object[2];
                                        int irownum = 0;

                                        RateCardInfo.SetValue(dsAgent.Tables[0].Rows[j]["AWBNumber"].ToString(), irownum);
                                        irownum++;

                                        RateCardInfo.SetValue(Convert.ToDouble(dsAgent.Tables[0].Rows[j]["AWBTotalAmt"].ToString()), irownum);

                                        #endregion Prepare Parameters

                                        res = objBAL.MatchInvoicedAWBWithAgentInvoice(RateCardInfo);

                                        ClearPanel();

                                    }
                                    catch (Exception)
                                    {
                                        lblStatus.Text = res;
                                        lblStatus.ForeColor = Color.Red;

                                    }
                                }

                                bindGridView();
                            }
                            else
                            {
                                lblStatus.Text = "No AWBs with status 'Invoiced' are available for selected agent";
                                lblStatus.ForeColor = Color.Blue;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        #region Code to get AWB details to show OCDC OCDA breakup
        protected void OCDCOCDABreakup()
        {
            try
            {
                DataSet dsResult = new DataSet();
                string errormessage = "";
                if (objBAL.GetBillingAWBDetails(txtAWBPrefixPnl.Text.Trim() + txtAWBNo.Text.Trim(), ref dsResult, ref errormessage))
                {
                    // OCDetails
                    DataSet dsDetails = new DataSet();
                    dsDetails.Tables.Add();
                    dsDetails.Tables[0].TableName = "OCDA";
                    dsDetails.Tables[0].Columns.Add("Commodity Code");
                    dsDetails.Tables[0].Columns.Add("Charge Head Code");
                    dsDetails.Tables[0].Columns.Add("Charge Type");
                    dsDetails.Tables[0].Columns.Add("Charge");
                    dsDetails.Tables[0].Columns.Add("TaxPercent");
                    dsDetails.Tables[0].Columns.Add("Tax");
                    dsDetails.Tables[0].Columns.Add("DiscountPercent");
                    dsDetails.Tables[0].Columns.Add("Discount");
                    dsDetails.Tables[0].Columns.Add("CommPercent");
                    dsDetails.Tables[0].Columns.Add("Commission");



                    //AWBFrtRateDetails
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        DataRow newrow = dsDetails.Tables[0].NewRow();
                        newrow["Commodity Code"] = row["CommCode"];
                        newrow["Charge Head Code"] = row["RateLineSrNo"];
                        newrow["Charge Type"] = row["Type"];
                        newrow["Charge"] = row["Charge"];
                        newrow["TaxPercent"] = row["TaxPercent"];
                        newrow["Tax"] = row["Tax"];
                        newrow["DiscountPercent"] = row["DiscountPercent"];
                        newrow["Discount"] = row["Discount"];
                        newrow["CommPercent"] = row["CommPercent"];
                        newrow["Commission"] = row["Commission"];

                        dsDetails.Tables[0].Rows.Add(newrow);
                    }

                    //AWBOtherChargesDetails
                    foreach (DataRow row in dsResult.Tables[1].Rows)
                    {
                        DataRow newrow = dsDetails.Tables[0].NewRow();
                        newrow["Commodity Code"] = row["CommCode"];
                        newrow["Charge Head Code"] = row["ChargeHeadCode"];
                        newrow["Charge Type"] = row["ChargeType"];
                        newrow["Charge"] = row["Charge"];
                        newrow["TaxPercent"] = row["TaxPercent"];
                        newrow["Tax"] = row["Tax"];
                        newrow["DiscountPercent"] = row["DiscountPercent"];
                        newrow["Discount"] = row["Discount"];
                        newrow["CommPercent"] = row["CommPercent"];
                        newrow["Commission"] = row["Comission"];

                        dsDetails.Tables[0].Rows.Add(newrow);

                    }

                    Session["OCDetails"] = dsDetails.Copy();

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Code to get AWB details to show OCDC OCDA breakup

        #region Code to get AWB details to show OCDC OCDA breakup (Without Panel)
        protected void OCDCOCDABreakup(string AWBNumber)
        {
            try
            {
                DataSet dsResult = new DataSet();
                string errormessage = "";
                if (objBAL.GetBillingAWBDetails(AWBNumber, ref dsResult, ref errormessage))
                {
                    // OCDetails
                    DataSet dsDetails = new DataSet();
                    dsDetails.Tables.Add();
                    dsDetails.Tables[0].TableName = "OCDA";
                    dsDetails.Tables[0].Columns.Add("Commodity Code");
                    dsDetails.Tables[0].Columns.Add("Charge Head Code");
                    dsDetails.Tables[0].Columns.Add("Charge Type");
                    dsDetails.Tables[0].Columns.Add("Charge");
                    dsDetails.Tables[0].Columns.Add("TaxPercent");
                    dsDetails.Tables[0].Columns.Add("Tax");
                    dsDetails.Tables[0].Columns.Add("DiscountPercent");
                    dsDetails.Tables[0].Columns.Add("Discount");
                    dsDetails.Tables[0].Columns.Add("CommPercent");
                    dsDetails.Tables[0].Columns.Add("Commission");



                    //AWBFrtRateDetails
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        DataRow newrow = dsDetails.Tables[0].NewRow();
                        newrow["Commodity Code"] = row["CommCode"];
                        newrow["Charge Head Code"] = row["RateLineSrNo"];
                        newrow["Charge Type"] = row["Type"];
                        newrow["Charge"] = row["Charge"];
                        newrow["TaxPercent"] = row["TaxPercent"];
                        newrow["Tax"] = row["Tax"];
                        newrow["DiscountPercent"] = row["DiscountPercent"];
                        newrow["Discount"] = row["Discount"];
                        newrow["CommPercent"] = row["CommPercent"];
                        newrow["Commission"] = row["Commission"];

                        dsDetails.Tables[0].Rows.Add(newrow);
                    }

                    //AWBOtherChargesDetails
                    foreach (DataRow row in dsResult.Tables[1].Rows)
                    {
                        DataRow newrow = dsDetails.Tables[0].NewRow();
                        newrow["Commodity Code"] = row["CommCode"];
                        newrow["Charge Head Code"] = row["ChargeHeadCode"];
                        newrow["Charge Type"] = row["ChargeType"];
                        newrow["Charge"] = row["Charge"];
                        newrow["TaxPercent"] = row["TaxPercent"];
                        newrow["Tax"] = row["Tax"];
                        newrow["DiscountPercent"] = row["DiscountPercent"];
                        newrow["Discount"] = row["Discount"];
                        newrow["CommPercent"] = row["CommPercent"];
                        newrow["Commission"] = row["Comission"];

                        dsDetails.Tables[0].Rows.Add(newrow);

                    }

                    Session["OCDetails"] = dsDetails.Copy();

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Code to get AWB details to show OCDC OCDA breakup (Without Panel)

        #region Code to save OCDC OCDA breakup
        protected void SaveOCDCOCDAChanges(string AWBNumber)
        {
            try
            {
                DataSet dsDetails = (DataSet)Session["OCDetails"];
                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    //dsDetailsRow["Charge Type"] = "RateLineIATA";
                    if (row["Charge Type"].ToString().Trim() == "RateLineIATA")
                    {

                        // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission

                        object[] value = { AWBNumber,int.Parse(row["Charge Head Code"].ToString()),
                                           "IATA", decimal.Parse(row["DiscountPercent"].ToString()), 
                                           decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                                           decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                                           decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString() };

                        if (!objBAL.SaveAWBRatesChanges("Freight", value))
                        {
                            lblStatus.Text = "Error updating Rate information. Please try again...";
                            return;
                        }
                    }
                    else if (row["Charge Type"].ToString().Trim() == "RateLineMKT")
                    {

                        // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission

                        object[] value = { AWBNumber,int.Parse(row["Charge Head Code"].ToString()),
                                           "MKT", decimal.Parse(row["DiscountPercent"].ToString()), 
                                           decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                                           decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                                           decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString() };

                        if (!objBAL.SaveAWBRatesChanges("Freight", value))
                        {
                            lblStatus.Text = "Error updating Rate information. Please try again...";
                            return;
                        }
                    }
                    else
                    {
                        object[] value = { AWBNumber,row["Charge Head Code"].ToString(),
                                           row["Charge Type"].ToString().Trim(), decimal.Parse(row["DiscountPercent"].ToString()), 
                                           decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                                           decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                                           decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString() };

                        if (!objBAL.SaveAWBRatesChanges("OC", value))
                        {
                            lblStatus.Text = "Error updating OCDA and OCDC charges. Please try again...";
                            return;
                        }


                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion Code to save OCDC OCDA breakup

        protected void btnProformaInvoice_Click(object sender, EventArgs e)
        {
            generateProforma();
        }

        protected void generateProforma()
        {
            //Code to Generate Proforma Invoice for the AWB's with status Final.

            //Code to add AWB int AWBList with status Final.

            //string awbList = "";
            System.Text.StringBuilder awbList = new System.Text.StringBuilder();

            DataSet dsdetailsN = (DataSet)Session["dsDetails"];
            for (int y = 0; y < dsdetailsN.Tables[0].Rows.Count; y++)
            {
                if (dsdetailsN.Tables[0].Rows[y]["Confirmed"].ToString() == "Final" && Convert.ToString(dsdetailsN.Tables[0].Rows[y]["InvoiceNumber"]).Trim() == "")
                {
                    if (awbList.ToString() == "")
                    {
                        awbList.Append(dsdetailsN.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString());
                    }
                    else
                    {
                        awbList.Append("," + dsdetailsN.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsN.Tables[0].Rows[y]["AWBNumber"].ToString());
                    }
                }
                else
                {
                    //lblStatus.Text = "Invoice can be generated only for Finalized AWB's";
                    //lblStatus.ForeColor = Color.Blue;
                    //return;
                }
            }


            #region Prepare Parameters
            object[] AWBInfo = new object[3];
            int i = 0;

            AWBInfo.SetValue(awbList.ToString(), i);

            i++;
            string UserName = Session["UserName"].ToString();
            AWBInfo.SetValue(UserName, i);
            i++;
            AWBInfo.SetValue(System.DateTime.Now, i);

            #endregion Prepare Parameters

            if (awbList.ToString() != "")
            {
                string res = "";
                try
                {
                    if (Request.QueryString["id"] == "0")
                    {
                        res = objIntBAL.GenerateBunchProformaInterlineInvoiceNumber(AWBInfo);
                        dsInvoices = objIntBAL.GetProformaInterlineInvoiceNumber(AWBInfo);
                    }
                    else
                    {
                        res = objIntBAL.GenerateBunchProformaInterlineCreditNoteNumber(AWBInfo);
                        dsInvoices = objIntBAL.GetProformaInterlineCreditNoteNumber(AWBInfo);
                    }


                    if (dsInvoices != null)
                    {
                        if (dsInvoices.Tables != null)
                        {
                            if (dsInvoices.Tables.Count > 0)
                            {
                                if (dsInvoices.Tables[0].Rows.Count > 0)
                                {
                                    for (int invCnt = 0; invCnt < dsInvoices.Tables[0].Rows.Count; invCnt++)
                                    {
                                        #region Prepare Parameters
                                        object[] InvInfo = new object[2];
                                        int inv = 0;

                                        InvInfo.SetValue(dsInvoices.Tables[0].Rows[invCnt]["ProformaInvoiceNumber"].ToString(), inv);
                                        inv++;

                                        InvInfo.SetValue(System.DateTime.Now, inv);

                                        #endregion Prepare Parameters

                                        try
                                        {
                                            //Code to update BillingInvoiceImport with sum of fields in BillingAWBImport
                                            if (Request.QueryString["id"] == "0")
                                                res = objIntBAL.UpdateBillingProformaInterlineInvoiceSummary(InvInfo);
                                            else
                                                res = objIntBAL.UpdateBillingProformaInterlineCreditNoteSummary(InvInfo);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Commented by Vijay on 20/12/2013
                    #region Update Proforma Invoice amount from CCA and DCM
                    //if (dsInvoices != null)
                    //{
                    //    if (dsInvoices.Tables != null)
                    //    {
                    //        if (dsInvoices.Tables.Count > 0)
                    //        {
                    //            if (dsInvoices.Tables[0].Rows.Count > 0)
                    //            {
                    //                for (int invCnt = 0; invCnt < dsInvoices.Tables[0].Rows.Count; invCnt++)
                    //                {
                    //                    #region Prepare Parameters
                    //                    object[] InvInfo = new object[1];
                    //                    int inv = 0;

                    //                    InvInfo.SetValue(dsInvoices.Tables[0].Rows[invCnt]["ProformaInvoiceNumber"].ToString(), inv);

                    //                    #endregion Prepare Parameters

                    //                    try
                    //                    {
                    //                        //Code to Change Invoice Amount depending on CCA/DCM
                    //                        res = objBAL.UpdateBillingProformaInvoiceAmtFromCCADCM(InvInfo);

                    //                    }
                    //                    catch (Exception ex)
                    //                    {
                    //                        lblStatus.Text = "Error in updating Proforma Invoice amount from CCA/DCM";
                    //                        lblStatus.ForeColor = Color.Green;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion Update Proforma Invoice amount from CCA and DCM


                    //------------------------------------------Added for Loop---------------------------------------
                    bindGridView(); //To get latest records
                    DataSet dsdetailsCheck = (DataSet)Session["dsDetails"];
                    //Code to Check if any AWBs are not invoiced
                    System.Text.StringBuilder awbList1 = new System.Text.StringBuilder();
                    if (dsdetailsCheck != null)
                    {
                        for (int y = 0; y < dsdetailsCheck.Tables[0].Rows.Count; y++)
                        {
                            if (dsdetailsCheck.Tables[0].Rows[y]["Confirmed"].ToString() == "Final" && Convert.ToString(dsdetailsCheck.Tables[0].Rows[y]["InvoiceNumber"]).Trim() == "")
                            {
                                if (awbList1.ToString() == "")
                                {
                                    awbList1.Append(dsdetailsCheck.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsCheck.Tables[0].Rows[y]["AWBNumber"].ToString());
                                }
                                else
                                {
                                    awbList1.Append("," + dsdetailsCheck.Tables[0].Rows[y]["AWBPrefix"].ToString() + dsdetailsCheck.Tables[0].Rows[y]["AWBNumber"].ToString());
                                }
                            }
                        }
                    }

                    if (awbList1.ToString().Trim() != "")
                    {
                        generateProforma();
                    }

                    //------------------------------------------Added for Loop---------------------------------------

                }
                catch (Exception)
                {

                    throw;
                }

                if (res != "")
                {
                    bindGridView();
                    lblStatus.Text = "Bunched Proforma Invoices generated successfully";
                    lblStatus.ForeColor = Color.Green;
                    //GenerateInvoiceReport(awbList);


                }
                else
                {
                    lblStatus.Text = "Bunched Proforma Invoice generation failed";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            else
            {
                lblStatus.Text = "Select AWB numbers with status 'Final' to generate Proforma Invoice";
                lblStatus.ForeColor = Color.Blue;
            }
        }

        protected void btnSTChange_Click(object sender, EventArgs e)
        {
            CalcOnSTExit();
        }

        protected void CalcOnSTExit()
        {
            try
            {
                if (Convert.ToDouble(txtchargablewt.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Chargable Weight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRatePerKg.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Rate";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtfreightrate.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtspotrate.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Spot Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtocda.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtocdc.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtDiscount.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Discount";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtServiceTax.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Service Tax";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtcommission.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtTDSCommPer.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid TDS on Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtTDSFrtPer.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid TDS on Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //code to do calc if other calc depends on Charged Weight
                    DataSet dsCommodity = (DataSet)Session["dsCommodity"];

                    txtTADST.Text = Math.Round(Convert.ToDouble(txttotalafterdiscount.Text) + Convert.ToDouble(txtServiceTax.Text), 2).ToString();
                    //txtcommissionamt.Text = Math.Round((Convert.ToDouble(txtfreightrate.Text) * Convert.ToDouble(txtcommission.Text)) / 100, 2).ToString();
                    if (Convert.ToDouble(txtspotrate.Text) == 0)
                    {
                        txtcommissionamt.Text = Math.Round((Convert.ToDouble(txtfreightrate.Text) * Convert.ToDouble(txtcommission.Text)) / 100, 2).ToString();
                    }
                    else
                    {
                        txtcommissionamt.Text = Math.Round((Convert.ToDouble(txtspotrate.Text) * Convert.ToDouble(txtcommission.Text)) / 100, 2).ToString();
                    }
                    txtSTOnComm.Text = Math.Round(Convert.ToDouble(txtcommissionamt.Text) * ST / 100, 2).ToString();
                    txtTDSCommAmt.Text = Math.Round((Convert.ToDouble(txtcommissionamt.Text) + Convert.ToDouble(txtSTOnComm.Text)) * Convert.ToDouble(txtTDSCommPer.Text) / 100, 2).ToString();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtTADST.Text) + Convert.ToDouble(txtTDSCommAmt.Text) - (Convert.ToDouble(txtcommissionamt.Text) + Convert.ToDouble(txtSTOnComm.Text)), 2).ToString();
                    txtTDSFrtAmt.Text = Math.Round((Convert.ToDouble(txtTADST.Text) * Convert.ToDouble(txtTDSFrtPer.Text)) / 100, 2).ToString();
                    //txttotalaftertax.Text = Math.Round(Convert.ToDouble(txtRevisedTotal.Text) - Convert.ToDouble(txtTDSFrtAmt.Text), 2).ToString();
                    txttotalaftertax.Text = txtRevisedTotal.Text.Trim();
                }
            }
            catch
            {

                throw;
            }
        }

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/BillingInterlineInvoice.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click

    }
}
