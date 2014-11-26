using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Drawing;
using QID.DataAccess;


namespace ProjectSmartCargoManager
{
    public partial class UserListing : System.Web.UI.Page
    {
        UserCreationBAL objUserBAL = new UserCreationBAL();
        DataSet DSUserdata;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getAllRoles();
                GetStations();
                LoadGridUserList();
            }
        }

        protected void getAllRoles()
        {
            DataSet ds = null;
            try
            {
                ds = objUserBAL.GetAllRoles();
                if (ds != null)
                {
                    ddlRole.DataSource = ds;
                    ddlRole.DataMember = ds.Tables[0].TableName;
                    ddlRole.DataTextField = "RoleName";
                    ddlRole.DataValueField = "RoleID";
                    ddlRole.DataBind();
                    ddlRole.Items.Insert(0, new ListItem("All", "0"));
                    ddlRole.SelectedIndex = -1;
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

        protected void btnList_Click(object sender, EventArgs e)
        {
            bindUserList();
        }

        protected void bindUserList()
        {
            try
            {
                string ddl;
                if (ddlStations.SelectedIndex == 0)
                {
                    ddl = "";
                }
                else
                { 
                    ddl = ddlStations.SelectedValue; 
                }


                DSUserdata = objUserBAL.GetUserListData(txtUserID.Text.Trim(), Convert.ToInt32(ddlRole.SelectedValue), ddl);

                if (DSUserdata != null && DSUserdata.Tables.Count > 0 && DSUserdata.Tables[0].Rows.Count > 0)
                {
                    Session["dsUser"] = DSUserdata;
                    grdUserList.DataSource = DSUserdata.Tables[0];
                    grdUserList.DataBind();
                    grdUserList.Columns[10].Visible = true;
                    grdUserList.Columns[11].Visible = true;
                    grdUserList.Visible = true;
                }
                else
                {
                    grdUserList.Visible = true;
                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = Color.Blue;
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                if (DSUserdata != null)
                {
                    DSUserdata.Dispose();
                }
            }
        }

        protected void grdUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (Session["dsUser"] != null)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DSUserdata = (DataSet)Session["dsUser"];
                        CheckBox chkSu = (CheckBox)e.Row.FindControl("ChkSU");
                        CheckBox ChkActive = (CheckBox)e.Row.FindControl("ChkActive");
                        CheckBox ChkAllStn = (CheckBox)e.Row.FindControl("ChkAllStn");
                        if (DSUserdata.Tables[0].Rows[e.Row.RowIndex]["SU"].ToString() == "1")
                        {
                            chkSu.Checked = true;
                        }
                        if (DSUserdata.Tables[0].Rows[e.Row.RowIndex]["IsActive"].ToString() == "True")
                        {
                            ChkActive.Checked = true;
                        }
                        if (DSUserdata.Tables[0].Rows[e.Row.RowIndex]["IsAllStn"].ToString() == "True")
                        {
                            ChkAllStn.Checked = true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (DSUserdata != null)
                {
                    DSUserdata.Dispose();
                }
            }
        }

        #region grid row commmand "Edit" and "View"
        protected void grdUserList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit" || e.CommandName == "View")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                //GridViewRow row = grdUserList.Rows[index];
                string LoginID = ((Label)grdUserList.Rows[index].FindControl("lblLoginID")).Text;
                Response.Redirect("UserCreation.aspx?cmd=" + e.CommandName + "&LoginID=" + LoginID);
            }
        }
        #endregion grid row commmand "Edit" and "View"

        protected void grdUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dst = null;
            try
            {
                dst = (DataSet)Session["dsUser"];
                grdUserList.PageIndex = e.NewPageIndex;
                grdUserList.DataSource = dst.Tables[0];
                grdUserList.DataBind();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dst != null)
                {
                    dst.Dispose();
                }
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UserListing.aspx");
        }

        protected void GetStations()
        {
            DataSet ds = null;
           
            try
            {
               SQLServer da = new SQLServer(Global.GetConnectionString());
                
                ds = da.SelectRecords("spGetAirportCodes");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlStations.DataSource = ds;
                    ddlStations.DataMember = ds.Tables[0].TableName;
                    ddlStations.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                    ddlStations.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                    ddlStations.DataBind();
                    ddlStations.Items.Insert(0,new ListItem("Select","0"));
                    ddlStations.SelectedIndex = -1;

                }

            }
            catch (Exception ex)
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

        #region Add New Row to Grid
        public void LoadGridUserList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "LoginID";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "UserName";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "DefStn";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Lang";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "EmailID";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AgentCode";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "SU";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IsActive";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "GSA";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IsAllStn";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["LoginID"] = "";
            dr["UserName"] = "";
            dr["DefStn"] = "";
            dr["Lang"] = "";
            dr["EmailID"] = "";
            dr["AgentCode"] = "";
            dr["SU"] = "";
            dr["IsActive"] = "";
            dr["GSA"] = "";
            dr["IsAllStn"] = "";

            myDataTable.Rows.Add(dr);

            grdUserList.DataSource = null;
            grdUserList.DataSource = myDataTable;
            grdUserList.DataBind();

            grdUserList.Columns[10].Visible = false;
            grdUserList.Columns[11].Visible = false;
            Session["dtUserInfo"] = myDataTable.Copy();

        }
        #endregion

    }
}
