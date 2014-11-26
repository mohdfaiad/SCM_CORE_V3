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
    public partial class ListHandoverDetails : System.Web.UI.Page
    {
        
        #region Variables
        UserCreationBAL objBal = new UserCreationBAL();
        BALCollectionDetails objBALCD = new BALCollectionDetails();
        string strfromdate = "", strtodate = "";
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    txtUserID.Text = Session["UserName"].ToString();
                    txtLocation.Text = Session["Station"].ToString();
                }
            }
        }
        #endregion

        #region Search Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                DateTime dt;

                try
                {
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

                ListHandoverAmountDetails(txtUserID.Text.Trim());
            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region List HandoverAmountDetails
        private void ListHandoverAmountDetails(string UserID)
        {
            try
            {
                DataSet DSHandoverGrid = new DataSet();
                DSHandoverGrid = objBALCD.ListHandoverDetails(UserID);
                if (DSHandoverGrid != null && DSHandoverGrid.Tables.Count > 0)
                {
                    if (DSHandoverGrid.Tables[0].Rows.Count > 0)
                    {
                        Session["DSHandoverGrid"] = DSHandoverGrid;
                        grdHandoverAmount.DataSource = DSHandoverGrid;
                        grdHandoverAmount.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error while listing Handover amount";
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Page Index Changing
        protected void grdInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Session["dsInvoiceData"]
            try
            {
                DataSet dst = (DataSet)Session["DSHandoverGrid"];
                grdHandoverAmount.PageIndex = e.NewPageIndex;
                grdHandoverAmount.DataSource = dst.Tables[0];
                grdHandoverAmount.DataBind();
            }
            catch
            {
                lblStatus.Text = "Error in Page Changing !!";
            }
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/ListHandoverDetails.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click

        #region grdHandoverAmount_PageIndexChanging
        protected void grdHandoverAmount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet dst = (DataSet)Session["DSHandoverGrid"];
                grdHandoverAmount.PageIndex = e.NewPageIndex;
                grdHandoverAmount.DataSource = dst.Tables[0];
                grdHandoverAmount.DataBind();
            }
            catch (Exception)
            {
            }
        }
        #endregion grdHandoverAmount_PageIndexChanging

    }
}
