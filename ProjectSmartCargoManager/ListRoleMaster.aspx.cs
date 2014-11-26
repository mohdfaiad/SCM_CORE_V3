using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class ListRoleMaster : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblStatus.Text = "";
                    LoadDropdown();
                    LoadGridRoleList();
                }

                
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                grdRoles.DataSource = null;
                grdRoles.DataBind();

                string[] QueryNames = new string[2];
                object[] QueryValues = new object[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];

                QueryNames[0] = "RoleID";
                QueryNames[1] = "IsActive";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;

                if (ddlRole.SelectedIndex != 0)
                {
                    QueryValues[0] = ddlRole.SelectedValue.ToString();
                }
                else
                { QueryValues[0] = ""; }
                QueryValues[1] = chkActive.Checked;


                SQLServer db = new SQLServer(Global.GetConnectionString());

                DataSet ds = db.SelectRecords("sp_GetRolesAsPerSelection", QueryNames, QueryValues, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdRoles.DataSource = ds;
                            grdRoles.DataBind();

                            grdRoles.Columns[4].Visible = true;
                            grdRoles.Columns[5].Visible = true;
                        }
                    }

                }
                db = null;
                ds.Dispose();

                

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                Response.Redirect("~/ListRoleMaster.aspx");
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Load DropDown
        public void LoadDropdown()
        {
            try
            {
                lblStatus.Text = "";

                SQLServer db = new SQLServer(Global.GetConnectionString());

                DataSet ds = db.SelectRecords("sp_GetRolesForDropDown");

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlRole.DataSource = ds;
                            ddlRole.DataValueField = "RoleID";
                            ddlRole.DataTextField = "RoleName";
                            ddlRole.DataBind();
                            ddlRole.Items.Insert(0, "Select");
                            ddlRole.SelectedIndex = 0;
                        }
                    }

                }
                db = null;
                ds.Dispose();
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Add New Row to Grid
        public void LoadGridRoleList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "RoleName";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "CreatedBy";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "CreatedOn";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "IsActive";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "RoleID";
            myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "RoleID";
            //myDataTable.Columns.Add(myDataColumn);



            DataRow dr;
            dr = myDataTable.NewRow();
            dr["RoleName"] = "";
            dr["CreatedBy"] = "";
            dr["CreatedOn"] = "";
            dr["IsActive"] = "";
            dr["RoleID"] = "";
            //dr["RoleID"] = "";


            myDataTable.Rows.Add(dr);

            grdRoles.DataSource = null;
            grdRoles.DataSource = myDataTable;
            grdRoles.DataBind();

            grdRoles.Columns[4].Visible = false;
            grdRoles.Columns[5].Visible = false;
            Session["dtUserInfo"] = myDataTable.Copy();

        }
        #endregion
    }
}
