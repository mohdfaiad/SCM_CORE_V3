using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using QID.DataAccess;

namespace BAL
{
    public class RoleMasterBAL
    {

        #region Variables
        SQLServer objDataAccess = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        public bool FillMenuTreeView(TreeView TRV)
        {
            DataSet dsMenuMaster = null;
            DataSet dsSubMenuMaster = null;
            DataSet dsPageMaster = null;
            DataView dv1 = new DataView();
            DataView dv2 = new DataView();
            DataTable dtSubMenuMaster = null;
            DataTable dtPageMaster = null;
            try
            {
                MakeTreeEmpty(TRV);

                dsMenuMaster = objDataAccess.SelectRecords("SpGetMenuMaster");
                dsSubMenuMaster = objDataAccess.SelectRecords("SpGetSubMenuMaster");
                dsPageMaster = objDataAccess.SelectRecords("SpGetPageMaster");

                if (dsMenuMaster == null || dsSubMenuMaster == null || dsPageMaster == null)
                    return false;

                TreeNode newMenuNode = new TreeNode();
                TreeNode newSubMenuNode = new TreeNode();
                TreeNode newPageNode = new TreeNode();

                dtSubMenuMaster = new DataTable();
                dtPageMaster = new DataTable();

                foreach (DataRow row in dsMenuMaster.Tables[0].Rows)
                {

                    int MenuID = int.Parse(row["MenuId"].ToString());
                    newMenuNode = new TreeNode(row["MenuName"].ToString(), "MM-" + row["MenuId"].ToString());
                    newMenuNode.ShowCheckBox = true;

                    dv1 = new DataView(dsSubMenuMaster.Tables[0]);
                    dv1.RowFilter = "MenuId=" + MenuID;
                    dtSubMenuMaster = dv1.ToTable();

                    foreach (DataRow row1 in dtSubMenuMaster.Rows)
                    {
                        int SubMenuID = int.Parse(row1["SubMenuID"].ToString());
                        newSubMenuNode = new TreeNode(row1["SubMenuName"].ToString(), "SM-" + row1["SubMenuID"].ToString());
                        newSubMenuNode.ShowCheckBox = true;

                        dv2 = new DataView(dsPageMaster.Tables[0]);
                        dv2.RowFilter = "SubMenuID=" + SubMenuID + "and MenuID=" + MenuID;
                        dtPageMaster = dv2.ToTable();

                        foreach (DataRow row2 in dtPageMaster.Rows)
                        {
                            int PageID = int.Parse(row2["PageID"].ToString());
                            newPageNode = new TreeNode(row2["PageName"].ToString(), "PM-" + row2["PageID"].ToString());
                            newPageNode.ShowCheckBox = true;
                            newSubMenuNode.ChildNodes.Add(newPageNode);
                        }
                        newMenuNode.ChildNodes.Add(newSubMenuNode);
                    }
                    TRV.Nodes.Add(newMenuNode);
                }
                newMenuNode = null;
                newSubMenuNode = null;
                newPageNode = null;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (dsMenuMaster != null)
                    dsMenuMaster.Dispose();
                if (dsSubMenuMaster != null)
                    dsSubMenuMaster.Dispose();
                if (dsPageMaster != null)
                    dsPageMaster.Dispose();
                if(dv1 != null)
                    dv1.Dispose();
                if(dv2 != null)
                    dv2.Dispose();
                if (dtSubMenuMaster != null)
                    dtSubMenuMaster.Dispose();
                if (dtPageMaster != null)
                    dtPageMaster.Dispose();
            }
            return true;
        }

        public void MakeTreeEmpty(TreeView trv)
        {
            try
            {
                while (trv.Nodes.Count != 0)
                    trv.Nodes.RemoveAt(trv.Nodes.Count - 1);
            }
            catch (Exception)
            {
            }
        }

        public bool IsRoleExist(string RoleName)
        {
            DataSet ds = null;
            try
            {
                ds = objDataAccess.SelectRecords("SpRoleMasterValidationChkDuplicateRole", "RoleName", RoleName, SqlDbType.VarChar);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count != 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        public bool AddNewRole(string RoleName, int ClientID, ArrayList Pages,string UserName, string RoleType)
        {
            string[] param = new string[4];
            object[] values = new object[4];
            SqlDbType[] dbtypes = new SqlDbType[4]; 
            DataSet ds = null;
            try
            {

                param[0] = "RoleName";
                values[0] = RoleName;
                dbtypes[0] = SqlDbType.VarChar;

                param[1] = "ClientID";
                values[1] = ClientID;
                dbtypes[1] = SqlDbType.Int;

                param[2] = "CreatedBy";
                values[2] = UserName;
                dbtypes[2] = SqlDbType.VarChar;

                param[3] = "RoleType";
                values[3] = RoleType;
                dbtypes[3] = SqlDbType.VarChar;

                ds = objDataAccess.SelectRecords("SpAddRole", param, values, dbtypes);
                if (ds != null && ds.Tables[0].Rows.Count != 0)
                {
                    int id = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    for (int i = 0; i < Pages.Count; i++)
                    {
                        param = new string[2];
                        values = new object[2];
                        dbtypes = new SqlDbType[2];

                        param[0] = "RoleID";
                        values[0] = id;
                        dbtypes[0] = SqlDbType.Int;

                        param[1] = "PageID";
                        values[1] = Pages[i].ToString();
                        dbtypes[1] = SqlDbType.VarChar;

                        objDataAccess.ExecuteProcedure("SpAddRoleRight", param, dbtypes, values);
                        
                        param = null;
                        values = null;
                        dbtypes = null;
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                param = null;
                values = null;
                dbtypes = null;
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        public bool EditRole(int RoleID, int ClientID, ArrayList Pages,string RoleType)
        {
            try
            {
                string[] param = { "RoleID", "RoleType" };
                SqlDbType[] paramType = { SqlDbType.Int, SqlDbType.VarChar };
                object[] paramValues = { RoleID, RoleType };
                bool status = objDataAccess.ExecuteProcedure("SpDeleteRights", param, paramType, paramValues);
                int id = RoleID;
                string[] param1 = null;
                object[] values1 = null;
                SqlDbType[] dbtypes1 = null;
                for (int i = 0; i < Pages.Count; i++)
                {
                    param1 = new string[2];
                    values1 = new object[2];
                    dbtypes1 = new SqlDbType[2];

                    param1[0] = "RoleID";
                    values1[0] = id;
                    dbtypes1[0] = SqlDbType.Int;

                    param1[1] = "PageID";
                    values1[1] = Pages[i].ToString();
                    dbtypes1[1] = SqlDbType.VarChar;

                    status = objDataAccess.ExecuteProcedure("SpAddRoleRight", param1, dbtypes1, values1);

                }
                param1 = null;
                values1 = null;
                dbtypes1 = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteRole(int RoleID)
        {
            try
            {
                if (objDataAccess.ExecuteProcedure("SpDeleteRole", "RoleID", SqlDbType.Int, RoleID))
                { return true; }
                else
                { return false; };
            }
            catch (Exception)
            {
                return false;
            }
            //return true;
        }

        #region GetRoles
        public DataSet GetRoles()
        {
            DataSet ds = null;
            try
            {
                ds = objDataAccess.SelectRecords("SpGetRoles");
                return (ds);
            }
            catch (Exception)
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                return (null);
            }
        }
        #endregion

        #region GetRoleRights
        public DataSet GetRoleRights(int RoleID)
        {
            DataSet ds = null;
            try
            {
                ds = objDataAccess.SelectRecords("SpGetRoleRights", "RoleID", RoleID, SqlDbType.Int);
                return (ds);
            }
            catch (Exception)
            {
                if (ds != null)
                    ds.Dispose();
                return (null);
            }
        }

        #endregion

    }
}
