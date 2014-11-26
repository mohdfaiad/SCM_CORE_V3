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
    public partial class frmAgentCapacityNew : System.Web.UI.Page
    {

        #region Variables
        DateTime timeTimer;
        HomeBL objBLL = new HomeBL();
        BookingBAL objBkgBL = new BookingBAL();
        ListBookingBAL objBAL = new ListBookingBAL();
        AgentBAL objAg = new AgentBAL();
        
        #endregion
        
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.Red;
            if (!IsPostBack)
            {
                timeTimer = DateTime.Now;
                LoadCommodityDropdown();
                LoadDestination();
                TXTAgentCode.Focus();
                GetFlights();
                loadWeekDays();
                LoadFlightPrefix();

                if (Convert.ToString(Session["AgentCode"]) != "")
                {
                    TXTAgentCode.Text = Convert.ToString(Session["AgentCode"]);
                    txtAgentName.Text = Convert.ToString(Session["AgentName"]);
                    TXTAgentCode.ReadOnly = true;
                    txtAgentName.ReadOnly = true;
                    IBOrigin.Visible = false;
                }
            }
            
        }
        #endregion

        #region GetAgentCode
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {

            //string[] orgdest = new ConBooking().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') ", con);
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

        #region GetStation
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

        #region getFlights
        public void GetFlights()
        {
            try
            {
                {
                    //source = ddlSource.SelectedItem.Text.Trim();
                    //dest = ddlDest.SelectedItem.Text.Trim();
                    //date = "";// txtDate.Text;


                    //for default instance

                    SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                    DataSet dsInstance = new DataSet();
                    //string FlightPrefix;
                    dsInstance = objSQL.SelectRecords("GetCurrentInstance");
                    string current = dsInstance.Tables[0].Rows[0][0].ToString();
                    DataSet dsResult = new DataSet();
                    string errormessage = "";

                    //if (objBAL.GetAllFlightsNew(source, dest, date, ref dsResult, ref errormessage))
                    string procedure = "spGetAllFlightList";
                    dsResult = objSQL.SelectRecords(procedure);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                ddlFlight.Items.Clear();
                                ddlFlight.DataSource = dsResult.Tables[0];
                                ddlFlight.DataTextField = "FltNumber";
                                ddlFlight.DataValueField = "DeptTime";
                                ddlFlight.DataBind();
                                //ddlFlight.Items.Insert(0, new ListItem("Select", ""));
                                //ddlFlight.SelectedIndex = -1;

                                ddlFlight.SelectedValue = current;
                                GetFlight(current);
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
        //protected void ddlFlight_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //if (ddlFlight.SelectedItem.Text.Trim() != "Select")
        //    //{
        //    //    string flt = ddlFlight.SelectedItem.Text;
        //    //    txtDeptTime.Text = ddlFlight.SelectedValue;
        //    //    ddlFlight.SelectedItem.Text = flt;
        //    //    if (TXTAgentCode.Text.Trim() != "")
        //    //    {
        //    //        pnlBookingWeight.Visible = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        lblStatus.Text = "Please Enter Agent Code";
        //    //        lblStatus.ForeColor = Color.Red;
        //    //    }
        //    //}
        //}
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //btnSave.Visible = false;
                TXTAgentCode.Text = "";
                txtAgentName.Text = "";
                ddlSource.SelectedIndex = 0;
                ddlDest.SelectedIndex = 0;
                //ddlFlight.Items.Clear();
                //ddlFlight.Items.Add("Select");
                ddlFlight.SelectedIndex = 0;
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlComodityCd.SelectedValue = "GEN";
                txtDeptTime.Text = "";
                txtBookingWgt.Text = "";
                txtBidPrice.Text = "";
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
                ddlTime.SelectedIndex = 0;
                loadWeekDays();

                //for (int i = 0; i < chkWeekDays.Items.Count; i++)
                //{
                //    chkWeekDays.Items[i].Selected = false;
                //}
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region ddlSource_IndexChanged
        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSource.SelectedIndex > 0 && ddlSource.SelectedIndex > 0)
            {
                GetFlights();
            }
        }
        #endregion

        #region ddlDest_IndexChanged
        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlSource.SelectedIndex > 0 && ddlSource.SelectedIndex > 0)
            //{
            //    GetFlights();
            //}
            if (ddlDest.SelectedIndex == 0)
            {
                GetFlights();
            }
            else
            {
                if (ddlSource.SelectedIndex == 0)
                { return; }
                else
                {
                    string source = ddlSource.SelectedItem.Text.Trim();
                    string dest = ddlDest.SelectedItem.Text.Trim();
                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    if (objBAL.GetAllFlightsNew(source, dest, "", ref dsResult, ref errormessage))
                    {

                        if (dsResult != null)
                        {
                            if (dsResult.Tables.Count > 0)
                            {
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    ddlFlight.Items.Clear();
                                    ddlFlight.DataSource = dsResult.Tables[0];
                                    ddlFlight.DataTextField = "FltNumber";
                                    ddlFlight.DataValueField = "DeptTime";
                                    ddlFlight.DataBind();
                                    ddlFlight.Items.Insert(0, new ListItem("Select", ""));
                                    ddlFlight.SelectedIndex = -1;
                                }
                            }
                        }
                    }
                }
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
              //  DataSet ds = objBkgBL.GetCommodityList("");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlComodityCd.Items.Clear();

                            //ddlComodityCd.Items.Add("Select");
                            ddlComodityCd.DataSource = ds.Tables[0];
                            //ddlComodityCd.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlComodityCd.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlComodityCd.DataTextField = "CommDesc";
                            ddlComodityCd.DataValueField = "CommodityCode";
                            //new code
                            //string first = ddlComodityCd.DataTextField;
                            //FirstCharacterLoad(first);
                           
                            ddlComodityCd.DataBind();
                            //ddlComodityCd.SelectedIndex = 0;
                            ddlComodityCd.Items.Insert(0, "GEN");


                           // if (ddlComodityCd.SelectedItem.Text.Length >0)
                           // {
                           //       //ddlComodityCd.SelectedItem.Text.Substring(0,20);
                           //     ddlComodityCd.Text = ddlComodityCd.Text.Substring(0, 20);
                           // }
                           // //Otherwise return the original string
                           // else
                           // {
                           //      ddlComodityCd.SelectedIndex=-1;
                           // }
                           //// ddlComodityCd.SelectedValue = "GEN";
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
                object[] paramvalue = new object[] { source, dest, flightid };
                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet ds = da.SelectRecords(procedure, paramname, paramvalue, paramtype);
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
            if (ddlComodityCd.SelectedIndex == 0)
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
                bool flag = CheckFlightAvailibility(ddlSource.SelectedItem.Text.Trim(), ddlDest.SelectedItem.Text.Trim(), ddlFlight.SelectedItem.Text.Trim(), float.Parse(txtBookingWgt.Text.Trim()));
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
                lblStatus.Text = "Invalid Bide Rate";
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                bool flag = false;
                flag = checkBideRate(ddlSource.SelectedItem.Text.Trim(), ddlDest.SelectedItem.Text.Trim(), ddlFlight.SelectedItem.Text.Trim(), ddlComodityCd.SelectedItem.Text.Trim(), txtBookingWgt.Text.Trim());
                if (flag == true)
                {
                    lblStatus.Text = "Bide rate is Approved.";
                    lblStatus.ForeColor = Color.Green;
                    imgBidAproved.Visible = true;
                    txtBidPrice.Enabled = false;
                    pnlBookTimer.Visible = true;
                    btnSave.Visible = true;
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>checkval();</SCRIPT>");
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('a');</SCRIPT>");
                }
                else
                {
                    lblStatus.Text = "Bide Rate is Rejected";
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
                object[] paramvalue = new object[] { Origin, Dest, Flight, Comodity, BkWeight };
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
            int count = 0;
            try
            {
                for (int i = 0; i < grdWeekDay.Rows.Count; i++)
                {
                    if (((CheckBox)grdWeekDay.Rows[i].FindControl("chkSelect")).Checked == true)
                    {
                        count++;
                    }
                }
            }
            catch (Exception ex)
            { }

            if (TXTAgentCode.Text.Trim() == "")
            {
                lblStatus.Text = "Invalid Agent Code";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            else if (ddlFlight.SelectedItem.Text.Trim() == "Select")
            {
                lblStatus.Text = "Invalid Flight";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            else if (txtFromDate.Text.Trim() == "")
            {
                lblStatus.Text = "Invalid From Date";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            else if (txtToDate.Text.Trim() == "")
            {
                lblStatus.Text = "Invalid To Date";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            else if (count <= 0)
            {
                lblStatus.Text = "Please Select Day of Week";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            //else if (txtBookingWgt.Text.Trim() == "")
            //{
            //    lblStatus.Text = "Invalid Booking Weight";
            //    lblStatus.ForeColor = Color.Red;
            //    return;
            //}
            //else if (txtBidPrice.Text.Trim() == "")
            //{
            //    lblStatus.Text = "Invalid Booking Rate";
            //    lblStatus.ForeColor = Color.Red;
            //    return;
            //}
            else
            {
                for (int i = 0; i < grdWeekDay.Rows.Count; i++)
                {
                    if (((CheckBox)grdWeekDay.Rows[i].FindControl("chkSelect")).Checked == true)
                    {
                        if (((TextBox)grdWeekDay.Rows[i].FindControl("txtWeight")).Text.Trim() == "")
                        {
                            lblStatus.Text = "Please Enter Weight For " + (((Label)grdWeekDay.Rows[i].FindControl("lblDayOfWeek")).Text.Trim());
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        else if (((TextBox)grdWeekDay.Rows[i].FindControl("txtRate")).Text.Trim() == "")
                        {
                            lblStatus.Text = "Please Enter Rate For " + (((Label)grdWeekDay.Rows[i].FindControl("lblDayOfWeek")).Text.Trim());
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                if (SaveAgentQuote() == true)
                {
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Params = new object[7];
                    int k=0;
                    
                    //0
                    Params.SetValue("Agent Capacity", k);
                    k++;

                    //1
                    Params.SetValue(TXTAgentCode.Text.Trim(), k);
                    k++;

                    //2
                    Params.SetValue("ADD", k);
                    k++;

                    //3
                    string Msg = "New Agent Capacity For " + TXTAgentCode.Text.Trim();
                    Params.SetValue(Msg, k);
                    k++;

                    //4
                    string Desc="";
                    if (ddlSource.SelectedIndex > 0 && ddlDest.SelectedIndex > 0)
                        Desc = "Org:" + ddlSource.SelectedValue + "-Dest:" + ddlDest.SelectedValue+"-FltNo:" + ddlFlightNumber.SelectedItem.Text + "-From:" + txtFromDate.Text + "-To:" + txtToDate.Text;
                    else
                        Desc = "FltNo:" + ddlFlightNumber.SelectedItem.Text + "-From:" + txtFromDate.Text + "-To:" + txtToDate.Text;
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
        }
        #endregion

        #region SaveAgentQuote
        private bool SaveAgentQuote()
        {
            bool flag = false;
            bool IsDuplicate = false;
            string errormessage = "";
            try
            {
                //for (int i = 0; i < chkWeekDays.Items.Count; i++)
                for (int i = 0; i < grdWeekDay.Rows.Count;i++ )
                {
                    if (((CheckBox)grdWeekDay.Rows[i].FindControl("chkSelect")).Checked == true)
                    {
                        SQLServer objSQL = new SQLServer(BAL.Global.GetConnectionString());
                        string procedure = "spAddAgentCapacity";
                        string[] paramname = new string[] { "AgentCode",
                                                            "AgentName",
                                                            "FlightNo",
                                                            "FromDate",
                                                            "ToDate",
                                                            "ComodityCode",
                                                            "FreightWeight",
                                                            "FreightRate",
                                                            "UpdatedBy",
                                                            "UpdatedOn",
                                                            "StationCode",
                                                            "DayOfWeek"};

                        object[] paramvalue = new object[] {TXTAgentCode.Text.Trim(),
                                                            txtAgentName.Text.Trim(),
                                                            ddlFlightNumber.SelectedItem.Text.Trim(),
                                                            txtFromDate.Text.Trim(),
                                                            txtToDate.Text.Trim(),
                                                            ddlComodityCd.SelectedItem.Text.Trim(),
                                                            //txtBookingWgt.Text.Trim(),
                                                            ((TextBox)grdWeekDay.Rows[i].FindControl("txtWeight")).Text.Trim(),
                                                            //txtBidPrice.Text.Trim(),
                                                            ((TextBox)grdWeekDay.Rows[i].FindControl("txtRate")).Text.Trim(),
                                                            Session["UserName"].ToString(), 
                                                            DateTime.Now.ToString(),
                                                            Session["Station"].ToString(),
                                                            //chkWeekDays.Items[i].Text.ToString().Trim()};
                                                            ((Label)grdWeekDay.Rows[i].FindControl("lblDayOfWeek")).Text.Trim()};

                        //for check duplicate
                        object[] paramvalue1 = new object[] {TXTAgentCode.Text.Trim(),
                                                            txtAgentName.Text.Trim(),
                                                            ddlFlightNumber.SelectedItem.Text.Trim(),
                                                            txtFromDate.Text.Trim(),
                                                            txtToDate.Text.Trim(),
                                                            ddlComodityCd.SelectedItem.Text.Trim(),
                                                            //txtBookingWgt.Text.Trim(),
                                                            ((TextBox)grdWeekDay.Rows[i].FindControl("txtWeight")).Text.Trim(),
                                                            //txtBidPrice.Text.Trim(),
                                                            ((TextBox)grdWeekDay.Rows[i].FindControl("txtRate")).Text.Trim(),
                                                            //Session["UserName"].ToString(), 
                                                            //DateTime.Now.ToString(),
                                                            Session["Station"].ToString(),
                                                            //chkWeekDays.Items[i].Text.ToString().Trim()};
                                                            ((Label)grdWeekDay.Rows[i].FindControl("lblDayOfWeek")).Text.Trim()};

                        SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar,
                                                                SqlDbType.VarChar,
                                                                SqlDbType.VarChar,
                                                                SqlDbType.DateTime,
                                                                SqlDbType.DateTime,
                                                                SqlDbType.VarChar,
                                                                SqlDbType.VarChar,
                                                                SqlDbType.VarChar,
                                                                SqlDbType.VarChar,
                                                                SqlDbType.DateTime,
                                                                SqlDbType.VarChar,
                                                                SqlDbType.VarChar };


                        if (objAg.CheckDuplicate(paramvalue1, ref IsDuplicate, ref errormessage))
                        {
                            if (IsDuplicate)
                            {
                                lblStatus.Text = "Agent Capacity already exists.";
                                lblStatus.ForeColor = Color.Red;
                                return false;
                            }
                            else
                            {
                                flag = objSQL.InsertData(procedure, paramname, paramtype, paramvalue);
                            }

                        }
                        else
                        {

                            flag = objSQL.InsertData(procedure, paramname, paramtype, paramvalue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return flag;
        }
        #endregion

        #region loadWeekDays
        private void loadWeekDays()
        {
            try
            {
                DataTable tempWeekDays = new DataTable();
                tempWeekDays.Columns.Add("DayOfWeek");
                
                DataRow dr1 = tempWeekDays.NewRow();
                dr1[0] = "Sun";
                tempWeekDays.Rows.Add(dr1);
                DataRow dr2 = tempWeekDays.NewRow();
                dr2[0] = "Mon";
                tempWeekDays.Rows.Add(dr2);
                DataRow dr3 = tempWeekDays.NewRow();
                dr3[0] = "Tue";
                tempWeekDays.Rows.Add(dr3);
                DataRow dr4 = tempWeekDays.NewRow();
                dr4[0] = "Wed";
                tempWeekDays.Rows.Add(dr4);
                DataRow dr5 = tempWeekDays.NewRow();
                dr5[0] = "Thu";
                tempWeekDays.Rows.Add(dr5);
                DataRow dr6 = tempWeekDays.NewRow();
                dr6[0] = "Fri";
                tempWeekDays.Rows.Add(dr6);
                DataRow dr7 = tempWeekDays.NewRow();
                dr7[0] = "Sat";
                tempWeekDays.Rows.Add(dr7);

                grdWeekDay.DataSource = null;
                grdWeekDay.DataSource = tempWeekDays;
                grdWeekDay.DataBind();
            }
            catch (Exception ex)
            { }
        }
        #endregion
         
        #region chkSelect_CheckChanged
        protected void chkSelect_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                int RowIndex = 0;
                CheckBox chk = (CheckBox)sender;
                GridViewRow grdRow = (GridViewRow)chk.NamingContainer;
                
                if (grdRow.RowIndex < 0)
                    return;

                RowIndex = grdRow.RowIndex;

                if (chk.Checked == true)
                {
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtWeight")).Enabled = true;
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtRate")).Enabled = true;
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtCurrency")).Enabled = true;
                }
                else
                {
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtWeight")).Enabled = false;
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtWeight")).Text="";
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtRate")).Enabled = false;
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtRate")).Text="";
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtCurrency")).Enabled = false;
                    ((TextBox)grdWeekDay.Rows[RowIndex].FindControl("txtCurrency")).Text = "INR";
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
        public void LoadFlightPrefix()
        { try{
              DataSet ds = objBkgBL.GetFlightPrefixList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlFlight.Items.Clear();
                            //ddlComodityCd.Items.Add("Select");
                            ddlFlight.DataSource = ds.Tables[0];
                            //ddlComodityCd.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlComodityCd.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlFlight.DataTextField = "PartnerCode";
                            ddlFlight.DataValueField = "PartnerCode";
                            ddlFlight.DataBind();
                            //ddlComodityCd.SelectedIndex = 0;
                            ddlFlight.Items.Insert(0, "SELECT");
                           // ddlComodityCd.SelectedValue = "GEN";
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        
        }

        protected void ddlFlight_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FlightPrefix = ddlFlight.SelectedItem.Value.ToString();
            GetFlight(FlightPrefix);
            //string first = ddlComodityCd.SelectedItem.Value.ToString();
            //FirstCharacterLoad(first);
                           
           
        }
        public void GetFlight(string FlightPrefix )
        {
         try
            {


                DataSet dsResult = new DataSet();

                //if (Session["Region"] == null || Session["City"] == null || Session["Agent"] == null)
                //{
                //    FillSession();
                //}
               // Clear();

              
                //clear grid view
                //grdStockAllocation.DataSource = "";

                //grdStockAllocationChild.DataSource = "";
                //grdStockAllocation.DataBind();
                //grdStockAllocationChild.DataBind();

                if (ddlFlight.SelectedItem.Value.ToString() == "Select")
                {
                    ddlFlightNumber.DataSource = "";
                    ddlFlightNumber.DataBind();
                }
          
                   //string FlightPrefix=ddlFlight.SelectedValue.ToString();
                
                
                    
               
                   SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                   dsResult = objSQL.SelectRecords("spGetAllFlightListPrefixWise","FlightPrefix",FlightPrefix,SqlDbType.VarChar);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                ddlFlightNumber.Items.Clear();
                                ddlFlightNumber.DataSource = dsResult.Tables[0];
                                ddlFlightNumber.DataTextField = "FlightID";
                                ddlFlightNumber.DataValueField = "FlightID";
                                ddlFlightNumber.DataBind();
                                ddlFlightNumber.Items.Insert(0, new ListItem("Select", ""));
                                ddlFlightNumber.SelectedIndex = -1;
                            }

                            else
                            {
                                ddlFlightNumber.Items.Clear();
                                lblStatus.Text = "No Flight for this Partner";
                                lblStatus.ForeColor = Color.Red;
                                ddlFlightNumber.DataSource = null;
                                ddlFlightNumber.DataBind();
                                ddlFlightNumber.Items.Insert(0, new ListItem("Select", null));
                            }
                        }
                    }
            }
            catch (Exception)
            {
            }
        }
        //public string FirstCharacterLoad( string FIRST)
        //{
        //    if (FIRST.Length > 20)
        //    {
        //        return FIRST.Substring(0, 20);
        //    }
        //    else
            
        //    {
        //        return FIRST;
            
        //    }
        
        //}
        }
    }