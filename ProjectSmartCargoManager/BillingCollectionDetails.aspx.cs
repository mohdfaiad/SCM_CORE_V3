using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class BillingCollectionDetails : System.Web.UI.Page
    {
        BillingAWBFileInvoiceBAL objBillBAL = new BillingAWBFileInvoiceBAL();
        BALCollectionDetails objBAL = new BALCollectionDetails();

        string strfromdate, strtodate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAgentDropdown(); //AgentName 
                LoadAgentCodeDropdown(); //AgentCode
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


                DataSet DSInvoicedata = objBAL.GetCollectionDetailsdata(ddlAgentName.SelectedValue, ddlBillType.SelectedValue, txtOrigin.Text, Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

                if (DSInvoicedata != null && DSInvoicedata.Tables.Count > 0 && DSInvoicedata.Tables[0].Rows.Count > 0)
                {
                    Session["dsInvoiceData"] = DSInvoicedata;
                    grdInvoiceList.DataSource = DSInvoicedata.Tables[0];
                    grdInvoiceList.DataBind();
                    grdInvoiceList.Visible = true;
                    btnExport.Visible = true;
                    btnUpdate.Visible = true;
                    lblStatus.Visible = false;
                }
                else
                {
                    grdInvoiceList.Visible = true;
                    btnExport.Visible = false;
                    btnUpdate.Visible = false;
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

        protected void TxtCollAmtChanged(object sender, EventArgs eventArgs)
        {
            //Check whether the Hiddenfield has some value or not, if not do not move forward
            if (HdnSelectedRowIndex.Value.Trim().Length == 0)
            {
                return;
            }

            //Sender will be the text box which raised the postback event
            TextBox TxtCollAmt = (TextBox)sender;
            Label lblInvoiceAmt = (Label)grdInvoiceList.Rows[Convert.ToInt32(HdnSelectedRowIndex.Value)].FindControl("lblInvoiceAmount");

            //Find the destination textbox to which you want to copy the value from sender.
            //We need to find the row index of the grid in which the value got changed, this rowindex is stored in the hiddenfield. (Javascript method will store the RowIndex on hidden field).
            Label lblPendAmt = (Label)grdInvoiceList.Rows[Convert.ToInt32(HdnSelectedRowIndex.Value)].FindControl("lblPendingAmount");

            if (TxtCollAmt != null && lblPendAmt != null)
            {
                try
                {
                    double InvoiceAmt = Convert.ToDouble(lblInvoiceAmt.Text);
                    double CollectedAmt = Convert.ToDouble(TxtCollAmt.Text);
                    double PendingAmt = Convert.ToDouble(lblPendAmt.Text);
                    if ((InvoiceAmt - CollectedAmt) < 0)
                    {
                        TxtCollAmt.Text = (InvoiceAmt - PendingAmt).ToString() ;
                        lblStatus.Visible = true;
                        lblStatus.Text = "Enter valid amount";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        lblPendAmt.Text = (PendingAmt - CollectedAmt).ToString();
                        lblStatus.Visible = false;
                        lblStatus.Text = "";
                    }
                    
                }
                catch (Exception ex)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Enter valid amount";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                
            }

            //Empty the hiddenfield value once you perform the action
            HdnSelectedRowIndex.Value = string.Empty;
        }

        protected void grdInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox TxtCollAmt = (TextBox)e.Row.FindControl("txtCollectedAmount");

                TxtCollAmt.Attributes.Add("onkeydown", "javascript:return DoPostBackWithRowIndex('" + e.Row.RowIndex + "');");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gvRow in grdInvoiceList.Rows)
                {
                    Label lblInvoiceNumber = (Label)gvRow.FindControl("lblInvoiceNumber");
                    TextBox txtCollectedAmt = (TextBox)gvRow.FindControl("txtCollectedAmount");
                    Label lblPengingAmt = (Label)gvRow.FindControl("lblPendingAmount");

                    #region Prepare Parameters
                    object[] RateCardInfo = new object[3];
                    int i = 0;

                    RateCardInfo.SetValue(lblInvoiceNumber.Text, i);
                    i++;

                    RateCardInfo.SetValue(txtCollectedAmt.Text, i);
                    i++;

                    RateCardInfo.SetValue(lblPengingAmt.Text, i);
                   
                    #endregion Prepare Parameters

                    string res = "";
                    res = objBAL.UpdateInvoiceCollectionDetails(RateCardInfo);

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

            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Updating collection details failed.";
                lblStatus.ForeColor = Color.Red;
            }
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

                                Response.Write("<script>");
                                Response.Write("window.open('ShowCollectionDetailsReport.aspx','_blank')");
                                Response.Write("</script>");


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
    }
}
