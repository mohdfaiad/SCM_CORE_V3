using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using QID.DataAccess;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace ProjectSmartCargoManager
{
    public partial class frmCustomsPopup : System.Web.UI.Page
    {

        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        CustomsImportBAL objCustoms = new CustomsImportBAL();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    LoadLocationDropdown(ddlDeptAirport);
                    //LoadFlightDesignatorDropdown(ddlFightDesignator);
                    LoadingEntryCodeDropDown();
                    LoadingAmendmentCodeDropDown();
                    LoadingErrorCodeDropDown();
                    LoadingStatusCodeDropDown();
                    LoadingActionCodeDropDown();
                    LoadingCountryCodeDropDown();
                    LoadingCurrencyDropDown();
                    txtMsgType.Enabled = false;
                    txtContLocation.Enabled = false;

                    if (Request.QueryString["Mode"] != null)
                    {
                        string Mode = Request.QueryString["Mode"].ToString();
                        hdFSNMsgType.Value = Mode;
                        if (Mode == "FRI")
                        {
                            if (Request.QueryString["AWBNo"] != null)
                            {
                                if (Request.QueryString["FlightNo"] != null && Request.QueryString["FlightDate"] != null)
                                {
                                    string[] AWBDetails = Request.QueryString["AWBNo"].ToString().Split('-');
                                    CustomsImportBAL objCustoms = new CustomsImportBAL();
                                    object[] QueryValues = new object[4];
                                    QueryValues[0] = Request.QueryString["AWBNo"].ToString().Replace("-", "");
                                    QueryValues[1] = 1;
                                    QueryValues[2] = Request.QueryString["FlightNo"].ToString();
                                    QueryValues[3] = Request.QueryString["FlightDate"].ToString();

                                    objCustoms.UpdateFRIMessage(QueryValues);
                                    txtPrefix.Text = AWBDetails[0];
                                    txtAirWayBillNo.Text = AWBDetails[1];
                                    ddlFightDesignator.Text = Request.QueryString["FlightNo"].ToString().Substring(0, 2);
                                    txtFlightID.Text = Request.QueryString["FlightNo"].ToString().Substring(2, Request.QueryString["FlightNo"].ToString().Length - 2);
                                    txtFlightDate.Text = Request.QueryString["FlightDate"].ToString();
                                    BtnList_Click(sender, e);
                                    ((RadioButton)GRDBooking.Rows[0].FindControl("rdbCheck")).Checked = true;
                                    btnFetch_Click(sender, e);
                                    for (int i = 0; i < grdMessages.Rows.Count; i++)
                                    {
                                        if (((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).Text == Mode)
                                        {
                                            CommandEventArgs CommandEvtArgs = new CommandEventArgs(((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).CommandName, ((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).CommandArgument);
                                            GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(grdMessages.Rows[i], grdMessages, CommandEvtArgs);
                                            grdMessages_RowCommand(sender, eventArgs);
                                            btnSendMessage_Click(sender, e);
                                            break;
                                            //grdMessages_RowCommand(sender//((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).co
                                        }
                                    }
                                }
                            }

                        }
                        else
                            if (Mode == "FDM")
                            {
                                if (Request.QueryString["AWBNo"] != null)
                                {
                                    string[] AWBDetails = Request.QueryString["AWBNo"].ToString().Split('-');
                                    if (Request.QueryString["FlightNo"] != null && Request.QueryString["FlightDate"] != null)
                                    {
                                        string FlightNo = Request.QueryString["FlightNo"].ToString();
                                        DateTime dtFltDate = DateTime.ParseExact(Request.QueryString["FlightDate"].ToString(), "dd/MM/yyyy", null);
                                        object[] QueryValues = new object[7];

                                        QueryValues[0] = AWBDetails[1];
                                        if (FlightNo != "")
                                        {
                                            QueryValues[1] = FlightNo.Substring(0, 2);
                                            QueryValues[2] = FlightNo.Substring(2, FlightNo.Length - 2);
                                        }
                                        else
                                        {
                                            QueryValues[1] = "";
                                            QueryValues[2] = "";
                                        }
                                        QueryValues[3] = (DateTime)Session["IT"];
                                        QueryValues[4] = Session["UserName"].ToString();
                                        QueryValues[5] = AWBDetails[0];
                                        QueryValues[6] = dtFltDate;
                                        CustomsImportBAL objCustoms = new CustomsImportBAL();
                                        objCustoms.UpdateFDMMessage(QueryValues);
                                        txtPrefix.Text = AWBDetails[0];
                                        txtAirWayBillNo.Text = AWBDetails[1];
                                        ddlFightDesignator.Text = FlightNo.Substring(0, 2);
                                        txtFlightID.Text = FlightNo.Substring(2, FlightNo.Length - 2);
                                        txtFlightDate.Text = dtFltDate.ToString("dd/MM/yyyy");
                                        BtnList_Click(sender, e);
                                        ((RadioButton)GRDBooking.Rows[0].FindControl("rdbCheck")).Checked = true;
                                        btnFetch_Click(sender, e);
                                        for (int i = 0; i < grdMessages.Rows.Count; i++)
                                        {
                                            if (((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).Text == Mode)
                                            {
                                                CommandEventArgs CommandEvtArgs = new CommandEventArgs(((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).CommandName, ((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).CommandArgument);
                                                GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(grdMessages.Rows[i], grdMessages, CommandEvtArgs);
                                                grdMessages_RowCommand(sender, eventArgs);
                                                btnSendMessage_Click(sender, e);
                                                break;
                                                //grdMessages_RowCommand(sender//((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).co
                                            }
                                        }
                                    }
                                }
 
                            }
                            else
                                if (Mode == "FSN")
                                {
                                    if (Request.QueryString["AWBNo"] != null)
                                    {
                                        string[] AWBDetails = Request.QueryString["AWBNo"].ToString().Split('-');
                                        if (Request.QueryString["FlightNo"] != null && Request.QueryString["FlightDate"] != null)
                                        {
                                            string FlightNo = Request.QueryString["FlightNo"].ToString();
                                            DateTime dtFltDate = DateTime.ParseExact(Request.QueryString["FlightDate"].ToString(), "dd/MM/yyyy", null);
                                            object[] QueryValues = new object[7];

                                            QueryValues[0] = AWBDetails[1];
                                            if (FlightNo != "")
                                            {
                                                QueryValues[1] = FlightNo.Substring(0, 2);
                                                QueryValues[2] = FlightNo.Substring(2, FlightNo.Length - 2);
                                            }
                                            else
                                            {
                                                QueryValues[1] = "";
                                                QueryValues[2] = "";
                                            }
                                            QueryValues[3] = (DateTime)Session["IT"];
                                            QueryValues[4] = Session["UserName"].ToString();
                                            QueryValues[5] = AWBDetails[0];
                                            QueryValues[6] = dtFltDate;
                                            CustomsImportBAL objCustoms = new CustomsImportBAL();
                                            //objCustoms.UpdateFDMMessage(QueryValues);
                                            txtPrefix.Text = AWBDetails[0];
                                            txtAirWayBillNo.Text = AWBDetails[1];
                                            ddlFightDesignator.Text = FlightNo.Substring(0, 2);
                                            txtFlightID.Text = FlightNo.Substring(2, FlightNo.Length - 2);
                                            txtFlightDate.Text = dtFltDate.ToString("dd/MM/yyyy");
                                            BtnList_Click(sender, e);
                                            ((RadioButton)GRDBooking.Rows[0].FindControl("rdbCheck")).Checked = true;
                                            btnFetch_Click(sender, e);
                                            int count = 0;
                                            for (int i = 0; i < grdMessages.Rows.Count; i++)
                                            {
                                                if (((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).Text == Mode)
                                                {
                                                    count++;
                                                    CommandEventArgs CommandEvtArgs = new CommandEventArgs(((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).CommandName, ((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).CommandArgument);
                                                    GridViewCommandEventArgs eventArgs = new GridViewCommandEventArgs(grdMessages.Rows[i], grdMessages, CommandEvtArgs);
                                                    grdMessages_RowCommand(sender, eventArgs);
                                                    btnSendMessage_Click(sender, e);
                                                    break;
                                                    //grdMessages_RowCommand(sender//((LinkButton)grdMessages.Rows[i].FindControl("lnkMessageType")).co
                                                }
                                            }
                                            if (count == 0)
                                            {
                                                grdMessages.DataSource = null;
                                                grdMessages.DataBind();
                                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                btnSendMessage_Click(sender, e);
                                            }

                                        }
                                    }

                                }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Freight Inbound Click
        protected void lnkFreightInbound_Click(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAMSDetails();</SCRIPT>", false);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                //txtMsgType.Text = "FRI";


            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading DropDowns

        #region Loading Location Codes

        public void LoadLocationDropdown(DropDownList ddlLoc)
        {
            try
            {
                DataSet ds = db.SelectRecords("SP_GetAllStations");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlLoc.DataSource = ds;
                            ddlLoc.DataTextField = "Code";
                            ddlLoc.DataValueField = "Code";
                            ddlLoc.DataBind();
                            ddlLoc.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Flight Designator Codes
        public void LoadFlightDesignatorDropdown(DropDownList ddlFlight)
        {
            try
            {
                DataSet ds = db.SelectRecords("SP_GetFlightDesignators");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlFlight.DataSource = ds;
                            ddlFlight.DataTextField = "Code";
                            ddlFlight.DataValueField = "Code";
                            ddlFlight.DataBind();
                            ddlFlight.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Entry Codes
        public void LoadingEntryCodeDropDown()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = db.SelectRecords("Sp_GetEntryCodes");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlEntryType.DataSource = ds;
                            ddlEntryType.DataTextField = "Description";
                            ddlEntryType.DataValueField = "EntryCode";
                            ddlEntryType.DataBind();
                            ddlCBPEntryCodes.DataSource = ds;
                            ddlCBPEntryCodes.DataTextField = "Description";
                            ddlCBPEntryCodes.DataValueField = "EntryCode";
                            ddlCBPEntryCodes.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Amendment Codes
        public void LoadingAmendmentCodeDropDown()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = db.SelectRecords("Sp_GetAmendmentCodes");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlAmendmentCode.DataSource = ds;
                            ddlAmendmentCode.DataTextField = "Description";
                            ddlAmendmentCode.DataValueField = "AmendmentCode";
                            ddlAmendmentCode.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Country Codes
        public void LoadingCountryCodeDropDown()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = db.SelectRecords("Sp_GetCustomsCountryCode");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlCountryCode.DataSource = ds;
                            ddlCountryCode.DataTextField = "CountryName";
                            ddlCountryCode.DataValueField = "CountryCode";
                            ddlCountryCode.DataBind();
                            ddlCountryCode.Items.Insert(0, "Select");

                            ddlConCountryCode.DataSource = ds;
                            ddlConCountryCode.DataTextField = "CountryName";
                            ddlConCountryCode.DataValueField = "CountryCode";
                            ddlConCountryCode.DataBind();
                            ddlConCountryCode.Items.Insert(0, "Select");

                            ddlDescriptionCountryCode.DataSource = ds;
                            ddlDescriptionCountryCode.DataTextField = "CountryName";
                            ddlDescriptionCountryCode.DataValueField = "CountryCode";
                            ddlDescriptionCountryCode.DataBind();
                            ddlDescriptionCountryCode.Items.Insert(0, "Select");
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Action Codes

        public void LoadingActionCodeDropDown()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = db.SelectRecords("Sp_GetActionCodes");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlActionCode.DataSource = ds;
                            ddlActionCode.DataTextField = "StatusActionCode";
                            ddlActionCode.DataValueField = "Description";
                            ddlActionCode.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Error Codes

        public void LoadingErrorCodeDropDown()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = db.SelectRecords("Sp_GetErrorCodes");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlErrorCode.DataSource = ds;
                            ddlErrorCode.DataTextField = "ErrorMessage";
                            ddlErrorCode.DataValueField = "ErrorCode";
                            ddlErrorCode.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Status Codes
        public void LoadingStatusCodeDropDown()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = db.SelectRecords("Sp_GetStatusCodes");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlStatusRequestCode.DataSource = ds.Tables[0];
                            ddlStatusRequestCode.DataTextField = "Description";
                            ddlStatusRequestCode.DataValueField = "Code";
                            ddlStatusRequestCode.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ddlStatusAnswerCode.DataSource = ds.Tables[1];
                            ddlStatusAnswerCode.DataTextField = "Description";
                            ddlStatusAnswerCode.DataValueField = "Code";
                            ddlStatusAnswerCode.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            ddlStatusCode.DataSource = ds.Tables[2];
                            ddlStatusCode.DataTextField = "Description";
                            ddlStatusCode.DataValueField = "Code";
                            ddlStatusCode.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Loading Currency Codes

        public void LoadingCurrencyDropDown()
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = db.SelectRecords("SP_GETCustomsCurrencyCode");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlCurrencyCode.DataSource = ds.Tables[0];
                            ddlCurrencyCode.DataTextField = "CurrencyName";
                            ddlCurrencyCode.DataValueField = "CurrencyCode";
                            ddlCurrencyCode.DataBind();
                            //ddlEntryType.Items.Insert(0, "Select");
                        }

                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #endregion

        #region Button List
        protected void BtnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                GRDBooking.DataSource = null;
                GRDBooking.DataBind();
                Session["dsListAWBCustoms"] = null;
                grdMessages.DataSource = null;
                grdMessages.DataBind();
                Session["AWBCustomMessages"] = null;
                string strtodate = string.Empty;
                DateTime dt;

                if (txtFlightDate.Text != "")
                {
                    try
                    {

                        string day = txtFlightDate.Text.Substring(0, 2);
                        string mon = txtFlightDate.Text.Substring(3, 2);
                        string yr = txtFlightDate.Text.Substring(6, 4);
                        strtodate = yr + "-" + mon + "-" + day;
                        dt = Convert.ToDateTime(strtodate);
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Flight Date format invalid!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }

                //string[] QueryNames = new string[6];
                object[] QueryValues = new object[6];
                //SqlDbType[] QueryTypes = new SqlDbType[6];

                //QueryNames[0] = "AWBPrefix";
                //QueryNames[1] = "AWBNumber";
                //QueryNames[2] = "FlightNumber";
                //QueryNames[3] = "FlightDate";
                //QueryNames[4] = "ULD";
                //QueryNames[5] = "DestAirport";

                QueryValues[0] = txtPrefix.Text.Trim();
                QueryValues[1] = txtAirWayBillNo.Text.Trim();
                //if (ddlFightDesignator.SelectedIndex != 0)
                //{
                //    QueryValues[2] = ddlFightDesignator.SelectedItem.Text.Trim() + txtFlightID.Text.Trim();
                //}
                //else
                //    QueryValues[2] = "" + txtFlightID.Text.Trim();
                if (ddlFightDesignator.Text != "")
                {
                    QueryValues[2] = ddlFightDesignator.Text.Trim() + txtFlightID.Text.Trim();
                }
                else
                    QueryValues[2] = "" + txtFlightID.Text.Trim();

                //QueryValues[2] = ddlFightDesignator.SelectedItem.Text.Trim() + txtFlightID.Text.Trim();
                QueryValues[3] = txtFlightDate.Text.Trim();
                QueryValues[4] = txtULD.Text.Trim();
                if (ddlDeptAirport.SelectedIndex != 0)
                {
                    QueryValues[5] = ddlDeptAirport.SelectedItem.Text.Trim();
                }
                else
                    QueryValues[5] = "";

                //QueryTypes[0] = SqlDbType.VarChar;
                //QueryTypes[1] = SqlDbType.VarChar;
                //QueryTypes[2] = SqlDbType.VarChar;
                //QueryTypes[3] = SqlDbType.VarChar;
                //QueryTypes[4] = SqlDbType.VarChar;
                //QueryTypes[5] = SqlDbType.VarChar;
                btnFetch.Visible = false;
                btnAudit.Visible = false;
                DataSet ds = objCustoms.GetCustomsAWBList(QueryValues);
                //DataSet ds = db.SelectRecords("SP_GetCustomsAWBList", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GRDBooking.DataSource = ds;
                            GRDBooking.DataBind();
                            Session["dsListAWBCustoms"] = ds;
                            btnFetch.Visible = true;
                            btnAudit.Visible = true;

                        }
                        else
                        {
                            lblStatus.Text = "No Records Found !";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                    else
                    {
                        lblStatus.Text = "No Records Found !";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }

                }
                else
                {
                    lblStatus.Text = "No Records Found !";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Clear
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/CustomsImports.aspx");
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Fetch
        protected void btnFetch_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                grdMessages.DataSource = null;
                grdMessages.DataBind();
                Session["AWBCustomMessages"] = null;
                foreach (GridViewRow row in GRDBooking.Rows)
                {
                    if (((RadioButton)row.FindControl("rdbCheck")).Checked)
                    {
                        //string[] QueryNames = new string[2];
                        object[] QueryValues = new object[4];
                        //SqlDbType[] QueryTypes = new SqlDbType[2];

                        //QueryNames[0] = "AWBPrefix";
                        //QueryNames[1] = "AWBNumber";

                        QueryValues[0] = ((Label)row.FindControl("AWBPrefix")).Text.Trim();
                        QueryValues[1] = ((Label)row.FindControl("AWBNo")).Text.Trim();
                        QueryValues[1] = QueryValues[1].ToString().Substring(QueryValues[1].ToString().IndexOf('-') + 1, 8);
                        QueryValues[2] = ((Label)row.FindControl("FlightNo")).Text.Trim();
                        QueryValues[3] = ((Label)row.FindControl("FlightDate")).Text.Trim();
                        hdFlightNo.Value = ((Label)row.FindControl("FlightNo")).Text.Trim();
                        hdFlightDate.Value = ((Label)row.FindControl("FlightDate")).Text.Trim();
                        //QueryTypes[0] = SqlDbType.VarChar;
                        //QueryTypes[0] = SqlDbType.VarChar;
                        ddlHAWB.Enabled = true;
                        DataSet ds = objCustoms.FetchCustomsAWBDetails(QueryValues);
                        //DataSet ds = db.SelectRecords("SP_GetCustomsMessagingData", QueryNames, QueryValues, QueryTypes);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    Session["AWBCustomMessages"] = ds.Tables[2];
                                    grdMessages.DataSource = ds.Tables[2];
                                    grdMessages.DataBind();
                                    GRDBooking.DataSource = null;
                                    GRDBooking.DataBind();
                                    Session["dsListAWBCustoms"] = null;
                                    btnFetch.Visible = false;
                                    btnAudit.Visible = false;
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        #region Populating Message Details
                                        txtContLocation.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        txtawbprefix.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                                        txtawbserialnumber.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                                        //string[] HAWB = ds.Tables[0].Rows[0]["HAWB"].ToString().Split('';
                                        string TotalHAWB = string.Empty;
                                        foreach (DataRow dr in ds.Tables[1].Rows)
                                        {
                                            TotalHAWB = dr["HAWBNumber"].ToString() + ",";
                                        }
                                        char[] charSeparaters = new char[] { ',' };
                                        string[] HAWB = TotalHAWB.Split(charSeparaters, StringSplitOptions.RemoveEmptyEntries);
                                        ddlHAWB.DataSource = HAWB;
                                        ddlHAWB.DataBind();
                                        if (ddlHAWB.Items.Count < 1)
                                        {
                                            ddlHAWB.Enabled = false;
                                        }
                                        TxtOrigin.Text = ds.Tables[0].Rows[0]["OriginAirport"].ToString();
                                        //txtDestination.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        txtShipmentDescriptionCode.Text = ds.Tables[0].Rows[0]["ShipmentDescCode"].ToString();
                                        txtPieces.Text = ds.Tables[0].Rows[0]["Pieces"].ToString();
                                        ddlWBLWeightCode.Text = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        txtWeight.Text = ds.Tables[0].Rows[0]["Weight"].ToString();
                                        txtDateofArrival.Text = ds.Tables[0].Rows[0]["ArrivalDate"].ToString();
                                        txtImportingCarrier.Text = ds.Tables[0].Rows[0]["CarrierCode"].ToString();
                                        txtFlightNumber.Text = ds.Tables[0].Rows[0]["FlightNumber"].ToString();
                                        txtScheduledArrDate.Text = ds.Tables[0].Rows[0]["ArrivalDate"].ToString();
                                        txtShipperName.Text = ds.Tables[0].Rows[0]["ShipperName"].ToString();
                                        txtShipperAddress.Text = ds.Tables[0].Rows[0]["ShipperAddress"].ToString();
                                        txtCity.Text = ds.Tables[0].Rows[0]["ShipperCity"].ToString();
                                        txtState.Text = ds.Tables[0].Rows[0]["ShipperState"].ToString();
                                        ddlCountryCode.Text = ds.Tables[0].Rows[0]["ShipperCountry"].ToString();
                                        txtPostalCode.Text = ds.Tables[0].Rows[0]["ShipperPinCode"].ToString();
                                        txtConsigneeName.Text = ds.Tables[0].Rows[0]["ConsigneeName"].ToString();
                                        txtConCity.Text = ds.Tables[0].Rows[0]["ConsigneeCity"].ToString();
                                        txtConState.Text = ds.Tables[0].Rows[0]["ConsigneeState"].ToString();
                                        ddlConCountryCode.Text = ds.Tables[0].Rows[0]["ConsigneeCountry"].ToString();
                                        txtConPostalCode.Text = ds.Tables[0].Rows[0]["ConsigneePinCode"].ToString();
                                        txtConStreetAddress.Text = ds.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
                                        txtTelephoneNo.Text = ds.Tables[0].Rows[0]["ConsigneeTelephone"].ToString();
                                        txtTransferDestArpt.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        //txtDomInt.Text = ds.Tables[0].Rows[0]["DomIdentifier"].ToString();
                                        txtErrorMessage.Text = ddlErrorCode.SelectedItem.Value;
                                        ddlDescriptionCountryCode.Text = ds.Tables[0].Rows[0]["CountryCode"].ToString();
                                        txtDeclaredValue.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                                        ddlCurrencyCode.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtCommodityCode.Text = ds.Tables[0].Rows[0]["CommodityCode"].ToString();
                                        txtDestination.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        ddlArrWeightCode.Text = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        txtArrWeight.Text = ds.Tables[0].Rows[0]["BoardedWeight"].ToString();
                                        txtBoardedPieceCount.Text = ds.Tables[0].Rows[0]["BoardedPcs"].ToString();
                                        txtBoardedQtyIdentifier.Text = ds.Tables[0].Rows[0]["BoardedQuantityIdentifier"].ToString();
                                        txtLineIdentifier.Text = "FDA";
                                        txtCargoDescription.Text = ds.Tables[0].Rows[0]["CommodityCode"].ToString();
                                        txtDepDetailsFlightNumber.Text = ds.Tables[0].Rows[0]["FlightNumber"].ToString();
                                        txtDepDetailsImportingCarrier.Text = ds.Tables[0].Rows[0]["CarrierCode"].ToString();
                                        txtDateOfScheduledArrival.Text = ds.Tables[0].Rows[0]["ArrivalDate"].ToString();
                                        txtActualFlightNumber.Text = ds.Tables[0].Rows[0]["FlightNumber"].ToString();
                                        txtActualImportingCarrier.Text = ds.Tables[0].Rows[0]["CarrierCode"].ToString();
                                        GRDBooking.DataSource = null;
                                        GRDBooking.DataBind();
                                        Session["dsListAWBCustoms"] = null;
                                        btnFetch.Visible = false;
                                        btnAudit.Visible = false;
                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        #region Populating Message Details
                                        txtContLocation.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        txtawbprefix.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                                        txtawbserialnumber.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                                        //string[] HAWB = ds.Tables[0].Rows[0]["HAWB"].ToString().Split('';
                                        string TotalHAWB = string.Empty;
                                        foreach (DataRow dr in ds.Tables[1].Rows)
                                        {
                                            TotalHAWB = dr["HAWBNumber"].ToString() + ",";
                                        }
                                        char[] charSeparaters = new char[] { ',' };
                                        string[] HAWB = TotalHAWB.Split(charSeparaters, StringSplitOptions.RemoveEmptyEntries);
                                        ddlHAWB.DataSource = HAWB;
                                        ddlHAWB.DataBind();
                                        if (ddlHAWB.Items.Count < 1)
                                        {
                                            ddlHAWB.Enabled = false;
                                        }
                                        TxtOrigin.Text = ds.Tables[0].Rows[0]["OriginAirport"].ToString();
                                        //txtDestination.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        txtShipmentDescriptionCode.Text = ds.Tables[0].Rows[0]["ShipmentDescCode"].ToString();
                                        txtPieces.Text = ds.Tables[0].Rows[0]["Pieces"].ToString();
                                        ddlWBLWeightCode.Text = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        txtWeight.Text = ds.Tables[0].Rows[0]["Weight"].ToString();
                                        txtDateofArrival.Text = ds.Tables[0].Rows[0]["ArrivalDate"].ToString();
                                        txtImportingCarrier.Text = ds.Tables[0].Rows[0]["CarrierCode"].ToString();
                                        txtFlightNumber.Text = ds.Tables[0].Rows[0]["FlightNumber"].ToString();
                                        txtScheduledArrDate.Text = ds.Tables[0].Rows[0]["ArrivalDate"].ToString();
                                        txtShipperName.Text = ds.Tables[0].Rows[0]["ShipperName"].ToString();
                                        txtShipperAddress.Text = ds.Tables[0].Rows[0]["ShipperAddress"].ToString();
                                        txtCity.Text = ds.Tables[0].Rows[0]["ShipperCity"].ToString();
                                        txtState.Text = ds.Tables[0].Rows[0]["ShipperState"].ToString();
                                        ddlCountryCode.Text = ds.Tables[0].Rows[0]["ShipperCountry"].ToString();
                                        txtPostalCode.Text = ds.Tables[0].Rows[0]["ShipperPinCode"].ToString();
                                        txtConsigneeName.Text = ds.Tables[0].Rows[0]["ConsigneeName"].ToString();
                                        txtConCity.Text = ds.Tables[0].Rows[0]["ConsigneeCity"].ToString();
                                        txtConState.Text = ds.Tables[0].Rows[0]["ConsigneeState"].ToString();
                                        ddlConCountryCode.Text = ds.Tables[0].Rows[0]["ConsigneeCountry"].ToString();
                                        txtConPostalCode.Text = ds.Tables[0].Rows[0]["ConsigneePinCode"].ToString();
                                        txtConStreetAddress.Text = ds.Tables[0].Rows[0]["ConsigneeAddress"].ToString();
                                        txtTelephoneNo.Text = ds.Tables[0].Rows[0]["ConsigneeTelephone"].ToString();
                                        txtTransferDestArpt.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        //txtDomInt.Text = ds.Tables[0].Rows[0]["DomIdentifier"].ToString();
                                        txtErrorMessage.Text = ddlErrorCode.SelectedItem.Value;
                                        ddlDescriptionCountryCode.Text = ds.Tables[0].Rows[0]["CountryCode"].ToString();
                                        txtDeclaredValue.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                                        ddlCurrencyCode.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtCommodityCode.Text = ds.Tables[0].Rows[0]["CommodityCode"].ToString();
                                        txtDestination.Text = ds.Tables[0].Rows[0]["ArrivalAirport"].ToString();
                                        ddlArrWeightCode.Text = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        txtArrWeight.Text = ds.Tables[0].Rows[0]["BoardedWeight"].ToString();
                                        txtBoardedPieceCount.Text = ds.Tables[0].Rows[0]["BoardedPcs"].ToString();
                                        txtBoardedQtyIdentifier.Text = ds.Tables[0].Rows[0]["BoardedQuantityIdentifier"].ToString();
                                        txtLineIdentifier.Text = "FDA";
                                        txtCargoDescription.Text = ds.Tables[0].Rows[0]["CommodityCode"].ToString();
                                        txtDepDetailsFlightNumber.Text = ds.Tables[0].Rows[0]["FlightNumber"].ToString();
                                        txtDepDetailsImportingCarrier.Text = ds.Tables[0].Rows[0]["CarrierCode"].ToString();
                                        txtDateOfScheduledArrival.Text = ds.Tables[0].Rows[0]["ArrivalDate"].ToString();
                                        txtActualFlightNumber.Text = ds.Tables[0].Rows[0]["FlightNumber"].ToString();
                                        txtActualImportingCarrier.Text = ds.Tables[0].Rows[0]["CarrierCode"].ToString();
                                        GRDBooking.DataSource = null;
                                        GRDBooking.DataBind();
                                        Session["dsListAWBCustoms"] = null;
                                        btnFetch.Visible = false;
                                        btnAudit.Visible = false;
                                        #endregion

                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAMSDetails();</SCRIPT>", false);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Gridview PageIndexing
        protected void GRDBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsResult = (DataSet)Session["dsListAWBCustoms"];

            GRDBooking.PageIndex = e.NewPageIndex;
            GRDBooking.DataSource = dsResult.Copy();
            GRDBooking.DataBind();

        }
        #endregion

        #region Button Update
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                lblReasonStatus.Text = "";
                lblShipperStatus.Text = "";
                lblConsigneeStatus.Text = "";
                lblDepartureStatus.Text = "";
                lblErrorMessageStatus.Text = "";
                lblFDAStatus.Text = "";
                lblHLDStatus.Text = "";
                lblStatusCarrierStatus.Text = "";
                lblAWBStatus.Text = "";
                lblWBLStatus.Text = "";
                lblArrivalStatus.Text = "";
                lblAgentStatus.Text = "";
                lblCEDStatus.Text = "";
                lblDescriptionStatus.Text = "";
                lblTextStatus.Text = "";
                lblStatusCBPStatus.Text = "";
                lblTransferStatus.Text = "";
                lblStatusQueryStatus.Text = "";
                lblStatusConditionStatus.Text = "";

                #region Validate
                //Getting Message Format

                ////Validating Mandatory fields for FRI Messages
                //if (hdMessageType.Value.Trim() == "FRI")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || txtWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                //    {
                //        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                //        lblWBLStatus.ForeColor = Color.Red;

                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }
                //    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.Text.Trim() == "")
                //    {
                //        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                //        lblShipperStatus.ForeColor = Color.Red;
                //    }

                //    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.Text.Trim() == "")
                //    {
                //        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                //        lblConsigneeStatus.ForeColor = Color.Red;
                //    }
                //    if (txtLineIdentifier.Text.Trim()=="")
                //    {
                //        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                //        lblFDAStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 

                //}

                ////Validating Mandatory fields for FXI Messages
                //if (hdMessageType.Value.Trim() == "FXI")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || txtWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                //    {
                //        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                //        lblWBLStatus.ForeColor = Color.Red;
                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }
                //    if ((ddlEntryType.SelectedItem.Value == "11" || ddlEntryType.SelectedItem.Value == "86" || ddlEntryType.SelectedItem.Value == "92") && TxtEntryNo.Text.Trim() == "")
                //    {
                //        if (ddlEntryType.Text.Trim() == "")
                //        {
                //            lblCEDStatus.Text = "Please Enter CBP Entry Detail(CED) Mandatory fields marked with (*)!";
                //            lblCEDStatus.ForeColor = Color.Red;
                //        }
                //    }
                //    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.Text.Trim() == "")
                //    {
                //        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                //        lblShipperStatus.ForeColor = Color.Red;
                //    }

                //    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.Text.Trim() == "")
                //    {
                //        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                //        lblConsigneeStatus.ForeColor = Color.Red;
                //    }
                //    if (txtLineIdentifier.Text.Trim() == "")
                //    {
                //        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                //        lblFDAStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 
                //    return;
                //}

                ////Validating Mandatory fields for FRC Messages
                //if (hdMessageType.Value.Trim() == "FRC")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || txtWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                //    {
                //        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                //        lblWBLStatus.ForeColor = Color.Red;
                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.Text.Trim() == "")
                //    {
                //        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                //        lblShipperStatus.ForeColor = Color.Red;
                //    }

                //    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.Text.Trim() == "")
                //    {
                //        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                //        lblConsigneeStatus.ForeColor = Color.Red;
                //    }
                //    if (txtLineIdentifier.Text.Trim() == "")
                //    {
                //        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                //        lblFDAStatus.ForeColor = Color.Red;
                //    }
                //    if (ddlAmendmentCode.Text.Trim() == "")
                //    {
                //        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                //        lblReasonStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                //    return;
                //}
                ////Validating Mandatory fields for FXC Messages
                //if (hdMessageType.Value.Trim() == "FXC")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || txtWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                //    {
                //        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                //        lblWBLStatus.ForeColor = Color.Red;
                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.Text.Trim() == "")
                //    {
                //        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                //        lblShipperStatus.ForeColor = Color.Red;
                //    }

                //    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.Text.Trim() == "")
                //    {
                //        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                //        lblConsigneeStatus.ForeColor = Color.Red;
                //    }
                //    if (txtLineIdentifier.Text.Trim() == "")
                //    {
                //        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                //        lblFDAStatus.ForeColor = Color.Red;
                //    }
                //    if (ddlAmendmentCode.Text.Trim() == "")
                //    {
                //        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                //        lblReasonStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                //    return;
                //}

                ////Validating Mandatory fields for FRX Messages
                //if (hdMessageType.Value.Trim() == "FRX")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (ddlAmendmentCode.Text.Trim() == "")
                //    {
                //        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                //        lblReasonStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                //    return;                    
                //}
                ////Validating Mandatory fields for FRH Messages
                //if (hdMessageType.Value.Trim() == "FRH")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }

                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (ddlHoldType.Text.Trim() == "")
                //    {
                //        lblHLDStatus.Text = "Please Enter HLD Mandatory fields marked with (*)!";
                //        lblHLDStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                //    return;

                //}
                ////Validating Mandatory fields for FXX Messages
                //if (hdMessageType.Value.Trim() == "FXX")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (ddlAmendmentCode.Text.Trim() == "")
                //    {
                //        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                //        lblReasonStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 

                //    return;
                //}

                ////Validating Mandatory fields for FXH Messages
                //if (hdMessageType.Value.Trim() == "FXH")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || txtWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                //    {
                //        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                //        lblWBLStatus.ForeColor = Color.Red;
                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (ddlHoldType.Text.Trim() == "")
                //    {
                //        lblHLDStatus.Text = "Please Enter HLD Mandatory fields marked with (*)!";
                //        lblHLDStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 
                //    return;

                //}

                ////Validating Mandatory fields for FSN Messages
                //if (hdMessageType.Value.Trim() == "FSN")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }

                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {

                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (ddlHoldType.Text.Trim() == "")
                //    {
                //        lblHLDStatus.Text = "Please Enter HLD Mandatory fields marked with (*)!";
                //        lblHLDStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 
                //    return;


                //}
                ////Validating Mandatory fields for FSI Messages
                //if (hdMessageType.Value.Trim() == "FSI")
                //{

                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }

                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }

                //    if (ddlActionCode.Text.Trim() == "" || txtNumberofPieces.Text.Trim() == "" || txtTransactionDate.Text.Trim() == "" || txtTransactionTime.Text.Trim() == "")
                //    {

                //        lblStatusCBPStatus.Text = "Please Enter CBP Status Notification(CSN) Mandatory fields marked with (*)!";
                //        lblStatusCBPStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 
                //    return;


                //}
                ////Validating Mandatory fields for FDM Messages
                //if (hdMessageType.Value.Trim() == "FDM")
                //{
                //    if (txtDepDetailsImportingCarrier.Text.Trim() == "" || txtDepDetailsFlightNumber.Text.Trim() == "" || txtDateOfScheduledArrival.Text.Trim() == "" || txtActualImportingCarrier.Text.Trim() == "" || txtActualFlightNumber.Text.Trim() == "")
                //    {

                //        lblDepartureStatus.Text = "Please Enter Departure(DEP) Mandatory fields marked with (*)!";
                //        lblDepartureStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 
                //    return;

                //}

                ////Validating Mandatory fields for FER Messages
                //if (hdMessageType.Value.Trim() == "FER")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (ddlErrorCode.Text.Trim() == "" || txtErrorMessage.Text.Trim() == "")
                //    {
                //        lblErrorMessageStatus.Text = "Please Enter Error(ERR) Mandatory fields marked with (*)!";
                //        lblErrorMessageStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 
                //    return;


                //}
                ////Validating Mandatory fields for FSQ Messages
                //if (hdMessageType.Value.Trim() == "FSQ")
                //{
                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (ddlStatusRequestCode.Text.Trim() == "")
                //    {
                //        lblStatusQueryStatus.Text = "Please Enter Freight Status Query(FSQ) Mandatory fields marked with (*)!";
                //        lblStatusQueryStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                //    return;


                //}
                ////Validating Mandatory fields for FSC Messages
                //if (hdMessageType.Value.Trim() == "FSC")
                //{

                //    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                //    {
                //        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                //        lblAWBStatus.ForeColor = Color.Red;
                //    }
                //    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                //    {
                //        //lblStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                //        lblArrivalStatus.ForeColor = Color.Red;
                //    }
                //    if (ddlStatusAnswerCode.Text.Trim() == "")
                //    {
                //        lblStatusConditionStatus.Text = "Please Enter Freight Status Condition(FSC) Mandatory fields marked with (*)!";
                //        lblStatusConditionStatus.ForeColor = Color.Red;
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false); 
                //    return;


                //}
                #endregion

                if (!ValidatingAMSMessages())
                { return; }
                //btnPreviewMessage_Click(sender, e);

                //Encoding AMS Message
                EncodeMessage();

                //Preparing Parameters to save the Message Details Against the AWB
                //string[] QueryNames = new string[82];
                object[] QueryValues = new object[84];
                //SqlDbType[] QueryTypes = new SqlDbType[82];
                #region Commented Code
                //int i=0;
                //      QueryNames[i++] = "AWBPrefix";
                //      QueryNames[i++] = "AWBNumber";
                //      QueryNames[i++] = "MessageType";
                //      QueryNames[i++] = "HAWBNumber";
                //      QueryNames[i++] = "ConsolidationIdentifier";
                //      QueryNames[i++] = "PackageTrackingIdentifier";
                //      QueryNames[i++] = "AWBPartArrivalReference";
                //      QueryNames[i++] = "ArrivalAirport";
                //      QueryNames[i++] = "AirCarrier";
                //      QueryNames[i++] = "Origin";
                //      QueryNames[i++] = "DestinionCode";
                //      QueryNames[i++] = "WBLNumberOfPieces";
                //      QueryNames[i++] = "WBLWeightIndicator";
                //      QueryNames[i++] = "WBLWeight";
                //      QueryNames[i++] = "WBLCargoDescription";
                //      QueryNames[i++] = "ArrivalDate";
                //      QueryNames[i++] = "PartArrivalReference";
                //      QueryNames[i++] = "BoardedQuantityIdentifier";
                //      QueryNames[i++] = "BoardedPieceCount";
                //      QueryNames[i++] = "BoardedWeight";
                //      QueryNames[i++] = "ImportingCarrier";
                //      QueryNames[i++] = "FlightNumber";
                //      QueryNames[i++] = "ARRPartArrivalReference";
                //      QueryNames[i++] = "RequestType";
                //      QueryNames[i++] = "RequestExplanation";
                //      QueryNames[i++] = "EntryType";
                //      QueryNames[i++] = "EntryNumber";
                //      QueryNames[i++] = "AMSParticipantCode";
                //      QueryNames[i++] = "ShipperName";
                //      QueryNames[i++] = "ShipperAddress";
                //      QueryNames[i++] = "ShipperCity";
                //      QueryNames[i++] = "ShipperState";
                //      QueryNames[i++] = "ShipperCountry";
                //      QueryNames[i++] = "ShipperPostalCode";
                //      QueryNames[i++] = "ConsigneeName";
                //      QueryNames[i++] = "ConsigneeAddress";
                //      QueryNames[i++] = "ConsigneeCity";
                //      QueryNames[i++] = "ConsigneeState";
                //      QueryNames[i++] = "ConsigneeCountry";
                //      QueryNames[i++] = "ConsigneePostalCode";
                //      QueryNames[i++] = "TransferDestAirport";
                //      QueryNames[i++] = "DomesticIdentifier";
                //      QueryNames[i++] = "BondedCarrierID";
                //      QueryNames[i++] = "OnwardCarrier";
                //      QueryNames[i++] = "BondedPremisesIdentifier";
                //      QueryNames[i++] = "InBondControlNumber";
                //      QueryNames[i++] = "OriginOfGoods";
                //      QueryNames[i++] = "DeclaredValue";
                //      QueryNames[i++] = "CurrencyCode";
                //      QueryNames[i++] = "CommodityCode";
                //      QueryNames[i++] = "LineIdentifier";
                //      QueryNames[i++] = "AmendmentCode";
                //      QueryNames[i++] = "AmendmentExplanation";
                //      QueryNames[i++] = "DeptImportingCarrier";
                //      QueryNames[i++] = "DeptFlightNumber";
                //      QueryNames[i++] = "DeptScheduledArrivalDate";
                //      QueryNames[i++] = "LiftoffDate";
                //      QueryNames[i++] = "LiftoffTime";
                //      QueryNames[i++] = "DeptActualImportingCarrier";
                //      QueryNames[i++] = "DeptActualFlightNumber";
                //      QueryNames[i++] = "ASNStatusCode";
                //      QueryNames[i++] = "ASNActionExplanation";
                //      QueryNames[i++] = "CSNActionCode";
                //      QueryNames[i++] = "CSNPieces";
                //      QueryNames[i++] = "TransactionDate";
                //      QueryNames[i++] = "TransactionTime";
                //      QueryNames[i++] = "CSNEntryType";
                //      QueryNames[i++] = "CSNEntryNumber";
                //      QueryNames[i++] = "CSNRemarks";
                //      QueryNames[i++] = "ErrorCode";
                //      QueryNames[i++] = "ErrorMessage";
                //      QueryNames[i++] = "StatusRequestCode";
                //      QueryNames[i++] = "StatusAnswerCode";
                //      QueryNames[i++] = "Information";
                //      QueryNames[i++] = "ERFImportingCarrier";
                //      QueryNames[i++] = "ERFFlightNumber";
                //      QueryNames[i++] = "ERFDate";
                //      QueryNames[i++] = "Message";
                //      QueryNames[i++] = "UpdatedOn";
                //      QueryNames[i++] = "UpdatedBy";
                //      QueryNames[i++] = "CreatedOn";
                //      QueryNames[i++] = "CreatedBy";

                //  int j=0;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.Int;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.Decimal;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.Int;
                //      QueryTypes[j++] = SqlDbType.Decimal;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.BigInt;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.DateTime;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                //      QueryTypes[j++] = SqlDbType.DateTime;
                //      QueryTypes[j++] = SqlDbType.VarChar;
                #endregion

                int k = 0;
                QueryValues[k++] = txtawbprefix.Text.Trim();
                QueryValues[k++] = txtawbserialnumber.Text.Trim();
                QueryValues[k++] = hdMessageType.Value.Trim();
                QueryValues[k++] = ddlHAWB.Text;
                QueryValues[k++] = txtconsolidateid.Text.Trim();
                QueryValues[k++] = txtpackagetrackid.Text.Trim();
                QueryValues[k++] = txtPartArrival.Text.Trim();
                QueryValues[k++] = txtContLocation.Text.Trim();
                QueryValues[k++] = txtImportingCarrier.Text.Trim();
                QueryValues[k++] = TxtOrigin.Text.Trim();
                QueryValues[k++] = txtDestination.Text.Trim();
                if (txtPieces.Text == "")
                { QueryValues[k++] = 0; }
                else
                {
                    QueryValues[k++] = txtPieces.Text.Trim();
                }
                QueryValues[k++] = ddlWBLWeightCode.Text.Trim();
                if (txtWeight.Text == "")
                { QueryValues[k++] = 0; }
                else
                {
                    QueryValues[k++] = txtWeight.Text.Trim();

                }
                QueryValues[k++] = txtCargoDescription.Text.Trim();
                QueryValues[k++] = txtScheduledArrDate.Text.Trim();
                QueryValues[k++] = txtPartArrivalReference.Text.Trim();
                QueryValues[k++] = txtBoardedQtyIdentifier.Text.Trim();
                if (txtBoardedPieceCount.Text == "")
                { QueryValues[k++] = 0; }
                else
                {
                    QueryValues[k++] = txtBoardedPieceCount.Text.Trim();
                }

                if (txtArrWeight.Text == "")
                { QueryValues[k++] = 0; }
                else
                {
                    QueryValues[k++] = txtArrWeight.Text.Trim();

                }


                QueryValues[k++] = txtImportingCarrier.Text.Trim();
                QueryValues[k++] = txtFlightNumber.Text.Trim();
                QueryValues[k++] = txtPartArrivalReference.Text.Trim();
                QueryValues[k++] = ddlHoldType.SelectedItem.Value.Trim();
                QueryValues[k++] = txtRequestExplanation.Text.Trim();
                QueryValues[k++] = ddlEntryType.SelectedItem.Value.Trim();
                QueryValues[k++] = TxtEntryNo.Text.Trim();
                QueryValues[k++] = txtAgent.Text.Trim();
                QueryValues[k++] = txtShipperName.Text;
                QueryValues[k++] = txtShipperAddress.Text;
                QueryValues[k++] = txtCity.Text.Trim();
                QueryValues[k++] = txtState.Text.Trim();
                QueryValues[k++] = ddlCountryCode.Text.Trim();
                QueryValues[k++] = txtPostalCode.Text.Trim();
                QueryValues[k++] = txtConsigneeName.Text;
                QueryValues[k++] = txtConStreetAddress.Text;
                QueryValues[k++] = txtConCity.Text.Trim();
                QueryValues[k++] = txtConState.Text.Trim();
                QueryValues[k++] = ddlConCountryCode.Text.Trim();
                QueryValues[k++] = txtConPostalCode.Text.Trim();
                QueryValues[k++] = txtTransferDestArpt.Text.Trim();
                QueryValues[k++] = txtDomInt.Text.Trim();
                QueryValues[k++] = txtBondedCarrierID.Text.Trim();
                QueryValues[k++] = txtOnwardCarrier.Text.Trim();
                QueryValues[k++] = txtBondedPremIdentifier.Text.Trim();
                QueryValues[k++] = txtInbondControlNo.Text.Trim();
                QueryValues[k++] = ddlDescriptionCountryCode.Text.Trim();
                if (txtDeclaredValue.Text == "")
                { QueryValues[k++] = 0; }
                else
                {
                    QueryValues[k++] = txtDeclaredValue.Text.Trim();
                }
                QueryValues[k++] = ddlCurrencyCode.Text.Trim();
                QueryValues[k++] = txtCommodityCode.Text.Trim();
                QueryValues[k++] = txtLineIdentifier.Text.Trim();
                QueryValues[k++] = ddlAmendmentCode.SelectedItem.Value;
                QueryValues[k++] = txtExplanation.Text;
                QueryValues[k++] = txtDepDetailsImportingCarrier.Text.Trim();
                QueryValues[k++] = txtDepDetailsFlightNumber.Text.Trim();
                QueryValues[k++] = txtDateOfScheduledArrival.Text.Trim();
                QueryValues[k++] = txtLiftoffDate.Text.Trim();
                if (txtLiftoffTime.Text != "")
                {
                    QueryValues[k++] = txtLiftoffTime.Text.Trim();
                }
                else
                {
                    QueryValues[k++] = "";
                }
                QueryValues[k++] = txtActualImportingCarrier.Text.Trim();
                QueryValues[k++] = txtActualFlightNumber.Text.Trim();
                QueryValues[k++] = ddlStatusCode.SelectedItem.Value;
                QueryValues[k++] = txtActionExplanation.Text;
                QueryValues[k++] = ddlActionCode.SelectedItem.Value.Trim();
                QueryValues[k++] = txtNumberofPieces.Text.Trim();
                QueryValues[k++] = txtTransactionDate.Text.Trim();
                if (txtTransactionTime.Text != "")
                {
                    QueryValues[k++] = txtTransactionTime.Text.Trim();

                }
                else
                { QueryValues[k++] = ""; }
                QueryValues[k++] = ddlCBPEntryCodes.Text.Trim();
                QueryValues[k++] = txtEntryNumber.Text.Trim();
                QueryValues[k++] = txtremarks.Text;
                QueryValues[k++] = ddlErrorCode.SelectedItem.Text.Trim();
                QueryValues[k++] = txtErrorMessage.Text;
                QueryValues[k++] = ddlStatusRequestCode.SelectedItem.Value;
                QueryValues[k++] = ddlStatusAnswerCode.SelectedItem.Value;
                QueryValues[k++] = txtInformation.Text;
                QueryValues[k++] = txtImportingCarrier.Text.Trim();
                QueryValues[k++] = txtFlightNumber.Text.Trim();
                QueryValues[k++] = ((DateTime)Session["IT"]).ToString("dd-MMM-yyyy");
                QueryValues[k++] = txtMessagePreview.Text;
                QueryValues[k++] = ((DateTime)Session["IT"]).ToString();
                QueryValues[k++] = Session["UserName"].ToString();
                QueryValues[k++] = ((DateTime)Session["IT"]).ToString(); 
                QueryValues[k++] = Session["UserName"].ToString();
                QueryValues[k++] = hdFlightNo.Value;
                QueryValues[k++] = hdFlightDate.Value;
                if (objCustoms.UpdateCustomsMessages(QueryValues, hdFSNMsgType.Value.ToString()))
                {
                    lblStatus.Text = hdMessageType.Value + " Message Updated Successfully!";
                    lblStatus.ForeColor = Color.Green;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                }
                else
                { }
                //if (hdMessageType.Value.Trim() == "FRI" || hdMessageType.Value.Trim() == "FXI" || hdMessageType.Value.Trim() == "FRC" || hdMessageType.Value.Trim() == "FXC" || hdMessageType.Value.Trim() == "FRX" || hdMessageType.Value.Trim() == "FXX" || hdMessageType.Value.Trim() == "FDM" || hdMessageType.Value.Trim() == "FER" || hdMessageType.Value.Trim() == "FSQ")
                //{
                //    if (db.InsertData("SP_UpdateOutboxCustomsMessage", QueryNames, QueryTypes, QueryValues))
                //    {
                //        lblStatus.Text = hdMessageType.Value + " Message Updated Successfully!";
                //        lblStatus.ForeColor = Color.Green;
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);

                //    }
                //}
                //else
                //{
                //    if (db.InsertData("SP_UpdateInboxCustomsMessage", QueryNames, QueryTypes, QueryValues))
                //    {
                //        lblStatus.Text = hdMessageType.Value + " Message Updated Successfully!";
                //        lblStatus.ForeColor = Color.Green;
                //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                //    }
                //}


            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Preview Message
        protected void btnPreviewMessage_Click(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                if (!ValidatingAMSMessages())
                { return; }
                StringBuilder sb = new StringBuilder();
                //Generating Freight Report Inbound Message
                if (hdMessageType.Value == "FRI")
                {
                    if (ddlHAWB.Items.Count > 0)
                    {
                        //Message Type   ---SMI----
                        sb.AppendLine(hdMessageType.Value);

                        //AirWay Bill Details  -----AWB------
                        if (txtpackagetrackid.Text == "")
                        {
                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim() + "-" + "M" + ddlHAWB.Text.Trim());
                        }
                        else
                        {
                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim() + "-" + "M" + ddlHAWB.Text.Trim() + "/" + txtpackagetrackid.Text);
                        }
                        //WayBill Details   ----WBL----
                        sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                            + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                            + "/" + txtCargoDescription.Text.Trim());
                        //Arrival Details   ------ARR------
                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                        //Shipper Details  -----SHP-----
                        sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                        sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                        sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                        sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                        //Consignee Details ------CNE-----
                        sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                        sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                        sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                        sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                        //Transfer Details -------TRN----------
                        sb.AppendLine("TRN" + "/" + txtTransferDestArpt.Text.Trim());
                        //--------FDA----------
                        sb.AppendLine("FDA");
                        //Transfer Details -------TRN-----

                        //sb.AppendLine("TRN"

                        txtMessagePreview.Text = sb.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                    }
                    else
                    {

                        //Message Type   ---SMI----
                        sb.AppendLine(hdMessageType.Value);
                        //Control Location Details   -----CCL-------
                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                        //AirWay Bill Details  -----AWB------
                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                        //WayBill Details   ----WBL----
                        sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                            + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                            + "/" + txtCargoDescription.Text.Trim());
                        //Arrival Details   ------ARR------
                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                        //Shipper Details  -----SHP-----
                        sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                        sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                        sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                        sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                        //Consignee Details ------CNE-----
                        sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                        sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                        sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                        sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                        //Transfer Details -------TRN----------
                        sb.AppendLine("TRN" + "/" + txtTransferDestArpt.Text.Trim());
                        //--------FDA----------
                        sb.AppendLine("FDA");
                        //Transfer Details -------TRN-----

                        //sb.AppendLine("TRN"

                        txtMessagePreview.Text = sb.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);

                    }
                }
                else
                    //Generating Freight Express Inbound Message

                    if (hdMessageType.Value.Trim() == "FXI")
                    {
                        //Message Type   ---SMI----
                        sb.AppendLine(hdMessageType.Value);
                        //Control Location Details   -----CCL-------
                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                        //AirWay Bill Details  -----AWB------
                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                        //WayBill Details   ----WBL----
                        sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                            + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                            + "/" + txtCargoDescription.Text.Trim());
                        //Arrival Details   ------ARR------
                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                        //CBP Entry Detail -----CED-----
                        if ((ddlEntryType.SelectedItem.Value == "11" || ddlEntryType.SelectedItem.Value == "86" || ddlEntryType.SelectedItem.Value == "92") && TxtEntryNo.Text.Trim() == "")
                        {
                            lblStatus.Text = "Please Enter the Entry Number & Try again..";
                            lblStatus.ForeColor = Color.Red;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                            return;
                        }
                        //else if ((ddlEntryType.SelectedItem.Value == "11" || ddlEntryType.SelectedItem.Value == "86" || ddlEntryType.SelectedItem.Value == "92") && txtEntryNumber.Text.Trim() == "")
                        //{
                        //    sb.AppendLine("CED" + "/" + ddlEntryType.SelectedItem.Value.Trim()+"/"+txtEntryNumber.Text.Trim());
                        //}
                        else
                        {
                            sb.AppendLine("CED" + "/" + ddlEntryType.SelectedItem.Value.Trim() + "/" + TxtEntryNo.Text.Trim());

                        }
                        //Shipper Details  -----SHP-----
                        sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                        sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                        sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                        sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                        //Consignee Details ------CNE-----
                        sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                        sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                        sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                        sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());

                        txtMessagePreview.Text = sb.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                    }
                    else
                        //Generating Freight Report Change Message

                        if (hdMessageType.Value.Trim() == "FRC")
                        {
                            //Message Type   ---SMI----
                            sb.AppendLine(hdMessageType.Value);
                            //Control Location Details   -----CCL-------
                            sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                            //AirWay Bill Details  -----AWB------
                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                            //WayBill Details   ----WBL----
                            sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                                + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                                + "/" + txtCargoDescription.Text.Trim());
                            //Arrival Details   ------ARR------
                            sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                            //Shipper Details  -----SHP-----
                            sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                            sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                            sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                            sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                            //Consignee Details ------CNE-----
                            sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                            sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                            sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                            sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                            //Reason for Amendment
                            sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim().PadLeft(2, '0'));
                            txtMessagePreview.Text = sb.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                        }
                        else
                            //Generating Freight Express Change Message

                            if (hdMessageType.Value.Trim() == "FXC")
                            {
                                //Message Type   ---SMI----
                                sb.AppendLine(hdMessageType.Value);
                                //Control Location Details   -----CCL-------
                                sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                //AirWay Bill Details  -----AWB------
                                sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                //WayBill Details   ----WBL----
                                sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                                    + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                                    + "/" + txtCargoDescription.Text.Trim());
                                //Arrival Details   ------ARR------
                                sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                    txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                //Shipper Details  -----SHP-----
                                sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                                sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                                sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                                sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                                //Consignee Details ------CNE-----
                                sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                                sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                                sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                                sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                                //Reason for Amendment
                                sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim());
                                txtMessagePreview.Text = sb.ToString();
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                            }
                            else
                                //Generating Freight Report Cancellation Message

                                if (hdMessageType.Value.Trim() == "FRX")
                                {
                                    //Message Type   ---SMI----
                                    sb.AppendLine(hdMessageType.Value);
                                    //Control Location Details   -----CCL-------
                                    sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                    //AirWay Bill Details  -----AWB------
                                    sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                    //Arrival Details   ------ARR------
                                    sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                        txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                    //Reason for Amendment ----RFA----
                                    sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim());
                                    txtMessagePreview.Text = sb.ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                }
                                else
                                    //Generating Freight Report Hold Message
                                    if (hdMessageType.Value.Trim() == "FRH")
                                    {
                                        //Message Type   ---SMI----
                                        sb.AppendLine(hdMessageType.Value);
                                        //Control Location Details   -----CCL-------
                                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                        //AirWay Bill Details  -----AWB------
                                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                        //Arrival Details   ------ARR------
                                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                        //Hold/Release/Denied Entry Information  ------HLD-------
                                        sb.AppendLine("HLD" + "/" + ddlHoldType.SelectedValue.Trim() + "/" + txtRequestExplanation.Text.Trim());
                                        txtMessagePreview.Text = sb.ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                    }
                                    else
                                        //Generating Freight Express Cancellation Message
                                        if (hdMessageType.Value.Trim() == "FXX")
                                        {
                                            //Message Type   ---SMI----
                                            sb.AppendLine(hdMessageType.Value);
                                            //Control Location Details   -----CCL-------
                                            sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                            //AirWay Bill Details  -----AWB------
                                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                            //Arrival Details   ------ARR------
                                            sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                            //Reason for Amendment ----RFA----
                                            sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim());
                                            txtMessagePreview.Text = sb.ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                        }
                                        else
                                            //Generating Freight Express Hold Message
                                            if (hdMessageType.Value.Trim() == "FXH")
                                            {
                                                //Message Type   ---SMI----
                                                sb.AppendLine(hdMessageType.Value);
                                                //Control Location Details   -----CCL-------
                                                sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                //AirWay Bill Details  -----AWB------
                                                sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                //Arrival Details   ------ARR------
                                                sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                    txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                //Hold/Release/Denied Entry Information  ------HLD-------
                                                sb.AppendLine("HLD" + "/" + ddlHoldType.SelectedValue.Trim() + "/" + txtRequestExplanation.Text.Trim());
                                                txtMessagePreview.Text = sb.ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                            }
                                            else
                                                //Generating Freight Status Notification Message
                                                if (hdFSNMsgType.Value.Trim() == "FSN")
                                                {
                                                    //Message Type   ---SMI----
                                                    sb.AppendLine(hdMessageType.Value);
                                                    //Control Location Details   -----CCL-------
                                                    sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                    //AirWay Bill Details  -----AWB------
                                                    sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                    //Arrival Details   ------ARR------
                                                    sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                        txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                    //Airline Status Notification -----ASN-----
                                                    sb.AppendLine("ASN" + "/" + ddlStatusCode.SelectedItem.Value.Trim() + txtActionExplanation.Text);
                                                    //CBP Status Notification   -----CSN-------
                                                    //sb.AppendLine("CSN" + "/" + ddlActionCode.SelectedItem.Value.Trim() + "-" + txtNumberofPieces.Text.Trim() + "/" + txtTransactionDate.Text.Trim() + txtTransactionTime.Text.Trim());
                                                    txtMessagePreview.Text = sb.ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                                }
                                                else
                                                    //Generating Freight Status Information Message
                                                    if (hdMessageType.Value.Trim() == "FSI")
                                                    {
                                                        //Message Type   ---SMI----
                                                        sb.AppendLine(hdMessageType.Value);
                                                        //Control Location Details   -----CCL-------
                                                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                        //AirWay Bill Details  -----AWB------
                                                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                        //Arrival Details   ------ARR------
                                                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                        //CBP Status Notification   -----CSN-------
                                                        sb.AppendLine("CSN" + "/" + ddlActionCode.SelectedItem.Value.Trim() + "-" + txtNumberofPieces.Text.Trim() + "/" + txtTransactionDate.Text.Trim().Replace("-", "").Substring(0, 5) + txtTransactionTime.Text.Trim());
                                                        txtMessagePreview.Text = sb.ToString();
                                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                                    }
                                                    else
                                                        //Generating Flight Departure Message
                                                        if (hdMessageType.Value.Trim() == "FDM")
                                                        {
                                                            //Message Type   ---SMI----
                                                            sb.AppendLine(hdMessageType.Value);
                                                            //Departure Details   -----DEP-------
                                                            sb.AppendLine("DEP" + "/" + txtDepDetailsImportingCarrier.Text.Trim() + txtDepDetailsFlightNumber.Text.Trim() + "/" + txtActualImportingCarrier.Text.Trim() + txtActualFlightNumber.Text.Trim());
                                                            txtMessagePreview.Text = sb.ToString();
                                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                                        }
                                                        else
                                                            //Generating Freight Error Report Message
                                                            if (hdMessageType.Value.Trim() == "FER")
                                                            {
                                                                //Message Type   ---SMI----
                                                                sb.AppendLine(hdMessageType.Value);
                                                                //Error Report Flight Details   -----ERF-------
                                                                sb.AppendLine(txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" + DateTime.Now.ToString("dd-MMM-yyyy").Replace("-", "").Substring(0, 5));
                                                                //AirWay Bill Details  -----AWB------
                                                                sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                //Error Message   -----ERR-------
                                                                sb.AppendLine("ERR" + "/" + ddlErrorCode.SelectedItem.Text.Trim() + txtErrorMessage.Text);
                                                                txtMessagePreview.Text = sb.ToString();
                                                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                                            }
                                                            else
                                                                //Generating Freight Status Query Message
                                                                if (hdMessageType.Value.Trim() == "FSQ")
                                                                {
                                                                    //Message Type   ---SMI----
                                                                    sb.AppendLine(hdMessageType.Value);
                                                                    //Control Location Details   -----CCL-------
                                                                    sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                                    //AirWay Bill Details  -----AWB------
                                                                    sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                    //Freight Status Query   -----FSQ-------
                                                                    sb.AppendLine("FSQ" + "/" + ddlStatusRequestCode.SelectedItem.Value.Trim());
                                                                    txtMessagePreview.Text = sb.ToString();
                                                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                                                }
                                                                else
                                                                    //Generating Freight Status Condition Message
                                                                    if (hdMessageType.Value.Trim() == "FSC")
                                                                    {
                                                                        //Message Type   ---SMI----
                                                                        sb.AppendLine(hdMessageType.Value);
                                                                        //Control Location Details   -----CCL-------
                                                                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                                        //AirWay Bill Details  -----AWB------
                                                                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                        //Arrival Details   ------ARR------
                                                                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                                        //Freight Status Condition   -----FSC-------
                                                                        sb.AppendLine("FSC" + "/" + ddlStatusAnswerCode.SelectedItem.Value.Trim());
                                                                        txtMessagePreview.Text = sb.ToString();
                                                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                                                    }
                                                                    else
                                                                        //Generating Freight Status Notification Message
                                                                        if (hdFSNMsgType.Value.Trim() == "FSNInbox")
                                                                        {
                                                                            //Message Type   ---SMI----
                                                                            sb.AppendLine(hdMessageType.Value);
                                                                            //Control Location Details   -----CCL-------
                                                                            sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                                            //AirWay Bill Details  -----AWB------
                                                                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                            //Arrival Details   ------ARR------
                                                                            sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                                                txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                                            //CBP Status Notification   -----CSN-------
                                                                            sb.AppendLine("CSN" + "/" + ddlActionCode.SelectedItem.Value.Trim() + "-" + txtNumberofPieces.Text.Trim() + "/" + txtTransactionDate.Text.Trim().Replace("-", "").Substring(0, 5) + txtTransactionTime.Text.Trim());
                                                                            txtMessagePreview.Text = sb.ToString();
                                                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailSplit();</SCRIPT>", false);
                                                                        }
                //if (hdMessageType.Value == "FSC" || hdMessageType.Value == "FSI" || hdMessageType.Value == "FRH")
                //{
                //    cls_Encode_Decode obj = new cls_Encode_Decode();
                //    //StringReader sReader=new StringReader(txtMessagePreview.Text);
                //    bool flag = obj.DecodeCustomsMessage("FSI  HA  173-40000004  ARR/HA566/03Jan  CSN/1A-10/03Jan2222      ");
                //}
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Send Message
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                if (!ValidatingAMSMessages())
                { return; }
                EncodeMessage();
                //if (hdMessageType.Value == "FRI" || hdMessageType.Value == "FXI" || hdMessageType.Value == "FRC")
                //{
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewEmailMessageSplit();</SCRIPT>", false);
                //}
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Send Mail
        public bool sendMail(string fromEmailId, string toEmailId, string password, string subject, string body, bool isBodyHTML)
        {
            bool flag = false;
            try
            {
                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress(fromEmailId);
                Mail.To.Add(toEmailId.Trim());
                Mail.Subject = subject;
                Mail.IsBodyHtml = isBodyHTML;
                Mail.Body = body;
                SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 80);//previous 25
                smtp.Credentials = new NetworkCredential(fromEmailId, password);
                Mail.Priority = MailPriority.High;


                try
                {
                    smtp.Send(Mail);
                    flag = true;
                    //clsLog.WriteLog("Mail Sent @ "+DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    //clsLog.WriteLog("Exception while Sending Mail : "+ ex.Message );
                }

            }
            catch (Exception ex)
            {
                //clsLog.WriteLog("Exception while collection Mail Info : "+ ex.Message );
                flag = false;
            }
            return flag;
        }
        #endregion

        #region Button Ok Click
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                lblStatus.ForeColor = Color.Red;
                string FromEmailID = ConfigurationManager.AppSettings["FromEmailID"].ToString();
                string EmailPwd = ConfigurationManager.AppSettings["Pass"].ToString();

                if (txtEmailID.Text != "")
                {
                    string[] emailid = txtEmailID.Text.Split(',');
                    for (int i = 0; i < emailid.Length; i++)
                    {
                        bool isFlag = cls_BL.addMsgToOutBox(hdMessageType.Value.Trim() + ":" + txtawbprefix.Text.Trim() + txtawbserialnumber.Text.Trim(), txtMessagePreview.Text, "", emailid[i]);
                        //bool flag = sendMail(FromEmailID, emailid[i], EmailPwd,hdMessageType.Value.Trim()+":"+txtawbprefix.Text.Trim()+txtawbserialnumber.Text.Trim(),txtMessagePreview.Text, false);
                        if (isFlag == false)
                        {
                            lblStatus.Text = hdMessageType.Value.Trim() + " Message Sending Failed";
                            lblStatus.ForeColor = Color.Red;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                            return;

                        }
                    }
                    lblStatus.Text = hdMessageType.Value.Trim() + " Message Sent Successfully";
                    lblStatus.ForeColor = Color.Green;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                }
                else
                { }

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region Encode AMS Message
        public void EncodeMessage()
        {
            try
            {
                lblStatus.Text = "";
                StringBuilder sb = new StringBuilder();
                //Generating Freight Report Inbound Message
                if (hdMessageType.Value == "FRI")
                {
                    //Message Type   ---SMI----
                    sb.AppendLine(hdMessageType.Value);
                    //Control Location Details   -----CCL-------
                    sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                    //AirWay Bill Details  -----AWB------
                    sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                    //WayBill Details   ----WBL----
                    sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                        + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                        + "/" + txtCargoDescription.Text.Trim());
                    //Arrival Details   ------ARR------
                    sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                        txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                    //Shipper Details  -----SHP-----
                    sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                    sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                    sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                    sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                    //Consignee Details ------CNE-----
                    sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                    sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                    sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                    sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                    //Transfer Details -------TRN----------
                    if (txtTransferDestArpt.Text != "")
                    {
                        sb.AppendLine("TRN" + "/" + txtTransferDestArpt.Text.Trim());
                    }
                    //--------FDA----------
                    sb.AppendLine("FDA");

                    txtMessagePreview.Text = sb.ToString();
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                }
                else
                    //Generating Freight Express Inbound Message

                    if (hdMessageType.Value.Trim() == "FXI")
                    {
                        //Message Type   ---SMI----
                        sb.AppendLine(hdMessageType.Value);
                        //Control Location Details   -----CCL-------
                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                        //AirWay Bill Details  -----AWB------
                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                        //WayBill Details   ----WBL----
                        sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                            + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                            + "/" + txtCargoDescription.Text.Trim());
                        //Arrival Details   ------ARR------
                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                        //CBP Entry Detail -----CED-----
                        if ((ddlEntryType.SelectedItem.Value == "11" || ddlEntryType.SelectedItem.Value == "86" || ddlEntryType.SelectedItem.Value == "92") && TxtEntryNo.Text.Trim() == "")
                        {
                            lblStatus.Text = "Please Enter the Entry Number & Try again..";
                            lblStatus.ForeColor = Color.Red;
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                            return;
                        }
                        //else if ((ddlEntryType.SelectedItem.Value == "11" || ddlEntryType.SelectedItem.Value == "86" || ddlEntryType.SelectedItem.Value == "92") && txtEntryNumber.Text.Trim() == "")
                        //{
                        //    sb.AppendLine("CED" + "/" + ddlEntryType.SelectedItem.Value.Trim()+"/"+txtEntryNumber.Text.Trim());
                        //}
                        else
                        {
                            sb.AppendLine("CED" + "/" + ddlEntryType.SelectedItem.Value.Trim() + "/" + TxtEntryNo.Text.Trim());

                        }
                        //Shipper Details  -----SHP-----
                        sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                        sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                        sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                        sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                        //Consignee Details ------CNE-----
                        sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                        sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                        sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                        sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                        //--------FDA----------
                        sb.AppendLine("FDA");

                        txtMessagePreview.Text = sb.ToString();
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                    }
                    else
                        //Generating Freight Report Change Message

                        if (hdMessageType.Value.Trim() == "FRC")
                        {
                            //Message Type   ---SMI----
                            sb.AppendLine(hdMessageType.Value);
                            //Control Location Details   -----CCL-------
                            sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                            //AirWay Bill Details  -----AWB------
                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                            //WayBill Details   ----WBL----
                            sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                                + txtPieces.Text.Trim() + "/" + ddlWBLWeightCode.Text.Trim() + txtWeight.Text.Trim()
                                + "/" + txtCargoDescription.Text.Trim());
                            //Arrival Details   ------ARR------
                            sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                            //Shipper Details  -----SHP-----
                            sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                            sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                            sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                            sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                            //Consignee Details ------CNE-----
                            sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                            sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                            sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                            sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                            //Reason for Amendment
                            sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim().PadLeft(2, '0'));
                            txtMessagePreview.Text = sb.ToString();
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                        }
                        else
                            //Generating Freight Express Change Message

                            if (hdMessageType.Value.Trim() == "FXC")
                            {
                                //Message Type   ---SMI----
                                sb.AppendLine(hdMessageType.Value);
                                //Control Location Details   -----CCL-------
                                sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                //AirWay Bill Details  -----AWB------
                                sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                //WayBill Details   ----WBL----
                                sb.AppendLine("WBL" + "/" + TxtOrigin.Text.Trim() + "/" + txtShipmentDescriptionCode.Text.Trim()
                                    + txtPieces.Text.Trim() + "/" +
                                    ddlWBLWeightCode.Text.Trim() +//txtWeightCode.Text.Trim() + 
                                    txtWeight.Text.Trim()
                                    + "/" + txtCargoDescription.Text.Trim());
                                //Arrival Details   ------ARR------
                                sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                    txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                //Shipper Details  -----SHP-----
                                sb.AppendLine("SHP" + "/" + txtShipperName.Text.Trim());
                                sb.AppendLine("/" + txtShipperAddress.Text.Trim());
                                sb.AppendLine("/" + txtCity.Text.Trim() + "/" + txtState.Text.Trim());
                                sb.AppendLine("/" + ddlCountryCode.Text.Trim() + "/" + txtPostalCode.Text.Trim());
                                //Consignee Details ------CNE-----
                                sb.AppendLine("CNE" + "/" + txtConsigneeName.Text.Trim());
                                sb.AppendLine("/" + txtConStreetAddress.Text.Trim());
                                sb.AppendLine("/" + txtConCity.Text.Trim() + "/" + txtConState.Text.Trim());
                                sb.AppendLine("/" + ddlConCountryCode.Text.Trim() + "/" + txtConPostalCode.Text.Trim());
                                //Reason for Amendment
                                sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim());
                                txtMessagePreview.Text = sb.ToString();
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                            }
                            else
                                //Generating Freight Report Cancellation Message

                                if (hdMessageType.Value.Trim() == "FRX")
                                {
                                    //Message Type   ---SMI----
                                    sb.AppendLine(hdMessageType.Value);
                                    //Control Location Details   -----CCL-------
                                    sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                    //AirWay Bill Details  -----AWB------
                                    sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                    //Arrival Details   ------ARR------
                                    sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                        txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                    //Reason for Amendment ----RFA----
                                    sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim());
                                    txtMessagePreview.Text = sb.ToString();
                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                }
                                else
                                    //Generating Freight Report Hold Message
                                    if (hdMessageType.Value.Trim() == "FRH")
                                    {
                                        //Message Type   ---SMI----
                                        sb.AppendLine(hdMessageType.Value);
                                        //Control Location Details   -----CCL-------
                                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                        //AirWay Bill Details  -----AWB------
                                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                        //Arrival Details   ------ARR------
                                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                        //Hold/Release/Denied Entry Information  ------HLD-------
                                        sb.AppendLine("HLD" + "/" + ddlHoldType.SelectedValue.Trim() + "/" + txtRequestExplanation.Text.Trim());
                                        txtMessagePreview.Text = sb.ToString();
                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                    }
                                    else
                                        //Generating Freight Express Cancellation Message
                                        if (hdMessageType.Value.Trim() == "FXX")
                                        {
                                            //Message Type   ---SMI----
                                            sb.AppendLine(hdMessageType.Value);
                                            //Control Location Details   -----CCL-------
                                            sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                            //AirWay Bill Details  -----AWB------
                                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                            //Arrival Details   ------ARR------
                                            sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                            //Reason for Amendment ----RFA----
                                            sb.AppendLine("RFA" + "/" + ddlAmendmentCode.SelectedItem.Value.Trim());
                                            txtMessagePreview.Text = sb.ToString();
                                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                        }
                                        else
                                            //Generating Freight Express Hold Message
                                            if (hdMessageType.Value.Trim() == "FXH")
                                            {
                                                //Message Type   ---SMI----
                                                sb.AppendLine(hdMessageType.Value);
                                                //Control Location Details   -----CCL-------
                                                sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                //AirWay Bill Details  -----AWB------
                                                sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                //Arrival Details   ------ARR------
                                                sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                    txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                //Hold/Release/Denied Entry Information  ------HLD-------
                                                sb.AppendLine("HLD" + "/" + ddlHoldType.SelectedValue.Trim() + "/" + txtRequestExplanation.Text.Trim());
                                                txtMessagePreview.Text = sb.ToString();
                                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                            }
                                            else
                                                //Generating Freight Status Notification Message
                                                if (hdFSNMsgType.Value.Trim() == "FSN")
                                                {
                                                    //Message Type   ---SMI----
                                                    sb.AppendLine(hdMessageType.Value);
                                                    //Control Location Details   -----CCL-------
                                                    sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                    //AirWay Bill Details  -----AWB------
                                                    sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                    //Arrival Details   ------ARR------
                                                    sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                        txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                    //Airline Status Notification -----ASN-----
                                                    sb.AppendLine("ASN" + "/" + ddlStatusCode.SelectedItem.Value.Trim() + txtActionExplanation.Text);
                                                    //CBP Status Notification   -----CSN-------
                                                    //sb.AppendLine("CSN" + "/" + ddlActionCode.SelectedItem.Value.Trim() + "-" + txtNumberofPieces.Text.Trim() + "/" + txtTransactionDate.Text.Trim() + txtTransactionTime.Text.Trim());
                                                    txtMessagePreview.Text = sb.ToString();
                                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                }
                                                else
                                                    //Generating Freight Status Information Message
                                                    if (hdMessageType.Value.Trim() == "FSI")
                                                    {
                                                        //Message Type   ---SMI----
                                                        sb.AppendLine(hdMessageType.Value);
                                                        //Control Location Details   -----CCL-------
                                                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                        //AirWay Bill Details  -----AWB------
                                                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                        //Arrival Details   ------ARR------
                                                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                        //CBP Status Notification   -----CSN-------
                                                        sb.AppendLine("CSN" + "/" + ddlActionCode.SelectedItem.Value.Trim() + "-" + txtNumberofPieces.Text.Trim() + "/" + txtTransactionDate.Text.Trim().Replace("-", "").Substring(0, 5) + txtTransactionTime.Text.Trim());
                                                        txtMessagePreview.Text = sb.ToString();
                                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                    }
                                                    else
                                                        //Generating Flight Departure Message
                                                        if (hdMessageType.Value.Trim() == "FDM")
                                                        {
                                                            //Message Type   ---SMI----
                                                            sb.AppendLine(hdMessageType.Value);
                                                            //Departure Details   -----DEP-------
                                                            sb.AppendLine("DEP" + "/" + txtDepDetailsImportingCarrier.Text.Trim() + txtDepDetailsFlightNumber.Text.Trim() + "/" + txtActualImportingCarrier.Text.Trim() + txtActualFlightNumber.Text.Trim());
                                                            txtMessagePreview.Text = sb.ToString();
                                                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                        }
                                                        else
                                                            //Generating Freight Error Report Message
                                                            if (hdMessageType.Value.Trim() == "FER")
                                                            {
                                                                //Message Type   ---SMI----
                                                                sb.AppendLine(hdMessageType.Value);
                                                                //Error Report Flight Details   -----ERF-------
                                                                sb.AppendLine(txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" + DateTime.Now.ToString("dd-MMM-yyyy").Replace("-", "").Substring(0, 5));
                                                                //AirWay Bill Details  -----AWB------
                                                                sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                //Error Message   -----ERR-------
                                                                sb.AppendLine("ERR" + "/" + ddlErrorCode.SelectedItem.Text.Trim() + txtErrorMessage.Text);
                                                                txtMessagePreview.Text = sb.ToString();
                                                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                            }
                                                            else
                                                                //Generating Freight Status Query Message
                                                                if (hdMessageType.Value.Trim() == "FSQ")
                                                                {
                                                                    //Message Type   ---SMI----
                                                                    sb.AppendLine(hdMessageType.Value);
                                                                    //Control Location Details   -----CCL-------
                                                                    sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                                    //AirWay Bill Details  -----AWB------
                                                                    sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                    //Freight Status Query   -----FSQ-------
                                                                    sb.AppendLine("FSQ" + "/" + ddlStatusRequestCode.SelectedItem.Value.Trim());
                                                                    txtMessagePreview.Text = sb.ToString();
                                                                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                                }
                                                                else
                                                                    //Generating Freight Status Condition Message
                                                                    if (hdMessageType.Value.Trim() == "FSC")
                                                                    {
                                                                        //Message Type   ---SMI----
                                                                        sb.AppendLine(hdMessageType.Value);
                                                                        //Control Location Details   -----CCL-------
                                                                        sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                                        //AirWay Bill Details  -----AWB------
                                                                        sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                        //Arrival Details   ------ARR------
                                                                        sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                                            txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                                        //Freight Status Condition   -----FSC-------
                                                                        sb.AppendLine("FSC" + "/" + ddlStatusAnswerCode.SelectedItem.Value.Trim());
                                                                        txtMessagePreview.Text = sb.ToString();
                                                                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                                    }
                                                                    else
                                                                        //Generating Freight Status Notification Message
                                                                        if (hdFSNMsgType.Value.Trim() == "FSNInbox")
                                                                        {
                                                                            //Message Type   ---SMI----
                                                                            sb.AppendLine(hdMessageType.Value);
                                                                            //Control Location Details   -----CCL-------
                                                                            sb.AppendLine(txtContLocation.Text.Trim() + txtImportingCarrier.Text.Trim());
                                                                            //AirWay Bill Details  -----AWB------
                                                                            sb.AppendLine(txtawbprefix.Text.Trim() + "-" + txtawbserialnumber.Text.Trim());
                                                                            //Arrival Details   ------ARR------
                                                                            sb.AppendLine("ARR" + "/" + txtImportingCarrier.Text.Trim() + txtFlightNumber.Text.Trim() + "/" +
                                                                                txtScheduledArrDate.Text.Trim().Replace("-", "").Substring(0, 5));
                                                                            //CBP Status Notification   -----CSN-------
                                                                            sb.AppendLine("CSN" + "/" + ddlActionCode.SelectedItem.Value.Trim() + "-" + txtNumberofPieces.Text.Trim() + "/" + txtTransactionDate.Text.Trim().Replace("-", "").Substring(0, 5) + txtTransactionTime.Text.Trim());
                                                                            txtMessagePreview.Text = sb.ToString();
                                                                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                                                                        }
                if (sb != null)
                {

                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Validating Messages
        public bool ValidatingAMSMessages()
        {
            try
            {
                lblStatus.Text = "";
                lblReasonStatus.Text = "";
                lblShipperStatus.Text = "";
                lblConsigneeStatus.Text = "";
                lblDepartureStatus.Text = "";
                lblErrorMessageStatus.Text = "";
                lblFDAStatus.Text = "";
                lblHLDStatus.Text = "";
                lblStatusCarrierStatus.Text = "";
                lblAWBStatus.Text = "";
                lblWBLStatus.Text = "";
                lblArrivalStatus.Text = "";
                lblAgentStatus.Text = "";
                lblCEDStatus.Text = "";
                lblDescriptionStatus.Text = "";
                lblTextStatus.Text = "";
                lblStatusCBPStatus.Text = "";
                lblTransferStatus.Text = "";
                lblStatusQueryStatus.Text = "";
                lblStatusConditionStatus.Text = "";
                bool flag = false;
                //Getting Message Format

                //Validating Mandatory fields for FRI Messages
                if (hdFSNMsgType.Value.Trim() == "FRI")
                {
                    if (ddlHAWB.Items.Count > 0)
                    {
                        if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "" || ddlHAWB.Text == "")
                        {
                            lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                            lblAWBStatus.ForeColor = Color.Red;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                            flag = true;
                        }
                    }
                    else
                    {
                        if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                        {
                            lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                            lblAWBStatus.ForeColor = Color.Red;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                            flag = true;
                        }
                    }
                    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || ddlWBLWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                    {
                        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                        lblWBLStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.SelectedIndex == 0)
                    {
                        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                        lblShipperStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.SelectedIndex == 0)
                    {
                        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                        lblConsigneeStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtLineIdentifier.Text.Trim() == "")
                    {
                        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                        lblFDAStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    { return false; }
                }
                flag = false;
                //Validating Mandatory fields for FXI Messages
                if (hdFSNMsgType.Value.Trim() == "FXI")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || ddlWBLWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                    {
                        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                        lblWBLStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if ((ddlEntryType.SelectedItem.Value == "11" || ddlEntryType.SelectedItem.Value == "86" || ddlEntryType.SelectedItem.Value == "92") && TxtEntryNo.Text.Trim() == "")
                    {
                        if (ddlEntryType.Text.Trim() == "")
                        {
                            lblCEDStatus.Text = "Please Enter CBP Entry Detail(CED) Mandatory fields marked with (*)!";
                            lblCEDStatus.ForeColor = Color.Red;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                            flag = true;
                        }
                    }
                    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.SelectedIndex == 0)
                    {
                        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                        lblShipperStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.SelectedIndex == 0)
                    {
                        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                        lblConsigneeStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtLineIdentifier.Text.Trim() == "")
                    {
                        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                        lblFDAStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }
                }
                flag = false;
                //Validating Mandatory fields for FRC Messages
                if (hdFSNMsgType.Value.Trim() == "FRC")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || ddlWBLWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                    {
                        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                        lblWBLStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.SelectedIndex == 0)
                    {
                        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                        lblShipperStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.SelectedIndex == 0)
                    {
                        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                        lblConsigneeStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtLineIdentifier.Text.Trim() == "")
                    {
                        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                        lblFDAStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (ddlAmendmentCode.Text.Trim() == "")
                    {
                        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                        lblReasonStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    { return false; }
                }
                flag = false;
                //Validating Mandatory fields for FXC Messages
                if (hdFSNMsgType.Value.Trim() == "FXC")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || ddlWBLWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                    {
                        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                        lblWBLStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtShipperName.Text.Trim() == "" || txtCity.Text.Trim() == "" || ddlCountryCode.SelectedIndex == 0)
                    {
                        lblShipperStatus.Text = "Please Enter Shipper(SHP) Mandatory fields marked with (*)!";
                        lblShipperStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtConsigneeName.Text.Trim() == "" || txtConCity.Text.Trim() == "" || ddlConCountryCode.SelectedIndex == 0)
                    {
                        lblConsigneeStatus.Text = "Please Enter Consignee(CNE) Mandatory fields marked with (*)!";
                        lblConsigneeStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtLineIdentifier.Text.Trim() == "")
                    {
                        lblFDAStatus.Text = "Please Enter FDA Mandatory fields marked with (*)!";
                        lblFDAStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (ddlAmendmentCode.Text.Trim() == "")
                    {
                        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                        lblReasonStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    { return false; }
                }

                flag = false;
                //Validating Mandatory fields for FRX Messages
                if (hdFSNMsgType.Value.Trim() == "FRX")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (ddlAmendmentCode.Text.Trim() == "")
                    {
                        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                        lblReasonStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    { return false; }
                }
                flag = false;
                //Validating Mandatory fields for FRH Messages
                if (hdFSNMsgType.Value.Trim() == "FRH")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (ddlHoldType.Text.Trim() == "")
                    {
                        lblHLDStatus.Text = "Please Enter HLD Mandatory fields marked with (*)!";
                        lblHLDStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                flag = false;
                //Validating Mandatory fields for FXX Messages
                if (hdFSNMsgType.Value.Trim() == "FXX")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (ddlAmendmentCode.Text.Trim() == "")
                    {
                        lblReasonStatus.Text = "Please Enter RFA Mandatory fields marked with (*)!";
                        lblReasonStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }
                }
                flag = false;
                //Validating Mandatory fields for FXH Messages
                if (hdFSNMsgType.Value.Trim() == "FXH")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (TxtOrigin.Text.Trim() == "" || txtShipmentDescriptionCode.Text.Trim() == "" || txtPieces.Text.Trim() == "" || ddlWBLWeightCode.Text.Trim() == "" || txtWeight.Text.Trim() == "" || txtCargoDescription.Text.Trim() == "")
                    {
                        lblWBLStatus.Text = "Please Enter WBL Mandatory fields marked with (*)!";
                        lblWBLStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (ddlHoldType.Text.Trim() == "")
                    {
                        lblHLDStatus.Text = "Please Enter HLD Mandatory fields marked with (*)!";
                        lblHLDStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                flag = false;
                //Validating Mandatory fields for FSN Messages
                if (hdFSNMsgType.Value.Trim() == "FSN")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {

                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (ddlStatusCode.Text.Trim() == "")
                    {
                        lblStatusCarrierStatus.Text = "Please Enter Status from Carrier/Handler Mandatory fields marked with (*)!";
                        lblStatusCarrierStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                flag = false;
                //Validating Mandatory fields for FSI Messages
                if (hdFSNMsgType.Value.Trim() == "FSI")
                {

                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (ddlActionCode.Text.Trim() == "" || txtNumberofPieces.Text.Trim() == "" || txtTransactionDate.Text.Trim() == "" || txtTransactionTime.Text.Trim() == "")
                    {

                        lblStatusCBPStatus.Text = "Please Enter CBP Status Notification(CSN) Mandatory fields marked with (*)!";
                        lblStatusCBPStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                flag = false;
                //Validating Mandatory fields for FDM Messages
                if (hdFSNMsgType.Value.Trim() == "FDM")
                {
                    if (txtDepDetailsImportingCarrier.Text.Trim() == "" || txtDepDetailsFlightNumber.Text.Trim() == "" || txtDateOfScheduledArrival.Text.Trim() == "" || txtActualImportingCarrier.Text.Trim() == "" || txtActualFlightNumber.Text.Trim() == "")
                    {

                        lblDepartureStatus.Text = "Please Enter Departure(DEP) Mandatory fields marked with (*)!";
                        lblDepartureStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    { return false; }
                }
                flag = false;
                //Validating Mandatory fields for FER Messages
                if (hdFSNMsgType.Value.Trim() == "FER")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (ddlErrorCode.Text.Trim() == "" || txtErrorMessage.Text.Trim() == "")
                    {
                        lblErrorMessageStatus.Text = "Please Enter Error(ERR) Mandatory fields marked with (*)!";
                        lblErrorMessageStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                flag = false;
                //Validating Mandatory fields for FSQ Messages
                if (hdFSNMsgType.Value.Trim() == "FSQ")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (ddlStatusRequestCode.Text.Trim() == "")
                    {
                        lblStatusQueryStatus.Text = "Please Enter Freight Status Query(FSQ) Mandatory fields marked with (*)!";
                        lblStatusQueryStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                flag = false;
                //Validating Mandatory fields for FSC Messages
                if (hdFSNMsgType.Value.Trim() == "FSC")
                {

                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {
                        //lblStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (ddlStatusAnswerCode.Text.Trim() == "")
                    {
                        lblStatusConditionStatus.Text = "Please Enter Freight Status Condition(FSC) Mandatory fields marked with (*)!";
                        lblStatusConditionStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                flag = false;
                //Validating Mandatory fields for FSN Incoming Messages
                if (hdFSNMsgType.Value.Trim() == "FSNInbox")
                {
                    if (txtawbprefix.Text.Trim() == "" || txtawbserialnumber.Text.Trim() == "")
                    {
                        lblAWBStatus.Text = "Please Enter AWB Mandatory fields marked with (*)!";
                        lblAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (txtImportingCarrier.Text.Trim() == "" || txtFlightNumber.Text.Trim() == "" || txtScheduledArrDate.Text.Trim() == "")
                    {

                        lblArrivalStatus.Text = "Please Enter Arrival(ARR) Mandatory fields marked with (*)!";
                        lblArrivalStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }

                    if (ddlActionCode.Text.Trim() == "" || txtNumberofPieces.Text.Trim() == "" || txtTransactionDate.Text.Trim() == "" || txtTransactionTime.Text.Trim() == "")
                    {

                        lblStatusCBPStatus.Text = "Please Enter CBP Status Notification(CSN) Mandatory fields marked with (*)!";
                        lblStatusCBPStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidatingMessages();</SCRIPT>", false);
                        flag = true;
                    }
                    if (flag == true)
                    {
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        #region Gridview Message Types RowCommand
        protected void grdMessages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (e.CommandName == "MessageType")
                {
                    if (Session["AWBCustomMessages"] != null)
                    {
                        DataRow[] dtRow = (((DataTable)Session["AWBCustomMessages"])).Select("SerialNumber = " + e.CommandArgument.ToString() + "");
                        DataTable dt = new DataTable();
                        dt = ((DataTable)Session["AWBCustomMessages"]).Clone();
                        foreach (DataRow row in dtRow)
                        {
                            dt.ImportRow(row);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["MessageType"].ToString() == "FRI")
                            {
                                PopulateFRI(dt);
                            }
                            else
                                if (dt.Rows[0]["MessageType"].ToString() == "FXI")
                                {
                                    PopulateFXI(dt);
                                }
                                else
                                    if (dt.Rows[0]["MessageType"].ToString() == "FRC")
                                    {
                                        PopulateFRC(dt);
                                    }
                                    else
                                        if (dt.Rows[0]["MessageType"].ToString() == "FXC")
                                        {
                                            PopulateFXC(dt);
                                        }
                                        else
                                            if (dt.Rows[0]["MessageType"].ToString() == "FRX")
                                            {
                                                PopulateFRX(dt);
                                            }
                                            else
                                                if (dt.Rows[0]["MessageType"].ToString() == "FRH")
                                                {
                                                    PopulateFRH(dt);
                                                }
                                                else
                                                    if (dt.Rows[0]["MessageType"].ToString() == "FXX")
                                                    {
                                                        PopulateFXX(dt);
                                                    }
                                                    else
                                                        if (dt.Rows[0]["MessageType"].ToString() == "FXH")
                                                        {
                                                            PopulateFXH(dt);
                                                        }
                                                        else
                                                            if (dt.Rows[0]["MessageType"].ToString() == "FSN")
                                                            {
                                                                if (dt.Rows[0]["MsgType"].ToString() == "Outbox")
                                                                {
                                                                    PopulateFSN(dt);
                                                                }
                                                                else
                                                                {
                                                                    PopulateFSNInbox(dt);
                                                                }
                                                            }
                                                            else
                                                                if (dt.Rows[0]["MessageType"].ToString() == "FSI")
                                                                {
                                                                    PopulateFSI(dt);
                                                                }
                                                                else
                                                                    if (dt.Rows[0]["MessageType"].ToString() == "FDM")
                                                                    {
                                                                        PopulateFDM(dt);
                                                                    }
                                                                    else
                                                                        if (dt.Rows[0]["MessageType"].ToString() == "FER")
                                                                        {
                                                                            PopulateFER(dt);
                                                                        }
                                                                        else
                                                                            if (dt.Rows[0]["MessageType"].ToString() == "FSQ")
                                                                            {
                                                                                PopulateFSQ(dt);
                                                                            }
                                                                            else
                                                                                if (dt.Rows[0]["MessageType"].ToString() == "FSC")
                                                                                {
                                                                                    PopulateFSC(dt);
                                                                                }
                            Session["AWBCustomMessages"] = null;
                            grdMessages.DataSource = null;
                            grdMessages.DataBind();
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                        }

                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Populating Custom Messages

        #region Populate FRI
        public void PopulateFRI(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (ddlHAWB.Items.Count > 0)
                    {
                        //------Header Record--------
                        hdMessageType.Value = row["MessageType"].ToString();
                        hdFSNMsgType.Value = row["MessageType"].ToString();
                        txtMsgType.Text = row["MessageType"].ToString();
                        //---------CCL--------------
                        txtContLocation.Text = row["DestinionCode"].ToString();
                        //----------AWB--------------
                        txtawbprefix.Text = row["AWBPrefix"].ToString();
                        txtawbserialnumber.Text = row["AWBNumber"].ToString();
                        txtconsolidateid.Text = "M";
                        ddlHAWB.Text = row["HAWBNumber"].ToString();
                        txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                        txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();
                        //---------WBL---------------
                        TxtOrigin.Text = row["Origin"].ToString();
                        txtDestination.Text = row["DestinionCode"].ToString();
                        txtCargoDescription.Text = row["WBLCargoDescription"].ToString();
                        txtPieces.Text = row["WBLNumberOfPieces"].ToString();
                        ddlWBLWeightCode.Text = row["WBLWeightIndicator"].ToString();
                        txtWeight.Text = row["WBLWeight"].ToString();
                        txtDateofArrival.Text = row["ArrivalDate"].ToString();
                        //-----------ARR----------------
                        txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                        txtFlightNumber.Text = row["FlightNumber"].ToString();
                        txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                        txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                        txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                        txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                        ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                        txtArrWeight.Text = row["BoardedWeight"].ToString();
                        //----------AGT------------------
                        txtAgent.Text = row["AMSParticipantCode"].ToString();
                        //--------------SHP----------------
                        txtShipperName.Text = row["ShipperName"].ToString();
                        txtShipperAddress.Text = row["ShipperAddress"].ToString();
                        txtCity.Text = row["ShipperCity"].ToString();
                        txtState.Text = row["ShipperState"].ToString();
                        ddlCountryCode.Text = row["ShipperCountry"].ToString();
                        txtPostalCode.Text = row["ShipperPostalCode"].ToString();
                        //--------------CNE----------------
                        txtConsigneeName.Text = row["ConsigneeName"].ToString();
                        txtConStreetAddress.Text = row["ConsigneeAddress"].ToString();
                        txtConCity.Text = row["ConsigneeCity"].ToString();
                        txtConState.Text = row["ConsigneeState"].ToString();
                        ddlConCountryCode.Text = row["ConsigneeCountry"].ToString();
                        txtConPostalCode.Text = row["ConsigneePostalCode"].ToString();
                        //txtTelephoneNo.Text = row["ConsigneePostalCode"].ToString();
                        //-------------TRN---------------------
                        txtTransferDestArpt.Text = row["TransferDestAirport"].ToString();
                        txtDomInt.Text = row["DomesticIdentifier"].ToString();
                        txtBondedCarrierID.Text = row["BondedCarrierID"].ToString();
                        txtOnwardCarrier.Text = row["OnwardCarrier"].ToString();
                        txtBondedPremIdentifier.Text = row["BondedPremisesIdentifier"].ToString();
                        txtInbondControlNo.Text = row["InBondControlNumber"].ToString();
                        //-------------DESCRIPTION------------------
                        ddlDescriptionCountryCode.Text = row["OriginOfGoods"].ToString();
                        txtDeclaredValue.Text = row["DeclaredValue"].ToString();
                        ddlCurrencyCode.Text = row["CurrencyCode"].ToString();
                        txtCommodityCode.Text = row["CommodityCode"].ToString();
                        //---------------FDA------------------
                        txtLineIdentifier.Text = row["LineIdentifier"].ToString();
                    }
                    else
                    {
                        //------Header Record--------
                        hdMessageType.Value = row["MessageType"].ToString();
                        hdFSNMsgType.Value = row["MessageType"].ToString();
                        txtMsgType.Text = row["MessageType"].ToString();
                        //---------CCL--------------
                        txtContLocation.Text = row["DestinionCode"].ToString();
                        //----------AWB--------------
                        txtawbprefix.Text = row["AWBPrefix"].ToString();
                        txtawbserialnumber.Text = row["AWBNumber"].ToString();
                        txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                        ddlHAWB.Text = row["HAWBNumber"].ToString();
                        txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                        txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();
                        //---------WBL---------------
                        TxtOrigin.Text = row["Origin"].ToString();
                        txtDestination.Text = row["DestinionCode"].ToString();
                        txtCargoDescription.Text = row["WBLCargoDescription"].ToString();
                        txtPieces.Text = row["WBLNumberOfPieces"].ToString();
                        ddlWBLWeightCode.Text = row["WBLWeightIndicator"].ToString();
                        txtWeight.Text = row["WBLWeight"].ToString();
                        txtDateofArrival.Text = row["ArrivalDate"].ToString();
                        //-----------ARR----------------
                        txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                        txtFlightNumber.Text = row["FlightNumber"].ToString();
                        txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                        txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                        txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                        txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                        ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                        txtArrWeight.Text = row["BoardedWeight"].ToString();
                        //----------AGT------------------
                        txtAgent.Text = row["AMSParticipantCode"].ToString();
                        //--------------SHP----------------
                        txtShipperName.Text = row["ShipperName"].ToString();
                        txtShipperAddress.Text = row["ShipperAddress"].ToString();
                        txtCity.Text = row["ShipperCity"].ToString();
                        txtState.Text = row["ShipperState"].ToString();
                        ddlCountryCode.Text = row["ShipperCountry"].ToString();
                        txtPostalCode.Text = row["ShipperPostalCode"].ToString();
                        //--------------CNE----------------
                        txtConsigneeName.Text = row["ConsigneeName"].ToString();
                        txtConStreetAddress.Text = row["ConsigneeAddress"].ToString();
                        txtConCity.Text = row["ConsigneeCity"].ToString();
                        txtConState.Text = row["ConsigneeState"].ToString();
                        ddlConCountryCode.Text = row["ConsigneeCountry"].ToString();
                        txtConPostalCode.Text = row["ConsigneePostalCode"].ToString();
                        //txtTelephoneNo.Text = row["ConsigneePostalCode"].ToString();
                        //-------------TRN---------------------
                        txtTransferDestArpt.Text = row["TransferDestAirport"].ToString();
                        txtDomInt.Text = row["DomesticIdentifier"].ToString();
                        txtBondedCarrierID.Text = row["BondedCarrierID"].ToString();
                        txtOnwardCarrier.Text = row["OnwardCarrier"].ToString();
                        txtBondedPremIdentifier.Text = row["BondedPremisesIdentifier"].ToString();
                        txtInbondControlNo.Text = row["InBondControlNumber"].ToString();
                        //-------------DESCRIPTION------------------
                        ddlDescriptionCountryCode.Text = row["OriginOfGoods"].ToString();
                        txtDeclaredValue.Text = row["DeclaredValue"].ToString();
                        ddlCurrencyCode.Text = row["CurrencyCode"].ToString();
                        txtCommodityCode.Text = row["CommodityCode"].ToString();
                        //---------------FDA------------------
                        txtLineIdentifier.Text = row["LineIdentifier"].ToString();
                    }

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FRC
        public void PopulateFRC(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();
                    //---------WBL---------------
                    TxtOrigin.Text = row["Origin"].ToString();
                    txtDestination.Text = row["DestinionCode"].ToString();
                    txtCargoDescription.Text = row["WBLCargoDescription"].ToString();
                    txtPieces.Text = row["WBLNumberOfPieces"].ToString();
                    ddlWBLWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtWeight.Text = row["WBLWeight"].ToString();
                    txtDateofArrival.Text = row["ArrivalDate"].ToString();
                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();
                    //--------------SHP----------------
                    txtShipperName.Text = row["ShipperName"].ToString();
                    txtShipperAddress.Text = row["ShipperAddress"].ToString();
                    txtCity.Text = row["ShipperCity"].ToString();
                    txtState.Text = row["ShipperState"].ToString();
                    ddlCountryCode.Text = row["ShipperCountry"].ToString();
                    txtPostalCode.Text = row["ShipperPostalCode"].ToString();
                    //--------------CNE----------------
                    txtConsigneeName.Text = row["ConsigneeName"].ToString();
                    txtConStreetAddress.Text = row["ConsigneeAddress"].ToString();
                    txtConCity.Text = row["ConsigneeCity"].ToString();
                    txtConState.Text = row["ConsigneeState"].ToString();
                    ddlConCountryCode.Text = row["ConsigneeCountry"].ToString();
                    txtConPostalCode.Text = row["ConsigneePostalCode"].ToString();
                    //txtTelephoneNo.Text = row["ConsigneePostalCode"].ToString();
                    //-------------TRN---------------------
                    txtTransferDestArpt.Text = row["TransferDestAirport"].ToString();
                    txtDomInt.Text = row["DomesticIdentifier"].ToString();
                    txtBondedCarrierID.Text = row["BondedCarrierID"].ToString();
                    txtOnwardCarrier.Text = row["OnwardCarrier"].ToString();
                    txtBondedPremIdentifier.Text = row["BondedPremisesIdentifier"].ToString();
                    txtInbondControlNo.Text = row["InBondControlNumber"].ToString();
                    //-------------DESCRIPTION------------------
                    ddlDescriptionCountryCode.Text = row["OriginOfGoods"].ToString();
                    txtDeclaredValue.Text = row["DeclaredValue"].ToString();
                    ddlCurrencyCode.Text = row["CurrencyCode"].ToString();
                    txtCommodityCode.Text = row["CommodityCode"].ToString();
                    //---------------FDA------------------
                    txtLineIdentifier.Text = row["LineIdentifier"].ToString();
                    //----------------Reason---------------
                    ddlAmendmentCode.Text = row["AmendmentCode"].ToString();
                    lblExplanation.Text = row["AmendmentExplanation"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FRX
        public void PopulateFRX(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();

                    //----------------Reason---------------
                    ddlAmendmentCode.Text = row["AmendmentCode"].ToString();
                    lblExplanation.Text = row["AmendmentExplanation"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FXI
        public void PopulateFXI(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();
                    //---------WBL---------------
                    TxtOrigin.Text = row["Origin"].ToString();
                    txtDestination.Text = row["DestinionCode"].ToString();
                    txtCargoDescription.Text = row["WBLCargoDescription"].ToString();
                    txtPieces.Text = row["WBLNumberOfPieces"].ToString();
                    ddlWBLWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtWeight.Text = row["WBLWeight"].ToString();
                    txtDateofArrival.Text = row["ArrivalDate"].ToString();
                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();
                    //----------CED------------------
                    //ddlEntryType.Text = row["ImportingCarrier"].ToString();
                    //TxtEntryNo.Text = row["AMSParticipantCode"].ToString();
                    //--------------SHP----------------
                    txtShipperName.Text = row["ShipperName"].ToString();
                    txtShipperAddress.Text = row["ShipperAddress"].ToString();
                    txtCity.Text = row["ShipperCity"].ToString();
                    txtState.Text = row["ShipperState"].ToString();
                    ddlCountryCode.Text = row["ShipperCountry"].ToString();
                    txtPostalCode.Text = row["ShipperPostalCode"].ToString();
                    //--------------CNE----------------
                    txtConsigneeName.Text = row["ConsigneeName"].ToString();
                    txtConStreetAddress.Text = row["ConsigneeAddress"].ToString();
                    txtConCity.Text = row["ConsigneeCity"].ToString();
                    txtConState.Text = row["ConsigneeState"].ToString();
                    ddlConCountryCode.Text = row["ConsigneeCountry"].ToString();
                    txtConPostalCode.Text = row["ConsigneePostalCode"].ToString();
                    //txtTelephoneNo.Text = row["ConsigneePostalCode"].ToString();
                    //-------------TRN---------------------
                    txtTransferDestArpt.Text = row["TransferDestAirport"].ToString();
                    txtDomInt.Text = row["DomesticIdentifier"].ToString();
                    txtBondedCarrierID.Text = row["BondedCarrierID"].ToString();
                    txtOnwardCarrier.Text = row["OnwardCarrier"].ToString();
                    txtBondedPremIdentifier.Text = row["BondedPremisesIdentifier"].ToString();
                    txtInbondControlNo.Text = row["InBondControlNumber"].ToString();
                    //-------------DESCRIPTION------------------
                    ddlDescriptionCountryCode.Text = row["OriginOfGoods"].ToString();
                    txtDeclaredValue.Text = row["DeclaredValue"].ToString();
                    ddlCurrencyCode.Text = row["CurrencyCode"].ToString();
                    txtCommodityCode.Text = row["CommodityCode"].ToString();
                    //---------------FDA------------------
                    txtLineIdentifier.Text = row["LineIdentifier"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FXC
        public void PopulateFXC(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();
                    //---------WBL---------------
                    TxtOrigin.Text = row["Origin"].ToString();
                    txtDestination.Text = row["DestinionCode"].ToString();
                    txtCargoDescription.Text = row["WBLCargoDescription"].ToString();
                    txtPieces.Text = row["WBLNumberOfPieces"].ToString();
                    ddlWBLWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtWeight.Text = row["WBLWeight"].ToString();
                    txtDateofArrival.Text = row["ArrivalDate"].ToString();
                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();
                    //----------CED------------------
                    //ddlEntryType.Text = row["ImportingCarrier"].ToString();
                    //TxtEntryNo.Text = row["AMSParticipantCode"].ToString();
                    //--------------SHP----------------
                    txtShipperName.Text = row["ShipperName"].ToString();
                    txtShipperAddress.Text = row["ShipperAddress"].ToString();
                    txtCity.Text = row["ShipperCity"].ToString();
                    txtState.Text = row["ShipperState"].ToString();
                    ddlCountryCode.Text = row["ShipperCountry"].ToString();
                    txtPostalCode.Text = row["ShipperPostalCode"].ToString();
                    //--------------CNE----------------
                    txtConsigneeName.Text = row["ConsigneeName"].ToString();
                    txtConStreetAddress.Text = row["ConsigneeAddress"].ToString();
                    txtConCity.Text = row["ConsigneeCity"].ToString();
                    txtConState.Text = row["ConsigneeState"].ToString();
                    ddlConCountryCode.Text = row["ConsigneeCountry"].ToString();
                    txtConPostalCode.Text = row["ConsigneePostalCode"].ToString();
                    //txtTelephoneNo.Text = row["ConsigneePostalCode"].ToString();
                    //-------------TRN---------------------
                    txtTransferDestArpt.Text = row["TransferDestAirport"].ToString();
                    txtDomInt.Text = row["DomesticIdentifier"].ToString();
                    txtBondedCarrierID.Text = row["BondedCarrierID"].ToString();
                    txtOnwardCarrier.Text = row["OnwardCarrier"].ToString();
                    txtBondedPremIdentifier.Text = row["BondedPremisesIdentifier"].ToString();
                    txtInbondControlNo.Text = row["InBondControlNumber"].ToString();
                    //-------------DESCRIPTION------------------
                    ddlDescriptionCountryCode.Text = row["OriginOfGoods"].ToString();
                    txtDeclaredValue.Text = row["DeclaredValue"].ToString();
                    ddlCurrencyCode.Text = row["CurrencyCode"].ToString();
                    txtCommodityCode.Text = row["CommodityCode"].ToString();
                    //---------------FDA------------------
                    txtLineIdentifier.Text = row["LineIdentifier"].ToString();
                    //----------------Reason---------------
                    ddlAmendmentCode.Text = row["AmendmentCode"].ToString();
                    lblExplanation.Text = row["AmendmentExplanation"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FXX
        public void PopulateFXX(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();

                    //----------------Reason---------------
                    ddlAmendmentCode.Text = row["AmendmentCode"].ToString();
                    txtExplanation.Text = row["AmendmentExplanation"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FDM
        public void PopulateFDM(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();

                    //----------------Departure---------------
                    txtDepDetailsImportingCarrier.Text = row["DeptImportingCarrier"].ToString();
                    txtDepDetailsFlightNumber.Text = row["DeptFlightNumber"].ToString();
                    txtDateOfScheduledArrival.Text = row["DeptScheduledArrivalDate"].ToString();
                    txtLiftoffDate.Text = row["LiftoffDate"].ToString();
                    txtLiftoffTime.Text = row["LiftoffTime"].ToString();
                    txtActualImportingCarrier.Text = row["DeptActualImportingCarrier"].ToString();
                    txtActualFlightNumber.Text = row["DeptActualFlightNumber"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FSQ
        public void PopulateFSQ(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();

                    //----------------Status Query---------------
                    ddlStatusRequestCode.Text = row["StatusRequestCode"].ToString();


                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FER
        public void PopulateFER(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();

                    //----------------Error---------------
                    ddlErrorCode.Text = row["ErrorCode"].ToString();
                    txtErrorMessage.Text = row["ErrorMessage"].ToString();


                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FRH
        public void PopulateFRH(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();

                    //----------------HLD---------------
                    ddlHoldType.Text = row["RequestType"].ToString();
                    txtRequestExplanation.Text = row["RequestExplanation"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FXH
        public void PopulateFXH(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();

                    //----------------HLD---------------
                    ddlHoldType.Text = row["RequestType"].ToString();
                    txtRequestExplanation.Text = row["RequestExplanation"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FSN
        public void PopulateFSN(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();


                    ////----------------StatusFromCBP---------------
                    //if (row["CSNActionCode"].ToString() != "")
                    //{
                    //    ddlActionCode.Text = row["CSNActionCode"].ToString();
                    //}
                    //txtNumberofPieces.Text = row["CSNPieces"].ToString();
                    //txtTransactionDate.Text = row["TransactionDate"].ToString();
                    //txtTransactionTime.Text = row["TransactionTime"].ToString();
                    //if (row["CSNEntryType"].ToString() != "")
                    //{
                    //    ddlCBPEntryCodes.Text = row["CSNEntryType"].ToString();
                    //}
                    //txtEntryNumber.Text = row["CSNEntryNumber"].ToString();
                    //txtremarks.Text = row["CSNRemarks"].ToString();

                    //----------------StatusFromCarrier--------------
                    if (row["ASNStatusCode"].ToString() != "")
                    {
                        ddlStatusCode.Text = row["ASNStatusCode"].ToString();
                    }
                    txtActionExplanation.Text = row["ASNActionExplanation"].ToString();
                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FSI
        public void PopulateFSI(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();

                    //----------------StatusFromCBP---------------
                    if (row["CSNActionCode"].ToString() != "")
                    {
                        ddlActionCode.Text = row["CSNActionCode"].ToString();
                    }
                    txtNumberofPieces.Text = row["CSNPieces"].ToString();
                    txtTransactionDate.Text = row["TransactionDate"].ToString();
                    txtTransactionTime.Text = row["TransactionTime"].ToString();
                    if (row["CSNEntryType"].ToString() != "")
                    {
                        ddlCBPEntryCodes.Text = row["CSNEntryType"].ToString();
                    }
                    txtEntryNumber.Text = row["CSNEntryNumber"].ToString();
                    txtremarks.Text = row["CSNRemarks"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FSC
        public void PopulateFSC(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = row["MessageType"].ToString();
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    if (row["HAWBNumber"].ToString() != "")
                    {
                        ddlHAWB.Text = row["HAWBNumber"].ToString();
                    }
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //---------WBL---------------
                    TxtOrigin.Text = row["Origin"].ToString();
                    txtDestination.Text = row["DestinionCode"].ToString();
                    txtCargoDescription.Text = row["WBLCargoDescription"].ToString();
                    txtPieces.Text = row["WBLNumberOfPieces"].ToString();
                    ddlWBLWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtWeight.Text = row["WBLWeight"].ToString();
                    txtDateofArrival.Text = row["ArrivalDate"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();

                    //-------------TRN---------------------
                    txtTransferDestArpt.Text = row["TransferDestAirport"].ToString();
                    txtDomInt.Text = row["DomesticIdentifier"].ToString();
                    txtBondedCarrierID.Text = row["BondedCarrierID"].ToString();
                    txtOnwardCarrier.Text = row["OnwardCarrier"].ToString();
                    txtBondedPremIdentifier.Text = row["BondedPremisesIdentifier"].ToString();
                    txtInbondControlNo.Text = row["InBondControlNumber"].ToString();

                    //----------------StatusFromCBP---------------
                    if (row["CSNActionCode"].ToString() != "")
                    {
                        ddlActionCode.Text = row["CSNActionCode"].ToString();
                    }
                    txtNumberofPieces.Text = row["CSNPieces"].ToString();
                    txtTransactionDate.Text = row["TransactionDate"].ToString();
                    txtTransactionTime.Text = row["TransactionTime"].ToString();
                    if (row["CSNEntryType"].ToString() != "")
                    {
                        ddlCBPEntryCodes.Text = row["CSNEntryType"].ToString();
                    }
                    txtEntryNumber.Text = row["CSNEntryNumber"].ToString();
                    txtremarks.Text = row["CSNRemarks"].ToString();
                    //----------------StatusFromCarrier--------------
                    if (row["ASNStatusCode"].ToString() != "")
                    {
                        ddlStatusCode.Text = row["ASNStatusCode"].ToString();
                    }
                    txtActionExplanation.Text = row["ASNActionExplanation"].ToString();
                    //----------------StatusCondition--------------
                    if (row["StatusAnswerCode"].ToString() != "")
                    {
                        ddlStatusAnswerCode.Text = row["StatusAnswerCode"].ToString();
                    }
                    //----------------Text---------------
                    txtInformation.Text = row["Information"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion

        #region Populate FSN Inbox
        public void PopulateFSNInbox(DataTable dt)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    //------Header Record--------
                    hdMessageType.Value = row["MessageType"].ToString();
                    hdFSNMsgType.Value = "FSNInbox";
                    txtMsgType.Text = row["MessageType"].ToString();
                    //---------CCL--------------
                    txtContLocation.Text = row["DestinionCode"].ToString();
                    //----------AWB--------------
                    txtawbprefix.Text = row["AWBPrefix"].ToString();
                    txtawbserialnumber.Text = row["AWBNumber"].ToString();
                    txtconsolidateid.Text = row["ConsolidationIdentifier"].ToString();
                    ddlHAWB.Text = row["HAWBNumber"].ToString();
                    txtpackagetrackid.Text = row["PackageTrackingIdentifier"].ToString();
                    txtPartArrival.Text = row["AWBPartArrivalReference"].ToString();

                    //-----------ARR----------------
                    txtImportingCarrier.Text = row["ImportingCarrier"].ToString();
                    txtFlightNumber.Text = row["FlightNumber"].ToString();
                    txtScheduledArrDate.Text = row["ArrivalDate"].ToString();
                    txtPartArrivalReference.Text = row["ARRPartArrivalReference"].ToString();
                    txtBoardedQtyIdentifier.Text = row["BoardedQuantityIdentifier"].ToString();
                    txtBoardedPieceCount.Text = row["BoardedPieceCount"].ToString();
                    ddlArrWeightCode.Text = row["WBLWeightIndicator"].ToString();
                    txtArrWeight.Text = row["BoardedWeight"].ToString();


                    //----------------StatusFromCBP---------------
                    if (row["CSNActionCode"].ToString() != "")
                    {
                        ddlActionCode.Text = row["CSNActionCode"].ToString();
                    }
                    txtNumberofPieces.Text = row["CSNPieces"].ToString();
                    txtTransactionDate.Text = row["TransactionDate"].ToString();
                    txtTransactionTime.Text = row["TransactionTime"].ToString();
                    if (row["CSNEntryType"].ToString() != "")
                    {
                        ddlCBPEntryCodes.Text = row["CSNEntryType"].ToString();
                    }
                    txtEntryNumber.Text = row["CSNEntryNumber"].ToString();
                    txtremarks.Text = row["CSNRemarks"].ToString();

                }

            }
            catch (Exception ex)
            { }


        }
        #endregion



        #endregion

        #region Button Audit
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                foreach (GridViewRow row in GRDBooking.Rows)
                {
                    if (((RadioButton)row.FindControl("rdbCheck")).Checked)
                    {

                        string AWBNumber = ((Label)row.FindControl("AWBNo")).Text.Trim();

                        Response.Redirect("~/frmCustomsAuditTrail.aspx?AWB=" + AWBNumber);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Print
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string FileName = ":" + txtawbprefix.Text.Trim() + txtawbserialnumber.Text.Trim();
                if (!ValidatingAMSMessages())
                { return; }

                EncodeMessage();
                if (txtMessagePreview.Text != "")
                {
                    Response.Clear();
                    Response.ContentType = "application/txt";
                    Response.AddHeader("content-disposition", "attachment; filename= " + FileName);
                    Response.Write(txtMessagePreview.Text);
                    Response.End();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowAccordianDetails();</SCRIPT>", false);
                    //Session["CustomsMessages"] = txtMessagePreview.Text;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

    }
}
