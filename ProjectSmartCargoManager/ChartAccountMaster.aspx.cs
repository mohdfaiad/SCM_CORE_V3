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
using System.IO;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class ChartAccountMaster : System.Web.UI.Page
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        MasterAuditBAL ObjMAL = new MasterAuditBAL();
        string srno = "";
        protected void Page_Load(object sender, EventArgs e)
        {   if (!IsPostBack)
            {
                LoadDropDowns();
            }
        }

        protected void LoadDropDowns()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = db.SelectRecords("Sp_GetSCNAcctField");

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {

                        try
                        {
                            ddlSCMAcctField.DataSource = ds;
                            ddlSCMAcctField.DataMember = ds.Tables[0].TableName;
                            ddlSCMAcctField.DataTextField = ds.Tables[0].Columns["ChargeHeadCode"].ColumnName;
                            ddlSCMAcctField.DataValueField = ds.Tables[0].Columns["ChargeHeadCode"].ColumnName;
                            ddlSCMAcctField.DataBind();
                            ddlSCMAcctField.Items.Insert(0, "Select");
                        }
                        catch (Exception)
                        {

                        }

                        try
                        {
                            ddlGLAccount.DataSource = ds;
                            ddlGLAccount.DataMember = ds.Tables[1].TableName;
                            ddlGLAccount.DataTextField = ds.Tables[1].Columns["GLAccountCode"].ColumnName;
                            ddlGLAccount.DataValueField = ds.Tables[1].Columns["GLAccountCode"].ColumnName;
                            ddlGLAccount.DataBind();
                            ddlGLAccount.Items.Insert(0, "Select");
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            ddlCostCenter.DataSource = ds;
                            ddlCostCenter.DataMember = ds.Tables[2].TableName;
                            ddlCostCenter.DataTextField = ds.Tables[2].Columns["CostCenterID"].ColumnName;
                            ddlCostCenter.DataValueField = ds.Tables[2].Columns["CostCenterID"].ColumnName;
                            ddlCostCenter.DataBind();
                            ddlCostCenter.Items.Insert(0, "Select");
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            ddlParentAcc.DataSource = ds;
                            ddlParentAcc.DataMember = ds.Tables[3].TableName;
                            ddlParentAcc.DataTextField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            ddlParentAcc.DataValueField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            ddlParentAcc.DataBind();
                            ddlParentAcc.Items.Insert(0, "Select");
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            //ddlbalancing.DataSource = ds;
                            //ddlbalancing.DataMember = ds.Tables[3].TableName;
                            //ddlbalancing.DataTextField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //ddlbalancing.DataValueField = ds.Tables[3].Columns["AccountID"].ColumnName;
                            //ddlbalancing.DataBind();
                            //ddlbalancing.Items.Insert(0, "Select");
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            ddlAccCat.DataSource = ds;
                            ddlAccCat.DataMember = ds.Tables[4].TableName;
                            ddlAccCat.DataTextField = ds.Tables[4].Columns["CategoryID"].ColumnName;
                            ddlAccCat.DataValueField = ds.Tables[4].Columns["CategoryID"].ColumnName;
                            ddlAccCat.DataBind();
                            ddlAccCat.Items.Insert(0, "Select");
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            ddlAccType.DataSource = ds;
                            ddlAccType.DataMember = ds.Tables[5].TableName;
                            ddlAccType.DataTextField = ds.Tables[5].Columns["TypeID"].ColumnName;
                            ddlAccType.DataValueField = ds.Tables[5].Columns["TypeID"].ColumnName;
                            ddlAccType.DataBind();
                            ddlAccType.Items.Insert(0, "Select");
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            ddlDbAccId.DataSource = ds;
                            ddlDbAccId.DataMember = ds.Tables[7].TableName;
                            ddlDbAccId.DataTextField = ds.Tables[7].Columns["Account"].ColumnName;
                            ddlDbAccId.DataValueField = ds.Tables[7].Columns["Account"].ColumnName;
                            ddlDbAccId.DataBind();
                            ddlDbAccId.Items.Insert(0, "Select");

                            ddlCrAccId.DataSource = ds;
                            ddlCrAccId.DataMember = ds.Tables[7].TableName;
                            ddlCrAccId.DataTextField = ds.Tables[7].Columns["Account"].ColumnName;
                            ddlCrAccId.DataValueField = ds.Tables[7].Columns["Account"].ColumnName;
                            ddlCrAccId.DataBind();
                            ddlCrAccId.Items.Insert(0, "Select");
                        }
                        catch (Exception ex)
                        { }

                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtAccDesc.Text = string.Empty ;
            txtAccName.Text = string.Empty;
            txtAccountID.Text = string.Empty;
            ddlParentAcc.SelectedIndex = 0;
            ddlCostCenter.SelectedIndex = 0;
            ddlGLAccount.SelectedIndex = 0;
            ddlSCMAcctField.SelectedIndex = 0;
            ddlAccCat.SelectedIndex = 0;
            ddlAccType.SelectedIndex = 0;
            ddlDbAccId.SelectedIndex = 0;
            ddlCrAccId.SelectedIndex = 0;
            ddlRefEntity.SelectedIndex = 0;
            chkactive.Checked = false;
            chkSystem.Checked = false; 
            btnSave.Text = "Save";
            Session["SrNo"] = null;
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters

                string[] paramname = new string[18];
                paramname[0] = "AccountID";
                paramname[1] = "AccountName";
                paramname[2] = "AccountDesc";
                paramname[3] = "ParentAccount";
                paramname[4] = "CostCenter";
                paramname[5] = "GLAccount";
                paramname[6] = "SCMAcctField";
                paramname[7] = "AccountCat";
                paramname[8] = "IsActive";
                paramname[9] = "IsSystem";
                paramname[10] = "AccountType";
                paramname[11] = "DbAccId";
                paramname[12] = "CrAccId";
                paramname[13] = "RefEntity";
                paramname[14] = "Flag";
                paramname[15] = "UpdatedBy";
                paramname[16] = "UpdatedOn";
                paramname[17] = "AccountserialNo";

                object[] paramvalue = new object[18];

                paramvalue[0] = txtAccountID.Text.Trim();
                paramvalue[1] = txtAccName.Text.Trim();
                paramvalue[2] = txtAccDesc.Text.Trim();
                //parent Account
                if (ddlParentAcc.SelectedIndex == 0)
                {
                    paramvalue[3] = "";
                }
                else
                {
                    paramvalue[3] = ddlParentAcc.SelectedItem.Text.Trim();
                }
                //Cost Center
                if (ddlCostCenter.SelectedIndex == 0)
                {
                    paramvalue[4] = "";
                }
                else
                {
                    paramvalue[4] = ddlCostCenter.SelectedItem.Text.Trim();
                }
                //GL Acc
                if (ddlGLAccount.SelectedIndex == 0)
                {
                    paramvalue[5] = "";
                }
                else
                {
                    paramvalue[5] = ddlGLAccount.SelectedItem.Text.Trim();
                }
                //SCM Acc
                if (ddlSCMAcctField.SelectedIndex == 0)
                {
                    paramvalue[6] = "";
                }
                else
                {
                    paramvalue[6] = ddlSCMAcctField.SelectedItem.Text.Trim();
                }
                //Acc Category
                if (ddlAccCat.SelectedIndex == 0)
                {
                    paramvalue[7] = "";
                }
                else
                {
                    paramvalue[7] = ddlAccCat.SelectedItem.Text.Trim();
                }
                paramvalue[8] = chkactive.Checked;
                paramvalue[9] = chkSystem.Checked;
                //Acc Type
                if (ddlAccType.SelectedItem.Text == "Select")
                {
                    paramvalue[10] = "";
                }
                else
                {
                    paramvalue[10] = ddlAccType.SelectedItem.Text.Trim();
                }
                //Db Acc Id
                if (ddlDbAccId.SelectedIndex == 0)
                {
                    paramvalue[11] = "";
                }
                else
                {
                    paramvalue[11] = ddlDbAccId.SelectedItem.Text.Trim();
                }
                //Cr Acc Id
                if (ddlCrAccId.SelectedIndex == 0)
                {
                    paramvalue[12] = "";
                }
                else
                {
                    paramvalue[12] = ddlCrAccId.SelectedItem.Text.Trim();
                }
                //Ref Entity
                if (ddlRefEntity.SelectedIndex == 0)
                {
                    paramvalue[13] = "";
                }
                else
                {
                    paramvalue[13] = ddlRefEntity.SelectedItem.Text.Trim();
                }
                //Flag
                paramvalue[14] = "List";
                paramvalue[15] = Session["UserName"].ToString();
                paramvalue[16] =Session["IT"];
                if (Session["SrNo"] == null)
                { paramvalue[17] = ""; }
                else
                { paramvalue[17] = Session["SrNo"].ToString(); }

                SqlDbType[] paramtype = new SqlDbType[18];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.Bit;
                paramtype[9] = SqlDbType.Bit;
                paramtype[10] = SqlDbType.VarChar;
                paramtype[11] = SqlDbType.VarChar;
                paramtype[12] = SqlDbType.VarChar;
                paramtype[13] = SqlDbType.VarChar;
                paramtype[14] = SqlDbType.VarChar;
                paramtype[15] = SqlDbType.VarChar;
                paramtype[16] = SqlDbType.DateTime;
                paramtype[17] = SqlDbType.VarChar;


                #endregion Prepare Parameters

                #region OLD Prepare Parameters

                //string[] paramname = new string[12];
                //paramname[0] = "AccountName";
                //paramname[1] = "AccountDesc";
                //paramname[2] = "ParentAccount";
                //paramname[3] = "BalancingAccount";
                //paramname[4] = "CostCenter";
                //paramname[5] = "GLAccount";
                //paramname[6] = "SCMAcctField";
                //paramname[7] = "IsActive";
                //paramname[8] = "AccountID";
                //paramname[9] = "Flag";
                //paramname[10] = "AccountCat";
                //paramname[11] = "AccountType";

                //object[] paramvalue = new object[12];
                //paramvalue[0] = txtAccName.Text.Trim();

                //paramvalue[1] = txtAccDesc.Text.Trim();
                //if (ddlParentAcc.SelectedItem.Text == "Select")
                //{
                //    paramvalue[2] = "";
                //}
                //else
                //{
                //    paramvalue[2] = ddlParentAcc.SelectedItem.Text.Trim();
                //}
                ////if (ddlbalancing.SelectedItem.Text == "Select")
                ////{
                ////    paramvalue[3] = "";
                ////}
                ////else
                ////{
                ////    paramvalue[3] = ddlbalancing.SelectedItem.Text.Trim();
                ////}
                //if (ddlCostCenter.SelectedItem.Text == "Select")
                //{
                //    paramvalue[4] = "";
                //}
                //else
                //{
                //    paramvalue[4] = ddlCostCenter.SelectedItem.Text.Trim();
                //}
                //if (ddlGLAccount.SelectedItem.Text == "Select")
                //{
                //    paramvalue[5] = "";
                //}
                //else
                //{
                //    paramvalue[5] = ddlGLAccount.SelectedItem.Text.Trim();
                //}
                //if (ddlSCMAcctField.SelectedItem.Text == "Select")
                //{
                //    paramvalue[6] = "";
                //}
                //else
                //{
                //    paramvalue[6] = ddlSCMAcctField.SelectedItem.Text.Trim();
                //}
                //paramvalue[7] = chkactive.Checked;
                //paramvalue[8] = txtAccountID.Text.Trim();
                //paramvalue[9] = "List";
                //if (ddlAccCat.SelectedItem.Text == "Select")
                //{
                //    paramvalue[10] = "";
                //}
                //else
                //{
                //    paramvalue[10] = ddlAccCat.SelectedItem.Text.Trim();
                //}
                //if (ddlAccType.SelectedItem.Text == "Select")
                //{
                //    paramvalue[11] = "";
                //}
                //else
                //{
                //    paramvalue[11] = ddlAccType.SelectedItem.Text.Trim();
                //}

                //SqlDbType[] paramtype = new SqlDbType[12];
                //paramtype[0] = SqlDbType.VarChar;
                //paramtype[1] = SqlDbType.VarChar;
                //paramtype[2] = SqlDbType.VarChar;
                //paramtype[3] = SqlDbType.VarChar;
                //paramtype[4] = SqlDbType.VarChar;
                //paramtype[5] = SqlDbType.VarChar;
                //paramtype[6] = SqlDbType.VarChar;
                //paramtype[7] = SqlDbType.Bit;
                //paramtype[8] = SqlDbType.VarChar;
                //paramtype[9] = SqlDbType.VarChar;
                //paramtype[10] = SqlDbType.VarChar;
                //paramtype[11] = SqlDbType.VarChar;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("SP_ListAccount", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdChartAccount.PageIndex = 0;
                                grdChartAccount.DataSource = ds;
                                grdChartAccount.DataMember = ds.Tables[0].TableName;
                                grdChartAccount.DataBind();
                                grdChartAccount.Visible = true;
                                Session["ChartAccountds"] = ds;
                                for (int j = 0; j < grdChartAccount.Rows.Count; j++)
                                {
                                    if (((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text.ToString().ToUpper() == "TRUE")
                                    {
                                        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text = "Active";
                                    }
                                    else
                                    {
                                        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text = "InActive";
                                    }
                                    if (((Label)(grdChartAccount.Rows[j].FindControl("lblIsSys"))).Text.ToString().ToUpper() == "TRUE")
                                    {
                                        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsSys"))).Text = "Active";
                                    }
                                    else
                                    {
                                        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsSys"))).Text = "InActive";
                                    }
                                }
                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {

                                grdChartAccount.DataSource = null;
                                grdChartAccount.DataBind();
                                btnExport.Visible = false;
                                lblStatus.Text = "Record does not exist";
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

        protected void grdChartAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataSet dstemp = (DataSet)Session["ChartAccountds"];
                grdChartAccount.PageIndex = e.NewPageIndex;
                grdChartAccount.DataSource = dstemp;
                grdChartAccount.DataMember = dstemp.Tables[0].TableName;
                grdChartAccount.DataBind();
                for (int j = 0; j < grdChartAccount.Rows.Count; j++)
                {
                    if (((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text.ToString() == "True")
                    {
                        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text = "Active";
                    }
                    else if (((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text.ToString() == "False")
                    {
                        ((Label)(grdChartAccount.Rows[j].FindControl("lblIsActive"))).Text = "InActive";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAccountID.Text == "" && txtAccName.Text == "")
                {
                    lblStatus.Text = "Please Fill AccountID and AccountName.";
                    return;
                }
                #region Prepare Parameters

                string[] paramname = new string[18];
                paramname[0] = "AccountID";
                paramname[1] = "AccountName";
                paramname[2] = "AccountDesc";
                paramname[3] = "ParentAccount";
                paramname[4] = "CostCenter";
                paramname[5] = "GLAccount";
                paramname[6] = "SCMAcctField";
                paramname[7] = "AccountCat";
                paramname[8] = "IsActive";
                paramname[9] = "IsSystem";
                paramname[10] = "AccountType";
                paramname[11] = "DbAccId";
                paramname[12] = "CrAccId";
                paramname[13] = "RefEntity";
                paramname[14] = "Flag";
                paramname[15] = "UpdatedBy";
                paramname[16] = "UpdatedOn";
                paramname[17] = "AccountserialNo";

                object[] paramvalue = new object[18];

                paramvalue[0] = txtAccountID.Text.Trim();
                paramvalue[1] = txtAccName.Text.Trim();
                paramvalue[2] = txtAccDesc.Text.Trim();
                //parent Account
                if (ddlParentAcc.SelectedIndex==0)
                {
                    paramvalue[3] = "";
                }
                else
                {
                    paramvalue[3] = ddlParentAcc.SelectedItem.Text.Trim();
                }
                //Cost Center
                if (ddlCostCenter.SelectedIndex==0)
                {
                    paramvalue[4] = "";
                }
                else
                {
                    paramvalue[4] = ddlCostCenter.SelectedItem.Text.Trim();
                }
                //GL Acc
                if (ddlGLAccount.SelectedIndex==0)
                {
                    paramvalue[5] = "";
                }
                else
                {
                    paramvalue[5] = ddlGLAccount.SelectedItem.Text.Trim();
                }
                //SCM Acc
                if (ddlSCMAcctField.SelectedIndex==0)
                {
                    paramvalue[6] = "";
                }
                else
                {
                    paramvalue[6] = ddlSCMAcctField.SelectedItem.Text.Trim();
                }
                //Acc Category
                if (ddlAccCat.SelectedIndex==0)
                {
                    paramvalue[7] = "";
                }
                else
                {
                    paramvalue[7] = ddlAccCat.SelectedItem.Text.Trim();
                }
                paramvalue[8] = chkactive.Checked;
                paramvalue[9] = chkSystem.Checked;
                //Acc Type
                if (ddlAccType.SelectedItem.Text == "Select")
                {
                    paramvalue[10] = "";
                }
                else
                {
                    paramvalue[10] = ddlAccType.SelectedItem.Text.Trim();
                }
                //Db Acc Id
                if (ddlDbAccId.SelectedIndex==0)
                {
                    paramvalue[11] = "";
                }
                else
                {
                    paramvalue[11] = ddlDbAccId.SelectedItem.Text.Trim();
                }
                //Cr Acc Id
                if (ddlCrAccId.SelectedIndex == 0)
                {
                    paramvalue[12] = "";
                }
                else
                {
                    paramvalue[12] = ddlCrAccId.SelectedItem.Text.Trim();
                }
                //Ref Entity
                if (ddlRefEntity.SelectedIndex == 0)
                {
                    paramvalue[13] = "";
                }
                else
                {
                    paramvalue[13] = ddlRefEntity.SelectedItem.Text.Trim();
                }
                //Flag
                if (btnSave.Text == "Save")
                { paramvalue[14] = "Insert"; }
                else
                { paramvalue[14] = "Update"; }
                paramvalue[15] = Session["UserName"].ToString();
                paramvalue[16] = Convert.ToDateTime(Session["IT"].ToString());
                if (Session["SrNo"] == null)
                { paramvalue[17] = ""; }
                else
                { paramvalue[17] = Session["SrNo"].ToString(); }

                SqlDbType[] paramtype = new SqlDbType[18];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.Bit;
                paramtype[9] = SqlDbType.Bit;
                paramtype[10] = SqlDbType.VarChar;
                paramtype[11] = SqlDbType.VarChar;
                paramtype[12] = SqlDbType.VarChar;
                paramtype[13] = SqlDbType.VarChar;
                paramtype[14] = SqlDbType.VarChar;
                paramtype[15] = SqlDbType.VarChar;
                paramtype[16] = SqlDbType.DateTime;
                paramtype[17] = SqlDbType.VarChar;


                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = db.SelectRecords("SP_ListAccount", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                btnList_Click(null, null);
                                //btnClear_Click(null, null);
                                lblStatus.Text = ds.Tables[0].Rows[0][0].ToString();
                                lblStatus.ForeColor = System.Drawing.Color.Green;

                                #region for Master Audit Log
                                #region Prepare Parameters
                                object[] Paramsmaster = new object[7];
                                int count = 0;

                                //1

                                Paramsmaster.SetValue("Chat Account Master", count);
                                count++;

                                //2
                                Paramsmaster.SetValue(txtAccountID.Text, count);
                                count++;

                                //3

                                Paramsmaster.SetValue("Save", count);
                                count++;

                                //4

                                Paramsmaster.SetValue("", count);
                                count++;


                                //5

                                Paramsmaster.SetValue("", count);
                                count++;

                                //6

                                Paramsmaster.SetValue(Session["UserName"], count);
                                count++;

                                //7
                                Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                                count++;


                                #endregion Prepare Parameters
                                ObjMAL.AddMasterAuditLog(Paramsmaster);
                                #endregion
                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {

                                grdChartAccount.DataSource = null;
                                grdChartAccount.DataBind();
                                grdChartAccount.Visible = false;
                                btnExport.Visible = false;
                                lblStatus.Text = "Record does not exist";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void grdChartAccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region Edit
            try
            {
                if (e.CommandName == "Edit")
                {
                    btnSave.Text = "Update";
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    srno = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblAccountIdd")).Text;
                    Session["SrNo"] = srno;
                    txtAccountID.Text = ((Label)(grdChartAccount.Rows[RowIndex].FindControl("lblAccount"))).Text.ToString();
                    txtAccName.Text = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblAccountName")).Text;
                    txtAccDesc.Text = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblAccountDescription")).Text;
                    
                    string ParntAcc = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblParentAccount")).Text;
                    ddlParentAcc.SelectedIndex = ddlParentAcc.Items.IndexOf(ddlParentAcc.Items.FindByText(ParntAcc));

                    string cost = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblCostCenter")).Text;
                    ddlCostCenter.SelectedIndex = ddlCostCenter.Items.IndexOf(ddlCostCenter.Items.FindByText(cost));

                    string glacc = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblGLAccount")).Text;
                    ddlGLAccount.SelectedIndex = ddlGLAccount.Items.IndexOf(ddlGLAccount.Items.FindByText(glacc));

                    string scmacc = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblSCMAcctField")).Text;
                    ddlSCMAcctField.SelectedIndex = ddlSCMAcctField.Items.IndexOf(ddlSCMAcctField.Items.FindByText(scmacc));

                    string accCat = ((Label)grdChartAccount.Rows[RowIndex].FindControl("lblAccCat")).Text;
                    ddlAccCat.SelectedIndex = ddlAccCat.Items.IndexOf(ddlAccCat.Items.FindByText(accCat));

                    string accType=((Label)grdChartAccount.Rows[RowIndex].FindControl("lblAccType")).Text;
                    ddlAccType.SelectedIndex = ddlAccType.Items.IndexOf(ddlAccType.Items.FindByText(accType));

                    if (((Label)grdChartAccount.Rows[RowIndex].FindControl("lblIsActive")).Text.ToUpper() == "ACTIVE")
                        chkactive.Checked = true;
                    else
                        chkactive.Checked = false;
                    if (((Label)grdChartAccount.Rows[RowIndex].FindControl("lblIsSys")).Text.ToUpper() == "ACTIVE")
                        chkSystem.Checked = true;
                    else
                        chkSystem.Checked = false;

                    string dacc = ((Label)(grdChartAccount.Rows[RowIndex].FindControl("lblDbAccountID"))).Text.ToString();
                    ddlDbAccId.SelectedIndex = ddlDbAccId.Items.IndexOf(ddlDbAccId.Items.FindByText(dacc));

                    string crcc = ((Label)(grdChartAccount.Rows[RowIndex].FindControl("lblCrAccountID"))).Text.ToString();
                    ddlCrAccId.SelectedIndex = ddlCrAccId.Items.IndexOf(ddlCrAccId.Items.FindByText(crcc));

                    string refen = ((Label)(grdChartAccount.Rows[RowIndex].FindControl("lblRefEntity"))).Text.ToString();
                    ddlRefEntity.SelectedIndex = ddlRefEntity.Items.IndexOf(ddlRefEntity.Items.FindByText(refen));

                    #region for Master Audit Log
                    #region Prepare Parameters
                    object[] Paramsmaster = new object[7];
                    int count = 0;

                    //1

                    Paramsmaster.SetValue("Chat Account Master", count);
                    count++;

                    //2
                    Paramsmaster.SetValue(txtAccountID.Text, count);
                    count++;

                    //3

                    Paramsmaster.SetValue("Edit", count);
                    count++;

                    //4

                    Paramsmaster.SetValue("", count);
                    count++;


                    //5

                    Paramsmaster.SetValue("", count);
                    count++;

                    //6

                    Paramsmaster.SetValue(Session["UserName"], count);
                    count++;

                    //7
                    Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                    count++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsmaster);
                    #endregion

                }
            }
            catch (Exception ex) { }
            #endregion Edit

            #region Delete
            if (e.CommandName == "DeleteRecord")
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument);

                Label lblid = ((Label)(grdChartAccount.Rows[RowIndex].FindControl("lblAccountIdd")));
                int id = int.Parse(lblid.Text);

                try
                {
                    #region Prepare Parameters

                    string[] paramname = new string[1];
                    paramname[0] = "AccountID";
                    

                    object[] paramvalue = new object[1];
                    paramvalue[0] = id.ToString();

                    

                    SqlDbType[] paramtype = new SqlDbType[1];
                    paramtype[0] = SqlDbType.VarChar;
                    
                    #endregion Prepare Parameters


                    #region for Master Audit Log
                    #region Prepare Parameters
                    object[] Paramsmaster = new object[7];
                    int count = 0;

                    //1

                    Paramsmaster.SetValue("Chat Account Master", count);
                    count++;

                    //2
                    Paramsmaster.SetValue(id.ToString(), count);
                    count++;

                    //3

                    Paramsmaster.SetValue("Delete", count);
                    count++;

                    //4

                    Paramsmaster.SetValue("", count);
                    count++;


                    //5

                    Paramsmaster.SetValue("", count);
                    count++;

                    //6

                    Paramsmaster.SetValue(Session["UserName"], count);
                    count++;

                    //7
                    Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                    count++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsmaster);
                    #endregion

                    int ID = 0;
                    DataSet dsdel= db.SelectRecords("SPDelAccount", paramname, paramvalue, paramtype);
                    //ID = objBAL.DeleteProRate(Params);
                    if (dsdel.Tables[0].Rows[0][0].ToString() == "Deleted")
                    {
                        btnClear_Click(null, null);
                        btnList_Click(null, null);
                        lblStatus.Text = "Record Deleted Successfully";
                        lblStatus.ForeColor = Color.Green;

                    }
                    else
                    {
                        btnClear_Click(null, null);
                        btnList_Click(null, null);
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Record Deletion Failed..";

                    }
                }
                catch (Exception ex)
                {

                }
            }
            #endregion Delete
        }

        protected void grdChartAccount_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

    }
}
