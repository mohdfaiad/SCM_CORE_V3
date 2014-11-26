using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BAL;


namespace ProjectSmartCargoManager
{
    public partial class HistoryCapacity : System.Web.UI.Page
    {

        #region Variable
        BALRegionMaster objBAL = new BALRegionMaster();
        #endregion variable

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrigin();
                LoadDaysandMonth();
                FillProductType();
            }
        }
        #endregion Page_Load

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BALFlightCapacity objBAL = new BALFlightCapacity();
                # region Save

                if (btnSave.Text == "Save")
                {
                    try
                    {
                        bool blnResult = false;

                        decimal AvailableCapacity = 0, LowerExpCapacity = 0, ExpectedCapacity = 0, UpperExpCapacity = 0;

                        if (txtAvlCapacity.Text.Trim() != "")
                            AvailableCapacity = Convert.ToDecimal(txtAvlCapacity.Text.Trim());

                        if (txtLowerExpCapacity.Text.Trim() != "")
                            LowerExpCapacity = Convert.ToDecimal(txtLowerExpCapacity.Text.Trim());

                        if (txtExpCapacity.Text.Trim() != "")
                            ExpectedCapacity = Convert.ToDecimal(txtExpCapacity.Text.Trim());

                        if (txtUpperExpCapacity.Text.Trim() != "")
                            UpperExpCapacity = Convert.ToDecimal(txtUpperExpCapacity.Text.Trim());

                        blnResult = objBAL.SaveHistoricalCapacity(0, ddlOrgin.Text.Trim(), ddlDestination.Text.Trim(), txtFlightNo.Text.Trim(), txtFlightDt.Text.Trim(),
                            txtCommodity.Text.Trim(), txtCategory.Text.Trim(), ddlDayOfWeek.Text.Trim(), ddlMonth.Text.Trim(), AvailableCapacity, LowerExpCapacity,
                            ExpectedCapacity, UpperExpCapacity, true, Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]), ddlProductType.Text.Trim());

                        if (blnResult)
                        {
                            btnClear_Click(null, null);
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Data Added Sucessfully..";
                            btnSave.Text = "Save";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data Insertion Failed..";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                # endregion Save

                # region Update
                if (btnSave.Text == "Update")
                {
                    bool blnResult = false;
                    int intRowId = Convert.ToInt32(hdnRowId.Value);

                    decimal AvailableCapacity = 0, LowerExpCapacity = 0, ExpectedCapacity = 0, UpperExpCapacity = 0;

                    if (txtAvlCapacity.Text.Trim() != "")
                        AvailableCapacity = Convert.ToDecimal(txtAvlCapacity.Text.Trim());

                    if (txtLowerExpCapacity.Text.Trim() != "")
                        LowerExpCapacity = Convert.ToDecimal(txtLowerExpCapacity.Text.Trim());

                    if (txtExpCapacity.Text.Trim() != "")
                        ExpectedCapacity = Convert.ToDecimal(txtExpCapacity.Text.Trim());

                    if (txtUpperExpCapacity.Text.Trim() != "")
                        UpperExpCapacity = Convert.ToDecimal(txtUpperExpCapacity.Text.Trim());

                    blnResult = objBAL.SaveHistoricalCapacity(intRowId, ddlOrgin.Text.Trim(), ddlDestination.Text.Trim(), txtFlightNo.Text.Trim(), txtFlightDt.Text.Trim(),
                        txtCommodity.Text.Trim(), txtCategory.Text.Trim(), ddlDayOfWeek.Text.Trim(), ddlMonth.Text.Trim(), AvailableCapacity, LowerExpCapacity,
                        ExpectedCapacity, UpperExpCapacity, true, Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]), ddlProductType.Text.Trim());

                    if (blnResult)
                    {
                        btnClear_Click(null, null);
                        btnList_Click(null, null);
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Data Updated Sucessfully..";
                        btnSave.Text = "Save";
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data Updation Failed..";
                    }

                }
                # endregion Update
            }
            catch (Exception ex)
            {
            }
        }
            
        #endregion btnSave_Click

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            BALFlightCapacity objBAL = new BALFlightCapacity();
            DataSet ds = null;

            try
            {
                lblStatus.Text = "";

                string ProductType = string.Empty;

                if (ddlProductType.SelectedItem.Text.Trim().ToUpper() != "SELECT")
                    ProductType = ddlProductType.SelectedItem.Text.Trim();

                ds = objBAL.GetHistoricalCapacity(ddlOrgin.Text.Trim(), ddlDestination.Text.Trim(), txtFlightNo.Text.Trim(), txtFlightDt.Text.Trim(),
                    txtCommodity.Text.Trim(), txtCategory.Text.Trim(), ddlDayOfWeek.Text.Trim(), ddlMonth.Text.Trim(), ProductType);

                objBAL = null;

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Session["HD"] = ds;
                                grvRegionList.PageIndex = 0;
                                grvRegionList.DataSource = ds;
                                grvRegionList.DataMember = ds.Tables[0].TableName;
                                grvRegionList.DataBind();
                                grvRegionList.Visible = true;
                                ds.Clear();
                            }
                            else
                            {
                                lblStatus.Text = "Records not available for selected criteria...";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }
        #endregion btnList_Click

        #region grvRegionList_RowCommand
        protected void grvRegionList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    for (int intCount = 0; intCount < grvRegionList.Rows.Count; intCount++)
                    {
                        ((CheckBox)grvRegionList.Rows[intCount].FindControl("chkSelect")).Checked = false;
                    }

                    ((CheckBox)grvRegionList.Rows[RowIndex].FindControl("chkSelect")).Checked = true;

                    hdnRowId.Value = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblSrno")).Text.Trim();
                    ddlOrgin.SelectedValue = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblOrigin")).Text.Trim();
                    ddlDestination.SelectedValue = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblDestination")).Text.Trim();
                    txtFlightNo.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblFlightNo")).Text.Trim();
                    txtFlightDt.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblFlightDate")).Text.Trim();
                    txtCommodity.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblCommodity")).Text.Trim();
                    txtCategory.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblCategory")).Text.Trim();
                    ddlDayOfWeek.SelectedValue = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblDayOfWeek")).Text.Trim();
                    ddlMonth.SelectedValue = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblMonth")).Text.Trim();
                    txtAvlCapacity.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblAvailableCapacity")).Text.Trim();
                    txtLowerExpCapacity.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblLowerExpCapacity")).Text.Trim();
                    txtExpCapacity.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblExpextedCapacity")).Text.Trim();
                    txtUpperExpCapacity.Text = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblUpperExpCapacity")).Text.Trim();
                    
                    try
                    {
                        ddlProductType.SelectedValue = ((Label)grvRegionList.Rows[RowIndex].FindControl("lblProductType")).Text.Trim();
                    }
                    catch
                    {
                        ddlProductType.SelectedIndex = 0;
                    }

                    btnSave.Text = "Update";
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion grvRegionList_RowCommand

        #region grvRegionList_PageIndexChanging
        protected void grvRegionList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {   
                grvRegionList.PageIndex = e.NewPageIndex;
                grvRegionList.DataSource = ((DataSet)Session["HD"]);
                grvRegionList.DataBind();
            }
            catch (Exception)
            {
            }
        }
        #endregion grvRegionList_PageIndexChanging

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlOrgin.SelectedValue = "";
            ddlDestination.SelectedValue = "";
            txtFlightNo.Text = "";
            txtFlightDt.Text = "";
            txtCommodity.Text = "";
            txtCategory.Text = "";
            ddlDayOfWeek.SelectedValue = "";
            ddlMonth.SelectedValue = "";
            ddlProductType.SelectedIndex = 0;
            txtUpperExpCapacity.Text = "";
            txtLowerExpCapacity.Text = "";
            txtAvlCapacity.Text = "";
            txtExpCapacity.Text = "";
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
        }
        #endregion btnClear_Click

        public void LoadOrigin()
        {
            BookingBAL objBLL = new BookingBAL();
            DataSet ds = null;
            try
            {
                ds = objBLL.GetDestinationsForSource("");
                if (ds != null)
                {
                    DataRow row = ds.Tables[0].NewRow();

                    row["AirportCode"] = "";
                    ds.Tables[0].Rows.Add(row);

                    ddlOrgin.DataSource = ds;
                    ddlOrgin.DataMember = ds.Tables[0].TableName;
                    ddlOrgin.DataTextField = "AirportCode";
                    ddlOrgin.DataValueField = "AirportCode";
                    ddlOrgin.DataBind();
                    ddlOrgin.Text = "";

                    ddlDestination.DataSource = ds;
                    ddlDestination.DataMember = ds.Tables[0].TableName;
                    ddlDestination.DataTextField = "AirportCode";
                    ddlDestination.DataValueField = "AirportCode";
                    ddlDestination.DataBind();

                    ddlDestination.Text = "";
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                objBLL = null;
                if (ds != null)
                    ds.Dispose();
            }
        }

        private void LoadDaysandMonth()
        {
            ddlDayOfWeek.DataSource = CommonUtility.GetDayOfWeekList();
            ddlDayOfWeek.DataBind();

            ddlMonth.DataSource = CommonUtility.GetMonthsList();
            ddlMonth.DataBind();
        }

        #region grvRegionList_RowEditing
        protected void grvRegionList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion grvRegionList_RowEditing

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int intRowCnt = 0;
            if (grvRegionList.Rows.Count > 0)
            {
                BALFlightCapacity oblFltBAL = new BALFlightCapacity();

                for (int intCount = 0; intCount < grvRegionList.Rows.Count; intCount++)
                {
                    if (((CheckBox)grvRegionList.Rows[intCount].FindControl("chkSelect")).Checked)
                    {
                        intRowCnt = intRowCnt + 1;
                        int intSerialNo = Convert.ToInt32(((Label)grvRegionList.Rows[intCount].FindControl("lblSrno")).Text.Trim());

                        bool blnResult = oblFltBAL.SaveHistoricalCapacity(intSerialNo, ddlOrgin.Text.Trim(), ddlDestination.Text.Trim(), txtFlightNo.Text.Trim(), txtFlightDt.Text.Trim(),
                            txtCommodity.Text.Trim(), txtCategory.Text.Trim(), ddlDayOfWeek.Text.Trim(), ddlMonth.Text.Trim(), 0, 0,
                            0, 0, false, Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]), ddlProductType.Text.Trim());

                        if (blnResult)
                        {
                            btnClear_Click(null, null);
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Data Deleted Sucessfully..";
                            btnSave.Text = "Save";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Data Deletion Failed..";
                        }
                    }
                }
            }

            if (intRowCnt == 0)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please select the rows to Delete.";
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetCommodityCodesWithName(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT CommodityCode + '(' + Description + ')' from CommodityMaster where (Description like '%" + prefixText + "%' or CommodityCode like '%" + prefixText + "%')", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }
            dad = null;
            if (ds != null)
                ds.Dispose();

            return list.ToArray();
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetCommodityCategoryCodes(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string"); 
            SqlDataAdapter dad = new SqlDataAdapter("SELECT CommCategory + '(' + CatDescription + ')' from CommCategoryMaster where (CatDescription like '%" + prefixText + "%' or CommCategory like '%" + prefixText + "%' and IsActive = 1)", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }
            dad = null;
            if (ds != null)
                ds.Dispose();

            return list.ToArray();
        }

        private void FillProductType()
        {
            LoginBL Bal = new LoginBL();
            DataTable dsIrr = null;

            try
            {
                dsIrr = Bal.LoadSystemMasterDataNew("PM").Tables[0];
                if (dsIrr != null && dsIrr.Rows.Count > 0)
                {
                    ddlProductType.Items.Clear();
                    ddlProductType.DataSource = dsIrr;
                    ddlProductType.DataTextField = "ProductType";
                    ddlProductType.DataValueField = "ProductType";
                    ddlProductType.DataBind();
                    ddlProductType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Bal = null;
                dsIrr = null;
            }
            finally
            {
                Bal = null;
                dsIrr = null;
            }
        }
    }
}
