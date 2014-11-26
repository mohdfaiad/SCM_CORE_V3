using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using QID.DataAccess;
using System.Collections.Generic;
using System.Collections;
using System.IO;
//using System.Text.RegularExpressions;.
using System.Data.SqlClient;
using BAL;
using System.Threading;
using System.Text;
using GenCode128;

/*

 2012-05-04  vinayak
 2012-05-05  vinayak
 2012-05-23  vinayak
 2012-06-04  vinayak
 2012-06-05  vinayak
 2012-06-12  vinayak
 2012-06-18  vinayak
 2012-06-18  vinayak
 2012-06-25  vinayak
 2012-07-06  vinayak Edit/View
 2012-07-09  vinayak Special Commodity
 2012-07-23  vinayak Special Commodity with three charge types
 2012-07-24  vinayak
 2012-07-25  vinayak
 2012-07-30  vinayak
 2012-08-03  vinayak
 2012-08-03 530 vinayak
 2012-08-06  vinayak
 2012-08-07  vinayak
 
*/

namespace ProjectSmartCargoManager
{
    public partial class ConBooking_GHA : System.Web.UI.Page
    {

        #region Variables
        // DataSet dsAWBData;
        BLExpManifest objExpMani = new BLExpManifest();
        BookingBAL objBLL = new BookingBAL();
        DGRCargoBAL dgr = new DGRCargoBAL();
        MasterBAL ObjMsBAl = new MasterBAL();
        LoginBL lBal = new LoginBL();
        CustomsImportBAL ObjCustoms = new CustomsImportBAL();
        int intTotalPcs = 0;
        float floatGrossWt = 0;
        float floatVolWt = 0;
        float floatChargedWt = 0;
        //decimal FinalIATA = 0;
        DateTime dtCurrentDate = DateTime.Now;
        //bool IsRateExists = false;        
        string strFlightNumbers = string.Empty;
        string strUserName = string.Empty;
        decimal dcActualAmt = 0;
        decimal dcActualTax = 0;
        decimal dcMinAmount = 0;
        decimal dcMinTax = 0;
        //bool IsMinimum = false;
        decimal dcActualAmtMKT = 0;
        decimal dcActualTaxMKT = 0;
        decimal dcMinAmountMKT = 0;
        decimal dcMinTaxMKT = 0;
        bool rs;
        string OtherFltSpe = string.Empty;
        string OCDCCurrency = "";
        string AgentCurrency = "";
        string RateClass = "";
        bool IsIATAasMKT = false;
        //jayant
        string UserName;
        SQLServer da = new SQLServer(Global.GetConnectionString());
        bool prrate = false;
        //end
        #endregion Variables

        #region Form Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                IsIATAasMKT = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["IsIATAasMkt"]);

                dtCurrentDate = (DateTime)Session["IT"];
                strUserName = Convert.ToString(Session["UserName"]);

                if (!IsPostBack)
                {
                    Session["IsBulk"] = false;
                    Session["DgrCargo"] = null;
                    //string ShowEAWBButton = lBal.GetMasterConfiguration("ShowEAWBButton");
                    string ShowEAWBButton = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowEAWBButton");
                    if (ShowEAWBButton.Length > 0)
                    {
                        if (ShowEAWBButton.Contains(Session["RoleName"].ToString()) || Session["RoleName"].ToString().Equals(ShowEAWBButton, StringComparison.OrdinalIgnoreCase))
                        {
                            btnShowEAWB.Visible = true;
                            imgbtnEAWBPopUp.Visible = true;
                        }
                        else
                        {
                            //btnShowEAWB.Enabled = false;
                            imgbtnEAWBPopUp.Visible = false;
                        }
                    }
                    else
                    {
                        btnShowEAWB.Visible = false;
                        imgbtnEAWBPopUp.Visible = false;
                    }
                    //bool scroll = Convert.ToBoolean(lBal.GetMasterConfiguration("BookingScroll"));
                    bool scroll = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "BookingScroll"));
                    if (scroll == false)
                    {
                        lblUOM.Visible = false;
                        txtUOM.Visible = false;
                        lblIAC.Visible = false;
                        txtIACCode.Visible = false;
                        lblCCSF.Visible = false;
                        txtCCSF.Visible = false;
                        DivConBook.Attributes.Add("style", "height:400px; width:1024px;");
                    }
                    else
                    {
                        lblUOM.Visible = true;
                        txtUOM.Visible = true;
                        lblIAC.Visible = true;
                        txtIACCode.Visible = true;
                        lblCCSF.Visible = true;
                        txtCCSF.Visible = true;
                    }

                    Session["HAWBDocument"] = null;
                    Session["QB"] = "";
                    if (Session["awbPrefix"] == null)
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                    }
                    txtAwbPrefix.Text = "";
                    if (Request.QueryString["GHA"] == null && Convert.ToString(CommonUtility.SmartKargoInstance) == "GH")
                    {
                        Session["GHABooking"] = true;
                        hdnBookingType.Value = "I"; //In Coming Booking.
                        lblPageName.Text = "Partner Booking";
                    }
                    else if (Request.QueryString["GHA"] != null && Convert.ToBoolean(Request.QueryString["GHA"]) == true)
                    {
                        Session["GHABooking"] = true;
                        hdnBookingType.Value = "I"; //In Coming Booking.
                        lblPageName.Text = "Partner Booking";
                    }
                    else
                    {
                        Session["GHABooking"] = false;
                        hdnBookingType.Value = "N"; //Normal Booking.
                        lblPageName.Text = "New Booking";
                        txtAwbPrefix.Text = Convert.ToString(Session["awbPrefix"]);
                        txtAwbPrefix.Enabled = false;
                        ddlAirlineCode.Enabled = false;
                        grdRouting.Columns[14].Visible = true;
                        grdRouting.Columns[15].Visible = true;
                        grdRouting.Columns[16].Visible = true;
                    }

                    TXTAgentCode.Enabled = true;
                    Session["HAWBDetails"] = null;
                    Session["Message"] = null;
                    Session["AWBULD"] = null;

                    SetLabelValues(true);
                    Session["BookingID"] = "0";
                    //ViewState["BookingID"] = "0";
                    //LoadOrigin();
                    LoadAWBMasterData();


                    ddlOrg.SelectedValue = Session["Station"].ToString();
                    ////Added by Vishal - 04 MAY 2014 ******************** 
                    LoadCurrencyType();
                    //LoadCurrencyWithUOM();
                    ////******************** Added by Vishal - 04 MAY 2014

                    //LoadDestination();
                    //ddlOrg.Items.Add(new ListItem(Session["Station"].ToString(), Session["Station"].ToString()));
                    //ddlOrg.Enabled = false;
                    //BookingBAL.OrgStation = Session["Station"].ToString();

                    LoadGridRateDetail();
                    LoadGridRoutingDetail();
                    LoadGridMaterialDetail();

                    CheckPartnerFlightSupport();
                    LoadSystemParameters();
                    // FOR G8
                    LoadCommodityDropdown();

                    //AWBDesignators(string.Empty);
                    //LoadCnoteCodes();
                    LoadViability();

                    //Set paymode types in dropdown.
                    //DropDownList ddl = ddlPaymentMode;
                    DropDownList ddl = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                    if (Session["ConBooking_PayModeMaster"] != null)
                    {
                        DataTable dt = (DataTable)Session["ConBooking_PayModeMaster"];
                        ddl.Items.Clear();
                        ddl.DataSource = dt;
                        ddl.DataTextField = "PayModeText";
                        ddl.DataValueField = "PayModeCode";
                        ddl.DataBind();
                    }

                    txtExecutionDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                    txtShipmentDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");//dtCurrentDate.AddHours(4).ToString("dd/MM/yyyy");
                    LoadShipmentTime();
                    ddlShipmentTime.SelectedValue = dtCurrentDate.ToString("HH:00");
                    txtExecutedBy.Text = Session["UserName"].ToString();
                    txtExecutedAt.Text = Session["Station"].ToString();
                    //if (Session["AWBNumber"] != null)
                    //{
                    //    txtAWBNo.Text = Session["AWBNumber"].ToString();
                    //    HidAWBNumber.Value = Session["AWBNumber"].ToString();
                    //}

                    //Vijay - Load country dropdown from CountryMaster
                    //LoadCountryDropdown();

                    // Currency Filling
                    //                    FillCurrencyCodes(drpCurrency, "INR");

                    //FillIrregularityCode();

                    //FillProductType();

                    // For OCDA/OCDC Calculations ////////////////////////////////

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
                    dsDetails.Tables[0].Columns.Add("ChargeCode");
                    Session["OCDetails"] = dsDetails.Copy();

                    /////////////////////////////////////////////////////////////

                    CreateDimensionDataSet();
                    CreateAWBPiecesDataSet();
                    CreateAWBRoutePiecesds();
                    //jayant
                    BindRepeaterData();
                    //end jayant
                    btnfinalize.Enabled = false;
                    //btnPrintLabels.Enabled = false;
                    btnSendFFR.Enabled = true;
                    // btnShowEAWB.Enabled = false;
                    btnClose.Enabled = false;
                    btnDelete.Enabled = false;
                    // btnDgr.Enabled = false;
                    if (Session["ConBooking_AllowRatesEdit"] != null &&
                        Session["ConBooking_AllowRatesEdit"].ToString() != "" &&
                        Convert.ToBoolean(Session["ConBooking_AllowRatesEdit"]))
                    {
                        btnPopupSave.Visible = true;
                        btnPopupCancel.Visible = true;
                    }
                    btnExecute.Enabled = false;
                    btnGenerateTracer.Enabled = false;
                    btnPrintShipper.Enabled = false;
                    btnReopen.Enabled = false;
                    btnSenfwb.Enabled = false;


                    #region Autopopulate from listbooking

                    Session["OCTotal"] = null;
                    Session["OATotal"] = null;

                    Session["AWBStatus"] = "B";
                    if (Request.QueryString["command"] != null && !(Request.QueryString["command"].Contains("TemplateBooking")))
                    {
                        string status = "", errormessage = "", state = "", AWBPrefix = "", AWBNumber = "";

                        string Number = Request.QueryString["AWBNumber"].ToString().Trim();

                        if (Number.IndexOf('-') == -1)
                        {
                            AWBNumber = Number.Substring(Number.Length - 8, 8);
                            if (Number.Length > 8)
                            {
                                AWBPrefix = Number.Substring(0, 3);
                            }
                            else
                            {
                                AWBPrefix = Request.QueryString["AWBPrefix"].ToString().Trim();
                            }
                        }
                        else
                        {
                            AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                            AWBNumber = Number.Substring(Number.IndexOf('-') + 1, 8);
                        }

                        if (objBLL.GetAWBStatus(AWBNumber, AWBPrefix, ref status, ref errormessage))
                        {
                            Session["DGRAWB"] = AWBNumber;

                            if (status == "E")
                            {
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "AWB is executed.";
                                btnPopupSave.Visible = false;
                                btnPopupCancel.Visible = false;
                                btnExecute.Enabled = false;
                                txtExecutionDate.Enabled = false;
                                btnExecutionDate.Enabled = false;
                                btnReopen.Enabled = true;
                                btnDelete.Enabled = false;
                                btnSave.Enabled = false;
                                state = "EditExecuted";

                                btnProcess.Enabled = true;
                                DropDownList ddlPaymentMode = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                                if (ddlPaymentMode.SelectedValue == "PP")
                                    btnCollect.Enabled = true;
                                else
                                    btnCollect.Enabled = false;
                            }

                            if (status == "B")
                            {
                                if (Session["ConBooking_AllowRatesEdit"] != null &&
                                    Session["ConBooking_AllowRatesEdit"].ToString() != "" &&
                                    Convert.ToBoolean(Session["ConBooking_AllowRatesEdit"]))
                                {
                                    btnPopupSave.Visible = true;
                                    btnPopupCancel.Visible = true;
                                }
                                btnExecute.Enabled = true;
                                txtExecutionDate.Enabled = true;
                                btnExecutionDate.Enabled = true;
                                btnReopen.Enabled = false;

                                //Commented by Vijay...Agents and Ops staff should not have access to Delete button
                                //btnDelete.Enabled = true;
                                if (Session["RoleName"].ToString() == "Super User" && Session["DelExecAWB"].ToString() == "True")
                                {
                                    btnDelete.Enabled = true;

                                }

                                btnSave.Enabled = true;
                                state = "Edit";

                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "AWB Booked.";

                                btnProcess.Enabled = true;
                            }

                            if (status == "R")
                            {
                                btnExecute.Enabled = true;
                                txtExecutionDate.Enabled = true;
                                btnExecutionDate.Enabled = true;
                                if (Session["ConBooking_AllowRatesEdit"] != null &&
                                    Session["ConBooking_AllowRatesEdit"].ToString() != "" &&
                                    Convert.ToBoolean(Session["ConBooking_AllowRatesEdit"]))
                                {
                                    btnPopupSave.Visible = true;
                                    btnPopupCancel.Visible = true;
                                }
                                btnReopen.Enabled = false;
                                btnDelete.Enabled = false;
                                btnSave.Enabled = true;
                                state = "EditReopen";

                                btnProcess.Enabled = true;

                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "AWB is Re-Opened.";
                                //start modify jayant on 12/02/2014
                                if (Session["RoleName"].ToString() == "Super User" && Session["DelExecAWB"].ToString() == "True")
                                {
                                    btnDelete.Enabled = true;
                                }

                                //end modify jayant on 12/02/2014
                            }

                            if (status == "V")
                            {
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "AWB is Voided.";

                                //Added by Vijay - 11-10-2014
                                btnExecute.Enabled = true;
                                txtExecutionDate.Enabled = true;
                                btnExecutionDate.Enabled = true;

                                if (Session["RoleName"].ToString() != "Super User")
                                {
                                    btnPopupSave.Visible = false;
                                    btnPopupCancel.Visible = false;
                                    //commented by Vijay - 11-10-2014
                                    //btnExecute.Enabled = false;
                                    txtExecutionDate.Enabled = false;
                                    btnExecutionDate.Enabled = false;
                                    //btnReopen.Enabled = false;
                                    btnDelete.Enabled = false;
                                    btnSave.Enabled = false;
                                    btnProcess.Enabled = false;
                                    ddlServiceclass.Enabled = false;
                                    TXTAgentCode.Enabled = false;
                                }
                            }
                            btnSenfwb.Enabled = true;
                            Session["AWBStatus"] = status;
                            AutoPopulateData((Request.QueryString["command"].ToString().Trim() == "Edit" && status != "E"), AWBNumber, AWBPrefix, state);
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Error :" + errormessage;
                            if (Request.QueryString["AWBPrefix"] != null)
                            {
                                txtAwbPrefix.Text = Convert.ToString(Request.QueryString["AWBPrefix"]);
                                txtAwbPrefix_TextChanged(null, null);
                            }
                            if (Request.QueryString["AWBNumber"] != null)
                                txtAWBNo.Text = Convert.ToString(Request.QueryString["AWBNumber"]);

                        }

                    }
                    else if (Request.QueryString["command"] != null && (Request.QueryString["command"].Contains("TemplateBooking")))
                    {
                        if (Session["ConBooking_AllowRatesEdit"] != null &&
                        Session["ConBooking_AllowRatesEdit"].ToString() != "" &&
                        Convert.ToBoolean(Session["ConBooking_AllowRatesEdit"]))
                        {
                            btnPopupSave.Visible = true;
                            btnPopupCancel.Visible = true;
                        }
                        btnExecute.Enabled = true;
                        txtExecutionDate.Enabled = true;
                        btnExecutionDate.Enabled = true;
                        //btnReopen.Enabled = false;
                        btnDelete.Enabled = true;
                        btnSave.Enabled = true;

                        btnProcess.Enabled = true;
                        string TemplateID = Request.QueryString["TemplateID"].ToString().Trim(); ;
                        AutoPopulateTemplate(true, TemplateID, "Edit");
                    }

                    ShipperConDetailsChenged(null, null);

                    #endregion
                    #region Update Epouch Uploaded Count
                    try
                    {
                        EpouchBAL objEpouch = new EpouchBAL();
                        btnePouch.Text = (string)HttpContext.GetGlobalResourceObject("LabelNames", "LBL_BTN_EPOUCH") + "(" + objEpouch.GetEpouchAWBCount(txtAwbPrefix.Text.Trim(), txtAWBNo.Text.Trim()) + ")";
                        objEpouch = null;
                    }
                    catch (Exception Ex)
                    { }
                    #endregion
                    try
                    {
                        if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                        {
                            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                            {
                                ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Visible = false;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                            {
                                ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Visible = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    //btnOrgSearch.Attributes.Add("onclick", "javascript:OpenSearchPopup('Origin','"+ txtOrg.Text +"','null');return false;");                                     
                }
                LoadFlights();
                //ApplyPermissions();

                //string AgentCode = Convert.ToString(Session["ACode"]);

                //int RoleId = Convert.ToInt32(Session["RoleID"]);

                //if (AgentCode != "")
                //{
                //    TXTAgentCode.Text = AgentCode;
                //    ddlAgtCode_SelectedIndexChanged(null, null);
                //}

                ((ImageButton)GRDRates.Rows[0].FindControl("btnOcDueCar")).Enabled = true;
                ((ImageButton)GRDRates.Rows[0].FindControl("btnOcDueAgent")).Enabled = true;
                ((ImageButton)grdMaterialDetails.Rows[0].FindControl("btnDimensionsPopup")).Enabled = true;


                //vijay
                //To load values from Session declared in BillingEditOCDCOCDA
                if (Session["OCTotal"] != null)
                    ((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueCar")).Text = Session["OCTotal"].ToString();
                if (Session["OATotal"] != null)
                    ((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueAgent")).Text = Session["OATotal"].ToString();
                //vijay

                //Get airport currency & booking type
                //LoadCurrencyType();
                CheckAWBStatus();

                #region Execute Button disable if Save & Execute Combine
                try
                {
                    MasterBAL objBAL = new MasterBAL();
                    
                    if (objBAL.CheckConfiguration("Booking", "CombineSave&Execute"))
                    {
                        btnExecute.Visible = false;
                    }
                }
                catch (Exception ex) { }
                #endregion

                if (Request.QueryString["command"] != null && Request.QueryString["command"].ToString().Trim().ToUpper() == "VIEW")
                {
                    btnSave.Enabled = false;
                    btnExecute.Enabled = false;
                    txtExecutionDate.Enabled = false;
                    btnExecutionDate.Enabled = false;
                    btnPopupSave.Visible = false;
                    btnPopupCancel.Visible = false;
                    btnReopen.Enabled = false;
                    btnDgr.Enabled = false;
                    btnSendFFR.Enabled = false;
                    btnSendFHL.Enabled = false;
                    btnSenfwb.Enabled = false;
                    //btnePouch.Enabled = false;
                    btnDelete.Enabled = false;
                }
                if (!IsPostBack)
                    txtAwbPrefix_TextChanged(null, null);

                HideControlsOnDemoFlag();


                if (Convert.ToString(Session["AgentCode"]) != "")
                {
                    ddlServiceclass.Enabled = false;
                    TXTAgentCode.Text = Convert.ToString(Session["AgentCode"]);
                    TXTAgentCode.Enabled = false;
                    txtAgentName.Text = Convert.ToString(Session["AgentName"]);
                    TXTCustomerCode.Text = Convert.ToString(Session["CustCode"]);
                    btnExecute.Enabled = false;
                    txtExecutionDate.Enabled = false;
                    btnExecutionDate.Enabled = false;
                    ddlOrg.Enabled = false;
                    TXTAgentCode_TextChanged(null, null);
                    
                    //btnReopen.Enabled = false;
                    btnPopupSave.Visible = false;
                    btnPopupCancel.Visible = false;
                }

                if (Session["Validate_ReopenAllowedBy"] == null)
                    Session["Validate_ReopenAllowedBy"] = CheckIfAWBUpdateAllowedForRole();

                if (Convert.ToBoolean(Session["Validate_ReopenAllowedBy"]) == false)
                    btnDelete.Enabled = true;
                if (Convert.ToBoolean(Session["GHABooking"]) && HidAWBNumber.Value == "")
                {
                    TXTAgentCode.Enabled = true;
                    txtAgentName.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error :" + ex.Message + "');</SCRIPT>");
            }
            finally
            {
                //restoreScroll(this, new EventArgs());
                try
                {
                    if (Convert.ToString(Session["ButtonID"]) == "btnSave")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CallPopulateClick();</script>", false);
                        Session["ButtonID"] = "";
                    }

                    //if (Session["ShpConsgMandatory"] != null && Convert.ToBoolean(Session["ShpConsgMandatory"]))
                    //    DivShipperCon.Style.Add("display", "block");
                    //else
                    //    DivShipperCon.Style.Add("display", "none");
                }
                catch (Exception ex)
                {
                }
            }

            //ScriptManager.GetCurrent(this).RegisterPostBackControl(btnAddUld);
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(btnHABDetails);
        }
        #endregion Form Load

        #region Load Shipment Time
        private void LoadShipmentTime()
        {
            ddlShipmentTime.Items.Clear();
            ddlShipmentTime.Items.Add(new ListItem("Select", "25:00"));
            for (int i = 0; i < 25; i++)
            {
                ddlShipmentTime.Items.Add(new ListItem(i.ToString().PadLeft(2, '0') + ":00",
                    i.ToString().PadLeft(2, '0') + ":00"));
            }
        }
        #endregion Load Shipment Time

        #region LoadCurrencyType
        private void LoadCurrencyType()
        {
            DataSet ds = null;
            try
            {

                if (objBLL.GetAirpotCurrency(ddlOrg.Text, ref ds))
                {
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //drpCurrency.Items.Clear();
                                //drpCurrency.Items.Add(ds.Tables[0].Rows[0]["BookingCurrrency"].ToString().Length > 1 ? ds.Tables[0].Rows[0]["BookingCurrrency"].ToString() : "INR");
                                drpCurrency.Text = ds.Tables[0].Rows[0]["BookingCurrrency"].ToString().Length > 1 ? ds.Tables[0].Rows[0]["BookingCurrrency"].ToString() : "INR";
                                //drpCurrency.Text = ds.Tables[0].Rows[0]["BookingCurrrency"].ToString().Length > 1 ? ds.Tables[0].Rows[0]["BookingCurrrency"].ToString() : "INR";
                                Session["CurrencyType"] = ds.Tables[0].Rows[0]["BookingType"].ToString().Length > 0 ? ds.Tables[0].Rows[0]["BookingType"].ToString() : "IATA";
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion


        //Added by Vishal - 04 MAY 2014 ********************
        #region LoadCurrencyUOM
        private void LoadCurrencyWithUOM()
        {
            DataSet ds = null;
            try
            {
                Session["ConBooking_RouteType"] = "D"; //Route type whether Internation or Domestic.
                if (objBLL.GetAirpotCurrencyWithUOM(ddlOrg.Text, ddlDest.SelectedValue, ref ds))
                {
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //drpCurrency.Items.Clear();
                                //drpCurrency.Items.Add(ds.Tables[0].Rows[0]["BookingCurrrency"].ToString().Length > 1 ? ds.Tables[0].Rows[0]["BookingCurrrency"].ToString() : "INR");
                                drpCurrency.SelectedItem.Text = ds.Tables[0].Rows[0]["BookingCurrrency"].ToString().Length > 1 ? ds.Tables[0].Rows[0]["BookingCurrrency"].ToString() : "INR";
                                //drpCurrency.Text = ds.Tables[0].Rows[0]["BookingCurrrency"].ToString().Length > 1 ? ds.Tables[0].Rows[0]["BookingCurrrency"].ToString() : "INR";
                                txtUOM.Text = ds.Tables[0].Rows[0]["UOM"] != null ? ds.Tables[0].Rows[0]["UOM"].ToString() : "L";
                                Session["CurrencyType"] = ds.Tables[0].Rows[0]["BookingType"].ToString().Length > 0 ? ds.Tables[0].Rows[0]["BookingType"].ToString() : "IATA";
                                Session["ConBooking_RouteType"] = ds.Tables[0].Rows[0]["RouteType"] != null ? ds.Tables[0].Rows[0]["RouteType"].ToString() : "D";

                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion
        //******************** Added by Vishal - 04 MAY 2014

        #region Load Matching Product Types
        private void LoadMatchingProductTypes()
        {
            DataSet ds = null;
            try
            {
                string fltNum = "";
                if (((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedIndex > 0)
                {
                    fltNum = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text;
                    if (fltNum.ToUpper() == "SELECT")
                    {
                        fltNum = "";
                    }
                }
                string fltDt = "";
                DateTime fltDate;
                if (DateTime.TryParseExact(((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out fltDate))
                {
                    fltDt = ((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text;
                }
                string commCode = "";
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                {
                    if (((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode")).Text != "")
                    {
                        commCode = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode")).Text;
                    }
                }
                else
                {
                    if (((TextBox)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1")).Text != "")
                    {
                        commCode = ((TextBox)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1")).Text;
                    }
                }
                decimal weight = 0;
                BALProductType objBAL = new BALProductType();
                ds = objBAL.GetMatchingProductType(ddlOrg.SelectedValue, ddlDest.SelectedValue, fltNum, fltDt, "", commCode,
                    weight, txtShipmentDate.Text);

                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 1)
                        {
                            ddlProductType.DataSource = ds.Tables[1];
                            ddlProductType.DataValueField = "SerialNumber";
                            ddlProductType.DataTextField = "ProductType";
                            ddlProductType.DataBind();
                        }
                        else
                        {
                            if (ds.Tables.Count > 2)
                            {
                                if (ds.Tables[2].Rows.Count > 1)
                                {
                                    ddlProductType.DataSource = ds.Tables[2];
                                    ddlProductType.DataValueField = "SerialNumber";
                                    ddlProductType.DataTextField = "ProductType";
                                    ddlProductType.DataBind();
                                }
                            }
                        }
                        ddlProductType.Enabled = true;
                    }

                }
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion

        #region Load Shipper and Consignee Country Dropdown
        //public void LoadCountryDropdown()
        //{
        //    DataSet ds = objBLL.GetAllCountry();
        //    try
        //    {
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            ds.Tables[0].Rows.Add("", "");
        //            ddlShipCountry.DataSource = ds;
        //            ddlShipCountry.DataMember = ds.Tables[0].TableName;
        //            ddlShipCountry.DataTextField = "CountryName";
        //            ddlShipCountry.DataValueField = "CountryCode";
        //            ddlShipCountry.DataBind();
        //            ddlShipCountry.SelectedValue = "";

        //            ddlConCountry.DataSource = ds;
        //            ddlConCountry.DataMember = ds.Tables[0].TableName;
        //            ddlConCountry.DataTextField = "CountryName";
        //            ddlConCountry.DataValueField = "CountryCode";
        //            ddlConCountry.DataBind();
        //            ddlConCountry.SelectedValue = "";
        //        }
        //    }
        //    catch (Exception ex) { }
        //    finally
        //    {
        //        if (ds != null)
        //            ds.Dispose();
        //    }
        //}
        #endregion LoadCountryDropdown

        #region dimension

        public void CreateDimensionDataSet()
        {
            DataSet dsDimesionAll = new DataSet();

            try
            {
                dsDimesionAll.Tables.Add(new DataTable());

                dsDimesionAll.Tables[0].Columns.Add("Length");
                dsDimesionAll.Tables[0].Columns.Add("Breadth");
                dsDimesionAll.Tables[0].Columns.Add("Height");
                dsDimesionAll.Tables[0].Columns.Add("PcsCount");
                dsDimesionAll.Tables[0].Columns.Add("RowIndex");
                dsDimesionAll.Tables[0].Columns.Add("MeasureUnit");

                Session["dsDimesionAll"] = dsDimesionAll.Copy();


            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                return;
            }
            finally
            {
                if (dsDimesionAll != null)
                    dsDimesionAll.Dispose();
            }

        }

        public void CreateAWBPiecesDataSet()
        {
            DataSet dsAWBPiecesAll = new DataSet();
            try
            {

                dsAWBPiecesAll.Tables.Add(new DataTable());

                dsAWBPiecesAll.Tables[0].Columns.Add("Pieces");
                dsAWBPiecesAll.Tables[0].Columns.Add("GrossWt");
                dsAWBPiecesAll.Tables[0].Columns.Add("PieceId");
                dsAWBPiecesAll.Tables[0].Columns.Add("RowIndex");
                dsAWBPiecesAll.Tables[0].Columns.Add("isHeavy");
                Session["dsPiecesDet"] = dsAWBPiecesAll.Copy();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                return;
            }
            finally
            {
                if (dsAWBPiecesAll != null)
                    dsAWBPiecesAll.Dispose();

            }

        }

        #endregion

        #region LoadGridMaterialDetail
        public void LoadGridMaterialDetail()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();
            DataSet dsMaterialDetails = new DataSet();

            try
            {
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CommodityCode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CodeDescription";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Pieces";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "GrossWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dimensions";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "VolumetricWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ChargedWeight";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RowIndex";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PaymentMode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AccountInfo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ShipmentType";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ShipmentPriority";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Remarks";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FlightNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FlightDate";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["CommodityCode"] = "";// "TV";
                dr["CodeDescription"] = "";
                dr["Pieces"] = "";// "1";
                dr["GrossWeight"] = "";// "10KG";
                dr["Dimensions"] = "NO";
                dr["VolumetricWeight"] = "";
                dr["ChargedWeight"] = "";
                dr["RowIndex"] = "1";
                dr["PaymentMode"] = "1";
                dr["AccountInfo"] = "";
                dr["ShipmentPriority"] = "";
                dr["Remarks"] = "";
                dr["FlightNo"] = "";
                dr["FlightDate"] = "";

                myDataTable.Rows.Add(dr);
                grdMaterialDetails.DataSource = null;
                grdMaterialDetails.DataSource = myDataTable;
                grdMaterialDetails.DataBind();

                dsMaterialDetails.Tables.Add(myDataTable.Copy());

                Session["dsMaterialDetails"] = dsMaterialDetails.Copy();

                //Set paymode types in dropdown.
                DropDownList ddl = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                if (Session["ConBooking_PayModeMaster"] != null)
                {
                    DataTable dt = (DataTable)Session["ConBooking_PayModeMaster"];
                    ddl.Items.Clear();
                    ddl.DataSource = dt;
                    ddl.DataTextField = "PayModeText";
                    ddl.DataValueField = "PayModeCode";
                    ddl.DataBind();
                    if (ddl.Items.Count > 1)
                        ddl.SelectedIndex = 1;

                }

            }
            catch (Exception ex)
            {
                myDataTable = null;
                Ds = null;
                dsMaterialDetails = null;
            }
            finally
            {
                if (myDataTable != null)
                    myDataTable.Dispose();
                if (Ds != null)
                    Ds.Dispose();
                if (dsMaterialDetails != null)
                    dsMaterialDetails.Dispose();
            }
        }
        #endregion Load Grid Material Detail

        #region Load Grid Routing Detail
        public void LoadGridRoutingDetail()
        {
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();
            DataSet dsRoutDetails = new DataSet();
            try
            {
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltOrigin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltDestination";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltNumber";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltTime";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltDate";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Pcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Wt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Status";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Accepted";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AcceptedPcs";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AcceptedWt";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ScheduleID";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ChrgWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PartnerStatus";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Airline";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PartnerType";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["FltOrigin"] = ddlOrg.Text.Trim();
                dr["FltDestination"] = "";// "DEL";
                dr["FltNumber"] = "";// "IT101";
                dr["FltDate"] = dtCurrentDate.ToString("dd/MM/yyyy");// "14/Jan/2012";
                dr["Pcs"] = "";// 
                dr["Wt"] = "";// 
                dr["ChrgWt"] = "";// 
                dr["Status"] = "C";// 
                dr["Accepted"] = "N";// 
                dr["AcceptedPcs"] = "";// 
                dr["AcceptedWt"] = "";
                dr["PartnerStatus"] = "";
                dr["Airline"] = "";
                dr["PartnerType"] = "";
                myDataTable.Rows.Add(dr);

                grdRouting.DataSource = null;
                grdRouting.DataSource = myDataTable;
                grdRouting.DataBind();



                dsRoutDetails.Tables.Add(myDataTable);
                Session["dsRoutDetails"] = dsRoutDetails.Copy();
                LoadAirlineCode("");
                //ddlPartnerType_SelectionChange(null, null);
                LoadAWBStatusDropdown();
                // SetRouteGridValues(myDataTable);
            }
            catch (Exception ex)
            {
                myDataTable = null;
                Ds = null;
                dsRoutDetails = null;
            }
            finally
            {
                if (myDataTable != null)
                    myDataTable.Dispose();
                if (Ds != null)
                    Ds.Dispose();
                if (dsRoutDetails != null)
                    dsRoutDetails.Dispose();
            }
        }

        #endregion Load Grid Routing Detail

        #region Load Destination Dropdown
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        public void LoadDestination()
        {
            DataSet ds = null;
            try
            {
                ds = objBLL.GetDestinationsForSource(Session["Station"].ToString());
                if (ds != null)
                {
                    //Dest dropdown
                    DataRow row = ds.Tables[0].NewRow();

                    row["AirportCode"] = "Select";
                    ds.Tables[0].Rows.Add(row);


                    ddlDest.DataSource = ds;
                    ddlDest.DataMember = ds.Tables[0].TableName;
                    ddlDest.DataTextField = "AirportCode";
                    ddlDest.DataValueField = "AirportCode";
                    ddlDest.DataBind();

                    ddlDest.Text = "Select";

                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion Load Location Dropdown

        #region Load Origin Dropdown
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        public void LoadOrigin()
        {
            DataSet ds = null;
            try
            {
                ds = objBLL.GetDestinationsForSource("");
                if (ds != null)
                {
                    //Dest dropdown
                    DataRow row = ds.Tables[0].NewRow();

                    row["AirportCode"] = "Select";
                    ds.Tables[0].Rows.Add(row);


                    ddlOrg.DataSource = ds;
                    ddlOrg.DataMember = ds.Tables[0].TableName;
                    ddlOrg.DataTextField = "AirportCode";
                    ddlOrg.DataValueField = "AirportCode";
                    ddlOrg.DataBind();

                    ddlOrg.Text = "Select";

                    ddlDest.DataSource = ds;
                    ddlDest.DataMember = ds.Tables[0].TableName;
                    ddlDest.DataTextField = "AirportCode";
                    ddlDest.DataValueField = "AirportCode";
                    ddlDest.DataBind();

                    ddlDest.Text = "Select";
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion Load Location Dropdown

        #region Set Material Grid Values
        /// <summary>
        /// Displays values in material grid from given Data Table.
        /// </summary>        
        /// <param name="dt">Data Tabel which contains values to be shown in grid.</param>
        public void SetMaterialGridValues(DataTable dt)
        {
            DataRow dr;
            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                dr = dt.Rows[rowIndex];
                if (dr == null)
                    continue;
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                {
                    ((DropDownList)(grdMaterialDetails.Rows[rowIndex].FindControl("ddlMaterialCommCode"))).Text = dr["CommodityCode"].ToString();
                }
                else
                {
                    ((TextBox)(grdMaterialDetails.Rows[rowIndex].FindControl("ddlMaterialCommCode1"))).Text = dr["CommodityCode"].ToString();
                }
                ((TextBox)(grdMaterialDetails.Rows[rowIndex].FindControl("txtMaterialCommDesc"))).Text = dr["CommodityDesc"].ToString();
                ((TextBox)(grdMaterialDetails.Rows[rowIndex].FindControl("TXTPcs"))).Text = dr["Pieces"].ToString();
                ((TextBox)(grdMaterialDetails.Rows[rowIndex].FindControl("txtCommGrossWt"))).Text = dr["GrossWeight"].ToString();
                ((Label)(grdMaterialDetails.Rows[rowIndex].FindControl("lblDimensions"))).Text = dr["Dimensions"].ToString();
                ((TextBox)(grdMaterialDetails.Rows[rowIndex].FindControl("txtCommVolWt"))).Text = dr["VolumetricWeight"].ToString();
                ((TextBox)(grdMaterialDetails.Rows[rowIndex].FindControl("txtCommChargedWt"))).Text = dr["ChargedWeight"].ToString();
                ((DropDownList)(grdMaterialDetails.Rows[rowIndex].FindControl("ddlPaymentMode"))).SelectedValue = dr["PaymentMode"].ToString();
                ((TextBox)(grdMaterialDetails.Rows[rowIndex].FindControl("txtAccountInfo"))).Text = dr["AccountInfo"].ToString();
            }
        }
        #endregion Set Material Grid Values

        #region Set Route Grid Values
        /// <summary>
        /// Displays values in Flight grid from given Data Table.
        /// </summary>        
        /// <param name="dt">Data Table which contains values to be shown in grid.</param>
        public void SetRouteGridValues(DataTable dt)
        {
            DataRow dr;
            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                dr = dt.Rows[rowIndex];
                if (dr == null)
                    continue;
                ((TextBox)(grdRouting.Rows[rowIndex].FindControl("txtFltOrig"))).Text = dr["FltOrigin"].ToString();
                ((TextBox)(grdRouting.Rows[rowIndex].FindControl("txtFltDest"))).Text = dr["FltDestination"].ToString();
                if (dr["FltDestination"].ToString() != "" && dr["FltOrigin"].ToString() != "")
                {
                    LoadFlightDropdown(dr["FltOrigin"].ToString(), dr["FltDestination"].ToString(), rowIndex);
                    ((DropDownList)(grdRouting.Rows[rowIndex].FindControl("ddlFltNum"))).SelectedValue = dr["FltNumber"].ToString();
                    ((HiddenField)(grdRouting.Rows[rowIndex].FindControl("hdnFltNum"))).Value = dr["FltNumber"].ToString();
                }
                else
                {
                    ((DropDownList)(grdRouting.Rows[rowIndex].FindControl("ddlFltNum"))).Items.Clear();
                    ((HiddenField)(grdRouting.Rows[rowIndex].FindControl("hdnFltNum"))).Value = "";
                }
                ((TextBox)(grdRouting.Rows[rowIndex].FindControl("txtFdate"))).Text = dr["FltDate"].ToString();
            }
        }
        #endregion Set Material Grid Values

        #region Load Flight Dropdown
        public void LoadFlightDropdown(string Origin, string Destination, int rowIndex)
        {
            string ds = objBLL.GetFlightListByRoute(Origin, Destination);
            DropDownList ddl = ((DropDownList)(grdRouting.Rows[rowIndex].FindControl("ddlFltNum")));
            if (ds != null && ds != "")
            {
                string[] fltnum = ds.Split(';');
                if (fltnum != null)
                {
                    ddl.Items.Clear();
                    foreach (var item in fltnum)
                    {
                        if (item != "")
                        {
                            ddl.Items.Add(new ListItem(item, item));
                        }
                    }
                }
            }
        }
        #endregion Load Flight Dropdown

        #region LoadGridRateDetail
        /// <summary>
        /// Loads data into Rate grid.
        /// </summary>
        public void LoadGridRateDetail()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("CommCode");
                dt.Columns.Add("Pcs");
                dt.Columns.Add("Weight");
                dt.Columns.Add("FrIATA");
                dt.Columns.Add("FrMKT");
                dt.Columns.Add("ValCharge");
                dt.Columns.Add("PayMode");
                dt.Columns.Add("OcDueCar");
                dt.Columns.Add("OcDueAgent");
                dt.Columns.Add("DynRate");
                dt.Columns.Add("ServTax");
                dt.Columns.Add("Total");
                dt.Columns.Add("ChargedWeight");
                dt.Columns.Add("SpotFreight");
                dt.Columns.Add("SpotRate");
                dt.Columns.Add("SpotRateID");
                dt.Columns.Add("RatePerKg");
                dt.Columns.Add("RateClass");
                dt.Columns.Add("Currency");
                dt.Columns.Add("IATATax");
                dt.Columns.Add("MKTTax");
                dt.Columns.Add("OCTax");
                dt.Columns.Add("OATax");
                dt.Columns.Add("SpotTax");
                dt.Columns.Add("CommTax");
                dt.Columns.Add("DiscTax");
                dt.Columns.Add("Commission");
                dt.Columns.Add("Discount");
                dt.Columns.Add("CommPercent");
                dt.Columns.Add("SpotStatus");
                dt.Columns.Add("IATARateID");
                dt.Columns.Add("MKTRateID");
                //IATATax	MKTTax	OCTax	OATax	SpotTax	CommTax	DiscTax	Commission	Discount	CommPercent

                DataRow row;
                row = dt.NewRow();
                row["CommCode"] = "";// "TV";
                row["Pcs"] = "";
                row["Weight"] = "";// "7KG";
                row["FrIATA"] = "0.0";// "IATA";
                row["FrMKT"] = "";// "IATA";
                row["ValCharge"] = "";// "IATA";
                row["PayMode"] = "";// "COD";
                row["OcDueCar"] = "";// "200.00";
                row["OcDueAgent"] = "";// "200.00";
                row["SpotRate"] = "0";// "500.00";
                row["SpotFreight"] = "0";// "500.00";
                row["DynRate"] = "";// "0";
                row["ServTax"] = "";// "0";
                row["Total"] = "";// "0";
                row["ChargedWeight"] = "";// "0";
                row["SpotRateID"] = "";// "0";
                row["RatePerKg"] = "";// "0";
                row["RateClass"] = "";// "0";               
                row["Currency"] = "";

                row["IATATax"] = "0";
                row["MKTTax"] = "0";
                row["OCTax"] = "0";
                row["OATax"] = "0";
                row["SpotTax"] = "0";
                row["CommTax"] = "0";
                row["DiscTax"] = "0";
                row["Commission"] = "0";
                row["Discount"] = "0";
                row["CommPercent"] = "0";
                row["SpotStatus"] = "0";
                row["IATARateID"] = "0";
                row["MKTRateID"] = "0";
                dt.Rows.Add(row);


                GRDRates.DataSource = null;
                GRDRates.DataSource = dt.Copy();
                GRDRates.DataBind();

                Session["dtRates"] = dt.Copy();
                //Set paymode types in dropdown.
                DropDownList ddl = (DropDownList)GRDRates.Rows[0].FindControl("ddlPayMode");
                if (Session["ConBooking_PayModeMaster"] != null)
                {
                    DataTable dtPayMode = null;
                    dtPayMode = (DataTable)Session["ConBooking_PayModeMaster"];
                    ddl.Items.Clear();
                    ddl.DataSource = dtPayMode;
                    ddl.DataTextField = "PayModeText";
                    ddl.DataValueField = "PayModeCode";
                    ddl.DataBind();
                    if (ddl.Items.Count > 1)
                        ddl.SelectedIndex = 1;

                }

            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        #endregion LoadGridRateDetail

        #region IsMod7
        /// <summary>
        /// Verifies if AWB number is satisfying Mod7 condition.
        /// </summary>
        /// <param name="value">AWB Number to be validated for Mod7.</param>
        private bool IsMod7(int value)
        {
            try
            {
                string val = value.ToString();
                if (value % 7 != int.Parse(val.Substring(val.Length - 1)))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion IsMod7

        #region btnSearchAWB_Click
        protected void btnSearchAWB_Click(object sender, EventArgs e)
        {

            //Code to check wether agent code is coming or not
            if (Session["AgentCode"] == null || Session["AgentCode"].ToString() == "")
            {
                btnSave.Enabled = true;


            }
            else
            {
                btnSave.Visible = false;
                //ddlAgtCode.Enabled = false;
                TXTAgentCode.Enabled = false;
                txtAgentName.Enabled = false;
                //txtAWBNo.Enabled = false;
                txtAWBNo.ReadOnly = true;
                ddlDest.Enabled = false;
                btnProcess.Enabled = false;
                txtShipmentDate.Enabled = false;
                ddlShipmentTime.Enabled = false;
                ddlProductType.Enabled = false;
                txtSLAC.Enabled = false;
                ddlProductType.Enabled = false;
                imgProductType.Enabled = false;

                txtExecutionDate.Enabled = false;
                btnExecutionDate.Enabled = false;
                txtShipmentDate.Enabled = false;
                ddlShipmentTime.Enabled = false;
                txtExecutedAt.Enabled = false;
                txtExecutedBy.Enabled = false;

            }
            //end


            //ViewState["BookingID"] = "0";
            Session["BookingID"] = "0";
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.Red;
            try
            {
                if (txtAWBNo.Text.Length < 5)
                {
                    lblStatus.Text = "Please enter Valid AWB Number.";
                    return;
                }

                object[] AWBParams = new string[4];
                int i = 0;
                //0
                AWBParams.SetValue(ddlDocType.SelectedValue, i);
                i++;

                //1
                AWBParams.SetValue(txtAwbPrefix.Text, i);
                i++;

                //2
                AWBParams.SetValue(txtAWBNo.Text, i);
                i++;

                //3
                //if (ddlAgtCode.SelectedIndex < 1)
                //{
                //    AWBParams.SetValue("", i);
                //}
                //else
                //{
                //    AWBParams.SetValue(ddlAgtCode.SelectedItem.Text, i);
                //}

                AWBParams.SetValue(TXTAgentCode.Text, i);

                DataSet ds = objBLL.GetAWBInfo(AWBParams);
                if (ds != null)
                {
                    DataTable dt;
                    if (ds.Tables.Count > 2)
                    {
                        //Show AWB summary
                        dt = ds.Tables[0];
                        Session["BookingID"] = dt.Rows[0]["SerialNumber"].ToString();
                        //ViewState["BookingID"] = dt.Rows[0]["SerialNumber"].ToString();
                        ddlOrg.SelectedValue = dt.Rows[0]["OriginCode"].ToString();
                        ddlDest.SelectedValue = dt.Rows[0]["DestinationCode"].ToString();
                        //User Agent Name as value member for ddlAgtCode
                        //ddlAgtCode.SelectedValue = dt.Rows[0]["AgentName"].ToString();
                        TXTAgentCode.Text = dt.Rows[0]["AgentName"].ToString();

                        txtAgentName.Text = dt.Rows[0]["AgentName"].ToString();
                        ddlServiceclass.SelectedValue = dt.Rows[0]["ServiceCargoClassId"].ToString();
                        txtHandling.Text = dt.Rows[0]["HandlingInfo"].ToString();
                        txtExecutionDate.Text = dt.Rows[0]["ExecutionDate"].ToString();
                        txtExecutedBy.Text = dt.Rows[0]["ExecutedBy"].ToString();
                        txtExecutedAt.Text = dt.Rows[0]["ExecutedAt"].ToString();
                        if (dt.Rows[0]["ShipmentDate"] != null && dt.Rows[0]["ShipmentDate"].ToString() != "")
                        {
                            txtShipmentDate.Text = Convert.ToDateTime(dt.Rows[0]["ShipmentDate"].ToString()).ToString("dd/MM/yyyy");
                            ddlShipmentTime.SelectedValue = Convert.ToDateTime(dt.Rows[0]["ShipmentDate"].ToString()).ToString("HH:mm");
                        }
                        else
                        {
                            txtShipmentDate.Text = "";
                            ddlShipmentTime.SelectedValue = "25:00";
                        }
                        //Show AWB Material Details
                        grdMaterialDetails.DataSource = ds.Tables[1];
                        grdMaterialDetails.DataBind();
                        LoadCommodityDropdown();
                        SetMaterialGridValues(ds.Tables[1]);

                        //Show AWB Route Details
                        grdRouting.DataSource = ds.Tables[2];
                        grdRouting.DataBind();
                        SetRouteGridValues(ds.Tables[2]);
                        ////grdRouting.DataSource = ds.Tables[2];
                        ////grdMaterialDetails.DataBind();




                        //Show Rates Details
                        GRDRates.DataSource = ds.Tables[3];
                        GRDRates.DataBind();



                        //txtAWBNo.Enabled = false;
                        txtAWBNo.ReadOnly = true;
                        SetLabelValues(false);

                        //Calculate Total Wt, Total Pieces for FFR
                        if (Session["AgentCode"] == null || Session["AgentCode"].ToString() == "")
                        {
                            grdMaterialDetails.Enabled = true;

                        }
                        else
                        {
                            grdMaterialDetails.Enabled = false;
                        }
                        //

                        //vinayak                        
                        grdMaterialDetails.Enabled = false;
                        //ddlAgtCode.Enabled = false;
                        TXTAgentCode.Enabled = false;
                        ddlDest.Enabled = false;
                        btnProcess.Enabled = false;
                        txtShipmentDate.Enabled = false;
                        ddlShipmentTime.Enabled = false;
                        ddlProductType.Enabled = false;
                        txtSLAC.Enabled = false;
                        ddlProductType.Enabled = false;
                        imgProductType.Enabled = false;

                        txtAgentName.Enabled = false;
                        txtExecutedAt.Enabled = false;
                        txtExecutedBy.Enabled = false;
                        txtExecutionDate.Enabled = false;
                        btnExecutionDate.Enabled = false;
                        txtShipmentDate.Enabled = false;
                        ddlShipmentTime.Enabled = false;

                        txtHandling.Enabled = false;

                        //////////////////////////////////
                        grdRouting.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        try
                        {
                            if (ds != null)
                                ds.Dispose();
                            if (dt != null)
                                dt.Dispose();
                        }
                        catch (Exception ex) { }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "AWB details not found: " + ex.Message;
            }
            lblStatus.Text = "AWB details not found. Please enter valid AWB Number.";
            SetLabelValues(true);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }
        #endregion btnSearchAWB_Click

        #region Set Label Values
        /// <summary>
        /// Sets session values which are used in Label Printing popup screen.
        /// </summary>
        /// <param name="SetEmpty">If true, then session variables are made empty.
        /// Else, actual values are assigned to session variables.</param>
        private void SetLabelValues(bool SetEmpty)
        {
            if (!SetEmpty)
            {
                if (txtAWBNo.Text.Length <= 4)
                    return;
                HidAWBP1.Value = txtAWBNo.Text.Substring(0, txtAWBNo.Text.Length - 4);
                //Session["AWBPartI"] = txtAWBNo.Text.Substring(0, txtAWBNo.Text.Length - 4);
                HidAWBP2.Value = txtAWBNo.Text.Substring(txtAWBNo.Text.Length - 4);
                //Session["AWBPartII"] = txtAWBNo.Text.Substring(txtAWBNo.Text.Length - 4);
                HidDest.Value = ddlDest.SelectedValue;
                //Session["Destination"] = ddlDest.SelectedValue;
                HidSource.Value = ddlOrg.SelectedValue;
                //Session["Origin"] = ddlOrg.SelectedValue;
                //Get Total PCS count.
                intTotalPcs = 0;
                for (int cnt = 0; cnt < grdMaterialDetails.Rows.Count; cnt++)
                {
                    //Total pieces
                    TextBox tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("TXTPcs"));
                    intTotalPcs = intTotalPcs + int.Parse(tempTextBox.Text);
                    tempTextBox = null;

                    //Total Weight
                    tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("txtCommGrossWt"));
                    floatGrossWt = floatGrossWt + (float)Convert.ToDecimal(tempTextBox.Text);
                    tempTextBox = null;

                }
                HidPcsCount.Value = intTotalPcs.ToString();
                //Session["Pcs"] = intTotalPcs;
                HidVia.Value = "";
                //Session["Via"] = "";
                HidWt.Value = floatGrossWt.ToString();
                //Session["PcsCount"] = "1";

                //HidAWBP1.Value = "11000";
                //HidAWBP2.Value = "0012";
                //HidDest.Value = "DEL";
                //HidSource.Value = "BOM";
                //HidPcsCount.Value = "3";
                //HidVia.Value = "";
                //HidWt.Value = "0";
                //btnPrintLabels.Enabled = true;
                HidAWBPrefix.Value = txtAwbPrefix.Text.ToString();
            }
            else
            {
                //Session["AWBPartI"] = "";
                //Session["AWBPartII"] = "";
                //Session["Destination"] = "";
                //Session["Origin"] = "";
                //Session["Pcs"] = "0";
                //Session["Via"] = "";
                //Session["PcsCount"] = "0";
                HidAWBP1.Value = "";
                HidAWBP2.Value = "";
                HidDest.Value = "";
                HidSource.Value = "";
                HidPcsCount.Value = "0";
                HidVia.Value = "";
                HidWt.Value = "0";
                //btnPrintLabels.Enabled = false;
            }
        }
        #endregion Set Label Values

        #region btnSave_Click
        public void btnSave_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.Red;

            int intAcceptedPcsforSummary = 0;
            decimal dcAcceptedWtforSummary = 0;
            string strCommodityforSummary = string.Empty;

            try
            {
                if (txtAWBNo.Text.Trim() != "")
                {
                    Int64 AWBNo = Convert.ToInt64(txtAWBNo.Text.Trim().Substring(0, 7));
                    int LstDigit = Convert.ToInt32(txtAWBNo.Text.Trim().Substring(7, 1));

                    if ((AWBNo % 7) != LstDigit)
                    {
                        lblStatus.Text = "Please enter valid AWB.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }
            }
            catch
            {
                lblStatus.Text = "Please enter valid AWB.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                txtAWBNo.Focus();
                return;
            }

            try
            {
                string AgentCode = Convert.ToString(Session["ACode"]);

                if (AgentCode.Trim() != "" && Convert.ToString(Session["AWBStatus"]).Trim() != "B")
                {
                    lblStatus.Text = "AWB can not be modified at this stage.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                bool checkbeforereopen = true;
                checkbeforereopen = CheckIfAWBUpdateAllowedForRole();
                if (checkbeforereopen)
                {
                    if (HidAWBNumber.Value.ToString().Trim() != "")
                    {
                        if (objBLL.CheckforFinalStatus(HidAWBNumber.Value.ToString().Trim(), txtAwbPrefix.Text))
                        {
                            lblStatus.Text = "AWB number is finalized in billing. Modifications can not be done.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                string Errmessage = "";
                if (HidAWBNumber.Value.ToString().Trim() != "")
                {
                    string PaymentMode = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                    if (objBLL.SetAWBStatus(HidAWBNumber.Value.ToString().Trim(), "", ref Errmessage,
                        dtCurrentDate.ToString("dd/MM/yyyy"), strUserName, dtCurrentDate,
                        txtAwbPrefix.Text, checkbeforereopen, PaymentMode) == false)
                    {
                        if (Errmessage.Trim().Length > 0)
                        {
                            lblStatus.Text = Errmessage;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }

                //-----------------------------------------------------------------------------------------------------------
                //Swapnil
                if (!ValidateData())
                    return;
                //-------------------------------------------------------------------------------------------------------------

                if (txtAWBNo.Text.Trim() != "" && HidAWBNumber.Value == "")
                    HidAWBNumber.Value = txtAWBNo.Text.Trim();

                #region Check Manifest
                if (checkbeforereopen)
                {
                    if (txtAWBNo.Text.Trim() != "")
                    {
                        System.Text.StringBuilder strFligtDet = new System.Text.StringBuilder();
                        try
                        {
                            for (int ICount = 0; ICount < grdRouting.Rows.Count; ICount++)
                            {
                                string aircargo = ((DropDownList)grdRouting.Rows[ICount].FindControl("ddlPartner")).Text.Trim();
                                strFligtDet.Append("Insert into #tblFltDetails values('");
                                if (aircargo.Equals("other", StringComparison.OrdinalIgnoreCase))
                                {
                                    strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtFlightID")).Text.Trim());
                                }
                                else
                                {
                                    strFligtDet.Append(((DropDownList)grdRouting.Rows[ICount].FindControl("ddlFltNum")).Text.Trim());
                                }

                                strFligtDet.Append("','");
                                strFligtDet.Append(DateTime.ParseExact(((TextBox)grdRouting.Rows[ICount].FindControl("txtFdate")).Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
                                strFligtDet.Append("','");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtFltOrig")).Text.Trim());
                                strFligtDet.Append("','");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtFltDest")).Text.Trim());
                                strFligtDet.Append("',");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtPcs")).Text.Trim());
                                strFligtDet.Append(",");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtWt")).Text.Trim());
                                strFligtDet.Append(") ");
                            }
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "ERR:" + ex.Message;
                        }

                        string strResult = new BookingBAL().CheckFlightDetails(txtAWBNo.Text.Trim(), txtAwbPrefix.Text, strFligtDet.ToString());

                        if (strResult.Length > 0)
                        {
                            lblStatus.Text = strResult;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                #endregion Check Manifest


                #region DGR Cargo
                //21-8-2012 Swapnil
                //-----------------------------------------------------------------------------
                bool blnres;

                for (int cnt = 0; cnt < grdMaterialDetails.Rows.Count; cnt++)
                {
                    string CommCode = "";
                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                    {
                        CommCode = ((DropDownList)(grdMaterialDetails.Rows[cnt].FindControl("ddlMaterialCommCode"))).Text.ToString();
                    }
                    else
                    {
                        CommCode = ((TextBox)(grdMaterialDetails.Rows[cnt].FindControl("ddlMaterialCommCode1"))).Text.ToString();
                    }
                    if (CommCode == "")
                    {
                        lblStatus.Text = "Please Select Commodity Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }


                    if (CommCode.ToUpper().Trim() == "DGR" || txtSpecialHandlingCode.Text.ToUpper().Trim() == "DGR" || objBLL.CheckDGRCargo(CommCode, txtSpecialHandlingCode.Text.Trim()))
                    {
                        if (Session["DgrCargo"] == null || ((DataTable)Session["DgrCargo"]).Rows.Count < 1)
                        {
                            SQLServer da = new SQLServer(Global.GetConnectionString());
                            DataSet ds = da.SelectRecords("SpGetDGRCargo", "AWBNumber", HidAWBNumber.Value, SqlDbType.VarChar);

                            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                            {
                                lblStatus.Text = "Please enter DGR Details";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                            else
                            {
                                try
                                {
                                    da = null;
                                    ds.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    ds = null;
                                }
                            }
                        }
                    }

                }

                #endregion DGR Cargo

                //  txtAWBNo.Text = "23456788";

                //Calculate totals...
                intTotalPcs = 0;
                floatGrossWt = 0;
                floatVolWt = 0;
                floatChargedWt = 0;
                for (int cnt = 0; cnt < grdMaterialDetails.Rows.Count; cnt++)
                {
                    //Total pieces                   
                    TextBox tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("TXTPcs"));
                    intTotalPcs = intTotalPcs + int.Parse(tempTextBox.Text);
                    tempTextBox = null;

                    //Total gross wt                    
                    tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("txtCommGrossWt"));
                    floatGrossWt = floatGrossWt + (float)Convert.ToDecimal(tempTextBox.Text);
                    tempTextBox = null;

                    //Volumetric wt                    
                    tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("txtCommVolWt"));
                    floatVolWt = floatVolWt + (float)Convert.ToDecimal(tempTextBox.Text.Trim() == "" ? "0" : tempTextBox.Text.Trim());
                    tempTextBox = null;

                    //Charged wt                    
                    tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("txtCommChargedWt"));
                    floatChargedWt = floatChargedWt + (float)Convert.ToDecimal(tempTextBox.Text);

                    tempTextBox.Dispose();

                }

                if (CHKExportShipment.Checked == true && txtHandling.Text.Trim() == "")
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Please enter Invoice details in Handling info.";
                    return;
                }

                //Check Accepted pieces with Actual


                for (int IntCount = 0; IntCount < grdRouting.Rows.Count; IntCount++)
                {
                    int BkPcs = 0;
                    decimal BkWt = 0;

                    int AccPcs = 0;
                    decimal AccWt = 0;

                    try
                    {
                        AccPcs = Convert.ToInt32(((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedPcs")).Text);
                    }
                    catch
                    {
                        AccPcs = 0;
                    }

                    try
                    {
                        AccWt = Convert.ToDecimal(((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedWt")).Text);
                    }
                    catch
                    {
                        AccWt = 0;
                    }

                    if (Convert.ToBoolean(((CheckBox)grdRouting.Rows[IntCount].FindControl("chkAccepted")).Checked) || AccPcs > 0)
                    {
                        BkPcs = Convert.ToInt32(((TextBox)grdRouting.Rows[IntCount].FindControl("txtPcs")).Text);
                        BkWt = Convert.ToDecimal(((TextBox)grdRouting.Rows[IntCount].FindControl("txtWt")).Text);

                        //AccPcs = Convert.ToInt32(((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedPcs")).Text);
                        //AccWt = Convert.ToDecimal(((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedWt")).Text);
                        LoginBL objLBAL = new LoginBL();
                        bool validatePcsWt = true;
                        try
                        {
                            //validatePcsWt = Convert.ToBoolean(objLBAL.GetMasterConfiguration("AcceptAdditionalWt"));
                            validatePcsWt = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AcceptAdditionalWt"));
                        }
                        catch (Exception ex)
                        {
                            validatePcsWt = true;
                        }
                        if (!validatePcsWt)
                        {
                            if (BkPcs < AccPcs)
                            {
                                LBLRouteStatus.ForeColor = Color.Red;
                                LBLRouteStatus.Text = "Accepted pieces can not be more than booked pieces.";
                                return;
                            }

                            if (BkWt < AccWt)
                            {
                                LBLRouteStatus.ForeColor = Color.Red;
                                LBLRouteStatus.Text = "Accepted weight can not be more than booked weight.";
                                return;
                            }
                        }
                        //Commented by Vishal for issue DS-48
                        /*
                        if (BkPcs == AccPcs && BkWt != AccWt)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            LBLRouteStatus.Text = "Please enter valid Weight.";
                            ((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedWt")).Focus();
                            return;
                        }

                        if (BkWt == AccWt && BkPcs != AccPcs)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            LBLRouteStatus.Text = "Please enter valid Pieces.";
                            ((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedPcs")).Focus();
                            return;
                        }
                        */
                        if (AccPcs == 0 && AccWt != 0)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            LBLRouteStatus.Text = "Please enter valid Pieces.";
                            ((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedPcs")).Focus();
                            return;
                        }

                        if (AccWt == 0 && AccPcs != 0)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            LBLRouteStatus.Text = "Please enter valid Weight.";
                            ((TextBox)grdRouting.Rows[IntCount].FindControl("txtAcceptedWt")).Focus();
                            return;
                        }
                    }
                }

                //End

                float fltMtGrossWt = floatGrossWt;
                float fltMtChrgblWt = floatChargedWt;

                float fltFltGrossWt = 0;
                float fltFltChrgblWt = 0;

                if (Session["AWBStatus"] != null)
                {
                    if ((string)Session["AWBStatus"] == "E" || (string)Session["AWBStatus"] == "B" || (string)Session["AWBStatus"] == "R")
                    {
                        //if (Session["Mod"] != null && (string)Session["Mod"] == "1")
                        //{
                        //    lblStatus.ForeColor = Color.Green;
                        //    lblStatus.Text = "Kindly click on Process rates to proceed.";
                        //    return;
                        //}

                        if (HidProcessFlag.Value == "1")
                        {
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Kindly click on Process rates to proceed.";
                            txtAWBNo.Focus();
                            return;
                        }
                    }
                }

                #region "Embargo Verification"

                //Check for Embargo cargo

                string strCommodity = string.Empty;
                DateTime dtExecutionDt = DateTime.ParseExact(txtExecutionDate.Text, "dd/MM/yyyy", null);
                string strOrigin = string.Empty;
                string strDestination = string.Empty;
                string strPaymentType = string.Empty;
                string strFlightNo = string.Empty;
                // BookingBAL objBooking = new BookingBAL();
                string strcarrier = string.Empty;
                for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                {
                    strCommodity = ((TextBox)(GRDRates.Rows[0].FindControl("TXTCommCode"))).Text;
                    strOrigin = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text;
                    strDestination = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltDest"))).Text;
                    strPaymentType = ((DropDownList)(GRDRates.Rows[0].FindControl("ddlPayMode"))).Text;
                    strcarrier = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartner"))).SelectedItem.Text;
                    try
                    {
                        if (strcarrier.Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            strFlightNo = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFlightID"))).Text;
                        }
                        else
                        {
                            strFlightNo = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text;
                        }
                    }
                    catch (Exception ex)
                    { }
                    if (strOrigin.Trim() == ddlOrg.Text.Trim())
                    {
                        fltFltGrossWt = fltFltGrossWt + (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtWt"))).Text);
                        fltFltChrgblWt = fltFltChrgblWt + (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtChrgWt"))).Text);
                    }

                    DataSet Objds = null;
                    try
                    {
                        Objds = objBLL.VerifyEmbargoCargo(dtExecutionDt, strOrigin, strDestination, strCommodity, strPaymentType, strFlightNo);

                        if (Objds != null && Objds.Tables.Count > 0 && Objds.Tables[0].Rows.Count > 0)
                        {
                            if ((bool)Objds.Tables[0].Rows[0]["IsInvalid"] == true)
                            {
                                if ((string)Objds.Tables[0].Rows[0]["ErrorType"] == "Error")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = Convert.ToString(Objds.Tables[0].Rows[0]["ErrorDesc"]);
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Objds = null;
                    }
                    finally
                    {
                        if (Objds != null)
                            Objds.Dispose();
                    }
                }

                #endregion

                if (fltMtGrossWt != fltFltGrossWt || fltMtChrgblWt != fltFltChrgblWt)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Material weights & Flight weights are not matching.";
                    return;
                }



                //Check for Agent account balance incase of "PP"
                string PaymentType = ((DropDownList)GRDRates.Rows[0].FindControl("ddlPayMode")).Text;
                decimal AWBAmount = Convert.ToDecimal(((TextBox)GRDRates.Rows[0].FindControl("TXTTotal")).Text);
                string TranAccount = string.Empty, Remarks = string.Empty, TranType = string.Empty;
                decimal BGAmount = 0, AWBPrevAmt = 0, BankGAmt = 0, ThrValue = 0;
                bool ValidateBG = false;

                //Check if Pay Mode 'PX' exists in the system.
                DropDownList ddl = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"));
                ListItem litem = ddl.Items.FindByValue("PX");
                if ((litem != null && PaymentType == "PX") || (litem == null && PaymentType == "PP"))
                {
                    bool blnAllow = CheckforAgentsBalance(TXTAgentCode.Text.Trim(), AWBAmount, HidAWBNumber.Value,
                        ref TranAccount, ref BGAmount, ref AWBPrevAmt, ref BankGAmt, ref ThrValue, ref ValidateBG, txtAwbPrefix.Text.Trim(), 1);
                    if (blnAllow != true && ValidateBG == true)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Agent is not having sufficient balance to create AWB.";
                        return;
                    }

                    if ((AWBPrevAmt - AWBAmount) >= 0)
                    {
                        AWBAmount = AWBPrevAmt - AWBAmount;
                        TranType = "Credit";
                    }
                    else
                    {
                        AWBAmount = AWBAmount - AWBPrevAmt;
                        TranType = "Debit";
                    }

                }

                HidEAWB.Value = "0";

                #region Save AWB Summary
                //Save AWB Summary data in database for new AWB.
                #region Prepare Parameters
                //Added by Vishal - 04 MAY 2014 ******************** 
                object[] awbInfo = new object[51];
                //******************** Added by Vishal - 04 MAY 2014 
                int i = 0;

                //0
                awbInfo.SetValue(Session["BookingID"], i);
                i++;

                //1
                awbInfo.SetValue(ddlDocType.SelectedValue, i);
                i++;

                //2
                awbInfo.SetValue(txtAwbPrefix.Text, i);
                i++;

                //3
                //awbInfo.SetValue(txtAWBNo.Text, i);
                awbInfo.SetValue(HidAWBNumber.Value, i);
                i++;

                //4
                awbInfo.SetValue(intTotalPcs, i);
                i++;

                //5
                awbInfo.SetValue(fltMtGrossWt, i);
                i++;

                //6
                awbInfo.SetValue(floatVolWt, i);
                i++;

                //7
                awbInfo.SetValue(floatChargedWt, i);
                i++;

                //8
                awbInfo.SetValue(ddlOrg.SelectedValue.ToString().ToUpper(), i);
                i++;

                //9
                awbInfo.SetValue(ddlDest.SelectedValue.ToString().ToUpper(), i);
                i++;

                ////10
                //awbInfo.SetValue(ddlAgtCode.SelectedItem.Text, i);
                //i++;

                ////11
                //awbInfo.SetValue(ddlAgtCode.SelectedValue, i);
                //txtAgentName.Text = ddlAgtCode.SelectedValue;
                //i++;

                //10
                awbInfo.SetValue(TXTAgentCode.Text, i);
                i++;

                //11
                awbInfo.SetValue(txtAgentName.Text, i);
                i++;

                //12
                awbInfo.SetValue(ddlServiceclass.SelectedIndex, i);
                i++;

                //13
                awbInfo.SetValue(txtHandling.Text, i);
                i++;

                //14
                awbInfo.SetValue(txtExecutionDate.Text, i);
                i++;

                //15
                awbInfo.SetValue(txtExecutedBy.Text, i);
                i++;

                //16
                awbInfo.SetValue(txtExecutedAt.Text, i);
                i++;

                //17
                awbInfo.SetValue(Session["UserName"], i);
                i++;

                //18
                awbInfo.SetValue(dtCurrentDate, i);
                i++;

                //19
                awbInfo.SetValue(CHKConsole.Checked, i);
                i++;

                //20
                awbInfo.SetValue(CHKBonded.Checked, i);
                i++;

                //21
                awbInfo.SetValue(CHKExportShipment.Checked, i);
                i++;

                //22
                awbInfo.SetValue(CHKAsAggred.Checked, i);
                i++;

                //23
                //awbInfo.SetValue(txtRemarks.Text.Trim(), i);
                awbInfo.SetValue(txtComment.Text.Trim(), i);
                i++;
                //24
                awbInfo.SetValue(chkInterline.Checked, i);

                i++;
                //25
                awbInfo.SetValue(txtSLAC.Text.Trim(), i);

                i++;
                //26
                awbInfo.SetValue(txtCustoms.Text.Trim(), i);

                i++;
                //27
                awbInfo.SetValue(txtEURIN.Text.Trim(), i);

                i++;
                //28
                awbInfo.SetValue(ddlIrregularityCode.SelectedValue, i);

                i++;
                //29

                float fltForCus = 0;

                if (TXTDvForCustoms.Text.Trim() != "")
                    fltForCus = float.Parse(TXTDvForCustoms.Text.Trim());

                awbInfo.SetValue(fltForCus, i);

                i++;
                //30

                float fltForCarr = 0;

                if (TXTDvForCarriage.Text.Trim() != "")
                    fltForCarr = float.Parse(TXTDvForCarriage.Text.Trim());

                awbInfo.SetValue(fltForCarr, i);

                i++;
                awbInfo.SetValue(txtSpecialHandlingCode.Text.Trim(','), i);

                i++;
                awbInfo.SetValue(ddlProductType.SelectedValue, i);

                i++;
                awbInfo.SetValue(chkDlvOnHAWB.Checked, i);

                i++;
                awbInfo.SetValue(ddlAirlineCode.Text.Trim(), i);

                i++;
                awbInfo.SetValue(TXTAgentCode.Text.Trim(), i);

                i++;
                awbInfo.SetValue(ddlHandler.SelectedValue, i);

                i++;
                awbInfo.SetValue(txtShippingAWB.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtAttchDoc.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtDriverName.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtDriverDL.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtPhoneNo.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtVehicleNo.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtIACCode.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtKnownShipper.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtCCSF.Text.Trim(), i);

                i++;
                awbInfo.SetValue(chkTBScreened.Checked, i);

                i++;
                if (txtShipmentDate.Text == "")
                    awbInfo.SetValue("", i);
                else
                    awbInfo.SetValue(txtShipmentDate.Text + " " + ddlShipmentTime.SelectedValue, i);

                //Added by Vishal - 04 MAY 2014 ******************** 
                i++;
                awbInfo.SetValue(txtUOM.Text, i);
                //******************** Added by Vishal - 04 MAY 2014

                //Added by Poorna - 06 June 2014 ******************** 
                i++;
                awbInfo.SetValue(txtPackingInfo.Text.Trim(), i);

                i++;
                awbInfo.SetValue(txtSCI.Text.Trim(), i);
                //Added by Poorna - 06 June 2014 ********************

                #endregion Prepare Parameters

                string ErrorMessage = string.Empty;
                int iteration = 0;

                while (iteration < 2)
                {
                    //Update database.
                    if (objBLL.SaveAWBSummary_GHA(awbInfo, ref ErrorMessage) < 0)
                    {
                        //HidBookingError.Value = "AWB Save failed. Please try again...";
                        lblStatus.Text = "AWB Save failed. Please try again...";

                        if (ErrorMessage.Length > 0 && ErrorMessage.ToUpper().IndexOf("DUPLICATE AWB") > 0)
                        {
                            //HidBookingError.Value = "Unable to save at this time. Please try again.";
                            lblStatus.Text = "Unable to save at this time. Please try again.";

                            if (Convert.ToString(Session["BookingID"]) == "0")
                                HidAWBNumber.Value = "";

                            if (ValidateData())
                            {
                                awbInfo.SetValue(HidAWBNumber.Value, 3);
                            }
                        }

                        iteration = iteration + 1;
                        ErrorMessage = "";
                    }
                    else
                    {
                        iteration = 2;
                        //HidBookingError.Value = "";
                    }
                }


                #endregion Save AWB Summary

                //Save Remarks
                if (txtComment.Text.Trim() != "")
                    BookingRemarks();
                //End

                //Delete AWB Material & Route Details.
                //objBLL.DeleteAWBDetails(txtAWBNo.Text);
                objBLL.DeleteAWBDetails(HidAWBNumber.Value, txtAwbPrefix.Text);
                //Check if Pay Mode 'PX' exists in the system.
                if (((litem != null && PaymentType == "PX") || (litem == null && PaymentType == "PP")) && AWBAmount != 0)
                {
                    BALAgentCredit objCredit = new BALAgentCredit();
                    objCredit.SaveTransacation("", TranAccount, "", TranType, 0, 0, double.Parse(AWBAmount.ToString()), "AWB Transaction,AWBNo:" + HidAWBNumber.Value, Session["UserName"].ToString(), HidAWBNumber.Value, "");
                }

                string ShipmentDescription = string.Empty;

                #region Save AWB Material
                //Save material information.
                try
                {

                    //Add material info...
                    for (int lstIndex = 0; lstIndex < grdMaterialDetails.Rows.Count; lstIndex++)
                    {
                        i = 0;
                        awbInfo = null;
                        awbInfo = new object[15];
                        //0
                        //awbInfo.SetValue(txtAWBNo.Text, i);
                        awbInfo.SetValue(HidAWBNumber.Value, i);
                        i++;

                        //1
                        if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                        {
                            DropDownList tempDropDown = (DropDownList)(grdMaterialDetails.Rows[lstIndex].FindControl("ddlMaterialCommCode"));
                            awbInfo.SetValue(tempDropDown.Text.ToUpper(), i);
                            i++;
                            ShipmentDescription = tempDropDown.Text.ToUpper();
                        }
                        else
                        {
                            TextBox tempDropDown = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("ddlMaterialCommCode1"));
                            awbInfo.SetValue(tempDropDown.Text.ToUpper(), i);
                            i++;
                            ShipmentDescription = tempDropDown.Text.ToUpper();
                        }

                        //2
                        TextBox tempTextBox = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("TXTPcs"));
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        //3
                        tempTextBox = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtCommGrossWt"));
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        //4
                        awbInfo.SetValue(((Label)(grdMaterialDetails.Rows[lstIndex].FindControl("lblDimensions"))).Text, i);
                        i++;

                        //5
                        tempTextBox = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtCommVolWt"));
                        awbInfo.SetValue(tempTextBox.Text.Trim() == "" ? "0" : tempTextBox.Text.Trim(), i);
                        i++;

                        //6
                        tempTextBox = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtCommChargedWt"));
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        //7                        
                        awbInfo.SetValue(Session["UserName"], i);
                        i++;

                        //8
                        awbInfo.SetValue(dtCurrentDate, i);
                        i++;

                        //9
                        tempTextBox = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtMaterialCommDesc"));
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        ShipmentDescription = ShipmentDescription + " - " + tempTextBox.Text;

                        //10
                        TextBox txtAccountInfo = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtAccountInfo"));
                        awbInfo.SetValue(txtAccountInfo.Text.Trim(), i);
                        i++;

                        //11
                        DropDownList ddlShipmentType = (DropDownList)(grdMaterialDetails.Rows[lstIndex].FindControl("ddlShipmentType"));
                        awbInfo.SetValue(ddlShipmentType.Text.Trim(), i);
                        i++;

                        //12
                        TextBox txtShipmentRemarks = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtShipmentRemarks"));
                        awbInfo.SetValue(txtShipmentRemarks.Text.Trim(), i);
                        i++;
                        //13 -AWB Prefix
                        awbInfo.SetValue(txtAwbPrefix.Text, i);
                        i++;

                        TextBox txtShipmentPriority = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtShipmentPriority"));
                        awbInfo.SetValue(txtShipmentPriority.Text.Trim(), i);

                        //Call SP to update database.
                        if (objBLL.SaveAWBMaterial_GHA(awbInfo) < 0)
                        {
                            lblStatus.Text = "Error updating AWB Information. Please try again...";
                            awbInfo = null;
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error updating AWB Information: " + ex.Message;
                    return;
                }
                #endregion Save AWB Material

                #region Save AWB Routing
                //Save routing information.
                try
                {
                    for (int lstIndex = 0; lstIndex < grdRouting.Rows.Count; lstIndex++)
                    {

                        int intAccPieces = 0;
                        string strIsAccp = string.Empty;
                        decimal dcAccepWt = 0;

                        intAccPieces = Convert.ToInt32(((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedPcs")).Text.Trim());
                        dcAccepWt = Convert.ToDecimal(((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedWt")).Text.Trim());

                        strIsAccp = ((CheckBox)grdRouting.Rows[lstIndex].FindControl("chkAccepted")).Checked ? "Y" : "N";

                        if (ddlOrg.Text.Trim().ToUpper() == ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFltOrig")).Text.Trim().ToUpper())
                        {
                            intAcceptedPcsforSummary = intAcceptedPcsforSummary + intAccPieces;
                            dcAcceptedWtforSummary = dcAcceptedWtforSummary + dcAccepWt;
                        }

                        if (intAccPieces > 0 && strIsAccp == "N")
                            strIsAccp = "Y";

                        object[] values = {       
                                              //txtAWBNo.Text.Trim(),
                                              HidAWBNumber.Value.Trim(),
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFltOrig")).Text.ToUpper(),
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFltDest")).Text.ToUpper(),
                                              
                                              //(((CheckBox)grdRouting.Rows[lstIndex].FindControl("CHKInterline")).Checked? 
                                              //      ((TextBox)grdRouting.Rows[lstIndex].FindControl("NewFlightID")).Text:
                                              //      ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlFltNum")).SelectedItem.Text),

                                              (((DropDownList)(grdRouting.Rows[lstIndex].FindControl("ddlPartner"))).Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase)? 
                                                    ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFlightID")).Text:
                                                    ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlFltNum")).SelectedItem.Text),
                                              
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFdate")).Text,
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtPcs")).Text,
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtWt")).Text,
                                              ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlStatus")).SelectedItem.Value,
                                              strIsAccp,
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedPcs")).Text.Trim()==""? "0":((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedPcs")).Text.Trim(),
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedWt")).Text.Trim()==""?"0":((TextBox)grdRouting.Rows[lstIndex].FindControl("txtAcceptedWt")).Text.Trim(),
                                              Convert.ToString(Session["UserName"]),
                                              dtCurrentDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                              ((HiddenField)grdRouting.Rows[lstIndex].FindControl("HidScheduleID")).Value.Trim()==""?0:int.Parse(((HiddenField)grdRouting.Rows[lstIndex].FindControl("HidScheduleID")).Value.Trim()),
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtChrgWt")).Text,
                                              ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlPartner")).Text.ToString(),
                                              ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlPartnerType")).Text.ToString(),
                                              txtAwbPrefix.Text, ShipmentDescription.Trim()
                                          };


                        if (objBLL.SaveAWBRoute(values) < 0)
                        {
                            lblStatus.Text = "Error updating Route information. Please try again...";
                            values = null;
                            return;
                        }
                        else
                        {
                            values = null;
                            if (strIsAccp == "Y")
                                SaveOperationTime();
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error updating AWB information: " + ex.Message;
                    return;
                }
                #endregion Save AWB Routing

                #region AWB Dimensions

                int AWBPcs = int.Parse(((TextBox)GRDRates.Rows[0].FindControl("TXTPcs")).Text);
                decimal AWBWt = Convert.ToDecimal(((TextBox)GRDRates.Rows[0].FindControl("TXTCWt")).Text);
                GenerateAWBDimensions(HidAWBNumber.Value, AWBPcs, (DataSet)Session["dsDimesionAll"], AWBWt, true, txtAwbPrefix.Text);
                //GenerateAWBDimensions(HidAWBNumber.Value, AWBPcs, (DataSet)Session["dsDimesionAll"], AWBWt, true, txtAwbPrefix.Text, Convert.ToBoolean(Session["IsBulk"]));
                #endregion

                #region AWB Rates

                for (int k = 0; k < GRDRates.Rows.Count; k++)
                {
                    string CommCode = ((TextBox)GRDRates.Rows[k].FindControl("TXTCommCode")).Text;
                    strCommodityforSummary = CommCode;
                    string PayMode = ((DropDownList)GRDRates.Rows[k].FindControl("ddlPayMode")).Text;

                    int Pcs = int.Parse(((TextBox)GRDRates.Rows[k].FindControl("TXTPcs")).Text);

                    decimal Wt = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTWt")).Text);
                    decimal FrIATA = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTFrIATA")).Text);
                    decimal FrMKT = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTFrMKT")).Text);
                    decimal ValCharge = 0;
                    // Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTValCharg")).Text);
                    decimal OcDueCar = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTOcDueCar")).Text);
                    decimal OcDueAgent = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTOcDueAgent")).Text);
                    decimal SpotRate = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTSpotRate")).Text == "" ? "0" : ((TextBox)GRDRates.Rows[k].FindControl("TXTSpotRate")).Text);
                    decimal SpotFreight = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTSpotFreight")).Text == "" ? "0" : ((TextBox)GRDRates.Rows[k].FindControl("TXTSpotFreight")).Text);
                    decimal DynRate = 0;
                    // Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTDynRate")).Text);
                    decimal ServiceTax = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTServiceTax")).Text);
                    decimal Total = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTTotal")).Text);
                    decimal RatePerKg = Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("txtRatePerKg")).Text);
                    string RateCluse = ((TextBox)GRDRates.Rows[k].FindControl("txtRateClass")).Text.Trim();
                    string currency = ((TextBox)GRDRates.Rows[k].FindControl("TXTCurrency")).Text;
                    //Taxex
                    decimal IATATax = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTIATATax")).Text);
                    decimal MKTTax = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTMKTTax")).Text);
                    decimal OATax = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTOATax")).Text);
                    decimal OCTax = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTOCTax")).Text);
                    decimal SpotTax = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTSpotTax")).Text);
                    decimal CommTax = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTCommTax")).Text);
                    decimal DiscTax = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTDiscTax")).Text);
                    decimal Commission = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTCommission")).Text);
                    decimal Discount = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTDiscount")).Text);
                    decimal CommPercent = Convert.ToDecimal(((Label)GRDRates.Rows[k].FindControl("TXTCommPercent")).Text);
                    string SpotStatus = ((Label)GRDRates.Rows[k].FindControl("TXTSpotStatus")).Text;
                    bool AllInRate = CHKAllIn.Checked;
                    string IATARateID = ((Label)GRDRates.Rows[k].FindControl("TXTIATARateID")).Text;
                    string MKTRateID = ((Label)GRDRates.Rows[k].FindControl("TXTMKTRateID")).Text;
                    object[] values = { HidAWBNumber.Value.Trim(), CommCode, PayMode, Pcs, Wt, FrIATA, FrMKT, ValCharge, OcDueCar, OcDueAgent, SpotRate, DynRate, ServiceTax, 
                                          Total, RatePerKg, RateCluse, currency, txtAwbPrefix.Text,SpotFreight,IATATax,MKTTax,OATax,OCTax,SpotTax,CommTax,
                                      DiscTax,Commission,Discount,CommPercent,SpotStatus,AllInRate,IATARateID,MKTRateID};


                    if (!objBLL.SaveAWBRates(values))
                    {
                        lblStatus.Text = "Error updating Rate information. Please try again...";
                        return;
                    }

                }


                DataSet dsDetails = (DataSet)Session["OCDetails"];

                #region Validation for OtherCharges breakup and Total
                //Validation for OtherCharges breakup and Total by Vijay
                decimal FrtIATATotal, FrtMKTTotal, OCDCTotal, ODCATotal;
                FrtIATATotal = 0; FrtMKTTotal = 0; OCDCTotal = 0; ODCATotal = 0;
                decimal txtIATA, txtOcDueCar, txtOcDueAgent;
                txtIATA = 0; txtOcDueCar = 0; txtOcDueAgent = 0;
                if (GRDRates.Rows.Count > 0)
                {
                    txtIATA = Convert.ToDecimal(((TextBox)GRDRates.Rows[0].FindControl("TXTFrIATA")).Text);
                    txtOcDueCar = Convert.ToDecimal(((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueCar")).Text);
                    txtOcDueAgent = Convert.ToDecimal(((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueAgent")).Text);
                }

                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    if (row["Charge Type"].ToString().Trim() == "RateLineIATA" || row["Charge Type"].ToString().Trim() == "IATA")
                    {
                        FrtIATATotal = FrtIATATotal + decimal.Parse(row["Charge"].ToString());
                    }
                }
                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    if (row["Charge Type"].ToString().Trim() == "DC")
                    {
                        OCDCTotal = OCDCTotal + decimal.Parse(row["Charge"].ToString());
                    }
                }
                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    if (row["Charge Type"].ToString().Trim() == "DA")
                    {
                        ODCATotal = ODCATotal + decimal.Parse(row["Charge"].ToString());
                    }
                }
                //if (Math.Round(txtIATA, 0) != Math.Round(FrtIATATotal, 0) || Math.Round(txtOcDueCar, 0) != Math.Round(OCDCTotal, 0) || Math.Round(txtOcDueAgent, 0) != Math.Round(ODCATotal, 0))
                if (Math.Round(txtOcDueCar, 0) != Math.Round(OCDCTotal, 0) || Math.Round(txtOcDueAgent, 0) != Math.Round(ODCATotal, 0))
                {
                    lblStatus.Text = "Kindly click on Process rates to proceed.";
                    lblStatus.ForeColor = Color.Green;
                    return;
                }

                #endregion Validation for OtherCharges breakup and Total
                if (dsDetails != null && dsDetails.Tables.Count > 0 && dsDetails.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strOtherCharges = new StringBuilder();
                    foreach (DataRow row in dsDetails.Tables[0].Rows)
                    {


                        //dsDetailsRow["Charge Type"] = "RateLineIATA";
                        //if (row["Charge Type"].ToString().Trim() == "RateLineIATA")
                        //{

                        //    // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission

                        //    object[] value = { HidAWBNumber.Value.Trim(),int.Parse(row["Charge Head Code"].ToString()),
                        //                       "IATA", decimal.Parse(row["DiscountPercent"].ToString()), 
                        //                       decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                        //                       decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                        //                       decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString(),txtAwbPrefix.Text };

                        //    if (!objBLL.SaveAWBRatesDetails("Freight", value))
                        //    {
                        //        lblStatus.Text = "Error updating Rate information. Please try again...";
                        //        return;
                        //    }
                        //}
                        //else if (row["Charge Type"].ToString().Trim() == "RateLineMKT")
                        //{

                        //    // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission

                        //    object[] value = { HidAWBNumber.Value.Trim(),int.Parse(row["Charge Head Code"].ToString()),
                        //                       "MKT", decimal.Parse(row["DiscountPercent"].ToString()), 
                        //                       decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                        //                       decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                        //                       decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString()};

                        //    if (!objBLL.SaveAWBRatesDetails("Freight", value))
                        //    {
                        //        lblStatus.Text = "Error updating Rate information. Please try again...";
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        if (row["Discount"].ToString() == "")
                            row["Discount"] = "0";
                        if (row["Commission"].ToString() == "")
                            row["Commission"] = "0";
                        if (row["SerialNumber"].ToString() == "")
                            row["SerialNumber"] = "0";
                        string Commo = row["Commission"].ToString();
                        if (Commo == "")
                            Commo = "0";

                        //object[] value = { HidAWBNumber.Value.Trim(),row["Charge Head Code"].ToString(),
                        //                       row["Charge Type"].ToString().Trim(), decimal.Parse(row["DiscountPercent"].ToString()), 
                        //                       decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                        //                       decimal.Parse(row["Discount"].ToString()), decimal.Parse(Commo), 
                        //                       decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),
                        //                       row["Commodity Code"].ToString(),row["ChargeCode"].ToString(),txtAwbPrefix.Text,
                        //                     row["SerialNumber"].ToString()};

                        //if (!objBLL.SaveAWBRatesDetails("OC", value))
                        //{
                        //    lblStatus.Text = "Error updating Rate information. Please try again...";
                        //    return;
                        //}


                        //}
                        strOtherCharges.Append("Insert into #AWBRate (ChargeHeadCode, ChargeType, DiscountPercent, CommPercent, TaxPercent, Discount,");
                        strOtherCharges.Append("Comission, Tax, Charge, CCode, ChargeSrNo) values ('");
                        strOtherCharges.Append(Convert.ToString(row["Charge Head Code"]).Replace("'", "") + "','" + Convert.ToString(row["Charge Type"]).Replace("'", "") + "',");
                        strOtherCharges.Append(Convert.ToString(row["DiscountPercent"]) + "," + Convert.ToString(row["CommPercent"]) + ",");
                        strOtherCharges.Append(Convert.ToString(row["TaxPercent"]) + "," + Convert.ToString(row["Discount"]) + ",");
                        strOtherCharges.Append(Convert.ToString(row["Commission"]) + "," + Convert.ToString(row["Tax"]) + ",");
                        strOtherCharges.Append(Convert.ToString(row["Charge"]) + ",'" + Convert.ToString(row["ChargeCode"]) + "',");
                        strOtherCharges.Append(Convert.ToString(row["SerialNumber"]) + "); ");

                    }
                    if (strOtherCharges != null && strOtherCharges.Length > 0)
                    {

                        if (!objBLL.SaveAWBRatesDetails_QB(txtAwbPrefix.Text.Trim(), HidAWBNumber.Value.Trim(), strCommodityforSummary.Trim(), strOtherCharges.ToString()))
                        {
                            lblStatus.Text = "Error updating Rate information. Please try again...";
                            return;
                        }
                        strOtherCharges = null;
                    }
                }

                #endregion

                #region AWB Shipper/Consignee

                if (!objBLL.SaveAWBShipperConsignee(new object[] { HidAWBNumber.Value ,TXTShipper.Text.Trim(),TXTShipAddress.Text.Trim(),ddlShipCountry.Text,TXTShipTelephone.Text.Trim(),
                                                                   TXTConsignee.Text.Trim(),TXTConAddress.Text.Trim(),ddlConCountry.Text,TXTConTelephone.Text.Trim(),
                                                                   TXTShipperAdd2.Text.Trim(),TXTShipperCity.Text.Trim(),TXTShipperState.Text.Trim(),TXTShipPinCode.Text.Trim(),//Shipper extra data
                                                                   TXTConsigAdd2.Text.Trim(),TXTConsigCity.Text.Trim(),TXTConsigState.Text.Trim(),TXTConsigPinCode.Text.Trim(),
                                                                   txtShipperCode.Text.Trim(), txtConsigneeCode.Text.Trim(),TXTShipperEmail.Text.Trim(),TXTConsigEmail.Text.Trim(),txtAwbPrefix.Text,
                                                                   txtShipperCCSF.Text.Trim(),""
                }))//Consig Extra Data
                {
                    lblStatus.Text = "Save failed (Shipper/Consignee)";
                    return;
                }


                #endregion

                #region Save DGR Cargo

                //21-8-2012 Swapnil
                //-----------------------------------------------------------------------------

                if (Session["DgrCargo"] != null && ((DataTable)Session["DgrCargo"]).Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DgrCargo"];

                    string AWBNumber, UNID, Weight, UpdatedBy, UpdatedOn, PG,ERGCode,Desc;
                    int Pieces;

                    for (int ro = 0; ro < dt.Rows.Count; ro++)
                    {
                       
                        Pieces = Convert.ToInt32(dt.Rows[ro]["Pieces"].ToString());
                        UNID = dt.Rows[ro]["UNID"].ToString();
                        UpdatedBy = dt.Rows[ro]["UpdatedBy"].ToString();
                        UpdatedOn = dt.Rows[ro]["UpdatedOn"].ToString();
                        Weight = dt.Rows[ro]["Weight"].ToString();
                        ERGCode = dt.Rows[ro]["ERGCode"].ToString();
                        Desc = dt.Rows[ro]["Desc"].ToString();
                        AWBNumber = HidAWBNumber.Value;
                        PG = dt.Rows[ro]["PG"].ToString();

                        blnres = dgr.SaveDGRCargo(AWBNumber, UNID, Pieces, Weight, ERGCode, UpdatedBy, UpdatedOn, txtAwbPrefix.Text, PG, Desc);
                        
                        Session["DGRAWB"] = AWBNumber;
                        
                    }

                    try
                    {
                        if (dt != null)
                            dt.Dispose();
                    }
                    catch (Exception ex)
                    {
                        dt = null;
                    }
                }

                #endregion Save DGR Cargo

                SaveULDInformation();

                SaveHAWBInformation();

                #region "AWB Rate Details"
                try
                {
                    if (Session["FltRoute"] != null)
                    {
                        DataTable dtRoute = (DataTable)Session["FltRoute"];

                        if (dtRoute != null && dtRoute.Rows.Count > 0)
                        {
                            System.Text.StringBuilder strRateDetails = new System.Text.StringBuilder();

                            for (int intCount = 0; intCount < dtRoute.Rows.Count; intCount++)
                            {
                                strRateDetails.Append("Insert into #tblRate (FltOrigin, FltDestination, FltNumber, FltDate, Pcs, GWt, CWt, IsPrime,IsHeavy, Freight, FrtTax, RatePerKg,Cost,Proration,AgentComm,Discount,MKTFrt,MKTTax,CostType,Currency) values ('");
                                strRateDetails.Append(dtRoute.Rows[intCount]["FltOrigin"].ToString() + "','" + dtRoute.Rows[intCount]["FltDestination"].ToString() + "','" + dtRoute.Rows[intCount]["FltNumber"].ToString());
                                strRateDetails.Append("','" + dtRoute.Rows[intCount]["FltDate"].ToString() + "'," + dtRoute.Rows[intCount]["Pcs"].ToString() + "," + dtRoute.Rows[intCount]["GWt"].ToString());
                                strRateDetails.Append("," + dtRoute.Rows[intCount]["CWt"].ToString() + ",'" + dtRoute.Rows[intCount]["IsPrime"].ToString() + "','" + dtRoute.Rows[intCount]["IsHeavy"].ToString() + "'," + dtRoute.Rows[intCount]["Freight"].ToString());
                                strRateDetails.Append("," + dtRoute.Rows[intCount]["FreightTax"].ToString() + "," + dtRoute.Rows[intCount]["RatePerKg"].ToString());
                                strRateDetails.Append("," + dtRoute.Rows[intCount]["Cost"].ToString() + "," + dtRoute.Rows[intCount]["Proration"].ToString());
                                strRateDetails.Append("," + dtRoute.Rows[intCount]["AgentComm"].ToString() + "," + dtRoute.Rows[intCount]["Discount"].ToString());
                                strRateDetails.Append("," + dtRoute.Rows[intCount]["MKTFrt"].ToString() + "," + dtRoute.Rows[intCount]["MKTTax"].ToString());
                                strRateDetails.Append(",'" + dtRoute.Rows[intCount]["CostType"].ToString() + "','" + dtRoute.Rows[intCount]["Currency"].ToString() + "'); ");
                            }

                            objBLL.SaveAWBRateDetails(HidAWBNumber.Value.Trim(), Session["UserName"].ToString(), Convert.ToDateTime(Session["IT"]), strRateDetails.ToString(), txtAwbPrefix.Text);
                            strRateDetails = null;
                            if (dtRoute != null)
                            {
                                dtRoute.Dispose();
                            }
                            else
                            {
                                dtRoute = null;
                            }
                        }
                    }
                }
                catch (Exception ex) { }
                #endregion "AWB Rate Details"
                //TaxCode ,TaxType ,IATATax ,MKtTax ,DiscountTax ,ComissionTax ,SpotTax ,OCTax ,OATax ,TaxPercent ,TotalTax ,IndividualAdd
                #region "AWB Tax Details"
                try
                {
                    if (Session["TaxDetails"] != null)
                    {
                        DataTable dtRoute = (DataTable)Session["TaxDetails"];

                        if (dtRoute != null && dtRoute.Rows.Count > 0)
                        {
                            System.Text.StringBuilder strTaxDetails = new System.Text.StringBuilder();

                            for (int intCount = 0; intCount < dtRoute.Rows.Count; intCount++)
                            {
                                strTaxDetails.Append("Insert into #tblTax (TaxCode ,TaxType ,IATATax ,MKtTax ,DiscountTax ,ComissionTax ,SpotTax ,OCTax ,OATax ,TaxPercent ,TotalTax ,IndividualAdd,TaxSrNo) values (");
                                strTaxDetails.Append("'" + dtRoute.Rows[intCount]["TaxCode"].ToString() + "','" + dtRoute.Rows[intCount]["TaxType"].ToString() + "'," + dtRoute.Rows[intCount]["IATATax"].ToString());
                                strTaxDetails.Append("," + dtRoute.Rows[intCount]["MKtTax"].ToString() + "," + dtRoute.Rows[intCount]["DiscTax"].ToString() + "," + dtRoute.Rows[intCount]["CommTax"].ToString());
                                strTaxDetails.Append("," + dtRoute.Rows[intCount]["SpotTax"].ToString() + "," + dtRoute.Rows[intCount]["OCTax"].ToString() + "," + dtRoute.Rows[intCount]["OATax"].ToString());
                                strTaxDetails.Append("," + dtRoute.Rows[intCount]["TaxPercent"].ToString() + "," + dtRoute.Rows[intCount]["Total"].ToString());
                                strTaxDetails.Append("," + dtRoute.Rows[intCount]["AddInTotal"].ToString());
                                strTaxDetails.Append("," + dtRoute.Rows[intCount]["SerialNumber"].ToString() + ");");
                            }

                            objBLL.SaveAWBTaxDetails(HidAWBNumber.Value.Trim(), Session["UserName"].ToString(), Convert.ToDateTime(Session["IT"]), strTaxDetails.ToString(), txtAwbPrefix.Text);
                            strTaxDetails = null;
                            if (dtRoute != null)
                            {
                                dtRoute.Dispose();
                            }
                            else
                            {
                                dtRoute = null;
                            }
                        }
                    }
                }
                catch (Exception ex) { }
                #endregion

                //Code Commented by Deepak as the Stored procedure is not present in the DB itself
                //if (intAcceptedPcsforSummary > 0)
                //SaveAcceptanceSummary(intAcceptedPcsforSummary, dcAcceptedWtforSummary, strCommodityforSummary);

                //Insert details in AWB Piece Details table

                DataSet dtAWBPiece = (DataSet)Session["dsPiecesDet"];
                System.Text.StringBuilder strAWBDetailsv = new System.Text.StringBuilder();

                if (dtAWBPiece != null && dtAWBPiece.Tables.Count > 0 && dtAWBPiece.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dtAWBPiece.Tables[0].Rows.Count; intCount++)
                    {
                        strAWBDetailsv.Append("Insert into #AWBPieceDetails values ('");
                        strAWBDetailsv.Append(HidAWBNumber.Value);
                        strAWBDetailsv.Append("',");
                        strAWBDetailsv.Append(dtAWBPiece.Tables[0].Rows[intCount]["Pieces"].ToString());
                        strAWBDetailsv.Append(",");
                        strAWBDetailsv.Append(dtAWBPiece.Tables[0].Rows[intCount]["GrossWt"].ToString());
                        strAWBDetailsv.Append(",'");
                        strAWBDetailsv.Append(dtAWBPiece.Tables[0].Rows[intCount]["PieceId"].ToString());
                        strAWBDetailsv.Append("','");
                        strAWBDetailsv.Append(Session["UserName"].ToString());
                        strAWBDetailsv.Append("','");
                        strAWBDetailsv.Append(((DateTime)Session["IT"]).ToString("MM/dd/yyyy"));
                        strAWBDetailsv.Append("'); ");
                    }

                    object[] value = { HidAWBNumber.Value, strAWBDetailsv.ToString(), txtAwbPrefix.Text };

                    if (!objBLL.SaveAWBPiecedetails(value))
                    {
                        lblStatus.Text = "Save failed (AWB Pieces Details)";
                        return;
                    }
                    strAWBDetailsv = null;
                }
                try
                {
                    if (dtAWBPiece != null)
                        dtAWBPiece.Dispose();
                }
                catch (Exception ex)
                {
                    dtAWBPiece = null;
                }
                //End

                AWBMilestonesBAL objMilestone = new AWBMilestonesBAL();

                float wtaccepted = 0;
                int pcsaccepted = 0;
                string flightids = "";
                int AcpPcs = 0;

                foreach (GridViewRow rw in grdRouting.Rows)
                {
                    AcpPcs = int.Parse(((TextBox)rw.FindControl("txtAcceptedPcs")).Text.Trim() == "" ? "0" : ((TextBox)rw.FindControl("txtAcceptedPcs")).Text.Trim());

                    if (((CheckBox)rw.FindControl("chkAccepted")).Checked || AcpPcs > 0)
                    {
                        if (((TextBox)rw.FindControl("txtFltOrig")).Text.Trim() == ddlOrg.Text.Trim())
                        {
                            wtaccepted += float.Parse(((TextBox)rw.FindControl("txtAcceptedWt")).Text.Trim() == "" ? "0" : ((TextBox)rw.FindControl("txtAcceptedWt")).Text.Trim());
                            pcsaccepted += int.Parse(((TextBox)rw.FindControl("txtAcceptedPcs")).Text.Trim() == "" ? "0" : ((TextBox)rw.FindControl("txtAcceptedPcs")).Text.Trim());
                        }
                    }

                    if (((DropDownList)rw.FindControl("ddlPartner")).SelectedItem.Text.Trim().Equals("other", StringComparison.OrdinalIgnoreCase))
                    {
                        flightids += " " + ((TextBox)rw.FindControl("txtFlightID")).Text.Trim();
                    }
                    else
                    {
                        flightids += " " + ((DropDownList)rw.FindControl("ddlFltNum")).SelectedItem.Text.Trim();

                    }
                }

                #region 16-12-2013
                /*AWBMilestone tables
                 * objMilestone.AddAWBMilestone(new object[] { 
                    HidAWBNumber.Value.Trim(), 
                    int.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.Trim()), 
                    flightids.Trim(), 
                    "Booked" ,
                    ""+ float.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text.Trim()),
                    "",
                    txtAwbPrefix.Text
                });

                if (wtaccepted != 0)
                    objMilestone.AddAWBMilestone(new object[] { 
                    HidAWBNumber.Value.Trim(), 
                    pcsaccepted, 
                    flightids.Trim(), 
                    "Accepted" ,
                    ""+wtaccepted,
                    ""
                });
                */
                #endregion

                #region Check to combine Save & execute functionality
                try
                {
                    MasterBAL objBAL = new MasterBAL();
                    string strIsAccp = string.Empty;

                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        if (((CheckBox)grdRouting.Rows[intCount].FindControl("chkAccepted")).Checked)
                        {
                            strIsAccp = "Y";
                            break;
                        }
                    }

                    if (objBAL.CheckConfiguration("Booking", "CombineSave&Execute") && strIsAccp != "Y")
                    {
                        btnExecute_Click(null, null);
                    }
                }
                catch (Exception ex) { }
                #endregion

                //btnSearchAWB_Click(this, new EventArgs());
                lblStatus.Text = "AWB Details Saved Successfully ! (Showing in view mode)";
                Session["ButtonID"] = "btnSave";
                txtAWBNo.Text = HidAWBNumber.Value;
                lblStatus.ForeColor = Color.Green;
                //txtAWBNo.Enabled = false;
                txtAWBNo.ReadOnly = true;
                SetLabelValues(false);
                // btnShowEAWB.Enabled = true;
                //EnableDisable(false);
                EnableDisable("Save");
                //showEAWBData();
                //Function for creates auto messages for AWB creation updation
                #region Auto Message Creation
                bool GenerateAutoMSG = false;
                LoginBL objLoginBAL = new LoginBL();
                try
                {
                    //GenerateAutoMSG = Convert.ToBoolean(objLoginBAL.GetMasterConfiguration("GenerateAutoMSG"));
                    GenerateAutoMSG = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "GenerateAutoMSG"));
                }
                catch (Exception ex)
                { //default validate route in route grid
                    GenerateAutoMSG = true;
                }
                if (GenerateAutoMSG)
                {
                    AutoMessagingFunction();
                }
                #endregion
                //Written to load page again once created.
                Server.Transfer("~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim() + "&GHA=" + Convert.ToString(Session["GHABooking"]));
                //btnListAgentStock_Click(null, null);
                // For Showing HAWB As Pop Up
                btnHABDetails_Click(null, null);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error updating AWB Information: " + ex.Message;
            }
        }
        #endregion btnSave_Click

        #region Validate Data
        /// <summary>
        /// Validate data entered by user.
        /// </summary>
        /// <returns>Returns True if valid data is entered.</returns>
        private bool ValidateData()
        {
            try
            {
                if (chkInterline.Checked || Convert.ToBoolean(Session["GHABooking"]))
                {
                    if (txtAwbPrefix.Text.Length < 1 || txtAwbPrefix.Text == "")
                    {
                        lblStatus.Text = "Please enter valid AWB Prefix";
                        txtAwbPrefix.Focus();
                        return false;
                    }

                    DataSet dsPartner = (DataSet)Session["PartnerCode"];
                    int Count = 0;

                    if (dsPartner != null && dsPartner.Tables.Count > 0 && dsPartner.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsPartner.Tables[0].Rows.Count; i++)
                        {
                            if (txtAwbPrefix.Text.Trim() == dsPartner.Tables[0].Rows[i][0].ToString())
                            {
                                Count = Count + 1;
                                break;
                            }
                        }
                    }

                    dsPartner = null;

                    if (Count == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid AWB Prefix (Should match with Self / Partner code)";
                        txtAwbPrefix.Focus();
                        return false;
                    }
                }

                if (ddlAirlineCode.Text.Trim() == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Designator Code.";
                    txtAwbPrefix.Focus();
                    return false;
                }

                string AWBNumber = "", errormessage = "";

                if (Convert.ToBoolean(Session["GHABooking"]))
                {
                    if (txtAWBNo.Text.Length < 1 || txtAWBNo.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter valid AWB Number";
                        txtAWBNo.Focus();
                        return false;
                    }
                    else
                    {
                        if (HidAWBNumber.Value.Trim() == "")
                        {
                            if (objBLL.CheckAWBExists(txtAwbPrefix.Text, txtAWBNo.Text))
                            {
                                lblStatus.Text = "AWB Number already exists in the system. Please validate.";
                                txtAWBNo.Focus();
                                return false;
                            }
                            else
                            {
                                HidAWBNumber.Value = txtAWBNo.Text.ToString();
                            }
                        }
                    }
                }

                if (Convert.ToBoolean(Session["GHABooking"]) == false)
                {
                    if (HidIsManual.Value != "1")
                    {
                        if (HidAWBNumber.Value.Trim() == "")
                        {
                            if (txtAwbPrefix.Text.Equals(Session["awbPrefix"].ToString(), StringComparison.OrdinalIgnoreCase) || (chkInterline.Checked == false))
                            {
                                if (GetAutoAWBNumber(ref errormessage, ref AWBNumber, txtAwbPrefix.Text.Trim(), ddlDocType.Text.Trim()))
                                {
                                    HidAWBNumber.Value = AWBNumber;
                                }
                                else
                                {
                                    lblStatus.Text = "No AWB stock exists for the agent.";
                                    return false;
                                }

                            }
                            else
                            {
                                if (txtAWBNo.Text.Length < 1 || txtAWBNo.Text == "")
                                {
                                    lblStatus.Text = "Please enter valid AWB Number";
                                    txtAWBNo.Focus();
                                    return false;
                                }
                                else
                                {
                                    if (objBLL.CheckAWBExists(txtAwbPrefix.Text, txtAWBNo.Text))
                                    {
                                        lblStatus.Text = "AWB Number already exists in the system. Please validate.";
                                        txtAWBNo.Focus();
                                        return false;
                                    }
                                    else
                                    {
                                        HidAWBNumber.Value = txtAWBNo.Text.ToString();
                                    }
                                }

                            }
                        }
                    }
                    Int64 awb = Convert.ToInt64(HidAWBNumber.Value, 10);
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Please enter valid AWB Number";
                txtAWBNo.Focus();
                return false;
            }
            if (ddlDest.SelectedItem.Text == "Select")
            {
                lblStatus.Text = "Please select valid Destination Code";
                ddlDest.Focus();
                return false;
            }
            if (ddlOrg.SelectedValue == ddlDest.SelectedValue)
            {
                lblStatus.Text = "Origin and Destination Code can not be same.";
                ddlDest.Focus();
                return false;
            }

            if (hdnBookingType.Value == "N" && TXTAgentCode.Text == "")
            {
                lblStatus.Text = "Please select valid Agent Code";
                TXTAgentCode.Focus();
                return false;
            }

            if (TXTAgentCode.Text.Trim() != "")
            {
                BookingBAL objBAL = new BookingBAL();
                if (objBAL.ValidateAgentCode(TXTAgentCode.Text.Trim(), Convert.ToDateTime(Session["IT"]).Date) == false)
                {
                    lblStatus.Text = "Agent account expired / not valid agent.";
                    TXTAgentCode.Focus();
                    return false;
                }
            }

            if (txtShipmentDate.Text != "")
            {
                DateTime shipmentDate;
                if (!DateTime.TryParseExact(txtShipmentDate.Text + " " + ddlShipmentTime.SelectedValue,
                    "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out shipmentDate))
                {
                    lblStatus.Text = "Please select valid Shipment Date & Time";
                    txtShipmentDate.Focus();
                    return false;
                }
            }

            //Validate Known Shipper if configuration is set.
            string COMMOCODE = "";
            if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
            {
                COMMOCODE = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode")).Text.ToString();
            }
            else
            {
                COMMOCODE = ((TextBox)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1")).Text.ToString();
            }
            if (Session["ConBooking_ValidateKnownShipper"] != null && Session["ConBooking_ValidateKnownShipper"].ToString() != ""
                && Convert.ToBoolean(Session["ConBooking_ValidateKnownShipper"]) == true)
            {

                if (!objBLL.ValidateKnownShipper(ddlOrg.SelectedValue, ddlDest.SelectedValue, txtShipperCode.Text,
                    ddlProductType.SelectedValue, txtSpecialHandlingCode.Text,
                    COMMOCODE))
                {
                    lblStatus.Text = "Shipper entered is not a Known Shipper. Shipment can not be booked";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }
            //if(!ValidateMasterEntry(TXTAgentCode.Text.ToString(),"Agent"))
            //{
            //    lblStatus.Text = "Please Validate Agent Code against Master record";
            //    TXTAgentCode.Focus();
            //    return false;
            //}
            try
            {
                if (CHKConsole.Checked)
                {
                    DataTable dtHAWB = (DataTable)Session["HAWBDetails"];
                    if (dtHAWB == null)
                    {
                        lblStatus.Text = "Please enter HAWB details.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                    else if (dtHAWB != null)
                    {
                        if (dtHAWB.Rows.Count < 1)
                        {
                            lblStatus.Text = "Please enter HAWB details.";
                            lblStatus.ForeColor = Color.Red;
                            return false;
                        }
                        dtHAWB.Dispose();
                    }

                }
            }
            catch (Exception ex) { }

            #region Validate Commodity grid.
            if (grdMaterialDetails.Rows.Count < 1)
            {
                lblStatus.Text = "Please enter Material Details";
                return false;
            }
            string commodity = "";
            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
            {
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                {
                    commodity = ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Text.Trim();
                }
                else
                {
                    commodity = ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Text.Trim();
                }
                if (commodity == "" || commodity.Length < 1)
                {
                    lblStatus.Text = "Please enter commodity details.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (Convert.ToBoolean(Session["GHABooking"]) == false)
                {
                    if (!ValidateMasterEntry(commodity, "Commodity"))
                    {
                        lblStatus.Text = "Please Validate Commodity Code against Master record";
                        if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                        {
                            ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Focus();
                        }
                        else
                        {
                            ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Focus();
                        }
                        return false;
                    }
                }

                if (((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Text.Trim().Equals("Select", StringComparison.OrdinalIgnoreCase))
                {
                    lblStatus.Text = "Please Select Pay Mode.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                TextBox tempTextBox;
                //Validate code description
                tempTextBox = (TextBox)(grdMaterialDetails.Rows[i].FindControl("txtMaterialCommDesc"));
                try
                {
                    if (tempTextBox.Text == "")
                    {
                        lblStatus.Text = "Please enter valid Code description";
                        tempTextBox.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.Text = "Please enter valid Code description";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //Validate number of pieces
                tempTextBox = (TextBox)(grdMaterialDetails.Rows[i].FindControl("TXTPcs"));
                try
                {
                    if (int.Parse(tempTextBox.Text) <= 0)
                    {
                        lblStatus.Text = "Please enter valid Number of Pieces";
                        tempTextBox.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.Text = "Please enter valid Number of Pieces";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //Validate gross weight
                tempTextBox = (TextBox)(grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt"));
                try
                {
                    if (Convert.ToDouble(tempTextBox.Text) <= 0)
                    {
                        lblStatus.Text = "Please enter valid Gross Weight";
                        tempTextBox.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.Text = "Please enter valid Gross Weight";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //Validate volumetric weight
                tempTextBox = (TextBox)(grdMaterialDetails.Rows[i].FindControl("txtCommVolWt"));
                try
                {
                    Convert.ToDouble(tempTextBox.Text.Trim() == "" ? "0" : tempTextBox.Text.Trim());
                }
                catch (Exception)
                {
                    lblStatus.Text = "Please enter valid Volumetric Weight";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //Validate charged weight
                tempTextBox = (TextBox)(grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt"));
                try
                {
                    if (Convert.ToDouble(tempTextBox.Text) <= 0)
                    {
                        lblStatus.Text = "Please enter valid Charged Weight";
                        tempTextBox.Focus();
                        return false;
                    }
                }
                catch (Exception)
                {
                    lblStatus.Text = "Please enter valid Charged Weight";
                    tempTextBox.Focus();
                    return false;
                }
            }
            #endregion

            #region Validate routing information.
            if (grdRouting.Rows.Count < 1)
            {
                lblStatus.Text = "Please enter Flight Details";
                return false;
            }

            string[] arrayFlight = new string[grdRouting.Rows.Count];
            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {

                TextBox tempTextBox;
                //Validate flight origin
                tempTextBox = (TextBox)(grdRouting.Rows[i].FindControl("txtFltOrig"));
                if (tempTextBox.Text == "")
                {
                    lblStatus.Text = "Please enter valid Flight Origin";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //Validate flight destination
                tempTextBox = (TextBox)(grdRouting.Rows[i].FindControl("txtFltDest"));
                if (tempTextBox.Text == "")
                {
                    lblStatus.Text = "Please enter valid Flight Destination";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //------------------------------

                tempTextBox = null;
                //Validate flight date
                tempTextBox = (TextBox)(grdRouting.Rows[i].FindControl("txtFdate"));
                if (tempTextBox.Text == "")
                {
                    lblStatus.Text = "Please select valid Flight Date";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox.Dispose();

                string Flight = "";
                if (CommonUtility.FlightValidation)
                {
                    string POL = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.ToString();
                    string POU = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.ToString();
                    if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase))
                    {
                        Flight = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.ToString();
                    }
                    else
                    {
                        Flight = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.ToString();
                    }

                    string Date = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.ToString();

                    if (objBLL.CheckFlightDeparted(POL, POU, Flight, Date))
                    {
                        lblStatus.Text = "Flight No. " + Flight + " Dated " + Date + " already departed. Please select another flight";
                        tempTextBox.Focus();
                        return false;
                    }
                    if (Session["AWBStatus"].ToString() == "B")
                    {

                        if (((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked ||
                           (((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text.Trim() != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text.Trim() != "0") && Convert.ToDecimal(((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text.ToString()) > 0 ||
                           (((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Text.Trim() != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Text.Trim() != "0") && Convert.ToDecimal(((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Text.ToString()) > 0)
                        {
                            lblStatus.Text = "Accepted pcs/wt is not allowed to enter, as AWB status is not executed.";
                            lblStatus.ForeColor = Color.Red;
                            return false;
                        }

                    }
                }

                //Validate if same flight number and flight date is entered twice in Route Details
                Flight = "";
                if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase))
                {
                    Flight = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.ToString();
                }
                else
                {
                    Flight = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.ToString();
                }
                //Check if split shipment is allowed as per configuration
                //if (!Convert.ToBoolean(lBal.GetMasterConfiguration("AllowSplitShipment")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AllowSplitShipment")))
                {
                    int txtTotalPcs = Convert.ToInt32(((TextBox)grdMaterialDetails.Rows[0].FindControl("TXTPcs")).Text);
                    int txtFlightPcs = Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text);
                    if (txtFlightPcs != txtTotalPcs)
                    {
                        lblStatus.Text = "Shipment cannot be split!";
                        lblStatus.ForeColor = Color.Red;
                        return (false);
                    }
                }

                //Combine flight number and flight date in an array to validate split shipment on same flight.
                if (arrayFlight.Contains(Flight + ";" + ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.ToString()))
                {
                    lblStatus.Text = "Shipment cannot be split on same flight on same day.";
                    lblStatus.ForeColor = Color.Red;
                    return (false);
                }
                else
                {
                    arrayFlight[i] = Flight + ";" + ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.ToString();
                }

                //Check if blank flight is allowed during booking.
                if (Session["ConBooking_FlightNumMandatory"] != null && Session["ConBooking_FlightNumMandatory"].ToString() != ""
                    && Convert.ToBoolean(Session["ConBooking_FlightNumMandatory"].ToString()) == true)
                {

                    if (Flight.Trim() == "" || Flight.ToUpper() == "SELECT")
                    {
                        lblStatus.Text = "Please select a valid Flight #";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
            }
            #endregion

            try
            {
                DateTime.ParseExact(txtExecutionDate.Text, "dd/MM/yyyy", null);
            }
            catch (Exception)
            {
                lblStatus.Text = "Please select valid Execution Date";
                txtExecutionDate.Focus();
                return false;
            }
            if (txtExecutedBy.Text == "")
            {
                lblStatus.Text = "Please enter Executed By";
                txtExecutedBy.Focus();
                return false;
            }
            if (txtExecutedAt.Text == "")
            {
                lblStatus.Text = "Please enter Execution Location";
                txtExecutedBy.Focus();
                return false;
            }

            //Shipper consignee validation.
            if (CommonUtility.ShipperMandatoryDuring != null && CommonUtility.ShipperMandatoryDuring.ToUpper() == "BK")
            {
                if (TXTShipper.Text.Trim() == "" || TXTShipTelephone.Text.Trim() == "" ||
                    TXTShipAddress.Text.Trim() == "" || ddlShipCountry.Text.Trim() == "")
                {
                    lblStatus.Text = "Please enter Shipper details.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }
            if (CommonUtility.ConsigneeMandatoryDuring != null && CommonUtility.ConsigneeMandatoryDuring.ToUpper() == "BK")
            {
                if (TXTConsignee.Text.Trim() == "" || TXTConTelephone.Text.Trim() == "" ||
                    TXTConAddress.Text.Trim() == "" || ddlConCountry.Text.Trim() == "")
                {
                    lblStatus.Text = "Please enter Consignee details.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }

            for (int ICount = 0; ICount < grdRouting.Rows.Count; ICount++)
            {
                if (CommonUtility.FlightValidation)
                {
                    try
                    {
                        if (!((DropDownList)grdRouting.Rows[ICount].FindControl("ddlPartner")).Text.Trim().Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            if (((DropDownList)grdRouting.Rows[ICount].FindControl("ddlFltNum")).Text.Trim() == "Select")
                            {
                                lblStatus.Text = "Please enter flight no in route details.";
                                lblStatus.ForeColor = Color.Red;
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Exception in flight no in route details.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }

                }

                if (((TextBox)grdRouting.Rows[ICount].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[ICount].FindControl("txtFltDest")).Text.Trim())
                {
                    lblStatus.Text = "Flight origin and destination can't be same.";
                    lblStatus.ForeColor = Color.Red;
                    return false;

                }
                /*Remove for Compulsary documents
                 * try 
                {
                    if (((CheckBox)grdRouting.Rows[ICount].FindControl("chkAccepted")).Checked)
                    {
                        if (txtAttchDoc.Text.Length < 1)
                        {
                            lblStatus.Text = "Please select Attched Documents with Consignment.";
                            lblStatus.ForeColor = Color.Red;
                            return false;
                        }

                    }
                }
                catch (Exception ex) { }*/
            }

            string strResult = ValidateULDFlow();

            if (strResult != "")
            {
                lblStatus.Text = strResult;
                lblStatus.ForeColor = Color.Red;
                return false;
            }

            return true;
        }

        public bool ValidateDimensionWtPerPiece()
        {
            DataSet dsResult = new DataSet();

            try
            {
                foreach (GridViewRow row in grdRouting.Rows)
                {
                    int scheduleid = 0;
                    try
                    {
                        scheduleid = int.Parse(((HiddenField)row.FindControl("HidScheduleID")).Value);
                    }
                    catch (Exception ex)
                    {
                        return true;
                    }

                    int wt = int.Parse(((TextBox)row.FindControl("txtWt")).Text);
                    int pccount = int.Parse(((TextBox)row.FindControl("txtPcs")).Text);


                    SQLServer da = new SQLServer(Global.GetConnectionString());
                    dsResult = da.SelectRecords("SP_GetRestrictionsForFlight", "scheduleid", scheduleid, SqlDbType.Int);

                    if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                    {
                        lblStatus.Text = "Error: (ValidateDimensionWtPerPiece) CODE-I";
                        return false;
                    }

                    float Weight = float.Parse(dsResult.Tables[0].Rows[0]["Weight"].ToString());
                    int Length = int.Parse(dsResult.Tables[0].Rows[0]["Length"].ToString());
                    int Breadth = int.Parse(dsResult.Tables[0].Rows[0]["Breadth"].ToString());
                    int Height = int.Parse(dsResult.Tables[0].Rows[0]["Height"].ToString());

                    DataSet dsDimension = (DataSet)Session["dsDimesionAll"];

                    float unitconversion = 1;
                    if (dsDimension != null)
                    {
                        foreach (DataRow drDimension in dsDimension.Tables[0].Rows)
                        {
                            if (drDimension["MeasureUnit"].ToString() == "inches")
                            {
                                unitconversion = float.Parse("2.5");
                            }

                            if (((int.Parse(drDimension["Length"].ToString()) * unitconversion) > Length))
                            {
                                lblStatus.Text = "Error :Length of piece is greater than allowed length";
                                return false;
                            }
                            if (((int.Parse(drDimension["Breadth"].ToString()) * unitconversion) > Breadth))
                            {
                                lblStatus.Text = "Error :Breadth of piece is greater than allowed breadth";
                                return false;
                            }
                            if (((int.Parse(drDimension["Height"].ToString()) * unitconversion) > Height))
                            {
                                lblStatus.Text = "Error :Height of piece is greater than allowed height";
                                return false;
                            }
                            if ((wt / pccount) > Weight)
                            {
                                lblStatus.Text = "Error :Weight of piece is greater than allowed weight";
                                return false;
                            }

                        }

                        if (dsDimension != null)
                            dsDimension.Dispose();

                    }
                    else
                    {

                        dsDimension = null;
                    }

                    // "Length" "Breadth" "Height" "PcsCount" "RowIndex" "MeasureUnit"

                }

                return true;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :(ValidateDimensionWtPerPiece)" + ex.Message;
                dsResult = null;
                return false;
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        public bool GetAutoAWBNumber(ref string errormessage, ref string AWBNumber, string AWBPrefix, string CnoteType)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = null;

            try
            {
                #region Stock Alert
                DataSet ds = null;
                try
                {
                    ds = objBLL.CheckStockThreshold(TXTAgentCode.Text.Trim());
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                if (dr[0].ToString().Length > 3)
                                {
                                    string strBody = "Please allocate Stock for agent" + dr[4].ToString() + ".\r\nStock Remaining" + dr[3].ToString() + "\r\nThank You.";
                                    //swapnil
                                    //  SendEmail("sg@qidtech.com", dr[0].ToString().Trim(','), "qidtech#1", "Stock depletion Reminder for" + dr[4].ToString(), strBody, false);
                                }
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[1].Rows[0];
                                if (dr[0].ToString().Length > 3)
                                {
                                    string strBody = dr[2].ToString();
                                    //"Please allocate Stock for agent" + dr[4].ToString() + ".\r\nStock Remaining" + dr[3].ToString() + "\r\nThank You.";
                                    //swapnil
                                    //SendEmail("sg@qidtech.com", dr[0].ToString().Trim(','), "qidtech#1", "Bank Guarantee Expiration Reminder for " + dr[1].ToString(), strBody, false);
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ds = null;
                }
                finally
                {
                    if (ds != null)
                        ds.Dispose();
                }
                #endregion

                string[] param = { "AgentCode", "Station", "AWBPrefix", "CNote" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                //object[] values = { ddlAgtCode.SelectedItem.Text.Trim(), Session["Station"].ToString() };
                object[] values = { TXTAgentCode.Text.Trim(), ddlOrg.Text.Trim(), AWBPrefix, CnoteType };


                dsResult = db.SelectRecords("SP_Booking_GetNextAWBForAgent", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            AWBNumber = dsResult.Tables[0].Rows[0][0].ToString();
                            if (dsResult.Tables[1].Rows.Count != 0)
                                txtAwbPrefix.Text = dsResult.Tables[1].Rows[0][0].ToString();

                            if (AWBNumber.Trim() == "0")
                            {
                                errormessage = "No AWBNumber available for agent.";
                                return false;
                            }

                            return true;
                        }
                        else
                        {
                            errormessage = "No AWBNumber available for agent.";
                            return false;
                        }
                    }
                    else
                    {
                        errormessage = "No AWBNumber available for agent.";
                        return false;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                db = null;
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        #endregion Validate Data

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtAgentName.Enabled = true;
            //txtAWBNo.Enabled = true;
            txtAWBNo.ReadOnly = false;
            ddlDest.Enabled = true;
            btnProcess.Enabled = true;
            txtShipmentDate.Enabled = true;
            ddlShipmentTime.Enabled = true;
            ddlProductType.Enabled = true;
            txtSLAC.Enabled = true;
            ddlProductType.Enabled = true;
            imgProductType.Enabled = true;

            //ddlAgtCode.Enabled = true;
            TXTAgentCode.Enabled = true;
            txtExecutionDate.Enabled = true;
            btnExecutionDate.Enabled = true;
            txtShipmentDate.Enabled = true;
            ddlShipmentTime.Enabled = true;
            txtExecutedAt.Enabled = true;
            txtExecutedBy.Enabled = true;
            //ddlAgtCode.Enabled = true;
            btnSave.Visible = true;
            grdMaterialDetails.Enabled = true;
            grdRouting.Enabled = true;
            Session["BookingID"] = "0";
            //ViewState["BookingID"] = "0";
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.Red;
            //            txtAWBNo.Enabled = true;
            txtAWBNo.ReadOnly = false;
            if (Session["Station"] == null || Session["Station"].ToString() == "")
            {
                ddlOrg.SelectedIndex = 0;
                ddlOrg.Enabled = true;
            }
            ddlDest.Text = "Select";

            LoadGridMaterialDetail();
            LoadGridRoutingDetail();
            LoadGridRateDetail();
            LoadCommodityDropdown();

            txtHandling.Text = "";
            txtAWBNo.Text = "";
            HidAWBNumber.Value = "";
            TXTShipper.Text = "";
            TXTShipTelephone.Text = "";
            TXTShipAddress.Text = "";
            TXTConAddress.Text = "";
            TXTConsignee.Text = "";
            TXTConTelephone.Text = "";


            txtExecutionDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
            txtShipmentDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");// dtCurrentDate.AddHours(4).ToString("dd/MM/yyyy");
            ddlShipmentTime.SelectedValue = dtCurrentDate.ToString("HH:00");
            txtExecutedBy.Text = Session["UserName"].ToString();
            txtExecutedAt.Text = Session["Station"].ToString();
            SetLabelValues(true);
            ddlProductType.Enabled = true;
            Session["dsDimesionAll"] = null;
        }
        #endregion btnClear_Click

        #region btnSendFFR_Click
        //Modified on 4/9/2013
        protected void btnSendFFR_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                string FlightNumber = "";
                string PartnerCode = "";
                string AWBNumber = "", error = "";

                lblMsg.Text = "FFR";
                try
                {
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        FlightNumber = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                        PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                    }
                }
                catch (Exception ex) { }
                try
                {
                    BALEmailID ObjEmail = new BALEmailID();
                    ds = ObjEmail.GetEmail(ddlOrg.SelectedItem.Text, ddlDest.SelectedItem.Text, "FFR", FlightNumber, PartnerCode);
                    if (ds != null)
                    {
                        txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                        lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                        if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                        {
                            GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                        }
                    }

                }
                catch (Exception ex) { }

                if (HidAWBNumber.Value.ToString().Trim() != "")
                {
                    AWBNumber = HidAWBNumber.Value.ToString().Trim();
                }
                else if (txtAWBNo.Text.Trim() == "")
                {
                    AWBNumber = txtAWBNo.Text.Trim();
                }
                else
                {
                    lblStatus.Text = "Please Provide AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (AWBNumber.Length > 0)
                {
                    if (objBLL.GetAWBDetails(AWBNumber, txtAwbPrefix.Text.ToString(), ref ds, ref error))
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string Msg = cls_BL.EncodeFFR(ds, ref error);
                                    if (Msg.Length > 3)
                                    {
                                        txtMessageBody.Text = Msg;
                                        Session["Message"] = "FFR";
                                    }
                                    else if (error.Length > 0)
                                    {
                                        lblStatus.Text = "IATA FFR Message Error:" + error;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error on FFR Click" + ex.Message;
                ds = null;
                return;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);


        }
        #endregion

        public DataSet GetChargeSummury()
        {
            DataSet dsResult = new DataSet();
            try
            {
                // Result

                dsResult.Tables.Add();
                dsResult.Tables[0].Columns.Add("FrIATA");
                dsResult.Tables[0].Columns.Add("FrMKT");
                dsResult.Tables[0].Columns.Add("OCDC");
                dsResult.Tables[0].Columns.Add("OCDA");
                dsResult.Tables[0].Columns.Add("ServTax");
                dsResult.Tables[0].Columns.Add("Total");

                dsResult.Tables.Add();
                dsResult.Tables[1].Columns.Add("ChargeHead");
                dsResult.Tables[1].Columns.Add("Charge");


                DataTable dtRates = ((DataTable)Session["dtRates"]);
                DataSet dsDetails = (DataSet)Session["OCDetails"];

                // Rates
                decimal frtiata, frtmkt, ocdc, ocda, tax, alltotal;
                frtiata = frtmkt = ocdc = ocda = tax = alltotal = 0;

                foreach (DataRow rw in dtRates.Rows)
                {
                    frtiata += decimal.Parse(rw["FrIATA"].ToString());
                    frtmkt += decimal.Parse(rw["FrMKT"].ToString());
                    ocda += decimal.Parse(rw["OcDueAgent"].ToString());
                    ocdc += decimal.Parse(rw["OcDueCar"].ToString());
                    tax += decimal.Parse(rw["ServTax"].ToString());
                    alltotal += decimal.Parse(rw["Total"].ToString());
                }



                DataRow dsResultRow = dsResult.Tables[0].NewRow();

                dsResultRow["FrIATA"] = frtiata;
                dsResultRow["FrMKT"] = frtmkt;
                dsResultRow["OCDC"] = ocdc;
                dsResultRow["OCDA"] = ocda;
                dsResultRow["ServTax"] = tax;
                dsResultRow["Total"] = alltotal;

                dsResult.Tables[0].Rows.Add(dsResultRow);




                ArrayList ChargeHeads = new ArrayList();

                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    if (!ChargeHeads.Contains(row["Charge Head Code"].ToString()))
                        ChargeHeads.Add(row["Charge Head Code"].ToString());
                }

                for (int i = 0; i < ChargeHeads.Count; i++)
                {
                    decimal total = 0;
                    foreach (DataRow rw in dsDetails.Tables[0].Rows)
                    {
                        if (rw["Charge Head Code"].ToString() == ChargeHeads[i].ToString())
                        {
                            total += decimal.Parse(rw["Charge"].ToString());
                        }
                    }

                    DataRow newrow = dsResult.Tables[1].NewRow();
                    newrow["ChargeHead"] = ChargeHeads[i].ToString();
                    newrow["Charge"] = "" + total;

                    dsResult.Tables[1].Rows.Add(newrow);
                }

                return dsResult;
            }
            catch (Exception ex)
            {
                dsResult = null;
                return null;
            }
        }

        public string GetFlightNumber()
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = null;
            try
            {
                string[] param = { "source", "dest" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { ddlOrg.Text, ddlDest.Text };

                dsResult = db.SelectRecords("SP_GetFlightFromSourceAndDest", param, values, dbtypes);

                if (dsResult != null)
                {

                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            // done

                            return dsResult.Tables[0].Rows[0]["FlightNo"].ToString();
                        }
                        else
                            lblStatus.Text = "Error :Code-O09";
                    }
                    else
                        lblStatus.Text = "Error :Code-O08";
                }
                else
                    lblStatus.Text = "Error :Code-O07";


                return "";

            }
            catch (Exception ex)
            {
                dsResult = null;
                return "";
            }
            finally
            {
                db = null;
                if (dsResult != null)
                    dsResult.Dispose();
            }

        }

        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (ddlOrg.Text.Trim() == ddlDest.Text.Trim())
                {
                    ddlDest.SelectedValue = "Select";
                }

                BookingBAL.OrgStation = ddlOrg.SelectedItem.Text.Trim();
                Session["Origin"] = ddlOrg.SelectedItem.Text.Trim();
                Session["Destination"] = ddlDest.SelectedItem.Text.Trim();

                //Added by Vishal - 04 MAY 2014 ******************** 
                //LoadCurrencyType();
                LoadCurrencyWithUOM();
                //******************** Added by Vishal - 04 MAY 2014

                HidProcessFlag.Value = "1";
                FillHandlerCodes();
                TXTAgentCode.Focus();
                bool checkbeforereopen = true;
                checkbeforereopen = CheckIfAWBUpdateAllowedForRole();
                if (txtAWBNo.Text.Length > 0)
                {
                    //******************** Added by Jayant - 13 MAY 2014
                    if ((checkbeforereopen) && objBLL.CheckAWBExists(txtAwbPrefix.Text, txtAWBNo.Text))
                    {
                        //******************** Added by Jayant - 13 MAY 2014
                        lblStatus.Text = "AWB Number already exists in the system. Please validate.";
                        txtAWBNo.Focus();
                    }
                    else
                    {
                        HidAWBNumber.Value = txtAWBNo.Text.ToString();
                    }
                }
                LoadMatchingProductTypes();
                ddlOrg.Enabled = false;

            }
            catch (Exception ex)
            {

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage)
        {
            DataSet dsResult = new DataSet();

            if (strdate.Trim() == "")
            {
                if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, ""))
                {
                    FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                    return dsResult;
                }
                else
                {
                    return null;
                }
            }
            else
            {

                string[] splitdate = strdate.Split(new char[] { '/' });
                int year = int.Parse(splitdate[2]);
                int month = int.Parse(splitdate[1]);
                int day = int.Parse(splitdate[0]);
                DateTime dt = new DateTime(year, month, day);

                int diff = (dt - dtCurrentDate.Date).Days;

                if (new ShowFlightsBAL().GetFlightList(Origin, Dest, diff, ref dsResult, ref errormessage, dtCurrentDate.Date))
                {
                    FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                    return dsResult;
                }
                else
                {
                    return null;
                }



            }

        }

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage, string PartnerCode)
        {
            DataSet dsResult = new DataSet();
            if (strdate.Trim() == "")
            {
                if (PartnerCode == Convert.ToString(Session["AirlinePrefix"]))
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, ""))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {

                string[] splitdate = strdate.Split(new char[] { '/' });
                int year = int.Parse(splitdate[2]);
                int month = int.Parse(splitdate[1]);
                int day = int.Parse(splitdate[0]);
                DateTime dt = new DateTime(year, month, day);

                int diff = (dt - dtCurrentDate.Date).Days;

                if (PartnerCode == Convert.ToString(Session["AirlinePrefix"]))
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dt, ""))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        public void FormatRecords(string org, string dest, ref DataSet dsResult, int PrevHr, int PrevMin, int AllowedHr)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = dsResult.Clone();
            bool blOrignFlound, blDestFound;
            blOrignFlound = blDestFound = false;

            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                if (ScheduleID == "")
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                    }

                    ScheduleID = row["ScheduleID"].ToString();
                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);

                }
                else if (ScheduleID.Trim() == row["ScheduleID"].ToString())
                {
                    if (!blDestFound)
                    {
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["FltDestination"] = row["FltDestination"].ToString();
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["ArrTime"] = row["ArrTime"].ToString();

                        if (row["FltDestination"].ToString() == dest)
                        {
                            blDestFound = true;
                        }

                    }

                }
                else
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                        blDestFound = false;
                    }

                    ScheduleID = row["ScheduleID"].ToString();


                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);
                }

                i++;

            }

            dsResult = dsNewResult.Copy();
            DataView dv = new DataView(dsResult.Tables[0].Copy());
            dv.Sort = "DeptTime";

            dsResult = new DataSet();
            dsResult.Tables.Add(dv.ToTable().Copy());


            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


            DataTable dt = dsResult.Tables[0].Clone();
            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                string depttime = row["DeptTime"].ToString();
                int hr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                int min = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));

                string[] strDate = row["FltDate"].ToString().Split('/');
                int intFltDate = int.Parse(strDate[0]);
                int intCurrentDt = dtCurrentDate.Day;

                bool canAdd = true;
                //if (hr < (PrevHr + AllowedHr) && intFltDate == intCurrentDt)
                //{
                //    if (hr > PrevHr)
                //    {
                //        int hrdiff = hr - PrevHr;

                //        if (((hrdiff * 60) - PrevMin + min) > (AllowedHr * 60))
                //            canAdd = true;
                //    }
                //}
                //else
                //    canAdd = true;


                if (canAdd)
                {
                    DataRow rw = dt.NewRow();

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        rw[k] = row[k];
                    }

                    dt.Rows.Add(rw);
                }
            }

            dsResult = new DataSet();
            dsResult.Tables.Add(dt);

            try
            {
                if (dsNewResult != null)
                    dsNewResult.Dispose();
                if (dt != null)
                    dt.Dispose();
            }
            catch (Exception ex)
            {
                dt = null;
                dsNewResult = null;
            }
        }

        protected void btnShowEAWB_Click(object sender, EventArgs e)
        {
            //DataSet ds = new DataSet();
            //SqlDataAdapter dad = null;
            try
            {

                bool IsAgreed = false;
                string strAgentPreference = string.Empty;

                //string con = Global.GetConnectionString();
                //// SqlConnection con = new SqlConnection("connection string");
                //dad = new SqlDataAdapter("SELECT isnull(RatePreference,'IATA') RatePreference from dbo.AgentMaster where AgentCode ='" + TXTAgentCode.Text.Trim() + "'", con);

                //dad.Fill(ds);

                //if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                //{
                //    strAgentPreference = ds.Tables[0].Rows[0]["RatePreference"].ToString();
                //}
                //else
                //    strAgentPreference = "IATA";
                //if (ds != null)
                //{
                //    ds.Dispose();
                //}
                //else
                //{
                //    ds = null;
                //}
                //dad = null;

                strAgentPreference = objBLL.GeteAWBPrintPrefence(TXTAgentCode.Text.Trim(), txtAWBNo.Text, txtAwbPrefix.Text);

                if (strAgentPreference.Length < 1 || strAgentPreference == "")
                    strAgentPreference = "IATA";


                if (radMarket.Checked)
                {
                    strAgentPreference = "MKT";
                }

                if (radSpot.Checked)
                {
                    strAgentPreference = "Spot";
                }


                if (strAgentPreference == "As Agreed")
                    CHKAsAggred.Checked = true;
                else
                    CHKAsAggred.Checked = false;

                IsAgreed = CHKAsAggred.Checked;
                if (radAsAgree.Checked)
                {
                    IsAgreed = true;
                }
                System.Drawing.Image myimg = Code128Rendering.MakeBarcodeImage(txtAwbPrefix.Text.Trim() + txtAWBNo.Text.Trim(), 3, true);
                MemoryStream ms = new MemoryStream();
                myimg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                //img for report
                System.IO.MemoryStream Logo = null;
                try
                {

                    Logo = CommonUtility.GetImageStream(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }
                //end

                Session["DTExport"] = "";
                string DocType = ddlDocType.SelectedItem.Text;
                string AWBprefix = txtAwbPrefix.Text.Trim().ToString() + "|" + ddlOrg.SelectedItem.Text.ToString() + "|" + HidAWBNumber.Value.Trim().ToString();
                string AirlinePrefix = ddlAirlineCode.Text.Trim();//txtAwbPrefix.Text.Trim().ToString();
                string AWBno = txtAwbPrefix.Text.Trim().ToString() + "-" + HidAWBNumber.Value.Trim().ToString();
                string AirLineCode = ddlAirlineCode.Text.Trim();
                string Origin = ddlOrg.SelectedItem.Text.Trim();
                string Dest = ddlDest.SelectedItem.Text.Trim();
                //string AgentCode = ddlAgtCode.SelectedItem.Text;
                string AgentCode = TXTAgentCode.Text;
                string AgentName = txtAgentName.Text.ToString();
                string Serviceclass = ddlServiceclass.SelectedItem.Text;
                string Handlinginfo = txtHandling.Text.Trim();
                string AccountInfo = ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtAccountInfo")).Text;

                string SHCDesc = string.Empty;

                //bool SCHDesc = Convert.ToBoolean(lBal.GetMasterConfiguration("eAWBSHCDesc"));
                bool SCHDesc = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "eAWBSHCDesc"));
                if (SCHDesc)
                {
                    if (txtSpecialHandlingCode.Text.Trim() != "")
                        SHCDesc = new BookingBAL().GetSHCCodesandDesc(txtSpecialHandlingCode.Text.Trim());
                }
                else { SHCDesc = txtSpecialHandlingCode.Text.Trim(); }

                if (Handlinginfo != "")
                    Handlinginfo = Handlinginfo + " | " + SHCDesc;
                else
                    Handlinginfo = SHCDesc;

                string CommCode = "";
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                {
                    CommCode = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode")).Text;
                }
                else
                {
                    CommCode = ((TextBox)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1")).Text;
                }
                string CommDesc = ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtMaterialCommDesc")).Text;
                int Pcs = 0;
                //if ((((Label)grdMaterialDetails.Rows[0].FindControl("LBLTotalPcs")).Text) != null)
                //{
                int count = grdMaterialDetails.Rows.Count;
                try
                {
                    Pcs = int.Parse((((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text));
                    //    }
                }
                catch (Exception ex)
                {
                }
                double GrossWgt = Convert.ToDouble(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text);//Convert.ToDouble(((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommGrossWt")).Text);

                double Volume = 0;// Convert.ToDouble(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text.Trim());// == "" ? "0" : ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommVolWt")).Text.Trim());
                if (((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text.Trim() != "")
                {
                    Volume = Convert.ToDouble(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text.Trim());
                }
                double ChargeWgt = Convert.ToDouble(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text);


                ///function added for total of iata mkt rate on 6 may 12
                DataSet dsResult = GetChargeSummury();
                double frateIATA = 0.0;
                double frateMKT = 0.0;
                double frateSpot = 0.0, SpotRateKg = 0.0;
                double ValCharge = 0.0;
                string PayMode = "";

                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    frateIATA = Convert.ToDouble(dsResult.Tables[0].Rows[0][0].ToString()); //Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTFrIATA")).Text);
                    frateMKT = Convert.ToDouble(dsResult.Tables[0].Rows[0][1].ToString());//Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTFrMKT")).Text);
                    frateSpot = (((TextBox)GRDRates.Rows[0].FindControl("TXTSpotFreight")).Text.Trim() == "" ? 0 : Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTSpotFreight")).Text.Trim()));
                    SpotRateKg = (((TextBox)GRDRates.Rows[0].FindControl("TXTSpotRate")).Text.Trim() == "" ? 0 : Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTSpotRate")).Text.Trim()));
                    ValCharge = 0; //Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTValCharg")).Text);
                    PayMode = (((DropDownList)GRDRates.Rows[0].FindControl("ddlPayMode")).Text);
                }
                string OCDueCar = "";
                string OCDueAgent = "";


                OCDueCar = dsResult.Tables[0].Rows[0][2].ToString();
                OCDueAgent = dsResult.Tables[0].Rows[0][3].ToString();

                //decimal.Round(Convert.ToInt64(OCDueCar), 2, MidpointRounding.AwayFromZero);
                //decimal.Round(Convert.ToInt64(OCDueAgent), 2, MidpointRounding.AwayFromZero);

                //if ((((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueCar")).Text) != null)
                //{
                //    OCDueCar = (((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueCar")).Text);
                //}
                //if ((((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueAgent")).Text) != null)
                //{
                //    OCDueAgent = (((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueAgent")).Text);
                //}

                double SpotRate = SpotRateKg;// Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTSpotRate")).Text);
                double DynaRate = Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTDynRate")).Text.Trim() == "" ? "0" : ((TextBox)GRDRates.Rows[0].FindControl("TXTDynRate")).Text.Trim());
                double SerTax = Convert.ToDouble(dsResult.Tables[0].Rows[0][4].ToString());// Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTServiceTax")).Text);
                double Total = Convert.ToDouble(dsResult.Tables[0].Rows[0][5].ToString());//Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTTotal")).Text);

                Math.Round(Total, 2);
                Math.Round((decimal)Total, 2);

                Math.Round(SpotRate, 2);
                Math.Round((decimal)SpotRate, 2);

                Math.Round(DynaRate, 2);
                Math.Round((decimal)DynaRate, 2);

                Math.Round(SerTax, 2);
                Math.Round((decimal)SerTax, 2);


                string FltOrg = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text;
                string FltDest = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text;
                //string FltNo = (((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).Text);

                #region flt no as per configuration
                string FltNo = "";
                string FltDate = "";
                LoginBL lbal = new LoginBL();
                bool fltresult = true;
                try
                {
                    //fltresult = Convert.ToBoolean(lbal.GetMasterConfiguration("FlightDescInEAWBPrint"));
                    fltresult = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "FlightDescInEAWBPrint"));
                }
                catch (Exception ex)
                {
                    fltresult = true;
                }
                try
                {
                    if (fltresult)
                    {
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            FltNo = FltNo + ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text + ",";
                            //FltNo = FltNo + ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Value + ",";

                        }
                        if (FltNo != "")
                        {
                            FltNo = FltNo.Remove(FltNo.Length - 1, 1);
                            //FltNo = FltNo.Remove(FltNo.Length - 1, 1);

                        }
                        FltDate = (((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text);
                    }
                    else
                    {

                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            FltNo = string.Empty;
                            //FltNo = FltNo + ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Value + ",";
                        }
                        if (FltNo != "")
                        {
                            FltNo = string.Empty;
                            //FltNo = FltNo.Remove(FltNo.Length - 1, 1);
                        }
                        FltDate = string.Empty;
                    }

                }
                catch (Exception ex)
                { }
                #endregion


                //string FltNo = "";
                //for (int i = 0; i < grdRouting.Rows.Count; i++)
                //{
                //    FltNo = FltNo + ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text + ",";
                //}
                //if (FltNo != "")
                //{
                //    FltNo = FltNo.Remove(FltNo.Length - 1, 1);
                //}
                //HiddenField Flt = ((HiddenField)(grdRouting.Rows[0].FindControl("hdnFltNum")));
                //string FltNo = (((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).Text); // Flt.Value;
                //string FltDate = (((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text);
                bool FFRChecked = (((CheckBox)grdRouting.Rows[0].FindControl("chkFFR")).Checked);

                DataTable DTExportSubDetails = new DataTable();

                DTExportSubDetails.Columns.Add("OtherCharges");
                DTExportSubDetails.Columns.Add("Amount");
                DTExportSubDetails.Columns.Add("Type");


                string strOtherCharges = "";

                DataSet dsOtherDetails = (DataSet)Session["OCDetails"];

                try
                {
                    if (dsOtherDetails != null && dsOtherDetails.Tables.Count > 0 && dsOtherDetails.Tables[0].Rows.Count > 0)
                    {
                        //int Intcount = 0;
                        foreach (DataRow row in dsOtherDetails.Tables[0].Rows)
                        {
                            if (row["Charge Type"].ToString() == "DC" || row["Charge Type"].ToString() == "DA")
                            {
                                string strChargeType = string.Empty;
                                string strChargeCode = row["Charge Head Code"].ToString().Trim();

                                if (strChargeCode.Trim().IndexOf('/') > 0)
                                    strChargeCode = strChargeCode.Substring(0, strChargeCode.IndexOf("/"));
                                else
                                    strChargeCode = strChargeCode.Trim();

                                if (row["Charge Type"].ToString().Trim() == "DC")
                                    strChargeType = "Due Carrier";
                                else
                                    strChargeType = "Due Agent";

                                if (IsAgreed)
                                    DTExportSubDetails.Rows.Add(strChargeCode, "As agreed", strChargeType);
                                else
                                    DTExportSubDetails.Rows.Add(strChargeCode, row["Charge"].ToString(), strChargeType);

                                if (row[1].ToString().Substring(0, row[1].ToString().IndexOf('/')).ToUpper() == "VLC")
                                {
                                    ValCharge = Convert.ToDouble(row[3].ToString());
                                }
                                else
                                {
                                    strOtherCharges = strOtherCharges + row[1].ToString().Substring(0, row[1].ToString().IndexOf('/')) + ":" + row[3].ToString() + ", ";
                                    if (strOtherCharges.Contains("MOA:0,"))
                                    {
                                        strOtherCharges = strOtherCharges.Replace("MOA:0,", "");
                                    }
                                    if (strOtherCharges.Contains("MOC:0,"))
                                    {
                                        strOtherCharges = strOtherCharges.Replace("MOC:0,", "");
                                    }


                                }
                            }

                            if (row[0].ToString().Contains('/'))
                            {
                                strOtherCharges = strOtherCharges + row[0].ToString().Substring(0, row[0].ToString().IndexOf('/')) + ":" + row[1].ToString() + " , ";
                                DTExportSubDetails.Rows.Add(row[0].ToString().Substring(0, row[0].ToString().IndexOf('/')), row[1].ToString(), "Due Carriers");
                                //Intcount = Intcount + 1;
                                strOtherCharges = strOtherCharges + row[1].ToString() + ":" + row[3].ToString() + " , ";
                                if (strOtherCharges.Contains("MOA:0,"))
                                {
                                    strOtherCharges = strOtherCharges.Replace("MOA:0,", "");
                                }
                                if (strOtherCharges.Contains("MOC:0,"))
                                {
                                    strOtherCharges = strOtherCharges.Replace("MOC:0,", "");
                                }


                            }

                        }


                        //  DTExportSubDetails.Rows.Add();

                        //if (Intcount == 6)
                        //    break;
                        //}
                        //strOtherCharges = strOtherCharges.Remove(strOtherCharges.Length - 2, 1);

                    }
                    else
                    {
                        DTExportSubDetails.Rows.Add("-", "-", "-");
                    }
                }
                catch (Exception ex)
                {
                }



                string strDimension = "";// (((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text);


                string ExecDate = txtExecutionDate.Text;
                string ExecBy = txtExecutedBy.Text;
                string ExecAT = txtExecutedAt.Text;

                string Consigneename = TXTConsignee.Text.Trim() + " " + TXTConAddress.Text.Trim() + Environment.NewLine + " Phone No-" + TXTConTelephone.Text.Trim();
                string prepaid = "";
                string TotalPrepaid = "";
                string ShipperName = TXTShipper.Text.Trim() + "  " + TXTShipAddress.Text.Trim() + Environment.NewLine + " Phone No-" + TXTShipTelephone.Text.Trim();
                string RatePerKg = ((TextBox)GRDRates.Rows[0].FindControl("txtRatePerKg")).Text;
                string SCI = txtSCI.Text.Trim();
                DataTable DTExport = new DataTable();

                DTExport.Columns.Add("DocType");
                DTExport.Columns.Add("AWBPrefix");
                DTExport.Columns.Add("AWBNo");
                DTExport.Columns.Add("AirLineCode");
                DTExport.Columns.Add("Origin");
                DTExport.Columns.Add("Dest");
                DTExport.Columns.Add("AgentCode");
                DTExport.Columns.Add("AgentName");
                DTExport.Columns.Add("Serviceclass");
                DTExport.Columns.Add("HandlingInfo");
                DTExport.Columns.Add("CommCode");
                DTExport.Columns.Add("CommDesc");
                DTExport.Columns.Add("PCS");
                DTExport.Columns.Add("GrossWGT");
                DTExport.Columns.Add("Volume");
                DTExport.Columns.Add("ChargeWGT");


                DTExport.Columns.Add("frateIATA");
                DTExport.Columns.Add("frateMKT");
                DTExport.Columns.Add("ValCharge");
                DTExport.Columns.Add("PayMode");
                DTExport.Columns.Add("OCDueCar");
                DTExport.Columns.Add("OCDueAgent");
                DTExport.Columns.Add("SpotRate");
                DTExport.Columns.Add("DynaRate");
                DTExport.Columns.Add("SerTax");
                DTExport.Columns.Add("Total");

                DTExport.Columns.Add("FltOrg");
                DTExport.Columns.Add("FltDest");
                DTExport.Columns.Add("FltNo");
                DTExport.Columns.Add("FltDate");
                DTExport.Columns.Add("FFRChecked");
                DTExport.Columns.Add("ExecDate");
                DTExport.Columns.Add("ExecBy");
                DTExport.Columns.Add("ExecAT");

                DTExport.Columns.Add("ConsigneeName");
                DTExport.Columns.Add("Prepaid");
                DTExport.Columns.Add("TotalPrepaid");
                DTExport.Columns.Add("ShippersName");
                DTExport.Columns.Add("OtherCharges");
                DTExport.Columns.Add("Dimension");



                DTExport.Columns.Add("ShipperAccountNo");
                DTExport.Columns.Add("ConsigneeAcNo");
                DTExport.Columns.Add("IssuingCarrierName");
                DTExport.Columns.Add("AgentIataCode");
                DTExport.Columns.Add("AccountCode");
                DTExport.Columns.Add("AccountInformation");

                DTExport.Columns.Add("ChargesCode");
                DTExport.Columns.Add("WtVal");
                DTExport.Columns.Add("watvalother");
                DTExport.Columns.Add("DeclValCarr");
                DTExport.Columns.Add("DeclValcustoms");
                DTExport.Columns.Add("RateClassKG");

                DTExport.Columns.Add("RateClassN");
                DTExport.Columns.Add("CommodityItem");
                DTExport.Columns.Add("NatureOfgoods");
                DTExport.Columns.Add("Length");
                DTExport.Columns.Add("Width");
                DTExport.Columns.Add("Height");

                DTExport.Columns.Add("collectvalCharge");
                DTExport.Columns.Add("collecttax");
                DTExport.Columns.Add("collectDueAgent");
                DTExport.Columns.Add("CollectDueCarrier");
                DTExport.Columns.Add("collecttotal");
                DTExport.Columns.Add("CurrencyRate");

                DTExport.Columns.Add("CCDestCurrency");
                DTExport.Columns.Add("ForCarrOnlydest");
                DTExport.Columns.Add("chargeAtDest");
                DTExport.Columns.Add("AirlineAddress");
                DTExport.Columns.Add("AilinePrefix");
                DTExport.Columns.Add("RatePerKg");
                DTExport.Columns.Add("AccountInfo");
                DTExport.Columns.Add("DepartureCity");
                DTExport.Columns.Add("ArrivalCity");
                DTExport.Columns.Add("BarCode", System.Type.GetType("System.Byte[]"));
                DTExport.Columns.Add("CopyType");
                DTExport.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                DTExport.Columns.Add("CustomerSupportInfo");

                //New Columns 
                DTExport.Columns.Add("WTPPD");
                DTExport.Columns.Add("WTCOLL");
                DTExport.Columns.Add("OtherPPD");
                DTExport.Columns.Add("OtherCOLL");
                DTExport.Columns.Add("VLCCollect");
                DTExport.Columns.Add("PcsULDNo");

                //new coloumn added
                DTExport.Columns.Add("SCI");

                //for (int k = 0; k < 7; k++)
                //{



                // # show Voluem in EAWB

                DataSet dsDimesionAll = GenerateAWBDimensions("", Pcs, (DataSet)Session["dsDimesionAll"], Convert.ToDecimal(GrossWgt), false, "");

                //DataSet dsDimesionAll = GenerateAWBDimensions("", Pcs, (DataSet)Session["dsDimesionAll"], Convert.ToDecimal(GrossWgt), false,"",Convert.ToBoolean(Session["IsBulk"]));

                DataTable DTvolume = new DataTable();
                DTvolume.Columns.Add("CommDesc");
                DTvolume.Columns.Add("Length");
                DTvolume.Columns.Add("Width");
                DTvolume.Columns.Add("Height");
                DTvolume.Columns.Add("Volume");
                DTvolume.Columns.Add("PCSCount");

                float Length = 0; float Breadth = 0; float Height = 0;
                int dimPCS = 0;
                if (dsDimesionAll != null && dsDimesionAll.Tables.Count > 1 && dsDimesionAll.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDimesionAll.Tables[1].Rows.Count; i++)
                    {
                        dimPCS = 0;

                        Length = int.Parse(Convert.ToDecimal(dsDimesionAll.Tables[1].Rows[i]["Length"]).ToString("0"));
                        Breadth = int.Parse(Convert.ToDecimal(dsDimesionAll.Tables[1].Rows[i]["Breadth"]).ToString("0"));
                        Height = int.Parse(Convert.ToDecimal(dsDimesionAll.Tables[1].Rows[i]["Height"]).ToString("0"));
                        dimPCS = int.Parse(dsDimesionAll.Tables[1].Rows[i]["PcsCount"].ToString());

                        if (Length > 0 && Breadth > 0 && Height > 0)
                        {
                            Volume = (Length * Breadth * Height) * dimPCS;
                            DTvolume.Rows.Add(CommDesc, Length, Breadth, Height, Volume, dimPCS);
                            strDimension = strDimension + "  DIMS: " + Length + " * " + Breadth + " * " + Height + " * " + dimPCS + " ;    ";
                        }
                    }
                }
                else
                {
                    Length = 0;
                    Breadth = 0;
                    Height = 0;

                    DTvolume.Rows.Add(CommDesc, Length, Breadth, Height, Volume, dimPCS);
                }

                string ShipperAccountNo = "";
                string ConsigneeAcNo = "";
                string IssuingCarrierName = "";
                string AgentIataCode = "";
                string AccountCode = "";
                string AccountInformation = "";
                string ChargesCode = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                string AirlineAddress = "";
                string RateClause = ((TextBox)(GRDRates.Rows[0].FindControl("txtRateClass"))).Text.Trim();
                string OriginCity = "";
                string DestinationCity = "";
                string CopyType = string.Empty;
                string CustomerSupportInfo = "";

                string wtPPD = "", wtCOLL = "", OtherPPD = "", OtherCOLL = "";
                if (ChargesCode == "PP" || ChargesCode == "PX")
                    wtPPD = OtherPPD = "XX";
                if (ChargesCode == "CC")
                    wtCOLL = OtherCOLL = "XX";
                ArrayList arr1 = new ArrayList();
                if (Session["PieceTypeULDNo_ArrayList"] != null)
                    arr1 = (ArrayList)Session["PieceTypeULDNo_ArrayList"];


                string PscULDNo = "";
                if (arr1.Count > 0)
                {
                    foreach (string li in arr1)
                    {
                        PscULDNo += li.ToString() + ",";
                    }
                    PscULDNo.Remove(PscULDNo.Length - 1);
                }

                try
                {
                    DataSet dsMasterAirline = ObjMsBAl.GetAirlineDetails(Origin, Dest);
                    // DataSet dsOrginDestination = objBLL.GetOriginDest(Origin, Dest);
                    if (dsMasterAirline.Tables.Count > 0)
                    {
                        if (dsMasterAirline.Tables[1].Rows.Count > 0)
                        {
                            OriginCity = dsMasterAirline.Tables[1].Rows[0][0].ToString();
                            if (dsMasterAirline.Tables[2].Rows.Count > 0)
                            {

                                DestinationCity = dsMasterAirline.Tables[2].Rows[0][0].ToString();
                            }

                        }

                    }


                    if (dsMasterAirline.Tables.Count > 0)
                    {
                        if (dsMasterAirline.Tables[0].Rows.Count > 0)
                        {
                            AirlineAddress = dsMasterAirline.Tables[0].Rows[0][0].ToString() + ", " + dsMasterAirline.Tables[0].Rows[0][1].ToString();
                            CustomerSupportInfo = dsMasterAirline.Tables[0].Rows[0]["CustomerSupportInfo"].ToString();

                        }
                    }
                }
                catch (Exception ex)
                {

                }

                //int totalpcs = 0;
                //for (int iCount = 0; iCount < grdRouting.Rows.Count; iCount++)
                //{
                //    string strPcs = ((TextBox)grdRouting.Rows[iCount].FindControl("txtAcceptedPcs")).Text.Trim();
                //    totalpcs += (strPcs == "" ? 0 : int.Parse(strPcs));
                //}

                if (Convert.ToString(Session["AWBStatus"]) == "E")
                    CopyType = "Final Copy: Updated on " + Convert.ToDateTime(Session["UpdtOn"]).ToString("dd/MM/yy-HH:mm:ss") + " Printed on " + Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yy-HH:mm:ss");
                else
                    CopyType = "Draft Copy: Updated on " + Convert.ToDateTime(Session["UpdtOn"]).ToString("dd/MM/yy-HH:mm:ss") + " Printed on " + Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yy-HH:mm:ss");

                try
                {
                    if (IsAgreed == true)
                    {

                        DTExport.Rows.Add(DocType, AWBprefix, AWBno, AirLineCode, Origin, Dest, AgentCode, AgentName, Serviceclass, Handlinginfo, CommCode, CommDesc, Pcs, GrossWgt, Volume, ChargeWgt,
                                          "As agreed", "As agreed", "As agreed", PayMode, "As agreed", "As agreed", "As agreed", "As agreed", "As agreed", "As agreed", FltOrg, FltDest, FltNo, FltDate, FFRChecked, ExecDate, ExecBy, ExecAT, Consigneename,
                                          "As agreed", "As agreed", ShipperName, "As agreed", strDimension, ShipperAccountNo, ConsigneeAcNo, IssuingCarrierName, AgentIataCode, AccountCode, AccountInformation, ChargesCode, "P", "P", TXTDvForCarriage.Text, TXTDvForCustoms.Text,
                                          "KG", RateClause, CommCode, CommDesc, Length, Breadth, Height, "0", "0.0", "0.0", "0.0", "0.0", "", drpCurrency.SelectedItem.Text.Trim(), "", "", AirlineAddress, AirlinePrefix, "0", AccountInfo, OriginCity, DestinationCity, ms.ToArray(), CopyType, Logo.ToArray(),
                                          CustomerSupportInfo, wtPPD, wtCOLL, OtherPPD, OtherCOLL, "0", PscULDNo, SCI);



                    }
                    else
                    {
                        if (strAgentPreference != "IATA")
                        {
                            frateIATA = frateMKT;
                            frateMKT = frateIATA;
                        }

                        if (strAgentPreference == "Spot")
                        {
                            frateIATA = frateSpot;
                            RatePerKg = SpotRateKg.ToString();
                        }

                        if (ChargesCode == "PP" || ChargesCode == "PX")
                        {
                            DTExport.Rows.Add(DocType, AWBprefix, AWBno, AirLineCode, Origin, Dest, AgentCode, AgentName, Serviceclass, Handlinginfo, CommCode, CommDesc, Pcs, GrossWgt, Volume, ChargeWgt, +
                        frateIATA, frateMKT, ValCharge, PayMode, OCDueCar, OCDueAgent, SpotRate, DynaRate, SerTax, Total, FltOrg, FltDest, FltNo, FltDate, FFRChecked, ExecDate, ExecBy, ExecAT, Consigneename,
                        prepaid, TotalPrepaid, ShipperName, strOtherCharges, strDimension, ShipperAccountNo, ConsigneeAcNo, IssuingCarrierName, AgentIataCode, AccountCode, AccountInformation, ChargesCode, "P", "P", TXTDvForCarriage.Text, TXTDvForCustoms.Text,
                                              "KG", RateClause, CommCode, CommDesc, Length, Breadth, Height, "0", "0.0", "0.0", "0.0", "0.0", "", drpCurrency.SelectedItem.Text.Trim(), "", "", AirlineAddress, AirlinePrefix, RatePerKg, AccountInfo, OriginCity, DestinationCity, ms.ToArray(), CopyType, Logo.ToArray(),
                                              CustomerSupportInfo, wtPPD, wtCOLL, OtherPPD, OtherCOLL, "0", PscULDNo, SCI);
                        }
                        else
                        {
                            DTExport.Rows.Add(DocType, AWBprefix, AWBno, AirLineCode, Origin, Dest, AgentCode, AgentName, Serviceclass, Handlinginfo, CommCode, CommDesc, Pcs, GrossWgt, Volume, ChargeWgt,
                        "", "", "", PayMode, "", "", "", "", "", "", FltOrg, FltDest, FltNo, FltDate, FFRChecked, ExecDate, ExecBy, ExecAT, Consigneename,
                        prepaid, TotalPrepaid, ShipperName, strOtherCharges, strDimension, ShipperAccountNo, ConsigneeAcNo, IssuingCarrierName, AgentIataCode, AccountCode, AccountInformation, ChargesCode, "P", "P", TXTDvForCarriage.Text, TXTDvForCustoms.Text,
                                              "KG", RateClause, CommCode, CommDesc, Length, Breadth, Height, frateIATA, SerTax, OCDueAgent, OCDueCar, Total, "", drpCurrency.SelectedItem.Text.Trim(), "", "", AirlineAddress, AirlinePrefix, RatePerKg, AccountInfo, OriginCity, DestinationCity, ms.ToArray(), CopyType, Logo.ToArray(),
                                              CustomerSupportInfo, wtPPD, wtCOLL, OtherPPD, OtherCOLL, ValCharge, PscULDNo, SCI);
                        }


                    }

                    ///  Session["DTExport"] = DTExport;
                    Session["DTExport" + 0] = DTExport;
                    Session["DTExportSubDetails" + 0] = DTExportSubDetails;
                    Session["DTvolume" + 0] = DTvolume;

                }
                catch (Exception ex)
                {

                }
                //}

                string query = "'ShowEAWB.aspx?ID=" + 0 + "'";
                //Response.Write("<script>");
                //Response.Write("window.open(" + query + ",'_blank')");
                //Response.Write("</script>");


                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ");", true);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);

                try
                {
                    if (dsResult != null)
                        dsResult.Dispose();
                    if (DTExport != null)
                        DTExport.Dispose();
                    if (DTExportSubDetails != null)
                        DTExportSubDetails.Dispose();
                    if (DTvolume != null)
                        DTvolume.Dispose();
                    if (dsDimesionAll != null)
                        dsDimesionAll.Dispose();
                    if (dsOtherDetails != null)
                        dsOtherDetails.Dispose();
                }
                catch (Exception ex)
                {
                    dsResult = null;
                    DTExport = null;
                    DTExportSubDetails = null;
                    DTvolume = null;
                    dsDimesionAll = null;
                    dsOtherDetails = null;
                }


            }
            catch (Exception ex)
            {

            }

        }

        public void SaveMaterialGrid()
        {
            DataSet dsMaterialDetails = null;
            try
            {
                dsMaterialDetails = ((DataSet)Session["dsMaterialDetails"]).Clone();

                int i = 0;
                foreach (GridViewRow row in grdMaterialDetails.Rows)
                {
                    i++;
                    DataRow rw = dsMaterialDetails.Tables[0].NewRow();

                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                    {
                        rw["CommodityCode"] = "" + ((DropDownList)row.FindControl("ddlMaterialCommCode")).Text;
                    }
                    else
                    {
                        rw["CommodityCode"] = "" + ((TextBox)row.FindControl("ddlMaterialCommCode1")).Text;
                    }
                    rw["CodeDescription"] = "" + ((TextBox)row.FindControl("txtMaterialCommDesc")).Text;
                    rw["Pieces"] = "" + ((TextBox)row.FindControl("TXTPcs")).Text;
                    rw["GrossWeight"] = "" + ((TextBox)row.FindControl("txtCommGrossWt")).Text;
                    rw["Dimensions"] = "NO";
                    rw["VolumetricWeight"] = "" + ((TextBox)row.FindControl("txtCommVolWt")).Text;
                    rw["ChargedWeight"] = "" + ((TextBox)row.FindControl("txtCommChargedWt")).Text;
                    rw["AccountInfo"] = "" + ((TextBox)row.FindControl("txtAccountInfo")).Text;
                    rw["RowIndex"] = "" + i;


                    dsMaterialDetails.Tables[0].Rows.Add(rw);
                }

                Session["dsMaterialDetails"] = dsMaterialDetails.Copy();

                if (((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.Trim() != "")
                {
                    Session["TotalPcs"] = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text;
                    Session["TotalGrossWt"] = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text;
                    Session["TotalVolume"] = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text;
                    Session["TotalChgWt"] = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text;

                }
                else
                {
                    int totalpcs = 0;
                    decimal TotalGWt = 0, TotalCWt = 0, TotalVol = 0;
                    for (int iCount = 0; iCount < grdMaterialDetails.Rows.Count; iCount++)
                    {
                        string strPcs = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("TXTPcs")).Text.Trim();
                        string strGWt = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommGrossWt")).Text.Trim();
                        string strCWt = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommChargedWt")).Text.Trim();
                        string strVol = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommVolWt")).Text.Trim();

                        totalpcs += (strPcs == "" ? 0 : int.Parse(strPcs));
                        TotalGWt += (strGWt == "" ? 0 : Convert.ToDecimal(strGWt));
                        TotalCWt += (strCWt == "" ? 0 : Convert.ToDecimal(strCWt));
                        TotalVol += (strVol == "" ? 0 : Convert.ToDecimal(strVol));
                    }

                    Session["TotalPcs"] = totalpcs.ToString();
                    Session["TotalGrossWt"] = TotalGWt.ToString();
                    Session["TotalVolume"] = TotalVol.ToString();
                    Session["TotalChgWt"] = TotalCWt.ToString();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
                dsMaterialDetails = null;
            }
            finally
            {
                if (dsMaterialDetails != null)
                    dsMaterialDetails.Dispose();
            }
        }

        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            DataSet dsMaterialDetails = null;
            try
            {
                SaveMaterialGrid();
                dsMaterialDetails = (DataSet)Session["dsMaterialDetails"];

                DataRow row = dsMaterialDetails.Tables[0].NewRow();
                row["RowIndex"] = dsMaterialDetails.Tables[0].Rows.Count + 1;
                dsMaterialDetails.Tables[0].Rows.Add(row);



                grdMaterialDetails.DataSource = dsMaterialDetails.Copy();
                grdMaterialDetails.DataBind();

                //LoadCommodityDropdownWithData();

                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = Session["TotalPcs"].ToString();
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = Session["TotalGrossWt"].ToString();
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text = Session["TotalVolume"].ToString();
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = Session["TotalChgWt"].ToString();

            }
            catch (Exception ex)
            {
                dsMaterialDetails = null;
            }
            finally
            {
                if (dsMaterialDetails != null)
                    dsMaterialDetails.Dispose();
            }
        }

        protected void TXTPcs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ddlAgtCode_SelectedIndexChanged(null, null);
            }
            catch (Exception ex) { }
            try
            {
                int totalpcs = 0;

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {
                    string strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Text.Trim();
                    totalpcs += (strcount == "" ? 0 : int.Parse(strcount));
                }
                //totalpcs = Convert.ToDecimal(totalpcs).ToString("0.0");
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = "" + (totalpcs);
                ((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text = "" + (totalpcs);
                ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommGrossWt")).Focus();

                HidProcessFlag.Value = "1";

            }
            catch (Exception ex)
            {
                //lblStatus.Text = "" + ex.Message;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void txtCommGrossWt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int totalPieces = 0;
                int totalpcs = 0;
                //int GrossWt = 0;

                lblStatus.Text = "";

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {
                    string strPccount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Text.Trim();
                    totalPieces += (strPccount == "" ? 0 : int.Parse(strPccount));

                    string strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Text.Trim();
                    strcount = System.Math.Round(Convert.ToDecimal(strcount), 0).ToString("0");
                    ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Text = strcount;
                    totalpcs += (strcount == "" ? 0 : int.Parse(strcount));
                }

                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = "" + (totalPieces);
                ((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text = "" + (totalPieces);

                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = "" + totalpcs;
                ((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text = "" + (totalpcs);

                totalpcs = 0;

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {
                    string strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Text.Trim();
                    strcount = System.Math.Round(Convert.ToDecimal(strcount), 0).ToString("0");
                    ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Text = strcount;
                    totalpcs += (strcount == "" ? 0 : int.Parse(strcount));
                }

                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = "" + totalpcs;
                ((TextBox)grdRouting.Rows[0].FindControl("txtChrgWt")).Text = "" + (totalpcs);

                HidProcessFlag.Value = "1";

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommChargedWt")).Focus();

        }

        protected void txtCommVolWt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal totalvol = 0;
                float totalwt = 0;

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {
                    string strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommVolWt")).Text.Trim();
                    totalvol += (strcount == "" ? 0 : Convert.ToDecimal(strcount));

                    strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Text.Trim();
                    totalwt += (strcount == "" ? 0 : float.Parse(strcount));
                }

                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text = "" + totalvol;
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = "" + totalwt;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                    ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text = "" + (totalwt);

                HidProcessFlag.Value = "1";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
            }

        }

        public void SaveRouteDetails()
        {
            DataSet dsRoutDetails = null;
            try
            {
                dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Clone();

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    DataRow row = dsRoutDetails.Tables[0].NewRow();

                    row["FltOrigin"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                    row["FltDestination"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text;
                    //row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                    //row["FltTime"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Value;
                    row["FltDate"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                    row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                    row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                    row["Status"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).SelectedItem.Value;
                    row["Accepted"] = ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked ? "Y" : "N";
                    row["AcceptedPcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text.Trim(); //((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text;
                    row["ChrgWt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim();
                    row["AcceptedWt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Text.Trim();

                    try
                    {
                        row["PartnerType"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedItem.Text;

                        string str = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Value;

                        row["Airline"] = str;
                        if (str.Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            row["FltNumber"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.Trim();
                        }
                        else
                        {
                            row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                        }

                    }
                    catch (Exception ex) { }
                    dsRoutDetails.Tables[0].Rows.Add(row);
                }

                Session["dsRoutDetails"] = dsRoutDetails.Copy();
            }
            catch (Exception ex)
            {
                dsRoutDetails = null;
            }
            finally
            {
                if (dsRoutDetails != null)
                    dsRoutDetails.Dispose();
            }
        }

        protected void btnAddRouteDetails_Click(object sender, EventArgs e)
        {
            DataSet dsRoutDetails = null;
            try
            {
                string prevdest = "", prevtime = "", strDate = "";
                string Pieces = "", GrossWt = "", ChrgblWt = "";

                if (grdRouting.Rows.Count != 0)
                {
                    if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim() == "")
                    {
                        LBLRouteStatus.Text = "Set previous destination first.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return;
                    }
                    else
                    {
                        prevdest = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim();
                        if (prevdest.Equals(Session["Destination"].ToString().Trim(), StringComparison.OrdinalIgnoreCase) || prevdest.ToUpper() == Session["Destination"].ToString().Trim())
                        {
                            prevdest = BookingBAL.OrgStation;
                        }
                        Pieces = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtPcs")).Text.Trim();
                        GrossWt = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtWt")).Text.Trim();
                        ChrgblWt = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtChrgWt")).Text.Trim();
                    }

                    strDate = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFdate")).Text.Trim();
                }

                SaveRouteDetails();

                dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Copy();
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = prevdest;
                row["FltDate"] = strDate;  //dtCurrentDate.ToString("dd/MM/yyyy");
                row["Airline"] = ddlAirlineCode.Text.ToString();
                row["Pcs"] = Pieces;
                row["Wt"] = GrossWt;
                row["ChrgWt"] = ChrgblWt;

                dsRoutDetails.Tables[0].Rows.Add(row);

                Session["dsRoutDetails"] = dsRoutDetails.Copy();

                grdRouting.DataSource = dsRoutDetails.Copy();
                grdRouting.DataBind();

                LoadDropDownAndCheckBoxRouteDetails();
                LoadAirlineCode("");
                //ddlPartnerType_SelectionChange(null, null);
                LoadAWBStatusDropdown();
                CheckPartnerFlightSupport();
                HidProcessFlag.Value = "1";
                HidChangeDate.Value = "Y";

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                dsRoutDetails = null;
            }
            finally
            {
                if (dsRoutDetails != null)
                    dsRoutDetails.Dispose();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }

        protected void txtFltDest_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")) == ((TextBox)sender))
                    {
                        rowindex = i;
                    }
                }

                //Clear existing flights.
                ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = null;

                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);
                //Check for count
                //DropDownList ddlFlight = (DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum");
                //if (ddlFlight.Items.Count < 2)
                //{
                //}
                if (prrate == false)
                    HidProcessFlag.Value = "1";

                if (((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text.Trim() == "")
                {
                    if (((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.Trim() != "")
                    {
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text;
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text;
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text;

                    }
                    else
                    {
                        int totalpcs = 0;
                        decimal TotalGWt = 0, TotalCWt = 0, TotalVol = 0;
                        for (int iCount = 0; iCount < grdMaterialDetails.Rows.Count; iCount++)
                        {
                            string strPcs = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("TXTPcs")).Text.Trim();
                            string strGWt = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommGrossWt")).Text.Trim();
                            string strCWt = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommChargedWt")).Text.Trim();
                            string strVol = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommVolWt")).Text.Trim();

                            totalpcs += (strPcs == "" ? 0 : int.Parse(strPcs));
                            TotalGWt += (strGWt == "" ? 0 : Convert.ToDecimal(strGWt));
                            TotalCWt += (strCWt == "" ? 0 : Convert.ToDecimal(strCWt));
                            TotalVol += (strVol == "" ? 0 : Convert.ToDecimal(strVol));
                        }

                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = totalpcs.ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = TotalGWt.ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = TotalCWt.ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text = TotalVol.ToString();

                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text = totalpcs.ToString();
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text = TotalGWt.ToString();
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = TotalCWt.ToString();
                    }
                }
                ////Load product types based on commodity codes.
                //LoadMatchingProductTypes();
                if (ddlServiceclass.SelectedItem.Text == "FOC")
                    ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).SelectedItem.Text = ddlServiceclass.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void txtFltNumber_TextChanged(object sender, EventArgs e)
        {
            int rowindex = 0;
            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {
                if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")) == ((DropDownList)sender))
                {
                    rowindex = i;
                }
            }

            DataSet ds = (DataSet)Session["Flt"];

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string Origin = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper();
                string Dest = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper();
                string FlightNo = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedItem.ToString();
                int ScheduleID = 0;

                for (int intCount = 0; intCount < ds.Tables[0].Rows.Count; intCount++)
                {
                    if (Origin == ds.Tables[0].Rows[intCount]["FltOrigin"].ToString() && Dest == ds.Tables[0].Rows[intCount]["FltDestination"].ToString()
                        && FlightNo == ds.Tables[0].Rows[intCount]["FltNumber"].ToString())
                    {
                        ScheduleID = int.Parse(ds.Tables[0].Rows[intCount]["ScheduleID"].ToString());
                    }
                }

                ((HiddenField)grdRouting.Rows[rowindex].FindControl("HidScheduleID")).Value = ScheduleID.ToString();
                //Session["Mod"] = "1";
                if (ds != null)
                    ds.Dispose();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void btnDeleteMaterial_Click(object sender, EventArgs e)
        {
            DataSet dsMaterialDetailsTemp = null, dsMaterialDetails = null;
            try
            {
                SaveMaterialGrid();

                dsMaterialDetailsTemp = ((DataSet)Session["dsMaterialDetails"]).Clone();
                dsMaterialDetails = (DataSet)Session["dsMaterialDetails"];

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {
                    if (!((CheckBox)grdMaterialDetails.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow rw = dsMaterialDetailsTemp.Tables[0].NewRow();
                        rw["CommodityCode"] = dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString();
                        rw["CodeDescription"] = dsMaterialDetails.Tables[0].Rows[i]["CodeDescription"].ToString();
                        rw["Pieces"] = dsMaterialDetails.Tables[0].Rows[i]["Pieces"].ToString();
                        rw["GrossWeight"] = dsMaterialDetails.Tables[0].Rows[i]["GrossWeight"].ToString();
                        rw["Dimensions"] = "NO";
                        rw["VolumetricWeight"] = dsMaterialDetails.Tables[0].Rows[i]["VolumetricWeight"].ToString();
                        rw["ChargedWeight"] = dsMaterialDetails.Tables[0].Rows[i]["ChargedWeight"].ToString();
                        rw["AccountInfo"] = dsMaterialDetails.Tables[0].Rows[i]["AccountInfo"].ToString();
                        rw["RowIndex"] = "" + i;

                        dsMaterialDetailsTemp.Tables[0].Rows.Add(rw);
                    }
                }

                Session["dsMaterialDetails"] = dsMaterialDetailsTemp.Copy();

                grdMaterialDetails.DataSource = dsMaterialDetailsTemp.Copy();
                grdMaterialDetails.DataBind();

                //LoadCommodityDropdownWithData();

                CalculateTotal();
                SaveMaterialGrid();

                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = Session["TotalPcs"].ToString();
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = Session["TotalGrossWt"].ToString();
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text = Session["TotalVolume"].ToString();
                ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = Session["TotalChgWt"].ToString();
            }
            catch (Exception ex)
            {
                dsMaterialDetailsTemp = null;
                dsMaterialDetails = null;
            }
            finally
            {

                if (dsMaterialDetailsTemp != null)
                    dsMaterialDetailsTemp.Dispose();
                if (dsMaterialDetails != null)
                    dsMaterialDetails.Dispose();
            }
        }

        protected void btnDeleteRoute_Click(object sender, EventArgs e)
        {
            DataSet dsRouteDetailsTemp = null, dsRouteDetails = null;
            try
            {
                SaveRouteDetails();

                dsRouteDetailsTemp = ((DataSet)Session["dsRoutDetails"]).Clone();
                dsRouteDetails = (DataSet)Session["dsRoutDetails"];

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (!((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow row = dsRouteDetailsTemp.Tables[0].NewRow();

                        row["FltOrigin"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                        row["FltDestination"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text;
                        //row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem;
                        row["FltDate"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                        row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                        row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                        row["Status"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).SelectedItem.Value;
                        row["Accepted"] = ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked ? "Y" : "N";
                        row["AcceptedPcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text.Trim(); //((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Text;
                        row["ChrgWt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim();
                        row["AcceptedWt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Text.Trim();
                        try
                        {
                            row["PartnerType"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedItem.Value;

                            string str = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Value;
                            row["Airline"] = str;
                            if (str.Equals("other", StringComparison.OrdinalIgnoreCase))
                            {
                                row["FltNumber"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.Trim();
                            }
                            else
                            {
                                row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                            }

                        }
                        catch (Exception ex) { }

                        dsRouteDetailsTemp.Tables[0].Rows.Add(row);
                    }
                }

                Session["dsRoutDetails"] = dsRouteDetailsTemp.Copy();

                grdRouting.DataSource = dsRouteDetailsTemp.Copy();
                grdRouting.DataBind();

                if (dsRouteDetailsTemp == null || dsRouteDetailsTemp.Tables.Count < 1 || dsRouteDetailsTemp.Tables[0].Rows.Count < 1)
                    LoadGridRoutingDetail();

                LoadDropDownAndCheckBoxRouteDetails();
                LoadAirlineCode("");
                ddlPartnerType_SelectionChange(null, null);
                CheckPartnerFlightSupport();
                HidProcessFlag.Value = "1";
                HidChangeDate.Value = "Y";
            }
            catch (Exception ex)
            {
                dsRouteDetailsTemp = null;
                dsRouteDetails = null;
                LBLRouteStatus.Text = "" + ex.Message;
            }
            finally
            {
                if (dsRouteDetailsTemp != null)
                    dsRouteDetailsTemp.Dispose();
                if (dsRouteDetails != null)
                    dsRouteDetails.Dispose();
            }
        }

        public void LoadDropDownAndCheckBoxRouteDetails()
        {
            DataSet dsRouteDetails = null;
            try
            {
                dsRouteDetails = (DataSet)Session["dsRoutDetails"];

                for (int i = 0; i < dsRouteDetails.Tables[0].Rows.Count; i++)
                {
                    ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked = (dsRouteDetails.Tables[0].Rows[i]["Accepted"].ToString().Trim() == "Y");
                    try
                    {//Disable check box once Pieces are accepted
                        if (((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked)
                        {
                            ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = false;
                        }

                    }
                    catch (Exception ex) { }
                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).SelectedValue = (dsRouteDetails.Tables[0].Rows[i]["Status"].ToString().Trim() == "" ? "Q" : dsRouteDetails.Tables[0].Rows[i]["Status"].ToString().Trim());
                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim();
                    }
                    catch (Exception ex) { }

                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim();

                        if (dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim().Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            DropDownList droplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum"));
                            droplist.Visible = false;
                            TextBox txtflight = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID"));
                            txtflight.Text = "";
                            txtflight.Text = dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim();
                            txtflight.Visible = true;

                        }
                        else
                        {
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim(), dsRouteDetails.Tables[0].Rows[i]["FltTime"].ToString().Trim()));
                        }

                    }
                    catch (Exception ex) { }
                    ///ddlPartner

                }
            }
            catch (Exception ex)
            {
                dsRouteDetails = null;
                LBLRouteStatus.Text = "" + ex.Message;
            }
            finally
            {
                if (dsRouteDetails != null)
                    dsRouteDetails.Dispose();
            }

        }

        public void CalculateTotal()
        {
            int totalpcs = 0;

            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
            {
                string strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Text.Trim();
                totalpcs += (strcount == "" ? 0 : int.Parse(strcount));
            }

            ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = "" + totalpcs;

            totalpcs = 0;

            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
            {
                string strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Text.Trim();
                totalpcs += (strcount == "" ? 0 : int.Parse(strcount));
            }

            ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = "" + totalpcs;


            totalpcs = 0;

            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
            {
                string strcount = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Text.Trim();
                totalpcs += (strcount == "" ? 0 : int.Parse(strcount));
            }

            ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = "" + totalpcs;

        }

        public void LoadFlights()
        {
            DataSet dsRoutDetails = null;
            try
            {
                string Pieces = string.Empty;
                string GrossWt = string.Empty;
                string ChrgblWt = string.Empty;

                RefreshMaterialDG();
                LoadAirlineCode("");
                //ddlPartnerType_SelectionChange(null, null);
                Pieces = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text;
                GrossWt = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text;
                ChrgblWt = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text;

                if (HidFlightsChanged.Value == "1")
                {

                    dsRoutDetails = (DataSet)Session["dsSelectedFlights"];

                    //Check that dataset contains data.
                    if (dsRoutDetails == null || dsRoutDetails.Tables == null ||
                        dsRoutDetails.Tables.Count <= 0 || dsRoutDetails.Tables[0].Rows.Count <= 0)
                    {
                        return;
                    }
                    HidChangeDate.Value = "N";
                    //Check if data came from btnshowflight click in Route Details grid.
                    if (Session["SingleRow"] != null && Session["SingleRow"].ToString() == "Y")
                    {
                        if (Session["ShowFltRowIndex"] != null && Session["ShowFltRowIndex"].ToString() != "")
                        {
                            int rowIndex = int.Parse(Session["ShowFltRowIndex"].ToString());
                            if (rowIndex >= 0 && rowIndex < grdRouting.Rows.Count)
                            {
                                ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtFdate")).Text = dsRoutDetails.Tables[0].Rows[0]["FltDate"].ToString();
                                ((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlFltNum")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRoutDetails.Tables[0].Rows[0]["FltNumber"].ToString(), dsRoutDetails.Tables[0].Rows[0]["FltTime"].ToString()));
                                ((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlFltNum")).SelectedIndex = 0;
                                ((HiddenField)grdRouting.Rows[rowIndex].FindControl("hdnFltNum")).Value = dsRoutDetails.Tables[0].Rows[0]["FltNumber"].ToString();
                                ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtPcs")).Focus();

                                ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtPcs")).Text = Pieces;
                                ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtWt")).Text = GrossWt;
                                ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtChrgWt")).Text = ChrgblWt;

                                //hardcode on 17/07/2013
                                ((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlPartnerType")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlPartnerType")).Items.Add(new ListItem("AIR"));
                                ((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlPartner")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlPartner")).Items.Add(new ListItem(
                                    Convert.ToString(Session["AirlinePrefix"])));

                            }
                        }
                    }
                    else
                    {
                        grdRouting.DataSource = dsRoutDetails.Copy();
                        grdRouting.DataBind();

                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text = dsRoutDetails.Tables[0].Rows[i]["FltDate"].ToString();
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString(), dsRoutDetails.Tables[0].Rows[i]["FltTime"].ToString()));
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedIndex = 0;
                            ((HiddenField)grdRouting.Rows[i].FindControl("hdnFltNum")).Value = dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString();

                            ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = Pieces;
                            ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = GrossWt;
                            ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text = ChrgblWt;

                            string fltinfo = dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString();
                            try
                            {
                                string[] info = fltinfo.Split('-');
                                if (info.Length > 1)
                                {
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Clear();
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem(info[1]));
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(info[0]));
                                }
                                else
                                {
                                    //hardcode on 17/07/2013
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Clear();
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem("AIR"));
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(
                                        Convert.ToString(Session["AirlinePrefix"])));

                                }
                            }
                            catch (Exception ex)
                            {   //hardcode on 17/07/2013
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem("AIR"));
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(
                                    Convert.ToString(Session["AirlinePrefix"])));
                            }
                        }
                    }
                    HidFlightsChanged.Value = "0";

                }

            }
            catch (Exception ex)
            {
                dsRoutDetails = null;
                lblStatus.Text = "Loading Flight details failed [" + ex.Message + "]";
            }
            finally
            {
                if (dsRoutDetails != null)
                    dsRoutDetails.Dispose();
            }
        }

        protected void txtPcs_TextChanged1(object sender, EventArgs e)
        {
            try
            {

                LBLRouteStatus.Text = "";

                if (((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.Trim() == "")
                {
                    LBLRouteStatus.ForeColor = Color.Red;
                    LBLRouteStatus.Text = "Fill Shipment details.(Total Pcs count is zero)";


                    int rowindex = 0;
                    string flightid = "";
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")) == ((TextBox)sender))
                        {
                            flightid = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                            rowindex = i;
                        }
                    }

                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.Trim() == flightid)
                        {
                            ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "";
                        }

                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }
                else if (((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text.Trim() == "")
                {
                    LBLRouteStatus.ForeColor = Color.Red;
                    LBLRouteStatus.Text = "Fill Shipment details.(Total charged wt is zero)";


                    int rowindex = 0;
                    string flightid = "";
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")) == ((TextBox)sender))
                        {
                            flightid = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                            rowindex = i;
                        }
                    }

                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Text.Trim() == flightid)
                        {
                            ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "";
                        }

                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }
                else
                {

                    try
                    {
                        int.Parse(((TextBox)sender).Text);

                    }
                    catch (Exception ex)
                    {
                        LBLRouteStatus.Text = "Pcs count is invalid.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        return;
                    }

                    string weight = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text.Trim();
                    string ChrgblWt = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text.Trim();
                    if (weight.Contains('.'))
                        weight = weight.Substring(0, weight.IndexOf('.'));

                    //if (ChrgblWt.Contains('.'))
                    //    ChrgblWt = ChrgblWt.Substring(0, ChrgblWt.IndexOf('.'));

                    int total = int.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.Trim());
                    //int wt = int.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text.Trim());
                    int wt = int.Parse(weight);
                    float Cwt = float.Parse(ChrgblWt);

                    ArrayList flightlist = new ArrayList();
                    int currenttotal = 0;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        //if (!flightlist.Contains(((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.Trim()))
                        //{
                        //    currenttotal += int.Parse(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                        //    flightlist.Add(((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.Trim());
                        //}
                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == Session["Origin"].ToString())
                        {
                            currenttotal += int.Parse(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                        }

                    }

                    if ((currenttotal) > total)
                    {
                        LBLRouteStatus.Text = "Total Pcs count in route cannot be greater than total pcs in shipment details.";

                        int rowindex = 0;
                        string flightid = "";
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")) == ((TextBox)sender))
                            {
                                flightid = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                                rowindex = i;
                            }
                        }

                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.Trim() == flightid)
                            {
                                ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "";
                            }

                        }


                    }
                    else
                    {
                        int rowindex = 0;
                        string flightid = "", carrier = "";
                        string date = "";
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")) == ((TextBox)sender))
                            {
                                carrier = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                                if (carrier.Equals("Other", StringComparison.OrdinalIgnoreCase))
                                {
                                    flightid = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.Trim();
                                }
                                else
                                {
                                    flightid = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                                }
                                date = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                                rowindex = i;
                            }
                        }

                        int calwt = (int)((int.Parse(((TextBox)sender).Text)) * wt / total);
                        float calChrgblwt = (float)((float.Parse(((TextBox)sender).Text)) * Cwt / total);


                        string routeFLT = "";
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text.ToString().Equals("Other", StringComparison.OrdinalIgnoreCase))
                            {
                                routeFLT = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.Trim();
                            }
                            else
                            {
                                routeFLT = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                            }
                            if (routeFLT.Trim() == flightid.Trim() && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == date.Trim())
                            {
                                ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "" + int.Parse(((TextBox)sender).Text);
                                ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = "" + calwt;
                                ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text = "" + calChrgblwt;
                            }

                        }


                    }
                }

                HidProcessFlag.Value = "1";
            }
            catch (Exception ex)
            {
                //LBLRouteStatus.ForeColor = Color.Red;
                //LBLRouteStatus.Text = "" + ex.Message;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void ShipperConDetailsChenged(object sender, EventArgs e)
        {
            LBLShipperConStatus.Text = "";

            if (TXTShipper.Text.Trim() == "" || TXTShipAddress.Text.Trim() == "" || TXTShipTelephone.Text.Trim() == "" ||
               TXTConsignee.Text.Trim() == "" || TXTConAddress.Text.Trim() == "" || TXTConTelephone.Text.Trim() == "")
            {
                imgcross.Visible = true;
                imgcross.ImageUrl = "~/Images/Cross.jpg";
            }
            else
            {
                imgcross.Visible = true;
                imgcross.ImageUrl = "~/Images/Check.jpg";
            }


            if (TXTShipTelephone.Text.Trim() != "")
            {
                string str = TXTShipTelephone.Text;
                for (int i = 0; i < str.Length; i++)
                {
                    if (!(str[i] == '0' || str[i] == '1' || str[i] == '2' || str[i] == '3' || str[i] == '4' || str[i] == '5' || str[i] == '6' || str[i] == '7' || str[i] == '8' || str[i] == '9' || str[i] == '+' || str[i] == '-'))
                    {
                        LBLShipperConStatus.Text = "Shipper Telephone is invalid";
                        TXTShipTelephone.Text = "";
                        imgcross.Visible = true;
                        imgcross.ImageUrl = "~/Images/Cross.jpg";

                        //ScriptManager.RegisterStartupScript(TXTShipper, TXTShipper.GetType(), "HidUnhide", "document.getElementById('DivShipperCon').style.display='block'", true);
                        return;
                    }
                }

            }


            if (TXTConTelephone.Text.Trim() != "")
            {
                string str = TXTConTelephone.Text;
                for (int i = 0; i < str.Length; i++)
                {
                    if (!(str[i] == '0' || str[i] == '1' || str[i] == '2' || str[i] == '3' || str[i] == '4' || str[i] == '5' || str[i] == '6' || str[i] == '7' || str[i] == '8' || str[i] == '9' || str[i] == '+' || str[i] == '-'))
                    {
                        LBLShipperConStatus.Text = "Consignee Telephone is invalid";
                        TXTConTelephone.Text = "";
                        imgcross.Visible = true;
                        imgcross.ImageUrl = "~/Images/Cross.jpg";

                        //ScriptManager.RegisterStartupScript(TXTShipper, TXTShipper.GetType(), "HidUnhide", "document.getElementById('DivShipperCon').style.display='block'", true);
                        return;
                    }
                }

            }

            if (TXTShipper == ((TextBox)sender))
                TXTShipAddress.Focus();
            if (TXTShipAddress == ((TextBox)sender))
                ddlShipCountry.Focus();

            if (TXTShipTelephone == ((TextBox)sender))
                TXTConsignee.Focus();
            if (TXTConsignee == ((TextBox)sender))
                TXTConAddress.Focus();

            if (TXTConAddress == ((TextBox)sender))
                ddlConCountry.Focus();

            //ScriptManager.RegisterStartupScript(TXTShipper, TXTShipper.GetType(), "HidUnhide", "document.getElementById('DivShipperCon').style.display='block'", true);
        }

        public void AutoPopulateData(bool editable, string AWBNumber, string AWBPrefix, string state)
        {
            DataSet dsResult = new DataSet();
            try
            {

                string errormessage = "";
                bool blnAcceptance = false;
                prrate = true;
                #region Validate Login & AWBNumber
                try
                {
                    if (!objBLL.ValidateUserToView(AWBNumber, AWBPrefix, Session["UserName"].ToString(), Session["Station"].ToString()))
                    {
                        lblStatus.Text = "You are not allowed to view entered AWB.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        txtAWBNo.Focus();
                        return;
                    }

                }
                catch (Exception ex) { }

                #endregion
                if (objBLL.GetAWBDetails(AWBNumber, AWBPrefix, ref dsResult, ref errormessage))
                {

                    // AWBSummaryMaster
                    try
                    {
                        if (dsResult != null && dsResult.Tables.Count > 12 && dsResult.Tables[12].Rows.Count > 0)
                        {
                            btnHABDetails.Text = btnHABDetails.Text + "(" + dsResult.Tables[12].Rows[0]["Count"].ToString() + ")";
                            if (dsResult.Tables[12].Rows[0]["Count"].ToString() != "0")
                            {
                                imgHAWB.Visible = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    { }

                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0 &&
                    dsResult.Tables[0].Rows[0]["CutOffTime"].ToString().Trim() != "")
                    {
                        string CutOffTime = dsResult.Tables[0].Rows[0]["CutOffTime"].ToString().Trim();

                        int hr = int.Parse(CutOffTime.Substring(0, CutOffTime.IndexOf(":")));
                        int min = int.Parse(CutOffTime.Substring(CutOffTime.IndexOf(":") + 1));

                        if (min > 0)
                            Session["StationCutOffTime"] = hr + 1;
                        else
                            Session["StationCutOffTime"] = hr;
                    }
                    else
                        Session["StationCutOffTime"] = 99999;

                    //Commented by Vijay - 11-10-2014
                    //if (dsResult != null && dsResult.Tables.Count > 11 && dsResult.Tables[11].Rows.Count > 0)
                    //{
                    //    if (Convert.ToString(dsResult.Tables[11].Rows[0][0]) == "Y")
                    //        lblStatus.Text = lblStatus.Text + " Invoice exists.";
                    //}


                    Session["BookingID"] = dsResult.Tables[0].Rows[0]["SerialNumber"].ToString();
                    txtAWBNo.Text = AWBNumber;
                    HidAWBNumber.Value = AWBNumber;
                    ddlDocType.Text = "" + dsResult.Tables[0].Rows[0]["DocumentType"].ToString();
                    txtAwbPrefix.Text = "" + dsResult.Tables[0].Rows[0]["AWBPrefix"].ToString();
                    chkDlvOnHAWB.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["IsDlvOnHAWB"]);
                    ddlAirlineCode.Text = dsResult.Tables[0].Rows[0]["DesignatorCode"].ToString();
                    TXTCustomerCode.Text = dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();
                    Session["UpdtOn"] = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["UpdatedOn"]);
                    chkTBScreened.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["ToBeScreened"]);
                    txtComment.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["ShpRemarks"]);
                    try
                    {
                        txtSCI.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["SCI"]);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (dsResult.Tables[0].Rows[0]["SpotRateId"].ToString().Length > 0)
                            radSpot.Enabled = true;
                    }
                    catch (Exception es) { }
                    try
                    {
                        chkInterline.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["Interline"].ToString());
                    }
                    catch (Exception EX) { }
                    try
                    {
                        txtAttchDoc.Text = dsResult.Tables[0].Rows[0]["Documents"].ToString();
                    }
                    catch (Exception ex) { }
                    /* Commented on 31/7/2013
                     * Interline check box not shown
                     * if (chkInterline.Checked)
                     {
                         ddlDest.Visible = false;
                         txtDest.Visible = true;
                         txtDest.Text = dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();
                         ddlAirlineCode.Text = "";
                         ddlAirlineCode.Visible = false;
                      
                        
                     }
                     else
                     {
                         ddlDest.Visible = true;
                         txtDest.Visible = false;
                         ddlAirlineCode.Text = Session["awbPrefix"].ToString();
                         ddlAirlineCode.Visible = true;
                        
                     }*/

                    ddlOrg.Items.Clear();
                    ddlOrg.Items.Add("" + dsResult.Tables[0].Rows[0]["OriginCode"].ToString());
                    ddlOrg.SelectedIndex = 0;

                    Session["Origin"] = dsResult.Tables[0].Rows[0]["OriginCode"].ToString();
                    Session["Destination"] = dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();


                    SetOrgDest(dsResult.Tables[0].Rows[0]["OriginCode"].ToString(), dsResult.Tables[0].Rows[0]["DestinationCode"].ToString());

                    ddlDest.SelectedItem.Text = "" + dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();
                    ddlDest.Text = "" + dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();
                    ddlServiceclass.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["ServiceCargoClassId"].ToString());

                    CHKAsAggred.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Agreed"]);
                    CHKBonded.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Bonded"]);
                    CHKConsole.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Console"]);
                    CHKExportShipment.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Export"]);

                    FillHandlerCodes();

                    ddlHandler.SelectedValue = dsResult.Tables[0].Rows[0]["Handler"].ToString();

                    //ddlAgtCode.Items.Clear();
                    //ddlAgtCode.Items.Add(new ListItem("" + dsResult.Tables[0].Rows[0]["AgentCode"].ToString(), "" + dsResult.Tables[0].Rows[0]["AgentName"].ToString()));
                    //ddlAgtCode.SelectedIndex = 0;

                    TXTAgentCode.Text = dsResult.Tables[0].Rows[0]["AgentCode"].ToString();
                    txtAgentName.Text = dsResult.Tables[0].Rows[0]["AgentName"].ToString();
                    txtRemarks.Text = dsResult.Tables[0].Rows[0]["Remarks"].ToString();
                    txtSpecialHandlingCode.Text = dsResult.Tables[0].Rows[0]["SHCCodes"].ToString();

                    try
                    {
                        TXTAgentCode_TextChanged(null, null);
                    }
                    catch (Exception ex) { }

                    txtAgentName.Text = "" + dsResult.Tables[0].Rows[0]["AgentName"].ToString();
                    txtHandling.Text = "" + dsResult.Tables[0].Rows[0]["HandlingInfo"].ToString();
                    TXTCustomerCode.Text = "" + dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();

                    txtDriverName.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["DriverName"]);
                    txtDriverDL.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["DLNumber"]);
                    txtPhoneNo.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["PhoneNumber"]);
                    txtVehicleNo.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["VehicleNo"]);

                    txtIACCode.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["IACCode"]);
                    txtKnownShipper.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["KnownShipper"]);
                    txtCCSF.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["CCSFCode"]);

                    TXTDvForCustoms.Text = "" + dsResult.Tables[0].Rows[0]["DVCustom"].ToString();
                    TXTDvForCarriage.Text = "" + dsResult.Tables[0].Rows[0]["DVCarriage"].ToString();
                    txtSLAC.Text = dsResult.Tables[0].Rows[0]["SLAC"].ToString();
                    txtCustoms.Text = dsResult.Tables[0].Rows[0]["Customs"].ToString();
                    txtEURIN.Text = dsResult.Tables[0].Rows[0]["EURIN"].ToString();
                    ddlIrregularityCode.SelectedValue = dsResult.Tables[0].Rows[0]["IrregularityCode"].ToString();
                    ddlProductType.SelectedValue = dsResult.Tables[0].Rows[0]["ProductType"].ToString();

                    //Added by Vishal - 04 MAY 2014 ********************
                    txtUOM.Text = dsResult.Tables[0].Rows[0]["UOM"].ToString();
                    //******************** Added by Vishal - 04 MAY 2014

                    CHKBonded.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsBonded"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsBonded"].ToString().Trim());
                    CHKConsole.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsConsole"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsConsole"].ToString().Trim());
                    CHKExportShipment.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsExport"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsExport"].ToString().Trim());
                    CHKAsAggred.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsAsAggred"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsAsAggred"].ToString().Trim());

                    // AWBShipperConsigneeDetails

                    if (dsResult != null && dsResult.Tables.Count > 5 && dsResult.Tables[6].Rows.Count > 0)
                    {
                        TXTShipper.Text = "" + dsResult.Tables[6].Rows[0]["ShipperName"].ToString();
                        TXTShipTelephone.Text = "" + dsResult.Tables[6].Rows[0]["ShipperTelephone"].ToString();
                        TXTShipAddress.Text = "" + dsResult.Tables[6].Rows[0]["ShipperAddress"].ToString();
                        ddlShipCountry.Text = "" + dsResult.Tables[6].Rows[0]["ShipperCountry"].ToString();

                        TXTShipperAdd2.Text = "" + dsResult.Tables[6].Rows[0]["ShipperAdd2"].ToString();
                        TXTShipperCity.Text = "" + dsResult.Tables[6].Rows[0]["ShipperCity"].ToString();
                        TXTShipperState.Text = "" + dsResult.Tables[6].Rows[0]["ShipperState"].ToString();
                        TXTShipPinCode.Text = "" + dsResult.Tables[6].Rows[0]["ShipperPincode"].ToString();

                        TXTConsignee.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeName"].ToString();
                        TXTConTelephone.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeTelephone"].ToString();
                        TXTConAddress.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeAddress"].ToString();
                        ddlConCountry.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeCountry"].ToString();

                        TXTConsigAdd2.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeAddress2"].ToString();
                        TXTConsigCity.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeCity"].ToString();
                        TXTConsigState.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeState"].ToString();
                        TXTConsigPinCode.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneePincode"].ToString();

                        txtShipperCode.Text = dsResult.Tables[6].Rows[0]["ShipperAccCode"].ToString();
                        txtConsigneeCode.Text = dsResult.Tables[6].Rows[0]["ConsigAccCode"].ToString();
                        TXTShipperEmail.Text = dsResult.Tables[6].Rows[0]["ShipperEmailId"].ToString();
                        TXTConsigEmail.Text = dsResult.Tables[6].Rows[0]["ConsigEmailId"].ToString();

                        txtShipperCCSF.Text = dsResult.Tables[6].Rows[0]["ShipperCCSF"].ToString();
                        //txtConsigneeCCSF.Text = dsResult.Tables[6].Rows[0]["ConsigneeCCSF"].ToString();
                    }

                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[1].Rows.Count > 0)
                    {
                        // AWBRateMaster
                        DataSet dsMaterialDetails = new DataSet();
                        // CommodityCode CodeDescription Pieces GrossWeight Dimensions VolumetricWeight ChargedWeight RowIndex
                        dsMaterialDetails.Tables.Clear();
                        dsMaterialDetails.Tables.Add(dsResult.Tables[1].Copy());

                        grdMaterialDetails.DataSource = dsMaterialDetails.Copy();
                        grdMaterialDetails.DataBind();
                        LoadCommodityDropdown();
                        try
                        {
                            CHKAllIn.Checked = Convert.ToBoolean(dsMaterialDetails.Tables[0].Rows[0]["AllinRate"].ToString());
                        }
                        catch (Exception ex)
                        {
                            CHKAllIn.Checked = false;
                        }
                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            //string res = ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Text = dsMaterialDetails.Tables[0].Rows[i]["CodeDescription"].ToString();
                            if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                            {
                                ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Text = dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString();
                            }
                            else
                            {
                                ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Text = dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString();
                            }

                            //Load Payment Mode dropdown with all available payment mode values.
                            try
                            {
                                //DropDownList ddl = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                                //if (Session["ConBooking_PayModeMaster"] != null)
                                //{
                                //    DataTable dt = (DataTable)Session["ConBooking_PayModeMaster"];
                                //    ddl.Items.Clear();
                                //    ddl.DataSource = dt;
                                //    ddl.DataTextField = "PayModeText";
                                //    ddl.DataValueField = "PayModeCode";
                                //    ddl.DataBind();
                                //    if (ddl.Items.Count > 1)
                                //        ddl.SelectedIndex = 1;
                                    
                                //}
                                SetPaymentMode();
                                ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).SelectedValue = dsMaterialDetails.Tables[0].Rows[i]["PaymentMode"].ToString();
                            }
                            catch (Exception)
                            {
                            }
                            ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlShipmentType")).Text = dsMaterialDetails.Tables[0].Rows[i]["ShipmentType"].ToString();
                        }

                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = "" + dsResult.Tables[8].Rows[0]["TotalPcs"].ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = "" + dsResult.Tables[8].Rows[0]["TotalGrossWt"].ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text = "" + dsResult.Tables[8].Rows[0]["TotalVolume"].ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = "" + dsResult.Tables[8].Rows[0]["TotalChargedWt"].ToString();

                        Session["dsMaterialDetails"] = dsMaterialDetails.Copy();
                    }
                    else
                    {
                        LoadGridMaterialDetail();
                    }


                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[3].Rows.Count > 0)
                    {
                        //AWBRouteMaster
                        //FltOrigin FltDestination FltNumber FltDate Pcs Wt Status Accepted AcceptedPcs
                        DataSet dsRoutDetails = new DataSet();
                        dsRoutDetails.Tables.Clear();
                        dsRoutDetails.Tables.Add(dsResult.Tables[3].Copy());

                        LoadAirlineCode("");
                        ddlPartnerType_SelectionChange(null, null);
                        LoadAWBStatusDropdown();


                        grdRouting.DataSource = dsRoutDetails.Copy();
                        grdRouting.DataBind();

                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {

                            ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked = dsRoutDetails.Tables[0].Rows[i]["Accepted"].ToString() == "Y";
                            try
                            {//Disable check box once Pieces are accepted
                                if (((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked)
                                {
                                    ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = false;
                                    blnAcceptance = true;
                                }

                            }
                            catch (Exception ex) { }
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Status"].ToString();
                            #region Flight Selection
                            TextBox txtFltDestination = (TextBox)grdRouting.Rows[i].FindControl("txtFltDest");
                            txtFltDest_TextChanged(txtFltDestination, new EventArgs());
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedIndex = -1;
                            DropDownList ddl = (DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum");
                            if (ddl.Items.FindByText(dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString()) != null)
                            {
                                ddl.ClearSelection();
                                ddl.Items.FindByText(dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString()).Selected = true;
                            }
                            else
                            {
                                ddl.Items.Add(dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString());
                                ddl.Items.FindByText(dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString()).Selected = true;


                                if (ddl.Items.FindByText("") != null)
                                {
                                    ddl.Items.FindByText("").Selected = true;
                                }
                                else
                                {
                                    if (ddl.Items.FindByText(" ") != null)
                                    {
                                        ddl.Items.FindByText(" ").Selected = true;
                                    }
                                }
                            }
                            #endregion

                            try
                            {
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString());
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString();

                                //((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString();
                            }
                            catch (Exception ex) { }
                            try
                            {
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(dsRoutDetails.Tables[0].Rows[i]["PartnerType"].ToString());
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["PartnerType"].ToString();

                                //((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString();
                            }
                            catch (Exception ex) { }

                            try
                            {
                                if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text.ToString().Equals("Other", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Visible = false;
                                    ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text = dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString(); ;
                                    ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Visible = true;
                                }
                            }
                            catch (Exception ex) { }
                        }
                        CheckPartnerFlightSupport();
                        Session["dsRoutDetails"] = dsRoutDetails.Copy();
                    }
                    else
                    {
                        LoadGridRoutingDetail();
                    }


                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[3].Rows.Count > 0)
                    {
                        // AWBRateMaster
                        // CommCode Pcs Weight FrIATA FrMKT ValCharge PayMode OcDueCar OcDueAgent SpotRate DynRate ServTax Total
                        DataSet dtRates = new DataSet();
                        dtRates.Tables.Clear();
                        dtRates.Tables.Add(dsResult.Tables[7].Copy());

                        GRDRates.DataSource = dtRates.Copy();
                        GRDRates.DataBind();
                        try
                        {
                            drpCurrency.SelectedValue = dtRates.Tables[0].Rows[0]["CurrencyIndex"].ToString();
                            // drpCurrency.SelectedItem.Text = dtRates.Tables[0].Rows[0]["Currency"].ToString();
                            drpCurrency.Text = dtRates.Tables[0].Rows[0]["Currency"].ToString();
                            string IATAID = "", MKTID = "";
                            IATAID = dtRates.Tables[0].Rows[0]["IATARateID"].ToString();
                            MKTID = dtRates.Tables[0].Rows[0]["MKTRateID"].ToString();
                            if (ddlServiceclass.SelectedItem.Text != "FOC")
                            {
                                if (IATAID.Length > 0 && MKTID.Length > 0 && !(IATAID.Contains(",")) && !(MKTID.Contains(",")))
                                {
                                    if (Convert.ToInt64(IATAID.Replace(",", "")) < 1 && Convert.ToInt64(MKTID.Replace(",", "")) < 1)
                                    {
                                        LBLRouteStatus.Text = "No rates available";
                                        LBLRouteStatus.ForeColor = Color.Red;
                                    }

                                }
                            }
                        }
                        catch (Exception ex) { }
                        for (int i = 0; i < GRDRates.Rows.Count; i++)
                        {
                            //Load Payment Mode dropdown with all available payment mode values.
                            try
                            {
                                DropDownList ddl = (DropDownList)GRDRates.Rows[0].FindControl("ddlPayMode");
                                if (Session["ConBooking_PayModeMaster"] != null)
                                {
                                    DataTable dt = (DataTable)Session["ConBooking_PayModeMaster"];
                                    ddl.Items.Clear();
                                    ddl.DataSource = dt;
                                    ddl.DataTextField = "PayModeText";
                                    ddl.DataValueField = "PayModeCode";
                                    ddl.DataBind();
                                    if (ddl.Items.Count > 1)
                                        ddl.SelectedIndex = 1;
                                }
                                //SetPaymentMode();
                                ((DropDownList)GRDRates.Rows[i].FindControl("ddlPayMode")).SelectedValue = dtRates.Tables[0].Rows[i]["PayMode"].ToString();
                            }
                            catch (Exception)
                            {
                            }
                            try
                            {
                                ((TextBox)GRDRates.Rows[i].FindControl("TXTCurrency")).Text = dtRates.Tables[0].Rows[i]["Currency"].ToString();
                            }
                            catch (Exception ex) { }
                        }

                        Session["dtRates"] = dtRates.Tables[0].Copy();
                    }
                    else
                    {
                        LoadGridRateDetail();
                        LoadCommodityDropdown();
                    }

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
                    dsDetails.Tables[0].Columns.Add("Currency");
                    dsDetails.Tables[0].Columns.Add("ChargeCode");
                    dsDetails.Tables[0].Columns.Add("SerialNUmber");


                    //AWBFrtRateDetails
                    foreach (DataRow row in dsResult.Tables[4].Rows)
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
                    foreach (DataRow row in dsResult.Tables[5].Rows)
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
                        newrow["ChargeCode"] = row["ChargeCode"].ToString().Length > 0 ? row["ChargeCode"] : "N";
                        newrow["SerialNumber"] = row["ChargeSrNo"];
                        dsDetails.Tables[0].Rows.Add(newrow);

                    }

                    Session["OCDetails"] = dsDetails.Copy();

                    PrepareAWBDimensions(AWBNumber, txtAwbPrefix.Text.Trim());

                    txtExecutedAt.Text = "" + dsResult.Tables[0].Rows[0]["ExecutedAt"].ToString();
                    txtExecutedBy.Text = "" + dsResult.Tables[0].Rows[0]["ExecutedBy"].ToString();
                    txtExecutionDate.Text = "" + dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();
                    //Set shipment date and time.
                    if (dsResult.Tables[0].Rows[0]["ShipmentDate"] != null &&
                        dsResult.Tables[0].Rows[0]["ShipmentDate"].ToString() != "")
                    {
                        txtShipmentDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["ShipmentDate"].ToString()).ToString("dd/MM/yyyy");
                        ddlShipmentTime.SelectedValue = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["ShipmentDate"].ToString()).ToString("HH:mm");
                        if (txtShipmentDate.Text == "01/01/1900")
                        {
                            txtShipmentDate.Text = "";
                            ddlShipmentTime.SelectedValue = "25:00";
                        }
                    }
                    else
                    {
                        txtShipmentDate.Text = "";
                        ddlShipmentTime.SelectedValue = "25:00";
                    }
                    //Disable product type dropdown so that user has to open product popup.
                    ddlProductType.Enabled = false;
                    try
                    {
                        //ULD Data
                        if (dsResult != null && dsResult.Tables.Count > 9 && dsResult.Tables[9].Rows.Count > 0)
                            Session["AWBULD"] = dsResult.Tables[9];
                        else
                            Session["AWBULD"] = null;
                    }
                    catch (Exception ex) { }

                    try
                    {
                        //Rate Details

                        if (dsResult != null && dsResult.Tables.Count > 10 && dsResult.Tables[10].Rows.Count > 0)
                        {
                            GrdRateDetails.DataSource = dsResult.Tables[10].Copy();
                            GrdRateDetails.DataBind();
                            Session["FltRoute"] = dsResult.Tables[10].Copy();
                        }
                        else
                            Session["FltRoute"] = null;
                    }
                    catch (Exception ex) { }

                    try
                    {//HAWB Details
                        BALHAWBDetails HAWB = new BALHAWBDetails();
                        DataSet ds = HAWB.GetHAWBDetails(AWBNumber, txtAwbPrefix.Text.Trim());
                        Session["HAWBDetails"] = ds.Tables[0];
                        HAWB = null;
                        ds = null;
                    }
                    catch (Exception ex) { }
                    //EnableDisable(editable);
                    EnableDisable(state);

                    if (blnAcceptance == true && Convert.ToString(Session["AWBStatus"]) == "E")
                    {
                        lblStatus.Text = "AWB is Accepted.";
                        DropDownList ddlPaymentMode = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                        if (ddlPaymentMode.SelectedValue == "PP")
                        {
                            btnCollect.Enabled = true;
                            lblStatus.Text = "AWB is Accepted.";
                        }
                        else
                            btnCollect.Enabled = false;
                    }
                    else if (Convert.ToString(Session["AWBStatus"]) == "E")
                    {
                        DropDownList ddlPaymentMode = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                        if (ddlPaymentMode.SelectedValue == "PP")
                        {
                            btnCollect.Enabled = true;
                            lblStatus.Text = "AWB is Executed. Please collect AWB amount.";
                        }
                        else
                            btnCollect.Enabled = false;
                    }

                    string AWBStatus = lblStatus.Text.Trim();

                    if (AWBStatus == "AWB Booked.")
                    {
                        if (ValidateLoadableULD() == false)
                            lblStatus.Text = lblStatus.Text + " Shipment May not fit in the flight !";
                    }

                    if (AWBStatus == "AWB Booked.")
                    {
                        if (ValidateFlightCapacity() == false)
                            lblStatus.Text = lblStatus.Text + " Flight is getting overloaded !";
                    }
                }
            }
            catch (Exception ex)
            {
                dsResult = null;
                lblStatus.Text = "" + ex.Message;
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        #region Control EnableDisable Click
        public void EnableDisable(bool IsEnabled)
        {
            //            txtAWBNo.Enabled = false;
            txtAWBNo.ReadOnly = true;
            ddlDocType.Enabled = false;
            txtAwbPrefix.Enabled = false;
            ddlAirlineCode.Enabled = false;

            ddlOrg.Enabled = false;

            ddlDest.Enabled = false;
            btnProcess.Enabled = false;

            txtShipmentDate.Enabled = false;
            ddlShipmentTime.Enabled = false;
            ddlProductType.Enabled = false;
            txtSLAC.Enabled = false;
            ddlProductType.Enabled = false;
            imgProductType.Enabled = false;

            ddlServiceclass.Enabled = false;

            //ddlAgtCode.Enabled = false;

            TXTAgentCode.Enabled = false;


            txtAgentName.Enabled = false;

            txtHandling.Enabled = false;

            TXTCustomerCode.Enabled = false;

            TXTDvForCustoms.Enabled = false;
            TXTDvForCarriage.Enabled = false;

            CHKBonded.Enabled = false;
            CHKConsole.Enabled = false;
            CHKExportShipment.Enabled = false;
            CHKAsAggred.Enabled = false;

            chkTBScreened.Enabled = false;

            // AWBShipperConsigneeDetails
            TXTShipper.Enabled = false;
            TXTShipTelephone.Enabled = false;
            TXTShipAddress.Enabled = false;
            ddlShipCountry.Enabled = false;

            TXTConsignee.Enabled = false;
            TXTConTelephone.Enabled = false;
            TXTConAddress.Enabled = false;
            ddlConCountry.Enabled = false;


            // AWBRateMaster           

            btnAddMaterial.Enabled = false;
            btnDeleteMaterial.Enabled = false;

            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
            {
                //grdMaterialDetails.Rows[i].Enabled = false;

                ((CheckBox)grdMaterialDetails.Rows[i].FindControl("CHKSelect")).Enabled = false;
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                {
                    ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Enabled = false;
                }
                else
                {
                    ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Enabled = false;
                }
                ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtMaterialCommDesc")).Enabled = false;
                ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Enabled = false;
                ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Enabled = false;
                ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommVolWt")).Enabled = false;
                ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Enabled = false;
                ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Enabled = false;
                ((ImageButton)grdMaterialDetails.Rows[i].FindControl("btnDimensionsPopup")).Enabled = false;
            }


            //AWBRouteMaster          

            btnAddRouteDetails.Enabled = IsEnabled;
            btnDeleteRouteDetails.Enabled = IsEnabled;
            btnShowFlights.Enabled = IsEnabled;
            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {
                //grdRouting.Rows[i].Enabled = false;
                ((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Enabled = IsEnabled;
                ((CheckBox)grdRouting.Rows[i].FindControl("CHKInterline")).Enabled = IsEnabled;
                ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = IsEnabled;
                ((CheckBox)grdRouting.Rows[i].FindControl("chkFFR")).Enabled = IsEnabled;

                ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).Enabled = IsEnabled;
                ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Enabled = IsEnabled;
            }

            //btnProcess.Enabled = IsEnabled;
            for (int i = 0; i < GRDRates.Rows.Count; i++)
            {
                GRDRates.Rows[i].Cells[0].Enabled = false;
                GRDRates.Rows[i].Cells[1].Enabled = false;
                GRDRates.Rows[i].Cells[2].Enabled = false;
                GRDRates.Rows[i].Cells[3].Enabled = false;
                GRDRates.Rows[i].Cells[4].Enabled = false;

                GRDRates.Rows[i].Cells[5].Enabled = false;
                GRDRates.Rows[i].Cells[6].Enabled = false;
                GRDRates.Rows[i].Cells[7].Enabled = false;
                GRDRates.Rows[i].Cells[8].Enabled = IsEnabled;
                GRDRates.Rows[i].Cells[9].Enabled = IsEnabled;

                //GRDRates.Rows[i].Cells[10].Enabled = false;
                ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueCar")).ReadOnly = true;
                //GRDRates.Rows[i].Cells[11].Enabled = true;
                ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueAgent")).ReadOnly = true;
                ((TextBox)GRDRates.Rows[i].FindControl("TXTServiceTax")).ReadOnly = IsEnabled;
                ((TextBox)GRDRates.Rows[i].FindControl("TXTTotal")).ReadOnly = IsEnabled;

                GRDRates.Rows[i].Cells[12].Enabled = false;
                GRDRates.Rows[i].Cells[13].Enabled = false;

                GRDRates.Rows[i].Cells[14].Enabled = false;
                //GRDRates.Rows[i].Cells[15].Enabled = false;
                //GRDRates.Rows[i].Cells[16].Enabled = false;

            }

            btnSave.Enabled = IsEnabled;
            //btnShowEAWB.Enabled = IsEnabled;
        }

        public void EnableDisable(string State)
        {

            switch (State)
            {

                case "Save":
                    #region Save
                    //                    txtAWBNo.Enabled = false;
                    txtAWBNo.ReadOnly = true;
                    ddlDocType.Enabled = false;
                    txtAwbPrefix.Enabled = false;
                    ddlAirlineCode.Enabled = false;

                    ddlOrg.Enabled = false;

                    ddlDest.Enabled = false;
                    btnProcess.Enabled = false;

                    txtShipmentDate.Enabled = false;
                    ddlShipmentTime.Enabled = false;
                    ddlProductType.Enabled = false;
                    txtSLAC.Enabled = false;
                    ddlProductType.Enabled = false;
                    imgProductType.Enabled = false;

                    ddlServiceclass.Enabled = false;

                    //ddlAgtCode.Enabled = false;

                    TXTAgentCode.Enabled = false;


                    txtAgentName.Enabled = false;
                    txtHandling.Enabled = false;

                    TXTCustomerCode.Enabled = false;

                    TXTDvForCustoms.Enabled = false;
                    TXTDvForCarriage.Enabled = false;

                    CHKBonded.Enabled = false;
                    CHKConsole.Enabled = false;
                    CHKExportShipment.Enabled = false;
                    CHKAsAggred.Enabled = false;
                    chkTBScreened.Enabled = false;


                    // AWBShipperConsigneeDetails
                    TXTShipper.Enabled = false;
                    TXTShipTelephone.Enabled = false;
                    TXTShipAddress.Enabled = false;
                    ddlShipCountry.Enabled = false;

                    TXTConsignee.Enabled = false;
                    TXTConTelephone.Enabled = false;
                    TXTConAddress.Enabled = false;
                    ddlConCountry.Enabled = false;


                    // AWBRateMaster           

                    btnAddMaterial.Enabled = false;
                    btnDeleteMaterial.Enabled = false;

                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        //grdMaterialDetails.Rows[i].Enabled = false;
                        ((CheckBox)grdMaterialDetails.Rows[i].FindControl("CHKSelect")).Enabled = false;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtMaterialCommDesc")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommVolWt")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Enabled = false;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Enabled = false;
                        ((ImageButton)grdMaterialDetails.Rows[i].FindControl("btnDimensionsPopup")).Enabled = false;
                    }


                    //AWBRouteMaster          

                    btnAddRouteDetails.Enabled = false;
                    btnDeleteRouteDetails.Enabled = false;
                    btnShowFlights.Enabled = false;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        //grdRouting.Rows[i].Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKInterline")).Enabled = false;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkFFR")).Enabled = false;

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Enabled = false;
                    }

                    //btnProcess.Enabled = false;
                    for (int i = 0; i < GRDRates.Rows.Count; i++)
                    {
                        GRDRates.Rows[i].Cells[0].Enabled = false;
                        GRDRates.Rows[i].Cells[1].Enabled = false;
                        GRDRates.Rows[i].Cells[2].Enabled = false;
                        GRDRates.Rows[i].Cells[3].Enabled = false;
                        GRDRates.Rows[i].Cells[4].Enabled = false;

                        GRDRates.Rows[i].Cells[5].Enabled = false;
                        GRDRates.Rows[i].Cells[6].Enabled = false;
                        GRDRates.Rows[i].Cells[7].Enabled = false;
                        GRDRates.Rows[i].Cells[8].Enabled = false;
                        GRDRates.Rows[i].Cells[9].Enabled = false;

                        //GRDRates.Rows[i].Cells[10].Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueCar")).ReadOnly = true;
                        //GRDRates.Rows[i].Cells[11].Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueAgent")).ReadOnly = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTServiceTax")).Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTTotal")).Enabled = true;

                        GRDRates.Rows[i].Cells[12].Enabled = false;
                        GRDRates.Rows[i].Cells[13].Enabled = false;

                        GRDRates.Rows[i].Cells[14].Enabled = false;
                        //GRDRates.Rows[i].Cells[15].Enabled = false;
                        //GRDRates.Rows[i].Cells[16].Enabled = false;

                    }

                    btnSave.Enabled = false;
                    //   btnShowEAWB.Enabled = false;

                    #endregion
                    break;

                case "EditTemp":
                    #region Edit
                    //                    txtAWBNo.Enabled = false;
                    txtAWBNo.ReadOnly = false;
                    ddlDocType.Enabled = false;
                    txtAwbPrefix.Enabled = false;
                    ddlAirlineCode.Enabled = false;

                    ddlOrg.Enabled = true;


                    ddlDest.Enabled = true;
                    btnProcess.Enabled = true;
                    txtShipmentDate.Enabled = true;
                    ddlShipmentTime.Enabled = true;
                    ddlProductType.Enabled = true;
                    txtSLAC.Enabled = true;
                    ddlProductType.Enabled = true;
                    imgProductType.Enabled = true;

                    ddlServiceclass.Enabled = true;

                    //ddlAgtCode.Enabled = false;

                    TXTAgentCode.Enabled = true;
                    txtAgentName.Enabled = true;

                    txtHandling.Enabled = true;

                    TXTCustomerCode.Enabled = true;

                    TXTDvForCustoms.Enabled = true;
                    TXTDvForCarriage.Enabled = true;
                    txtShipmentDate.Enabled = true;
                    ddlShipmentTime.Enabled = true;
                    ddlProductType.Enabled = true;
                    txtSLAC.Enabled = true;
                    ddlProductType.Enabled = true;
                    imgProductType.Enabled = true;

                    CHKBonded.Enabled = true;
                    CHKConsole.Enabled = true;
                    CHKExportShipment.Enabled = true;
                    CHKAsAggred.Enabled = true;
                    chkTBScreened.Enabled = true;


                    // AWBShipperConsigneeDetails
                    TXTShipper.Enabled = true;
                    TXTShipTelephone.Enabled = true;
                    TXTShipAddress.Enabled = true;
                    ddlShipCountry.Enabled = true;

                    TXTConsignee.Enabled = true;
                    TXTConTelephone.Enabled = true;
                    TXTConAddress.Enabled = true;
                    ddlConCountry.Enabled = true;


                    // AWBRateMaster           

                    btnAddMaterial.Enabled = false;
                    btnDeleteMaterial.Enabled = false;

                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        //grdMaterialDetails.Rows[i].Enabled = true;

                        ((CheckBox)grdMaterialDetails.Rows[i].FindControl("CHKSelect")).Enabled = true;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtMaterialCommDesc")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommVolWt")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Enabled = true;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Enabled = true;
                        ((ImageButton)grdMaterialDetails.Rows[i].FindControl("btnDimensionsPopup")).Enabled = true;
                    }


                    //AWBRouteMaster          

                    btnAddRouteDetails.Enabled = true;
                    btnDeleteRouteDetails.Enabled = true;
                    btnShowFlights.Enabled = true;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        //grdRouting.Rows[i].Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKInterline")).Enabled = true;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkFFR")).Enabled = true;

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Enabled = true;
                    }

                    btnProcess.Enabled = true;
                    GRDRates.Enabled = true;
                    for (int i = 0; i < GRDRates.Rows.Count; i++)
                    {
                        GRDRates.Rows[i].Cells[0].Enabled = true;
                        GRDRates.Rows[i].Cells[1].Enabled = true;
                        GRDRates.Rows[i].Cells[2].Enabled = true;
                        GRDRates.Rows[i].Cells[3].Enabled = true;
                        GRDRates.Rows[i].Cells[4].Enabled = true;

                        GRDRates.Rows[i].Cells[5].Enabled = true;
                        GRDRates.Rows[i].Cells[6].Enabled = true;
                        GRDRates.Rows[i].Cells[7].Enabled = true;
                        GRDRates.Rows[i].Cells[8].Enabled = true;
                        GRDRates.Rows[i].Cells[9].Enabled = true;

                        //GRDRates.Rows[i].Cells[10].Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueCar")).ReadOnly = true;
                        //GRDRates.Rows[i].Cells[11].Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueAgent")).ReadOnly = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTServiceTax")).Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTTotal")).Enabled = true;

                        GRDRates.Rows[i].Cells[12].Enabled = true;
                        GRDRates.Rows[i].Cells[13].Enabled = true;

                        GRDRates.Rows[i].Cells[14].Enabled = true;
                        //GRDRates.Rows[i].Cells[15].Enabled = false;
                        //GRDRates.Rows[i].Cells[16].Enabled = false;

                    }

                    btnSave.Enabled = true;
                    // btnShowEAWB.Enabled = true;

                    #endregion
                    break;
                case "Edit":
                    #region Edit
                    //                    txtAWBNo.Enabled = false;
                    txtAWBNo.ReadOnly = true;
                    ddlDocType.Enabled = false;
                    txtAwbPrefix.Enabled = false;
                    ddlAirlineCode.Enabled = false;

                    ddlOrg.Enabled = false;


                    ddlDest.Enabled = true;
                    btnProcess.Enabled = true;
                    txtShipmentDate.Enabled = true;
                    ddlShipmentTime.Enabled = true;
                    ddlProductType.Enabled = true;
                    txtSLAC.Enabled = true;
                    ddlProductType.Enabled = true;
                    imgProductType.Enabled = true;

                    ddlServiceclass.Enabled = true;

                    //ddlAgtCode.Enabled = false;

                    TXTAgentCode.Enabled = false;
                    txtAgentName.Enabled = false;

                    txtHandling.Enabled = true;
                    TXTCustomerCode.Enabled = true;

                    TXTDvForCustoms.Enabled = true;
                    TXTDvForCarriage.Enabled = true;
                    txtShipmentDate.Enabled = true;
                    ddlShipmentTime.Enabled = true;
                    ddlProductType.Enabled = true;
                    txtSLAC.Enabled = true;
                    ddlProductType.Enabled = true;
                    imgProductType.Enabled = true;

                    CHKBonded.Enabled = true;
                    CHKConsole.Enabled = true;
                    CHKExportShipment.Enabled = true;
                    CHKAsAggred.Enabled = true;
                    chkTBScreened.Enabled = true;


                    // AWBShipperConsigneeDetails
                    TXTShipper.Enabled = true;
                    TXTShipTelephone.Enabled = true;
                    TXTShipAddress.Enabled = true;
                    ddlShipCountry.Enabled = true;

                    TXTConsignee.Enabled = true;
                    TXTConTelephone.Enabled = true;
                    TXTConAddress.Enabled = true;
                    ddlConCountry.Enabled = true;


                    // AWBRateMaster           

                    btnAddMaterial.Enabled = false;
                    btnDeleteMaterial.Enabled = false;

                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        //grdMaterialDetails.Rows[i].Enabled = true;

                        ((CheckBox)grdMaterialDetails.Rows[i].FindControl("CHKSelect")).Enabled = true;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtMaterialCommDesc")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommVolWt")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Enabled = true;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Enabled = true;
                        ((ImageButton)grdMaterialDetails.Rows[i].FindControl("btnDimensionsPopup")).Enabled = true;
                    }


                    //AWBRouteMaster          

                    btnAddRouteDetails.Enabled = true;
                    btnDeleteRouteDetails.Enabled = true;
                    btnShowFlights.Enabled = true;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        //grdRouting.Rows[i].Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKInterline")).Enabled = true;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkFFR")).Enabled = true;

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Enabled = true;
                    }

                    btnProcess.Enabled = true;
                    for (int i = 0; i < GRDRates.Rows.Count; i++)
                    {
                        GRDRates.Rows[i].Cells[0].Enabled = false;
                        GRDRates.Rows[i].Cells[1].Enabled = false;
                        GRDRates.Rows[i].Cells[2].Enabled = false;
                        GRDRates.Rows[i].Cells[3].Enabled = false;
                        GRDRates.Rows[i].Cells[4].Enabled = false;

                        GRDRates.Rows[i].Cells[5].Enabled = false;
                        GRDRates.Rows[i].Cells[6].Enabled = false;
                        GRDRates.Rows[i].Cells[7].Enabled = false;
                        GRDRates.Rows[i].Cells[8].Enabled = true;
                        GRDRates.Rows[i].Cells[9].Enabled = true;

                        //GRDRates.Rows[i].Cells[10].Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueCar")).ReadOnly = true;
                        //GRDRates.Rows[i].Cells[11].Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueAgent")).ReadOnly = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTServiceTax")).Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTTotal")).Enabled = true;

                        GRDRates.Rows[i].Cells[12].Enabled = false;
                        GRDRates.Rows[i].Cells[13].Enabled = false;

                        GRDRates.Rows[i].Cells[14].Enabled = false;
                        //GRDRates.Rows[i].Cells[15].Enabled = false;
                        //GRDRates.Rows[i].Cells[16].Enabled = false;

                    }

                    btnSave.Enabled = true;
                    // btnShowEAWB.Enabled = true;

                    #endregion
                    break;

                case "EditExecuted":
                    #region EditExecuted
                    //btnSave.Enabled = false;
                    txtAWBNo.ReadOnly = true;
                    ddlDocType.Enabled = false;
                    txtAwbPrefix.Enabled = false;
                    ddlAirlineCode.Enabled = false;

                    ddlOrg.Enabled = false;
                    txtCustoms.Enabled = false;
                    txtEURIN.Enabled = false;
                    txtSpecialHandlingCode.Enabled = false;

                    txtAttchDoc.Enabled = false;
                    ddlProductType.Enabled = false;
                    ddlDest.Enabled = false;
                    btnProcess.Enabled = false;
                    txtShipmentDate.Enabled = false;
                    ddlShipmentTime.Enabled = false;
                    ddlProductType.Enabled = false;
                    txtSLAC.Enabled = false;
                    ddlProductType.Enabled = false;
                    imgProductType.Enabled = false;

                    ddlServiceclass.Enabled = false;
                    //ddlAgtCode.Enabled = false;

                    TXTAgentCode.Enabled = false;
                    txtAgentName.Enabled = false;

                    txtHandling.Enabled = false;
                    TXTCustomerCode.Enabled = false;

                    TXTDvForCustoms.Enabled = false;
                    TXTDvForCarriage.Enabled = false;


                    CHKBonded.Enabled = false;
                    CHKConsole.Enabled = false;
                    CHKExportShipment.Enabled = false;
                    CHKAsAggred.Enabled = false;
                    chkTBScreened.Enabled = false;


                    // AWBShipperConsigneeDetails
                    TXTShipper.Enabled = false;
                    TXTShipTelephone.Enabled = false;
                    TXTShipAddress.Enabled = false;
                    ddlShipCountry.Enabled = false;

                    TXTConsignee.Enabled = false;
                    TXTConTelephone.Enabled = false;
                    TXTConAddress.Enabled = false;
                    ddlConCountry.Enabled = false;


                    // AWBRateMaster           

                    btnAddMaterial.Enabled = false;
                    btnDeleteMaterial.Enabled = false;

                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        //grdMaterialDetails.Rows[i].Enabled = false;
                        ((CheckBox)grdMaterialDetails.Rows[i].FindControl("CHKSelect")).Enabled = false;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtMaterialCommDesc")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommVolWt")).Enabled = false;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Enabled = false;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Enabled = false;
                        ((ImageButton)grdMaterialDetails.Rows[i].FindControl("btnDimensionsPopup")).Enabled = false;
                    }


                    //AWBRouteMaster          

                    btnAddRouteDetails.Enabled = true;
                    btnDeleteRouteDetails.Enabled = true;
                    btnShowFlights.Enabled = true;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        //grdRouting.Rows[i].Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKInterline")).Enabled = false;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkFFR")).Enabled = false;

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Enabled = true;

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Enabled = false;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Enabled = false;


                    }

                    //btnProcess.Enabled = false;
                    for (int i = 0; i < GRDRates.Rows.Count; i++)
                    {

                        GRDRates.Rows[i].Cells[0].Enabled = false;
                        GRDRates.Rows[i].Cells[1].Enabled = false;
                        GRDRates.Rows[i].Cells[2].Enabled = false;
                        GRDRates.Rows[i].Cells[3].Enabled = false;
                        GRDRates.Rows[i].Cells[4].Enabled = false;

                        GRDRates.Rows[i].Cells[5].Enabled = false;
                        GRDRates.Rows[i].Cells[6].Enabled = false;
                        GRDRates.Rows[i].Cells[7].Enabled = false;
                        GRDRates.Rows[i].Cells[8].Enabled = true;
                        GRDRates.Rows[i].Cells[9].Enabled = true;

                        //GRDRates.Rows[i].Cells[10].Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueCar")).Enabled = false;
                        //GRDRates.Rows[i].Cells[11].Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueAgent")).Enabled = false;

                        //list button
                        ((ImageButton)GRDRates.Rows[i].FindControl("btnOcDueCar")).Enabled = false;
                        //list button
                        ((ImageButton)GRDRates.Rows[i].FindControl("btnOcDueAgent")).Enabled = false;


                        ((TextBox)GRDRates.Rows[i].FindControl("TXTServiceTax")).Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTTotal")).Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTRateClass")).Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTCurrency")).Enabled = false;

                        GRDRates.Rows[i].Cells[12].Enabled = false;
                        GRDRates.Rows[i].Cells[13].Enabled = false;

                        GRDRates.Rows[i].Cells[14].Enabled = false;
                        //GRDRates.Rows[i].Cells[15].Enabled = false;
                        //GRDRates.Rows[i].Cells[16].Enabled = false;

                    }

                    btnSave.Enabled = true;
                    //   btnShowEAWB.Enabled = false;

                    #endregion
                    break;

                case "EditReopen":
                    #region EditReopen
                    //                    txtAWBNo.Enabled = false;
                    txtAWBNo.ReadOnly = true;
                    ddlDocType.Enabled = false;
                    txtAwbPrefix.Enabled = false;
                    ddlAirlineCode.Enabled = false;

                    ddlOrg.Enabled = false;

                    //updated on 11/06/2014 Need to freeze Destination & agent
                    ddlDest.Enabled = false;
                    TXTAgentCode.Enabled = false;
                    //ddlAgtCode.Enabled = false;    
                    //ddlServiceclass.Enabled = false;



                    TXTAgentCode.Enabled = false;
                    txtAgentName.Enabled = false;
                    //txtHandling.Enabled = false;
                    TXTCustomerCode.Enabled = false;

                    TXTDvForCustoms.Enabled = false;
                    TXTDvForCarriage.Enabled = false;

                    //CHKBonded.Enabled = false;
                    //CHKConsole.Enabled = false;
                    //CHKExportShipment.Enabled = false;
                    //CHKAsAggred.Enabled = false;
                    chkTBScreened.Enabled = true;


                    // AWBShipperConsigneeDetails
                    //TXTShipper.Enabled = false;
                    //TXTShipTelephone.Enabled = false;
                    //TXTShipAddress.Enabled = false;
                    //ddlShipCountry.Enabled = false;

                    //TXTConsignee.Enabled = false;
                    //TXTConTelephone.Enabled = false;
                    //TXTConAddress.Enabled = false;
                    //ddlConCountry.Enabled = false;


                    // AWBRateMaster           

                    btnAddMaterial.Enabled = false;
                    btnDeleteMaterial.Enabled = false;

                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        //grdMaterialDetails.Rows[i].Enabled = true;
                        ((CheckBox)grdMaterialDetails.Rows[i].FindControl("CHKSelect")).Enabled = true;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtMaterialCommDesc")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommVolWt")).Enabled = true;
                        ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Enabled = true;
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Enabled = true;
                        ((ImageButton)grdMaterialDetails.Rows[i].FindControl("btnDimensionsPopup")).Enabled = true;
                    }


                    //AWBRouteMaster          

                    btnAddRouteDetails.Enabled = true;
                    btnDeleteRouteDetails.Enabled = true;
                    btnShowFlights.Enabled = true;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        //grdRouting.Rows[i].Enabled = false;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("CHKInterline")).Enabled = true;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkFFR")).Enabled = true;

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).Enabled = true;
                        ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtAcceptedWt")).Enabled = true;
                    }

                    btnProcess.Enabled = true;
                    for (int i = 0; i < GRDRates.Rows.Count; i++)
                    {

                        GRDRates.Rows[i].Cells[0].Enabled = false;
                        GRDRates.Rows[i].Cells[1].Enabled = false;
                        GRDRates.Rows[i].Cells[2].Enabled = false;
                        GRDRates.Rows[i].Cells[3].Enabled = false;
                        GRDRates.Rows[i].Cells[4].Enabled = false;

                        GRDRates.Rows[i].Cells[5].Enabled = false;
                        GRDRates.Rows[i].Cells[6].Enabled = false;
                        GRDRates.Rows[i].Cells[7].Enabled = false;
                        GRDRates.Rows[i].Cells[8].Enabled = true;
                        GRDRates.Rows[i].Cells[9].Enabled = true;

                        //GRDRates.Rows[i].Cells[10].Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueCar")).ReadOnly = true;
                        //GRDRates.Rows[i].Cells[11].Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueAgent")).ReadOnly = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTServiceTax")).Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTTotal")).Enabled = true;

                        ((TextBox)GRDRates.Rows[i].FindControl("TXTRateClass")).Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTCurrency")).Enabled = false;


                        GRDRates.Rows[i].Cells[12].Enabled = false;
                        GRDRates.Rows[i].Cells[13].Enabled = false;

                        GRDRates.Rows[i].Cells[14].Enabled = false;
                        //GRDRates.Rows[i].Cells[15].Enabled = false;
                        //GRDRates.Rows[i].Cells[16].Enabled = false;

                    }

                    btnSave.Enabled = true;
                    //  btnShowEAWB.Enabled = false;

                    #endregion
                    break;

            }
        }
        #endregion

        protected void ddlAgtCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlAgtCode.SelectedItem.Text == "Select")
            //    txtAgentName.Text = "";
            //else
            //    txtAgentName.Text = ddlAgtCode.SelectedItem.Value;
            SqlDataAdapter dad = null;
            DataSet ds = null;
            try
            {
                if (TXTAgentCode.Text.Trim().Length > 0)
                {
                    // String For Agents
                    string AgentCodeWithName = TXTAgentCode.Text.Trim();
                    string[] AgentCodeArray = AgentCodeWithName.Split('(');
                    TXTAgentCode.Text = AgentCodeArray[0].ToString().Trim();

                    string[] orgdest = GetOrgDest();

                    string TranAccount = string.Empty, Remarks = string.Empty, TranType = string.Empty;
                    decimal BGAmount = 0, AWBPrevAmt = 0, BankGAmt = 0, ThrValue = 0, CreditPer = 0;
                    bool ValidateBG = false;

                    string con = Global.GetConnectionString();
                    // SqlConnection con = new SqlConnection("connection string");
                    dad = new SqlDataAdapter("SELECT AgentName, Isnull(CurrencyCode,'INR') CurrencyCode, Country from dbo.AgentMaster where AgentCode ='" + TXTAgentCode.Text.Trim() + "'", con);
                    ds = new DataSet();
                    dad.Fill(ds);

                    if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                    {
                        txtAgentName.Text = ds.Tables[0].Rows[0][0].ToString();
                        TXTAgentCode.Text = TXTAgentCode.Text.ToUpper().Trim();

                        //Used for Agent Currency selection
                        //drpCurrency.Items.Clear();
                        //drpCurrency.Items.Add(ds.Tables[0].Rows[0]["CurrencyCode"].ToString());


                        drpCurrency.SelectedIndex = 0;
                        ddlConCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                        ddlShipCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                        //drpCurrency.Enabled = false;

                        //For walk-in customers make payment mode as PP
                        if (TXTAgentCode.Text.Trim().Contains("WALKIN"))
                        {
                            ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Text = "PP";
                            ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Enabled = false;
                        }
                        else
                        {
                            //((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Text = "Select";
                            ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Enabled = true;
                        }

                        //Check for BG Account

                        bool blnAllow = CheckforAgentsBalance(TXTAgentCode.Text.Trim(), 0, "",
                            ref TranAccount, ref BGAmount, ref AWBPrevAmt, ref BankGAmt, ref ThrValue, ref ValidateBG, txtAwbPrefix.Text.Trim(), 1);

                        if (BankGAmt > 0)
                        {
                            CreditPer = (BGAmount / BankGAmt) * 100;

                            if (CreditPer < ThrValue || CreditPer < 0)
                            {
                                rateprocessstatus.ForeColor = Color.Red;
                                rateprocessstatus.Text = "Agent account balance is below Threshold value.";
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                                return;
                            }
                            else
                                rateprocessstatus.Text = "";

                        }
                        //End
                        Session["ACode"] = TXTAgentCode.Text;
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Agent code invalid.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                dad = null;
                lblStatus.Text = "Error :" + ex.Message;
            }
            finally
            {
                dad = null;
                if (ds != null)
                    ds.Dispose();
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {

            string[] orgdest = new ConBooking_GHA().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCodeWithName(string prefixText, int count)
        {
            try
            {
                string[] orgdest = new ConBooking_GHA().GetOrgDest();

                //string con = Global.GetConnectionString();
                //// SqlConnection con = new SqlConnection("connection string"); 
                //SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode + '(' + AgentName + ')' + '$' + CustomerCode from dbo.AgentMaster where (AgentName like '%" + prefixText + "%' or AgentCode like '%" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
                //DataSet ds = new DataSet();
                //dad.Fill(ds);

                DataSet ds = new DataSet();
                AgentBAL bal = new AgentBAL();
                ds = bal.GetAgentList(prefixText, orgdest[0], System.DateTime.Now.ToString("dd/MM/yyyy"));
                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());

                }

                if (ds != null)
                    ds.Dispose();
                bal = null;
                return list.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetShipper(string prefixText, int count)
        {

            string[] orgdest = new ConBooking_GHA().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentName from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetConsignee(string prefixText, int count)
        {
            string[] orgdest = new ConBooking_GHA().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentName from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[1] + "'", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetShipperCode(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AccountCode + '(' +AccountName+ ')' from AccountMaster where (AccountName like '" + prefixText + "%' or AccountCode like '" + prefixText + "%') and ParticipationType in ('Both','Shipper')", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetConsigneeCode(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AccountCode + '(' +AccountName+ ')' from AccountMaster where (AccountName like '" + prefixText + "%' or AccountCode like '" + prefixText + "%') and ParticipationType in ('Both','Consignee')", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetULDs(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            SqlDataAdapter dad = new SqlDataAdapter("SELECT Distinct ULDNumber from tblULDMaster where ULDNumber like '" + prefixText + "%'", con);
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

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                string errormessage = "";
                string strCommodity = ((TextBox)GRDRates.Rows[0].FindControl("TXTCommCode")).Text.Trim();
                string ToEmail = string.Empty;
                string strSubject = strCommodity + " - Email Intimation";
                string strBody = string.Empty;

                //commented by Vijay - 11-10-2014
                //if (ddlServiceclass.SelectedItem.Text.Trim() == "Void")
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Voided AWB can not be executed.";
                //    return;
                //}

                //Validate shipper consignee details based on configured flag.
                if (CommonUtility.ShipperMandatoryDuring != null && CommonUtility.ShipperMandatoryDuring.ToUpper() == "EX")
                {
                    if (TXTShipper.Text.Trim() == "" || TXTShipTelephone.Text.Trim() == "" ||
                        TXTShipAddress.Text.Trim() == "" || ddlShipCountry.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Shipper details.";
                        lblStatus.ForeColor = Color.Red;
                        if (Session["ConBooking_AllowRatesEdit"] != null &&
                        Session["ConBooking_AllowRatesEdit"].ToString() != "" &&
                        Convert.ToBoolean(Session["ConBooking_AllowRatesEdit"]))
                        {
                            btnPopupSave.Visible = true;
                            btnPopupCancel.Visible = true;
                        }
                        btnExecute.Enabled = false;
                        btnSave.Enabled = true;
                        return;
                    }
                }
                if (CommonUtility.ConsigneeMandatoryDuring != null && CommonUtility.ConsigneeMandatoryDuring.ToUpper() == "EX")
                {
                    if (TXTConsignee.Text.Trim() == "" || TXTConTelephone.Text.Trim() == "" ||
                        TXTConAddress.Text.Trim() == "" || ddlConCountry.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Consignee details.";
                        lblStatus.ForeColor = Color.Red;
                        if (Session["ConBooking_AllowRatesEdit"] != null &&
                        Session["ConBooking_AllowRatesEdit"].ToString() != "" &&
                        Convert.ToBoolean(Session["ConBooking_AllowRatesEdit"]))
                        {
                            btnPopupSave.Visible = true;
                            btnPopupCancel.Visible = true;
                        }
                        btnExecute.Enabled = false;
                        btnSave.Enabled = true;
                        return;
                    }
                }
                bool checkbeforereopen = true;
                checkbeforereopen = CheckIfAWBUpdateAllowedForRole();
                string PaymentMode = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;

                //If AWB is Voided - Vijay - 11-10-2014
                string AWBStatus = "";
                if (ddlServiceclass.SelectedItem.Text.Trim() == "Void" || ddlServiceclass.SelectedValue == "0")
                    AWBStatus = "V";
                else
                    AWBStatus = "E";


                if (objBLL.SetAWBStatus(HidAWBNumber.Value.Trim(), AWBStatus, ref errormessage,
                    dtCurrentDate.ToString("dd/MM/yyyy"), strUserName, dtCurrentDate, txtAwbPrefix.Text, checkbeforereopen, PaymentMode, dtCurrentDate))
                {
                    //ObjCustoms.UpdateFRIMessage(txtAwbPrefix.Text.Trim() + txtAWBNo.Text.Trim());
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWB executed successfully!";
                    btnPopupSave.Visible = false;
                    btnPopupCancel.Visible = false;
                    btnExecute.Enabled = false;
                    txtExecutionDate.Enabled = false;
                    btnExecutionDate.Enabled = false;
                    btnReopen.Enabled = true;

                    //added by amit
                    SetLabelValues(false);

                    Session["AWBStatus"] = "E";

                    //bool blnEmail = objBLL.GetEmailFormat(txtAWBNo.Text.Trim(), strCommodity, dtCurrentDate, ref strBody, ref ToEmail);

                    //if (blnEmail)
                    //{
                    //    SendEmail(FromEmail, ToEmail, EmailPassword, strSubject, strBody, false);
                    //}

                    EnableDisable("EditExecuted");

                    //CheckAWBStatus();

                    //Generate WalkIn invoice for Void AWBs on execution (AC-22)
                    GenerateWalkinInvoice();

                    //If AWB is Voided - Vijay - 23-09-2014
                    DropDownList ddlPaymentMode = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                    if (ddlPaymentMode.SelectedValue == "PP")
                    {
                        btnCollect.Enabled = true;
                        lblStatus.Text = "AWB is Executed. Please collect AWB amount.";
                    }
                    else
                        btnCollect.Enabled = false;

                    if (ddlServiceclass.SelectedValue == "0" || ddlServiceclass.SelectedItem.Text.Trim() == "Void")
                    {
                        Session["AWBStatus"] = "V";
                      
                    }

                    #region ACAS PRI Message Automation Deepak(15/04/2014)
                    try
                    {
                        LoginBL objLogin = new LoginBL();
                        //if (Convert.ToBoolean(objLogin.GetMasterConfiguration("ACASAutomation")))
                        if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ACASAutomation")))
                        {
                            for (int i = 0; i < grdRouting.Rows.Count; i++)
                            {
                                string FlightNo = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                                string FlightDate = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;

                                ACASBAL objACAS = new ACASBAL();

                                if (objACAS.ACASFRITriggerPointValidation() == "EX")
                                {

                                    object[] QueryValues = { txtAwbPrefix.Text + "-" + HidAWBNumber.Value.Trim(), FlightNo, FlightDate };
                                    DataSet dsACAS = objACAS.CheckACASAWBAvailability(QueryValues);
                                    if (dsACAS != null)
                                    {
                                        if (dsACAS.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                        {
                                            StringBuilder sbPRI = objACAS.EncodingPRIMessage(QueryValues);

                                            object[] QueryVal = { txtAwbPrefix.Text + "-" + HidAWBNumber.Value.Trim(), 1, FlightNo, FlightDate, sbPRI.ToString().ToUpper() };

                                            if (objACAS.UpdatePRIMessage(QueryVal))
                                            {
                                                if (sbPRI != null)
                                                {
                                                    if (sbPRI.ToString() != "")
                                                    {
                                                        object[] QueryValMail = { "PRI", FlightNo, FlightDate };
                                                        //Getting MailID for PRI Message
                                                        DataSet dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
                                                        string MailID = string.Empty;
                                                        if (dMail != null)
                                                        {
                                                            MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                        }
                                                        cls_BL.addMsgToOutBox("PRI", sbPRI.ToString().ToUpper(), "", MailID);
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception Ex)
                    { }

                    #endregion

                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "" + errormessage;
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Error :" + ex.Message;
            }
        }

        protected void btnReopen_Click(object sender, EventArgs e)
        {
            try
            {
                string errormessage = "";
                bool checkbeforereopen = true;
                checkbeforereopen = CheckIfAWBUpdateAllowedForRole();
                if (checkbeforereopen)
                {
                    if (HidAWBNumber.Value.ToString().Trim() != "")
                    {
                        if (objBLL.CheckforFinalStatus(HidAWBNumber.Value.ToString().Trim(), txtAwbPrefix.Text))
                        {
                            lblStatus.Text = "AWB number is finalized in billing. Modifications can not be done.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }

                    #region Check Manifest

                    if (txtAWBNo.Text.Trim() != "")
                    {
                        System.Text.StringBuilder strFligtDet = new System.Text.StringBuilder();
                        try
                        {
                            for (int ICount = 0; ICount < grdRouting.Rows.Count; ICount++)
                            {
                                string aircargo = ((DropDownList)grdRouting.Rows[ICount].FindControl("ddlPartner")).Text.Trim();
                                strFligtDet.Append("Insert into #tblFltDetails values('");
                                if (aircargo.Equals("other", StringComparison.OrdinalIgnoreCase))
                                {
                                    strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtFlightID")).Text.Trim());
                                }
                                else
                                {
                                    strFligtDet.Append(((DropDownList)grdRouting.Rows[ICount].FindControl("ddlFltNum")).Text.Trim());
                                }

                                strFligtDet.Append("','");
                                strFligtDet.Append(DateTime.ParseExact(((TextBox)grdRouting.Rows[ICount].FindControl("txtFdate")).Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
                                strFligtDet.Append("','");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtFltOrig")).Text.Trim());
                                strFligtDet.Append("','");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtFltDest")).Text.Trim());
                                strFligtDet.Append("',");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtPcs")).Text.Trim());
                                strFligtDet.Append(",");
                                strFligtDet.Append(((TextBox)grdRouting.Rows[ICount].FindControl("txtWt")).Text.Trim());
                                strFligtDet.Append(") ");
                            }
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "ERR:" + ex.Message;
                        }

                        string strResult = new BookingBAL().CheckFlightDetails(txtAWBNo.Text.Trim(), txtAwbPrefix.Text, strFligtDet.ToString());

                        if (strResult.Length > 0)
                        {
                            lblStatus.Text = strResult;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                    }
                    #endregion Check Manifest
                }
                string PaymentMode = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                if (objBLL.SetAWBStatus(HidAWBNumber.Value.Trim(), "R", ref errormessage,
                    dtCurrentDate.ToString("dd/MM/yyyy"), strUserName, dtCurrentDate,
                    txtAwbPrefix.Text, checkbeforereopen, PaymentMode))
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWB reopened successfully!";
                    if (Session["ConBooking_AllowRatesEdit"] != null &&
                        Session["ConBooking_AllowRatesEdit"].ToString() != "" &&
                        Convert.ToBoolean(Session["ConBooking_AllowRatesEdit"]))
                    {
                        btnPopupSave.Visible = true;
                        btnPopupCancel.Visible = true;
                    }
                    btnExecute.Enabled = true;
                    txtExecutionDate.Enabled = true;
                    btnExecutionDate.Enabled = true;
                    btnReopen.Enabled = false;

                    EnableDisable("EditReopen");

                    btnListAgentStock_Click(null, null);
                    //start modify jayant on 12/02/2014
                    if (checkbeforereopen == false && Session["DelExecAWB"].ToString() == "True")
                    {
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        btnDelete.Enabled = false;
                    }
                    //end modify jayant on 12/02/2014
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "" + errormessage;
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Error :" + ex.Message;
            }
        }

        #region Checks If AWB Update is Allowed For Role without Validation
        private bool CheckIfAWBUpdateAllowedForRole()
        {
            bool checkbeforereopen = true;
            try
            {
                //Check if value already loaded in session.
                if (Session["Validate_ReopenAllowedBy"] != null && Session["Validate_ReopenAllowedBy"].ToString() != "")
                {
                    checkbeforereopen = Convert.ToBoolean(Session["Validate_ReopenAllowedBy"].ToString());
                }
                else
                {
                    LoginBL objLoginBAL = new LoginBL();
                    string reopenallowuser = "";
                    //reopenallowuser = objLoginBAL.GetMasterConfiguration("ReopenAllowdedBy");
                    reopenallowuser = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ReopenAllowdedBy");
                    if (reopenallowuser.Length > 0)
                    {
                        if (reopenallowuser.Contains(Session["RoleName"].ToString()) || Session["RoleName"].ToString().Equals(reopenallowuser, StringComparison.OrdinalIgnoreCase))
                        {
                            checkbeforereopen = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            Session["Validate_ReopenAllowedBy"] = checkbeforereopen;
            return (checkbeforereopen);
        }
        #endregion Checks If AWB Update is Allowed For Role without Validation

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string errormessage = "";
                bool checkbeforereopen = true;
                checkbeforereopen = CheckIfAWBUpdateAllowedForRole();
                string PaymentMode = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                if (objBLL.SetAWBStatus(HidAWBNumber.Value.Trim(), "D", ref errormessage,
                    dtCurrentDate.ToString("dd/MM/yyyy"), strUserName, dtCurrentDate, txtAwbPrefix.Text, checkbeforereopen, PaymentMode))
                {
                    btnClear_Click(sender, e);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWB deleted successfully!";
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "" + errormessage;
                }


            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Error :" + ex.Message;
            }
        }

        public string[] GetOrgDest()
        {
            string[] result = { "", "" };
            if (Session["Origin"] != null)
            {
                result[0] = Session["Origin"].ToString();
            }
            if (Session["Destination"] != null)
            {
                result[1] = Session["Destination"].ToString();
            }

            return result;
        }

        public bool SetOrgDest(string org, string dest)
        {
            Session["Org"] = org;
            Session["Dest"] = dest;

            return true;
        }

        protected void txtFdate_TextChanged(object sender, EventArgs e)
        {
            if (HidChangeDate.Value == "N")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message",
                    "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                return;
            }

            if (hdnDateChange.Value == "")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message",
                    "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                return;
            }

            try
            {

                //int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFdate")) == ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender))
                    {
                        hdnDateChange.Value = "";
                        txtFltDest_TextChanged(((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")), e);
                    }
                }
                //GetFlightRouteData(rowindex);
                ////Reload product types based on flight date selection.
                //LoadMatchingProductTypes();
            }
            catch (Exception)
            {
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void btnListAgentStock_Click(object sender, EventArgs e)
        {
            DataSet AWB = null;
            try
            {
                if (txtAWBNo.Text.Trim() != "")
                {
                    object[] AWBStock = new object[3];
                    int i = 0;

                    try
                    {
                        Int64 AWBNo = Convert.ToInt64(txtAWBNo.Text.Trim().Substring(0, 7));
                        int LstDigit = Convert.ToInt32(txtAWBNo.Text.Trim().Substring(7, 1));

                        if ((AWBNo % 7) != LstDigit)
                        {
                            lblStatus.Text = "Please enter valid AWB.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                    catch
                    {
                        lblStatus.Text = "Please enter valid AWB.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        txtAWBNo.Focus();
                        return;
                    }

                    AWBStock.SetValue(Convert.ToInt32(txtAWBNo.Text.Trim()), i);

                    i = i + 1;
                    AWBStock.SetValue(txtAwbPrefix.Text.Trim(), i);

                    i = i + 1;
                    AWBStock.SetValue(ddlOrg.SelectedItem.Text.Trim(), i);

                    AWB = objBLL.GetAWBStock(AWBStock);

                    if (AWB != null && AWB.Tables != null && AWB.Tables.Count > 1 && AWB.Tables[0].Rows.Count > 0)
                    {
                        grdStockAllocation.DataSource = AWB.Tables[0];
                        grdStockAllocation.DataBind();
                        pnlStockDetails.Visible = true;
                        TXTAgentCode.Text = AWB.Tables[1].Rows[0]["AgentCode"].ToString();
                        txtAgentName.Text = AWB.Tables[1].Rows[0]["AgentName"].ToString();
                        HidAWBNumber.Value = txtAWBNo.Text.Trim();
                        lblStatus.Text = "";
                        //Session["IsManual"] = true;
                        HidIsManual.Value = "1";

                        TXTAgentCode.Enabled = false;
                        txtAgentName.Enabled = false;
                        ddlOrg.Enabled = false;
                        SetPaymentMode();

                        string AgentCode = Convert.ToString(Session["ACode"]);

                        //int RoleId = Convert.ToInt32(Session["RoleID"]);

                        if (AgentCode.Trim() != "" && AgentCode.Trim() != TXTAgentCode.Text.Trim())
                        {
                            lblStatus.Text = "AWB is not belonged to current user.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            grdStockAllocation.DataSource = null;
                            grdStockAllocation.DataBind();
                            pnlStockDetails.Visible = false;
                            return;
                        }

                        if (Convert.ToBoolean(AWB.Tables[1].Rows[0]["Exists"]))
                        {
                            Server.Transfer("~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim() + "&GHA=" + Convert.ToString(Session["GHABooking"]));
                        }

                        if (Convert.ToBoolean(AWB.Tables[1].Rows[0]["IsManual"]) == false)
                        {
                            lblStatus.Text = "AWB entered is Electronic AWB.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            grdStockAllocation.DataSource = null;
                            grdStockAllocation.DataBind();
                            pnlStockDetails.Visible = false;
                            return;
                        }

                    }
                    else
                    {
                        lblStatus.Text = "No data found";
                        lblStatus.ForeColor = System.Drawing.Color.Blue;
                        pnlStockDetails.Visible = false;
                        Server.Transfer("~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim());
                    }
                }
                else
                {
                    lblStatus.Text = "Please enter valid AWB Number.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    pnlStockDetails.Visible = false;
                }
            }
            catch (Exception ex)
            {
                AWB = null;
                lblStatus.Text = "Error. Please try again";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                pnlStockDetails.Visible = false;
            }
            finally
            {
                if (AWB != null)
                    AWB.Dispose();
            }
        }

        protected void btnClearAgentStock_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GHA_ConBooking.aspx" + "?GHA=" + Convert.ToString(Session["GHABooking"]));
        }

        protected void btnDgr_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "callexport1();", true);
            }
            catch (Exception ex)
            { }
        }

        protected void txtAccPcsWt_TextChanged(object sender, EventArgs e)
        {
            HidProcessFlag.Value = "1";
        }

        protected void TXTSpotRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Session["SpotRate"] = "true";
                HidProcessFlag.Value = "1";
            }
            catch (Exception ex) { }
        }


        private int GetScheduleId(string Origin, string Destination, string FlightNo)
        {
            int ScheduleId = 0;
            BookingBAL objBooking = new BookingBAL();
            objBooking.GetScheduleId(Origin, Destination, FlightNo, ref ScheduleId);
            return ScheduleId;
        }

        private void PrepareAWBDimensions(string AWBNumber, string AWBPrefix)
        {
            DataSet ds = null;
            BookingBAL objBooking = new BookingBAL();
            try
            {
                ds = objBooking.GetAWBDimensions(AWBNumber, AWBPrefix);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["dsDimesionAll"] = ds;
                    try
                    {
                        Session["IsBulk"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBulk"]);
                    }
                    catch (Exception ex)
                    { }
                }
                else
                    Session["dsDimesionAll"] = null;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    DataSet ddt = new DataSet();
                    DataTable dt = ds.Tables[1].Copy();
                    ddt.Tables.Add(dt);
                    Session["dsPiecesDet"] = ddt;
                    ddt = null;
                    dt = null;
                }
                else
                    CreateAWBPiecesDataSet();

                if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    DataSet ddt = new DataSet();
                    DataTable dt = ds.Tables[1].Copy();
                    ddt.Tables.Add(dt);
                    Session["dsRouteds"] = ddt;
                    ddt = null;
                    dt = null;
                }
                else
                    CreateAWBRoutePiecesds();


            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void ddlServiceclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            HidProcessFlag.Value = "1";

        }
        protected void ddlMaterialCommCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {

                ds = (DataSet)Session["Description"];
                string desc = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode"))).SelectedValue.ToString();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (desc == ds.Tables[0].Rows[i]["CommodityCode"].ToString())
                    {
                        ((TextBox)(grdMaterialDetails.Rows[0].FindControl("txtMaterialCommDesc"))).Text = ds.Tables[0].Rows[i]["Description"].ToString();

                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        private bool CheckforAgentsBalance(string AgentCode, decimal AWBRate, string AWBNo, ref string TranAccount, ref decimal BGAmount
            , ref decimal dcAWBPrevAmount, ref decimal BankGAmt, ref decimal ThrValue, ref bool ValidateBG, string AWBPrefix, decimal CurrencyConversion)
        {
            //decimal dcAWBPrevAmount = 0;            

            dcAWBPrevAmount = objBLL.GetAWBRateAmount(AWBNo, AWBPrefix, CurrencyConversion);

            if (objBLL.GetAccountDetails(AgentCode, dtCurrentDate.Date, ref TranAccount, ref BGAmount, ref BankGAmt, ref ThrValue, ref ValidateBG) != true)
            {
                return false;
            }

            if ((BGAmount + dcAWBPrevAmount - AWBRate) > 0)
                return true;
            else
                return false;
        }

        protected void CHKExportShipment_CheckedChanged(object sender, EventArgs e)
        {
            HidProcessFlag.Value = "1";
        }

        protected void btnShowCapacity_Click(object sender, EventArgs e)
        {
            DataSet dtCapacityDet = new DataSet();
            DataTable dtCapacity = new DataTable();
            try
            {
                dtCapacity.Columns.Add("FltNumber");
                dtCapacity.Columns.Add("Accepted");
                dtCapacity.Columns.Add("Booked");
                dtCapacity.Columns.Add("CargoCapacity");

                BALshowCapacity capacity = new BALshowCapacity();

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (grdRouting.Rows.Count > 0)
                    {
                        DateTime dt = DateTime.ParseExact(((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text, "dd/MM/yyyy", null);
                        //string fltId = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Text.ToString()
                        string fltId = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.ToString();
                        // string abc = ((DropDownList) grdRouting.Rows[i].FindControl("ddlFltNum")).Text;
                        object[] objcap = new object[3];

                        objcap.SetValue(dt, 0);
                        objcap.SetValue(fltId, 1);
                        objcap.SetValue(ddlOrg.Text.Trim(), 2);


                        //DataTable dataCap = capacity.ShowCapacity(objcap);
                        //if (dataCap != null)
                        //{
                        //    if (dataCap.Rows.Count > 0)
                        //    {
                        //g8395
                        //        DataRow dr = dtCapacity.NewRow();
                        //        dr[0] = dataCap.Rows[0][0].ToString();
                        //        dr[1] = dataCap.Rows[0][1].ToString();
                        //        dr[2] = dataCap.Rows[0][2].ToString();
                        //        dr[3] = dataCap.Rows[0][3].ToString();
                        //        dtCapacity.Rows.Add(dr);
                        //    }
                        //}
                        DataSet dataCap = capacity.ShowCapacity(objcap);
                        if (dataCap != null)
                        {
                            if (dataCap.Tables.Count > 0)
                            {
                                if (dataCap.Tables[0].Rows.Count > 0)
                                {
                                    DataRow dr = dtCapacity.NewRow();
                                    dr[0] = dataCap.Tables[0].Rows[0][0].ToString();
                                    dr[1] = dataCap.Tables[0].Rows[0][1].ToString();
                                    dr[2] = dataCap.Tables[0].Rows[0][2].ToString();
                                    dr[3] = dataCap.Tables[0].Rows[0][3].ToString();
                                    dtCapacity.Rows.Add(dr);
                                }
                                if (dataCap.Tables[1].Rows.Count > 0)
                                {
                                    dtCapacityDet.Tables.Add(dataCap.Tables[1].Copy());
                                }
                                else
                                {
                                    dtCapacityDet.Tables.Add(dataCap.Tables[1].Copy());
                                }

                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Please select Flight";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }

                }
                if (dtCapacity.Rows.Count > 0)
                {
                    Session["fltCapacity"] = dtCapacity;
                    Session["fltVarDetails"] = dtCapacityDet;

                    Response.Redirect("showCapacity.aspx");
                }
            }
            catch (Exception ex)
            {
                dtCapacityDet = null;
                dtCapacity = null;
            }
            finally
            {
                if (dtCapacityDet != null)
                    dtCapacityDet.Dispose();
                if (dtCapacity != null)
                    dtCapacity.Dispose();
            }
        }

        private bool SendEmail(string EmailFrom, string EmailTo, string EmailPassword, string EmailSubject, string EmailBody, bool IsHTML)
        {
            EMAILOUT em = new EMAILOUT();
            bool blnSent = em.sendMail(EmailFrom, EmailTo, EmailPassword, EmailSubject, EmailBody, IsHTML);
            em = null;
            return blnSent;
        }

        #region IATA Message OK Click
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string toEmailID = txtEmailID.Text;
                if (toEmailID.Trim() == "" || toEmailID.Length < 1)
                {
                    lblStatus.Text = "Sender Mail ID not availabe.Please configure";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                string MessageType = "";
                try
                {
                    //MessageType = Session["Message"].ToString();
                    MessageType = lblMsg.Text.ToString();
                    if (MessageType.Length > 0 && txtMessageBody.Text.Length > 0)
                    {
                        cls_BL.addMsgToOutBox(MessageType, txtMessageBody.Text.ToString(), "", txtEmailID.Text.ToString());
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

                    }
                }
                catch (Exception ex)
                { }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region btnHAWB_Click
        protected void btnHAWB_Click(object sender, EventArgs e)
        {
            Response.Redirect("HAWBBooking.aspx");
        }
        #endregion

        #region getFFADataOfAWB
        private DataSet getFFADataOfAWB(string AWBNo)
        {
            DataSet dsData = new DataSet();
            try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                string[] paramname = new string[] { "AWBNo" };
                object[] paramvalue = new object[] { AWBNo };
                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar };
                dsData = objSQL.SelectRecords("spGetFFADataAsPerAWBNumber", paramname, paramvalue, paramtype);
            }
            catch (Exception ex)
            { }
            return dsData;
        }
        #endregion

        #region Fill Currency
        //private void FillCurrencyCodes(DropDownList drp, string SelectedCurrency)
        //{
        //    BALCurrency BalCur = new BALCurrency();
        //    DataSet dsCur = null;
        //    try
        //    {
        //        dsCur = BalCur.GetCurrencyCodeList("");
        //        if (dsCur != null && dsCur.Tables.Count > 0)
        //        {
        //            if (dsCur.Tables[0].Rows.Count > 0)
        //            {
        //                drp.Items.Clear();
        //                //drpWWR.Items.Add("Select");
        //                drp.DataSource = dsCur.Tables[0];
        //                drp.DataTextField = "Code";
        //                drp.DataValueField = "ID";
        //                drp.DataBind();
        //                drp.SelectedIndex = drp.Items.IndexOf(drp.Items.FindByText(SelectedCurrency));
        //            }
        //            else
        //            {
        //                drp.Items.Clear();
        //                drp.SelectedIndex = 0;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        BalCur = null;
        //        dsCur = null;
        //    }
        //    finally
        //    {
        //        BalCur = null;
        //        if (dsCur != null)
        //            dsCur.Dispose();

        //    }
        //}
        #endregion

        #region Fill Irregularity Code
        //private void FillIrregularityCode()
        //{
        //    LoginBL Bal = new LoginBL();
        //    DataTable dsIrr = null;

        //    try
        //    {
        //        dsIrr = Bal.LoadSystemMasterDataNew("IC").Tables[0];
        //        if (dsIrr != null && dsIrr.Rows.Count > 0)
        //        {
        //            ddlIrregularityCode.Items.Clear();
        //            //drpWWR.Items.Add("Select");
        //            ddlIrregularityCode.DataSource = dsIrr;
        //            ddlIrregularityCode.DataTextField = "IrregularityCode";
        //            ddlIrregularityCode.DataValueField = "ID";
        //            ddlIrregularityCode.DataBind();
        //            ddlIrregularityCode.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bal = null;
        //        dsIrr = null;
        //    }
        //    finally
        //    {
        //        Bal = null;
        //        if (dsIrr != null)
        //            dsIrr.Dispose();
        //    }
        //}
        #endregion

        #region Load Airline code
        public void LoadAirlineCode(string filter)
        {
            DataSet ds = null;
            try
            {
                //DataSet ds = objBLL.GetPartnerType(chkInterline.Checked);
                ds = objBLL.GetPartnerType(true);

                DropDownList ddl = new DropDownList();
                TextBox txtdest = new TextBox();
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdRouting.Rows[i].FindControl("ddlPartnerType")));
                    txtdest = ((TextBox)(grdRouting.Rows[i].FindControl("txtFltDest")));
                    if (txtdest.Text.Length < 1 || txtdest.Text == "")//(ddl.Text.Equals("SG",StringComparison.OrdinalIgnoreCase)|| ddl.Text=="")
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                //ddl.DataSource = ds;
                                //ddl.DataMember = ds.Tables[0].TableName;
                                //ddl.DataTextField = "PartnerType";
                                //ddl.DataValueField = "PartnerType";
                                //ddl.DataBind();
                                ddl.Items.Clear();
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    ddl.Items.Add(ds.Tables[0].Rows[k][0].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion

        #region ddlPartner_SelectionChange
        protected void ddlPartner_SelectionChange(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {

                    if ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner") == (DropDownList)sender)
                    {
                        DropDownList ddl = (DropDownList)grdRouting.Rows[i].FindControl("ddlPartner");
                        DropDownList drop = (DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum");
                        TextBox txtbox = (TextBox)grdRouting.Rows[i].FindControl("txtFlightID");

                        if (ddl.Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            drop.Visible = false;
                            txtbox.Visible = true;
                        }
                        else
                        {
                            drop.Visible = true;
                            txtbox.Visible = false;
                        }
                        rowindex = i;
                        break;
                    }
                }
                GetFlightRouteData(rowindex);
                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                //((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Text = "";
            }
            catch (Exception ex) { }
        }
        #endregion

        #region GetFlightRouteData
        private void GetFlightRouteData(int rowindex)
        {
            DataSet dsresult = null;
            try
            {
                string strPartnerCode = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                string errormessage = "";
                // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage, strPartnerCode);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    DataSet ds = (DataSet)Session["Flt"];
                    if (ds != null)
                    {
                        string name = "Table" + rowindex.ToString();
                        try
                        {
                            if (ds.Tables.Count > rowindex)
                            {
                                try
                                {
                                    if (ds.Tables[name] != null && ds.Tables[name].Rows.Count > 0)
                                    {
                                        ds.Tables.Remove(name);
                                        DataTable dt = new DataTable();
                                        dt = dsresult.Tables[0].Copy();
                                        dt.TableName = name;
                                        ds.Tables.Add(dt);
                                        ds.AcceptChanges();
                                        Session["Flt"] = ds.Copy();
                                    }
                                }
                                catch (Exception ex) { }

                            }
                            else if (ds.Tables.Count == 1)
                            {
                                Session["Flt"] = dsresult.Copy();
                            }
                        }
                        catch (Exception ex) { }

                    }
                    else
                    {
                        Session["Flt"] = dsresult.Copy();
                    }
                    DataRow row = dsresult.Tables[0].NewRow();
                    if (CommonUtility.FlightValidation)
                    {
                        row["FltNumber"] = "Select";
                        row["ArrTime"] = "Select";
                    }
                    else
                    {
                        row["FltNumber"] = " ";
                        row["ArrTime"] = " ";
                    }

                    dsresult.Tables[0].Rows.Add(row);

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;

                    //HidProcessFlag.Value = "1";
                }
                else
                {
                    LBLRouteStatus.Text = "no record found";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }

            }
            catch (Exception ex)
            {
                dsresult = null;
            }
            finally
            {
                if (dsresult != null)
                    dsresult.Dispose();
            }

            //code flt dest changed
            //BookingBAL.OrgStation = BookingBAL.OrgStation.Trim();

            //((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper().Trim();
            //((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper().Trim();
            //string strPartnerCode = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

            //string errormessage = "";
            ////DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage);
            //DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage, strPartnerCode);

            //Session["Flt"] = dsresult.Copy();

            //if (dsresult != null && dsresult.Tables.Count != 0)
            //{
            //    DataRow row = dsresult.Tables[0].NewRow();
            //    row["FltNumber"] = "Select";
            //    row["ArrTime"] = "Select";

            //    dsresult.Tables[0].Rows.Add(row);

            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();

            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;
            //}
            //else
            //{
            //    LBLRouteStatus.Text = "no record found";
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            //    return;
            //}

        }
        #endregion

        #region txtDest_TextChanged
        protected void txtDest_TextChanged(object sender, EventArgs e)
        {
            if (txtDest.Text.Trim() != "")
            {
                ddlDest.Items.Clear();
                ddlDest.Items.Add(txtDest.Text.Trim());
                ddlDest.SelectedIndex = 0;

                LoadGridRoutingDetail();

                Session["Origin"] = ddlOrg.SelectedItem.Text.Trim();
                Session["Destination"] = ddlDest.SelectedItem.Text.Trim();
                SetOrgDest(ddlOrg.SelectedItem.Text.Trim(), ddlDest.SelectedItem.Text.Trim());
                //                BookingBAL.DestStation = Session["Destination"].ToString();
            }
        }
        #endregion

        #region btnSenfwb_Click
        protected void btnSenfwb_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                string FlightNumber = "";
                string PartnerCode = "";
                string AWBNumber = "", error = "";

                lblMsg.Text = "FWB";
                try
                {
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        FlightNumber = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                        PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                    }
                }
                catch (Exception ex) { }
                BALEmailID ObjEmail = new BALEmailID();
                ds = ObjEmail.GetEmail(ddlOrg.SelectedItem.Text, ddlDest.SelectedItem.Text, "FWB", FlightNumber, PartnerCode);
                try
                {
                    if (ds != null)
                    {
                        txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                        lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                        if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                        {
                            GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                        }
                    }
                    ObjEmail = null;
                }
                catch (Exception ex) { }

                if (HidAWBNumber.Value.ToString().Trim() != "")
                {
                    AWBNumber = HidAWBNumber.Value.ToString().Trim();
                }
                else if (txtAWBNo.Text.Trim() == "")
                {
                    AWBNumber = txtAWBNo.Text.Trim();
                }
                else
                {
                    lblStatus.Text = "Please Provide AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (AWBNumber.Length > 0)
                {
                    if (objBLL.GetAWBDetails(AWBNumber, txtAwbPrefix.Text, ref ds, ref error))
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string Msg = cls_BL.EncodeFWB(ds, ref error);
                                    if (Msg.Length > 3)
                                    {
                                        txtMessageBody.Text = Msg;
                                        Session["Message"] = "FWB";
                                    }
                                    else if (error.Length > 0)
                                    {
                                        lblStatus.Text = "IATA FWB Message Error: " + error;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                lblStatus.Text = "Error on FWB Click" + ex.Message;
                return;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);

            //try
            //{
            //    string AWBNo = "";
            //    AWBNo = HidAWBNumber.Value.ToString().Trim();
            //    if (AWBNo.Length > 0)
            //    {
            //        Session["Message"] = "FWB";
            //        DataSet dsTempData = new DataSet();
            //        string AWBPrefix = txtAwbPrefix.Text.Trim();
            //        string Origin = ddlOrg.Text.Trim();
            //        string Destination = ddlDest.Text.Trim();
            //        string FlightNumber = "";
            //        FlightNumber = ((DropDownList)(grdRouting.Rows[0].FindControl("ddlFltNum"))).SelectedItem.Text.Trim();
            //        //string PartnerCode = "";
            //        string toEmailID = "";
            //        dsTempData = ObjFFR.GetEmail(Origin, Destination, "FWB", "", "");
            //        if (dsTempData != null)
            //        {
            //            if (dsTempData.Tables.Count > 0)
            //            {
            //                if (dsTempData.Tables[0].Rows.Count > 0)
            //                {
            //                    toEmailID = dsTempData.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
            //                    //cls_BL.EncodeFWBForSend(AWBPrefix, AWBNo, FlightNumber, "swapnil@qidtech.com", toEmailID);
            //                    //cls_BL.EncodeFWBForSend(AWBPrefix, AWBNo, FlightNumber, "swapnil@qidtech.com", "sumit@qidtech.com");
            //                    txtEmailID.Text = toEmailID;
            //                    lblMsg.Text = "FWB";
            //                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            //                }
            //            }
            //        }


            //    }
            //}
            //catch (Exception ex)
            //{ }
        }
        #endregion

        #region HAWB SECTION

        #region btnHABDetails_Click
        protected void btnHABDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Session["HAWBDetails"] = null;
                string MAWBNo = HidAWBNumber.Value;
                lblMAWB.Text = MAWBNo;
                lblUOMHAWB.Text = txtUOM.Text.Trim();

                //int TotalHAWBPcs = int.Parse(((TextBox)(GVArrDet.Rows[index].FindControl("RcvPcs"))).Text.ToString()) + int.Parse(((TextBox)(GVArrDet.Rows[index].FindControl("txtArrivedPcs"))).Text.ToString());
                //int TotalHAWBWt = int.Parse(((TextBox)(GVArrDet.Rows[index].FindControl("RcvWt"))).Text.ToString()) + int.Parse(((TextBox)(GVArrDet.Rows[index].FindControl("txtArrivedWt"))).Text.ToString());


                lblMAWBTotWt.Text = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text.Trim();
                lblMAWBTotPcs.Text = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.Trim();

                // lblMAWBTotPcs.Text = HidPcsCount.Value;
                //lblMAWBTotWt.Text = HidWt.Value;

                int MAWBTotPcs;
                decimal MAWBTotWt;

                if (!int.TryParse(lblMAWBTotPcs.Text, out MAWBTotPcs))
                {
                    lblStatus.Text = "Total Pieces Are Not In Correct Format(Shipment Details)";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                    return;
                }

                if (!decimal.TryParse(lblMAWBTotWt.Text, out MAWBTotWt))
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Total Wt Are Not In Correct Format(Shipment Details)";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                    return;
                }

                Session["MAWBNo"] = MAWBNo;

                BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
                DataSet ds = new DataSet();

                //ds = HAWB.GetHAWBDetails(Session["MAWBNo"].ToString(), txtAwbPrefix.Text);

                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 0)
                //    {
                //        if (ds.Tables[0].Rows.Count >= 0)
                //        {
                //            Session["HAWBDetails"] = ds.Tables[0];
                //            Refresh_gvH();
                //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                //            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                //        }
                //    }
                //}

                if (Session["HAWBDetails"] == null)
                    ds = HAWB.GetHAWBDetails(MAWBNo, txtAwbPrefix.Text);//(Session["MAWBNo"].ToString());
                else
                {
                    ds = new DataSet();
                    ds.Tables.Add(((DataTable)Session["HAWBDetails"]).Copy());
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 0)
                {
                    Session["HAWBDetails"] = ds.Tables[0];
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    Refresh_gvH();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "callShow1();", true);
                }
                else
                {
                    Refresh_gvH();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                }




            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error In HAWB";
            }

            finally
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message2", "<SCRIPT LANGUAGE='javascript'>callLoadingClose();</script>", false);
            }


        }
        #endregion btnHABDetails_Click

        #region Button Add HAWB click
        protected void btnADDHAWB_Click(object sender, EventArgs e)
        {
            try
            {
                Label4.Text = string.Empty;
                if (CheckDuplicateHAWBNo(gvHAWBDetails))
                {
                    Label4.Text = "Duplicate HAWB Number Present in details.Please enter valid HAWB Number.";
                    Label4.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    return;
                }


                float SumWt = 0;
                int SumPcs = 0;                   // For tracking the Sum
                int parsePcs = 0;
                float parseWt = 0;

                for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                {
                    if (!int.TryParse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text, out parsePcs))
                    {
                        Label4.Text = "HAWB Pcs are not in correct format at row no: " + i + " !!";
                        Label4.ForeColor = Color.Blue;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                        return;
                    }
                    if (!float.TryParse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text, out parseWt))
                    {
                        Label4.Text = "HAWB Wt is not in correct format at row no: " + i + " !!";
                        Label4.ForeColor = Color.Blue;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                        return;
                    }
                    if (float.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text) <= 0 || int.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text) <= 0)
                    {
                        Label4.Text = "HAWB Pcs/Wt cannot be less than or equal to zero!";
                        Label4.ForeColor = Color.Blue;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                        return;
                    }

                    int HAWBPcs = int.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text);
                    float HAWBWt = float.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text);
                    SumPcs = SumPcs + HAWBPcs;
                    SumWt = SumWt + HAWBWt;
                }
                if (SumPcs > int.Parse(lblMAWBTotPcs.Text))
                {
                    Label4.Text = "The Given No. of Pieces Exceeds Main AWB Total No. of Pieces !!";
                    Label4.ForeColor = Color.Blue;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    return;
                }
                if (SumWt > float.Parse(lblMAWBTotWt.Text))
                {
                    Label4.Text = "The Given Weight Exceeds Main AWB Total Weight !!";
                    Label4.ForeColor = Color.Blue;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message2", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    return;
                }
                if (!ValidateHAWBSHPCONInformation())
                {
                    Label4.Text = "Please enter mandatory Shipper/Consignee details marked with (*).";
                    Label4.ForeColor = Color.Blue;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    return;
                }
                #region Commented code for Implementing new HAWB Functionality

                //int SumWt = 0, SumPcs = 0;                   // For tracking the Sum
                //TextBox textHAWBNo = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtHAWBNo"));
                //TextBox textHAWBPcs = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtHAWBPcs"));
                //TextBox textHAWBWt = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtHAWBWt"));
                //TextBox textCustID = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCustID"));
                //TextBox textCustName = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCustName"));
                //TextBox textCustAddress = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCustAddress"));
                //TextBox textCity = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCity"));
                //TextBox textZipcode = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtZipcode"));

                //for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                //{
                //    int HAWBPcs = int.Parse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text);
                //    int HAWBWt = int.Parse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text);
                //    SumPcs = SumPcs + HAWBPcs;
                //    SumWt = SumWt + HAWBWt;
                //}
                //if (SumPcs + int.Parse(textHAWBPcs.Text) > int.Parse(lblMAWBTotPcs.Text))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidateHAWBPcs();</script>", false);
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                //    return;
                //}
                //if (SumWt + int.Parse(textHAWBWt.Text) > int.Parse(lblMAWBTotWt.Text))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidateHAWBWt();</script>", false);
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message2", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                //    return;
                //}

                //((DataTable)Session["HAWBDetails"]).Rows.Add(Session["MAWBNo"].ToString(), textHAWBNo.Text, textHAWBPcs.Text, textHAWBWt.Text, textCustID.Text, textCustName.Text, textCustAddress.Text, textCity.Text, textZipcode.Text);
                //Refresh_gvH();
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                #endregion
                for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                {
                    if (((RadioButton)gvHAWBDetails.Rows[i].FindControl("check")).Checked)
                    {
                        string HAWBNo = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBNo")).Text.Trim();
                        string HAWBPcs = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text.Trim();
                        string HAWBWt = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text.Trim();
                        string Description = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblDescription")).Text.Trim();
                        string Shipper = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblCustName")).Text.Trim();
                        string Consignee = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblConsigneeName")).Text.Trim();
                        string Origin = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblOrigin")).Text.Trim();
                        string Destination = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblDestination")).Text.Trim();
                        string SHC = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblSHC")).Text.Trim();
                        string SLAC = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblSLAC")).Text.Trim();
                        int slaC = 0;


                        ((DataTable)Session["HAWBDetails"]).Rows[i].ItemArray = new object[]{
                               Session["MAWBNo"].ToString(), HAWBNo, HAWBPcs, HAWBWt, Description, Shipper, Consignee, Origin, Destination, txtShipperNameHAWB.Text,
                               txtShipperTelephoneHAWB.Text, txtShipperAddressHAWB.Text, txtShipperCityHAWB.Text, txtShipperStateHAWB.Text, ddlShipperCountryHAWB.Text, txtShipperPinCodeHAWB.Text, txtShipperEmailHAWB.Text, txtConsigneeNameHAWB.Text, txtConsigneeTelephoneHAWB.Text, txtConsigneeAddressHAWB.Text,
                               txtConsigneeCityHAWB.Text, txtConsigneeStateHAWB.Text, ddlConsigneeCountryHAWB.Text, txtConsigneePincodeHAWB.Text, txtConsigneeEmailHAWB.Text, string.Empty, string.Empty, string.Empty,string.Empty, SHC, int.TryParse(SLAC, out slaC) == true ? slaC : 0
                    };
                    }

                }
                ((DataTable)Session["HAWBDetails"]).Rows.Add(
                    Session["MAWBNo"].ToString(), string.Empty, "0", "0", string.Empty, string.Empty, string.Empty, ddlOrg.Text, ddlDest.Text, string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0);
                //Refresh_gvH();
                gvHAWBDetails.DataSource = ((DataTable)Session["HAWBDetails"]);
                gvHAWBDetails.DataBind();

                ((RadioButton)gvHAWBDetails.Rows[gvHAWBDetails.Rows.Count - 1].FindControl("check")).Checked = true;
                txtShipperNameHAWB.Text = string.Empty;
                txtShipperAddressHAWB.Text = string.Empty;
                txtShipperCityHAWB.Text = string.Empty;
                txtShipperStateHAWB.Text = string.Empty;
                ddlShipperCountryHAWB.SelectedIndex = 0;
                txtShipperPinCodeHAWB.Text = string.Empty;
                txtShipperEmailHAWB.Text = string.Empty;
                txtShipperTelephoneHAWB.Text = string.Empty;

                txtConsigneeNameHAWB.Text = string.Empty;
                txtConsigneeAddressHAWB.Text = string.Empty;
                txtConsigneeCityHAWB.Text = string.Empty;
                txtConsigneeStateHAWB.Text = string.Empty;
                ddlConsigneeCountryHAWB.SelectedIndex = 0;
                txtConsigneePincodeHAWB.Text = string.Empty;
                txtConsigneeEmailHAWB.Text = string.Empty;
                txtConsigneeTelephoneHAWB.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);


            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }
        #endregion

        #region SaveHAWBButton click
        protected void btnSaveHAWB_Click(object sender, EventArgs e)
        {
            try
            {
                Label4.Text = string.Empty;
                if (CheckDuplicateHAWBNo(gvHAWBDetails))
                {
                    Label4.Text = "Duplicate HAWB Number Present in details.Please enter valid HAWB Number.";
                    Label4.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    return;
                }


                float SumWt = 0;
                int SumPcs = 0;                   // For tracking the Sum
                int parsePcs = 0;
                float parseWt = 0;

                for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                {
                    if (!int.TryParse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text, out parsePcs))
                    {
                        Label4.Text = "HAWB Pcs are not in correct format at row no: " + i + " !!";
                        Label4.ForeColor = Color.Blue;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                        return;
                    }
                    if (!float.TryParse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text, out parseWt))
                    {
                        Label4.Text = "HAWB Wt is not in correct format at row no: " + i + " !!";
                        Label4.ForeColor = Color.Blue;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                        return;
                    }

                    if (float.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text) <= 0 || int.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text) <= 0)
                    {
                        Label4.Text = "HAWB Pcs/Wt cannot be less than or equal to zero!";
                        Label4.ForeColor = Color.Blue;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                        return;
                    }


                    int HAWBPcs = int.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text);
                    float HAWBWt = float.Parse(((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text);
                    SumPcs = SumPcs + HAWBPcs;
                    SumWt = SumWt + HAWBWt;
                }
                if (SumPcs > int.Parse(lblMAWBTotPcs.Text))
                {
                    Label4.Text = "The Given No. of Pieces Exceeds Main AWB Total No. of Pieces !!";
                    Label4.ForeColor = Color.Blue;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    return;
                }
                if (SumWt > float.Parse(lblMAWBTotWt.Text))
                {
                    Label4.Text = "The Given Weight Exceeds Main AWB Total Weight !!";
                    Label4.ForeColor = Color.Blue;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message2", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    return;
                }
                if (gvHAWBDetails.Rows.Count > 0)
                {
                    if (!ValidateHAWBSHPCONInformation())
                    {
                        Label4.Text = "Please enter mandatory Shipper/Consignee details marked with (*).";
                        Label4.ForeColor = Color.Blue;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        return;
                    }
                }
                #region Comment Old HAWB Code
                //BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
                //if (HAWB.Delete_All_MAWB_HAWB(Session["MAWBNo"].ToString()))      // First Delete all the entries and then Add all the Gridview Entries
                //{

                //    for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                //    {

                //        string HAWBNo = ((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBNo")).Text;
                //        int HAWBPcs = int.Parse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text);
                //        int HAWBWt = int.Parse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text);
                //        string CustID = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCustID")).Text;
                //        string CustName = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCustName")).Text;
                //        string CustAddress = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCustAddress")).Text;
                //        string City = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCity")).Text;
                //        string Zipcode = ((Label)gvHAWBDetails.Rows[i].FindControl("lblZipcode")).Text;
                //        if (HAWBNo == "DUMMY" && gvHAWBDetails.Rows.Count == 1)
                //        {

                //        }
                //        else
                //        {
                //            HAWB.PutHAWBDetails(Session["MAWBNo"].ToString(), HAWBNo, HAWBPcs, HAWBWt, CustID, CustName, CustAddress, City, Zipcode);
                //        }
                //    }
                //    lblStatus.Text = "HAWB Details Saved Successfully!";
                //}
                //Refresh_gvH();
                #endregion

                for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                {
                    if (((RadioButton)gvHAWBDetails.Rows[i].FindControl("check")).Checked)
                    {
                        string HAWBNo = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBNo")).Text.Trim();
                        string HAWBPcs = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text.Trim();
                        string HAWBWt = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text.Trim();
                        string Description = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblDescription")).Text.Trim();
                        string Shipper = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblCustName")).Text.Trim();
                        string Consignee = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblConsigneeName")).Text.Trim();
                        string Origin = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblOrigin")).Text.Trim();
                        string Destination = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblDestination")).Text.Trim();
                        string SHC = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblSHC")).Text.Trim();
                        string SLAC = ((TextBox)gvHAWBDetails.Rows[i].FindControl("lblSLAC")).Text.Trim();
                        int slaC = 0;


                        ((DataTable)Session["HAWBDetails"]).Rows[i].ItemArray = new object[]{
                               Session["MAWBNo"].ToString(), HAWBNo, HAWBPcs, HAWBWt, Description, Shipper, Consignee, Origin, Destination, txtShipperNameHAWB.Text,
                               txtShipperTelephoneHAWB.Text, txtShipperAddressHAWB.Text, txtShipperCityHAWB.Text, txtShipperStateHAWB.Text, ddlShipperCountryHAWB.Text, txtShipperPinCodeHAWB.Text, txtShipperEmailHAWB.Text, txtConsigneeNameHAWB.Text, txtConsigneeTelephoneHAWB.Text, txtConsigneeAddressHAWB.Text,
                               txtConsigneeCityHAWB.Text, txtConsigneeStateHAWB.Text, ddlConsigneeCountryHAWB.Text, txtConsigneePincodeHAWB.Text, txtConsigneeEmailHAWB.Text, string.Empty, string.Empty, string.Empty,string.Empty, SHC, int.TryParse(SLAC, out slaC) == true ? slaC : 0
                    };
                    }

                }
            }
            catch (Exception Ex)
            {
                lblStatus.Text = Ex.Message;
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
            }
        }
        #endregion

        #region btnDeleteHAWB click
        protected void btnDeleteAWB_Click(object sender, EventArgs e)
        {
            try
            {
                //string HAWBNoTest = ((Label)gvHAWBDetails.Rows[0].FindControl("lblHAWBNo")).Text;
                //if (HAWBNoTest == "DUMMY" && gvHAWBDetails.Rows.Count == 1)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                //    return;
                //}
                DataTable dt = (DataTable)(Session["HAWBDetails"]);
                for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                {
                    if (((RadioButton)(gvHAWBDetails.Rows[i].FindControl("check"))).Checked)
                    {
                        DataRow dr = dt.Rows[i];
                        dt.Rows.Remove(dr);
                    }
                }
                (Session["HAWBDetails"]) = dt;
                gvHAWBDetails.DataSource = dt;
                gvHAWBDetails.DataBind();
                if (gvHAWBDetails.Rows.Count > 0)
                {
                    ((RadioButton)gvHAWBDetails.Rows[0].FindControl("check")).Checked = true;
                    txtShipperNameHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperName")).Value;
                    txtShipperAddressHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperAddress")).Value;
                    txtShipperCityHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperCity")).Value;
                    txtShipperStateHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperState")).Value;
                    ddlShipperCountryHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperCountry")).Value;
                    txtShipperPinCodeHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperPinCode")).Value;
                    txtShipperEmailHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperEmail")).Value;
                    txtShipperTelephoneHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperTelephone")).Value;

                    txtConsigneeNameHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeName")).Value;
                    txtConsigneeAddressHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeAddress")).Value;
                    txtConsigneeCityHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeCity")).Value;
                    txtConsigneeStateHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeState")).Value;
                    ddlConsigneeCountryHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeCountry")).Value;
                    txtConsigneePincodeHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneePinCode")).Value;
                    txtConsigneeEmailHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeEmail")).Value;
                    txtConsigneeTelephoneHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeTelephone")).Value;
                }
                else
                {
                    txtShipperNameHAWB.Text = string.Empty;
                    txtShipperAddressHAWB.Text = string.Empty;
                    txtShipperCityHAWB.Text = string.Empty;
                    txtShipperStateHAWB.Text = string.Empty;
                    ddlShipperCountryHAWB.SelectedIndex = 0;
                    txtShipperPinCodeHAWB.Text = string.Empty;
                    txtShipperEmailHAWB.Text = string.Empty;
                    txtShipperTelephoneHAWB.Text = string.Empty;

                    txtConsigneeNameHAWB.Text = string.Empty;
                    txtConsigneeAddressHAWB.Text = string.Empty;
                    txtConsigneeCityHAWB.Text = string.Empty;
                    txtConsigneeStateHAWB.Text = string.Empty;
                    ddlConsigneeCountryHAWB.SelectedIndex = 0;
                    txtConsigneePincodeHAWB.Text = string.Empty;
                    txtConsigneeEmailHAWB.Text = string.Empty;
                    txtConsigneeTelephoneHAWB.Text = string.Empty;
                }
                //Refresh_gvH();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
            }
            catch (Exception Ex)
            {
                lblStatus.Text = Ex.Message;
            }
        }
        #endregion

        #region Refresh Gridview
        protected void Refresh_gvH()
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)(Session["HAWBDetails"]);
                if (dt.Rows.Count == 0)
                {

                    dt.Rows.Add(string.Empty, string.Empty, 0, 0, "",
    string.Empty,
    string.Empty, ddlOrg.Text, ddlDest.Text, string.Empty, string.Empty,
    string.Empty,
    string.Empty,
    string.Empty, "", "", "", "", string.Empty, string.Empty,
    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0);
                    gvHAWBDetails.DataSource = dt;
                    gvHAWBDetails.DataBind();

                    ((RadioButton)gvHAWBDetails.Rows[0].FindControl("check")).Checked = true;
                    txtShipperNameHAWB.Text = string.Empty;
                    txtShipperAddressHAWB.Text = string.Empty;
                    txtShipperCityHAWB.Text = string.Empty;
                    txtShipperStateHAWB.Text = string.Empty;
                    ddlShipperCountryHAWB.SelectedIndex = 0;
                    txtShipperPinCodeHAWB.Text = string.Empty;
                    txtShipperEmailHAWB.Text = string.Empty;
                    txtShipperTelephoneHAWB.Text = string.Empty;

                    txtConsigneeNameHAWB.Text = string.Empty;
                    txtConsigneeAddressHAWB.Text = string.Empty;
                    txtConsigneeCityHAWB.Text = string.Empty;
                    txtConsigneeStateHAWB.Text = string.Empty;
                    ddlConsigneeCountryHAWB.SelectedIndex = 0;
                    txtConsigneePincodeHAWB.Text = string.Empty;
                    txtConsigneeEmailHAWB.Text = string.Empty;
                    txtConsigneeTelephoneHAWB.Text = string.Empty;

                }
                else if (dt.Rows.Count > 0)
                {

                    gvHAWBDetails.DataSource = dt;
                    gvHAWBDetails.DataBind();
                    ((RadioButton)gvHAWBDetails.Rows[0].FindControl("check")).Checked = true;
                    txtShipperNameHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperName")).Value;
                    txtShipperAddressHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperAddress")).Value;
                    txtShipperCityHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperCity")).Value;
                    txtShipperStateHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperState")).Value;
                    ddlShipperCountryHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperCountry")).Value;
                    txtShipperPinCodeHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperPinCode")).Value;
                    txtShipperEmailHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperEmail")).Value;
                    txtShipperTelephoneHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdShipperTelephone")).Value;

                    txtConsigneeNameHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeName")).Value;
                    txtConsigneeAddressHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeAddress")).Value;
                    txtConsigneeCityHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeCity")).Value;
                    txtConsigneeStateHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeState")).Value;
                    ddlConsigneeCountryHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeCountry")).Value;
                    txtConsigneePincodeHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneePinCode")).Value;
                    txtConsigneeEmailHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeEmail")).Value;
                    txtConsigneeTelephoneHAWB.Text = ((HiddenField)gvHAWBDetails.Rows[0].FindControl("hdConsigneeTelephone")).Value;



                }





            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }
        #endregion

        private bool SaveHAWBInformation()
        {
            BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
            DataTable dt = (DataTable)Session["HAWBDetails"];
            try
            {


                if (dt != null)
                {
                    HAWB.Delete_All_MAWB_HAWB(HidAWBNumber.Value, txtAwbPrefix.Text.Trim());

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                        {

                            if (dt.Rows[intCount]["HAWBNo"].ToString() != string.Empty && dt.Rows[intCount]["ConsigneeCountry"].ToString() != string.Empty && dt.Rows[intCount]["CustCountry"].ToString() != string.Empty)
                            {

                                HAWB.PutHAWBDetails(HidAWBNumber.Value, dt.Rows[intCount]["HAWBNo"].ToString(), Convert.ToInt16(dt.Rows[intCount]["HAWBPcs"]), Convert.ToInt16(dt.Rows[intCount]["HAWBWt"]),
                                    dt.Rows[intCount]["Description"].ToString(), dt.Rows[intCount]["CustID"].ToString(), dt.Rows[intCount]["CustName"].ToString(), dt.Rows[intCount]["CustAddress"].ToString(),
                                    dt.Rows[intCount]["CustCity"].ToString(), dt.Rows[intCount]["Zipcode"].ToString(), dt.Rows[intCount]["Origin"].ToString(), dt.Rows[intCount]["Destination"].ToString(),
                                    dt.Rows[intCount]["SHC"].ToString(), dt.Rows[intCount]["HAWBPrefix"].ToString(),
                                    txtAwbPrefix.Text, "", "", "", "", "", dt.Rows[intCount]["ConsigneeName"].ToString(), dt.Rows[intCount]["ConsigneeAddress"].ToString(), dt.Rows[intCount]["ConsigneeCity"].ToString(),
                                    dt.Rows[intCount]["ConsigneeState"].ToString(), dt.Rows[intCount]["ConsigneeCountry"].ToString(), dt.Rows[intCount]["ConsigneePostalCode"].ToString(), dt.Rows[intCount]["CustState"].ToString(),
                                    dt.Rows[intCount]["CustCountry"].ToString(), dt.Rows[intCount]["UOM"].ToString(), dt.Rows[intCount]["SLAC"].ToString(),
                                    dt.Rows[intCount]["ConsigneeID"].ToString(), dt.Rows[intCount]["ShipperEmail"].ToString(), dt.Rows[intCount]["ShipperTelephone"].ToString(), dt.Rows[intCount]["ConsigneeEmail"].ToString(), dt.Rows[intCount]["ConsigneeTelephone"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
            return true;
        }

        public bool ValidateHAWBSHPCONInformation()
        {
            try
            {
                if (txtShipperNameHAWB.Text == string.Empty || txtConsigneeNameHAWB.Text == string.Empty || txtShipperTelephoneHAWB.Text == string.Empty ||
                    txtConsigneeTelephoneHAWB.Text == string.Empty || txtShipperAddressHAWB.Text == string.Empty || txtConsigneeAddressHAWB.Text == string.Empty)
                { return false; }
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        public bool CheckDuplicateHAWBNo(GridView ds)
        {
            bool flag = false;
            try
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    for (int j = (i + 1); j < ds.Rows.Count; j++)
                    {

                        if (((TextBox)ds.Rows[i].FindControl("lblHAWBNo")).Text.Length > 0 && ((TextBox)ds.Rows[j].FindControl("lblHAWBNo")).Text.Length > 0)
                        {
                            if (((TextBox)ds.Rows[i].FindControl("lblHAWBNo")).Text.Equals(((TextBox)ds.Rows[j].FindControl("lblHAWBNo")).Text, StringComparison.OrdinalIgnoreCase))
                            {
                                flag = true;
                                i = ds.Rows.Count;
                                j = ds.Rows.Count;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                flag = true;
            }
            return flag;
        }

        #region btnHAWBCancel_Click
        protected void btnHAWBCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Session["HAWBDetails"] = null;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose1();</script>", false);
            }
            catch (Exception Ex)
            {
                lblStatus.Text = Ex.Message;
            }
        }
        #endregion

        #endregion

        //Code Added on 21/05/2013
        //Performance updation for Process Rate Click
        #region LoadFlightRouteDetails
        public void LoadFlightRouteDetails()
        {
            DataTable myDataTable = new DataTable("DT");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();
            try
            {
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltOrigin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltDestination";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltNumber";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Carrier";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Pcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "GWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IsPrime";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Freight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FrtTax";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FreightTax";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RatePerKg";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Proration";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "isHeavy";
                myDataTable.Columns.Add(myDataColumn);

                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "isHeavy";
                //myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Discount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AgentComm";
                myDataTable.Columns.Add(myDataColumn);
                ///added on 07/05/2014      
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Cost";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "MKTFrt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "MKTTax";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CostType";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Currency";
                myDataTable.Columns.Add(myDataColumn);

                Session["FltRoute"] = myDataTable;
            }
            catch (Exception ex)
            {
                myDataTable = null;
                Ds = null;
            }
            finally
            {
                if (myDataTable != null)
                    myDataTable.Dispose();
                if (Ds != null)
                    Ds.Dispose();
            }
        }
        #endregion

        #region LoadAWBStatusDropdown
        public void LoadAWBStatusDropdown()
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)Session["StatusMaster"];
                DropDownList ddl = null;

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdRouting.Rows[i].FindControl("ddlStatus")));

                    if (dt != null)
                    {
                        ddl.DataSource = dt;
                        ddl.DataMember = dt.TableName;
                        ddl.DataTextField = "Status";
                        ddl.DataValueField = "StatusCode";
                        ddl.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }
        #endregion

        #region button AddULD Click Operation
        protected void btnAddUld_Click(object sender, EventArgs e)
        {
            try
            {
                string AWBNo = "", Origin, Destination, FlightNumber;
                string FlightDate;
                int Totpcs, PcsToAssign;
                Decimal TotWgt, WgtToAssign;
                if (HidAWBNumber.Value != null)
                {
                    AWBNo = HidAWBNumber.Value;
                }
                else if (txtAWBNo.Text != "")
                {
                    AWBNo = txtAWBNo.Text.Trim();
                }
                Totpcs = Convert.ToInt32(((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text);
                TotWgt = Convert.ToDecimal(((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text);
                PcsToAssign = Convert.ToInt32(((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text);
                WgtToAssign = Convert.ToDecimal(((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text);
                Origin = ((TextBox)(grdRouting.Rows[0].FindControl("txtFltOrig"))).Text;
                Destination = ((TextBox)(grdRouting.Rows[0].FindControl("txtFltDest"))).Text;
                FlightNumber = ((DropDownList)(grdRouting.Rows[0].FindControl("ddlFltNum"))).SelectedItem.Text;
                FlightDate = DateTime.ParseExact(((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
                allocFirstLine(AWBNo, Totpcs, TotWgt, PcsToAssign, WgtToAssign, Origin, Destination, FlightNumber, FlightDate);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error in Add to ULD";
            }
        }
        #endregion

        #region allocFirstLine
        protected void allocFirstLine(string AWBNo, int Totpcs, decimal TotWgt, int PcsToAssign, decimal WgtToAssign, string Origin, string Destination, string FlightNumber, string FlightDate)
        {
            DataTable dtCreditInfoA = new DataTable();
            try
            {
                dtCreditInfoA.Columns.Add("AWB");
                dtCreditInfoA.Columns.Add("TotPcs");
                dtCreditInfoA.Columns.Add("TotWgt");
                dtCreditInfoA.Columns.Add("PcsToAssign");
                dtCreditInfoA.Columns.Add("WgttoAssign");
                dtCreditInfoA.Columns.Add("Origin");
                dtCreditInfoA.Columns.Add("Destination");
                dtCreditInfoA.Columns.Add("FlightNumber");
                dtCreditInfoA.Columns.Add("FliightDate");

                DataRow rw = dtCreditInfoA.NewRow();
                rw["AWB"] = AWBNo;
                rw["TotPcs"] = Totpcs;
                rw["TotWgt"] = TotWgt;
                rw["PcsToAssign"] = PcsToAssign;
                rw["WgttoAssign"] = WgtToAssign;
                rw["Origin"] = Origin;
                rw["Destination"] = Destination;
                rw["FlightNumber"] = FlightNumber;
                rw["FliightDate"] = FlightDate;
                dtCreditInfoA.Rows.Add(rw);
                Session["AWBForULDAssoc"] = dtCreditInfoA;
                if (Session["AWBForULDAssoc"] != null)
                {
                    //Response.Redirect("frmULDToAWBAssoc.aspx",false);
                    ClientScript.RegisterStartupScript(this.GetType(), "", "callexportULD();", true);
                }

            }
            catch (Exception ex)
            {
                dtCreditInfoA = null;
            }
            finally
            {
                if (dtCreditInfoA != null)
                    dtCreditInfoA.Dispose();
            }

        }
        #endregion allocFirstLine

        #region ddlPartnerType_SelectionChange
        protected void ddlPartnerType_SelectionChange(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")) == ((DropDownList)sender))
                    {
                        rowindex = i;
                    }
                }
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);


            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region ShipperCodeChanges Event
        protected void ShipperCodeDetailsChanged(object sender, EventArgs e)
        {
            ShipperBAL objSHBal = null;
            DataSet ds = null;

            try
            {
                if (sender.Equals(txtShipperCode))
                {
                    objSHBal = null;
                    ds = null;

                    if (txtShipperCode.Text != "")
                    {
                        objSHBal = new ShipperBAL();
                        ds = objSHBal.GetShipperAccountInfo(txtShipperCode.Text.Trim(), Convert.ToDateTime(Session["IT"]).Date);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            TXTShipper.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
                            TXTShipTelephone.Text = ds.Tables[0].Rows[0]["PhoneNumber"].ToString();
                            TXTShipAddress.Text = ds.Tables[0].Rows[0]["Adress1"].ToString();
                            TXTShipperAdd2.Text = ds.Tables[0].Rows[0]["Adress2"].ToString();
                            TXTShipperCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                            TXTShipperState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                            ddlShipCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                            TXTShipPinCode.Text = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                            TXTShipperEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                            txtShipperCCSF.Text = ds.Tables[0].Rows[0]["CCSFApprovalNo"].ToString();

                            if (txtShipperCCSF.Text.Trim() != "")
                                chkTBScreened.Checked = false;
                        }
                    }
                    else if (txtShipperCode.Text == "")
                    {
                        return;
                    }
                }
                else if (sender.Equals(txtConsigneeCode))
                {
                    objSHBal = null;
                    ds = null;

                    if (txtConsigneeCode.Text == "")
                    {
                        return;
                    }

                    objSHBal = new ShipperBAL();
                    ds = objSHBal.GetShipperAccountInfo(txtConsigneeCode.Text.Trim(), Convert.ToDateTime(Session["IT"]).Date);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        TXTConsignee.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
                        TXTConTelephone.Text = ds.Tables[0].Rows[0]["PhoneNumber"].ToString();
                        TXTConAddress.Text = ds.Tables[0].Rows[0]["Adress1"].ToString();
                        TXTConsigAdd2.Text = ds.Tables[0].Rows[0]["Adress2"].ToString();
                        TXTConsigCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                        TXTConsigState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                        ddlConCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                        TXTConsigPinCode.Text = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                        TXTConsigEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                        //txtConsigneeCCSF.Text = ds.Tables[0].Rows[0]["CCSFApprovalNo"].ToString();

                        //if (txtConsigneeCCSF.Text.Trim() != "")
                        //    chkTBScreened.Checked = false;
                    }
                    else
                    {
                        return;
                    }
                }
                ShipperConDetailsChenged(null, null);
            }
            catch (Exception ex)
            {
                objSHBal = null;
                ds = null;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in ShipperCode";
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                objSHBal = null;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
        }
        #endregion

        #region GenerateWalkinAgentInvoiceNumber
        protected void GenerateWalkinAgentInvoiceNumber()
        {
            DataSet dsAWBData = null;
            try
            {
                string AWBNumber = string.Empty;

                if (txtAWBNo.Text.Trim() == "")
                    AWBNumber = HidAWBNumber.Value;
                else
                    AWBNumber = txtAWBNo.Text.Trim();

                object[] AwbRateInfo = new object[2];
                int i = 0;

                AwbRateInfo.SetValue(txtAWBNo.Text.Trim(), i);
                i++;

                AwbRateInfo.SetValue(txtAwbPrefix.Text.Trim(), i);

                dsAWBData = objExpMani.GetDepartedPPAWBData(AwbRateInfo);

                if (dsAWBData != null)
                {
                    if (dsAWBData.Tables != null)
                    {
                        if (dsAWBData.Tables.Count > 0)
                        {
                            if (dsAWBData.Tables[0].Rows.Count > 0)
                            {
                                try
                                {
                                    string res = "";
                                    string InvoiceNumList = "";
                                    for (int cnt = 0; cnt < dsAWBData.Tables[0].Rows.Count; cnt++)
                                    {
                                        //Generate Invoice Number (WALKIN Agent) for each AWB
                                        #region Prepare Parameters
                                        object[] AwbInfo = new object[4];
                                        int j = 0;

                                        AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["AWBPrefix"].ToString() + "-" + dsAWBData.Tables[0].Rows[cnt]["AWBNumber"].ToString(), j);
                                        j++;
                                        AwbInfo.SetValue(Session["UserName"].ToString(), j);
                                        j++;
                                        AwbInfo.SetValue(Convert.ToDateTime(Session["IT"].ToString()), j);
                                        j++;
                                        AwbInfo.SetValue("", j);

                                        #endregion Prepare Parameters

                                        res = objExpMani.InsertDepartedPPAWBData(AwbInfo);

                                        //Generate Invoice Number (WALKIN Agent) for each AWB
                                        #region Prepare Parameters
                                        object[] AwbInvoiceInfo = new object[2];
                                        int k = 0;

                                        AWBNumber = dsAWBData.Tables[0].Rows[cnt]["AWBPrefix"].ToString().Trim() + dsAWBData.Tables[0].Rows[cnt]["AWBNumber"].ToString().Trim();

                                        AwbInvoiceInfo.SetValue(AWBNumber, k);
                                        k++;
                                        //Set UpdatedOn
                                        AwbInvoiceInfo.SetValue(Convert.ToDateTime(Session["IT"].ToString()), k);

                                        #endregion Prepare Parameters

                                        if (InvoiceNumList == "")
                                        {
                                            InvoiceNumList = InvoiceNumList + objExpMani.GenerateBunchInvoiceNumWalkInAgent(AwbInvoiceInfo);
                                        }
                                        else
                                        {
                                            InvoiceNumList = InvoiceNumList + "," + objExpMani.GenerateBunchInvoiceNumWalkInAgent(AwbInvoiceInfo);
                                        }

                                    }

                                    //Code to Generate Invoice (WALKIN Agent)
                                    hfInvoiceNos.Value = InvoiceNumList;

                                    DropDownList ddlPaymentMode = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
                                    if (ddlPaymentMode.SelectedValue == "PP")
                                        btnCollect.Enabled = true;
                                    else
                                        btnCollect.Enabled = false;

                                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateInvoices();", true);
                                    if (Convert.ToBoolean(Session["GenerateWalkingInvoiceShow"]))
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "GenerateInvoices();", true);

                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dsAWBData = null;
            }
            finally
            {
                if (dsAWBData != null)
                    dsAWBData.Dispose();
            }

        }
        #endregion

        #region Add details in Billing
        public void AddBillingDetails(string AWBNumber, string BillingEvent)
        {
            try
            {
                #region Prepare Parameters
                object[] AWBInfo = new object[5];
                int irow = 0;

                AWBInfo.SetValue(AWBNumber, irow);
                irow++;

                AWBInfo.SetValue(BillingEvent, irow);
                irow++;

                string UserName = Session["UserName"].ToString();
                AWBInfo.SetValue(UserName, irow);

                irow++;

                //AWBInfo.SetValue(System.DateTime.Now, irow);
                AWBInfo.SetValue(Convert.ToDateTime(Session["IT"]), irow);

                irow++;

                AWBInfo.SetValue(Session["Station"].ToString(), irow);

                #endregion Prepare Parameters

                string res = "";
                res = objBLL.InsertAWBDataInBilling(AWBInfo); //Normal Billing

                string resIntInv = "";
                resIntInv = objBLL.InsertAWBDataInInterlineInvoice(AWBInfo); //Interline Invoice

                string resIntCN = "";
                resIntCN = objBLL.InsertAWBDataInInterlineCreditNote(AWBInfo); //Interline Credit Note

            }
            catch (Exception EX)
            {

            }
        }
        #endregion Add details in Billing

        #region btnSendFHL_Click
        protected void btnSendFHL_Click(object sender, EventArgs e)
        {
            if (CHKConsole.Checked)
            {
                try
                {
                    string FlightNumber = "";
                    string PartnerCode = "";
                    string AWBNumber = "", error = "";
                    DataSet ds = new DataSet();
                    lblMsg.Text = "FHL";
                    try
                    {
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            FlightNumber = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                            PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        BALEmailID ObjEmail = new BALEmailID();
                        ds = ObjEmail.GetEmail(ddlOrg.SelectedItem.Text, ddlDest.SelectedItem.Text, "FHL", FlightNumber, PartnerCode);
                        if (ds != null)
                        {
                            txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                            lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                            if (lblMsgCommType.Text.Length > 0)
                            {
                                if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                                {
                                    GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                                }
                            }
                        }
                        ObjEmail = null;
                    }
                    catch (Exception ex) { }
                    if (HidAWBNumber.Value.ToString().Trim() != "")
                    {
                        AWBNumber = HidAWBNumber.Value.ToString().Trim();
                    }
                    else if (txtAWBNo.Text.Trim() == "")
                    {
                        AWBNumber = txtAWBNo.Text.Trim();
                    }
                    else
                    {
                        lblStatus.Text = "Please Provide AWB Number";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (AWBNumber.Length > 0)
                    {
                        string Msg = cls_BL.EncodeFHLForSend(AWBNumber, txtAwbPrefix.Text, ref error);
                        if (Msg.Length > 3)
                        {
                            txtMessageBody.Text = Msg;
                            Session["Message"] = "FHL";
                        }
                        else if (error.Length > 0)
                        {
                            lblStatus.Text = "IATA FHL Message Error:" + error;
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error on FHL Click" + ex.Message;
                    return;
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            }
            else
            {
                lblStatus.Text = "Shipment is not Console shipment";
                return;
            }

        }
        #endregion

        #region Button ePouch
        protected void btnePouch_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ePouchAWBNo"] = txtAwbPrefix.Text + txtAWBNo.Text;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('ePouchNew.aspx','_blank')", true);
            }
            catch (Exception ex)
            { }

        }
        #endregion

        private string CheckAWBValidations()
        {
            string strResult = string.Empty;
            StringBuilder StrDimensions = new StringBuilder();
            DataSet dsDim = (DataSet)Session["dsDimesionAll"];

            StringBuilder StrFltDetails = new StringBuilder();

            if (dsDim != null && dsDim.Tables.Count > 0 && dsDim.Tables[0].Rows.Count > 0)
            {
                for (int intCount = 0; intCount < dsDim.Tables[0].Rows.Count; intCount++)
                {
                    if (Convert.ToDecimal(dsDim.Tables[0].Rows[intCount]["Length"]) > 0 || Convert.ToDecimal(dsDim.Tables[0].Rows[intCount]["Breath"]) > 0
                        || Convert.ToDecimal(dsDim.Tables[0].Rows[intCount]["Height"]) > 0)
                    {
                        StrDimensions.Append("Insert into #Dimensions values(");
                        StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Length"]);
                        StrDimensions.Append(",");
                        StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Breath"]);
                        StrDimensions.Append(",");
                        StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Height"]);
                        StrDimensions.Append(",'");
                        StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Units"]);
                        StrDimensions.Append("'); ");
                    }
                }
            }

            dsDim = null;

            if (grdRouting.Rows.Count > 0)
            {
                for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                {
                    string flight = "";
                    string DeptTime = "";
                    try
                    {
                        if (((DropDownList)grdRouting.Rows[intCount].FindControl("ddlPartner")).SelectedItem.Text.Trim().Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            flight = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFlightID")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[intCount].FindControl("txtFlightID")).Text.Trim();
                            DeptTime = "00:00";
                        }
                        else
                        {
                            //DataSet ds =(DataSet)(Session["Flt"]);
                            //if (ds != null)
                            //{
                            //    if (ds.Tables.Count > intCount)
                            //    {
                            //        if (ds.Tables[intCount].Rows.Count > 0)
                            //        {
                            //            DeptTime = ds.Tables[intCount].Rows[((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedIndex]["DeptTime"].ToString();
                            //        }
                            //    }
                            //}
                            //else
                            DeptTime = "00:00";// ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedValue;

                            flight = ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedItem.Text;
                        }
                    }
                    catch (Exception ex) { }


                    StrFltDetails.Append("Insert into #FltDetails (fltNo, fltDate, origin, Dest, FltDepTime, FltPcs, FltGrossWt) values('");
                    StrFltDetails.Append(flight);
                    StrFltDetails.Append("','");
                    StrFltDetails.Append(DateTime.ParseExact(((TextBox)grdRouting.Rows[intCount].FindControl("txtFdate")).Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
                    StrFltDetails.Append("','");
                    StrFltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim());
                    StrFltDetails.Append("','");
                    StrFltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text.Trim());
                    StrFltDetails.Append("','");
                    StrFltDetails.Append(DeptTime);
                    StrFltDetails.Append("',");
                    StrFltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtPcs")).Text.Trim());
                    StrFltDetails.Append(",");
                    StrFltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtWt")).Text.Trim());
                    StrFltDetails.Append("); ");
                }
            }

            if (StrFltDetails.Length > 0)
                strResult = objBLL.CheckAWBValidations(StrFltDetails.ToString(), ddlOrg.Text.Trim(), ddlDest.Text.Trim(), (DateTime)Session["IT"], StrDimensions.ToString());


            StrFltDetails = null;

            return strResult;
        }

        //private void AWBDesignators(string AWBPrefix)
        //{
        //    if (AWBPrefix == string.Empty)
        //        AWBPrefix = Session["awbPrefix"].ToString();

        //    ddlAirlineCode.Items.Clear();

        //    LoginBL Bal = new LoginBL();

        //    DataTable dt = Bal.LoadSystemMasterDataNew("DC").Tables[0];
        //    for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
        //    {
        //        ddlAirlineCode.Items.Add(dt.Rows[intCount]["DesignatorCode"].ToString());
        //    }
        //    ddlAirlineCode.Items.Add("");
        //    Bal = null;
        //    dt = null;
        //}

        //private void LoadCnoteCodes()
        //{
        //    LoginBL Bal = new LoginBL();
        //    DataTable dt = Bal.LoadSystemMasterDataNew("CN").Tables[0];
        //    ddlDocType.DataSource = dt;
        //    ddlDocType.DataMember = dt.TableName;
        //    ddlDocType.DataTextField = "AWBCnoteMaster";
        //    ddlDocType.DataValueField = "AWBCnoteMaster";
        //    ddlDocType.DataBind();
        //    Bal = null;
        //    dt = null;
        //}

        //private void FillProductType()
        //{
        //    LoginBL Bal = new LoginBL();
        //    DataTable dsIrr = null;

        //    try
        //    {
        //        dsIrr = Bal.LoadSystemMasterDataNew("PM").Tables[0];
        //        if (dsIrr != null && dsIrr.Rows.Count > 0)
        //        {
        //            ddlProductType.Items.Clear();
        //            ddlProductType.DataSource = dsIrr;
        //            ddlProductType.DataTextField = "ProductType";
        //            ddlProductType.DataValueField = "SerialNumber";
        //            ddlProductType.DataBind();
        //            ddlProductType.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bal = null;
        //        dsIrr = null;
        //    }
        //    finally
        //    {
        //        Bal = null;
        //        dsIrr = null;
        //    }
        //}

        private void FillHandlerCodes()
        {
            DataSet dsHandler = new DataSet();
            try
            {

                dsHandler = objBLL.GetHandlerCode(ddlOrg.Text.Trim(), ddlProductType.Text.Trim());
                if (dsHandler != null)
                {
                    if (dsHandler.Tables.Count > 0)
                    {
                        if (dsHandler.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = dsHandler.Tables[0].NewRow();

                            row["HandlerName"] = "Select";
                            row["HandlerCode"] = "";
                            dsHandler.Tables[0].Rows.Add(row);

                            ddlHandler.Items.Clear();
                            ddlHandler.DataSource = dsHandler;
                            ddlHandler.DataTextField = "HandlerName";
                            ddlHandler.DataValueField = "HandlerCode";
                            ddlHandler.DataBind();
                            ddlHandler.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dsHandler = null;
            }
            finally
            {
                if (dsHandler != null)
                    dsHandler.Dispose();
            }

        }

        private bool SaveULDInformation()
        {
            DataTable dtULDInfo = (DataTable)Session["AWBULD"];
            SQLServer sq = new SQLServer(Global.GetConnectionString());

            try
            {
                if (dtULDInfo != null && dtULDInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < dtULDInfo.Rows.Count; i++)
                    {
                        if (dtULDInfo.Rows[i]["ULD"].ToString().Trim() != "")
                        {
                            string[] paramname = new string[14];
                            paramname[0] = "ULDNo";
                            paramname[1] = "AWBNo";
                            paramname[2] = "Pieces";
                            paramname[3] = "Weight";
                            paramname[4] = "FlightNo";
                            paramname[5] = "FlightDate";
                            paramname[6] = "PosInFlight";
                            paramname[7] = "ULDType";
                            paramname[8] = "ULDOrigin";
                            paramname[9] = "ULDDest";
                            paramname[10] = "CreatedOn";
                            paramname[11] = "CreatedBy";
                            paramname[12] = "SrNo";
                            paramname[13] = "AWBPrefix";

                            object[] paramvalue = new object[14];
                            paramvalue[0] = dtULDInfo.Rows[i]["ULD"].ToString();
                            paramvalue[1] = HidAWBNumber.Value;
                            paramvalue[2] = dtULDInfo.Rows[i]["Pcs"].ToString();
                            paramvalue[3] = dtULDInfo.Rows[i]["Wgt"].ToString();
                            paramvalue[4] = dtULDInfo.Rows[i]["FlightNo"].ToString();
                            paramvalue[5] = dtULDInfo.Rows[i]["FlightDate"].ToString();
                            paramvalue[6] = dtULDInfo.Rows[i]["PosInFlight"].ToString();
                            paramvalue[7] = dtULDInfo.Rows[i]["Type"].ToString();
                            paramvalue[8] = dtULDInfo.Rows[i]["ULDOrigin"].ToString();
                            paramvalue[9] = dtULDInfo.Rows[i]["ULDDest"].ToString();
                            paramvalue[10] = Session["IT"].ToString();
                            paramvalue[11] = Session["UserName"].ToString();
                            paramvalue[12] = dtULDInfo.Rows[i]["SrNo"].ToString();
                            paramvalue[13] = txtAwbPrefix.Text;

                            SqlDbType[] paramtype = new SqlDbType[14];
                            paramtype[0] = SqlDbType.VarChar;
                            paramtype[1] = SqlDbType.VarChar;
                            paramtype[2] = SqlDbType.Int;
                            paramtype[3] = SqlDbType.Decimal;
                            paramtype[4] = SqlDbType.VarChar;
                            paramtype[5] = SqlDbType.VarChar;
                            paramtype[6] = SqlDbType.VarChar;
                            paramtype[7] = SqlDbType.VarChar;
                            paramtype[8] = SqlDbType.VarChar;
                            paramtype[9] = SqlDbType.VarChar;
                            paramtype[10] = SqlDbType.DateTime;
                            paramtype[11] = SqlDbType.VarChar;
                            paramtype[12] = SqlDbType.NVarChar;
                            paramtype[13] = SqlDbType.NVarChar;

                            bool res = sq.InsertData("spInsertAndUpdateULDtoAWBAssoc", paramname, paramtype, paramvalue);
                        }
                    }
                }
            }
            catch
            {
                sq = null;
                dtULDInfo = null;
                return false;
            }
            finally
            {
                if (dtULDInfo != null)
                    dtULDInfo.Dispose();
            }
            return true;
        }

        //private bool SaveHAWBInformation()
        //{
        //    BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
        //    DataTable dt = (DataTable)Session["HAWBDetails"];
        //    try
        //    {
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
        //            {
        //                HAWB.PutHAWBDetails(HidAWBNumber.Value, dt.Rows[intCount]["HAWBNo"].ToString(), Convert.ToInt16(dt.Rows[intCount]["HAWBPcs"]), Convert.ToInt16(dt.Rows[intCount]["HAWBWt"]),
        //                    dt.Rows[intCount]["Description"].ToString(), dt.Rows[intCount]["CustID"].ToString(), dt.Rows[intCount]["CustName"].ToString(), dt.Rows[intCount]["CustAddress"].ToString(),
        //                    dt.Rows[intCount]["CustCity"].ToString(), dt.Rows[intCount]["Zipcode"].ToString(), dt.Rows[intCount]["Origin"].ToString(), dt.Rows[intCount]["Destination"].ToString(),
        //                    dt.Rows[intCount]["SHC"].ToString(), dt.Rows[intCount]["HAWBPrefix"].ToString(),
        //                    txtAwbPrefix.Text, "", "", "", "", "", dt.Rows[intCount]["ConsigneeName"].ToString(), dt.Rows[intCount]["ConsigneeAddress"].ToString(), dt.Rows[intCount]["ConsigneeCity"].ToString(), dt.Rows[intCount]["ConsigneeState"].ToString(), dt.Rows[intCount]["ConsigneeCountry"].ToString(),
        //                    dt.Rows[intCount]["ConsigneePostalCode"].ToString(), dt.Rows[intCount]["CustState"].ToString(), dt.Rows[intCount]["CustCountry"].ToString(), dt.Rows[intCount]["UOM"].ToString(), dt.Rows[intCount]["SLAC"].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dt = null;
        //    }
        //    finally
        //    {
        //        if (dt != null)
        //            dt.Dispose();
        //    }
        //    return true;
        //}

        #region GetHeavyWtDefinedInSystem
        private decimal GetHeavyWtDefinedInSystem()
        {
            decimal val = decimal.MaxValue;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                ds = da.SelectRecords("spGetHeavyWt");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            val = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                da = null;
            }
            finally
            {
                da = null;
                if (ds != null)
                    ds.Dispose();
            }
            return val;
        }
        #endregion

        #region isHeavyShipmentPresent
        private bool isHeavyShipmentPresent()
        {
            bool flag = false;
            DataSet dsPieces = null;
            try
            {
                dsPieces = (DataSet)Session["dsPiecesDet"];
                if (dsPieces != null)
                {
                    if (dsPieces.Tables.Count > 0)
                    {
                        if (dsPieces.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsPieces.Tables[0].Rows.Count; i++)
                            {
                                if (dsPieces.Tables[0].Rows[i]["isHeavy"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
                                {
                                    flag = true;
                                    i = (dsPieces.Tables[0].Rows.Count + 1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dsPieces = null;
                flag = false;
            }
            finally
            {
                if (dsPieces != null)
                    dsPieces.Dispose();
            }
            return flag;
        }
        #endregion

        #region isHeavyShipmentOnRoute
        private bool isHeavyShipmentOnRoute()
        {
            bool flag = false;
            DataSet dsPieces = null;
            try
            {
                dsPieces = (DataSet)Session["dsRouteds"];
                if (dsPieces != null)
                {
                    if (dsPieces.Tables.Count > 0)
                    {
                        if (dsPieces.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsPieces.Tables[0].Rows.Count; i++)
                            {
                                if (dsPieces.Tables[0].Rows[i]["isHeavy"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
                                {
                                    flag = true;
                                    i = (dsPieces.Tables[0].Rows.Count + 1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dsPieces = null;
                flag = false;
            }
            finally
            {
                if (dsPieces != null)
                    dsPieces.Dispose();
            }
            return flag;
        }
        #endregion

        public void CreateAWBRoutePiecesds()
        {
            DataSet dsAWBPiecesAll = new DataSet();
            try
            {
                dsAWBPiecesAll.Tables.Add(new DataTable());

                dsAWBPiecesAll.Tables[0].Columns.Add("Pieces");
                dsAWBPiecesAll.Tables[0].Columns.Add("GrossWt");
                dsAWBPiecesAll.Tables[0].Columns.Add("PieceId");
                dsAWBPiecesAll.Tables[0].Columns.Add("RowIndex");
                dsAWBPiecesAll.Tables[0].Columns.Add("isHeavy");
                Session["dsRouteds"] = dsAWBPiecesAll.Copy();
            }
            catch (Exception ex)
            {
                dsAWBPiecesAll = null;
                lblStatus.Text = "" + ex.Message;
                //return;
            }
            finally
            {
                if (dsAWBPiecesAll != null)
                    dsAWBPiecesAll.Dispose();
            }
        }

        #region BookingRemarks
        protected void BookingRemarks()
        {
            //string awbnmbr;
            //string Date="";
            try
            {
                //if (Request.QueryString["cmd"] == "Edit")
                //    rtlid = Convert.ToInt32(HidSrNo.Value);
                //if(Request.QueryString["command"]=="Edit" || Request.QueryString["Command"]=="View")
                //awbnmbr = txtAWBNo.Text.ToString(); 



                string AWBnmbr = HidAWBNumber.Value;
                string Date = Session["IT"].ToString();

                UserName = Session["UserName"].ToString();
                string[] param = { "name", "comments", "date", "AWBNumber", "AWBPrefix" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { UserName, txtComment.Text, Date, AWBnmbr, txtAwbPrefix.Text.Trim() };
                bool flag = da.ExecuteProcedure("SP_InsertRemarksBooking", param, dbtypes, values);
            }
            catch (Exception ex)
            { }
            txtComment.Text = "";
            BindRepeaterData();

        }
        #endregion

        #region BindRemarks
        protected void BindRepeaterData()
        {
            DataSet ds = new DataSet();
            // string[] param= { "" };
            try
            {
                string awbnmbr = HidAWBNumber.Value;

                if (Request.QueryString["command"] == "Edit" || Request.QueryString["Command"] == "View")
                    awbnmbr = Request.QueryString["AWBNumber"].ToString();

                if (awbnmbr.Length > 0)
                {
                    string[] param = { "AWBNumber" };
                    SqlDbType[] dbtypes = { SqlDbType.VarChar };
                    object[] values = { awbnmbr };

                    ds = da.SelectRecords("[SP_GetRemarksBooking]", param, values, dbtypes);

                    RepDetails.DataSource = ds;
                    RepDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }

        }
        #endregion

        protected void btnCargoReceipt_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {
                if (txtAWBNo.Text != "")
                {
                    ds = objBLL.getAvailableforCargoReceipt(txtAwbPrefix.Text.Trim(), txtAWBNo.Text.Trim());
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Fetching AWB data for Cargo Receipt failed";
                                }
                                else if (ds.Tables[0].Rows[0][0].ToString() == "2")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Cannot generate Cargo Receipt as Shipment is not Accepted yet";
                                }
                                else
                                {
                                    Session["CargoRec"] = ds.Tables[1];
                                    string query = "'frmCargoReceipt.aspx'";
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ");", true);
                                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);
                                }
                            }
                            else
                            {
                                lblStatus.Text = "No AWB record available";
                                lblStatus.ForeColor = Color.Blue;
                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Fetching AWB data for Cargo Receipt failed";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblStatus.Text = "Please Provide AWB #";
                    lblStatus.ForeColor = Color.Blue;
                }
            }
            catch (Exception ex)
            {
                ds = null;
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Generation of Cargo Receipt failed [" + ex.Message + "]";
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void txtAWBNo_TextChanged(object sender, EventArgs e)
        {
            string strAWBNumber = ((TextBox)sender).Text.Trim();
            if (strAWBNumber.Length > 8)
            {
                txtAwbPrefix.Text = strAWBNumber.Replace(strAWBNumber.Substring(strAWBNumber.Length - 8, 8), "");
                txtAWBNo.Text = strAWBNumber.Replace(txtAwbPrefix.Text.Trim(), "");
                btnListAgentStock_Click(null, null);
            }
        }

        protected void txtAWBNo_DataBinding(object sender, EventArgs e)
        {
            string strAWBNumber = ((TextBox)sender).Text.Trim();
            if (strAWBNumber.Length > 8)
            {
                txtAwbPrefix.Text = strAWBNumber.Replace(strAWBNumber.Substring(strAWBNumber.Length - 8, 8), "");
                txtAWBNo.Text = strAWBNumber.Replace(txtAwbPrefix.Text.Trim(), "");
                btnListAgentStock_Click(null, null);
            }
        }

        protected void btnShowFlights_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = -1;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((ImageButton)grdRouting.Rows[i].FindControl("btnShowFlightPopup")) == ((ImageButton)sender))
                    {
                        rowIndex = i;
                        break;
                    }
                }

                if (((TextBox)grdRouting.Rows[rowIndex].FindControl("txtFltOrig")).Text == "" ||
                    ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtFltDest")).Text == "")
                {
                    return;
                }

                Session["FltOrigin"] = ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtFltOrig")).Text;
                Session["FltDestination"] = ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtFltDest")).Text;
                Session["FltDate"] = ((TextBox)grdRouting.Rows[rowIndex].FindControl("txtFdate")).Text;
                Session["ShowFltRowIndex"] = rowIndex;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>showFlightsByFltDate('')</script>", false);

            }
            catch (Exception)
            {
            }
        }

        #region Button ShowRemarks
        protected void btnShowRemarks_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanellSplit();</script>", false);
        }
        #endregion

        private void RefreshMaterialDG()
        {
            int totalpcs = 0;
            decimal TotalGWt = 0, TotalCWt = 0, TotalVol = 0;
            for (int iCount = 0; iCount < grdMaterialDetails.Rows.Count; iCount++)
            {
                string strPcs = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("TXTPcs")).Text.Trim();
                string strGWt = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommGrossWt")).Text.Trim();
                string strCWt = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommChargedWt")).Text.Trim();
                string strVol = ((TextBox)grdMaterialDetails.Rows[iCount].FindControl("txtCommVolWt")).Text.Trim();

                totalpcs += (strPcs == "" ? 0 : int.Parse(strPcs));
                TotalGWt += (strGWt == "" ? 0 : Convert.ToDecimal(strGWt));
                TotalCWt += (strCWt == "" ? 0 : Convert.ToDecimal(strCWt));
                TotalVol += (strVol == "" ? 0 : Convert.ToDecimal(strVol));
            }

            ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = totalpcs.ToString();
            ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = TotalGWt.ToString();
            ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = TotalCWt.ToString();
            ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text = TotalVol.ToString();
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetPriorityCodeService(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT Priority FROM shipmentprioritymaster ", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetCommodityCodesWithName(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT CommodityCode + '(' + Description + ')' from CommodityMaster where (Description like '%" + prefixText + "%' or CommodityCode like '%" + prefixText + "%')", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetDriverDetails(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT DriverName + '(' + DLNumber + ')' from DriverMaster where (DriverCode like '%" + prefixText + "%' or DriverName like '%" + prefixText + "%')", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetIACDetails(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT IACNumber from CCSFMaster where IACNumber like '%" + prefixText + "%'", con);
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetKnownShipperDetails(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT ShipperCode + '(' + ShipperName + ')' from KnownShipperMaster where (ShipperCode like '%" + prefixText + "%' or ShipperName like '%" + prefixText + "%')", con);
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

        #region UpdatePartnerCode
        private void UpdatePartnerCode(int rowindex)
        {
            DataSet dsResult = new DataSet();

            try
            {
                string errormessage = "";
                if (objBLL.GetAvailabePartners(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), dtCurrentDate, ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartnerType")).Text.ToUpper(), ref dsResult, ref errormessage))
                {
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            DropDownList ddl = ((DropDownList)(grdRouting.Rows[rowindex].FindControl("ddlPartner")));
                            TextBox txtdest = ((TextBox)(grdRouting.Rows[rowindex].FindControl("txtFltDest")));

                            if (dsResult != null)
                            {
                                if (dsResult.Tables.Count > 0)
                                {
                                    ddl.Items.Clear();
                                    //ddl.Items.Add("Select");
                                    for (int j = 0; j < dsResult.Tables.Count; j++)
                                    {
                                        if (dsResult.Tables[j].Rows.Count > 0)
                                        {
                                            for (int k = 0; k < dsResult.Tables[j].Rows.Count; k++)
                                            {
                                                ddl.Items.Add(dsResult.Tables[j].Rows[k][0].ToString());
                                            }
                                            ddl.Items.FindByText(ddlAirlineCode.SelectedItem.Text).Selected = true;
                                        }
                                    }
                                    try
                                    {
                                        if (ddl.Items.Count < 1)
                                        {
                                            ddl.Items.Add("Select");
                                        }
                                        ddl.Items.Add("Other");
                                    }
                                    catch (Exception ex) { }
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dsResult = null;
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }
        #endregion

        #region ProcessRatesV5 _ Background Processing
        protected void ProcessRatesV5_Click(object sender, EventArgs e)
        {
            decimal IATAcharge, MKTcharge, MKTTax, IATATax;
            float OCDC, OCDA, OCTax, TotalWt, OATax;
            string IATARateID = "", MKTRateID = "";
            string RateCurrency = "";
            try
            {
                string Origin = ddlOrg.Text.ToString();
                string Destination = ddlDest.Text.ToString();
                BALProcessRates objProcessRate = new BALProcessRates();
                LBLRouteStatus.Text = "";
                rateprocessstatus.Text = "";
                lblStatus.Text = "";
                //if (!ValidateData())
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                //    return; 
                //}
                //string RateCurrency = "";
                AgentCurrency = drpCurrency.SelectedItem.Text.Trim().ToUpper();
                OCDCCurrency = "";
                decimal spotrate = 0;
                double fltGrossWt = 0;
                double fltChrgWt = 0;
                double fltSumChrgblWt = 0;
                DataSet dsResult = new DataSet();
                //DataSet dsPrimeFlight = null;
                int intVIA = 0;
                int AWBPieces = 0;
                float fltMtGrossWt = 0;
                float fltMtChrgblWt = 0;
                //bool IsRateExists = false;
                Session["OCTotal"] = null;
                Session["OATotal"] = null;

                DataTable dtRates = null;
                DataSet dsDetails = null;
                DataRow RatesRow = null;
                //Thread objProcess = null;

                RefreshMaterialDG();

                #region Gross & Chargable Weight Check
                if (grdMaterialDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        fltGrossWt = Convert.ToDouble(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommGrossWt")).Text);
                        fltChrgWt = Convert.ToDouble(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtCommChargedWt")).Text);
                        AWBPieces = AWBPieces + Convert.ToInt16(((TextBox)grdMaterialDetails.Rows[i].FindControl("TXTPcs")).Text);

                        fltSumChrgblWt = fltSumChrgblWt + fltChrgWt;

                        fltMtGrossWt = fltMtGrossWt + (float)fltGrossWt;
                        fltMtChrgblWt = fltMtChrgblWt + (float)fltChrgWt;

                        if (fltChrgWt < fltGrossWt)
                        {
                            rateprocessstatus.ForeColor = Color.Red;
                            rateprocessstatus.Text = "Material Gross Wt. can not be greater than Chargeable Wt.";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                            return;
                        }
                    }
                }
                #endregion

                #region Pay Mode
                if (((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue == "Select")
                {
                    rateprocessstatus.ForeColor = Color.Red;
                    rateprocessstatus.Text = "Please select the pay mode from Material details.";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                    return;

                }
                #endregion

                string strOrigin = string.Empty;
                float fltFltGrossWt = 0;
                float fltFltChrgblWt = 0;

                float FGWt = 0;
                float FCWt = 0;
                string strFinalDest = string.Empty;
                int intPieces = 0, totalPcs = 0;
                //strFlightNumbers = string.Empty;
                OtherFltSpe = string.Empty;
                string partner = string.Empty;
                try
                {
                    LoginBL bal = new LoginBL();
                    DataSet objDS = bal.LoadSystemMasterDataNew("DC");
                    bal = null;

                    chkInterline.Checked = true;

                    for (int intCount = 0; intCount < objDS.Tables[0].Rows.Count; intCount++)
                    {
                        if (txtAwbPrefix.Text.ToUpper().Equals(objDS.Tables[0].Rows[intCount]["AWBPrefix"].ToString().ToUpper()))
                        {
                            chkInterline.Checked = false;
                            break;
                        }
                    }
                }
                catch (Exception ex) { }

                //End
                LoginBL objLoginBAL = new LoginBL();
                bool routeflag = true;
                try
                {
                    //routeflag = Convert.ToBoolean(objLoginBAL.GetMasterConfiguration("RouteValidation")); 
                    routeflag = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RouteValidation"));
                }
                catch (Exception ex)
                {
                    routeflag = true;
                }
                if (routeflag)
                {
                    #region Check Route Info

                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        strOrigin = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text;
                        strFinalDest = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltDest"))).Text.Trim();
                        partner = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartner"))).SelectedItem.Text.Trim();
                        if (CommonUtility.FlightValidation)
                        {
                            if (partner.Equals("Select", StringComparison.OrdinalIgnoreCase))
                            {
                                rateprocessstatus.ForeColor = Color.Red;
                                rateprocessstatus.Text = "Partner Code is mandatory.";
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                                return;
                            }

                            if (!partner.Equals("Other", StringComparison.OrdinalIgnoreCase))//Self/Partner Flight
                            {

                                if (((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text.Trim() != "" &&
                                    ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text.Trim() != "Select" &&
                                    ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text.Trim() != "SELECT")
                                {
                                    if (ddlAirlineCode.Items.FindByText(partner) == null)
                                    {
                                        chkInterline.Checked = true;
                                    }

                                    if (partner.Equals("other", StringComparison.OrdinalIgnoreCase))
                                    {
                                        strFlightNumbers = strFlightNumbers + ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFlightID"))).Text.Trim() + ",";
                                    }
                                    else
                                    {
                                        strFlightNumbers = strFlightNumbers + ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text.Trim() + ",";
                                    }
                                }
                                else
                                {
                                    rateprocessstatus.ForeColor = Color.Red;
                                    rateprocessstatus.Text = "Flight number is mandatory.";
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                                    return;
                                }
                            }
                            else if (partner.Equals("Other", StringComparison.OrdinalIgnoreCase))
                            {

                                if (((TextBox)(grdRouting.Rows[intCount].FindControl("txtFlightID"))).Text.Trim().Length > 2)
                                {

                                    chkInterline.Checked = true;
                                    if (partner.Equals("other", StringComparison.OrdinalIgnoreCase))
                                    {
                                        strFlightNumbers = strFlightNumbers + ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFlightID"))).Text.Trim() + ",";
                                    }
                                    else
                                    {
                                        strFlightNumbers = strFlightNumbers + ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text.Trim() + ",";
                                    }
                                }
                                else
                                {
                                    rateprocessstatus.ForeColor = Color.Red;
                                    rateprocessstatus.Text = "Flight number is mandatory.";
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                                    return;
                                }
                            }
                        }

                        if (strOrigin != ddlOrg.Text.Trim())
                            intVIA = intVIA + 1;

                        if (strOrigin.Trim() == ddlOrg.Text.Trim())
                        {
                            FGWt = (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtWt"))).Text);
                            FCWt = (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtChrgWt"))).Text);
                            intPieces = Convert.ToInt32(((TextBox)(grdRouting.Rows[intCount].FindControl("txtPcs"))).Text.Trim());

                            if (intPieces == 0 || FGWt == 0 || FCWt == 0)
                            {
                                rateprocessstatus.ForeColor = Color.Red;
                                rateprocessstatus.Text = "Route detail values (Pcs / Gwt. / Cwt.) can not be 0.";
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                                return;
                            }

                            if (FGWt > FCWt)
                            {
                                rateprocessstatus.ForeColor = Color.Red;
                                rateprocessstatus.Text = "Route Gross Wt. can not be greater than Chargeable Wt.";
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                                return;
                            }

                            fltFltGrossWt = fltFltGrossWt + FGWt;
                            fltFltChrgblWt = fltFltChrgblWt + FCWt;
                            totalPcs = totalPcs + intPieces;
                        }

                    }
                    #endregion

                    if (AWBPieces != totalPcs)
                    {
                        rateprocessstatus.ForeColor = Color.Red;
                        rateprocessstatus.Text = " pieces count in Material description & Flight route are not matching.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return;
                    }
                    #region Material weights & Flight weights comparision
                    if (fltMtGrossWt != fltFltGrossWt || fltMtChrgblWt != fltFltChrgblWt)
                    {
                        rateprocessstatus.ForeColor = Color.Red;
                        rateprocessstatus.Text = "Material weights & Flight weights are not matching.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return;
                    }
                    #endregion

                    #region destination details are not matching
                    if (ddlDest.Text.ToUpper().Trim() != strFinalDest.ToUpper().Trim())
                    {
                        rateprocessstatus.ForeColor = Color.Red;
                        rateprocessstatus.Text = "Route details are not matching with AWB destination.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return;
                    }
                    #endregion
                }
                string strResult = CheckAWBValidations();

                if (strResult != string.Empty)
                {
                    rateprocessstatus.ForeColor = Color.Red;
                    rateprocessstatus.Text = strResult;
                    //return;
                }

                //////////////////////////////////////////////////////////////////////
                // Special Commodity

                Session["IsSPL"] = null;

                //Session["Mod"] = null;

                HidProcessFlag.Value = "";

                #region "Embargo Verification"

                //Check for Embargo cargo

                string strCommodity = string.Empty;
                DateTime dtExecutionDt = DateTime.ParseExact(txtExecutionDate.Text, "dd/MM/yyyy", null);
                //string strOrigin = string.Empty;
                string strFltOrigin = string.Empty;
                string strDestination = string.Empty;
                string strPaymentType = string.Empty;
                string strFlightNo = string.Empty;
                BookingBAL objBooking = new BookingBAL();

                for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                {
                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                    {
                        strCommodity = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode"))).Text;
                    }
                    else
                    {
                        strCommodity = ((TextBox)(grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1"))).Text;
                    }
                    strFltOrigin = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text;
                    strDestination = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltDest"))).Text;
                    strPaymentType = ((DropDownList)(GRDRates.Rows[0].FindControl("ddlPayMode"))).Text;

                    //Check partner code.
                    partner = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartner"))).SelectedItem.Text.Trim();
                    if (partner.ToLower() == "other")
                    {   //If Other is selected in partner code text box.
                        strFlightNo = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFlightID"))).Text.Trim();
                    }
                    else
                    {
                        if (((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedIndex >= 0)
                            strFlightNo = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text;
                    }

                    if (strFltOrigin.Trim() == ddlOrg.Text.Trim())
                    {
                        fltFltGrossWt = fltFltGrossWt + (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtWt"))).Text);
                        fltFltChrgblWt = fltFltChrgblWt + (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtChrgWt"))).Text);
                    }

                    DataSet Objds = objBooking.VerifyEmbargoCargo(dtExecutionDt, strFltOrigin, strDestination, strCommodity, strPaymentType, strFlightNo);

                    if (Objds != null && Objds.Tables.Count > 0 && Objds.Tables[0].Rows.Count > 0)
                    {
                        if ((bool)Objds.Tables[0].Rows[0]["IsInvalid"] == true)
                        {
                            rateprocessstatus.ForeColor = Color.Red;
                            rateprocessstatus.Text = Convert.ToString(Objds.Tables[0].Rows[0]["ErrorDesc"]);
                            break;
                        }
                    }
                }

                #endregion

                #region Agent Exception
                bool AgntException = objBLL.CheckforAgentException(TXTAgentCode.Text.Trim());
                #endregion

                #region "FOC Shipments"

                if (((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue.Trim() == "FOC" || ddlServiceclass.SelectedItem.Text.Trim() == "FOC")
                {
                    dtRates = ((DataTable)Session["dtRates"]);
                    dtRates.Rows.Clear();

                    RatesRow = dtRates.NewRow();
                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                    {
                        RatesRow["CommCode"] = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode")).Text.ToUpper();
                    }
                    else
                    {
                        RatesRow["CommCode"] = ((TextBox)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1")).Text.ToUpper();
                    }
                    RatesRow["Pcs"] = ((TextBox)grdMaterialDetails.Rows[0].FindControl("TXTPcs")).Text;
                    RatesRow["Weight"] = ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommGrossWt")).Text;
                    RatesRow["FrIATA"] = "0";
                    RatesRow["FrMKT"] = "0";
                    RatesRow["ValCharge"] = "0";
                    RatesRow["PayMode"] = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                    RatesRow["OcDueCar"] = "0";
                    RatesRow["OcDueAgent"] = "0";
                    RatesRow["SpotRate"] = "0";
                    RatesRow["SpotFreight"] = "0";
                    RatesRow["DynRate"] = "0";
                    RatesRow["ServTax"] = "0";
                    RatesRow["Total"] = "0";
                    RatesRow["ChargedWeight"] = ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommChargedWt")).Text;
                    RatesRow["SpotRateID"] = "";
                    RatesRow["RatePerKg"] = "0";
                    RatesRow["IATATax"] = "0";
                    RatesRow["MKTTax"] = "0";
                    RatesRow["OCTax"] = "0";
                    RatesRow["OATax"] = "0";
                    RatesRow["SpotTax"] = "0";
                    RatesRow["CommTax"] = "0";
                    RatesRow["DiscTax"] = "0";
                    RatesRow["Commission"] = "0";
                    RatesRow["Discount"] = "0";
                    RatesRow["CommPercent"] = "0";

                    dtRates.Rows.Add(RatesRow);

                    GRDRates.DataSource = dtRates.Copy();
                    GRDRates.DataBind();
                    DropDownList ddl = (DropDownList)GRDRates.Rows[0].FindControl("ddlPayMode");
                    if (Session["ConBooking_PayModeMaster"] != null)
                    {
                        DataTable dtPayMode = (DataTable)Session["ConBooking_PayModeMaster"];
                        ddl.Items.Clear();
                        ddl.DataSource = dtPayMode;
                        ddl.DataTextField = "PayModeText";
                        ddl.DataValueField = "PayModeCode";
                        ddl.DataBind();
                        if (ddl.Items.Count > 1)
                            ddl.SelectedIndex = 1;
                        dtPayMode.Dispose();
                    }
                    ((DropDownList)(GRDRates.Rows[0].FindControl("ddlPayMode"))).Text = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                    dsDetails = (DataSet)Session["OCDetails"];

                    if (dsDetails != null && dsDetails.Tables.Count > 0 && dsDetails.Tables[0].Rows.Count > 0)
                    {
                        dsDetails.Tables[0].Rows.Clear();
                        Session["OCDetails"] = dsDetails;
                    }

                    #region FLTRoute Details
                    LoadFlightRouteDetails();
                    DataTable FOCRoute = ((DataTable)Session["FltRoute"]).Clone();
                    FOCRoute.Rows.Clear();
                    DataRow drow = null;
                    drow = FOCRoute.NewRow();

                    drow["FltOrigin"] = ddlOrg.Text.Trim();
                    drow["FltDestination"] = ddlDest.Text.Trim();
                    drow["FltNumber"] = "";
                    drow["FltDate"] = "";
                    drow["Pcs"] = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text;
                    drow["GWt"] = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text;
                    drow["CWt"] = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text;
                    drow["IsPrime"] = "false";
                    drow["Freight"] = "0";
                    drow["FrtTax"] = "0";
                    drow["FreightTax"] = "0";
                    drow["RatePerKg"] = "0";
                    drow["isHeavy"] = "0";
                    drow["Proration"] = "0";
                    drow["Discount"] = "0";
                    drow["AgentComm"] = "0";
                    drow["Cost"] = "0";
                    drow["MKTFrt"] = "0";
                    drow["MKTTax"] = "0";
                    drow["CostType"] = "0";
                    drow["Currency"] = "0";
                    FOCRoute.Rows.Add(drow);
                    Session["FltRoute"] = null;
                    Session["FltRoute"] = FOCRoute;
                    GrdRateDetails.DataSource = null;
                    GrdRateDetails.DataSource = FOCRoute;
                    GrdRateDetails.DataBind();
                    #endregion
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                    return;
                }

                #endregion "FOC Shipments"


                if (strFlightNumbers.Length > 0)
                    strFlightNumbers = strFlightNumbers.Substring(0, strFlightNumbers.Length - 1);
                if (intVIA > 0)
                {
                    DataSet dsCommonRate = objBLL.CheckPrimeRate(ddlOrg.Text.Trim(), ddlDest.Text.Trim(), dtCurrentDate.Date, strFlightNumbers);

                    if (dsCommonRate != null)
                        strFlightNumbers = dsCommonRate.Tables[0].Rows[0][0].ToString();
                    else
                        strFlightNumbers = string.Empty;

                    dsCommonRate = null;
                }
                else
                    strFlightNumbers = string.Empty;

                string agentcode, commcode, carrier;

                DataSet dsCharges = new DataSet();
                //bool blRouteWise = false;
                string FlightNumber = string.Empty;
                DataTable DT = null;

                if (((DataTable)Session["FltRoute"]) != null)
                    DT = ((DataTable)Session["FltRoute"]).Copy();

                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                {
                    commcode = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode")).Text.Trim().ToUpper();
                }
                else
                {
                    commcode = ((TextBox)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1")).Text.Trim().ToUpper();
                }
                agentcode = TXTAgentCode.Text.Trim().ToUpper();

                try
                {
                    if (Convert.ToBoolean(Session["GHABooking"]) == false)
                    {
                        if (!ValidateMasterEntry(commcode, "Commodity"))
                        {
                            rateprocessstatus.Text = "Commodity Code not found in Master record. Please validate";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                            if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                            {
                                ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode")).Focus();
                            }
                            else
                            {
                                ((TextBox)grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode1")).Focus();
                            }
                            return;
                        }
                    }

                    /*
                    if (!ValidateMasterEntry(agentcode, "Agent"))
                    {
                        rateprocessstatus.Text = "Agent is not availabe in Agent Master.Please validate";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        return;
                    }
                    */
                }
                catch (Exception ex) { }

                carrier = ddlAirlineCode.Text.Trim();
                IATAcharge = IATATax = MKTcharge = MKTTax = 0;

                //bool IsSpecialCommodity = IsSpecialComm(commcode);

                //if (IsSpecialCommodity)
                //    Session["IsSPL"] = "1";

                // Freight
                dtRates = ((DataTable)Session["dtRates"]);
                dtRates.Rows.Clear();
                //Session["dtRates"] = dtRates.Copy();

                dsDetails = (DataSet)Session["OCDetails"];
                dsDetails.Tables[0].Rows.Clear();
                Session["OCDetails"] = dsDetails.Copy();

                OCDC = OCDA = OCTax = OATax = TotalWt = 0;

                //agentcode = ddlAgtCode.SelectedItem.Text;                

                TotalWt = float.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text);
                //Rate Calculation
                string AWBTotalasPer = "IATA";
                try
                {
                    //AWBTotalasPer = objLoginBAL.GetMasterConfiguration("AWBBookingTotal");
                    AWBTotalasPer = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AWBBookingTotal");
                    if (AWBTotalasPer.Length < 1)
                    {
                        AWBTotalasPer = "IATA";
                    }
                }
                catch (Exception es)
                {
                    AWBTotalasPer = "IATA";
                }
                try
                {
                    GrdRateDetails.DataSource = null;
                    GrdRateDetails.DataBind();
                }
                catch (Exception ex) { }
                Decimal tax = 0;
                try
                {
                    dsCharges = ProcessRatesSP(agentcode, commcode);
                    try
                    {
                        GrdRateDetails.DataSource = dsCharges.Tables[1];
                        GrdRateDetails.DataBind();



                        Session["FltRoute"] = dsCharges.Tables[1].Copy();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (dsCharges.Tables.Count > 2)
                        {
                            Session["OCDetails"] = null;
                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable();
                            dt = dsCharges.Tables[2].Copy();
                            ds.Tables.Add(dt);
                            ds.AcceptChanges();
                            Session["OCDetails"] = ds.Copy();
                            SetOtherChargesSummary(ref OCDC, ref OCDA, ref OCTax, ref OATax);
                        }
                        if (dsCharges.Tables.Count > 3)
                        {
                            DataSet dsTax = new DataSet();
                            DataTable dtTax = new DataTable();
                            dtTax = dsCharges.Tables[4].Copy();
                            Session["TaxDetails"] = dtTax.Copy();

                        }
                    }
                    catch (Exception ex) { }
                    #region Spot rate calculation if required
                    //Commented on 19/03/2013
                    //try
                    //{
                    //    spotrate = Convert.ToDecimal(((TextBox)GRDRates.Rows[0].FindControl("TXTSpotRate")).Text.ToString());
                    //}
                    //catch(Exception ex)
                    //{
                    //    spotrate = 0;
                    //}

                    //if (spotrate > 0)
                    //{

                    //    decimal cwt = Convert.ToDecimal(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text.ToString());
                    //    try
                    //    {
                    //        if (cwt > 0)
                    //        {
                    //            IATAcharge = MKTcharge = (spotrate * cwt);
                    //            tax = 0;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        IATAcharge = MKTcharge = 0;
                    //        tax = 0;
                    //    }
                    //}
                    //else
                    //{
                    //    IATAcharge = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["Charge"].ToString());
                    //    MKTcharge = Convert.ToDecimal(dsCharges.Tables[0].Rows[1]["Charge"].ToString());
                    //    RateClass = dsCharges.Tables[0].Rows[0]["RateClass"].ToString();
                    //    tax = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["ServiceTax"].ToString());
                    //}
                    #endregion

                    IATAcharge = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["Charge"].ToString());
                    MKTcharge = Convert.ToDecimal(dsCharges.Tables[0].Rows[1]["Charge"].ToString());
                    RateClass = dsCharges.Tables[0].Rows[1]["RateClass"].ToString();
                    //tax = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["ServiceTax"].ToString());
                    IATATax = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["ServiceTax"].ToString());
                    MKTTax = Convert.ToDecimal(dsCharges.Tables[0].Rows[1]["ServiceTax"].ToString());
                    RateCurrency = dsCharges.Tables[0].Rows[1]["Currency"].ToString().Trim();

                    IATARateID = dsCharges.Tables[0].Rows[0]["RateLineID"].ToString();
                    MKTRateID = dsCharges.Tables[0].Rows[1]["RateLineID"].ToString();

                    if (RateClass.Length < 1 || RateClass == "")
                        RateClass = dsCharges.Tables[0].Rows[0]["RateClass"].ToString();


                    #region AWB total As per IATA/MKT
                    if (MKTcharge > 0)
                        AWBTotalasPer = "MKT";
                    else if (MKTcharge == 0 && IATAcharge > 0)
                    {
                        AWBTotalasPer = "IATA";
                        RateCurrency = dsCharges.Tables[0].Rows[0]["Currency"].ToString().Trim();

                    }
                    #endregion

                    if (dsCharges.Tables.Count > 3)
                    {
                        if (dsCharges.Tables[3].Rows.Count > 0)
                        {
                            if (AWBTotalasPer.Equals("MKT", StringComparison.OrdinalIgnoreCase))
                            {
                                tax = Convert.ToDecimal(dsCharges.Tables[0].Rows[1]["ServiceTax"].ToString())
                                    + Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["MKTTax"].ToString()) +
                                    Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["OCTax"].ToString()) +
                                    Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["OATax"].ToString());
                            }
                            else
                            {
                                tax = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["ServiceTax"].ToString())
                                    + Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["IATATax"].ToString()) +
                                    Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["OCTax"].ToString()) +
                                    Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["OATax"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex) { }

                //decimal total = 0;

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0 && OCDC == 0 && dsResult.Tables[0].Columns["TotalCharge"] != null)
                {
                    float dcTotalAmt = float.Parse(dsResult.Tables[0].Rows[0]["TotalCharge"].ToString());
                    OCTax = float.Parse(dsResult.Tables[0].Rows[0]["Tax"].ToString());

                    string Chargeype = dsResult.Tables[0].Rows[0]["ChargeType"].ToString().Trim();

                    if (Chargeype == "All" || Chargeype == "ALL")
                    {
                        OCDC = dcTotalAmt;
                        OCDA = dcTotalAmt;
                    }
                    else if (Chargeype == "DC")
                    {
                        OCDC = dcTotalAmt;
                    }
                    else
                        OCDA = dcTotalAmt;
                }

                dtRates.Rows.Clear();

                #region Rounding Off Call
                /*  SCM_FREIGHT,SCM_OCDC,SCM_OCDA*/
                #region OCDC Round OFF
                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_OCDC", OCDC.ToString(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        //OCDC = Convert.ToInt32(Val);
                        OCDC = float.Parse(Val, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    }

                }
                catch (Exception ex) { }
                #endregion

                #region OCDA Round Off
                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_OCDA", OCDA.ToString(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        //OCDA = Convert.ToInt32(Val);
                        OCDA = float.Parse(Val, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    }

                }
                catch (Exception ex) { }
                #endregion

                #region MKT Freight
                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_FREIGHT", IATAcharge.ToString(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        IATAcharge = Convert.ToDecimal(Val);
                    }

                }
                catch (Exception ez) { }
                #endregion

                #region IATA Freight
                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_FREIGHT", MKTcharge.ToString(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        MKTcharge = Convert.ToDecimal(Val);
                    }

                }
                catch (Exception ex)
                { }
                #endregion
                #endregion

                bool IsMini = false;
                if (RateClass.Equals("M", StringComparison.OrdinalIgnoreCase))
                    IsMini = true;

                RatesRow = dtRates.NewRow();
                RatesRow["CommCode"] = commcode.ToUpper();
                RatesRow["Pcs"] = ((TextBox)grdMaterialDetails.Rows[0].FindControl("TXTPcs")).Text;
                RatesRow["Weight"] = ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommGrossWt")).Text;
                RatesRow["FrIATA"] = Convert.ToString((IATAcharge + 0));
                RatesRow["FrMKT"] = Convert.ToString((MKTcharge + 0));
                RatesRow["ValCharge"] = "0";
                RatesRow["PayMode"] = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                RatesRow["OcDueCar"] = Convert.ToString((OCDC + 0));
                RatesRow["OcDueAgent"] = Convert.ToString((OCDA + 0));
                RatesRow["SpotRate"] = Convert.ToString(spotrate);
                RatesRow["SpotFreight"] = 0;
                RatesRow["DynRate"] = "0";
                RatesRow["ServTax"] = "0";// Convert.ToString(Math.Round(((decimal)tax + (decimal)OCTax + (decimal)OATax), 2));
                RatesRow["RateClass"] = RateClass;
                decimal dcTotal = 0, TaxTotal = 0;

                #region Tax Total
                if (AWBTotalasPer.Equals("MKT", StringComparison.OrdinalIgnoreCase))
                {
                    TaxTotal = Math.Round(((decimal)MKTTax + (decimal)tax + (decimal)OCTax + (decimal)OATax), 2);

                }
                else
                {
                    TaxTotal = Math.Round(((decimal)IATATax + (decimal)tax + (decimal)OCTax + (decimal)OATax), 2);

                }

                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_TAX", TaxTotal.ToString(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        TaxTotal = Convert.ToDecimal(Val);
                    }

                }
                catch (Exception ex)
                { }
                #endregion

                #region Total Freight
                if (AWBTotalasPer.Equals("MKT", StringComparison.OrdinalIgnoreCase))
                {

                    dcTotal = (MKTcharge + (decimal)OCDA + (decimal)OCDC + TaxTotal);
                }
                else
                {
                    TaxTotal = Math.Round(((decimal)IATATax + (decimal)tax + (decimal)OCTax + (decimal)OATax), 2);
                    dcTotal = (IATAcharge + (decimal)OCDA + (decimal)OCDC + TaxTotal);
                }

                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_TOTAL", dcTotal.ToString(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        dcTotal = Convert.ToDecimal(Val);
                    }

                }
                catch (Exception em) { }
                #endregion

                #region Logic to Make round-off
                try
                {
                    //MasterBAL objBAL = new MasterBAL();
                    bool roundoff = true;
                    try
                    {
                        //roundoff = Convert.ToBoolean(objLoginBAL.GetMasterConfiguration("TotalRoundingOFF"));
                        roundoff = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "TotalRoundingOFF"));
                    }
                    catch (Exception ex)
                    {
                        roundoff = true;
                    }
                    if (roundoff)
                    {
                        if (dcTotal > 0)
                        {
                            decimal dcRem = dcTotal % Math.Floor(dcTotal);

                            if (dcRem != 0)
                            {
                                if (dcRem < 0.5m)
                                    dcTotal = dcTotal + (0.5m - dcRem);
                                else if (dcRem > 0.5m)
                                    dcTotal = dcTotal + (1 - dcRem);
                            }
                        }
                    }
                }
                catch (Exception ex) { }
                #endregion
                //
                RatesRow["ServTax"] = Convert.ToString(TaxTotal);
                RatesRow["Total"] = Convert.ToString((0 + dcTotal));
                RatesRow["ChargedWeight"] = ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommChargedWt")).Text;// "7KG";
                RatesRow["SpotRateID"] = "";
                RatesRow["IATARateID"] = IATARateID;
                RatesRow["MKTRateID"] = MKTRateID;
                if (AWBTotalasPer.Equals("MKT", StringComparison.OrdinalIgnoreCase))
                {
                    if (IsMini)
                        RatesRow["RatePerKg"] = Convert.ToString((0 + MKTcharge));
                    else
                        RatesRow["RatePerKg"] = Convert.ToString((0 + (MKTcharge / Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommChargedWt")).Text))));
                }
                else
                {
                    if (IsMini)
                        RatesRow["RatePerKg"] = Convert.ToString((0 + IATAcharge));
                    else
                        RatesRow["RatePerKg"] = Convert.ToString((0 + (IATAcharge / Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommChargedWt")).Text))));

                }

                RatesRow["IATATax"] = Convert.ToString((decimal)IATATax);
                RatesRow["MKTTax"] = Convert.ToString((decimal)MKTTax);
                RatesRow["OCTax"] = Convert.ToString((decimal)OCTax);
                RatesRow["OATax"] = Convert.ToString((decimal)OATax);
                RatesRow["SpotTax"] = "0";
                RatesRow["CommTax"] = "0";
                RatesRow["DiscTax"] = "0";
                RatesRow["Commission"] = "0";
                RatesRow["Discount"] = "0";
                RatesRow["CommPercent"] = "0";
                RatesRow["SpotStatus"] = "";
                try
                {
                    if (dsCharges.Tables.Count > 3)
                    {
                        if (dsCharges.Tables[3].Rows.Count > 0)
                        {
                            RatesRow["IATATax"] = dsCharges.Tables[3].Rows[0]["IATATax"].ToString().Length > 0 ? (Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["IATATax"].ToString()) + (decimal)IATATax).ToString() : Convert.ToString((decimal)IATATax); ;
                            RatesRow["MKTTax"] = dsCharges.Tables[3].Rows[0]["MKTTax"].ToString().Length > 0 ? (Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["MKTTax"].ToString()) + (decimal)MKTTax).ToString() : Convert.ToString((decimal)MKTTax); ;
                            RatesRow["OCTax"] = dsCharges.Tables[3].Rows[0]["OCTax"].ToString().Length > 0 ? (Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["OCTax"].ToString()) + (decimal)OCTax).ToString() : Convert.ToString((decimal)OCTax);
                            RatesRow["OATax"] = dsCharges.Tables[3].Rows[0]["OATax"].ToString().Length > 0 ? (Convert.ToDecimal(dsCharges.Tables[3].Rows[0]["OATax"].ToString()) + (decimal)OATax).ToString() : Convert.ToString((decimal)OATax);
                            RatesRow["SpotTax"] = dsCharges.Tables[3].Rows[0]["SpotTax"].ToString().Length > 0 ? dsCharges.Tables[3].Rows[0]["SpotTax"].ToString() : "0";
                            RatesRow["CommTax"] = dsCharges.Tables[3].Rows[0]["CommTax"].ToString().Length > 0 ? dsCharges.Tables[3].Rows[0]["CommTax"].ToString() : "0";
                            RatesRow["DiscTax"] = dsCharges.Tables[3].Rows[0]["DiscTax"].ToString().Length > 0 ? dsCharges.Tables[3].Rows[0]["DiscTax"].ToString() : "0";
                            RatesRow["Commission"] = dsCharges.Tables[3].Rows[0]["Commission"].ToString().Length > 0 ? dsCharges.Tables[3].Rows[0]["Commission"].ToString() : "0";
                            RatesRow["Discount"] = dsCharges.Tables[3].Rows[0]["Discount"].ToString().Length > 0 ? dsCharges.Tables[3].Rows[0]["Discount"].ToString() : "0";
                            RatesRow["CommPercent"] = dsCharges.Tables[3].Rows[0]["CommPercent"].ToString().Length > 0 ? dsCharges.Tables[3].Rows[0]["CommPercent"].ToString() : "0";
                            RatesRow["SpotStatus"] = dsCharges.Tables[3].Rows[0]["SpotStatus"].ToString();
                        }
                    }
                }
                catch (Exception ss) { }
                dtRates.Rows.Add(RatesRow);
                GRDRates.DataSource = dtRates.Copy();
                GRDRates.DataBind();

                dcActualAmtMKT = dcActualTaxMKT = dcMinTaxMKT = dcMinAmountMKT = 0;
                dcActualAmt = dcActualTax = dcMinTax = dcMinAmount = 0;

                DropDownList ddlPayMode = (DropDownList)GRDRates.Rows[0].FindControl("ddlPayMode");
                if (Session["ConBooking_PayModeMaster"] != null)
                {
                    DataTable dt = (DataTable)Session["ConBooking_PayModeMaster"];
                    ddlPayMode.Items.Clear();
                    ddlPayMode.DataSource = dt;
                    ddlPayMode.DataTextField = "PayModeText";
                    ddlPayMode.DataValueField = "PayModeCode";
                    ddlPayMode.DataBind();
                    if (ddlPayMode.Items.Count > 1)
                        ddlPayMode.SelectedIndex = 1;
                }
                ((DropDownList)(GRDRates.Rows[0].FindControl("ddlPayMode"))).Text = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue;
                TextBox txtcur = (TextBox)GRDRates.Rows[0].FindControl("TXTCurrency");
                txtcur.Text = RateCurrency;//drpCurrency.SelectedItem.Text.Trim().ToUpper();
                txtcur.Enabled = false;


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :(ProcessRates5_Click)" + ex.Message;
            }
            #region Disable Rates Grid after Reprocess if the AWB is Executed
            try
            {
                string AWBStatus = Convert.ToString(Session["AWBStatus"]);
                if (AWBStatus == "E")
                {

                    for (int i = 0; i < GRDRates.Rows.Count; i++)
                    {
                        GRDRates.Rows[i].Cells[0].Enabled = false;
                        GRDRates.Rows[i].Cells[1].Enabled = false;
                        GRDRates.Rows[i].Cells[2].Enabled = false;
                        GRDRates.Rows[i].Cells[3].Enabled = false;
                        GRDRates.Rows[i].Cells[4].Enabled = false;

                        GRDRates.Rows[i].Cells[5].Enabled = false;
                        GRDRates.Rows[i].Cells[6].Enabled = false;
                        GRDRates.Rows[i].Cells[7].Enabled = false;
                        GRDRates.Rows[i].Cells[8].Enabled = false;
                        GRDRates.Rows[i].Cells[9].Enabled = false;

                        //GRDRates.Rows[i].Cells[10].Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueCar")).Enabled = false;
                        //GRDRates.Rows[i].Cells[11].Enabled = true;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTOcDueAgent")).Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTServiceTax")).Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTTotal")).Enabled = false;
                        ((TextBox)GRDRates.Rows[i].FindControl("TXTRateClass")).Enabled = false;
                        GRDRates.Rows[i].Cells[12].Enabled = false;
                        GRDRates.Rows[i].Cells[13].Enabled = false;

                        GRDRates.Rows[i].Cells[14].Enabled = false;
                        //GRDRates.Rows[i].Cells[15].Enabled = false;
                        //GRDRates.Rows[i].Cells[16].Enabled = false;

                    }
                }
            }
            catch (Exception ex)
            { }
            #endregion
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }
        #endregion

        #region ProcessRatesSP
        private DataSet ProcessRatesSP(string agentcode, string commcode)
        {
            DataSet dsRates = null;
            try
            {
                string Origin = ddlOrg.Text.Trim();
                string Destination = ddlDest.Text.Trim();

                BALProcessRates objProcessRate = new BALProcessRates();
                System.Text.StringBuilder FltDetails = new System.Text.StringBuilder();
                System.Text.StringBuilder PieceDetails = new System.Text.StringBuilder();
                System.Text.StringBuilder RoutePieceDetails = new System.Text.StringBuilder();
                System.Text.StringBuilder ULD = new System.Text.StringBuilder();
                Decimal AllowedWT = GetHeavyWtDefinedInSystem();

                #region Route Details
                try
                {

                    if (grdRouting.Rows.Count > 0)
                    {
                        for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                        {
                            string flight = "", isHeavy = "0";
                            string ChrgWT = ((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text.Trim().Replace(",", "");
                            try
                            {
                                ChrgWT = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_Weight", ChrgWT, txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                                if (ChrgWT.Length < 1)
                                {
                                    ChrgWT = ((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text.Trim().Replace(",", "");
                                }
                                else
                                {
                                    ((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text = Convert.ToString(ChrgWT);
                                }
                            }
                            catch (Exception ex)
                            {
                                ChrgWT = ((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text.Trim().Replace(",", "");
                            }
                            string carrier = ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlPartner")).SelectedItem.Text.Trim();
                            try
                            {
                                if (((DropDownList)grdRouting.Rows[intCount].FindControl("ddlPartner")).SelectedItem.Text.Trim().Equals("other", StringComparison.OrdinalIgnoreCase))
                                {
                                    flight = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFlightID")).Text.Trim();
                                }
                                else
                                {
                                    flight = ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                                }
                            }
                            catch (Exception ex) { }
                            try
                            {
                                Decimal wt = Decimal.Parse(((TextBox)grdRouting.Rows[intCount].FindControl("txtWt")).Text.Trim());
                                Decimal Pcs = Decimal.Parse(((TextBox)grdRouting.Rows[intCount].FindControl("txtPcs")).Text.Trim());
                                if ((wt / Pcs) > AllowedWT)
                                {
                                    isHeavy = "1";
                                }
                            }
                            catch (Exception ex) { }

                            FltDetails.Append("Insert into #FltRoute ( Origin, Dest, FltNo, FltDate, Pcs , GWt , CWt , Carrier , IsPrime, IsHeavy)values ('");
                            FltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim().ToUpper());
                            FltDetails.Append("','");
                            FltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text.Trim().ToUpper());
                            FltDetails.Append("','");
                            FltDetails.Append(flight);
                            FltDetails.Append("', convert(datetime,'");
                            FltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtFdate")).Text.Trim());
                            FltDetails.Append("',103),");
                            FltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtPcs")).Text.Trim().Replace(",", ""));
                            FltDetails.Append(",");
                            FltDetails.Append(((TextBox)grdRouting.Rows[intCount].FindControl("txtWt")).Text.Trim().Replace(",", ""));
                            FltDetails.Append(",");
                            FltDetails.Append(ChrgWT);//((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text.Trim().Replace(",", ""));
                            FltDetails.Append(",'");
                            FltDetails.Append(carrier);
                            FltDetails.Append("',0,");
                            FltDetails.Append(isHeavy);
                            FltDetails.Append(");");
                        }
                    }
                }
                catch (Exception ex) { }
                #endregion

                #region PieceDetails
                //try
                //{
                //    DataSet dsPieces = (DataSet)Session["dsDimesionAll"];
                //    if (true)
                //    {

                //        if (dsPieces != null)
                //        {
                //            if (dsPieces.Tables.Count > 0)
                //            {
                //                if (dsPieces.Tables[0].Rows.Count > 0)
                //                {
                //                    for (int i = 0; i < dsPieces.Tables[0].Rows.Count; i++)
                //                    {
                //                        string isPHeavy = "0";
                //                        try
                //                        {
                //                            if (Convert.ToDecimal(dsPieces.Tables[0].Rows[i]["Wt"].ToString()) > AllowedWT)
                //                            {
                //                                isPHeavy = "1";
                //                            }
                //                        }
                //                        catch (Exception ex) { }
                //                        PieceDetails.Append("Insert into #PieceInfo values (1,");
                //                        PieceDetails.Append(dsPieces.Tables[0].Rows[i]["Wt"].ToString());//WT                                        
                //                        PieceDetails.Append(",");
                //                        PieceDetails.Append(isPHeavy);
                //                        PieceDetails.Append(",0);");


                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //catch (Exception ex) { }
                #endregion

                #region RoutePieceDetails
                try
                {
                    if (isHeavyShipmentOnRoute())
                    {
                        DataSet dsPieces = (DataSet)Session["dsRouteds"];
                        if (dsPieces != null)
                        {
                            if (dsPieces.Tables.Count > 0)
                            {
                                if (dsPieces.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsPieces.Tables[0].Rows.Count; i++)
                                    {
                                        string isPHeavy = "0";
                                        try
                                        {
                                            if (dsPieces.Tables[0].Rows[i]["isHeavy"].ToString().Equals("True", StringComparison.OrdinalIgnoreCase))
                                            {
                                                isPHeavy = "1";
                                            }
                                        }
                                        catch (Exception ex) { }
                                        RoutePieceDetails.Append("Insert into #PieceInfo values (");
                                        RoutePieceDetails.Append(dsPieces.Tables[0].Rows[i]["Pieces"].ToString());//Pcs
                                        RoutePieceDetails.Append(",");
                                        RoutePieceDetails.Append(dsPieces.Tables[0].Rows[i]["GrossWt"].ToString());//WT
                                        RoutePieceDetails.Append(",");
                                        RoutePieceDetails.Append(isPHeavy);//isHeavy
                                        RoutePieceDetails.Append(",");
                                        RoutePieceDetails.Append(dsPieces.Tables[0].Rows[i]["RowIndex"].ToString());//Row
                                        RoutePieceDetails.Append(");");


                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { }
                #endregion

                try
                {
                    string currency = drpCurrency.SelectedItem.ToString();
                    DateTime execdate = DateTime.ParseExact(txtExecutionDate.Text.Trim(), "dd/MM/yyyy", null);
                    string PayMode = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue.ToString();
                    if (ddlServiceclass.SelectedItem.Text.Trim() == "FOC")
                        PayMode = "FOC";
                    //string Origin = ddlOrg.Text.Trim();
                    //string Destination = ddlDest.Text.Trim();
                    bool IsVoided = false;
                    try
                    {
                        if (ddlServiceclass.SelectedItem.Text.Equals("Void", StringComparison.OrdinalIgnoreCase))
                        {
                            IsVoided = true;
                        }
                    }
                    catch (Exception ex) { }
                    int Pieces = Convert.ToInt16(((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs"))).Text.Trim());
                    decimal GrossWt = Convert.ToDecimal(((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt"))).Text.Trim());
                    decimal ChargeableWt = Convert.ToDecimal(((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt"))).Text.Trim());
                    try
                    {
                        decimal num = 0;
                        string Wt = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_Weight", ((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt"))).Text.Trim(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                        if (Decimal.TryParse(Wt, out num))
                        {
                            ChargeableWt = Convert.ToDecimal(Wt);
                            ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommChargedWt")).Text = Wt;
                            ((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt"))).Text = Wt;
                        }

                    }
                    catch (Exception ex)
                    {
                        ChargeableWt = Convert.ToDecimal(((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt"))).Text.Trim());
                    }
                    //string SHC = txtSpecialHandlingCode.Text.ToString().Trim(',');
                    string ProductType = "";
                    bool interline = chkInterline.Checked;
                    if (ddlProductType.SelectedIndex > 0)
                    {
                        ProductType = ddlProductType.SelectedItem.ToString();
                    }
                    #region ULDInfo
                    string ULDInfo = "";

                    DataTable dtULDInfo = (DataTable)Session["AWBULD"];

                    try
                    {
                        if (dtULDInfo != null && dtULDInfo.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtULDInfo.Rows.Count; i++)
                            {
                                if (dtULDInfo.Rows[i]["ULD"].ToString().Trim() != "")
                                {
                                    ULD.Append("insert into #ULDInfo (ULDNo,Pieces,Weight,ULDType ) values ('");
                                    ULD.Append(dtULDInfo.Rows[i]["ULD"].ToString());
                                    ULD.Append("',");
                                    ULD.Append(dtULDInfo.Rows[i]["Pcs"].ToString());
                                    ULD.Append(",");
                                    ULD.Append(dtULDInfo.Rows[i]["Wgt"].ToString());
                                    ULD.Append(",'");
                                    ULD.Append(dtULDInfo.Rows[i]["Type"].ToString());
                                    ULD.Append("');");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    DataSet dsDimensions = (DataSet)Session["dsDimesionAll"];
                    try
                    {
                        if (dsDimensions != null && dsDimensions.Tables.Count > 0 && dsDimensions.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsDimensions.Tables[0].Rows.Count; i++)
                            {
                                DataRow dr = dsDimensions.Tables[0].Rows[i];
                                if (dr["PieceType"].ToString().Equals("ULD", StringComparison.OrdinalIgnoreCase))
                                {
                                    ULD.Append("insert into #ULDInfo (ULDNo,Pieces,Weight,ULDType ) values ('");
                                    ULD.Append(dr["ULDNo"].ToString());
                                    ULD.Append("',1,");
                                    ULD.Append(dr["Wt"].ToString());
                                    ULD.Append(",'");
                                    ULD.Append(dr["ULDNo"].ToString().Substring(0, 3));
                                    ULD.Append("');");
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                    if (ULD.Length > 0)
                    {
                        ULDInfo = ULD.ToString();
                    }
                    #endregion

                    string AWBNumber = txtAWBNo.Text.Trim();
                    string TriggerPt = "B";
                    string Handler = ddlHandler.SelectedValue.ToString();
                    //decimal DVPrice = Convert.ToDecimal(TXTDvForCustoms.Text.ToString().Length > 0 ? TXTDvForCustoms.Text.ToString() : "0");
                    decimal DVPrice = Convert.ToDecimal(TXTDvForCarriage.Text.ToString().Length > 0 ? TXTDvForCarriage.Text.ToString() : "0");
                    try
                    {
                        decimal num = 0;
                        string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_DeclaredValue", TXTDvForCarriage.Text.ToString(), txtExecutionDate.Text.ToString(), TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                        if (Decimal.TryParse(Val, out num))
                        {
                            DVPrice = Convert.ToDecimal(Val);
                            TXTDvForCarriage.Text = Val;
                        }

                    }
                    catch (Exception ex)
                    {
                        DVPrice = Convert.ToDecimal(TXTDvForCarriage.Text.ToString().Length > 0 ? TXTDvForCarriage.Text.ToString() : "0");
                    }
                    #region Check DV Price to update SHC
                    try
                    {
                        if (DVPrice / ChargeableWt > 199)
                        {
                            if (!txtSpecialHandlingCode.Text.ToUpper().Contains("VAL"))
                                txtSpecialHandlingCode.Text = "VAL," + txtSpecialHandlingCode.Text;

                        }
                        else
                        {
                            txtSpecialHandlingCode.Text = txtSpecialHandlingCode.Text.Replace("VAL", "");
                        }
                    }
                    catch (Exception ez) { }
                    #endregion

                    string SHC = txtSpecialHandlingCode.Text.ToString().Trim(',');

                    string AWBPrefix = txtAwbPrefix.Text.ToString();
                    bool isExport = CHKExportShipment.Checked;
                    bool ScreenedReq = chkTBScreened.Checked;
                    string IssueCarrier = ddlAirlineCode.SelectedItem.ToString();
                    string shippercode = txtShipperCode.Text.Trim();
                    string consigneecode = txtConsigneeCode.Text.Trim();
                    decimal slac = 0;
                    try
                    {
                        if (decimal.TryParse(txtSLAC.Text, out slac))
                        {
                            slac = decimal.Parse(txtSLAC.Text.ToString().Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        slac = 0;
                    }
                    dsRates = objProcessRate.ProcessRates(agentcode, commcode, currency, execdate, PayMode, Origin, Destination, IsVoided, Pieces,
                        GrossWt, ChargeableWt, FltDetails.ToString(), SHC, ProductType, ULDInfo, AWBNumber, TriggerPt, DVPrice,
                        PieceDetails.ToString(), RoutePieceDetails.ToString(), interline, Handler, AWBPrefix, isExport, ScreenedReq,
                        IssueCarrier, shippercode, consigneecode, slac, "", CHKAllIn.Checked);


                }
                catch (Exception ex)
                {
                    dsRates = null;
                };

            }
            catch (Exception ex)
            {
                dsRates = null;
            }
            return dsRates;
        }
        #endregion

        #region LoadViability
        public void LoadViability()
        {
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();
            try
            {
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Origin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Destination";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "LegCarrier";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Flight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Charge";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ServTax";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Currency";
                myDataTable.Columns.Add(myDataColumn);
                DataRow dr;
                dr = myDataTable.NewRow();
                dr["Origin"] = "";
                dr["Destination"] = "";// "DEL";
                dr["LegCarrier"] = "";// "IT101";
                dr["Flight"] = "";// "14/Jan/2012";
                dr["Charge"] = "";// 
                dr["ServTax"] = "";// 
                dr["Charge"] = "";// 
                dr["Currency"] = "";// 
                myDataTable.Rows.Add(dr);
                GrDVia.DataSource = null;
                GrDVia.DataSource = myDataTable;
                GrDVia.DataBind();
                Session["Viability"] = myDataTable;
            }
            catch (Exception ec)
            {
                myDataTable = null;
                Ds = null;
            }
            finally
            {
                if (myDataTable != null)
                    myDataTable.Dispose();
                if (Ds != null)
                    Ds.Dispose();
            }

        }
        #endregion

        #region btnViabilityShow_Click
        protected void btnViabilityShow_Click(object sender, EventArgs e)
        {
            #region Viability Data
            try
            {
                BALProcessRates objProcessRate = new BALProcessRates();
                string currency = drpCurrency.SelectedItem.ToString();
                string commcode = "", agentcode = "";
                DateTime execdate = DateTime.ParseExact(txtExecutionDate.Text.Trim(), "dd/MM/yyyy", null);
                string PayMode = ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).SelectedValue.ToString();
                string Origin = ddlOrg.Text.Trim();
                string Destination = ddlDest.Text.Trim();
                bool IsVoided = false;
                int Pieces = Convert.ToInt16(((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs"))).Text.Trim());
                decimal GrossWt = Convert.ToDecimal(((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt"))).Text.Trim());
                decimal ChargeableWt = Convert.ToDecimal(((Label)(grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt"))).Text.Trim());
                string SHC = txtSpecialHandlingCode.Text.ToString().Trim(',');
                string ProductType = "";
                string org = "", dest = "", issuecarrier = "";
                float weight = 0;
                DateTime date;
                if (ddlProductType.SelectedIndex > 0)
                {
                    ProductType = ddlProductType.SelectedItem.ToString();
                }

                if (grdRouting.Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        string flight = "", isHeavy = "0";

                        string carrier = ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlPartner")).SelectedItem.Text.Trim();
                        try
                        {
                            if (((DropDownList)grdRouting.Rows[intCount].FindControl("ddlPartner")).SelectedItem.Text.Trim().Equals("other", StringComparison.OrdinalIgnoreCase))
                            {
                                flight = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFlightID")).Text.Trim();
                            }
                            else
                            {
                                flight = ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                            }
                        }
                        catch (Exception ex) { }

                        if (!carrier.Equals(Convert.ToString(Session["AirlinePrefix"]), StringComparison.OrdinalIgnoreCase))
                        {
                            org = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim().ToUpper();
                            dest = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text.Trim().ToUpper();
                            issuecarrier = Convert.ToString(Session["AirlinePrefix"]);
                            weight = float.Parse(((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text.Trim());
                            date = DateTime.ParseExact(((TextBox)grdRouting.Rows[intCount].FindControl("txtFdate")).Text.Trim(), "dd/MM/yyyy", null);
                            DataSet dsViability = objProcessRate.GetViabilityResult(org, dest, flight, carrier, weight, commcode, SHC, date, agentcode, ProductType, issuecarrier, currency);
                            if (dsViability != null)
                            {
                                if (dsViability.Tables.Count > 0)
                                {
                                    if (dsViability.Tables[0].Rows.Count > 0)
                                    {
                                        GrDVia.DataSource = null;
                                        GrDVia.DataSource = dsViability.Copy();
                                        GrDVia.DataBind();
                                        Session["Viability"] = dsViability.Tables[0];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            #endregion
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowViability();</script>", false);
        }
        #endregion

        #region btnViabilitySelect_Click
        protected void btnViabilitySelect_Click(object sender, EventArgs e)
        {
            try
            {//check
                //legcarrier//flight
                int count = 0, RowIndex = -1;
                for (int i = 0; i < GrDVia.Rows.Count; i++)
                {
                    bool check = ((CheckBox)(GrDVia.Rows[i].FindControl("check"))).Checked;
                    if (check)
                    {
                        count++;
                        RowIndex = i;
                    }
                }
                if (count > 1)
                {
                    lblEMsg.ForeColor = Color.Red;
                    lblEMsg.Text = "Selected Options exceeds from allowed.";

                }
                else if (count == 1 && RowIndex > (-1))
                {
                    string origin = ((TextBox)(GrDVia.Rows[RowIndex].FindControl("TXTOrigin"))).Text;
                    string dest = ((TextBox)(GrDVia.Rows[RowIndex].FindControl("dest"))).Text;
                    string Carrier = ((TextBox)(GrDVia.Rows[RowIndex].FindControl("legcarrier"))).Text;
                    string flight = ((TextBox)(GrDVia.Rows[RowIndex].FindControl("flight"))).Text.Trim();
                    lblEMsg.Text = "Carrier:" + Carrier + "Flight:" + flight;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {

                    }
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);

                }
                else if (count < 1)
                {
                    lblEMsg.ForeColor = Color.Red;
                    lblEMsg.Text = "No option Selected.";

                }
            }
            catch (Exception ex) { }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowViability();</script>", false);
        }
        #endregion



        #region GenerateAWBDimensions
        private DataSet GenerateAWBDimensions(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate, string AWBPrefix)
        {
            DataSet ds = null;
            BookingBAL BAL = new BookingBAL();
            int Rowcount = 0;
            string FlightNo = string.Empty, FlightDt = string.Empty;

            int intDimRowCount = 0;
            int FltPieces = 0;

            try
            {
                //if (Dimensions == null || Dimensions.Tables.Count < 1 || Dimensions.Tables[0].Rows.Count < 1)
                //{
                //    Dimensions = BAL.GenerateAWBDimensions(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
                //Convert.ToDateTime(Session["IT"]), false);
                //}

                if (Dimensions != null && Dimensions.Tables.Count > 0 && Dimensions.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        if (ddlOrg.Text.Trim() == ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text.Trim())
                        {
                            FlightNo = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text.Trim();
                            FlightDt = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFdate"))).Text.Trim();
                            FltPieces = Convert.ToInt32(((TextBox)(grdRouting.Rows[intCount].FindControl("txtPcs"))).Text.Trim());

                            for (int X = 0; X < FltPieces; X++)
                            {
                                Dimensions.Tables[0].Rows[intDimRowCount]["FlightNo"] = FlightNo;
                                Dimensions.Tables[0].Rows[intDimRowCount]["FlightDate"] = FlightDt;
                                intDimRowCount = intDimRowCount + 1;
                            }
                        }
                    }

                    ds = BAL.GenerateAWBDimensions(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
               Convert.ToDateTime(Session["IT"]), IsCreate, AWBPrefix, false);

                }
            }
            catch { }


            BAL = null;
            Dimensions = null;
            return ds;
        }
        #endregion

        #region GenerateAWBDimensions
        private DataSet GenerateAWBDimensions(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate, string AWBPrefix, string FlightNo, string FlightDate)
        {
            DataSet ds = null;
            BookingBAL BAL = new BookingBAL();
            int Rowcount = 0;
            // string FlightNo = string.Empty, FlightDt = string.Empty;

            int intDimRowCount = 0;
            int FltPieces = 0;

            try
            {
                //if (Dimensions == null || Dimensions.Tables.Count < 1 || Dimensions.Tables[0].Rows.Count < 1)
                //{
                //    Dimensions = BAL.GenerateAWBDimensions(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
                //Convert.ToDateTime(Session["IT"]), false);
                //}

                if (Dimensions != null && Dimensions.Tables.Count > 0 && Dimensions.Tables[0].Rows.Count > 0)
                {

                    ds = BAL.GenerateAWBDimensionsAcceptance(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
               Convert.ToDateTime(Session["IT"]), IsCreate, AWBPrefix, "1", FlightNo, FlightDate);

                }
            }
            catch { }


            BAL = null;
            Dimensions = null;
            return ds;
        }
        #endregion


        #region CheckAWBStatus
        private void CheckAWBStatus()
        {
            string AWBStatus = Convert.ToString(Session["AWBStatus"]);

            if (grdRouting.Rows.Count > 0)
            {
                for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                {
                    if (AWBStatus == "E")
                    {
                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtAcceptedPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtAcceptedWt")).Enabled = true;
                        if (!((CheckBox)grdRouting.Rows[intCount].FindControl("chkAccepted")).Checked)
                        {
                            ((CheckBox)grdRouting.Rows[intCount].FindControl("chkAccepted")).Enabled = true;
                        }
                    }
                    else
                    {
                        ((CheckBox)grdRouting.Rows[intCount].FindControl("chkAccepted")).Enabled = false;
                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtAcceptedPcs")).Enabled = false;
                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtAcceptedWt")).Enabled = false;
                    }
                }
            }

            for (int intCount = 0; intCount < grdMaterialDetails.Rows.Count; intCount++)
            {
                if (AWBStatus == "E")
                {
                    ((ImageButton)grdMaterialDetails.Rows[intCount].FindControl("btnPiecesPopup")).Enabled = false;
                }
                else
                {
                    ((ImageButton)grdMaterialDetails.Rows[intCount].FindControl("btnPiecesPopup")).Enabled = true;
                }
            }
        }
        #endregion

        #region GenerateSITAHeader
        private void GenerateSITAHeader(string receiverSITAID)
        {
            try
            {
                receiverSITAID = receiverSITAID.Replace(" ", ",");
                string[] sitareciver = receiverSITAID.Split(',');
                string Header = "=PRIORITY" + "\r\n" + "QP" + "\r\n" + "=DESTINATION TYPE B" + "\r\n";
                for (int i = 0; i < sitareciver.Length; i++)
                {
                    Header = Header + "STX," + sitareciver[i] + "\r\n";
                }
                Header = Header + "=SUBJ" + "\r\n" + "CIMP Message" + "\r\n" + "=TEXT";
                txtSITAHeader.Text = Header;
                txtSITAHeader.Visible = true;
                //receiverSITAID = receiverSITAID.Replace(',', ' ');
                //string SenderSITAID = objBLL.getSITAID(ddlAirlineCode.SelectedItem.Text.ToString());
                //txtSITAHeader.Text = "QP " + receiverSITAID;
                //txtSITAHeader.Text = txtSITAHeader.Text + "\r\n" + "." + SenderSITAID + " " + System.DateTime.Now.ToString("dd") + System.DateTime.Now.ToUniversalTime().ToString("hhMM") + " P25";

            }
            catch (Exception ex) { }
        }
        #endregion

        #region btnSitaUpload_Click
        protected void btnSitaUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMessageBody.Text.Length > 0 || txtSITAHeader.Text.Length > 0)
                {
                    FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + txtMessageBody.Text.ToString(), lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                    cls_BL.addMsgToOutBox("SITA:" + lblMsg.Text.ToString(), txtSITAHeader.Text.ToString() + "\r\n" + txtMessageBody.Text.ToString(), "", "SITAFTP");
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region btnFTPUpload_Click
        protected void btnFTPUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMessageBody.Text.Length > 0)
                {
                    FTP.Saveon72FTP(txtMessageBody.Text.ToString(), lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region SetOtherChargesSummary
        public void SetOtherChargesSummary(ref float OCDC, ref float OCDA, ref float OCTax, ref float OATax)
        {
            DataSet dsDetails = null;
            DataSet dsDetailsFinal = null;
            try
            {
                dsDetails = ((DataSet)Session["OCDetails"]);
                dsDetailsFinal = dsDetails.Clone();
                ArrayList list = new ArrayList();

                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    if (row["Charge Type"].ToString() == "DC" || row["Charge Type"].ToString() == "DA")
                    {
                        if (!list.Contains(row["Charge Head Code"].ToString()))
                        {

                            DataRow newrow = dsDetailsFinal.Tables[0].NewRow();
                            for (int i = 0; i < dsDetailsFinal.Tables[0].Columns.Count; i++)
                            {
                                newrow[i] = row[i];
                            }

                            dsDetailsFinal.Tables[0].Rows.Add(newrow);
                            list.Add(row["Charge Head Code"].ToString());
                        }
                        else
                        {
                            foreach (DataRow rw in dsDetailsFinal.Tables[0].Rows)
                            {

                                if (rw["Charge Head Code"].ToString() == row["Charge Head Code"].ToString())
                                {
                                    if (OCDCCurrency.Trim() != "")
                                    {
                                        if (OCDCCurrency.Trim().ToUpper() != AgentCurrency.Trim().ToUpper())
                                        {
                                            try
                                            {
                                                string Charge = "" + (float.Parse(rw["Charge"].ToString()) + float.Parse(row["Charge"].ToString()));
                                                string Tax = "" + (float.Parse(rw["Tax"].ToString()) + float.Parse(row["Tax"].ToString()));
                                                string Discount = "" + (float.Parse(rw["Discount"].ToString()) + float.Parse(row["Discount"].ToString()));
                                                string Commission = "" + (float.Parse(rw["Commission"].ToString()) + float.Parse(row["Commission"].ToString()));
                                                string[] val = new string[] { Charge, Tax, Discount, Commission };

                                            }
                                            catch (Exception ex)
                                            { }
                                        }
                                        else
                                        {
                                            rw["Charge"] = "" + (float.Parse(rw["Charge"].ToString()) + float.Parse(row["Charge"].ToString()));
                                            rw["Tax"] = "" + (float.Parse(rw["Tax"].ToString()) + float.Parse(row["Tax"].ToString()));
                                            rw["Discount"] = "" + (float.Parse(rw["Discount"].ToString()) + float.Parse(row["Discount"].ToString()));
                                            rw["Commission"] = "" + (float.Parse(rw["Commission"].ToString()) + float.Parse(row["Commission"].ToString()));
                                        }
                                    }
                                    else
                                    {
                                        rw["Charge"] = "" + (float.Parse(rw["Charge"].ToString()) + float.Parse(row["Charge"].ToString()));
                                        rw["Tax"] = "" + (float.Parse(rw["Tax"].ToString()) + float.Parse(row["Tax"].ToString()));
                                        rw["Discount"] = "" + (float.Parse(rw["Discount"].ToString()) + float.Parse(row["Discount"].ToString()));
                                        rw["Commission"] = "" + (float.Parse(rw["Commission"].ToString()) + float.Parse(row["Commission"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow newrow = dsDetailsFinal.Tables[0].NewRow();
                        for (int i = 0; i < dsDetailsFinal.Tables[0].Columns.Count; i++)
                        {
                            newrow[i] = row[i];
                        }
                        dsDetailsFinal.Tables[0].Rows.Add(newrow);
                    }
                }
                foreach (DataRow dr in dsDetailsFinal.Tables[0].Rows)
                {
                    if (dr["Charge Type"].ToString() == "DC")
                    {
                        OCDC += float.Parse(dr["Charge"].ToString());
                        OCTax += float.Parse(dr["Tax"].ToString());

                    }
                    else if (dr["Charge Type"].ToString() == "DA")
                    {
                        OCDA += float.Parse(dr["Charge"].ToString());
                        OATax += float.Parse(dr["Tax"].ToString());

                    }
                }

                Session["OCDetails"] = dsDetailsFinal;
            }
            catch (Exception ex)
            {
                dsDetails = null;
                dsDetailsFinal = null;
            }
            finally
            {
                if (dsDetails != null)
                    dsDetails.Dispose();
                if (dsDetailsFinal != null)
                    dsDetailsFinal.Dispose();
            }
        }
        #endregion

        #region ValidateMasterEntry
        private bool ValidateMasterEntry(string Airport, string Level)
        {
            bool isPresent = false;
            try
            {
                BALAirportMaster objBAL = new BALAirportMaster();
                isPresent = objBAL.CheckAirportMasterEntry(Airport, Level);
                objBAL = null;
            }
            catch (Exception ex)
            {
                isPresent = false;
            }
            return isPresent;
        }
        #endregion

        #region AutoMessagingFunction
        private void AutoMessagingFunction()
        {
            DataSet ds = new DataSet();
            try
            {
                string AWBNumber = "", error = "";

                if (HidAWBNumber.Value.ToString().Trim() != "")
                {
                    AWBNumber = HidAWBNumber.Value.ToString().Trim();
                }
                else if (txtAWBNo.Text.Trim() == "")
                {
                    AWBNumber = txtAWBNo.Text.Trim();
                }
                else
                {
                    lblStatus.Text = "Please Provide AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (AWBNumber.Length > 0)
                {
                    BALEmailID ObjEmail = new BALEmailID();
                    ds = ObjEmail.GetEmail(ddlOrg.SelectedItem.Text, ddlDest.SelectedItem.Text, "FBL", "", TXTAgentCode.Text.ToString());
                    if (ds != null)
                    {
                        txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                        lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                        if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                        {
                            GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                        }
                    }
                    if (objBLL.GetAWBDetails(AWBNumber, txtAwbPrefix.Text.ToString(), ref ds, ref error))
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    #region FFR Message
                                    string Msg = cls_BL.EncodeFFR(ds, ref error);
                                    if (Msg.Length > 3)
                                    {
                                        //save FFR message to outbox
                                        if (txtSITAHeader.Text.Length > 0)
                                        {
                                            FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + Msg, lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                                            cls_BL.addMsgToOutBox("SITA:" + lblMsg.Text.ToString(), txtSITAHeader.Text.ToString() + "\r\n" + Msg, "", "SITAFTP");
                                        }
                                        else
                                        {
                                            cls_BL.addMsgToOutBox("FFR", txtSITAHeader.Text.Trim() + "\r\n" + Msg, "", "");
                                        }
                                    }
                                    else if (error.Length > 0)
                                    {
                                        lblStatus.Text = "IATA FFR Message Error:" + error;
                                    }
                                    #endregion

                                    #region FWB Message
                                    string fwbMsg = cls_BL.EncodeFWB(ds, ref error);
                                    if (fwbMsg.Length > 3)
                                    {
                                        //save FFR message to outbox
                                        if (txtSITAHeader.Text.Length > 0)
                                        {
                                            FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + fwbMsg, lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                                            cls_BL.addMsgToOutBox("SITA:" + lblMsg.Text.ToString(), txtSITAHeader.Text.ToString() + "\r\n" + fwbMsg, "", "SITAFTP");
                                        }
                                        else
                                        {
                                            cls_BL.addMsgToOutBox("FWB", txtSITAHeader.Text.Trim() + "\r\n" + fwbMsg, "", "");
                                        }
                                    }
                                    else if (error.Length > 0)
                                    {
                                        lblStatus.Text = "IATA FWB Message Error:" + error;
                                    }
                                    #endregion

                                    #region FHL Message
                                    string fhl = cls_BL.EncodeFHLForSend(AWBNumber, txtAwbPrefix.Text, ref error);
                                    if (fhl.Length > 3)
                                    {
                                        //Save Mesage to outbox
                                        if (txtSITAHeader.Text.Length > 0)
                                        {
                                            FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + fhl, lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                                            cls_BL.addMsgToOutBox("SITA:" + lblMsg.Text.ToString(), txtSITAHeader.Text.ToString() + "\r\n" + fhl, "", "SITAFTP");
                                        }
                                        else
                                        {
                                            cls_BL.addMsgToOutBox("FHL", txtSITAHeader.Text.Trim() + "\r\n" + fhl, "", "");
                                        }
                                    }
                                    else if (error.Length > 0)
                                    {
                                        lblStatus.Text = "IATA FHL Message Error:" + error;
                                        return;
                                    }
                                    #endregion

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion

        #region Save Template
        protected void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string AWBNumber = HidAWBNumber.Value.Trim();
                string AWBPrefix = txtAwbPrefix.Text.Trim();
                //strUserName
                if (objBLL.SaveAsTemplate(AWBPrefix, AWBNumber, strUserName))
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWB Template saved successfully!";

                }

            }
            catch (Exception ex) { }
        }
        #endregion


        private bool SaveAcceptanceSummary(int AcceptedPcs, decimal AcceptedWt, string CommodityCode)
        {
            BalGHADockAccp objAcceptance = new BalGHADockAccp();
            object[] AWBParams = new object[8];
            int i = 0;
            bool blnResult = false;
            string AWBNumber = txtAWBNo.Text.Trim();
            string SHCCode = txtSpecialHandlingCode.Text.Trim();

            try
            {
                //1
                AWBParams.SetValue(AWBNumber, i);
                i++;

                //2
                AWBParams.SetValue(AcceptedPcs, i);
                i++;

                //3
                AWBParams.SetValue(AcceptedWt, i);
                i++;

                //4
                AWBParams.SetValue(Convert.ToDateTime(Session["IT"]), i);
                i++;

                //5
                AWBParams.SetValue(Convert.ToString(Session["UserName"]), i);
                i++;

                //6
                AWBParams.SetValue(SHCCode, i);
                i++;

                //7
                AWBParams.SetValue(CommodityCode, i);
                i++;

                //8
                AWBParams.SetValue(true, i);

                blnResult = objAcceptance.SaveGHAAcceptanceData(AWBParams);
            }
            catch (Exception ex)
            {
                blnResult = false;
            }
            finally
            {
                objAcceptance = null;
                AWBParams = null;
            }

            return blnResult;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetPartnerPrefix(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            SqlDataAdapter dad = null;
            DataSet ds = new DataSet();
            List<string> list = null;

            try
            {
                dad = new SqlDataAdapter("SELECT DISTINCT PartnerPrefix + '-' + PartnerName+'('+PartnerCode+')' FROM dbo.tblPartnerMaster WHERE isActive = 1 AND PartnerPrefix IS NOT NULL", con);
                dad.Fill(ds);
                list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                dad = null;
                ds = null;
            }

            return list.ToArray();
        }

        protected void btnDGRLbl_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDGR = (DataTable)Session["DgrCargo"];
                if (dtDGR == null || dtDGR.Rows.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DGRAlert", "<SCRIPT LANGUAGE='javascript'>DGRAlert();</script>", false);
                    return;
                }
                radPrnAllLbl.Checked = true;
                radPrnSelLbl.Checked = false;
                txtPrnLblFrom.Text = txtPrnLblTo.Text = string.Empty;
                btnPrintSelLbl.Visible = false;
                btnPrintSelDGRLbl.Visible = true;
                lblErrorMsg.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
            }
            catch (Exception ex) { }

        }

        protected void btnPrintSelDGRLbl_Click(object sender, EventArgs e)
        {
            string pcs = ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.ToString();

            try
            {
                lblErrorMsg.Text = string.Empty;
                int FromLbl, ToLbl;
                int totPcs = int.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.ToString());
                if (radPrnAllLbl.Checked)
                {
                    string awb = txtAwbPrefix.Text + txtAWBNo.Text;
                    string org = ddlOrg.SelectedItem.Text;
                    string dest = ddlDest.SelectedItem.Text;

                    string frmPcs, toPcs;
                    frmPcs = "1";
                    toPcs = pcs;

                    DataTable dtDGR = (DataTable)Session["DgrCargo"];
                    if (dtDGR == null || dtDGR.Rows.Count <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DGRAlert", "<SCRIPT LANGUAGE='javascript'>DGRAlert();</script>", false);
                        return;
                    }
                    if (dtDGR != null && dtDGR.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDGR.Rows.Count; i++)
                        {
                            //awb = dtDGR.Rows[i]["AWBNumber"].ToString();
                            HdnUNIDForDGRLbl.Value = dtDGR.Rows[0]["UNID"].ToString();
                            HdnPcsForDGRLbl.Value = dtDGR.Rows[0]["Pieces"].ToString();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>popup('" + HdnUNIDForDGRLbl.Value + "','" + org + "','" + dest + "','" + awb + "','" + HdnPcsForDGRLbl.Value + "','" + pcs + "','" + frmPcs + "','" + toPcs + "');</script>", false);
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>popup('1005','bom','del','58981515604','100');</script>", false);
                    }
                }
                else
                {
                    if (txtPrnLblFrom.Text.Trim() == "" || txtPrnLblTo.Text.Trim() == "")
                    {
                        lblErrorMsg.Text = "Values cannot be balnk...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    try
                    {
                        FromLbl = int.Parse(txtPrnLblFrom.Text);
                        ToLbl = int.Parse(txtPrnLblTo.Text);
                    }
                    catch (Exception ex)
                    {
                        lblErrorMsg.Text = "Enter digits only...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    if (FromLbl <= 0 || ToLbl <= 0)
                    {
                        lblErrorMsg.Text = "Enter digits greater than 0...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    else if (ToLbl < FromLbl)
                    {
                        lblErrorMsg.Text = "To Lbl cannot be less than From Lbl...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }

                    else if (FromLbl > totPcs || ToLbl > totPcs)
                    {
                        lblErrorMsg.Text = "Pcs cannot be greater than total pcs...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    else
                    {
                        #region Print Sel
                        string awb = txtAwbPrefix.Text + txtAWBNo.Text;
                        string org = ddlOrg.SelectedItem.Text;
                        string dest = ddlDest.SelectedItem.Text;

                        string frmPcs, toPcs;
                        frmPcs = txtPrnLblFrom.Text;
                        toPcs = txtPrnLblTo.Text;

                        DataTable dtDGR = (DataTable)Session["DgrCargo"];
                        if (dtDGR == null || dtDGR.Rows.Count <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DGRAlert", "<SCRIPT LANGUAGE='javascript'>DGRAlert();</script>", false);
                            return;
                        }
                        if (dtDGR != null && dtDGR.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtDGR.Rows.Count; i++)
                            {
                                //awb = dtDGR.Rows[i]["AWBNumber"].ToString();
                                HdnUNIDForDGRLbl.Value = dtDGR.Rows[i]["UNID"].ToString();
                                HdnPcsForDGRLbl.Value = ((int.Parse(txtPrnLblTo.Text) + 1) - int.Parse(txtPrnLblFrom.Text)).ToString();
                            }
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>popup();</script>", false);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>popup('" + HdnUNIDForDGRLbl.Value + "','" + org + "','" + dest + "','" + awb + "','" + HdnPcsForDGRLbl.Value + "','" + pcs + "','" + frmPcs + "','" + toPcs + "');</script>", false);
                        }
                        #endregion
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit_SelLbl();</script>", false);
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void LoadSystemParameters()
        {
            LoginBL objBL = new LoginBL();
            string SmartKargoInstance = string.Empty;
            string ManifestPartner = string.Empty;
            string FlightValidation = string.Empty;

            //if (CommonUtility.SmartKargoInstance == null || CommonUtility.SmartKargoInstance == "")
            //{
            //    SmartKargoInstance = objBL.GetMasterConfiguration("SmartKargoInstance");

            //    if (SmartKargoInstance != "")
            //        Session["SKI"] = SmartKargoInstance; // Smart Kargo Instance
            //    else
            //        Session["SKI"] = "AR";
            //}

            //if (Session["MPA"] == null || Convert.ToString(Session["MPA"]) == "")
            //{
            //    ManifestPartner = objBL.GetMasterConfiguration("ManifestPartner");

            //    if (ManifestPartner != "")
            //        Session["MPA"] = Convert.ToBoolean(ManifestPartner); // Manifest Partner Airlines
            //    else
            //        Session["MPA"] = false;
            //}

            //if (Session["FRV"] == null || Convert.ToString(Session["FRV"]) == "")
            //{
            //    FlightValidation = objBL.GetMasterConfiguration("FlightValidation");

            //    if (FlightValidation != "")
            //        Session["FRV"] = Convert.ToBoolean(FlightValidation); // FlightRouteValidation
            //    else
            //        Session["FRV"] = true;
            //}
            //Get if Flight Number is Mandatory for booking or not.
            if (Session["ConBooking_FlightNumMandatory"] == null || Session["ConBooking_FlightNumMandatory"].ToString() == "")
            {
                //Session["ConBooking_FlightNumMandatory"] = objBL.GetMasterConfiguration("FltNumMandatoryInBooking").ToString();
                Session["ConBooking_FlightNumMandatory"] = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "FltNumMandatoryInBooking");
            }

            //Get Shipper Mandatory During.
            //if (Session["ShipperMandatoryDuring"] == null || Session["ShipperMandatoryDuring"].ToString() == "")
            //{
            //    Session["ShipperMandatoryDuring"] = objBL.GetMasterConfiguration("ValidateShipperDuring").ToString();
            //}
            //Set flag for shipper validation

            CommonUtility.ShipperMandatoryDuring = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateShipperDuring");
            CommonUtility.ConsigneeMandatoryDuring = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateConsigneeDuring");

            if (CommonUtility.ShipperMandatoryDuring != null && CommonUtility.ShipperMandatoryDuring.ToUpper() == "BK")
            {
                HidShipper.Value = "1";
            }
            else
            {
                HidShipper.Value = "0";
            }
            //Get Consignee Mandatory During.
            //if (Session["ConsigneeMandatoryDuring"] == null || Session["ConsigneeMandatoryDuring"].ToString() == "")
            //{
            //    Session["ConsigneeMandatoryDuring"] = objBL.GetMasterConfiguration("ValidateConsigneeDuring").ToString();
            //}
            //Set flag for cosnginee validation
            if (CommonUtility.ConsigneeMandatoryDuring != null && CommonUtility.ConsigneeMandatoryDuring.ToUpper() == "BK")
            {
                HidConsignee.Value = "1";
            }
            else
            {
                HidConsignee.Value = "0";
            }
            //Get Known Shipper Validation configuration.
            if (Session["ConBooking_ValidateKnownShipper"] == null || Session["ConBooking_ValidateKnownShipper"].ToString() == "")
            {
                //Session["ConBooking_ValidateKnownShipper"] = objBL.GetMasterConfiguration("KnownShipper").ToString();
                Session["ConBooking_ValidateKnownShipper"] = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "KnownShipper");
            }
            //Get configuration to Allow editing of Rates from Rate Details grid.
            if (Session["ConBooking_AllowRatesEdit"] == null || Session["ConBooking_AllowRatesEdit"].ToString() == "")
            {
                //Session["ConBooking_AllowRatesEdit"] = objBL.GetMasterConfiguration("AllowRatesEdit").ToString();
                Session["ConBooking_AllowRatesEdit"] = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AllowRatesEdit");
            }
            try
            {
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("handleCIMPMsg"))) 
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "handleCIMPMsg")))
                {
                    btnSendFFR.Visible = false;
                    btnSendFHL.Visible = false;
                    btnSenfwb.Visible = false;
                    btnSendPHL.Visible = false;
                    btnSendPWB.Visible = false;
                }
            }
            catch (Exception ex) { }
            try
            {
                //if (Convert.ToBoolean(objBL.GetMasterConfiguration("AcceptPartnerAWB")))
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AcceptPartnerAWB")))
                {
                    txtAwbPrefix.Enabled = true;
                }
            }
            catch (Exception ex) { }
            //try
            //{
            //    if (!Convert.ToBoolean(objBL.GetMasterConfiguration("DemoInstance")))
            //    {
            //        btnDGRLbl.Visible = false;
            //        CHKConsole.Visible = false;
            //        CHKBonded.Visible = false;
            //        CHKExportShipment.Visible = false;
            //        CHKAsAggred.Visible = false;
            //        chkTBScreened.Visible = false;
            //        BtnViability.Visible = false;

            //        GrdRateDetails.Rows[0].FindControl("TXTSpotFreight").Visible = false;
            //        GrdRateDetails.Rows[0].FindControl("TXTSpotRate").Visible = false;
            //        btnCargoReceipt.Visible = false;
            //        btnHABDetails.Visible = false;
            //        btnSaveTemplate.Visible = false;
            //    }
            //}
            //catch (Exception ex) { }

            try
            {
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("SupportHAWB")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SupportHAWB")))
                {
                    btnADDHAWB.Visible = false;
                }
            }
            catch (Exception ex) { }

            try
            {
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Session["ConfigXML"].ToString(), "SupportSLAC")))
                {
                    lblSLAC.Visible = false;
                    txtSLAC.Visible = false;
                }
            }
            catch (Exception ex) { }

            try
            {
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("SupportHAWB")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SupportHAWB")))
                {
                    btnADDHAWB.Visible = false;
                }
            }
            catch (Exception ex) { }

            try
            {
                //bool flag = Convert.ToBoolean(objBL.GetMasterConfiguration("SupportBagging").ToString());
                bool flag = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SupportBagging"));
                for (int i = 0; i <= grdMaterialDetails.Rows.Count; i++)
                {
                    if (!flag)
                    {
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlShipmentType")).Items.Remove("Bags");

                    }
                    if (!Convert.ToBoolean(Session["ULDACT"].ToString()))
                    {
                        ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlShipmentType")).Items.Remove("ULD");
                        if (!flag)
                            ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlShipmentType")).Items.Remove("Mixed");
                    }

                }


            }
            catch (Exception ex) { }
            try
            {
                Session["GenerateWalkingInvoiceShow"] = null;
                //if (Convert.ToBoolean(objBL.GetMasterConfiguration("GenerateWalkingInvoiceShow")))
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "GenerateWalkingInvoiceShow")))
                {
                    Session["GenerateWalkingInvoiceShow"] = true;
                }
            }
            catch (Exception ex) { }
            try
            {
                //string countrycode = objBL.GetMasterConfiguration("DefaultCountry").ToString();
                string countrycode = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DefaultCountry");
                ddlConCountry.SelectedValue = countrycode;
                ddlShipCountry.SelectedValue = countrycode;

            }
            catch (Exception ex) { }

            objBL = null;
        }

        protected void txtAwbPrefix_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (txtAwbPrefix.Text.Length > 0)
                {
                    if (txtAwbPrefix.Text.Trim().IndexOf('-') > -1)
                        txtAwbPrefix.Text = txtAwbPrefix.Text.Trim().Substring(0, txtAwbPrefix.Text.Trim().IndexOf('-'));

                    string Design = objBLL.GetDesignatorCode(txtAwbPrefix.Text);
                    if (Design.Length > 0)
                    {
                        ddlAirlineCode.Text = Design;
                    }
                    else
                        ddlAirlineCode.Text = "";
                }
            }
            catch (Exception ex) { }
        }

        private string ValidateULDFlow()
        {
            DataSet dsDimensions = (DataSet)Session["dsDimesionAll"];
            string strULDNumbers = string.Empty;
            BookingBAL objBAL = new BookingBAL();
            string strResult = string.Empty;

            try
            {
                string FlightNo = "";
                string partner = "";
                //Check partner code.
                if (((DropDownList)(grdRouting.Rows[0].FindControl("ddlPartner"))).SelectedIndex >= 0)
                {
                    partner = ((DropDownList)(grdRouting.Rows[0].FindControl("ddlPartner"))).SelectedItem.Text.Trim();
                }
                if (partner.ToLower() == "other")
                {   //If Other is selected in partner code text box.
                    FlightNo = ((TextBox)(grdRouting.Rows[0].FindControl("txtFlightID"))).Text.Trim();
                }
                else
                {
                    if (((DropDownList)(grdRouting.Rows[0].FindControl("ddlFltNum"))).SelectedIndex >= 0)
                        FlightNo = ((DropDownList)(grdRouting.Rows[0].FindControl("ddlFltNum"))).SelectedItem.Text;
                }
                //FlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim();

                DateTime FlightDt = DateTime.MinValue;
                if (((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text.Trim() != "")
                    FlightDt = DateTime.ParseExact(((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text.Trim(), "dd/MM/yyyy", null);
                else
                    FlightDt = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                string Origin = Convert.ToString(Session["Station"]);

                if (dsDimensions != null && dsDimensions.Tables.Count > 0 && dsDimensions.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dsDimensions.Tables[0].Rows.Count; intCount++)
                    {
                        if (dsDimensions.Tables[0].Rows[intCount]["PieceType"].ToString() == "ULD")
                            strULDNumbers = strULDNumbers + dsDimensions.Tables[0].Rows[intCount]["ULDNo"].ToString() + ",";
                    }
                    strULDNumbers = strULDNumbers.Substring(0, strULDNumbers.Length - 1);
                    strResult = objBAL.ValidateULDFlow(strULDNumbers, FlightNo, FlightDt, Origin,
                        txtAWBNo.Text, txtAwbPrefix.Text);
                    return strResult;
                }
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (dsDimensions != null)
                    dsDimensions.Dispose();
                objBAL = null;
            }
        }

        private void LoadAWBMasterData()
        {
            BookingBAL objBAL = new BookingBAL();
            DataSet objDS = objBAL.LoadAWBMasterData();
            //LoginBL objBal = new LoginBL();
            HomeBL objBLL = new HomeBL();
            string restStation = "false";

            if (objDS != null)
            {
                ddlAirlineCode.Items.Clear();
                //Loading designator codes
                if (objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < objDS.Tables[0].Rows.Count; intCount++)
                    {
                        ddlAirlineCode.Items.Add(objDS.Tables[0].Rows[intCount]["DesignatorCode"].ToString());
                    }
                    ddlAirlineCode.Items.Add("");
                }

                //Loading Cnote Master codes
                if (objDS.Tables.Count > 1 && objDS.Tables[1].Rows.Count > 0)
                {
                    ddlDocType.DataSource = objDS.Tables[1];
                    ddlDocType.DataMember = objDS.Tables[1].TableName;
                    ddlDocType.DataTextField = "AWBCnoteMaster";
                    ddlDocType.DataValueField = "AWBCnoteMaster";
                    ddlDocType.DataBind();
                }

                //Loading Country Master codes
                if (objDS.Tables.Count > 2 && objDS.Tables[2].Rows.Count > 0)
                {
                    objDS.Tables[2].Rows.Add("", "");
                    ddlShipCountry.DataSource = objDS;
                    ddlShipCountry.DataMember = objDS.Tables[2].TableName;
                    ddlShipCountry.DataTextField = "CountryName";
                    ddlShipCountry.DataValueField = "CountryCode";
                    ddlShipCountry.DataBind();
                    ddlShipCountry.SelectedValue = "";

                    ddlConCountry.DataSource = objDS;
                    ddlConCountry.DataMember = objDS.Tables[2].TableName;
                    ddlConCountry.DataTextField = "CountryName";
                    ddlConCountry.DataValueField = "CountryCode";
                    ddlConCountry.DataBind();
                    ddlConCountry.SelectedValue = "";

                    ddlShipperCountryHAWB.DataSource = objDS;
                    ddlShipperCountryHAWB.DataMember = objDS.Tables[2].TableName;
                    ddlShipperCountryHAWB.DataTextField = "CountryName";
                    ddlShipperCountryHAWB.DataValueField = "CountryCode";
                    ddlShipperCountryHAWB.DataBind();

                    ddlConsigneeCountryHAWB.DataSource = objDS;
                    ddlConsigneeCountryHAWB.DataMember = objDS.Tables[2].TableName;
                    ddlConsigneeCountryHAWB.DataTextField = "CountryName";
                    ddlConsigneeCountryHAWB.DataValueField = "CountryCode";
                    ddlConsigneeCountryHAWB.DataBind();
                }

                //Loading Currency Master codes
                if (objDS.Tables.Count > 3 && objDS.Tables[3].Rows.Count > 0)
                {
                    drpCurrency.Items.Clear();
                    drpCurrency.DataSource = objDS.Tables[3];
                    drpCurrency.DataTextField = "Code";
                    drpCurrency.DataValueField = "ID";
                    drpCurrency.DataBind();
                    drpCurrency.SelectedIndex = drpCurrency.Items.IndexOf(drpCurrency.Items.FindByText("INR"));
                }
                else
                {
                    drpCurrency.Items.Clear();
                    drpCurrency.SelectedIndex = 0;
                }

                //Loading Irregularity Master codes
                if (objDS.Tables.Count > 4 && objDS.Tables[4].Rows.Count > 0)
                {
                    ddlIrregularityCode.Items.Clear();
                    ddlIrregularityCode.DataSource = objDS.Tables[4];
                    ddlIrregularityCode.DataTextField = "IrregularityCode";
                    ddlIrregularityCode.DataValueField = "ID";
                    ddlIrregularityCode.DataBind();
                    ddlIrregularityCode.SelectedIndex = 0;
                }

                //Loading Product Types
                if (objDS.Tables.Count > 5 && objDS.Tables[5].Rows.Count > 0)
                {
                    ddlProductType.Items.Clear();
                    ddlProductType.DataSource = objDS.Tables[5];
                    ddlProductType.DataTextField = "ProductType";
                    ddlProductType.DataValueField = "SerialNumber";
                    ddlProductType.DataBind();
                    ddlProductType.SelectedIndex = 0;
                }

                //Loading Airport codes
                 DataSet dsResult = new DataSet("GHABooking_dsResult_LoadStations");
                 restStation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RestrictStationAccess");
                // restStation = objBal.GetMasterConfiguration("RestrictStationAccess");
            try
            {
                if (restStation == "true" && restStation != null&&restStation!="")
                {
                    object[] UserParams = new string[1];
                    int i = 0;
                    UserParams.SetValue(Session["UserName"].ToString(), i);

                    DataSet ds = new DataSet("MasterPage_getStationLB_ds");
                    ds = objBLL.GetUserRollDetails(UserParams);
                    if (ddlOrg.DataSource == null||ddlOrg.DataSource=="")
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            //DataRow row = ds.Tables[2].NewRow();
                            //row["StationCode"] = "Select";
                            //ds.Tables[2].Rows.Add(row);
                            ddlOrg.DataSource = null;
                            ddlOrg.DataSource = ds;
                            ddlOrg.DataMember = ds.Tables[2].TableName;
                            ddlOrg.DataValueField = ds.Tables[2].Columns[0].ColumnName;
                            ddlOrg.DataTextField = ds.Tables[2].Columns[0].ColumnName;
                            ddlOrg.DataBind();
                            //ddlOrg.Text = "Select";
                            ddlOrg.Text = Session["Station"].ToString();

                        }
                    }
                    //dsResult = new DataSet("ListBooking_dsResult_LoadStations");
                    if (objDS.Tables.Count > 6 && objDS.Tables[6].Rows.Count > 0)
                    {
                        DataRow row = objDS.Tables[6].NewRow();

                        row["AirportCode"] = "Select";
                        objDS.Tables[6].Rows.Add(row);
                        ddlDest.DataSource = objDS.Tables[6];
                        ddlDest.DataMember = objDS.Tables[6].TableName;
                        ddlDest.DataTextField = "AirportCode";
                        ddlDest.DataValueField = "AirportCode";
                        ddlDest.DataBind();

                        ddlDest.Text = "Select";

                        Session["AirportCode"] = objDS.Tables[6].Copy();


                    }
                    ds = null;
                }
                else
                {
                    if (objDS.Tables.Count > 6 && objDS.Tables[6].Rows.Count > 0)
                    {
                        DataRow row = objDS.Tables[6].NewRow();

                        row["AirportCode"] = "Select";
                        objDS.Tables[6].Rows.Add(row);

                        ddlOrg.DataSource = objDS.Tables[6];
                        ddlOrg.DataMember = objDS.Tables[6].TableName;
                        ddlOrg.DataTextField = "AirportCode";
                        ddlOrg.DataValueField = "AirportCode";
                        ddlOrg.DataBind();

                        ddlOrg.Text = "Select";

                        ddlDest.DataSource = objDS.Tables[6];
                        ddlDest.DataMember = objDS.Tables[6].TableName;
                        ddlDest.DataTextField = "AirportCode";
                        ddlDest.DataValueField = "AirportCode";
                        ddlDest.DataBind();

                        ddlDest.Text = "Select";

                        Session["AirportCode"] = objDS.Tables[6].Copy();

                    }
                }
            }
            catch (Exception ex)
            { }
                //Loading Pay Modes.
                if (objDS.Tables.Count > 7 && objDS.Tables[7].Rows.Count > 0)
                {
                    Session["ConBooking_PayModeMaster"] = objDS.Tables[7].Copy();
                }

                objDS = null;
            }
        }

        public void AutoPopulateTemplate(bool editable, string TemplateID, string state)
        {
            DataSet dsResult = new DataSet();
            try
            {

                string errormessage = "";

                if (objBLL.GetAWBTemplateDetails(TemplateID, ref dsResult, ref errormessage))
                {
                    // AWBSummaryMaster
                    txtAWBNo.Text = "";//AWBNumber;
                    HidAWBNumber.Value = "";// AWBNumber;
                    ddlDocType.Text = "" + dsResult.Tables[0].Rows[0]["DocumentType"].ToString();
                    txtAwbPrefix.Text = "" + dsResult.Tables[0].Rows[0]["AWBPrefix"].ToString();
                    chkDlvOnHAWB.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["IsDlvOnHAWB"]);
                    ddlAirlineCode.Text = dsResult.Tables[0].Rows[0]["DesignatorCode"].ToString();
                    TXTCustomerCode.Text = dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();
                    Session["UpdtOn"] = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["UpdatedOn"]);
                    chkTBScreened.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["ToBeScreened"]);

                    try
                    {
                        chkInterline.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["Interline"].ToString());
                    }
                    catch (Exception EX) { }
                    try
                    {
                        txtAttchDoc.Text = dsResult.Tables[0].Rows[0]["Documents"].ToString();
                    }
                    catch (Exception ex) { }
                    /* Commented on 31/7/2013
                     * Interline check box not shown
                     * if (chkInterline.Checked)
                     {
                         ddlDest.Visible = false;
                         txtDest.Visible = true;
                         txtDest.Text = dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();
                         ddlAirlineCode.Text = "";
                         ddlAirlineCode.Visible = false;
                      
                        
                     }
                     else
                     {
                         ddlDest.Visible = true;
                         txtDest.Visible = false;
                         ddlAirlineCode.Text = Session["awbPrefix"].ToString();
                         ddlAirlineCode.Visible = true;
                        
                     }*/

                    ddlOrg.Items.Clear();
                    ddlOrg.Items.Add("" + dsResult.Tables[0].Rows[0]["OriginCode"].ToString());
                    ddlOrg.SelectedIndex = 0;

                    Session["Origin"] = dsResult.Tables[0].Rows[0]["OriginCode"].ToString();
                    Session["Destination"] = dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();

                    SetOrgDest(dsResult.Tables[0].Rows[0]["OriginCode"].ToString(), dsResult.Tables[0].Rows[0]["DestinationCode"].ToString());

                    ddlDest.SelectedItem.Text = "" + dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();
                    ddlDest.Text = "" + dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();
                    ddlServiceclass.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["ServiceCargoClassId"].ToString());

                    CHKAsAggred.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Agreed"]);
                    CHKBonded.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Bonded"]);
                    CHKConsole.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Console"]);
                    CHKExportShipment.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Export"]);

                    FillHandlerCodes();

                    ddlHandler.SelectedValue = dsResult.Tables[0].Rows[0]["Handler"].ToString();

                    //ddlAgtCode.Items.Clear();
                    //ddlAgtCode.Items.Add(new ListItem("" + dsResult.Tables[0].Rows[0]["AgentCode"].ToString(), "" + dsResult.Tables[0].Rows[0]["AgentName"].ToString()));
                    //ddlAgtCode.SelectedIndex = 0;

                    TXTAgentCode.Text = dsResult.Tables[0].Rows[0]["AgentCode"].ToString();
                    txtAgentName.Text = dsResult.Tables[0].Rows[0]["AgentName"].ToString();
                    txtRemarks.Text = dsResult.Tables[0].Rows[0]["Remarks"].ToString();
                    txtSpecialHandlingCode.Text = dsResult.Tables[0].Rows[0]["SHCCodes"].ToString();


                    txtAgentName.Text = "" + dsResult.Tables[0].Rows[0]["AgentName"].ToString();
                    txtHandling.Text = "" + dsResult.Tables[0].Rows[0]["HandlingInfo"].ToString();
                    TXTCustomerCode.Text = "" + dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();

                    txtDriverName.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["DriverName"]);
                    txtDriverDL.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["DLNumber"]);
                    txtPhoneNo.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["PhoneNumber"]);
                    txtVehicleNo.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["VehicleNo"]);

                    txtIACCode.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["IACCode"]);
                    txtKnownShipper.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["KnownShipper"]);
                    txtCCSF.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["CCSFCode"]);

                    TXTDvForCustoms.Text = "" + dsResult.Tables[0].Rows[0]["DVCustom"].ToString();
                    TXTDvForCarriage.Text = "" + dsResult.Tables[0].Rows[0]["DVCarriage"].ToString();
                    txtSLAC.Text = dsResult.Tables[0].Rows[0]["SLAC"].ToString();
                    txtCustoms.Text = dsResult.Tables[0].Rows[0]["Customs"].ToString();
                    txtEURIN.Text = dsResult.Tables[0].Rows[0]["EURIN"].ToString();
                    ddlIrregularityCode.SelectedValue = dsResult.Tables[0].Rows[0]["IrregularityCode"].ToString();
                    ddlProductType.SelectedValue = dsResult.Tables[0].Rows[0]["ProductType"].ToString();

                    CHKBonded.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsBonded"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsBonded"].ToString().Trim());
                    CHKConsole.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsConsole"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsConsole"].ToString().Trim());
                    CHKExportShipment.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsExport"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsExport"].ToString().Trim());
                    CHKAsAggred.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsAsAggred"].ToString().Trim() == "" ? "false" : dsResult.Tables[0].Rows[0]["IsAsAggred"].ToString().Trim());

                    // AWBShipperConsigneeDetails

                    if (dsResult != null && dsResult.Tables.Count > 5 && dsResult.Tables[6].Rows.Count > 0)
                    {
                        TXTShipper.Text = "" + dsResult.Tables[6].Rows[0]["ShipperName"].ToString();
                        TXTShipTelephone.Text = "" + dsResult.Tables[6].Rows[0]["ShipperTelephone"].ToString();
                        TXTShipAddress.Text = "" + dsResult.Tables[6].Rows[0]["ShipperAddress"].ToString();
                        ddlShipCountry.Text = "" + dsResult.Tables[6].Rows[0]["ShipperCountry"].ToString();

                        TXTShipperAdd2.Text = "" + dsResult.Tables[6].Rows[0]["ShipperAdd2"].ToString();
                        TXTShipperCity.Text = "" + dsResult.Tables[6].Rows[0]["ShipperCity"].ToString();
                        TXTShipperState.Text = "" + dsResult.Tables[6].Rows[0]["ShipperState"].ToString();
                        TXTShipPinCode.Text = "" + dsResult.Tables[6].Rows[0]["ShipperPincode"].ToString();

                        TXTConsignee.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeName"].ToString();
                        TXTConTelephone.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeTelephone"].ToString();
                        TXTConAddress.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeAddress"].ToString();
                        ddlConCountry.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeCountry"].ToString();

                        TXTConsigAdd2.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeAddress2"].ToString();
                        TXTConsigCity.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeCity"].ToString();
                        TXTConsigState.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneeState"].ToString();
                        TXTConsigPinCode.Text = "" + dsResult.Tables[6].Rows[0]["ConsigneePincode"].ToString();

                        txtShipperCode.Text = dsResult.Tables[6].Rows[0]["ShipperAccCode"].ToString();
                        txtConsigneeCode.Text = dsResult.Tables[6].Rows[0]["ConsigAccCode"].ToString();
                        TXTShipperEmail.Text = dsResult.Tables[6].Rows[0]["ShipperEmailId"].ToString();
                        TXTConsigEmail.Text = dsResult.Tables[6].Rows[0]["ConsigEmailId"].ToString();
                    }

                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[1].Rows.Count > 0)
                    {
                        // AWBRateMaster
                        DataSet dsMaterialDetails = new DataSet();
                        // CommodityCode CodeDescription Pieces GrossWeight Dimensions VolumetricWeight ChargedWeight RowIndex
                        dsMaterialDetails.Tables.Clear();
                        dsMaterialDetails.Tables.Add(dsResult.Tables[1].Copy());

                        grdMaterialDetails.DataSource = dsMaterialDetails.Copy();
                        grdMaterialDetails.DataBind();
                        LoadCommodityDropdown();

                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            //string res = ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Text = dsMaterialDetails.Tables[0].Rows[i]["CodeDescription"].ToString();

                            if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowDropdownCommodity")))
                            {
                                ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")).Text = dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString();
                            }
                            else
                            {
                                ((TextBox)grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode1")).Text = dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString();
                            }
                            ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlPaymentMode")).Text = dsMaterialDetails.Tables[0].Rows[i]["PaymentMode"].ToString();
                            ((DropDownList)grdMaterialDetails.Rows[i].FindControl("ddlShipmentType")).Text = dsMaterialDetails.Tables[0].Rows[i]["ShipmentType"].ToString();
                        }

                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text = "" + dsResult.Tables[8].Rows[0]["TotalPcs"].ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalGrWt")).Text = "" + dsResult.Tables[8].Rows[0]["TotalGrossWt"].ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text = "" + dsResult.Tables[8].Rows[0]["TotalVolume"].ToString();
                        ((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text = "" + dsResult.Tables[8].Rows[0]["TotalChargedWt"].ToString();

                        Session["dsMaterialDetails"] = dsMaterialDetails.Copy();
                    }
                    else
                    {
                        LoadGridMaterialDetail();
                    }


                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[3].Rows.Count > 0)
                    {
                        //AWBRouteMaster
                        //FltOrigin FltDestination FltNumber FltDate Pcs Wt Status Accepted AcceptedPcs
                        DataSet dsRoutDetails = new DataSet();
                        dsRoutDetails.Tables.Clear();
                        dsRoutDetails.Tables.Add(dsResult.Tables[3].Copy());

                        grdRouting.DataSource = dsRoutDetails.Copy();
                        grdRouting.DataBind();
                        LoadAirlineCode("");
                        ddlPartnerType_SelectionChange(null, null);
                        LoadAWBStatusDropdown();

                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString());
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedIndex = 0;
                            ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked = dsRoutDetails.Tables[0].Rows[i]["Accepted"].ToString() == "Y";
                            try
                            {//Disable check box once Pieces are accepted
                                if (((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Checked)
                                {
                                    ((CheckBox)grdRouting.Rows[i].FindControl("chkAccepted")).Enabled = false;
                                }

                            }
                            catch (Exception ex) { }
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlStatus")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Status"].ToString();
                            try
                            {
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString());
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString();

                                //((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString();
                            }
                            catch (Exception ex) { }
                            try
                            {
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(dsRoutDetails.Tables[0].Rows[i]["PartnerType"].ToString());
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["PartnerType"].ToString();

                                //((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Carrier"].ToString();
                            }
                            catch (Exception ex) { }
                            try
                            {
                                if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text.ToString().Equals("Other", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Visible = false;
                                    ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text = dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString(); ;
                                    ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Visible = true;
                                }
                            }
                            catch (Exception ex) { }
                        }
                        CheckPartnerFlightSupport();
                        Session["dsRoutDetails"] = dsRoutDetails.Copy();
                    }
                    else
                    {
                        LoadGridRoutingDetail();
                    }


                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[3].Rows.Count > 0)
                    {
                        // AWBRateMaster
                        // CommCode Pcs Weight FrIATA FrMKT ValCharge PayMode OcDueCar OcDueAgent SpotRate DynRate ServTax Total
                        DataSet dtRates = new DataSet();
                        dtRates.Tables.Clear();
                        dtRates.Tables.Add(dsResult.Tables[7].Copy());

                        GRDRates.DataSource = dtRates.Copy();
                        GRDRates.DataBind();
                        GRDRates.Enabled = true;
                        GrdRateDetails.Enabled = true;
                        drpCurrency.SelectedValue = dtRates.Tables[0].Rows[0]["CurrencyIndex"].ToString();

                        for (int i = 0; i < GRDRates.Rows.Count; i++)
                        {
                            ((DropDownList)GRDRates.Rows[i].FindControl("ddlPayMode")).Text = dtRates.Tables[0].Rows[i]["PayMode"].ToString();
                            try
                            {
                                ((TextBox)GRDRates.Rows[i].FindControl("TXTCurrency")).Text = dtRates.Tables[0].Rows[i]["Currency"].ToString();
                            }
                            catch (Exception ex) { }
                        }

                        Session["dtRates"] = dtRates.Tables[0].Copy();
                    }
                    else
                    {
                        LoadGridRateDetail();
                        LoadCommodityDropdown();
                    }

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
                    dsDetails.Tables[0].Columns.Add("Currency");
                    dsDetails.Tables[0].Columns.Add("ChargeCode");


                    //AWBFrtRateDetails
                    foreach (DataRow row in dsResult.Tables[4].Rows)
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
                    foreach (DataRow row in dsResult.Tables[5].Rows)
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
                        newrow["ChargeCode"] = row["ChargeCode"].ToString().Length > 0 ? row["ChargeCode"] : "N";
                        dsDetails.Tables[0].Rows.Add(newrow);

                    }

                    Session["OCDetails"] = dsDetails.Copy();

                    //PrepareAWBDimensions(AWBNumber, txtAwbPrefix.Text.Trim());

                    txtExecutedAt.Text = "" + dsResult.Tables[0].Rows[0]["ExecutedAt"].ToString();
                    txtExecutedBy.Text = "" + dsResult.Tables[0].Rows[0]["ExecutedBy"].ToString();
                    //txtExecutionDate.Text = "" + dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();

                    string dtd = dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();
                    try
                    {
                        DateTime dt = DateTime.Parse(Session["IT"].ToString());
                        dtd = dt.ToString("dd/MM/yyyy");
                    }
                    catch (Exception ex)
                    {
                        dtd = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    txtExecutionDate.Text = "" + dtd;
                    try
                    {
                        //ULD Data
                        if (dsResult != null && dsResult.Tables.Count > 9 && dsResult.Tables[9].Rows.Count > 0)
                            Session["AWBULD"] = dsResult.Tables[9];
                        else
                            Session["AWBULD"] = null;
                    }
                    catch (Exception ex) { }

                    try
                    {
                        //Rate Details

                        if (dsResult != null && dsResult.Tables.Count > 10 && dsResult.Tables[10].Rows.Count > 0)
                        {
                            GrdRateDetails.DataSource = dsResult.Tables[10].Copy();
                            GrdRateDetails.DataBind();
                            Session["FltRoute"] = dsResult.Tables[10].Copy();
                        }
                        else
                            Session["FltRoute"] = null;
                    }
                    catch (Exception ex) { }

                    //try
                    //{//HAWB Details
                    //    BALHAWBDetails HAWB = new BALHAWBDetails();
                    //    DataSet ds = HAWB.GetHAWBDetails(AWBNumber, txtAwbPrefix.Text.Trim());
                    //    Session["HAWBDetails"] = ds.Tables[0];
                    //    HAWB = null;
                    //    ds = null;
                    //}
                    //catch (Exception ex) { }
                    //EnableDisable(editable);
                    EnableDisable("EditTemp");



                }

            }
            catch (Exception ex)
            {
                dsResult = null;
                lblStatus.Text = "" + ex.Message;
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        protected void btnPrnSelLbl_Click(object sender, EventArgs e)
        {
            try
            {
                radPrnAllLbl.Checked = true;
                radPrnSelLbl.Checked = false;
                txtPrnLblFrom.Text = txtPrnLblTo.Text = string.Empty;
                btnPrintSelLbl.Visible = true;
                btnPrintSelDGRLbl.Visible = false;
                lblErrorMsg.Text = string.Empty;
                if (Convert.ToBoolean(Session["DemoInstance"]))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                }
                else
                    btnPrintSelLbl_Click(null, null);
            }
            catch (Exception ex)
            { }
        }

        protected void btnPrintSelLbl_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrorMsg.Text = string.Empty;
                int FromLbl, ToLbl;
                int totPcs = int.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalPcs")).Text.ToString());
                if (radPrnAllLbl.Checked)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>PrintLabels();</script>", false);
                }
                else
                {
                    if (txtPrnLblFrom.Text.Trim() == "" || txtPrnLblTo.Text.Trim() == "")
                    {
                        lblErrorMsg.Text = "Values cannot be balnk...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    try
                    {
                        FromLbl = int.Parse(txtPrnLblFrom.Text);
                        ToLbl = int.Parse(txtPrnLblTo.Text);
                    }
                    catch (Exception ex)
                    {
                        lblErrorMsg.Text = "Enter digits only...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    if (FromLbl <= 0 || ToLbl <= 0)
                    {
                        lblErrorMsg.Text = "Enter digits greater than 0...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    else if (ToLbl < FromLbl)
                    {
                        lblErrorMsg.Text = "To Lbl cannot be less than From Lbl...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }

                    else if (FromLbl > totPcs || ToLbl > totPcs)
                    {
                        lblErrorMsg.Text = "Pcs cannot be greater than total pcs...";
                        lblErrorMsg.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>PrintSelLabels();</script>", false);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit_SelLbl();</script>", false);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private bool ValidateLoadableULD()
        {
            DataSet dsDim = (DataSet)Session["dsDimesionAll"];
            bool blnResult = true;
            StringBuilder StrDimensions = new StringBuilder();
            GHA_BALFlightCapacity objBAL = new GHA_BALFlightCapacity();

            try
            {
                string strFlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                DateTime dtFlightDt = DateTime.ParseExact(((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text.Trim(), "dd/MM/yyyy", null);
                string Station = Convert.ToString(Session["Station"]);

                if (dsDim != null && dsDim.Tables.Count > 0 && dsDim.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dsDim.Tables[0].Rows.Count; intCount++)
                    {
                        if (Convert.ToDecimal(dsDim.Tables[0].Rows[intCount]["Length"]) > 0 || Convert.ToDecimal(dsDim.Tables[0].Rows[intCount]["Breath"]) > 0
                            || Convert.ToDecimal(dsDim.Tables[0].Rows[intCount]["Height"]) > 0)
                        {
                            StrDimensions.Append("Insert into #Dimensions values(");
                            StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Length"]);
                            StrDimensions.Append(",");
                            StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Breath"]);
                            StrDimensions.Append(",");
                            StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Height"]);
                            StrDimensions.Append(",'");
                            StrDimensions.Append(dsDim.Tables[0].Rows[intCount]["Units"]);
                            StrDimensions.Append("'); ");
                        }
                    }
                }

                if (StrDimensions.Length > 0)
                {
                    blnResult = objBAL.ValidateLoadableWeight(Station, strFlightNo, dtFlightDt, StrDimensions.ToString());
                }

                return blnResult;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dsDim != null)
                    dsDim.Dispose();
                StrDimensions = null;
                objBAL = null;
            }
        }

        private bool ValidateFlightCapacity()
        {
            bool blnResult = true;
            GHA_BALFlightCapacity objBAL = new GHA_BALFlightCapacity();

            try
            {
                string strFlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                DateTime dtFlightDt = DateTime.ParseExact(((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text.Trim(), "dd/MM/yyyy", null);

                string FltOrgin = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                string FltDest = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim();
                Decimal AWBWeight = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommGrossWt")).Text.Trim());

                blnResult = objBAL.ValidateFlightCapacity(FltOrgin, FltDest, strFlightNo, dtFlightDt, AWBWeight);

                return blnResult;
            }
            catch
            {
                return false;
            }
            finally
            {
                objBAL = null;
            }
        }

        #region E-AWB Pop Up

        protected void imgbtnEAWBPopUp_Click(object sender, EventArgs e)
        {
            try
            {
                lblEAWBPopUpError.Text = string.Empty;
                ///radSpot.Enabled = false;
                if (CHKAsAggred.Checked)
                {
                    radAsAgree.Checked = true;
                    radMarket.Enabled = false;
                    radSpot.Enabled = false;
                    radIATADef.Enabled = false;
                }

                //radAsAgree.Checked = radMarket.Checked = radSpot.Checked = false;
                radIATADef.Checked = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_EAWBPopUp();</script>", false);
            }
            catch (Exception ex)
            { }
        }

        protected void btnPrintEAWB_Click(object sender, EventArgs e)
        {
            try
            {
                if (radAsAgree.Checked)
                {
                    CHKAsAggred.Checked = true;
                }
                btnShowEAWB_Click(null, null);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_EAWBPopUp();</script>", false);
            }
            catch (Exception ex)
            { }
        }

        #endregion

        protected DataTable fillDropinGrid()
        {

            DataTable dtK9 = new DataTable();
            try
            {
                dtK9 = (DataTable)Session["AirportCode"];
            }
            catch (Exception ex)
            {


            }

            return dtK9;
        }

        #region Shipment Date Changed
        protected void txtShipmentDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool bothDatesValid = false;
                DateTime fltDate;
                if (DateTime.TryParseExact(((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out fltDate))
                {   //Get flight date.
                    bothDatesValid = true;
                }
                DateTime shipDate;
                if (DateTime.TryParseExact(txtShipmentDate.Text, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out shipDate))
                {   //Get Shipment date.
                    bothDatesValid = true;
                }
                if (bothDatesValid)
                {   //If shipmnet date is later than flight date then set shipment date as flight date.
                    if (DateTime.Compare(shipDate, fltDate) > 0)
                    {
                        ((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text = shipDate.ToString("dd/MM/yyyy");
                        HidChangeDate.Value = "Y";
                        txtFdate_TextChanged(txtShipmentDate, new EventArgs());
                    }
                    else
                    {
                        LoadMatchingProductTypes();
                    }
                }

            }
            catch (Exception)
            {
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }
        #endregion Shipment Date Changed

        private void SaveOperationTime()
        {
            try
            {

                //Set dataset for AWBs in AWB grid.                    
                List<SCM.Common.Struct.clsOperationTimeStamp> objListOpsTime = new List<SCM.Common.Struct.clsOperationTimeStamp>();

                SCM.Common.Struct.clsOperationTimeStamp objOpsTimeStamp;
                new SCM.Common.Struct.clsOperationTimeStamp();

                //Set data of AWB for updating Arrival time stamp.
                foreach (GridViewRow gvr in grdRouting.Rows)
                {
                    //if (((RadioButton)gvr.FindControl("radSelectAWB")).Checked)
                    //{
                    objOpsTimeStamp = new SCM.Common.Struct.clsOperationTimeStamp();

                    objOpsTimeStamp.AWBPrefix = txtAwbPrefix.Text.Trim();
                    objOpsTimeStamp.AWBNumber = txtAWBNo.Text.Trim();

                    objOpsTimeStamp.Description = "";
                    DateTime dt = (DateTime)Session["IT"];
                    if (DateTime.TryParseExact(((TextBox)gvr.FindControl("txtFdate")).Text, "dd/MM/yyyy", null,
                        System.Globalization.DateTimeStyles.None, out dt))
                    {
                        objOpsTimeStamp.FlightDt = dt;
                    }
                    else
                    {
                        objOpsTimeStamp.FlightDt = (DateTime)Session["IT"];
                    }

                    objOpsTimeStamp.FlightNo = ((DropDownList)gvr.FindControl("ddlFltNum")).SelectedItem.Text;
                    objOpsTimeStamp.OperationalStatus = "RCS";
                    objOpsTimeStamp.OperationalType = "RCS";
                    objOpsTimeStamp.OperationDate = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy"); //DateTime.Now.ToString("dd/MM/yyyy");
                    objOpsTimeStamp.OperationTime = ((DateTime)Session["IT"]).ToString("HH:mm"); //DateTime.Now.ToString("HH:mm");
                    int pieceCount = 0;
                    if (int.TryParse(((TextBox)gvr.FindControl("txtPcs")).Text.Trim(), out pieceCount))
                    {
                        objOpsTimeStamp.Pieces = pieceCount;
                    }
                    else
                    {
                        objOpsTimeStamp.Pieces = 0;
                    }
                    decimal weight = 0;
                    if (decimal.TryParse(((TextBox)gvr.FindControl("txtWt")).Text.Trim(), out weight))
                    {
                        objOpsTimeStamp.Weight = weight;
                    }
                    else
                    {
                        objOpsTimeStamp.Weight = 0;
                    }
                    objOpsTimeStamp.ULDNumber = "";
                    objOpsTimeStamp.Station = Session["Station"].ToString();

                    //Add individual item to list.
                    objListOpsTime.Add(objOpsTimeStamp);

                    objOpsTimeStamp = null;
                    //Only single record will be selected at a time, so break from the loop.
                    //    break;

                    //}
                }
                //Check if list is built.
                if (objListOpsTime.Count > 0)
                {
                    BALCommon objBALCommon = new BALCommon();
                    //System.Collections.Generic.List<SCM.Common.Struct.clsOperationTimeStamp> LstOperation = null;
                    //LstOperation = (System.Collections.Generic.List<SCM.Common.Struct.clsOperationTimeStamp>)Session["listOperationTime"];

                    if (objListOpsTime != null && objListOpsTime.Count > 0)
                    {
                        if (objListOpsTime[0].UpdatedBy == null)
                        {
                            objListOpsTime[0].UpdatedBy = Convert.ToString(Session["UserName"]);
                            objListOpsTime[0].UpdatedOn = Convert.ToDateTime(Session["IT"]);
                        }

                        objBALCommon.SaveOperationalTimeStamp(objListOpsTime);
                    }
                    objBALCommon = null;
                    objListOpsTime = null;
                    //End                    
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void CheckPartnerFlightSupport()
        {
            try
            {
                LoginBL objBL = new LoginBL();
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("SupportPartnerFlight")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SupportPartnerFlight")))
                {
                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartnerType"))).Enabled = false;
                        ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartner"))).Enabled = false;
                        ((TextBox)(grdRouting.Rows[intCount].FindControl("txtPartnerStatus"))).Enabled = false;
                        ((CheckBox)(grdRouting.Rows[intCount].FindControl("chkFFR"))).Enabled = false;

                    }
                }
            }
            catch (Exception ex) { }
        }

        #region btnSendPHL_Click
        protected void btnSendPHL_Click(object sender, EventArgs e)
        {
            if (CHKConsole.Checked)
            {
                try
                {
                    string FlightNumber = "";
                    string PartnerCode = "";
                    string AWBNumber = "", error = "";
                    DataSet ds = new DataSet();
                    lblMsg.Text = "PHL";
                    try
                    {
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            FlightNumber = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                            PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        BALEmailID ObjEmail = new BALEmailID();
                        ds = ObjEmail.GetEmail(ddlOrg.SelectedItem.Text, ddlDest.SelectedItem.Text, "PHL", FlightNumber, PartnerCode);
                        if (ds != null)
                        {
                            txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                            lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                            if (lblMsgCommType.Text.Length > 0)
                            {
                                if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                                {
                                    GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                                }
                            }
                        }
                        ObjEmail = null;
                    }
                    catch (Exception ex) { }
                    if (HidAWBNumber.Value.ToString().Trim() != "")
                    {
                        AWBNumber = HidAWBNumber.Value.ToString().Trim();
                    }
                    else if (txtAWBNo.Text.Trim() == "")
                    {
                        AWBNumber = txtAWBNo.Text.Trim();
                    }
                    else
                    {
                        lblStatus.Text = "Please Provide AWB Number";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (AWBNumber.Length > 0)
                    {
                        string Msg = cls_BL.EncodePHLForSend(AWBNumber, ref error);
                        if (Msg.Length > 3)
                        {
                            txtMessageBody.Text = Msg;
                            Session["Message"] = "PHL";
                        }
                        else if (error.Length > 0)
                        {
                            lblStatus.Text = "PHL Message Error:" + error;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error on PHL Click" + ex.Message;
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            }
            else
            {
                lblStatus.Text = "Shipment is not Console shipment";
                lblStatus.ForeColor = Color.Red;
                return;
            }

        }
        #endregion

        #region btnSendPWB_Click
        protected void btnSendPWB_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                string FlightNumber = "";
                string PartnerCode = "";
                string AWBNumber = "", error = "";

                lblMsg.Text = "PWB";
                try
                {
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        FlightNumber = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                        PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                    }
                }
                catch (Exception ex) { }
                BALEmailID ObjEmail = new BALEmailID();
                ds = ObjEmail.GetEmail(ddlOrg.SelectedItem.Text, ddlDest.SelectedItem.Text, "PWB", FlightNumber, PartnerCode);
                try
                {
                    if (ds != null)
                    {
                        txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                        lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                        if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                        {
                            GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                        }
                    }
                    ObjEmail = null;
                }
                catch (Exception ex) { }

                if (HidAWBNumber.Value.ToString().Trim() != "")
                {
                    AWBNumber = HidAWBNumber.Value.ToString().Trim();
                }
                else if (txtAWBNo.Text.Trim() == "")
                {
                    AWBNumber = txtAWBNo.Text.Trim();
                }
                else
                {
                    lblStatus.Text = "Please Provide AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (AWBNumber.Length > 0)
                {
                    if (objBLL.GetAWBDetails(AWBNumber, txtAwbPrefix.Text, ref ds, ref error))
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string Msg = cls_BL.EncodePWB(ds, ref error);
                                    if (Msg.Length > 3)
                                    {
                                        txtMessageBody.Text = Msg;
                                        Session["Message"] = "PWB";
                                    }
                                    else if (error.Length > 0)
                                    {
                                        lblStatus.Text = "PWB Message Error: " + error;
                                        lblStatus.ForeColor = Color.Red;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
                lblStatus.Text = "Error on PWB Click" + ex.Message;
                lblStatus.ForeColor = Color.Red;
                return;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);


        }
        #endregion

        private void HideControlsOnDemoFlag()
        {
            LoginBL objBL = new LoginBL();
            try
            {
                string IsDemoInstance = string.Empty;

                if (Session["DemoInstance"] == null)
                {
                    //IsDemoInstance = objBL.GetMasterConfiguration("DemoInstance");
                    IsDemoInstance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DemoInstance");

                    if (IsDemoInstance != "")
                        Session["DemoInstance"] = Convert.ToBoolean(IsDemoInstance);
                    else
                        Session["DemoInstance"] = true;
                }

                if (!Convert.ToBoolean(Session["DemoInstance"]))
                {
                    btnDGRLbl.Visible = false;
                    CHKConsole.Visible = false;
                    CHKBonded.Visible = false;
                    CHKExportShipment.Visible = false;
                    CHKAsAggred.Visible = false;
                    chkTBScreened.Visible = false;
                    BtnViability.Visible = false;
                    CHKAllIn.Visible = false;

                    GRDRates.Columns[12].Visible = false; //Spot Freight
                    GRDRates.Columns[13].Visible = false; //Spot Rate
                    btnCargoReceipt.Visible = false;
                    btnHABDetails.Visible = false;
                    btnSaveTemplate.Visible = false;
                }
            }
            catch (Exception ex) { }
            finally
            {
                objBL = null;
            }
        }

        protected void TXTAgentCode_TextChanged(object sender, EventArgs e)
        {
            //lblStatus.Text = "";
            chkTBScreened.Checked = true;
            BookingBAL objBAL = new BookingBAL();
            DataSet DS = objBAL.GetAgentCodeDetails(TXTAgentCode.Text.Trim(), Convert.ToDateTime(Session["IT"]).Date);
            if (DS != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                txtAgentName.Text = DS.Tables[0].Rows[0]["AgentName"].ToString();
                TXTCustomerCode.Text = DS.Tables[0].Rows[0]["IATAAgentCode"].ToString();
                txtIACCode.Text = DS.Tables[0].Rows[0]["IACNo"].ToString();
                txtCCSF.Text = DS.Tables[0].Rows[0]["CCSFNo"].ToString();

                if (txtCCSF.Text.Trim() != "")
                    chkTBScreened.Checked = false;

                if (Convert.ToString(DS.Tables[0].Rows[0]["IACError"]) != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = Convert.ToString(DS.Tables[0].Rows[0]["IACError"]);
                }

                //Remove 'PX' from Paymode as no credit available for agent
                //CS-437-SK alloeing acceptance of AWBs as 'PX' when agent has no credit facility

                //try 
                //{
                //    ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Items.Remove("PX");
                //    if (Convert.ToDecimal(DS.Tables[0].Rows[0]["CreditAmount"].ToString()) > 1)
                //    {
                //        ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Items.Add("PX");
                //    }
                //}
                //catch (Exception ex) { }

                if (txtAWBNo.Text.Length<1) 
                {
                    SetPaymentMode();
                    DropDownList ddl = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"));
                    ddl.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["PayMode"]);
                }
                
                
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid Agent code";
                TXTAgentCode.Focus();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        #region Button HAWB Upload Event
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Label4.Text = string.Empty;
                int count = 0;
                foreach (GridViewRow row in gvHAWBDetails.Rows)
                {
                    if (((CheckBox)row.FindControl("check")).Checked)
                    {
                        count++;
                        if (count > 1)
                        {
                            Label4.Text = "Please select a single HAWB to Upload!";
                            Label4.ForeColor = Color.Blue;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                            return;
                        }

                    }
                }
                if (count == 0)
                {
                    Label4.Text = "No HAWB selected to Download!";
                    Label4.ForeColor = Color.Blue;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    return;
                }
                foreach (GridViewRow row in gvHAWBDetails.Rows)
                {
                    if (((CheckBox)row.FindControl("check")).Checked)
                    {
                        string HAWBPrefix = ((Label)row.FindControl("lblHAWBPrefix")).Text.Trim();
                        string HAWBNumber = ((Label)row.FindControl("lblHAWBNo")).Text.Trim();
                        string AWBPrefix = txtAwbPrefix.Text.Trim();
                        string AWBNumber = HidAWBNumber.Value;//txtAWBNo.Text.Trim();
                        if (CheckHAWBValidityUploading(AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber))
                        {
                            if (uploadHAWB.HasFile)
                            {
                                if (uploadHAWB.PostedFile.ContentLength < 1048576)
                                {
                                    string DocumentName = uploadHAWB.FileName;
                                    string DocumentExtension = uploadHAWB.FileName.Substring(uploadHAWB.FileName.Length - 4);
                                    byte[] Document = new byte[uploadHAWB.PostedFile.ContentLength];
                                    HttpPostedFile uploadedDocument = uploadHAWB.PostedFile;
                                    uploadedDocument.InputStream.Read(Document, 0, (int)(uploadHAWB.PostedFile.ContentLength));

                                    if (Document.Length > 0)
                                    {

                                        BALHAWBDetails objHAWB = new BALHAWBDetails();
                                        if (objHAWB.SaveHAWBDocuments(AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber, DocumentName, DocumentExtension, Document, Session["UserName"].ToString(), ((DateTime)Session["IT"])))
                                        {
                                            Label4.Text = "Document uploaded successfully!";
                                            Label4.ForeColor = Color.Green;

                                        }
                                        else
                                        {
                                            Label4.Text = "Document couldn't be uploaded! Please try again...";
                                            Label4.ForeColor = Color.Red;
                                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                                            return;
                                        }
                                    }

                                }
                                else
                                {
                                    Label4.Text = "File exceeds maximum size limit!";
                                    Label4.ForeColor = Color.Blue;
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                                    return;
                                }
                            }
                        }

                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Check Validity Of HAWB for Uploading
        public bool CheckHAWBValidityUploading(string AWBPrefix, string AWBNumber, string HAWBPrefix, string HAWBNumber)
        {
            try
            {
                BALHAWBDetails objHAWB = new BALHAWBDetails();
                return objHAWB.CheckHAWBValidity(AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber);


            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        #region Link Button HAWB Download
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Label4.Text = string.Empty;
                Session["HAWBDocument"] = null;
                int count = 0;
                foreach (GridViewRow row in gvHAWBDetails.Rows)
                {
                    if (((CheckBox)row.FindControl("check")).Checked)
                    {
                        count++;
                        if (count > 1)
                        {
                            Label4.Text = "Please select a single HAWB to Download!";
                            Label4.ForeColor = Color.Blue;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                            return;
                        }

                    }
                }
                if (count == 0)
                {
                    Label4.Text = "No HAWB selected to Download!";
                    Label4.ForeColor = Color.Blue;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    return;
                }
                foreach (GridViewRow row in gvHAWBDetails.Rows)
                {
                    if (((CheckBox)row.FindControl("check")).Checked)
                    {
                        string HAWBPrefix = ((Label)row.FindControl("lblHAWBPrefix")).Text.Trim();
                        string HAWBNumber = ((Label)row.FindControl("lblHAWBNo")).Text.Trim();
                        string AWBPrefix = txtAwbPrefix.Text.Trim();
                        string AWBNumber = HidAWBNumber.Value;//txtAWBNo.Text.Trim();
                        BALHAWBDetails objHAWB = new BALHAWBDetails();
                        DataSet ds = objHAWB.DownloadHAWBDocument(AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber);
                        if (ds != null)
                        {
                            Session["HAWBDocument"] = ds;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DownloadHAWB();</SCRIPT>", false);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        }
                        else
                        {
                            Label4.Text = "No Documents found!";
                            Label4.ForeColor = Color.Blue;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                            return;
                        }

                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region gvHAWBDetails RowCommand
        protected void gvHAWBDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Download")
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;
                    Label4.Text = string.Empty;
                    Session["HAWBDocument"] = null;
                    int count = 0;



                    string HAWBPrefix = ((Label)gvHAWBDetails.Rows[RowIndex].FindControl("lblHAWBPrefix")).Text.Trim();
                    string HAWBNumber = ((Label)gvHAWBDetails.Rows[RowIndex].FindControl("lblHAWBNo")).Text.Trim();
                    string AWBPrefix = txtAwbPrefix.Text.Trim();
                    string AWBNumber = HidAWBNumber.Value;//txtAWBNo.Text.Trim();
                    BALHAWBDetails objHAWB = new BALHAWBDetails();
                    DataSet ds = objHAWB.DownloadHAWBDocument(AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber);
                    if (ds != null)
                    {
                        Session["HAWBDocument"] = ds;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DownloadHAWB();</SCRIPT>", false);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    }
                    else
                    {
                        Label4.Text = "No Documents found!";
                        Label4.ForeColor = Color.Blue;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        return;
                    }



                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                }
            }
            catch (Exception Ex)
            { }
        }
        #endregion

        #region SaveULDNoinMaster
        private bool SaveULDNoinMaster(string UDLNumber, string DollyWt)
        {
            try
            {
                string strULDPrefix = UDLNumber.Trim().Substring(0, 3);
                string strULDSuffix = UDLNumber.Trim().Substring(UDLNumber.Trim().Length - 2, 2);
                string strULDSerial = UDLNumber.Trim().Replace(strULDPrefix, "").Replace(strULDSuffix, "");

                BALULDMaster blULD = new BALULDMaster();

                blULD.SelectRecords(UDLNumber, strULDSuffix, 0, "0", "0", 0, 0, 0, "0", "", "", "", false, "", strULDSerial, Convert.ToString(Session["Station"]), Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]),
                    "", 0, "0", "0", Convert.ToDateTime(Session["IT"]), "", "", "", false, "", "Y", DollyWt);

                blULD = null;

                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        #region PopupFreightChanged
        protected void PopupRateChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtRate = sender as TextBox;
                GridViewRow row = txtRate.NamingContainer as GridViewRow;
                int intCount = row.RowIndex;
                //Calculate existing Freight Tax percentage.
                float existingFreight = 0;
                float.TryParse(((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreight")).Text, out existingFreight);
                float taxPercent = 0;
                if (existingFreight > 0)
                {
                    float.TryParse(((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreightTax")).Text, out taxPercent);
                    //Calculate tax percent by dividing freight by tax percent value.
                    taxPercent = taxPercent / existingFreight;
                }
                float ChWt = 0;
                float.TryParse(((Label)GrdRateDetails.Rows[intCount].FindControl("lblPopupCWt")).Text, out ChWt);
                float rate = 0;
                float.TryParse(((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupRatePerKg")).Text, out rate);


                //Set freight as Rate * Charged Wt in Rate Details popup grid.
                ((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreight")).Text =
                    Convert.ToString(ChWt * rate);

                //Calculate Tax
                ((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreightTax")).Text =
                    Convert.ToString(ChWt * rate * taxPercent);

            }
            catch (Exception)
            {
            }
        }
        #endregion PopupFreightChanged

        #region btnPopupSave_Click
        protected void btnPopupSave_Click(object sender, EventArgs e)
        {
            try
            { //Reset values to original processed rates values.
                if (Session["FltRoute"] != null)
                {
                    DataTable dtRoute = (DataTable)Session["FltRoute"];
                    //Set values in dtRoute.                    
                    float FrtMkt = 0;
                    float Tax = 0;
                    float OCTax = 0;
                    float.TryParse(((TextBox)GRDRates.Rows[0].FindControl("TXTServiceTax")).Text, out OCTax);

                    for (int intCount = 0; intCount < dtRoute.Rows.Count; intCount++)
                    {   //Set values from grid.
                        if (GrdRateDetails.Rows.Count > intCount)
                        {
                            dtRoute.Rows[intCount].BeginEdit();
                            if (((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreight")).Text == "")
                            {
                                dtRoute.Rows[intCount]["Freight"] = "0";
                                dtRoute.Rows[intCount]["MKTFrt"] = "0";
                            }
                            else
                            {
                                dtRoute.Rows[intCount]["Freight"] =
                                    ((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreight")).Text;
                                dtRoute.Rows[intCount]["MKTFrt"] =
                                    ((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreight")).Text;
                            }
                            float tempFloat = 0;
                            if (float.TryParse(((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreight")).Text,
                                out tempFloat))
                            {
                                FrtMkt = FrtMkt + tempFloat;
                            }
                            if (((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreightTax")).Text == "")
                            {
                                dtRoute.Rows[intCount]["FreightTax"] = "0";
                                dtRoute.Rows[intCount]["MKTTax"] = "0";
                            }
                            else
                            {   //Find out OC Tax to calculate new Total Tax with modified freight tax.
                                tempFloat = 0;
                                if (float.TryParse(dtRoute.Rows[intCount]["FreightTax"].ToString(),
                                out tempFloat))
                                {
                                    if (OCTax > tempFloat)
                                    {
                                        OCTax = OCTax - tempFloat;
                                    }
                                }
                                dtRoute.Rows[intCount]["FreightTax"] =
                                    ((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreightTax")).Text;
                                dtRoute.Rows[intCount]["MKTTax"] =
                                    ((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreightTax")).Text;
                            }
                            tempFloat = 0;
                            if (float.TryParse(((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupFreightTax")).Text,
                                out tempFloat))
                            {
                                Tax = Tax + tempFloat;
                            }
                            if (((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupRatePerKg")).Text == "")
                            {
                                dtRoute.Rows[intCount]["RatePerKg"] = "0";
                            }
                            else
                            {
                                dtRoute.Rows[intCount]["RatePerKg"] =
                                    ((TextBox)GrdRateDetails.Rows[intCount].FindControl("txtPopupRatePerKg")).Text;
                            }
                            dtRoute.Rows[intCount].EndEdit();
                        }
                    }
                    dtRoute.AcceptChanges();
                    //Set values in AWB Rates.
                    ((TextBox)GRDRates.Rows[0].FindControl("TXTServiceTax")).Text = Convert.ToString(Tax + OCTax);
                    ((TextBox)GRDRates.Rows[0].FindControl("TXTFrMKT")).Text = FrtMkt.ToString();
                    ((TextBox)GRDRates.Rows[0].FindControl("txtRatePerKg")).Text =
                        Convert.ToString(FrtMkt / float.Parse(((TextBox)GRDRates.Rows[0].FindControl("TXTCWt")).Text));
                    //Set total frieght.
                    float TotalOCDC = 0;
                    float.TryParse(((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueCar")).Text, out TotalOCDC);
                    float TotalOCDA = 0;
                    float.TryParse(((TextBox)GRDRates.Rows[0].FindControl("TXTOcDueAgent")).Text, out TotalOCDA);
                    ((TextBox)GRDRates.Rows[0].FindControl("TXTTotal")).Text =
                        Convert.ToString(Tax + OCTax + FrtMkt + TotalOCDC - TotalOCDA);
                    //Set session value.
                    Session["FltRoute"] = dtRoute.Copy();
                }
            }
            catch (Exception)
            {
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }
        #endregion btnPopupSave_Click

        #region btnPopupCancel_Click
        protected void btnPopupCancel_Click(object sender, EventArgs e)
        {
            try
            { //Reset values to original processed rates values.
                if (Session["FltRoute"] != null)
                {
                    GrdRateDetails.DataSource = (DataTable)Session["FltRoute"];
                    GrdRateDetails.DataBind();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion btnPopupCancel_Click

        #region Load Commodity Dropdown
        public void LoadCommodityDropdown()
        {
            DataSet ds = objBLL.GetCommodityList("");
            Session["Description"] = ds;
            DropDownList ddl = new DropDownList();

            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].NewRow();

                row["CommodityCode"] = "Select";
                row["Commodity"] = "Select";
                ds.Tables[0].Rows.Add(row);

                ddl = ((DropDownList)(grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")));
                if (ds != null)
                {
                    ddl.DataSource = ds;
                    ddl.DataMember = ds.Tables[0].TableName;
                    ddl.DataTextField = "Commodity";
                    ddl.DataValueField = "CommodityCode";
                    ddl.DataBind();
                    ddl.Text = "Select";
                }
            }
        }

        public void LoadCommodityDropdown(int i)
        {
            DataSet ds = objBLL.GetCommodityList("");
            DropDownList ddl = new DropDownList();


            ddl = ((DropDownList)(grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")));
            if (ds != null)
            {
                ddl.DataSource = ds;
                ddl.DataMember = ds.Tables[0].TableName;
                ddl.DataTextField = "Commodity";
                ddl.DataValueField = "CommodityCode";
                ddl.DataBind();
            }

        }

        public void LoadCommodityDropdownWithData()
        {
            DataSet ds = objBLL.GetCommodityList("");
            DropDownList ddl = new DropDownList();

            DataSet dsMaterialDetails = (DataSet)Session["dsMaterialDetails"];

            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
            {
                ddl = ((DropDownList)(grdMaterialDetails.Rows[i].FindControl("ddlMaterialCommCode")));
                if (ds != null)
                {
                    ddl.DataSource = ds;
                    ddl.DataMember = ds.Tables[0].TableName;
                    ddl.DataTextField = "Commodity";
                    ddl.DataValueField = "CommodityCode";
                    ddl.DataBind();
                }

                if ((dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString()).Trim() != "")
                {
                    ddl.SelectedItem.Text = (dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString());
                    ddl.SelectedItem.Value = (dsMaterialDetails.Tables[0].Rows[i]["CommodityCode"].ToString());
                }
            }
        }



        #endregion Load Commodity Dropdown

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetShipperCodeHAWB(string prefixText, int count)
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

        private void GenerateWalkinInvoice()
        {
            //Check if Pay Mode 'PX' exists in the system.
            DropDownList ddl = (DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode");
            ListItem litem = ddl.Items.FindByValue("PX");
            if (litem != null)
            {   //If PX exists in the system and if PayMode is set to PP then generate walkin invoice.
                if (ddl.SelectedValue == "PP")
                {
                    GenerateWalkinAgentInvoiceNumber();
                }
            }
            else
            {   //PX does not exist in the system. So, validate Walkin agent and PP for generating walkin invoice.
                if (TXTAgentCode.Text.ToUpper().Contains("WALKIN") && ddl.SelectedValue == "PP")
                {
                    GenerateWalkinAgentInvoiceNumber();
                }
            }
            try
            {
                //Vijay - Code to check BillingEvent flag (EX- execution, AC- Acceptance, DP- Departure)
                string BillingEvent = "";
                BillingEvent = objBLL.getConfiguredBillingEvent(txtAwbPrefix.Text + HidAWBNumber.Value.Trim());

                //If BillingEvent is EX, record should be inserted in Billing tables
                if (BillingEvent.ToUpper() == "EX")
                {
                    AddBillingDetails(txtAwbPrefix.Text + "-" + HidAWBNumber.Value.Trim(), BillingEvent.ToUpper());
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void SetPaymentMode()
        {
            DropDownList ddl = ((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"));
            DropDownList ddlRateGrid = ((DropDownList)GRDRates.Rows[0].FindControl("ddlPayMode"));
            ListItem litem = new ListItem();

            if (Session["ConBooking_PayModeMaster"] != null)
            {
                DataTable dt = (DataTable)Session["ConBooking_PayModeMaster"];
                ddl.Items.Clear();
                ddl.DataSource = dt;
                ddl.DataTextField = "PayModeText";
                ddl.DataValueField = "PayModeCode";
                ddl.DataBind();
            } 
            if (Session["ConBooking_PayModeMaster"] != null)
            {
                DataTable dt = (DataTable)Session["ConBooking_PayModeMaster"];
                ddlRateGrid.Items.Clear();
                ddlRateGrid.DataSource = dt;
                ddlRateGrid.DataTextField = "PayModeText";
                ddlRateGrid.DataValueField = "PayModeCode";
                ddlRateGrid.DataBind();
            }

            string ShowAllPaymentModes = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowAllPaymentModes");

            if (ShowAllPaymentModes != "" && Convert.ToBoolean(ShowAllPaymentModes) == true)
                return;

            if (TXTAgentCode.Text.Length>0)
            {
                 SQLServer db = new SQLServer(Global.GetConnectionString());

                 DataSet dsPayMode = db.SelectRecords("spGetAgentPayMode", "AgentCode", TXTAgentCode.Text.ToString().Trim(), SqlDbType.VarChar);

                if (dsPayMode != null)
                {
                    if (dsPayMode.Tables.Count > 0)
                    {
                        if (dsPayMode.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = dsPayMode.Tables[0].Copy();
                            DataTable dt1 = dsPayMode.Tables[0].Copy();
                            Session["ConBooking_PayModeMaster"] = dsPayMode.Tables[0];
                            ddl.Items.Clear();
                            ddl.DataSource = dt;
                            ddl.DataTextField = "PayModeText";
                            ddl.DataValueField = "PayModeCode";
                            ddl.DataBind();
                            ddl.SelectedValue = "Select";
                            //ddlRateGrid
                            ddlRateGrid.Items.Clear();
                            ddlRateGrid.DataSource = dt1;
                            ddlRateGrid.DataTextField = "PayModeText";
                            ddlRateGrid.DataValueField = "PayModeCode";
                            ddlRateGrid.DataBind();
                            ddlRateGrid.SelectedValue = "Select";

                        }
                    }
                }
            }

            //if (TXTAgentCode.Text.Trim().ToUpper().IndexOf("WALKIN") > -1)
            //{
            //    litem = ddl.Items.FindByText("PX");
            //    ddl.Items.Remove(litem);

            //    litem = ddl.Items.FindByText("FOC");
            //    ddl.Items.Remove(litem);

            //    litem = ddl.Items.FindByText("Select");
            //    ddl.Items.Remove(litem);

            //    ddl.SelectedValue = "PP";
            //}
            //else if (TXTAgentCode.Text.Trim().ToUpper().IndexOf("FOC") > -1)
            //{
            //    litem = ddl.Items.FindByText("PX");
            //    ddl.Items.Remove(litem);

            //    litem = ddl.Items.FindByText("PP");
            //    ddl.Items.Remove(litem);

            //    litem = ddl.Items.FindByText("CC");
            //    ddl.Items.Remove(litem);

            //    litem = ddl.Items.FindByText("Select");
            //    ddl.Items.Remove(litem);

            //    ddl.SelectedValue = "FOC";
            //}
            //else
            //{
            //    litem = ddl.Items.FindByText("PP");
            //    ddl.Items.Remove(litem);

            //    litem = ddl.Items.FindByText("FOC");
            //    ddl.Items.Remove(litem);

            //    litem = ddl.Items.FindByText("Select");
            //    ddl.Items.Remove(litem);

            //    ddl.SelectedValue = "PX";
            //}
        }

    }
}