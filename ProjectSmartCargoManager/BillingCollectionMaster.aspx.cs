using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Drawing;
using QID.DataAccess;
using Microsoft.Reporting.WebForms;


namespace ProjectSmartCargoManager
{
    public partial class BillingCollectionMaster : System.Web.UI.Page
    {
        BillingAWBFileInvoiceBAL objBillBAL = new BillingAWBFileInvoiceBAL();
        BALCollectionDetails objBAL = new BALCollectionDetails();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BALAgentCredit ObjTrans = new BALAgentCredit();
        DateTime dtCurrentDate = DateTime.Now;

        string strfromdate, strtodate, strchequedate;
        protected void Page_Load(object sender, EventArgs e)
        {
            dtCurrentDate = (DateTime)Session["IT"];
            if (!IsPostBack)
            {
                try
                {
                    txtInvoiceFrom.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                    txtInvoiceTo.Text = dtCurrentDate.ToString("dd/MM/yyyy");

                    LoadAgentDropdown(); //AgentName 
                    LoadAgentCodeDropdown(); //AgentCode

                    //Agent authorization
                    string AgentCode = Convert.ToString(Session["AgentCode"]);

                    if (AgentCode != "")
                    {
                        ddlAgentName.SelectedValue = AgentCode;
                        ddlAgentName_SelectedIndexChanged(null, null);
                        ddlAgentCode.Enabled = false;
                        ddlAgentName.Enabled = false;

                    }

                    //By default Cash is selected so disable bank textbox
                    txtChequeDdNo.Enabled = false;
                    txtChequeDate.Enabled = false;
                    txtBankName.Enabled = false;

                }
                catch (Exception)
                {
                }

                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdInvoiceList.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
        }

        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            DataSet ds = objBillBAL.GetAllAgents();
            if (ds != null)
            {
                ddlAgentName.DataSource = ds;
                ddlAgentName.DataMember = ds.Tables[0].TableName;
                ddlAgentName.DataTextField = "AgentName";
                ddlAgentName.DataValueField = "AgentCode";
                ddlAgentName.DataBind();
                ddlAgentName.Items.Insert(0, new ListItem("All", ""));
                ddlAgentName.SelectedIndex = -1;
            }
        }
        #endregion LoadAgentDropdown

        #region Load Payment Types
        public void LoadPaymentTypes()
        {
            DataSet ds = objBAL.GetAllPaymentTypes();
            if (ds != null)
            {
                ddlPaymentType.DataSource = ds;
                ddlPaymentType.DataMember = ds.Tables[0].TableName;
                ddlPaymentType.DataTextField = "PaymentType";
                ddlPaymentType.DataValueField = "PaymentType";
                ddlPaymentType.DataBind();
            }
        }
        #endregion Load Payment Types

        #region Load Agent Code Dropdown
        public void LoadAgentCodeDropdown()
        {
            DataSet ds = objBillBAL.GetAllAgents();
            if (ds != null)
            {
                ddlAgentCode.DataSource = ds;
                ddlAgentCode.DataMember = ds.Tables[0].TableName;
                ddlAgentCode.DataTextField = "AgentCode";
                ddlAgentCode.DataValueField = "AgentName";
                ddlAgentCode.DataBind();
                ddlAgentCode.Items.Insert(0, new ListItem("All", ""));
                ddlAgentCode.SelectedIndex = -1;
            }
        }
        #endregion LoadAgentCodeDropdown

        protected void ddlAgentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentName.SelectedIndex = ddlAgentCode.SelectedIndex;
        }

        protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAgentCode.SelectedIndex = ddlAgentName.SelectedIndex;
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            bindCollectionDetails();
        }

        protected void disableForAgent()
        {
            pnlAddCollection.Visible = false;
            pnlDCMAdjustment.Visible = false;
        }

        protected void rbSelect_CheckedChanged(object sender, System.EventArgs e)
        {
            //Clear the existing selected row 
            foreach (GridViewRow oldrow in grdInvoiceList.Rows)
            {
                ((RadioButton)oldrow.FindControl("rbSelect")).Checked = false;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("rbSelect")).Checked = true;

            //Get the values in textboxes from selected row
            txtCollectedAmount.Text = ((Label)row.FindControl("lblCollectedAmount")).Text;
            txtTDS.Text = ((Label)row.FindControl("lblTDSAmount")).Text;
            if (((Label)row.FindControl("lblPaymentType")).Text != "")
            {
                ddlPaymentType.SelectedValue = ((Label)row.FindControl("lblPaymentType")).Text;
            }
            txtChequeDdNo.Text = ((Label)row.FindControl("lblChequeDdNo")).Text;
            txtChequeDate.Text = ((Label)row.FindControl("lblChequeDate")).Text;
            txtBankName.Text = ((Label)row.FindControl("lblBankName")).Text;
            txt194C.Text = ((Label)row.FindControl("lbl194CAmount")).Text;
            hdPendingAmt.Value = ((Label)row.FindControl("lblPendingAmount")).Text;
            hdTransactionId.Value = "";
            ddlPaymentType_SelectedIndexChanged(null, null);
        }

        protected void bindCollectionDetails()
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                //Validation for From date
                if (txtInvoiceFrom.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;

                try
                {
                    //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                    //Change 03082012
                    string day = txtInvoiceFrom.Text.Substring(0, 2);
                    string mon = txtInvoiceFrom.Text.Substring(3, 2);
                    string yr = txtInvoiceFrom.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Validation for To date
                if (txtInvoiceTo.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                    //Change 03082012
                    string day = txtInvoiceTo.Text.Substring(0, 2);
                    string mon = txtInvoiceTo.Text.Substring(3, 2);
                    string yr = txtInvoiceTo.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dt)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;

                    return;
                }

                DataSet DSInvoicedata = objBAL.GetCollectionMasterData(ddlAgentName.SelectedValue, ddlBillType.SelectedValue, txtOrigin.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), txtInvoiceNumber.Text.Trim(), txtAWBPrefix.Text.Trim() + txtAWBNumber.Text.Trim(),"All");

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    Session["dsInvoiceData"] = DSInvoicedata;
                    grdInvoiceList.DataSource = DSInvoicedata.Tables[0];
                    grdInvoiceList.DataBind();

                    LoadPaymentTypes();

                    grdInvoiceList.Visible = true;
                    pnlAddCollection.Visible = true;
                    pnlDCMAdjustment.Visible = true;
                    btnExport.Visible = true;
                    lblStatus.Visible = false;
                    editCollectionGridInvoiceAmt();
                    editCollectionGridPendingAmt();
                    txtCollectedAmount.Text = "";
                    txtTDS.Text = "";
                    txtChequeDdNo.Text = "";
                    txtChequeDate.Text = "";
                    txtBankName.Text = "";
                    txtDCMNumber.Text = "";
                    txtDCMAmount.Text = "";
                    txtReason.Text = "";
                    txt194C.Text = "";

                    //Agent authorization
                    if (Convert.ToString(Session["AgentCode"]) != "")
                    {
                        disableForAgent();
                    }
                }
                else
                {
                    grdInvoiceList.Visible = false;
                    pnlAddCollection.Visible = false;
                    pnlDCMAdjustment.Visible = false;
                    btnExport.Visible = false;
                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Blue;
                }

            }
            catch (Exception ex)
            {

            }

        }

        protected void editCollectionGridInvoiceAmt()
        {
            string OldInvNo="", NewInvNo="";
            int FirstLoop = 0;
            for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
            {
                if(FirstLoop == 0)
                    OldInvNo = ((Label)grdInvoiceList.Rows[0].FindControl("lblInvoiceNumber")).Text;
                else
                    OldInvNo = ((Label)grdInvoiceList.Rows[y-1].FindControl("lblInvoiceNumber")).Text;

                if (grdInvoiceList.Rows.Count > y)
                {
                    NewInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                }
                if (NewInvNo == OldInvNo && y != 0)
                {
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceAmount")).Visible  = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblCentralAgent")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblLocalAgent")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lbl194CAmount")).Visible = false;
                }
                else
                {
                    FirstLoop = 1;
                    OldInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                }
            }
        }

        protected void editCollectionGridPendingAmt()
        {
            string OldInvNo = "", NewInvNo = "";
            int FirstLoop = 0;
            for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
            {
                if (FirstLoop == 0)
                    OldInvNo = ((Label)grdInvoiceList.Rows[0].FindControl("lblInvoiceNumber")).Text;
                else
                    OldInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;

                if (grdInvoiceList.Rows.Count - 1 > y)
                {

                    NewInvNo = ((Label)grdInvoiceList.Rows[y + 1].FindControl("lblInvoiceNumber")).Text;
                    if (NewInvNo == OldInvNo)
                    {
                        ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Visible = false;
                    }
                    else
                    {
                        FirstLoop = 1;
                        OldInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                    }
                }  
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                invNo = "";
                string AgentName = "";
                string PendingAmt = "";
                string TransactionType = "";
                string CollectedAmt = txtCollectedAmount.Text.Trim();
                string PaymentType = ddlPaymentType.SelectedItem.Text;
                

                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                    {
                        invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                        AgentName = ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Text;
                        PendingAmt = ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Text;
                       
                    }
                }

                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                if (!Validation())
                    return;

                #region Prepare Parameters
                object[] RateCardInfo = new object[14];
                int i = 0;

                RateCardInfo.SetValue(invNo, i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtCollectedAmount.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtTDS.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(ddlPaymentType.SelectedValue, i);
                i++;

                RateCardInfo.SetValue(txtChequeDdNo.Text.Trim(), i);
                i++;

                if (txtChequeDate.Text.Trim() != "")
                    RateCardInfo.SetValue(Convert.ToDateTime(strchequedate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                else
                    RateCardInfo.SetValue(txtChequeDate.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(txtBankName.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                i++;

                RateCardInfo.SetValue(txtPPRemarks.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(txt194C.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(hdTransactionId.Value.Trim(), i);
                i++;

                //RateCardInfo.SetValue(System.DateTime.Now, i);
                RateCardInfo.SetValue(Convert.ToDateTime(Session["IT"]), i);

                i++;
                //new added
                RateCardInfo.SetValue("", i);

                i++;
                RateCardInfo.SetValue(Convert.ToDouble(0), i);

                

                #endregion Prepare Parameters

                string res = "";
                res = objBAL.AddInvoiceCollectionDetails(RateCardInfo);

                if (res != "error")
                {
                    bindCollectionDetails();
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    try
                    {
                        
                        if (CollectedAmt == "")
                        { CollectedAmt = "0"; }
                       
                            TransactionType = "Credit";
                        
                     
                        if (ObjTrans.SaveTransacation(AgentName, "",PaymentType,TransactionType,0, 0, Convert.ToDouble(CollectedAmt), txtPPRemarks.Text, Session["UserName"].ToString(), "", invNo))
                        {
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = ex.Message;
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected bool Validation()
        {
            if (txtCollectedAmount.Text.Trim() == "" && txtTDS.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter Collected amount or TDS ";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (txtCollectedAmount.Text.Trim() == "")
                txtCollectedAmount.Text = "0";
            if (txtTDS.Text.Trim() == "")
                txtTDS.Text = "0";

            if (txt194C.Text.Trim() == "")
                txt194C.Text = "0";



            if (ddlPaymentType.SelectedValue == "Cheque" || ddlPaymentType.SelectedValue == "DD" || ddlPaymentType.SelectedValue == "RTGS")
            {
                if (txtChequeDdNo.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please enter Cheque/DD/RTGS/Card Number ";
                    lblStatus.ForeColor = Color.Blue;
                    return false;
                }
            }
            if (ddlPaymentType.SelectedValue == "Cheque" && (txtChequeDate.Text.Trim() == "" || txtBankName.Text.Trim() == ""))
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter Cheque date and Bank name ";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if(txtChequeDate.Text.Trim() != "")
            {
                DateTime dt;

                try
                {
                    string day = txtChequeDate.Text.Substring(0, 2);
                    string mon = txtChequeDate.Text.Substring(3, 2);
                    string yr = txtChequeDate.Text.Substring(6, 4);
                    strchequedate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strchequedate);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Selected Date format invalid');</SCRIPT>");
                    lblStatus.Text = "Cheque date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }


            return true;
        }

        protected bool DCMValidation()
        {
            if (txtDCMAmount.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter DCM amount";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }
            if (txtDCMNumber.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter DCM number";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            if (txtReason.Text.Trim() == "")
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Please enter reason";
                lblStatus.ForeColor = Color.Blue;
                return false;
            }

            return true;
        }

        protected void grdInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            //    chkSelect.Attributes.Add("onclick", "javascript:return DoPostBackWithRowIndex('" + e.Row.RowIndex + "');");
            //    //TxtCollAmt.Attributes.Add("onkeydown", "javascript:return DoPostBackWithRowIndex('" + e.Row.RowIndex + "');");
            //}
            DataSet dsTotal = (DataSet )Session["dsInvoiceData"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblGTotalText = (Label)e.Row.FindControl("lblGTotalText");
                Label lblGTotalInvoiceAmt = (Label)e.Row.FindControl("lblGTotalInvoiceAmt");
                Label lblGTotalCollectedAmt = (Label)e.Row.FindControl("lblGTotalCollectedAmt");
                Label lblGTotal194CAmt = (Label)e.Row.FindControl("lblGTotal194CAmt");
                Label lblGTotalPendingAmt = (Label)e.Row.FindControl("lblGTotalPendingAmt");


                lblGTotalText.Text = "Grant Total";
                lblGTotalInvoiceAmt.Text = dsTotal.Tables[1].Rows[0]["InvoiceAmount"].ToString();
                lblGTotalCollectedAmt.Text = dsTotal.Tables[1].Rows[0]["CollectedAmount"].ToString();
                lblGTotal194CAmt.Text = dsTotal.Tables[1].Rows[0]["Amt194C"].ToString();
                lblGTotalPendingAmt.Text = dsTotal.Tables[1].Rows[0]["PendingAmount"].ToString();

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                
                int SrNo; SrNo = 0;
                invNo = "";
                string AgentName = "";
                string PendingAmt = "";
                string TransactionType = "";
                string CollectedAmt = txtCollectedAmount.Text.Trim();
                string CollAmt2 = "";
                string PaymentType = ddlPaymentType.SelectedItem.Text;

                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                    {
                        SrNo = Convert.ToInt32(((Label)grdInvoiceList.Rows[y].FindControl("lblSrNo")).Text);
                        invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                        AgentName = ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Text;
                        PendingAmt = ((Label)grdInvoiceList.Rows[y].FindControl("lblPendingAmount")).Text;
                        CollAmt2 = ((Label)grdInvoiceList.Rows[y].FindControl("lblCollectedAmount")).Text;
                    }
                }

                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                #region Prepare Parameters
                object[] RateCardInfo = new object[13];
                int i = 0;

                RateCardInfo.SetValue(invNo, i);
                i++;

                RateCardInfo.SetValue(SrNo, i);
                i++;
                
                RateCardInfo.SetValue(Convert.ToDouble(txtCollectedAmount.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtTDS.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(ddlPaymentType.SelectedValue, i);
                i++;

                RateCardInfo.SetValue(txtChequeDdNo.Text.Trim(), i);
                i++;
                
                RateCardInfo.SetValue(txtChequeDate.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(txtBankName.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                i++;

                //added by jayant
                RateCardInfo.SetValue(txtPPRemarks.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(txt194C.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue("", i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(0), i);


                #endregion Prepare Parameters

                string res = "";
                res = objBAL.EditInvoiceCollectionDetails(RateCardInfo);

                if (res != "error")
                {
                    bindCollectionDetails();
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    try
                    {
                        if (CollectedAmt == "")
                        { CollectedAmt = "0"; }

                        double ColAmt = Convert.ToDouble(CollAmt2) - Convert.ToDouble(CollectedAmt);
                        if (Convert.ToDouble(ColAmt) > 0)
                        {
                            TransactionType = "Debit";
                        }
                        else
                        {
                            TransactionType = "Credit";
                            ColAmt = -ColAmt;
                        }
                        if (ObjTrans.SaveTransacation(AgentName, "", PaymentType, TransactionType, 0, 0, ColAmt, txtPPRemarks.Text, Session["UserName"].ToString(), "", invNo))
                        {
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                invNo = "";
                int SrNo; SrNo = 0;
                string Amount = "";
                string PaymentType = ddlPaymentType.SelectedItem.Text;
                string AgentName = "";

                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                    {
                        invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                        SrNo = Convert.ToInt32(((Label)grdInvoiceList.Rows[y].FindControl("lblSrNo")).Text);
                        Amount = ((Label)grdInvoiceList.Rows[y].FindControl("lblCollectedAmount")).Text;
                        AgentName = ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Text;
                    }
                }

                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                #region Prepare Parameters
                object[] RateCardInfo = new object[7];
                int i = 0;

                RateCardInfo.SetValue(invNo, i);
                i++;

                RateCardInfo.SetValue(SrNo, i);
                i++;

                RateCardInfo.SetValue(Convert.ToDouble(txtCollectedAmount.Text.Trim()), i);
                i++;

                RateCardInfo.SetValue(ddlPaymentType.SelectedValue, i);
                i++;

                RateCardInfo.SetValue(txtChequeDdNo.Text.Trim(), i);
                i++;

                RateCardInfo.SetValue(Session["UserName"].ToString(), i);
                i++;

                RateCardInfo.SetValue("", i);

                #endregion Prepare Parameters

                string res = "";
                res = objBAL.DeleteInvoiceCollectionDetails(RateCardInfo);

                if (res != "error")
                {
                    bindCollectionDetails();
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    try
                    {
                        if (ObjTrans.SaveTransacation(AgentName, "", PaymentType, "Debit", 0, 0, Convert.ToDouble(Amount), "Remarks", Session["UserName"].ToString(), "", invNo))
                        {
                            // lblStatus.Text = "True";
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void grdInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Session["dsInvoiceData"]
            DataSet dst = (DataSet)Session["dsInvoiceData"];
            grdInvoiceList.PageIndex = e.NewPageIndex;
            grdInvoiceList.DataSource = dst.Tables[0];
            grdInvoiceList.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsInvoiceData = (DataSet)Session["dsInvoiceData"];
            DataRow dr = dsInvoiceData.Tables[0].NewRow();
            dr = dsInvoiceData.Tables[1].Rows[0];
            dsInvoiceData.Tables[0].ImportRow(dr);

            if (dsInvoiceData != null)
            {
                if (dsInvoiceData.Tables != null)
                {
                    if (dsInvoiceData.Tables.Count > 0)
                    {
                        if (dsInvoiceData.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                Session["Filters"] = "";

                                DataTable DTFilters = new DataTable();

                                DTFilters.Columns.Add("AgentName");
                                DTFilters.Columns.Add("AgentCode");
                                DTFilters.Columns.Add("BillType");
                                DTFilters.Columns.Add("FromDate");
                                DTFilters.Columns.Add("ToDate");
                                DTFilters.Columns.Add("Origin");

                                DTFilters.Rows.Add(
                                    ddlAgentName.SelectedItem.Text,
                                    ddlAgentCode.SelectedItem.Text,
                                    ddlBillType.SelectedItem.Text,
                                    //Convert.ToDateTime(txtInvoiceFrom.Text).ToShortDateString(),
                                    //Convert.ToDateTime(txtInvoiceTo.Text).ToShortDateString(),
                                    txtInvoiceFrom.Text,
                                    txtInvoiceTo.Text,
                                    txtOrigin.Text);


                                Session["Filters"] = DTFilters;

                                Session["CollData"] = "";
                                DataTable DTCollData = new DataTable();
                                DTCollData = dsInvoiceData.Tables[0];
                                Session["CollData"] = DTCollData;

                                //Response.Write("<script>");
                                //Response.Write("window.open('ShowCollectionDetailsReport.aspx','_blank')");
                                //Response.Write("</script>");
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('ShowCollectionDetailsReport.aspx','_blank')", true);

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                    }
                }
            }
        }

        protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCreditPopup.Enabled = false;
            
            if (ddlPaymentType.SelectedValue == "Cash")
            {
                txtChequeDdNo.Enabled = false;
                txtChequeDate.Enabled = false;
                txtBankName.Enabled = false;
            }
            else if (ddlPaymentType.SelectedValue == "Cheque")
            {
                txtChequeDdNo.Enabled = true;
                txtChequeDate.Enabled = true;
                txtBankName.Enabled = true;
            }
            else
            {
                txtChequeDdNo.Enabled = true;
                txtChequeDate.Enabled = false;
                txtBankName.Enabled = false;
                txtChequeDate.Text = "";
                txtBankName.Text = "";
            }

            if (ddlPaymentType.SelectedValue == "Card")
            {
                btnCreditPopup.Enabled = true;
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }

        }

        protected void btnSaveDCM_Click(object sender, EventArgs e)
        {
            try
            {
                string invNo;
                invNo = "";

                for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
                {
                    if (((CheckBox)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                    {
                        invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                    }
                }

                if (invNo == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please select Invoice Number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    lblStatus.Visible = false;
                    lblStatus.Text = "";
                }

                if (!DCMValidation())
                    return;

                #region Prepare Parameters
                object[] RateCardInfo = new object[6];
                int j = 0;

                RateCardInfo.SetValue(invNo, j);
                j++;

                RateCardInfo.SetValue(txtDCMNumber.Text.Trim(), j);
                j++;

                RateCardInfo.SetValue(txtDCMAmount.Text.Trim(), j);
                j++;

                //If DCMType.SelectedValue = 0 -> Collection/TDS amount
                //If DCMType.SelectedValue = 1 -> DCM debit amount
                //If DCMType.SelectedValue = 2 -> DCM credit amount
                RateCardInfo.SetValue(ddlDCMType.SelectedValue, j);
                j++;

                RateCardInfo.SetValue(txtReason.Text.Trim(), j);
                j++;

                RateCardInfo.SetValue(Session["UserName"].ToString(), j);

                #endregion Prepare Parameters

                string res = "";
                res = objBAL.AddInvoiceDCMCollectionDetails(RateCardInfo);

                if (res != "error")
                {
                    bindCollectionDetails();
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region RowCommand
        protected void grdInvoiceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            BALCollectionDetails BALCol = new BALCollectionDetails();
            
            try
            {
                if (e.CommandName == "Detail")
                {
                    string InvoiceNumber = e.CommandArgument.ToString();
                    try
                    {
                       
                            DataSet ds = db.SelectRecords("SP_GetInvoiceDataInvMatchTest", "InvoiceNumber", InvoiceNumber, SqlDbType.VarChar);
                            if (InvoiceNumber != "")
                            {
                                if (ds != null)
                                {
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[2].Rows.Count > 0 && ds.Tables[3].Rows.Count > 0)
                                        {
                                            grdInvoiceDetails.DataSource = ds.Tables[2];
                                            grdInvoiceDetails.DataBind();

                                            txtAgentComm.Text = ds.Tables[3].Rows[0]["AgentsCommission"].ToString();
                                            txtBaseAmtServiceTax.Text = ds.Tables[3].Rows[0]["TotalBaseAmtForST"].ToString();
                                            txtCommSales.Text = ds.Tables[3].Rows[0]["CommissionableSales"].ToString();
                                            txtNetCharges.Text = ds.Tables[3].Rows[0]["NETDueAirlinesAgentINR"].ToString();
                                            txtOCDA.Text = ds.Tables[3].Rows[0]["OtherChargesDueAgent"].ToString();
                                            txtSTOnComm.Text = ds.Tables[3].Rows[0]["STOnCommission"].ToString();
                                            txtTaxDueAirline.Text = ds.Tables[3].Rows[0]["TotalSTDueAirline"].ToString();
                                            txtTotalChargesDueAirline.Text = ds.Tables[3].Rows[0]["TotalChargesDueAirline"].ToString();
                                            txtTotalDeductions.Text = ds.Tables[3].Rows[0]["TotalDeductions"].ToString();
                                            txtTDSOnComm.Text = ds.Tables[3].Rows[0]["TDSOnCommission"].ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false); 

                                        }
                                    }
                                }

                            }
                        
                    }
                    catch (Exception ex)
                    {
                    }

                }

            }
            catch (Exception ex)
            { }

            try
            {
                if (e.CommandName == "ORNumber")
                {
                    string ORNumber = e.CommandArgument.ToString();
                    string Station = Session["Station"].ToString();
                    DateTime IssuingDate = (DateTime)Session["IT"];
                    Session["ORReceiptDownload"] = null;
                    Session["ORReceiptName"] = null;

                    DataSet ds = BALCol.GetORDetailsRegularCollection(ORNumber, Station, IssuingDate, "Invoice");

                    System.IO.MemoryStream Logo = null;
                    try
                    {
                        Logo = CommonUtility.GetImageStream(Page.Server);
                             }
                    catch (Exception ex)
                    {
                        Logo = new System.IO.MemoryStream();
                    }

                    if (ds.Tables[0].Columns.Contains("Logo") == false)
                    {
                        DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                        col1.DefaultValue = Logo.ToArray();
                        ds.Tables[0].Columns.Add(col1);
                    }


                    Session["ORReceiptName"] = "InvoiceCollection";
                    Session["ORReceiptDownload"] = ds;
                    #region Commented Code
                    //ReportViewer rptLoadPlanReport = new ReportViewer();
                    //ReportDataSource rds1 = new ReportDataSource();
                    //rptLoadPlanReport.ProcessingMode = ProcessingMode.Local;

                    //LocalReport rep1 = rptLoadPlanReport.LocalReport;

                    //rep1.ReportPath = Server.MapPath("/Reports/rptORReceipt.rdlc");

                    //rds1.Name = "dsORReceipt_dtORReceipt";
                    //// rds1.Value = dtTable1; //ULD Section Table

                    //rds1.Value = ds.Tables[0];
                    //rep1.DataSources.Add(rds1);

                    ////rptLoadPlanReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                 
                    //try
                    //{
                    //    string reportType = "PDF";
                    //    //string mimeType;
                    //    //string encoding;
                    //    string fileNameExtension;
                    //    string deviceInfo = "<DeviceInfo><PageHeight>40cm</PageHeight><PageWidth>50cm</PageWidth></DeviceInfo>";

                    //    //"<DeviceInfo>" +

                    //    //"  <OutputFormat>PDF</OutputFormat>" +

                    //    //"</DeviceInfo>";

                    //    Warning[] warnings;
                    //    string[] streamIds;
                    //    string mimeType; //= string.Empty;
                    //    string encoding;//= string.Empty;
                    //    string extension;//= string.Empty;

                    //    //Render the report
                    //    // send it to the client to download
                    //    byte[] bytes = rptLoadPlanReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    //    Response.Buffer = true;
                    //    Response.Clear();
                    //    Response.ContentType = mimeType;
                    //    Response.AddHeader("content-disposition", "attachment; filename=" + "InvoiceCollection" +"."+ ".pdf");
                    //    Response.BinaryWrite(bytes); // create the file
                    //    Response.Flush();

                    //    //Response.Clear();
                    //    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //    //Response.ContentType = "text/plain";
                    //    //Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceCollection"  + ".pdf");
                    //    //Response.BinaryWrite(bytes);
                    //    //Response.Flush();
                    //    //Response.End();
                    //}


                    //catch (Exception ex)
                    //{
                    //}
                    #endregion

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download();</SCRIPT>", false);

                }
            }
            catch (Exception ex)
            { }
        }
#endregion

        #region Button Close
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false); 
            }
            catch (Exception Ex)
            {
                lblStatus.Text = Ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/BillingCollectionMaster.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click

        protected void btnPostTransaction_Click(object sender, EventArgs e)
        {
            string invNo = string.Empty, AgentName = string.Empty, AgentCode = string.Empty, Description = string.Empty, TranError = string.Empty;
            decimal PendingAmt = 0;
            BALCardTransaction BalTransaction = new BALCardTransaction();
            bool blnTranResult = false;

            for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
            {
                if (((CheckBox)grdInvoiceList.Rows[y].FindControl("rbSelect")).Checked)
                {
                    invNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text.Trim();
                    AgentCode = ((Label)grdInvoiceList.Rows[y].FindControl("lblLocalAgent")).Text.Trim();
                    PendingAmt = Convert.ToDecimal(txtCollectedAmount.Text.Trim());
                    Description = txtPPRemarks.Text.Trim();
                    break;
                }
            }
            hdTransactionId.Value = "";
            string TransactionId = string.Empty;

            if (invNo != "" && PendingAmt > 0)
            {
                blnTranResult = BalTransaction.ProcessCardPayment("", txtCardNumber.Text.Trim(), ddlMonth.SelectedValue + ddlYear.SelectedValue, PendingAmt, Description,
                    txtCVV.Text.Trim(), invNo, AgentCode, txtCardholdername.Text.Trim(), "", "", "", "", "", "", "", "", "", "", ref TranError, ref TransactionId);

                if (blnTranResult)
                {
                    lblError.Text = "";
                    hdTransactionId.Value = TransactionId;
                    btnSave_Click(null, null);
                }
                else
                {
                    lblError.ForeColor = Color.Red;
                    if (TranError == "")
                        lblError.Text = "Card payment process failed.";
                    else
                        lblError.Text = TranError;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CreditCardPopup();</SCRIPT>", false);
                }
            }
        }
    }

}
