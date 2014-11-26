using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BAL;
using System.Data;
using System.Data.Sql;
using QID.DataAccess;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class frmReverseBidding : System.Web.UI.Page
    {

        #region Variables
        DateTime timeTimer;
        HomeBL objBLL = new HomeBL();
        BookingBAL objBkgBL = new BookingBAL();
        ListBookingBAL objBAL = new ListBookingBAL();
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.Red;
            if (!IsPostBack)
            {
                
                txtFlightDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                timeTimer = DateTime.Now;
                LoadCommodityDropdown();
                LoadDestination();
                GetFlightPrefix();
                TXTAgentCode.Focus();
                btnBookNow.Enabled = false;

                if (Convert.ToString(Session["AgentCode"]) != "")
                {
                    TXTAgentCode.Text = Convert.ToString(Session["AgentCode"]);
                    txtAgentName.Text = Convert.ToString(Session["AgentName"]);
                    TXTAgentCode.ReadOnly = true;
                    txtAgentName.ReadOnly = true;
                    IBOrigin.Visible = false;
                }
                try
                {
                    if (Request.QueryString["command"].ToString() == "Edit")
                    {
                        AutoLoadList();
                        TXTAgentCode.Enabled = false;
                        txtAgentName.Enabled = false;
                        ddlSource.Enabled = false;
                        ddlDest.Enabled = false;
                        txtFlightDate.Enabled = false;
                        ddlFlight.Enabled = false;
                        ddlFlightPrefix.Enabled = false;
                        ddlComodityCd.Enabled = false;
                        btnCheck.Visible = false;
                        //btnSave.Enabled = false;

                    }
                }
                catch (Exception ex)
                { }
            }
        }
        #endregion

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {

            string[] orgdest = new ConBooking_GHA().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%')", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        #region newMethod
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetStation(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'

            SqlDataAdapter dad = new SqlDataAdapter("SELECT AirportCode from AirportMaster where AirportName like '" + prefixText + "%' or AirportCode like '" + prefixText + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        #endregion

        # region Get Flight Prefix
        public void GetFlightPrefix()
        {
             try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                DataSet dsInstance = new DataSet();
                //string FlightPrefix;
                dsInstance = objSQL.SelectRecords("GetCurrentInstance");
                string current = dsInstance.Tables[0].Rows[0][0].ToString();
              //  FlightPrefix = ddlFlightPrefix.SelectedValue.ToString().Trim();
                {
                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    //if (objBAL.GetAllFlightsNew(source, dest, date, ref dsResult, ref errormessage))
                    string procedure = "sp_GetFlightPrefix";
                    dsResult = objSQL.SelectRecords(procedure);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                               ddlFlightPrefix.Items.Clear();
                               ddlFlightPrefix.DataSource = dsResult.Tables[0];
                               ddlFlightPrefix.DataTextField = "PartnerCode";
                               ddlFlightPrefix.DataValueField = "PartnerCode";
                               ddlFlightPrefix.DataBind();
                           
                           ddlFlightPrefix.SelectedValue = current;
                          // GetFlights(current);

                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "" + errormessage;
                        lblStatus.ForeColor = Color.Red;
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }

        # endregion

        #region getFlights
        public void GetFlights(string FlightPrefix)
        {
            try
            {
                lblStatus.Text = "";
                string source, dest, date;
                source = dest = date = "";
                DateTime dt;
                if (txtFlightDate.Text == "")
                {
                    lblStatus.Text = "Please enter the flight date !";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (ddlSource.SelectedItem.Text.Trim() == ddlDest.SelectedItem.Text.Trim())
                {
                    lblStatus.Text = "Source and Destination can't be same !";
                }
                try
                {
                    string day = txtFlightDate.Text.Substring(0, 2);
                    string mon = txtFlightDate.Text.Substring(3, 2);
                    string yr = txtFlightDate.Text.Substring(6, 4);
                    date = yr + "/" + mon + "/" + day;
                    dt = Convert.ToDateTime(date);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (ddlSource.Text.Trim() == "Select" && ddlDest.Text.Trim() == "Select")
                {
                    //ddlFlight.Items.Clear();
                    //ddlFlight.Items.Add(new ListItem("All", "All"));
                    //ddlFlight.SelectedIndex = 0;
                    lblStatus.Text = "Please Select Source and Destination";
                    return;
                }
                else
                {
                    string prefix = FlightPrefix;
                    source = ddlSource.SelectedItem.Value;
                    dest = ddlDest.SelectedItem.Value;
                    //source = ddlSource.SelectedItem.Text.Trim();
                    //dest = ddlDest.SelectedItem.Text.Trim();
                    //date = "";// txtDate.Text;
                    date = dt.ToString("MM/dd/yyyy");// txtDate.Text;


                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    dsResult= objBAL.GetAllFlightsNewForQuotes(prefix, source, dest, date);
                    

                        if (dsResult != null)
                        {
                            if (dsResult.Tables.Count > 0)
                            {
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    ddlFlight.Items.Clear();
                                    //DataRow row = dsResult.Tables[0].NewRow();
                                    //row["FltNumber"] = "Select";
                                    //dsResult.Tables[0].Rows.Add(row);
                                    //ddlFlight.DataSource = dsResult.Tables[0].Copy();
                                    ddlFlight.DataSource = dsResult.Tables[0];
                                    ddlFlight.DataTextField = "FltNumber";
                                    //ddlFlight.DataValueField = "DeptTime";
                                    ddlFlight.DataValueField = "FltNumber";
                                    ddlFlight.DataBind();
                                    ddlFlight.Items.Insert(0, new ListItem("Select", ""));
                                    ddlFlight.SelectedIndex = -1;
                                }
                                else
                                {
                                    lblStatus.Text = "No Flight found for given criteria";
                                    lblStatus.ForeColor = Color.Red;
                                }
                            }
                        }
                    
                    else
                    {
                        lblStatus.Text = "" + errormessage;
                        lblStatus.ForeColor = Color.Red;
                    }
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region ddlFlight_SelectedIndexChanged
        protected void ddlFlight_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFlight.SelectedItem.Text.Trim() != "Select")
            {
                //txtDeptTime.Text = ddlFlight.SelectedValue;
                if (TXTAgentCode.Text.Trim() != "")
                {
                    pnlBookingWeight.Visible = true;
                }
                else
                {
                    lblStatus.Text = "Please Enter Agent Code";
                    lblStatus.ForeColor = Color.Red;
                }
            }
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (btnClear.Text == "Clear")
            {
                //btnSave.Visible = false;
                TXTAgentCode.Text = "";
                txtAgentQID.Text = "";
                txtAgentName.Text = "";
                ddlSource.SelectedIndex = 0;
                ddlDest.SelectedIndex = 0;
                ddlFlight.Items.Clear();
                ddlFlight.Items.Add("Select");
                ddlComodityCd.SelectedIndex = 0;
                txtDeptTime.Text = "";
                txtBookingWgt.Text = "";
                txtBidPrice.Text = "";
                txtFlightDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtPCS.Text = "";
                //pnlBidRate.Visible = false;
                //pnlBookingWeight.Visible = false;
                //pnlBookTimer.Visible = false;

                imgBidAproved.Visible = false;
                imgBidRejected.Visible = false;
                imgInvalidBkgWgt.Visible = false;
                imgValidBkgWgt.Visible = false;
                ddlFlight.Enabled = true;
                ddlSource.Enabled = true;
                ddlDest.Enabled = true;
                ddlComodityCd.Enabled = true;
                txtBookingWgt.Enabled = true;
                txtBidPrice.Enabled = true;
            }
            else
            {
                Response.Redirect("~/frmReverseBidding.aspx",false);
            }
        }
        #endregion

        #region ddlSource_IndexChanged
        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSource.SelectedIndex > 0 && ddlDest.SelectedIndex > 0)
            {
                string prefix = ddlFlightPrefix.SelectedItem.Text.Trim();
                GetFlights(prefix);

            }
           
        }
        #endregion

        public void txtFlightDate_Changed(object sender, EventArgs e)
        {
            if (ddlSource.SelectedIndex > 0 && ddlDest.SelectedIndex > 0)
            {
                string prefix = ddlFlightPrefix.SelectedItem.Text.Trim();
                GetFlights(prefix);

            }

        }

        #region ddlDest_IndexChanged
        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlSource.SelectedIndex > 0 && ddlDest.SelectedIndex > 0)
            {
                string prefix = ddlFlightPrefix.SelectedItem.Text.Trim();
                GetFlights(prefix);

            }
           
        }
        #endregion

        #region Load Destination
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        private void LoadDestination()
        {
            try
            {
                DataSet ds = objBkgBL.GetDestinationsForSource("");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlSource.Items.Clear();
                            ddlDest.Items.Clear();
                            ddlSource.Items.Add("Select");
                            ddlDest.Items.Add("Select");
                            ddlSource.DataSource = ds.Tables[0];
                            ddlDest.DataSource = ds.Tables[0];
                            ddlSource.DataTextField = "Airport";
                            ddlSource.DataValueField = "AirportCode";
                            ddlSource.DataBind();
                            ddlSource.SelectedIndex = 0;
                            ddlDest.DataTextField = "Airport";
                            ddlDest.DataValueField = "AirportCode";
                            ddlDest.DataBind();
                            ddlDest.SelectedIndex = 0;
                        }
                    }
                    
                    //DataRow row = ds.Tables[0].NewRow();

                    //row["AirportCode"] = "Select";
                    //ds.Tables[0].Rows.Add(row);


                    //ddlDest.DataSource = ds;
                    //ddlDest.DataMember = ds.Tables[0].TableName;
                    //ddlDest.DataTextField = "AirportCode";
                    //ddlDest.DataValueField = "AirportCode";
                    //ddlDest.DataBind();

                    //ddlDest.Text = "Select";

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load Location Dropdown

        #region Load Commodity Dropdown
        public void LoadCommodityDropdown()
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet ds = da.SelectRecords("spGetCommCodeDesc");
                //DataSet ds = objBkgBL.GetCommodityList("");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlComodityCd.Items.Clear();
                            ddlComodityCd.DataSource = ds.Tables[0];
                            ddlComodityCd.DataTextField = "CommDesc";
                            ddlComodityCd.DataValueField = "CommodityCode";
                            ddlComodityCd.DataBind();
                            ddlComodityCd.Items.Insert(0, new ListItem("Select"));
                            ddlComodityCd.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion Load Commodity Dropdown

        #region CheckFlightAvailibility
        private bool CheckFlightAvailibility(string source, string dest, string flightid, float agentWeight)
        {
            bool flag = true;
            try
            {
                string procedure = "Spgetcapacityplanningdata_test";
                string[] paramname = new string[] { "Origin", "Destination", "FltNo" };
                object[] paramvalue = new object[] {source,dest,flightid};
                SqlDbType[] paramtype = new SqlDbType[] {SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar};
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet ds = da.SelectRecords(procedure, paramname,paramvalue,paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                float AvailableWeight = float.Parse(ds.Tables[0].Rows[0][9].ToString());
                                if (AvailableWeight > agentWeight)
                                    flag = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        #region btnCheck_Click
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (ddlComodityCd.SelectedItem.Text.ToString().Trim() == "Select")
            {
                lblStatus.Text = "Please select Comodity Code";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            else if (txtBookingWgt.Text.Trim() == "")
            {
                lblStatus.Text = "Please enter Booking Weight";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            else
            {
                bool flag = CheckFlightAvailibility(ddlSource.SelectedItem.Text.Trim(), ddlDest.SelectedItem.Text.Trim(), ddlFlight.SelectedItem.Text.Trim(), float.Parse( txtBookingWgt.Text.Trim()));
                if (flag == true)
                {
                    lblStatus.Text = "Weight Available";
                    lblStatus.ForeColor = Color.Green;
                    imgValidBkgWgt.Visible = true;
                    pnlBidRate.Visible = true;
                    ddlFlight.Enabled = false;
                    ddlSource.Enabled = false;
                    ddlDest.Enabled = false;
                    ddlComodityCd.Enabled = false;
                    txtBookingWgt.Enabled = false;
                }
                else
                {
                    imgInvalidBkgWgt.Visible = true;
                    lblStatus.Text = "Unavailabale Weight";
                    lblStatus.ForeColor = Color.Red;
                    
                    return;
                }
            }
        }
        #endregion

        #region btnBideRate_Click
        protected void btnBidRate_Click(object sender, EventArgs e)
        {
            if (txtBidPrice.Text.Trim() == "")
            {
                lblStatus.Text = "Invalid Quote Rate";
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                bool flag = false;
                flag = checkBideRate(ddlSource.SelectedItem.Text.Trim(), ddlDest.SelectedItem.Text.Trim(), ddlFlight.SelectedItem.Text.Trim(), ddlComodityCd.SelectedValue.ToString().Trim(), txtBookingWgt.Text.Trim());
                if (flag == true)
                {
                    lblStatus.Text = "Quote rate is Approved.";
                    lblStatus.ForeColor = Color.Green;
                    imgBidAproved.Visible = true;
                    txtBidPrice.Enabled = false;
                    //pnlBookTimer.Visible = true;
                    btnSave.Visible = true;
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>checkval();</SCRIPT>");
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('a');</SCRIPT>");
                }
                else
                {
                    lblStatus.Text = "Quote Rate is Rejected";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
        }
        #endregion

        #region checkBideRate
        private bool checkBideRate(string Origin, string Dest, string Flight, string Comodity, string BkWeight)
        {
            bool flag = false;
            try
            {
                //string procedure = "spGetDiscountedRate";
                string procedure = "spGetDiscountedRateForBidding";
                string[] paramname = new string[] { "Origin", "Destination", "FltNo", "Commodity", "BookedWt" };
                object[] paramvalue = new object[] { Origin, Dest, Flight,Comodity,BkWeight };
                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float };
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet ds = da.SelectRecords(procedure, paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string result = ds.Tables[0].Rows[0][0].ToString();
                            if (result.Trim().ToUpper() == "A")
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        #region TXTAgentCode_TextChanged
        protected void TXTAgentCode_TextChanged(object sender, EventArgs e)
        {

            try
            {

                string[] orgdest = new ConBooking_GHA().GetOrgDest();

                string TranAccount = string.Empty, Remarks = string.Empty, TranType = string.Empty;
                decimal BGAmount = 0, AWBPrevAmt = 0, BankGAmt = 0, ThrValue = 0, CreditPer = 0;
                bool ValidateBG = false;

                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentName from dbo.AgentMaster where AgentCode ='" + TXTAgentCode.Text.Trim() + "'", con);
                DataSet ds = new DataSet();
                dad.Fill(ds);

                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                {
                    txtAgentName.Text = ds.Tables[0].Rows[0][0].ToString();

                    TXTAgentCode.Text = TXTAgentCode.Text.ToUpper().Trim();

                    //For walk-in customers make payment mode as PP
                    if (TXTAgentCode.Text.Trim().Contains("WALKIN"))
                    {
                        //((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Text = "PP";
                        //((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Enabled = false;
                    }
                    else
                    {
                        //((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Text = "Select";
                        //((DropDownList)grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode")).Enabled = true;
                    }

                    //Check for BG Account

                    //bool blnAllow = CheckforAgentsBalance(TXTAgentCode.Text.Trim(), 0, "",
                    //    ref TranAccount, ref BGAmount, ref AWBPrevAmt, ref BankGAmt, ref ThrValue, ref ValidateBG);

                    if (BankGAmt > 0)
                    {
                        CreditPer = (BGAmount / BankGAmt) * 100;

                        if (CreditPer < ThrValue || CreditPer < 0)
                        {
                            //rateprocessstatus.ForeColor = Color.Red;
                            //rateprocessstatus.Text = "Agent account balance is below Threshold value.";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                            return;
                        }
                        else
                        { 
                           // rateprocessstatus.Text = "";
                        }
                    }
                    //End
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Agent code invalid.";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                    return;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveAgentQuote() == true)
            {
                #region for Master Audit Log
                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                #region Prepare Parameters
                object[] Params = new object[7];
                int k = 0;

                //0
                Params.SetValue("Agent Quote", k);
                k++;

                //1
                Params.SetValue(TXTAgentCode.Text.Trim(), k);
                k++;

                //2
                Params.SetValue("ADD", k);
                k++;

                //3
                string Msg = "New Agent Quote For " + TXTAgentCode.Text.Trim();
                Params.SetValue(Msg, k);
                k++;

                //4
                string Desc = "";
                if (ddlSource.SelectedIndex > 0 && ddlDest.SelectedIndex > 0)
                    Desc = "Org:" + ddlSource.SelectedValue + "-Dest:" + ddlDest.SelectedValue + "-FltNo:" + ddlFlight.SelectedItem.Text + "-FltDt:"+txtFlightDate.Text;
                else
                    Desc = "FltNo:" + ddlFlight.SelectedItem.Text + "-FltDt:"+txtFlightDate.Text;
                Params.SetValue(Desc, k);
                k++;

                //5
                Params.SetValue(Session["UserName"], k);
                k++;

                //6
                Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                k++;


                #endregion Prepare Parameters
                ObjMAL.AddMasterAuditLog(Params);
                #endregion

                lblStatus.Text = "Record Saved Successfully";
                lblStatus.ForeColor = Color.Green;
                btnClear_Click(sender, e);
            }
            
        }
        #endregion

        #region SaveAgentQuote
        private bool SaveAgentQuote()
        {
            bool flag = false;
            try
            {
                string SerialNumber = "";
                try
                {
                    if (Request.QueryString["AgentQuoteID"].ToString() != null || Request.QueryString["AgentQuoteID"].ToString() != "")
                    {
                        SerialNumber = Request.QueryString["AgentQuoteID"].ToString();
                    }
                }
                catch (Exception ex) 
                {
                    SerialNumber = "";
                }
                SQLServer objSQL = new SQLServer(BAL.Global.GetConnectionString());
                string procedure = "spAddAgentQuote";
                string[] paramname = new string[] { "AgentCode",
                                                    "AgentName",
                                                    "Origin",
                                                    "Dest",
                                                    "FlightNo",
                                                    "FlightDate",
                                                    "ComodityCode",
                                                    "FreightWeight",
                                                    "FreightRate",
                                                    "UpdatedBy",
                                                    "UpdatedOn",
                                                    "StationCode",
                                                    "IsBooked",
                                                    "FreightPcs",
                                                    "SerialNumber"
                                                        };
                
                object[] paramvalue = new object[] {TXTAgentCode.Text.Trim(),
                                                    txtAgentName.Text.Trim(),
                                                    ddlSource.SelectedItem.Value,
                                                    ddlDest.SelectedItem.Value,
                                                    //ddlSource.SelectedItem.Text.Trim(),
                                                    //ddlDest.SelectedItem.Text.Trim(),
                                                    ddlFlight.SelectedItem.Text.Trim(),
                                                    //DateTime.Parse(txtFlightDate.Text.Trim()).ToString(),
                                                    DateTime.ParseExact(txtFlightDate.Text.Trim(),"dd/MM/yyyy",null).ToString(),
                                                    ddlComodityCd.SelectedValue.ToString().Trim(),
                                                    txtBookingWgt.Text.Trim(),
                                                    txtBidPrice.Text.Trim(),
                                                    Session["UserName"].ToString(), 
                                                    DateTime.Now.ToString(),
                                                    Session["Station"].ToString(),
                                                    "false",
                                                    txtPCS.Text,
                                                    SerialNumber};
                
                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, 
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.DateTime,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.DateTime,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar};
                
                flag = objSQL.InsertData(procedure, paramname, paramtype, paramvalue);
            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        protected void ddlFlightPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetFlights(ddlFlightPrefix.SelectedItem.Text.Trim());
        }

        protected void btnBookNow_Click(object sender, EventArgs e)
        {
            try
            {

                SQLServer objSQL = new SQLServer(BAL.Global.GetConnectionString());
                string procedure = "spInsertBookingDataFromQuote";
                string[] paramname = new string[] { 
                                                    "Origin",
                                                    "Dest",
                                                    "PcsCount",
                                                    "Weight",
                                                    "ComodityCode",
                                                    "FlightNum",
                                                    "FlightDate",
                                                    "FlightOrigin",
                                                    "FlightDest",
                                                    "AgentCode",
                                                    "AgentName",
                                                    "FreightRate",
                                                    "CarrierCode"
                                                        };

                object[] paramvalue = new object[] {
                                                    ddlSource.SelectedItem.Value,
                                                    ddlDest.SelectedItem.Value,
                                                    txtPCS.Text.Trim(),
                                                    txtBookingWgt.Text.Trim(),
                                                    ddlComodityCd.SelectedValue.ToString().Trim(),
                                                    //ddlFlight.SelectedItem.Text.Trim(),
                                                    ddlFlight.SelectedValue.ToString(),
                                                    txtFlightDate.Text.Trim(),
                                                    ddlSource.SelectedItem.Value,
                                                    ddlDest.SelectedItem.Value,
                                                    TXTAgentCode.Text.Trim(),
                                                    txtAgentName.Text.Trim(),
                                                    txtBidPrice.Text.Trim(),
                                                    ddlFlightPrefix.SelectedItem.Text.Trim()
                                                     };

                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, 
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.VarChar,
                                                    SqlDbType.Int,
                                                    SqlDbType.VarChar};

                DataSet dsQuote = objSQL.SelectRecords(procedure, paramname, paramvalue, paramtype);
                if (dsQuote.Tables.Count>0)
                {
                    if (dsQuote.Tables[0].Rows.Count > 0)
                    {
                        btnBookNow.Enabled = false;
                        //btnClear_Click(sender, e);
                        //btnClear_Click(null, null);
                        lblStatus.Text = "Record Saved Successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        // protected void ddlFlightPrefix_SelectedIndexChanged(object sender, EventArgs e)
        //{
           
        //    try
        //    {
                
        //        DataSet dsResult = new DataSet();
        //        string FlightPrefix = ddlFlightPrefix.SelectedItem.Value.ToString();

        //        if (ddlFlightPrefix.SelectedItem.Value.ToString() == "Select")
        //        {
        //            ddlFlight.DataSource = "";
        //            ddlFlight.DataBind();
        //        }
        //        else
        //        {

        //            SQLServer objSQL = new SQLServer(Global.GetConnectionString());
        //            dsResult = objSQL.SelectRecords("spGetAllFlightListFromOrgToDestNEW", "FlightPrefix", FlightPrefix, SqlDbType.VarChar);
        //            if (dsResult != null)
        //            {
        //                if (dsResult.Tables.Count > 0)
        //                {
        //                    if (dsResult.Tables[0].Rows.Count > 0)
        //                    {
        //                        ddlFlight.Items.Clear();
        //                        ddlFlight.DataSource = dsResult.Tables[0];
        //                        ddlFlight.DataTextField = "FlightID";
        //                        ddlFlight.DataValueField = "FlightID";
        //                        ddlFlight.DataBind();
        //                        ddlFlight.Items.Insert(0, new ListItem("Select", ""));
        //                        ddlFlight.SelectedIndex = -1;
        //                    }

        //                    else
        //                    {
        //                        lblStatus.Text = "No Flight Avaliable for this parnter";
        //                        lblStatus.ForeColor = Color.Red;
        //                        ddlFlight.DataSource = "";
        //                        ddlFlight.DataBind();
        //                    }
        //                }
        //            }

        //        }
                
               


        //    }
        //    catch (Exception)
        //    {


        //    }

        
        //}

        #region AutoLoadList
        public void AutoLoadList()
        {
            try
            {
              
                SQLServer objSQL = new SQLServer(BAL.Global.GetConnectionString());
               
                DataSet dsData = new DataSet();
                dsData = objSQL.SelectRecords("spGetAgentQuoteListAuto", "srno", Request.QueryString["AgentQuoteID"].ToString(), SqlDbType.VarChar);
                if (dsData != null)
                {
                    if (dsData.Tables.Count > 0)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            TXTAgentCode.Text = dsData.Tables[0].Rows[0]["AgentCode"].ToString();
                            txtAgentName.Text = dsData.Tables[0].Rows[0]["AgentName"].ToString();
                            ddlSource.SelectedValue = dsData.Tables[0].Rows[0]["Origin"].ToString();
                            ddlDest.SelectedValue = dsData.Tables[0].Rows[0]["Dest"].ToString();
                            //ddlFlight.SelectedValue = dsData.Tables[0].Rows[0]["FlightNo"].ToString();
                            ddlFlight.SelectedItem.Text = dsData.Tables[0].Rows[0]["FlightNo"].ToString();
                            ddlComodityCd.SelectedValue = dsData.Tables[0].Rows[0]["ComodityCode"].ToString();
                            txtBookingWgt.Text = dsData.Tables[0].Rows[0]["FreightWeight"].ToString();
                            txtPCS.Text = dsData.Tables[0].Rows[0]["FreightPcs"].ToString();
                            txtBidPrice.Text = dsData.Tables[0].Rows[0]["FreightRate"].ToString();
                            txtFlightDate.Text = dsData.Tables[0].Rows[0]["FlightDate"].ToString();
                            string QuoteStatus = dsData.Tables[0].Rows[0]["QuoteStatus"].ToString().ToUpper();
                            if (QuoteStatus == "C")
                            {
                                btnBookNow.Enabled = true;
                                btnSave.Text = "Update";
                                //btnClear.Text = "Cancle";
                            }
                            lblStatus.Text = string.Empty;
                            txtAgentQID.Text = dsData.Tables[0].Rows[0]["AgentQuoteID"].ToString();
                            txtAgentQID.Enabled = false;
                            btnList.Enabled = false;
                        }
                        else
                        {
                            lblStatus.Text = "No data found for given criteria";
                            lblStatus.ForeColor = Color.Red;
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

        }
        #endregion

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                SQLServer objSQL = new SQLServer(BAL.Global.GetConnectionString());
                DataSet dsData = new DataSet();
                dsData = objSQL.SelectRecords("spGetAgentQuoteNewList", "AgentQuoteID", txtAgentQID.Text.Trim(), SqlDbType.VarChar);
                if (dsData != null)
                {
                    if (dsData.Tables.Count > 0)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            TXTAgentCode.Text = dsData.Tables[0].Rows[0]["AgentCode"].ToString();
                            txtAgentName.Text = dsData.Tables[0].Rows[0]["AgentName"].ToString();
                            ddlSource.SelectedValue = dsData.Tables[0].Rows[0]["Origin"].ToString();
                            ddlDest.SelectedValue = dsData.Tables[0].Rows[0]["Dest"].ToString();
                            //ddlFlight.SelectedValue = dsData.Tables[0].Rows[0]["FlightNo"].ToString();
                            ddlFlight.SelectedItem.Text = dsData.Tables[0].Rows[0]["FlightNo"].ToString();
                            ddlComodityCd.SelectedValue = dsData.Tables[0].Rows[0]["ComodityCode"].ToString();
                            txtBookingWgt.Text = dsData.Tables[0].Rows[0]["FreightWeight"].ToString();
                            txtPCS.Text = dsData.Tables[0].Rows[0]["FreightPcs"].ToString();
                            txtBidPrice.Text = dsData.Tables[0].Rows[0]["FreightRate"].ToString();
                            txtFlightDate.Text = dsData.Tables[0].Rows[0]["FlightDate"].ToString();
                            string QuoteStatus = dsData.Tables[0].Rows[0]["QuoteStatus"].ToString().ToUpper();
                            if (QuoteStatus == "C")
                            {
                                btnBookNow.Enabled = true;
                                btnSave.Text = "Update";
                                //btnClear.Text = "Cancle";
                            }
                            lblStatus.Text = string.Empty;
                            TXTAgentCode.Enabled = false;
                            txtAgentName.Enabled = false;
                            ddlSource.Enabled = false;
                            ddlDest.Enabled = false;
                            txtFlightDate.Enabled = false;
                            ddlFlight.Enabled = false;
                            ddlFlightPrefix.Enabled = false;
                            ddlComodityCd.Enabled = false;
                            btnCheck.Visible = false;
                        }
                        else
                        {
                            lblStatus.Text = "No data found for given criteria";
                            lblStatus.ForeColor = Color.Red;

                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}

