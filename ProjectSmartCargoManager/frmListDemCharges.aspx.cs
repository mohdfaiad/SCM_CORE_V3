using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmListDemCharges : System.Web.UI.Page
    {

        clsFillCombo cfc = new clsFillCombo();

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["DemurrageCharges"] = null;
                if (ddllevel.SelectedIndex == 0)
                    cfc.FillAllComboBoxes("tblCountryMaster", "Select", ddlStation);
                else
                    cfc.FillAllComboBoxes("tblWarehouseMaster", "Select", ddlStation);
            }
        }
        #endregion Page_Load

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                string[] paramnameRBNew = new string[3];
                paramnameRBNew[0] = "ChargeCode";
                paramnameRBNew[1] = "LocationLevel";
                paramnameRBNew[2] = "Location";
                
                object[] paramvalueRBNew = new object[3];
                paramvalueRBNew[0] = txtChargeCode.Text;
                paramvalueRBNew[1] = ddllevel.SelectedValue;
                paramvalueRBNew[2] = ddlStation.SelectedItem.Text;

                
                SqlDbType[] paramtypeRBNew = new SqlDbType[3];
                paramtypeRBNew[0] = SqlDbType.NVarChar;
                paramtypeRBNew[1] = SqlDbType.NVarChar;
                paramtypeRBNew[2] = SqlDbType.NVarChar;

                SQLServer dbn = new SQLServer(Global.GetConnectionString());
                DataSet ds = dbn.SelectRecords("spGetDemChargeList", paramnameRBNew, paramvalueRBNew, paramtypeRBNew);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvResult.DataSource = ds;
                    gvResult.DataMember = ds.Tables[0].TableName;
                    gvResult.DataBind();
                    Session["DemurrageCharges"] = ds.Copy();
                }
                else
                {
                    lblError.Text = "No record found.";
                    lblError.ForeColor = System.Drawing.Color.Blue;
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion btnSearch_Click

        #region ddllevel_SelectedIndexChanged
        protected void ddllevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddllevel.SelectedIndex == 0)
                    cfc.FillAllComboBoxes("tblCountryMaster", "Select", ddlStation);
                else
                    cfc.FillAllComboBoxes("tblWarehouseMaster", "Select", ddlStation);
            }
            catch (Exception)
            {
            }
        }
        #endregion ddllevel_SelectedIndexChanged

        #region gvResult_RowEditing
        protected void gvResult_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                lblError.Text = "";
                string DemCharge = ((Label)gvResult.Rows[e.NewEditIndex].FindControl("lblChargeCode")).Text;
                Response.Redirect("~/frmDemCharges.aspx?type=Edit&ChargeCode=" + DemCharge, false);
            }
            catch (Exception)
            {
            }
        }
        #endregion gvResult_RowEditing

        #region gvResult_PageIndexChanging
        protected void gvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                lblError.Text = "";
                gvResult.PageIndex = e.NewPageIndex;
                gvResult.DataSource = (DataSet)Session["DemurrageCharges"];
                gvResult.DataBind();
            }
            catch (Exception)
            {
            }
        }
        #endregion gvResult_PageIndexChanging

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmListDemCharges.aspx");
        }
        #endregion btnClear_Click

    }
}
