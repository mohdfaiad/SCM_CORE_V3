using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using QID.DataAccess;
using System.Data;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class AirlineMaster : System.Web.UI.Page
    {

        #region Variables
        SQLServer db = new SQLServer(Global.GetConnectionString());
        AirlineMasterBAL ObjBAL = new AirlineMasterBAL();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadingDropDowns(ddlCountry, ddlCity, ddlLanguage, ddlCurrency, ddlBillingCurrency);
                    FillPartnerType();

                    if (Request.QueryString["Command"] == "Edit" || Request.QueryString["Command"] == "View")
                    {
                        getRateLineDetails();
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>hideDiv('<%= ckhULDRate.ClientID %>');</script>", false);
                    }
                    if (Request.QueryString["Command"] == "Edit")
                    {
                        btnSave.Text = "Update";
                        //Session["mode"] = "Edit";

                    }
                    else if (Request.QueryString["Command"] == "View")
                    {
                        btnSave.Visible = false;
                        //Session["mode"] = "View";
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Loading DropDown
        public void LoadingDropDowns(DropDownList ddlCountry, DropDownList ddlCity, DropDownList ddlLanguage, DropDownList ddlCurrency,DropDownList ddlBillingCurrency)
        {
            try
            {
                DataSet ds = db.SelectRecords("SP_GetDropDownDataAirlineMaster");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlCountry.DataSource = ds.Tables[0];
                            ddlCountry.DataTextField = "CountryName";
                            ddlCountry.DataValueField = "CountryCode";
                            ddlCountry.DataBind();
                            ddlCountry.Items.Insert(0, "Select");
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ddlCity.DataSource = ds.Tables[1];
                            ddlCity.DataTextField = "AirportName";
                            ddlCity.DataValueField = "AirportCode";
                            ddlCity.DataBind();
                            ddlCity.Items.Insert(0, "Select");
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            //ddlCurrency.DataSource = ds.Tables[2];
                            //ddlCurrency.DataTextField = "CurrencyCode";
                            //ddlCurrency.DataValueField = "CurrencyCode";
                            //ddlCurrency.DataBind();
                            //ddlCurrency.Items.Insert(0, "Select");

                            ddlCurrency.DataSource = ds;
                            ddlCurrency.DataMember = ds.Tables[2].TableName;
                            ddlCurrency.DataTextField = ds.Tables[2].Columns["CurrencyName"].ColumnName;
                            ddlCurrency.DataValueField = ds.Tables[2].Columns["CurrencyCode"].ColumnName;
                            ddlCurrency.DataBind();
                            ddlCurrency.Items.Insert(0, new ListItem("Select", "0"));
                            ddlCurrency.SelectedIndex = -1;

                            //ddlBillingCurrency.DataSource = ds.Tables[2];
                            //ddlBillingCurrency.DataTextField = "CurrencyCode";
                            //ddlBillingCurrency.DataValueField = "CurrencyCode";
                            //ddlBillingCurrency.DataBind();
                            //ddlBillingCurrency.Items.Insert(0, "Select");

                            ddlBillingCurrency.DataSource = ds;
                            ddlBillingCurrency.DataMember = ds.Tables[2].TableName;
                            ddlBillingCurrency.DataTextField = ds.Tables[2].Columns["CurrencyName"].ColumnName;
                            ddlBillingCurrency.DataValueField = ds.Tables[2].Columns["CurrencyCode"].ColumnName;
                            ddlBillingCurrency.DataBind();
                            ddlBillingCurrency.Items.Insert(0, new ListItem("Select", "0"));
                            ddlBillingCurrency.SelectedIndex = -1;

                        }
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            ddlLanguage.DataSource = ds.Tables[3];
                            ddlLanguage.DataTextField = "LanguageName";
                            ddlLanguage.DataValueField = "LanguageCode";
                            ddlLanguage.DataBind();
                            //ddlLanguage.Items.Insert(0, "Select");
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Button Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MasterAuditBAL ObjMAL = new MasterAuditBAL();

                lblStatus.Text = "";
                DateTime dtTemp;
                object[] QueryValues = new object[35];
                if (txtValidFrom.Text != "" && !DateTime.TryParse(txtValidFrom.Text, out dtTemp))
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Please enter a valid date!";
                    return;
                }

                if (txtValidTo.Text != "" && !DateTime.TryParse(txtValidTo.Text, out dtTemp))
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Please enter a valid date!";
                    return;
                }
                if (ddlBillingCurrency.SelectedIndex == 0 || ddlCity.SelectedIndex == 0 || ddlCountry.SelectedIndex == 0 || ddlCurrency.SelectedIndex == 0 || ddlPartnerType.SelectedIndex == 0)
                {
                    lblStatus.Text = "Please select all the mandatory fields and try again..";
                    lblStatus.ForeColor = Color.Red;
                    return;

                }
                else
                {
                    try
                    { Convert.ToDecimal(txtTolerance.Text.Trim()); }
                    catch (Exception ex)
                    { txtTolerance.Text = "0"; }

                    QueryValues[0] = txtAirlineName.Text;
                    QueryValues[1] = txtLegalName.Text;
                    QueryValues[2] = txtPrefix.Text.Trim();
                    QueryValues[3] = txtDesigCode.Text.Trim();
                    QueryValues[4] = txtAirlineLocId.Text.Trim();
                    QueryValues[5] = txtAccountingCode.Text.Trim();
                    QueryValues[6] = txtRegistrationID.Text.Trim();
                    QueryValues[7] = chkDigitalSign.Checked.ToString();
                    QueryValues[8] = chkSuspend.Checked.ToString();
                    QueryValues[9] = txtPresident.Text;
                    QueryValues[10] = txtCFO.Text;
                    QueryValues[11] = ddlCurrency.SelectedItem.Value;
                    QueryValues[12] = ddlLanguage.SelectedItem.Value;
                    QueryValues[13] = txtTaxRegID.Text.Trim();
                    QueryValues[14] = txtAddTaxID.Text.Trim();
                    QueryValues[15] = ddlSettlement.SelectedItem.Value;
                    QueryValues[16] = txtAddress.Text;
                    QueryValues[17] = ddlCountry.SelectedItem.Value;
                    QueryValues[18] = ddlCity.SelectedItem.Value;
                    QueryValues[19] = txtPostalCode.Text;
                    QueryValues[20] = ddlBillingCurrency.SelectedItem.Value;
                    QueryValues[21] = Session["Username"].ToString();
                    QueryValues[22] = ddlPartnerType.SelectedItem.Text.Trim();
                    QueryValues[23] = txtValidFrom.Text.Trim();
                    QueryValues[24] = txtValidTo.Text.Trim();
                    QueryValues[25] = chkIsScheduled.Checked.ToString();
                    QueryValues[26] = txtSITAID.Text.Trim();
                    QueryValues[27] = txtEmailID.Text.Trim();


                    if (chkPartialAcceptance.Checked)
                    { QueryValues[28] = "1"; }
                    else
                    { QueryValues[28] = "0"; }
                    
                    QueryValues[29] = txtTolerance.Text.Trim();
                    QueryValues[30] = chkOtherCharges.Checked;
                    QueryValues[31] = chkCustomsMsg.Checked;
                    QueryValues[32] = ddlonbilling.SelectedValue.ToString();
                    if (chkautogeninvoice.Checked)
                    {
                        QueryValues[33] = true;
                    }
                    else
                    {
                        QueryValues[33] = false;
                    }

                    QueryValues[34] = ddlBillType.SelectedItem.Text.Trim();
                }

                if (ObjBAL.SaveAirline(QueryValues))
                {
                    #region for Master Audit Log
                    #region Prepare Parameters
                    object[] Paramsmaster = new object[7];
                    int count = 0;

                    //1

                    Paramsmaster.SetValue("Partner Master", count);
                    count++;

                    //2
                    Paramsmaster.SetValue(txtAirlineName.Text, count);
                    count++;

                    //3

                    Paramsmaster.SetValue("SAVE", count);
                    count++;

                    //4
                    string Msg = "Partner Added";
                    Paramsmaster.SetValue(Msg, count);
                    count++;


                    //5
                    string Desc = "Partner Name:" + txtAirlineName.Text + "/Partner Prefix:" + txtPrefix.Text;
                    Paramsmaster.SetValue(Desc, count);
                    count++;

                    //6

                    Paramsmaster.SetValue(Session["UserName"], count);
                    count++;

                    //7
                    Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                    count++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsmaster);
                    #endregion

                    lblStatus.Text = "Records Saved Successfully!";
                    lblStatus.ForeColor = Color.Green;
                    if(btnSave.Text=="Save")
                    ClearControls();
                    ddlPartnerType.Enabled = txtPrefix.Enabled = txtAirlineName.Enabled = true;
                }
                else
                {
                    lblStatus.Text = "Records Saving Failed! Please try again..";
                    lblStatus.ForeColor = Color.Red;
                }


               
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Fill Partner Type Master
        public void FillPartnerType()
        {
            try
            {
                PartnerBAL objPM = new PartnerBAL();
                DataSet dsPartnerType = objPM.GetPartnerTypeMaster();
                if (dsPartnerType != null && dsPartnerType.Tables.Count > 0)
                {
                    if (dsPartnerType.Tables[0].Rows.Count > 0)
                    {
                        ddlPartnerType.Items.Clear();
                        //drpWWR.Items.Add("Select");
                        ddlPartnerType.DataSource = dsPartnerType.Tables[0];
                        ddlPartnerType.DataTextField = "Code";
                        ddlPartnerType.DataValueField = "Description";
                        ddlPartnerType.DataBind();
                        ddlPartnerType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in Fill PartnerType!";
            }
        }
        #endregion

        #region Get Partner Details
        public void getRateLineDetails()
        {
            string Id = Request.QueryString["Id"].ToString();
            Session["MsgId"] = Id;
            DataSet dsRateCard = new DataSet();
            dsRateCard = ObjBAL.GetPartnerListForId(Id);
            fillPartnerDetails(dsRateCard);
        }
        #endregion Get Partner Details

        #region Fill Partner Details
        public void fillPartnerDetails(DataSet dsMsgDetails)
        {
            //Code to fill details of msg from dataset

           string type = dsMsgDetails.Tables[0].Rows[0]["PartnerType"].ToString();
           ddlPartnerType.SelectedIndex = ddlPartnerType.Items.IndexOf(((ListItem)ddlPartnerType.Items.FindByText(type)));
            txtAirlineName.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerName"].ToString();
            txtPrefix.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerPrefix"].ToString();
            txtDesigCode.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerCode"].ToString();
            txtLegalName.Text = dsMsgDetails.Tables[0].Rows[0]["LegalName"].ToString();
            txtValidFrom.Text = dsMsgDetails.Tables[0].Rows[0]["validFrom"].ToString();
            txtValidTo.Text = dsMsgDetails.Tables[0].Rows[0]["validTo"].ToString();
            txtSITAID.Text = dsMsgDetails.Tables[0].Rows[0]["SITAiD"].ToString();
            txtEmailID.Text = dsMsgDetails.Tables[0].Rows[0]["EmailiD"].ToString();
            txtAirlineLocId.Text = dsMsgDetails.Tables[0].Rows[0]["ZoneId"].ToString();
            txtAccountingCode.Text = dsMsgDetails.Tables[0].Rows[0]["AccountCode"].ToString();
            string digsign = dsMsgDetails.Tables[0].Rows[0]["DigitalSignature"].ToString();
            string issusp = dsMsgDetails.Tables[0].Rows[0]["IsSuspended"].ToString();
            string issched=dsMsgDetails.Tables[0].Rows[0]["IsScheduled"].ToString();
            string PartialAcceptance = dsMsgDetails.Tables[0].Rows[0]["AcceptPartial"].ToString();
            txtTolerance.Text = dsMsgDetails.Tables[0].Rows[0]["AcceptedTolerance"].ToString();
            string OtherChares = dsMsgDetails.Tables[0].Rows[0]["OCApplied"].ToString();
            string CustomsMsg = dsMsgDetails.Tables[0].Rows[0]["IsAutoCustMsg"].ToString();

            if (PartialAcceptance == "1")
                chkPartialAcceptance.Checked = true;
            else
                chkPartialAcceptance.Checked = false;

            if (OtherChares == "True")
                chkOtherCharges.Checked = true;
            else chkOtherCharges.Checked = false;

            if (CustomsMsg == "True")
                chkCustomsMsg.Checked = true;
            else chkCustomsMsg.Checked = false;


            if(digsign=="True")
                chkDigitalSign.Checked=true;
            else chkDigitalSign.Checked = false;

            if (issusp == "True")
                chkSuspend.Checked = true;
            else chkSuspend.Checked = false;
            
            if (issched == "True")
                chkIsScheduled.Checked = true;
            else chkIsScheduled.Checked = false;

            txtRegistrationID.Text = dsMsgDetails.Tables[0].Rows[0]["RegistrationId"].ToString();
            txtPresident.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerPresident"].ToString();
            txtCFO.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerCFO"].ToString();
            txtCFO.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerCFO"].ToString();

            string listcurr=dsMsgDetails.Tables[0].Rows[0]["ListingCurrency"].ToString();
            string billcurr = dsMsgDetails.Tables[0].Rows[0]["BillingCurrency"].ToString();
            //ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(((ListItem)ddlCurrency.Items.FindByText(listcurr)));
            ddlCurrency.SelectedValue = listcurr;
            //ddlBillingCurrency.SelectedIndex = ddlBillingCurrency.Items.IndexOf(((ListItem)ddlBillingCurrency.Items.FindByText(billcurr)));
            ddlBillingCurrency.SelectedValue = billcurr;

            txtTaxRegID.Text=dsMsgDetails.Tables[0].Rows[0]["TaxRegistrationID"].ToString();
            txtAddTaxID.Text = dsMsgDetails.Tables[0].Rows[0]["AdditionalTaxRegID"].ToString();

            string setmethod = dsMsgDetails.Tables[0].Rows[0]["SettlementMethod"].ToString();
            ddlSettlement.SelectedIndex = ddlSettlement.Items.IndexOf(((ListItem)ddlSettlement.Items.FindByText(setmethod)));

            txtAddress.Text = dsMsgDetails.Tables[0].Rows[0]["Address"].ToString();

            string country = dsMsgDetails.Tables[0].Rows[0]["Country"].ToString();
            ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(((ListItem)ddlCountry.Items.FindByValue(country)));

            string city = dsMsgDetails.Tables[0].Rows[0]["City"].ToString();
            ddlCity.SelectedIndex = ddlCity.Items.IndexOf(((ListItem)ddlCity.Items.FindByValue(city)));

            txtPostalCode.Text = dsMsgDetails.Tables[0].Rows[0]["PostalCode"].ToString();

            string lng = dsMsgDetails.Tables[0].Rows[0]["Language"].ToString();
            ddlLanguage.SelectedIndex = ddlLanguage.Items.IndexOf(((ListItem)ddlLanguage.Items.FindByValue(lng)));

            ddlonbilling.SelectedValue = dsMsgDetails.Tables[0].Rows[0]["BillingEvent"].ToString();
            ddlBillType.SelectedValue = dsMsgDetails.Tables[0].Rows[0]["PartnerBillType"].ToString();

            if (dsMsgDetails.Tables[0].Rows[0]["AutoGenerateInvoice"].ToString() == "True")
            {
                chkautogeninvoice.Checked = true;
            }
            else
            {
                chkautogeninvoice.Checked = false;
            }
            
            ddlPartnerType.Enabled = txtPrefix.Enabled = txtAirlineName.Enabled = false;
        }
        #endregion Fill Partner Details

        protected void ClearControls()
        {
            ddlBillingCurrency.SelectedIndex = 0;
            ddlCity.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;
            ddlCurrency.SelectedIndex = 0;
            ddlLanguage.SelectedIndex = 0;
            ddlPartnerType.SelectedIndex = 0;
            ddlSettlement.SelectedIndex = 0;

            txtAccountingCode.Text = txtAddress.Text = txtAddTaxID.Text = txtAirlineLocId.Text = string.Empty;
            txtAirlineName.Text = txtCFO.Text = txtDesigCode.Text = txtEmailID.Text = txtLegalName.Text = string.Empty;
            txtPostalCode.Text = txtPrefix.Text = txtPresident.Text = txtRegistrationID.Text = txtSITAID.Text = string.Empty;
            txtTaxRegID.Text = txtValidFrom.Text = txtValidTo.Text = string.Empty;

            chkDigitalSign.Checked = chkIsScheduled.Checked = chkSuspend.Checked = false;
            chkPartialAcceptance.Checked = true;
            txtTolerance.Text = string.Empty;
            chkOtherCharges.Checked = false;
            chkCustomsMsg.Checked = false;

        }

    }
}
