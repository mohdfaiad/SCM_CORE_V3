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

namespace ProjectSmartCargoManager
{
    public partial class UCRPopup : System.Web.UI.Page
    {
        #region Variables
        BALUCR objBAL = new BALUCR();
        clsFillCombo clsCombo;
        DataTable SupInDetails = new DataTable("UCRPUP_DT1");
        DataTable dtUCRULD = new DataTable("UCRPUP_DT2");

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                clsCombo = new clsFillCombo();
                if (!IsPostBack)
                {
                    Session["NEWUCR"] = null;
                    Session["SdtUCRULD"] = null;
                    if (Request.QueryString["type"] == "List")
                    {
                        SearchEdit.Visible = true;
                        New.Visible = false;
                        lblSError.Visible = false;
                        btnSave.Visible = false;
                        btnPrint.Visible = false;
                        btnCancel.Visible = false;
                        btnClear.Visible = false;
                        clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpSTraWH);
                        clsCombo.FillAllComboBoxes("tblTraCarrier", "SELECT", drpSTraCar);
                        clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpSFinWH);
                        clsCombo.FillAllComboBoxes("tblRecCarrier", "SELECT", drpSRecCar);

                        // Set Dates
                        txtSfrmDate.Attributes.Add("readonly", "readonly");
                        txtSfrmDate.Text = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
                        txtSToDate.Attributes.Add("readonly", "readonly");
                        txtSToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    else if (Request.QueryString["type"] == "New")
                    {
                        SearchEdit.Visible = false;
                        New.Visible = true;
                        lblError.Visible = false;
                        NEWTitle.InnerHtml = "NEW UCR";

                        // Make All Buttons visible
                        btnSave.Visible = true;
                        btnPrint.Visible = true;
                        btnCancel.Visible = true;
                        btnClear.Visible = true;

                        // Fill All Combo Box
                        clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpTraWH);
                        clsCombo.FillAllComboBoxes("tblSubWHMaster", "SELECT", drpTraSubWH);
                        clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpFinWH);
                        clsCombo.FillAllComboBoxes("tblSubWHMaster", "SELECT", drpFinSubWH);
                        //clsCombo.FillAllComboBoxes("tblRecCarrier", "SELECT", drpRecCar);
                        //clsCombo.FillAllComboBoxes("tblTraCarrier", "SELECT", drpTraCar);
                        //clsCombo.FillAllComboBoxes("AgentMaster", "SELECT", drpRecCarAgent);
                        //clsCombo.FillAllComboBoxes("AgentMaster", "SELECT", drpTraCarAgent);


                        // Get Dates
                        txtTraDt.Attributes.Add("readonly", "readonly");
                        txtTraDt.Text = Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy");
                        txtTraCarDt.Attributes.Add("readonly", "readonly");
                        txtTraCarDt.Text = Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy");
                        txtRecCarDt.Attributes.Add("readonly", "readonly");
                        txtRecCarDt.Text = Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy");
                        if (GetDetailsUCRULD(txtUCR.Text) == null)
                        {
                            lblError.Text = lblError.Text + "| Error in Listing ULD..";
                        }

                        if (Request.QueryString["Mode"] == null || Request.QueryString["Mode"].ToString() == "")
                        {

                        }
                        else
                        {
                            if (Request.QueryString["Mode"].ToString() == "A")
                            {
                                BALUCR objBAL = new BALUCR();
                                string AWBNo = Request.QueryString["AWBNo"].ToString();
                                if (Request.QueryString["UCRNo"].ToString() == "NO")
                                {
                                    DataSet dsULDInfo = new DataSet("UCRPUP_DS1");
                                    dsULDInfo = objBAL.GetULDInfo(AWBNo, "A", "", DateTime.Now.ToString());
                                    if (dsULDInfo.Tables[0].Rows.Count > 0)
                                    {
                                        txtAWBPrefix.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[0]["AWBPrefix"]);
                                        txtAWBNo.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[0]["AWBNumber"]);
                                        drpTraWH.SelectedIndex = drpTraWH.Items.IndexOf(drpTraWH.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["FlightOrigin"].ToString()));
                                        drpFinWH.SelectedIndex = drpFinWH.Items.IndexOf(drpFinWH.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["FlightDestination"].ToString()));
                                        if (dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString() == null || dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString() == null)
                                            // drpTraCar.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString()));
                                            txtTraPar.Text = dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString();
                                        else
                                            txtTraPar.Text = dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString();

                                        //drpTraCarAgent.SelectedIndex = drpTraCarAgent.Items.IndexOf(drpTraCarAgent.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString()));
                                        //if (dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString() == null || dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString() == null)
                                        //drpRecCar.SelectedIndex = drpRecCar.Items.IndexOf(drpRecCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString()));
                                        txtRecPar.Text = dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString();
                                        //else
                                        //drpRecCarAgent.SelectedIndex = drpRecCarAgent.Items.IndexOf(drpRecCarAgent.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString()));

                                        //drpRecCar.Enabled = false;
                                        //drpRecCarAgent.Enabled = false;
                                        for (int i = 0; i < dsULDInfo.Tables[0].Rows.Count; i++)
                                        {
                                            TextBox txtULDNos = (TextBox)(gvULDDetails.FooterRow.FindControl("txtULDNo"));
                                            txtULDNos.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[i]["ULDNo"]);
                                            TextBox txtAWBPrefixTemp = (TextBox)(gvULDDetails.FooterRow.FindControl("txtAWBPrefix"));
                                            txtAWBPrefixTemp.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[i]["AWBPrefix"]);
                                            TextBox txtAWBNoTemp = (TextBox)(gvULDDetails.FooterRow.FindControl("txtAWBNo"));
                                            txtAWBNoTemp.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[i]["AWBNumber"]);
                                            CheckBox chkLoadedTemp = (CheckBox)(gvULDDetails.FooterRow.FindControl("chkLoaded"));
                                            //SaveULDNoinMaster(txtULDNos.Text,"0");
                                            if (txtAWBNoTemp.Text.Trim() == "")
                                            {
                                                chkLoadedTemp.Checked = false;
                                            }
                                            else
                                            {
                                                chkLoadedTemp.Checked = true;
                                            }
                                            btnADDULD_Click(null, null);
                                            chkIsLoaded.Checked = true;
                                        }
                                    }
                                    else
                                    {
                                        lblError.Visible = true;
                                        lblError.Text = "There are no ULDs based on this AWBNo";
                                        return;
                                    }
                                    //TextBox textULDNo = (TextBox)(gvULDDetails.FooterRow.FindControl("txtULDNo"));
                                }
                                else
                                {
                                    string UCRnoForList = Request.QueryString["UCRNo"].ToString();
                                    txtUCR.Text = UCRnoForList;
                                    btnSearch_Click(null, null);
                                    chkIsLoaded.Checked = true;
                                }
                            }

                            else if (Request.QueryString["Mode"].ToString() == "M")
                            {
                                BALUCR objBAL = new BALUCR();
                                string FlightNo = Request.QueryString["FlightNo"].ToString();
                                DateTime dtFlightDate = DateTime.Now;
                                if (Request.QueryString["pg"].ToString() == "Ctm")
                                {
                                    try
                                    {
                                        dtFlightDate = DateTime.ParseExact(Request.QueryString["FlightDate"].ToString(), "M/d/yyyy", null);
                                    }
                                    catch (Exception ex)
                                    {
                                        lblError.Visible = true;
                                        lblError.ForeColor = System.Drawing.Color.Red;
                                        lblError.Text = "Datetime not in correct format..";
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        dtFlightDate = DateTime.ParseExact(Request.QueryString["FlightDate"].ToString(), "dd/MM/yyyy", null);
                                    }
                                    catch (Exception ex)
                                    {
                                        lblError.Visible = true;
                                        lblError.ForeColor = System.Drawing.Color.Red;
                                        lblError.Text = "Datetime not in correct format..";
                                    }
                                }
                                DataSet dsULDInfo = new DataSet("UCRPUP_DS2");
                                dsULDInfo = objBAL.GetULDInfo("", "M", FlightNo, dtFlightDate.ToString());

                                if (dsULDInfo.Tables[0].Rows.Count > 0)
                                {
                                    txtAWBPrefix.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[0]["AWBPrefix"]);
                                    txtAWBNo.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[0]["AWBNumber"]);
                                    drpTraWH.SelectedIndex = drpTraWH.Items.IndexOf(drpTraWH.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["FlightOrigin"].ToString()));

                                    try
                                    {
                                        if (Request.QueryString["pg"].ToString() == "Arr")
                                        {
                                            //drpTraCar.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString()));
                                            txtTraPar.Text = dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString();
                                            //drpRecCar.SelectedIndex = drpRecCar.Items.IndexOf(drpRecCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString()));
                                            txtRecPar.Text = dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString();
                                            //drpTraCarAgent.Enabled = false;
                                            drpFinWH.SelectedIndex = drpFinWH.Items.IndexOf(drpFinWH.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["FlightDestination"].ToString()));
                                        }

                                        else if (Request.QueryString["pg"].ToString() == "Del")
                                        {
                                            //drpTraCar.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString()));
                                            txtTraPar.Text = dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString();
                                            //drpRecCar.SelectedIndex = drpRecCar.Items.IndexOf(drpRecCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString()));
                                            txtRecPar.Text = dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString();
                                            drpFinWH.SelectedIndex = 0;
                                        }
                                        else if (Request.QueryString["pg"].ToString() == "Ctm")
                                        {
                                            //drpTraCar.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString()));
                                            txtTraPar.Text = dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString();
                                            //drpRecCar.SelectedIndex = drpRecCar.Items.IndexOf(drpRecCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString()));
                                            txtRecPar.Text = dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString();
                                            drpFinWH.SelectedIndex = drpFinWH.Items.IndexOf(drpFinWH.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["FlightDestination"].ToString()));
                                        }
                                        //else if (Request.QueryString["pg"].ToString() == "Acc")
                                        //{
                                        //    if (dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString() == "" || dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString() == null)
                                        //    {
                                        //        drpTraCar.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AWBDesign"].ToString()));
                                        //        //drpTraCarAgent.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["Select"].ToString()));
                                        //    }
                                        //    else
                                        //    {
                                        //        //drpTraCar.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["Select"].ToString()));
                                        //        drpTraCarAgent.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["AgentCode"].ToString()));
                                        //    }
                                        //    drpRecCar.SelectedIndex = drpRecCar.Items.IndexOf(drpRecCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString()));
                                        //}
                                        else
                                        {
                                            // drpTraCar.SelectedIndex = drpTraCar.Items.IndexOf(drpTraCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString()));
                                            txtTraPar.Text = dsULDInfo.Tables[0].Rows[0]["ULDOwnerCode"].ToString();
                                            // drpRecCar.SelectedIndex = drpRecCar.Items.IndexOf(drpRecCar.Items.FindByText(dsULDInfo.Tables[0].Rows[0]["9W"].ToString()));
                                            txtRecPar.Text = dsULDInfo.Tables[0].Rows[0]["9W"].ToString();
                                        }
                                    }
                                    catch (Exception ex)
                                    { }


                                    //drpRecCar.Enabled = false;
                                    //drpRecCarAgent.Enabled = false;
                                    //drpTraCar.Enabled = false;
                                    //drpTraCarAgent.Enabled = false;
                                    for (int i = 0; i < dsULDInfo.Tables[0].Rows.Count; i++)
                                    {
                                        TextBox txtULDNos = (TextBox)(gvULDDetails.FooterRow.FindControl("txtULDNo"));
                                        txtULDNos.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[i]["ULDNo"]);
                                        TextBox txtAWBPrefixTemp = (TextBox)(gvULDDetails.FooterRow.FindControl("txtAWBPrefix"));
                                        txtAWBPrefixTemp.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[i]["AWBPrefix"]);
                                        TextBox txtAWBNoTemp = (TextBox)(gvULDDetails.FooterRow.FindControl("txtAWBNo"));
                                        txtAWBNoTemp.Text = Convert.ToString(dsULDInfo.Tables[0].Rows[i]["AWBNumber"]);
                                        CheckBox chkLoadedTemp = (CheckBox)(gvULDDetails.FooterRow.FindControl("chkLoaded"));
                                        if (txtAWBNoTemp.Text.Trim() == "")
                                        {
                                            chkLoadedTemp.Checked = false;
                                        }
                                        else
                                        {
                                            chkLoadedTemp.Checked = true;
                                        }
                                        btnADDULD_Click(null, null);
                                        chkIsLoaded.Checked = true;
                                    }

                                }
                                else
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "There are no ULDs based on this Flight";
                                    return;
                                }
                                //TextBox textULDNo = (TextBox)(gvULDDetails.FooterRow.FindControl("txtULDNo"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.ForeColor = Color.Red;
                lblError.Text = ex.Message;
            }
        }
        #endregion

        #region btnOpenclick Operations
        protected void btnOpenPic_Click(object sender, EventArgs e)
        {
            Session["ImageBytes"] = PhotoUpload.FileBytes;
            ImagePreview.ImageUrl = "~/ImageHandler.ashx";
        }
        #endregion

        #region ADD ULD button click
        public void btnADDULD_Click(object sender, EventArgs e)
        {
            // addgrid();
            if (!ADDDetailsUCRULD())
            {
                lblError.Text = "Error in Adding ULD Details.";
                return;
            }
        }
        #endregion

        #region Button Save Click
        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {

                if (!ValidateEntries())
                    return;

                // Execute SP 
                if (AddUCRDetails())
                {
                    AddULDDetailsToDatabase();
                    AddAccessoriesToDB();
                    btnSearch_Click(null, null);

                    lblError.Visible = true;
                    lblError.ForeColor = System.Drawing.Color.Green;
                    lblError.Text = "UCR Details Saved Successfully";
                }
                else
                {
                    lblError.Text = lblError + "| ERROR in creating UCR..";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error in UCR Updating:" + ex.Message;
            }


        }
        #endregion

        #region Validate Entries
        private bool ValidateEntries()
        {
            try
            {
                
                //Validate Transferring party and receiving party
                if (txtTraPar.Text == "")
                {
                    lblError.ForeColor = System.Drawing.Color.Red; 
                    lblError.Visible = true;
                    lblError.Text = "Please enter Transferring Party Code..";
                    txtTraPar.Focus();
                    return false;
                }

                if (txtRecPar.Text == "")
                {
                    lblError.ForeColor = System.Drawing.Color.Red; 
                    lblError.Visible = true;
                    lblError.Text = "Please enter Receiving Party Code..";
                    txtRecPar.Focus();
                    return false;
                }

                //Validate dates selected.
                DateTime dt = DateTime.Now;
                if (!DateTime.TryParse(txtTraDt.Text, out dt))
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                    lblError.Text = "Please select valid Transfer Date..";
                    txtTraDt.Focus();
                    return false;
                }
                if (!DateTime.TryParse(txtTraCarDt.Text, out dt))
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                    lblError.Text = "Please select valid Act Tra Date..";
                    txtTraCarDt.Focus();
                    return false;
                }
                if (!DateTime.TryParse(txtRecCarDt.Text, out dt))
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                    lblError.Text = "Please select valid Act Rec Date..";
                    txtRecCarDt.Focus();
                    return false;
                }


                //Validate Transfer/ Final Location
                if (drpTraWH.SelectedIndex <= 0 && drpFinWH.SelectedIndex <= 0)
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                    lblError.Text = "Please select either Transferring or Final Warehouse..";
                    return false;
                } 
                
                if (((Label)gvULDDetails.Rows[0].FindControl("lblULDNo")).Text == "DUMMY")
                {
                    lblError.ForeColor = System.Drawing.Color.Red;
                    lblError.Visible = true;
                    lblError.Text = "Please enter valid ULD Number(s) and click \"Add ULD\" button..";
                    ((Label)gvULDDetails.Rows[0].FindControl("lblULDNo")).Focus();
                    return false;
                }
                //if (drpFinWH.SelectedIndex <= 0)
                //{
                //    lblError.Visible = true;
                //    lblError.Text = "Please select Valid Final Warehouse";
                //    return false;
                //}
                return true;
            }
            catch (Exception)
            {
                lblError.Text = "Error occured. Please verify data entered and try again...";
                lblError.Visible = true;
                return (false);
            }
        }
        #endregion

        #region Execute SP Add UCR Details
        private bool AddUCRDetails()
        {
            DataSet dsUCRNo = new DataSet("UCRPUP_DS3");
            if (!chkIsLoaded.Checked)
            {
                txtAWBPrefix.Text = "";
                txtAWBNo.Text = "";
            }
            //string[] pname = new string[17]
            //{   
            //    "SPType",
            //    "UCRNo",
            //    "TraDt",
            //    "ActTraDt",
            //    "ActRecDt",
            //    "TraCar",
            //    "TraCarText",
            //    "RecCar",
            //    "RecCarText",
            //    "TraWH",
            //    "TraSubWHID",
            //    "FinWH",
            //    "FinSubWHID",
            //    "Remarks",
            //    "Picture",
            //    "isLoaded",
            //    "AWBNo"
            //};
            string TRACARDT = "", RECCARDT = "";
            if (txtTraCarDt.Text.Trim() == "")
            {
                TRACARDT = "1/1/1800 12:00:00 AM";
            }
            else
            {
                TRACARDT = txtTraCarDt.Text.Trim();
            }

            if (txtRecCarDt.Text.Trim() == "")
            {
                RECCARDT = "1/1/1800 12:00:00 AM";
            }
            else
            {
                RECCARDT = txtRecCarDt.Text.Trim();
            }

            //if ((drpRecCar.SelectedItem.Text.Trim().Substring(0, 2)) == "YY")
            //{
            //    RecCar = (Int32.Parse(drpRecCar.SelectedValue) - 100000).ToString();
            //}

            string tra = "";
            string car = "";
            //if (drpTraCar.SelectedIndex <= 0)
            //{
            //    tra = txt//drpTraCarAgent.SelectedItem.Text.Trim();
            //}
            //else
            //{
            //    tra = drpTraCar.SelectedItem.Text.Trim();
            //}
            //if (drpRecCar.SelectedIndex <=0)
            //{
            //    car = drpRecCarAgent.SelectedItem.Text.Trim();
            //}
            //else
            //{
            //    car = drpRecCar.SelectedItem.Text.Trim();
            //}
            //object[] pvalue = new object[17];
            tra = txtTraPar.Text.ToUpper();
            car = txtRecPar.Text.ToUpper();

            if ((byte[])Session["ImageBytes"] != null)
            {
                //pvalue = new object[17]
                //{
                //    "INSERT",
                //    txtUCR.Text,
                //    txtTraDt.Text,
                //    TRACARDT,
                //    RECCARDT,
                //    drpTraCar.SelectedValue.ToString(),
                //    drpTraCar.SelectedItem.Text,
                //    drpRecCar.SelectedValue.ToString(),
                //    RecCar,
                //    drpTraWH.SelectedItem.Text,
                //    Int64.Parse(drpTraSubWH.SelectedValue.ToString()),
                //    drpFinWH.SelectedItem.Text,
                //    Int64.Parse(drpFinSubWH.SelectedValue.ToString()),
                //    txtRemarks.Text,
                //    (byte[])Session["ImageBytes"],
                //    (bool)chkIsLoaded.Checked,
                //    txtAWBNo.Text
                //};
                if (Request.QueryString["Mode"] == null || Request.QueryString["Mode"].ToString() == "")
                {
                    
                    dsUCRNo = objBAL.AddUCRDetails("INSERT", txtUCR.Text.Trim(), txtTraDt.Text.Trim(), TRACARDT, 
                        RECCARDT, "0", tra, "0", car, drpTraWH.SelectedItem.Text, 
                        Int64.Parse(drpTraSubWH.SelectedValue.ToString()), drpFinWH.SelectedItem.Text,
                        Int64.Parse(drpFinSubWH.SelectedValue.ToString()), txtRemarks.Text.ToUpper(), 
                        (byte[])(Session["ImageBytes"]), (bool)chkIsLoaded.Checked, txtAWBNo.Text.Trim(),
                        txtAWBPrefix.Text.Trim(), "", Session["UserName"].ToString().ToUpper(), 
                        Convert.ToDateTime(Session["IT"].ToString())); //db.SelectRecords("spcreateUCR", pname, pvalue, ptype);drpTraCar.SelectedValue.ToString()
                }
                else
                {
                    if (Request.QueryString["Mode"].ToString() == "A")
                    {
                        dsUCRNo = objBAL.AddUCRDetails("INSERT", txtUCR.Text.Trim(), txtTraDt.Text.Trim(), TRACARDT,
                            RECCARDT, "0", tra, "0", car, drpTraWH.SelectedItem.Text, 
                            Int64.Parse(drpTraSubWH.SelectedValue.ToString()), drpFinWH.SelectedItem.Text,
                            Int64.Parse(drpFinSubWH.SelectedValue.ToString()), txtRemarks.Text.ToUpper(), 
                            (byte[])(Session["ImageBytes"]), (bool)chkIsLoaded.Checked, txtAWBNo.Text.Trim(),
                            txtAWBPrefix.Text.Trim(), "Ac", Session["UserName"].ToString().ToUpper(), 
                            Convert.ToDateTime(Session["IT"].ToString())); //db.SelectRecords("spcreateUCR", pname, pvalue, ptype);drpTraCar.SelectedValue.ToString()
                    }
                    else
                    {
                        dsUCRNo = objBAL.AddUCRDetails("INSERT", txtUCR.Text.Trim(), txtTraDt.Text.Trim(), TRACARDT, 
                            RECCARDT, "0", tra, "0", car, drpTraWH.SelectedItem.Text, 
                            Int64.Parse(drpTraSubWH.SelectedValue.ToString()), drpFinWH.SelectedItem.Text, 
                            Int64.Parse(drpFinSubWH.SelectedValue.ToString()), txtRemarks.Text, 
                            (byte[])(Session["ImageBytes"]), (bool)chkIsLoaded.Checked, txtAWBNo.Text.Trim(),
                            txtAWBPrefix.Text.Trim(), "", Session["UserName"].ToString().ToUpper(),
                            Convert.ToDateTime(Session["IT"].ToString())); //db.SelectRecords("spcreateUCR", pname, pvalue, ptype);drpTraCar.SelectedValue.ToString()
                    }
                }
            }
            else
            {
                //pvalue = new object[17]
                //{
                //    "INSERT",
                //    txtUCR.Text,
                //    txtTraDt.Text,
                //    TRACARDT,
                //    RECCARDT,
                //    drpTraCar.SelectedValue.ToString(),
                //    drpTraCar.SelectedItem.Text,
                //    RecCar,
                //    drpRecCar.SelectedItem.Text,
                //    drpTraWH.SelectedItem.Text,
                //    Int64.Parse(drpTraSubWH.SelectedValue.ToString()),
                //    drpFinWH.SelectedItem.Text,
                //    Int64.Parse(drpFinSubWH.SelectedValue.ToString()),
                //    txtRemarks.Text,
                //    DBNull.Value,
                //    (bool)chkIsLoaded.Checked,
                //    txtAWBNo.Text
                //};
                if (Request.QueryString["Mode"] == null || Request.QueryString["Mode"].ToString() == "")
                {
                    dsUCRNo = objBAL.AddUCRDetails("INSERT", txtUCR.Text.Trim(), txtTraDt.Text.Trim(), TRACARDT, RECCARDT, 
                        "0", tra, "0", car, drpTraWH.SelectedItem.Text, Int64.Parse(drpTraSubWH.SelectedValue.ToString()),
                        drpFinWH.SelectedItem.Text, Int64.Parse(drpFinSubWH.SelectedValue.ToString()), txtRemarks.Text.ToUpper(),
                        DBNull.Value, (bool)chkIsLoaded.Checked, txtAWBNo.Text.Trim(), txtAWBPrefix.Text.Trim(), "",
                                    Session["UserName"].ToString().ToUpper(), Convert.ToDateTime(Session["IT"].ToString())); //db.SelectRecords("spcreateUCR", pname, pvalue, ptype);drpRecCar.SelectedValue.ToString() = "0"
                }
                else
                {
                    if (Request.QueryString["Mode"].ToString() == "A")
                    {
                        dsUCRNo = objBAL.AddUCRDetails("INSERT", txtUCR.Text.Trim(), txtTraDt.Text.Trim(), TRACARDT, 
                            RECCARDT, "0", tra, "0", car, drpTraWH.SelectedItem.Text, 
                            Int64.Parse(drpTraSubWH.SelectedValue.ToString()), drpFinWH.SelectedItem.Text,
                            Int64.Parse(drpFinSubWH.SelectedValue.ToString()), txtRemarks.Text.ToUpper(), DBNull.Value,
                            (bool)chkIsLoaded.Checked, txtAWBNo.Text.Trim(), txtAWBPrefix.Text.Trim(), "Acc",
                                    Session["UserName"].ToString().ToUpper(), Convert.ToDateTime(Session["IT"].ToString())); //db.SelectRecords("spcreateUCR", pname, pvalue, ptype);drpRecCar.SelectedValue.ToString() = "0"
                    }
                    else
                    {
                        dsUCRNo = objBAL.AddUCRDetails("INSERT", txtUCR.Text.Trim(), txtTraDt.Text.Trim(), TRACARDT, 
                            RECCARDT, "0", tra, "0", car, drpTraWH.SelectedItem.Text, 
                            Int64.Parse(drpTraSubWH.SelectedValue.ToString()), drpFinWH.SelectedItem.Text,
                            Int64.Parse(drpFinSubWH.SelectedValue.ToString()), txtRemarks.Text.ToUpper(), DBNull.Value,
                            (bool)chkIsLoaded.Checked, txtAWBNo.Text.Trim(), txtAWBPrefix.Text.Trim(), "",
                             Session["UserName"].ToString().ToUpper(), Convert.ToDateTime(Session["IT"].ToString())); //db.SelectRecords("spcreateUCR", pname, pvalue, ptype);drpRecCar.SelectedValue.ToString() = "0"
                    }
                }
                SaveULDsinULDMaster();    // Save ULDS Based on parameters..
            }
            //SqlDbType[] ptype = new SqlDbType[17]
            //{
            //    SqlDbType.VarChar,
            //    SqlDbType.VarChar,
            //    SqlDbType.DateTime,
            //    SqlDbType.DateTime,
            //    SqlDbType.DateTime,
            //    SqlDbType.BigInt,
            //    SqlDbType.VarChar,
            //    SqlDbType.BigInt,
            //    SqlDbType.VarChar,
            //    SqlDbType.VarChar,
            //    SqlDbType.BigInt,
            //    SqlDbType.VarChar,
            //    SqlDbType.BigInt,
            //    SqlDbType.Text,
            //    SqlDbType.VarBinary,
            //    SqlDbType.Bit,
            //    SqlDbType.VarChar
            //};

            
            if (dsUCRNo.Tables.Count > 0)
            {
                if (dsUCRNo.Tables[0].Rows.Count > 0)
                {
                    txtUCR.Text = dsUCRNo.Tables[0].Rows[0][0].ToString();
                }
            }
            if (txtUCR.Text == "")
            {
                lblError.Text = lblError.Text + "ERROR in Inserting Data..";
                return false;
            }
            return true;
        }
        #endregion

        #region List UCRs
        private void ListUCRs(string UCRNumber, string ULDNumber, DateTime fromDt, DateTime ToDt, string TraWHCode, string TraCar, string TraCarText, string FinWHCode, string RecCar, string RecCarText)
        {
            //string[] pname = new string[10]
            //{   
            //    "UCRNo",
            //    "ULDNo",
            //    "frmDate",
            //    "toDate",
            //    "TraWHCode",
            //    "TraCar",
            //    "TraCarText",
            //    "FinWHCode",
            //    "RecCar",
            //    "RecCarText",
            //};
            object[] pvalue = new object[10]
            {
                UCRNumber,
                ULDNumber,
                fromDt,
                ToDt,
                TraWHCode,
                Int64.Parse(TraCar),
                TraCarText,
                FinWHCode,
                Int64.Parse(RecCar),
                RecCarText,
            };
            //SqlDbType[] ptype = new SqlDbType[10]
            //{
            //    SqlDbType.VarChar,
            //    SqlDbType.VarChar,
            //    SqlDbType.DateTime,
            //    SqlDbType.DateTime,
            //    SqlDbType.VarChar,
            //    SqlDbType.BigInt,
            //    SqlDbType.VarChar,
            //    SqlDbType.VarChar,
            //    SqlDbType.BigInt,
            //    SqlDbType.VarChar,
            //};
            DataSet dsGetListUCRs = new DataSet("UCRPUP_DS4");
            dsGetListUCRs = objBAL.ListUCRs(UCRNumber, ULDNumber, fromDt, ToDt, TraWHCode, Int64.Parse(TraCar), TraCarText, FinWHCode, Int64.Parse(RecCar), RecCarText);
            if (dsGetListUCRs != null)
            {
                if (dsGetListUCRs.Tables.Count > 0)
                {
                    if (dsGetListUCRs.Tables[0].Rows.Count > 0)
                    {
                        gvUCRSearch.DataSource = dsGetListUCRs;
                        gvUCRSearch.DataBind();
                        lblSError.Visible = false;
                        Session["gvUCRSearch"] = dsGetListUCRs;
                    }
                    else
                    {
                        gvUCRSearch.DataSource = dsGetListUCRs;
                        gvUCRSearch.DataBind();
                        lblSError.Visible = true;
                        lblSError.Text = "No Records Found with such criteria..";
                        Session["gvUCRSearch"] = dsGetListUCRs;
                    }
                }
                else
                {
                    gvUCRSearch.DataSource = null;
                    gvUCRSearch.DataBind();
                    lblSError.Text = "Error in getting data..";
                    Session["gvUCRSearch"] = null;
                }
            }
            else
            {
                gvUCRSearch.DataSource = null;
                gvUCRSearch.DataBind();
                lblSError.Text = "Error in getting data..";
                Session["gvUCRSearch"] = null;
            }
        }
        #endregion

        #region GET UCR DETAILS
        private string getUCRDetails(string UCRNumber)
        {
            //Database dbN = new Database();
            //string[] pname = new string[1] { "UCRNo" };
            //object[] pvalue = new object[1] { UCRNumber };
            //SqlDbType[] ptype = new SqlDbType[1] { SqlDbType.VarChar };

            DataSet dsgetUCRDetails = new DataSet("UCRPUP_DS5");
            dsgetUCRDetails = objBAL.getUCRDetails(UCRNumber);
            if (dsgetUCRDetails != null)
            {
                if (dsgetUCRDetails.Tables.Count > 0)
                {
                    if (dsgetUCRDetails.Tables[0].Rows.Count > 0)
                    {
                        if (dsgetUCRDetails.Tables[0].Rows[0]["UCRNo"].ToString() == "DNE")
                        {
                            return "EMPTY";
                        }
                        else
                        {
                            txtUCR.Text = UCRNumber;
                            txtTraDt.Text = dsgetUCRDetails.Tables[0].Rows[0]["TransferTime"].ToString();
                            try
                            {
                                // drpTraCar.SelectedValue = dsgetUCRDetails.Tables[0].Rows[0]["TransferID"].ToString();
                                txtTraPar.Text = dsgetUCRDetails.Tables[0].Rows[0]["TransferID"].ToString();
                            }
                            catch (Exception ex)
                            { }
                            //drpTraCar.SelectedItem.Text = dsgetUCRDetails.Tables[0].Rows[0]["TransferID"].ToString();
                            try
                            {
                                //drpRecCar.SelectedValue = dsgetUCRDetails.Tables[0].Rows[0]["ReceiverID"].ToString();
                                txtRecPar.Text = dsgetUCRDetails.Tables[0].Rows[0]["ReceiverID"].ToString();
                            }
                            catch (Exception ex)
                            { }
                            if (dsgetUCRDetails.Tables[0].Rows[0]["ActualTransferTime"].ToString().Trim() == "01/01/1800")
                            {
                                txtTraCarDt.Text = "";
                            }
                            else
                            {
                                txtTraCarDt.Text = dsgetUCRDetails.Tables[0].Rows[0]["ActualTransferTime"].ToString();
                            }

                            if (dsgetUCRDetails.Tables[0].Rows[0]["ActualReceiveTime"].ToString() == "01/01/1800")
                            {
                                txtRecCarDt.Text = "";
                            }
                            else
                            {
                                txtRecCarDt.Text = dsgetUCRDetails.Tables[0].Rows[0]["ActualReceiveTime"].ToString();
                            }

                            drpTraWH.SelectedIndex = drpTraWH.Items.IndexOf(drpTraWH.Items.FindByText(dsgetUCRDetails.Tables[0].Rows[0]["TransferWHCode"].ToString()));
                            //drpTraWH.SelectedItem.Text = dsgetUCRDetails.Tables[0].Rows[0]["TransferWHCode"].ToString();
                            drpTraSubWH.SelectedValue = dsgetUCRDetails.Tables[0].Rows[0]["TransferSubWHID"].ToString();
                            drpFinWH.SelectedIndex = drpFinWH.Items.IndexOf(drpFinWH.Items.FindByText(dsgetUCRDetails.Tables[0].Rows[0]["FinalWHCode"].ToString()));
                            //drpFinWH.SelectedItem.Text = dsgetUCRDetails.Tables[0].Rows[0]["FinalWHCode"].ToString();
                            drpFinSubWH.SelectedValue = dsgetUCRDetails.Tables[0].Rows[0]["FinalSubWHID"].ToString();
                            txtRemarks.Text = dsgetUCRDetails.Tables[0].Rows[0]["Remarks"].ToString();
                            if (dsgetUCRDetails.Tables[0].Rows[0]["Picture"] != DBNull.Value)
                            {
                                Session["ImageBytes"] = (byte[])(dsgetUCRDetails.Tables[0].Rows[0]["Picture"]);
                                ImagePreview.ImageUrl = "~/ImageHandler.ashx";
                            }
                            else
                            {
                                Session["ImageBytes"] = null;
                            }
                            chkIsLoaded.Checked = bool.Parse(dsgetUCRDetails.Tables[0].Rows[0]["isLoaded"].ToString());
                            txtAWBNo.Text = dsgetUCRDetails.Tables[0].Rows[0]["AWBNo"].ToString();
                            txtAWBPrefix.Text = dsgetUCRDetails.Tables[0].Rows[0]["AWBPrefix"].ToString();
                            clsFillCombo cls = new clsFillCombo();
                            cls.FillSubWHBasedOnWH(drpTraWH.SelectedItem.Text, drpTraSubWH);
                            cls.FillSubWHBasedOnWH(drpFinWH.SelectedItem.Text, drpFinSubWH);
                            GetSetUCRAccessories(UCRNumber);
                            return "FULL";
                        }
                    }
                    return "ERROR";
                }
                else
                {
                    return "ERROR";
                }
            }
            else
            {
                return "ERROR";
            }
        }
        #endregion

        #region GetDetails of UCRULD details
        private DataTable GetDetailsUCRULD(string strUCRNo)
        {
            DataSet dsUCRULDDetails = new DataSet("UCRPUP_DS6");
            dsUCRULDDetails = objBAL.GetDetailsUCRULD("GET", strUCRNo, "", "", true, "", DateTime.Now, "", "", false);
            if (dsUCRULDDetails != null)
            {
                if (dsUCRULDDetails.Tables[0].Rows.Count > 0)
                {
                    if (Session["SdtUCRULD"] == null)
                    {
                        Session["SdtUCRULD"] = dsUCRULDDetails.Tables[0];
                        gvULDDetails.DataSource = dsUCRULDDetails.Tables[0];
                        gvULDDetails.DataBind();
                        for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                        {
                            TextBox lblRetOnDate = (TextBox)(gvULDDetails.Rows[i].FindControl("lblRetOn"));
                            if (lblRetOnDate.Text.Contains("1/1/1800"))
                                lblRetOnDate.Text = "";
                        }
                        //  (Session["SdtUCRULD"]) = dsUCRULDDetails.Tables[0];
                        return (DataTable)(Session["SdtUCRULD"]);
                    }
                    else if (((DataTable)Session["SdtUCRULD"]).Rows.Count > dsUCRULDDetails.Tables[0].Rows.Count)
                    {
                        gvULDDetails.DataSource = (DataTable)Session["SdtUCRULD"];
                        gvULDDetails.DataBind();
                        for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                        {
                            TextBox lblRetOnDate = (TextBox)(gvULDDetails.Rows[i].FindControl("lblRetOn"));
                            if (lblRetOnDate.Text.Contains("1/1/1800"))
                                lblRetOnDate.Text = "";
                        }
                        return ((DataTable)Session["SdtUCRULD"]);
                    }
                    else if (((DataTable)Session["SdtUCRULD"]).Rows.Count <= dsUCRULDDetails.Tables[0].Rows.Count)
                    {
                        gvULDDetails.DataSource = dsUCRULDDetails;
                        gvULDDetails.DataBind();
                        (Session["SdtUCRULD"]) = dsUCRULDDetails.Tables[0];
                        for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                        {
                            TextBox lblRetOnDate = (TextBox)(gvULDDetails.Rows[i].FindControl("lblRetOn"));
                            if (lblRetOnDate.Text.Contains("1/1/1800"))
                                lblRetOnDate.Text = "";
                        }
                        return ((DataTable)Session["SdtUCRULD"]);
                    }
                }

                else
                {
                    DataTable dtULDDetails = new DataTable("UCRPUP_DT3");
                    if (Session["SdtUCRULD"] == null || !(((DataTable)Session["SdtUCRULD"]).Columns.Contains("ULDNo")))
                    {
                        dtULDDetails.Columns.Add("ULDNo");
                        dtULDDetails.Columns.Add("RecNo");
                        dtULDDetails.Columns.Add("isDamaged", typeof(bool));
                        dtULDDetails.Columns.Add("returnedAtWHCode");
                        dtULDDetails.Columns.Add("returnedOn");
                        dtULDDetails.Columns.Add("AWBPrefix");
                        dtULDDetails.Columns.Add("AWBNo");
                        dtULDDetails.Columns.Add("isLoaded", typeof(bool));
                        dtULDDetails.Rows.Add("DUMMY", "", 0, "", "","","",0);
                        Session["SdtUCRULD"] = dtULDDetails;
                    }
                    gvULDDetails.DataSource = ((DataTable)Session["SdtUCRULD"]);
                    gvULDDetails.DataBind();
                    gvULDDetails.Rows[0].Visible = false;
                    TextBox txtRetOnGV = (TextBox)(gvULDDetails.FooterRow.FindControl("txtRetOn"));
                    txtRetOnGV.Attributes.Add("readonly", "readonly");
                    return ((DataTable)Session["SdtUCRULD"]);
                }
            }
            else
            {
                lblError.Text = "Error in connecting database..";
            }
            return null;
        }
        #endregion

        #region ADD UCRULD Details
        private bool ADDDetailsUCRULD()
        {
            try
            {
                TextBox textULDNo = (TextBox)(gvULDDetails.FooterRow.FindControl("txtULDNo"));
                CheckBox chkisDamaged = (CheckBox)(gvULDDetails.FooterRow.FindControl("chkFDamaged"));
                TextBox textRetAt = (TextBox)(gvULDDetails.FooterRow.FindControl("txtRetAt"));
                TextBox textReton = (TextBox)(gvULDDetails.FooterRow.FindControl("txtRetOn"));
                TextBox textAWBPrefix = (TextBox)(gvULDDetails.FooterRow.FindControl("txtAWBPrefix"));
                TextBox textAWBNo = (TextBox)(gvULDDetails.FooterRow.FindControl("txtAWBNo"));
                CheckBox chkLoaded = (CheckBox)(gvULDDetails.FooterRow.FindControl("chkLoaded"));
                
                string lblLastRecNo = "";

                if (gvULDDetails.Rows.Count > 0)
                {
                    Label lblULDNo = (Label)gvULDDetails.Rows[0].FindControl("lblULDNo");
                    if (lblULDNo.Text.ToUpper() == "DUMMY")
                    {

                    }
                    else
                    {
                        Label lblRecNo = (Label)gvULDDetails.Rows[gvULDDetails.Rows.Count - 1].FindControl("lblRecNo");
                        lblLastRecNo = lblRecNo.Text.Trim();
                    }
                }

                (Session["SdtUCRULD"]) = GetDetailsUCRULD(txtUCR.Text);

                if (((DataTable)Session["SdtUCRULD"]) != null)
                {
                    if (((DataTable)Session["SdtUCRULD"]).Rows[0]["ULDNo"].ToString() == "DUMMY")
                    {
                        ((DataTable)Session["SdtUCRULD"]).Clear();
                        DataTable dt =new DataTable("UCRPUP_DT4");
                            dt = (DataTable)Session["SdtUCRULD"];

                        Object RetAt = (string)textRetAt.Text.ToUpper(), Reton = (string)textReton.Text, AWBPrefix = (string)textAWBPrefix.Text, AWB = (string)textAWBNo.Text;

                        if (textULDNo.Text == "")
                        {
                            return false;
                        }

                        if (textRetAt.Text == "") RetAt = (string)"";
                        if (textReton.Text == "")
                        {
                            Reton = DateTime.Parse("1/1/1800 12:00:00 AM");  // Min Value For Null
                        }
                        if (textAWBPrefix.Text == "") AWBPrefix = (string)"";
                        if (textAWBNo.Text == "") AWB = (string)"";

                        dt.Rows.Add(textULDNo.Text.ToUpper(), "Autogenerated", chkisDamaged.Checked, RetAt, Reton, AWBPrefix, AWB, chkLoaded.Checked);
                        Session["SdtUCRULD"] = dt;
                        gvULDDetails.DataSource = ((DataTable)Session["SdtUCRULD"]);
                        gvULDDetails.DataBind();
                        for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                        {
                            TextBox lblRetOnDate = (TextBox)(gvULDDetails.Rows[i].FindControl("lblRetOn"));
                            if (lblRetOnDate.Text.Contains("1/1/1800"))
                                lblRetOnDate.Text = "";
                        }
                        gvULDDetails.Rows[0].Visible = true;
                    }
                    else
                    {
                        DataTable dt = new DataTable("UCRPUP_DT5");
                            dt = (DataTable)Session["SdtUCRULD"];
                        // ((DataTable)Session["SdtUCRULD"]).Rows.Add(textULDNo.Text, textNets.Text, textDoors.Text, textStraps.Text, textFittings.Text, chkisLoaded.Checked, textAWB.Text, chkisDamaged.Checked, textRetAt.Text, textReton.Text);
                        Object RetAt = (string)textRetAt.Text.ToUpper(), Reton = (string)textReton.Text, AWBPrefix = (string)textAWBPrefix.Text, AWB = (string)textAWBNo.Text;

                        if (textULDNo.Text == "")
                        {
                            return false;
                        }
                        if (textRetAt.Text == "") RetAt = (string)"";
                        if (textReton.Text == "")
                        {
                            Reton = DateTime.Parse("1/1/1800 12:00:00 AM");  // Min Value For Null
                        }
                        if (textAWBPrefix.Text == "") AWBPrefix = (string)"";
                        if (textAWBNo.Text == "") AWB = (string)"";

                        if (lblLastRecNo.Trim() == "" || lblLastRecNo.Trim() == "Autogenerated")
                        {
                            dt.Rows.Add(textULDNo.Text.ToUpper(), lblLastRecNo, chkisDamaged.Checked, RetAt, Reton, AWBPrefix, AWB, chkLoaded.Checked);
                        }
                        else
                        {
                            dt.Rows.Add(textULDNo.Text.ToUpper(), IncreamentRect(lblLastRecNo), chkisDamaged.Checked, RetAt, Reton, AWBPrefix, AWB, chkLoaded.Checked);
                        }

                        Session["SdtUCRULD"] = dt;
                        gvULDDetails.DataSource = ((DataTable)Session["SdtUCRULD"]);
                        gvULDDetails.DataBind();
                        for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                        {
                            TextBox lblRetOnDate = (TextBox)(gvULDDetails.Rows[i].FindControl("lblRetOn"));
                            if (lblRetOnDate.Text.Contains("1/1/1800"))
                                lblRetOnDate.Text = "";
                        }
                        gvULDDetails.Rows[0].Visible = true;
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception Ex)
            {
                lblError.Visible = true;
                lblError.Text = Ex.Message;
                return false;
            }
        }
        #endregion

        #region Function To Increament Receipt No.
        private string IncreamentRect(string RecNo)
        {
            try
            {
                if (RecNo.Trim() == "Autogenerated")
                {
                    return "Autogenerated";
                }
                string SubInt2Digit = RecNo.Substring(4, 2);
                int IncreamentPart = int.Parse(SubInt2Digit);
                IncreamentPart++;
                string PrevPart = RecNo.Substring(0, 4);
                string LaterPart = RecNo.Substring(6, RecNo.Length - 6);
                return (PrevPart + IncreamentPart.ToString().PadLeft(2, '0') + LaterPart);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                return "ERROR";
            }
        }
        #endregion

        #region Add ULDDetailsGrid to database
        private void AddULDDetailsToDatabase()
        {
            try
            {
                if (!objBAL.GetDetailsUCRULDbool("DELETE", txtUCR.Text, "DUM", "DUM", true, "DUM", DateTime.Now,"","",true))
                {
                    lblError.Text = "Error in Updating ULDDetails";
                    return;
                }


                for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                {
                    string strRetOnGV = "";
                    TextBox lblRetOnGV = (TextBox)gvULDDetails.Rows[i].FindControl("lblRetOn");
                    if (lblRetOnGV.Text == "")
                    {
                        strRetOnGV = "1/1/1800 12:00:00 AM";
                    }
                    else
                    {
                        strRetOnGV = lblRetOnGV.Text;
                    }
                //    object[] pvalue = new object[7]
                //{
                //    "ADD",
                //    txtUCR.Text,
                //    ((Label)gvULDDetails.Rows[i].FindControl("lblULDNo")).Text,
                //    txtUCR.Text.Trim().Substring(0,3).ToString()+"-"+i.ToString().PadLeft(2,'0')+txtUCR.Text.Trim().Substring(txtUCR.Text.Trim().Length-6,6),
                //    ((CheckBox)gvULDDetails.Rows[i].FindControl("chkDamaged")).Checked,
                //    ((TextBox)gvULDDetails.Rows[i].FindControl("lblRetAt")).Text,
                //    Convert.ToDateTime(strRetOnGV),
                //};
                    if (!objBAL.GetDetailsUCRULDbool("ADD", txtUCR.Text, ((Label)gvULDDetails.Rows[i].FindControl("lblULDNo")).Text.ToUpper(), txtUCR.Text.Trim().Substring(0, 3).ToString() + "-" + i.ToString().PadLeft(2, '0') + txtUCR.Text.Trim().Substring(txtUCR.Text.Trim().Length - 6, 6), ((CheckBox)gvULDDetails.Rows[i].FindControl("chkDamaged")).Checked, ((TextBox)gvULDDetails.Rows[i].FindControl("lblRetAt")).Text, Convert.ToDateTime(strRetOnGV), ((TextBox)gvULDDetails.Rows[i].FindControl("lblAWBPrefix")).Text, ((TextBox)gvULDDetails.Rows[i].FindControl("lblAWBNo")).Text, ((CheckBox)gvULDDetails.Rows[i].FindControl("chkTLoaded")).Checked))
                    {
                        lblError.Visible = true;
                        lblError.Text = "Error in Updating ULDDetails";
                        return;
                    }
                   // pvalue = null;
                    lblRetOnGV = null;
                    strRetOnGV = null;
                }
                lblError.Visible = true;
                lblError.Text = "Saved Successfully!";
            }
            catch (Exception Ex)
            {
                lblError.Visible = true;
                lblError.Text = Ex.Message;
            }
        }
        #endregion

        #region GVRowDeleting
        protected void gvULDDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            try
            {
                DataTable dt = new DataTable("UCRPUP_DT6");
                dt = (DataTable)Session["SdtUCRULD"];
                ((DataTable)Session["SdtUCRULD"]).Rows.RemoveAt(e.RowIndex);
                if (((DataTable)Session["SdtUCRULD"]).Rows.Count == 0)
                {
                    ((DataTable)Session["SdtUCRULD"]).Rows.Add("DUMMY", "", 0, "", "", "", "", 0);
                    gvULDDetails.DataSource = ((DataTable)Session["SdtUCRULD"]);
                    gvULDDetails.DataBind();
                    gvULDDetails.Rows[0].Visible = false;
                }
                else
                {
                    gvULDDetails.DataSource = ((DataTable)Session["SdtUCRULD"]);
                    gvULDDetails.DataBind();
                    gvULDDetails.Rows[0].Visible = true;
                }

                // For Setting Date
                for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                {
                    TextBox lblRetOnDate = (TextBox)(gvULDDetails.Rows[i].FindControl("lblRetOn"));
                    if (lblRetOnDate.Text.Contains("1/1/1800"))
                        lblRetOnDate.Text = "";
                }

                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Text = "Row Deleted Successfully.";
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Text = ex.Message;
            }
        }
        #endregion

        #region On Row Editing of gvSearchUCR
        protected void gvUCRSearch_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Label lbUCRNo = (Label)(gvUCRSearch.Rows[e.NewEditIndex].FindControl("lblUCRNo"));
            string strUCRNumber = lbUCRNo.Text;
            // Session["SdtUCRULD"] = null;
            Session["ImageBytes"] = null;
            SearchEdit.Visible = false;
            New.Visible = true;
            lblError.Visible = false;
            NEWTitle.InnerHtml = "EDIT UCR";

            lblSError.Visible = false;
            btnSave.Visible = true;
            btnPrint.Visible = true;
            btnCancel.Visible = true;
            btnClear.Visible = true;

            // Fill All Combo Box
            drpTraWH.ClearSelection();
            drpTraSubWH.ClearSelection();
            drpFinWH.ClearSelection();
            drpFinSubWH.ClearSelection();
            txtTraPar.Text = ""; // drpTraCar.ClearSelection();
            txtRecPar.Text = ""; //drpRecCar.ClearSelection();
            clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpTraWH);
            clsCombo.FillAllComboBoxes("tblSubWHMaster", "SELECT", drpTraSubWH);
            clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", drpFinWH);
            clsCombo.FillAllComboBoxes("tblSubWHMaster", "SELECT", drpFinSubWH);
            //clsCombo.FillAllComboBoxes("tblRecCarrier", "SELECT", drpRecCar);
            //clsCombo.FillAllComboBoxes("tblTraCarrier", "SELECT", drpTraCar);

            // Set Dates
            txtTraDt.Attributes.Add("readonly", "readonly");
            txtTraDt.Text = Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy");
            txtTraCarDt.Attributes.Add("readonly", "readonly");
            txtTraCarDt.Text = Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy");
            txtRecCarDt.Attributes.Add("readonly", "readonly");
            txtRecCarDt.Text = Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy");

            DataTable dtGetDetailsUCRULD = new DataTable("UCRPUP_DT7");
            dtGetDetailsUCRULD = GetDetailsUCRULD(strUCRNumber);
            if (dtGetDetailsUCRULD == null)
            {
                lblError.Visible = true;
                lblError.Text = lblError.Text + "| Error in Listing ULD..";
            }
            string strUCRDetails = getUCRDetails(strUCRNumber);
            if (strUCRDetails == "ERROR")
            {
                lblError.Visible = true;
                lblError.Text = "Error in Getting UCR Details";
            }
            else if (strUCRDetails == "EMPTY")
            {
                lblError.Visible = true;
                lblError.Text = "No Such UCR Exists";
            }
            else if (strUCRDetails == "FULL")
            {
                lblError.Visible = false;
                lblError.Text = "";
                DataTable dt = new DataTable("UCRPUP_DT8");
                    dt = (DataTable)Session["SdtUCRULD"];
            }

        }
        #endregion

        #region gvEditing
        protected void gvULDDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion

        #region Validate ListUCR
        private bool ValidateLISTUCR()
        {
            DateTime dtParsableFrmDt, dtParsableToDt;
            if (!DateTime.TryParse(txtSfrmDate.Text, out dtParsableFrmDt) || !DateTime.TryParse(txtSToDate.Text, out dtParsableToDt))
            {
                lblSError.Visible = true;
                lblSError.Text = "Please put valid Date..";
                return false;
            }
            return true;
        }
        #endregion

        #region TraWHSelectionIndexChanged
        protected void drpTraWH_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsFillCombo cls = new clsFillCombo();
            cls.FillSubWHBasedOnWH(drpTraWH.SelectedItem.Text, drpTraSubWH);
        }
        #endregion

        #region FinWHSelectionChangedEvent
        protected void drpFinWH_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsFillCombo cls = new clsFillCombo();
            cls.FillSubWHBasedOnWH(drpFinWH.SelectedItem.Text, drpFinSubWH);
        }
        #endregion

        #region Button Search Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dtGetDetailsUCRULD = new DataTable("UCRPUP_DT9");
            dtGetDetailsUCRULD = GetDetailsUCRULD(txtUCR.Text);
            if (dtGetDetailsUCRULD == null)
            {
                lblError.Visible = true;
                lblError.Text = lblError.Text + "| Error in Listing ULD..";
            }
            string strUCRDetails = getUCRDetails(txtUCR.Text);
            if (strUCRDetails == "ERROR")
            {
                lblError.Visible = true;
                lblError.Text = "Error in Getting UCR Details";
            }
            else if (strUCRDetails == "EMPTY")
            {
                lblError.Visible = true;
                lblError.Text = "No Such UCR Exists";
            }
            else if (strUCRDetails == "FULL")
            {
                lblError.Visible = false;
                lblError.Text = "";
                DataTable dt = new DataTable("UCRPUP_DT10");
                dt = (DataTable)Session["SdtUCRULD"];
            }
        }
        #endregion

        #region Add Accessories to tblUCRAccessories
        private void AddAccessoriesToDB()
        {
            try
            {
                string[] pname = new string[7]
            {   
                "SPType",
                "UCRNo",
                "Status",
                "Nets",
                "Doors",
                "Straps",
                "Fittings"                
            };

                SqlDbType[] ptype = new SqlDbType[7]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt,
                SqlDbType.TinyInt
            };

                //    if (!db.UpdateData("spADDUCRAccessoriesDetails", pname, pvalue1, ptype))  // First Delete the Existing Data and Then Insert New
                //    {
                //        lblError.Text = "Error in Updating ULDDetails";
                //        return;
                //    }

                if (SetZero(txtNetsRel) && SetZero(txtDoorsRel) && SetZero(txtStrapsRel) && SetZero(txtFittingsRel) && SetZero(txtNetsRet) && SetZero(txtDoorsRet) && SetZero(txtStrapsRet) && SetZero(txtFittingsRet) && SetZero(txtNetsDam) && SetZero(txtDoorsDam) && SetZero(txtStrapsDam) && SetZero(txtFittingsDam))
                {
                    // Released entries Add/Update
                    //object[] pvalue2 = new object[7]
                    //{
                    //    "ADD",
                    //    txtUCR.Text.Trim(),
                    //    lblRelealsed.Text.Trim(),
                    //    txtNetsRel.Text.Trim(),
                    //    txtDoorsRel.Text.Trim(),
                    //    txtStrapsRel.Text.Trim(),
                    //    txtFittingsRel.Text.Trim()
                    //};
                    if (!objBAL.AddAccessoriesToDB("ADD", txtUCR.Text.Trim(), lblRelealsed.Text.Trim(), txtNetsRel.Text.Trim(), txtDoorsRel.Text.Trim(), txtStrapsRel.Text.Trim(), txtFittingsRel.Text.Trim()))  // First Delete the Existing Data and Then Insert New
                    {
                        lblError.Text = "Error in Updating ULDDetails";
                        return;
                    }

                    // Returned Entries Add/Update
                    //object[] pvalue3 = new object[7]
                    //{
                    //    "ADD",
                    //    txtUCR.Text.Trim(),
                    //    lblReturned.Text.Trim(),
                    //    txtNetsRet.Text.Trim(),
                    //    txtDoorsRet.Text.Trim(),
                    //    txtStrapsRet.Text.Trim(),
                    //    txtFittingsRet.Text.Trim()
                    //};
                    if (!objBAL.AddAccessoriesToDB("ADD", txtUCR.Text.Trim(), lblReturned.Text.Trim(), txtNetsRet.Text.Trim(), txtDoorsRet.Text.Trim(), txtStrapsRet.Text.Trim(), txtFittingsRet.Text.Trim()))  // First Delete the Existing Data and Then Insert New
                    {
                        lblError.Text = "Error in Updating ULDDetails";
                        return;
                    }

                    // Damaged Entries Add/Update
                    //object[] pvalue4 = new object[7]
                    //{
                    //    "ADD",
                    //    txtUCR.Text.Trim(),
                    //    lblDamaged.Text.Trim(),
                    //    txtNetsDam.Text.Trim(),
                    //    txtDoorsDam.Text.Trim(),
                    //    txtStrapsDam.Text.Trim(),
                    //    txtFittingsDam.Text.Trim()
                    //};
                    if (!objBAL.AddAccessoriesToDB("ADD", txtUCR.Text.Trim(), lblDamaged.Text.Trim(), txtNetsDam.Text.Trim(), txtDoorsDam.Text.Trim(), txtStrapsDam.Text.Trim(), txtFittingsDam.Text.Trim()))  // First Delete the Existing Data and Then Insert New
                    {
                        lblError.Text = "Error in Updating ULDDetails";
                        return;
                    }
                }

                lblError.Text = "UCR Details Added Successfully!!";
            }
            catch (Exception ex)
            {
                lblError.Text = "In AddAccessoriesToDB " + ex.Message;
            }
        }
        #endregion

        #region Set Textbox 0 if no value
        private bool SetZero(TextBox objText)
        {
            if (objText.Text == "")
            {
                objText.Text = "0";
                return true;
            }
            int N = 0;
            if (!int.TryParse(objText.Text, out N))
            {
                lblError.Text = "Error " + objText.Text + " is not a number";
                return false;
            }

            return true;
        }
        #endregion

        #region Get and Set UCRAccessories Details Based On UCR
        private void GetSetUCRAccessories(string UCRNo)
        {
            //string[] pname = new string[7]
            //{   
            //    "SPType",
            //    "UCRNo",
            //    "Status",
            //    "Nets",
            //    "Doors",
            //    "Straps",
            //    "Fittings"                
            //};

            //object[] pvalue = new object[7]
            //{
            //    "GET",
            //    UCRNo,
            //    "DUM",
            //    1,
            //    1,
            //    1,
            //    1,
            //};
            //SqlDbType[] ptype = new SqlDbType[7]
            //{
            //    SqlDbType.VarChar,
            //    SqlDbType.VarChar,
            //    SqlDbType.VarChar,
            //    SqlDbType.TinyInt,
            //    SqlDbType.TinyInt,
            //    SqlDbType.TinyInt,
            //    SqlDbType.TinyInt
            //};

            //Database dbN = new Database();

            DataSet dsgetUCRAccessoriesDetails = new DataSet("UCRPUP_DS7");
            dsgetUCRAccessoriesDetails = objBAL.GetAccessoriesFromDB("GET", UCRNo, "DUM", "1", "1", "1", "1");
            if (dsgetUCRAccessoriesDetails != null && dsgetUCRAccessoriesDetails.Tables.Count > 0)
            {
                if (dsgetUCRAccessoriesDetails.Tables[0].Rows.Count > 0)
                {
                    // Set All Values

                    txtNetsRel.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[0]["Nets"].ToString();
                    txtDoorsRel.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[0]["Doors"].ToString();
                    txtStrapsRel.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[0]["Straps"].ToString();
                    txtFittingsRel.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[0]["Fittings"].ToString();

                    txtNetsRet.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[1]["Nets"].ToString();
                    txtDoorsRet.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[1]["Doors"].ToString();
                    txtStrapsRet.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[1]["Straps"].ToString();
                    txtFittingsRet.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[1]["Fittings"].ToString();

                    txtNetsDam.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[2]["Nets"].ToString();
                    txtDoorsDam.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[2]["Doors"].ToString();
                    txtStrapsDam.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[2]["Straps"].ToString();
                    txtFittingsDam.Text = dsgetUCRAccessoriesDetails.Tables[0].Rows[2]["Fittings"].ToString();
                }

                else
                {
                    lblError.Text = "There is no data of Accessories for this UCR";
                    return;
                }
            }

        }
        #endregion

        #region Print Click
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Session["DtUCRReport"] = "";
                DataTable dtRpt = new DataTable("UCRPUP_DT11");
                dtRpt = FillUCRReport(txtUCR.Text);
                DataTable dtIATARpt = new DataTable("UCRPUP_DT12");
                dtIATARpt = GetULDIATACodes(txtUCR.Text);
                DataTable dtUCRULDRpt = new DataTable("UCRPUP_DT13");
                dtUCRULDRpt = GetUCRULDRpt(txtUCR.Text);

                if (dtIATARpt != null)
                {
                    if (dtIATARpt.Rows.Count > 0)
                    {
                        Session["dtIATARpt"] = dtIATARpt;
                    }
                    else
                    {
                        lblError.Text = "No ULDS for this UCR.";
                        return;
                    }
                }
                else
                {
                    lblError.Text = "Error in Report";
                    return;
                }

                if (dtUCRULDRpt != null)
                {
                    if (dtUCRULDRpt.Rows.Count > 0)
                    {
                        Session["dtUCRULDRpt"] = dtUCRULDRpt;
                    }
                    else
                    {
                        lblError.Text = "No ULDS for this UCR.";
                        return;
                    }
                }
                else
                {
                    lblError.Text = "Error in Report";
                    return;
                }

                if (dtRpt != null)
                {
                    if (dtRpt.Rows.Count > 0)
                    {

                        Session["DtUCRReport"] = dtRpt;
                        string query = "'ShowUCR.aspx?ID=" + 0 + "'";
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ");", true);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);
                    }
                    else
                    {
                        lblError.Text = "No Records Found For This UCR";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }


        }
        #endregion

        #region FillUCRReport Session
        private DataTable FillUCRReport(string UCR)
        {
            //string[] pname = new string[1]
            //{   
            //    "UCRNo"
            //};

            //object[] pvalue = new object[1]
            //{
            //    UCR
            //};

            //SqlDbType[] ptype = new SqlDbType[1]
            //{
            //    SqlDbType.VarChar
            //};

            //Database dbN = new Database();
            DataSet dsgetUCRRpt = new DataSet("UCRPUP_DS8");
            dsgetUCRRpt = objBAL.GetUCRReport(UCR);
            if (dsgetUCRRpt != null && dsgetUCRRpt.Tables.Count > 0)
            {
                return dsgetUCRRpt.Tables[0];
            }
            return null;
        }
        #endregion

        #region Fill ULDIATA Codes
        private DataTable GetULDIATACodes(string UCR)
        {
            //string[] pname = new string[1]
            //{   
            //    "UCRNo"
            //};

            //object[] pvalue = new object[1]
            //{
            //    UCR
            //};

            //SqlDbType[] ptype = new SqlDbType[1]
            //{
            //    SqlDbType.VarChar
            //};

            //Database dbN = new Database();
            DataSet dsgetIATARpt= new DataSet("UCRPUP_DS9");
            dsgetIATARpt = objBAL.GetIATACodesReport(UCR); //dbN.SelectRecords("spSubRptIATACodes", pname, pvalue, ptype);
            if (dsgetIATARpt != null && dsgetIATARpt.Tables.Count > 0)
            {
                return dsgetIATARpt.Tables[0];
            }
            return null;
        }
        #endregion

        #region Get UCRULDReport Details
        private DataTable GetUCRULDRpt(string UCR)
        {
            //string[] pname = new string[1]
            //{   
            //    "UCRNo"
            //};

            //object[] pvalue = new object[1]
            //{
            //    UCR
            //};

            //SqlDbType[] ptype = new SqlDbType[1]
            //{
            //    SqlDbType.VarChar
            //};

            //Database dbN = new Database();
            DataSet dsgetUCRULDRpt = new DataSet("UCRPUP_DS10");
            dsgetUCRULDRpt = objBAL.GetUCRULDRpt(UCR);
            if (dsgetUCRULDRpt != null && dsgetUCRULDRpt.Tables.Count > 0)
            {
                return dsgetUCRULDRpt.Tables[0];
            }
            return null;
        }
        #endregion

        #region Clear Click
        protected void btnClear_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Cancel Click
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Upload Image
        protected void btnUploadImage_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region List Click
        protected void btnListUCR_Click(object sender, EventArgs e)
        {
            if (!ValidateLISTUCR())
            {
                return;
            }

            string strSTraWH = drpSTraWH.SelectedItem.Text, strSTraCarText = drpSTraCar.SelectedItem.Text, strSFinWH = drpSFinWH.SelectedItem.Text, strSRecCarText = drpSRecCar.SelectedItem.Text;
            string strSTraCar = drpSTraCar.SelectedValue;

            string strSRecCar = drpSRecCar.SelectedValue;
            int intstrRecCar = int.Parse(strSRecCar);
            if (intstrRecCar >= 100000)
            {
                intstrRecCar = intstrRecCar - 100000;
            }
            else
            {

            }
            if (strSTraWH == "SELECT")
                strSTraWH = "";

            if (strSTraCarText == "SELECT")
                strSTraCarText = "";
            if (strSFinWH == "SELECT")
                strSFinWH = "";

            if (strSRecCarText == "SELECT")
                strSRecCarText = "";

            try
            {
                ListUCRs(txtSUCR.Text, txtSUld.Text, Convert.ToDateTime(txtSfrmDate.Text), Convert.ToDateTime(txtSToDate.Text), strSTraWH, strSTraCar, strSTraCarText, strSFinWH, intstrRecCar.ToString(), strSRecCarText);
            }
            catch (Exception ex)
            {
                lblSError.Text = ex.Message;
            }
        }
        #endregion

        #region UCR Search Page Index Changing event
        protected void gvUCRSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUCRSearch.PageIndex = e.NewPageIndex;
            gvUCRSearch.DataSource = (DataSet)Session["gvUCRSearch"];
            gvUCRSearch.DataBind();
            lblSError.Visible = false;
            
            //Session["gvUCRSearch"] = dsGetListUCRs;
        }
        #endregion

        #region Save ULD in ULDMaster
        private bool SaveULDNoinMaster(string UDLNumber, string DollyWt)
        {
            string strULDPrefix = UDLNumber.Trim().Substring(0, 3);
            string strULDSuffix = UDLNumber.Trim().Substring(UDLNumber.Trim().Length - 2, 2);
            string strULDSerial = UDLNumber.Trim().Replace(strULDPrefix, "").Replace(strULDSuffix, "");

            BALULDMaster blULD = new BALULDMaster();

            blULD.SelectRecords(UDLNumber, strULDSuffix, 0, "0", "0", 0, 0, 0, "0", "", "", "", false, "", strULDSerial, Convert.ToString(Session["Station"]), Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]),
                "", 0, "0", "0", Convert.ToDateTime(Session["IT"]), "", "", "", false, "3", "N", DollyWt);

            blULD = null;

            return true;
        }
        #endregion

        #region Save ULDs in ULDGrid loop
        private void SaveULDsinULDMaster()
        {
            try
            {
                for (int i = 0; i < gvULDDetails.Rows.Count; i++)
                {
                    Label lblULDNumText = (Label)(gvULDDetails.Rows[i].FindControl("lblULDNo"));
                    if (Request.QueryString["Mode"] == null || Request.QueryString["Mode"].ToString() == "")
                    {

                    }
                    else
                    {
                        if (Request.QueryString["Mode"].ToString() == "A")
                        {
                            SaveULDNoinMaster(lblULDNumText.Text.ToUpper(), "0");
                            UpdateULDMovement(lblULDNumText.Text.ToUpper(), Session["UserName"].ToString().ToUpper(), "IN", drpTraWH.SelectedItem.Text.Trim(), drpFinWH.SelectedItem.Text.Trim());
                        }
                        else if(Request.QueryString["Mode"].ToString() == "M")
                        {
                            if (Request.QueryString["pg"].ToString() == "Arr")
                            {
                                SaveULDNoinMaster(lblULDNumText.Text.ToUpper(), "0");
                                UpdateULDMovement(lblULDNumText.Text.ToUpper(), Session["UserName"].ToString().ToUpper(), "IN", drpTraWH.SelectedItem.Text.Trim(), drpFinWH.SelectedItem.Text.Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.ForeColor = Color.Red;
                lblError.Text = "Error in SaveULDsinULDMaster";
            }

        }
        #endregion

        #region Update IN movement 
        private void UpdateULDMovement(string ULDNumber, string UpdatedBy, string MoveType, string Origin, string Destiantaion)
        {
            BALULDMaster objBALULD = new BALULDMaster();
            if (!objBALULD.UpdateLatestULDMovementHistory(ULDNumber.ToUpper(), UpdatedBy, MoveType, Origin, Destiantaion))
            {
                lblError.Visible = true;
                lblError.Text = "Error in Updating ULDMovement.";
                lblError.ForeColor = Color.Red;
            }
        }
        #endregion

    }
}
