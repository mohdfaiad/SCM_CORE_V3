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
    public partial class VolumetricException : System.Web.UI.Page
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
                HidCurrency.Value = "1";

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
                RBExSH.Checked = true;
                RBExPH.Checked = true;
                RBExWE.Checked = true;
                RBExCH.Checked = true;
                RBExCountry.Checked = true;
                RBExCountryD.Checked = true;
                RBExPT.Checked = true;
                RBExHand.Checked = true;
                rbExIS.Checked = true;
                RBExOC.Checked = true;

                dsExceptions = new DataSet();
                dsExceptions.Tables.Add(new DataTable());
                dsExceptions.Tables[0].Columns.Add("Agent");
                dsExceptions.Tables[0].Columns.Add("Commision");
                dsExceptions.Tables[0].Columns.Add("Discount");
                dsExceptions.Tables[0].Columns.Add("Tax");

                Session["dsExceptions"] = dsExceptions.Copy();

                LoadSlabSession();
                
                if (Request.QueryString["cmd"] != null)
                {
                    int SrNo = int.Parse(Request.QueryString["SrNo"].ToString());
                    //rtlid = Convert.ToInt32(HidSrNo.Value);
                    rtlid = SrNo;
                    AutoPopulateOtherCharges(SrNo);
                    EnableDisable(Request.QueryString["cmd"].ToString() == "Edit");
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);

        }

        #region Slab Region Not Used
        private void LoadSlabSession()
        {
            try
            {
                dsSlabs = new DataSet();
                dsSlabs.Tables.Add(new DataTable());
                dsSlabs.Tables[0].Columns.Add("Type");
                dsSlabs.Tables[0].Columns.Add("Weight");
                dsSlabs.Tables[0].Columns.Add("Charge");
                dsSlabs.Tables[0].Columns.Add("Cost");
                dsSlabs.Tables[0].Columns.Add("SrNo");
                Session["dsSlabs"] = dsSlabs.Copy();

                AddNewRow();

            }
            catch (Exception ex) { }
        }

        #endregion

        public void AddNewRow()
        {
            try
            {
                dsExceptions = (DataSet)Session["dsExceptions"];

                DataRow row = dsExceptions.Tables[0].NewRow();
                row["Agent"] = "Select";
                row["Commision"] = "0";
                row["Discount"] = "0";
                row["Tax"] = "0";

                dsExceptions.Tables[0].Rows.Add(row);

                GRDException.DataSource = dsExceptions.Tables[0];
                GRDException.DataBind();


                for (int i = 0; i < GRDException.Rows.Count; i++)
                {
                    LoadAgentDropdown(((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")));
                    ((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")).SelectedValue = dsExceptions.Tables[0].Rows[i]["Agent"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
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

        public void SaveGridState()
        {
            try
            {

                DataSet ds = ((DataSet)Session["dsExceptions"]).Clone();

                for (int i = 0; i < GRDException.Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].NewRow();

                    row["Agent"] = ((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")).SelectedValue.ToString();
                    row["Commision"] = ((TextBox)GRDException.Rows[i].FindControl("TXTCommision")).Text.ToString();
                    row["Discount"] = ((TextBox)GRDException.Rows[i].FindControl("TXTDiscount")).Text.ToString();
                    row["Tax"] = ((TextBox)GRDException.Rows[i].FindControl("TXTTax")).Text.ToString();

                    ds.Tables[0].Rows.Add(row);
                }


                Session["dsExceptions"] = ds.Copy();

            }
            catch (Exception ex)
            {

            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridState();

                dsExceptions = ((DataSet)Session["dsExceptions"]).Copy();
                DataSet dsAfterDelete = dsExceptions.Clone();

                for (int i = 0; i < GRDException.Rows.Count; i++)
                {
                    if (!((CheckBox)GRDException.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow row = dsAfterDelete.Tables[0].NewRow();

                        row[0] = dsExceptions.Tables[0].Rows[i][0].ToString();
                        row[1] = dsExceptions.Tables[0].Rows[i][1].ToString();
                        row[2] = dsExceptions.Tables[0].Rows[i][2].ToString();
                        row[3] = dsExceptions.Tables[0].Rows[i][3].ToString();

                        dsAfterDelete.Tables[0].Rows.Add(row);
                    }

                }

                GRDException.DataSource = dsAfterDelete.Copy();
                GRDException.DataBind();

                Session["dsExceptions"] = dsAfterDelete.Copy();


                for (int i = 0; i < GRDException.Rows.Count; i++)
                {
                    LoadAgentDropdown(((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")));
                    ((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")).SelectedValue = dsAfterDelete.Tables[0].Rows[i]["Agent"].ToString();
                }
            }
            catch (Exception ex)
            {

            }

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
                dsExceptions = (DataSet)Session["dsExceptions"];
                
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
                int time = 0, refval = 0;
                float baserate = 0;
                try
                {
                    if (int.TryParse(txtHrs.Text.ToString(), out refval))
                    {
                        time = Convert.ToInt16(txtHrs.Text.ToString());
                    }
                }
                catch (Exception ex) { }
                
                string Date = Session["IT"].ToString();
                int retVal = objBAL.SaveVolumetricinfo(
                    HidSrNo.Value,
                    Convert.ToDateTime(TXTStartDate.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                    Convert.ToDateTime(TXTEndDate.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                    chkStatus.Checked,
                    Convert.ToString(Session["UserName"]),
                    Convert.ToDateTime(Session["IT"]),
                    TXTFlightNumber.Text,
                    TXTFlightCarrier.Text,
                    TXTHandlingCode.Text,
                    TXTAirLineCode.Text,
                    TXTIATAComCode.Text,
                    TXTAgentCode.Text,
                    TXTShipperCode.Text,
                    RBIncFN.Checked,
                    RBIncFC.Checked,
                    RBIncHC.Checked,
                    RBIncAC.Checked,
                    RBIncCC.Checked,
                    RBIncAD.Checked,
                    RBIncSC.Checked,
                    TXTParamORG.Text,
                    TXTParamDest.Text,
                    RBIncORG.Checked,
                    RBIncDest.Checked,
                    wkDaysval,
                    rbWeekdaysInclude.Checked,
                    TXTParamCountry.Text,
                    TXTParamCountryD.Text,
                    TXTParamPT.Text,
                    TXTParamHand.Text,
                    RBIncCountry.Checked,
                    RBIncCountryD.Checked,
                    RBIncPT.Checked,
                    RBIncHand.Checked,
                    TXTEquipmentType.Text.Trim(),
                    RBIncET.Checked,
                    TXTIssueCarrier.Text,
                    rbIncIS.Checked,
                    TxTOCCode.Text,
                    RBIncOC.Checked,
                    dsExceptions,
                    ref errormessage
                    );
                if ((retVal) > 0)
                {   
                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Other Charges record added successfully!');", true);                  
                    lblStatus.Text = "Volumetric exemptions added successfully!";
                    lblStatus.ForeColor = Color.Green;                    
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
                TXTEndDate.Text = "";
                TXTFlightCarrier.Text = "";
                TXTFlightNumber.Text = "";
                TXTHandlingCode.Text = "";
                TXTIATAComCode.Text = "";
                TXTShipperCode.Text = "";
                TXTStartDate.Text = "";
                TXTEquipmentType.Text = "";
                TXTParamCountry.Text = "";
                TXTParamCountryD.Text = "";
                TXTParamHand.Text = "";
                TXTParamPT.Text = "";
                TXTIssueCarrier.Text = "";
                
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

                dsExceptions = new DataSet();
                dsExceptions.Tables.Add(new DataTable());
                dsExceptions.Tables[0].Columns.Add("Agent");
                dsExceptions.Tables[0].Columns.Add("Commision");
                dsExceptions.Tables[0].Columns.Add("Discount");
                dsExceptions.Tables[0].Columns.Add("Tax");

                Session["dsExceptions"] = dsExceptions.Copy();

                AddNewRow();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error:" + ex.Message + "');", true);
            }
        }

        public void AutoPopulateOtherCharges(int SrNo)
        {
            try
            {
                DataSet dsResult = new DataSet();
                string errormessage = "";
                if (objBAL.GetVolumetricinfo(SrNo.ToString(), ref dsResult, ref errormessage))
                {
                    HidSrNo.Value = dsResult.Tables[0].Rows[0]["SerialNumber"].ToString();
                    TXTStartDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["StartDate"].ToString()).ToString("yyyy-MM-dd");
                    TXTEndDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["EndDate"].ToString()).ToString("yyyy-MM-dd");
                    if (dsResult.Tables[0].Rows[0]["Status"].ToString() == "1")
                        chkStatus.Checked = true;
                    else
                        chkStatus.Checked = false;
                    
                    
                    foreach (DataRow row in dsResult.Tables[1].Rows)
                    {

                        if (row["ParamName"].ToString() == "FlightNum")
                        {
                            TXTFlightNumber.Text = row["ParamValue"].ToString();
                            RBIncFN.Checked = bool.Parse(row["IsInclude"].ToString());
                        }

                        if (row["ParamName"].ToString() == "FlightCarrier")
                        {
                            TXTFlightCarrier.Text = row["ParamValue"].ToString();
                            RBIncFC.Checked = bool.Parse(row["IsInclude"].ToString());

                        }

                        if (row["ParamName"].ToString() == "HandlingCode")
                        {
                            TXTHandlingCode.Text = row["ParamValue"].ToString();
                            RBIncHC.Checked = bool.Parse(row["IsInclude"].ToString());

                        }

                        if (row["ParamName"].ToString() == "AirlineCode")
                        {
                            TXTAirLineCode.Text = row["ParamValue"].ToString();
                            RBIncAC.Checked = bool.Parse(row["IsInclude"].ToString());

                        }

                        if (row["ParamName"].ToString() == "CommCode")
                        {
                            TXTIATAComCode.Text = row["ParamValue"].ToString();
                            RBIncCC.Checked = bool.Parse(row["IsInclude"].ToString());

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
                        if (row["ParamName"].ToString() == "Source")
                        {
                            TXTParamORG.Text = row["ParamValue"].ToString();
                            RBIncORG.Checked = bool.Parse(row["IsInclude"].ToString());

                        }
                        if (row["ParamName"].ToString() == "Destination")
                        {
                            TXTParamDest.Text = row["ParamValue"].ToString();
                            RBIncDest.Checked = bool.Parse(row["IsInclude"].ToString());

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

                        if (row["ParamName"].ToString() == "CountrySource")
                        {
                            TXTParamCountry.Text = row["ParamValue"].ToString();
                            RBIncCountry.Checked = bool.Parse(row["IsInclude"].ToString());

                        }

                        if (row["ParamName"].ToString() == "CountryDestination")
                        {
                            TXTParamCountryD.Text = row["ParamValue"].ToString();
                            RBIncCountryD.Checked = bool.Parse(row["IsInclude"].ToString());

                        }

                        if (row["ParamName"].ToString() == "ProductType")
                        {
                            TXTParamPT.Text = row["ParamValue"].ToString();
                            RBIncPT.Checked = bool.Parse(row["IsInclude"].ToString());

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
                        if (row["ParamName"].ToString().Equals("IssueCarrier", StringComparison.OrdinalIgnoreCase))
                        {
                            TXTIssueCarrier.Text = row["ParamValue"].ToString();
                            rbIncIS.Checked = bool.Parse(row["IsInclude"].ToString());

                        }
                        if (row["ParamName"].ToString() == "OCCodes")
                        {
                            TxTOCCode.Text = row["ParamValue"].ToString();
                            RBIncOC.Checked = bool.Parse(row["IsInclude"].ToString());

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
            TXTStartDate.Enabled = IsEnabled;
            TXTEndDate.Enabled = IsEnabled;
            
            TXTFlightNumber.Enabled = IsEnabled;
            TXTHandlingCode.Enabled = IsEnabled;
            TXTIATAComCode.Enabled = IsEnabled;
            TXTFlightCarrier.Enabled = IsEnabled;
            TXTShipperCode.Enabled = IsEnabled;
            TXTAgentCode.Enabled = IsEnabled;
            TXTAirLineCode.Enabled = IsEnabled;
            TXTParamORG.Enabled = IsEnabled;
            TXTParamDest.Enabled = IsEnabled;
            btnSave.Enabled = IsEnabled;

            GRDException.Enabled = IsEnabled;
            IBAC.Enabled = IsEnabled;
            IBAD.Enabled = IsEnabled;
            IBCC.Enabled = IsEnabled;
            IBFN.Enabled = IsEnabled;
            IBSC.Enabled = IsEnabled;
            IBStartDate.Enabled = IsEnabled;
            IBEndDate.Enabled = IsEnabled;
            
            RBExAC.Enabled = IsEnabled;
            RBExAD.Enabled = IsEnabled;
            RBExCC.Enabled = IsEnabled;
            RBExFC.Enabled = IsEnabled;
            RBExFN.Enabled = IsEnabled;
            RBExHC.Enabled = IsEnabled;
            RBExSC.Enabled = IsEnabled;
            RBExDest.Enabled = IsEnabled;
            RBExORG.Enabled = IsEnabled;
            RBExCH.Enabled = IsEnabled;
            RBExWE.Enabled = IsEnabled;
            RBExPH.Enabled = IsEnabled;
            RBExSH.Enabled = IsEnabled;
            RBExET.Enabled = IsEnabled;

            RBIncAC.Enabled = IsEnabled;
            RBIncAD.Enabled = IsEnabled;
            RBIncCC.Enabled = IsEnabled;
            RBIncFC.Enabled = IsEnabled;
            RBIncFN.Enabled = IsEnabled;
            RBIncHC.Enabled = IsEnabled;
            RBIncSC.Enabled = IsEnabled;
            RBIncDest.Enabled = IsEnabled;
            RBIncORG.Enabled = IsEnabled;
            RBIncET.Enabled = IsEnabled;

            ddlTriggerPoint.Enabled = IsEnabled;
            txtHrs.Enabled = IsEnabled;
            RBInCH.Enabled = IsEnabled;
            RBInWE.Enabled = IsEnabled;
            RBInPH.Enabled = IsEnabled;
            RBInSH.Enabled = IsEnabled;

            TXTParamCountry.Enabled = IsEnabled;
            TXTParamCountryD.Enabled = IsEnabled;
            TXTParamPT.Enabled = IsEnabled;
            TXTParamHand.Enabled = IsEnabled;
            RBIncHand.Enabled = IsEnabled;
            RBIncCountryD.Enabled = IsEnabled;
            RBIncCountry.Enabled = IsEnabled;
            RBIncPT.Enabled = IsEnabled;
            TXTEquipmentType.Enabled = IsEnabled;

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

    }
}
