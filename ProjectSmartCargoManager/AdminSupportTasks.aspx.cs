using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class Admin_Support_Tasks : System.Web.UI.Page
    {
        #region Variables
        BAL.BALAdminSupportTask ObjBAL = new BAL.BALAdminSupportTask();

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSet ds = new DataSet();
                ds = ObjBAL.GetAgentCode();

                ddlNewAgtCode.Items.Clear();
                ddlNewAgtCode.DataSource = ds.Tables[0];
                ddlNewAgtCode.DataTextField  ="AgentCode" ;
                ddlNewAgtCode.DataValueField = "AgentCode";
                ddlNewAgtCode.DataBind();
                ddlNewAgtCode.SelectedIndex  = 0;

                BtnReopen.Visible = false;
               // BtnAWBVoid.Visible = false;
                BtnVoidListedAWB.Visible = false;
                lblNewRate.Visible = false;
                txtAWBNewRatePerKg.Visible = false;
                BtnRateReProcess.Visible = false;
                btnRemoveAWBSpotrate.Visible = false;
                btnRemoveAWBDCM.Visible = false;


                LblNewAgentCcode.Visible = false;
                ddlNewAgtCode.Visible = false;
                BtnChgAgtCd.Visible = false;

                LblNewAWBDt.Visible = false;
                txtAWBNewDt.Visible = false;
                BtnChangeDt.Visible = false;


                //ds = ObjBAL.GetAgentCode();
                //ddlOldAgentCode.Items.Clear();
                //ddlOldAgentCode.DataSource = ds.Tables[0];
                //ddlOldAgentCode.DataTextField = "AgentCode";
                //ddlOldAgentCode.DataValueField = "AgentCode";
                //ddlOldAgentCode.DataBind();
                //ddlOldAgentCode.SelectedIndex = 0;
            }
        }

        #endregion

        #region Validate Controls
        public void ValidateControls(object sender, EventArgs e)
        {
            try
            {
                if (txtInvoiceNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter Invoice No";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBPrifix.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtDateAWBPre.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtDateAWBNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBDate.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                //return true;
            }

            catch (Exception ex)
            {
                //return false;
            }


        }

        #endregion

        #region Button Invoice Details
        protected void BtnOk_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = false;
            lblStatus.Text="";

            try
            {
                if (txtInvoiceNo.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter Invoice No";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                DataSet DsInvoice = new DataSet();
                DsInvoice = ObjBAL.GetInvoiceDetails(txtInvoiceNo.Text.Trim());

                if (DsInvoice != null && DsInvoice.Tables.Count > 0 && DsInvoice.Tables[0].Rows.Count > 0)
                {
                    grdInvoiceList.DataSource = DsInvoice.Tables[0];
                    grdInvoiceList.DataBind();
                    //BtnReopen.Enabled = true;
                    BtnReopen.Visible = true;
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Button Reopen

        protected void BtnReopen_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                

                bool Result = false;
                Result = ObjBAL.ReopenInvoice(txtInvoiceNo.Text.Trim(),Convert.ToDateTime(Session["IT"]),
                    Session["UserName"].ToString());

                if (Result==true)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Invoice Reopened successfully!";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Invoice was not Reopened";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception  ex)
            {
               lblStatus.Text = "Error:" + ex.Message;
               lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion
        
        #region Button Cancel
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtInvoiceNo.Text = string.Empty;
            BtnReopen.Visible = false;
            grdInvoiceList.DataSource = null;
            grdInvoiceList.DataBind();
            lblStatus.Text = string.Empty;
        }
        #endregion

        #region Button Agent Code List
        protected void BtnAgentList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifix.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNo.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DataSet dsAgentList = new DataSet();
                //dsAgentList = ObjBAL.GetAgentCodeDetails(txtAWBPrifix.Text.ToString(), txtAWBNo.Text.ToString(), ddlOldAgentCode.SelectedItem.Text);

                dsAgentList = ObjBAL.GetAgentCodeDetails(txtAWBPrifix.Text.ToString(), txtAWBNo.Text.ToString());


                if (dsAgentList != null && dsAgentList.Tables.Count > 0 && dsAgentList.Tables[0].Rows.Count > 0)
                {
                    grdAgentCode.DataSource = dsAgentList.Tables[0];
                    grdAgentCode.DataBind();

                    LblNewAgentCcode.Visible = true;
                    ddlNewAgtCode.Visible = true;
                    BtnChgAgtCd.Visible = true;
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion

        #region Button AWB DateWise Details 
        protected void BtnAWBDateList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtDateAWBPre.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtDateAWBNo.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DataSet dsAWBDate = new DataSet();

                dsAWBDate = ObjBAL.GetAWBDateWiseDetails(txtDateAWBPre.Text.ToString(), txtDateAWBNo.Text.ToString(), txtAWBDate.Text.ToString());

                if (dsAWBDate != null && dsAWBDate.Tables.Count > 0 && dsAWBDate.Tables[0].Rows.Count > 0)
                {
                    grdAWBDate.DataSource = dsAWBDate.Tables[0];
                    grdAWBDate.DataBind();
                    LblNewAWBDt.Visible = true;
                    txtAWBNewDt.Visible = true;
                    BtnChangeDt.Visible = true;
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion
        
        #region Button Change AWB Date
        protected void BtnChangeDt_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";
                string strtodate;

                if (txtAWBNewDt.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter new AWB Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DateTime dtNewdate;

                try
                {
                    //dtto = Convert.ToDateTime(txtbillingto.Text);
                    //Change 03082012
                    string day = txtAWBNewDt.Text.Substring(0, 2);
                    string mon = txtAWBNewDt.Text.Substring(3, 2);
                    string yr = txtAWBNewDt.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtNewdate = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "New AWB Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DataSet dsAWBNewDate = new DataSet();

                dsAWBNewDate = ObjBAL.SetAWBDate(txtDateAWBPre.Text.ToString(), txtDateAWBNo.Text.ToString(), 
                    txtAWBNewDt.Text.ToString(),Convert.ToDateTime(Session["IT"]),Session["UserName"].ToString());

                if (dsAWBNewDate != null && dsAWBNewDate.Tables.Count > 0 && dsAWBNewDate.Tables[0].Rows.Count > 0)
                {

                    if (dsAWBNewDate.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB INVOICED")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Cannot change AWB date as AWB is already invoiced.";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else if (dsAWBNewDate.Tables[0].Rows[0][0].ToString().ToUpper() == "UPDATED")
                    {
                        dsAWBNewDate = ObjBAL.GetAWBDateWiseDetails(txtDateAWBPre.Text.ToString(), txtDateAWBNo.Text.ToString(), txtAWBDate.Text.ToString());

                        grdAWBDate.DataSource = dsAWBNewDate.Tables[0];
                        grdAWBDate.DataBind();
                        lblStatus.Visible = true;
                        lblStatus.Text = "Record updated successfully!";
                        lblStatus.ForeColor = Color.Green;
                    }
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record is not updated";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion 

        #region Button AWB Date Clear
        protected void BtnClear3_Click(object sender, EventArgs e)
        {
            txtDateAWBPre.Text = txtDateAWBNo.Text = txtAWBDate.Text = string.Empty;
            grdAWBDate.DataSource = null;
            grdAWBDate.DataBind();
            LblNewAWBDt.Visible = false;
            txtAWBNewDt.Text = string.Empty;
            txtAWBNewDt.Visible = false;
            BtnChangeDt.Visible = false;
            lblStatus.Text = string.Empty;
        }
        #endregion

        #region Button Agent Code Clear
        protected void BtnClear2_Click(object sender, EventArgs e)
        {
            txtAWBPrifix.Text = txtAWBNo.Text = string.Empty;
            grdAgentCode.DataSource = null;
            grdAgentCode.DataBind();
            LblNewAgentCcode.Visible = false;
            ddlNewAgtCode.Visible = false;
            BtnChgAgtCd.Visible = false;
            lblStatus.Text = string.Empty;
            
        }
        #endregion

        #region Button Change Agent Code
        protected void BtnChgAgtCd_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (ddlNewAgtCode.SelectedIndex == 0)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select New Agent Code";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //bool Result = false;
                DataSet dsResult;

                string UserName = Session["UserName"].ToString();

                DateTime UpdateOn = System.DateTime.Now;

                dsResult = ObjBAL.SetAgentCode(txtAWBPrifix.Text.ToString(), txtAWBNo.Text.ToString(), ddlNewAgtCode.SelectedItem.Text, UserName, UpdateOn);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB INVOICED")
                            {
                                lblStatus.Visible = true;
                                lblStatus.Text = "Agent Code cannot be changed as AWB is already Invoiced";
                                lblStatus.ForeColor = Color.Red;
                            }
                            else if (dsResult.Tables[0].Rows[0][0].ToString().ToUpper() == "AGENT UPDATED")
                            {
                                DataSet dsAgentList = new DataSet();
                                dsAgentList = ObjBAL.GetAgentCodeDetails(txtAWBPrifix.Text.ToString(), txtAWBNo.Text.ToString());

                                if (dsAgentList != null && dsAgentList.Tables.Count > 0 && dsAgentList.Tables[0].Rows.Count > 0)
                                {
                                    grdAgentCode.DataSource = dsAgentList.Tables[0];
                                    grdAgentCode.DataBind();
                                    lblStatus.Visible = true;
                                    lblStatus.Text = "Agent Code Updated Successfully";
                                    lblStatus.ForeColor = Color.Green;
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record is not updated";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;              
            }

        }

        #endregion

        #region AWB List
      /*  protected void BtnListAWB_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifix.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNo.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DataSet dsAWBList = new DataSet();
                //DataSet dsAgentList = new DataSet();
                //dsAgentList = ObjBAL.GetAgentCodeDetails(txtAWBPrifix.Text.ToString(), txtAWBNo.Text.ToString(), ddlOldAgentCode.SelectedItem.Text);
                dsAWBList = ObjBAL.GetAWBList(txtAWBPrifix.Text.ToString(), txtAWBNo.Text.ToString());

                //dsAgentList = ObjBAL.GetAgentCodeDetails(txtAWBPrifix.Text.ToString(), txtAWBNo.Text.ToString());


                if (dsAWBList != null && dsAWBList.Tables.Count > 0 && dsAWBList.Tables[0].Rows.Count > 0)
                {
                    grdAgentCode.DataSource = dsAWBList.Tables[0];
                    grdAgentCode.DataBind();

                    LblNewAgentCcode.Visible = true;
                    ddlNewAgtCode.Visible = true;
                    BtnChgAgtCd.Visible = true;
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }*/
        #endregion

        #region AWB Void with Zero Changes
        
        protected void BtnAWBVoid_Click(object sender, EventArgs e)
        {
           
        
        }
        #endregion

        #region Get AWB List to Void
        protected void BtnVoidAWB_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifix1.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNo1.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                DataSet dsAWBListToVoid=new DataSet();
             
                dsAWBListToVoid = ObjBAL.GetAWBListToVoid(txtAWBPrifix1.Text.ToString(), txtAWBNo1.Text.ToString());

                if (dsAWBListToVoid != null && dsAWBListToVoid.Tables.Count > 0 && dsAWBListToVoid.Tables[0].Rows.Count > 0)
                {
                    
                    grdAWBList.DataSource = dsAWBListToVoid.Tables[0];
                    grdAWBList.DataBind();
                    BtnVoidListedAWB.Visible = true;
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Make AWB Void with Zero Changes
        protected void BtnVoidListedAWB_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifix1.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNo1.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DataSet dsAWBList1 = new DataSet();
                //bool result;
                dsAWBList1 = ObjBAL.AWBVoid(txtAWBPrifix1.Text.ToString(), txtAWBNo1.Text.ToString(),Session["UserName"].ToString(),
                    Convert.ToDateTime(Session["IT"]));

                if (dsAWBList1 != null)
                {
                    if (dsAWBList1.Tables.Count > 0)
                    {
                        if (dsAWBList1.Tables[0].Rows.Count > 0)
                        {
                            if (dsAWBList1.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB INVOICED")
                            {
                                lblStatus.Visible = true;
                                lblStatus.Text = "AWB cannot be voided as AWB is already Invoiced";
                                lblStatus.ForeColor = Color.Red;
                            }
                            else if (dsAWBList1.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB VOIDED")
                            {
                                BtnVoidAWB_Click(null, null);
                                lblStatus.Text = "Record updated successfully!";
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Visible = true;
                            }
                            else
                            {
                            }
                        }
                    }
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record is not updated";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Get AWB List to ReProcess Rate
        protected void BtnListAWBReProcess_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifixReProcess.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNoReProcess.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

              
                DataSet dsAWBListReprocess = new DataSet();

                dsAWBListReprocess = ObjBAL.GetAWBListToReProcess(txtAWBPrifixReProcess.Text.ToString(), txtAWBNoReProcess.Text.ToString());

                if (dsAWBListReprocess != null && dsAWBListReprocess.Tables.Count > 0 && dsAWBListReprocess.Tables[0].Rows.Count > 0)
                {
                    GrdListAWBtoReProcess.DataSource = dsAWBListReprocess.Tables[0];
                    GrdListAWBtoReProcess.DataBind();
                    lblNewRate.Visible = true;
                    txtAWBNewRatePerKg.Visible = true;
                    BtnRateReProcess.Visible = true;
                }

                else
                {
                    GrdListAWBtoReProcess.DataSource = null;
                    GrdListAWBtoReProcess.DataBind();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Change Rate perKg to Reprocess
        protected void BtnRateReProcess_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifixReProcess.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNoReProcess.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (txtAWBNewRatePerKg.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter New Rate PerKg";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                DataSet dsAWBChangeRate = new DataSet();

                //bool result= ObjBAL.ChangeAWBRate(txtAWBPrifixReProcess.Text.ToString(), txtAWBNoReProcess.Text.ToString(), txtAWBNewRatePerKg.Text.ToString());
                dsAWBChangeRate = ObjBAL.ChangeAWBRate(txtAWBPrifixReProcess.Text.ToString(), 
                    txtAWBNoReProcess.Text.ToString(), txtAWBNewRatePerKg.Text.ToString(),
                    Convert.ToDateTime(Session["IT"].ToString()),Session["UserName"].ToString());

                if (dsAWBChangeRate != null && dsAWBChangeRate.Tables.Count > 0 && dsAWBChangeRate.Tables[0].Rows.Count > 0)
                {

                    if (dsAWBChangeRate.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB INVOICED")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Cannot change Rate as AWB is already invoiced.";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else if (dsAWBChangeRate.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB FINAL")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Cannot change Rate as AWB is Finalised in Billing.";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else if (dsAWBChangeRate.Tables[0].Rows[0][0].ToString().ToUpper() == "RATE UPDATED")
                    {
                        BtnListAWBReProcess_Click(null, null);
                        lblStatus.Visible = true;
                        lblStatus.Text = "Rate updated successfully!";
                        lblStatus.ForeColor = Color.Green;
                    }
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record is not updated";
                    lblStatus.ForeColor = Color.Red;
                }
              
                //if (result == true)
                //{
                   
                //    BtnListAWBReProcess_Click(null, null);
                //    lblStatus.Visible = true;
                //    lblStatus.Text = "Record Updated Successfully";
                //    lblStatus.ForeColor = Color.Green;

                //}

                //else
                //{
                //    lblStatus.Visible = true;
                //    lblStatus.Text = "No Records Found";
                //    lblStatus.ForeColor = Color.Red;
                //}

            }
            catch(Exception ex)
            {
            }


        }
        #endregion

        protected void btnListAWBDetailsSoptrate_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifixSpotrate.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (txtAWBNoSpotrate.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
                DataSet dsAWBDetailsSpotrate = new DataSet();
                dsAWBDetailsSpotrate = ObjBAL.AWBDetailsSpotrate(txtAWBPrifixSpotrate.Text.ToString(), txtAWBNoSpotrate.Text.ToString());

                if (dsAWBDetailsSpotrate != null && dsAWBDetailsSpotrate.Tables.Count > 0 && dsAWBDetailsSpotrate.Tables[0].Rows.Count > 0)
                {
                    grdAWBListSpotrate.DataSource = dsAWBDetailsSpotrate.Tables[0];
                    grdAWBListSpotrate.DataBind();
                    btnRemoveAWBSpotrate.Visible = true;
                }

                else
                {
                    grdAWBListSpotrate.DataSource = null;
                    grdAWBListSpotrate.DataBind();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch(Exception ex)
            {
            }
        }

        protected void btnRemoveAWBSpotrate_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrifixSpotrate.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNoSpotrate.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }


                //bool result;
                DataSet dsRemoveSpot = new DataSet();
                //result = ObjBAL.DeleteAWBSpotRate(txtAWBPrifixSpotrate.Text.ToString(), txtAWBNoSpotrate.Text.ToString());
                dsRemoveSpot = ObjBAL.DeleteAWBSpotRate(txtAWBPrifixSpotrate.Text.ToString(), 
                    txtAWBNoSpotrate.Text.ToString(),Convert.ToDateTime(Session["IT"]),Session["UserName"].ToString());

                if (dsRemoveSpot != null)
                {
                    if (dsRemoveSpot.Tables.Count > 0)
                    {
                        if (dsRemoveSpot.Tables[0].Rows.Count > 0)
                        {
                            if (dsRemoveSpot.Tables[0].Rows[0][0].ToString().ToUpper() == "SPOT RATE APPLIED")
                            {
                                lblStatus.Visible = true;
                                lblStatus.Text = "Spot rate already applied for this AWB";
                                lblStatus.ForeColor = Color.Red;
                            }
                            else //if (dsRemoveSpot.Tables[0].Rows[0][0].ToString().ToUpper() == "SPOT RATE DELETED")
                            {
                                btnListAWBDetailsSoptrate_Click(null, null);
                                
                                lblStatus.Visible = true;
                                lblStatus.Text = "Spot rate deleted successfully";
                                lblStatus.ForeColor = Color.Green;
                            }
                            //else
                            //{
                            //}
                        }
                    }
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record is not updated";
                    lblStatus.ForeColor = Color.Red;
                }

                //if (result == true)
                //{
                //    lblStatus.Visible = true;
                //    lblStatus.Text = "Record Deleted Successfully";
                //    lblStatus.ForeColor = Color.Green;
                //}

                //else
                //{
                //    lblStatus.Visible = true;
                //    lblStatus.Text = "No Records Found";
                //    lblStatus.ForeColor = Color.Red;
                //}

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnListAWBDCM_Click(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrefixDCM.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNoDCM.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtFlightNoDCM.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter Flight Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtFlightDateDCM.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter Flight Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                string strtodate;
                DateTime dtNewdate;

                try
                {
                   
                    string day = txtFlightDateDCM.Text.Substring(0, 2);
                    string mon = txtFlightDateDCM.Text.Substring(3, 2);
                    string yr = txtFlightDateDCM.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtNewdate = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "New AWB Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }


                DataSet dsAWBDetailsDCM = new DataSet();
                dsAWBDetailsDCM = ObjBAL.AWBDetailsDCM(txtAWBPrefixDCM.Text.ToString(), txtAWBNoDCM.Text.ToString(), txtFlightNoDCM.Text.ToString(),txtFlightDateDCM.Text.ToString());

                if (dsAWBDetailsDCM != null && dsAWBDetailsDCM.Tables.Count > 0 && dsAWBDetailsDCM.Tables[0].Rows.Count > 0)
                {
                    grdListAWBDCM.DataSource = dsAWBDetailsDCM.Tables[0];
                    grdListAWBDCM.DataBind();
                    btnRemoveAWBDCM.Visible = true;
                }

                else
                {
                    grdListAWBDCM.DataSource = null;
                    grdListAWBDCM.DataBind();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnRemoveAWBDCM_Click(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrefixDCM.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNoDCM.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtFlightNoDCM.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter Flight Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                bool result;
                result = ObjBAL.DeleteAWBDCM(txtAWBPrefixDCM.Text.Trim(), txtAWBNoDCM.Text.Trim(), 
                    txtFlightNoDCM.Text.Trim(), txtFlightDateDCM.Text.Trim(),Session["UserName"].ToString(),
                    Convert.ToDateTime(Session["IT"]));

                if (result == true)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "DCM Deleted Successfully";
                    lblStatus.ForeColor = Color.Green;
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
            }

        }

        protected void btnListAWBDetailsST_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                if (txtAWBPrefixST.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Prefix";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNoST.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                //DataSet dsAWBListToVoid = new DataSet();
                DataSet dsAWBListToST = new DataSet();
                dsAWBListToST = ObjBAL.GetAWBlistToST(txtAWBPrefixST.Text.ToString(), txtAWBNoST.Text.ToString());

                if (dsAWBListToST != null && dsAWBListToST.Tables.Count > 0 && dsAWBListToST.Tables[0].Rows.Count > 0)
                {

                   grdAWBListST.DataSource = dsAWBListToST.Tables[0];
                   grdAWBListST.DataBind();
                    BtnVoidListedAWB.Visible = true;
                }

                else
                {
                    grdAWBListST.DataSource = null;
                    grdAWBListST.DataBind();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected void btnCorrectST_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";
                string UpdatedBy = Session["Username"].ToString();
                DateTime UpdatedOn = Convert.ToDateTime(Session["IT"].ToString());
                //DataSet dsAWBNewDate = new DataSet();
                DataSet dsAWBUpdateST = new DataSet();
                dsAWBUpdateST = ObjBAL.UpdateServiceTax(txtAWBPrefixST.Text.ToString(), txtAWBNoST.Text.ToString(), UpdatedBy,UpdatedOn);


                if (dsAWBUpdateST != null && dsAWBUpdateST.Tables.Count > 0 && dsAWBUpdateST.Tables[0].Rows.Count > 0)
                {

                    if (dsAWBUpdateST.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB INVOICED")
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Cannot change Service Tax as AWB is already invoiced.";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else if (dsAWBUpdateST.Tables[0].Rows[0][0].ToString().ToUpper() == "AWB FINAL")
               
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Cannot change Service Tax as AWB is Finalised in Billing.";
                        lblStatus.ForeColor = Color.Red;
                    }
                    else if (dsAWBUpdateST.Tables[0].Rows[0][0].ToString().ToUpper() == "ST UPDATED")
                    {
                        dsAWBUpdateST = ObjBAL.GetAWBDateWiseDetails(txtDateAWBPre.Text.ToString(), txtDateAWBNo.Text.ToString(), txtAWBDate.Text.ToString());

                        grdAWBListST.DataSource = dsAWBUpdateST.Tables[0];
                        grdAWBListST.DataBind();
                        lblStatus.Visible = true;
                        lblStatus.Text = "Service Tax updated successfully!";
                        lblStatus.ForeColor = Color.Green;
                    }
                }

                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Record is not updated";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected void btnClearVoid_Click(object sender, EventArgs e)
        {
            txtAWBPrifix1.Text = txtAWBNo1.Text = string.Empty;
            grdAWBList.DataSource = null;
            grdAWBList.DataBind();
            BtnVoidListedAWB.Visible = false;
        }

    }
}
