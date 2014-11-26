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
                Result = ObjBAL.ReopenInvoice(txtInvoiceNo.Text.Trim());

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

                dsAWBNewDate = ObjBAL.SetAWBDate(txtDateAWBPre.Text.ToString(), txtDateAWBNo.Text.ToString(), txtAWBNewDt.Text.ToString());


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
            txtAWBPrifix.Text = txtAWBNo.Text = txtAWBDate.Text = string.Empty;
            grdAWBDate.DataSource = null;
            grdAWBDate.DataBind();
            LblNewAWBDt.Visible = false;
            txtAWBNewDt.Text = string.Empty;
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


    }
}
