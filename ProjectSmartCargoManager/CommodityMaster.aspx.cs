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
    public partial class CommodityMaster : System.Web.UI.Page
    {
        #region Variable
        BALCommodityMaster objBAL = new BALCommodityMaster();
        LoginBL lBal = new LoginBL();
        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetCommodityCategory();
                    GetSHCCode();
                    chkActive.Checked = true;

                    bool scroll = Convert.ToBoolean(lBal.GetMasterConfiguration("KnownShipper"));
                    if (scroll == false)
                    {
                        chkShipper.Visible = false;
                        lblShipper.Visible = false;
                    }
                    else
                    {
                        chkShipper.Visible = true;
                        lblShipper.Visible = true;
                    }

                    #region Define PageSize for grid as per configuration
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        grvCommodityList.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion

                }
            }
            catch (Exception ex)
            { }
        }

        #region Get Commodity Category
        private void GetCommodityCategory()
        {
            DataSet ds = objBAL.GetCommodityCategory();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlCommCategory.DataSource = ds;
                    ddlCommCategory.DataMember = ds.Tables[0].TableName;
                    ddlCommCategory.DataValueField = ds.Tables[0].Columns["SerialNumber"].ColumnName;
                    ddlCommCategory.DataTextField = ds.Tables[0].Columns["CommCategory"].ColumnName;
                    ddlCommCategory.DataBind();
                    ddlCommCategory.Items.Insert(0, new ListItem("Select", "Select"));
                }
            }
        }
        #endregion
        #region GetSHCCode
        private void GetSHCCode()
        {
            DataSet ds = objBAL.GetSHCCode();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlshccode.DataSource = ds.Tables[0];
                    ddlshccode.DataMember = ds.Tables[0].TableName;
                    ddlshccode.DataValueField = ds.Tables[0].Columns["SpecialHandelingCode"].ColumnName;
                    ddlshccode.DataTextField = ds.Tables[0].Columns["SpecialHandelingCode"].ColumnName;
                    ddlshccode.DataBind();
                    ddlshccode.Items.Insert(0, new ListItem("Select", "Select"));
                }
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
                        #region Prepare Parameters
                        object[] CommodityInfo = new object[9];
                        int i = 0;

                        //0
                        CommodityInfo.SetValue(txtCommodityCode.Text.Trim(), i);
                        i++;

                        //1
                        CommodityInfo.SetValue(txtCommodityName.Text.Trim(), i);
                        i++;

                        //2
                        CommodityInfo.SetValue(txtCommodityDesc.Text.Trim(), i);
                        i++;

                        //3
                        if (chkActive.Checked == true)
                        {
                            CommodityInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            CommodityInfo.SetValue("False", i);
                            i++;
                        }

                        //4
                        int CategorySrNo;
                        if(ddlCommCategory.SelectedIndex==0)
                            CategorySrNo = 0;
                        else
                        CategorySrNo=int.Parse(ddlCommCategory.SelectedValue.ToString());
                        CommodityInfo.SetValue(CategorySrNo, i);
                        i++;
                        //newly added field
                        CommodityInfo.SetValue(chkShipper.Checked, i);
                        i++;
                        try
                        {
                            if (ddlisnotoc.SelectedItem.Value == "Select")
                            { CommodityInfo.SetValue("", i); }

                            if (ddlisnotoc.SelectedItem.Value == "DGR")
                            { CommodityInfo.SetValue(ddlisnotoc.SelectedItem.Value.ToString(), i); }


                            if (ddlisnotoc.SelectedItem.Value == "Special Cargo")
                            { CommodityInfo.SetValue(ddlisnotoc.SelectedItem.Value.ToString(), i); }
                           // CommodityInfo.SetValue(chkdgr.Checked, i);
                        }
                        catch (Exception ex)
                        { }
                        i++;
                        if (txtPriority.Text == "")
                            CommodityInfo.SetValue(0, i);
                        else
                            CommodityInfo.SetValue(txtPriority.Text.Trim(),i);
                        i++;
                        if (ddlshccode.SelectedItem.Text == "select")
                            CommodityInfo.SetValue("", i);
                        else
                            CommodityInfo.SetValue(ddlshccode.SelectedItem.Value.ToString(), i);
                        //
                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBAL.AddCommodity(CommodityInfo);
                        if (ID >= 0)
                        {
                            string value = "SAVE";
                            MasterLog(value);

                            btnList_Click(null, null);
                            txtCommodityCode.Text = string.Empty;
                            txtCommodityName.Text = string.Empty;
                            txtCommodityDesc.Text = string.Empty;
                            txtCommodityCode.Enabled = true;
                            txtPriority.Text = string.Empty;
                            ddlshccode.SelectedIndex = 0;
                            ddlCommCategory.SelectedIndex = 0;
                           
                             btnSave.Text = "Save";
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Commodity Added Sucessfully..";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Commodity Insertion Failed..";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error while saving record.";
                    }
                }

                # endregion Save

                # region Update
                if (btnSave.Text == "Update")
                {
                    try
                    {

                        #region Prepare Parameters
                        object[] updateCommodityInfo = new object[10];
                        int i = 0;
                         string shccode="";

                        //0
                        updateCommodityInfo.SetValue(txtCommodityCode.Text.Trim(), i);
                        i++;

                        //1
                        updateCommodityInfo.SetValue(txtCommodityName.Text.Trim(), i);
                        i++;

                        //2
                        updateCommodityInfo.SetValue(txtCommodityDesc.Text.Trim(), i);
                        i++;

                        //3
                        if (chkActive.Checked == true)
                        {
                            updateCommodityInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateCommodityInfo.SetValue("False", i);
                            i++;
                        }

                       //4
                        int srnumber = (int)Session["srnum"];
                        updateCommodityInfo.SetValue(srnumber, i);
                        i++;

                        //5
                        int CategorySrNo;
                        if (ddlCommCategory.SelectedIndex == 0)
                            CategorySrNo = 0;
                        else
                        CategorySrNo = int.Parse(ddlCommCategory.SelectedValue.ToString());
                        updateCommodityInfo.SetValue(CategorySrNo, i);
                        i++;
//shipper
                        if (chkShipper.Checked == true)
                        {
                            updateCommodityInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateCommodityInfo.SetValue("False", i);
                            i++;
                        }
                        //isnotoc
                        if (ddlisnotoc.SelectedIndex == 0)
                        {
                            string isnotoc = "";
                            updateCommodityInfo.SetValue(isnotoc.ToString(), i);
                        }
                        else
                            updateCommodityInfo.SetValue(ddlisnotoc.SelectedValue.ToString(), i);
                        //newly added field
                        i++;
                        if (txtPriority.Text.Trim() == "")
                            updateCommodityInfo.SetValue(0, i);
                        else
                        updateCommodityInfo.SetValue(txtPriority.Text.Trim(), i);

                        i++;
                        if (ddlshccode.SelectedIndex == 0)
                        { 
                            shccode ="";
                            updateCommodityInfo.SetValue(shccode.ToString(), i);
                        }
                        else
                        updateCommodityInfo.SetValue(ddlshccode.SelectedValue.ToString(),i);

                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateCommodity(updateCommodityInfo);
                        if (UpdateID >= 0)
                        {
                            string value = "UPDATE";
                            MasterLog(value);

                            btnList_Click(sender, e);
                            txtCommodityCode.Text = string.Empty;
                            txtCommodityName.Text = string.Empty;
                            txtCommodityDesc.Text = string.Empty;
                            txtCommodityCode.Enabled = true;
                            ddlCommCategory.SelectedIndex = 0;
                            ddlshccode.SelectedIndex = 0;
                            txtPriority.Text = string.Empty;
                            btnSave.Text = "Save";

                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Commodity Updated Sucessfully..";

                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Commodity Updation Failed..";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error while saving record.";
                    }

                }
                # endregion Update
            }

            catch (Exception ex)
            {

            }

        }
        # endregion btnSave_Click

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            txtCommodityCode.Text = string.Empty;
            txtCommodityName.Text = string.Empty;
            txtCommodityDesc.Text = string.Empty;
            txtCommodityCode.Enabled = true;
            txtPriority.Text = string.Empty;
            ddlshccode.SelectedIndex = 0;
            ddlCommCategory.SelectedIndex = 0;

            btnSave.Text = "Save";
            
            grvCommodityList.DataSource = null;
            grvCommodityList.DataBind();
        }
        #endregion btnClear_Click

        public void MasterLog(string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;

            //1

            Paramsmaster.SetValue("Commodity Master", count);
            count++;

            //2
            if (txtCommodityName.Text == "")
            {
                txtCommodityName.Text = grvCommodityList.Columns[1].ToString();
            }
            Paramsmaster.SetValue(txtCommodityName.Text, count);
            count++;

            //3

            Paramsmaster.SetValue(value, count);
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

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                #region Prepare Parameters
                object[] CommodityListInfo = new object[8];
                int i = 0;

                 //0
                 CommodityListInfo.SetValue(txtCommodityCode.Text.Trim(), i);
                 i++;

                 //1
                 int CatSrNo = 0;
                 if (ddlCommCategory.SelectedIndex > 0)
                     CatSrNo = int.Parse(ddlCommCategory.SelectedValue);
                 CommodityListInfo.SetValue(CatSrNo, i);
                 i++;

                 //2 CommodityName
                 CommodityListInfo.SetValue(txtCommodityName.Text.Trim(), i);
                 i++;

                 //3 IsNoTOC
                 string isnotoc = "";
                 if (ddlisnotoc.SelectedIndex > 0)
                 {
                     isnotoc = ddlisnotoc.SelectedValue.ToString();
                     isnotoc = isnotoc.Replace(" ", "");
                     CommodityListInfo.SetValue(isnotoc, i);
                     i++;
                 }
                 else
                    
                 {
                     isnotoc = "";
                     CommodityListInfo.SetValue(isnotoc, i);
                 i++;
                 }

                 //4 SHCCode
                 string shc = "";
                 if (ddlshccode.SelectedIndex > 0)
                 {
                     shc = ddlshccode.SelectedValue.ToString();
                     CommodityListInfo.SetValue(shc, i);
                     i++;
                 }
                 else
                 {
                     shc = "";
                     CommodityListInfo.SetValue(shc, i);
                     i++;
                 }
                 //5 IsActive
                 CommodityListInfo.SetValue(chkActive.Checked, i);
                 i++;
                //6 proprity
                 CommodityListInfo.SetValue(txtPriority.Text.Trim(), i);

                 i++;





                //7
                 CommodityListInfo.SetValue(txtCommodityDesc.Text.Trim(), i);
                 i++;

                
                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetCommodityList(CommodityListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvCommodityList.PageIndex = 0;
                                grvCommodityList.DataSource = ds;
                                grvCommodityList.DataMember = ds.Tables[0].TableName;
                                grvCommodityList.DataBind();
                                grvCommodityList.Visible = true;
                                Session["ds"] = ds;
                                //btnClear_Click(null,null);
                                //ds.Clear();

                                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                                {
                                    if (((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text.ToString() == "True")
                                    {
                                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text = "Active";
                                    }
                                    //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                                    else if (((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text.ToString() == "False")
                                    {
                                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text = "InActive";
                                    }
                                }

                            }
                            else
                            {
                                lblStatus.Text = "No records found";
                                lblStatus.ForeColor = Color.Red;
                                grvCommodityList.DataSource = null;
                                grvCommodityList.DataBind();
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

        # region grvCommodityList_RowCommand
        protected void grvCommodityList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            try
            {

                #region EDIT
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = string.Empty;
                    txtCommodityCode.Enabled = false;
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblCommCode = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblCommodityCode");
                    Label lblCommName = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblCommodityName");
                    Label lblCommDesc = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblCommDesc");
                    Label lblstat = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblStatus");
                    string commcategory = ((Label)grvCommodityList.Rows[RowIndex].FindControl("lblCommCategory")).Text.Trim();
                    Session["srnum"] = int.Parse(((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());
                    Label lblShipper = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblShipper");
                    Label lblPriority = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblPriority");

                    txtCommodityCode.Text = lblCommCode.Text;
                    txtCommodityName.Text = lblCommName.Text;
                    txtCommodityDesc.Text = lblCommDesc.Text;
                    txtPriority.Text = lblPriority.Text;

                    if (commcategory.Trim() == "")
                        ddlCommCategory.SelectedIndex = 0;
                    else
                    {
                        DataSet ds = da.SelectRecords("SPGetCommcategory", "commCode", commcategory, SqlDbType.VarChar);
                        string value = "EDIT";
                        MasterLog(value);
                        string srno = ds.Tables[0].Rows[0][0].ToString();
                        ddlCommCategory.SelectedIndex = ddlCommCategory.Items.IndexOf(((ListItem)ddlCommCategory.Items.FindByValue(srno)));
                    }
                    try
                    {
                        Label code=((Label)grvCommodityList.Rows[RowIndex].FindControl("lblSHCcode"));
                        if (code.Text == "0"||code.Text=="")
                            ddlshccode.SelectedIndex = 0;
                        else
                         ddlshccode.SelectedValue = ((Label)grvCommodityList.Rows[RowIndex].FindControl("lblSHCcode")).Text;
                    }
                    catch (Exception ex)
                    { }
                    if (lblstat.Text == "Active")
                    {
                        chkActive.Checked = true;
                    }

                    if (lblstat.Text == "InActive")
                    {
                        chkActive.Checked = false;
                    }

                    if (lblShipper.Text == "Active")
                    {
                       chkShipper.Checked = true;
                    }

                    if (lblShipper.Text == "InActive")
                    {
                        chkShipper.Checked = false;
                    }

                    btnSave.Text = "Update";

                }
                #endregion EDIT

                //Unit Testing Done
                #region Delete

                if (e.CommandName == "DeleteRecord")
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    int srno = int.Parse(((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());
                    # region Delete
                    try
                    {
                        #region Prepare Parameters
                        DataSet ds = new DataSet();
                        object[] Params = new object[1];
                        int i = 0;

                        //1
                        Params.SetValue(srno, i);
                        i++;

                        #endregion Prepare Parameters

                        int ID = 0;
                        int res = objBAL.DeleteCommodityDetail(Params);
                        if (res == 0)
                        {
                            string value = "DELETE";
                            MasterLog(value);
                            btnClear_Click(null, null);
                            btnList_Click(null, null);
                            lblStatus.Text = "Record Deleted Successfully";
                            lblStatus.ForeColor = Color.Red;

                        }

                    }

                    catch (Exception ex)
                    {

                    }
                }
                    # endregion Delete

                #endregion Delete
            }
            catch (Exception ex)
            {

            }
            finally
            {
                da = null;
            }

        }
        # endregion grvCommodityList_RowCommand

        # region grvCommodityList_RowEditing
        protected void grvCommodityList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvCommodityList_RowEditing

        # region grvCommodityList_PageIndexChanging
        protected void grvCommodityList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                lblStatus.Text = "";

                DataSet dsnew = (DataSet)Session["ds"];
                grvCommodityList.PageIndex = e.NewPageIndex;
                grvCommodityList.DataSource = dsnew; //ds.Copy();
                grvCommodityList.DataBind();


                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                {
                    if (((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text.ToString() == "True")
                    {
                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text = "Active";
                    }
                    //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                    else if (((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text.ToString() == "False")
                    {
                        ((Label)(grvCommodityList.Rows[j].FindControl("lblStatus"))).Text = "InActive";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion grvCommodityList_PageIndexChanging
    }
}
