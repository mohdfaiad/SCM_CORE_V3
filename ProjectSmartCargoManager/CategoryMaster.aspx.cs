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
    public partial class CategoryMaster : System.Web.UI.Page
    {
        #region Variables
        BALCategoryMaster objBAL = new BALCategoryMaster();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                try
                {
                    Session["dsCatMaster"] = null;
                    chkActive.Checked = true;
                    ADD();
                    btnSave.Enabled = true;
                    btnDel.Enabled = true;
                    btnAdd.Enabled = true;
                }
                catch (Exception)
                {
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
        #endregion

        #region List Button Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                #region Prepare Parameters
                object[] CategoryListInfo = new object[3];
                int i = 0;

                //0
                CategoryListInfo.SetValue(txtCategoryName.Text, i);
                i++;

                CategoryListInfo.SetValue(txtCategoryDesc.Text, i);
                i++;

                CategoryListInfo.SetValue(chkActive.Checked, i);
                i++;
                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetComCategoryList(CategoryListInfo);
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
                                Session["dsCatMaster"] = ds.Tables[0];
                                
                                txtCategoryName.Text = string.Empty;
                                txtCategoryDesc.Text = string.Empty;
                                chkActive.Checked = true;

                                //ds.Clear();

                                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                                {
                                    ((TextBox)(grvCommodityList.Rows[j].FindControl("grdtxtCategoryName"))).Enabled = false;
                                }

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Visible = true;
                                lblStatus.Text = "No records found...";
                                grvCommodityList.PageIndex = 0;
                            }
                        }
                    }
                }
                btnSave.Enabled = true;
                btnAdd.Enabled = true;
                btnDel.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCategoryName.Text = string.Empty;
            txtCategoryDesc.Text = string.Empty;
            chkActive.Checked = true;
            btnSave.Text = "Save";
            btnDel.Enabled = false;
            lblStatus.Text = string.Empty;
            grvCommodityList.DataSource = null;
            grvCommodityList.DataBind();
        }
        #endregion btnClear_Click

        #region Button Save Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool CheckForSelect = false;
            lblStatus.Text = string.Empty;
            int count = 0;
            for (int i = 0; i < grvCommodityList.Rows.Count; i++)
            { 
                CheckBox chkBox = (CheckBox)grvCommodityList.Rows[i].FindControl("chkRow");
                if (chkBox.Checked)
                {
                    CheckForSelect = true;
                    count++;
                    TextBox txtCategoryName = (TextBox)grvCommodityList.Rows[i].FindControl("grdtxtCategoryName");
                    TextBox txtCategoryDesc = (TextBox)grvCommodityList.Rows[i].FindControl("grdtxtCategoryDesc");
                    CheckBox chkIsActive = (CheckBox)grvCommodityList.Rows[i].FindControl("chkActive");

                    if (!ValidateCategoryName(txtCategoryName.Text))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                        lblStatus.Text = "Category name can not be blank !!";
                        return;
                    }
                    #region Prepare Parameters
                    object[] CategoryListInfo = new object[3];
                    int j = 0;

                    //if (txtCategoryID.Text == "")
                    //{
                    //    CategoryListInfo.SetValue(0, i);
                    //    i++;
                    //}
                    //else
                    //{
                    //    CategoryListInfo.SetValue(txtCategoryID.Text, i);
                    //    i++;
                    //}

                    CategoryListInfo.SetValue(txtCategoryName.Text, j);
                    j++;

                    CategoryListInfo.SetValue(txtCategoryDesc.Text, j);
                    j++;

                    CategoryListInfo.SetValue(chkIsActive.Checked, j);
                    j++;
                    #endregion Prepare Parameters

                    DataSet dsCategory = objBAL.InsertCategory(CategoryListInfo);
                    string value = "SAVE";
                    MasterLog(value);

                    if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                    {
                        if (dsCategory.Tables[0].Rows[0][0].ToString() == "INSERTED")
                        {
                            txtCategoryName.Text = string.Empty;
                            txtCategoryDesc.Text = string.Empty;
                            chkActive.Checked = true;
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Category Inserted Successfully !!";
                            
                        }
                        else if (dsCategory.Tables[0].Rows[0][0].ToString() == "UPDATED")
                        {
                            txtCategoryName.Text = string.Empty;
                            txtCategoryDesc.Text = string.Empty;
                            chkActive.Checked = true;
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Category Updated Successfully !!";
                            
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                        lblStatus.Text = "Error In Inserting Data !!";
                    }
                }
                
            }
            if (count == 0)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Visible = true;
                lblStatus.Text = "Please select a checkbox !!";
                return;
            }
            btnSave.Enabled = true;
            btnAdd.Enabled = true;
            btnDel.Enabled = true;
        }
        #endregion

        # region grvCommodityList_RowCommand
        protected void grvCommodityList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //#region EDIT
              //  if (e.CommandName == "Edit")
              //  {
                //    lblStatus.Text = "";
                //    int RowIndex = Convert.ToInt32(e.CommandArgument);
                //    Label lblCatID = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblCategoryID");
                //    Label lblCatName = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblCategoryName");
                //    Label lblCatDesc = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblCategoryDesc");
                //    //Session["srnum"] = int.Parse(((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());

                //    //txtCategoryID.Text = lblCatID.Text;
                //    txtCategoryName.Text = lblCatName.Text;
                //    txtCategoryDesc.Text = lblCatDesc.Text;
                //    chkActive.Checked = true;

                //    btnSave.Text = "Update";
               // }
                //#endregion EDIT

                ////Unit Testing Done
                ////#region Delete

                ////if (e.CommandName == "DeleteRecord")
                ////{
                ////    int RowIndex = Convert.ToInt32(e.CommandArgument);
                ////    int srno = int.Parse(((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());
                ////    # region Delete
                ////    try
                ////    {
                ////        #region Prepare Parameters
                ////        DataSet ds = new DataSet();
                ////        object[] Params = new object[1];
                ////        int i = 0;

                ////        //1
                ////        Params.SetValue(srno, i);
                ////        i++;

                ////        #endregion Prepare Parameters

                ////        int ID = 0;
                ////        int res = objBAL.DeleteCommodityDetail(Params);
                ////        if (res == 0)
                ////        {
                ////            btnClear_Click(null, null);
                ////            btnList_Click(null, null);
                ////            lblStatus.Text = "Record Deleted Successfully";
                ////            lblStatus.ForeColor = Color.Red;

                ////        }

                ////    }

                ////    catch (Exception ex)
                ////    {

                ////    }
                ////}
                ////    # endregion Delete

                ////#endregion Delete
            }
            catch (Exception ex)
            {

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

        #region Validation
        private bool ValidateCategoryName(string CategoryName)
        {
            if (CategoryName.Trim() == "")
            {
                return false;
            }
            else
                return true;
        }
        #endregion

        #region Button Add Click
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string value = "ADD";
            MasterLog(value);
            ADD();
            btnSave.Enabled = true;
            //btnAdd.Enabled = false;
            //btnDel.Enabled = false;
        }
        #endregion

        #region Button Del Click
        protected void btnDel_Click(object sender, EventArgs e)
        {
            DataSet dsDel = null;
            try
            {
                bool chkdel = false;
                for (int i = 0; i < grvCommodityList.Rows.Count; i++)
                {

                    if (((CheckBox)grvCommodityList.Rows[i].FindControl("chkRow")).Checked == true)
                    {
                        chkdel = true;
                        string txtCatName = ((TextBox)(grvCommodityList.Rows[i].FindControl("grdtxtCategoryName"))).Text;
                        
                        #region Prepare Parameters
                        object[] CategoryListInfo = new object[1];
                        int j = 0;
                        //0
                        CategoryListInfo.SetValue(txtCatName, j);
                        j++;
                        #endregion Prepare Parameters

                        //DataSet dsDel = objBAL.DeleteCategory(CategoryListInfo);

                        dsDel = objBAL.DeleteCategory(CategoryListInfo);

                        if (dsDel != null && dsDel.Tables.Count > 0)
                        {
                            if (dsDel.Tables[0].Rows.Count > 0)
                            {
                                if ((dsDel.Tables[0].Rows[0][0].ToString()) == "DELETED")
                                {
                                    string value = "DELETE";
                                    MasterLog(value);

                                   
                                    lblStatus.Text = "Record Deleted Successfully...";
                                    lblStatus.ForeColor = Color.Red;
                                    btnList_Click(null, null);
                                }
                                else
                                {
                                    lblStatus.Text = "Record doesn't exist...";
                                    lblStatus.ForeColor = Color.Red;
                                }
                            }
                        }

                    }
                }
                if (chkdel != true)
                {
                    lblStatus.Text = "Please select Record(s).";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Add Click
        private void ADD()
        {
            DataTable dtNewList = new DataTable();
            if (Session["dsCatMaster"] == null)
            {
                dtNewList = null;
            }
            else
            {
                dtNewList = (DataTable)Session["dsCatMaster"];
            }
            if (dtNewList == null)
            {
                dtNewList = new DataTable();
                dtNewList.Columns.Add("CategoryName");
                dtNewList.Columns.Add("CategoryDesc");
                dtNewList.Columns.Add("IsActive", typeof(bool));
            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
                DataRow l_Datarow = dtNewList.NewRow();
                l_Datarow["CategoryName"] = "";
                l_Datarow["CategoryDesc"] = "";
                l_Datarow["IsActive"] = 1;
                dtNewList.Rows.Add(l_Datarow);

                grvCommodityList.DataSource = dtNewList;
                grvCommodityList.DataBind();
                Session["dsCatMaster"] = dtNewList;


                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                {
                    ((TextBox)(grvCommodityList.Rows[j].FindControl("grdtxtCategoryName"))).Enabled = false;

                    if (j == grvCommodityList.Rows.Count - 1)
                    {
                        ((TextBox)(grvCommodityList.Rows[j].FindControl("grdtxtCategoryName"))).Enabled = true;
                    }
                }

            }

            catch (Exception ex) 
            { 
            }
            finally
            {
                if (dtNewList != null)
                    dtNewList.Dispose();
            }
        }
        #endregion

        public void MasterLog( string value)
        {
            MasterAuditBAL ObjMAL = new MasterAuditBAL();
            #region for Master Audit Log
            #region Prepare Parameters
            object[] Paramsmaster = new object[7];
            int count = 0;
          
                //1

            Paramsmaster.SetValue("Category Master", count);
            count++;
             
            //2
            string data=null;
            
                for (int j = 0; j < grvCommodityList.Rows.Count; j++)
                {
                    if (((CheckBox)(grvCommodityList.Rows[j].FindControl("chkRow"))).Checked==true )
                    
                        data =(((TextBox)(grvCommodityList.Rows[j].FindControl("grdtxtCategoryName"))).Text.ToString());
                    
                    Paramsmaster.SetValue(data, count);
                }

            
         
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

    }

  
}

