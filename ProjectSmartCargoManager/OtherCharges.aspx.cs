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
    public partial class OtherCharges : System.Web.UI.Page
    {
        DataSet dsExceptions;
        OtherChargesBAL objBAL = new OtherChargesBAL();
        DataSet dsSlabs = new DataSet();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        int rtlid;
        DataSet dsULDSlabs;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadOCCode();
                // Fill Default Values ///////////////
                
                DataSet dsDefaultValues = new DataSet();
                string errormessage = "";
                if (objBAL.GetDefaultValues(ref dsDefaultValues, ref errormessage))
                {
                    foreach (DataRow row in dsDefaultValues.Tables[0].Rows)
                    {
                        if (row["ParameterName"].ToString() == "ServiceTax")
                        {
                            TXTTax.Text = "" + row["ParameterValue"].ToString();
                        }
                        else if (row["ParameterName"].ToString() == "OCDiscountPercent")
                        {
                            TXTDiscount.Text = "" + row["ParameterValue"].ToString();
                        }
                        else if (row["ParameterName"].ToString() == "OCCommissionPercent")
                        {
                            TXTCommision.Text = "" + row["ParameterValue"].ToString();
                        }

                    }

                }


                LoadGLDropDown();
                //TXTCurrency.Text = "INR";
                FillCurrencyCodes(TXTCurrency, "INR");
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
                
                dsExceptions = new DataSet();
                dsExceptions.Tables.Add(new DataTable());
                dsExceptions.Tables[0].Columns.Add("Agent");
                dsExceptions.Tables[0].Columns.Add("Commision");
                dsExceptions.Tables[0].Columns.Add("Discount");
                dsExceptions.Tables[0].Columns.Add("Tax");

                Session["dsExceptions"] = dsExceptions.Copy();

                LoadSlabSession();
                LoadAirportCombo();
                
                LoadChargeType();

                BindRepeaterData(0);
                try
                {
                    dsULDSlabs = new DataSet();
                    dsULDSlabs.Tables.Add(new DataTable());
                    dsULDSlabs.Tables[0].Columns.Add("ULDType");
                    dsULDSlabs.Tables[0].Columns.Add("Type");
                    dsULDSlabs.Tables[0].Columns.Add("Weight");
                    dsULDSlabs.Tables[0].Columns.Add("Charge");
                    dsULDSlabs.Tables[0].Columns.Add("SrNo");
                    Session["dsULDSlabs"] = dsULDSlabs.Copy();
                }
                catch (Exception ex)
                { }
                if (Request.QueryString["cmd"] != null)
                {
                    int SrNo = int.Parse(Request.QueryString["SrNo"].ToString());
                    //rtlid = Convert.ToInt32(HidSrNo.Value);
                    rtlid = SrNo;
                    AutoPopulateOtherCharges(SrNo);
                    EnableDisable(Request.QueryString["cmd"].ToString() == "Edit");
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                }
                else
                {
                    AddNewULDSlab();
                    AddNewULDSlab();
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);
                
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);
            try
            {
                if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                {
                    divMap.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }

        }

        protected void LoadOCCode()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = da.SelectRecords("sp_GetOCCode");
                if (ds != null & ds.Tables[0].Rows.Count > 0)
                {
                    ddlOCCode.DataSource = ds.Tables[0];
                    ddlOCCode.DataTextField = "OCDescription";
                    ddlOCCode.DataValueField = "OCCode";
                    ddlOCCode.DataBind();
                    ddlOCCode.Items.Insert(0, "Select");
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

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

                AddNewSlab();
                AddNewSlab();
                AddNewSlab();
                AddNewSlab();
                AddNewRow();

            }
            catch (Exception ex) { }
        }

        public void AddNewSlab()
        {
            try
            {
                dsSlabs = (DataSet)Session["dsSlabs"];
                dsSlabs.Tables[0].Rows.Add(dsSlabs.Tables[0].NewRow());

                if (dsSlabs.Tables[0].Rows.Count == 1)
                {
                    dsSlabs.Tables[0].Rows[0]["Type"] = "M";
                    dsSlabs.Tables[0].Rows[0]["SrNo"] = 0;
                    dsSlabs.Tables[0].Rows[0]["Weight"] = 0;
                    dsSlabs.Tables[0].Rows[0]["Charge"] = 0;
                    dsSlabs.Tables[0].Rows[0]["Cost"] = 0;

                }
                else if (dsSlabs.Tables[0].Rows.Count == 2)
                {
                    dsSlabs.Tables[0].Rows[1]["Type"] = "N";
                    dsSlabs.Tables[0].Rows[1]["SrNo"] = 1;
                    dsSlabs.Tables[0].Rows[1]["Weight"] = 0;
                    dsSlabs.Tables[0].Rows[1]["Charge"] = 0;
                    dsSlabs.Tables[0].Rows[1]["Cost"] = 0;
                }
                else
                {
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Type"] = "Q";
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["SrNo"] = dsSlabs.Tables[0].Rows.Count - 1;
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Weight"] = 0;
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Charge"] = 0;
                    dsSlabs.Tables[0].Rows[dsSlabs.Tables[0].Rows.Count - 1]["Cost"] = 0;
                }

                GRDRateSlabs.DataSource = null;
                GRDRateSlabs.DataSource = dsSlabs.Tables[0].Copy();
                GRDRateSlabs.DataBind();

                for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
                {
                    ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlType")).Text = dsSlabs.Tables[0].Rows[i]["Type"].ToString();

                }

                Session["dsSlabs"] = dsSlabs;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

        }
       
        protected void LBNAdd_OnClick(object sender, EventArgs e)
        {
            SaveGridStateslab();

            AddNewSlab();
        }
        
        public bool SaveGridStateslab()
        {
            dsSlabs = null;
            try
            {
                if (DDLHeadBasis.Text.Equals("K", StringComparison.OrdinalIgnoreCase) || DDLHeadBasis.Text.Equals("S", StringComparison.OrdinalIgnoreCase) || DDLHeadBasis.SelectedValue.Equals("T", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        if ((DataSet)Session["dsSlabs"] == null)
                        {
                            LoadSlabSession();
                        }
                    }
                    catch (Exception ex) 
                    { }
                    dsSlabs = (DataSet)Session["dsSlabs"];

                    for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
                    {
                        dsSlabs.Tables[0].Rows[i]["Type"] = ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlType")).Text;
                        dsSlabs.Tables[0].Rows[i]["Weight"] = ((TextBox)GRDRateSlabs.Rows[i].FindControl("TXTWeight")).Text;
                        dsSlabs.Tables[0].Rows[i]["Charge"] = ((TextBox)GRDRateSlabs.Rows[i].FindControl("TXTCharge")).Text;
                        dsSlabs.Tables[0].Rows[i]["Cost"] = ((TextBox)GRDRateSlabs.Rows[i].FindControl("TXTCost")).Text;

                        if (dsSlabs.Tables[0].Rows[i]["Weight"].ToString().Trim() == "")
                            dsSlabs.Tables[0].Rows[i]["Weight"] = "0";

                        if (dsSlabs.Tables[0].Rows[i]["Charge"].ToString().Trim() == "")
                            dsSlabs.Tables[0].Rows[i]["Charge"] = "0";

                        if (dsSlabs.Tables[0].Rows[i]["Cost"].ToString().Trim() == "")
                            dsSlabs.Tables[0].Rows[i]["Cost"] = "0";

                        //Added by Poorna for issue 10
                        #region Validate Slabs vales as decimal
                        try
                        {
                            float val = 0;
                            if (!float.TryParse(dsSlabs.Tables[0].Rows[i]["Weight"].ToString(), out val))
                            {
                                lblStatus.Text = "Please insert Weight in decimal number";
                                lblStatus.ForeColor = Color.Red;
                                return false;
                            }
                            if (!float.TryParse(dsSlabs.Tables[0].Rows[i]["Charge"].ToString(), out val))
                            {
                                lblStatus.Text = "Please insert Charge in decimal number";
                                lblStatus.ForeColor = Color.Red;
                                return false;
                            }
                            if (!float.TryParse(dsSlabs.Tables[0].Rows[i]["Cost"].ToString(), out val))
                            {
                                lblStatus.Text = "Please insert Cost in decimal number";
                                lblStatus.ForeColor = Color.Red;
                                return false;
                            }

                        }
                        catch (Exception ex) { }
                        #endregion
                        /*15/06/2014 allow 0 in slabs
                         * 
                         * if (Convert.ToString(dsSlabs.Tables[0].Rows[i]["Type"]) == "M")
                        {
                            if (Convert.ToInt32(dsSlabs.Tables[0].Rows[i]["Weight"]) != 0)
                            {
                                lblStatus.Text = "Minimum weight should be 0.";
                                lblStatus.ForeColor = Color.Blue;
                                return false;
                            }

                            if (Convert.ToInt32(dsSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                            {
                                lblStatus.Text = "Minimum charge should not be 0.";
                                lblStatus.ForeColor = Color.Blue;
                                return false;
                            }
                        }

                        if (Convert.ToString(dsSlabs.Tables[0].Rows[i]["Type"]) == "N")
                        {
                            if (Convert.ToInt32(dsSlabs.Tables[0].Rows[i]["Weight"]) != 0)
                            {
                                lblStatus.Text = "Normal weight should be 0.";
                                lblStatus.ForeColor = Color.Blue;
                                return false;
                            }

                            if (Convert.ToInt32(dsSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                            {
                                lblStatus.Text = "Normal charge should not be 0.";
                                lblStatus.ForeColor = Color.Blue;
                                return false;
                            }
                        }
                        */
                    }
                    Session["dsSlabs"] = dsSlabs.Copy();

                }
                else 
                {
                    Session["dsSlabs"] = null;
                }
            }
            catch (Exception ex) 
            {
                Session["dsSlabs"] = null;
            }
            return true;
                
        }
        
        protected void LBNDelete_OnClick(object sender, EventArgs e)
        {

            SaveGridState();

            dsSlabs = (DataSet)Session["dsSlabs"];
            DataSet dsSlabsTemp = dsSlabs.Copy();

            for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
            {
                if (((CheckBox)GRDRateSlabs.Rows[i].FindControl("CHK")).Checked)
                {
                    string srno = dsSlabs.Tables[0].Rows[i]["SrNo"].ToString();

                    for (int j = 0; j < dsSlabsTemp.Tables[0].Rows.Count; j++)
                    {
                        if (srno == dsSlabsTemp.Tables[0].Rows[j]["SrNo"].ToString())
                        {
                            dsSlabsTemp.Tables[0].Rows.Remove(dsSlabsTemp.Tables[0].Rows[j]);
                            break;
                        }
                    }
                }
            }

            GRDRateSlabs.DataSource = null;
            GRDRateSlabs.DataSource = dsSlabsTemp.Tables[0];
            GRDRateSlabs.DataBind();

            for (int i = 0; i < dsSlabsTemp.Tables[0].Rows.Count; i++)
            {
                ((DropDownList)GRDRateSlabs.Rows[i].FindControl("DdlType")).Text = dsSlabsTemp.Tables[0].Rows[i]["Type"].ToString();

            }

            Session["dsSlabs"] = dsSlabsTemp.Copy();

        }
      
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

        protected void DDLHeadBasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            TXTMinimum.Text = "0";
            TXTCharge.Text = "0";
            DDLWtType.SelectedIndex = 0;
           
            if (DDLHeadBasis.SelectedValue == "P")
            {
                DDLWtType.Items.Add("%");
                DDLWtType.SelectedValue = "%";
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            }
            else
            {
                for (int i = 0; i < DDLWtType.Items.Count; i++)
                {
                    if (DDLWtType.Items[i].Value == "%")
                    {
                        DDLWtType.Items.RemoveAt(i);
                        break;
                    }
                }
            }

            if (DDLHeadBasis.SelectedValue == "F")
            {                
                TXTMinimum.Enabled = false;
                DDLWtType.Enabled = false;
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            }
            else if(DDLHeadBasis.SelectedValue == "W")
            {
                TXTMinimum.Enabled = true;
                DDLWtType.Enabled = true;
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            }
            else if (DDLHeadBasis.SelectedValue == "H")
            {
                TXTMinimum.Enabled = true;
                DDLWtType.SelectedValue = "P";
                DDLWtType.Enabled = false;
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            }
            else if (DDLHeadBasis.SelectedValue == "P")
            {
                TXTMinimum.Enabled = true;
                DDLWtType.Enabled = false;
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            }
            else if (DDLHeadBasis.SelectedValue == "C")
            {//PCS count
                TXTMinimum.Enabled = true;
                DDLWtType.SelectedValue = "P";
                DDLWtType.Enabled = false;
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);
                
            }
            else if (DDLHeadBasis.SelectedValue == "K")
            {//show slab div 
                TXTMinimum.Enabled = false;
                DDLWtType.Enabled = false;
                TXTCharge.Enabled = false;
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

            }
            else if (DDLHeadBasis.SelectedValue == "S")
            {//show slab div
                TXTMinimum.Enabled = false;
                DDLWtType.Enabled = false;
                TXTCharge.Enabled = false;
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveGridStateslab();
            AddNewRow();
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

                dsExceptions=((DataSet)Session["dsExceptions"]).Copy();
                DataSet dsAfterDelete=dsExceptions.Clone();

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
                if (SaveGridStateslab() == false)
                    return;
                if (SaveGridULDState() == false)
                    return;
                if (!ValidateSave())
                    return;
                string errormessage = "";
                dsExceptions = (DataSet)Session["dsExceptions"];

                string wkDaysval=string.Empty;
                for (int i=0; i<cblWeekdays.Items.Count;i++)
                {
                if(cblWeekdays.Items[i].Selected==true)
                {
                wkDaysval+='1';
                }
                else
                wkDaysval+='0';

                }
                int time = 0,refval=0;
                float baserate = 0;
                try 
                {
                    if (int.TryParse(txtHrs.Text.ToString(), out refval)) 
                    {
                        time = Convert.ToInt16(txtHrs.Text.ToString());
                    }
                }
                catch (Exception ex) { }
                try 
                {
                    baserate = float.Parse(TXTBaseRate.Text.ToString());
                }
                catch (Exception ex) { }

                string Date = Session["IT"].ToString();

                string maxval, maxcost;
                if (txtMaxValue.Text.Trim() == "")
                    maxval = "0";
                else maxval = txtMaxValue.Text.Trim();

                if (txtMaxCost.Text.Trim() == "")
                    maxcost = "0";
                else
                    maxcost = txtMaxCost.Text.Trim();

                string status = "ACT";
                if (!chkActive.Checked)
                    status = "INACT";

                DateTime FromDt1 = new DateTime();
                DateTime ToDt1 = new DateTime();

                FromDt1 = DateTime.ParseExact(TXTStartDate.Text.Trim(), "dd/MM/yyyy", null);
                ToDt1 = DateTime.ParseExact(TXTEndDate.Text.Trim(), "dd/MM/yyyy", null);

            int retVal= objBAL.SaveOtherCharges(HidSrNo.Value,TXTChargeHeadCode.Text, TXTChargeHeadName.Text, DDLParticipationType.Text, CHKRefundable.Checked, 
                    
                   // Convert.ToDateTime(TXTStartDate.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                    //Convert.ToDateTime(TXTStartDate.Text).ToString("dd/MM/yyyy"),
                    FromDt1,
                    //Convert.ToDateTime(TXTEndDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), 
                    //Convert.ToDateTime(TXTEndDate.Text).ToString("dd/MM/yyyy"), 
                    ToDt1,
                    
                    DDLLevel.SelectedIndex, TXTLocation.Text, DDLOriginLevel.SelectedIndex, TXTOrigin.Text, DDLDestinationLevel.SelectedIndex, TXTDestination.Text, TXTCurrency.SelectedValue,
                    DDLPaymentType.Text, DDLChargeType.Text, Convert.ToDecimal(TXTDiscount.Text.Trim() == "" ? "0" : TXTDiscount.Text.Trim()), Convert.ToDecimal(TXTCommision.Text), Convert.ToDecimal(TXTTax.Text), DDLHeadBasis.Text, Convert.ToDecimal(TXTMinimum.Text.Trim() == "" ? "0" : TXTMinimum.Text.Trim()), Convert.ToDecimal(TXTCharge.Text.Trim() == "" ? "0" : TXTCharge.Text.Trim()), DDLWtType.Text, 
                    status,"", TXTFlightNumber.Text, TXTFlightCarrier.Text, TXTHandlingCode.Text, TXTAirLineCode.Text, TXTIATAComCode.Text, TXTAgentCode.Text, TXTShipperCode.Text,
                    RBIncFN.Checked, RBIncFC.Checked, RBIncHC.Checked, RBIncAC.Checked, RBIncCC.Checked, RBIncAD.Checked, RBIncSC.Checked,dsExceptions, ref errormessage,ddlType.Text,ddlViaStation.Text,
                    DDLCharge.SelectedValue, TXTParamORG.Text, TXTParamDest.Text, RBIncORG.Checked, RBIncDest.Checked, wkDaysval, rbWeekdaysInclude.Checked, (DataSet)Session["dsSlabs"],
                    Session["UserName"].ToString(),txtComment.Text,Date,ddlTriggerPoint.Text,time,RBInWE.Checked,RBInPH.Checked,RBInCH.Checked,RBInSH.Checked,ddlBasedOn.SelectedValue,baserate,
                    Convert.ToDecimal(TXTMCost.Text.Trim().Length<1 ? "0" : TXTMCost.Text.Trim()), Convert.ToDecimal(TXTCost.Text.Trim().Length<1 ? "0" : TXTCost.Text.Trim()),TXTParamCountry.Text,TXTParamCountryD.Text,
                    TXTParamPT.Text,TXTParamHand.Text,RBIncCountry.Checked,RBIncCountryD.Checked,RBIncPT.Checked,RBIncHand.Checked,TXTEquipmentType.Text.Trim(),RBIncET.Checked,ddlGLCode.Text,TXTIssueCarrier.Text,rbIncIS.Checked,
                    Convert.ToDecimal(maxval), Convert.ToDecimal(maxcost), ddlOCCode.SelectedValue, chkIgnoreCCSF.Checked, chkIsPackaging.Checked);

            if ((retVal) > 0)
            {
                int id1;
                //to Update Rate Card
                //if (HidSrNo.Value != "")
                if (Request.QueryString["cmd"] == "Edit")
                    id1 = Convert.ToInt32(HidSrNo.Value);
                //to Save Rate Card
                else
                    id1 = retVal;

                //delete rate line slabs before insert or update for RateLineSrNo
                string[] param = new string[] { "OCSrNo" };
                SqlDbType[] dbtypes = new SqlDbType[] { SqlDbType.Int };
                object[] values = new object[] { id1 };
                if (!da.ExecuteProcedure("SP_DeleteOCULDSlabs", param, dbtypes, values))
                {
                    // error
                }

                dsULDSlabs = (DataSet)Session["dsULDSlabs"];

                
                
                
                    for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
                    {
                        param = new string[] { "OCSrNo", "SlabName", "Weight", "Charge", "ULDType" };
                        dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.VarChar };
                        values = new object[] { id1, dsULDSlabs.Tables[0].Rows[i]["Type"].ToString(), Convert.ToDouble(dsULDSlabs.Tables[0].Rows[i]["Weight"].ToString()), Convert.ToDouble(dsULDSlabs.Tables[0].Rows[i]["Charge"].ToString()), dsULDSlabs.Tables[0].Rows[i]["ULDType"].ToString() };

                        if (!da.ExecuteProcedure("SP_InsertULDOCSlabs", param, dbtypes, values))
                        {
                            // error
                        }

                    }
                
            }
                if((retVal)>0)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Other Charges record added successfully!');", true);                  


                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Params = new object[7];
                    int k = 0;

                    //0
                    Params.SetValue("Other Charges", k);
                    k++;

                    //1
                    Params.SetValue(TXTChargeHeadCode.Text.Trim(), k);
                    k++;

                    //2
                    Params.SetValue("ADD", k);
                    k++;

                    //3
                    string Msg = "Other Charges " + TXTChargeHeadCode.Text.Trim();
                    Params.SetValue(Msg, k);
                    k++;

                    //4
                    string desc = "";
                    desc = "Charge Head Code:" + TXTChargeHeadCode.Text.Trim() + "/Charge Head Name:" + TXTChargeHeadName.Text.Trim();
                    Params.SetValue(desc, k);
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

                    lblStatus.Text = "Other Charges record added successfully!";
                    lblStatus.ForeColor = Color.Green;
                    BindRepeaterData(retVal);
                }
            }
            catch (Exception ex)
            {
                // error
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error:" + ex.Message + "');", true);
                lblStatus.Text = "Error :" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            ClearALL();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivViewSelect();</script>", false);

        }

        public bool ValidateSave()
        {
            try
            {
                if (TXTChargeHeadCode.Text.Trim() == "")
                {
                    lblStatus.Text = "Charge Head Code cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                
                if(TXTChargeHeadName.Text.Trim()=="")
                {
                    lblStatus.Text = "Charge Head Name cannot be blank.";
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
                if (ddlOCCode.SelectedIndex<=0)
                {
                    lblStatus.Text = "Select OC Code";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                try
                {
                    if (!ValidateMasterEntry(TXTOrigin.Text.ToString(), DDLOriginLevel.Text.ToString()))
                    {
                        lblStatus.Text = "Please check Origin entry present in Master records.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
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
                    if (!ValidateMasterEntry(TXTDestination.Text.ToString(), DDLDestinationLevel.Text.ToString()))
                    {
                        lblStatus.Text = "Please check Destination entry present in Master records";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
                catch (Exception ec) { }
                if (TXTCurrency.Text.Trim() == "")
                {
                    lblStatus.Text = "Currency cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (TXTDiscount.Text.Trim() == "")
                {
                    lblStatus.Text = "Discount cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (TXTCommision.Text.Trim() == "")
                {
                    lblStatus.Text = "Commision cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (TXTTax.Text.Trim() == "")
                {
                    lblStatus.Text = "Tax cannot be blank.";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                if(DDLHeadBasis.Text.Trim()=="F" || DDLHeadBasis.Text.Trim()=="W" || DDLHeadBasis.Text.Trim()=="P"||DDLHeadBasis.Text.Trim()=="H"|| DDLHeadBasis.Text.Trim()=="C")
                {
                    if (TXTCharge.Text.Trim() == "" || TXTCharge.Text.Trim() == "0") 
                    {
                        lblStatus.Text = "Charge cannot be blank or Zero.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
                
                if (DDLHeadBasis.Text.Trim() == "T")
                {
                    if (txtHrs.Text.Trim() == "0")
                    {
                        lblStatus.Text = "Applicable After cannot be 0.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                 
                }

                bool IsDuplicate = false;
                string errormessage = "";
                string ViaStation = string.Empty;

                DateTime FromDt1 = new DateTime();
                DateTime ToDt1 = new DateTime();

                FromDt1 = DateTime.ParseExact(TXTStartDate.Text.Trim(), "dd/MM/yyyy", null);
                ToDt1 = DateTime.ParseExact(TXTEndDate.Text.Trim(), "dd/MM/yyyy", null);

                if (ddlViaStation.Text == "Select" | ddlViaStation.Text == "SELECT")
                    ViaStation = "";

                object[] values = {
                                    TXTChargeHeadCode.Text, TXTChargeHeadName.Text, 
                                    //Convert.ToDateTime(TXTStartDate.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                                    //Convert.ToDateTime(TXTStartDate.Text).ToString("dd/MM/yyyy"),
                                    FromDt1,
                                    //Convert.ToDateTime(TXTEndDate.Text).ToString("yyyy-MM-dd HH:mm:ss"), 
                                    //Convert.ToDateTime(TXTEndDate.Text).ToString("dd/MM/yyyy"), 
                                    ToDt1,
                                    
                                    DDLOriginLevel.SelectedIndex, TXTOrigin.Text, 
                                    DDLDestinationLevel.SelectedIndex, TXTDestination.Text, 
                                    TXTFlightNumber.Text, TXTFlightCarrier.Text, TXTHandlingCode.Text, TXTAirLineCode.Text, TXTIATAComCode.Text, TXTAgentCode.Text, TXTShipperCode.Text,
                                    RBIncFN.Checked, RBIncFC.Checked, RBIncHC.Checked, RBIncAC.Checked, RBIncCC.Checked, RBIncAD.Checked, RBIncSC.Checked,
                                    HidSrNo.Value,ViaStation,TXTParamCountry.Text,TXTParamCountryD.Text,TXTParamPT.Text,TXTParamHand.Text,RBIncCountry.Checked,
                                    RBIncCountryD.Checked,RBIncPT.Checked,RBIncHand.Checked,TXTEquipmentType.Text,RBIncET.Checked,TXTIssueCarrier.Text,rbIncIS.Checked
                                  };

                if (objBAL.CheckDuplicate(values, ref IsDuplicate, ref errormessage))
                {
                    if (IsDuplicate)
                    {
                        lblStatus.Text = "Other charge already exists.";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
                else
                {
                    lblStatus.Text = "" + errormessage;
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                //////////////////////////////////////////////




                return true;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :(Validate)" + ex.Message;
                lblStatus.ForeColor = Color.Red;
                return false;
            }

        }

        private bool ValidateMasterEntry(string Airport,string Level)
        {
            bool isPresent = false;
            try 
            {
                BALAirportMaster objBAL = new BALAirportMaster();
                isPresent = objBAL.CheckAirportMasterEntry(Airport,Level);
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
                TXTCharge.Text = "0";
                TXTChargeHeadCode.Text = "";
                TXTChargeHeadName.Text = "";
                TXTCommision.Text = "";
                TXTCurrency.Text = "";                
                TXTDestination.Text = "";
                TXTDiscount.Text = "";
                TXTEndDate.Text = "";
                TXTFlightCarrier.Text = "";
                TXTFlightNumber.Text = "";
                TXTHandlingCode.Text = "";
                TXTIATAComCode.Text = "";
                TXTLocation.Text = "";
                TXTMinimum.Text = "0";
                TXTOrigin.Text = "";
                TXTShipperCode.Text = "";
                TXTStartDate.Text = "";
                TXTTax.Text = "0";
                TXTEquipmentType.Text = "";
                TXTParamCountry.Text = "";
                TXTParamCountryD.Text = "";
                TXTParamHand.Text = "";
                TXTParamPT.Text = "";
                TXTIssueCarrier.Text = "";
                DDLChargeType.SelectedIndex = 0;
                DDLDestinationLevel.SelectedIndex = 0;
                DDLHeadBasis.SelectedIndex = 0;
                DDLLevel.SelectedIndex = 0;
                DDLOriginLevel.SelectedIndex = 0;
                DDLParticipationType.SelectedIndex = 0;
                DDLPaymentType.SelectedIndex = 0;
                DDLWtType.SelectedIndex = 0;

                CHKRefundable.Checked = false;

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
                DataSet dsResult=new DataSet();
                string errormessage="";
                if (objBAL.GetOtherChargesDetails(SrNo, ref dsResult, ref errormessage))
                {                    
                    HidSrNo.Value = dsResult.Tables[0].Rows[0]["SerialNumber"].ToString();                    
                    TXTChargeHeadCode.Text = dsResult.Tables[0].Rows[0]["ChargeHeadCode"].ToString();
                    TXTChargeHeadName.Text = dsResult.Tables[0].Rows[0]["ChargeHeadName"].ToString();
                    DDLParticipationType.Text = dsResult.Tables[0].Rows[0]["ParticipationType"].ToString();
                    CHKRefundable.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["Refundable"].ToString());
                    //TXTStartDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["StartDate"].ToString()).ToString("yyyy-MM-dd");
                    TXTStartDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy");

                    //TXTEndDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["EndDate"].ToString()).ToString("yyyy-MM-dd");
                    TXTEndDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["EndDate"].ToString()).ToString("dd/MM/yyyy");

                    DDLLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["LocationLevel"].ToString());
                    TXTLocation.Text = dsResult.Tables[0].Rows[0]["Location"].ToString();
                    DDLOriginLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["OriginLevel"].ToString());
                    TXTOrigin.Text = dsResult.Tables[0].Rows[0]["Origin"].ToString();
                    DDLDestinationLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["DestinationLevel"].ToString());
                    TXTDestination.Text = dsResult.Tables[0].Rows[0]["Destination"].ToString();
                    HidCurrency.Value = dsResult.Tables[0].Rows[0]["CurrencyID"].ToString();
                    TXTCurrency.SelectedValue = dsResult.Tables[0].Rows[0]["CurrencyID"].ToString();
                    DDLPaymentType.Text = dsResult.Tables[0].Rows[0]["PaymentType"].ToString();
                    DDLChargeType.Text = dsResult.Tables[0].Rows[0]["ChargeType"].ToString();
                    TXTDiscount.Text = dsResult.Tables[0].Rows[0]["DiscountPercent"].ToString();
                    TXTCommision.Text = dsResult.Tables[0].Rows[0]["CommPercent"].ToString();
                    TXTTax.Text = dsResult.Tables[0].Rows[0]["ServiceTax"].ToString();
                    DDLHeadBasis.Text = dsResult.Tables[0].Rows[0]["ChargeHeadBasis"].ToString();
                    TXTMinimum.Text = dsResult.Tables[0].Rows[0]["MinimumCharge"].ToString();
                    TXTCharge.Text = dsResult.Tables[0].Rows[0]["PerUnitCharge"].ToString();
                    DDLWtType.Text = dsResult.Tables[0].Rows[0]["WeightType"].ToString();
                    ddlType.Text=dsResult.Tables[0].Rows[0]["chargedAt"].ToString();
                    ddlViaStation.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["ViaStation"]);
                    ddlBasedOn.Text = dsResult.Tables[0].Rows[0]["BasedOn"].ToString();
                    TXTBaseRate.Text = Convert.ToString(dsResult.Tables[0].Rows[0]["BaseRate"]);

                    txtMaxValue.Text = dsResult.Tables[0].Rows[0]["MaxValue"].ToString();
                    txtMaxCost.Text = dsResult.Tables[0].Rows[0]["MaxCost"].ToString();

                    try
                    {
                        chkIsPackaging.Checked = bool.Parse(dsResult.Tables[0].Rows[0]["IsPackaging"].ToString());
                    }
                    catch
                    {
                        chkIsPackaging.Checked = false;
                    }

                    string OCCode = dsResult.Tables[0].Rows[0]["OCCode"].ToString();
                    ddlOCCode.SelectedIndex = ddlOCCode.Items.IndexOf(ddlOCCode.Items.FindByValue(OCCode));

                    try
                    {
                        DDLCharge.SelectedValue = dsResult.Tables[0].Rows[0]["ChargeCode"].ToString();                        
                    }
                    catch (Exception ex) { }
                    try 
                    {
                        ddlGLCode.Text = dsResult.Tables[0].Rows[0]["GLCode"].ToString();    
                    }
                    catch (Exception ex) { }
                    try 
                    {
                        chkActive.Checked = true;
                        if (dsResult.Tables[0].Rows[0]["Status"].ToString().Equals("INACT", StringComparison.OrdinalIgnoreCase))
                            chkActive.Checked = false;
                    }
                    catch (Exception ex) { }
                    if (DDLHeadBasis.SelectedValue == "P")
                    {
                        DDLWtType.Items.Add("%");
                        DDLWtType.SelectedValue = "%";
                        DDLWtType.Enabled = false;
                    }
                  
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
                             for(int iD=0;iD<7;iD++)
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
                         if (row["ParamName"].ToString().Equals("EquipType",StringComparison.OrdinalIgnoreCase))
                         {
                             TXTEquipmentType.Text = row["ParamValue"].ToString();
                             RBIncET.Checked = bool.Parse(row["IsInclude"].ToString());

                         }
                         if (row["ParamName"].ToString().Equals("IssueCarrier", StringComparison.OrdinalIgnoreCase))
                         {
                             TXTIssueCarrier.Text = row["ParamValue"].ToString();
                             rbIncIS.Checked = bool.Parse(row["IsInclude"].ToString());

                         }


                    }

                    GRDException.DataSource = dsResult.Tables[2].Copy();
                    GRDException.DataBind();
                    int i=0;
                    
                    foreach(DataRow rw in dsResult.Tables[2].Rows)
                    {
                        LoadAgentDropdown(((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")));
                        ((DropDownList)GRDException.Rows[i++].FindControl("DDLAgent")).Text = "" + rw["AgentCode"].ToString();
                    }

                    #region Slab Details
                    try
                    {
                        if (dsResult.Tables.Count > 3)
                        {
                            if (dsResult.Tables[3].Rows.Count > 0) 
                            {
                                GRDRateSlabs.DataSource = dsResult.Tables[3];
                                GRDRateSlabs.DataBind();

                                for (int cnt = 0; cnt < dsResult.Tables[3].Rows.Count; cnt++)
                                {
                                    ((CheckBox)GRDRateSlabs.Rows[cnt].FindControl("CHK")).Checked = true;
                                    ((DropDownList)GRDRateSlabs.Rows[cnt].FindControl("DdlType")).SelectedValue = dsResult.Tables[3].Rows[cnt]["Type"].ToString();
                                    dsResult.Tables[3].Rows[cnt]["SrNo"] = i;
                                }

                                DataSet dsSlabsTemp = new DataSet();
                                dsSlabsTemp.Tables.Add(dsResult.Tables[3].Copy());
                                Session["dsSlabs"] = dsSlabsTemp.Copy();

                            }
                        }
                    }
                    catch (Exception ex) 
                    { }
                    #endregion

                    #region Trigger Details
                    try 
                    {
                        if (dsResult.Tables.Count > 4)
                        {
                            if (dsResult.Tables[4].Rows.Count > 0)
                            {
                                DataRow dr = dsResult.Tables[4].Rows[0];
                                ddlTriggerPoint.Text = dr["TriggerON"].ToString();
                                txtHrs.Text = Convert.ToString(dr["TimeAfter"].ToString());
                                RBInWE.Checked = bool.Parse(dr["WeekEnd"].ToString());
                                RBInPH.Checked = bool.Parse(dr["PubHoliday"].ToString());
                                RBInCH.Checked = bool.Parse(dr["CompHoliday"].ToString());
                                RBInSH.Checked = bool.Parse(dr["StationHoliday"].ToString());

                            }
                        }
                    }
                    catch (Exception ex) { }
                    #endregion

                    #region ULD Slab Details
                    try
                    {
                        if (dsResult.Tables.Count > 5)
                        {
                            if (dsResult.Tables[5].Rows.Count > 0)
                            {
                                grdULDslabs.DataSource = dsResult.Tables[5];
                                grdULDslabs.DataBind();

                                for (int cnt = 0; cnt < dsResult.Tables[5].Rows.Count; cnt++)
                                {
                                    ((CheckBox)grdULDslabs.Rows[cnt].FindControl("CHKULD")).Checked = true;
                                    LoadULDTypes(cnt);
                                    ((DropDownList)grdULDslabs.Rows[cnt].FindControl("DdlULDType")).SelectedValue = dsResult.Tables[5].Rows[cnt]["ULDType"].ToString();
                                    dsResult.Tables[5].Rows[cnt]["SrNo"] = cnt;
                                }

                                DataSet dsULDSlabsTemp = new DataSet();
                                dsULDSlabsTemp.Tables.Add(dsResult.Tables[5].Copy());
                                Session["dsULDSlabs"] = dsULDSlabsTemp.Copy();

                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

        }

        public void EnableDisable(bool IsEnabled)
        {
            TXTChargeHeadCode.Enabled = IsEnabled;
            TXTChargeHeadName.Enabled = IsEnabled;
            DDLParticipationType.Enabled = IsEnabled;
            CHKRefundable.Enabled = IsEnabled;
            TXTStartDate.Enabled = IsEnabled;
            TXTEndDate.Enabled = IsEnabled;
            DDLLevel.Enabled = IsEnabled;
            TXTLocation.Enabled = IsEnabled;
            DDLOriginLevel.Enabled = IsEnabled;
            TXTOrigin.Enabled = IsEnabled;
            DDLDestinationLevel.Enabled = IsEnabled;
            TXTDestination.Enabled = IsEnabled;
            DDLCharge.Enabled = IsEnabled;
            ddlGLCode.Enabled = IsEnabled;
            DDLPaymentType.Enabled = IsEnabled;
            DDLChargeType.Enabled = IsEnabled;
            TXTDiscount.Enabled = IsEnabled;
            TXTCommision.Enabled = IsEnabled;
            TXTTax.Enabled = IsEnabled;
            DDLHeadBasis.Enabled = IsEnabled;
            TXTMinimum.Enabled = IsEnabled;
            TXTCharge.Enabled = IsEnabled;
            DDLWtType.Enabled = IsEnabled;

            TXTFlightNumber.Enabled = IsEnabled;
            TXTHandlingCode.Enabled = IsEnabled;
            TXTIATAComCode.Enabled = IsEnabled;
            TXTFlightCarrier.Enabled = IsEnabled;
            TXTShipperCode.Enabled = IsEnabled;
            TXTAgentCode.Enabled = IsEnabled;
            TXTAirLineCode.Enabled = IsEnabled;
            TXTParamORG.Enabled = IsEnabled;
            TXTParamDest.Enabled = IsEnabled;

            btnAdd.Enabled = IsEnabled;
            btnDelete.Enabled = IsEnabled;
            btnSave.Enabled = IsEnabled;

            GRDException.Enabled = IsEnabled;
            IBAC.Enabled = IsEnabled;
            IBAD.Enabled = IsEnabled;
            IBCC.Enabled = IsEnabled;
            IBFN.Enabled = IsEnabled;
            IBSC.Enabled = IsEnabled;
            IBStartDate.Enabled = IsEnabled;
            IBEndDate.Enabled = IsEnabled;
            IBLoc.Enabled = IsEnabled;
            IBOrigin.Enabled = IsEnabled;
            //IBCurrency.Enabled = IsEnabled;
            TXTCurrency.Enabled = IsEnabled;

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

            ddlType.Enabled = IsEnabled;
            ddlViaStation.Enabled = IsEnabled;
            GRDRateSlabs.Enabled = IsEnabled;
            txtComment.Enabled = IsEnabled;
            TXTParamCountry.Enabled = IsEnabled;
            TXTParamCountryD.Enabled = IsEnabled;
            TXTParamPT.Enabled = IsEnabled;
            TXTParamHand.Enabled = IsEnabled;
            RBIncHand.Enabled = IsEnabled;
            RBIncCountryD.Enabled= IsEnabled;
            RBIncCountry.Enabled = IsEnabled;
            RBIncPT.Enabled = IsEnabled;
            TXTEquipmentType.Enabled = IsEnabled;

        }

        private void LoadAirportCombo()
        {
            StockAllocationBAL objBAL = new StockAllocationBAL();
            DataSet City = objBAL.GetCityCode();
            ddlViaStation.DataSource = City.Tables[0];
            ddlViaStation.DataTextField = "CityCode";
            ddlViaStation.DataValueField = "CityCode";
            ddlViaStation.DataBind();
            ddlViaStation.Items.Insert(0, "Select");
            ddlViaStation.SelectedIndex = 0;
        }

        private void LoadChargeType()
        {
            try
            {
                OtherChargesBAL objBAL = new OtherChargesBAL();
                DataSet oth = objBAL.GetChargeType();
                if (oth != null)
                {
                    if (oth.Tables.Count > 0)
                    {
                        DDLCharge.DataSource = oth.Tables[0];
                        DDLCharge.DataTextField = "ChargeText";
                        DDLCharge.DataValueField = "ChargeValue";
                        DDLCharge.DataBind();
                    }
                }
            }
            catch (Exception ex) 
            { }
        }

        protected void BindRepeaterData(int SrNo)
        {
            try
            {
                //RepDetails.DataSource = null;
                if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                {
                    //string Date = Session["IT"].ToString();
                    rtlid = Convert.ToInt32(Request.QueryString["SrNo"].ToString());
                    string[] param = { "ChargeHeadCode" };
                    SqlDbType[] dbtypes = { SqlDbType.VarChar };
                    object[] values = { rtlid };
                    DataSet ds = new DataSet();
                    ds = da.SelectRecords("SP_GetRemarksOtherCharges", param, values, dbtypes);


                    RepDetails.DataSource = ds;
                    RepDetails.DataBind();
                }
                else if(SrNo>0)
                {
                    string[] param = { "ChargeHeadCode" };
                    SqlDbType[] dbtypes = { SqlDbType.VarChar };
                    object[] values = { SrNo };
                    DataSet ds = new DataSet();
                    ds = da.SelectRecords("SP_GetRemarksOtherCharges", param, values, dbtypes);
                    RepDetails.DataSource = ds;
                    RepDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
            }

        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                string errormessage = "";
                DataSet dsResult = new DataSet();

                if (TXTChargeHeadCode.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Kindly select other charge head code.');", true);
                }

                if (objBAL.GetOtherChargeInfo(TXTChargeHeadCode.Text.Trim(), ref dsResult, ref errormessage))
                {
                    TXTAgentCode.Text = dsResult.Tables[1].Rows[0]["AgentCode"].ToString();
                    TXTAirLineCode.Text = dsResult.Tables[1].Rows[0]["AirlineCode"].ToString();
                    TXTCharge.Text = dsResult.Tables[0].Rows[0]["PerUnitCharge"].ToString();
                    TXTChargeHeadCode.Text = dsResult.Tables[0].Rows[0]["ChargeHeadCode"].ToString();
                    TXTChargeHeadName.Text = dsResult.Tables[0].Rows[0]["ChargeHeadName"].ToString();
                    TXTCommision.Text = dsResult.Tables[0].Rows[0]["CommPercent"].ToString();
                    //TXTCurrency.Text = dsResult.Tables[0].Rows[0]["CurrencyCode"].ToString();
                    FillCurrencyCodes(TXTCurrency, dsResult.Tables[0].Rows[0]["CurrencyCode"].ToString());
                    TXTDestination.Text = dsResult.Tables[0].Rows[0]["Destination"].ToString();
                    TXTDiscount.Text = dsResult.Tables[0].Rows[0]["DiscountPercent"].ToString();
                    //TXTEndDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["EndDate"].ToString()).ToString("yyyy-MM-dd");
                    TXTEndDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["EndDate"].ToString()).ToString("dd/MM/yyyy");

                    TXTFlightCarrier.Text = dsResult.Tables[1].Rows[0]["FlightCarrier"].ToString();
                    TXTFlightNumber.Text = dsResult.Tables[1].Rows[0]["FlightNum"].ToString();
                    TXTHandlingCode.Text = dsResult.Tables[1].Rows[0]["HandlingCode"].ToString();
                    TXTIATAComCode.Text = dsResult.Tables[1].Rows[0]["CommCode"].ToString();
                    TXTLocation.Text = dsResult.Tables[0].Rows[0]["Location"].ToString();
                    TXTMinimum.Text = dsResult.Tables[0].Rows[0]["MinimumCharge"].ToString();
                    TXTOrigin.Text = dsResult.Tables[0].Rows[0]["Origin"].ToString();
                    TXTShipperCode.Text = dsResult.Tables[1].Rows[0]["ShipperCode"].ToString();
                    //TXTStartDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["StartDate"].ToString()).ToString("yyyy-MM-dd");
                    TXTStartDate.Text = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy");

                    TXTTax.Text = dsResult.Tables[0].Rows[0]["ServiceTax"].ToString();

                    txtMaxValue.Text = dsResult.Tables[0].Rows[0]["MaxValue"].ToString();
                    txtMaxCost.Text = dsResult.Tables[0].Rows[0]["MaxCost"].ToString();

                    string OCCode = dsResult.Tables[0].Rows[0]["OCCode"].ToString();
                    ddlOCCode.SelectedIndex = ddlOCCode.Items.IndexOf(ddlOCCode.Items.FindByValue(OCCode));

                    DDLChargeType.Text = dsResult.Tables[0].Rows[0]["ChargeType"].ToString();
                    DDLDestinationLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["DestinationLevel"].ToString());
                    DDLHeadBasis.SelectedValue = dsResult.Tables[0].Rows[0]["ChargeHeadBasis"].ToString();
                    DDLLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["LocationLevel"].ToString());
                    DDLOriginLevel.SelectedIndex = int.Parse(dsResult.Tables[0].Rows[0]["OriginLevel"].ToString());
                    DDLParticipationType.SelectedValue = dsResult.Tables[0].Rows[0]["ParticipationType"].ToString();
                    DDLPaymentType.SelectedValue = dsResult.Tables[0].Rows[0]["PaymentType"].ToString();
                    DDLWtType.SelectedValue = dsResult.Tables[0].Rows[0]["WeightType"].ToString();

                    CHKRefundable.Checked = Convert.ToBoolean(dsResult.Tables[0].Rows[0]["Refundable"].ToString());


                    RBIncAC.Checked = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["RBIncAC"].ToString());
                    RBIncAD.Checked = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["RBIncAD"].ToString());
                    RBIncCC.Checked = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["RBIncCC"].ToString());
                    RBIncFC.Checked = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["RBIncFC"].ToString());
                    RBIncFN.Checked = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["RBIncFN"].ToString());
                    RBIncHC.Checked = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["RBIncHC"].ToString());
                    RBIncSC.Checked = Convert.ToBoolean(dsResult.Tables[1].Rows[0]["RBIncSC"].ToString());


                    dsExceptions = ((DataSet)Session["dsExceptions"]).Clone();

                    foreach (DataRow row in dsResult.Tables[2].Rows)
                    {

                        DataRow newrow = dsExceptions.Tables[0].NewRow();

                        newrow["Agent"] = row["AgentCode"].ToString();
                        newrow["Commision"] = row["CommissionPercent"].ToString();
                        newrow["Discount"] = row["DiscountPercent"].ToString();
                        newrow["Tax"] = row["ServiceTax"].ToString();

                        dsExceptions.Tables[0].Rows.Add(newrow);
                    }

                    GRDException.DataSource = dsExceptions.Tables[0];
                    GRDException.DataBind();


                    for (int i = 0; i < dsExceptions.Tables[0].Rows.Count; i++)
                    {
                        LoadAgentDropdown(((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")));
                        ((DropDownList)GRDException.Rows[i].FindControl("DDLAgent")).Text = dsExceptions.Tables[0].Rows[i]["Agent"].ToString();
                    }


                    Session["dsExceptions"] = dsExceptions.Copy();

                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error:" + ex.Message + "');", true);
            }
        }
   
        #region Fill Currency Previous
        //private void FillCurrencyCodes(DropDownList drp, string SelectedCurrency)
        //{
        //    try
        //    {
        //        BALCurrency BalCur = new BALCurrency();
        //        DataSet dsCur = BalCur.GetCurrencyCodeList("");
        //        if (dsCur != null && dsCur.Tables.Count > 0)
        //        {
        //            if (dsCur.Tables[0].Rows.Count > 0)
        //            {
        //                drp.Items.Clear();
        //                //drpWWR.Items.Add("Select");
        //                try
        //                {
        //                    drp.DataSource = dsCur.Tables[0];
        //                    drp.DataTextField = "Code";
        //                    drp.DataValueField = "SrNo";
        //                    drp.DataBind();
        //                }
        //                catch (Exception ex) { }
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

        //    }
        //}
        #endregion


        #region Fill Currency
        private void FillCurrencyCodes(DropDownList drp, string SelectedCurrency)
        {
            try
            {
                BALCurrency BalCur = new BALCurrency();
                BookingBAL objBLL = new BookingBAL();
                DataSet dsCur = BalCur.GetCurrencyCodeList("");
                DataSet dsCurrency = null;

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
                            drp.DataValueField = "ID";
                            drp.DataBind();
                        }
                        catch (Exception ex) { }

                        objBLL.GetAirpotCurrency(Session["Station"].ToString(), ref dsCurrency);
                        if (dsCurrency != null && dsCurrency.Tables[0].Rows.Count > 0)
                        {
                            string Currency = dsCurrency.Tables[0].Rows[0]["BookingCurrrency"].ToString();
                            drp.Text = Currency;
                        }
                        else
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

        #region Load DropDowns
        public void LoadGLDropDown()
        {
            try
            {
                DataSet ds = da.SelectRecords("SP_GetGLAccountCode");
                ddlGLCode.Items.Insert(0, "Select");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlGLCode.DataSource = ds;
                            ddlGLCode.DataTextField = "GLAccountCode";
                            ddlGLCode.DataValueField = "GLAccountCode";
                            ddlGLCode.DataBind();
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

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
                // Code commented - Table "OCCodeMaster" deleted from DB
                //dad = new SqlDataAdapter("SELECT DISTINCT OCCode+'-'+OCDescription FROM dbo.OCCodeMaster WHERE isActive = 1 AND OCCode IS NOT NULL", con);

                // get the data from table "tblOCMaster" updated on 9 July 2014
                dad = new SqlDataAdapter("SELECT DISTINCT OCCode+'-'+OCDesc FROM dbo.tblOCMaster WHERE isActiove = 1 AND OCCode IS NOT NULL", con);
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

        protected void btnULDAdd_Click(object sender, EventArgs e)
        {
            SaveGridULDState();
            AddNewULDSlab();
        }
        public bool SaveGridULDState()
        {
            dsULDSlabs = (DataSet)Session["dsULDSlabs"];

            for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
            {
                dsULDSlabs.Tables[0].Rows[i]["ULDType"] = ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).Text;
                dsULDSlabs.Tables[0].Rows[i]["Type"] = ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType1")).Text;
                try
                {
                    dsULDSlabs.Tables[0].Rows[i]["Weight"] = Convert.ToDouble(((TextBox)grdULDslabs.Rows[i].FindControl("TXTULDWeight")).Text);
                }
                catch (Exception ex)
                {
                    dsULDSlabs.Tables[0].Rows[i]["Weight"] = 0;
                }
                dsULDSlabs.Tables[0].Rows[i]["Charge"] = Convert.ToDouble(((TextBox)grdULDslabs.Rows[i].FindControl("TXTULDCharge")).Text);

                if (dsULDSlabs.Tables[0].Rows[i]["Weight"].ToString().Trim() == "")
                    dsULDSlabs.Tables[0].Rows[i]["Weight"] = "0";

                if (dsULDSlabs.Tables[0].Rows[i]["Charge"].ToString().Trim() == "")
                    dsULDSlabs.Tables[0].Rows[i]["Charge"] = "0";


                //if (ckhULDRate.Checked == true)
                //{
                    if (Convert.ToString(dsULDSlabs.Tables[0].Rows[i]["Type"]) == "M")
                    {
                        //if (Convert.ToInt32(dsULDSlabs.Tables[0].Rows[i]["Weight"]) == 0)
                        //{
                        //    lblStatus.Text = "Minimum ULD weight should not be 0.";
                        //    lblStatus.ForeColor = Color.Blue;
                        //    return false;
                        //}

                        //if (Convert.ToDecimal(dsULDSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                        //{
                        //    lblStatus.Text = "Minimum ULD charge should not be 0.";
                        //    lblStatus.ForeColor = Color.Blue;
                        //    return false;
                        //}
                    }

                    if (Convert.ToString(dsULDSlabs.Tables[0].Rows[i]["Type"]) == "OverPivot")
                    {
                        //if (Convert.ToInt32(dsULDSlabs.Tables[0].Rows[i]["Weight"]) == 0)
                        //{
                        //    lblStatus.Text = "Normal ULD weight should nto be 0.";
                        //    lblStatus.ForeColor = Color.Blue;
                        //    return false;
                        //}

                        //if (Convert.ToDecimal(dsULDSlabs.Tables[0].Rows[i]["Charge"]) == 0)
                        //{
                        //    lblStatus.Text = "Normal ULD charge should not be 0.";
                        //    lblStatus.ForeColor = Color.Blue;
                        //    return false;
                        //}
                    }
                //}
                //End
            }

            Session["dsULDSlabs"] = dsULDSlabs.Copy();
            return true;
        }
        public void AddNewULDSlab()
        {
            try
            {
                dsULDSlabs = (DataSet)Session["dsULDSlabs"];
                dsULDSlabs.Tables[0].Rows.Add(dsULDSlabs.Tables[0].NewRow());

                if (dsULDSlabs.Tables[0].Rows.Count == 1)
                {
                    //dsULDSlabs.Tables[0].Rows[0]["ULDType"] = "AKE";
                    //dsULDSlabs.Tables[0].Rows[0]["Type"] = "M";
                    dsULDSlabs.Tables[0].Rows[0]["SrNo"] = 0;
                    dsULDSlabs.Tables[0].Rows[0]["Weight"] = 0;
                    dsULDSlabs.Tables[0].Rows[0]["Charge"] = 0;

                }
                else if (dsULDSlabs.Tables[0].Rows.Count == 2)
                {
                    //dsULDSlabs.Tables[0].Rows[1]["ULDType"] = "UKA";
                    //dsULDSlabs.Tables[0].Rows[1]["Type"] = "OverPivot";
                    dsULDSlabs.Tables[0].Rows[1]["SrNo"] = 1;
                    dsULDSlabs.Tables[0].Rows[1]["Weight"] = 0;
                    dsULDSlabs.Tables[0].Rows[1]["Charge"] = 0;
                }
                else
                {
                    //dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["ULDType"] = "UKA";
                    //dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["Type"] = "OverPivot";
                    dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["SrNo"] = dsULDSlabs.Tables[0].Rows.Count - 1;
                    dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["Weight"] = 0;
                    dsULDSlabs.Tables[0].Rows[dsULDSlabs.Tables[0].Rows.Count - 1]["Charge"] = 0;
                }

                grdULDslabs.DataSource = null;
                grdULDslabs.DataSource = dsULDSlabs.Tables[0].Copy();
                grdULDslabs.DataBind();

                for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
                {
                    LoadULDTypes(i);
                    ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).Text = dsULDSlabs.Tables[0].Rows[i]["ULDType"].ToString();

                }
                for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
                {
                    ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType1")).Text = dsULDSlabs.Tables[0].Rows[i]["Type"].ToString();

                }

                Session["dsULDSlabs"] = dsULDSlabs;
                //if (ckhULDRate.Checked == true)
                //{
                //    //btnULDAdd.Visible = true;
                //    //btnULDDel.Visible = true;
                //    //grdULDslabs.Visible = true;
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

                //}
                //else
                //{

                //    //btnULDAdd.Visible = false;
                //    //btnULDDel.Visible = false;
                //    //grdULDslabs.Visible = false;
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

                //}

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

        }

        protected void btnULDDel_Click(object sender, EventArgs e)
        {
            SaveGridULDState();

            dsULDSlabs = (DataSet)Session["dsULDSlabs"];
            DataSet dsULDSlabsTemp = dsULDSlabs.Copy();

            for (int i = 0; i < dsULDSlabs.Tables[0].Rows.Count; i++)
            {
                if (((CheckBox)grdULDslabs.Rows[i].FindControl("CHKULD")).Checked)
                {
                    string srno = dsULDSlabs.Tables[0].Rows[i]["SrNo"].ToString();

                    for (int j = 0; j < dsULDSlabsTemp.Tables[0].Rows.Count; j++)
                    {
                        if (srno == dsULDSlabsTemp.Tables[0].Rows[j]["SrNo"].ToString())
                        {
                            dsULDSlabsTemp.Tables[0].Rows.Remove(dsULDSlabsTemp.Tables[0].Rows[j]);
                            break;
                        }
                    }
                }
            }

            grdULDslabs.DataSource = null;
            grdULDslabs.DataSource = dsULDSlabsTemp.Tables[0];
            grdULDslabs.DataBind();


            for (int i = 0; i < dsULDSlabsTemp.Tables[0].Rows.Count; i++)
            {
                ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType")).Text = dsULDSlabsTemp.Tables[0].Rows[i]["ULDType"].ToString();

            }
            for (int i = 0; i < dsULDSlabsTemp.Tables[0].Rows.Count; i++)
            {
                ((DropDownList)grdULDslabs.Rows[i].FindControl("DdlULDType1")).Text = dsULDSlabsTemp.Tables[0].Rows[i]["Type"].ToString();

            }

            Session["dsULDSlabs"] = dsULDSlabsTemp.Copy();
            //if (ckhULDRate.Checked == true)
            //{
            //    //btnULDAdd.Visible = true;
            //    //btnULDDel.Visible = true;
            //    //grdULDslabs.Visible = true;
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivShow();</script>", false);

            //}
            //else
            //{

            //    //btnULDAdd.Visible = false;
            //    //btnULDDel.Visible = false;
            //    //grdULDslabs.Visible = false;
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>DivHide();</script>", false);

            //}

        }
        #region Load ULD Types
        public void LoadULDTypes(int i)
        {
            try
            {

                DataSet ds = da.SelectRecords("SP_GetAvailabeULDTypes");
                DropDownList ddl = new DropDownList();
                {
                    ddl = ((DropDownList)(grdULDslabs.Rows[i].FindControl("DdlULDType")));
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddl.DataSource = ds;
                            ddl.DataMember = ds.Tables[0].TableName;
                            ddl.DataTextField = "TypeCode";
                            ddl.DataValueField = "VAL";
                            ddl.DataBind();
                            //ddl.Items.Clear();
                            //for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            //{
                            //    ddl.Items.Add(ds.Tables[0].Rows[k][0].ToString());
                            //}
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        protected void btnAddAnother_Click(object sender, EventArgs e)
        {
            btnAddAnother.Visible = false;
            btnSave.Enabled = true;
            btnSave.Text = "Save";
            HidSrNo.Value = "0";
         //   ddlDestination.SelectedIndex = 0;
            lblStatus.Text = "";
            ViewState["IsAddAnother"] = "T";
           // ckhULDRate.Checked = true;
        }

        public void ClearALL()
        {
            btnSave.Enabled = false;
            btnAddAnother.Visible = true;
            ViewState["IsAddAnother"] = "F";            
        }

     }
}
