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
    public partial class frmWalkinAgentCollection : System.Web.UI.Page
    {
        #region Variables
        UserCreationBAL objBal = new UserCreationBAL();
        BALCollectionDetails objBALCD = new BALCollectionDetails();
        string strfromdate = "", strtodate = "";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    txtUserID.Text = Session["UserName"].ToString();
                    txtLocation.Text = Session["Station"].ToString();
                    FillUserDropdown();
                }
            }
        }

        #region Search Click
        protected void btnSearch_Click(object sender, EventArgs e)
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

            DataSet DSCollectiondata = objBALCD.GetCollectionWalkInDataNew(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(strtodate).ToString("yyyy-MM-dd HH:mm:ss"), txtUserID.Text.Trim());

            if (DSCollectiondata != null && DSCollectiondata.Tables.Count > 0 && DSCollectiondata.Tables[0].Rows.Count > 0)
            {
                Session["DSCollectiondata"] = DSCollectiondata;
                grdInvoiceList.DataSource = DSCollectiondata.Tables[0];
                grdInvoiceList.DataBind();
                editCollectionGridInvoiceAmt();
                editCollectionGridPendingAmt();
            }
            else
            {
                grdInvoiceList.Visible = false;
                lblStatus.Text = "No Records Found";
                lblStatus.ForeColor = Color.Blue;
            }

            ListHandoverAmountDetails(txtUserID.Text.Trim());
        }
        #endregion

        #region Fill User Dropdown
        private void FillUserDropdown()
        {
            DataSet dsUsers = new DataSet();
            dsUsers = objBal.GetUserListData("", 0, "");
            if (dsUsers != null)
            {
                drpUsers.DataSource = dsUsers;
                drpUsers.DataMember = dsUsers.Tables[0].TableName;
                drpUsers.DataTextField = "LoginID";
                drpUsers.DataValueField = "UserName";
                drpUsers.DataBind();
                drpUsers.Items.Insert(0, new ListItem("Select", ""));
                drpUsers.SelectedIndex = -1;
            }
        }
        #endregion

        #region Page Index Changing
        protected void grdInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Session["dsInvoiceData"]
            DataSet dst = (DataSet)Session["DSCollectiondata"];
            grdInvoiceList.PageIndex = e.NewPageIndex;
            grdInvoiceList.DataSource = dst.Tables[0];
            grdInvoiceList.DataBind();
        }
        #endregion

        #region Invoice List RowBound
        protected void grdInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet dsTotal = (DataSet)Session["DSCollectiondata"];
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblGTotalText = (Label)e.Row.FindControl("lblGTotalText");
                Label lblGTotalInvoiceAmt = (Label)e.Row.FindControl("lblGTotalInvoiceAmt");
                Label lblGTotalCollectedAmt = (Label)e.Row.FindControl("lblGTotalCollectedAmt");
                Label lblGTotalPendingAmt = (Label)e.Row.FindControl("lblGTotalPendingAmt");

                lblGTotalText.Text = "Grant Total";
                lblGTotalInvoiceAmt.Text = dsTotal.Tables[1].Rows[0]["InvoiceAmount"].ToString();
                lblGTotalCollectedAmt.Text = dsTotal.Tables[1].Rows[0]["CollectedAmount"].ToString();
                lblGTotalPendingAmt.Text = dsTotal.Tables[1].Rows[0]["PendingAmount"].ToString();
            }
        }
        #endregion

        #region Editing Grid
        protected void editCollectionGridInvoiceAmt()
        {
            string OldInvNo = "", NewInvNo = "";
            int FirstLoop = 0;
            for (int y = 0; y < grdInvoiceList.Rows.Count; y++)
            {
                if (FirstLoop == 0)
                    OldInvNo = ((Label)grdInvoiceList.Rows[0].FindControl("lblInvoiceNumber")).Text;
                else
                    OldInvNo = ((Label)grdInvoiceList.Rows[y - 1].FindControl("lblInvoiceNumber")).Text;

                if (grdInvoiceList.Rows.Count > y)
                {
                    NewInvNo = ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Text;
                }
                if (NewInvNo == OldInvNo && y != 0)
                {
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceAmount")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblInvoiceNumber")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblAgentName")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblCentralAgent")).Visible = false;
                    ((Label)grdInvoiceList.Rows[y].FindControl("lblLocalAgent")).Visible = false;

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

        #region Submit Button Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";

                double HandAmt;
                if (!double.TryParse(txtHandOverAmt.Text, out HandAmt))
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "HandOver amount not in correct format.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (drpUsers.SelectedItem.Text == "Select")
                {

                    lblStatus.Visible = true;
                    lblStatus.Text = "Please Select the user to handover amount to.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                bool insertDetails = objBALCD.InsertHandoverDetails((DateTime)Session["IT"], txtUserID.Text.Trim(), txtLocation.Text.Trim(), drpUsers.SelectedItem.Text, HandAmt, txtRemarks.Text.Trim(), Session["UserName"].ToString(), (DateTime)Session["IT"]);

                if (!insertDetails)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Error in inserting details";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                ListHandoverAmountDetails(txtUserID.Text.Trim());

                lblStatus.Visible = true;
                lblStatus.Text = "Successfully inserted the handover details";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
            }
        }
        #endregion

        #region Page Index Changing
        protected void grdHandoverAmount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Session["dsInvoiceData"]
            DataSet dst = (DataSet)Session["DSHandoverGrid"];
            grdInvoiceList.PageIndex = e.NewPageIndex;
            grdInvoiceList.DataSource = dst.Tables[0];
            grdInvoiceList.DataBind();
        }
        #endregion

        #region Button Handover Click
        protected void btnHandover_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message2", "<SCRIPT LANGUAGE='javascript'>callLoadingClose();</script>", false);
            }
        }
        #endregion

        #region Button Handover Click
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message2", "<SCRIPT LANGUAGE='javascript'>callLoadingClose();</script>", false);
            }
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/frmWalkinAgentCollection.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click
        
    }
}
