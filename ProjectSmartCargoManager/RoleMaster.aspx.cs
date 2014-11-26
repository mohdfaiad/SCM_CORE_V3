using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using QID.DataAccess;
using System.Data;
using BAL;
using System.Collections;

namespace ProjectSmartCargoManager
{
    public partial class RoleMaster : System.Web.UI.Page
    {
        #region Variables
        RoleMasterBAL objBl = new RoleMasterBAL();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try 
            {
                if (!IsPostBack)
                {

                    if (Request.QueryString["command"] != null)
                    {
                        string Command = Request.QueryString["command"].ToString();

                        if (Command == "Edit")
                        {
                            if (Request.QueryString["RoleID"] != null)
                            {
                                string RoleID = Request.QueryString["RoleID"].ToString();

                                objBl.FillMenuTreeView(TreeView1);
                                TreeView1.Visible = false;

                                ArrayList checkednodes = GetTreeState();
                                ViewState["CheckedNodes"] = checkednodes;
                                ViewState["state"] = "nostate";
                                checkednodes = null;

                                Button1_Click(btnEdit, e);
                                DDLRoleName.SelectedValue = RoleID;
                                Button1_Click(btnGetRolRights, e);

                            }
                        }

                        else
                        {
                            if (Request.QueryString["RoleID"] != null)
                            {
                                string RoleID = Request.QueryString["RoleID"].ToString();

                                objBl.FillMenuTreeView(TreeView1);
                                TreeView1.Visible = false;

                                ArrayList checkednodes = GetTreeState();
                                ViewState["CheckedNodes"] = checkednodes;
                                ViewState["state"] = "nostate";
                                checkednodes = null;

                                Button1_Click(btnView, e);
                                DDLRoleName.SelectedValue = RoleID;
                                Button1_Click(btnGetRolRights, e);
                            }
                        }
                    }

                    else
                    {


                        objBl.FillMenuTreeView(TreeView1);
                        TreeView1.Visible = false;

                        ArrayList checkednodes = GetTreeState();
                        ViewState["CheckedNodes"] = checkednodes;
                        ViewState["state"] = "nostate";
                        checkednodes = null;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Load Roles
        public void LoadRoles(DropDownList ddlRoles)
        {
            DataSet ds = null;
            try
            {
                ds = objBl.GetRoles();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlRoles.DataSource = ds;
                            ddlRoles.DataTextField = "RoleName";
                            ddlRoles.DataValueField = "RoleID";
                            ddlRoles.DataBind();
                            ddlRoles.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion

        #region Button Click
        protected void Button1_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            Button btn = null;
            try
            {
                btn = (Button)sender;
                if (btn.Text == "New Role")
                {
                    objBl.FillMenuTreeView(TreeView1);
                    TreeView1.Visible = true;
                    TreeView1.Enabled = true;

                    Label3.Visible = true;
                    btnNew.Text = "Add Role";
                    btnNew.CausesValidation = true;
                    btnEdit.Text = "Cancel";
                    btnEdit.Visible = true;
                    btnView.Visible = false;
                    btnDelete.Visible = false;
                    ddlRoleType.Visible = true;
                    lblRoleType.Visible = true;
                    ddlRoleType.Enabled = true;

                    lblRoleName.Visible = true;
                    txtRoleName.Visible = true;
                    Label2.Visible = true;

                    ArrayList checkednodes = GetTreeState();
                    ViewState["CheckedNodes"] = checkednodes;
                    TreeView1.CollapseAll();
                    checkednodes = null;
                }
                else if (btn.Text == "Cancel")
                {
                    btnNew.Visible = btnEdit.Visible = btnView.Visible = btnDelete.Visible = true;
                    btnGetRolRights.Visible = false;
                    lblRoleName.Visible = DDLRoleName.Visible = txtRoleName.Visible = false;
                    btnNew.Text = "New Role";
                    btnEdit.Text = "Edit Role";
                    objBl.FillMenuTreeView(TreeView1);
                    TreeView1.Visible = false;
                    TreeView1.Enabled = true;
                    btnGetRolRights.Text = "Get Role Rights";
                    DDLRoleName.Enabled = true;
                    btnGetRolRights.Enabled = true;
                    txtRoleName.Text = "";
                    Label3.Visible = false;
                    ddlRoleType.Visible = false;
                    lblRoleType.Visible = false;
                }
                else if (btn.Text == "Add Role")
                {
                    if (txtRoleName.Text.Trim() == "")
                    {
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role cannot be blank.');</script>");
                        lblStatus.Text = "Role cannot be blank.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (objBl.IsRoleExist(txtRoleName.Text.ToUpper()))
                    {
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role Already Exists.');</script>");
                        lblStatus.Text = "Role Already Exists.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        string RoleName = txtRoleName.Text.ToUpper();
                        string RoleType = ddlRoleType.SelectedItem.Text;
                        ArrayList pages = GetAllowedPages();
                        if (!objBl.AddNewRole(RoleName, 0, pages, Session["UserName"].ToString(),RoleType))
                        {
                            //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role cannot be added.');</script>");
                            lblStatus.Text = "Role cannot be added.";
                            lblStatus.ForeColor = Color.Red;
                            pages = null;
                            return;
                        }
                        else
                        {
                            #region for Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int i = 0;

                            //1

                            Params.SetValue("RoleMaster", i);
                            i++;

                            //2
                            Params.SetValue(txtRoleName.Text, i);
                            i++;

                            //3

                            Params.SetValue("ADD", i);
                            i++;

                            //4

                            Params.SetValue("", i);
                            i++;


                            //5

                            Params.SetValue("", i);
                            i++;

                            //6

                            Params.SetValue(Session["UserName"], i);
                            i++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                            i++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion
                            //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role added Successfully.');</script>");
                            lblStatus.Text = "Role added Successfully.";
                            lblStatus.ForeColor = Color.Green;
                            btnNew.Visible = btnEdit.Visible = btnView.Visible = btnDelete.Visible = true;
                            btnGetRolRights.Visible = false;
                            lblRoleName.Visible = DDLRoleName.Visible = txtRoleName.Visible = false;
                            ddlRoleType.Visible = false;
                            lblRoleType.Visible = false;

                            btnNew.Text = "New Role";
                            btnEdit.Text = "Edit Role";
                            txtRoleName.Text = "";
                            objBl.FillMenuTreeView(TreeView1);
                            TreeView1.Visible = false;
                            Label3.Visible = false;
                            pages = null;
                            return;
                        }
                    }
                }
                else if (btn.Text == "Edit Role" || btn.Text == "View Role")
                {
                    DataSet ds = objBl.GetRoles();
                    DDLRoleName.Visible = true;
                    txtRoleName.Visible = false;
                    lblRoleName.Visible = true;
                    btnGetRolRights.Visible = true;
                    ddlRoleType.Visible = false;
                    lblRoleType.Visible = false;
                    DDLRoleName.DataSource = ds;
                    DDLRoleName.DataMember = ds.Tables[0].TableName;
                    DDLRoleName.DataTextField = "RoleName";
                    DDLRoleName.DataValueField = "RoleID";
                    DDLRoleName.DataBind();

                    btnNew.Visible = false;
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnEdit.Text = "Cancel";
                    btnView.Visible = false;
                    if (btn.Text == "View Role")
                        ViewState["state"] = "View State";
                    else
                        ViewState["state"] = "";
                    if (ds != null)
                    {
                        ds.Dispose();
                    }
                }
                else if (btn.Text == "Get Role Rights")
                {
                    DataSet ds = objBl.GetRoleRights(int.Parse(DDLRoleName.SelectedValue));
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ddlRoleType.Text = ds.Tables[1].Rows[0]["RoleType"].ToString();
                                ddlRoleType.Visible = true;
                                lblRoleType.Visible = true;
                                ddlRoleType.Enabled = true;
                            }
                        }
                    }
                    ArrayList nodes = new ArrayList();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        nodes.Add(dr[1].ToString());
                    }

                    TreeView1.Visible = true;
                    objBl.FillMenuTreeView(TreeView1);
                    int i = 0, j = 0;
                    foreach (TreeNode trn in TreeView1.Nodes)
                    {
                        i = 0;
                        foreach (TreeNode trn1 in trn.ChildNodes)
                        {
                            j = 0;
                            foreach (TreeNode trn2 in trn1.ChildNodes)
                            {
                                if (nodes.Contains(trn2.Text))
                                {
                                    trn2.Checked = true;
                                    j++;
                                }
                            }
                            //Code Added to select the parent node even if a single child is selected----Start Code
                            if (j > 0)
                            {
                                trn1.Checked = true;
                            }
                            //end Code

                            if (j == trn1.ChildNodes.Count && trn1.ChildNodes.Count != 0)
                            {
                                trn1.Checked = true;
                                i++;
                            }
                        }
                        if (i == trn.ChildNodes.Count && trn.ChildNodes.Count != 0)
                            trn.Checked = true;
                        //Code Added to select the parent node even if a single child is selected----Start Code
                        if (i > 0)
                        {
                            trn.Checked = true;
                        }
                        //end Code

                    }
                    nodes = null;
                    TreeView1.CollapseAll();
                    //Commented Code for New Modifications
                    // TreeView1.ExpandAll();
                    //btn.Text = "Edit Role.";
                    btn.Text = "Save";
                    btnNew.Visible = btnView.Visible = false;
                    btnEdit.Visible = true;
                    btnEdit.Text = "Cancel";
                    DDLRoleName.Enabled = false;
                    btnDelete.Visible = false;

                    if (ViewState["state"].ToString() == "View State")
                    {
                        btn.Text = "Get Role Rights";
                        btn.Enabled = false;
                        TreeView1.Enabled = false;
                        ddlRoleType.Enabled = false;
                        ViewState["state"] = "nostate";
                    }
                }
                //Commented Code for New Modifications
               //else if (btn.Text.Contains("Edit Role"))
                //{
                else if (btn.Text.Contains("Save"))
                {
                    int RoleID = int.Parse(DDLRoleName.SelectedValue);
                    ArrayList pages = GetAllowedPages();
                    if (!objBl.EditRole(RoleID, 0, pages,ddlRoleType.Text))
                    {
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role cannot be edited.');</script>");
                        lblStatus.Text = "Role cannot be edited.";
                        lblStatus.ForeColor = Color.Red;
                        pages = null;
                        return;
                    }
                    else
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int i = 0;

                        //1

                        Params.SetValue("RoleMaster", i);
                        i++;

                        //2
                        Params.SetValue(DDLRoleName.SelectedItem.Text, i);
                        i++;

                        //3

                        Params.SetValue("EDIT", i);
                        i++;

                        //4

                        Params.SetValue("", i);
                        i++;


                        //5

                        Params.SetValue("", i);
                        i++;

                        //6

                        Params.SetValue(Session["UserName"], i);
                        i++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                        i++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role edited Successfully.');</script>");
                        lblStatus.Text = "Role edited Successfully.";
                        lblStatus.ForeColor = Color.Green;
                        btnNew.Visible = btnEdit.Visible = btnView.Visible = btnDelete.Visible = true;
                        ddlRoleType.Visible = false;
                        lblRoleType.Visible = false;
                        btnGetRolRights.Visible = false;
                        lblRoleName.Visible = DDLRoleName.Visible = txtRoleName.Visible = false;
                        objBl.FillMenuTreeView(TreeView1);
                        TreeView1.Visible = false;
                        btnEdit.Text = "Edit Role";
                        btnNew.Text = "New Role";
                        btnGetRolRights.Text = "Get Role Rights";
                        DDLRoleName.Enabled = true;
                        pages = null;
                        return;
                    }
                }
                else if (btn.Text == "Delete Role")
                {
                    DataSet ds = objBl.GetRoles();
                    DDLRoleName.Visible = true;
                    txtRoleName.Visible = false;
                    lblRoleName.Visible = true;
                    btnGetRolRights.Visible = true;
                    ddlRoleType.Visible = false;
                    lblRoleType.Visible = false;

                    DDLRoleName.DataSource = ds;
                    DDLRoleName.DataMember = ds.Tables[0].TableName;
                    DDLRoleName.DataTextField = "RoleName";
                    DDLRoleName.DataValueField = "RoleID";
                    DDLRoleName.DataBind();

                    btnNew.Visible = true;
                    btnNew.Text = "Delete";
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnEdit.Text = "Cancel";
                    btnView.Visible = false;
                    btnGetRolRights.Visible = false;
                    if (ds != null)
                    {
                        ds.Dispose();
                    }
                }
                else if (btn.Text == "Delete")
                {
                    int id = int.Parse(DDLRoleName.SelectedValue);
                    if (!objBl.DeleteRole(id))
                    {
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role cannot be deleted.');</script>");
                        lblStatus.Text = "Role cannot be deleted.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int i = 0;

                        //1

                        Params.SetValue("RoleMaster", i);
                        i++;

                        //2
                        Params.SetValue(DDLRoleName.SelectedItem.Text, i);
                        i++;

                        //3

                        Params.SetValue("DELETE", i);
                        i++;

                        //4

                        Params.SetValue("", i);
                        i++;


                        //5

                        Params.SetValue("", i);
                        i++;

                        //6

                        Params.SetValue(Session["UserName"], i);
                        i++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                        i++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion
                        //ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Role deleted successfully');</script>");
                        lblStatus.Text = "Role deleted successfully.";
                        lblStatus.ForeColor = Color.Green;
                        btnNew.Visible = btnEdit.Visible = btnView.Visible = btnDelete.Visible = true;
                        btnGetRolRights.Visible = false;
                        lblRoleName.Visible = DDLRoleName.Visible = txtRoleName.Visible = false;
                        objBl.FillMenuTreeView(TreeView1);
                        TreeView1.Visible = false;
                        btnEdit.Text = "Edit Role";
                        btnNew.Text = "New Role";
                        btnGetRolRights.Text = "Get Role Rights";
                        DDLRoleName.Enabled = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                if(btn != null)
                    btn.Dispose();
            }
        }
        #endregion

        #region Get Allowed Pages
        protected ArrayList GetAllowedPages()
        {
            ArrayList pages = new ArrayList();
            try
            {
                
                foreach (TreeNode trn in TreeView1.CheckedNodes)
                {
                    if (trn.Value.Contains("PM-"))
                    {
                        if (!pages.Contains(trn.Text))
                        {
                            pages.Add(trn.Text);
                        }
                    }
                    if(trn.Value.Contains("SM-"))
                    {
                        if (!pages.Contains(trn.Text))
                        {
                            pages.Add(trn.Text);
                        }
                    }
                    
                    
                }
                foreach (TreeNode trn in TreeView1.Nodes)
                {
                    int count = 0;

                    foreach (TreeNode Child in trn.ChildNodes)
                    {
                        if (Child.Checked)
                        {
                            count++;
                        }
                    }
                    if (count != 0)
                    { pages.Add(trn.Text); }
                }
                
            }
            catch (Exception)
            {
            }
            return pages;
        }
        #endregion

        #region Get Checked Node
        protected TreeNode GetCheckedNode()
        {
            try
            {
                ArrayList prevchekednodes = (ArrayList)ViewState["CheckedNodes"];

                foreach (TreeNode trn in TreeView1.Nodes)
                {
                    if (trn.Checked && !prevchekednodes.Contains(trn.Value))
                        return trn;
                    if (!trn.Checked && prevchekednodes.Contains(trn.Value))
                        return trn;

                    foreach (TreeNode trn1 in trn.ChildNodes)
                    {
                        if (trn1.Checked && !prevchekednodes.Contains(trn1.Value))
                            return trn1;
                        if (!trn1.Checked && prevchekednodes.Contains(trn1.Value))
                            return trn1;

                        foreach (TreeNode trn2 in trn1.ChildNodes)
                        {
                            if (trn2.Checked && !prevchekednodes.Contains(trn2.Value))
                                return trn2;
                            if (!trn2.Checked && prevchekednodes.Contains(trn2.Value))
                                return trn2;
                        }
                    }
                }
                prevchekednodes = null;
                return null;
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
                return null;
            }
        }
        #endregion

        #region Get Tree State
        protected ArrayList GetTreeState()
        {
            try
            {
                ArrayList checkednodes = new ArrayList();
                foreach (TreeNode trn in TreeView1.CheckedNodes)
                {
                    checkednodes.Add(trn.Value);
                }
                return checkednodes;
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
                return null;
            }
        }
        #endregion

        #region Tree Node Check Event
        protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            try
            {
                TreeNode node = GetCheckedNode();
                if (node.Value.Contains("MM-"))
                {
                    foreach (TreeNode trn1 in node.ChildNodes)
                    {
                        trn1.Checked = node.Checked;

                        foreach (TreeNode trn2 in trn1.ChildNodes)
                        {
                            trn2.Checked = node.Checked;
                        }
                    }
                }
                else if (node.Value.Contains("SM-"))
                {
                    foreach (TreeNode trn2 in node.ChildNodes)
                    {
                        trn2.Checked = node.Checked;
                    }
                    // reverse effect
                    // all true case
                    int i = 0;
                    foreach (TreeNode trn in node.Parent.ChildNodes)
                    {
                        if (trn.Checked == true)
                            i++;
                    }
                    if (i == node.Parent.ChildNodes.Count)
                    {
                        node.Parent.Checked = true;
                    }
                    else
                        node.Parent.Checked = false;

                    // all false case
                    int j = 0;

                    foreach (TreeNode trn in node.Parent.ChildNodes)
                    {
                        if (trn.Checked == false)
                            j++;
                    }
                    if (j == node.Parent.ChildNodes.Count)
                    {
                        node.Parent.Checked = false;
                    }
                }
                else if (node.Value.Contains("PM-"))
                {

                    // reverse effect

                    // all true case
                    int i = 0;

                    foreach (TreeNode trn in node.Parent.ChildNodes)
                    {
                        if (trn.Checked == true)
                            i++;
                    }
                    if (i == node.Parent.ChildNodes.Count)
                    {
                        node.Parent.Checked = true;
                    }
                    else
                        node.Parent.Checked = false;

                    // all false case
                    int j = 0;

                    foreach (TreeNode trn in node.Parent.ChildNodes)
                    {
                        if (trn.Checked == false)
                            j++;
                    }
                    if (j == node.Parent.ChildNodes.Count)
                    {
                        node.Parent.Checked = false;
                    }

                    // all true case
                    i = 0;
                    foreach (TreeNode trn in node.Parent.Parent.ChildNodes)
                    {
                        if (trn.Checked == true)
                            i++;
                    }
                    if (i == node.Parent.Parent.ChildNodes.Count)
                    {
                        node.Parent.Parent.Checked = true;
                    }
                    else
                        node.Parent.Parent.Checked = false;

                    // all false case
                    j = 0;

                    foreach (TreeNode trn in node.Parent.Parent.ChildNodes)
                    {
                        if (trn.Checked == false)
                            j++;
                    }
                    if (j == node.Parent.Parent.ChildNodes.Count)
                    {
                        node.Parent.Parent.Checked = false;
                    }
                }

                ArrayList checkednodes = GetTreeState();
                ViewState["CheckedNodes"] = checkednodes;
                checkednodes = null;
                node = null;
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

    }
}
