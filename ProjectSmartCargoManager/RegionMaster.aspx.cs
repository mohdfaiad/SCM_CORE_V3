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
    public partial class RegionMaster : System.Web.UI.Page
    {

        #region Variable
        BALRegionMaster objBAL = new BALRegionMaster();
        #endregion variable

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCountryCode();
                Session["dsRegionMaster"] = null;
                BindEmptyRow();
            }

        }
        #endregion Page_Load

        #region Bind Empty Row
        protected void BindEmptyRow()
        {
            DataTable dtEmpty = new DataTable();
            try
            {
                dtEmpty.Columns.Add("RegionCode");
                dtEmpty.Columns.Add("RegionName");
                dtEmpty.Columns.Add("CountryCode");
                dtEmpty.Columns.Add("CountryName");
                dtEmpty.Columns.Add("IsActive");

                DataRow dr = dtEmpty.NewRow();
                dtEmpty.Rows.Add(dr);

                grvRegionList.DataSource = dtEmpty;
                grvRegionList.DataBind();

                grvRegionList.Columns[5].Visible = false;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dtEmpty != null)
                    dtEmpty.Dispose();
            }
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string CountryName = string.Empty;

                if (txtRegionCode.Text == "")
                {
                    lblStatus.Text = "Please Enter Region Code";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                else if (txtRegionName.Text == "")
                {
                    lblStatus.Text = "Please Enter Region Name";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                else if (ddlCountryCode.SelectedIndex == 0)
                {
                    lblStatus.Text = "Please Select Country Code";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {

                    CountryName = ddlCountryCode.SelectedItem.Text.ToString();
                    string str = ddlCountryCode.SelectedItem.Value.ToString() + "-";
                    CountryName = CountryName.Replace(str, string.Empty);
                    CountryName = CountryName.Trim(' ');
                }
                # region Save

                if (btnSave.Text == "Save")
                {
                    try
                    {

                        #region Prepare Parameters
                        object[] RegionCountryInfo = new object[2];
                        int j = 0;

                        //0
                        RegionCountryInfo.SetValue(txtRegionCode.Text.Trim(), j);
                        j++;

                        //1
                        RegionCountryInfo.SetValue(ddlCountryCode.SelectedValue.ToString(), j);
                        j++;


                        #endregion Prepare Parameters

                        DataSet ds = new DataSet();
                        ds = objBAL.chkRegionWithCountry(RegionCountryInfo);

                        if (ds != null)
                        {
                            if (ds.Tables != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Country with Region Already Exist ";
                                }
                                else
                                {
                                    #region Prepare Parameters
                                    object[] RegionInfo = new object[5];
                                    int i = 0;

                                    //0
                                    RegionInfo.SetValue(txtRegionCode.Text.Trim(), i);
                                    i++;

                                    //1
                                    RegionInfo.SetValue(txtRegionName.Text.Trim(), i);
                                    i++;

                                    //2
                                    RegionInfo.SetValue(ddlCountryCode.SelectedValue.ToString(), i);
                                    i++;

                                    //3
                                    RegionInfo.SetValue(CountryName, i);
                                    i++;

                                    //4
                                    if (chkActive.Checked == true)
                                    {
                                        RegionInfo.SetValue("True", i);
                                    }
                                    else
                                    {
                                        RegionInfo.SetValue("False", i);
                                    }


                                    #endregion Prepare Parameters

                                    int ID = 0;
                                    ID = objBAL.AddRegion(RegionInfo);
                                    if (ID >= 0)
                                    {
                                        #region for Master Audit Log
                                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                        #region Prepare Parameters
                                        object[] Params = new object[7];
                                        int k = 0;

                                        //1

                                        Params.SetValue("RegionMaster", k);
                                        k++;

                                        //2
                                        Params.SetValue("Region Code:"+txtRegionCode.Text, k);
                                        k++;

                                        //3

                                        Params.SetValue("ADD", k);
                                        k++;

                                        //4

                                        Params.SetValue("New Region Added", k);
                                        k++;


                                        //5
                                        string Desc="New Region "+txtRegionCode.Text+ " added";
                                        Params.SetValue(Desc, k);
                                        k++;

                                        //6

                                        Params.SetValue(Session["UserName"], k);
                                        k++;

                                        //7
                                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                                        k++;


                                        #endregion Prepare Parameters
                                        ObjMAL.AddMasterAuditLog(Params);
                                        #endregion
                                        txtRegionCode.Text = string.Empty;
                                        txtRegionName.Text = string.Empty;
                                        ddlCountryCode.SelectedIndex = 0;
                                        txtCountryName.Text = string.Empty;
                                        chkActive.Checked = false;
                                        btnSave.Text = "Save";
                                        lblStatus.Text = string.Empty;

                                        btnList_Click(null, null);
                                        lblStatus.ForeColor = Color.Green;
                                        lblStatus.Text = "Region Added Sucessfully..";
                                        btnSave.Text = "Save";
                                    }
                                    else
                                    {
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "Region Insertion Failed..";
                                    }
                                }
                            }
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
                    try
                    {

                        //btnList_Click(sender, e);

                        #region Prepare Parameters
                        object[] updateRegionInfo = new object[5];
                        int i = 0;

                        //0
                        updateRegionInfo.SetValue(txtRegionCode.Text.Trim(), i);
                        i++;

                        //1
                        updateRegionInfo.SetValue(txtRegionName.Text.Trim(), i);
                        i++;

                        //2
                        updateRegionInfo.SetValue(ddlCountryCode.SelectedValue.ToString(), i);
                        i++;

                        //3
                        updateRegionInfo.SetValue(CountryName, i);
                        i++;

                        //4
                        if (chkActive.Checked == true)
                        {
                            updateRegionInfo.SetValue("True", i);
                        }
                        else
                        {
                            updateRegionInfo.SetValue("False", i);
                        }


                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateRgion(updateRegionInfo);
                        if (UpdateID >= 0)
                        {

                            #region for Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int k = 0;

                            //1

                            Params.SetValue("RegionMaster", k);
                            k++;

                            //2
                            Params.SetValue("Region Code:"+txtRegionCode.Text, k);
                            k++;

                            //3

                            Params.SetValue("ADD", k);
                            k++;

                            //4

                            Params.SetValue("Region Updated", k);
                            k++;


                            //5
                            string Desc = "";
                            Params.SetValue(Desc, k);
                            k++;

                            //6

                            Params.SetValue(Session["UserName"], k);
                            k++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                            k++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion
                            txtRegionCode.Text = string.Empty;
                            txtRegionName.Text = string.Empty;
                            ddlCountryCode.SelectedIndex = 0;
                            txtCountryName.Text = string.Empty;
                            chkActive.Checked = false;
                            btnSave.Text = "Save";
                            lblStatus.Text = string.Empty;

                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Region Updated Sucessfully..";

                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Region Updation Failed..";
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                # endregion Update
            }
            catch (Exception ex)
            {
            }
        }
        #endregion btnSave_Click

        # region GetCountryCode List
        private void GetCountryCode()
        {
            try
            {
                DataSet ds = objBAL.GetCountryCodeList(ddlCountryCode.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlCountryCode.DataSource = ds;
                            ddlCountryCode.DataMember = ds.Tables[0].TableName;
                            ddlCountryCode.DataValueField = ds.Tables[0].Columns["CountryCode"].ColumnName;

                            ddlCountryCode.DataTextField = ds.Tables[0].Columns["CountryDesc"].ColumnName;
                            ddlCountryCode.DataBind();
                            ddlCountryCode.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters
                object[] RegionListInfo = new object[4];
                int i = 0;

                //0
                RegionListInfo.SetValue(txtRegionCode.Text, i);
                i++;

                //1
                RegionListInfo.SetValue(txtRegionName.Text, i);
                i++;

                //2
                if (ddlCountryCode.SelectedIndex == 0)
                {
                    RegionListInfo.SetValue("", i);
                    i++;
                }
                else
                {
                    RegionListInfo.SetValue(ddlCountryCode.SelectedValue.ToString(), i);
                    i++;
                }

                //3
                if (chkActive.Checked == true)
                {
                    RegionListInfo.SetValue("true", i);
                    i++;
                }
                else if (chkActive.Checked == false)
                {
                    RegionListInfo.SetValue("false", i);
                    i++;
                }



                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetRegionList(RegionListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvRegionList.PageIndex = 0;
                                grvRegionList.DataSource = ds;
                                grvRegionList.DataMember = ds.Tables[0].TableName;
                                grvRegionList.DataBind();
                                grvRegionList.Visible = true;
                                ViewState["dsRegionData"] = ds;
                                Session["dsRegionMaster"] = ds;
                                grvRegionList.Columns[5].Visible = true;
                            }
                            else
                            {
                                lblStatus.Text = "Records not available for selected criteria...";
                                lblStatus.ForeColor = Color.Red;
                                Session["dsRegionMaster"] = null;
                                BindEmptyRow();
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
                    Label lblRegCode = (Label)grvRegionList.Rows[RowIndex].FindControl("lblRegionCode");
                    Label lblRegName = (Label)grvRegionList.Rows[RowIndex].FindControl("lblRegionName");
                    Label lblCounCode = (Label)grvRegionList.Rows[RowIndex].FindControl("lblCoutryCode");
                    Label lblCounName = (Label)grvRegionList.Rows[RowIndex].FindControl("lblCoutryName");
                    Label lblstat = (Label)grvRegionList.Rows[RowIndex].FindControl("lblStatus");


                    txtRegionCode.Text = lblRegCode.Text;
                    txtRegionName.Text = lblRegName.Text;
                    ddlCountryCode.SelectedIndex = ddlCountryCode.Items.IndexOf(((ListItem)ddlCountryCode.Items.FindByValue(lblCounCode.Text.Trim())));
                    txtCountryName.Text = lblCounName.Text;

                    if (lblstat.Text.ToUpper() == "ACTIVE")
                    {
                        chkActive.Checked = true;
                    }
                    else
                    {
                        chkActive.Checked = false;
                    }
                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion grvRegionList_RowCommand

        #region grvRegionList_RowEditing
        protected void grvRegionList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion grvRegionList_RowEditing

        #region grvRegionList_PageIndexChanging
        protected void grvRegionList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dsRegion = new DataSet();
            try
            {
                dsRegion = (DataSet)ViewState["dsRegionData"];
                grvRegionList.PageIndex = e.NewPageIndex;
                grvRegionList.DataSource = dsRegion.Tables[0];
                grvRegionList.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion grvRegionList_PageIndexChanging

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtRegionCode.Text = string.Empty;
            txtRegionName.Text = string.Empty;
            ddlCountryCode.SelectedIndex = 0;
            txtCountryName.Text = string.Empty;
            chkActive.Checked = false;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;

            grvRegionList.DataSource = null;
            grvRegionList.DataBind();
        }
        #endregion btnClear_Click

        # region ddlCountryCode_SelectedIndexChanged
        protected void ddlCountryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCountryCode.SelectedIndex == 0)
            {
                txtCountryName.Text = string.Empty;
                return;
            }
            try
            {
                #region Prepare Parameters
                object[] CountryNameInfo = new object[1];
                int i = 0;

                //0
                CountryNameInfo.SetValue(ddlCountryCode.SelectedItem.Text, i);
                i++;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetCountryName(CountryNameInfo);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //txtCountryName.Text = ds.tables[0].rows[0][0].ToString();
                                txtCountryName.Text = ds.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        # endregion ddlCountryCode_SelectedIndexChanged

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            try
            {
                if (Session["dsRegionMaster"] == null)
                {
                    btnList_Click(null, null);
                    BindEmptyRow();
                }
                lblStatus.Text = string.Empty;

                dsExp = (DataSet)Session["dsRegionMaster"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)dsExp.Tables[0];

                    string attachment = "attachment; filename=RegionMaster.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dt.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found for the selected search criteria!');</SCRIPT>", false);
                    return;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsExp != null)
                    dsExp = null;
                if (dt != null)
                    dt = null;
            }
        }
    }
}