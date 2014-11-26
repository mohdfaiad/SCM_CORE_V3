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
    public partial class TaxCodeMaster : System.Web.UI.Page
    {
       #region Variable
        BALTaxCode objBAL = new BALTaxCode();
       #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnAdd.Attributes.Add("onclick", "return CheckBlank();");
                btnList_Click(sender, e);
            }
        }

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TaxCodeMaster.aspx");
            //txtOcDesc.Text = "";
            //txtOcCode.Text = "";
            //lblStatus.Text = "";
        }
        #endregion


        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
          DataSet dsList = new DataSet();

            try 
	        {
                dsList = objBAL.dsTaxCodeDetails(txtOcCode.Text.Trim(), txtOcDesc.Text.Trim());

                if (dsList != null && dsList.Tables.Count > 0 && dsList.Tables[0].Rows.Count > 0)
                {
                    grdTaxCode.DataSource = dsList.Tables[0];
                    grdTaxCode.DataBind();
                    

                    for (int i = 0; i < grdTaxCode.Rows.Count; i++)
                    {
                        if (((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text.ToString() == "True")
                        {
                            ((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text = "Active";
                        }
                        else if (((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text.ToString() == "False")
                        {
                            grdTaxCode.Rows[i].Visible = false;
                            //grdTaxCode.Rows[i].Enabled = false;
                            ((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text = "In-Active";
                        }
                    
                    }

                    Session["TaxCode"] = dsList;

                    if (grdTaxCode.Rows.Count == 0)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "No Records Found!";
                    }
                    else
                    {
                        lblStatus.Text = "";
                    }

                }

                else
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Error in Listing OC!";
                }

	        }
	        catch (Exception ex)
	        {
                lblStatus.Text = "Error:-"+ex.Message;
                lblStatus.ForeColor = Color.Red;
	        }
        }

        #endregion


        #region Button Add
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DateTime CreatedOn = DateTime.Parse(Session["IT"].ToString());
            string CreatedBy = Session["UserName"].ToString();
            DateTime UpdatedOn = DateTime.Parse(Session["IT"].ToString());
            string UpdatedBy = Session["UserName"].ToString();

            try
            {
                if (txtOcCode.Text == "" || txtOcDesc.Text == "")
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Entries Can't be Blank";
                }

                if (objBAL.ModifyTaxCode("insert", txtOcCode.Text.Trim(), txtOcDesc.Text.Trim(), CreatedOn, CreatedBy, UpdatedOn, UpdatedBy))
                {
                    txtOcCode.Text = "";
                    txtOcDesc.Text = "";
                    btnList.Enabled = true;
                    btnClear.Enabled = true;

                    btnList_Click(sender, e);

                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "TaxCode Updated Successfully";
                    txtOcCode.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion


        #region Button Delete
        protected void btnDelete0_Click(object sender, EventArgs e)
        {
            DateTime CreatedOn = DateTime.Parse(Session["IT"].ToString());
            string CreatedBy = Session["UserName"].ToString();
            DateTime UpdatedOn = DateTime.Parse(Session["IT"].ToString());
            string UpdatedBy = Session["UserName"].ToString();

            try
            {
                if (objBAL.ModifyTaxCode("delete", txtOcCode.Text.Trim(), txtOcDesc.Text.Trim(), CreatedOn, CreatedBy, UpdatedOn, UpdatedBy))
                {
                    txtOcCode.Text = "";
                    txtOcDesc.Text = "";
                    btnList.Enabled = true;
                    btnClear.Enabled = true;

                    btnList_Click(sender, e);

                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "TaxCode Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }

        #endregion

        #region On Row Command
        protected void grdTaxCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {

                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblOcCode = (Label)grdTaxCode.Rows[RowIndex].FindControl("lblOcCode");
                    Label lblOcDes = (Label)grdTaxCode.Rows[RowIndex].FindControl("lblOcDesc");

                    txtOcCode.Text = lblOcCode.Text;
                    txtOcDesc.Text = lblOcDes.Text;

                    lblStatus.Text = "";

                    // Disable List..
                    txtOcCode.ReadOnly = true;
                    
                    btnList.Enabled = false;
                    btnClear.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error:-" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region on PageIndexChanging
        protected void grdTaxCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = (DataSet)Session["TaxCode"];
                grdTaxCode.PageIndex = e.NewPageIndex;
                grdTaxCode.DataSource = ds;
                grdTaxCode.DataBind();

                for (int i = 0; i < grdTaxCode.Rows.Count; i++)
                {
                    if (((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text.ToString() == "True")
                    {
                        ((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text = "Active";
                    }
                    else if (((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text.ToString() == "False")
                    {
                        grdTaxCode.Rows[i].Visible = false;
                        //grdOc.Rows[i].Enabled = false;
                        ((Label)(grdTaxCode.Rows[i].FindControl("lblisact"))).Text = "In-Active";
                    }
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
