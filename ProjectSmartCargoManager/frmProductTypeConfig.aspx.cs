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
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmProductTypeConfig : System.Web.UI.Page
    {

        #region Variable
        BALProductType objBAL = new BALProductType();
        #endregion variable

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["PR_TYPE_CONFIG_PRODUCTTYPES"] = objBAL.GetProductTypes();
                    Session["PR_TYPE_CONFIG_FLIGHTPREFIXES"] = null;
                    Session["PR_TYPE_CONFIG_DAYOFWEEK"] = null;
                    Session["PR_TYPE_CONFIG_MONTH"] = null;
                    Session["PR_TYPE_CONFIG_COMMCATEGORY"] = null;

                    //Load Product Type dropdown in Filter Criteria.
                    if (ddlProductType.DataSource == null)
                    {
                        ddlProductType.DataSource = ((DataSet)Session["PR_TYPE_CONFIG_PRODUCTTYPES"]).Tables[1];
                        ddlProductType.DataValueField = "ProductType";
                        ddlProductType.DataTextField = "ProductDescription";
                        ddlProductType.DataBind();
                    }

                    GetProductTypeConfigList(-1);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion Page_Load

        #region Get Product Type Config List
        private void GetProductTypeConfigList(int rowIndex)
        {
            DataSet ds = null;
            try
            {

                if (Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"] == null)
                {
                    object[] ParameterValues = new object[9];

                    ParameterValues[0] = ddlProductType.SelectedValue;
                    ParameterValues[1] = txtOriginCode.Text;
                    ParameterValues[2] = txtDestCode.Text;
                    ParameterValues[3] = txtFlightPrefix.Text + txtFlightNumber.Text;
                    if (rowIndex == -1)
                    {   //For Empty Grid.    
                        ParameterValues[4] = "01/01/2000";
                        ParameterValues[5] = "01/01/2000";
                    }
                    else
                    {
                        DateTime dtSearchDate = DateTime.Now;
                        if (DateTime.TryParseExact(txtFromDate.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None,
                            out dtSearchDate))
                        {
                            ParameterValues[4] = txtFromDate.Text;
                        }
                        else
                        {
                            lblStatus.Text = "Please select/ enter valid From Date.";
                            lblStatus.ForeColor = Color.Red;
                            txtFromDate.Focus();
                        }
                        if (DateTime.TryParseExact(txtToDate.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None,
                            out dtSearchDate))
                        {
                            ParameterValues[5] = txtToDate.Text;
                        }
                        else
                        {
                            lblStatus.Text = "Please select/ enter valid To Date.";
                            lblStatus.ForeColor = Color.Red;
                            txtToDate.Focus();
                        }
                    }
                    ParameterValues[6] = txtCommodityCategory.Text;
                    ParameterValues[7] = txtCommodityCode.Text;
                    ParameterValues[8] = txtPriority.Text;

                    ds = objBAL.GetProductTypeConfigList(ParameterValues);

                }
                else
                {
                    ds = new DataSet();
                    ds.Tables.Add(((DataTable)Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"]).Copy());
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 0)
                {
                    Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"] = ds.Tables[0];
                    Refresh_Grid();
                }
                else
                {
                    Refresh_Grid();
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion Get Product Type Config List

        #region Refresh Gridview
        protected void Refresh_Grid()
        {
            DataTable dt = null;
            try
            {
                grvProductType.DataSource = null;
                grvProductType.DataBind();
                dt = (DataTable)(Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"]);
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add("DUMMY", "", "", "", "", 0, "", "", "", "", "", "", "", "", false,0,0);
                    grvProductType.DataSource = dt;
                    grvProductType.DataBind();
                    grvProductType.Rows[0].Visible = false;
                }
                else if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ProductType"].ToString() == "DUMMY" && dt.Rows.Count > 1)
                    {
                        DataRow drDummy = dt.Rows[0];
                        dt.Rows.Remove(drDummy);
                    }
                    grvProductType.DataSource = dt;
                    grvProductType.DataBind();
                    if (grvProductType.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["ProductType"].ToString() == "DUMMY" && dt.Rows.Count == 1)
                        {
                            grvProductType.Rows[0].Visible = false;
                        }
                        else
                        {
                            grvProductType.Rows[0].Visible = true;
                            int rowIndex = 10 * grvProductType.PageIndex;
                            foreach (GridViewRow gvr in grvProductType.Rows)
                            {

                                //Add and set Product Type values.
                                ((DropDownList)gvr.FindControl("ddlProductTypeRow")).Items.Add(
                                    dt.Rows[rowIndex]["ProductType"].ToString());

                                //Add and set Flight Prefix value.
                                ((DropDownList)gvr.FindControl("ddlFlightPrefixRow")).Items.Add(
                                    dt.Rows[rowIndex]["FlightPrefix"].ToString());

                                //Add and set Day of Week value.
                                ((DropDownList)gvr.FindControl("ddlDayOfWeekRow")).Items.Add(
                                    dt.Rows[rowIndex]["DayOfWeek"].ToString());

                                //Add and set Month value.
                                ((DropDownList)gvr.FindControl("ddlMonthRow")).Items.Add(
                                    dt.Rows[rowIndex]["Month"].ToString());

                                //Add and set Commodity Category value.
                                ((DropDownList)gvr.FindControl("ddlCommodityCategoryRow")).Items.Add(
                                    dt.Rows[rowIndex]["CommCategory"].ToString());

                                //Set active status.
                                ((CheckBox)gvr.FindControl("chkStatusRow")).Checked =
                                    Convert.ToBoolean(dt.Rows[rowIndex]["IsActive"]);

                                //Check if Flight Number is 01/01/1900
                                if (((TextBox)gvr.FindControl("txtFlightDateRow")).Text == "01/01/1900")
                                {
                                    ((TextBox)gvr.FindControl("txtFlightDateRow")).Text = "";
                                }

                                rowIndex++;
                            }
                        }
                    }
                }
                //Load dropdowns in Footer row.
                LoadDropDowns(-1);

                //((TextBox)grvProductType.FooterRow.FindControl("txtOrigin")).Text = lblHAWBOrigin.Text;
                //((TextBox)grvProductType.FooterRow.FindControl("txtOrigin")).Enabled = false;
                //((TextBox)grvProductType.FooterRow.FindControl("txtDestination")).Text = lblHAWBDest.Text;
                //((TextBox)grvProductType.FooterRow.FindControl("txtDestination")).Enabled = false;
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }
        #endregion

        #region Load Product Types
        private void LoadDropDowns(int rowIndex)
        {
            try
            {
                //Product Type Master.
                if(Session["PR_TYPE_CONFIG_PRODUCTTYPES"] == null)
                    Session["PR_TYPE_CONFIG_PRODUCTTYPES"] = objBAL.GetProductTypes();

                //Partner Type Master.
                if (Session["PR_TYPE_CONFIG_FLIGHTPREFIXES"] == null)
                    Session["PR_TYPE_CONFIG_FLIGHTPREFIXES"] = objBAL.GetPartnerCodes();

                //Day of Week.
                if (Session["PR_TYPE_CONFIG_DAYOFWEEK"] == null)
                    Session["PR_TYPE_CONFIG_DAYOFWEEK"] = CommonUtility.GetDayOfWeekList();

                //Month.
                if (Session["PR_TYPE_CONFIG_MONTH"] == null)
                    Session["PR_TYPE_CONFIG_MONTH"] = CommonUtility.GetMonthsList();

                //Commodity Category
                if (Session["PR_TYPE_CONFIG_COMMCATEGORY"] == null)
                    Session["PR_TYPE_CONFIG_COMMCATEGORY"] = objBAL.GetCommodityCategory();

                //Load dropdowns for footer row if row index = -1.
                if (rowIndex == -1)
                {   
                    //Product Types
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlProductTypeHeader")).DataSource
                    = ((DataSet)Session["PR_TYPE_CONFIG_PRODUCTTYPES"]).Tables[1];
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlProductTypeHeader")).DataValueField = "ProductType";
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlProductTypeHeader")).DataTextField = "ProductDescription";
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlProductTypeHeader")).DataBind();

                    //Flight Prefixes
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).DataSource
                    = ((DataSet)Session["PR_TYPE_CONFIG_FLIGHTPREFIXES"]).Tables[0];
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).DataValueField = "PartnerCode";
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).DataTextField = "PartnerCode";
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).DataBind();
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).SelectedValue = "Select";

                    //Day of Week
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlDayOfWeekHeader")).DataSource
                    = (List<String>)Session["PR_TYPE_CONFIG_DAYOFWEEK"];
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlDayOfWeekHeader")).DataBind();

                    //Month
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlMonthHeader")).DataSource
                    = (List<String>)Session["PR_TYPE_CONFIG_MONTH"];
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlMonthHeader")).DataBind();

                    //Commodity Category
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlCommodityCategoryHeader")).DataSource
                    = ((DataSet)Session["PR_TYPE_CONFIG_COMMCATEGORY"]).Tables[0];
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlCommodityCategoryHeader")).DataValueField = "CommCategory";
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlCommodityCategoryHeader")).DataTextField = "CommCategoryText";
                    ((DropDownList)grvProductType.FooterRow.FindControl("ddlCommodityCategoryHeader")).DataBind();

                    //Set IsActive = True
                    ((CheckBox)grvProductType.FooterRow.FindControl("chkStatusHeader")).Checked = true;

                }
                else
                {
                    //Read previous value for setting after loading values.
                    string selectedValue = "";
                    selectedValue = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).SelectedValue;
                    //Product Types
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).DataSource
                    = ((DataSet)Session["PR_TYPE_CONFIG_PRODUCTTYPES"]).Tables[1];
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).DataValueField = "ProductType";
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).DataTextField = "ProductDescription";
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).DataBind();
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).Enabled = true;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).SelectedValue = selectedValue;

                    //Flight Prefixes
                    selectedValue = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).SelectedValue;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).DataSource
                    = ((DataSet)Session["PR_TYPE_CONFIG_FLIGHTPREFIXES"]).Tables[0];
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).DataValueField = "PartnerCode";
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).DataTextField = "PartnerCode";
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).DataBind();
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).Enabled = true;
                    if (((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).Items.Contains(
                        new ListItem(selectedValue,selectedValue)))
                        ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).SelectedValue = selectedValue;
                    else
                        ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).SelectedValue = "Select";

                    //Day of Week
                    selectedValue = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlDayOfWeekRow")).SelectedValue;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlDayOfWeekRow")).DataSource
                    = (List<String>)Session["PR_TYPE_CONFIG_DAYOFWEEK"];
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlDayOfWeekRow")).DataBind();
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlDayOfWeekRow")).Enabled = true;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlDayOfWeekRow")).SelectedValue = selectedValue;

                    //Month
                    selectedValue = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlMonthRow")).SelectedValue;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlMonthRow")).DataSource
                    = (List<String>)Session["PR_TYPE_CONFIG_MONTH"];
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlMonthRow")).DataBind();
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlMonthRow")).Enabled = true;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlMonthRow")).SelectedValue = selectedValue;

                    //Commodity Category
                    selectedValue = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).SelectedValue;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).DataSource
                    = ((DataSet)Session["PR_TYPE_CONFIG_COMMCATEGORY"]).Tables[0];
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).DataValueField = "CommCategory";
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).DataTextField = "CommCategoryText";
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).DataBind();
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).Enabled = true;
                    ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).SelectedValue = selectedValue;

                    //Enable controls for editing data in a row.
                    SetControlStatus(rowIndex, true);

                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error occured while fetching data.";
            }
        }
        #endregion Load Product Types

        #region Set Control Status
        /// <summary>
        /// Sets control status to Enabled or Disabled based.
        /// </summary>
        /// <param name="rowIndex">Index of row in which control status has to be set.</param>
        /// <param name="Status">Enable or Disable</param>
        private void SetControlStatus(int rowIndex, bool Status)
        {
            try
            {
                //Enable other controls for editing.
                ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).Enabled = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtOriginCodeRow")).Enabled = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtDestCodeRow")).Enabled = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFromDateRow")).Enabled = Status;
                ((ImageButton)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("imgFromDateRow")).Visible = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtToDateRow")).Enabled = Status;
                ((ImageButton)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("imgToDateRow")).Visible = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtAllocatedCapacityRow")).Enabled = Status;
                
                //############### Enabled status for flight fields set to False by default temporarily.
                ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).Enabled = false;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightNumberRow")).Enabled = false;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightDateRow")).Enabled = false;
                //Enabled status for flight fields set to False by default temporarily. ###############

                ((ImageButton)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("imgFltDtRow")).Visible = Status;
                ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlDayOfWeekRow")).Enabled = Status;
                ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlMonthRow")).Enabled = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtPriorityRow")).Enabled = Status;
                ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).Enabled = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCommodityCodeRow")).Enabled = Status;
                ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCapacityThresholdRow")).Enabled = Status;
                ((CheckBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("chkStatusRow")).Enabled = Status;
                ((Button)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("btnEdit")).Visible = !Status;
                ((Button)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("btnDelete")).Visible = !Status;
                ((Button)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("btnCancel")).Visible = Status;
                ((Button)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("btnUpdate")).Visible = Status;
            }
            catch (Exception)
            {
            }
        }
        #endregion Set Control Status

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"] = null;
            txtCommodityCode.Text = string.Empty;
            lblStatus.Text = string.Empty;
        }
        #endregion btnClear_Click

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                
                lblStatus.Text = "";
                //Validate from date and to date.
                DateTime dtFrom = DateTime.Now;
                if (!DateTime.TryParseExact(txtFromDate.Text,
                    "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFrom))
                {
                    lblStatus.Text = "Please select/ enter valid From Date.";
                    lblStatus.ForeColor = Color.Red;
                    txtFromDate.Focus();
                    return;
                }
                if (!DateTime.TryParseExact(txtToDate.Text,
                    "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFrom))
                {
                    lblStatus.Text = "Please select/ enter valid To Date.";
                    lblStatus.ForeColor = Color.Red;
                    txtToDate.Focus();
                    return;
                }

                Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"] = null;

                GetProductTypeConfigList(0);

                if (grvProductType.Rows.Count == 1)
                {
                    if (((DropDownList)grvProductType.Rows[0].FindControl("ddlProductTypeRow")).SelectedValue == "DUMMY"
                        || ((DropDownList)grvProductType.Rows[0].FindControl("ddlProductTypeRow")).SelectedValue == "")
                    {
                        lblStatus.Text = "No records found";
                        lblStatus.ForeColor = Color.Red;
                        lblRecordCount.Text = "Record 0 of 0";
                        return;
                    }
                }
                else
                {
                    if (grvProductType.Rows.Count == 0)
                    {
                        lblStatus.Text = "No records found";
                        lblStatus.ForeColor = Color.Red;
                        lblRecordCount.Text = "Record 0 of 0";
                    }
                }
                //Set record count.
                if (Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"] != null)
                {
                    int toCount = grvProductType.PageIndex * 10 + 10;
                    if (toCount > ((DataTable)(Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"])).Rows.Count)
                    {
                        toCount = ((DataTable)(Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"])).Rows.Count;
                    }
                    lblRecordCount.Text = "Records: " +
                        Convert.ToString(grvProductType.PageIndex * 10 + 1) + " - " +
                        toCount + " of " + ((DataTable)(Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"])).Rows.Count.ToString();
                }
                else
                {
                    lblRecordCount.Text = "Record 0 of 0";
                }
            }
            catch (Exception)
            {
            }
        }
        # endregion btnList_Click

        # region grvProductType_RowCommand
        protected void grvProductType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "STARTEDIT":
                        //Edit button clicked.
                        LoadDropDowns(Convert.ToInt32(e.CommandArgument));
                        break;
                    case "CANCELEDIT":
                        //Cancel button clicked.
                        btnList_Click(null, null);
                        break;
                    case "STARTDELETE":
                        DeleteProductTypeConfig(Convert.ToInt32(e.CommandArgument));
                        break;
                    case "STARTUPDATE":
                        SaveProductTypeConfig(Convert.ToInt32(e.CommandArgument));
                        break;
                    default:

                        break;
                }
                if (e.CommandName == "EDIT")
                {   
                    
                }
                if (e.CommandName == "EDIT")
                {   //Edit button clicked.
                    LoadDropDowns(Convert.ToInt32(e.CommandArgument));
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion grvProductType_RowCommand

        #region grvProductType_PageIndexChanging
        protected void grvProductType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                DataTable dt = (DataTable)Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"];
                grvProductType.PageIndex = e.NewPageIndex;
                grvProductType.DataSource = dt; //ds.Copy();
                grvProductType.DataBind();

                if (grvProductType.Rows.Count > 0)
                {
                    grvProductType.Rows[0].Visible = true;
                    int rowIndex = 10 * e.NewPageIndex;
                    foreach (GridViewRow gvr in grvProductType.Rows)
                    {

                        //Add and set Product Type values.
                        ((DropDownList)gvr.FindControl("ddlProductTypeRow")).Items.Add(
                            dt.Rows[rowIndex]["ProductType"].ToString());

                        //Add and set Flight Prefix value.
                        ((DropDownList)gvr.FindControl("ddlFlightPrefixRow")).Items.Add(
                            dt.Rows[rowIndex]["FlightPrefix"].ToString());

                        //Add and set Day of Week value.
                        ((DropDownList)gvr.FindControl("ddlDayOfWeekRow")).Items.Add(
                            dt.Rows[rowIndex]["DayOfWeek"].ToString());

                        //Add and set Month value.
                        ((DropDownList)gvr.FindControl("ddlMonthRow")).Items.Add(
                            dt.Rows[rowIndex]["Month"].ToString());

                        //Add and set Commodity Category value.
                        ((DropDownList)gvr.FindControl("ddlCommodityCategoryRow")).Items.Add(
                            dt.Rows[rowIndex]["CommCategory"].ToString());

                        //Set active status.
                        ((CheckBox)gvr.FindControl("chkStatusRow")).Checked =
                            Convert.ToBoolean(dt.Rows[rowIndex]["IsActive"]);

                        //Check if Flight Number is 01/01/1900
                        if (((TextBox)gvr.FindControl("txtFlightDateRow")).Text == "01/01/1900")
                        {
                            ((TextBox)gvr.FindControl("txtFlightDateRow")).Text = "";
                        }

                        rowIndex++;
                    }
                }

                //Set record count.
                if (Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"] != null)
                {
                    int toCount = e.NewPageIndex * 10 + 10;
                    if (toCount > ((DataTable)(Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"])).Rows.Count)
                    {
                        toCount = ((DataTable)(Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"])).Rows.Count;
                    }
                    lblRecordCount.Text = "Records: " +
                        Convert.ToString(e.NewPageIndex * 10 + 1) + " - " +
                        toCount + " of " + ((DataTable)(Session["PR_TYPE_CONFIG_PRODUCTCONFIGLIST"])).Rows.Count.ToString();
                }
                else
                {
                    lblRecordCount.Text = "Record 0 of 0";
                }

                //Load dropdown values in Footer row.
                LoadDropDowns(-1);

            }
            catch (Exception ex)
            {
            }
        }

        # endregion grvProductType_PageIndexChanging

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveProductTypeConfig(-1);
            }
            catch (Exception)
            {

            }
        }
        #endregion btnSave_Click

        #region Save Product Type Config
        private void SaveProductTypeConfig(int rowIndex)
        {
            try
            {
                if (!ValidateProductTypeConfig(rowIndex))
                {
                    return;
                }
                object[] ParameterValues = new object[20];
                if (rowIndex > -1)
                {   //Edit Product Type Config
                    ParameterValues[0] = ((Label)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("lblSerialNumber")).Text;
                    ParameterValues[1] = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).SelectedValue;
                    ParameterValues[2] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtOriginCodeRow")).Text;
                    ParameterValues[3] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtDestCodeRow")).Text;
                    //If Flight Prefix is not selected then ignore Flight #.
                    if (((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).SelectedValue.ToUpper() == "SELECT")
                    {
                        ParameterValues[4] = "";
                    }
                    else
                    {
                        ParameterValues[4] = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).SelectedValue
                                        + ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightNumberRow")).Text;
                    }
                    ParameterValues[5] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightDateRow")).Text;
                    ParameterValues[6] = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlDayOfWeekRow")).SelectedValue;
                    ParameterValues[7] = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlMonthRow")).SelectedValue;
                    ParameterValues[8] = ((CheckBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("chkStatusRow")).Checked;
                    ParameterValues[9] = Session["UserName"].ToString();
                    ParameterValues[10] = Convert.ToDateTime(Session["IT"]);
                    ParameterValues[11] = Session["UserName"].ToString();
                    ParameterValues[12] = Convert.ToDateTime(Session["IT"]);
                    ParameterValues[13] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFromDateRow")).Text;
                    ParameterValues[14] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtToDateRow")).Text;
                    ParameterValues[15] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtAllocatedCapacityRow")).Text;
                    ParameterValues[16] = ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlCommodityCategoryRow")).SelectedValue;
                    ParameterValues[17] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCommodityCodeRow")).Text;
                    ParameterValues[18] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCapacityThresholdRow")).Text;
                    ParameterValues[19] = ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtPriorityRow")).Text;
                }
                else
                {   //New Product Type Config
                    ParameterValues[0] = 0;
                    ParameterValues[1] = ((DropDownList)grvProductType.FooterRow.FindControl("ddlProductTypeHeader")).SelectedValue;
                    ParameterValues[2] = ((TextBox)grvProductType.FooterRow.FindControl("txtOriginCodeHeader")).Text;
                    ParameterValues[3] = ((TextBox)grvProductType.FooterRow.FindControl("txtDestCodeHeader")).Text;
                    //If Flight Prefix is not selected then ignore Flight #.
                    if (((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).SelectedValue.ToUpper() == "SELECT")
                    {
                        ParameterValues[4] = "";
                    }
                    else
                    {
                        ParameterValues[4] = ((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).SelectedValue
                                + ((TextBox)grvProductType.FooterRow.FindControl("txtFlightNumberHeader")).Text;
                    }
                    ParameterValues[5] = ((TextBox)grvProductType.FooterRow.FindControl("txtFlightDateHeader")).Text;
                    ParameterValues[6] = ((DropDownList)grvProductType.FooterRow.FindControl("ddlDayOfWeekHeader")).SelectedValue;
                    ParameterValues[7] = ((DropDownList)grvProductType.FooterRow.FindControl("ddlMonthHeader")).SelectedValue;
                    ParameterValues[8] = ((CheckBox)grvProductType.FooterRow.FindControl("chkStatusHeader")).Checked;
                    ParameterValues[9] = Session["UserName"].ToString();
                    ParameterValues[10] = Convert.ToDateTime(Session["IT"]);
                    ParameterValues[11] = Session["UserName"].ToString();
                    ParameterValues[12] = Convert.ToDateTime(Session["IT"]);
                    ParameterValues[13] = ((TextBox)grvProductType.FooterRow.FindControl("txtFromDateHeader")).Text;
                    ParameterValues[14] = ((TextBox)grvProductType.FooterRow.FindControl("txtToDateHeader")).Text;
                    ParameterValues[15] = ((TextBox)grvProductType.FooterRow.FindControl("txtAllocatedCapacityHeader")).Text;
                    ParameterValues[16] = ((DropDownList)grvProductType.FooterRow.FindControl("ddlCommodityCategoryHeader")).SelectedValue;
                    ParameterValues[17] = ((TextBox)grvProductType.FooterRow.FindControl("txtCommodityCodeHeader")).Text;
                    ParameterValues[18] = ((TextBox)grvProductType.FooterRow.FindControl("txtCapacityThresholdHeader")).Text;
                    ParameterValues[19] = ((TextBox)grvProductType.FooterRow.FindControl("txtPriorityHeader")).Text;
                }
                //Save row.
                if (objBAL.SaveProductTypeConfig(ParameterValues) == 0)
                {
                    btnList_Click(null, null);
                    lblStatus.Text = "Product Type Configuration Saved Successfully.";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "Saving Product Type Configuration Failed. Please try again.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Saving Product Type Configuration Failed: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion Save Product Type Config

        #region Validate Product Type Config
        private bool ValidateProductTypeConfig(int rowIndex)
        {
            try
            {
                if (rowIndex > -1)
                {   //Validate data in a row.
                    if (((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).SelectedValue.ToUpper() == "SELECT")
                    {
                        lblStatus.Text = "Please select Product Type.";
                        lblStatus.ForeColor = Color.Red;
                        ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlProductTypeRow")).Focus();
                        return false;
                    }
                    DateTime dtFrom = DateTime.Now;
                    if (!DateTime.TryParseExact(((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFromDateRow")).Text,
                        "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFrom))
                    {
                        lblStatus.Text = "Please select/ enter valid From Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFromDateRow")).Focus();
                        return false;
                    }
                    DateTime dtTo = DateTime.Now;
                    if (!DateTime.TryParseExact(((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtToDateRow")).Text,
                        "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtTo))
                    {
                        lblStatus.Text = "Please select/ enter valid To Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtToDateRow")).Focus();
                        return false;
                    }
                    if (dtTo < dtFrom)
                    {
                        lblStatus.Text = "To Date cannot be older than From Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtToDateRow")).Focus();
                        return false;
                    }
                    decimal dec = 0;
                    if (!Decimal.TryParse(((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtAllocatedCapacityRow")).Text, out dec))
                    {
                        lblStatus.Text = "Please enter valid Allocated Capacity.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtAllocatedCapacityRow")).Focus();
                        return false;
                    }
                    int threshold = 0;
                    //If threshold is blank then set to 0.
                    if (((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCapacityThresholdRow")).Text == "")
                        ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCapacityThresholdRow")).Text = "0";
                    if (!int.TryParse(((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCapacityThresholdRow")).Text, out threshold))
                    {
                        lblStatus.Text = "Please enter valid Capacity Threshold.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtCapacityThresholdRow")).Focus();
                        return false;
                    }
                    if (((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightDateRow")).Text != "" &&
                        !DateTime.TryParseExact(((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightDateRow")).Text,
                        "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFrom))
                    {
                        lblStatus.Text = "Please select/ enter valid Flight Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightDateRow")).Focus();
                        return false;
                    }
                    if (((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).SelectedValue.ToUpper() == "SELECT"
                        && ((TextBox)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("txtFlightNumberRow")).Text != "")
                    {
                        lblStatus.Text = "Please select valid Flight Prefix.";
                        lblStatus.ForeColor = Color.Red;
                        ((DropDownList)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("ddlFlightPrefixRow")).Focus();
                        return (false);
                    }
                }
                else
                {   //Validate data in footer row.
                    if (((DropDownList)grvProductType.FooterRow.FindControl("ddlProductTypeHeader")).SelectedValue.ToUpper() == "SELECT")
                    {
                        lblStatus.Text = "Please select Product Type.";
                        lblStatus.ForeColor = Color.Red;
                        ((DropDownList)grvProductType.FooterRow.FindControl("ddlProductTypeHeader")).Focus();
                        return false;
                    }
                    DateTime dtFrom = DateTime.Now;
                    if (!DateTime.TryParseExact(((TextBox)grvProductType.FooterRow.FindControl("txtFromDateHeader")).Text,
                        "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFrom))
                    {
                        lblStatus.Text = "Please select/ enter valid From Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.FooterRow.FindControl("txtFromDateHeader")).Focus();
                        return false;
                    }
                    DateTime dtTo = DateTime.Now;
                    if (!DateTime.TryParseExact(((TextBox)grvProductType.FooterRow.FindControl("txtToDateHeader")).Text,
                        "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtTo))
                    {
                        lblStatus.Text = "Please select/ enter valid To Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.FooterRow.FindControl("txtToDateHeader")).Focus();
                        return false;
                    }
                    if (dtTo < dtFrom)
                    {
                        lblStatus.Text = "To Date cannot be older than From Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.FooterRow.FindControl("txtToDateHeader")).Focus();
                        return false;
                    }
                    decimal dec = 0;
                    if (!Decimal.TryParse(((TextBox)grvProductType.FooterRow.FindControl("txtAllocatedCapacityHeader")).Text, out dec))
                    {
                        lblStatus.Text = "Please enter valid Allocated Capacity.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.FooterRow.FindControl("txtAllocatedCapacityHeader")).Focus();
                        return false;
                    }
                    //If threshold is blank then set to 0.
                    if (((TextBox)grvProductType.FooterRow.FindControl("txtCapacityThresholdHeader")).Text == "")
                        ((TextBox)grvProductType.FooterRow.FindControl("txtCapacityThresholdHeader")).Text = "0";
                    int threshold = 0;
                    if (!int.TryParse(((TextBox)grvProductType.FooterRow.FindControl("txtCapacityThresholdHeader")).Text, out threshold))
                    {
                        lblStatus.Text = "Please enter valid Capacity Threshold.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.FooterRow.FindControl("txtCapacityThresholdHeader")).Focus();
                        return false;
                    }
                    if (((TextBox)grvProductType.FooterRow.FindControl("txtFlightDateHeader")).Text != "" && 
                        !DateTime.TryParseExact(((TextBox)grvProductType.FooterRow.FindControl("txtFlightDateHeader")).Text,
                        "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtFrom))
                    {
                        lblStatus.Text = "Please select/ enter valid Flight Date.";
                        lblStatus.ForeColor = Color.Red;
                        ((TextBox)grvProductType.FooterRow.FindControl("txtFlightDateHeader")).Focus();
                        return false;
                    }
                    if (((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).SelectedValue.ToUpper() == "SELECT"
                        && ((TextBox)grvProductType.FooterRow.FindControl("txtFlightNumberHeader")).Text != "")
                    {
                        lblStatus.Text = "Please select valid Flight Prefix.";
                        lblStatus.ForeColor = Color.Red;
                        ((DropDownList)grvProductType.FooterRow.FindControl("ddlFlightPrefixHeader")).Focus();
                        return (false);
                    }
                }
                return (true);
            }
            catch (Exception)
            {
                return (false);
            }
        }
        #endregion Validate Product Type Config

        #region Delete Product Type Config
        private void DeleteProductTypeConfig(int rowIndex)
        {
            try
            {
                string ParameterValue = 
                    ((Label)grvProductType.Rows[rowIndex - 10 * grvProductType.PageIndex].FindControl("lblSerialNumber")).Text;
                //Save row.
                if (objBAL.DeleteProdTypeConfig(Convert.ToInt64(ParameterValue)) == 0)
                {
                    btnList_Click(null, null);
                    lblStatus.Text = "Product type configuration deleted successfully.";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "Deleting product type configuration failed. Please try again.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Deleting product type configuration failed: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion Save Product Type Config

    }
}
