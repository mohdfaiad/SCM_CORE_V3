using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class UserAccessException : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDowns();
            }
        }

        protected void LoadDropDowns()
        {
            DataSet ds = new DataSet();
            DataSet dsRole = new DataSet();
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                ds=da.SelectRecords("sp_GetModulePage");
                dsRole = da.SelectRecords("SpGetRoles");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddlModule.DataSource = ds.Tables[0];
                    ddlModule.DataTextField = "ModuleName";
                    ddlModule.DataValueField = "ModuleID";
                    ddlModule.DataBind();
                    ddlModule.Items.Insert(0, new ListItem("All", "0"));
                    ddlModule.SelectedIndex = 0;
                    ddlModule_SelectedIndexChanged(null, null);
                }
                if (dsRole != null && dsRole.Tables[0].Rows.Count > 0)
                {
                    ddlRole.DataSource = dsRole.Tables[0];
                    ddlRole.DataTextField = "RoleName";
                    ddlRole.DataValueField = "RoleID";
                    ddlRole.DataBind();
                    ddlRole.Items.Insert(0, new ListItem("All", "0"));
                    ddlRole.SelectedIndex = 0;
                    ddlRole_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                if (dsRole != null)
                    dsRole.Dispose();
            }
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                if (ddlModule.SelectedIndex > 0)
                {
                    SQLServer da = new SQLServer(Global.GetConnectionString());
                    ds = da.SelectRecords("sp_GetModulePage", "moduleId", ddlModule.SelectedValue, SqlDbType.VarChar);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlPage.Items.Clear();
                        ddlPage.DataSource = ds.Tables[1];
                        ddlPage.DataTextField = "PageName";
                        ddlPage.DataValueField = "PageURL";
                        ddlPage.DataBind();
                        ddlPage.Items.Insert(0, new ListItem("All", "All"));
                        ddlPage.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlPage.Items.Clear();
                        ddlPage.Items.Insert(0, new ListItem("All", "All"));
                    }
                }
                else
                {
                    ddlPage.Items.Clear();
                    ddlPage.Items.Insert(0, new ListItem("All", "All"));
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

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                if (ddlRole.SelectedIndex > 0)
                {
                    SQLServer da = new SQLServer(Global.GetConnectionString());
                    ds = da.SelectRecords("sp_GetUserList", "RoleId", ddlRole.SelectedValue, SqlDbType.VarChar);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlUser.Items.Clear();
                        ddlUser.DataSource = ds.Tables[0];
                        ddlUser.DataTextField = "LoginName";
                        ddlUser.DataBind();
                        ddlUser.Items.Insert(0, new ListItem("All", "All"));
                        ddlUser.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlUser.Items.Clear();
                        ddlUser.Items.Insert(0, new ListItem("All", "All"));
                    }
                }
                else
                {
                    ddlUser.Items.Clear();
                    ddlUser.Items.Insert(0, new ListItem("All", "All"));
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            lblStatus.Text = string.Empty;
            try
            {
                if (ddlStatus.SelectedIndex == 0)
                {
                    lblStatus.Text = "Select Status";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (btnSave.Text == "Save")
                {
                    #region Save

                    #region Parameters

                    object [] ParamValues=new object[9];
                    SqlDbType[] ParamType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.VarChar };
                    string[] ParamName = { "RoleId", "User", "Module", "Page", "Action", "Status", "IsAct", "CreatedOn", "CreatedBy" };

                    int i=0;
                    //0
                    ParamValues.SetValue(ddlRole.SelectedValue, i);
                    i++;

                    //1
                    ParamValues.SetValue(ddlUser.SelectedItem.Text, i);
                    i++;

                    //2
                    ParamValues.SetValue(ddlModule.SelectedItem.Text, i);
                    i++;

                    //3
                    string PageName="";
                    if(ddlPage.SelectedIndex>0)
                        PageName=ddlPage.SelectedValue;
                    ParamValues.SetValue(PageName, i);
                    i++;

                    //4
                    ParamValues.SetValue(txtAction.Text, i);
                    i++;

                    //5
                    ParamValues.SetValue(ddlStatus.SelectedItem.Text, i);
                    i++;

                    //6
                    ParamValues.SetValue(chkActive.Checked, i);
                    i++;

                    //7
                    string CreatedOn=DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy");
                    ParamValues.SetValue(CreatedOn, i);
                    i++;

                    //8
                    ParamValues.SetValue(Session["UserName"].ToString(), i);

                    #endregion

                    bool res=da.ExecuteProcedure("sp_SaveUserAccessException", ParamName, ParamType, ParamValues);

                    if (res)
                    {
                        ddlRole.SelectedIndex = 0;
                        ddlUser.SelectedIndex = 0;
                        ddlModule.SelectedIndex = 0;
                        ddlPage.SelectedIndex = 0;
                        txtAction.Text = string.Empty;
                        ddlStatus.SelectedIndex = 0;
                        chkActive.Checked = false;

                        lblStatus.Text = "Record added successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Record could not be added";
                        lblStatus.ForeColor = Color.Green;
                    }
                    #endregion
                }
                    
                else
                {
                    #region Update

                    #region Parameters

                    object[] ParamValues = new object[10];
                    SqlDbType[] ParamType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.BigInt };
                    string[] ParamName = { "RoleId", "User", "Module", "Page", "Action", "Status", "IsAct", "CreatedOn", "CreatedBy", "SrNo" };

                    int i = 0;
                    //0
                    ParamValues.SetValue(ddlRole.SelectedValue, i);
                    i++;

                    //1
                    ParamValues.SetValue(ddlUser.SelectedItem.Text, i);
                    i++;

                    //2
                    ParamValues.SetValue(ddlModule.SelectedItem.Text, i);
                    i++;

                    //3
                    ParamValues.SetValue(ddlPage.SelectedValue, i);
                    i++;

                    //4
                    ParamValues.SetValue(txtAction.Text, i);
                    i++;

                    //5
                    ParamValues.SetValue(ddlStatus.SelectedItem.Text, i);
                    i++;

                    //6
                    ParamValues.SetValue(chkActive.Checked, i);
                    i++;

                    //7
                    string CreatedOn = DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy");
                    ParamValues.SetValue(CreatedOn, i);
                    i++;

                    //8
                    ParamValues.SetValue(Session["UserName"].ToString(), i);
                    i++;

                    //9
                    ParamValues.SetValue(int.Parse(ViewState["srnum"].ToString()), i);

                    #endregion

                    bool res = da.ExecuteProcedure("sp_SaveUserAccessException", ParamName, ParamType, ParamValues);

                    if (res)
                    {
                        btnList_Click(null,null);
                        ddlRole.SelectedIndex = 0;
                        ddlUser.SelectedIndex = 0;
                        ddlModule.SelectedIndex = 0;
                        ddlPage.SelectedIndex = 0;
                        txtAction.Text = string.Empty;
                        ddlStatus.SelectedIndex = 0;
                        chkActive.Checked = false;
                        btnSave.Text = "Save";

                        lblStatus.Text = "Record updated successfully";
                        lblStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Record could not be updated";
                        lblStatus.ForeColor = Color.Green;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            { }
            finally { }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet();
            lblStatus.Text = string.Empty;
            try
            {
                #region Parameter

                object[] ParamValues = new object[6];
                SqlDbType[] ParamType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                string[] ParamName = { "RoleId", "User", "Module", "Page", "Action", "Status" };

                int i = 0;

                //0
                string RoleId = "";
                if (ddlRole.SelectedIndex > 0)
                    RoleId = ddlRole.SelectedValue;
                ParamValues.SetValue(ddlRole.SelectedValue, i);
                i++;

                //1
                string User = "";
                if (ddlUser.SelectedIndex > 0)
                    User = ddlUser.SelectedItem.Text;
                ParamValues.SetValue(User, i);
                i++;

                //2
                string Module="";
                if(ddlModule.SelectedIndex>0)
                    Module=ddlModule.SelectedItem.Text;
                ParamValues.SetValue(Module, i);
                i++;

                //3
                string PageName = "";
                if (ddlPage.SelectedIndex > 0)
                    PageName = ddlPage.SelectedValue;
                ParamValues.SetValue(PageName, i);
                i++;

                //4
                ParamValues.SetValue(txtAction.Text, i);
                i++;

                //5
                string Status="";
                if(ddlStatus.SelectedIndex>0)
                    Status=ddlStatus.SelectedItem.Text;
                ParamValues.SetValue(Status, i);
                i++;

                #endregion

                ds = da.SelectRecords("ssp_GetUserAccessException", ParamName, ParamValues, ParamType);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddlRole.SelectedIndex = 0;
                    ddlUser.SelectedIndex = 0;
                    ddlModule.SelectedIndex = 0;
                    ddlPage.SelectedIndex = 0;
                    txtAction.Text = string.Empty;
                    ddlStatus.SelectedIndex = 0;
                    chkActive.Checked = false;

                    grdUserException.DataSource = ds.Tables[0];
                    grdUserException.DataBind();

                    ViewState["dsUserException"] = ds;
                }
                else
                {
                    grdUserException.DataSource = null;
                    grdUserException.DataBind();

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
            ddlRole.SelectedIndex = 0;
            ddlUser.SelectedIndex = 0;
            ddlModule.SelectedIndex = 0;
            ddlPage.SelectedIndex = 0;
            txtAction.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
            chkActive.Checked = false;

            btnSave.Text = "Save";

            lblStatus.Text = string.Empty;

            grdUserException.DataSource = null;
            grdUserException.DataBind();
        }

        protected void grdUserException_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    ViewState["srnum"] = Convert.ToInt32(((Label)grdUserException.Rows[rowIndex].FindControl("lblSrNo")).Text);
                    
                    string Role = ((Label)grdUserException.Rows[rowIndex].FindControl("lblRole")).Text;
                    ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByText(Role));
                    ddlRole_SelectedIndexChanged(null, null);

                    string User = ((Label)grdUserException.Rows[rowIndex].FindControl("lblUser")).Text; ;
                    ddlUser.SelectedIndex = ddlUser.Items.IndexOf(ddlRole.Items.FindByText(User));

                    string Module = ((Label)grdUserException.Rows[rowIndex].FindControl("lblModule")).Text;
                    ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByText(Module));
                    ddlModule_SelectedIndexChanged(null, null);

                    string Page = ((Label)grdUserException.Rows[rowIndex].FindControl("lblPage")).Text;
                    ddlPage.SelectedIndex = ddlPage.Items.IndexOf(ddlPage.Items.FindByText(Page));

                    txtAction.Text = ((Label)grdUserException.Rows[rowIndex].FindControl("lblAction")).Text;

                    string Status = ((Label)grdUserException.Rows[rowIndex].FindControl("lblStatus")).Text;
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(Status));

                    string Active = ((Label)grdUserException.Rows[rowIndex].FindControl("lblActive")).Text;
                    if (Active.ToUpper() == "ACTIVE")
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;

                    btnSave.Text = "Update";
                }
            }
            catch (Exception ex)
            { }
        }

        protected void grdUserException_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grdUserException_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (DataSet)ViewState["dsUserException"];
                grdUserException.PageIndex = e.NewPageIndex;
                grdUserException.DataSource = ds.Tables[0];
                grdUserException.DataBind();
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }

        }

    }
}
