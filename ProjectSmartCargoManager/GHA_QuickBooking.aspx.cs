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
using Microsoft.Reporting.WebForms;
using ProjectSmartCargoManager.UserControls;

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
    public partial class GHA_QuickBooking : System.Web.UI.Page
    {

        #region Variables
        // DataSet dsAWBData;
        BLExpManifest objExpMani = new BLExpManifest();
        BookingBAL objBLL = new BookingBAL();
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
        //bool rs;
        string OtherFltSpe = string.Empty;
        string OCDCCurrency = "";
        string AgentCurrency = "";
        string RateClass = "";
        //bool IsIATAasMKT = false;
        //jayant
        string UserName;
        SQLServer da = new SQLServer(Global.GetConnectionString());
        //end
        string m_Designator = "DY";
        //DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable("GHA_QuickBooking_150");
        DataTable dtTable3 = new DataTable("GHA_QuickBooking_151");
        ReportDataSource rds1 = new ReportDataSource();
        BALUCR objUCR = new BALUCR();

        #endregion Variables

        #region Form Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                txtShipmentDate1.TextChange += new EventHandler(txtShipmentDate_TextChanged);
                // ------ Sumit 2014-10-30
                try
                {
                    if ((string)Session["FocusShipper"] == "true")
                        txtShipperCode.Focus();
                    else
                        txtAWBNo.Focus();                    
                }
                catch (Exception ex) { }

                // ------ Sumit 2014-10-30
                try
                {
                    dtCurrentDate = (DateTime)Session["IT"];
                    strUserName = Convert.ToString(Session["UserName"]);
                }
                catch (Exception ex) { }
                
                try
                {
                    if(Session["Piecetype_DIM"] !=null)
                    ddlShipmentType.SelectedValue = Session["Piecetype_DIM"].ToString();
                }
                catch (Exception ex)
                { }

                if (Request["__EVENTTARGET"] == "TXTAgentCode")
                {
                    TXTAgentCode_TextChanged(null, null);
                }

                
                if (!IsPostBack)
                {
                    ClearSession();
                    txtAWBNo.Focus();
                    //Session["DGRAWB"] = null;
                    //Session["dsDimesionAll"] = null;
                    HidBookingError.Value = "";
                    HidProcessFlag.Value = "";
                    if (HidCCSFFlag.Value == "")
                        HidCCSFFlag.Value = CommonUtility.ValidateCCSFandIAC;                        

                    Session["QB"] = "1";
                    txtSpotRate.Text = "";
                    txtAccpPieces.Enabled = false;
                    txtAccpWeight.Enabled = false;
                    imgAcceptance.Enabled = false;
                    btnSaveAcceptance.Enabled = false;
                    LoadSystemParameters();
                    //LoadOperationTimeConfig();
                    //txtShipmentDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                    LoadShipmentTime();
                    ddlShipmentTime.SelectedValue = dtCurrentDate.ToString("HH:00");

                    if (Request.QueryString["GHA"] == null && CommonUtility.SmartKargoInstance == "GH")
                    {
                        Session["GHABooking"] = true;
                        hdnBookingType.Value = "I"; //In Coming Booking.
                        //lblPageName.Text = "Partner Booking";
                    }
                    else if (Request.QueryString["GHA"] != null && Convert.ToString(Request.QueryString["GHA"]) != "" && Convert.ToBoolean(Request.QueryString["GHA"]) == true)
                    {
                        Session["GHABooking"] = true;
                        hdnBookingType.Value = "I"; //In Coming Booking.
                        //lblPageName.Text = "Partner Booking";
                    }
                    else
                    {
                        Session["GHABooking"] = false;
                        hdnBookingType.Value = "N"; //Normal Booking.
                    }

                    TXTAgentCode.Enabled = true;
                    Session["HAWBDetails"] = null;
                    Session["Message"] = null;
                    Session["AWBULD"] = null;
                    if (Session["awbPrefix"] == null)
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        objBal = null;
                    }

                    txtAwbPrefix.Text = Convert.ToString(Session["awbPrefix"]);

                    SetLabelValues(true);
                    Session["BookingID"] = "0";
                    //LoadOrigin();
                    LoadAWBMasterData();
                    txtAwbPrefix_TextChanged(null, null);
                    LoadCurrencyType();   

                    ddlOrg.SelectedValue = Session["Station"].ToString();
                    Session["Origin"] = Session["Station"].ToString();

                    // ------ Sumit 2014-10-30
                    try
                    {
                        if (Convert.ToString(Session["AgentCode"]) != "")
                        {
                            TXTAgentCode.Text = Convert.ToString(Session["AgentCode"]);
                            txtAgentName.Text = Convert.ToString(Session["AgentName"]);
                            //TXTCustomerCode.Text = Convert.ToString(Session["CustCode"]);
                            TXTAgentCode.Enabled = false;
                            txtAgentName.Enabled = false;
                        }
                    }
                    catch (Exception ex) { }
                    //LoadGridRateDetail();
                    LoadGridRoutingDetail();
                    LoadViability();

                    //txtExecutionDate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                    txtExecutedBy.Text = Session["UserName"].ToString();
                    txtExecutedAt.Text = Session["Station"].ToString();
                    
                    // For OCDA/OCDC Calculations ////////////////////////////////

                    DataSet dsDetails = new DataSet("GHA_QuickBooking_1");
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
                    btnExecute.Enabled = false;
                    btnGenerateTracer.Enabled = false;
                    btnPrintShipper.Enabled = false;
                    btnReopen.Enabled = false;
                    btnSenfwb.Enabled = false;

                    //Set paymode types in dropdown.
                    DropDownList ddl = ddlPaymentMode;
                    if (Session["ConBooking_PayModeMaster"] != null)
                    {
                        DataTable dt = new DataTable("GHA_QuickBooking_152");
                            dt = (DataTable)Session["ConBooking_PayModeMaster"];
                        ddl.Items.Clear();
                        ddl.DataSource = dt;
                        ddl.DataTextField = "PayModeText";
                        ddl.DataValueField = "PayModeCode";
                        ddl.DataBind();
                    }

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

                                btnExecute.Enabled = false;
                                btnReopen.Enabled = true;
                                btnDelete.Enabled = false;
                                btnSave.Enabled = false;
                                btnSaveAndAccept.Enabled = false;
                                state = "EditExecuted";
                                //txtShipmentDate.Enabled = false;
                                //imgShipmentDate.Enabled = false;
                                txtShipmentDate1.Enabled = false;

                                //btnProcess.Enabled = true;
                                txtAccpPieces.Enabled = true;
                                txtAccpWeight.Enabled = true;
                                imgAcceptance.Enabled = true;
                                btnSaveAcceptance.Enabled = true;

                                if (ddlPaymentMode.SelectedValue == "PP")
                                    btnCollect.Enabled = true;
                            }

                            if (status == "B")
                            {
                                btnExecute.Enabled = true;
                                btnReopen.Enabled = false;
                                btnDelete.Enabled = true;
                                btnSave.Enabled = true;
                                state = "Edit";
                                txtTotalAmount.BackColor = Color.Yellow;
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "AWB Booked.";
                                
                                //btnProcess.Enabled = true;
                                txtAccpPieces.Enabled = false;
                                txtAccpWeight.Enabled = false;
                                imgAcceptance.Enabled = false;
                                btnSaveAcceptance.Enabled = false;
                            }

                            if (status == "R")
                            {
                                btnExecute.Enabled = true;
                                btnReopen.Enabled = false;
                                btnDelete.Enabled = false;
                                btnSave.Enabled = true;
                                state = "EditReopen";

                                //btnProcess.Enabled = true;

                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "AWB is Re-Opened.";
                                //start modify jayant on 12/02/2014

                                // ------ Sumit 2014-10-30
                                try
                                {
                                    if (Session["RoleName"].ToString() == "Super User" && Session["DelExecAWB"].ToString() == "True")
                                    {
                                        btnDelete.Enabled = true;
                                    }
                                }
                                catch (Exception ex) { }
                                
                                //end modify jayant on 12/02/2014
                                txtAccpPieces.Enabled = true;
                                txtAccpWeight.Enabled = true;
                                imgAcceptance.Enabled = true;
                                btnSaveAcceptance.Enabled = false;
                            }

                            if (status == "V")
                            {
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "AWB is Voided.";

                                //If AWB is Void, Execute button should be enabled - Vijay - 23-09-2014
                                btnExecute.Enabled = true;

                                btnReopen.Enabled = false;
                                btnDelete.Enabled = false;
                                btnSave.Enabled = false;
                                btnSaveAndAccept.Enabled = false;
                                //btnProcess.Enabled = true;
                                //txtShipmentDate.Enabled = false;
                                //imgShipmentDate.Enabled = false;
                                txtShipmentDate1.Enabled = false;

                                TXTAgentCode.Enabled = false;
                                txtAccpPieces.Enabled = false;
                                txtAccpWeight.Enabled = false;
                                imgAcceptance.Enabled = false;
                                btnSaveAcceptance.Enabled = false;
                            }
                            btnSenfwb.Enabled = true;
                            Session["AWBStatus"] = status;
                            AutoPopulateData((Request.QueryString["command"].ToString().Trim() == "Edit" && status != "E"), AWBNumber, AWBPrefix, state);
                            SetLabelValues(false);
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
                        btnExecute.Enabled = true;
                        btnReopen.Enabled = false;
                        btnDelete.Enabled = true;
                        btnSave.Enabled = true;

                        //btnProcess.Enabled = true;
                        string TemplateID = Request.QueryString["TemplateID"].ToString().Trim(); ;
                        AutoPopulateTemplate(true, TemplateID, "Edit");
                    }

                    #endregion

                    try
                    {
                        Session["QB_PartnerCode"] = ddlAirlineCode.SelectedItem.Text.Trim();
                    }
                    catch (Exception ex)
                    
                    {
                        Session["QB_PartnerCode"] = Session["AirlinePrefix"].ToString();
                    }
                    //btnOrgSearch.Attributes.Add("onclick", "javascript:OpenSearchPopup('Origin','"+ txtOrg.Text +"','null');return false;");                 
                }
                LoadFlights();
                VerifyShipperConsignee();
                
                #region Execute Button disable if Save & Execute Combine
                try
                {

                    //MasterBAL objBAL = new MasterBAL();
                    //if (objBAL.CheckConfiguration("Booking", "CombineSave&Execute"))
                    if(CommonUtility.CombineSaveandExecute)
                    {
                        btnExecute.Visible = false;
                    }

                }
                catch (Exception ex) { }
                #endregion

                if (Request.QueryString["command"] != null && Request.QueryString["command"].ToString().Trim().ToUpper() == "VIEW")
                {
                    btnSave.Enabled = false;
                    btnSaveAndAccept.Enabled = false;
                    //txtShipmentDate.Enabled = false;
                    //imgShipmentDate.Enabled = false;
                    txtShipmentDate1.Enabled = false;

                    btnExecute.Enabled = false;
                    btnReopen.Enabled = false;
                    btnDgr.Enabled = false;
                    btnSendFFR.Enabled = false;
                    btnSendFHL.Enabled = false;
                    btnSenfwb.Enabled = false;
                    btnePouch.Enabled = false;
                    btnDelete.Enabled = false;
                }
                
                btnPrintAWB.Visible = false;
                string rolesforhiding = CommonUtility.HideRatesAcceptance;
                if (rolesforhiding != null) 
                {
                    if (rolesforhiding.Contains(Session["RoleName"].ToString()))
                    {   //Hide rates and acceptance section of quick booking page.
                        divRatesAcceptance.Visible = false;
                        divSaveAccept.Visible = false;
                        btnPrintAWB.Visible = true;
                    }
                }
                AddEventtoGrid();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error :" + ex.Message + "');</SCRIPT>");
            }
            finally
            {
                //restoreScroll(this, new EventArgs());
                Session["FocusShipper"] = "false";
                try
                {
                    if (Convert.ToString(Session["ButtonID"]) == "btnSave")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CallPopulateClick();</script>", false);
                        Session["ButtonID"] = "";
                    }
                }
                catch (Exception ex)
                {
                }
            }            
        }
        #endregion Form Load

        #region LoadCurrencyType
        private void LoadCurrencyType()
        {
            DataSet ds = new DataSet("GHA_QuickBooking_2");
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
                                drpCurrency.SelectedIndex = drpCurrency.Items.IndexOf(drpCurrency.Items.FindByText(ds.Tables[0].Rows[0]["BookingCurrrency"].ToString()));
                                Session["CurrencyType"] = ds.Tables[0].Rows[0]["BookingType"].ToString().Length > 0 ? ds.Tables[0].Rows[0]["BookingType"].ToString() : "IATA";
                                Session["QB_AGTCurrency"] = ds.Tables[0].Rows[0]["BookingCurrrency"].ToString();
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

        #region Load Matching Product Types
        private void LoadMatchingProductTypes()
        {
            DataSet ds = new DataSet("GHA_QuickBooking_3");
            try
            {
                string fltNum = "";
                if (((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedIndex >= 0)
                {
                    fltNum = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text;
                }
                string fltDt = "";
                DateTime fltDate;
                if (DateTime.TryParse(((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY, out fltDate))
                {
                    fltDt = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY;
                }
                string commCode = "";
                if (txtCommodityCode.Text.Trim() != "")
                {
                    commCode = txtCommodityCode.Text.Trim();
                }
                decimal weight = 0;
                BALProductType objBAL = new BALProductType();
                ds = objBAL.GetMatchingProductType(ddlOrg.SelectedValue, ddlDest.SelectedValue, fltNum, fltDt, "", commCode, weight, txtShipmentDate1.DateFormatDDMMYYYY);

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
        
        #region dimension

        public void CreateDimensionDataSet()
        {
            DataSet dsDimesionAll = new DataSet("GHA_QuickBooking_4");

            try
            {
                dsDimesionAll.Tables.Add(new DataTable("GHA_QuickBooking_153"));

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
            DataSet dsAWBPiecesAll = new DataSet("GHA_QuickBooking_5");
            try
            {

                dsAWBPiecesAll.Tables.Add(new DataTable("GHA_QuickBooking_154"));

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

        #region Load Grid Routing Detail
        public void LoadGridRoutingDetail()
        {
            DataTable myDataTable = new DataTable("GHA_QuickBooking_7");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet("GHA_QuickBooking_6");
            DataSet dsRoutDetails = new DataSet("GHA_QuickBooking_8");
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
                try
                {
                    dr["Airline"] = ddlAirlineCode.Text.ToString();
                }
                catch (Exception ee) 
                {
                    dr["Airline"] = "";
                }
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

        #region Set Route Grid Values
        /// <summary>
        /// Displays values in Flight grid from given Data Table.
        /// </summary>        
        /// <param name="dt">Data Table which contains values to be shown in grid.</param>
        public void SetRouteGridValues(DataTable dt)
        {
            // ------ Sumit 2014-10-30
            try
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
                    ((DateControl)(grdRouting.Rows[rowIndex].FindControl("txtFdate"))).DateFormatDDMMYYYY = dr["FltDate"].ToString();
                }
            }
            catch (Exception ex) { }
        }
        #endregion Set Material Grid Values

        #region Load Flight Dropdown
        public void LoadFlightDropdown(string Origin, string Destination, int rowIndex)
        {
            // ------ Sumit 2014-10-30
            try
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
            catch (Exception ex) { }
        }
        #endregion Load Flight Dropdown

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

            // ------ Sumit 2014-10-30
            try
            {
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
                    //txtExecutionDate.Enabled = false;
                    txtExecutionDate1.Enabled = false;
                    txtExecutedAt.Enabled = false;
                    txtExecutedBy.Enabled = false;

                }
            }
            catch (Exception ex) { }
            //end


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
                
                AWBParams.SetValue(TXTAgentCode.Text, i);

                DataSet ds = new DataSet("GHA_QuickBooking_9");
                    ds = objBLL.GetAWBInfo(AWBParams);
                if (ds != null)
                {
                    DataTable dt = new DataTable("GHA_QuickBooking_155");
                    if (ds.Tables.Count > 2)
                    {
                        //Show AWB summary
                        dt = ds.Tables[0];
                        Session["BookingID"] = dt.Rows[0]["SerialNumber"].ToString();
                        ddlOrg.SelectedValue = dt.Rows[0]["OriginCode"].ToString();
                        ddlDest.SelectedValue = dt.Rows[0]["DestinationCode"].ToString();
                        //User Agent Name as value member for ddlAgtCode
                        //ddlAgtCode.SelectedValue = dt.Rows[0]["AgentName"].ToString();
                        TXTAgentCode.Text = dt.Rows[0]["AgentName"].ToString();

                        txtAgentName.Text = dt.Rows[0]["AgentName"].ToString();
                        ddlServiceclass.SelectedValue = dt.Rows[0]["ServiceCargoClassId"].ToString();
                        txtHandling.Text = dt.Rows[0]["HandlingInfo"].ToString();
                        txtExecutionDate1.DateFormatDDMMYYYY = dt.Rows[0]["ExecutionDate"].ToString();
                        txtExecutedBy.Text = dt.Rows[0]["ExecutedBy"].ToString();
                        txtExecutedAt.Text = dt.Rows[0]["ExecutedAt"].ToString();

                        //Show AWB Route Details
                        grdRouting.DataSource = ds.Tables[2];
                        grdRouting.DataBind();
                        SetRouteGridValues(ds.Tables[2]);
                        
                        txtAWBNo.ReadOnly = true;
                        SetLabelValues(false);
                                                
                        TXTAgentCode.Enabled = false;
                        ddlDest.Enabled = false;
                        txtAgentName.Enabled = false;
                        txtExecutedAt.Enabled = false;
                        txtExecutedBy.Enabled = false;
                        txtExecutionDate1.Enabled = false;
                        txtHandling.Enabled = false;
                                                
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
            // ------ Sumit 2014-10-30
            try
            {
                if (!SetEmpty)
                {
                    if (txtAWBNo.Text.Length <= 4)
                        return;
                    HidAWBP1.Value = txtAWBNo.Text.Substring(0, txtAWBNo.Text.Length - 4);
                    HidAWBP2.Value = txtAWBNo.Text.Substring(txtAWBNo.Text.Length - 4);
                    HidDest.Value = ddlDest.SelectedValue;
                    HidSource.Value = ddlOrg.SelectedValue;
                    intTotalPcs = 0;

                    HidPcsCount.Value = intTotalPcs.ToString();
                    HidVia.Value = "";
                    HidWt.Value = floatGrossWt.ToString();

                    HidAWBPrefix.Value = txtAwbPrefix.Text.ToString();
                }
                else
                {
                    HidAWBP1.Value = "";
                    HidAWBP2.Value = "";
                    HidDest.Value = "";
                    HidSource.Value = "";
                    HidPcsCount.Value = "0";
                    HidVia.Value = "";
                    HidWt.Value = "0";
                }
            }
            catch (Exception ex) { }
        }
        #endregion Set Label Values

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
                        HidBookingError.Value = "Please enter valid AWB Prefix";
                        lblStatus.Text = "Please enter valid AWB Prefix";
                        txtAwbPrefix.Focus();
                        return false;
                    }

                    DataSet dsPartner = new DataSet("GHA_QuickBooking_10");
                        dsPartner = (DataSet)Session["PartnerCode"];
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
                        HidBookingError.Value = "Please enter valid AWB Prefix (Should match with Self / Partner code)";
                        lblStatus.Text = "Please enter valid AWB Prefix (Should match with Self / Partner code)";
                        txtAwbPrefix.Focus();
                        return false;
                    }
                }

                if (ddlAirlineCode.Text.Trim() == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    HidBookingError.Value = "Please enter valid Designator Code.";
                    lblStatus.Text = "Please enter valid Designator Code.";
                    txtAwbPrefix.Focus();
                    return false;
                }

                /* As per Amit Product type is not mandatory
                if (ddlProductType.SelectedItem.Text.Trim().ToUpper() == "SELECT")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Product Type.";
                    ddlProductType.Focus();
                    return false;
                }
                */

                //string AWBNumber = "", errormessage = "";

                if (Convert.ToBoolean(Session["GHABooking"]))
                {
                    if (txtAWBNo.Text.Length < 1 || txtAWBNo.Text.Trim() == "")
                    {
                        HidBookingError.Value = "Please enter valid AWB Number";
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
                                HidBookingError.Value = "AWB Number already exists in the system. Please validate.";
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
            catch (Exception)
            {
                HidBookingError.Value = "Please enter valid AWB Number";
                lblStatus.Text = "Please enter valid AWB Number";
                txtAWBNo.Focus();
                return false;
            }
            if (ddlDest.SelectedItem.Text == "Select")
            {
                HidBookingError.Value = "Please select valid Destination Code";
                lblStatus.Text = "Please select valid Destination Code";
                ddlDest.Focus();
                return false;
            }
            if (ddlOrg.SelectedValue == ddlDest.SelectedValue)
            {
                HidBookingError.Value = "Origin and Destination Code can not be same.";
                lblStatus.Text = "Origin and Destination Code can not be same.";
                ddlDest.Focus();
                return false;
            }


            if (hdnBookingType.Value == "N" && TXTAgentCode.Text == "")
            {
                HidBookingError.Value = "Please select valid Agent Code";
                lblStatus.Text = "Please select valid Agent Code";
                TXTAgentCode.Focus();
                return false;
            }

            if (txtShipmentDate1.Text != "")
            {
                DateTime shipmentDate;
                if (!DateTime.TryParseExact(txtShipmentDate1.DateFormatDDMMYYYY + " " + ddlShipmentTime.SelectedValue,
                    "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out shipmentDate))
                {
                    HidBookingError.Value = "Please select valid Shipment Date & Time";
                    lblStatus.Text = "Please select valid Shipment Date & Time";
                    txtShipmentDate1.Focus();
                    return false;
                }
            }

            if (!ValidateMasterEntry(TXTAgentCode.Text.ToString(), "Agent"))
            {
                HidBookingError.Value = "Please enter valid Agent Code.";
                lblStatus.Text = "Please enter valid Agent Code.";
                TXTAgentCode.Focus();
                return false;
            }

            try
            {
                if (CHKConsole.Checked)
                {
                    DataTable dtHAWB = new DataTable("GHA_QuickBooking_156");
                        dtHAWB = (DataTable)Session["HAWBDetails"];
                    if (dtHAWB == null)
                    {
                        HidBookingError.Value = "Please enter HAWB details.";
                        lblStatus.Text = "Please enter HAWB details.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                    else if (dtHAWB != null)
                    {
                        if (dtHAWB.Rows.Count < 1)
                        {
                            HidBookingError.Value = "Please enter HAWB details.";
                            lblStatus.Text = "Please enter HAWB details.";
                            lblStatus.ForeColor = Color.Red;
                            return false;
                        }
                        dtHAWB.Dispose();
                    }

                }
            }
            catch (Exception ex) { }

            #region Validate routing information.
            if (grdRouting.Rows.Count < 1)
            {
                HidBookingError.Value = "Please enter Flight Details";
                lblStatus.Text = "Please enter Flight Details";
                return false;
            }

            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {

                TextBox tempTextBox;
                //Validate flight origin
                tempTextBox = (TextBox)(grdRouting.Rows[i].FindControl("txtFltOrig"));
                if (tempTextBox.Text == "")
                {
                    HidBookingError.Value = "Please enter valid Flight Origin";
                    lblStatus.Text = "Please enter valid Flight Origin";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //Validate flight destination
                tempTextBox = (TextBox)(grdRouting.Rows[i].FindControl("txtFltDest"));
                if (tempTextBox.Text == "")
                {
                    HidBookingError.Value = "Please enter valid Flight Destination";
                    lblStatus.Text = "Please enter valid Flight Destination";
                    tempTextBox.Focus();
                    return false;
                }
                tempTextBox = null;
                //------------------------------

                tempTextBox = null;
                //Validate flight date
                //tempTextBox = (DateControl)(grdRouting.Rows[i].FindControl("txtFdate"));
                if (((DateControl)(grdRouting.Rows[i].FindControl("txtFdate"))).Text == "")
                {
                    HidBookingError.Value = "Please select valid Flight Date";
                    lblStatus.Text = "Please select valid Flight Date";
                    tempTextBox.Focus();
                    return false;
                }
                //tempTextBox.Dispose();
                //if (ddlServiceclass.SelectedItem.Text.ToUpper() != "VOID" && Convert.ToBoolean(Session["FRV"]))
                if (ddlServiceclass.SelectedItem.Text.ToUpper() != "VOID" && CommonUtility.FlightValidation)
                {
                    string POL = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.ToString();
                    string POU = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.ToString();
                    string Flight = "";
                    if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase))
                    {
                        Flight = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text.ToString();
                    }
                    else
                    {
                        Flight = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text.ToString();
                    }

                    string Date = ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).DateFormatDDMMYYYY;
                    
                    if (objBLL.CheckFlightDeparted(POL, POU, Flight, Date))
                    {
                        lblStatus.Text = "Flight No. " + Flight + " Dated " + Date + " already departed. Please select another flight";
                        tempTextBox.Focus();
                        return false;
                    }                    
                }
            }
            #endregion

            //try
            //{
            //    DateTime.ParseExact(txtExecutionDate.Text, "dd/MM/yyyy", null);
            //}
            //catch (Exception)
            //{
            //    HidBookingError.Value = "Please select valid Execution Date";
            //    lblStatus.Text = "Please select valid Execution Date";
            //    txtExecutionDate.Focus();
            //    return false;
            //}
            if (txtExecutedBy.Text == "")
            {
                HidBookingError.Value = "Please enter Executed By";
                lblStatus.Text = "Please enter Executed By";
                txtExecutedBy.Focus();
                return false;
            }
            if (txtExecutedAt.Text == "")
            {
                HidBookingError.Value = "Please enter Execution Location";
                lblStatus.Text = "Please enter Execution Location";
                txtExecutedBy.Focus();
                return false;
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
                                HidBookingError.Value = "Please enter flight no in route details.";
                                lblStatus.Text = "Please enter flight no in route details.";
                                lblStatus.ForeColor = Color.Red;
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        HidBookingError.Value = "Exception in flight no in route details.";
                        lblStatus.Text = "Exception in flight no in route details.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }

                }

                if (((TextBox)grdRouting.Rows[ICount].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[ICount].FindControl("txtFltDest")).Text.Trim())
                {
                    HidBookingError.Value = "Flight origin and destination can't be same.";
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
            DataSet dsResult = new DataSet("GHA_QuickBooking_11");

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
                    float Length = int.Parse(dsResult.Tables[0].Rows[0]["Length"].ToString());
                    float Breadth = int.Parse(dsResult.Tables[0].Rows[0]["Breadth"].ToString());
                    float Height = int.Parse(dsResult.Tables[0].Rows[0]["Height"].ToString());

                    DataSet dsDimension = new DataSet("GHA_QuickBooking_12");
                        dsDimension = (DataSet)Session["dsDimesionAll"];

                    float unitconversion = 1;
                    if (dsDimension != null)
                    {
                        foreach (DataRow drDimension in dsDimension.Tables[0].Rows)
                        {
                            if (drDimension["MeasureUnit"].ToString() == "inches")
                            {
                                unitconversion = float.Parse("2.5");
                            }

                            if (((float.Parse(drDimension["Length"].ToString()) * unitconversion) > Length))
                            {
                                lblStatus.Text = "Error :Length of piece is greater than allowed length";
                                return false;
                            }
                            if (((float.Parse(drDimension["Breadth"].ToString()) * unitconversion) > Breadth))
                            {
                                lblStatus.Text = "Error :Breadth of piece is greater than allowed breadth";
                                return false;
                            }
                            if (((float.Parse(drDimension["Height"].ToString()) * unitconversion) > Height))
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
            DataSet dsResult = new DataSet("GHA_QuickBooking_13");

            try
            {
                #region Stock Alert
                DataSet ds = new DataSet("GHA_QuickBooking_14");
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

        public bool GetAutoAWBNumber(ref string errormessage, ref string AWBNumber, string AWBPrefix, string CnoteType, string IsDomestic)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            DataSet dsResult = new DataSet("GHA_QuickBooking_15");

            try
            {
                #region Stock Alert
                DataSet ds = new DataSet("GHA_QuickBooking_16");
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

                string[] param = { "AgentCode", "Station", "AWBPrefix", "CNote", "IsDomestic" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                //object[] values = { ddlAgtCode.SelectedItem.Text.Trim(), Session["Station"].ToString() };
                object[] values = { TXTAgentCode.Text.Trim(), ddlOrg.Text.Trim(), AWBPrefix, CnoteType, IsDomestic };

                dsResult = db.SelectRecords("SP_Booking_GetNextAWBForAgent_CEBU_Latest", param, values, sqldbtype);

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
                                errormessage = "Stock not available.";
                                return false;
                            }

                            return true;
                        }
                        else
                        {
                            errormessage = "Stock not available.";
                            return false;
                        }
                    }
                    else
                    {
                        errormessage = "Stock not available.";
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
            // ------ Sumit 2014-10-30
            try
            {
                txtAgentName.Enabled = true;
                txtAWBNo.ReadOnly = false;
                ddlDest.Enabled = true;
                TXTAgentCode.Enabled = true;
                txtExecutionDate1.Enabled = true;
                txtExecutedAt.Enabled = true;
                txtExecutedBy.Enabled = true;
                btnSave.Visible = true;
                grdRouting.Enabled = true;
                Session["BookingID"] = "0";
                lblStatus.Text = "";
                lblStatus.ForeColor = Color.Red;
                txtAWBNo.ReadOnly = false;
                if (Session["Station"] == null || Session["Station"].ToString() == "")
                {
                    ddlOrg.SelectedIndex = 0;
                    ddlOrg.Enabled = true;
                }
                ddlDest.Text = "Select";

                LoadGridRoutingDetail();
                txtHandling.Text = "";
                txtAWBNo.Text = "";
                HidAWBNumber.Value = "";
                TXTShipper.Text = "";
                TXTShipTelephone.Text = "";
                TXTShipAddress.Text = "";
                TXTConAddress.Text = "";
                TXTConsignee.Text = "";
                TXTConTelephone.Text = "";
                txtExecutionDate1.DateFormatDDMMYYYY = dtCurrentDate.ToString("dd/MM/yyyy");
                txtExecutedBy.Text = Session["UserName"].ToString();
                txtExecutedAt.Text = Session["Station"].ToString();
                SetLabelValues(true);
                Session["dsDimesionAll"] = null;
            }
            catch (Exception ex) { }
        }
        #endregion btnClear_Click

        #region btnSendFFR_Click
        //Modified on 4/9/2013
        protected void btnSendFFR_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("GHA_QuickBooking_17");
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
            DataSet dsResult = new DataSet("GHA_QuickBooking_18");
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


                DataTable dtRates = new DataTable("GHA_QuickBooking_19");
                   dtRates = ((DataTable)Session["dtRates"]);
                   DataSet dsDetails = new DataSet("GHA_QuickBooking_20");
                    dsDetails = (DataSet)Session["OCDetails"];

                // Rates
                decimal frtiata, frtmkt, ocdc, ocda, tax, alltotal;
                frtiata = frtmkt = ocdc = ocda = tax = alltotal = 0;

                //if (dtRates == null)
                //{
                DataSet dsRates = new DataSet("GHA_QuickBooking_21");
                    string errormessage = string.Empty;

                    objBLL.GetAWBDetails(txtAWBNo.Text.Trim(), txtAwbPrefix.Text.Trim(), ref dsRates, ref errormessage);

                    if (dsRates != null && dsRates.Tables.Count > 0 && dsRates.Tables[0].Rows.Count > 0)
                        hdAWBShipmentType.Value = dsRates.Tables[0].Rows[0]["AWBShipmentType"].ToString();

                    if (dsRates != null && dsRates.Tables.Count > 6 && dsRates.Tables[7].Rows.Count > 0)
                    {
                        dtRates = dsRates.Tables[7].Copy();
                        Session["dtRates"] = dsRates.Tables[7].Copy();
                        dsRates = null;
                    }
                //}

                if (dtRates != null)
                {
                    foreach (DataRow rw in dtRates.Rows)
                    {
                        frtiata += decimal.Parse(rw["FrIATA"].ToString());
                        frtmkt += decimal.Parse(rw["FrMKT"].ToString());
                        ocda += decimal.Parse(rw["OcDueAgent"].ToString());
                        ocdc += decimal.Parse(rw["OcDueCar"].ToString());
                        tax += decimal.Parse(rw["ServTax"].ToString());
                        alltotal += decimal.Parse(rw["Total"].ToString());
                    }
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
            DataSet dsResult = new DataSet("GHA_QuickBooking_22");
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

        //Added by Vishal - 04 MAY 2014 ********************
        #region LoadCurrencyUOM
        private void LoadCurrencyWithUOM()
        {
            DataSet ds = new DataSet("GHA_QuickBooking_23");
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
                                drpCurrency.SelectedIndex =drpCurrency.Items.IndexOf(drpCurrency.Items.FindByText(ds.Tables[0].Rows[0]["BookingCurrrency"].ToString()));
                                txtUOM.Text = ds.Tables[0].Rows[0]["UOM"] != null ? ds.Tables[0].Rows[0]["UOM"].ToString() : "L";
                                Session["CurrencyType"] = ds.Tables[0].Rows[0]["BookingType"].ToString().Length > 0 ? ds.Tables[0].Rows[0]["BookingType"].ToString() : "IATA";
                                Session["ConBooking_RouteType"] = ds.Tables[0].Rows[0]["RouteType"] != null ? ds.Tables[0].Rows[0]["RouteType"].ToString() : "D";
                                Session["QB_AGTCurrency"] = ds.Tables[0].Rows[0]["BookingCurrrency"].ToString();
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

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage)
        {
            // ------ Sumit 2014-10-30
            try
            {
                DataSet dsResult = new DataSet("GHA_QuickBooking_24");
                if (strdate.Trim() == "")
                {
                    if (new ShowFlightsBAL().GetFlightListforDay_QB(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, ""))
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
            catch (Exception ex) { return null; }
        }

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage, string PartnerCode)
        {
            // ------ Sumit 2014-10-30
            try
            {
                DataSet dsResult = new DataSet("GHA_QuickBooking_25");
                bool blnSelfAirline = false;
                DataSet dsAWBPrefixs = new DataSet("GHA_QuickBooking_26");
                    dsAWBPrefixs = CommonUtility.AWBPrefixMaster;

                if (PartnerCode != "")
                {
                    if (dsAWBPrefixs != null && dsAWBPrefixs.Tables.Count > 0 && dsAWBPrefixs.Tables[0].Rows.Count > 0)
                    {
                        for (int intCount = 0; intCount < dsAWBPrefixs.Tables[0].Rows.Count; intCount++)
                        {
                            if (PartnerCode.ToUpper() == Convert.ToString(dsAWBPrefixs.Tables[0].Rows[intCount]["AirlinePrefix"]).ToUpper())
                            {
                                blnSelfAirline = true;
                                dsAWBPrefixs = null;
                                break;
                            }
                        }
                    }
                }

                if (strdate.Trim() == "")
                {
                    if (blnSelfAirline)
                    {
                        if (new ShowFlightsBAL().GetFlightListforDay_QB(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
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

                    if (blnSelfAirline)
                    {
                        if (new ShowFlightsBAL().GetFlightListforDay_QB(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
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
            catch (Exception ex) { return null; }
        }

        public void FormatRecords(string org, string dest, ref DataSet dsResult, int PrevHr, int PrevMin, int AllowedHr)
        {
            // ------ Sumit 2014-10-30
            try
            {
                int i = 0;
                string ScheduleID = "";
                DataSet dsNewResult = new DataSet("GHA_QuickBooking_27");
                    dsNewResult = dsResult.Clone();
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

                dsResult = new DataSet("GHA_QuickBooking_28");
                dsResult.Tables.Add(dv.ToTable().Copy());

                DataTable dt = new DataTable("GHA_QuickBooking_157");
                    dt = dsResult.Tables[0].Clone();
                foreach (DataRow row in dsResult.Tables[0].Rows)
                {
                    string depttime = row["DeptTime"].ToString();
                    int hr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                    int min = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));

                    string[] strDate = row["FltDate"].ToString().Split('/');
                    int intFltDate = int.Parse(strDate[0]);
                    int intCurrentDt = dtCurrentDate.Day;

                    bool canAdd = false;
                    if (hr < (PrevHr + AllowedHr) && intFltDate == intCurrentDt)
                    {
                        if (hr > PrevHr)
                        {
                            int hrdiff = hr - PrevHr;

                            if (((hrdiff * 60) - PrevMin + min) > (AllowedHr * 60))
                                canAdd = true;
                        }
                    }
                    else
                        canAdd = true;


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

                dsResult = new DataSet("GHA_QuickBooking_29");
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
            catch (Exception ex) { }
        }

        protected void btnShowEAWB_Click(object sender, EventArgs e)
        {
            try
            {

                bool IsAgreed = false;
                string strAgentPreference = string.Empty;

                strAgentPreference = objBLL.GeteAWBPrintPrefence(TXTAgentCode.Text.Trim(), txtAWBNo.Text, txtAwbPrefix.Text);

                if (strAgentPreference.Length < 1 || strAgentPreference == "")
                    strAgentPreference = "IATA";
                

                if (strAgentPreference == "As Agreed")
                    CHKAsAggred.Checked = true;
                //else
                //    CHKAsAggred.Checked = false;

                IsAgreed = CHKAsAggred.Checked;

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
                string Handlinginfo = txtHandling.Text.ToString();
                string AccountInfo = "";

                string SHCDesc = string.Empty;

                //bool SCHDesc = Convert.ToBoolean(lBal.GetMasterConfiguration("eAWBSHCDesc"));
                bool SCHDesc = false;
                try
                {
                    SCHDesc = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "eAWBSHCDesc"));
                }
                catch { }

                if (SCHDesc)
                {
                    if (txtSpecialHandlingCode.Text.Trim() != "")
                    {
                        SHCDesc = new BookingBAL().GetSHCCodesandDesc(txtSpecialHandlingCode.Text.Trim());
                        SHCDesc = SHCDesc.Replace("&amp;", "&");
                    }
                }
                else { SHCDesc = "SHC:" + txtSpecialHandlingCode.Text.Trim(); }

                if (Handlinginfo != "")
                    Handlinginfo = Handlinginfo + " | " + SHCDesc;
                else
                    Handlinginfo = SHCDesc;


                string CommCode = txtCommodityCode.Text.Trim();
                string CommDesc = txtCommodityName.Text.Trim();
                int Pcs = 0;

                try
                {
                    Pcs = int.Parse(txtPieces.Text.Trim());
                    //    }
                }
                catch (Exception ex)
                {
                }
                double GrossWgt = Convert.ToDouble(txtGrossWt.Text.Trim());

                double Volume = 0;// Convert.ToDouble(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalVolume")).Text.Trim());// == "" ? "0" : ((TextBox)grdMaterialDetails.Rows[0].FindControl("txtCommVolWt")).Text.Trim());
                
                // ------ Sumit 2014-10-30
                try
                {
                    if (txtVolume.Text.Trim() != "")
                    {
                        Volume = Convert.ToDouble(txtVolume.Text.Trim());
                    }
                }
                catch (Exception ex) { }

                double ChargeWgt = 0;
                // ------ Sumit 2014-10-30
                try
                {
                    if (txtChargeableWt.Text.Trim() != "")
                    {
                        ChargeWgt = Convert.ToDouble(txtChargeableWt.Text.Trim());
                    }
                }
                catch (Exception ex) { }
                
                ///function added for total of iata mkt rate on 6 may 12
                DataSet dsResult = new DataSet("GHA_QuickBooking_30");
                    dsResult = GetChargeSummury();
                double frateIATA = 0.0;
                double frateMKT = 0.0;
                double ValCharge = 0.0;
                string PayMode = "";
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    frateIATA = Convert.ToDouble(dsResult.Tables[0].Rows[0][0].ToString()); //Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTFrIATA")).Text);
                    frateMKT = Convert.ToDouble(dsResult.Tables[0].Rows[0][1].ToString());//Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTFrMKT")).Text);
                    ValCharge = 0; //Convert.ToDouble(((TextBox)GRDRates.Rows[0].FindControl("TXTValCharg")).Text);
                    PayMode = ddlPaymentMode.SelectedValue.ToString();
                }
                string OCDueCar = "";
                string OCDueAgent = "";


                OCDueCar = dsResult.Tables[0].Rows[0][2].ToString();
                OCDueAgent = dsResult.Tables[0].Rows[0][3].ToString();

                double SpotRate = 0;
                double DynaRate = 0;
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
                //LoginBL lbal = new LoginBL();
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
                            FltNo = FltNo + ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Value + ",";
                            
                        }
                        if (FltNo != "")
                        {
                            FltNo = FltNo.Remove(FltNo.Length - 1, 1);
                     
                       }
                        FltDate = (((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY);
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
                
                bool FFRChecked = false;//(((CheckBox)grdRouting.Rows[0].FindControl("chkFFR")).Checked);

                DataTable DTExportSubDetails = new DataTable("GHA_QuickBooking_158");

                DTExportSubDetails.Columns.Add("OtherCharges");
                DTExportSubDetails.Columns.Add("Amount");
                DTExportSubDetails.Columns.Add("Type");
                
                string strOtherCharges = "";

                DataSet dsOtherDetails = new DataSet("GHA_QuickBooking_31");
                   dsOtherDetails = (DataSet)Session["OCDetails"];
                
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

                                if (dsOtherDetails.Tables[0].Columns["Charge Head"] != null)
                                {
                                    if (row["Charge Head"].ToString().Substring(0, row["Charge Head"].ToString().IndexOf('/')).ToUpper() == "VLC")
                                    {
                                        ValCharge = Convert.ToDouble(row["Charge"].ToString());
                                    }
                                    else
                                    {
                                        strOtherCharges = strOtherCharges + row["Charge Head"].ToString().Substring(0, row["Charge Head"].ToString().IndexOf('/')) + ":" + row["Charge"].ToString() + ", ";
                                    }
                                }
                                else if (dsOtherDetails.Tables[0].Columns["Charge Head Code"] != null)
                                {
                                    if (row["Charge Head Code"].ToString().Substring(0, row["Charge Head Code"].ToString().IndexOf('/')).ToUpper() == "VLC")
                                    {
                                        ValCharge = Convert.ToDouble(row["Charge"].ToString());
                                    }
                                    else
                                    {
                                        strOtherCharges = strOtherCharges + row["Charge Head Code"].ToString().Substring(0, row["Charge Head Code"].ToString().IndexOf('/')) + ":" + row["Charge"].ToString() + ", ";
                                        //AC-186 changes done
                                        if (strOtherCharges.Contains("MOA:0"))
                                        {
                                            strOtherCharges=strOtherCharges.Replace("MOA:0", "");
                                        }

                                        if (strOtherCharges.Contains("MOC:0"))
                                        {
                                            strOtherCharges = strOtherCharges.Replace("MOC:0", "");
                                        }
                                    }


                                }
                            }

                            if (row[0].ToString().Contains('/'))
                            {
                                if (row[0].ToString().ToUpper() != "VAL")
                                {
                                    strOtherCharges = strOtherCharges + row[0].ToString().Substring(0, row[0].ToString().IndexOf('/')) + ":" + row[1].ToString() + " , ";
                                    DTExportSubDetails.Rows.Add(row[0].ToString().Substring(0, row[0].ToString().IndexOf('/')), row[1].ToString(), "Due Carriers");
                                    //Intcount = Intcount + 1;
                                    strOtherCharges = strOtherCharges + row[1].ToString() + ":" + row[3].ToString() + " , ";
                                   //for AC-186 chnges are done
                                    if (strOtherCharges.Contains("MOA:0,"))
                                    {
                                        strOtherCharges=strOtherCharges.Replace("MOA:0,", "");
                                    }
                                    if (strOtherCharges.Contains("MOC:0,"))
                                    {
                                        strOtherCharges = strOtherCharges.Replace("MOC:0,", "");
                                    }
                                
                                }
                                else
                                    ValCharge = ValCharge + Convert.ToDouble(row[3].ToString());
                            }
                        }
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
                string ExecDate = txtExecutionDate1.DateFormatDDMMYYYY;
                string ExecBy = (Session["UpdtBy"] != null && Session["UpdtBy"].ToString() != string.Empty) ? Session["UpdtBy"].ToString() : txtExecutedBy.Text;
                string ExecAT = txtExecutedAt.Text;
                string Consigneename = TXTConsignee.Text.Trim() + " " + TXTConAddress.Text.Trim() + " " + TXTConsigAdd2.Text.Trim() + Environment.NewLine + " Phone No-" + TXTConTelephone.Text.Trim();
                string prepaid = "";
                string TotalPrepaid = "";
                string ShipperName = TXTShipper.Text.Trim() + "  " + TXTShipAddress.Text.Trim() + "  " + TXTShipperAdd2.Text.Trim() + Environment.NewLine + " Phone No-" + TXTShipTelephone.Text.Trim();
                string RatePerKg = txtRatePerKG.Text.Trim();
                string SCI = txtSCI.Text.Trim();
                DataTable DTExport = new DataTable("GHA_QuickBooking_159");

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
                //new added field
                DTExport.Columns.Add("SCI");

                DataSet dsDimesionAll = new DataSet("GHA_QuickBooking_32");
                    dsDimesionAll = GenerateAWBDimensions("", Pcs, (DataSet)Session["dsDimesionAll"], Convert.ToDecimal(GrossWgt), false, "");

                    DataTable DTvolume = new DataTable("GHA_QuickBooking_160");
                DTvolume.Columns.Add("CommDesc");
                DTvolume.Columns.Add("Length");
                DTvolume.Columns.Add("Width");
                DTvolume.Columns.Add("Height");
                DTvolume.Columns.Add("Volume");
                DTvolume.Columns.Add("PCSCount");

                float Length = 0; float Breadth = 0; float Height = 0;
                int dimPCS = 0;
                string Units = string.Empty;

                if (dsDimesionAll != null && dsDimesionAll.Tables.Count > 1 && dsDimesionAll.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDimesionAll.Tables[1].Rows.Count; i++)
                    {
                        dimPCS = 0;

                        // ---- Sumit 2014-10-30
                        try
                        {
                            Length = float.Parse(dsDimesionAll.Tables[1].Rows[i]["Length"].ToString());
                            Breadth = float.Parse(dsDimesionAll.Tables[1].Rows[i]["Breadth"].ToString());
                            Height = float.Parse(dsDimesionAll.Tables[1].Rows[i]["Height"].ToString());
                            dimPCS = int.Parse(dsDimesionAll.Tables[1].Rows[i]["PcsCount"].ToString());
                            Units = dsDimesionAll.Tables[0].Rows[0]["Units"].ToString();
                        }
                        catch (Exception ex) { }
                        
                        if (Length > 0 && Breadth > 0 && Height > 0)
                        {
                            Volume = (Length * Breadth * Height) * dimPCS;
                            DTvolume.Rows.Add(CommDesc, Length, Breadth, Height, Volume, dimPCS);
                            strDimension = strDimension + "  DIMS: " + Length + " * " + Breadth + " * " + Height + " * " + dimPCS + "  " + Units +" ;    ";
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
                string ChargesCode = ddlPaymentMode.SelectedValue.ToString();
                string AirlineAddress = "";
                string RateClause = txtRateClass.Text;
                string OriginCity = "";
                string DestinationCity = "";
                string CopyType = string.Empty;
                string CustomerSupportInfo = "";

                string PscULDNo = "";
                ArrayList arr1 = new ArrayList();
                if (Session["PieceTypeULDNo_ArrayList"] != null)
                    arr1 = (ArrayList)Session["PieceTypeULDNo_ArrayList"];

                
                if (arr1.Count > 0)
                {
                    foreach (string li in arr1)
                    {
                        PscULDNo += li.ToString() + ",";
                    }
                    PscULDNo.Remove(PscULDNo.Length - 1);
                }


                string wtPPD = "", wtCOLL = "", OtherPPD = "", OtherCOLL = "";
                if (ChargesCode == "PP" || ChargesCode == "PX")
                    wtPPD = OtherPPD = "XX";
                if (ChargesCode == "CC")
                    wtCOLL = OtherCOLL = "XX";

                try
                {
                    MasterBAL ObjMsBAl = new MasterBAL();
                    DataSet dsMasterAirline = new DataSet("GHA_QuickBooking_33");
                       dsMasterAirline = ObjMsBAl.GetAirlineDetails(Origin, Dest);
                    ObjMsBAl = null;
                    // DataSet dsOrginDestination = objBLL.GetOriginDest(Origin, Dest);
                    if (dsMasterAirline.Tables.Count > 0)
                    {
                        if (dsMasterAirline.Tables[1].Rows.Count > 0)
                        {
                            OriginCity = dsMasterAirline.Tables[1].Rows[0][0].ToString();
                            if (dsMasterAirline.Tables[2].Rows.Count > 0)
                            {
                                DestinationCity = dsMasterAirline.Tables[2].Rows[0][0].ToString();
                                CustomerSupportInfo = dsMasterAirline.Tables[0].Rows[0]["CustomerSupportInfo"].ToString();
                            }
                        }
                    }

                    if (dsMasterAirline.Tables.Count > 0)
                    {
                        if (dsMasterAirline.Tables[0].Rows.Count > 0)
                        {
                            AirlineAddress = dsMasterAirline.Tables[0].Rows[0][0].ToString() + ", " + dsMasterAirline.Tables[0].Rows[0][1].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                
                if (Session["ShipAccp"] != null && Convert.ToBoolean(Session["ShipAccp"]))
                    CopyType = "Final Copy: Updated on " + Convert.ToDateTime(Session["UpdtOn"]).ToString("ddMMyy:HHmmss") + " Printed on " + Convert.ToDateTime(Session["IT"]).ToString("ddMMyy:HHmmss");
                else
                    CopyType = "Draft Copy: Updated on " + Convert.ToDateTime(Session["UpdtOn"]).ToString("ddMMyy:HHmmss") + " Printed on " + Convert.ToDateTime(Session["IT"]).ToString("ddMMyy:HHmmss");

                try
                {
                    if (IsAgreed == true)
                    {
                        if (ChargesCode == "PP" || ChargesCode == "PX")
                        {
                            DTExport.Rows.Add(DocType, AWBprefix, AWBno, AirLineCode, Origin, Dest, AgentCode, AgentName, Serviceclass, Handlinginfo, CommCode, CommDesc, Pcs,
                                GrossWgt.ToString("0.00"), Volume.ToString("0.00"), ChargeWgt.ToString("0.00"),
                                              "As agreed", "As agreed", "As agreed", PayMode, "As agreed", "As agreed", "As agreed", "As agreed", "As agreed", "As agreed", FltOrg, FltDest, FltNo, FltDate, FFRChecked, ExecDate, ExecBy, ExecAT, Consigneename,
                                              "As agreed", "As agreed", ShipperName, "As agreed", strDimension, ShipperAccountNo, ConsigneeAcNo, IssuingCarrierName, AgentIataCode, AccountCode, AccountInformation, ChargesCode, "P", "P", TXTDvForCarriage.Text, TXTDvForCustoms.Text,
                                              "KG", RateClause, CommCode, CommDesc, Length, Breadth, Height, "", "", "", "", "", "", drpCurrency.SelectedItem.Text.Trim(), "", "", AirlineAddress, AirlinePrefix, "As agreed", AccountInfo, OriginCity, DestinationCity, ms.ToArray(), CopyType, Logo.ToArray(),
                                              CustomerSupportInfo, wtPPD, wtCOLL, OtherPPD, OtherCOLL, "", PscULDNo,SCI);
                        }
                        else
                        {
                            DTExport.Rows.Add(DocType, AWBprefix, AWBno, AirLineCode, Origin, Dest, AgentCode, AgentName, Serviceclass, Handlinginfo, CommCode, CommDesc, Pcs, GrossWgt.ToString("0.00"), Volume.ToString("0.00"), ChargeWgt.ToString("0.00"),
                                              "", "", "", PayMode, "", "", "", "", "", "", FltOrg, FltDest, FltNo, FltDate, FFRChecked, ExecDate, ExecBy, ExecAT, Consigneename,
                                              "", "", ShipperName, "As agreed", strDimension, ShipperAccountNo, ConsigneeAcNo, IssuingCarrierName, AgentIataCode, AccountCode, AccountInformation, ChargesCode, "P", "P", TXTDvForCarriage.Text, TXTDvForCustoms.Text,
                                              "KG", RateClause, CommCode, CommDesc, Length, Breadth, Height, "As agreed", "As agreed", "As agreed", "As agreed", "As agreed", "", drpCurrency.SelectedItem.Text.Trim(), "", "", AirlineAddress, AirlinePrefix, "As agreed", AccountInfo, OriginCity, DestinationCity, ms.ToArray(), CopyType, Logo.ToArray(),
                                              CustomerSupportInfo, wtPPD, wtCOLL, OtherPPD, OtherCOLL, "As agreed", PscULDNo,SCI);
                        }
                    }
                    else
                    {                        
                        if (frateMKT > 0)
                        {
                            frateIATA = frateMKT;
                        }

                        try
                        {
                            OCDueCar = (Convert.ToDecimal(OCDueCar) - Convert.ToDecimal(ValCharge)).ToString("0.00");
                            OCDueCar = OCDueCar != string.Empty ? Convert.ToDecimal(OCDueCar).ToString("0.00") : OCDueCar;
                        }
                        catch
                        { }

                        if (ChargesCode == "PP" || ChargesCode == "PX")
                        {
                            DTExport.Rows.Add(DocType, AWBprefix, AWBno, AirLineCode, Origin, Dest, AgentCode, AgentName, Serviceclass, Handlinginfo, CommCode, CommDesc, Pcs, GrossWgt.ToString("0.00"), Volume.ToString("0.00"), ChargeWgt.ToString("0.00"), frateIATA.ToString("0.00"), frateMKT.ToString("0.00"), ValCharge.ToString("0.00"), PayMode, OCDueCar, OCDueAgent,
                                SpotRate.ToString("0.00"), DynaRate.ToString("0.00"), SerTax.ToString("0.00"), Total.ToString("0.00"), FltOrg, FltDest, FltNo, FltDate, FFRChecked, ExecDate, ExecBy, ExecAT, Consigneename,
                        prepaid, TotalPrepaid, ShipperName, strOtherCharges, strDimension, ShipperAccountNo, ConsigneeAcNo, IssuingCarrierName, AgentIataCode, AccountCode, AccountInformation, ChargesCode, "P", "P", TXTDvForCarriage.Text, TXTDvForCustoms.Text,
                                              "KG", RateClause, CommCode, CommDesc, Length, Breadth, Height, "0.00", "0.00", "0.00", "0.00", "0.00", "", drpCurrency.SelectedItem.Text.Trim(), "", "", AirlineAddress, AirlinePrefix, RatePerKg, AccountInfo, OriginCity, DestinationCity, ms.ToArray(), CopyType, Logo.ToArray(),
                                          CustomerSupportInfo, wtPPD, wtCOLL, OtherPPD, OtherCOLL, "0.00", PscULDNo,SCI);
                        }
                        else
                        {
                            DTExport.Rows.Add(DocType, AWBprefix, AWBno, AirLineCode, Origin, Dest, AgentCode, AgentName, Serviceclass, Handlinginfo, CommCode, CommDesc, Pcs, GrossWgt.ToString("0.00"), Volume.ToString("0.00"), ChargeWgt.ToString("0.00"),
                        "", "", "", PayMode, "", "", "", "", "", "", FltOrg, FltDest, FltNo, FltDate, FFRChecked, ExecDate, ExecBy, ExecAT, Consigneename,
                        prepaid, TotalPrepaid, ShipperName, strOtherCharges, strDimension, ShipperAccountNo, ConsigneeAcNo, IssuingCarrierName, AgentIataCode, AccountCode, AccountInformation, ChargesCode, "P", "P", TXTDvForCarriage.Text, TXTDvForCustoms.Text,
                                              "KG", RateClause, CommCode, CommDesc, Length, Breadth, Height, frateIATA.ToString("0.00"), SerTax.ToString("0.00"), OCDueAgent, OCDueCar, Total.ToString("0.00"), "", drpCurrency.SelectedItem.Text.Trim(), "", "", AirlineAddress, AirlinePrefix, RatePerKg, AccountInfo, OriginCity, DestinationCity, ms.ToArray(), CopyType, Logo.ToArray(),
                                          CustomerSupportInfo, wtPPD, wtCOLL, OtherPPD, OtherCOLL, ValCharge.ToString("0.00"), PscULDNo,SCI);
                        }


                    }

                    #region Print EAWB on the Same Page (Deepak)

                    #endregion
                    RenderReport(DTExport, DTExportSubDetails);
                }
                catch (Exception ex)
                {

                }
                //}

                string query = "'ShowEAWB.aspx?ID=" + 0 + "'";
                
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

        public void SaveRouteDetails()
        {
            DataSet dsRoutDetails = new DataSet("GHA_QuickBooking_34");
            try
            {
                dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Clone();

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    DataRow row = dsRoutDetails.Tables[0].NewRow();

                    row["FltOrigin"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                    row["FltDestination"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text;
                    row["FltDate"] = ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).DateFormatDDMMYYYY;
                    row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                    row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                    row["ChrgWt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim();
                    
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
            DataSet dsRoutDetails = new DataSet("GHA_QuickBooking_35");
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
                        if (prevdest.Equals(HidDestination.Value, StringComparison.OrdinalIgnoreCase) || prevdest.ToUpper() == HidDestination.Value)
                        {
                            prevdest = BookingBAL.OrgStation;
                        }
                        Pieces = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtPcs")).Text.Trim();
                        GrossWt = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtWt")).Text.Trim();
                        ChrgblWt = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtChrgWt")).Text.Trim();
                    }

                    strDate = ((DateControl)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFdate")).DateFormatDDMMYYYY;
                }

                SaveRouteDetails();

                // ---- Sumit 2014-10-30
                try
                {
                    dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Copy();
                }
                catch (Exception ex) { }
                
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

                HidProcessFlag.Value = "1";
                SetAWBStatusOnRoute();
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
                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender) ||
                        ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")) == ((DateControl)sender))
                    {
                        rowindex = i;
                    }
                }
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);
                
                #region Check Split Shipment

                int TotalPcs=0,TotalGrossWt=0;
                int.TryParse(txtPieces.Text.Trim(), out TotalPcs);
                int.TryParse(txtGrossWt.Text.Trim(), out TotalGrossWt);
                
                string org=((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper() ; 
                string dest=((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper();

                if (grdRouting.Rows.Count > 1)
                {
                    if (txtPieces.Text.Trim() != "" && txtGrossWt.Text.Trim() != "" && TotalPcs > 0 && TotalGrossWt > 0)
                    {
                        int EnteredPcs = 0,EnteredWt = 0;
                        double PCs = 0, Wt = 0, ChWt = 0;

                        for (int i = 0; i < rowindex; i++)
                        {
                            string CurrIndexOrg = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper();
                            string OtherDest = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.ToUpper();

                            if (org == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.ToUpper() && dest == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.ToUpper())
                            {
                                string pcs = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text;
                                string wt = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text;
                                if (pcs != "" && wt != "")
                                {
                                    if (EnteredPcs < TotalPcs && EnteredWt < TotalGrossWt)
                                    {
                                        EnteredPcs += Convert.ToInt32(pcs);
                                        EnteredWt += Convert.ToInt32(wt);

                                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text = (TotalPcs - EnteredPcs).ToString();
                                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text = (TotalGrossWt - EnteredWt).ToString();
                                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text;
                                    }
                                }
                            }
                            if (CurrIndexOrg == OtherDest)
                            {
                                PCs += Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text);
                                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text = PCs.ToString();

                                Wt += Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text);
                                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text = Wt.ToString();

                                ChWt += Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text);
                                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = ChWt.ToString();
                            }
                        }
                    }
                }
                #endregion

                HidProcessFlag.Value = "1";

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void txtFltNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")) == ((DropDownList)sender))
                    {
                        rowindex = i;
                    }
                }

                DataSet ds = new DataSet("GHA_QuickBooking_36");
                    ds = (DataSet)Session["Flt"];

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
            catch (Exception ex) { }
        }

        protected void btnDeleteRoute_Click(object sender, EventArgs e)
        {
            DataSet dsRouteDetailsTemp = new DataSet("GHA_QuickBooking_37"), dsRouteDetails = new DataSet("GHA_QuickBooking_38");
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
                        row["FltDate"] = ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).DateFormatDDMMYYYY;
                        row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                        row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                        row["ChrgWt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Text.Trim();
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
                HidProcessFlag.Value = "1";
                SetAWBStatusOnRoute();
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
            DataSet dsRouteDetails = new DataSet("GHA_QuickBooking_39");
            try
            {
                dsRouteDetails = (DataSet)Session["dsRoutDetails"];

                for (int i = 0; i < dsRouteDetails.Tables[0].Rows.Count; i++)
                {
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

        public void LoadFlights()
        {
            DataSet dsRoutDetails = new DataSet("GHA_QuickBooking_40");
            try
            {
                string Pieces = string.Empty;
                string GrossWt = string.Empty;
                string ChrgblWt = string.Empty;

                //RefreshMaterialDG();
                LoadAirlineCode("");
                //ddlPartnerType_SelectionChange(null, null);
                Pieces = txtPieces.Text;
                GrossWt = txtGrossWt.Text;
                ChrgblWt = txtChargeableWt.Text;

                if (HidFlightsChanged.Value == "1")
                {
                    // ---- Sumit 2014-10-30
                    try
                    {
                        dsRoutDetails = (DataSet)Session["dsSelectedFlights"];
                    }
                    catch (Exception ex) { }
                    
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
                                ((DateControl)grdRouting.Rows[rowIndex].FindControl("txtFdate")).DateFormatDDMMYYYY = dsRoutDetails.Tables[0].Rows[0]["FltDate"].ToString();
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
                                //((DropDownList)grdRouting.Rows[rowIndex].FindControl("ddlPartner")).Items.Add(new ListItem(m_Designator));
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
                            ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).DateFormatDDMMYYYY = dsRoutDetails.Tables[0].Rows[i]["FltDate"].ToString(); 
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
                            {//hardcode on 17/07/2013
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Clear();
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem("AIR"));
                                ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                                //((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(m_Designator));
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

        public void AutoPopulateData(bool editable, string AWBNumber, string AWBPrefix, string state)
        {
            DataSet dsResult = new DataSet("GHA_QuickBooking_41");
            lblRateStatus.Text = string.Empty;
            try
            {

                string errormessage = "";
                bool blnAcceptance = false;
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
                    txtShipmentDate1.SetCurrentDate = false;
                    txtExecutionDate1.SetCurrentDate = false;
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

                    if (dsResult != null && dsResult.Tables.Count > 11 && dsResult.Tables[11].Rows.Count > 0)
                    {
                        if (Convert.ToString(dsResult.Tables[11].Rows[0][0]) == "Y")
                            lblStatus.Text = lblStatus.Text + " Invoice exists.";
                    }

                    Session["BookingID"] = dsResult.Tables[0].Rows[0]["SerialNumber"].ToString();
                    txtAWBNo.Text = AWBNumber;
                    HidAWBNumber.Value = AWBNumber;
                    ddlDocType.Text = "" + dsResult.Tables[0].Rows[0]["DocumentType"].ToString();
                    txtAwbPrefix.Text = "" + dsResult.Tables[0].Rows[0]["AWBPrefix"].ToString();
                    chkDlvOnHAWB.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["IsDlvOnHAWB"]);
                    ddlAirlineCode.Text = dsResult.Tables[0].Rows[0]["DesignatorCode"].ToString();
                    txtShipmentPriority.Text = dsResult.Tables[0].Rows[0]["ShipmentPriority"].ToString();
                    //TXTCustomerCode.Text = dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();
                    Session["UpdtOn"] = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["UpdatedOn"]);
                    Session["UpdtBy"] = dsResult.Tables[0].Rows[0]["UpdatedBy"] != null ? dsResult.Tables[0].Rows[0]["UpdatedBy"] : string.Empty;
                    chkTBScreened.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["ToBeScreened"]);
                    txtPackingInfo.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["PackagingInfo"]);
                    hdAWBShipmentType.Value = dsResult.Tables[0].Rows[0]["AWBShipmentType"].ToString();
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
                    try
                    {
                        txtSCI.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["SCI"]);
                    }
                    catch (Exception ex) { }
                   
                    ddlOrg.Items.Clear();
                    ddlOrg.Items.Add("" + dsResult.Tables[0].Rows[0]["OriginCode"].ToString());
                    ddlOrg.SelectedIndex = 0;

                    HidOrigin.Value = dsResult.Tables[0].Rows[0]["OriginCode"].ToString();
                    HidDestination.Value = dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();

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

                    TXTAgentCode.Text = dsResult.Tables[0].Rows[0]["AgentCode"].ToString();
                    txtAgentName.Text = dsResult.Tables[0].Rows[0]["AgentName"].ToString();

                    if (TXTAgentCode.Text.Trim() != "")
                    {
                        TXTAgentCode.Enabled = false;
                        txtAgentName.Enabled = false;
                    }

                    //txtRemarks.Text = dsResult.Tables[0].Rows[0]["Remarks"].ToString();
                    txtComment.Text = dsResult.Tables[0].Rows[0]["Remarks"].ToString();
                    txtSpecialHandlingCode.Text = dsResult.Tables[0].Rows[0]["SHCCodes"].ToString();


                    txtAgentName.Text = "" + dsResult.Tables[0].Rows[0]["AgentName"].ToString();
                    txtHandling.Text = "" + dsResult.Tables[0].Rows[0]["HandlingInfo"].ToString();
                    //TXTCustomerCode.Text = "" + dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();
                    try 
                    {
                        //for checking paymode & ccsf
                        TXTAgentCode_TextChanged(null, null);
                    }
                    catch (Exception e) { }
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

                        //check shipper 
                        if (TXTShipper.Text == "" || TXTShipAddress.Text == "" || ddlShipCountry.SelectedItem.Text == "")
                        {
                            imgShipperTick.Attributes.Add("style", "display:none");
                            imgShipperCross.Attributes.Add("style", "display:inline");
                        }
                        else
                        {
                            imgShipperCross.Attributes.Add("style", "display:none");
                            imgShipperTick.Attributes.Add("style", "display:inline");
                        }

                        //check consignee
                        if (TXTConsignee.Text.Trim() == "" || TXTConAddress.Text.Trim() == "" || ddlConCountry.SelectedItem.Text == "")
                        {
                            imgConTick.Attributes.Add("style", "display:none");
                            imgConCross.Attributes.Add("style", "display:inline");
                        }
                        else
                        {
                            imgConCross.Attributes.Add("style", "display:none");
                            imgConTick.Attributes.Add("style", "display:inline");
                        }

                        txtShipperCCSF.Text = dsResult.Tables[6].Rows[0]["ShipperCCSF"].ToString();
                    }

                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[1].Rows.Count > 0)
                    {
                        // AWBRateMaster
                        DataSet dsMaterialDetails = new DataSet("GHA_QuickBooking_42");
                        // CommodityCode CodeDescription Pieces GrossWeight Dimensions VolumetricWeight ChargedWeight RowIndex
                        dsMaterialDetails.Tables.Clear();
                        dsMaterialDetails.Tables.Add(dsResult.Tables[1].Copy());
                        try
                        {
                            CHKAllIn.Checked = Convert.ToBoolean(dsMaterialDetails.Tables[0].Rows[0]["AllinRate"].ToString());
                        }
                        catch (Exception ex)
                        {
                            CHKAllIn.Checked = false;
                        }
                        
                        txtCommodityCode.Text = dsMaterialDetails.Tables[0].Rows[0]["CommodityCode"].ToString();
                        txtCommodityName.Text = dsMaterialDetails.Tables[0].Rows[0]["CodeDescription"].ToString();
                        ddlPaymentMode.SelectedValue = dsMaterialDetails.Tables[0].Rows[0]["PaymentMode"].ToString();
                        ddlShipmentType.SelectedValue = dsMaterialDetails.Tables[0].Rows[0]["ShipmentType"].ToString();

                        txtPieces.Text = "" + dsResult.Tables[8].Rows[0]["TotalPcs"].ToString();
                        txtGrossWt.Text = "" + dsResult.Tables[8].Rows[0]["TotalGrossWt"].ToString();
                        txtVolume.Text = "" + dsResult.Tables[8].Rows[0]["TotalVolume"].ToString();
                        txtChargeableWt.Text = "" + dsResult.Tables[8].Rows[0]["TotalChargedWt"].ToString();
                    }
                    
                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[3].Rows.Count > 0)
                    {
                        //AWBRouteMaster
                        //FltOrigin FltDestination FltNumber FltDate Pcs Wt Status Accepted AcceptedPcs
                        DataSet dsRoutDetails = new DataSet("GHA_QuickBooking_43");
                        dsRoutDetails.Tables.Clear();
                        dsRoutDetails.Tables.Add(dsResult.Tables[3].Copy());

                        grdRouting.DataSource = dsRoutDetails.Copy();
                        grdRouting.DataBind();
                        LoadAirlineCode("");
                        ddlPartnerType_SelectionChange(null, null);
                        LoadAWBStatusDropdown();

                        txtAccpPieces.Text = dsRoutDetails.Tables[0].Rows[0]["TotalAcceptedPcs"].ToString();
                        txtAccpWeight.Text = dsRoutDetails.Tables[0].Rows[0]["TotalAcceptedWt"].ToString();

                        Session["QB_ScreenedPcs"] = Convert.ToInt16(dsRoutDetails.Tables[0].Rows[0]["ScannedPcs"]);
                        Session["QB_ScreenedWt"] = Convert.ToDecimal(dsRoutDetails.Tables[0].Rows[0]["ScannedWt"]);

                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {                            
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(dsRoutDetails.Tables[0].Rows[i]["FltNumber"].ToString());
                            ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedIndex = 0;

                            if (dsRoutDetails.Tables[0].Rows[i]["Accepted"].ToString() == "Y" && txtAccpPieces.Text.Trim() != "" && txtAccpPieces.Text.Trim() != "0")
                            {
                                Session["ShipAccp"] = true;
                                blnAcceptance = true;
                            }
                            else
                                Session["ShipAccp"] = false;
                                                        
                            ddlAWBStatus.SelectedValue = dsRoutDetails.Tables[0].Rows[i]["Status"].ToString();
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
                        DataSet dtRates = new DataSet("GHA_QuickBooking_44");
                        dtRates.Tables.Clear();
                        dtRates.Tables.Add(dsResult.Tables[7].Copy());

                        try
                        {
                            drpCurrency.SelectedValue = dtRates.Tables[0].Rows[0]["CurrencyIndex"].ToString();
                            // drpCurrency.SelectedItem.Text = dtRates.Tables[0].Rows[0]["Currency"].ToString();
                            drpCurrency.Text = dtRates.Tables[0].Rows[0]["Currency"].ToString();
                            Session["QB_AGTCurrency"] = dtRates.Tables[0].Rows[0]["Currency"].ToString();
                        }
                        catch (Exception ex) { }

                        drpCurrency.SelectedValue = dtRates.Tables[0].Rows[0]["CurrencyIndex"].ToString();
                        txtFreightIATA.Text = dtRates.Tables[0].Rows[0]["FrIATA"].ToString();
                        txtFreightMKT.Text = dtRates.Tables[0].Rows[0]["FrMKT"].ToString();
                        txtRatePerKG.Text = dtRates.Tables[0].Rows[0]["RatePerKg"].ToString();
                        txtRatePerKGInt.Text = dtRates.Tables[0].Rows[0]["RatePerKg"].ToString();
                        txtRateClass.Text = dtRates.Tables[0].Rows[0]["RateClass"].ToString();
                        txtSpotRate.Text = dtRates.Tables[0].Rows[0]["SpotRate"].ToString();
                        txtspotid.Text = dtRates.Tables[0].Rows[0]["SpotRateId"].ToString();
                        txtOCDueCar.Text = dtRates.Tables[0].Rows[0]["OCDueCar"].ToString();
                        txtOCDueAgent.Text = dtRates.Tables[0].Rows[0]["OCDueAgent"].ToString();
                        if (dtRates.Tables[0].Rows[0]["FrMKT"].ToString().Trim() != "" && dtRates.Tables[0].Rows[0]["FrMKT"].ToString().Trim() != "0" && Convert.ToDecimal(dtRates.Tables[0].Rows[0]["FrMKT"]) > 0)
                            txtSubTotal.Text = Convert.ToString(Convert.ToDecimal(dtRates.Tables[0].Rows[0]["FrMKT"]) + Convert.ToDecimal(dtRates.Tables[0].Rows[0]["OCDueCar"]) + Convert.ToDecimal(dtRates.Tables[0].Rows[0]["OCDueAgent"]));
                        else
                            txtSubTotal.Text = Convert.ToString(Convert.ToDecimal(dtRates.Tables[0].Rows[0]["FrIATA"]) + Convert.ToDecimal(dtRates.Tables[0].Rows[0]["OCDueCar"]) + Convert.ToDecimal(dtRates.Tables[0].Rows[0]["OCDueAgent"]));
                        txtServiceTax.Text = dtRates.Tables[0].Rows[0]["ServTax"].ToString();
                        txtTotalAmount.Text = dtRates.Tables[0].Rows[0]["Total"].ToString();
                        txtCurrency.Text = drpCurrency.SelectedItem.ToString();

                        txtSpotFreight.Text = dtRates.Tables[0].Rows[0]["SpotFreight"].ToString();
                        txtSpotTax.Text = dtRates.Tables[0].Rows[0]["SpotTax"].ToString();
                        txtSpotStatus.Text = dtRates.Tables[0].Rows[0]["SpotStatus"].ToString();
                        
                        string IATAID = "", MKTID = "";
                        IATAID = dtRates.Tables[0].Rows[0]["IATARateID"].ToString();
                        MKTID = dtRates.Tables[0].Rows[0]["MKTRateID"].ToString();
                        //21/7/2014 - CS -442 Error listing AWB in Quick Booking
                        if (ddlServiceclass.SelectedItem.Text != "FOC" || ddlPaymentMode.SelectedItem.Text != "FOC")
                        {
                            if (IATAID.Length > 0 && MKTID.Length > 0 && !(IATAID.Contains(",")) && !(MKTID.Contains(",")))
                            {
                                if (Convert.ToInt64(IATAID.Replace(",","")) < 1 && Convert.ToInt64(MKTID.Replace(",","")) < 1)
                                {
                                    //lblRateStatus.Text = "No rates available";
                                    //lblRateStatus.ForeColor = Color.Red;
                                }
                            }
                        }
                        Session["dtRates"] = dtRates.Tables[0].Copy();
                    }                    

                    // OCDetails
                    DataSet dsDetails = new DataSet("GHA_QuickBooking_45");
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
                    dsDetails.Tables[0].Columns.Add("SerialNumber");

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
                    txtExecutionDate1.DateFormatDDMMYYYY = "" + dsResult.Tables[0].Rows[0]["ExecutionDate"].ToString();
                    //Set shipment date and time.
                    if (dsResult.Tables[0].Rows[0]["ShipmentDate"] != null &&
                        dsResult.Tables[0].Rows[0]["ShipmentDate"].ToString() != "")
                    {                        
                        //txtShipmentDate1.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["ShipmentDate"].ToString()).ToString("dd/MM/yyyy");
                        txtShipmentDate1.DateFormatDDMMYYYY = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["ShipmentDate"].ToString()).ToString("dd/MM/yyyy");
                        ddlShipmentTime.SelectedValue = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["ShipmentDate"].ToString()).ToString("HH:mm");
                        if (txtShipmentDate1.DateFormatDDMMYYYY == "01/01/1900")
                        {
                            //txtShipmentDate.Text = "";
                            txtShipmentDate1.SetCurrentDate=true;
                            ddlShipmentTime.SelectedValue = "25:00";
                        }
                    }
                    else
                    {
                        //txtShipmentDate.Text = "";
                        txtShipmentDate1.SetCurrentDate = true;
                        ddlShipmentTime.SelectedValue = "25:00";
                    }
                    //Disable product type dropdown so that user has to open product popup.
                    //ddlProductType.Enabled = false;

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
                        DataSet ds = new DataSet("GHA_QuickBooking_46");
                            ds = HAWB.GetHAWBDetails(AWBNumber, txtAwbPrefix.Text.Trim());
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
                        //added by jayant button is not enabled after acceptance of pcs
                        btnCollect.Enabled = true;
                        btnSaveAcceptance.Enabled = false;
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
                    btnSaveBooking.Enabled = false;

                    if (txtAccpPieces.Text.Trim() == "" || txtAccpPieces.Text.Trim() == "0")
                    {
                        if (Convert.ToInt16(Session["QB_ScreenedPcs"]) > 0)
                        {
                            txtAccpPieces.Text = Convert.ToString(Session["QB_ScreenedPcs"]);
                            txtAccpWeight.Text = Convert.ToString(Session["QB_ScreenedWt"]);
                        }
                    }

                    CheckAWBValidationsFlightCheckIn();
                }

                //visibility of shownotes image 
                #region shownotes
                bool flag = false;
                flag = CommonUtility.ShowNotes(txtAwbPrefix.Text.Trim(), txtAWBNo.Text.Trim(), "", "");
                imgNotebtn.Visible = flag;
                #endregion shownotes
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
            txtAWBNo.ReadOnly = true;
            ddlDocType.Enabled = false;
            txtAwbPrefix.Enabled = false;
            ddlAirlineCode.Enabled = false;
            ddlOrg.Enabled = false;
            ddlDest.Enabled = false;
            ddlServiceclass.Enabled = false;
            TXTAgentCode.Enabled = false;
            txtAgentName.Enabled = false;
            txtHandling.Enabled = false;
            TXTDvForCarriage.Enabled = true;

            CHKBonded.Enabled = false;
            CHKConsole.Enabled = false;
            CHKExportShipment.Enabled = false;
            CHKAsAggred.Enabled = false;

            chkTBScreened.Enabled = false;

            TXTShipper.Enabled = false;
            TXTShipTelephone.Enabled = false;
            TXTShipAddress.Enabled = false;
            ddlShipCountry.Enabled = false;

            TXTConsignee.Enabled = false;
            TXTConTelephone.Enabled = false;
            TXTConAddress.Enabled = false;
            ddlConCountry.Enabled = false;

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
                ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = IsEnabled;
                ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = IsEnabled;
            }

            btnSave.Enabled = IsEnabled;            
        }

        public void EnableDisable(string State)
        {

            switch (State)
            {

                case "Save":
                    #region Save
                    
                    txtAWBNo.ReadOnly = true;
                    ddlDocType.Enabled = false;
                    txtAwbPrefix.Enabled = false;
                    ddlAirlineCode.Enabled = false;

                    ddlOrg.Enabled = false;
                    ddlDest.Enabled = false;
                    ddlServiceclass.Enabled = false;
                    txtHandling.Enabled = false;
                    TXTDvForCarriage.Enabled = true;

                    CHKBonded.Enabled = false;
                    CHKConsole.Enabled = false;
                    CHKExportShipment.Enabled = false;
                    CHKAsAggred.Enabled = false;
                    chkTBScreened.Enabled = false;

                    TXTShipper.Enabled = false;
                    TXTShipTelephone.Enabled = false;
                    TXTShipAddress.Enabled = false;
                    ddlShipCountry.Enabled = false;

                    TXTConsignee.Enabled = false;
                    TXTConTelephone.Enabled = false;
                    TXTConAddress.Enabled = false;
                    ddlConCountry.Enabled = false;
                    
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
                        ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = false;
                    }
                                       
                    btnSave.Enabled = false;
                    btnSaveAndAccept.Enabled = false;
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
                    ddlServiceclass.Enabled = true;

                    //ddlAgtCode.Enabled = false;

                    TXTAgentCode.Enabled = true;
                    txtAgentName.Enabled = true;

                    txtHandling.Enabled = true;
                    //TXTCustomerCode.Enabled = true;

                    TXTDvForCustoms.Enabled = true;
                    TXTDvForCarriage.Enabled = true;

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
                        ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = true;
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
                    ddlServiceclass.Enabled = true;
                    txtHandling.Enabled = true;
                    TXTDvForCustoms.Enabled = true;
                    TXTDvForCarriage.Enabled = true;

                    CHKBonded.Enabled = true;
                    CHKConsole.Enabled = true;
                    CHKExportShipment.Enabled = true;
                    CHKAsAggred.Enabled = true;
                    chkTBScreened.Enabled = true;
                    TXTShipper.Enabled = true;
                    TXTShipTelephone.Enabled = true;
                    TXTShipAddress.Enabled = true;
                    ddlShipCountry.Enabled = true;

                    TXTConsignee.Enabled = true;
                    TXTConTelephone.Enabled = true;
                    TXTConAddress.Enabled = true;
                    ddlConCountry.Enabled = true;

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
                        ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = true;
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
                    txtSLAC.Enabled = false;
                    txtCustoms.Enabled = false;
                    txtEURIN.Enabled = false;
                    txtSpecialHandlingCode.Enabled = false;
                    txtAttchDoc.Enabled = false;
                    //ddlProductType.Enabled = false;
                    ddlDest.Enabled = false;
                    ddlServiceclass.Enabled = false;
                    TXTAgentCode.Enabled = false;
                    txtAgentName.Enabled = false;

                    txtHandling.Enabled = false;
                    TXTDvForCarriage.Enabled = true;

                    CHKBonded.Enabled = false;
                    CHKConsole.Enabled = false;
                    CHKExportShipment.Enabled = false;
                    CHKAsAggred.Enabled = false;
                    chkTBScreened.Enabled = false;
                    TXTShipper.Enabled = false;
                    TXTShipTelephone.Enabled = false;
                    TXTShipAddress.Enabled = false;
                    ddlShipCountry.Enabled = false;

                    TXTConsignee.Enabled = false;
                    TXTConTelephone.Enabled = false;
                    TXTConAddress.Enabled = false;
                    ddlConCountry.Enabled = false;

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
                        ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = false;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Enabled = false;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Enabled = false;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtChrgWt")).Enabled = false;
                    }
                                        
                    btnSave.Enabled = false;
                    btnSaveAndAccept.Enabled = false;
                    btnReopen.Enabled = true;
                    //txtShipmentDate.Enabled = false;
                    //imgShipmentDate.Enabled = false;
                    txtShipmentDate1.Enabled = false;
                    #endregion
                    break;

                case "EditReopen":
                    #region EditReopen
                    txtAWBNo.ReadOnly = true;
                    ddlDocType.Enabled = false;
                    txtAwbPrefix.Enabled = false;
                    ddlAirlineCode.Enabled = false;
                    ddlOrg.Enabled = false;
                    TXTAgentCode.Enabled = false;
                    txtAgentName.Enabled = false;
                    TXTDvForCarriage.Enabled = true;
                    chkTBScreened.Enabled = true;

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
                        ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Enabled = true;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Enabled = true;
                    }
                    
                    btnSave.Enabled = true;

                    #endregion
                    break;

            }
        }
        #endregion

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {
            // ---- Sumit 2014-10-30
            try
            {
                string[] orgdest = new GHA_QuickBooking().GetOrgDest();

                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '%" + prefixText + "%' or AgentCode like '%" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
                DataSet ds = new DataSet("GHA_QuickBooking_47");
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
            catch (Exception ex) { return null; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCodeWithName(string prefixText, int count)
        {
            try
            {
                string[] orgdest = new GHA_QuickBooking().GetOrgDest();

                DataSet ds = new DataSet("GHA_QuickBooking_48");
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
            // ---- Sumit 2014-10-30
            try
            {
                string[] orgdest = new GHA_QuickBooking().GetOrgDest();

                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentName from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
                DataSet ds = new DataSet("GHA_QuickBooking_49");
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
            catch (Exception ex) { return null; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetConsignee(string prefixText, int count)
        {
            // ---- Sumit 2014-10-30
            try
            {
                string[] orgdest = new GHA_QuickBooking().GetOrgDest();

                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentName from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[1] + "'", con);
                DataSet ds = new DataSet("GHA_QuickBooking_50");
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
            catch (Exception ex) { return null; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetShipperCode(string prefixText, int count)
        {
            // ---- Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                string strQuery = "SELECT Isnull(AccountCode,'') + '|' + Isnull(AccountName,'') + '|' + Isnull(PhoneNumber,'') + '|' ";
                strQuery = strQuery + "+ Isnull(Adress1,'') + '|' + Isnull(Country,'') + '|' + Isnull(Adress2,'') + '|' + Isnull(City,'')";
                strQuery = strQuery + "+ '|' + Isnull(State,'') + '|' +  CASE WHEN len(Isnull(Email,''))>30 THEN SUBSTRING(Isnull(Email,''),0,30) ELSE Isnull(Email,'') END + '|' + CONVERT(VARCHAR,Isnull(ZipCode,''))";
                strQuery = strQuery + " from AccountMaster where (AccountName like '" + prefixText + "%' or AccountCode like '" + prefixText + "%')";
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter(strQuery, con);
                DataSet ds = new DataSet("GHA_QuickBooking_51");
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
            catch (Exception ex) { return null; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetULDs(string prefixText, int count)
        {
            // ---- Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                SqlDataAdapter dad = new SqlDataAdapter("SELECT Distinct ULDNumber from tblULDMaster where ULDNumber like '" + prefixText + "%'", con);
                DataSet ds = new DataSet("GHA_QuickBooking_52");
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
            catch (Exception ex) { return null; }
        }

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                string errormessage = "";
                string FromEmail = "goaircargo@qidtech.com";
                string EmailPassword = "qidtech#1";
                string strCommodity = txtCommodityCode.Text.Trim();
                string ToEmail = string.Empty;
                string strSubject = strCommodity + " - Email Intimation";
                string strBody = string.Empty;

                if (ddlPaymentMode.SelectedIndex <= 0 || ddlPaymentMode.SelectedItem.Text.ToUpper() == "SELECT")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Select Payment Mode.";
                    return;
                }
                                
                //Validate shipper consignee details based on configured flag.
                if (CommonUtility.ShipperMandatoryDuring != null && CommonUtility.ShipperMandatoryDuring.ToUpper() == "EX")
                {
                    if (TXTShipper.Text.Trim() == "" || TXTShipTelephone.Text.Trim() == "" ||
                        TXTShipAddress.Text.Trim() == "" || ddlShipCountry.Text.Trim() == "")
                    {
                        lblStatus.Text = "Please enter Shipper details.";
                        lblStatus.ForeColor = Color.Red;
                        btnExecute.Enabled = false;
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
                        btnExecute.Enabled = false;
                        return;
                    }
                }
                
                bool checkbeforereopen = true;
                checkbeforereopen = CheckIfAWBUpdateAllowedForRole();
                if (AWBExecution(checkbeforereopen))
                {
                    //ObjCustoms.UpdateFRIMessage(txtAwbPrefix.Text.Trim() + txtAWBNo.Text.Trim());
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWB executed successfully!";

                    btnExecute.Enabled = false;                    
                    txtAccpPieces.Enabled = true;
                    txtAccpWeight.Enabled = true;
                    imgAcceptance.Enabled = true;
                    btnSaveAcceptance.Enabled = true;                   

                    Session["AWBStatus"] = "E";

                    EnableDisable("EditExecuted");

                    btnSave.Enabled = false;
                    btnSaveAndAccept.Enabled = false;
                    //txtShipmentDate.Enabled = false;
                    //imgShipmentDate.Enabled = false;
                    txtShipmentDate1.Enabled = false;
                    btnReopen.Enabled = true;

                    //If AWB is Voided - Vijay - 23-09-2014
                    if (ddlServiceclass.SelectedValue == "0" || ddlServiceclass.SelectedItem.Text.Trim() == "Void")
                    {
                        Session["AWBStatus"] = "V";
                        txtAccpPieces.Enabled = false;
                        txtAccpWeight.Enabled = false;
                        imgAcceptance.Enabled = false;
                        btnSaveAcceptance.Enabled = false;
                        btnCollect.Enabled = true;

                        //Generate WalkIn invoice for Void AWBs on execution (AC-22)
                        GenerateWalkinInvoice();
                    }
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
                                strFligtDet.Append(((DateControl)grdRouting.Rows[ICount].FindControl("txtFdate")).DateFormatDDMMYYYY);
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
                if (objBLL.SetAWBStatus(HidAWBNumber.Value.Trim(), "R", ref errormessage,
                    dtCurrentDate.ToString("dd/MM/yyyy"), strUserName, dtCurrentDate, txtAwbPrefix.Text, checkbeforereopen, ddlPaymentMode.SelectedValue))
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWB reopend successfully!";

                    btnExecute.Enabled = true;
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string errormessage = "";
                bool checkbeforereopen = true;
                checkbeforereopen = CheckIfAWBUpdateAllowedForRole();
                if (objBLL.SetAWBStatus(HidAWBNumber.Value.Trim(), "D", ref errormessage,
                    dtCurrentDate.ToString("dd/MM/yyyy"), strUserName, dtCurrentDate, txtAwbPrefix.Text, checkbeforereopen, ddlPaymentMode.SelectedValue))
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
                result[0] = Convert.ToString(Session["Origin"]);
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
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if ((TextBox)(((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).FindControl("txtDate")) != ((TextBox)sender) && ((TextBox)sender).ID != "txtDate")
                    {
                        if (HidChangeDate.Value != "Y")
                        {                            
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                            return;
                        }
                    }

                    //if (((TextBox)grdRouting.Rows[i].FindControl("txtFdate")) == ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender))
                    if ((TextBox)(((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).FindControl("txtDate"))== ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender))
                    {
                        rowindex = i;
                    }
                }

                GetFlightRouteData(rowindex);
            }
            catch (Exception ex)
            {

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }

        protected void txtAWBNo1_TextChanged(object sender, EventArgs e)
        {
            if (txtAWBNo.Text == "")
            {
                lblStatus.Text = "Please enter valid AWB.";
                return;
            }
            else
                btnListAgentStock_Click(null, null);           
                
        }

        protected void btnListAgentStock_Click(object sender, EventArgs e)
        {
            if (CheckManualAWB("L") == false)
                return;
            
        }

        protected void btnClearAgentStock_Click(object sender, EventArgs e)
        {
            Session["dsDimesionAll"] = null;
            Response.Redirect("~/GHA_QuickBooking.aspx" + "?GHA=" + Convert.ToString(Session["GHABooking"]), false);
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

        private int GetScheduleId(string Origin, string Destination, string FlightNo)
        {
            int ScheduleId = 0;
            BookingBAL objBooking = new BookingBAL();
            objBooking.GetScheduleId(Origin, Destination, FlightNo, ref ScheduleId);
            return ScheduleId;
        }

        private void PrepareAWBDimensions(string AWBNumber, string AWBPrefix)
        {
            DataSet ds = new DataSet("GHA_QuickBooking_53");
            BookingBAL objBooking = new BookingBAL();
            try
            {
                ds = objBooking.GetAWBDimensions(AWBNumber, AWBPrefix);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["dsDimesionAll"] = ds;
                }
                else
                    Session["dsDimesionAll"] = null;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    DataSet ddt = new DataSet("GHA_QuickBooking_54");
                    DataTable dt = new DataTable("GHA_QuickBooking_161");
                        dt = ds.Tables[1].Copy();
                    ddt.Tables.Add(dt);
                    Session["dsPiecesDet"] = ddt;
                    ddt = null;
                    dt = null;
                }
                else
                    CreateAWBPiecesDataSet();

                if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    DataSet ddt = new DataSet("GHA_QuickBooking_55");
                    DataTable dt = new DataTable("GHA_QuickBooking_162");
                        dt = ds.Tables[1].Copy();
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

        private bool CheckforAgentsBalance(string AgentCode, decimal AWBRate, string AWBNo, ref string TranAccount, ref decimal BGAmount
            , ref decimal dcAWBPrevAmount, ref decimal BankGAmt, ref decimal ThrValue, ref bool ValidateBG, string AWBPrefix, decimal CurrencyConversion)
        {
            //decimal dcAWBPrevAmount = 0;            

            if (AWBNo != "")
                dcAWBPrevAmount = objBLL.GetAWBRateAmount(AWBNo, AWBPrefix, CurrencyConversion);
            else
                dcAWBPrevAmount = 0;

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
            DataSet dtCapacityDet = new DataSet("GHA_QuickBooking_56");
            DataTable dtCapacity = new DataTable("GHA_QuickBooking_57");
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
                        DateTime dt =((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).DateValue;
                        //string fltId = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Text.ToString()
                        string fltId = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.ToString();
                        // string abc = ((DropDownList) grdRouting.Rows[i].FindControl("ddlFltNum")).Text;
                        object[] objcap = new object[3];

                        objcap.SetValue(dt, 0);
                        objcap.SetValue(fltId, 1);
                        objcap.SetValue(ddlOrg.Text.Trim(), 2);

                        DataSet dataCap = new DataSet("GHA_QuickBooking_58");
                            dataCap = capacity.ShowCapacity(objcap);
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

                    Response.Redirect("showCapacity.aspx", false);
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
            Response.Redirect("HAWBBooking.aspx", false);
        }
        #endregion

        #region getFFADataOfAWB
        private DataSet getFFADataOfAWB(string AWBNo)
        {
            DataSet dsData = new DataSet("GHA_QuickBooking_59");
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

        #region Load Airline code
        public void LoadAirlineCode(string filter)
        {
            DataSet ds = new DataSet("GHA_QuickBooking_60");
            try
            {
                //DataSet ds = objBLL.GetPartnerType(chkInterline.Checked);
                //ds = objBLL.GetPartnerType(true);

                if (CommonUtility.PartnerTypeMaster == null)
                {
                    BookingBAL objBookingBal = new BookingBAL();
                    CommonUtility.PartnerTypeMaster = objBookingBal.GetPartnerType(true);
                    objBookingBal = null;
                }

                ds = CommonUtility.PartnerTypeMaster;

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
                ((DateControl)grdRouting.Rows[rowindex].FindControl("txtFdate")).DateFormatDDMMYYYY = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                //((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Text = "";
            }
            catch (Exception ex) { }
        }
        #endregion

        #region GetFlightRouteData
        private void GetFlightRouteData(int rowindex)
        {
            DataSet dsresult = new DataSet("GHA_QuickBooking_61");
            try
            {
                string strPartnerCode = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                string errormessage = "";
                // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                int CurrentHrs = Convert.ToDateTime(Session["IT"]).Hour;
                int CurrentMin = Convert.ToDateTime(Session["IT"]).Minute;
                int allowedHrs = Convert.ToInt32(Session["StationCutOffTime"]);

                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((DateControl)grdRouting.Rows[rowindex].FindControl("txtFdate")).DateFormatDDMMYYYY, CurrentHrs, CurrentMin, allowedHrs, ref errormessage, strPartnerCode);

                if (dsresult == null || dsresult.Tables.Count < 1 || dsresult.Tables[0].Rows.Count < 1)
                {
                    string strDate = ((DateControl)(grdRouting.Rows[rowindex].FindControl("txtFdate"))).DateFormatDDMMYYYY;

                    ((DateControl)(grdRouting.Rows[rowindex].FindControl("txtFdate"))).DateFormatDDMMYYYY = DateTime.ParseExact(strDate, "dd/MM/yyyy", null).AddDays(1).ToString("dd/MM/yyyy");
                    dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((DateControl)grdRouting.Rows[rowindex].FindControl("txtFdate")).DateFormatDDMMYYYY, CurrentHrs, CurrentMin, allowedHrs, ref errormessage, strPartnerCode);
                }

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    DataSet ds = new DataSet("GHA_QuickBooking_62");
                        ds = (DataSet)Session["Flt"];
                    if (ds != null)
                    {
                        string name = "Table" + rowindex.ToString();
                        //if (rowindex == 0)    //Added by Vishal for issue:CS-453 but commented as issue got resolved
                        //{                     //without this change and impact of this change is not known.
                        //    name = "Table";
                        //}
                        try
                        {
                            if (ds.Tables.Count > rowindex)
                            {
                                try
                                {
                                    if (ds.Tables[name] != null && ds.Tables[name].Rows.Count > 0)
                                    {
                                        ds.Tables.Remove(name);
                                        DataTable dt = new DataTable("GHA_QuickBooking_163");
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
        }
        #endregion

        #region btnSenfwb_Click
        protected void btnSenfwb_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("GHA_QuickBooking_63");
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

                lblMAWBTotWt.Text = txtGrossWt.Text.Trim();
                lblMAWBTotPcs.Text = txtPieces.Text.Trim();
                lblUOMHAWB.Text = txtUOM.Text.Trim();

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
                DataSet ds = new DataSet("GHA_QuickBooking_64");

                if (Session["HAWBDetails"] == null)
                    ds = HAWB.GetHAWBDetails(MAWBNo, txtAwbPrefix.Text);//(Session["MAWBNo"].ToString());
                else
                {
                    ds = new DataSet("GHA_QuickBooking_65");
                    ds.Tables.Add(((DataTable)Session["HAWBDetails"]).Copy());
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 0)
                {
                    Session["HAWBDetails"] = ds.Tables[0];
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);

                    Refresh_gvH();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "callShow1();", true);
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
                DataTable dt = new DataTable("GHA_QuickBooking_164");
                    dt = (DataTable)(Session["HAWBDetails"]);
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
            DataTable dt = new DataTable("GHA_QuickBooking_165");
            try
            {
                dt = (DataTable)(Session["HAWBDetails"]);
                if (dt.Rows.Count == 0)
                {

                    dt.Rows.Add(string.Empty, string.Empty, 0, 0, "",string.Empty,
                    string.Empty, ddlOrg.Text, ddlDest.Text, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, "", "", "", "", string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0);
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
            DataTable dt = new DataTable("GHA_QuickBooking_166");
                dt = (DataTable)Session["HAWBDetails"];
            try
            {
                if (dt != null)
                {
                    HAWB.Delete_All_MAWB_HAWB(HidAWBNumber.Value,txtAwbPrefix.Text.Trim());

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
            DataSet Ds = new DataSet("GHA_QuickBooking_66");
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

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Discount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AgentComm";
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
            DataTable dt = new DataTable("GHA_QuickBooking_167");
            try
            {
                dt = (DataTable)Session["StatusMaster"];
                DropDownList ddl = null;

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    ddl = ddlAWBStatus;

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
                FlightDate = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatMMDDYYYY; //DateTime.ParseExact(((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
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
            DataTable dtCreditInfoA = new DataTable("GHA_QuickBooking_168");
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
                    //Response.Redirect("frmULDToAWBAssoc.aspx",false,false);
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

                if (e != null)
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
            DataSet ds = new DataSet("GHA_QuickBooking_67");
            try
            {
                if (sender.Equals(txtConsigneeCode))
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
                            ddlShipCountry.SelectedIndex = ddlShipCountry.Items.IndexOf(ddlShipCountry.Items.FindByText(ds.Tables[0].Rows[0]["Country"].ToString()));
                            TXTShipPinCode.Text = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                            TXTShipperEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                            txtShipperCCSF.Text = ds.Tables[0].Rows[0]["CCSFApprovalNo"].ToString();

                            if (txtShipperCCSF.Text.Trim() != "")
                                chkTBScreened.Checked = false;
                        }
                    }

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
                        ddlConCountry.SelectedIndex = ddlShipCountry.Items.IndexOf(ddlShipCountry.Items.FindByText(ds.Tables[0].Rows[0]["Country"].ToString()));
                        TXTConsigPinCode.Text = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                        TXTConsigEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                    }
                    else
                    {
                        return;
                    }

                    //ShipperConDetailsChenged(null, null);
                }
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
            DataSet dsAWBData = new DataSet("GHA_QuickBooking_68");
            try
            {
                string AWBNumber = string.Empty;

                if (txtAWBNo.Text.Trim() == "")
                    AWBNumber = HidAWBNumber.Value;
                else
                    AWBNumber = txtAWBNo.Text.Trim();

                object[] AwbRateInfo = new object[2];
                int i = 0;

                AwbRateInfo.SetValue(AWBNumber, i);
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
                                    btnCollect.Enabled = true;

                                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "GenerateWalkingInvoiceShow")))
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
                    DataSet ds = new DataSet("GHA_QuickBooking_69");
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
            // ---------- Sumit 2014-10-30
            try
            {
                StringBuilder StrDimensions = new StringBuilder();
                DataSet dsDim = new DataSet("GHA_QuickBooking_70");
                    dsDim = (DataSet)Session["dsDimesionAll"];

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
                                DeptTime = "00:00";// ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedValue;

                                flight = ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedItem.Text;
                            }
                        }
                        catch (Exception ex) { }


                        StrFltDetails.Append("Insert into #FltDetails (fltNo, fltDate, origin, Dest, FltDepTime, FltPcs, FltGrossWt) values('");
                        StrFltDetails.Append(flight);
                        StrFltDetails.Append("','");
                        StrFltDetails.Append(((DateControl)grdRouting.Rows[intCount].FindControl("txtFdate")).DateFormatMMDDYYYY);
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
            }
            catch (Exception ex) { }
            return strResult;
        }

        private void FillHandlerCodes()
        {
            DataSet dsHandler = new DataSet("GHA_QuickBooking_71");
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
                            ddlHandler.SelectedValue = "";
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
            DataTable dtULDInfo = new DataTable("GHA_QuickBooking_169");
                dtULDInfo = (DataTable)Session["AWBULD"];
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
                            
                            paramname = null;
                            paramtype = null;
                            paramvalue = null;
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
        
        #region GetHeavyWtDefinedInSystem
        private decimal GetHeavyWtDefinedInSystem()
        {
            decimal val = decimal.MaxValue;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet("GHA_QuickBooking_72");
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
            DataSet dsPieces = new DataSet("GHA_QuickBooking_73");
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
            DataSet dsPieces = new DataSet("GHA_QuickBooking_74");
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
            DataSet dsAWBPiecesAll = new DataSet("GHA_QuickBooking_75");
            try
            {
                dsAWBPiecesAll.Tables.Add(new DataTable("GHA_QuickBooking_170"));

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
                string AWBnmbr = HidAWBNumber.Value;
                string Date = Session["IT"].ToString();

                UserName = Session["UserName"].ToString();
                string[] param = { "name", "comments", "date", "AWBNumber" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { UserName, txtComment.Text, Date, AWBnmbr };
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
            DataSet ds = new DataSet("GHA_QuickBooking_76");
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
                    param = null;
                    dbtypes = null;
                    values = null;
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
            DataSet ds = new DataSet("GHA_QuickBooking_77");
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
            // ------ Sumit 2014-10-30
            try
            {
                string strAWBNumber = ((TextBox)sender).Text.Trim();
                if (strAWBNumber.Length > 8)
                {
                    txtAwbPrefix.Text = strAWBNumber.Replace(strAWBNumber.Substring(strAWBNumber.Length - 8, 8), "");
                    txtAWBNo.Text = strAWBNumber.Replace(txtAwbPrefix.Text.Trim(), "");
                    btnListAgentStock_Click(null, null);
                }
            }
            catch (Exception ex) { }
        }

        protected void txtAWBNo_DataBinding(object sender, EventArgs e)
        {
            // ------ Sumit 2014-10-30
            try
            {
                string strAWBNumber = ((TextBox)sender).Text.Trim();
                if (strAWBNumber.Length > 8)
                {
                    txtAwbPrefix.Text = strAWBNumber.Replace(strAWBNumber.Substring(strAWBNumber.Length - 8, 8), "");
                    txtAWBNo.Text = strAWBNumber.Replace(txtAwbPrefix.Text.Trim(), "");
                    btnListAgentStock_Click(null, null);
                }
            }
            catch (Exception ex) { }
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
                Session["FltDate"] = ((DateControl)grdRouting.Rows[rowIndex].FindControl("txtFdate")).DateFormatDDMMYYYY;
                Session["ShowFltRowIndex"] = rowIndex;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>showFlights('',1)</script>", false);

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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetCommodityCodesWithName(string prefixText, int count)
        {
            // ------ Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT CommodityCode + '(' + Description + ')' + '$' + Isnull(SHCCode,'') from CommodityMaster where (Description like '%" + prefixText + "%' or CommodityCode like '%" + prefixText + "%')", con);
                DataSet ds = new DataSet("GHA_QuickBooking_78");
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
            catch (Exception ex) { return null; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetDriverDetails(string prefixText, int count)
        {
            // ------ Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT DriverName + '(' + DLNumber + ')' from DriverMaster where (DriverCode like '%" + prefixText + "%' or DriverName like '%" + prefixText + "%')", con);
                DataSet ds = new DataSet("GHA_QuickBooking_79");
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
            catch (Exception ex) { return null; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetIACDetails(string prefixText, int count)
        {
            // ------ Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT IACNumber from CCSFMaster where IACNumber like '%" + prefixText + "%'", con);
                DataSet ds = new DataSet("GHA_QuickBooking_80");
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
            catch (Exception ex) { return null; }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetKnownShipperDetails(string prefixText, int count)
        {
            // ------ Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT ShipperCode + '(' + ShipperName + ')' from KnownShipperMaster where (ShipperCode like '%" + prefixText + "%' or ShipperName like '%" + prefixText + "%')", con);
                DataSet ds = new DataSet("GHA_QuickBooking_81");
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
            catch (Exception ex) { return null; }
        }

        #region UpdatePartnerCode
        private void UpdatePartnerCode(int rowindex)
        {
            DataSet dsResult = new DataSet("GHA_QuickBooking_82");

            try
            {
                string errormessage = "";

                if (CommonUtility.PartnerMaster == null)
                {
                    BookingBAL objBookingBal = new BookingBAL();
                    CommonUtility.PartnerMaster = objBookingBal.GetAvailabePartners();
                    objBookingBal = null;
                }

                dsResult = CommonUtility.PartnerMaster;

                //if (objBLL.GetAvailabePartners(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), dtCurrentDate, ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartnerType")).Text.ToUpper(), ref dsResult, ref errormessage))
                //{
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
                //}

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

        #region ProcessRatesSP
        private DataSet ProcessRatesSP(string agentcode, string commcode)
        {
            DataSet dsRates = new DataSet("GHA_QuickBooking_83");
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
                                ChrgWT = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_Weight", ChrgWT, txtExecutionDate1.DateFormatDDMMYYYY,TXTAgentCode.Text,txtShipperCode.Text,txtConsigneeCode.Text);
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
                            FltDetails.Append(((DateControl)grdRouting.Rows[intCount].FindControl("txtFdate")).DateFormatDDMMYYYY);
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

                #region RoutePieceDetails
                try
                {
                    if (isHeavyShipmentOnRoute())
                    {
                        DataSet dsPieces = new DataSet("GHA_QuickBooking_84");
                            dsPieces = (DataSet)Session["dsRouteds"];
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
                    DateTime execdate = txtExecutionDate1.DateValue;// DateTime.ParseExact(txtExecutionDate.Text.Trim(), "dd/MM/yyyy", null);
                    string PayMode = ddlPaymentMode.SelectedValue.ToString();
                    if (ddlServiceclass.SelectedItem.Text.Trim() == "FOC")
                        PayMode = "FOC";

                    bool IsVoided = false;
                    try
                    {
                        if (ddlServiceclass.SelectedItem.Text.Equals("Void", StringComparison.OrdinalIgnoreCase))
                        {
                            IsVoided = true;
                        }
                    }
                    catch (Exception ex) { }
                    int Pieces = Convert.ToInt16(txtPieces.Text.Trim());
                    decimal GrossWt = Convert.ToDecimal(txtGrossWt.Text.Trim());
                    decimal ChargeableWt = Convert.ToDecimal(txtChargeableWt.Text.Trim());
                    try
                    {
                        decimal num = 0;
                        string Wt = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_Weight", txtChargeableWt.Text.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                        if (Decimal.TryParse(Wt, out num))
                        {
                            ChargeableWt = Convert.ToDecimal(Wt);
                            txtChargeableWt.Text = Wt;
                        }

                    }
                    catch (Exception ex)
                    {
                        ChargeableWt = Convert.ToDecimal(txtChargeableWt.Text.Trim());
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

                    DataTable dtULDInfo = new DataTable("GHA_QuickBooking_171");
                        dtULDInfo = (DataTable)Session["AWBULD"];

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
                    DataSet dsDimensions = new DataSet("GHA_QuickBooking_85");
                        dsDimensions = (DataSet)Session["dsDimesionAll"];
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
                        string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_DeclaredValue", TXTDvForCarriage.Text.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
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
                        PieceDetails.ToString(), RoutePieceDetails.ToString(), interline, Handler, AWBPrefix, isExport, ScreenedReq, IssueCarrier,
                        txtShipperCode.Text.Trim(), txtConsigneeCode.Text.Trim(), slac, txtPackingInfo.Text.Trim(), CHKAllIn.Checked);


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
            DataTable myDataTable = new DataTable("GHA_QuickBooking_172");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet("GHA_QuickBooking_86");
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
                DateTime execdate = txtExecutionDate1.DateValue; //DateTime.ParseExact(txtExecutionDate.Text.Trim(), "dd/MM/yyyy", null);
                string PayMode = ddlPaymentMode.SelectedValue.ToString();
                string Origin = ddlOrg.Text.Trim();
                string Destination = ddlDest.Text.Trim();
                bool IsVoided = false;
                int Pieces = Convert.ToInt16(txtPieces.Text.Trim());
                decimal GrossWt = Convert.ToDecimal(txtGrossWt.Text.Trim());
                decimal ChargeableWt = Convert.ToDecimal(txtChargeableWt.Text.Trim());
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

                        if (!carrier.Equals(m_Designator, StringComparison.OrdinalIgnoreCase))
                        {
                            org = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim().ToUpper();
                            dest = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text.Trim().ToUpper();
                            issuecarrier = m_Designator;
                            weight = float.Parse(((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text.Trim());
                            date = ((DateControl)grdRouting.Rows[intCount].FindControl("txtFdate")).DateValue;
                            DataSet dsViability = new DataSet("GHA_QuickBooking_87");
                                dsViability =  objProcessRate.GetViabilityResult(org, dest, flight, carrier, weight, commcode, SHC, date, agentcode, ProductType, issuecarrier, currency);
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
            DataSet ds = new DataSet("GHA_QuickBooking_88");
            BookingBAL BAL = new BookingBAL();
            int Rowcount = 0;
            string FlightNo = string.Empty, FlightDt = string.Empty;

            int intDimRowCount = 0;
            int FltPieces = 0;

            try
            {
                
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
               Convert.ToDateTime(Session["IT"]), IsCreate, AWBPrefix,false);

                }
            }
            catch { }


            BAL = null;
            Dimensions = null;
            return ds;
        }
        #region GenerateAWBDimensions
        private DataSet GenerateAWBDimensions(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate, string AWBPrefix, string FlightNo, string FlightDate)
        {
            DataSet ds = new DataSet("GHA_QuickBooking_89");
            BookingBAL BAL = new BookingBAL();
            int Rowcount = 0;
            // string FlightNo = string.Empty, FlightDt = string.Empty;

            int intDimRowCount = 0;
            int FltPieces = 0;

            try
            {
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
        public void SetOtherChargesSummary(ref float OCDC, ref float OCDA, ref float OCTax)
        {
            DataSet dsDetails = new DataSet("GHA_QuickBooking_90");
            DataSet dsDetailsFinal = new DataSet("GHA_QuickBooking_91");
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
                        OCTax += float.Parse(dr["Tax"].ToString());

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

        #region SetOtherChargesSummary
        public void SetOtherChargesSummary(ref float OCDC, ref float OCDA, ref float OCTax, ref float OATax)
        {
            DataSet dsDetails = new DataSet("GHA_QuickBooking_92");
            DataSet dsDetailsFinal = new DataSet("GHA_QuickBooking_93");
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
            DataSet ds = new DataSet("GHA_QuickBooking_94");
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
                                    // ------ Sumit 2014-10-30
                                    try
                                    {
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
                                    }
                                    catch (Exception ex) { }
                                    #endregion

                                    #region FWB Message
                                    string fwbMsg = cls_BL.EncodeFWB(ds, ref error);
                                    // ------ Sumit 2014-10-30
                                    try
                                    {
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
                                    }
                                    catch (Exception ex) { }
                                    #endregion

                                    #region FHL Message 
                                    // ------ Sumit 2014-10-30
                                    try
                                    {
                                        if (CHKConsole.Checked)
                                        {
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
                                        }
                                    }
                                    catch (Exception ex) { }
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
            DataSet ds = new DataSet("GHA_QuickBooking_95");
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
                DataTable dtDGR = new DataTable("GHA_QuickBooking_173");
                    dtDGR = (DataTable)Session["DgrCargo"];
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
            catch(Exception ex){}

        }

        protected void btnPrintSelDGRLbl_Click(object sender, EventArgs e)
        {
            string pcs = txtPieces.Text.ToString();
            
            try
            {
                lblErrorMsg.Text = string.Empty;
                int FromLbl, ToLbl;
                int totPcs = int.Parse(txtPieces.Text.ToString());
                if (radPrnAllLbl.Checked)
                {
                    string awb = txtAwbPrefix.Text + txtAWBNo.Text;
                    string org = ddlOrg.SelectedItem.Text;
                    string dest = ddlDest.SelectedItem.Text;

                    string frmPcs, toPcs;
                    frmPcs = "1";
                    toPcs=pcs;

                    DataTable dtDGR = new DataTable("GHA_QuickBooking_174");
                        dtDGR = (DataTable)Session["DgrCargo"];
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
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>popup('" + HdnUNIDForDGRLbl.Value + "','"+ org+"','"+dest+"','"+awb+"','"+HdnPcsForDGRLbl.Value+"','"+pcs+"','"+frmPcs+"','"+toPcs+"');</script>", false);
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

                        DataTable dtDGR = new DataTable("GHA_QuickBooking_175");
                            dtDGR = (DataTable)Session["DgrCargo"];
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
            //LoginBL objBL = new LoginBL();
            //string SmartKargoInstance = string.Empty;
            //string ManifestPartner = string.Empty;
            //string FlightValidation = string.Empty;

            //if (Session["SKI"] == null || Convert.ToString(Session["SKI"]) == "")
            //{
            //    //SmartKargoInstance = objBL.GetMasterConfiguration("SmartKargoInstance");
            //    SmartKargoInstance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SmartKargoInstance");

            //    if (SmartKargoInstance != "")
            //        Session["SKI"] = SmartKargoInstance; // Smart Kargo Instance
            //    else
            //        Session["SKI"] = "AR";
            //}

            //if (CommonUtility.SmartKargoInstance == "" || CommonUtility.SmartKargoInstance == null)
            //{
            //    CommonUtility.SmartKargoInstance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SmartKargoInstance");
            //    if (CommonUtility.SmartKargoInstance == "")
            //        CommonUtility.SmartKargoInstance = "AR";

            //    FlightValidation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "FlightValidation");
            //    if (FlightValidation != "")
            //        CommonUtility.FlightValidation = Convert.ToBoolean(FlightValidation); // FlightRouteValidation
            //    else
            //        CommonUtility.FlightValidation = true;

            //    CommonUtility.ShipperMandatoryDuring = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateShipperDuring");
            //    CommonUtility.ConsigneeMandatoryDuring = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateConsigneeDuring");                
            //}
            
            //if (Session["FRV"] == null || Convert.ToString(Session["FRV"]) == "")
            //{
            //    FlightValidation = objBL.GetMasterConfiguration("FlightValidation");
            //    if (FlightValidation != "")
            //        Session["FRV"] = Convert.ToBoolean(FlightValidation); // FlightRouteValidation
            //    else
            //        Session["FRV"] = true;
            //}

            try
            {
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("handleCIMPMsg")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "handleCIMPMsg")))
                {
                    btnSendFFR.Visible = false;
                    btnSendFHL.Visible = false;
                    btnSenfwb.Visible = false;
                }
            }
            catch (Exception ex) { }

            //Get Shipper Mandatory During.
            //if (Session["ShipperMandatoryDuring"] == null || Session["ShipperMandatoryDuring"].ToString() == "")
            //{
            //    //Session["ShipperMandatoryDuring"] = objBL.GetMasterConfiguration("ValidateShipperDuring").ToString();                
            //    Session["ShipperMandatoryDuring"] = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateShipperDuring");                
            //}
            ////Get Consignee Mandatory During.
            //if (Session["ConsigneeMandatoryDuring"] == null || Session["ConsigneeMandatoryDuring"].ToString() == "")
            //{
            //    //Session["ConsigneeMandatoryDuring"] = objBL.GetMasterConfiguration("ValidateConsigneeDuring").ToString();                
            //    Session["ConsigneeMandatoryDuring"] = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateConsigneeDuring");                
            //}

            //objBL = null;
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
            DataSet dsDimensions = new DataSet("GHA_QuickBooking_96");
                dsDimensions = (DataSet)Session["dsDimesionAll"];
            string strULDNumbers = string.Empty;
            BookingBAL objBAL = new BookingBAL();
            string strResult = string.Empty;

            try
            {
                string FlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                DateTime FlightDt = DateTime.MinValue;

                if (((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY != "")
                    FlightDt = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateValue;
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
                    if(strULDNumbers.Length > 1)
                        strULDNumbers = strULDNumbers.Substring(0, strULDNumbers.Length - 1);

                    strResult = objBAL.ValidateULDFlow(strULDNumbers, FlightNo, FlightDt, Origin,txtAWBNo.Text,
                        txtAwbPrefix.Text);
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
            DataSet objDS = new DataSet("GHA_QuickBooking_97");
                objDS = objBAL.LoadAWBMasterData();
            // ------ Sumit 2014-10-30
            try
            {
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

                        try
                        {
                            string countrycode = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DefaultCountry");
                            ddlConCountry.SelectedValue = countrycode;
                            ddlShipCountry.SelectedValue = countrycode;
                        }
                        catch (Exception ex) { }
                    }

                    //Loading Currency Master codes
                    if (objDS.Tables.Count > 3 && objDS.Tables[3].Rows.Count > 0)
                    {
                        drpCurrency.Items.Clear();
                        drpCurrency.DataSource = objDS.Tables[3];
                        drpCurrency.DataTextField = "Code";
                        drpCurrency.DataValueField = "ID";
                        drpCurrency.DataBind();
                        //drpCurrency.SelectedIndex = drpCurrency.Items.IndexOf(drpCurrency.Items.FindByText("INR"));
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

                    //Loading Irregularity Master codes
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

                    //Loading Pay Modes.
                    if (objDS.Tables.Count > 7 && objDS.Tables[7].Rows.Count > 0)
                    {
                        Session["ConBooking_PayModeMaster"] = objDS.Tables[7].Copy();
                    }

                    objDS = null;
                }
            }
            catch (Exception ex) { }
        }

        public void AutoPopulateTemplate(bool editable, string TemplateID, string state)
        {
            DataSet dsResult = new DataSet("GHA_QuickBooking_98");
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
//                    TXTCustomerCode.Text = dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();
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

                    HidOrigin.Value = dsResult.Tables[0].Rows[0]["OriginCode"].ToString();
                    HidDestination.Value = dsResult.Tables[0].Rows[0]["DestinationCode"].ToString();

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

                    TXTAgentCode.Text = dsResult.Tables[0].Rows[0]["AgentCode"].ToString();
                    txtAgentName.Text = dsResult.Tables[0].Rows[0]["AgentName"].ToString();
                    txtRemarks.Text = dsResult.Tables[0].Rows[0]["Remarks"].ToString();
                    txtSpecialHandlingCode.Text = dsResult.Tables[0].Rows[0]["SHCCodes"].ToString();


                    txtAgentName.Text = "" + dsResult.Tables[0].Rows[0]["AgentName"].ToString();
                    txtHandling.Text = "" + dsResult.Tables[0].Rows[0]["HandlingInfo"].ToString();
                    //TXTCustomerCode.Text = "" + dsResult.Tables[0].Rows[0]["CustomerCode"].ToString();

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
                        DataSet dsMaterialDetails = new DataSet("GHA_QuickBooking_99");
                        // CommodityCode CodeDescription Pieces GrossWeight Dimensions VolumetricWeight ChargedWeight RowIndex
                        dsMaterialDetails.Tables.Clear();
                        dsMaterialDetails.Tables.Add(dsResult.Tables[1].Copy());
                                                
                        Session["dsMaterialDetails"] = dsMaterialDetails.Copy();
                    }
                    

                    if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[3].Rows.Count > 0)
                    {
                        //AWBRouteMaster
                        //FltOrigin FltDestination FltNumber FltDate Pcs Wt Status Accepted AcceptedPcs
                        DataSet dsRoutDetails = new DataSet("GHA_QuickBooking_100");
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
                        DataSet dtRates = new DataSet("GHA_QuickBooking_101");
                        dtRates.Tables.Clear();
                        dtRates.Tables.Add(dsResult.Tables[7].Copy());

                        drpCurrency.SelectedValue = dtRates.Tables[0].Rows[0]["CurrencyIndex"].ToString();

                        Session["dtRates"] = dtRates.Tables[0].Copy();
                    }                    

                    // OCDetails
                    DataSet dsDetails = new DataSet("GHA_QuickBooking_102");
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

                    txtExecutionDate1.DateFormatDDMMYYYY = "" + dtd;
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
                            //GrdRateDetails.DataSource = dsResult.Tables[10].Copy();
                            //GrdRateDetails.DataBind();
                            Session["FltRoute"] = dsResult.Tables[10].Copy();
                        }
                        else
                            Session["FltRoute"] = null;
                    }
                    catch (Exception ex) { }
                                        
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
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_SelLbl();</script>", false);
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
                int totPcs = int.Parse(txtPieces.Text.ToString());
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
            DataSet dsDim = new DataSet("GHA_QuickBooking_104");
                dsDim = (DataSet)Session["dsDimesionAll"];
            bool blnResult = true;
            StringBuilder StrDimensions = new StringBuilder();
            GHA_BALFlightCapacity objBAL = new GHA_BALFlightCapacity();

            try
            {
                string strFlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                DateTime dtFlightDt = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateValue;
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
                DateTime dtFlightDt = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateValue;

                string FltOrgin = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                string FltDest = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim();
                Decimal AWBWeight = Convert.ToDecimal(txtGrossWt.Text.Trim());

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
                if(String.IsNullOrEmpty(txtUOM.Text))
                    LoadCurrencyWithUOM();
            }
            catch (Exception objEx)
            {
            }
            try
            {
                if (txtAWBNo.Text.Trim() != "")
                {
                    Int64 AWBNo = Convert.ToInt64(txtAWBNo.Text.Trim().Substring(0, 7));
                    int LstDigit = Convert.ToInt32(txtAWBNo.Text.Trim().Substring(7, 1));

                    if ((AWBNo % 7) != LstDigit)
                    {
                        HidBookingError.Value = "Please enter valid AWB.";
                        lblStatus.Text = "Please enter valid AWB.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }
            }
            catch
            {
                HidBookingError.Value = "Please enter valid AWB.";
                lblStatus.Text = "Please enter valid AWB.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                txtAWBNo.Focus();
                return;
            }

            if (txtAWBNo.Text.Trim() != "" && HidAWBNumber.Value == "")
            {
                if (CheckManualAWB("S") == false)
                    return;
            }

            int rowindex = 0;

            if (txtChargeableWt.Text.Trim() == "")
                txtChargeableWt.Text = txtGrossWt.Text.Trim();

            string RouteDestination = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim();
            string RouteFlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).Text.Trim();

            if (RouteDestination == "" && RouteFlightNo == "")
                LoadFlightDetails();

            if (grdRouting.Rows.Count == 1)
            {
                if (((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text.Trim() == "")
                {
                    if (txtPieces.Text.Trim() != "")
                    {
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text = txtPieces.Text;
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text = txtGrossWt.Text;
                        if(txtChargeableWt.Text.Trim()!="")
                        ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = txtChargeableWt.Text;
                        else
                            ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = txtGrossWt.Text;
                        HidProcessFlag.Value = "1";
                    }                    
                }

                if (Convert.ToDecimal(((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text.Trim()) != Convert.ToDecimal(txtPieces.Text.Trim())
                    ||Convert.ToDecimal( ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text.Trim()) != Convert.ToDecimal( txtGrossWt.Text.Trim())
                    ||Convert.ToDecimal( ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text.Trim()) != Convert.ToDecimal( txtChargeableWt.Text.Trim()))
                {

                    ((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text = txtPieces.Text;
                    ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text = txtGrossWt.Text;
                    if(txtGrossWt.Text.Trim() != "")
                    {
                        if(txtChargeableWt.Text.Trim()=="")
                            txtChargeableWt.Text = "0";
                        if (Convert.ToDecimal(txtGrossWt.Text.Trim()) > Convert.ToDecimal(txtChargeableWt.Text.Trim()))
                            txtChargeableWt.Text = txtGrossWt.Text;
                    }

                    ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = txtChargeableWt.Text;
                    HidProcessFlag.Value = "1";
                }
            }

            try
            {
                string AgentCode = Convert.ToString(Session["ACode"]);

                if (AgentCode.Trim() != "" && Convert.ToString(Session["AWBStatus"]).Trim() != "B")
                {
                    HidBookingError.Value = "AWB can not be modified at this stage.";
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
                            HidBookingError.Value = "AWB number is finalized in billing. Modifications can not be done.";
                            lblStatus.Text = "AWB number is finalized in billing. Modifications can not be done.";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                

                //-----------------------------------------------------------------------------------------------------------
                //Swapnil
                if (!ValidateData())
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }
                //-------------------------------------------------------------------------------------------------------------

                if (txtAWBNo.Text.Trim() != "" && HidAWBNumber.Value == "")
                    HidAWBNumber.Value = txtAWBNo.Text.Trim();

                if (checkbeforereopen)
                {
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
                                strFligtDet.Append(((DateControl)grdRouting.Rows[ICount].FindControl("txtFdate")).DateFormatMMDDYYYY);
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
                            HidBookingError.Value = strResult;
                            lblStatus.Text = strResult;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                    }
                    #endregion Check Manifest
                }

                #region DGR Cargo

                //21-8-2012 Swapnil
                //-----------------------------------------------------------------------------
                bool blnres;
                //for (int cnt = 0; cnt < grdMaterialDetails.Rows.Count; cnt++)
                //{
                    string CommCode = txtCommodityCode.Text.ToString();
                    if (CommCode == "")
                    {
                        HidBookingError.Value = "Please Select Commodity Code";
                        lblStatus.Text = "Please Select Commodity Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }


                    if (CommCode.ToUpper().Trim() == "DGR" || txtSpecialHandlingCode.Text.ToUpper().Trim()=="DGR" || objBLL.CheckDGRCargo(CommCode,txtSpecialHandlingCode.Text.Trim()))
                    {
                        if (Session["DgrCargo"] == null || ((DataTable)Session["DgrCargo"]).Rows.Count < 1)
                        {
                            SQLServer da = new SQLServer(Global.GetConnectionString());
                            DataSet ds = new DataSet("GHA_QuickBooking_105");
                                ds = da.SelectRecords("SpGetDGRCargo", "AWBNumber", HidAWBNumber.Value, SqlDbType.VarChar);

                            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                            {
                                HidBookingError.Value = "Please enter DGR Details";
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

                //}

                #endregion DGR Cargo

                //  txtAWBNo.Text = "23456788";

                //Calculate totals...
                intTotalPcs = 0;
                floatGrossWt = 0;
                floatVolWt = 0;
                floatChargedWt = 0;
                //for (int cnt = 0; cnt < grdMaterialDetails.Rows.Count; cnt++)
                //{
                    //Total pieces                   
                    //TextBox tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("TXTPcs"));
                    intTotalPcs = int.Parse(txtPieces.Text.Trim());
                    

                    //Total gross wt                    
                    //tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("txtCommGrossWt"));
                    floatGrossWt = (float)Convert.ToDecimal(txtGrossWt.Text);
                    //tempTextBox = null;

                    //Volumetric wt                    
                    //tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("txtCommVolWt"));
                    floatVolWt = (float)Convert.ToDecimal(txtVolume.Text.Trim() == "" ? "0" : txtVolume.Text.Trim());
                    //tempTextBox = null;

                    //Charged wt                    
                    //tempTextBox = (TextBox)(grdMaterialDetails.Rows[cnt].FindControl("txtCommChargedWt"));

                    if (txtChargeableWt.Text.Trim() == "")
                        txtChargeableWt.Text = txtGrossWt.Text;

                    floatChargedWt = (float)Convert.ToDecimal(txtChargeableWt.Text);

                    //tempTextBox.Dispose();

                //}

                if (CHKExportShipment.Checked == true && txtHandling.Text.Trim() == "")
                {
                    HidBookingError.Value = "Please enter Invoice details in Handling info.";
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Please enter Invoice details in Handling info.";
                    return;
                }

                //Check Accepted pieces with Actual

                //for (int IntCount = 0; IntCount < grdRouting.Rows.Count; IntCount++)
                //{
                    int BkPcs = 0;
                    decimal BkWt = 0;

                    int AccPcs = 0;
                    decimal AccWt = 0;

                    try
                    {
                        if (txtAccpPieces.Text.Trim() == "")
                            AccPcs = 0;
                        else
                            AccPcs = Convert.ToInt32(txtAccpPieces.Text.Trim());
                    }
                    catch
                    {
                        AccPcs = 0;
                    }

                    try
                    {
                        if (txtAccpWeight.Text.Trim() == "")
                            AccWt = 0;
                        else
                            AccWt = Convert.ToDecimal(txtAccpWeight.Text.Trim());
                    }
                    catch
                    {
                        AccWt = 0;
                    }

                    if (AccPcs > 0)
                    {
                        BkPcs = Convert.ToInt32(txtPieces.Text.Trim());
                        BkWt = Convert.ToDecimal(txtGrossWt.Text.Trim());

                        if (BkPcs == AccPcs && BkWt != AccWt)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            HidBookingError.Value = "Please enter valid Weight.";
                            LBLRouteStatus.Text = "Please enter valid Weight.";
                            txtAccpWeight.Focus();
                            return;
                        }

                        if (BkWt == AccWt && BkPcs != AccPcs)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            HidBookingError.Value = "Please enter valid Pieces.";
                            LBLRouteStatus.Text = "Please enter valid Pieces.";
                            txtAccpPieces.Focus();
                            return;
                        }

                        if (AccPcs == 0 && AccWt != 0)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            HidBookingError.Value = "Please enter valid Pieces.";
                            LBLRouteStatus.Text = "Please enter valid Pieces.";
                            txtAccpPieces.Focus();
                            return;
                        }

                        if (AccWt == 0 && AccPcs != 0)
                        {
                            LBLRouteStatus.ForeColor = Color.Red;
                            HidBookingError.Value = "Please enter valid Weight.";
                            LBLRouteStatus.Text = "Please enter valid Weight.";
                            txtAccpWeight.Focus();
                            return;
                        }
                    }
                //}

                //End

                float fltMtGrossWt = floatGrossWt;
                float fltMtChrgblWt = floatChargedWt;

                float fltFltGrossWt = 0;
                float fltFltChrgblWt = 0;

                string strClientFlag = CommonUtility.EmbargoFunctionality;
                                
                #region "Embargo Verification"

                    //Check for Embargo cargo

                    string strCommodity = string.Empty;
                    DateTime dtExecutionDt = txtExecutionDate1.DateValue; // DateTime.ParseExact(txtExecutionDate.Text, "dd/MM/yyyy", null);
                    string strOrigin = string.Empty;
                    string strDestination = string.Empty;
                    string strPaymentType = string.Empty;
                    string strFlightNo = string.Empty;
                    // BookingBAL objBooking = new BookingBAL();
                    string strcarrier = string.Empty;
                    DataTable dtFlightDetails = GetFlightDataSet();
                    dtFlightDetails.Columns["Pieces"].DataType = typeof(int);
                    dtFlightDetails.Columns["Gwt"].DataType = typeof(decimal);
                    dtFlightDetails.Columns["CWt"].DataType = typeof(decimal);
                                    
                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        DataRow DR = dtFlightDetails.NewRow();
                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim().ToUpper();
                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text.Trim().ToUpper();

                        strCommodity = txtCommodityCode.Text;
                        strOrigin = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text;
                        strDestination = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltDest"))).Text;
                        strPaymentType = ddlPaymentMode.SelectedValue;
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

                        DR["Origin"] = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim().ToUpper();
                        DR["Destination"] = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text.Trim().ToUpper();
                        DR["Pieces"] = Convert.ToInt16(((TextBox)grdRouting.Rows[intCount].FindControl("txtPcs")).Text.Trim());
                        DR["Gwt"] = Convert.ToDecimal(((TextBox)grdRouting.Rows[intCount].FindControl("txtWt")).Text.Trim().ToUpper());
                        DR["CWt"] = Convert.ToDecimal(((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text.Trim().ToUpper());
                        dtFlightDetails.Rows.Add(DR);

                        if (strClientFlag != "" && Convert.ToBoolean(strClientFlag) == true)
                        {
                            DataSet Objds = new DataSet("GHA_QuickBooking_106");
                            try
                            {
                                Objds = objBLL.VerifyEmbargoCargo(dtExecutionDt, strOrigin, strDestination, strCommodity, strPaymentType, strFlightNo);

                                if (Objds != null && Objds.Tables.Count > 0 && Objds.Tables[0].Rows.Count > 0)
                                {
                                    if ((bool)Objds.Tables[0].Rows[0]["IsInvalid"] == true)
                                    {
                                        if ((string)Objds.Tables[0].Rows[0]["ErrorType"] == "Error")
                                        {
                                            HidBookingError.Value = Convert.ToString(Objds.Tables[0].Rows[0]["ErrorDesc"]);
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
                    }

                #endregion
                
                if (fltMtGrossWt != fltFltGrossWt || fltMtChrgblWt != fltFltChrgblWt)
                {
                    lblStatus.ForeColor = Color.Red;
                    HidBookingError.Value = "Material weights & Flight weights are not matching.";
                    lblStatus.Text = "Material weights & Flight weights are not matching.";
                    return;
                }

                #region "Validate Flight Route"

                string strRouteValidation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RouteValidation");

                if (strRouteValidation != "" && Convert.ToBoolean(strRouteValidation))
                {

                    string strFltOrigin = string.Empty;
                    string strFltDest = string.Empty;
                    int OriginPcs = 0;
                    decimal OriginGWt = 0;
                    decimal OriginCWt = 0;
                    int DestPcs = 0;
                    decimal DestGWt = 0;
                    decimal DestCWt = 0;

                    if (dtFlightDetails != null && dtFlightDetails.Rows.Count > 0)
                    {
                        for (int intCount = 0; intCount < dtFlightDetails.Rows.Count; intCount++)
                        {
                            if (dtFlightDetails.Rows[intCount]["Destination"].ToString() != ddlDest.SelectedItem.Text.ToUpper())
                            {
                                strFltDest = dtFlightDetails.Rows[intCount]["Destination"].ToString();

                                DestPcs = Convert.ToInt16(dtFlightDetails.Compute("Sum ( Pieces ) ", "Destination = '" + strFltDest + "'") == System.DBNull.Value ? "0" : dtFlightDetails.Compute("Sum ( Pieces ) ", "Destination = '" + strFltDest + "'"));
                                DestGWt = Convert.ToDecimal(dtFlightDetails.Compute("Sum ( Gwt ) ", "Destination = '" + strFltDest + "'") == System.DBNull.Value ? "0" : dtFlightDetails.Compute("Sum ( Gwt ) ", "Destination = '" + strFltDest + "'"));
                                DestCWt = Convert.ToDecimal(dtFlightDetails.Compute("Sum ( Cwt ) ", "Destination = '" + strFltDest + "'") == System.DBNull.Value ? "0" : dtFlightDetails.Compute("Sum ( Cwt ) ", "Destination = '" + strFltDest + "'"));

                                OriginPcs = Convert.ToInt16(dtFlightDetails.Compute("Sum ( Pieces ) ", "Origin = '" + strFltDest + "'") == System.DBNull.Value ? "0" : dtFlightDetails.Compute("Sum ( Pieces ) ", "Origin = '" + strFltDest + "'"));
                                OriginGWt = Convert.ToDecimal(dtFlightDetails.Compute("Sum ( Gwt ) ", "Origin = '" + strFltDest + "'") == System.DBNull.Value ? "0" : dtFlightDetails.Compute("Sum ( Gwt ) ", "Origin = '" + strFltDest + "'"));
                                OriginCWt = Convert.ToDecimal(dtFlightDetails.Compute("Sum ( Cwt ) ", "Origin = '" + strFltDest + "'") == System.DBNull.Value ? "0" : dtFlightDetails.Compute("Sum ( Cwt ) ", "Origin = '" + strFltDest + "'"));

                                if (DestPcs != OriginPcs && DestGWt != OriginGWt && DestCWt != OriginCWt)
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    HidBookingError.Value = "Please check the route details.";
                                    lblStatus.Text = "Please check the route details.";
                                    dtFlightDetails = null;
                                    return;
                                }
                            }
                        }
                    }                    
                }

                dtFlightDetails = null;

                #endregion

                //System.Diagnostics.Debug.WriteLine("Before Rate Process - " + DateTime.Now.ToString());

                if (ProcessRates() == false)
                    return;

                //System.Diagnostics.Debug.WriteLine("After Rate Process - " + DateTime.Now.ToString());

                ////Check for Agent account balance incase of "PP"
                string PaymentType = ddlPaymentMode.SelectedValue;
                decimal AWBAmount = Convert.ToDecimal(txtTotalAmount.Text);
                string TranAccount = string.Empty, Remarks = string.Empty, TranType = string.Empty;
                decimal BGAmount = 0, AWBPrevAmt = 0, BankGAmt = 0, ThrValue = 0;
                bool ValidateBG = false;

                //Commented by Vijay to process when AWB is voided
                //if (PaymentType == "PX" && (HidAcceptance.Value == "1"|| HidAcceptance.Value == "Accept"))
                if (PaymentType == "PX" && (HidAcceptance.Value == "1" || HidAcceptance.Value == "Accept" || ddlServiceclass.SelectedIndex == 0))
                {
                    BookingBAL objBal = new BookingBAL();
                    string AgentCurrency = objBal.GetAgentCurrency(TXTAgentCode.Text.Trim());
                    decimal CurrencyConversion = 1;

                    if (AgentCurrency != "" && AgentCurrency.ToUpper() != drpCurrency.SelectedItem.Text.ToUpper())
                    {
                        //CurrencyConversion = objBal.CurrencyConversion("IATA", AgentCurrency, drpCurrency.SelectedItem.Text, DateTime.ParseExact(txtExecutionDate.Text.Trim(), "dd/MM/yyyy", null));
                        CurrencyConversion = objBal.CurrencyConversion("IATA", drpCurrency.SelectedItem.Text, AgentCurrency, txtExecutionDate1.DateValue);

                        objBal = null;

                        if (CurrencyConversion == 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            HidBookingError.Value = "IATA Rate conversion might be expired for " + drpCurrency.SelectedItem.Text + " - " + AgentCurrency;
                            lblStatus.Text = "IATA Rate conversion might be expired for " + drpCurrency.SelectedItem.Text + " - " + AgentCurrency;
                            return;
                        }

                        if (CurrencyConversion != 1)
                            //AWBAmount = AWBAmount * CurrencyConversion;
                            AWBAmount = AWBAmount / CurrencyConversion;
                    }

                    bool blnAllow = CheckforAgentsBalance(TXTAgentCode.Text.Trim(), AWBAmount, HidAWBNumber.Value,
                        ref TranAccount, ref BGAmount, ref AWBPrevAmt, ref BankGAmt, ref ThrValue, ref ValidateBG, txtAwbPrefix.Text.Trim(), CurrencyConversion);
                    if (blnAllow != true && ValidateBG == true)
                    {
                        lblStatus.ForeColor = Color.Red;
                        HidBookingError.Value = "Agent is not having sufficient balance to create AWB.";
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

                if (GenerateAWBNumber() == false)
                    return;

                string Errmessage = "";
                if (HidAWBNumber.Value.ToString().Trim() != "")
                {
                    if (Session["AWBStatus"] == null || Session["AWBStatus"].ToString() == "")
                    {
                        string AWBStatus = objBLL.GetAWBStatus(txtAwbPrefix.Text.Trim(), HidAWBNumber.Value.ToString().Trim());
                        Session["AWBStatus"] = AWBStatus;
                    }

                    if (objBLL.SetAWBStatus(HidAWBNumber.Value.ToString().Trim(), Session["AWBStatus"].ToString(), ref Errmessage,
                        dtCurrentDate.ToString("dd/MM/yyyy"), strUserName, dtCurrentDate,
                        txtAwbPrefix.Text, checkbeforereopen, ddlPaymentMode.SelectedValue) == false)
                    {
                        if (Errmessage.Trim().Length > 0)
                        {
                            lblStatus.Text = Errmessage;
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }

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
                awbInfo.SetValue(txtExecutionDate1.DateFormatDDMMYYYY, i);
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
                if (txtShipmentDate1.Text == "")
                    awbInfo.SetValue("", i);
                else
                    awbInfo.SetValue(txtShipmentDate1.DateFormatDDMMYYYY + " " + ddlShipmentTime.SelectedValue, i);
                
                //Added by Vishal - 04 MAY 2014 ******************** 
                i++;
                awbInfo.SetValue(txtUOM.Text, i);
                //******************** Added by Vishal - 04 MAY 2014

                //Added by Poorna - 06 June 2014 ******************** 
                i++;
                awbInfo.SetValue(txtPackingInfo.Text.Trim(), i);
                //Added by Poorna - 06 June 2014 ********************
                i++;
                awbInfo.SetValue(txtSCI.Text.Trim(), i);
                #endregion Prepare Parameters

                string ErrorMessage = string.Empty;
                int iteration = 0;

                while (iteration < 2)
                {
                    //Update database.
                    if (objBLL.SaveAWBSummary_GHA(awbInfo, ref ErrorMessage) < 0)
                    {
                        HidBookingError.Value = "AWB Save failed. Please try again...";
                        lblStatus.Text = "AWB Save failed. Please try again...";

                        if (ErrorMessage.Length > 0 && ErrorMessage.ToUpper().IndexOf("DUPLICATE AWB") > 0)
                        {
                            HidBookingError.Value = "Unable to save at this time. Please try again.";
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
                        HidBookingError.Value = "";
                    }
                }

                if (HidBookingError.Value != "")
                {
                    lblStatus.Text = "Unable to save at this time. Please try again.";
                    awbInfo = null;
                    return;
                }
                else
                    awbInfo = null;

                #endregion Save AWB Summary

                //Save Remarks
                if (txtComment.Text.Trim() != "")
                    BookingRemarks();
                //End

                //Delete AWB Material & Route Details.
                //objBLL.DeleteAWBDetails(txtAWBNo.Text);
                objBLL.DeleteAWBDetails(HidAWBNumber.Value, txtAwbPrefix.Text);

                //Commented by Vijay to process void AWBs Credit used
                //if (PaymentType == "PX" && AWBAmount != 0 && (HidAcceptance.Value == "1" || HidAcceptance.Value == "Accept"))
                if (PaymentType == "PX" && AWBAmount != 0 && (HidAcceptance.Value == "1" || HidAcceptance.Value == "Accept" || ddlServiceclass.SelectedIndex == 0))
                {
                    BALAgentCredit objCredit = new BALAgentCredit();
                    objCredit.SaveTransacation("", TranAccount, "", TranType, 0, 0, double.Parse(AWBAmount.ToString()), "AWB Transaction,AWBNo:" + HidAWBNumber.Value, Session["UserName"].ToString(), HidAWBNumber.Value, "");
                }

                #region Save AWB Material
                //Save material information.
                try
                {

                    //Add material info...
                    //for (int lstIndex = 0; lstIndex < grdMaterialDetails.Rows.Count; lstIndex++)
                    //{
                        i = 0;
                        awbInfo = null;
                        awbInfo = new object[15];
                        //0
                        //awbInfo.SetValue(txtAWBNo.Text, i);
                        awbInfo.SetValue(HidAWBNumber.Value, i);
                        i++;

                        //1
                        TextBox tempDropDown = (TextBox)(txtCommodityCode);
                        awbInfo.SetValue(tempDropDown.Text, i);
                        i++;

                        //2
                        TextBox tempTextBox = (TextBox)(txtPieces);
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        //3
                        tempTextBox = (TextBox)(txtGrossWt);
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        //4
                        awbInfo.SetValue(0, i);
                        i++;

                        //5
                        tempTextBox = (TextBox)(txtVolume);
                        awbInfo.SetValue(tempTextBox.Text.Trim() == "" ? "0" : tempTextBox.Text.Trim(), i);
                        i++;

                        //6
                        tempTextBox = (TextBox)(txtChargeableWt);
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        //7                        
                        awbInfo.SetValue(Session["UserName"], i);
                        i++;

                        //8
                        awbInfo.SetValue(dtCurrentDate, i);
                        i++;

                        //9
                        tempTextBox = (TextBox)(txtCommodityName);
                        awbInfo.SetValue(tempTextBox.Text, i);
                        i++;

                        //10
                        //TextBox txtAccountInfo = (TextBox)(grdMaterialDetails.Rows[lstIndex].FindControl("txtAccountInfo"));
                        awbInfo.SetValue("", i);
                        i++;

                        //11
                        //DropDownList ddlShipmentType = (DropDownList)(ddlShipmentType);
                        awbInfo.SetValue(ddlShipmentType.Text.Trim(), i);
                        i++;

                        //12
                        TextBox txtShipmentRemarks = (TextBox)(txtRemarks);
                        awbInfo.SetValue(txtShipmentRemarks.Text.Trim(), i);
                        i++;
                        //13 -AWB Prefix
                        awbInfo.SetValue(txtAwbPrefix.Text, i);
                        i++;

                        //TextBox txtShipmentPriority = (TextBox)(txtShipmentPriority);
                        awbInfo.SetValue(txtShipmentPriority.Text.Trim(), i);

                        //Call SP to update database.
                        if (objBLL.SaveAWBMaterial_GHA(awbInfo) < 0)
                        {
                            HidBookingError.Value = "Error updating AWB Information. Please try again...";
                            lblStatus.Text = "Error updating AWB Information. Please try again...";
                            awbInfo = null;
                            return;
                        }
                    //}
                }
                catch (Exception ex)
                {
                    HidBookingError.Value = "Error updating AWB Information: " + ex.Message;
                    lblStatus.Text = "Error updating AWB Information: " + ex.Message;
                    return;
                }
                #endregion Save AWB Material

                #region Save AWB Routing
                //Save routing information.
                try
                {
                    int totalAccPcs = Convert.ToInt32(txtAccpPieces.Text.Trim() == "" ? "0" : txtAccpPieces.Text.Trim());
                    decimal totalAccWt = Convert.ToDecimal(txtAccpWeight.Text.Trim() == "" ? "0" : txtAccpWeight.Text.Trim());
                    int totalAccPcsSoFar = 0;
                    decimal totalAccWtSoFar = 0;
                    for (int lstIndex = 0; lstIndex < grdRouting.Rows.Count; lstIndex++)
                    {
                        
                        int intAccPieces = 0;
                        string strIsAccp = string.Empty;
                        decimal dcAccepWt = 0;

                        if(totalAccPcs > 0 && 
                            !Int32.TryParse(((TextBox)grdRouting.Rows[lstIndex].FindControl("txtPcs")).Text,out intAccPieces))
                        {
                            intAccPieces = 0;
                        }

                        if(totalAccWt > 0 && 
                            !decimal.TryParse(((TextBox)grdRouting.Rows[lstIndex].FindControl("txtWt")).Text,
                            out dcAccepWt))
                        {
                            dcAccepWt = 0;
                        }
                        strIsAccp = (totalAccPcs <= 0) ? "N" : "Y";

                        if (ddlOrg.Text.Trim().ToUpper() == ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFltOrig")).Text.Trim().ToUpper())
                        {
                            intAcceptedPcsforSummary = intAcceptedPcsforSummary + intAccPieces;
                            dcAcceptedWtforSummary = dcAcceptedWtforSummary + dcAccepWt;
                            //Validate if complete Pieces to be accepted for a leg.
                            if (totalAccPcs > 0 && intAccPieces + totalAccPcsSoFar > totalAccPcs)
                            {
                                intAccPieces = totalAccPcs - totalAccPcsSoFar;
                            }
                            totalAccPcsSoFar = totalAccPcsSoFar + intAccPieces;
                            //Validate if complete Weight to be accepted for a leg.
                            if (totalAccWt > 0 && dcAccepWt + totalAccWtSoFar > totalAccWt)
                            {
                                dcAccepWt = totalAccWt - totalAccWtSoFar;
                            }
                            totalAccWtSoFar = totalAccWtSoFar + dcAccepWt;
                        }

                        if (totalAccPcs > 0 && strIsAccp == "N")
                            strIsAccp = "Y";

                        object[] values = {       
                                              //txtAWBNo.Text.Trim(),
                                              HidAWBNumber.Value.Trim(),
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFltOrig")).Text.ToUpper(),
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFltDest")).Text.ToUpper(),
                                              
                                              (((DropDownList)(grdRouting.Rows[lstIndex].FindControl("ddlPartner"))).Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase)? 
                                                    ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtFlightID")).Text:
                                                    ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlFltNum")).SelectedItem.Text),
                                              
                                              ((DateControl)grdRouting.Rows[lstIndex].FindControl("txtFdate")).DateFormatDDMMYYYY,
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtPcs")).Text,
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtWt")).Text,
                                              ddlAWBStatus.SelectedItem.Value,
                                              strIsAccp,
                                              intAccPieces, //Accep Pieces
                                              dcAccepWt, //Accep Wt
                                              Convert.ToString(Session["UserName"]),
                                              dtCurrentDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                              ((HiddenField)grdRouting.Rows[lstIndex].FindControl("HidScheduleID")).Value.Trim()==""?0:int.Parse(((HiddenField)grdRouting.Rows[lstIndex].FindControl("HidScheduleID")).Value.Trim()),
                                              ((TextBox)grdRouting.Rows[lstIndex].FindControl("txtChrgWt")).Text,
                                              ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlPartner")).Text.ToString(),
                                              ((DropDownList)grdRouting.Rows[lstIndex].FindControl("ddlPartnerType")).Text.ToString(),
                                              txtAwbPrefix.Text, txtCommodityCode.Text.Trim() + " - " + txtCommodityName.Text.Trim()
                                          };


                        if (objBLL.SaveAWBRoute(values) < 0)
                        {
                            HidBookingError.Value = "Error updating Route information. Please try again...";
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
                    HidBookingError.Value = "Error updating AWB information: " + ex.Message;
                    lblStatus.Text = "Error updating AWB information: " + ex.Message;
                    return;
                }
                #endregion Save AWB Routing

                #region AWB Dimensions
                
                int AWBPcs = int.Parse(txtPieces.Text);
                decimal AWBWt = Convert.ToDecimal(txtChargeableWt.Text);

                GenerateAWBDimensions(HidAWBNumber.Value, AWBPcs, (DataSet)Session["dsDimesionAll"], AWBWt, true, txtAwbPrefix.Text);
                                 
                #endregion

                #region AWB Rates

                //if (Session["QK_Rates"] != null)
                //{
                    //for (int k = 0; k < GRDRates.Rows.Count; k++)
                    //{
                    CommCode = txtCommodityCode.Text;
                    strCommodityforSummary = CommCode;
                    string PayMode = ddlPaymentMode.SelectedValue;

                    int Pcs = int.Parse(txtPieces.Text);

                    decimal Wt = Convert.ToDecimal(txtGrossWt.Text);
                    decimal FrIATA = Convert.ToDecimal(txtFreightIATA.Text);
                    decimal FrMKT = Convert.ToDecimal(txtFreightMKT.Text);
                    decimal ValCharge = 0;
                    // Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTValCharg")).Text);
                    decimal OcDueCar = Convert.ToDecimal(txtOCDueCar.Text);
                    decimal OcDueAgent = Convert.ToDecimal(txtOCDueAgent.Text);
                    decimal SpotRate = Convert.ToDecimal(txtSpotRate.Text.Length<1?"0":txtSpotRate.Text);
                    decimal DynRate = 0;
                    // Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTDynRate")).Text);
                    decimal ServiceTax = Convert.ToDecimal(txtServiceTax.Text);
                    decimal Total = Convert.ToDecimal(txtTotalAmount.Text);
                    decimal RatePerKg = Convert.ToDecimal(txtRatePerKG.Text);
                    string RateCluse = txtRateClass.Text.Trim();
                    string currency = txtCurrency.Text;

                    if (txtSpotRate.Text.Trim() != "")
                        SpotRate = Convert.ToDecimal(txtSpotRate.Text.Trim());

                    DataSet DSRates = new DataSet("GHA_QuickBooking_107");
                        DSRates = (DataSet)Session["QK_Rates"];

                    //Taxex

                    decimal IATATax = 0;
                    decimal MKTTax = 0;
                    decimal OATax = 0;
                    decimal OCTax = 0;
                    decimal SpotTax = 0;
                    decimal CommTax = 0;
                    decimal DiscTax = 0;
                    decimal Commission = 0;
                    decimal Discount = 0;
                    decimal CommPercent = 0;
                    string SpotStatus = txtSpotStatus.Text;
                    string IATARateID = "0";
                    string MKTRateID = "0";
                    if (DSRates != null && DSRates.Tables.Count > 0 && DSRates.Tables[0].Rows.Count > 0)
                    {
                        IATARateID = Convert.ToString(DSRates.Tables[0].Rows[0]["RateLineID"]);
                        MKTRateID = Convert.ToString(DSRates.Tables[0].Rows[1]["RateLineID"]);
                    }

                    if (DSRates != null && DSRates.Tables.Count > 3 && DSRates.Tables[3].Rows.Count > 0)
                    {
                        IATATax = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["IATATax"]);
                        MKTTax = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["MKTTax"]);
                        OATax = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["OATax"]);
                        OCTax = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["OCTax"]);
                        SpotTax = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["SpotTax"]);
                        CommTax = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["CommTax"]);
                        DiscTax = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["DiscTax"]);
                        Commission = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["Commission"]);
                        Discount = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["Discount"]);
                        CommPercent = Convert.ToDecimal(DSRates.Tables[3].Rows[0]["CommPercent"]);
                        SpotStatus = Convert.ToString(DSRates.Tables[3].Rows[0]["SpotStatus"]);
                    }

                    decimal SpotFreight = 0;
                    try
                    {
                        SpotFreight = Convert.ToDecimal(txtSpotFreight.Text.Length < 1 ? "0" : txtSpotFreight.Text);
                        SpotTax = Convert.ToDecimal(txtSpotTax.Text.Length < 1 ? "0" : txtSpotTax.Text);
                    }
                    catch (Exception ex) { SpotFreight = 0; SpotTax = 0; }    
               
                    bool AllInRate = CHKAllIn.Checked;
                    object[] values1 = { HidAWBNumber.Value.Trim(), CommCode, PayMode, Pcs, Wt, FrIATA, FrMKT, ValCharge, OcDueCar, OcDueAgent, 
                                           SpotRate, DynRate, ServiceTax, Total, RatePerKg, RateCluse, currency, txtAwbPrefix.Text,SpotFreight,IATATax,MKTTax,OATax,OCTax,SpotTax,CommTax,
                                      DiscTax,Commission,Discount,CommPercent,SpotStatus,AllInRate,IATARateID,MKTRateID };

                    if (!objBLL.SaveAWBRates(values1))
                    {
                        HidBookingError.Value = "Error updating Rate information. Please try again...";
                        lblStatus.Text = "Error updating Rate information. Please try again...";
                        return;
                    }

                    //}


                    DataSet dsDetails = new DataSet("GHA_QuickBooking_108");
                        dsDetails = (DataSet)Session["OCDetails"];

                    #region Validation for OtherCharges breakup and Total
                    //Validation for OtherCharges breakup and Total by Vijay
                    decimal FrtIATATotal, FrtMKTTotal, OCDCTotal, ODCATotal;
                    FrtIATATotal = 0; FrtMKTTotal = 0; OCDCTotal = 0; ODCATotal = 0;
                    decimal txtIATA, OCDC, txtOcDueAgent;
                    txtIATA = 0; OCDC = 0; txtOcDueAgent = 0;
                    //if (GRDRates.Rows.Count > 0)
                    //{
                    txtIATA = Convert.ToDecimal(txtFreightIATA.Text);
                    OcDueCar = Convert.ToDecimal(txtOCDueCar.Text);
                    txtOcDueAgent = Convert.ToDecimal(txtOCDueAgent.Text);
                    //}

                    if (dsDetails != null && dsDetails.Tables.Count > 0 && dsDetails.Tables[0].Rows.Count > 0)
                    {
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
                    }
                    //if (Math.Round(txtIATA, 0) != Math.Round(FrtIATATotal, 0) || Math.Round(txtOcDueCar, 0) != Math.Round(OCDCTotal, 0) || Math.Round(txtOcDueAgent, 0) != Math.Round(ODCATotal, 0))
                    if (Math.Round(OcDueCar, 0) != Math.Round(OCDCTotal, 0) || Math.Round(txtOcDueAgent, 0) != Math.Round(ODCATotal, 0))
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
                            if (row["Discount"].ToString() == "")
                                row["Discount"] = "0";

                            //string Commo = row["Commission"].ToString();
                            if (row["Commission"].ToString() == "")
                                row["Commission"] = "0";
                            if (row["SerialNumber"].ToString() == "")
                                row["SerialNumber"] = "0";
                            
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
                            if (!objBLL.SaveAWBRatesDetails_QB(txtAwbPrefix.Text.Trim(), HidAWBNumber.Value.Trim(), txtCommodityCode.Text.Trim(), strOtherCharges.ToString()))
                            {
                                HidBookingError.Value = "Error updating Rate information. Please try again...";
                                lblStatus.Text = "Error updating Rate information. Please try again...";
                                return;
                            }
                            strOtherCharges = null;
                        }
                    }
                //}
                #endregion

                #region AWB Shipper/Consignee

                if (!objBLL.SaveAWBShipperConsignee(new object[] { HidAWBNumber.Value ,TXTShipper.Text.Trim(),TXTShipAddress.Text.Trim(),ddlShipCountry.Text,TXTShipTelephone.Text.Trim(),
                                                                   TXTConsignee.Text.Trim(),TXTConAddress.Text.Trim(),ddlConCountry.Text,TXTConTelephone.Text.Trim(),
                                                                   TXTShipperAdd2.Text.Trim(),TXTShipperCity.Text.Trim(),TXTShipperState.Text.Trim(),TXTShipPinCode.Text.Trim(),//Shipper extra data
                                                                   TXTConsigAdd2.Text.Trim(),TXTConsigCity.Text.Trim(),TXTConsigState.Text.Trim(),TXTConsigPinCode.Text.Trim(),
                                                                   txtShipperCode.Text.Trim(), txtConsigneeCode.Text.Trim(),TXTShipperEmail.Text.Trim(),TXTConsigEmail.Text.Trim(),txtAwbPrefix.Text,
                                                                   txtShipperCCSF.Text.Trim(), ""
                }))//Consig Extra Data
                {
                    HidBookingError.Value = "Save failed (Shipper/Consignee)";
                    lblStatus.Text = "Save failed (Shipper/Consignee)";
                    return;
                }


                #endregion

                #region Save DGR Cargo

                //21-8-2012 Swapnil
                //-----------------------------------------------------------------------------

                if (Session["DgrCargo"] != null && ((DataTable)Session["DgrCargo"]).Rows.Count > 0)
                {
                    DataTable dt = new DataTable("GHA_QuickBooking_176");
                    dt = (DataTable)Session["DgrCargo"];

                    string AWBNumber, UNID, Weight, UpdatedBy, UpdatedOn,PG,ERGCode,Desc;
                    int Pieces;
                    DGRCargoBAL dgr = new DGRCargoBAL();

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

                        blnres = dgr.SaveDGRCargo(AWBNumber, UNID, Pieces, Weight,ERGCode, UpdatedBy, UpdatedOn, txtAwbPrefix.Text, PG,Desc);
                        
                        Session["DGRAWB"] = AWBNumber;
                        dgr = null;
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
                        DataTable dtRoute = new DataTable("GHA_QuickBooking_177");
                            dtRoute = (DataTable)Session["FltRoute"];

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

                #region "AWB Tax Details"
                try
                {
                    if (Session["TaxDetails"] != null)
                    {
                        DataTable dtRoute = new DataTable("GHA_QuickBooking_178");
                            dtRoute = (DataTable)Session["TaxDetails"];

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

                DataSet dtAWBPiece = new DataSet("GHA_QuickBooking_109");
                    dtAWBPiece = (DataSet)Session["dsPiecesDet"];
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
                        HidBookingError.Value = "Save failed (AWB Pieces Details)";
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

                #region Check to combine Save & execute functionality
                try
                {
                    //MasterBAL objBAL = new MasterBAL();
                    if (CommonUtility.CombineSaveandExecute)
                    {
                        btnExecute_Click(null, null);
                    }
                }
                catch (Exception ex) { }
                #endregion

                if (HidAcceptance.Value == "1")
                {
                    GenerateWalkinInvoice();

                    #region Saving Dimension Details

                    //string AWBNumber = string.Empty;
                    string FlightNo = string.Empty;
                    string FlightDate = string.Empty;

                    FlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).Text;
                    FlightDate = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY;
                    string awb = string.Empty;

                    if (txtAWBNo.Text.Trim() != "")
                        awb = txtAwbPrefix.Text + '-' + txtAWBNo.Text.Trim();
                    else
                        awb = txtAwbPrefix.Text + '-' + HidAWBNumber.Value;

                    if (Session["dsDimesionAll"] != null)
                    {
                        DataSet PieceDetails = new DataSet("GHA_QuickBooking_110");
                            PieceDetails = (DataSet)Session["dsDimesionAll"];

                        if (PieceDetails != null && PieceDetails.Tables.Count > 0)
                        {
                            if (PieceDetails.Tables[0].Rows.Count > 0)
                            {
                                //if (awb == PieceDetails.Tables[0].Rows[0]["AWBNumber"].ToString())
                                //{

                                #region New Code added to Accept ULD Against AWB(20/01/2014)
                                if (Session["dsDimesionAll"] != null)
                                {
                                    string AWBNum = awb;
                                    string[] AWBDet = AWBNum.Split('-');
                                    string FlightNum = FlightNo;
                                    string FltDt = FlightDate;
                                    if (AWBDet.Length > 0 && txtAccpPieces.Text != "" && txtAccpPieces.Text != "0")
                                    {
                                        GenerateAWBDimensions(AWBDet[1], int.Parse(txtAccpPieces.Text), (DataSet)Session["dsDimesionAll"],
                                            Convert.ToDecimal(txtAccpWeight.Text), true, AWBDet[0], FlightNum, FltDt);
                                    }

                                }

                                #endregion
                                i = 0;
                                for (i = 0; i < PieceDetails.Tables[0].Rows.Count; i++)
                                {   
                                        if (PieceDetails.Tables[0].Rows[i]["PieceType"].ToString() == "ULD" && PieceDetails.Tables[0].Rows[i]["ULDNo"].ToString() != "")
                                        {
                                            SaveULDNoinMaster(PieceDetails.Tables[0].Rows[i]["ULDNo"].ToString());
                                        }                                   
                                }
                                //}                            
                            }
                        }
                    }

                    #endregion
                }

                if (HidAcceptance.Value == "Accept")
                    return;

                //btnSearchAWB_Click(this, new EventArgs());
                lblStatus.Text = "AWB Details Saved Successfully.";
                Session["ButtonID"] = "btnSave";
                txtAWBNo.Text = HidAWBNumber.Value;
                lblStatus.ForeColor = Color.Green;
                txtAWBNo.ReadOnly = true;
                SetLabelValues(false);
                EnableDisable("Edit");
                Response.Redirect("~/GHA_QuickBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim() + "&GHA=" + Convert.ToString(Session["GHABooking"]), false);
                btnHABDetails_Click(null, null);
            }
            catch (Exception ex)
            {
                HidBookingError.Value = "Error updating AWB Information: " + ex.Message;
                lblStatus.Text = "Error updating AWB Information: " + ex.Message;
            }
        }
        #endregion btnSave_Click

        #region ProcessRates
        protected bool ProcessRates()
        {
            decimal IATAcharge, MKTcharge, MKTTax, IATATax, IATAtax;
            float OCDC, OCDA, OCTax, TotalWt, OATax;
            string IATARateID = "", MKTRateID = "";
            string RateCurrency = "";
            try
            {
                string Origin = ddlOrg.Text.ToString();
                string Destination = ddlDest.Text.ToString();
                BALProcessRates objProcessRate = new BALProcessRates();
                LBLRouteStatus.Text = "";
                lblStatus.Text = "";
                lblRateStatus.Text = string.Empty;
                
                if (HidProcessFlag.Value != "1")
                    return true;

                AgentCurrency = drpCurrency.SelectedItem.Text.Trim().ToUpper();
                OCDCCurrency = "";
                double fltGrossWt = 0;
                double fltChrgWt = 0;
                double fltSumChrgblWt = 0;
                DataSet dsResult = new DataSet("GHA_QuickBooking_111");
                //DataSet dsPrimeFlight = null;
                int intVIA = 0;
                int AWBPieces = 0;
                float fltMtGrossWt = 0;
                float fltMtChrgblWt = 0;
                //bool IsRateExists = false;
                Session["OCTotal"] = null;
                Session["OATotal"] = null;

                DataTable dtRates = new DataTable("GHA_QuickBooking_112");
                DataSet dsDetails = new DataSet("GHA_QuickBooking_113");
                DataRow RatesRow = null;
                //Thread objProcess = null;

                //RefreshMaterialDG();

                #region Gross & Chargable Weight Check
                
                fltGrossWt = Convert.ToDouble(txtGrossWt.Text);
                if (txtChargeableWt.Text.Trim() != "")
                    fltChrgWt = Convert.ToDouble(txtChargeableWt.Text);
                else
                {
                    txtChargeableWt.Text = txtGrossWt.Text;
                    fltChrgWt = Convert.ToDouble(txtChargeableWt.Text);
                }

                AWBPieces = AWBPieces + Convert.ToInt16(txtPieces.Text);

                fltSumChrgblWt = fltSumChrgblWt + fltChrgWt;

                fltMtGrossWt = fltMtGrossWt + (float)fltGrossWt;
                fltMtChrgblWt = fltMtChrgblWt + (float)fltChrgWt;

                if (fltChrgWt < fltGrossWt)
                {
                    lblStatus.ForeColor = Color.Red;
                    HidBookingError.Value = "Material Gross Wt. can not be greater than Chargeable Wt.";
                    lblStatus.Text = "Material Gross Wt. can not be greater than Chargeable Wt.";
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return false;
                }
                //    }
                //}
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
                    DataSet objDS = new DataSet("GHA_QuickBooking_114");
                        objDS = bal.LoadSystemMasterDataNew("DC");
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
                //if (Convert.ToBoolean(objLoginBAL.GetMasterConfiguration("RouteValidation")))
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RouteValidation")))
                {

                    #region Fill PCs Wt automatically if not entered

                    //If pcs and wt are not entered in Route Grid automatically fill pcs and wt..
                    if (grdRouting.Rows.Count == 1)
                    {
                        string Routepcs = ((TextBox)(grdRouting.Rows[0].FindControl("txtPcs"))).Text;
                        string Routewt = ((TextBox)(grdRouting.Rows[0].FindControl("txtWt"))).Text;

                        if (Routepcs == "")
                        {
                            ((TextBox)(grdRouting.Rows[0].FindControl("txtPcs"))).Text = txtPieces.Text.Trim();
                        }
                        if (Routewt == "")
                        {
                            ((TextBox)(grdRouting.Rows[0].FindControl("txtWt"))).Text = txtGrossWt.Text.Trim();
                            ((TextBox)(grdRouting.Rows[0].FindControl("txtChrgWt"))).Text = ((TextBox)(grdRouting.Rows[0].FindControl("txtWt"))).Text;
                        }
                    }
                    #endregion

                    #region Check Route Info

                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {

                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim().ToUpper();
                        ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text = ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltDest")).Text.Trim().ToUpper();

                        strOrigin = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text;
                        strFinalDest = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltDest"))).Text.Trim();
                        
                        if (CommonUtility.FlightValidation)
                        {
                            partner = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartner"))).SelectedItem.Text.Trim();

                            if (partner.Equals("Select", StringComparison.OrdinalIgnoreCase))
                            {
                                HidBookingError.Value = "Partner Code is mandatory.";
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Partner Code is mandatory.";
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                                return false;
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
                                    lblStatus.ForeColor = Color.Red;
                                    HidBookingError.Value = "Flight number is mandatory.";
                                    lblStatus.Text = "Flight number is mandatory.";
                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                                    return false;
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
                                    lblStatus.ForeColor = Color.Red;
                                    HidBookingError.Value = "Flight number is mandatory.";
                                    lblStatus.Text = "Flight number is mandatory.";
                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                                    return false;
                                }
                            }
                        }

                        if (strOrigin != ddlOrg.Text.Trim())
                            intVIA = intVIA + 1;

                        if (strOrigin.Trim() == ddlOrg.Text.Trim())
                        {
                            FGWt = (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtWt"))).Text);
                            if (((TextBox)(grdRouting.Rows[intCount].FindControl("txtChrgWt"))).Text.Trim() == "")
                                ((TextBox)(grdRouting.Rows[intCount].FindControl("txtChrgWt"))).Text = FGWt.ToString();
                            FCWt = (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtChrgWt"))).Text);
                            intPieces = Convert.ToInt32(((TextBox)(grdRouting.Rows[intCount].FindControl("txtPcs"))).Text.Trim());

                            if (intPieces == 0 || FGWt == 0 || FCWt == 0)
                            {
                                lblStatus.ForeColor = Color.Red;
                                HidBookingError.Value = "Route detail values (Pcs / Gwt. / Cwt.) can not be 0.";
                                lblStatus.Text = "Route detail values (Pcs / Gwt. / Cwt.) can not be 0.";
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                                return false;
                            }

                            if (FGWt > FCWt)
                            {
                                lblStatus.ForeColor = Color.Red;
                                HidBookingError.Value = "Route Gross Wt. can not be greater than Chargeable Wt.";
                                lblStatus.Text = "Route Gross Wt. can not be greater than Chargeable Wt.";
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                                return false;
                            }

                            fltFltGrossWt = fltFltGrossWt + FGWt;
                            fltFltChrgblWt = fltFltChrgblWt + FCWt;
                            totalPcs = totalPcs + intPieces;
                        }

                    }
                    #endregion

                    if (AWBPieces != totalPcs)
                    {
                        lblStatus.ForeColor = Color.Red;
                        HidBookingError.Value = " pieces count in Material description & Flight route are not matching.";
                        lblStatus.Text = " pieces count in Material description & Flight route are not matching.";
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return false;
                    }
                    #region Material weights & Flight weights comparision
                    if (fltMtGrossWt != fltFltGrossWt || fltMtChrgblWt != fltFltChrgblWt)
                    {
                        lblStatus.ForeColor = Color.Red;
                        HidBookingError.Value = "Material weights & Flight weights are not matching.";
                        lblStatus.Text = "Material weights & Flight weights are not matching.";
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return false;
                    }
                    #endregion

                    #region destination details are not matching
                    if (ddlDest.Text.ToUpper().Trim() != strFinalDest.ToUpper().Trim())
                    {
                        lblStatus.ForeColor = Color.Red;
                        HidBookingError.Value = "Route details are not matching with AWB destination.";
                        lblStatus.Text = "Route details are not matching with AWB destination.";
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return false;
                    }
                    #endregion
                }
                string strResult = CheckAWBValidations();

                if (strResult != string.Empty)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
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
                DateTime dtExecutionDt = txtExecutionDate1.DateValue; //DateTime.ParseExact(txtExecutionDate.Text, "dd/MM/yyyy", null);
                //string strOrigin = string.Empty;
                string strFltOrigin = string.Empty;
                string strDestination = string.Empty;
                string strPaymentType = string.Empty;
                string strFlightNo = string.Empty;
                BookingBAL objBooking = new BookingBAL();

                for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                {
                    strCommodity = txtCommodityCode.Text;
                    strFltOrigin = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text;
                    strDestination = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltDest"))).Text;
                    strPaymentType = ddlPaymentMode.SelectedValue;

                    if (((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem != null)
                        strFlightNo = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text;

                    if (strFltOrigin.Trim() == ddlOrg.Text.Trim())
                    {
                        fltFltGrossWt = fltFltGrossWt + (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtWt"))).Text);
                        fltFltChrgblWt = fltFltChrgblWt + (float)Convert.ToDecimal(((TextBox)(grdRouting.Rows[intCount].FindControl("txtChrgWt"))).Text);
                    }

                    DataSet Objds = new DataSet("GHA_QuickBooking_115");
                        Objds = objBooking.VerifyEmbargoCargo(dtExecutionDt, strFltOrigin, strDestination, strCommodity, strPaymentType, strFlightNo);

                    if (Objds != null && Objds.Tables.Count > 0 && Objds.Tables[0].Rows.Count > 0)
                    {
                        if ((bool)Objds.Tables[0].Rows[0]["IsInvalid"] == true)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = Convert.ToString(Objds.Tables[0].Rows[0]["ErrorDesc"]);
                            HidBookingError.Value = Convert.ToString(Objds.Tables[0].Rows[0]["ErrorDesc"]);
                            return false;
                        }
                    }
                }

                #endregion

                #region Agent Exception
                bool AgntException = objBLL.CheckforAgentException(TXTAgentCode.Text.Trim());
                #endregion

                #region "FOC Shipments"

                if (ddlPaymentMode.SelectedValue.Trim() == "FOC" || ddlServiceclass.SelectedItem.Text.Trim()=="FOC")
                {
                    txtFreightIATA.Text = "0";
                    txtFreightMKT.Text = "0";
                    txtRatePerKG.Text = "0";
                    txtRatePerKGInt.Text = "0";
                    txtRateClass.Text = "N";

                    //txtSpotRate.Text = "0";
                    txtOCDueCar.Text = "0";
                    txtOCDueAgent.Text = "0";
                    txtSubTotal.Text = "N";

                    txtServiceTax.Text = "0";
                    txtTotalAmount.Text = "0";
                    txtCurrency.Text = drpCurrency.SelectedValue;

                    dsDetails = (DataSet)Session["OCDetails"];

                    if (dsDetails != null && dsDetails.Tables.Count > 0 && dsDetails.Tables[0].Rows.Count > 0)
                    {
                        dsDetails.Tables[0].Rows.Clear();
                        Session["OCDetails"] = dsDetails;
                    }

                    #region FLTRoute Details
                    LoadFlightRouteDetails();
                    DataTable FOCRoute = new DataTable("GHA_QuickBooking_179");
                        FOCRoute = (DataTable)Session["FltRoute"];
                    FOCRoute.Rows.Clear();
                    DataRow drow = null;
                    drow = FOCRoute.NewRow();

                    drow["FltOrigin"] = ddlOrg.Text.Trim();
                    drow["FltDestination"] = ddlDest.Text.Trim();
                    drow["FltNumber"] = "";
                    drow["FltDate"] = "";
                    drow["Pcs"] = txtPieces.Text;
                    drow["GWt"] = txtGrossWt.Text;
                    drow["CWt"] = txtChargeableWt.Text;
                    drow["IsPrime"] = "false";
                    drow["Freight"] = "0";
                    drow["FrtTax"] = "0";
                    drow["RatePerKg"] = "0";
                    drow["isHeavy"] = "0";
                    drow["Proration"] = "0";
                    drow["Discount"] = "0";
                    drow["AgentComm"] = "0";
                    FOCRoute.Rows.Add(drow);
                    Session["FltRoute"] = null;
                    Session["FltRoute"] = FOCRoute;
                    GrdRateDetails.DataSource = null;
                    GrdRateDetails.DataSource = FOCRoute;
                    GrdRateDetails.DataBind();
                    #endregion
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                    return true;
                }

                #endregion "FOC Shipments"


                if (strFlightNumbers.Length > 0)
                    strFlightNumbers = strFlightNumbers.Substring(0, strFlightNumbers.Length - 1);
                if (intVIA > 0)
                {
                    DataSet dsCommonRate = new DataSet("GHA_QuickBooking_116");
                        dsCommonRate = objBLL.CheckPrimeRate(ddlOrg.Text.Trim(), ddlDest.Text.Trim(), dtCurrentDate.Date, strFlightNumbers);

                    if (dsCommonRate != null)
                        strFlightNumbers = dsCommonRate.Tables[0].Rows[0][0].ToString();
                    else
                        strFlightNumbers = string.Empty;

                    dsCommonRate = null;
                }
                else
                    strFlightNumbers = string.Empty;

                string agentcode, commcode, carrier;
                string FlightNumber = string.Empty;
                DataTable DT = new DataTable("GHA_QuickBooking_180");

                if (((DataTable)Session["FltRoute"]) != null)
                    DT = ((DataTable)Session["FltRoute"]).Copy();

                commcode = txtCommodityCode.Text.Trim();
                agentcode = TXTAgentCode.Text.Trim();

                try
                {
                    if (Convert.ToBoolean(Session["GHABooking"]) == false)
                    {
                        if (!ValidateMasterEntry(commcode, "Commodity"))
                        {
                            HidBookingError.Value = "Commodity Code not found in Master record. Please validate";
                            lblStatus.Text = "Commodity Code not found in Master record. Please validate";
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                            txtCommodityCode.Focus();
                            return false;
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
                IATAcharge = IATAtax = MKTcharge = MKTTax = IATATax = 0;
                OCDC = OCDA = OCTax = OATax = TotalWt = 0;
                
                TotalWt = float.Parse(txtChargeableWt.Text);
                //Rate Calculation
                try
                {
                    GrdRateDetails.DataSource = null;
                    GrdRateDetails.DataBind();
                }
                catch (Exception ex) { }
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
                Decimal tax = 0;
                try
                {
                    DataSet dsCharges = new DataSet("GHA_QuickBooking_117");
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
                            DataSet ds = new DataSet("GHA_QuickBooking_118");
                            DataTable dt = new DataTable("GHA_QuickBooking_119");
                            dt = dsCharges.Tables[2].Copy();
                            ds.Tables.Add(dt);
                            ds.AcceptChanges();
                            Session["OCDetails"] = ds.Copy();
                            //SetOtherChargesSummary(ref OCDC, ref OCDA, ref OCTax);
                            SetOtherChargesSummary(ref OCDC, ref OCDA, ref OCTax, ref OATax);

                        }
                        if (dsCharges.Tables.Count > 3)
                        {
                            DataSet dsTax = new DataSet("GHA_QuickBooking_120");
                            DataTable dtTax = new DataTable("GHA_QuickBooking_121");
                            dtTax = dsCharges.Tables[4].Copy();
                            Session["TaxDetails"] = dtTax.Copy();

                        }
                    }
                    catch (Exception ex) { }
                    IATAcharge = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["Charge"].ToString());
                    MKTcharge = Convert.ToDecimal(dsCharges.Tables[0].Rows[1]["Charge"].ToString());
                    RateClass = dsCharges.Tables[0].Rows[1]["RateClass"].ToString().Trim();

                    //tax = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["ServiceTax"].ToString());
                    Session["QK_Rates"] = dsCharges;
                    //tax = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["ServiceTax"].ToString());
                    IATATax = Convert.ToDecimal(dsCharges.Tables[0].Rows[0]["ServiceTax"].ToString());
                    MKTTax = Convert.ToDecimal(dsCharges.Tables[0].Rows[1]["ServiceTax"].ToString());
                    IATARateID = dsCharges.Tables[0].Rows[1]["RateLineID"].ToString().Trim();
                    MKTRateID = dsCharges.Tables[0].Rows[1]["RateLineID"].ToString().Trim();
                    RateCurrency = dsCharges.Tables[0].Rows[1]["Currency"].ToString().Trim();

                    if (RateClass.Trim().Length < 1 || RateClass.Trim() == "")
                        RateClass = dsCharges.Tables[0].Rows[0]["RateClass"].ToString().Trim();


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

                //dtRates.Rows.Clear();

                bool IsMini = false;
                if (RateClass.Equals("M", StringComparison.OrdinalIgnoreCase))
                    IsMini = true;
                
                #region Rounding Off Call
                /*  SCM_FREIGHT,SCM_OCDC,SCM_OCDA*/
                #region OCDC Round OFF
                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_OCDC", OCDC.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
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
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_OCDA", OCDA.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
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
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_FREIGHT", IATAcharge.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        IATAcharge = Convert.ToDecimal(Val);
                    }

                }
                catch (Exception e) { }
                #endregion

                #region IATA Freight
                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_FREIGHT", MKTcharge.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        MKTcharge = Convert.ToDecimal(Val);
                    }

                }
                catch (Exception ex)
                { }
                #endregion
                #endregion

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
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_TAX", TaxTotal.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
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

                    dcTotal = (IATAcharge + (decimal)OCDA + (decimal)OCDC + TaxTotal);
                }

                try
                {
                    decimal num = 0;
                    string Val = objProcessRate.GetRoundoffvalue(Origin, Destination, "SCM_TOTAL", dcTotal.ToString(), txtExecutionDate1.DateFormatDDMMYYYY, TXTAgentCode.Text, txtShipperCode.Text, txtConsigneeCode.Text);
                    if (Decimal.TryParse(Val, out num))
                    {
                        dcTotal = Convert.ToDecimal(Val);
                    }

                }
                catch (Exception e) { }
                #endregion

                #region Logic to Make round-off
                try
                {
                    //MasterBAL objBAL = new MasterBAL();
                    //if (Convert.ToBoolean(objLoginBAL.GetMasterConfiguration("TotalRoundingOFF")))
                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "TotalRoundingOFF")))
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

                string RatePerKG = string.Empty;

                if (AWBTotalasPer.Equals("MKT", StringComparison.OrdinalIgnoreCase))
                {
                    if (IsMini)
                        RatePerKG = (0 + MKTcharge).ToString("0.00");
                    else
                        RatePerKG = (0 + (MKTcharge / Convert.ToDecimal(txtChargeableWt.Text))).ToString("0.00"); ;
                }
                else
                {
                    if (IsMini)
                        RatePerKG = (0 + IATAcharge).ToString("0.00");
                    else
                        RatePerKG = (0 + (IATAcharge / Convert.ToDecimal(txtChargeableWt.Text))).ToString("0.00");

                }
              
                txtRatePerKG.Text = RatePerKG;
                txtRatePerKGInt.Text = RatePerKG;

                dcActualAmtMKT = dcActualTaxMKT = dcMinTaxMKT = dcMinAmountMKT = 0;
                dcActualAmt = dcActualTax = dcMinTax = dcMinAmount = 0;
               
                txtCurrency.Text = RateCurrency;//
                try
                {
                    drpCurrency.Enabled = true;

                    drpCurrency.SelectedIndex = drpCurrency.Items.IndexOf(drpCurrency.Items.FindByText(RateCurrency));
                    //drpCurrency.SelectedValue= RateCurrency;
                }
                catch (Exception ex) { }
                
                txtFreightIATA.Text = Convert.ToString(IATAcharge);
                txtFreightMKT.Text = Convert.ToString(MKTcharge);
                txtRatePerKG.Text = RatePerKG; 
                txtRatePerKGInt.Text = RatePerKG; 
                txtRateClass.Text = RateClass;

                //                txtSpotRate.Text = "";
                txtOCDueCar.Text = Convert.ToString(OCDC);
                txtOCDueAgent.Text = Convert.ToString(OCDA);
                txtSubTotal.Text = "0";// Convert.ToString(IATAcharge + (decimal)OCDC + (decimal)OCDA);
                if (AWBTotalasPer.Equals("MKT", StringComparison.OrdinalIgnoreCase))
                {

                    txtSubTotal.Text = Convert.ToString(MKTcharge + (decimal)OCDC + (decimal)OCDA);
                }
                else
                {
                    txtSubTotal.Text = Convert.ToString(IATAcharge + (decimal)OCDC + (decimal)OCDA);

                }
                txtServiceTax.Text = Convert.ToString(TaxTotal);//Convert.ToString(Math.Round(((decimal)tax + (decimal)OCTax + (decimal)OATax), 2));
                txtTotalAmount.Text = Convert.ToString(dcTotal);
                //txtCurrency.Text = drpCurrency.SelectedValue;
                if ((IATARateID.Length < 1 && MKTRateID.Length < 1) || (IATARateID == "" && MKTRateID == ""))
                {
                    lblRateStatus.Text = "No Rate available";
                    lblRateStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :(ProcessRates5_Click)" + ex.Message;
            }
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            return true;
        }
        #endregion

        #region ShipperCodeChanges Event
        protected void LoadShipperCodeDetails(object sender, EventArgs e)
        {
            ShipperBAL objSHBal = null;
            DataSet ds = new DataSet("GHA_QuickBooking_122");
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

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                            if (TXTShipper.Text == "" || TXTShipAddress.Text == "" || ddlShipCountry.SelectedItem.Text == "")
                            {
                                imgShipperTick.Attributes.Add("style", "display:none");
                                imgShipperCross.Attributes.Add("style", "display:inline-table");
                            }
                            else
                            {
                                imgShipperCross.Attributes.Add("style", "display:none");
                                imgShipperTick.Attributes.Add("style", "display:inline-table");
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanel_shipperPopUp();callclose();</script>", false);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanel_shipperPopUp();callclose();</script>", false);
                    }

                    txtConsigneeCode.Focus();

                }
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
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
        }

        protected void LoadConsigneeDetails(object sender, EventArgs e)
        {
            ShipperBAL objSHBal = null;
            DataSet ds = new DataSet("GHA_QuickBooking_123");
            try
            {
                if (sender.Equals(txtConsigneeCode))
                {
                    objSHBal = null;
                    ds = null;

                    if (txtConsigneeCode.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanel_shipperPopUp();callclose();</script>", false);
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
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        if (TXTConsignee.Text.Trim() == "" || TXTConAddress.Text.Trim() == "" || ddlConCountry.SelectedItem.Text == "")
                        {
                            imgConTick.Attributes.Add("style", "display:none");
                            imgConCross.Attributes.Add("style", "display:inline-table");
                        }
                        else
                        {
                            imgConCross.Attributes.Add("style", "display:none");
                            imgConTick.Attributes.Add("style", "display:inline-table");
                        }
                        
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanel_ConsigneePopUp();callclose();</script>", false);
                    }
                    txtShipmentDate1.Focus();
                }
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
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
        }
        #endregion
        
        protected void btnSaveBooking_Click(object sender, EventArgs e)
        {
            // ------ Sumit 2014-10-30
            try
            {
                //string FltDestination = ddlDest.SelectedItem.ToString();
                int rowindex = 0;

                if (grdRouting.Rows.Count == 1)
                {
                    if (((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text.Trim() == "")
                    {
                        if (txtPieces.Text.Trim() != "")
                        {
                            ((TextBox)grdRouting.Rows[rowindex].FindControl("txtPcs")).Text = txtPieces.Text;
                            ((TextBox)grdRouting.Rows[rowindex].FindControl("txtWt")).Text = txtGrossWt.Text;
                            ((TextBox)grdRouting.Rows[rowindex].FindControl("txtChrgWt")).Text = txtChargeableWt.Text;
                        }
                    }


                    btnSave_Click(null, null);
                    txtTotalAmount.BackColor = Color.Blue;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                }
            }
            catch (Exception ex) { }
        }

        protected void imgProcessRates_Click(object sender, ImageClickEventArgs e)
        {
            HidProcessFlag.Value = "1";
            ProcessRates();
        }

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
                    if (DateTime.TryParseExact(((DateControl)gvr.FindControl("txtFdate")).DateFormatDDMMYYYY, "dd/MM/yyyy", null,
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
                    if (int.TryParse(txtAccpPieces.Text.Trim(), out pieceCount))
                    {
                        objOpsTimeStamp.Pieces = pieceCount;
                    }
                    else
                    {
                        objOpsTimeStamp.Pieces = 0;
                    }
                    decimal weight = 0;
                    if (decimal.TryParse(txtAccpWeight.Text.Trim(), out weight))
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
                }
                //Check if list is built.
                if (objListOpsTime.Count > 0)
                {
                    BALCommon objBALCommon = new BALCommon();

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

        #region Shipment Date Changed
        protected void txtShipmentDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool bothDatesValid = false;
                DateTime fltDate;
                if (DateTime.TryParseExact(((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out fltDate))
                {   //Get flight date.
                    bothDatesValid = true;
                }
                DateTime shipDate;
                if (DateTime.TryParseExact(txtShipmentDate1.Text, Convert.ToString(Session["DateFormat"]), null,
                    System.Globalization.DateTimeStyles.None, out shipDate))
                {   //Get Shipment date.
                    bothDatesValid = true;
                }
                if (bothDatesValid)
                {   //If shipmnet date is later than flight date then set shipment date as flight date.
                    if (DateTime.Compare(shipDate, fltDate) > 0)
                    {
                        ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY = shipDate.ToString("dd/MM/yyyy");
                        HidChangeDate.Value = "Y";
                        if (((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim() != "")
                        {
                            txtFdate_TextChanged(txtShipmentDate1, new EventArgs());
                            ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedIndex = 0;
                        }
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

        private void LoadFlightDetails()
        {
            string FltDestination = ddlDest.SelectedItem.ToString();
            int rowindex = 0;

            if (grdRouting.Rows.Count == 1)
            {
                ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text = FltDestination;
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);
                ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedIndex = 0;
            }
        }

        protected void txtPcs_TextChanged1(object sender, EventArgs e)
        {
            try
            {

                LBLRouteStatus.Text = "";

                if (txtChargeableWt.Text.Trim() == "")
                    txtChargeableWt.Text = txtGrossWt.Text;
                
                if (txtPieces.Text.Trim() == "")
                {
                    LBLRouteStatus.ForeColor = Color.Red;
                    LBLRouteStatus.Text = "Please fill AWB Pieces to proceed.";


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
                else if (txtChargeableWt.Text.Trim() == "")
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

                    string weight = txtGrossWt.Text.Trim();
                    string ChrgblWt = txtChargeableWt.Text.Trim();
                    if (weight.Contains('.'))
                        weight = weight.Substring(0, weight.IndexOf('.'));

                    int total = int.Parse(txtPieces.Text.Trim());
                    //int wt = int.Parse(((Label)grdMaterialDetails.FooterRow.FindControl("LBLTotalChargedWt")).Text.Trim());
                    int wt = int.Parse(weight);
                    float Cwt = float.Parse(ChrgblWt);

                    ArrayList flightlist = new ArrayList();
                    int currenttotal = 0;
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == HidOrigin.Value)
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
                                date = ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).DateFormatDDMMYYYY;
                                rowindex = i;
                            }
                        }

                        int calwt = (int)((int.Parse(((TextBox)sender).Text)) * wt / total);
                        float calChrgblwt = (float)((float.Parse(((TextBox)sender).Text)) * Cwt / total);
                        string routeFLT = "";

                        TextBox changedText = (TextBox)sender;
                        GridViewRow row = (GridViewRow)changedText.Parent.Parent;
                        int index = row.RowIndex;

                        ((TextBox)grdRouting.Rows[index].FindControl("txtPcs")).Text = "" + int.Parse(((TextBox)sender).Text);
                        ((TextBox)grdRouting.Rows[index].FindControl("txtWt")).Text = "" + calwt;
                        ((TextBox)grdRouting.Rows[index].FindControl("txtChrgWt")).Text = "" + calChrgblwt;

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

        protected void TXTAgentCode_TextChanged(object sender, EventArgs e)
        {
            // ------ Sumit 2014-10-30
            try
            {
                BookingBAL objBAL = new BookingBAL();
                DataSet DS = new DataSet("GHA_QuickBooking_124");
                    DS = objBAL.GetAgentCodeDetails(TXTAgentCode.Text.Trim(), Convert.ToDateTime(Session["IT"]).Date);
                if (DS != null && DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    txtAgentName.Text = DS.Tables[0].Rows[0]["AgentName"].ToString();
                    txtIACCode.Text = DS.Tables[0].Rows[0]["IACNo"].ToString();
                    txtCCSF.Text = DS.Tables[0].Rows[0]["CCSFNo"].ToString();

                    Session["QB_AGTCurrency"] = DS.Tables[0].Rows[0]["CurrencyCode"].ToString();

                    if (txtIACCode.Text.Trim() != "" || txtCCSF.Text.Trim() != "")
                        chkTBScreened.Checked = false;

                    ddlProductType.Focus();
                    if (sender == null) 
                    {
                        SetPaymentMode(); 
                    }
                  
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Agent code";
                    TXTAgentCode.Focus();
                }

                objBAL = null;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
            catch (Exception ex) { }
        }

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

        #region Button Save Acceptance
        protected void btnSaveAcceptance_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string AWBNumber = string.Empty;
                string FlightNo = string.Empty;
                string FlightDate = string.Empty;
                string PartnerCode = string.Empty;

                int TotalPcs = int.Parse(txtPieces.Text);
                float TotalWeight = float.Parse(txtGrossWt.Text);
                int AcceptedPcs = int.Parse(txtAccpPieces.Text);
                float AcceptedWt = float.Parse(txtAccpWeight.Text);
                float ChargblWt = float.Parse(txtChargeableWt.Text);
                string FlightChrgblWt = ((TextBox)grdRouting.Rows[0].FindControl("txtChrgWt")).Text;
                PartnerCode = ((DropDownList)grdRouting.Rows[0].FindControl("ddlPartner")).Text;
                FlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).Text;
                FlightDate = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY;
                string awb = txtAwbPrefix.Text + '-' + txtAWBNo.Text;
                bool IsDimensionsMadantory = false;
                int intRoutes = 0;
                float FltChrgblWt = 0;

                try
                {
                    //IsDimensionsMadantory = Convert.ToBoolean(lBal.GetMasterConfiguration("MandatoryDimension"));
                    IsDimensionsMadantory = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "MandatoryDimension"));
                }
                catch { }

                #region Change Booking details if Booked & Accepted details are different

                if (IsDimensionsMadantory)
                {
                    DataSet Dimensions = new DataSet("GHA_QuickBooking_125");
                    Dimensions = (DataSet)Session["dsDimesionAll"];

                    if (Dimensions == null || Dimensions.Tables.Count < 1 || Dimensions.Tables[0].Rows.Count < 1)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter the Dimensions.";
                        return;
                    }

                    bool IsDimensions = true;
                    decimal Length = 0, Breadth = 0, Height = 0;

                    if (Dimensions != null && Dimensions.Tables.Count > 0 && Dimensions.Tables[0].Rows.Count > 0)
                    {
                        for (int iDim = 0; iDim < Dimensions.Tables[0].Rows.Count; iDim++)
                        {
                            if (Dimensions.Tables[0].Rows[iDim]["PieceType"].ToString().ToUpper() == "ULD" && Dimensions.Tables[0].Rows.Count == 1)
                            {
                                if (txtAccpWeight.Text.Trim() == "")
                                    txtAccpWeight.Text = txtGrossWt.Text;

                                Dimensions.Tables[0].Rows[iDim]["Wt"] = Convert.ToDecimal(txtAccpWeight.Text.Trim());
                            }

                            if (Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Wt"]) == 0)
                            {
                                IsDimensions = false;
                                break;
                            }

                            Length = Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Length"]);
                            Breadth = Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Breath"]);
                            Height = Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Height"]);

                            if (Dimensions.Tables[0].Rows[iDim]["PieceType"].ToString().ToUpper() != "ULD")
                            {
                                if (Length == 0 || Breadth == 0 || Height == 0)
                                {
                                    IsDimensions = false;
                                    break;
                                }
                            }
                        }

                        if (IsDimensions == false || AcceptedPcs != Dimensions.Tables[0].Rows.Count)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter the Dimensions. Dimensions are Mandatory.";
                            return;
                        }
                    }
                }

                try
                {
                    if (((TextBox)grdRouting.Rows[0].FindControl("txtChrgWt")).Text.Trim() != "")
                        FltChrgblWt = float.Parse(((TextBox)grdRouting.Rows[0].FindControl("txtChrgWt")).Text.Trim());
                }
                catch { }

                if (TotalPcs != AcceptedPcs || TotalWeight != AcceptedWt || FltChrgblWt != ChargblWt)
                {
                    txtPieces.Text = AcceptedPcs.ToString();
                    txtGrossWt.Text = AcceptedWt.ToString();

                    if (grdRouting.Rows.Count == 1)
                    {
                        ((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text = txtPieces.Text;
                        ((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text = txtGrossWt.Text;
                        ((TextBox)grdRouting.Rows[0].FindControl("txtChrgWt")).Text = txtChargeableWt.Text;
                        HidProcessFlag.Value = "1";
                    }
                    else
                    {
                        for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                        {
                            if (ddlOrg.Text.Trim() == ((TextBox)grdRouting.Rows[intCount].FindControl("txtFltOrig")).Text.Trim())
                            {
                                intRoutes = intRoutes + 1;
                            }
                        }

                        if (intRoutes == 1)
                        {
                            for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                            {
                                ((TextBox)grdRouting.Rows[intCount].FindControl("txtPcs")).Text = txtPieces.Text;
                                ((TextBox)grdRouting.Rows[intCount].FindControl("txtWt")).Text = txtGrossWt.Text;
                                ((TextBox)grdRouting.Rows[intCount].FindControl("txtChrgWt")).Text = txtChargeableWt.Text;
                                HidProcessFlag.Value = "1";
                            }
                        }
                    }
                }

                HidAcceptance.Value = "1";
                
                //if (FlightChrgblWt != txtChargeableWt.Text)
                btnSave_Click(null, null);

                HidAcceptance.Value = "";

                #endregion

                //Function for creates auto messages for AWB creation updation
                #region Auto Message Creation
                bool GenerateAutoMSG = false;
                //LoginBL objLoginBAL = new LoginBL();
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

            }
            catch (Exception ex)
            { }
            finally
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
        }
        #endregion

        #region Saving ULDNo in Master
        private bool SaveULDNoinMaster(string UDLNumber)
        {
            // ------ Sumit 2014-10-30
            try
            {
                if (UDLNumber.Trim() == "")
                    return false;

                string strULDPrefix = UDLNumber.Trim().Substring(0, 3);
                string strULDSuffix = UDLNumber.Trim().Substring(UDLNumber.Trim().Length - 2, 2);
                string strULDSerial = UDLNumber.Trim().Replace(strULDPrefix, "").Replace(strULDSuffix, "");

                BALULDMaster blULD = new BALULDMaster();

                blULD.SelectRecords(UDLNumber, strULDSuffix, 0, "0", "0", 0, 0, 0, "0", "", "", "", false, "", strULDSerial, Convert.ToString(Session["Station"]), Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]),
                    "", 0, "0", "0", Convert.ToDateTime(Session["IT"]), "", "", "", false, "3", "Y", "0");

                blULD = null;

                return true;
            }
            catch (Exception ex) { return false; }
        }
        #endregion

        protected void btnSaveShipperPopUP_Click(object sender, EventArgs e)
        {
            try
            {
                if (TXTShipper.Text == "" || TXTShipAddress.Text == "" || ddlShipCountry.SelectedItem.Text == "")
                {
                    imgShipperTick.Attributes.Add("style", "display:none");
                    imgShipperCross.Attributes.Add("style", "display:inline");
                }
                else
                {
                    imgShipperCross.Attributes.Add("style", "display:none");
                    imgShipperTick.Attributes.Add("style", "display:inline");
                }
                txtConsigneeCode.Focus();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanel_shipperPopUp();</script>", false);
            }
            catch (Exception ex) { }

        }

        protected void btnSaveConsigneePopUP_Click(object sender, EventArgs e)
        {
            try
            {
                if (TXTConsignee.Text.Trim() == "" || TXTConAddress.Text.Trim() == "" || ddlConCountry.SelectedItem.Text == "")
                {
                    imgConTick.Attributes.Add("style", "display:none");
                    imgConCross.Attributes.Add("style", "display:inline");
                }
                else
                {
                    imgConCross.Attributes.Add("style", "display:none");
                    imgConTick.Attributes.Add("style", "display:inline");
                }
                ddlProductType.Focus();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanel_ConsigneePopUp();</script>", false);
            }
            catch (Exception ex) { }

        }

        private void GenerateWalkinInvoice()
        {
            // ------ Sumit 2014-10-30
            try
            {
                //Check if Pay Mode 'PX' exists in the system.
                DropDownList ddl = ddlPaymentMode;
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
            catch (Exception ex) { }
        }
        #region Button Print
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                DataSet dsDimensions = new DataSet("GHA_QuickBooking_126");
                    dsDimensions = (DataSet)Session["dsDimesionAll"];
                if (dsDimensions != null && dsDimensions.Tables.Count > 0 && dsDimensions.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDimensions.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = dsDimensions.Tables[0].Rows[i];
                        if (dr["PieceType"].ToString().Equals("ULD", StringComparison.OrdinalIgnoreCase))
                        {
                            string ULDNumber = dr["ULDNo"].ToString();
                            //string Location = hdLocation.Value;
                            string AWBNumber = txtAwbPrefix.Text.Trim() + txtAWBNo.Text.Trim();
                            //string ULDOrigin = hdULDorigin.Value;
                            string ULDDestination = ddlDest.SelectedItem.Value;
                            string FlightNumber = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim();
                            string FlightDate = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY;
                            string UCR = objUCR.generateUCR(AWBNumber, ULDNumber, "true", ULDDestination, "", Convert.ToDateTime(Session["IT"]),
                                Session["UserName"].ToString(), Convert.ToDateTime(Session["IT"].ToString()));

                            if (UCR != "")
                            {
                                RenderUCRReport(UCR);
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
        #region Render Report
        public void RenderUCRReport(string UCRNo)
        {
            try
            {
                DataTable dtTable1 = new DataTable("GHA_QuickBooking_181");
                DataTable dtRpt = new DataTable("GHA_QuickBooking_182");
                    dtRpt = FillUCRReport(UCRNo);
                    DataTable dtIATARpt = new DataTable("GHA_QuickBooking_183");
                    dtIATARpt = GetULDIATACodes(UCRNo);
                    DataTable dtUCRULDRpt = new DataTable("GHA_QuickBooking_184");
                    dtUCRULDRpt = GetUCRULDRpt(UCRNo);
                ReportViewer rptUCRReport = new ReportViewer();

                string AWNNo = "";



                //A method that returns a collection for our report

                //Note: A report can have multiple data sources

                dtTable1 = new DataTable("GHA_QuickBooking_185");
                dtTable1 = dtRpt;

                dtTable2 = new DataTable("GHA_QuickBooking_186");
                dtTable2 = dtIATARpt;

                dtTable3 = new DataTable("GHA_QuickBooking_187");
                dtTable3 = dtUCRULDRpt;

                //List<Employee> employeeCollection = GetData();
                if (dtTable1 != null)
                {
                    if (dtTable1.Rows.Count > 0)
                    {
                        AWNNo = dtTable1.Rows[0]["AWBNo"].ToString();
                    }
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
                    dtTable1.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                }
                catch (Exception ex)
                { }
                //dtTable1.Rows.Add(Logo.ToArray());
                dtTable1.Rows[0]["Logo"] = Logo.ToArray();
                
                rptUCRReport.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = rptUCRReport.LocalReport;

                rep1.ReportPath = Server.MapPath("/rptUCR.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Billing1.rdlc";
                rds1.Name = "dsUCR_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                rptUCRReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                #region Render to PDF
                try
                {
                    //string reportType = "PDF";
                    //string fileNameExtension;
                    //string deviceInfo = "<DeviceInfo><PageHeight>35cm</PageHeight><PageWidth>48cm</PageWidth></DeviceInfo>";

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    //Render the report
                    // send it to the client to download
                    byte[] bytes = rptUCRReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + "UCR" + "." + ".pdf");
                    Response.BinaryWrite(bytes); // create the file
                    Response.Flush();
                }
                catch (Exception ex)
                {

                }
                #endregion
            }
            catch (Exception ex)
            { }
        }
        #endregion
        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsUCR_DataTable2", dtTable2));
            e.DataSources.Add(new ReportDataSource("dsUCR_DataTable3", dtTable3));
        }

        #region Getting UCR Report Details

        #region FillUCRReport Session
        private DataTable FillUCRReport(string UCR)
        {
            try
            {

                DataSet dsgetUCRRpt = new DataSet("GHA_QuickBooking_127");
                    dsgetUCRRpt = objUCR.GetUCRReportNew(UCR);
                if (dsgetUCRRpt != null && dsgetUCRRpt.Tables.Count > 0)
                {
                    return dsgetUCRRpt.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Fill ULDIATA Codes
        private DataTable GetULDIATACodes(string UCR)
        {
            try
            {

                DataSet dsgetIATARpt = new DataSet("GHA_QuickBooking_128");
                    dsgetIATARpt = objUCR.GetIATACodesReport(UCR); //dbN.SelectRecords("spSubRptIATACodes", pname, pvalue, ptype);
                if (dsgetIATARpt != null && dsgetIATARpt.Tables.Count > 0)
                {
                    return dsgetIATARpt.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion
        #region Get UCRULDReport Details
        private DataTable GetUCRULDRpt(string UCR)
        {
            try
            {
                DataSet dsgetUCRULDRpt = new DataSet("GHA_QuickBooking_129");
                    dsgetUCRULDRpt = objUCR.GetUCRULDRpt(UCR);
                if (dsgetUCRULDRpt != null && dsgetUCRULDRpt.Tables.Count > 0)
                {
                    return dsgetUCRULDRpt.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        protected void ddlAirlineCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["QB_PartnerCode"] = ddlAirlineCode.SelectedItem.Text.Trim();
            }
            catch (Exception ex)
            {
                Session["QB_PartnerCode"] = Session["AirlinePrefix"].ToString();
            }
        }
        #endregion

        private string CheckAWBValidationsFlightCheckIn()
        {
            string strResult = string.Empty;
            StringBuilder StrFltDetails = new StringBuilder();
            // ------ Sumit 2014-10-30
            try
            {
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
                                DeptTime = "00:00";// ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedValue;

                                flight = ((DropDownList)grdRouting.Rows[intCount].FindControl("ddlFltNum")).SelectedItem.Text;
                            }
                        }
                        catch (Exception ex) { }

                        StrFltDetails.Append("Insert into #FltDetails (fltNo, fltDate, origin, Dest, FltDepTime, FltPcs, FltGrossWt) values('");
                        StrFltDetails.Append(flight);
                        StrFltDetails.Append("','");
                        StrFltDetails.Append(((DateControl)grdRouting.Rows[intCount].FindControl("txtFdate")).DateFormatMMDDYYYY);
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
                {
                    strResult = objBLL.CheckAWBValidationsforFlightCheckin(StrFltDetails.ToString(), (DateTime)Session["IT"]);
                    HidCheckin.Value = strResult;
                }
                else
                    HidCheckin.Value = "";

                StrFltDetails = null;
            }
            catch (Exception ex) { StrFltDetails = null; }
            return strResult;
        }

        private void VerifyShipperConsignee()
        {
            // ----- Sumit 2014-10-30
            try
            {
                if (TXTShipper.Text.Trim() == "" || TXTShipAddress.Text.Trim() == "" || ddlShipCountry.SelectedItem.Text.Trim() == "" ||
                    TXTShipTelephone.Text.Trim() == "")
                {
                    imgShipperTick.Attributes.Add("style", "display:none");
                    imgShipperCross.Attributes.Add("style", "display:inline");
                }
                else
                {
                    imgShipperCross.Attributes.Add("style", "display:none");
                    imgShipperTick.Attributes.Add("style", "display:inline");
                }

                if (TXTConsignee.Text.Trim() == "" || TXTConAddress.Text.Trim() == "" || ddlConCountry.SelectedItem.Text.Trim() == "" ||
                    TXTConTelephone.Text.Trim() == "")
                {
                    imgConTick.Attributes.Add("style", "display:none");
                    imgConCross.Attributes.Add("style", "display:inline");
                }
                else
                {
                    imgConCross.Attributes.Add("style", "display:none");
                    imgConTick.Attributes.Add("style", "display:inline");
                }
            }
            catch (Exception ex) { }
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
                        DataSet ds = new DataSet("GHA_QuickBooking_130");
                            ds = objHAWB.DownloadHAWBDocument(AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber);
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
                    DataSet ds = new DataSet("GHA_QuickBooking_131");
                      ds =  objHAWB.DownloadHAWBDocument(AWBPrefix, AWBNumber, HAWBPrefix, HAWBNumber);
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

        protected DataTable fillDropinGrid()
        {

            DataTable dtK9 = new DataTable("GHA_QuickBooking_188");
            try
            {
                dtK9 = (DataTable)Session["AirportCode"];
            }
            catch (Exception ex)
            {


            }

            return dtK9;
        }

        protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ----- Sumit 2014-10-30
            try
            {
                Session["Origin"] = ddlOrg.SelectedItem.Text.Trim();
                ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text = ddlOrg.SelectedItem.Text.Trim();
                HidProcessFlag.Value = "1";
                ddlDest.Focus();
            }
            catch (Exception ex) { }
        }
        
        #region EAWB Print
        private void RenderReport(DataTable dtTable1,DataTable dtTable2)
        {

            try
            {

                string AWNNo = "";
                
                //List<Employee> employeeCollection = GetData();
                ReportViewer ReportViewer1 = new ReportViewer();
                AWNNo = dtTable1.Rows[0][2].ToString();
                string[] AWBPrefix = AWNNo.Split('-');
                string sessionawBPrefix = Session["awbPrefix"].ToString();
                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                if (Session["awbPrefix"].ToString() != null)
                {
                    if (AWBPrefix[0].ToString() == sessionawBPrefix)
                    {
                        rep1.ReportPath = (hdAWBShipmentType.Value == string.Empty ? "INT" : hdAWBShipmentType.Value) != "DOM" ? Server.MapPath("/Reports/EAWB.rdlc") : Server.MapPath("/Reports/EAWBDOM.rdlc");
                    }
                    else
                    {
                        rep1.ReportPath = (hdAWBShipmentType.Value == string.Empty ? "INT" : hdAWBShipmentType.Value) != "DOM" ? Server.MapPath("/Reports/EAWB.rdlc") : Server.MapPath("/Reports/EAWBDOM.rdlc");
                    }
                }

                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "AWB.rdlc";
                rds1.Name = "dsShowEAWB_DataTable1";// "dsShowEAWB";// "EMULDAWB_dtManifest";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler1);
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =

                "<DeviceInfo>" +

                "  <OutputFormat>PDF</OutputFormat>" +

                "</DeviceInfo>";

                Warning[] warnings;

                string[] streams;

                byte[] renderedBytes;
                
                //Render the report

                renderedBytes = rep1.Render(

                    reportType,

                    deviceInfo,

                    out mimeType,

                    out encoding,

                    out fileNameExtension,

                    out streams,

                    out warnings);

                Response.Clear();

                Response.ContentType = mimeType;

                Response.AddHeader("content-disposition", "attachment; filename=" + AWNNo + "." + fileNameExtension);

                Response.BinaryWrite(renderedBytes);
                //Response.End();
                renderedBytes = null;
            }
            catch (Exception ex)
            {

            }
            finally 
            {
                
                Response.Flush();
                Response.Close();

            }

        }


        public void ItemsSubreportProcessingEventHandler1(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsShowEAWB_DataTable2", dtTable2));

        }

        #endregion

        public void btnSaveAndAccept_Click(object sender, EventArgs e)
        {
            // ----- Sumit 2014-10-30
            try
            {
                //condition to check if AWB is Voided.(Acceptance shold not happen) - Vijay - 23-09-2014
                if (ddlServiceclass.SelectedIndex == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "AWB is voided. Please save booking.";
                    return;
                }

                int AcceptedPcs = 0;

                if (txtPieces.Text.Trim() != "")
                    AcceptedPcs = Convert.ToInt16(txtPieces.Text.Trim());

                if (ddlPaymentMode.SelectedIndex <= 0 || ddlPaymentMode.SelectedItem.Text.ToUpper() == "SELECT")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Select Payment Mode.";
                    return;
                }

                if (TXTShipper.Text.Trim() == "" || TXTShipTelephone.Text.Trim() == "" ||
                    TXTShipAddress.Text.Trim() == "" || ddlShipCountry.Text.Trim() == "")
                {
                    lblStatus.Text = "Please enter Shipper details.";
                    lblStatus.ForeColor = Color.Red;
                    btnExecute.Enabled = false;
                    return;
                }

                if (TXTConsignee.Text.Trim() == "" || TXTConTelephone.Text.Trim() == "" ||
                    TXTConAddress.Text.Trim() == "" || ddlConCountry.Text.Trim() == "")
                {
                    lblStatus.Text = "Please enter Consignee details.";
                    lblStatus.ForeColor = Color.Red;
                    btnExecute.Enabled = false;
                    return;
                }

                #region "Dimension Check"

                bool IsDimensionsMadantory = false;
                try
                {
                    //IsDimensionsMadantory = Convert.ToBoolean(lBal.GetMasterConfiguration("MandatoryDimension"));
                    IsDimensionsMadantory = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "MandatoryDimension"));
                }
                catch { }

                DataSet Dimensions = new DataSet("GHA_QuickBooking_132");
                   Dimensions = (DataSet)Session["dsDimesionAll"];

                if (IsDimensionsMadantory)
                {
                    if (Session["dsDimesionAll"] == null || Dimensions.Tables.Count < 1 || Dimensions.Tables[0].Rows.Count < 1)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter the Dimensions.";
                        return;
                    }

                    bool IsDimensions = true;
                    decimal Length = 0, Breadth = 0, Height = 0;

                    if (Dimensions != null && Dimensions.Tables.Count > 0 && Dimensions.Tables[0].Rows.Count > 0)
                    {
                        for (int iDim = 0; iDim < Dimensions.Tables[0].Rows.Count; iDim++)
                        {
                            if (Dimensions.Tables[0].Rows[iDim]["PieceType"].ToString().ToUpper() == "ULD" && Dimensions.Tables[0].Rows.Count == 1)
                            {
                                if (txtAccpWeight.Text.Trim() == "")
                                    txtAccpWeight.Text = txtGrossWt.Text;

                                Dimensions.Tables[0].Rows[iDim]["Wt"] = Convert.ToDecimal(txtAccpWeight.Text.Trim());
                            }

                            if (Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Wt"]) == 0)
                            {
                                IsDimensions = false;
                                break;
                            }

                            Length = Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Length"]);
                            Breadth = Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Breath"]);
                            Height = Convert.ToDecimal(Dimensions.Tables[0].Rows[iDim]["Height"]);

                            if(Dimensions.Tables[0].Rows[iDim]["PieceType"].ToString().ToUpper() != "ULD")
                            {
                                if(Length == 0 || Breadth ==0|| Height==0)
                                {
                                    IsDimensions = false;
                                    break;
                                }
                            }
                        }

                        if (IsDimensions == false || AcceptedPcs != Dimensions.Tables[0].Rows.Count)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter the Dimensions. Dimensions are Mandatory.";
                            return;
                        }
                    }
                }

                #endregion

                HidAcceptance.Value = "Accept";

                //System.Diagnostics.Debug.WriteLine("Before Save - " + DateTime.Now.ToString());

                HidBookingError.Value = "";

                btnSave_Click(null, null);

                if (HidBookingError.Value != "")
                {
                    HidBookingError.Value = "";
                    return;
                }

                //System.Diagnostics.Debug.WriteLine("After Save - " + DateTime.Now.ToString());

                AWBExecution(false);

                //System.Diagnostics.Debug.WriteLine("After Execution - " + DateTime.Now.ToString());

                AWBAcceptance();

                //System.Diagnostics.Debug.WriteLine("After Acceptance - " + DateTime.Now.ToString());

                if (HidAWBNumber.Value != "")
                {
                    string AWBNumber = HidAWBNumber.Value;
                    txtAWBNo.Text = AWBNumber;
                    txtAccpPieces.Text = txtPieces.Text;
                    txtAccpWeight.Text = txtGrossWt.Text;
                    Response.Redirect("~/GHA_QuickBooking.aspx?command=Edit&AWBNumber=" + AWBNumber + "&AWBPrefix=" + txtAwbPrefix.Text.Trim() + "&GHA=" + Convert.ToString(Session["GHABooking"]), false);

                    if (TXTAgentCode.Text.Trim() != "")
                    {
                        TXTAgentCode.Enabled = false;
                        txtAgentName.Enabled = false;
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected bool AWBExecution(bool checkbeforereopen)
        {
            // ----- Sumit 2014-10-30
            try
            {
                string errormessage = "";
                string strCommodity = txtCommodityCode.Text.Trim();
                string ToEmail = string.Empty;
                string strSubject = strCommodity + " - Email Intimation";
                string strBody = string.Empty;
                string AWBStatus = "";
                bool blnResult = false;

                //If AWB is Voided - Vijay - 23-09-2014
                if (ddlServiceclass.SelectedItem.Text.Trim() == "Void" || ddlServiceclass.SelectedValue == "0")
                    AWBStatus = "V";
                else
                    AWBStatus = "E";

                blnResult = objBLL.SetAWBStatus(HidAWBNumber.Value.Trim(), AWBStatus, ref errormessage, dtCurrentDate.ToString("dd/MM/yyyy"),
                    strUserName, dtCurrentDate, txtAwbPrefix.Text, checkbeforereopen, ddlPaymentMode.SelectedValue, dtCurrentDate);

                if (blnResult)
                {
                    #region "ACAS Message"

                    try
                    {
                        //LoginBL objLogin = new LoginBL();
                        //if (Convert.ToBoolean(objLogin.GetMasterConfiguration("ACASAutomation")))
                        if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ACASAutomation")))
                        {
                            for (int i = 0; i < grdRouting.Rows.Count; i++)
                            {
                                string FlightNo = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                                string FlightDate = ((DateControl)grdRouting.Rows[i].FindControl("txtFdate")).DateFormatDDMMYYYY;

                                ACASBAL objACAS = new ACASBAL();

                                if (objACAS.ACASFRITriggerPointValidation() == "EX")
                                {

                                    object[] QueryValues = { txtAwbPrefix.Text + "-" + HidAWBNumber.Value.Trim(), FlightNo, FlightDate };
                                    DataSet dsACAS = new DataSet("GHA_QuickBooking_133");
                                        dsACAS = objACAS.CheckACASAWBAvailability(QueryValues);
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
                                                        DataSet dMail = new DataSet("GHA_QuickBooking_134");
                                                            dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
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
                    { blnResult = false; }

                    #endregion
                }

                return true;
            }
            catch (Exception ex) { return false; }
        }

        protected bool AWBAcceptance()
        {
            try
            {
                //System.Diagnostics.Debug.WriteLine("Acceptance - Generate Invoice" + DateTime.Now.ToString());

                GenerateWalkinInvoice();
                
                #region Saving Dimension Details

                //string AWBNumber = string.Empty;
                string FlightNo = string.Empty;
                string FlightDate = string.Empty;

                if (((TextBox)grdRouting.Rows[0].FindControl("txtFlightID")).Text.Trim() == "")
                    FlightNo = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text;
                else
                    FlightNo = ((TextBox)grdRouting.Rows[0].FindControl("txtFlightID")).Text.Trim();
                
                FlightDate = ((DateControl)grdRouting.Rows[0].FindControl("txtFdate")).DateFormatDDMMYYYY;
                string awb = string.Empty;

                if (txtAWBNo.Text.Trim() != "")
                    awb = txtAwbPrefix.Text + '-' + txtAWBNo.Text.Trim();
                else
                    awb = txtAwbPrefix.Text + '-' + HidAWBNumber.Value;

                if (txtAccpPieces.Text.Trim() == "" || txtAccpPieces.Text.Trim() == "0")
                    txtAccpPieces.Text = txtPieces.Text;

                if (txtAccpWeight.Text.Trim() == "" || txtAccpWeight.Text.Trim() == "0")
                    txtAccpWeight.Text = txtGrossWt.Text;

                if (Session["dsDimesionAll"] != null)
                {
                    DataSet PieceDetails = new DataSet("GHA_QuickBooking_135");
                        PieceDetails = (DataSet)Session["dsDimesionAll"];

                    if (PieceDetails != null && PieceDetails.Tables.Count > 0)
                    {
                        if (PieceDetails.Tables[0].Rows.Count > 0)
                        {
                            //System.Diagnostics.Debug.WriteLine("Acceptance - Save Dimensions" + DateTime.Now.ToString());

                            #region New Code added to Accept ULD Against AWB(20/01/2014)
                            if (Session["dsDimesionAll"] != null)
                            {
                                string AWBNum = awb;
                                string[] AWBDet = AWBNum.Split('-');
                                string FlightNum = FlightNo;
                                string FltDt = FlightDate;
                                if (AWBDet.Length > 0 && txtAccpPieces.Text != "" && txtAccpPieces.Text != "0")
                                {
                                    GenerateAWBDimensions(AWBDet[1], int.Parse(txtAccpPieces.Text), (DataSet)Session["dsDimesionAll"],
                                        Convert.ToDecimal(txtAccpWeight.Text), true, AWBDet[0], FlightNum, FltDt);
                                }

                            }

                            #endregion

                            //System.Diagnostics.Debug.WriteLine("Acceptance - Save Accpt Dimensions" + DateTime.Now.ToString());

                            for (int i = 0; i < PieceDetails.Tables[0].Rows.Count; i++)
                            {
                                if (PieceDetails.Tables[0].Rows[i]["PieceType"].ToString() == "ULD" && PieceDetails.Tables[0].Rows[i]["ULDNo"].ToString() != "")
                                {
                                    SaveULDNoinMaster(PieceDetails.Tables[0].Rows[i]["ULDNo"].ToString());
                                }

                            }
                        }
                    }
                }

                #endregion

                //System.Diagnostics.Debug.WriteLine("Acceptance - Auto Message Creation" + DateTime.Now.ToString());

                //Function for creates auto messages for AWB creation updation
                #region Auto Message Creation
                bool GenerateAutoMSG = false;
                //LoginBL objLoginBAL = new LoginBL();
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

                //System.Diagnostics.Debug.WriteLine("Acceptance - Update Acceptance" + DateTime.Now.ToString());

                #region "Update Acceptance"

                string strQuery = string.Empty;

                strQuery = "UPDATE dbo.OperationAWBDetails SET AcceptedPcs = BulkPcs, AcceptedWgt=BulkWt, AcceptedStatus = 'C' WHERE AWBNumber = '" + HidAWBNumber.Value + "' and AWBPrefix='" + txtAwbPrefix.Text.Trim() + "' and IsActive = 1;";
                strQuery = strQuery + " UPDATE dbo.OperationULDDetails SET AcceptedStatus = 'C' WHERE AWBNumber = '" + HidAWBNumber.Value + "' and AWBPrefix='" + txtAwbPrefix.Text.Trim() + "' and IsActive = 1;";
                strQuery = strQuery + " UPDATE dbo.AWBRouteMaster SET Accepted = 'Y', AcceptedPcs = Pcs, AcceptedWt = Wt WHERE AWBNumber = '" + HidAWBNumber.Value + "' and AWBPrefix='" + txtAwbPrefix.Text.Trim() + "';";
                strQuery = strQuery + " UPDATE dbo.AWBSummaryMaster SET IsAccepted = 1 WHERE AWBNumber = '" + HidAWBNumber.Value + "' and AWBPrefix='" + txtAwbPrefix.Text.Trim() + "';";

                SQLServer da = new SQLServer(Global.GetConnectionString());
                bool blnResult = false;

                blnResult = da.InsertData(strQuery);

                #endregion

                //System.Diagnostics.Debug.WriteLine("Acceptance - Acceptance Complete" + DateTime.Now.ToString());

                #region "Log Audit Trail"

                string strOrigin = string.Empty;
                string strDestination = string.Empty;
                string Pieces = string.Empty;
                string Weight = string.Empty;
                string FlightDt = string.Empty;
                MasterAuditBAL objBal = new MasterAuditBAL();

                for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                {
                    if(((TextBox)(grdRouting.Rows[intCount].FindControl("txtFlightID"))).Text.Trim() !="")
                        FlightNo = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFlightID"))).Text.Trim();
                    else
                        FlightNo = ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlFltNum"))).SelectedItem.Text.Trim();

                    FlightDt = ((DateControl)(grdRouting.Rows[intCount].FindControl("txtFdate"))).DateFormatDDMMYYYY;

                    strDestination = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltDest"))).Text.Trim();
                    strOrigin = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtFltOrig"))).Text.Trim();                    
                    Pieces = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtPcs"))).Text.Trim();
                    Weight = ((TextBox)(grdRouting.Rows[intCount].FindControl("txtWt"))).Text.Trim();

                    objBal.AddAWBAuditLog(txtAwbPrefix.Text.Trim(), HidAWBNumber.Value, ddlOrg.Text.Trim(), ddlDest.Text.Trim(), Pieces, Weight, FlightNo,
                        FlightDt, strOrigin, strDestination, "Accepted", "Accepted Pcs/ Wt updated", "", Convert.ToString(Session["UserName"]),
                        Convert.ToString(Session["IT"]), false);

                }

                #endregion

                Session["ShipAccp"] = true;
                Session["UpdtOn"] = Session["IT"];
            }
            catch (Exception ex)
            { return false; }

            return true;
        }

        protected bool CheckManualAWB(string TriggerPoint)
        {
            DataSet AWB = new DataSet("GHA_QuickBooking_137");
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
                            HidBookingError.Value = "Please enter valid AWB.";
                            lblStatus.Text = "Please enter valid AWB.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return false;
                        }
                    }
                    catch
                    {
                        HidBookingError.Value = "Please enter valid AWB.";
                        lblStatus.Text = "Please enter valid AWB.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        txtAWBNo.Focus();
                        return false;
                    }

                    AWBStock.SetValue(Convert.ToInt32(txtAWBNo.Text.Trim()), i);

                    i = i + 1;
                    AWBStock.SetValue(txtAwbPrefix.Text.Trim(), i);

                    i = i + 1;
                    AWBStock.SetValue(ddlOrg.SelectedItem.Text.Trim(), i);

                    AWB = objBLL.GetAWBStock(AWBStock);

                    if (AWB != null && AWB.Tables != null && AWB.Tables.Count > 1 && AWB.Tables[0].Rows.Count > 0)
                    {
                        if (TriggerPoint != "S" && AWB.Tables[1].Rows[0]["AgentName"].ToString() != "")  //Trigger point S-Save, L-List
                        {
                            txtAgentName.Text = AWB.Tables[1].Rows[0]["AgentName"].ToString();
                            TXTAgentCode.Text = AWB.Tables[1].Rows[0]["AgentCode"].ToString();
                        }
                        
                        //string AgentCode = Convert.ToString(Session["ACode"]);

                        //int RoleId = Convert.ToInt32(Session["RoleID"]);

                        if (Convert.ToBoolean(AWB.Tables[1].Rows[0]["Exists"])==false)
                        {
                            if (TXTAgentCode.Text.Trim() != "" && TXTAgentCode.Text.Trim().ToUpper() != AWB.Tables[1].Rows[0]["AgentCode"].ToString().Trim().ToUpper())
                            {
                                HidBookingError.Value = "AWB doesn't belongs to current user.";
                                lblStatus.Text = "AWB doesn't belongs to current user.";
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                                return false;
                            }
                        }

                        if (TriggerPoint == "S" && Convert.ToBoolean(AWB.Tables[1].Rows[0]["Exists"]))
                        {
                            //Response.Redirect("~/GHA_QuickBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim() + "&GHA=" + Convert.ToString(Session["GHABooking"]), false);
                            HidBookingError.Value = "AWB no already Exists.";
                            lblStatus.Text = "AWB no already Exists.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return false;
                        }
                        else if (TriggerPoint == "L" && Convert.ToBoolean(AWB.Tables[1].Rows[0]["Exists"]))
                        {
                            Response.Redirect("~/GHA_QuickBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim() + "&GHA=" + Convert.ToString(Session["GHABooking"]), false);
                        }

                        if (Convert.ToBoolean(AWB.Tables[1].Rows[0]["IsManual"]) == false)
                        {
                            HidBookingError.Value = "AWB entered is not Physical AWB.";
                            lblStatus.Text = "AWB entered is not Physical AWB.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return false;
                        }
                    }
                    else if (AWB != null && AWB.Tables != null && AWB.Tables.Count > 2 && AWB.Tables[2].Rows.Count > 0)
                    {
                        bool PartnerAWB = Convert.ToBoolean(AWB.Tables[2].Rows[0]["PartnerAWB"]);
                        if (Convert.ToBoolean(AWB.Tables[2].Rows[0]["Exists"]))
                        {
                            Session["GHABooking"] = true;
                            hdnBookingType.Value = "I";

                            if (TriggerPoint == "S")
                            {
                                HidBookingError.Value = "AWB no already Exists.";
                                lblStatus.Text = "AWB no already Exists.";
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                                return false;
                            }
                            else if (TriggerPoint == "L")
                            {   
                                Response.Redirect("~/GHA_QuickBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim() + "&GHA=" + Convert.ToString(Session["GHABooking"]), false);
                            }
                        }
                        else if (AWB != null && AWB.Tables != null && AWB.Tables.Count > 1 && AWB.Tables[0].Rows.Count == 0 && PartnerAWB == false)
                        {
                            HidBookingError.Value = "Please check the AWB stock.";
                            lblStatus.Text = "Please check the AWB stock.";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return false;
                        }
                        else
                        {
                            Session["GHABooking"] = true;
                            hdnBookingType.Value = "I";
                        }
                    }
                    else
                    {
                        HidBookingError.Value = "Please check the AWB stock.";
                        lblStatus.Text = "Please check the AWB stock.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        //pnlStockDetails.Visible = false;
                        //Response.Redirect("~/GHA_QuickBooking.aspx?command=Edit&AWBNumber=" + txtAWBNo.Text.Trim() + "&AWBPrefix=" + txtAwbPrefix.Text.Trim(), false);
                        return false;
                    }
                }
                else
                {
                    HidBookingError.Value = "Please enter valid AWB Number.";
                    lblStatus.Text = "Please enter valid AWB Number.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return false;
                }
            }
            catch (Exception ex)
            {
                AWB = null;
                HidBookingError.Value = "Error. Please try again";
                lblStatus.Text = "Error. Please try again";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            finally
            {
                if (AWB != null)
                    AWB.Dispose();
            }

            HidAWBNumber.Value = txtAWBNo.Text.Trim();
            lblStatus.Text = "";
            HidIsManual.Value = "1";

            return true;
        }

        protected void DisableControlsOnAccept()
        {
            ddlDest.Enabled = false;
            ddlOrg.Enabled = false;
            txtCommodityCode.Enabled = false;
            txtCommodityName.Enabled = false;
            TXTAgentCode.Enabled = false;
            txtAgentName.Enabled = false;
            //ddlProductType.Enabled = false;

            txtSpecialHandlingCode.Enabled = false;
            txtPieces.Enabled = false;
            txtGrossWt.Enabled = false;
            //txtShipmentDate.Enabled = false;
            //imgShipmentDate.Enabled = false;
            txtShipmentDate1.Enabled = false;

            TXTDvForCarriage.Enabled = false;
            btnSave.Enabled = false;
            grdRouting.Enabled = false;
            txtTotalAmount.Enabled = false;
            drpCurrency.Enabled = false;

            btnExecute.Enabled = false;
            btnSaveAndAccept.Enabled = false;
            btnSaveAcceptance.Enabled = false;
        }

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
                    DataTable dtRoute = new DataTable("GHA_QuickBooking_189");
                        dtRoute = (DataTable)Session["FltRoute"];
                    //Set values in dtRoute.                    
                    float FrtMkt = 0;
                    float Tax = 0;
                    float OCTax = 0;
                    float.TryParse(txtServiceTax.Text, out OCTax);

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
                    txtServiceTax.Text = Convert.ToString(Tax + OCTax);
                    txtFreightMKT.Text = FrtMkt.ToString();
                    txtRatePerKG.Text =
                        Convert.ToString(FrtMkt / float.Parse(txtChargeableWt.Text));
                    //Set total frieght.
                    float TotalOCDC = 0;
                    float.TryParse(txtOCDueCar.Text, out TotalOCDC);
                    float TotalOCDA = 0;
                    float.TryParse(txtOCDueAgent.Text, out TotalOCDA);
                    txtTotalAmount.Text =
                        Convert.ToString(Tax + OCTax + FrtMkt + TotalOCDC - TotalOCDA);
                    txtSubTotal.Text= Convert.ToString( FrtMkt + TotalOCDC - TotalOCDA);
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

        private bool GenerateAWBNumber()
        {

            // ----- Sumit 2014-10-30
            try
            {
                string AWBNumber = "", errormessage = "";

                if (Convert.ToBoolean(Session["GHABooking"]) == false)
                {
                    if (HidIsManual.Value != "1")
                    {
                        if (HidAWBNumber.Value.Trim() == "")
                        {
                            if (txtAwbPrefix.Text.Equals(Session["awbPrefix"].ToString(), StringComparison.OrdinalIgnoreCase) || (chkInterline.Checked == false))
                            {
                                string ShipmentType = string.Empty;
                                //LoginBL objLogin = new LoginBL();
                                //string strClientFlag = objLogin.GetMasterConfiguration("StockAllocationCebu").ToString();
                                string strClientFlag = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "StockAllocationCebu");
                                //objLogin = null;

                                BookingBAL objBooking = new BookingBAL();
                                DataSet objDS = new DataSet("GHA_QuickBooking_138");
                                    objDS = objBooking.GetShipmentType(ddlOrg.SelectedItem.ToString(), ddlDest.SelectedItem.ToString());
                                objBooking = null;

                                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                                    ShipmentType = objDS.Tables[0].Rows[0][0].ToString();

                                if (strClientFlag.ToUpper() == "TRUE" && ShipmentType != "")
                                {
                                    if (GetAutoAWBNumber(ref errormessage, ref AWBNumber, txtAwbPrefix.Text.Trim(), ddlDocType.Text.Trim(), ShipmentType))
                                    {
                                        HidAWBNumber.Value = AWBNumber;
                                    }
                                    else
                                    {
                                        HidBookingError.Value = "No AWB stock exists for the agent.";
                                        lblStatus.Text = "No AWB stock exists for the agent.";
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (GetAutoAWBNumber(ref errormessage, ref AWBNumber, txtAwbPrefix.Text.Trim(), ddlDocType.Text.Trim()))
                                    {
                                        HidAWBNumber.Value = AWBNumber;
                                    }
                                    else
                                    {
                                        HidBookingError.Value = "No AWB stock exists for the agent.";
                                        lblStatus.Text = "No AWB stock exists for the agent.";
                                        return false;
                                    }
                                }

                            }
                            else
                            {
                                if (txtAWBNo.Text.Length < 1 || txtAWBNo.Text == "")
                                {
                                    HidBookingError.Value = "Please enter valid AWB Number";
                                    lblStatus.Text = "Please enter valid AWB Number";
                                    txtAWBNo.Focus();
                                    return false;
                                }
                                else
                                {
                                    if (objBLL.CheckAWBExists(txtAwbPrefix.Text, txtAWBNo.Text))
                                    {
                                        HidBookingError.Value = "AWB Number already exists in the system. Please validate.";
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
                return true;
            }
            catch (Exception ex) { return false; }
        }

        private void ClearSession()
        {
            Session["FocusShipper"] = null;
            Session["Piecetype_DIM"] = null;
            Session["DGRAWB"] = null;
            Session["dsDimesionAll"] = null;
            Session["QB"] = null;
            Session["GHABooking"] = null;
            Session["HAWBDetails"] = null;
            Session["Message"] = null;
            Session["AWBULD"] = null;
            Session["BookingID"] = null;
            Session["OCDetails"] = null;
            Session["ConBooking_PayModeMaster"] = null;
            Session["OCTotal"] = null;
            Session["OATotal"] = null;
            Session["AWBStatus"] = null;
            //Session["DelExecAWB"] = null;
            Session["QB_PartnerCode"] = null;
            Session["CurrencyType"] = null;
            Session["QB_AGTCurrency"] = null;
            Session["dsPiecesDet"] = null;
            //Session["PartnerCode"] = null;
            Session["Message"] = null;
            Session["dtRates"] = null;
            Session["ConBooking_RouteType"] = null;
            Session["DTExport"] = null;
            Session["ShipAccp"] = null;
            Session["dsRoutDetails"] = null;
            Session["Flt"] = null;
            Session["dsSelectedFlights"] = null;
            Session["SingleRow"] = null;
            Session["QB_ScreenedPcs"] = null;
            Session["QB_ScreenedWt"] = null;
            Session["FltRoute"] = null;
            Session["dsRouteds"] = null;
            Session["fltCapacity"] = null;
            Session["fltVarDetails"] = null;
            Session["MAWBNo"] = null;
            Session["AWBForULDAssoc"] = null;
            Session["ePouchAWBNo"] = null;
            Session["CargoRec"] = null;
            Session["FltOrigin"] = null;
            Session["FltDestination"] = null;
            Session["FltDate"] = null;
            Session["ShowFltRowIndex"] = null;
            Session["Viability"] = null;
            Session["DgrCargo"] = null;
            Session["dsMaterialDetails"] = null;
            Session["QK_Rates"] = null;
            Session["TaxDetails"] = null;
            Session["HAWBDocument"] = null;
        }

        private string GetShipmentType(string Origin, string Destination)
        {
            BookingBAL objBooking = new BookingBAL();
            DataSet objDS = new DataSet("GHA_QuickBooking_138");
            string strShipmentType = "D";

            try
            {
                objDS = objBooking.GetShipmentType(Origin, Destination);

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    strShipmentType = Convert.ToString(objDS.Tables[0].Rows[0]["ShipmentType"]);
                }
            }
            catch
            {}
            finally
            {
                objBooking = null;
                if (objDS != null)
                    objDS.Dispose();
            }

            return strShipmentType;
        }

        private void SetAWBStatusOnRoute()
        {
            try
            {
                int RouteCount = grdRouting.Rows.Count;
                string ShipmentType = GetShipmentType(ddlOrg.SelectedItem.Text.Trim(), ddlDest.SelectedItem.Text.Trim());

                if (ShipmentType == "I" && RouteCount > 1)
                    ddlAWBStatus.SelectedValue = "Q";
                else
                    ddlAWBStatus.SelectedValue = "C";
            }
            catch { }
        }

        private void SetPaymentMode()
        {
            DropDownList ddl = ddlPaymentMode;
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

            string ShowAllPaymentModes = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowAllPaymentModes");

            if (ShowAllPaymentModes != "" && Convert.ToBoolean(ShowAllPaymentModes) == true)
                return;

            if (TXTAgentCode.Text.Length > 0)
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());

                DataSet dsPayMode = db.SelectRecords("spGetAgentPayMode", "AgentCode", TXTAgentCode.Text.ToString().Trim(), SqlDbType.VarChar);

                if (dsPayMode != null)
                {
                    if (dsPayMode.Tables.Count > 0)
                    {
                        if (dsPayMode.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = dsPayMode.Tables[0];
                            Session["ConBooking_PayModeMaster"] = dsPayMode.Tables[0];
                            ddl.Items.Clear();
                            ddl.DataSource = dt;
                            ddl.DataTextField = "PayModeText";
                            ddl.DataValueField = "PayModeCode";
                            ddl.DataBind();
                            ddl.SelectedValue = "Select";
                        }
                    }
                }
            }

        }

        public DataTable GetFlightDataSet()
        {
            DataTable dsResult = new DataTable("GHA_QuickBooking_GetFlightDataSet");
            try
            {                
                dsResult.Columns.Add("Origin");
                dsResult.Columns.Add("Destination");
                dsResult.Columns.Add("Pieces");
                dsResult.Columns.Add("Gwt");
                dsResult.Columns.Add("CWt");
            }
            catch { }
            return dsResult;
        }
                
        private void AddEventtoGrid()
        {
            DateControl dt = null;
            for (int intcount = 0; intcount < grdRouting.Rows.Count; intcount++)
            {
                dt = (DateControl)grdRouting.Rows[intcount].FindControl("txtFdate");
                dt.TextChange += new EventHandler(txtFdate_TextChanged);                
            }
        }
    }
}