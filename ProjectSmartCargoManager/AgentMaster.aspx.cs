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
using System.Drawing;
using System.Configuration;
using BAL;

/*

 2012/05/23 vinayak.
 2012/05/31 vinayak
 16/7/2012 swapnil
 18/7/2012 Stock
*/
namespace ProjectSmartCargoManager
{
    public partial class AgentMaster : System.Web.UI.Page
    {
        #region Variables
        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        string gvSortExpr = String.Empty;
        SqlConnection sc = null;
        SqlCommand scmd = null;
        SqlDataReader sdr = null;
        SqlConnection sc1 = null;
        SqlCommand scmd1 = null;
        DataSet dsCredit;

        SQLServer da = new SQLServer(Global.GetConnectionString());
        AgentBAL objBLL = new AgentBAL();
        BLawbStockAllocBAL BLObj = new BLawbStockAllocBAL();
        UserCreationBAL objUserBAL = new UserCreationBAL();

        DataSet dsStockAllocation;
        #endregion Variables
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    ddlAgentType.SelectedIndex = 1;
                    ddlcontrolinglocator.SelectedIndex = 1;
                    ddlcontrolinglocator_SelectedIndexChanged(null, null);

                   // txtvalidfrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtvalidfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    //Load Deals Grid

                    LoadIACCode();
                    //shashi.....
                    GetCurrencyCode();

                    LoadCCSFCode();
                    HidFlag.Value = "";
                    lblStatus.Text = "";
                    
                    txtstationcode.Text = Session["Station"].ToString();
                    //LoadAgentDropdown();
                    //LoadAgentName();
                    LoadGridCredit();
                    LoadGridTDSOnComm();
                    LoadGLDropDown();
                    //LoadBank();
                    GetCountry();
                    GetCity();
                    GetRatePreference();
                    GetPaymentMode();
                    StoreAllocation();
                    //ddlAgentType.Items.Add("Domestic");
                    //ddlAgentType.Items.Add("International");
                    GetAllStationsForCountry(ddlcountry.SelectedValue);
                    //((DropDownList)grdCreditinfo.Rows[0].FindControl("ddltransaction")).Items.Add( "Credit");
                    //((DropDownList)grdCreditinfo.Rows[0].FindControl("ddltransaction")).Items.Add ("Cash");
                    //ddlbuildto.Enabled = false;
                    if (ddlbuildto.SelectedValue == "SELF")
                    {
                        //txtlocatorcode.Enabled = false;
                    }
                    else
                    {
                        //txtlocatorcode.Enabled = true;
                     
                    }
                    if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                    {
                        getRateCardDetails();
                        validateExpiry();
                        GetInvoices();
                        LoadCurrentTDSOnCommInfo();
                        getRateCardDetails();

                        
                    }
                    else
                    {
                        if (Session["CurrentTableDeals"] != null)
                        {
                            DataTable dt = (DataTable)Session["CurrentTableDeals"];
                            dt.Rows.Clear();
                            Session["CurrentTableDeals"] = dt;
                        }
                    }
                    
                    LoadGRDRates();
                    LoadDealsGrid();
                }


                if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                {
                    //getRateCardDetails();
                }
                if (Request.QueryString["cmd"] == "Edit")
                {
                    BtnAllSave.Text = "Update";
                    Session["mode"] = "Edit";

                }
                else if (Request.QueryString["cmd"] == "View")
                {
                    btnAddTDS.Visible = false;
                    btnDeleteTDS.Visible = false;

                    //btnBack.Visible = true;
                    BtnAllSave.Visible = false;
                    //btnCancel.Visible = false;
                    Session["mode"] = "View";
                    disableView();
                    validateExpiry(); 
                }
                else
                {
                    btnAddTDS.Visible = true;
                    btnDeleteTDS.Visible = true;

                    //BtnAllSave.Text = "Save";
                    //btnBack.Visible = false;
                    BtnAllSave.Visible = true;
                    //btnCancel.Visible = true;
                    Session["mode"] = "";
                }


            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load
        #region Validate Expiry
        public void validateExpiry()
        {
            try
            {
                
                string currdate=System.DateTime.Now.ToString("yyyy-MM-dd");
                int exp = 1;
                
               for (int i = 0; i < dsCredit.Tables[1].Rows.Count; i++)
               {
                   string expiry = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text;

                   if ((Convert.ToDateTime(expiry)) <= Convert.ToDateTime(currdate))
                   {
                       exp = 0;
                       ((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Checked = true;
                       disable();
                       //break;

                       //lblStatus.Text = "Your Credit is Expired";
                       //((CheckBox)grdCreditinfo.Rows[0].FindControl("chkExpire")).Checked = true;

                       //disable();
                   }
                   else
                       exp = exp + 1;

                    
               }
               if (exp == 0)
               {
                   lblStatus.Text = "Your Credit is Expired";
                   
                 
               }
              
            }
            catch (Exception ex)
            { }
        }
        #endregion Validate Expiry
        #region Disable
        public void disable()
        {
            for (int k = 0; k < grdCreditinfo.Rows.Count; k++)
            {
                //If new credit, dont disable
                if (((Label)grdCreditinfo.Rows[k].FindControl("lblSrNo")).Text != "")
                {
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtbankname")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtbankgurantee")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtbankguranteeamt")).Enabled = false;
                    ((DropDownList)grdCreditinfo.Rows[k].FindControl("ddltransaction")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtsatrtdate")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtvalidto")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtAmount")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txttresholdlimit")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txttresholdlimitdays")).Enabled = false;
                    ((CheckBox)grdCreditinfo.Rows[k].FindControl("chkExpire")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtbankaddress")).Enabled = false;

                }
                if (((DropDownList)grdCreditinfo.Rows[k].FindControl("ddltransaction")).SelectedItem.Text == "Cash")
                {
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtbankname")).Enabled = false;
                    ((TextBox)grdCreditinfo.Rows[k].FindControl("txtbankgurantee")).Enabled = false;
                }

            }
        }
        #endregion
        #region Copying Bank Guarentee to Credit Amt 
        public void Showdata(object sender, EventArgs e)
        {
          string strCredit=((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text;
          //txtcreditAmt.Text = "";
          //txtcreditAmt.Text = strCredit;
          txtcreditremain.Text = "";
          //commented by Vijay - 23-08-2014
          //txtcreditremain.Text = strCredit;

        }
        #endregion 
        #region Agent Stock Declaration
        public void StoreAllocation()
        {
            try
            {
                dsStockAllocation = new DataSet();

                dsStockAllocation.Tables.Add();
                dsStockAllocation.Tables[0].Columns.Add("AgentName");
                dsStockAllocation.Tables[0].Columns.Add("LevelCode");
                dsStockAllocation.Tables[0].Columns.Add("SubLevelCode");
                dsStockAllocation.Tables[0].Columns.Add("FromAWB");
                dsStockAllocation.Tables[0].Columns.Add("ToAWB");

                Session["dsStockAllocation"] = dsStockAllocation;

                DataRow row = dsStockAllocation.Tables[0].NewRow();
                dsStockAllocation.Tables[0].Rows.Add(row);

                GRDAllocatedToAgent.DataSource = dsStockAllocation.Copy();
                GRDAllocatedToAgent.DataBind();
                // AgentName LevelCode SubLevelCode FromAWB ToAWB
            }
            catch (Exception ex)
            { }
        }
        #endregion 
        #region Fetching  Agent related Credit & Invoice Amount
        public void GetInvoices()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] paramname = new string[1];
                //paramname[0] = "ValidFrom";
                //paramname[1] = "validTo";
                paramname[0] = "AgentCode";

                object[] paramvalue = new object[1];
               // paramvalue[0] = ((TextBox)grdCreditinfo.Rows[0].FindControl("txtsatrtdate")).Text;
                //paramvalue[1] = ((TextBox)grdCreditinfo.Rows[0].FindControl("txtvalidto")).Text;
                paramvalue[0] = TXTAgentCode.Text;

                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.NVarChar;
               // paramtype[1] = SqlDbType.NVarChar;
                //paramtype[2] = SqlDbType.NVarChar;

                DataSet ds = db.SelectRecords("Sp_InvoiceAmount", paramname, paramvalue, paramtype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdCreditinfo.Rows[0].Cells[6].Text = ds.Tables[0].Rows[0][1].ToString();
                            txtinvoice.Text = ds.Tables[0].Rows[0][1].ToString();
                            //txtcreditAmt.Text = ds.Tables[0].Rows[0][2].ToString(); 
                            txtcreditremain.Text = ds.Tables[0].Rows[0][3].ToString(); 
                            //grdCreditinfo.DataSource = ds;
                            //grdCreditinfo.DataBind();

                        }
                        else
                        {
                            lblStatus.Text = "";
                        }
                    }

                }
                //LoadAgentName();
                //LoadBank();
            }
            catch (Exception ex)
            {
            }
        }


        #endregion 
        #region Validating If Stock is Available for City
        public void AWBAvilbleForAllocForCity()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] paramname = new string[1];
                paramname[0] = "CityCode";

                object[] paramvalue = new object[1];
                paramvalue[0] = ddlcity.SelectedItem.Text  ;

                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.NVarChar;

                DataSet ds = db.SelectRecords("Sp_AWBStockAlloc", paramname, paramvalue, paramtype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            grdAWBAllocationDetails.DataSource = ds;
                            grdAWBAllocationDetails.DataBind();
                            lblStatus.Text = "";
                        }
                        else
                        {
                            grdAWBAllocationDetails.DataSource = ds;
                            grdAWBAllocationDetails.DataBind();
                            //lblStatus.Text = "No Records Found For Stock.Please Allocate Series.";
                        }
                    }

                }

            }
            catch (Exception e)
            {
            }
        }
        #endregion 
        #region AWBAvailbleForAlloc
        //previously used for validating Stock is available.
        public void AWBAvilbleForAlloc()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] paramname = new string[1];
                paramname[0] = "CityCode";

                object[] paramvalue = new object[1];
                paramvalue[0] = Session["Station"].ToString();

                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.NVarChar;

                DataSet ds = db.SelectRecords("Sp_AWBStockAllocation", paramname, paramvalue, paramtype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            grdAWBAllocationDetails.DataSource = ds;
                            grdAWBAllocationDetails.DataBind();

                        }
                        else
                        {
                            lblStatus.Text = "No Records Found For Stock.Please Allocate Series.";
                        }
                    }

                }

            }
            catch (Exception e)
            {
            }
        }

        #endregion 
        #region Disabling Control View
        protected void disableView()
        {
            try
            {
                ddlcity.Enabled = false;
                ddlcountry.Enabled = false;
                txtmobile.Enabled = false;
                txtvalidfrom.Enabled = false;
                txtvalidto.Enabled = false;
                txtcustomercode.Enabled = false;
                txtcontactperson.Enabled = false;
                txtadress.Enabled = false;
                ddlcontrolinglocator.Enabled = false;
                txtcommision.Enabled = false;
                TXTAgentCode.Enabled = false;
                ddlAgentType.Enabled = false;
                ddlBillType.Enabled = false;
                ddlType.Enabled = false;
                txtlblDealDateFrom.Enabled = false;
                txtlblDealDateTo.Enabled = false;  
                txtemail.Enabled = false;
                //ImageButton4.Enabled = false;
                txtremark.Enabled = false;
                txtIATAcode.Enabled = false;
                //ImageButton8.Enabled = false;
                txtcreditAmt.Enabled = false;
                //ImageButton6.Enabled = false;
                txtcreditremain.Enabled = false;
                //ImageButton7.Enabled = false;
                txtinvoice.Enabled = false;
                //ImageButton10.Enabled = false;
                txtaccountmailid.Enabled = false;
                txtsalesmail.Enabled = false;   
                grdCreditinfo.Enabled = false;
                BtnAllSave.Enabled = false;
                //btntextclear.Enabled = false;
                btnClear.Enabled = false;
                //btnGeneralsave.Enabled = false;
                btngeneralclear.Enabled = false;
                btnAlloc.Enabled = false;
                //ddltransaction.Enabled = false;   
                grdDeals.Enabled = false;   
                TXTNewAgentName.Enabled = false;
                //DDLLevel.Enabled = false;
                TXTFromAWB.Enabled = false;
                TXTToAWB.Enabled = false;
                btnListInfo.Enabled = false;
                txtAccountCode.Enabled = false;
                txtTDSFreight.Enabled = false;
                txttdsoncomm.Enabled = false;
                ddlcontrolinglocator.Enabled = false;
                ddlbuildto.Enabled = false;
                btnCalculate.Enabled = false;
                txtlocatorcode.Enabled = false;   
                //btnaddroow.Enabled = false; 
                txtAgentReferenceCode.Enabled = false;
                txtPancardNumber.Enabled = false;
                txtServicetaxno.Enabled = false;
                //jayant
                ddlRatelinePreference.Enabled = false;
                ddlCurrencyCode.Enabled = false;
                txtThresholdAlert.Enabled = false;
                chkvalidbg.Enabled = false;
                chkIsFOC.Enabled = false;
                chkAutoGenInv.Enabled = false;

                chkListStation.Enabled = false;
                chkSelectAll.Enabled = false;

                txtInvoiceDueDays.Enabled = false;

            }
            catch (Exception e)
            { }
        }
        #endregion disable controls for view
        #region getRateCardDetails
        public void getRateCardDetails()
        {
            try
            {
                string creditNo = Request.QueryString["CreditNo"].ToString();
                dsCredit = new DataSet();
                dsCredit = objBLL.GetCreditDetails(creditNo);
                
                fillCreditDetails(dsCredit);
                
            }
            catch (Exception ex)
            { }
        }
        #endregion getRateCardDetails
        #region Fill CreditDetails Tab
        public void fillCreditDetails(DataSet dsCreditDetails)
        {
            try
            {
                //GetInvoices(); buildto
                //Code to fill details of credit from dataset
                #region General 
                TXTNewAgentName.Text = dsCreditDetails.Tables[0].Rows[0]["AgentName"].ToString();
                TXTAgentCode.Text = dsCreditDetails.Tables[0].Rows[0]["AgentCode"].ToString();
                //ddlcity.SelectedItem.Text = dsCreditDetails.Tables[0].Rows[0]["Station"].ToString();//Change vikas

                string city = dsCreditDetails.Tables[0].Rows[0]["Station"].ToString();
                ddlcity.SelectedIndex = ddlcity.Items.IndexOf(((ListItem)ddlcity.Items.FindByValue(city)));

                txtcustomercode.Text = dsCreditDetails.Tables[0].Rows[0]["CustomerCode"].ToString();
                txtIATAcode.Text = dsCreditDetails.Tables[0].Rows[0]["IATAAgentCode"].ToString();
                txtemail.Text = dsCreditDetails.Tables[0].Rows[0]["Email"].ToString();
                txtremark.Text = dsCreditDetails.Tables[0].Rows[0]["Remark"].ToString();
                txtmobile.Text = dsCreditDetails.Tables[0].Rows[0]["MobileNumber"].ToString();
                TXTSubLevel.Text = dsCreditDetails.Tables[0].Rows[0]["City"].ToString();//Change vikas
                //ddltransaction.SelectedItem.Text = dsCreditDetails.Tables[0].Rows[0]["TransactionType"].ToString();      
                txtcontactperson.Text = dsCreditDetails.Tables[0].Rows[0]["PersonContact"].ToString();
                txtadress.Text = dsCreditDetails.Tables[0].Rows[0]["Adress"].ToString();
                txtcommision.Text = dsCreditDetails.Tables[0].Rows[0]["NorrmalComm%"].ToString();
                try
                {
                    if (dsCreditDetails.Tables[0].Rows[0]["ControllingLocator"].ToString().ToUpper() == "N")
                        ddlcontrolinglocator.Text = "NO";//dsCreditDetails.Tables[0].Rows[0]["ControllingLocator"].ToString();
                    if(dsCreditDetails.Tables[0].Rows[0]["ControllingLocator"].ToString().ToUpper() == "Y" || dsCreditDetails.Tables[0].Rows[0]["ControllingLocator"].ToString().ToUpper() == "YES")
                        ddlcontrolinglocator.Text = "YES";//dsCreditDetails.Tables[0].Rows[0]["ControllingLocator"].ToString();
                }
                catch (Exception ex)
                { }
                ddlcontrolinglocator_SelectedIndexChanged(this, new EventArgs());
                //if (ddlcontrolinglocator.Text == "NO")
                //{
                //    //ddlbuildto.SelectedIndex = 1;
                //    txtlocatorcode.Enabled = true;
                //    //imgcheck.Visible = true;
                //}
                
                string countryname = dsCreditDetails.Tables[0].Rows[0]["Country"].ToString();
                ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(((ListItem)ddlcountry.Items.FindByValue(countryname)));
                GetAllStationsForCountry(ddlcountry.SelectedValue);

                //txtvalidfrom.Text = Convert.ToDateTime(dsCreditDetails.Tables[0].Rows[0]["ValidFrom"].ToString()).ToString("yyyy-MM-dd");
                txtvalidfrom.Text = Convert.ToDateTime(dsCreditDetails.Tables[0].Rows[0]["ValidFrom"].ToString()).ToString("dd/MM/yyyy");
                //txtvalidto.Text = Convert.ToDateTime(dsCreditDetails.Tables[0].Rows[0]["ValidTo"].ToString()).ToString("yyyy-MM-dd");
                txtvalidto.Text = Convert.ToDateTime(dsCreditDetails.Tables[0].Rows[0]["ValidTo"].ToString()).ToString("dd/MM/yyyy");

                txtlocatorcode.Text = dsCreditDetails.Tables[0].Rows[0]["ControllingLocatorCode"].ToString();
                if(txtlocatorcode.Text!="")
                txtlocatorcode_TextChanged(null, null);
                txtAccountCode.Text = dsCreditDetails.Tables[0].Rows[0]["AccountCode"].ToString();
                txtTDSFreight.Text = dsCreditDetails.Tables[0].Rows[0]["TDSOnFreight"].ToString();
                txttdsoncomm.Text = dsCreditDetails.Tables[0].Rows[0]["TDSOnCommision"].ToString();
                txtsalesmail.Text = dsCreditDetails.Tables[0].Rows[0]["SalesMail"].ToString();
                txtaccountmailid.Text = dsCreditDetails.Tables[0].Rows[0]["AccountMail"].ToString();
                
                if (dsCreditDetails.Tables[0].Rows[0]["ControllingLocator"].ToString() == "YES")
                    ddlcontrolinglocator.Text = "YES";
                else
                    ddlcontrolinglocator.Text = "NO";
                if (ddlcontrolinglocator.SelectedItem.Text == "YES")
                {
                    ddlbuildto.SelectedIndex = 1;
                    txtlocatorcode.Enabled = false;
                    txtlocatorcode.Text = "";
                    grdCreditinfo.Enabled = true;
                    grdDeals.Enabled = true;
                    txtCreditdays.Enabled = true;
                    
                }
                else
                {
                    grdCreditinfo.Enabled = false;
                    ddlbuildto.SelectedIndex = 2;
                    txtlocatorcode.Enabled = true;
                    grdDeals.Enabled = false;
                    txtCreditdays.Enabled = false;
                    
                }
                ddlAgentType.Text = dsCreditDetails.Tables[0].Rows[0]["AgentType"].ToString();

                //Commented by Vijay - 26-08-2014
                //ddlBillType.SelectedItem.Text = dsCreditDetails.Tables[0].Rows[0]["BillType"].ToString();
                ddlBillType.SelectedValue = dsCreditDetails.Tables[0].Rows[0]["BillType"].ToString();
                //Commented by Vijay - 26-08-2014
                //if (ddlBillType.SelectedItem.Text == "SELF")
                //{

                //    grdCreditinfo.Enabled = false;
                //    ddlbuildto.SelectedIndex = 2;
                //    txtlocatorcode.Enabled = true;
                //    grdDeals.Enabled = false;
                //    txtCreditdays.Enabled = false;
                //}
                txtPancardNumber.Text = dsCreditDetails.Tables[0].Rows[0]["PanCardNumber"].ToString();
                txtServicetaxno.Text = dsCreditDetails.Tables[0].Rows[0]["ServiceTaxNumber"].ToString();
                ddlbuildto.SelectedItem.Text=(dsCreditDetails.Tables[0].Rows[0]["buildto"].ToString());   
                string ValidBG=dsCreditDetails.Tables[0].Rows[0]["ValidBG"].ToString();
                if (ValidBG == "Yes")
                {
                    chkvalidbg.Checked = true;
                }
                else
                {
                    chkvalidbg.Checked = false;
                }
                string IACCode = dsCreditDetails.Tables[0].Rows[0]["IACCode"].ToString();

                if (IACCode.ToString() == "")
                {
                    ddlIACCode.SelectedIndex = 0;
                }
                else
                {
                    ddlIACCode.SelectedValue = IACCode;
                }
                string CCSFCode= dsCreditDetails.Tables[0].Rows[0]["CCSFCode"].ToString();

                if (CCSFCode.ToString() == "")
                {
                   ddlCCSFCode.SelectedIndex = 0;
                }
                else
                {
                    ddlCCSFCode.SelectedValue = CCSFCode;
                }

                ddlCurrencyCode.SelectedItem.Text = dsCreditDetails.Tables[0].Rows[0]["CurrencyCode"].ToString();
                txtThresholdAlert.Text = dsCreditDetails.Tables[0].Rows[0]["threshold"].ToString();
                try
                {
                    chkIsFOC.Checked = Convert.ToBoolean(dsCreditDetails.Tables[0].Rows[0]["IsFOC"]);
                }
                catch (Exception ex)
                {
                    chkIsFOC.Checked = false;
                }
                txtAgentReferenceCode.Text = dsCreditDetails.Tables[0].Rows[0]["AgentReferenceCode"].ToString();
                //jayant
                if (dsCreditDetails.Tables[0].Rows[0]["RatePreference"].ToString() != "")
                {
                    try
                    {
                        if (dsCreditDetails.Tables[0].Rows[0]["RatePreference"].ToString().ToUpper() == "AS AGREED")
                        {
                            ddlRatelinePreference.SelectedValue = "As Agreed";//dsCreditDetails.Tables[0].Rows[0]["RatePreference"].ToString();
                        }
                        else
                        {
                            ddlRatelinePreference.SelectedValue = dsCreditDetails.Tables[0].Rows[0]["RatePreference"].ToString().ToUpper();
                        }
                        //else
                        //    ddlRatelinePreference.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    { }
                }
                try
                {
                    chkAutoGenInv.Checked = Convert.ToBoolean(dsCreditDetails.Tables[0].Rows[0]["AutoGenerateAgentInvoice"]);
                }
                catch (Exception ex)
                {
                    chkAutoGenInv.Checked = false;
                }
                try
                {                    
                chkKnownShipper.Checked = Convert.ToBoolean(dsCreditDetails.Tables[0].Rows[0]["isKnownShipper"]);
                ChkVAT.Checked = Convert.ToBoolean(dsCreditDetails.Tables[0].Rows[0]["TAXExemption"]);
                }
                catch(Exception objEx)
                {
                }
                //Default paymode
                if (dsCreditDetails.Tables[0].Rows[0]["DefaultPayMode"].ToString() == "")
                    ddlDefaultPayMode.SelectedValue = "Select";
                else
                    ddlDefaultPayMode.SelectedValue = dsCreditDetails.Tables[0].Rows[0]["DefaultPayMode"].ToString();
                if (dsCreditDetails.Tables[0].Rows[0]["AllowedPayMode"].ToString() == "")
                    txtAllowPayMode.Text = string.Empty;
                else
                    txtAllowPayMode.Text = dsCreditDetails.Tables[0].Rows[0]["AllowedPayMode"].ToString();

                txtInvoiceDueDays.Text = dsCreditDetails.Tables[0].Rows[0]["InvoiceDueDays"].ToString();

                string[] stnList = dsCreditDetails.Tables[0].Rows[0]["AllowedStations"].ToString().Split(',');
                int count = 0;
                for (int i = 0; i < chkListStation.Items.Count; i++)
                {
                    if (stnList.Contains(chkListStation.Items[i].Value.ToString()))
                    {
                        chkListStation.Items[i].Selected = true;
                        count++;
                    }
                }
                if (count == chkListStation.Items.Count)
                    chkSelectAll.Checked = true;
                ///added value to DBA field
                txtDBA.Text = dsCreditDetails.Tables[0].Rows[0]["DBA"].ToString();
                //int count = 0;
                //for (int i = 0; i < chkListStation.Items.Count; i++)
                //{
                //    if (chkListStation.Items[i].Selected)
                //        count++;
                //}
                //if (count == chkListStation.Items.Count)
                //    chkSelectAll.Checked = true;
                       

                #endregion General
                #region credit
                if (dsCreditDetails.Tables[1].Rows.Count > 0)
                {
                    if (!dsCreditDetails.Tables[1].Columns.Contains("txtAmount"))
                        dsCreditDetails.Tables[1].Columns.Add("txtAmount");

                    grdCreditinfo.DataSource = dsCreditDetails.Tables[1].Copy();
                    grdCreditinfo.DataBind();

                    for (int i = 0; i < dsCreditDetails.Tables[1].Rows.Count; i++)
                    {
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Text = dsCreditDetails.Tables[1].Rows[i]["BankName"].ToString();
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text = dsCreditDetails.Tables[1].Rows[i]["BankGuranteeNumber"].ToString();
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text = dsCreditDetails.Tables[1].Rows[i]["BankGuranteeAmount"].ToString();
                        ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).Items.Clear();
                        ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).Items.Add(dsCreditDetails.Tables[1].Rows[i]["TransactionType"].ToString());
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Text = dsCreditDetails.Tables[1].Rows[i]["TresholdLimit"].ToString();
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Text = dsCreditDetails.Tables[1].Rows[i]["TresholdLimitDays"].ToString();
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankaddress")).Text = dsCreditDetails.Tables[1].Rows[i]["BankAddress"].ToString();

                        ((Label)grdCreditinfo.Rows[i].FindControl("lblSrNo")).Text = dsCreditDetails.Tables[1].Rows[i]["SerialNumber"].ToString();

                        //get the data from AgentMaster
                        txtinvoice.Text = dsCreditDetails.Tables[0].Rows[0]["AgentInvoiceBalance"].ToString();
                        txtcreditAmt.Text = dsCreditDetails.Tables[0].Rows[0]["AgentCreditAmount"].ToString();
                        txtcreditremain.Text = dsCreditDetails.Tables[0].Rows[0]["AgentCreditRemaining"].ToString();

                        txtCreditdays.Text = dsCreditDetails.Tables[1].Rows[i]["CreditDays"].ToString(); 
     
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text = Convert.ToDateTime(dsCreditDetails.Tables[1].Rows[i]["ValidFrom"].ToString()).ToString("dd/MM/yyyy");
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text = Convert.ToDateTime(dsCreditDetails.Tables[1].Rows[i]["ValidTo"].ToString()).ToString("dd/MM/yyyy");

                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Enabled = false;
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Enabled = false;
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Enabled = false;

                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Enabled = false;
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Enabled = false;
                        ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankaddress")).Enabled = false;

                        //If credit is expired
                        if (dsCreditDetails.Tables[1].Rows[i]["Expired"].ToString() == "Yes")
                        {
                            ((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Checked = true;
                            ((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Enabled = false;
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Enabled = false;
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Enabled = false;
                        }
                        else
                        {
                            ((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Checked = false;
                        }
                    }
                }
                #endregion Credit
                #region ListDeals
                try
                {

                    DataSet ds = objBLL.GetDeals(TXTAgentCode.Text);
                    grdDeals.DataSource = ds.Tables[1];
                    grdDeals.DataBind();
                    //ViewState["CurrentTableDeals"] = ds.Tables[1];//Appended 20Jul
                    ddlType.Text = ds.Tables[0].Rows[0][0].ToString();

                    txtlblDealDateFrom.Text = Convert.ToDateTime(ds.Tables[0].Rows[0][1].ToString()).ToString("dd/MM/yyyy");
                    txtlblDealDateTo.Text = Convert.ToDateTime(ds.Tables[0].Rows[0][2].ToString()).ToString("dd/MM/yyyy");

                    Session["CurrentTableDeals"] = ds.Tables[1];
                }
                catch (Exception ex)
                { }

                #endregion ListDeals
                #region Stock
                if (dsCreditDetails.Tables[4].Rows.Count > 0)
                {
                    GRDAllocatedToAgent.DataSource = dsCreditDetails.Tables[4].Copy();
                    GRDAllocatedToAgent.DataBind();
                }
                #endregion stock
                //LoadAgentStockData();
            }
            catch (Exception e)
            { }


        }
        #endregion fillRateCardDetails
        #region Clear Function
        public void clear()
        {
            txtcreditAmt.Text = "";
            txtcreditremain.Text = "";
            txtinvoice.Text = "";
            TXTNewAgentName.Text="";
            TXTNewAgentName.Text="";
            LoadGridCredit();
            //LoadCreditDetails();
        }
        #endregion clear Function
        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            try
            {
                //DataSet ds = objBLL.GetAgentList(Session["AgentCode"].ToString());
                //if (ds != null)
                //{
                //    ddlAgtCode.DataSource = ds;
                //    ddlAgtCode.DataMember = ds.Tables[0].TableName;
                //    ddlAgtCode.DataTextField = "AgentName";
                //    ddlAgtCode.DataValueField = "AgentCode";
                //    ddlAgtCode.DataBind();
                //    // ddlAgtCode.SelectedIndex = -1;
                //}
            }
            catch (Exception ex)
            { }
        }
        #endregion Load Grid Material Detail
        #region Load AgentName
        public void LoadAgentName()
        {
            try
            {
                DataSet ds = objBLL.GetAgentList(Session["AgentCode"].ToString());
                if (ds != null)
                {
                    //ddlAgtCode.DataSource = ds;
                    //ddlAgtCode.DataMember = ds.Tables[0].TableName;
                    //ddlAgtCode.DataTextField = "AgentName";
                    //ddlAgtCode.DataValueField = "AgentCode";
                    //ddlAgtCode.DataBind();
                    //ddlAgtCode.SelectedIndex = -1;
                }
            }
            catch (Exception e)
            { }
        }
        #endregion LoadAgentName
        #region LoadBank Dropdown
        //public void LoadBank()
        //{
        //    try
        //    {
        //        DataSet ds = objBLL.GetBankList("");
        //        DropDownList ddl = new DropDownList();

        //        for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
        //        {
        //            ddl = ((DropDownList)(grdCreditinfo.Rows[i].FindControl("ddlbankname")));
        //            if (ds != null)
        //            {
        //                ddl.DataSource = ds;
        //                ddl.DataMember = ds.Tables[0].TableName;
        //                ddl.DataTextField = "BankName";
        //                ddl.DataValueField = "BankName";
        //                ddl.DataBind();
        //            }
        //        }
        //    }
        //    catch (Exception e) { }
        //}

        #endregion LoadBank Dropdown
        #region GetCountry
        public void GetCountry()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                // SQLServer db = new SQLServer(ConnectionString);
                //ConnectionString
                DataSet ds = db.SelectRecords("Sp_GetCountry");
                
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlcountry.DataSource = ds;
                            ddlcountry.DataMember = ds.Tables[0].TableName;
                            ddlcountry.DataTextField = ds.Tables[0].Columns["CountryName"].ColumnName;
                            ddlcountry.DataValueField = ds.Tables[0].Columns["CountryCode"].ColumnName;
                            ddlcountry.DataBind();
                            ddlcountry.Items.Insert(0, new ListItem("Select", "0"));
                            ddlcountry.SelectedIndex = -1;

                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion GetCountry
        #region GetCity
        public void GetCity()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                // SQLServer db = new SQLServer(ConnectionString);
                //ConnectionString
                DataSet ds = db.SelectRecords("Sp_GetCityName");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //ddlcity.Items.Add(new ListItem("Select", ""));

                            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            //{
                            //    //ddlcity.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CityCode"].ToString(), ds.Tables[0].Rows[i]["CityName"].ToString()));
                            //    ddlcity.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CityName"].ToString(), ds.Tables[0].Rows[i]["CityCode"].ToString()));
                            //}

                            ddlcity.DataSource = ds;
                            ddlcity.DataMember = ds.Tables[0].TableName;
                            ddlcity.DataTextField = "CityName";
                            ddlcity.DataValueField = "CityCode";
                            ddlcity.DataBind();
                            ddlcity.Items.Insert(0, new ListItem("Select", "Select"));
                            ddlcity.SelectedIndex = ddlcity.Items.IndexOf(ddlcity.Items.FindByValue(Session["Station"].ToString()));
                            ddlcity_SelectedIndexChanged(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion GetCity
        #region LoadgridCreditInfo Intial Row
        public void LoadGridCredit()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RowNumber";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ddltransaction";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ddlbankname";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Bank Gurantee Number";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Bank Gurantee Amount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Valid From";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Valid To";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "txttresholdlimit";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "txttresholdlimitdays";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "txtAmount";
                myDataTable.Columns.Add(myDataColumn);



                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FinalAmt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Expired";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "txtbankaddress";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "lblSrNo";
                myDataTable.Columns.Add(myDataColumn);

                //25-5-201
                //myDataColumn = new DataColumn();
                //myDataColumn.DataType = Type.GetType("System.String");
                //myDataColumn.ColumnName = "InvoiceAmount";
                //myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["RowNumber"] = 1;
                dr["ddltransaction"] = "";
                dr["ddlbankname"] = "select";//"5";
                dr["Bank Gurantee Number"] = "";// "5";
                dr["Bank Gurantee Amount"] = "";// "9";
                dr["Valid From"] = "";
                dr["Valid To"] = "";
                dr["txttresholdlimit"] = "";
                dr["txttresholdlimitdays"] = "";
                dr["txtAmount"] = "";
                dr["FinalAmt"] = "";
                dr["Expired"] = "";
                dr["txtbankaddress"] = "";
                dr["lblSrNo"] = "";
                
                myDataTable.Rows.Add(dr);
                ViewState["CurrentTable"] = myDataTable;
                //Bind the DataTable to the Grid

                grdCreditinfo.DataSource = null;
                grdCreditinfo.DataSource = myDataTable;
                grdCreditinfo.DataBind();
                
                ((DropDownList)grdCreditinfo.Rows[0].FindControl("ddltransaction")).Items.Add("Credit");
                ((DropDownList)grdCreditinfo.Rows[0].FindControl("ddltransaction")).Items.Add("Cash");

                Session["dtCreditInfo"] = myDataTable.Copy();
            }
            catch (Exception e) { }
        }
        #endregion LoadgridCreditInfo Intial Row

        #region Load grid TDS on Commission Intial Row
        public void LoadGridTDSOnComm()
        {
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "SrNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FromDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ToDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "TDSOnCommPerc";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Status";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["SrNo"] = 0;
                dr["FromDate"] = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                dr["ToDate"] = "31/12/2099";
                dr["TDSOnCommPerc"] = "0";
                dr["Status"] = "1";

                myDataTable.Rows.Add(dr);
                ViewState["vsTDSInfo"] = myDataTable;

                grdTDSInfo.DataSource = null;
                grdTDSInfo.DataSource = myDataTable;
                grdTDSInfo.DataBind();

                Session["dtTDSInfo"] = myDataTable.Copy();
            }
            catch (Exception e) { }
        }
        #endregion Load grid TDS on Commission Intial Row

        #region Load grid TDS on Commission
        public void LoadCurrentTDSOnCommInfo()
        {
            try
            {
                string Agentcode = TXTAgentCode.Text.Trim();
                DataSet dsTDS = objBLL.getTDSOnCommissionInfo(Agentcode);

                if (dsTDS != null)
                {
                    if (dsTDS.Tables[0].Rows.Count > 0)
                    {
                        grdTDSInfo.DataSource = dsTDS.Tables[0];
                        grdTDSInfo.DataBind();

                        Session["dtTDSInfo"] = dsTDS.Tables[0];

                        //to disable TDS rows
                        for (int i = 0; i < grdTDSInfo.Rows.Count; i++)
                        {
                            if (((Label)grdTDSInfo.Rows[i].FindControl("lblStatus")).Text == "0")
                            {
                                grdTDSInfo.Rows[i].Enabled = false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load grid TDS on Commission


        #region TotalCreditCalculation
        public void TotalAmount(object sender, EventArgs e)
        {
            int incr = 0;
            int cnt = 0;
            try
            {
                //int temp = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);
                //if(temp!=)
                //{
                //} 


                for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
                {
                    int BankAmt = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);

                    int str1 = Convert.ToInt32(grdCreditinfo.Rows[i].Cells[5].Text.ToString());


                    if (BankAmt < str1)
                    {
                        lblStatus.Text = "Bank Gurantee Amount Cannot Be Less Than Amount";
                        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text = "";
                    }
                    else
                    {
                        int TotalAmt = str1;
                        incr = incr + TotalAmt;
                        int Amt = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);
                        cnt = cnt + Amt;
                    }
                }
                txtcreditAmt.Text = Convert.ToString(cnt);
                txtinvoice.Text = Convert.ToString(incr);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Please Insesrt Numeric Value in Amount Block ";
                ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text = "";
            }
            try
            {
                int credit = Convert.ToInt32(txtcreditAmt.Text.Trim());
                int invoice = Convert.ToInt32(txtinvoice.Text.Trim());
                int Total;
                if (credit < invoice)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Check()", true);
                    lblStatus.Text = "Credit Value Does Not Less Than Invoice Value";
                    txtinvoice.Text = "";
                    txtcreditremain.Text = "";
                    txtcreditremain.Text = "";
                }
                else
                {
                    Total = credit - invoice;
                    (txtcreditremain.Text) = Convert.ToString(Total);
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion TotalAmountCalculation
        #region TO add new row
       
        public void Addrow(object sender, EventArgs e)
        {
            if (grdCreditinfo.Rows.Count > 0 && ((CheckBox)grdCreditinfo.Rows[0].FindControl("chkExpire")).Checked)
            {
                SaveCreditInfo();
                AddNewRowToGrid();
               
            }
            else
            {
                lblCreditStatus.Text = "Not allowed until previous credit expires.";
                return;
            }
        }
        #region SaveCreditInfo
        public void SaveCreditInfo()
        {
            DataTable dtCreditInfo = (DataTable)Session["dtCreditInfo"];

            dtCreditInfo.Rows.Clear();

            for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
            {
                DataRow row = dtCreditInfo.NewRow();

                row["RowNumber"] = i;
                row["ddltransaction"] = ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).SelectedItem.Text;
                row["ddlbankname"] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Text;
                row["Bank Gurantee Number"] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text;
                row["Bank Gurantee Amount"] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text;
                row["Valid From"] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text;
                row["Valid To"] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text;
                row["FinalAmt"] = "";
                row["Expired"] = ((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Checked ? "Y" : "N";
                row["txttresholdlimit"] =((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Text;
                row["txttresholdlimitdays"] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Text;
                row["txtAmount"] =((TextBox)grdCreditinfo.Rows[i].FindControl("txtAmount")).Text;
                row["txtbankaddress"] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankaddress")).Text;

                row["lblSrNo"] = ((Label)grdCreditinfo.Rows[i].FindControl("lblSrNo")).Text;

                dtCreditInfo.Rows.Add(row);
            }

            Session["dtCreditInfo"] = dtCreditInfo.Copy();
        }
        #endregion SaveCreditInfo

        #endregion TO add new row
        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
            try
            {
                DataTable dtCreditInfo = (DataTable)Session["dtCreditInfo"];
                DataRow rw = dtCreditInfo.NewRow();

                dtCreditInfo.Rows.Add(rw);

                grdCreditinfo.DataSource = dtCreditInfo.Copy();
                grdCreditinfo.DataBind();


                for (int i = 0; i < dtCreditInfo.Rows.Count; i++)
                {

                    DataRow row = dtCreditInfo.Rows[i];

                    ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).Items.Add("Credit");
                    ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).Items.Add("Cash");

                    //((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).SelectedItem.Text = row["ddltransaction"].ToString();
                    if (row["ddltransaction"].ToString() == "Cash")
                        ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).SelectedIndex = 1;
                    else
                        ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).SelectedIndex = 0;

                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Text = row["ddlbankname"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankaddress")).Text = row["txtbankaddress"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text = row["Bank Gurantee Number"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text = row["Bank Gurantee Amount"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text = row["Valid From"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text = row["Valid To"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Text = row["txttresholdlimit"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Text = row["txttresholdlimitdays"].ToString();
                    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtAmount")).Text = row["txtAmount"].ToString();
                    
                    ((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Checked = (row["Expired"].ToString() == "Y" ? true : false);

                    ((Label)grdCreditinfo.Rows[i].FindControl("lblSrNo")).Text = row["lblSrNo"].ToString();
                    

                   
                }
                

            }
            catch (Exception e) 
            { 
            
            }

        }
        #endregion AddnewRow To Grid
        #region SetPreviousData
        private void SetPreviousData()
        {
            try
            {
                int rowIndex = 0;
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TextBox box1 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[0].FindControl("txtbankname");
                            TextBox box2 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[1].FindControl("txtbankgurantee");
                            TextBox box3 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[2].FindControl("txtbankguranteeamt");
                            TextBox box4 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[3].FindControl("txtsatrtdate");
                            TextBox box5 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[4].FindControl("txtvalidto");
                            TextBox box6 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[4].FindControl("txtAmount");
                            TextBox box7 = (TextBox)grdCreditinfo.Rows[rowIndex].Cells[5].FindControl("txtbankaddress");
                            box1.Text = dt.Rows[i]["Bank Name"].ToString(); 
                            box2.Text = dt.Rows[i]["Bank Gurantee Number"].ToString();
                            box3.Text = dt.Rows[i]["Bank Gurantee Amount"].ToString();
                            box4.Text = dt.Rows[i]["Valid From"].ToString();
                            box5.Text = dt.Rows[i]["Valid To"].ToString();
                            box6.Text = dt.Rows[i]["txtAmount"].ToString();  //25-5-2012
                            box7.Text = dt.Rows[i]["txtbankaddress"].ToString();
                            //Fill the DropDownList with Data

                            rowIndex++;

                        }
                    }
                }

            }
            catch (Exception e)
            { }
        }
        #endregion SetPreviousData

        #region Not Used
        #region Dummy Data
        //private ArrayList GetDummyData()
        //{
        //    ArrayList arr = new ArrayList();
        //    arr.Add(new ListItem("AXIX", "1"));
        //    arr.Add(new ListItem("HDFC", "2"));
        //    arr.Add(new ListItem("Bank Of Baroda", "3"));


        //    return arr;
        //}
        #endregion Dummy Data
        #region sortdirection
        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        }
        #endregion sortdirection
        #region Sorting
        //protected void grdCreditdetails_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    DataTable dataTable = grdCreditdetails.DataSource as DataTable;

        //    if (dataTable != null)
        //    {
        //        DataView dataView = new DataView(dataTable);
        //        dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

        //        grdCreditdetails.DataSource = dataView;
        //        grdCreditdetails.DataBind();
        //    }
        //}
        #endregion Sorting
        #endregion

        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtcreditAmt.Text == "" && txtinvoice.Text == "" && txtcreditremain.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "EnterData()", true);
                lblStatus.Text = "Please Enter Data"; 
            }
            if (IsPostBack)
            {

                try
                {
                    string[] paramname = new string[9];
                    paramname[0] = "AgentCode";
                    paramname[1] = "BankName";
                    paramname[2] = "BankGuranteeNumber";
                    paramname[3] = "BankGuranteeAmount";
                    paramname[4] = "ValidFrom";
                    paramname[5] = "ValidTo";
                    paramname[6] = "CreditAmount";
                    paramname[7] = "InvoiceBalance";
                    paramname[8] = "CreditRemaining";
                    paramname[9] = "BankAddress";


                    object[] paramvalue = new object[9];
                    paramvalue[0] = TXTNewAgentName.Text;
                    for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
                    {
                        paramvalue[1] = ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddlbankname")).Text;
                        paramvalue[2] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text;
                        paramvalue[3] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text;
                        paramvalue[4] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text;
                        paramvalue[5] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text;

                        paramvalue[6] = txtcreditAmt.Text.Trim();
                        paramvalue[7] = txtinvoice.Text.Trim();
                        paramvalue[8] = txtcreditremain.Text.Trim();
                        paramvalue[9] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankaddress")).Text;


                        SqlDbType[] paramtype = new SqlDbType[9];
                        paramtype[0] = SqlDbType.NVarChar;
                        paramtype[1] = SqlDbType.NVarChar;
                        paramtype[2] = SqlDbType.NVarChar;
                        paramtype[3] = SqlDbType.NVarChar;
                        paramtype[4] = SqlDbType.NVarChar;
                        paramtype[5] = SqlDbType.NVarChar;
                        paramtype[6] = SqlDbType.Float;
                        paramtype[7] = SqlDbType.Float;
                        paramtype[8] = SqlDbType.Float;
                        paramtype[9] = SqlDbType.VarChar;

                        bool ds = da.InsertData("Sp_InsertCreditData", paramname, paramtype, paramvalue);

                    }


                }
                catch (Exception ex)
                {
                    // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "InsertFailure()", true);
                    lblStatus.Text = "Insert Failed";
                }
                //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Insert()", true);
                lblStatus.Text = "Agent Added Succssfully ..";
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "SelectRow()", true);
                lblStatus.Text = "Please Select Row";
            }
            clear();
            //LoadBank();
        }
        #endregion Save
        #region Clear
        protected void btntextclear_Click(object sender, EventArgs e)
        {
            TXTNewAgentName.Text = "";
            //LoadBank();
            //LoadCreditDetails();
        }
        #endregion Clear
        #region Check Credit and Invoice
        protected void txtinvoice_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    int credit = Convert.ToInt32(txtcredit.Text.Trim());
            //    int invoice = Convert.ToInt32(txtinvoice.Text.Trim());
            //    int Total;
            //    if (credit < invoice)
            //    {
            //        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Check()", true);
            //        lblStatus .Text= "Invoice Value Does Not Greater Than Credit Value";
            //        txtinvoice.Text= "";
            //        txtcreditremain.Text = "";
            //    }
            //    else
            //    {
            //        Total = credit - invoice;
            //        (txtcreditremain.Text) = Convert.ToString(Total);
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }
        #endregion Check Credit and Invoice
        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            TXTNewAgentName.Text = "";
            LoadGridCredit();
        }
        #endregion Clear

        #region Not used
        #region Indexing
        //protected void grdCreditdetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    LoadCreditDetails();
        //    grdCreditdetails.PageIndex = e.NewPageIndex;
        //    grdCreditdetails.DataBind();
        //}
        #endregion Indexing
        #region Servervalidation
        protected void cusCustom_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                if (e.Value.Length == 10)
                    e.IsValid = true;
                else
                    e.IsValid = false;
            }
            catch (Exception ex)
            { }
        }
        #endregion serverValidation
        #endregion

        #region GeneralSave
        protected void btnGeneralsave_Click(object sender, EventArgs e)
        {
            if (txtcontactperson.Text == "" && ddlcountry.Text == "" && txtIATAcode.Text == "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "EnterData()", true);
                lblStatus.Text = "Please Enter Data";
            }
            if (IsPostBack)
            {

                try
                {
                    string[] paramname = new string[15];
                    paramname[0] = "AgentCode";
                    paramname[1] = "IATAAgentCode";
                    paramname[2] = "AgentName";
                    paramname[3] = "ValidFrom";
                    paramname[4] = "ValidTo";
                    paramname[5] = "CustomerCode";
                    paramname[6] = "Station";
                    paramname[7] = "Country";
                    paramname[8] = "City";
                    paramname[9] = "Adress";
                    paramname[10] = "Email";
                    paramname[11] = "PersonContact";
                    paramname[12] = "MobileNumber";
                    paramname[13] = "Remark";
                    paramname[14] = "strFlag";
                    


                    object[] paramvalue = new object[15];
                    if (HidFlag.Value == "I")
                    {
                        paramvalue[0] = TXTNewAgentName.Text;
                    }
                    else
                    {
                        paramvalue[0] = TXTNewAgentName.Text;
                    }
                    paramvalue[1] = txtIATAcode.Text.Trim();
                    if (HidFlag.Value == "I")
                    {
                        paramvalue[2] = TXTNewAgentName.Text;
                    }
                    else
                    {
                        paramvalue[2] = TXTNewAgentName.Text;
                    }
                    paramvalue[3] = txtvalidfrom.Text.Trim();
                    paramvalue[4] = txtvalidto.Text.Trim();
                    paramvalue[5] = txtcustomercode.Text.Trim();
                    paramvalue[6] = txtstationcode.Text.Trim();
                    paramvalue[7] = ddlcountry.SelectedValue;
                    paramvalue[8] = ddlcity.SelectedValue;
                    paramvalue[9] = txtadress.Text.Trim();
                    paramvalue[10] = txtemail.Text.Trim();
                    paramvalue[11] = txtcontactperson.Text.Trim();
                    paramvalue[12] = txtmobile.Text.Trim();
                    paramvalue[13] = txtremark.Text.Trim();
                    paramvalue[14] = HidFlag.Value;
                    



                    SqlDbType[] paramtype = new SqlDbType[15];
                    paramtype[0] = SqlDbType.NVarChar;
                    paramtype[1] = SqlDbType.NVarChar;
                    paramtype[2] = SqlDbType.NVarChar;
                    paramtype[3] = SqlDbType.DateTime;
                    paramtype[4] = SqlDbType.DateTime;
                    paramtype[5] = SqlDbType.NVarChar;
                    paramtype[6] = SqlDbType.NVarChar;
                    paramtype[7] = SqlDbType.NVarChar;
                    paramtype[8] = SqlDbType.NVarChar;
                    paramtype[9] = SqlDbType.NVarChar;
                    paramtype[10] = SqlDbType.NVarChar;
                    paramtype[11] = SqlDbType.NVarChar;
                    paramtype[12] = SqlDbType.VarChar;
                    paramtype[13] = SqlDbType.NVarChar;
                    paramtype[14] = SqlDbType.NVarChar;


                    if (!ValidateOnSave())
                        return;

                    bool ds = da.InsertData("Sp_InsertAgentDetails", paramname, paramtype, paramvalue);
                    if (ds == false)
                    {
                        //        ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "InsertFailure()", true);
                        lblStatus.Text = "Insertion Failed";
                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Insert()", true);
                        lblStatus.Text = "Record Inserted Succssfully";
                    }
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "InsertFailure()", true);
                    lblStatus.Text = "Insertion Failed";
                }

            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "SelectRow()", true);
                lblStatus.Text = "Please Select Row";
            }
            LoadAgentDropdown();
            LoadAgentName();
            LoadGridCredit();
            //LoadBank();
            //LoadCreditDetails();
            GetCity();
            LoadAgentStockData();
            // GetAgentName();
            // getLocationdata();

            //GetAgentName();
            TXTNewAgentName.Visible = true;
            ClearGeneral();
            TXTNewAgentName.Enabled = false;
            TXTNewAgentName.Visible = false;
            TabContainer1.Visible = true;
        }
        #endregion GeneralSave
        #region ClearGeneral
        public void ClearGeneral()
        {
            try
            {
                //Clear General Tab
                lblStatus.Text = string.Empty;

                TXTAgentCode.Text = string.Empty; 
                TXTNewAgentName.Text = string.Empty;

                txtcustomercode.Text = string.Empty;
                txtcontactperson.Text = string.Empty;
                txtadress.Text = string.Empty;

                ddlAgentType.SelectedIndex = 0;
                ddlcity.SelectedIndex = ddlcity.Items.IndexOf(ddlcity.Items.FindByValue(Session["Station"].ToString()));
                ddlcity_SelectedIndexChanged(null,null);

                txtmobile.Text = string.Empty;
                txtcommision.Text = string.Empty;
                txtTDSFreight.Text = string.Empty;

                txttdsoncomm.Text = string.Empty;
                ddlcontrolinglocator.SelectedIndex = 1;
                ddlcontrolinglocator_SelectedIndexChanged(null, null);

                txtAccountCode.SelectedIndex = 0;
                txtemail.Text = string.Empty;

                txtIATAcode.Text = string.Empty;
                txtaccountmailid.Text = string.Empty;
               
                txtsalesmail.Text = string.Empty;
                //txtvalidfrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtvalidfrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtvalidto.Text = string.Empty;

                txtremark.Text = string.Empty;
                txtPancardNumber.Text = string.Empty;
                txtServicetaxno.Text = string.Empty;

                ddlCurrencyCode.SelectedIndex = 0;
                chkIsFOC.Checked = chkvalidbg.Checked = ChkVAT.Checked=false;
                txtThresholdAlert.Text = string.Empty;

                txtAgentReferenceCode.Text = string.Empty;
                ddlRatelinePreference.SelectedIndex = 0;
                ddlDefaultPayMode.SelectedIndex = 0;
                chkAutoGenInv.Checked = false;

                chkSelectAll.Checked = false;
                chkListStation.Items.Clear();

                imgcross.Visible = false; 
                imgcheck.Visible = false;

                //Clear Credit Tab
                LoadGridCredit();
                lblCreditStatus.Text = string.Empty;
                txtinvoice.Text = txtcreditAmt.Text = txtcreditremain.Text = string.Empty;
                txtCreditdays.Text = "0";

                txtInvoiceDueDays.Text = "0";

                //Load TDS Grid
                LoadGridTDSOnComm();

                BtnAllSave.Text = "Save";
            }
            catch (Exception e)
            { }
        }
        #endregion ClearGeneral
        #region Clear Button for General
        protected void btngeneralclear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGeneral();
                //Response.Redirect("AgentMaster.aspx");
                //Session["CurrentTableDeals"] = null;
                //lblStatus.Text = "";
                //ClearGeneral();
                //LoadGridCredit();
                //StoreAllocation();
                //ddlAgentType.Items.Clear();
                //ddlAgentType.Items.Add("Select");
                //ddlAgentType.Items.Add("Domestic");
                //ddlAgentType.Items.Add("International");
                //LoadGRDRates();
                //LoadDealsGrid();
                //ddlBillType.SelectedIndex = 0;
                //ddlbuildto.SelectedIndex = 0;
                //txtPancardNumber.Text = "";
                //txtServicetaxno.Text = "";
                //txtcreditAmt.Text = "";
                //txtCreditdays.Text = "0";
                //txtcreditremain.Text = "";
                //txtinvoice.Text = "";
                //txtlblDealDateFrom.Text = "";
                //txtlblDealDateTo.Text = "";
                //TXTSubLevel.Text = "";
                //TabContainer1.ActiveTabIndex = 0;
            }
            catch (Exception ex)
            { }
        }
        #endregion Clear Button for General
        #region Allocate
        public void AllocateStockToAgent()
        {
            // Save Agent Allocation
            //try
            //{
            //    if (TXTFromAWB.Text == "" && TXTToAWB.Text == "")
            //    {
            //        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "InsertValue();", true);
            //        lblStatus.Text = "Please insert Values..";
            //    }

            //    string Agentname = "", Level = "", SubLevel = "", FrmAWB = "", ToAWB = "", UpdatedOn = "";

            //    Agentname = DDLAgent.SelectedItem.Text;
            //    SubLevel = txtsublevel.Text;
            //    FrmAWB = TXTFromAWB.Text.PadLeft(8, '0');
            //    ToAWB = TXTToAWB.Text.PadLeft(8, '0');
            //    UpdatedOn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            //    if (Convert.ToDouble(FrmAWB) > Convert.ToDouble(ToAWB))
            //    {
            //        // getLocationdata();
            //        // GetAgentName();
            //        return;
            //    }
            //    if (FrmAWB != "" && ToAWB != "" && DDLAgent.SelectedIndex >= 0 && txtsublevel.Text != "")
            //    {
            //        // insertORupdateDevice(deviceName, from, to);

            //        bool res = BLObj.SaveAgentAllocatedData(Agentname, Level, SubLevel, FrmAWB, ToAWB, UpdatedOn);
                    
            //        TXTFromAWB.Text = "";
            //        TXTToAWB.Text = "";
            //        LoadAgentStockData();
            //    }
            //    else
            //    {
            //        // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "EmptyMsg();", true);
            //        lblStatus.Text = "Please Fill Data";
            //    }

            //}
            //catch (Exception ex)
            //{

            //}

        }
        public void btnAlloc_Click(object sender, EventArgs e)
        {
            try
            {
                // AgentName LevelCode SubLevelCode FromAWB ToAWB
#region If condiion
                if (TXTFromAWB.Text.Trim() == "" || TXTToAWB.Text.Trim() == "")
                {
                    LBLTABAllocStatus.Text = "Fill allocation details.";
                    return;
                }


                if (TXTAgentCode.Text.Trim() == "" || TXTNewAgentName.Text.Trim() == "")
                {
                    LBLTABAllocStatus.Text = "Fill agent name and agent code details.";
                    return;
                }
#endregion if condition
#region datarow
                DataRow row;

                dsStockAllocation = (DataSet)Session["dsStockAllocation"];

                if (dsStockAllocation != null && dsStockAllocation.Tables.Count != 0 && dsStockAllocation.Tables[0].Rows.Count == 1 &&
                    dsStockAllocation.Tables[0].Rows[0][0].ToString() == ""
                   )
                    dsStockAllocation.Tables[0].Rows.Clear();


                row = dsStockAllocation.Tables[0].NewRow();
                row["AgentName"] = TXTAgentCode.Text.Trim();
                row["SubLevelCode"] = TXTSubLevel.Text;
                row["FromAWB"] = TXTFromAWB.Text;
                row["ToAWB"] = TXTToAWB.Text;

                
#endregion datarow

                //GRDAllocatedToAgent.DataSource = dsStockAllocation.Tables[0];
                //GRDAllocatedToAgent.DataBind();

               // Session["dsStockAllocation"] = dsStockAllocation.Copy();
                
                #region Save Allocation
                //dsStockAllocation = (DataSet)Session["dsStockAllocation"];

                //if (dsStockAllocation != null && dsStockAllocation.Tables.Count != 0 && dsStockAllocation.Tables[0].Rows.Count != 0)
               // {
                    // AgentName LevelCode SubLevelCode FromAWB ToAWB

                   // for (int i = 0; i < dsStockAllocation.Tables[0].Rows.Count; i++)
                   // {
                       // DataRow row1 = dsStockAllocation.Tables[0].Rows[i];

                    DataSet ds = BLObj.SPValidateAgentAWBAlloctionData(TXTAgentCode.Text, TXTSubLevel.Text, TXTFromAWB.Text, TXTToAWB.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows[0][0].ToString() == "valid")
                                    {

                                        dsStockAllocation.Tables[0].Rows.Add(row);

                                        GRDAllocatedToAgent.DataSource = dsStockAllocation.Tables[0];
                                        GRDAllocatedToAgent.DataBind();
                                        grdAWBAllocationDetails.Visible = true ;  
                                        lblStatus.Text = "Stock Allocated Successfully.";
                                        lblStatus.ForeColor = Color.Green;
                                    }
                                    else if (ds.Tables[0].Rows[0][0].ToString() == "already allocated" )
                                    {
                                        lblStatus.Text = "Stock Already Allocated...";
                                        lblStatus.ForeColor = Color.Red;

                                        //grdAWBAllocationDetails.Visible = false;  
                                    }
                                    else if (ds.Tables[0].Rows[0][0].ToString() == "Already BlackListed")
                                    {
                                        lblStatus.Text = "Stock is BlackListed";
                                        lblStatus.ForeColor = Color.Red;    
                                    }
                                    else if (ds.Tables[0].Rows[0][0].ToString() == "invalid range")
                                    {
                                        lblStatus.Text = "Please select valid range of Stock for Allocation";
                                        lblStatus.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        lblStatus.Text = "Error.";
                                        lblStatus.ForeColor = Color.Red;
                                        grdAWBAllocationDetails.Visible = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            lblStatus.Text = "Error.";
                            lblStatus.ForeColor = Color.Red;
                        }
                        //if (!BLObj.SaveAgentAllocatedData(row["AgentName"].ToString(), "Airport", row["SubLevelCode"].ToString(), row["FromAWB"].ToString(), row["ToAWB"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                        //{
                        //    lblStatus.Text = "Error in stock allocation save.";
                        //    lblStatus.ForeColor = Color.Red;
                        //}
                    //}
                    //if confiion
               // }






                #endregion Save Allocation
                //LBLTABAllocStatus.Text = "Stock Allocated Successfully.";
                //LBLTABAllocStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                LBLTABAllocStatus.Text = "Stock Allocated failed.";
                LBLTABAllocStatus.ForeColor = Color.Red;               
            }
        }
        #endregion Allocate
        #region Get Agent Name
        public void GetAgentName()
        {
            try
            {
                //DataSet DsAgent = new DataSet();
                //SQLServer db = new SQLServer(Global.GetConnectionString());

                //DsAgent = BLObj.GetAgentName();
                //if (DsAgent != null && DsAgent.Tables[0].Rows.Count > 0)
                //{
                //    DDLAgent.DataSource = DsAgent;
                //    DDLAgent.DataTextField = "AgentName";
                //    DDLAgent.DataValueField = "AgentName"; ;
                //    // DDLAgent.DataMember = "AgentName";
                //    DDLAgent.DataBind();
                //}
            }
            catch (Exception ex)
            { }

        }
        #endregion Get Agent Name
        #region Load Data Grid/Get Agent Name Data
        protected void LoadAgentStockData()
        {
            // GVUBI.DataSource = null;
            // GVUBI.DataBind();
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                // SQLServer db = new SQLServer(ConnectionString);
                //ConnectionString
                string[] paramname = new string[1];
                paramname[0] = "Agentname";

                object[] paramvalue = new object[1];
                paramvalue[0] = TXTAgentCode.Text;
                

                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.NVarChar;

                DataSet ds = db.SelectRecords("SpGetAgentAWBStockData", paramname, paramvalue, paramtype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            
                            

                            GRDAllocatedToAgent.DataSource = ds;
                            GRDAllocatedToAgent.DataBind();

                            Session["dsStockAllocation"] = ds.Copy();

                        }
                        else
                        {
                            Session["dsStockAllocation"] = ds.Copy();

                            DataRow row = ds.Tables[0].NewRow();
                            row["FromAWB"] = 0;
                            row["ToAWB"] = 0;

                            ds.Tables[0].Rows.Add(row);

                            GRDAllocatedToAgent.DataSource = ds;
                            GRDAllocatedToAgent.DataBind();



                            LBLTABAllocStatus.Text = "No Records Found For Stock.Please Allocate Series.";
                        }
                    }

                }

            }
            catch (Exception ex)
            { }

        }
        #endregion Load Data Grid/Get Agent Name Data
        #region GridView1 Event Handlers
        //This event occurs for each row
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            string strSort = string.Empty;
            try
            {
                // Make sure we aren't in header/footer rows
                if (row.DataItem == null)
                {
                    return;
                }

                DataTable dt1 = new DataTable();

                //Find Child GridView control

                GridView gv = new GridView();
                gv = (GridView)row.FindControl("GridView2");

                //Check if any additional conditions (Paging, Sorting, Editing, etc) to be applied on child GridView
                if (gv.UniqueID == gvUniqueID)
                {
                    gv.PageIndex = gvNewPageIndex;
                    gv.EditIndex = gvEditIndex;
                    //Check if Sorting used
                    //if (gvSortExpr != string.Empty)
                    //{
                    //    GetSortDirection();
                    //    strSort = " ORDER BY " + string.Format("{0} {1}", gvSortExpr, gvSortDir);
                    //}
                    //Expand the Child grid
                    ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["Agentname"].ToString() + "','one');</script>");
                }

                //Prepare the query for Child GridView by passing the Customer ID of the parent row
                strSort = ((DataRowView)e.Row.DataItem)["Agentname"].ToString();
                sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
                sc.Open();
                scmd = new SqlCommand("SPGetAWbStockbyAGENTNAME", sc); //receipt come from xpress bag details
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@Agentname", strSort);
                sdr = scmd.ExecuteReader();
                dt1.Load(sdr);

                gv.DataSource = dt1;
                gv.DataBind();

                //for (int i = 0; i < dt1.Rows.Count; i++)
                //{
                //    if (dt1.Rows[i][4].ToString() == "True")
                //    {
                //        gv.Rows[i].Cells[6].Enabled = false;
                //    }
                //    else
                //    {
                //        gv.Rows[i].Cells[6].Enabled = true;
                //    }
                //}

                sc.Close();


                //Add delete confirmation message for Customer
                //LinkButton l = (LinkButton)e.Row.FindControl("linkDeleteCust");
                //l.Attributes.Add("onclick", "javascript:return " +
                //"confirm('Are you sure you want to delete this Customer " +
                //DataBinder.Eval(e.Row.DataItem, "CustomerID") + "')");
            }
            catch (Exception ex)
            { }
        }
        #endregion
        #region GridView2 Event Handlers
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvNewPageIndex = e.NewPageIndex;
            LoadAgentStockData();
        }
        #endregion GridView2 Event Handlers
        #region GridView Data Bound
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Check if this is our Blank Row being databound, if so make the row invisible
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((DataRowView)e.Row.DataItem)["Agentname"].ToString() == String.Empty) e.Row.Visible = false;
                }
            }
            catch (Exception ex1) { }
        }
        #endregion GridView Data Bound
        #region Grid RowEditing
        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GridView gvTemp = (GridView)sender;
                gvUniqueID = gvTemp.UniqueID;
                gvEditIndex = e.NewEditIndex;

                LoadAgentStockData();
            }
            catch (Exception ex) { }
        }
        #endregion Grid RowEditing
        #region CalcelEdit
        protected void GridView2_CancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GridView gvTemp = (GridView)sender;
                gvUniqueID = gvTemp.UniqueID;
                gvEditIndex = -1;
                LoadAgentStockData();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion CancelEdit
        #region GridView RowUpdate
        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridView gvTemp = (GridView)sender;
                gvUniqueID = gvTemp.UniqueID;

                //Get the values stored in the text boxes
                string strOrderID = ((Label)gvTemp.Rows[e.RowIndex].FindControl("lblOrderID")).Text;
                string strFreight = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("txtFreight")).Text;
                string strShipperName = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("txtShipperName")).Text;
                string strShipAdress = ((TextBox)gvTemp.Rows[e.RowIndex].FindControl("txtShipAdress")).Text;

                //Prepare the Update Command of the DataSource control
                AccessDataSource dsTemp = new AccessDataSource();
                dsTemp.DataFile = "App_Data/Northwind.mdb";
                string strSQL = "";
                strSQL = "UPDATE Orders set Freight = " + float.Parse(strFreight) + "" +
                         ",ShipName = '" + strShipperName + "'" +
                         ",ShipAddress = '" + strShipAdress + "'" +
                         " WHERE OrderID = " + strOrderID;
                dsTemp.UpdateCommand = strSQL;
                dsTemp.Update();
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Order updated successfully');</script>");

                //Reset Edit Index
                gvEditIndex = -1;

                LoadAgentStockData();
            }
            catch { }
        }
        #endregion GridView RowUpdate
        # region LoadDealsGrid
        public void LoadDealsGrid()//Create Equipment on Count
        {
            try
            {
                if (Session["CurrentTableDeals"] != null)
                {
                    DataTable myDataTableDeals = (DataTable)Session["CurrentTableDeals"];
                    if (Request.QueryString["cmd"] != "Edit" || Request.QueryString["cmd"] != "View")
                    {
                        if (myDataTableDeals.Rows.Count > 0)
                        { // myDataTableDeals.Rows.Clear();
                            // {

                            for (int i = 0; i < myDataTableDeals.Rows.Count; i++)
                            {

                                if (((TextBox)grdDeals.Rows[i].FindControl("txtDlFrom")).Text == "" || ((TextBox)grdDeals.Rows[i].FindControl("txtDlTo")).Text == "")
                                {
                                    lblStatus.Text = "Please Enter Deal values";
                                    lblStatus.ForeColor = Color.Red;
                                    return;

                                }
                                else
                                {
                                    lblStatus.Text = "";  
                                }

                                //dr["AFrom"] = "";
                                //dr["ATo"] = "";
                                //dr["Percent"] = "";
                                //dr["Value"] = "";
                                //  myDataTableDeals.Rows.Add(dr);
                                myDataTableDeals.Rows[i][0] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlFrom")).Text.ToString();
                                myDataTableDeals.Rows[i][1] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlTo")).Text.ToString();
                                myDataTableDeals.Rows[i][2] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlPercent")).Text.ToString();
                                myDataTableDeals.Rows[i][3] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlValue")).Text.ToString();
                                //  

                            }
                            DataRow dr = myDataTableDeals.NewRow();
                            myDataTableDeals.Rows.Add(dr);
                            grdDeals.DataSource = myDataTableDeals.Copy();
                            grdDeals.DataBind();
                            Session["CurrentTableDeals"] = myDataTableDeals;
                        }
                        else
                        {
                            DataRow dr = myDataTableDeals.NewRow();

                            //dr["AFrom"] = "";
                            //dr["ATo"] = "";
                            //dr["Percent"] = "";
                            //dr["Value"] = "";
                            myDataTableDeals.Rows.Add(dr);

                            grdDeals.DataSource = myDataTableDeals.Copy();
                            grdDeals.DataBind();
                        }
                        // Session["CurrentTableDeals"] = myDataTableDeals.Copy();
                    }
                    else
                    {

                    }
                }
                else
                {
                    DataTable myDataTableDeals = (DataTable)Session["CurrentTableDeals"];
                    if (Request.QueryString["cmd"] != "Edit" || Request.QueryString["cmd"] != "View")
                    {
                        // myDataTableDeals.Rows.Clear();
                        // {


                        myDataTableDeals = new DataTable();
                        DataColumn myDataColumn = new DataColumn();
                        DataRow dr = myDataTableDeals.NewRow();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "AFrom";
                        myDataTableDeals.Columns.Add(myDataColumn);

                        myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "ATo";
                        myDataTableDeals.Columns.Add(myDataColumn);

                        myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "Percent";
                        myDataTableDeals.Columns.Add(myDataColumn);

                        myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = "Value";
                        myDataTableDeals.Columns.Add(myDataColumn);

                        dr["AFrom"] = "";
                        dr["ATo"] = "";
                        dr["Percent"] = "";
                        dr["Value"] = "";
                        myDataTableDeals.Rows.Add(dr);

                        grdDeals.DataSource = myDataTableDeals.Copy();
                        grdDeals.DataBind();
                        Session["CurrentTableDeals"] = myDataTableDeals;

                        //for (int i = 0; i < myDataTableDeals.Rows.Count; i++)
                        //{
                        //    dr = myDataTableDeals.NewRow();


                        //    //dr["AFrom"] = "";
                        //    //dr["ATo"] = "";
                        //    //dr["Percent"] = "";
                        //    //dr["Value"] = "";
                        //  //  myDataTableDeals.Rows.Add(dr);
                        //    myDataTableDeals.Rows[i][0] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlFrom")).Text.ToString();
                        //    myDataTableDeals.Rows[i][1] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlTo")).Text.ToString();
                        //    myDataTableDeals.Rows[i][2] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlPercent")).Text.ToString();
                        //    myDataTableDeals.Rows[i][3] = ((TextBox)grdDeals.Rows[i].FindControl("txtDlValue")).Text.ToString();
                        //    myDataTableDeals.Rows.Add(dr);

                        //}
                        //grdDeals.DataSource = myDataTableDeals.Copy();
                        //grdDeals.DataBind();
                        //Session["CurrentTableDeals"] = myDataTableDeals;


                    }


                }
                //try
                //{
                //    DataTable myDataTableDeals = (DataTable)Session["CurrentTableDeals"];
                //    DataRow dr = myDataTableDeals.NewRow();

                //    myDataTableDeals.Rows.Add(dr);

                //    grdDeals.DataSource = myDataTableDeals.Copy();
                //    grdDeals.DataBind();


                //    //for (int i = 0; i < myDataTableDeals.Rows.Count; i++)
                //    //{

                //    //    DataRow row = myDataTableDeals.Rows[i];

                //    //    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtDlFrom")).Text = row["AFrom"].ToString();
                //    //    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtDlTo")).Text = row["ATo"].ToString();
                //    //    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtDlPercent")).Text = row["Percent"].ToString();
                //    //    ((TextBox)grdCreditinfo.Rows[i].FindControl("txtDlValue")).Text = row["Value"].ToString();

                //    //}


                //}
                //catch (Exception e)
                //{

                //}

            }

            catch (Exception ex) { }  

            #region Grid Display
            //DataTable myDataTableDeals = new DataTable();
            //DataColumn myDataColumn;
            //DataRow dr;
            ////if (ViewState["CurrentTableDeals"] == null)
            ////{
            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "AFrom";
            //    myDataTableDeals.Columns.Add(myDataColumn);

            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "ATo";
            //    myDataTableDeals.Columns.Add(myDataColumn);

            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "Percent";
            //    myDataTableDeals.Columns.Add(myDataColumn);

            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "Value";
            //    myDataTableDeals.Columns.Add(myDataColumn);

            //    dr = myDataTableDeals.NewRow();
            //    dr["AFrom"] = "";
            //    dr["ATo"] = "";
            //    dr["Percent"] = "";
            //    dr["Value"] = "";
            //    myDataTableDeals.Rows.Add(dr);
            //    grdDeals.DataSource = myDataTableDeals;
            //    grdDeals.DataBind();
            //    ViewState["CurrentTableDeals"] = myDataTableDeals;
            ////}
            ////else
            //    //if (((DataTable)ViewState["CurrentTableDeals"]).Rows.Count==1)
            //    //{
            //        myDataTableDeals = (DataTable)ViewState["CurrentTableDeals"];
            //        int ic = 0;
            //        while (ic < grdDeals.Rows.Count)
            //        {
            //            dr = myDataTableDeals.NewRow();
            //            dr["AFrom"] = "";
            //            dr["ATo"] = "";
            //            dr["Percent"] = "";
            //            dr["Value"] = "";
            //            myDataTableDeals.Rows.Add(dr);
            //            myDataTableDeals.Rows[ic][0] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlFrom")).Text.ToString();
            //            myDataTableDeals.Rows[ic][1] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlTo")).Text.ToString();
            //            myDataTableDeals.Rows[ic][2] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlPercent")).Text.ToString();
            //            myDataTableDeals.Rows[ic][3] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlValue")).Text.ToString();
            //            ic++;
            //        }
            //        grdDeals.DataSource = myDataTableDeals;
            //        grdDeals.DataBind();
            //        ViewState["CurrentTableDeals"]=myDataTableDeals;
                    
            //    //}
            //    //else 
            //    //{
            //        myDataTableDeals = (DataTable)ViewState["CurrentTableDeals"];
            //        dr = myDataTableDeals.NewRow();
            //        dr["AFrom"] = "";
            //        dr["ATo"] = "";
            //        dr["Percent"] = "";
            //        dr["Value"] = "";
            //        myDataTableDeals.Rows.Add(dr);
            //        grdDeals.DataSource = myDataTableDeals;
            //        grdDeals.DataBind();
            //        ViewState["CurrentTableDeals"] = myDataTableDeals;
            //    //}
            #endregion
            #region old GridBinding

            //lblStatus.Text = "";
            //DataTable myDataTableDeals = new DataTable();
            
            //    if (ViewState["CurrentTableDeals"] != null)
            //    {
            //    myDataTableDeals = (DataTable)ViewState["CurrentTableDeals"];
            //    }
            //        if (ViewState["CurrentTableDeals"] == null)
            //{
            //    DataColumn myDataColumn;
            //    DataSet Ds = new DataSet();
              
            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "AFrom";
            //    myDataTableDeals.Columns.Add(myDataColumn);

            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "ATo";
            //    myDataTableDeals.Columns.Add(myDataColumn);

            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "Percent";
            //    myDataTableDeals.Columns.Add(myDataColumn);

            //    myDataColumn = new DataColumn();
            //    myDataColumn.DataType = Type.GetType("System.String");
            //    myDataColumn.ColumnName = "Value";
            //    myDataTableDeals.Columns.Add(myDataColumn);
            
            //DataRow dr;
           
            //int ic=0;
            //while (ic<grdDeals.Rows.Count)
            //{
            //    dr = myDataTableDeals.NewRow();
            //    dr["AFrom"] = "";
            //    dr["ATo"] = "";
            //    dr["Percent"] = "";
            //    dr["Value"] = "";
            //    myDataTableDeals.Rows.Add(dr);
            //    myDataTableDeals.Rows[ic][0] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlFrom")).Text.ToString();
            //   myDataTableDeals.Rows[ic][1] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlTo")).Text.ToString();
            //        myDataTableDeals.Rows[ic][2] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlPercent")).Text.ToString();
            //        myDataTableDeals.Rows[ic][3] = ((TextBox)grdDeals.Rows[ic].FindControl("txtDlValue")).Text.ToString();
            //        ic++;
            //}
            //}
            //            //if (myDataTableDeals.Rows.Count == 0)
            //            //{
            //                DataRow drs;
            //    drs = myDataTableDeals.NewRow();
            //    drs["AFrom"] = "";
            //    drs["ATo"] = "";
            //    drs["Percent"] = "";
            //    drs["Value"] = "";
            //    myDataTableDeals.Rows.Add(drs);
            ////}
            // //if (myDataTableDeals.Rows.Count >0)
            ////{
            ////    myDataTableDeals.Rows[myDataTableDeals.Rows.Count - 1][0] = ((TextBox)grdDeals.Rows[myDataTableDeals.Rows.Count - 1].FindControl("txtDlFrom")).Text.ToString();
            ////    myDataTableDeals.Rows[myDataTableDeals.Rows.Count - 1][1] = ((TextBox)grdDeals.Rows[myDataTableDeals.Rows.Count - 1].FindControl("txtDlTo")).Text.ToString();
            ////    myDataTableDeals.Rows[myDataTableDeals.Rows.Count - 1][2] = ((TextBox)grdDeals.Rows[myDataTableDeals.Rows.Count - 1].FindControl("txtDlPercent")).Text.ToString();
            ////    myDataTableDeals.Rows[myDataTableDeals.Rows.Count - 1][3] = ((TextBox)grdDeals.Rows[myDataTableDeals.Rows.Count - 1].FindControl("txtDlValue")).Text.ToString();
            ////}

          
             







            //    ViewState["CurrentTableDeals"] = myDataTableDeals;
            ////Bind the DataTable to the Grid

            //grdDeals.DataSource = null;
            //grdDeals.DataSource = myDataTableDeals;
            //grdDeals.DataBind();
          
            //for (int i = 0; i < myDataTableDeals.Rows.Count; i++)
            //{
               
            //    if (i == myDataTableDeals.Rows.Count)
            //        return;
            //((TextBox)grdDeals.Rows[i].FindControl("txtDlFrom")).Text=myDataTableDeals.Rows[i][0].ToString();
            //   ((TextBox)grdDeals.Rows[i].FindControl("txtDlTo")).Text=myDataTableDeals.Rows[i][1].ToString();
            //  ((TextBox)grdDeals.Rows[i].FindControl("txtDlPercent")).Text=myDataTableDeals.Rows[i][2].ToString();
            //   ((TextBox)grdDeals.Rows[i].FindControl("txtDlValue")).Text=myDataTableDeals.Rows[i][3].ToString();
               
            //}
            //// grdDeals.DataBind();

            #endregion
        }
        # endregion 
        #region Load IAC Code
        public void LoadIACCode()
     

            {
                try
                {
                    DataSet ds = objBLL.GetIACCode();
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //ddlIACCode.Items.Clear();
                               
                                //ddlIACCode.DataSource = ds.Tables[0];
                                //ddlIACCode.DataTextField = "IACCode";
                                //ddlIACCode.DataValueField = "IACCode";
                                //ddlIACCode.DataBind();
                                DataTable dt = new DataTable();
                                dt = (DataTable)ds.Tables[0];

                                for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                                {
                                    ListItem item = new ListItem();
                                    item.Value = dt.Rows[intCount]["ApprovalNo"].ToString();
                                    item.Text = dt.Rows[intCount]["ApprovalNo"].ToString();
                                    ddlIACCode.Items.Add(item);
                                }

                                //ddlIACCode.Items.Add(ds);
                                  //ddlIACCode.Items.Insert(0, "Select");
                                   }
                        }
                    }
                }
                catch (Exception ex)
                { }

            }

        
        #endregion
        #region Load CCSF Code
        public void LoadCCSFCode()
        {
            try
            {
                DataSet ds = objBLL.GetCCSFCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //ddlCCSFCode.Items.Clear();

                            //ddlCCSFCode.DataSource = ds.Tables[0];
                            //ddlCCSFCode.DataTextField = "CCSFCode";
                            //ddlCCSFCode.DataValueField = "CCSFCode";
                            //ddlCCSFCode.DataBind();
                            //ddlCCSFCode.Items.Insert(0, "Select");
                            DataTable dt = new DataTable();

                            dt = (DataTable)ds.Tables[0];

                            for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
                            {
                                ListItem item = new ListItem();
                                item.Value = dt.Rows[intCount]["ApprovalNo"].ToString();
                                item.Text = dt.Rows[intCount]["ApprovalNo"].ToString();
                                ddlCCSFCode.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion

        protected void GridView2_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {

            //Check if there is any exception while deleting
            try
            {
                if (e.Exception != null)
                {
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + e.Exception.Message.ToString().Replace("'", "") + "');</script>");
                    e.ExceptionHandled = true;
                }
            }
            catch (Exception ex)
            { }
        }
        #region ViewAllStock
        protected void btnViewStock_Click(object sender, EventArgs e)
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());

                DataSet ds = db.SelectRecords("Sp_ShowAllStock");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            GRDAllocatedToAgent.DataSource = ds;
                            GRDAllocatedToAgent.DataBind();
                            lblStatus.Text = "";
                        }
                        else
                        {
                            GRDAllocatedToAgent.DataSource = ds;
                            GRDAllocatedToAgent.DataBind();
                            //lblStatus.Text = "No Records Found For Stock.Please Allocate Series.";
                        }
                    }

                }

            }
            catch (Exception e1)
            {
            }
        }
        #endregion ViewAllStock
        #region Sort Grid
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvSortExpr = e.SortExpression;
            LoadAgentStockData();
        }
        #endregion Sort Grid
        #region LstCity
        protected void lstCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            getLocationdata();
        }
        #endregion LstCity
        #region getLocationdata
        private void getLocationdata()
        {
            try
            {
                //DataTable dt1 = new DataTable();
                //string strSort = lstCity.SelectedItem.Text;
                //sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

                //sc.Open();
                //scmd = new SqlCommand("spXpressgetLocationCodebyCityCode", sc); //receipt come from xpress bag details
                //scmd.CommandType = CommandType.StoredProcedure;
                //scmd.Parameters.AddWithValue("@CityCode", strSort);
                //sdr = scmd.ExecuteReader();
                //dt1.Load(sdr);
                //GridView1.DataSource = dt1;
                //GridView1.DataBind();
                //sc.Close();
            }
            catch (Exception ex)
            { }
        }
        #endregion getLocationdata
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                string[] paramname = new string[1];
                paramname[0] = "AgentCode";
                string AgentCode = string.Empty;

                //if (FltDate == "")
                //{

                //    FltDate = "NA"; // NA=not available 
                //}


                object[] paramvalue = new object[1];

                string str = TXTNewAgentName.Text;

                DataSet ds1 = objBLL.GetAgentList(Session["AgentCode"].ToString());
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    if (str == ds1.Tables[0].Rows[i][1].ToString())
                    {
                        paramvalue[0] = ds1.Tables[0].Rows[i][0];
                        break;
                    }

                }
                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.VarChar;

                DataSet ds = da.SelectRecords("sp_SearchAgentList", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdCreditinfo.DataSource = ds.Tables[0];
                            grdCreditinfo.DataBind();
                        }
                        else
                        {
                            grdCreditinfo.Visible = false;
                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                            lblStatus.Text = "No Record Found Please Insert Data..";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion List
        #region ListInfo
        protected void btnListInfo_Click(object sender, EventArgs e)
        {
            //SpFillAgentMaster
            try
            {
                #region Old Code

                //  GetCity();
                //  LoadAgentStockData();
                //  GetCountry();

                //AWBAvilbleForAlloc();
                //  string[] paramname = new string[1];
                //  paramname[0] = "AgentCode";

                //  object[] paramvalue = new object[1];
                //  paramvalue[0] = TXTNewAgentName.Text;


                //  SqlDbType[] paramtype = new SqlDbType[1];
                //  paramtype[0] = SqlDbType.NVarChar;

                //  DataSet ds = da.SelectRecords("SpFillAgentMaster", paramname, paramvalue, paramtype);
                //  if (ds != null)
                //  {
                //      if (ds.Tables.Count > 0)
                //      {
                //          if (ds.Tables[0].Rows.Count > 0)
                //          {

                //              ddlcountry.Text = ds.Tables[0].Rows[0]["Country"].ToString();
                //              ddlcity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                //              txtmobile.Text = ds.Tables[0].Rows[0]["MobileNumber"].ToString();
                //              txtcustomercode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                //              txtcontactperson.Text = ds.Tables[0].Rows[0]["PersonContact"].ToString();
                //              txtadress.Text = ds.Tables[0].Rows[0]["Adress"].ToString();
                //              txtemail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                //              txtIATAcode.Text = ds.Tables[0].Rows[0]["IATAAgentCode"].ToString();
                //              txtvalidfrom.Text = ds.Tables[0].Rows[0]["ValidFrom"].ToString();
                //              txtvalidto.Text = ds.Tables[0].Rows[0]["ValidTo"].ToString();
                //              txtcreditAmt.Text = ds.Tables[0].Rows[0]["CreditAmount"].ToString();
                //              txtcreditremain.Text = ds.Tables[0].Rows[0]["CreditRemaining"].ToString();
                //              ddlcontrolinglocator.SelectedItem.Text = ds.Tables[0].Rows[0]["ControllingLocator"].ToString();
                //              txtAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString();
                //              txttdsoncomm.Text = ds.Tables[0].Rows[0]["TDSOnCommision"].ToString();
                //              txtTDSFreight.Text = ds.Tables[0].Rows[0]["TDSOnFreight"].ToString();
                //              txtlocatorcode.Text = ds.Tables[0].Rows[0]["ControllingLocatorCode"].ToString();  
                //              //((DropDownList)grdCreditinfo.Rows[0].FindControl("ddlbankname")).Text=ds.Tables[0].Rows[0]["BankName"].ToString();

                //          }

                //          else
                //          {

                //              //            ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                //              lblStatus.Text = "No Record Found Please Insert Data..";
                //          }

                //          if (txtcustomercode.Text == "" && txtIATAcode.Text == "")
                //          {
                //              //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoRecord()", true);
                //              lblStatus.Text = "No Record Found Please Insert Data..";
                //          }
                //      }

                //  }
                #endregion

                #region New Code
                if (lblStatus.Text == "Agent code already exist")
                {
                    lblStatus.Text = "";
                    string[] paramname = new string[2];
                    paramname[0] = "AgentCode";
                    paramname[1] = "City";

                    string AgentCode = TXTAgentCode.Text;
                    string City = "";
                    object[] paramvalue = new object[2];
                    paramvalue[0] = AgentCode;
                    paramvalue[1] = "";
                    SqlDbType[] paramtype = new SqlDbType[2];
                    paramtype[0] = SqlDbType.VarChar;
                    paramtype[1] = SqlDbType.VarChar;

                    DataSet ds = da.SelectRecords("SPGetAgentCreadit", paramname, paramvalue, paramtype);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblStatus.Text = "";
                                string CreditNo = ds.Tables[0].Rows[0]["SerialNumber"].ToString();
                                //Session["CurrentTableDeals"] = null;
                                dsCredit = new DataSet();
                                dsCredit = objBLL.GetCreditDetails(CreditNo);
                                fillCreditDetails(dsCredit);
                                validateExpiry();
                                GetInvoices();
                                LoadGRDRates();
                                LoadDealsGrid();
                                TabContainer1.ActiveTabIndex = 0;

                            }
                        }
                    }
                }
                #endregion

            }

            catch (Exception ex)
            {

            }
        }
        #endregion ListInfo

        #region Save All Button
        protected void BtnAllSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool resAgent = false; //Agent save result
                bool resCredit = false; //Credit save result

                lblStatus.Text = string.Empty;
                lblCreditStatus.Text = string.Empty;
                if (BtnAllSave.Text != "Update")
                {
                    #region check agent exists or not
                    string[] pname = new string[1];
                    pname[0] = "AgentCode";

                    object[] pvalue = new object[1];
                    pvalue[0] = TXTAgentCode.Text.Trim();

                    SqlDbType[] ptype = new SqlDbType[1];
                    ptype[0] = SqlDbType.NVarChar;

                    DataSet dsAgent = da.SelectRecords("SP_ValidateAgent", pname, pvalue, ptype);

                    if (dsAgent != null)
                    {

                        if (dsAgent.Tables.Count > 0)
                        {
                            if (dsAgent.Tables[0].Rows.Count > 0)
                            {
                                lblStatus.Text = "Agent code already exist";
                                lblStatus.ForeColor = Color.Red;
                                TXTNewAgentName.Text = dsAgent.Tables[0].Rows[0]["AgentName"].ToString();
                                return;
                            }
                        }
                    }
                    #endregion
                }
                string value = "SAVE";
                MasterLog(value);
                                
                #region SaveGeneral
                //if (txtcontactperson.Text == "" && ddlcountry.Text == "" && txtIATAcode.Text == "")
                //{
                //    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "EnterData()", true);
                //    lblStatus.Text = "Please Enter Data";
                //    lblStatus.ForeColor = Color.Blue;    
                //    return;
                //}

                if (ddlcity.SelectedIndex == 0)
                {
                    lblStatus.Text = "Please Select Airport";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                try
                {
                    string[] paramname = new string[46];
                    paramname[0] = "AgentCode";
                    paramname[1] = "IATAAgentCode";
                    paramname[2] = "AgentName";
                    paramname[3] = "ValidFrom";
                    paramname[4] = "ValidTo";
                    paramname[5] = "CustomerCode";
                    paramname[6] = "Station";
                    paramname[7] = "Country";
                    paramname[8] = "City";
                    paramname[9] = "Adress";
                    paramname[10] = "Email";
                    paramname[11] = "PersonContact";
                    paramname[12] = "MobileNumber";
                    paramname[13] = "Remark";
                    paramname[14] = "strFlag";
                    paramname[15] = "NormalComm";
                    paramname[16] = "ControllingLocator";
                    paramname[17] = "AccountCode";
                    paramname[18] = "TDSOnCommision";
                    paramname[19] = "TDSOnFreight";
                    paramname[20] = "ControllingLocatorCode";
                    paramname[21] = "BuildTo";
                    paramname[22] = "AccountMail";
                    paramname[23] = "SalesMail";
                    paramname[24] = "AgentType";
                    paramname[25] = "BillType";
                    paramname[26] = "PanCardNumber";
                    paramname[27] = "ServiceTaxNumber";
                    paramname[28] = "ValidBG";

                    //shashi
                    paramname[29] = "CurrencyCode";
                    paramname[30] = "IsFOC";
                    paramname[31] = "threshold";
                    paramname[32] = "AgentReferenceCode";
                    paramname[33] = "RatePreference";
                    paramname[34] = "AutoGenInv";
                    paramname[35] = "chkStnList";
                    paramname[36] = "IACCode";
                    paramname[37] = "CCSFCode";
                    paramname[38] = "KnownShipper";

                    paramname[39] = "InvoiceDueDays";
                    paramname[40] = "VatExemp";
                    paramname[41] = "DefPayMode";
                    paramname[42] = "AllowPayMode";
                    paramname[43] = "AddedBy";
                    paramname[44] = "AddedOn";
                    paramname[45] = "DBA";

                    object[] paramvalue = new object[46];
                    if (HidFlag.Value == "I")
                    {
                        paramvalue[0] = TXTAgentCode.Text.ToUpper();
                    }
                    else if (TXTAgentCode.Text == "")
                    {
                        lblStatus.Text = "Please Enter Agent Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        paramvalue[0] = TXTAgentCode.Text.ToUpper() ;
                    }
                    if (txtIATAcode.Text.Trim() == TXTAgentCode.Text)
                    {
                        paramvalue[1] = txtIATAcode.Text.Trim().ToUpper();
                    }
                    else
                    {
                        lblStatus.Text = "Please enter IATA Agent Code same as Agent Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (HidFlag.Value == "I")
                    {
                        paramvalue[2] = TXTNewAgentName.Text;
                    }
                    else if (TXTNewAgentName.Text == "")
                    {
                        lblStatus.Text = "Please Enter Agent Name";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        paramvalue[2] = TXTNewAgentName.Text;
                    }


                    //12-7-2012

                    if (txtvalidfrom.Text == "")
                    {

                        lblStatus.Text = "Please Enter Valid From Date";
                        lblStatus.ForeColor=Color.Red;    
                        return; 
                    }
                    else
                    {
                        try
                        {
                            paramvalue[3] = DateTime.ParseExact(txtvalidfrom.Text,"dd/MM/yyyy",null);
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Enter date in correct format";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                    }
                    if (txtvalidto.Text == "")
                    {
                        lblStatus.Text = "Please Enter Valid To Date";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        try
                        {
                            paramvalue[4] = DateTime.ParseExact(txtvalidto.Text.Trim(),"dd/MM/yyyy",null);
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Enter date in correct format";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                    paramvalue[5] = txtcustomercode.Text.Trim();
                    
                    if (ddlcity.SelectedIndex == 0)
                    {
                        lblStatus.Text = "Please select Airport Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    paramvalue[6] = ddlcity.SelectedValue.ToString(); //ddlcity.SelectedItem.Text.Trim();

                    paramvalue[7] = ddlcountry.SelectedValue.ToString(); //ddlcountry.SelectedItem.Text;
                    paramvalue[8] = Session["Station"].ToString();
                    paramvalue[9] = txtadress.Text.Trim();
                    paramvalue[10] = txtemail.Text.Trim();
                    paramvalue[11] = txtcontactperson.Text.Trim();

                    //if (txtmobile.Text.Trim() == "")
                    //{
                    //    lblStatus.Text = "Please enter Mobile No";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                    paramvalue[12] = txtmobile.Text.Trim();

                    paramvalue[13] = txtremark.Text.Trim();
                    paramvalue[14] = HidFlag.Value;
                    paramvalue[15] = txtcommision.Text.Trim();
                    if (ddlcontrolinglocator.SelectedIndex == 0)
                    {
                        lblStatus.Text = "Please select Controlling Locator";
                        lblStatus.ForeColor = Color.Red;
                        return; 
                    }

                    if (ddlcontrolinglocator.SelectedItem.Text.Trim().ToUpper() == "YES")
                    {
                        paramvalue[16] = "YES"; 
                    }
                    else if (ddlcontrolinglocator.SelectedItem.Text.Trim().ToUpper() == "NO")
                    {
                        paramvalue[16] = "NO";
                    }

                    // GLAccountCode DropDown
                    paramvalue[17] = txtAccountCode.Text.Trim();
                    paramvalue[18] = txttdsoncomm.Text.Trim();

                    //if (txttdsoncomm.Text == "")
                    //{
                    //    lblStatus.Text = "Please enter TDS On Commission";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                    //if (txttdsoncomm.Text == "")
                    //{
                    //    lblStatus.Text = "Please enter tds on commission";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;

                    //}
                    //else
                    //{
                    //    paramvalue[18] = txttdsoncomm.Text.Trim();
                    //}
                    //if (txtTDSFreight.Text == "")
                    //{
                    //    lblStatus.Text = "Please enter TDS On Freight";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                    paramvalue[19] = txtTDSFreight.Text.Trim();

                    if (ddlcontrolinglocator.SelectedItem.Text.Trim().ToUpper() == "NO")
                    {
                        if (txtlocatorcode.Text.Trim() == "")
                        {
                            lblStatus.Text = "Please Enter Controlling Locator Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (imgcheck.Visible == true)
                        {
                            paramvalue[20] = txtlocatorcode.Text.Trim();
                        }


                        else if (imgcross.Visible == true)
                        {
                            lblStatus.Text = "Please Enter Valid Controlling locator";
                            lblStatus.ForeColor = Color.Red;
                            return;

                        }
                    }
                    else
                    {
                        paramvalue[20] = "";
                    }
                    paramvalue[21] = ddlbuildto.SelectedValue;
                    if (ddlbuildto.SelectedItem.Text == "Controlling Locator")
                    {
                        if (txtlocatorcode.Text == "")
                        {
                            lblStatus.Text = "Please Enter Controlling Locator Code ";
                            lblStatus.ForeColor = Color.Red;
                            return;

                        }
                    }
                    paramvalue[22] = txtaccountmailid.Text.Trim();
                    paramvalue[23] = txtsalesmail.Text.Trim();
                    if (ddlAgentType.SelectedIndex == 0)
                        paramvalue[24] = "";
                    else
                    paramvalue[24] = ddlAgentType.SelectedItem.Text.Trim();
                    paramvalue[25] = ddlBillType.SelectedItem.Text.Trim();
                    paramvalue[26] = txtPancardNumber.Text.Trim();
                    //if (txtPancardNumber.Text == "")
                    //{
                    //    lblStatus.Text = "Please Enter Pan-Card Number";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                    //else
                    //{
                    //    paramvalue[26] = txtPancardNumber.Text.Trim();
                    //}
                    paramvalue[27] = txtServicetaxno.Text.Trim();
                    //if (txtServicetaxno.Text == "")
                    //{
                    //    lblStatus.Text = "Please Enter Service-TAX Number";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                    //else
                    //{
                    //    paramvalue[27] = txtServicetaxno.Text.Trim();
                    //}
                    if (chkvalidbg.Checked)
                    {
                        paramvalue[28] = "Yes"; 
                    }
                    else
                    {
                        paramvalue[28] = "No"; 
                    }

                    paramvalue[29] = ddlCurrencyCode.SelectedItem.Text.Trim();
                    paramvalue[30] = chkIsFOC.Checked;
                    paramvalue[31] = txtThresholdAlert.Text.Trim();
                    paramvalue[32] = txtAgentReferenceCode.Text.ToUpper().Trim();
                    if (ddlRatelinePreference.SelectedIndex == 0)
                        paramvalue[33] = "";
                    else
                    paramvalue[33] = ddlRatelinePreference.SelectedItem.Text.Trim();
                    paramvalue[34] = chkAutoGenInv.Checked;
                    string stnList = GetStnList();
                    paramvalue[35] = stnList;
                    if (ddlIACCode.SelectedItem.Text.ToString() == "Select")
                    {
                        paramvalue[36] = "";
                    }
                    else
                    {
                        paramvalue[36] = ddlIACCode.SelectedItem.Text.ToString();
                    }
                    if (ddlCCSFCode.SelectedItem.Text.ToString() == "Select")
                    {
                        paramvalue[37] = "";
                    }
                    else
                    {
                        paramvalue[37] = ddlCCSFCode.SelectedItem.Text.ToString();
                       
                    }
                    paramvalue[38] = chkKnownShipper.Checked;
                    
                    //Parameter for Invoice due days
                    try
                    {
                        if (txtInvoiceDueDays.Text.Trim() == "")
                        {
                            lblStatus.Text = "Please enter Invoice due days";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (Convert.ToInt32(txtInvoiceDueDays.Text.Trim()) < 0)
                        {
                            lblStatus.Text = "Invoice due days not valid";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                    catch
                    {
                        lblStatus.Text = "Invoice due days not valid";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    paramvalue[39] = Convert.ToInt32(txtInvoiceDueDays.Text.Trim());
                    paramvalue[40] = ChkVAT.Checked;
                     if (ddlDefaultPayMode.SelectedItem.Text.ToString() == "Select")
                    {
                        paramvalue[41] = "";
                    }
                    else
                    {
                        paramvalue[41] = ddlDefaultPayMode.SelectedItem.Text.ToString();
                       
                    }
                     if (!String.IsNullOrEmpty(txtAllowPayMode.Text))
                         paramvalue[42] = txtAllowPayMode.Text;
                     else
                         paramvalue[42] = "";

                    //Added by Vijay - 26-09-2014
                     paramvalue[43] = Session["UserName"].ToString();
                     
                     paramvalue[44] = Convert.ToDateTime(Session["IT"]);
                     paramvalue[45] = txtDBA.Text.Trim();
                    

                    SqlDbType[] paramtype = new SqlDbType[46];
                    paramtype[0] = SqlDbType.NVarChar;
                    paramtype[1] = SqlDbType.NVarChar;
                    paramtype[2] = SqlDbType.NVarChar;
                    paramtype[3] = SqlDbType.VarChar;
                    paramtype[4] = SqlDbType.VarChar;
                    paramtype[5] = SqlDbType.NVarChar;
                    paramtype[6] = SqlDbType.NVarChar;
                    paramtype[7] = SqlDbType.NVarChar;
                    paramtype[8] = SqlDbType.NVarChar;
                    paramtype[9] = SqlDbType.NVarChar;
                    paramtype[10] = SqlDbType.NVarChar;
                    paramtype[11] = SqlDbType.NVarChar;
                    paramtype[12] = SqlDbType.VarChar;
                    paramtype[13] = SqlDbType.NVarChar;
                    paramtype[14] = SqlDbType.NVarChar;
                    paramtype[15] = SqlDbType.NVarChar;
                    paramtype[16] = SqlDbType.NVarChar;
                    paramtype[17] = SqlDbType.NVarChar;
                    paramtype[18] = SqlDbType.NVarChar;
                    paramtype[19] = SqlDbType.NVarChar;
                    paramtype[20] = SqlDbType.NVarChar;
                    paramtype[21] = SqlDbType.NVarChar;
                    paramtype[22] = SqlDbType.NVarChar;
                    paramtype[23] = SqlDbType.NVarChar;
                    paramtype[24] = SqlDbType.NVarChar;
                    paramtype[25] = SqlDbType.NVarChar;
                    paramtype[26] = SqlDbType.VarChar;
                    paramtype[27] = SqlDbType.VarChar;
                    paramtype[28] = SqlDbType.VarChar;
                    paramtype[29] = SqlDbType.VarChar;
                    paramtype[30] = SqlDbType.Bit;
                    paramtype[31] = SqlDbType.VarChar;
                    paramtype[32] = SqlDbType.VarChar;
                    paramtype[33] = SqlDbType.VarChar;
                    paramtype[34] = SqlDbType.Bit;
                    paramtype[35] = SqlDbType.VarChar;
                    paramtype[36] = SqlDbType.VarChar;
                    paramtype[37] = SqlDbType.VarChar;
                    paramtype[38] = SqlDbType.Bit;
                    paramtype[39] = SqlDbType.Int;
                    paramtype[40] = SqlDbType.Bit;
                    paramtype[41] = SqlDbType.VarChar;
                    paramtype[42] = SqlDbType.VarChar;
                    paramtype[43] = SqlDbType.VarChar;
                    paramtype[44] = SqlDbType.DateTime;
                    paramtype[45] = SqlDbType.VarChar;

                    resAgent = da.InsertData("Sp_InsertAgentDetails", paramname, paramtype, paramvalue);

                    if (resAgent == false)
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "InsertFailure()", true);
                        lblStatus.Text = "Agent Insertion Failed";
                        lblStatus.ForeColor = Color.Red;
                        return;

                    }
                    else if (Request.QueryString["cmd"] == "Edit")
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Paramsss = new object[7];
                        int k = 0;

                        //1
                        Paramsss.SetValue("Agent Master", k);
                        k++;

                        //2
                        string MstValue = TXTAgentCode.Text;
                        Paramsss.SetValue(MstValue, k);
                        k++;

                        //3
                        Paramsss.SetValue("UPDATED", k);
                        k++;

                        //4
                        string Msg = "Agent Updated";
                        Paramsss.SetValue(Msg, k);
                        k++;

                        //5
                        string Desc = "Agent Code:" + TXTAgentCode.Text + "/Agent Name:" + TXTNewAgentName.Text;
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
                        lblStatus.Text = "Agent Updated Successfully";
                        lblStatus.ForeColor = Color.Green;

                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Insert()", true);
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Paramsss = new object[7];
                        int k = 0;

                        //1
                        Paramsss.SetValue("Agent Master", k);
                        k++;

                        //2
                        string MstValue = TXTAgentCode.Text;
                        Paramsss.SetValue(MstValue, k);
                        k++;

                        //3
                        Paramsss.SetValue("ADD", k);
                        k++;

                        //4
                        string Msg = "Agent Added";
                        Paramsss.SetValue(Msg, k);
                        k++;

                        //5
                        string Desc = "Agent Code:" + TXTAgentCode.Text + "/Agent Name:" + TXTNewAgentName.Text;
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
                        lblStatus.Text = "Agent created successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "InsertFailure()", true);
                    lblStatus.Text = "Agent Insertion  Failed";
                    lblStatus.ForeColor = Color.Red;
                    return;

                }


                //else
                //{
                //    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "SelectRow()", true);
                //    lblStatus.Text = "Please Select Row";
                //}

                #endregion savegeneral

                #region Save TDS on commission information
                try
                {
                    bool res = false;
                    int SrNo = 0;
                    string AgentCode = "", FromDate = "", ToDate = "", strResult = "", Updatedby = "", Updatedon = "";
                    float TDSOnCommPercent = 0;
                    AgentCode = TXTAgentCode.Text;

                    Updatedby = Convert.ToString(Session["Username"]);
                    Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");

                    //Code to inactivate all the values for TDS on commission before inserting newly updated
                    string strUpdate = objBLL.UpdateTDSOnCommissionFlag(AgentCode, Updatedby, Updatedon);

                    if (strUpdate == "TDS updated successfully")
                    {
                        for (int k = 0; k < grdTDSInfo.Rows.Count; k++)
                        {
                            SrNo = int.Parse(((Label)grdTDSInfo.Rows[k].FindControl("lblSrNo")).Text);
                            FromDate = ((TextBox)grdTDSInfo.Rows[k].FindControl("txtFromDate")).Text;
                            ToDate = ((TextBox)grdTDSInfo.Rows[k].FindControl("txtToDate")).Text;
                            TDSOnCommPercent = float.Parse(((TextBox)grdTDSInfo.Rows[k].FindControl("txtTDSonCommPerc")).Text);

                            DataSet dsResult = objBLL.AddTDSOnCommission(AgentCode, SrNo, FromDate, ToDate, TDSOnCommPercent, Updatedby, Updatedon, ref strResult);

                            if (dsResult != null)
                            {
                                if (dsResult.Tables != null)
                                {
                                    if (dsResult.Tables.Count > 0)
                                    {
                                        if (dsResult.Tables[0].Rows[0][0].ToString() == "TDS on commission saved successfully")
                                        {
                                            res = true;
                                        }
                                        else if (dsResult.Tables[0].Rows[0][0].ToString() == "Invalid date range")
                                        {
                                            res = false;
                                            lblStatus.ForeColor = Color.Red;
                                            lblStatus.Text = "Enter valid From Date and To Date in TDS on Commission information";
                                            return;
                                        }
                                        else
                                        {
                                            res = false;
                                            lblStatus.ForeColor = Color.Red;
                                            lblStatus.Text = "TDS on commission not saved";
                                            return;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "TDS on commission not saved";
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "TDS on commission not saved";
                        return;
                    }


                }
                catch (Exception ex)
                {
                }
                #endregion Save TDS on commission information
                
                #region SaveCredit
                try
                {
                    string[] paramname = new string[19];
                    paramname[0] = "AgentCode";
                    paramname[1] = "BankName";
                    paramname[2] = "BankGuranteeNumber";
                    paramname[3] = "BankGuranteeAmount";
                    paramname[4] = "ValidFrom";
                    paramname[5] = "ValidTo";
                    paramname[6] = "FinalAmt";
                    paramname[7] = "CreditAmount";
                    paramname[8] = "InvoiceBalance";
                    paramname[9] = "CreditRemaining";
                    paramname[10] = "Expired";
                    paramname[11] = "TresholdLimit";
                    paramname[12] = "TransactionType";
                    paramname[13] = "CreditDays";
                    paramname[14] = "TresholdLimitDays";
                    paramname[15] = "BankAddress";
                    paramname[16] = "SerialNumber";
                    paramname[15] = "BankAddress";
                    paramname[16] = "SerialNumber";
                    paramname[17] = "AddedBy";
                    paramname[18] = "AddedOn";

                    object[] paramvalue = new object[19];
                    paramvalue[0] = TXTAgentCode.Text;
                    //Commeted by Vijay
                    //da.ExecuteProcedure("Sp_DeleteCreditAndAllocationData", "AgentCode", SqlDbType.VarChar, TXTAgentCode.Text.Trim());

                    for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
                    {
                        paramvalue[1] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Text;
                        paramvalue[2] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text;
                        paramvalue[3] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text;
                        //23-7-2012
                        string dtvalidfromdate = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text;
                        string dtvalidtodate = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text;
                        if (((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text != "" && dtvalidfromdate == "" && dtvalidtodate=="")
                        {
                            lblStatus.Text = "Please Enter Valid From and To Date in Credit ";
                            lblStatus.ForeColor = Color.Red;    
                            return;
                        }
                        if (dtvalidfromdate == "")
                        {
                            paramvalue[4] = "";
                        }
                        else
                        {
                            paramvalue[4] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text;
                        }
                        if (dtvalidtodate == "")
                        {
                            paramvalue[5] = "";
                        }
                        else
                        {
                            paramvalue[5] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text;
                        }
                        paramvalue[6] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtAmount")).Text.Trim() == "" ? "0" : ((TextBox)grdCreditinfo.Rows[i].FindControl("txtAmount")).Text.Trim();
                        paramvalue[7] = txtcreditAmt.Text.Trim() == "" ? "0" : txtcreditAmt.Text.Trim();
                        paramvalue[8] = txtinvoice.Text.Trim() == "" ? "0" : txtinvoice.Text.Trim();
                        paramvalue[9] = txtcreditremain.Text.Trim() == "" ? "0" : txtcreditremain.Text.Trim();
                        if (((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Checked)
                        {
                            paramvalue[10] = "Yes";
                        }
                        else
                        {
                            paramvalue[10] = "No";
                        }
                        paramvalue[11] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Text.Trim() == "" ? "0" : ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Text.Trim();
                        paramvalue[12] = ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).SelectedItem.Text;

                        paramvalue[13] = (txtCreditdays.Text);
                        paramvalue[14] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Text.Trim() == "" ? "0" : ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Text.Trim();
                        paramvalue[15] = ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankaddress")).Text;

                        paramvalue[16] = ((Label)grdCreditinfo.Rows[i].FindControl("lblSrNo")).Text == "" ? "0" : ((Label)grdCreditinfo.Rows[i].FindControl("lblSrNo")).Text;

                        //Added by Vijay - 26-09-2014
                        paramvalue[17] = Session["UserName"].ToString();

                        paramvalue[18] = Convert.ToDateTime(Session["IT"]);

                        SqlDbType[] paramtype = new SqlDbType[19];
                        paramtype[0] = SqlDbType.NVarChar;
                        paramtype[1] = SqlDbType.NVarChar;
                        paramtype[2] = SqlDbType.NVarChar;
                        paramtype[3] = SqlDbType.Float;
                        paramtype[4] = SqlDbType.VarChar;
                        paramtype[5] = SqlDbType.VarChar;
                        paramtype[6] = SqlDbType.VarChar;
                        paramtype[7] = SqlDbType.Float;
                        paramtype[8] = SqlDbType.Float;
                        paramtype[9] = SqlDbType.Float;
                        paramtype[10] = SqlDbType.NVarChar;
                        paramtype[11] = SqlDbType.NVarChar;
                        paramtype[12] = SqlDbType.NVarChar;
                        paramtype[13] = SqlDbType.VarChar;
                        paramtype[14] = SqlDbType.Int;
                        paramtype[15] = SqlDbType.VarChar;
                        paramtype[16] = SqlDbType.Int;
                        paramtype[17] = SqlDbType.VarChar;
                        paramtype[18] = SqlDbType.DateTime;
                        
                        resCredit = da.InsertData("Sp_InsertCreditData", paramname, paramtype, paramvalue);

                        if (Request.QueryString["cmd"] == "Edit")
                        {
                            lblStatus.Text = "Agent Updated Successfully";
                            lblStatus.ForeColor = Color.Green;
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Agent Insertion Failed";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                 
                #endregion SaveCredit
                
                #region Load Commented
                //LoadAgentDropdown();
                //LoadAgentName();
                //LoadGridCredit();
                //LoadBank();              
                //GetCity();
             
                
                //ClearGeneral();
                //GetCountry();
                //TXTAgentCode.Enabled = false;
                //TXTNewAgentName.Visible = false;
                //TabContainer1.Visible = true;
                #endregion Load
                
                #region Save Allocation
                dsStockAllocation = (DataSet)Session["dsStockAllocation"];
                if (dsStockAllocation != null && dsStockAllocation.Tables.Count != 0 && dsStockAllocation.Tables[0].Rows.Count != 0)
                {
                    // AgentName LevelCode SubLevelCode FromAWB ToAWB
                    //Agentname  FrmAWB  ToAWB SubLevel  city    "Airport",
                    string preAWB = string.Empty;
                    if (Session["awbPrefix"] != null)
                    {
                        preAWB = Session["awbPrefix"].ToString();

                    }
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        preAWB = Session["awbPrefix"].ToString();
                    }
                    for (int i = 0; i < dsStockAllocation.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dsStockAllocation.Tables[0].Rows[i];

                        DataSet ds = BLObj.SaveAgentAllocatedData(row["AgentName"].ToString(), row["FromAWB"].ToString(), row["ToAWB"].ToString(), row["SubLevelCode"].ToString(), "City", Session["UserName"].ToString(), "", preAWB,Convert.ToDateTime(Session["IT"]));
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows[0][0].ToString() == "insert")
                                    {
                                        lblStatus.Text = "Agent Added Successfully.";
                                        lblStatus.ForeColor = Color.Green  ;
                                    }
                                    else if (Request.QueryString["cmd"] == "Edit")
                                    {
                                        lblStatus.Text = "Agent Updated Successfully";
                                        lblStatus.ForeColor = Color.Green;

                                    }
                                    else if (ds.Tables[0].Rows[0][0].ToString() == "notinsert" || ds.Tables[0].Rows[0][0].ToString() == "To" )
                                    {
                                        //lblStatus.Text = "Stock Already Allocated...";
                                        //lblStatus.ForeColor = Color.Red;
                                    }
                                    //else
                                    //{
                                    //    lblStatus.Text = "Error.";
                                    //    lblStatus.ForeColor = Color.Red;
                                    //}
                                }

                            }
                        }
                        else
                        {
                            //lblStatus.Text = "Error.";
                            //lblStatus.ForeColor = Color.Red;
                        }
                       
                    }
                }

                

                


                #endregion
                
                #region SaveDeals
                try
                {
                    //lblStatus.Text = "";

                    object[] ObjSummary = new object[4];
                    ObjSummary.SetValue(TXTAgentCode.Text, 0);
                    ObjSummary.SetValue(ddlType.SelectedValue.ToString(), 1);
                    string sDate;
                  //   for (int i = 0; i < grdDeals.Rows.Count; i++)
                  // {
                  //  if(((TextBox)grdDeals.Rows[i].FindControl("txtDlPercent")).Text=="" ||(((TextBox)grdDeals.Rows[i].FindControl("txtDlValue")).Text==""))
                  //  {
                         
                  //  }
                  //}
                    if (txtlblDealDateFrom.Text == "")
                    {
                         sDate = ""; 
                    }
                    else
                    {
                        DateTime dt = DateTime.ParseExact(txtlblDealDateFrom.Text.Trim(), "dd/MM/yyyy", null);
                         sDate = dt.ToShortDateString();
                    }
                    ObjSummary.SetValue(sDate, 2);
                   
                    if (txtlblDealDateTo.Text == "")
                    {
                         sDate = "";
                    }
                    else
                    {
                        DateTime dt = DateTime.ParseExact(txtlblDealDateTo.Text.Trim(), "dd/MM/yyyy", null);
                          sDate = dt.ToShortDateString();
                    }
                        ObjSummary.SetValue(sDate, 3);
                    string result = objBLL.SaveDealsSummary(ObjSummary);



                    if (result == "true")
                    {
                        object[] ObjDet = new object[5];

                        for (int i = 0; i < grdDeals.Rows.Count; i++)
                        {
                            ObjDet.SetValue(TXTAgentCode.Text, 0);
                            ObjDet.SetValue(((TextBox)grdDeals.Rows[i].FindControl("txtDlFrom")).Text, 1);
                            ObjDet.SetValue(((TextBox)grdDeals.Rows[i].FindControl("txtDlTo")).Text, 2);
                            ObjDet.SetValue(((TextBox)grdDeals.Rows[i].FindControl("txtDlPercent")).Text, 3);
                            ObjDet.SetValue(((TextBox)grdDeals.Rows[i].FindControl("txtDlValue")).Text, 4);
                            result = objBLL.SaveDealsDetails(ObjDet);

                        }
                       // ClearDeals();
                        result = "";
                    }
                    else
                    {

                        lblStatus.Text = "Deal saving failure";
                        lblStatus.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Deal saving failure";
                    lblStatus.ForeColor = Color.Red;
                }

                #endregion 

                #region result true list agent
                //To check the result of Agent save and credit save
                if(Request.QueryString["cmd"] == "Edit" && resAgent == true && resCredit == true)
                {
                    btnListAgent_Click(null, null);
                    lblStatus.Text = "Agent Updated Successfully";
                    lblStatus.ForeColor = Color.Green;

                }
                else if(resAgent == true && resCredit == true)
                {
                    btnListAgent_Click(null, null);
                    lblStatus.Text = "Agent created successfully";
                    lblStatus.ForeColor = Color.Green;
                }
                #endregion result true list agent

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(),"ShowPnl2", "InsertFailure()", true);
                lblStatus.Text = "Agent Insertion  Failed";
                lblStatus.ForeColor = Color.Red;    

            }
        }
        #endregion Save All Button

        #region ClearAll
        public void ClearAll()
        {
            ClearGeneral();
            TXTNewAgentName.Text = "";
            TXTSubLevel.Text = "";
            TXTToAWB.Text = "";
            TXTAgentCode.Text = "";

            for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
            {
                ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddlbankname")).SelectedIndex = 0;
                ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text = "";
                ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text = "";
                ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text = "";
                ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text = "";
                ((TextBox)grdCreditinfo.Rows[i].FindControl("txtAmount")).Text = "";

            }

        }
        #endregion ClearAll

        #region Calculation
        protected void btnCalculate_Click(object sender, EventArgs e)
        {

            int incr = 0;
            int cnt = 0;
            try
            {
                //int temp = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);
                //if(temp!=)
                //{
                //} 


                for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
                {
                    int BankAmt = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);

                    int str1 = Convert.ToInt32(grdCreditinfo.Rows[i].Cells[5].Text.ToString());


                    if (BankAmt < str1)
                    {
                        lblStatus.Text = "Bank Gurantee Amount Cannot Be Less Than Amount";
                        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text = "";
                    }
                    else
                    {
                        int TotalAmt = str1;
                        incr = incr + TotalAmt;
                        int Amt = Convert.ToInt32(((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text);
                        cnt = cnt + Amt;
                    }
                }
                txtcreditAmt.Text = Convert.ToString(cnt);
                txtinvoice.Text = Convert.ToString(incr);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Please Insesrt Numeric Value in Amount Block ";
                ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text = "";
            }
            try
            {
                int credit = Convert.ToInt32(txtcreditAmt.Text.Trim());
                int invoice = Convert.ToInt32(txtinvoice.Text.Trim());
                int Total;
                if (credit < invoice)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Check()", true);
                    lblStatus.Text = "Credit Value Does Not Less Than Invoice Value";
                    txtinvoice.Text = "";
                    txtcreditremain.Text = "";
                    txtcreditremain.Text = "";
                }
                else
                {
                    Total = credit - invoice;
                    (txtcreditremain.Text) = Convert.ToString(Total);
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Calculation
        #region City Allocation
        protected void ddlcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string countrycode;
            try
            {

                TXTSubLevel.Text = ddlcity.SelectedItem.Text;
                if (ddlcity.SelectedIndex == 0)
                {
                    ds = da.SelectRecords("sp_getcountrycodefromairport", "code", Session["Station"].ToString(), SqlDbType.VarChar);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        countrycode = ds.Tables[0].Rows[0][0].ToString();
                        ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(ddlcountry.Items.FindByValue(countrycode));
                        ddlcountry_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    ds = da.SelectRecords("sp_getcountrycodefromairport", "code", ddlcity.SelectedValue, SqlDbType.VarChar);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        countrycode = ds.Tables[0].Rows[0][0].ToString();
                        ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(ddlcountry.Items.FindByValue(countrycode));
                        ddlcountry_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            
            //AWBAvilbleForAllocForCity();
        }
        #endregion City Allocation
        #region AddClick
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //Commented by Vijay - 23-08-2014
            //for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
            //{
            //    if (!((CheckBox)grdCreditinfo.Rows[i].FindControl("chkExpire")).Checked)
            //    {
            //        lblCreditStatus.Text = "Not allowed until all previous credits are expired.";
            //        return;
            //    }
            //}

            SaveCreditInfo();
            AddNewRowToGrid();
            string value = "ADD";
            MasterLog(value);

            //for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
            //{
            //    ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).Items.Add("Credit");
            //    ((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).Items.Add("Cash");
            //    //LoadBank();
            //}

            disable();
            //LoadBank();
        }
        #endregion AddClick
       
        #region TrasactionChange cash/Credit
        //protected void ddltransaction_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try 
        //    {
        //        if (ddltransaction.SelectedItem.Text == "Cash")
        //        {
        //            ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankname")).Enabled = false ;
        //            ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankgurantee")).Enabled = false;
        //        }
        //        else
        //        {
        //            ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankname")).Enabled = true ;
        //            ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankgurantee")).Enabled = true ;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
            
        //    }

        //}
        #endregion TrasactionChange cash/Credit
        #region Gettransaction
        public void GetTransaction(object sender, EventArgs e)
        {


            try
            {
               
                
                DataTable dtCreditInfo = (DataTable)Session["dtCreditInfo"];
                int checkprev = dtCreditInfo.Rows.Count;   

                for (int i = 0; i < grdCreditinfo.Rows.Count; i++)
                {
                    
                    if (i > checkprev)
                    {
                        if (((DropDownList)grdCreditinfo.Rows[i].FindControl("ddltransaction")).SelectedItem.Text == "Cash")
                        {
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Enabled = false;
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Enabled = false;

                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtAmount")).Text = "";
                            return;
                        }
                        else
                        {
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Enabled = true;
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Enabled = true;

                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankname")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankgurantee")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimit")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txttresholdlimitdays")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtbankguranteeamt")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtsatrtdate")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtvalidto")).Text = "";
                            ((TextBox)grdCreditinfo.Rows[i].FindControl("txtAmount")).Text = "";
                        }
                    }
                    if (((DropDownList)grdCreditinfo.Rows[checkprev-1].FindControl("ddltransaction")).SelectedIndex != -1)
                        {
                       
                            if (((DropDownList)grdCreditinfo.Rows[checkprev-1].FindControl("ddltransaction")).SelectedItem.Text == "Cash")
                            {
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankname")).Enabled = false;
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankgurantee")).Enabled = false;

                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankname")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankgurantee")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txttresholdlimit")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txttresholdlimitdays")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankguranteeamt")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtsatrtdate")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtvalidto")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtAmount")).Text = "";

                            }
                            else
                            {
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankname")).Enabled = true;
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankgurantee")).Enabled = true;

                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankname")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankgurantee")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txttresholdlimit")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txttresholdlimitdays")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtbankguranteeamt")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtsatrtdate")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtvalidto")).Text = "";
                                ((TextBox)grdCreditinfo.Rows[checkprev-1].FindControl("txtAmount")).Text = "";
                            }
                        

                    }

                //    if (((DropDownList)grdCreditinfo.Rows[0].FindControl("ddltransaction")).SelectedItem.Text == "Cash")
                //    {
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankname")).Enabled = false;
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankgurantee")).Enabled = false;

                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankname")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankgurantee")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txttresholdlimit")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtsatrtdate")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtvalidto")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtAmount")).Text = "";

                //    }
                //    else
                //    {
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankname")).Enabled = true;
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankgurantee")).Enabled = true;

                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankname")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankgurantee")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txttresholdlimit")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtbankguranteeamt")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtsatrtdate")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtvalidto")).Text = "";
                //        ((TextBox)grdCreditinfo.Rows[0].FindControl("txtAmount")).Text = "";
                //    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Gettransaction
        #region Controlling locator Change
        protected void ddlcontrolinglocator_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            try
            {
                if (ddlcontrolinglocator.SelectedIndex  == 0)
                {
                    ddlbuildto.SelectedIndex  = 0;

                    grdCreditinfo.Enabled = true;
                    return;
                }
             if(ddlcontrolinglocator.SelectedItem.Text.ToUpper()=="YES")
             {
                 ddlbuildto.SelectedIndex = 1;
                 txtlocatorcode.Enabled = false;
                 txtlocatorcode.Text = "";
                 grdCreditinfo.Enabled = true;
                 grdDeals.Enabled = true;
                 txtCreditdays.Enabled = true;
                 imgcheck.Visible = false;
                 imgcross.Visible = false;
                 return; 
             }
             else
             {
                 grdCreditinfo.Enabled = false;   
                 ddlbuildto.SelectedIndex=2;
                 txtlocatorcode.Enabled = true;
                 grdDeals.Enabled = false;
                 txtCreditdays.Enabled = false;
                 imgcheck.Visible = false;
                 imgcross.Visible = false;
                 return;
             }
            }
            catch (Exception ex)
            { }
        }
        #endregion Controlling locator Change
        #region LOcatorCode
        protected void txtlocatorcode_TextChanged(object sender, EventArgs e)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] paramname = new string[1];
                paramname[0] = "ControllingLocatorCode";

                object[] paramvalue = new object[1];
                paramvalue[0] = txtlocatorcode.Text;

                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.NVarChar;

                DataSet ds = db.SelectRecords("Sp_ValidateControlingLocator", paramname, paramvalue, paramtype);

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblStatus.Text = "Controlling Locator is Valid";
                            lblStatus.ForeColor = Color.Green;
                            imgcheck.Visible = true;

                            imgcross.Visible = false;   

                        }
                        else
                        {
                           lblStatus.Text = "Controlling Locator does not exists";
                            lblStatus.ForeColor = Color.Red;
                            imgcross.Visible = true;
                            imgcheck.Visible = false;   
                        }
                    }
                }
                //    lblStatus.Text = "Controlling Locator is Valid";
                //    lblStatus.ForeColor = Color.Green;
                //}
                //else
                //{
                //    lblStatus.Text = "Controlling Locator does not exists";
                //    lblStatus.ForeColor = Color.Blue;

                //}
            }
            catch (Exception ex)
            {
            
            }

        }
        #endregion LOcatorCode
        #region Rates: LoadGRDRates

        public void LoadGRDRates()
        {
            try
            {

                DataSet dsRates = new DataSet();
                dsRates = objBLL.GetAgentRates(TXTAgentCode.Text);

                for (int i = 0; i < dsRates.Tables[0].Rows.Count; i++)
                {
                    if (dsRates.Tables[0].Rows[i]["OriginLevel"].ToString() == "0")
                    {
                        dsRates.Tables[0].Rows[i]["OriginLevel"] = "Airport";
                    }
                    if (dsRates.Tables[0].Rows[i]["OriginLevel"].ToString() == "1")
                    {
                        dsRates.Tables[0].Rows[i]["OriginLevel"] = "City";
                    }
                    if (dsRates.Tables[0].Rows[i]["OriginLevel"].ToString() == "2")
                    {
                        dsRates.Tables[0].Rows[i]["OriginLevel"] = "Region";
                    }
                    if (dsRates.Tables[0].Rows[i]["OriginLevel"].ToString() == "3")
                    {
                        dsRates.Tables[0].Rows[i]["OriginLevel"] = "Country";
                    }


                    if (dsRates.Tables[0].Rows[i]["DestinationLevel"].ToString() == "0")
                    {
                        dsRates.Tables[0].Rows[i]["DestinationLevel"] = "Airport";
                    }
                    if (dsRates.Tables[0].Rows[i]["DestinationLevel"].ToString() == "1")
                    {
                        dsRates.Tables[0].Rows[i]["DestinationLevel"] = "City";
                    }
                    if (dsRates.Tables[0].Rows[i]["DestinationLevel"].ToString() == "2")
                    {
                        dsRates.Tables[0].Rows[i]["DestinationLevel"] = "Region";
                    }
                    if (dsRates.Tables[0].Rows[i]["DestinationLevel"].ToString() == "3")
                    {
                        dsRates.Tables[0].Rows[i]["DestinationLevel"] = "Country";
                    }

                }
                GRDGeneralRates.DataSource = dsRates.Tables[0].Copy();
                GRDGeneralRates.DataBind();


                for (int i = 0; i < dsRates.Tables[1].Rows.Count; i++)
                {
                    if (dsRates.Tables[1].Rows[i]["OriginLevel"].ToString() == "0")
                    {
                        dsRates.Tables[1].Rows[i]["OriginLevel"] = "Airport";
                    }
                    if (dsRates.Tables[1].Rows[i]["OriginLevel"].ToString() == "1")
                    {
                        dsRates.Tables[1].Rows[i]["OriginLevel"] = "City";
                    }
                    if (dsRates.Tables[1].Rows[i]["OriginLevel"].ToString() == "2")
                    {
                        dsRates.Tables[1].Rows[i]["OriginLevel"] = "Region";
                    }
                    if (dsRates.Tables[1].Rows[i]["OriginLevel"].ToString() == "3")
                    {
                        dsRates.Tables[1].Rows[i]["OriginLevel"] = "Country";
                    }


                    if (dsRates.Tables[1].Rows[i]["DestinationLevel"].ToString() == "0")
                    {
                        dsRates.Tables[1].Rows[i]["DestinationLevel"] = "Airport";
                    }
                    if (dsRates.Tables[1].Rows[i]["DestinationLevel"].ToString() == "1")
                    {
                        dsRates.Tables[1].Rows[i]["DestinationLevel"] = "City";
                    }
                    if (dsRates.Tables[1].Rows[i]["DestinationLevel"].ToString() == "2")
                    {
                        dsRates.Tables[1].Rows[i]["DestinationLevel"] = "Region";
                    }
                    if (dsRates.Tables[1].Rows[i]["DestinationLevel"].ToString() == "3")
                    {
                        dsRates.Tables[1].Rows[i]["DestinationLevel"] = "Country";
                    }

                }


                PanelSpecificRates.Visible = true;
                    GRDSpecificRates.DataSource = dsRates.Tables[1].Copy();
                    GRDSpecificRates.DataBind();

                    if (dsRates.Tables[1].Rows.Count <= 0)
                        PanelSpecificRates.Visible = false;

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
            }
        }

        #endregion
        #region grid row commmand "View"
        protected void GRDGeneralRates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GRDGeneralRates.Rows[index];
            string RateCardId = ((HiddenField)row.FindControl("HidSerialNumber")).Value;
            Response.Redirect("MaintainRates.aspx?cmd=View&RCName=" + RateCardId);
            
        }
        #endregion grid row commmand "View"
        #region grid row commmand "View"
        protected void GRDSpecificRates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GRDSpecificRates.Rows[index];
            string RateCardId = ((HiddenField)row.FindControl("HidSerialNumber")).Value;
            Response.Redirect("MaintainRates.aspx?cmd=" + e.CommandName + "&RCName=" + RateCardId+"&AgentCode="+TXTAgentCode.Text);
        }
        #endregion grid row commmand "View"
        
        #region DealTabOperations
        
        protected void ChangeGridDealHeader(object sender, EventArgs e)
        {
           if(ddlType.Text.ToString()=="Tonnage")
           {
            grdDeals.Columns[0].HeaderText = ddlType.Text.ToString() + " From (Kg.)";
            grdDeals.Columns[1].HeaderText = ddlType.Text.ToString() + " To (Kg.)";

           }
           if (ddlType.Text.ToString() == "Revenue")
           {
               grdDeals.Columns[0].HeaderText = ddlType.Text.ToString() + " From (INR)";
               grdDeals.Columns[1].HeaderText = ddlType.Text.ToString() + " To (INR)";
           }
           grdDeals.DataSource = (DataTable)Session["CurrentTableDeals"];
            grdDeals.DataBind();
        }

        protected void ChangeGridDealItem(object sender, EventArgs e)
        {
            for (int r = 0; r < grdDeals.Rows.Count; r++)
            {
                if (((TextBox)grdDeals.Rows[r].FindControl("txtDlPercent")).Text.ToString() == "" && ((TextBox)grdDeals.Rows[r].FindControl("txtDlValue")).Text.ToString() == "")
                {
                    ((TextBox)grdDeals.Rows[r].FindControl("txtDlPercent")).Enabled = true;
                    ((TextBox)grdDeals.Rows[r].FindControl("txtDlValue")).Enabled = true;
                }
                else
                {
                    if (((TextBox)grdDeals.Rows[r].FindControl("txtDlPercent")).Text.ToString() != "" && ((TextBox)grdDeals.Rows[r].FindControl("txtDlValue")).Text.ToString() == "")
                    {
                        ((TextBox)grdDeals.Rows[r].FindControl("txtDlPercent")).Enabled = true;
                        ((TextBox)grdDeals.Rows[r].FindControl("txtDlValue")).Enabled = false;
                    }
                    if (((TextBox)grdDeals.Rows[r].FindControl("txtDlValue")).Text.ToString() != "" && ((TextBox)grdDeals.Rows[r].FindControl("txtDlPercent")).Text.ToString() == "")
                    {
                        ((TextBox)grdDeals.Rows[r].FindControl("txtDlPercent")).Enabled = false;
                        ((TextBox)grdDeals.Rows[r].FindControl("txtDlValue")).Enabled = true;
                    }
                }
            }
        }
       
        protected void btnDealAdd_Click(object sender, EventArgs e)
        {
            LoadDealsGrid();
        }

        protected void ClearDeals()
        {
            grdDeals.DataSource = "";
            grdDeals.DataBind();
            txtlblDealDateFrom.Text = txtlblDealDateTo.Text = "";
            ddlType.SelectedIndex = 0;

        }
        
        #region Check if agent Exists
        protected void FillGridDealHistory(object sender, EventArgs e)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            lblStatus.Text = string.Empty;
            try
            {
                string[] paramname = new string[1];
                paramname[0] = "AgentCode";

                object[] paramvalue = new object[1];
                paramvalue[0] = TXTAgentCode.Text.Trim();

                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.NVarChar;

                DataSet ds = db.SelectRecords("SP_ValidateAgent", paramname, paramvalue, paramtype);

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblStatus.Text = "Agent code already exist";
                            lblStatus.ForeColor = Color.Red;
                            TXTNewAgentName.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
                            return;
                        }
                        else
                        {
                            lblStatus.Text = "Agent Code does not exists";
                            lblStatus.ForeColor = Color.Green;
                            txtIATAcode.Text = txtcustomercode.Text = TXTAgentCode.Text.Trim();

                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            //try
            //{
            //    SQLServer db = new SQLServer(Global.GetConnectionString());
            //    string[] paramname = new string[1];
            //     paramname[0] = "AgentCode";
            //    object[] paramvalue = new object[1];
            //    paramvalue[0] = TXTAgentCode.Text;
            //    SqlDbType[] paramtype = new SqlDbType[1];
            //    paramtype[0] = SqlDbType.VarChar;
            //    DataSet ds = db.SelectRecords("SpGetAgentDealsDetails",paramname, paramvalue,  paramtype);

            //    if (ds != null)
            //    {
            //        if (ds.Tables.Count > 0)
            //        {
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {

            //                grdDealsHistory.DataSource = ds.Tables[0];
            //                grdDealsHistory.DataBind();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{ }

        }
        #endregion 

        #endregion

        //shashi.....
        # region GetCurrencyCode List shashi

        private void GetCurrencyCode()
        {
            try
            {
                DataSet ds = objBLL.GetCurrencyCodeList(ddlCurrencyCode.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlCurrencyCode.DataSource = ds;
                            ddlCurrencyCode.DataMember = ds.Tables[0].TableName;
                            ddlCurrencyCode.DataTextField = ds.Tables[0].Columns["ID"].ColumnName;
                            ddlCurrencyCode.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;
                            ddlCurrencyCode.DataBind();
                            ddlCurrencyCode.Items.Insert(0, new ListItem("Select", "0"));
                            ddlCurrencyCode.SelectedIndex = -1;


                            //ddlCurrencyCode.DataSource = ds;
                            //ddlCurrencyCode.DataMember = ds.Tables[0].TableName;
                            //ddlCurrencyCode.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            //ddlCurrencyCode.DataTextField = ds.Tables[0].Columns["Code"].ColumnName;
                            //ddlCurrencyCode.DataBind();
                            //ddlCurrencyCode.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetCurrencyCode List shashi

        //Jayant
        #region GetRatePreference
        public void GetRatePreference()
        {
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                
                DataSet dsRatePref = db.SelectRecords("Sp_GetRatePreference");

                if (dsRatePref != null)
                {
                    if (dsRatePref.Tables.Count > 0)
                    {
                        if (dsRatePref.Tables[0].Rows.Count > 0)
                        {

                            ddlRatelinePreference.DataSource = dsRatePref;
                            ddlRatelinePreference.DataMember = dsRatePref.Tables[0].TableName;
                            ddlRatelinePreference.DataValueField = dsRatePref.Tables[0].Columns["RatePref"].ColumnName;

                            ddlRatelinePreference.DataTextField = dsRatePref.Tables[0].Columns["RatePref"].ColumnName;
                            ddlRatelinePreference.DataBind();
                            ddlRatelinePreference.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion GetRatePreference
        #region GetPaymentMode
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

                            ddlDefaultPayMode.DataSource = dsPayMode;
                            ddlDefaultPayMode.DataMember = dsPayMode.Tables[0].TableName;
                            ddlDefaultPayMode.DataValueField = dsPayMode.Tables[0].Columns["PayModeText"].ColumnName;

                            ddlDefaultPayMode.DataTextField = dsPayMode.Tables[0].Columns["PayModeText"].ColumnName;
                            ddlDefaultPayMode.DataBind();
                            //ddlDefaultPayMode.Items.Insert(0, "Select");
                        }
                    }
                }
            }

            catch (Exception exObj)
            {

            }
        }
        #endregion GetPaymentMode


        #region Load DropDowns
        public void LoadGLDropDown()
        {
            try
            {
                DataSet ds = da.SelectRecords("SP_GetGLAccountCode");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtAccountCode.DataSource = ds;
                            txtAccountCode.DataMember = ds.Tables[0].TableName;
                            txtAccountCode.DataTextField = ds.Tables[0].Columns["GLActCodeDesc"].ColumnName;
                            txtAccountCode.DataValueField = ds.Tables[0].Columns["GLAccountCode"].ColumnName;
                            txtAccountCode.DataBind();
                            txtAccountCode.Items.Insert(0,new ListItem("Select", "0"));
                            txtAccountCode.SelectedIndex = -1;

                            //txtAccountCode.DataSource = ds;
                            //txtAccountCode.DataTextField = "GLAccountCode";
                            //txtAccountCode.DataValueField = "GLAccountCode";
                            //txtAccountCode.DataBind();
                            //txtAccountCode.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Drop Down Country 
        protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            try
            {
                GetAllStationsForCountry(ddlcountry.SelectedValue);
            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion

        #region GetAllStationsForCountry
        protected void GetAllStationsForCountry(string CountryCode)
        {
            DataSet ds = null;
            try
            {
                if (ddlcountry.SelectedIndex == 0)
                {
                    chkListStation.Items.Clear();
                    return;
                }

                //string Cnt = ddlcountry.SelectedItem.Text;
                //country = ddlcountry.SelectedItem.Value.ToString();
                //country = country.Replace(Cnt, " ");
                //country = country.Trim(' ');

                //string CountryCode = ddlcountry.SelectedValue.ToString();

                ds = objUserBAL.GetAllStationsForCountry(CountryCode);
                if (ds != null)
                {
                    chkListStation.DataSource = ds;
                    chkListStation.DataMember = ds.Tables[0].TableName;
                    chkListStation.DataTextField = ds.Tables[0].Columns["AirportName"].ColumnName;
                    chkListStation.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                    chkListStation.DataBind();
                    //chkListStation.Items.Insert(0, new ListItem("Select", "0"));
                    chkListStation.SelectedIndex = -1;
                    ds.Dispose();
                    chkSelectAll.Checked = false;
                }
            }
            catch (Exception ex)
            {
              
            }

            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion

        #region Get Station
        protected string GetStnList()
        {
            string stnList = string.Empty;
            for (int i = 0; i < chkListStation.Items.Count; i++)
            {
                if (chkSelectAll.Checked)
                {
                    if (stnList == "")
                        stnList = chkListStation.Items[i].Value;
                    else
                        stnList = stnList + "," + chkListStation.Items[i].Value;
                }
                else
                {
                    if (chkListStation.Items[i].Selected == true)
                    {
                        if (stnList == "")
                            stnList = chkListStation.Items[i].Value;
                        else
                            stnList = stnList + "," + chkListStation.Items[i].Value;
                    }
                }
            }
            return stnList;
        }
        #endregion

        protected void imgAgentPopUp_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                lblAgentPopUpError.Text = string.Empty;

                txtAppFrm.Text = txtAppTo.Text = string.Empty;
                txtTaxPerc.Text = string.Empty;

                chkAll.Checked = false;
                chkCommission.Checked = false;
                chkDiscount.Checked = false;
                chkIATAChrg.Checked = false;
                chkMKTChrg.Checked = false;
                chkServTax.Checked = false;

                txtTaxIdentifier.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit_EAWBPopUp();</script>", false);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSaveAgentChrg_Click(object sender, EventArgs e)
        {
            try 
            {
                if (TXTAgentCode.Text.Length > 0) 
                {
                    string Appliedon = "N";
                    if (chkAll.Checked)
                    {
                        Appliedon = "A";
                    }
                    else 
                    {
                        
                        if (chkIATAChrg.Checked)
                            Appliedon = Appliedon + ",I";
                        if (chkMKTChrg.Checked)
                            Appliedon = Appliedon + ",M";
                        if (chkServTax.Checked)
                            Appliedon = Appliedon + ",T";
                        if (chkCommission.Checked)
                            Appliedon = Appliedon + ",C";
                        if (chkDiscount.Checked)
                            Appliedon = Appliedon + ",D";
                        Appliedon = Appliedon.Replace('N',',').Trim(',');
                    }
                    if (Appliedon.Length<1) 
                    {
                        return;
                    }
                    string[] PName = new string[] 
                    {
                        "AgentCode",
                        "Taxidentifier",
                        "TaxPer",
                        "FromDt",
                        "ToDt",
                        "AppliedOn",
                        "User",
                        "Date"
                    };
                    object[] PValue = new object[]
                    {
                        TXTAgentCode.Text,
                        txtTaxIdentifier.Text,
                        Convert.ToDecimal(txtTaxPerc.Text),
                        txtAppFrm.Text,
                        txtAppTo.Text,
                        Appliedon.Trim(','),
                        Session["UserName"].ToString(),
                        Convert.ToDateTime(Session["IT"].ToString())
                    };
                    SqlDbType[] PType = new SqlDbType[] 
                    {
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Decimal,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.DateTime
                    };

                    bool flag = da.InsertData("spConfigureAgentTaxes", PName, PType, PValue);
                }
            }
            catch (Exception ex) { }
        }

        protected bool ValidateOnSave()
        {
            string strfromdate = "";
            DateTime fromdt;
            string strtodate = "";
            DateTime todt;
            List<DateTime> frmList = new List<DateTime>();
            List<DateTime> toList = new List<DateTime>();
            for (int i = 0; i < grdTDSInfo.Rows.Count; i++)
            {
                if (((TextBox)grdTDSInfo.Rows[i].FindControl("txtFromDate")).Text == "" || ((TextBox)grdTDSInfo.Rows[i].FindControl("txtToDate")).Text == "")
                {
                    lblStatus.Text = "Enter From Date and To Date to save TDS on Commission";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                if (((TextBox)grdTDSInfo.Rows[i].FindControl("txtTDSonCommPerc")).Text == "")
                {
                    lblStatus.Text = "Enter TDS on Commission percent";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                try
                {
                    string day = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtFromDate")).Text.Substring(0, 2);
                    string mon = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtFromDate")).Text.Substring(3, 2);
                    string yr = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtFromDate")).Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    fromdt = Convert.ToDateTime(strfromdate);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid From Date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                try
                {
                    string day = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtToDate")).Text.Substring(0, 2);
                    string mon = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtToDate")).Text.Substring(3, 2);
                    string yr = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtToDate")).Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    todt = Convert.ToDateTime(strtodate);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid To Date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                try
                {
                    float tdsPercent = float.Parse(((TextBox)grdTDSInfo.Rows[i].FindControl("txtTDSonCommPerc")).Text);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Please enter valid TDS on Commission percent";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (fromdt > todt)
                {
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                frmList.Add(fromdt);
                toList.Add(todt);
            }

            frmList.Sort();
            toList.Sort();

            for (int cnt = 1; cnt < frmList.Count; cnt++)
            {
                if ((frmList[cnt] - toList[cnt - 1]).TotalDays > 1)
                {
                    lblStatus.Text = "Date range missing in TDS on Commission information";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }

            return true;
        }

        protected void btnAddTDS_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTDS = (DataTable)Session["dtTDSInfo"];

                //Code to add value of rows which are already available in grid
                if (dtTDS.Rows.Count > 0)
                {
                    string srno = "", fromdate = "", todate = "", percent = "", status = "";
                    for (int i = 0; i < grdTDSInfo.Rows.Count; i++)
                    {
                        srno = ((Label)grdTDSInfo.Rows[i].FindControl("lblSrNo")).Text;
                        fromdate = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtFromDate")).Text;
                        todate = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtTodate")).Text;
                        percent = ((TextBox)grdTDSInfo.Rows[i].FindControl("txtTDSonCommPerc")).Text;
                        status = ((Label)grdTDSInfo.Rows[i].FindControl("lblStatus")).Text;

                        dtTDS.Rows[i]["SrNo"] = srno;
                        dtTDS.Rows[i]["FromDate"] = fromdate;
                        dtTDS.Rows[i]["ToDate"] = todate;
                        dtTDS.Rows[i]["TDSOnCommPerc"] = percent;
                        dtTDS.Rows[i]["Status"] = status;
                    }
                }

                DataRow dr;
                dr = dtTDS.NewRow();
                dr["SrNo"] = 0;
                dr["FromDate"] = "";
                dr["ToDate"] = "";
                dr["TDSOnCommPerc"] = 0;
                dr["Status"] = "1";

                dtTDS.Rows.Add(dr);
                ViewState["vsTDSInfo"] = dtTDS;

                grdTDSInfo.DataSource = null;
                grdTDSInfo.DataSource = dtTDS;
                grdTDSInfo.DataBind();

                Session["dtTDSInfo"] = dtTDS.Copy();

                //to disable TDS rows
                for (int i = 0; i < grdTDSInfo.Rows.Count; i++)
                {
                    if (((Label)grdTDSInfo.Rows[i].FindControl("lblStatus")).Text == "0")
                    {
                        grdTDSInfo.Rows[i].Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        #region Master Audit Log
        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;

            //1

            Paramsmaster.SetValue("Agent Master", count);
            count++;

            //2

            Paramsmaster.SetValue(TXTNewAgentName.Text, count);
            count++;

            //3

            Paramsmaster.SetValue(value, count);
            count++;

            //4

            Paramsmaster.SetValue("", count);
            count++;


            //5

            Paramsmaster.SetValue("", count);
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

        }
        #endregion 

        protected void btnDeleteTDS_Click(object sender, EventArgs e)
        {
            string Srno;
            try
            {
                DataTable dt = (DataTable)Session["dtTDSInfo"];
                DataTable dt1 = dt.Copy();
                string value = "DELETE";
                MasterLog(value);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (((CheckBox)grdTDSInfo.Rows[i].FindControl("chkTDS")).Checked == true)
                    {
                        ((CheckBox)grdTDSInfo.Rows[i].FindControl("chkTDS")).Checked = false;
                        Srno = ((Label)grdTDSInfo.Rows[i].FindControl("lblSrNo")).Text;

                        if (dt1.Rows.Count > 0)
                        {
                            for (int t = 0; t < dt1.Rows.Count; t++)
                            {
                                if (dt1.Rows[t][0].ToString() == Srno)
                                {
                                    dt1.Rows[t].Delete();
                                }
                            }
                            dt1.AcceptChanges();
                        }

                    }
                }

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    Session["dtTDSInfo"] = dt1;
                }

                grdTDSInfo.DataSource = "";

                grdTDSInfo.DataSource = (DataTable)Session["dtTDSInfo"];
                grdTDSInfo.DataBind();

                //to disable TDS rows
                for (int i = 0; i < grdTDSInfo.Rows.Count; i++)
                {
                    if (((Label)grdTDSInfo.Rows[i].FindControl("lblStatus")).Text == "0")
                    {
                        grdTDSInfo.Rows[i].Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnListAgent_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            lblStatus.Text = string.Empty;
            try
            {
                if (TXTAgentCode.Text.Trim() != "")
                {
                    #region Fill Agent Details if Agent Exists
                    string[] pname = { "AgentCode" };
                    object[] pvalue = { TXTAgentCode.Text.Trim() };
                    SqlDbType[] ptype = { SqlDbType.VarChar };
                    DataSet dsNewAgentDetails = da.SelectRecords("sp_getAgentInfo", pname, pvalue, ptype);
                    if (dsNewAgentDetails != null && dsNewAgentDetails.Tables[0].Rows.Count > 0)
                    {
                        fillCreditDetails(dsNewAgentDetails);
                        BtnAllSave.Text = "Update";
                    }
                    else
                    {
                        lblStatus.Text = "Agent does not exist";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    #endregion
                }
                else
                {
                    lblStatus.Text = "Enter agent Code";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex) { }

        }
    }
}