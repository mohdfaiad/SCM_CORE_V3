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
    public partial class CountryMaster : System.Web.UI.Page
    {
        # region Variable

        BALCountryMaster objBAL = new BALCountryMaster();

        # endregion Variable

        # region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["dsCountryMaster"] = null;
                GetCurrencyCode();
                BindEmptyRow();
            }
        }
        # endregion Page_Load

        #region Bind Empty Row
        protected void BindEmptyRow()
        {
            DataTable dtEmpty = new DataTable();

            try
            {
                dtEmpty.Columns.Add("CountryCode", typeof(string));
                dtEmpty.Columns.Add("CountryName", typeof(string));
                dtEmpty.Columns.Add("CurrencyCode", typeof(string));
                dtEmpty.Columns.Add("IsActive", typeof(string));

                DataRow dr = dtEmpty.NewRow();
                dtEmpty.Rows.Add(dr);

                grvCountryList.DataSource = dtEmpty;
                grvCountryList.DataBind();

                grvCountryList.Columns[4].Visible = false;
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

        # region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                # region Save
                if (btnSave.Text == "Save")
                {
                    try
                    {
                        if (txtCountryCode.Text == "")
                        {
                            lblStatus.Text = "Please Enter Country Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (txtCountryName.Text == "")
                        {
                            lblStatus.Text = "Please Enter Country Name";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (ddlCurrencyCode.SelectedIndex == 0)
                        {
                            lblStatus.Text = "Please Select Currency Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        #region Prepare Parameters
                        object[] CountrycodeInfo = new object[1];
                        int j = 0;

                        //0
                        CountrycodeInfo.SetValue(txtCountryCode.Text.Trim(), j);
                        j++;

                      
                        #endregion Prepare Parameters
                        int k = 0;
                        DataSet ds = new DataSet();
                        ds = objBAL.chkCountryCode(CountrycodeInfo);


                       if(ds != null)
                        {
                            if (ds.Tables != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Country Already Exist ";
                                }
                                else
                                {

                                    #region Prepare Parameters
                                    object[] countryInfo = new object[4];
                                    int i = 0;

                                    //0
                                    countryInfo.SetValue(txtCountryCode.Text.Trim(), i);
                                    i++;

                                    //1
                                    countryInfo.SetValue(txtCountryName.Text.Trim(), i);
                                    i++;

                                    //2
                                    countryInfo.SetValue(ddlCurrencyCode.SelectedValue.ToString(), i);
                                    i++;

                                    //3
                                    if (chkActive.Checked == true)
                                    {
                                        countryInfo.SetValue("True", i);
                                    }
                                    else
                                    {
                                        countryInfo.SetValue("False", i);
                                    }


                                    #endregion Prepare Parameters

                                    int ID = 0;
                                    ID = objBAL.AddCountry(countryInfo);
                                    
                                    if (ID >= 0)
                                    {
                                        #region for Master Audit Log
                                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                        #region Prepare Parameters
                                        object[] Params = new object[7];


                                        //1

                                        Params.SetValue("CountryMaster", k);
                                        k++;

                                        //2
                                        Params.SetValue(txtCountryCode.Text, k);
                                        k++;

                                        //3

                                        Params.SetValue("ADD", k);
                                        k++;

                                        //4

                                        Params.SetValue("", k);
                                        k++;


                                        //5

                                        Params.SetValue("", k);
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
                                        txtCountryCode.Text = "";
                                        txtCountryName.Text = "";
                                        ddlCurrencyCode.SelectedIndex = 0;
                                        chkActive.Checked = false;

                                        lblStatus.ForeColor = Color.Green;
                                        lblStatus.Text = "Country Added Sucessfully..";
                                        btnSave.Text = "Save";
                                        
                                        btnList_Click(null, null);
                                    }
                                    else
                                    {
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "Country Insertion Failed..";
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
                    if (txtCountryCode.Text == "")
                    {
                        lblStatus.Text = "Please Enter Country Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    if (txtCountryName.Text == "")
                    {
                        lblStatus.Text = "Please Enter Country Name";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    if (ddlCurrencyCode.SelectedIndex == 0)
                    {
                        lblStatus.Text = "Please Select Currency Code";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    try
                    {
                        #region Prepare Parameters
                        object[] updatecountryInfo = new object[4];
                        int i = 0;

                        //0
                        updatecountryInfo.SetValue(txtCountryCode.Text.Trim(), i);
                        i++;

                        //1
                        updatecountryInfo.SetValue(txtCountryName.Text.Trim(), i);
                        i++;

                        //2
                        updatecountryInfo.SetValue(ddlCurrencyCode.SelectedValue.ToString(), i);
                        i++;

                        //3
                        if (chkActive.Checked == true)
                        {
                            updatecountryInfo.SetValue("True", i);
                        }
                        else
                        {
                            updatecountryInfo.SetValue("False", i);
                        }


                        #endregion Prepare Parameters
                        int k = 0;
                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateCountry(updatecountryInfo);
                        if (UpdateID >= 0)
                        {
                            #region for Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];


                            //1

                            Params.SetValue("CountryMaster", k);
                            k++;

                            //2
                            Params.SetValue(txtCountryCode.Text, k);
                            k++;

                            //3

                            Params.SetValue("UPDATE", k);
                            k++;

                            //4

                            Params.SetValue("", k);
                            k++;


                            //5

                            Params.SetValue("", k);
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
                            txtCountryCode.Text = "";
                            txtCountryName.Text = "";
                            ddlCurrencyCode.SelectedIndex = 0;
                            chkActive.Checked = false;
                            btnSave.Text = "Save";
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Country Updated Sucessfully..";
                            
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Country Updation Failed..";
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
        # endregion btnSave_Click

        # region GetCurrencyCode List
        private void GetCurrencyCode()
        {
            try
            {
                DataSet ds = objBAL.GetCurrencyCodeList(ddlCurrencyCode.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlCurrencyCode.DataSource = ds;
                            ddlCurrencyCode.DataMember = ds.Tables[0].TableName;
                            ddlCurrencyCode.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlCurrencyCode.DataTextField = ds.Tables[0].Columns["CurrencyCode"].ColumnName;
                            ddlCurrencyCode.DataBind();
                            ddlCurrencyCode.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List 

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                #region Prepare Parameters
                object[] countryListInfo = new object[4];
                int i = 0;

                //0
                countryListInfo.SetValue(txtCountryCode.Text, i);
                i++;

                //1
                countryListInfo.SetValue(txtCountryName.Text, i);
                i++;

                //2


                if (ddlCurrencyCode.SelectedItem.Text == "Select")
                {
                    countryListInfo.SetValue("", i);
                }
                else
                {
                    string countryCode = ddlCurrencyCode.SelectedItem.Text;
                    countryCode = countryCode.Substring(0, countryCode.IndexOf("-")).Trim();
                    countryListInfo.SetValue(countryCode, i);
                }
                i++;
                //3
                countryListInfo.SetValue(chkActive.Checked, i);
               

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetCountryList(countryListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvCountryList.PageIndex = 0;
                                grvCountryList.DataSource = ds;
                                grvCountryList.DataMember = ds.Tables[0].TableName;
                                grvCountryList.DataBind();
                                grvCountryList.Visible = true;
                                grvCountryList.Columns[4].Visible = true;
                                Session["dsCountryMaster"] = ds;
                            }
                            else
                            {
                                lblStatus.Text = "No Records Found";
                                lblStatus.ForeColor = Color.Red;
                                Session["dsCountryMaster"] = null;
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
        # endregion btnList_Click

        # region grvCountryList_RowCommand
        protected void grvCountryList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblContCode = (Label)grvCountryList.Rows[RowIndex].FindControl("lblCountryCode");
                    Label lblContName = (Label)grvCountryList.Rows[RowIndex].FindControl("lblCountryName");
                    Label lblCurrCode = (Label)grvCountryList.Rows[RowIndex].FindControl("lblCurrencyCode");
                    Label lblstat = (Label)grvCountryList.Rows[RowIndex].FindControl("lblStatus");


                    txtCountryCode.Text = lblContCode.Text;
                    txtCountryName.Text = lblContName.Text;
                    //ddlCurrencyCode.Text = lblCurrCode.Text;
                    ddlCurrencyCode.SelectedIndex = ddlCurrencyCode.Items.IndexOf(((ListItem)ddlCurrencyCode.Items.FindByValue(lblCurrCode.Text)));

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
        # endregion grvCountryList_RowCommand

        # region grvCountryList_RowEditing
        protected void grvCountryList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvCountryList_RowEditing

        # region grvCountryList_PageIndexChanging
        protected void grvCountryList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet ds = (DataSet)Session["dsCountryMaster"];
                grvCountryList.PageIndex = e.NewPageIndex;
                grvCountryList.DataSource = ds.Copy();
                grvCountryList.DataBind();
            }
            catch (Exception ex)
            { }
        }
        # endregion grvCountryList_PageIndexChanging

        # region btnClear_Click
         protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCountryCode.Text = string.Empty;
            txtCountryName.Text = string.Empty;
            ddlCurrencyCode.SelectedIndex = 0;
            chkActive.Checked = false;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
            //grvCountryList.DataSource = null;
            //grvCountryList.DataBind();
            BindEmptyRow();
        }
        # endregion btnClear_Click

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;
            try
            {
                if (Session["dsCountryMaster"] == null)
                {
                    btnList_Click(null, null);
                    BindEmptyRow();
                }
                lblStatus.Text=string.Empty;

                dsExp = (DataSet)Session["dsCountryMaster"];
                if (dsExp != null && dsExp.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)dsExp.Tables[0];

                    string attachment = "attachment; filename=CountryMaster.xls";
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
