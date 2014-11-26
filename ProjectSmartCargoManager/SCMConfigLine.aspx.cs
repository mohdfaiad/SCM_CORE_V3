using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;
using QID.DataAccess;
using System.Data.SqlClient;

/*
 
 2012-07-10 vinayak
 
 */


namespace ProjectSmartCargoManager
{
    public partial class SCMConfigLine : System.Web.UI.Page
    {
        DataSet dsExceptions;
        OtherChargesBAL objBAL = new OtherChargesBAL();
        DataSet dsSlabs = new DataSet();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        int rtlid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HidSrNo.Value = "0";
                ////////////////////////////////////
                Session["FlightNumber"] = "";
                Session["AirlineCode"] = "";
                Session["CommCode"] = "";
                Session["AgentCode"] = "";
                Session["Shipper"] = "";
                Session["mode"] = "";

                RBExAC.Checked = true;
                RBExCC.Checked = true;
                RBExAD.Checked = true;
                RBExFC.Checked = true;
                RBExFN.Checked = true;
                RBExHC.Checked = true;
                RBExSC.Checked = true;
                RBExORG.Checked = true;
                RBExDest.Checked = true;
                rbWeekdaysExclude.Checked = true;
                RBExET.Checked = true;               
                RBExCountry.Checked = true;
                RBExCountryD.Checked = true;
                RBExPT.Checked = true;
                RBExHand.Checked = true;
                rbExIS.Checked = true;
                //RBExOC.Checked = true;

                LoadConfigDropDown();

                if (Request.QueryString["cmd"] != null)
                {
                    int SrNo = int.Parse(Request.QueryString["SrNo"].ToString());
                    //rtlid = Convert.ToInt32(HidSrNo.Value);
                    rtlid = SrNo;
                    AutoPopulateConfig(SrNo);
                    EnableDisable(Request.QueryString["cmd"].ToString() == "Edit");
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);

        }

        private void LoadConfigDropDown()
        {
            try 
            {
                DataSet ds = da.SelectRecords("SP_GetConfigSelect");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DDLConfigSelect.DataSource = ds;
                            DDLConfigSelect.DataTextField = "ConfigCode";
                            DDLConfigSelect.DataValueField = "ConfigCode";
                            DDLConfigSelect.DataBind();
                            DDLConfigSelect.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void LoadAgentDropdown(DropDownList ddl)
        {
            DataSet dsResult = new DataSet();
            if (!objBAL.GetAgentList(ref dsResult))
            {
                return;
            }

            if (dsResult != null && dsResult.Tables.Count != 0)
            {

                DataRow row = dsResult.Tables[0].NewRow();
                row["AgentName"] = "Select";
                row["AgentCode"] = "Select";

                dsResult.Tables[0].Rows.Add(row);

                ddl.DataSource = dsResult.Tables[0];
                ddl.DataTextField = "AgentName";
                ddl.DataValueField = "AgentCode";
                ddl.DataBind();

                ddl.Text = "Select";
            }
        }

        #region Config Select
        protected void DDLConfigSelect_SelectedIndexChanged(object sender, EventArgs e) 
        { }
        #endregion

        public void AutoPopulateConfig(int SrNo)
        {
            try
            {
                DataSet dsResult = new DataSet();
                dsResult = da.SelectRecords("SP_GetConfigInfo", "ConfigCode", SrNo, SqlDbType.Int);

                string errormessage = "";
                if (dsResult != null)
                {
                    HidSrNo.Value = dsResult.Tables[0].Rows[0]["SerialNumber"].ToString();
                    TXTConfigCode.Text = dsResult.Tables[0].Rows[0]["ConfigCode"].ToString();
                    TXTDescription.Text = dsResult.Tables[0].Rows[0]["ConfigDesc"].ToString();

                    TXTStartDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["StartDate"].ToString()).ToString("yyyy-MM-dd");
                    TXTEndDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["EndDate"].ToString()).ToString("yyyy-MM-dd");

                    DDLOriginLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["OriginLevel"].ToString());
                    TXTOrigin.Text = dsResult.Tables[0].Rows[0]["Origin"].ToString();
                    DDLDestinationLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["DestinationLevel"].ToString());
                    TXTDestination.Text = dsResult.Tables[0].Rows[0]["Destination"].ToString();

                    string Param = dsResult.Tables[0].Rows[0]["Parameter"].ToString();
                    DDLConfigSelect.SelectedIndex = DDLConfigSelect.Items.IndexOf(DDLConfigSelect.Items.FindByText(Param));
                    string format = dsResult.Tables[0].Rows[0]["Format"].ToString();
                    DDLFormatType.SelectedIndex = DDLFormatType.Items.IndexOf(DDLFormatType.Items.FindByValue(format));
                    string dateformat = dsResult.Tables[0].Rows[0]["DateFormat"].ToString();
                    ddldateFormat.SelectedIndex = ddldateFormat.Items.IndexOf(ddldateFormat.Items.FindByValue(dateformat));
                    TXTRoundoff.Text = dsResult.Tables[0].Rows[0]["NextRoundOff"].ToString();
                    txtdecimal.Text = dsResult.Tables[0].Rows[0]["DecimalAllow"].ToString();

                    #region Commented Code
                    //HidCurrency.Value = dsResult.Tables[0].Rows[0]["CurrencyID"].ToString();
                    //TXTCurrency.SelectedValue = dsResult.Tables[0].Rows[0]["CurrencyID"].ToString();
                    //DDLPaymentType.Text = dsResult.Tables[0].Rows[0]["PaymentType"].ToString();
                    //DDLChargeType.Text = dsResult.Tables[0].Rows[0]["ChargeType"].ToString();
                    //TXTDiscount.Text = dsResult.Tables[0].Rows[0]["DiscountPercent"].ToString();
                    //TXTCommision.Text = dsResult.Tables[0].Rows[0]["CommPercent"].ToString();
                    //TXTTax.Text = dsResult.Tables[0].Rows[0]["ServiceTax"].ToString();
                    //DDLHeadBasis.Text = dsResult.Tables[0].Rows[0]["ChargeHeadBasis"].ToString();
                    //txtTaxPercent.Text = dsResult.Tables[0].Rows[0]["TaxPercent"].ToString();
                    //TXTMinimum.Text = dsResult.Tables[0].Rows[0]["MinimumCharge"].ToString();
                    //TXTMax.Text = dsResult.Tables[0].Rows[0]["maximum"].ToString();

                    //string taxtype = dsResult.Tables[0].Rows[0]["TaxType"].ToString();
                    //ddlTaxCode.SelectedIndex = ddlTaxCode.Items.IndexOf(ddlTaxCode.Items.FindByValue(taxtype));

                    //string[] AppliedOn = dsResult.Tables[0].Rows[0]["AppliedOn"].ToString().Split(',');
                    //int count = 0;
                    //for (int i = 0; i < chkRateList.Items.Count; i++)
                    //{
                    //    if (AppliedOn.Contains(chkRateList.Items[i].Value))
                    //    {
                    //        chkRateList.Items[i].Selected = true;
                    //        count++;
                    //    }
                    //}
                    //if (count == chkRateList.Items.Count)
                    //    chkAll.Checked = true;
                    #endregion

                    foreach (DataRow row in dsResult.Tables[1].Rows)
                    {

                        if (row["ParamName"].ToString() == "FlightCarrier")
                        {
                            TXTFlightCarrier.Text = row["ParamValue"].ToString();
                            RBIncFC.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString().Equals("IssueCarrier", StringComparison.OrdinalIgnoreCase))
                        {
                            TXTIssueCarrier.Text = row["ParamValue"].ToString();
                            rbIncIS.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "Source")
                        {
                            TXTParamORG.Text = row["ParamValue"].ToString();
                            RBIncORG.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "CountrySource")
                        {
                            TXTParamCountry.Text = row["ParamValue"].ToString();
                            RBIncCountry.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "Destination")
                        {
                            TXTParamDest.Text = row["ParamValue"].ToString();
                            RBIncDest.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "CountryDestination")
                        {
                            TXTParamCountryD.Text = row["ParamValue"].ToString();
                            RBIncCountryD.Checked = bool.Parse(row["IsInclude"].ToString());
                        }

                        if (row["ParamName"].ToString() == "FlightNum")
                        {
                            TXTFlightNumber.Text = row["ParamValue"].ToString();
                            RBIncFN.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "DaysOfWeek")
                        {
                            string wkDayval = row["ParamValue"].ToString();
                            for (int iD = 0; iD < 7; iD++)
                            {
                                if (wkDayval[iD] == '1')
                                {
                                    cblWeekdays.Items[iD].Selected = true;
                                }

                            }
                            rbWeekdaysInclude.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "AgentCode")
                        {
                            TXTAgentCode.Text = row["ParamValue"].ToString();
                            RBIncAD.Checked = bool.Parse(row["IsInclude"].ToString());
                        }

                        if (row["ParamName"].ToString() == "ShipperCode")
                        {
                            TXTShipperCode.Text = row["ParamValue"].ToString();
                            RBIncSC.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "CommCode")
                        {
                            TXTIATAComCode.Text = row["ParamValue"].ToString();
                            RBIncCC.Checked = bool.Parse(row["IsInclude"].ToString());
                        }

                        if (row["ParamName"].ToString() == "ProductType")
                        {
                            TXTParamPT.Text = row["ParamValue"].ToString();
                            RBIncPT.Checked = bool.Parse(row["IsInclude"].ToString());
                        }

                        if (row["ParamName"].ToString() == "HandlingCode")
                        {
                            TXTHandlingCode.Text = row["ParamValue"].ToString();
                            RBIncHC.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString() == "Handler")
                        {
                            TXTParamHand.Text = row["ParamValue"].ToString();
                            RBIncHand.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                        if (row["ParamName"].ToString().Equals("EquipType", StringComparison.OrdinalIgnoreCase))
                        {
                            TXTEquipmentType.Text = row["ParamValue"].ToString();
                            RBIncET.Checked = bool.Parse(row["IsInclude"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

        }

        public void EnableDisable(bool IsEnabled)
        {
            try
            {
                TXTConfigCode.Enabled = IsEnabled;
                TXTDescription.Enabled = IsEnabled;
                
                TXTStartDate.Enabled = IsEnabled;
                TXTEndDate.Enabled = IsEnabled;

                DDLOriginLevel.Enabled = IsEnabled;
                TXTOrigin.Enabled = IsEnabled;
                DDLDestinationLevel.Enabled = IsEnabled;
                TXTDestination.Enabled = IsEnabled;

                DDLConfigSelect.Enabled = IsEnabled;
                DDLFormatType.Enabled = IsEnabled;

                ddldateFormat.Enabled = IsEnabled;
                TXTRoundoff.Enabled = IsEnabled;
                txtdecimal.Enabled = IsEnabled;

                DDLFormatType_SelectedIndexChanged(null, null);

                btnSave.Enabled = IsEnabled;

                //Enable or Disable Parameters

                ////Flight Carrier
                //TXTFlightCarrier.Enabled = IsEnabled;
                //RBExFC.Enabled = IsEnabled;
                //RBIncFC.Enabled = IsEnabled;

                ////Issue Carrier
                //TXTIssueCarrier.Enabled = IsEnabled;
                //rbExIS.Enabled = IsEnabled;
                //rbIncIS.Enabled = IsEnabled;

                ////Airline Code
                //TXTAirLineCode.Enabled = IsEnabled;
                //RBExAC.Enabled = IsEnabled;
                //RBIncAC.Enabled = IsEnabled;

                ////Origin
                //TXTParamORG.Enabled = IsEnabled;
                //RBExORG.Enabled = IsEnabled;
                //RBIncORG.Enabled = IsEnabled;

                ////Country Source
                //TXTParamCountry.Enabled = IsEnabled;
                //RBExCountry.Enabled = IsEnabled;
                //RBIncCountry.Enabled = IsEnabled;

                ////Destination
                //TXTParamDest.Enabled = IsEnabled;
                //RBExDest.Enabled = IsEnabled;
                //RBIncDest.Enabled = IsEnabled;

                ////Country Destination
                //TXTParamCountryD.Enabled = IsEnabled;
                //RBIncCountryD.Enabled = IsEnabled;
                //RBExCountryD.Enabled = IsEnabled;

                ////Flight Number
                //TXTFlightNumber.Enabled = IsEnabled;
                //RBExFN.Enabled = IsEnabled;
                //RBIncFN.Enabled = IsEnabled;

                ////WeekDays
                //cblWeekdays.Enabled = IsEnabled;
                //rbWeekdaysExclude.Enabled = IsEnabled;
                //rbWeekdaysInclude.Enabled = IsEnabled;

                //Agent Code
                TXTAgentCode.Enabled = IsEnabled;
                RBExAD.Enabled = IsEnabled;
                RBIncAD.Enabled = IsEnabled;

                //Shipper Code
                TXTShipperCode.Enabled = IsEnabled;
                RBExSC.Enabled = IsEnabled;
                RBIncSC.Enabled = IsEnabled;

                ////IATA Comm Code
                //TXTIATAComCode.Enabled = IsEnabled;
                //RBExCC.Enabled = IsEnabled;
                //RBIncCC.Enabled = IsEnabled;

                //Product Type
                TXTParamPT.Enabled = IsEnabled;
                RBIncPT.Enabled = IsEnabled;
                RBExPT.Enabled = IsEnabled;

                ////SPL Handling Code
                //TXTHandlingCode.Enabled = IsEnabled;
                //RBExHC.Enabled = IsEnabled;
                //RBIncHC.Enabled = IsEnabled;

                ////Handler
                //TXTParamHand.Enabled = IsEnabled;
                //RBIncHand.Enabled = IsEnabled;
                //RBExHand.Enabled = IsEnabled;

                ////Equipment Type
                //TXTEquipmentType.Enabled = IsEnabled;
                //RBExET.Enabled = IsEnabled;
                //RBIncET.Enabled = IsEnabled;
            }
            catch (Exception ex)
            { }
        }
      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (SaveGridStateslab() == false)
                //    return;
                if (!ValidateSave())
                    return;
                string errormessage = "", AppliedOn = "";
                
      
                string wkDaysval = string.Empty;
                
                for (int i = 0; i < cblWeekdays.Items.Count; i++)
                {
                    if (cblWeekdays.Items[i].Selected == true)
                    {
                        wkDaysval += '1';
                    }
                    else
                        wkDaysval += '0';

                }
               
                string Date = Session["IT"].ToString();
                int retVal = objBAL.SaveConfigLine(
                    HidSrNo.Value, 
                    TXTConfigCode.Text, 
                    TXTDescription.Text,
                    Convert.ToDateTime(TXTStartDate.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                    Convert.ToDateTime(TXTEndDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), 
                    DDLOriginLevel.SelectedIndex, 
                    TXTOrigin.Text, 
                    DDLDestinationLevel.SelectedIndex, 
                    TXTDestination.Text,                     
                    TXTFlightNumber.Text, 
                    TXTFlightCarrier.Text, 
                    TXTHandlingCode.Text, 
                    TXTAirLineCode.Text, 
                    TXTIATAComCode.Text, 
                    TXTAgentCode.Text, 
                    TXTShipperCode.Text,
                    TXTParamORG.Text, 
                    TXTParamDest.Text, 
                    TXTParamCountry.Text, 
                    TXTParamCountryD.Text,
                    TXTParamPT.Text, 
                    TXTParamHand.Text, 
                    TXTEquipmentType.Text.Trim(), 
                    TXTIssueCarrier.Text, 
                    wkDaysval, 
                    RBIncFN.Checked, 
                    RBIncFC.Checked, 
                    RBIncHC.Checked, 
                    RBIncAC.Checked, 
                    RBIncCC.Checked, 
                    RBIncAD.Checked, 
                    RBIncSC.Checked, 
                    RBIncORG.Checked, 
                    RBIncDest.Checked,                     
                    RBIncCountry.Checked, 
                    RBIncCountryD.Checked, 
                    RBIncPT.Checked, 
                    RBIncHand.Checked, 
                    RBIncET.Checked, 
                    rbIncIS.Checked,
                    rbWeekdaysInclude.Checked, 
                    Session["UserName"].ToString(),                     
                    Date,
                    DDLConfigSelect.Text,
                    DDLFormatType.Text,
                    ddldateFormat.Text,
                    TXTRoundoff.Text,
                    txtdecimal.Text,
                    ref errormessage
                    
                    );
                if ((retVal) > 0)
                {
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Paramsss = new object[7];
                    int k = 0;

                    //1
                    Paramsss.SetValue("Tax Line", k);
                    k++;

                    //2
                    string MstValue = TXTConfigCode.Text+"-"+TXTDescription.Text;
                    Paramsss.SetValue(MstValue, k);
                    k++;

                    //3
                    Paramsss.SetValue("ADD", k);
                    k++;

                    //4
                    string Msg = "Tax Line Added";
                    Paramsss.SetValue(Msg, k);
                    k++;

                    //5
                    string Desc = "";
                    Paramsss.SetValue(Desc, k);
                    k++;

                    //6

                    Paramsss.SetValue(Session["UserName"], k);
                    k++;

                    //7
                    Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                    k++;

                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsss);
                    #endregion
                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Other Charges record added successfully!');", true);                  
                    lblStatus.Text = "Configuration record added successfully!";
                    lblStatus.ForeColor = Color.Green;
                    //BindRepeaterData(retVal);
                }
            }
            catch (Exception ex)
            {
                // error
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error:" + ex.Message + "');", true);
                lblStatus.Text = "Error :" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);

        }

        public bool ValidateSave()
        {
            try
            {
                if (TXTConfigCode.Text.Trim() == "")
                {
                    lblStatus.Text = "Tax Code cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (TXTDescription.Text.Trim() == "")
                {
                    lblStatus.Text = "Tax Name cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (TXTStartDate.Text.Trim() == "")
                {
                    lblStatus.Text = "Start Date cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (TXTEndDate.Text.Trim() == "")
                {
                    lblStatus.Text = "End Date cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (TXTOrigin.Text.Trim() == "")
                {
                    lblStatus.Text = "Origin cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
               
                try
                {
                    if (!(TXTOrigin.Text.Equals("all", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (!ValidateMasterEntry(TXTOrigin.Text.ToString(), DDLOriginLevel.Text.ToString()))
                        {
                            lblStatus.Text = "Please check Origin entry present in Master records";
                            lblStatus.ForeColor = Color.Red;
                            return false;
                        }
                    }
                }
                catch (Exception ex) { }
                if (TXTDestination.Text.Trim() == "")
                {
                    lblStatus.Text = "Destination cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                try
                {
                    if (!(TXTDestination.Text.Equals("all", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (!ValidateMasterEntry(TXTDestination.Text.ToString(), DDLDestinationLevel.Text.ToString()))
                        {
                            lblStatus.Text = "Please check Destination entry present in Master records";
                            lblStatus.ForeColor = Color.Red;
                            return false;
                        }
                    }
                }
                catch (Exception ec) { }
                
                bool IsDuplicate = false;
                string errormessage = "";
                string ViaStation = string.Empty;

                string wkDaysval = string.Empty;

                for (int i = 0; i < cblWeekdays.Items.Count; i++)
                {
                    if (cblWeekdays.Items[i].Selected == true)
                    {
                        wkDaysval += '1';
                    }
                    else
                        wkDaysval += '0';

                }

                //object[] values = {
                //                    HidSrNo.Value,
                //                    TXTConfigCode.Text, 
                //                    TXTDescription.Text, 
                //                    Convert.ToDateTime(TXTStartDate.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                //                    Convert.ToDateTime(TXTEndDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), 
                //                    DDLOriginLevel.SelectedIndex, 
                //                    TXTOrigin.Text, 
                //                    DDLDestinationLevel.SelectedIndex, 
                //                    TXTDestination.Text,                                    
                //                    TXTFlightCarrier.Text,
                //                    RBIncFC.Checked, 
                //                    TXTIssueCarrier.Text.Trim(),
                //                    rbIncIS.Checked,
                //                    TXTAirLineCode.Text,
                //                    RBIncAC.Checked, 
                //                    TXTParamORG.Text.Trim(),
                //                    RBIncORG.Checked,
                //                    TXTParamCountry.Text,
                //                    RBIncCountry.Checked,
                //                    TXTParamDest.Text.Trim(),
                //                    RBIncDest.Checked,
                //                    TXTParamCountryD.Text,
                //                    RBIncCountryD.Checked,
                //                    TXTFlightNumber.Text.Trim(),
                //                    RBIncFN.Checked,
                //                    wkDaysval,
                //                    rbWeekdaysInclude.Checked,
                //                    TXTAgentCode.Text, 
                //                    RBIncAD.Checked, 
                //                    TXTShipperCode.Text,
                //                    RBIncSC.Checked,
                //                    TXTIATAComCode.Text, 
                //                    RBIncCC.Checked, 
                //                    TXTParamPT.Text.Trim(),
                //                    RBIncPT.Checked,
                //                    TXTHandlingCode.Text.Trim(),
                //                    RBIncHC.Checked,
                //                    TXTParamHand.Text.Trim(), 
                //                    RBIncHand.Checked, 
                //                    TXTEquipmentType.Text,
                //                    RBIncET.Checked,
                //                    TxTOCCode.Text.Trim(),
                //                    RBIncOC.Checked,
                                    
                //                  };

                //if (objBAL.CheckDuplicateTaxLine(values, ref IsDuplicate, ref errormessage))
                //{
                //    if (IsDuplicate)
                //    {
                //        lblStatus.Text = "Tax Line already exists.";
                //        lblStatus.ForeColor = Color.Red;
                //        return false;
                //    }
                //}
                //else
                //{
                //    lblStatus.Text = "" + errormessage;
                //    lblStatus.ForeColor = Color.Red;
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :(Validate)" + ex.Message;
                lblStatus.ForeColor = Color.Red;
                return false;
            }

        }

        private bool ValidateMasterEntry(string Airport, string Level)
        {
            bool isPresent = false;
            try
            {
                BALAirportMaster objBAL = new BALAirportMaster();
                isPresent = objBAL.CheckAirportMasterEntry(Airport, Level);
            }
            catch (Exception ex)
            {
                isPresent = false;
            }
            return isPresent;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                TXTAgentCode.Text = "";
                TXTAirLineCode.Text = "";
                
                TXTConfigCode.Text = "";
                TXTDescription.Text = "";
                TXTDestination.Text = "";
                TXTEndDate.Text = "";
                TXTFlightCarrier.Text = "";
                TXTFlightNumber.Text = "";
                TXTHandlingCode.Text = "";
                TXTIATAComCode.Text = "";
                
                TXTOrigin.Text = "";
                TXTShipperCode.Text = "";
                TXTStartDate.Text = "";
                TXTEquipmentType.Text = "";
                TXTParamCountry.Text = "";
                TXTParamCountryD.Text = "";
                TXTParamHand.Text = "";
                TXTParamPT.Text = "";
                TXTIssueCarrier.Text = "";
                DDLDestinationLevel.SelectedIndex = 0;
                DDLOriginLevel.SelectedIndex = 0;
                
                
                HidDest.Value = "";
                HidCurrency.Value = "";
                HidLocation.Value = "";
                HidOrigin.Value = "";


                RBExAC.Checked = true;
                RBExCC.Checked = true;
                RBExAD.Checked = true;
                RBExFC.Checked = true;
                RBExFN.Checked = true;
                RBExHC.Checked = true;
                RBExSC.Checked = true;
                RBExET.Checked = true;
                RBExCountry.Checked = true;
                RBExCountryD.Checked = true;
                RBExPT.Checked = true;
                RBExHand.Checked = true;
                rbExIS.Checked = true;

                RBIncAC.Checked = false;
                RBIncCC.Checked = false;
                RBIncAD.Checked = false;
                RBIncFC.Checked = false;
                RBIncFN.Checked = false;
                RBIncHC.Checked = false;
                RBIncSC.Checked = false;
                RBIncET.Checked = false;
                RBIncCountry.Checked = false;
                RBIncCountryD.Checked = false;
                RBIncPT.Checked = false;
                RBIncHand.Checked = false;
                rbIncIS.Checked = false;

                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error:" + ex.Message + "');", true);
            }
        }


        #region Fill Currency
        private void FillCurrencyCodes(DropDownList drp, string SelectedCurrency)
        {
            try
            {
                BALCurrency BalCur = new BALCurrency();
                DataSet dsCur = BalCur.GetCurrencyCodeList("");
                if (dsCur != null && dsCur.Tables.Count > 0)
                {
                    if (dsCur.Tables[0].Rows.Count > 0)
                    {
                        drp.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        try
                        {
                            drp.DataSource = dsCur.Tables[0];
                            drp.DataTextField = "Code";
                            drp.DataValueField = "SrNo";
                            drp.DataBind();
                        }
                        catch (Exception ex) { }
                        drp.SelectedIndex = drp.Items.IndexOf(drp.Items.FindByText(SelectedCurrency));
                    }
                    else
                    {
                        drp.Items.Clear();
                        drp.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        protected void DDLWtType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetIATAOCCodes(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            SqlDataAdapter dad = null;
            DataSet ds = new DataSet();
            List<string> list = null;

            try
            {
                dad = new SqlDataAdapter("SELECT DISTINCT OCCode+'-'+OCDescription FROM dbo.OCCodeMaster WHERE isActive = 1 AND OCCode IS NOT NULL", con);
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

        protected void DDLFormatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                TXTRoundoff.Enabled = false;
                txtdecimal.Enabled = false;
                ddldateFormat.Enabled = false;
                if (DDLFormatType.SelectedIndex > -1)
                {
                    if (DDLFormatType.Text.Equals("Number", StringComparison.OrdinalIgnoreCase) || DDLFormatType.Text.Equals("N", StringComparison.OrdinalIgnoreCase)) 
                    {   
                        TXTRoundoff.Enabled = true;                        
                        txtdecimal.Enabled = true;
                    }
                    else if (DDLFormatType.Text.Equals("Date", StringComparison.OrdinalIgnoreCase) || DDLFormatType.Text.Equals("D", StringComparison.OrdinalIgnoreCase))
                    {
                        ddldateFormat.Enabled = true;
                    }
                    else 
                    {
                        TXTRoundoff.Enabled = false;
                        txtdecimal.Enabled = false;
                        ddldateFormat.Enabled = false;
                    }
                }
            }
            catch (Exception ex) { }
        }

    }
}
