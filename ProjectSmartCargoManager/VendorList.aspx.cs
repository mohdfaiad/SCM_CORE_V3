using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class VendorList : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVendor();
            }
        }

        protected void LoadVendor()
        {
            DataSet ds = da.SelectRecords("Sp_GetVendorName");
            ddlVendorCode.DataSource = ds.Tables[0];
            ddlVendorCode.DataTextField = "VendorName";
            ddlVendorCode.DataValueField = "VendorCode";
            ddlVendorCode.DataBind();
            ddlVendorCode.Items.Insert(0, "Select");
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                string VendorCode;
                if (ddlVendorCode.SelectedIndex == 0)
                    VendorCode = "All";
                else
                    VendorCode = ddlVendorCode.SelectedValue;
                ds = da.SelectRecords("Sp_GetVendorList", "VendorCode", VendorCode, SqlDbType.VarChar);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["ds"] = ds;
                    grdVendorList.DataSource = ds.Tables[0];
                    grdVendorList.DataBind();
                }
                else
                {
                    grdVendorList.DataSource = null;
                    grdVendorList.DataBind();
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            grdVendorList.DataSource = null;
            grdVendorList.DataBind();

            ddlVendorCode.SelectedIndex = 0;
            txtVendorName.Text = string.Empty;

            lblStatus.Text = string.Empty;
        }

        protected void grdVendorList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string CommandName = e.CommandName;
                string VendorCode = ((Label)grdVendorList.Rows[index].FindControl("lblVendorCode")).Text;
                string ID = ((Label)grdVendorList.Rows[index].FindControl("lblSrNo")).Text;
                Response.Redirect("VendorMaster.aspx?Command="+CommandName+"&ID="+ID+"&Code="+VendorCode);
            }
            catch (Exception ex)
            { }
        }

        protected void grdVendorList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grdVendorList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = (DataSet)ViewState["ds"];
            grdVendorList.PageIndex = e.NewPageIndex;
            grdVendorList.DataSource = ds.Tables[0];
            grdVendorList.DataBind();
        }

    }
}
