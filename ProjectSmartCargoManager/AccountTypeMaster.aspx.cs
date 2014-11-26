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
    public partial class AccountTypeMaster : System.Web.UI.Page
    {
        #region Variables
        BALAccountTypeMaster objBAL = new BALAccountTypeMaster();
 
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                try
                {
                    Session["dsAccountTypeMaster"] = null;
                    chkActive.Checked = true;
                  AddList();
                  lblStatus.Visible = true;
                    btnSave.Enabled = true;
                    btnDel.Enabled = true;
                    btnAdd.Enabled = true;
                }
                catch (Exception)
                {
                }
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
                object[] AccountTypeListInfo = new object[3];
                int i = 0;

                //0
                AccountTypeListInfo.SetValue(txtAccountTypeName.Text, i);
                i++;

                AccountTypeListInfo.SetValue(txtAccountTypeID.Text, i);
                i++;

                AccountTypeListInfo.SetValue(chkActive.Checked, i);
                i++;
                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.ListAccountType(AccountTypeListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvAccountTypeMaster.PageIndex = 0;
                                grvAccountTypeMaster.DataSource = ds;
                                grvAccountTypeMaster.DataMember = ds.Tables[0].TableName;
                                grvAccountTypeMaster.DataBind();
                                grvAccountTypeMaster.Visible = true;
                                Session["dsAccountTypeMaster"] = ds.Tables[0];
                                
                               txtAccountTypeName.Text = string.Empty;
                              txtAccountTypeID.Text = string.Empty;
                                chkActive.Checked = true;

                                //ds.Clear();

                                for (int j = 0; j < grvAccountTypeMaster.Rows.Count; j++)
                                {
                                    ((TextBox)(grvAccountTypeMaster.Rows[j].FindControl("grdtxtUpdatedBy"))).Enabled = false;
                                    ((TextBox)(grvAccountTypeMaster.Rows[j].FindControl("grdtxtUpdatedOn"))).Enabled = false;
                                
                                }

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Visible = true;
                                lblStatus.Text = "No records found...";
                                grvAccountTypeMaster.PageIndex = 0;
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
            txtAccountTypeName.Text = string.Empty;
            txtAccountTypeID.Text = string.Empty;
            chkActive.Checked = true;
            btnSave.Text = "Save";
            btnDel.Enabled = false;
            lblStatus.Text = string.Empty;
            grvAccountTypeMaster.DataSource = null;
            grvAccountTypeMaster.DataBind();
        }
        #endregion btnClear_Click

        #region Button Save Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool CheckForSelect = false;
            lblStatus.Text = string.Empty;
            int count = 0;
            for (int i = 0; i < grvAccountTypeMaster.Rows.Count; i++)
            { 
                CheckBox chkBox = (CheckBox)grvAccountTypeMaster.Rows[i].FindControl("chkRow");
                if (chkBox.Checked)
                {
                    CheckForSelect = true;
                    count++;
                    TextBox txtAccountTypeName = (TextBox)grvAccountTypeMaster.Rows[i].FindControl("grdtxtAccountTypeName");
                    TextBox txtAccountTypeID = (TextBox)grvAccountTypeMaster.Rows[i].FindControl("grdtxtAccountTypeID");
                    TextBox txtAccountTypeDescription = (TextBox)grvAccountTypeMaster.Rows[i].FindControl("grdtxtAccountTypeDescription");
                    TextBox txtCategoryID = (TextBox)grvAccountTypeMaster.Rows[i].FindControl("grdtxtCategoryID");
                    TextBox txtUpdatedBy = (TextBox)grvAccountTypeMaster.Rows[i].FindControl("grdtxtUpdatedBy");
                    TextBox txtUpdatedOn  = (TextBox)grvAccountTypeMaster.Rows[i].FindControl("grdtxtUpdatedOn");
                    CheckBox chkIsActive = (CheckBox)grvAccountTypeMaster.Rows[i].FindControl("chkActive");
                    txtUpdatedBy.Text=Session["UserName"].ToString();
                    txtUpdatedOn.Text=Session["IT"].ToString();
                    bool chkIsSystem=false;
                    
                    
                    if (!ValidateCategoryName(txtAccountTypeID.Text))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                        lblStatus.Text = "Account Type ID can not be blank !!";
                        return;
                    }
                    #region Prepare Parameters
                    object[] AccountTypeListInfo= new object[8];
                    int j = 0;

                    AccountTypeListInfo.SetValue(txtAccountTypeName.Text, j);
                    j++;

                    AccountTypeListInfo.SetValue(txtAccountTypeDescription.Text, j);
                    j++;

                    AccountTypeListInfo.SetValue(txtAccountTypeID.Text, j);
                    j++;
                    AccountTypeListInfo.SetValue(txtUpdatedBy.Text, j);
                    j++;


                    AccountTypeListInfo.SetValue(txtUpdatedOn.Text, j);
                    j++;

                    AccountTypeListInfo.SetValue(chkIsActive.Checked, j);
                    j++; 
                    AccountTypeListInfo.SetValue(txtCategoryID.Text, j);
                    j++;
                    AccountTypeListInfo.SetValue(chkIsSystem, j);
                    j++;
                    #endregion Prepare Parameters

                    DataSet dsCategory = objBAL.InsertAccountType(AccountTypeListInfo);
                    string value = "SAVE";
                    MasterLog(value);

                    if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                    {
                        if (dsCategory.Tables[0].Rows[0][0].ToString() == "INSERTED")
                        {
                           txtAccountTypeName.Text = string.Empty;
                         txtAccountTypeID.Text = string.Empty;
                            chkActive.Checked = true;
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Account Type Inserted Successfully !!";
                            
                        }
                        else if (dsCategory.Tables[0].Rows[0][0].ToString() == "UPDATED")
                        {
                            txtAccountTypeName.Text = string.Empty;
                          txtAccountTypeID.Text = string.Empty;
                            chkActive.Checked = true;
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Account Type Updated Successfully !!";
                            
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

        # region grvAccountTypeMaster_RowCommand
        protected void grvAccountTypeMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //#region EDIT
              //  if (e.CommandName == "Edit")
              //  {
                //    lblStatus.Text = "";
                //    int RowIndex = Convert.ToInt32(e.CommandArgument);
                //    Label lblCatID = (Label)grvAccountTypeMaster.Rows[RowIndex].FindControl("lblCategoryID");
                //    Label lblCatName = (Label)grvAccountTypeMaster.Rows[RowIndex].FindControl("lblCategoryName");
                //    Label lblCatDesc = (Label)grvAccountTypeMaster.Rows[RowIndex].FindControl("lblCategoryDesc");
                //    //Session["srnum"] = int.Parse(((Label)(grvAccountTypeMaster.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());

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
                ////    int srno = int.Parse(((Label)(grvAccountTypeMaster.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());
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
        # endregion grvAccountTypeMaster_RowCommand

        # region grvAccountTypeMaster_RowEditing
        protected void grvAccountTypeMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvAccountTypeMaster_RowEditing

        # region grvAccountTypeMaster_PageIndexChanging
        protected void grvAccountTypeMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                lblStatus.Text = "";

                DataSet dsnew = (DataSet)Session["ds"];
                grvAccountTypeMaster.PageIndex = e.NewPageIndex;
                grvAccountTypeMaster.DataSource = dsnew; //ds.Copy();
                grvAccountTypeMaster.DataBind();


                for (int j = 0; j < grvAccountTypeMaster.Rows.Count; j++)
                {
                    if (((Label)(grvAccountTypeMaster.Rows[j].FindControl("lblStatus"))).Text.ToString() == "True")
                    {
                        ((Label)(grvAccountTypeMaster.Rows[j].FindControl("lblStatus"))).Text = "Active";
                    }
                    //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                    else if (((Label)(grvAccountTypeMaster.Rows[j].FindControl("lblStatus"))).Text.ToString() == "False")
                    {
                        ((Label)(grvAccountTypeMaster.Rows[j].FindControl("lblStatus"))).Text = "InActive";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion grvAccountTypeMaster_PageIndexChanging

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
                for (int i = 0; i < grvAccountTypeMaster.Rows.Count; i++)
                {

                    if (((CheckBox)grvAccountTypeMaster.Rows[i].FindControl("chkRow")).Checked == true)
                    {
                        chkdel = true;
                        string txtAccountTypeID = ((TextBox)(grvAccountTypeMaster.Rows[i].FindControl("grdtxtAccountTypeID"))).Text;
                        
                        #region Prepare Parameters
                        object[] AccountTypeListInfo = new object[1];
                        int j = 0;
                        //0
                        AccountTypeListInfo.SetValue(txtAccountTypeID, j);
                        j++;
                        #endregion Prepare Parameters

                        //DataSet dsDel = objBAL.DeleteCategory(AccountTypeListInfo);

                        dsDel = objBAL.DeleteAccountType(AccountTypeListInfo);

                        if (dsDel != null && dsDel.Tables.Count > 0)
                        {
                            if (dsDel.Tables[0].Rows.Count > 0)
                            {
                                if ((dsDel.Tables[0].Rows[0][0].ToString()) == "DELETED")
                                {
                                    string value = "DELETE";
                                    MasterLog(value);
                                    btnList_Click(null, null);
                                    lblStatus.Visible = true;
                                    lblStatus.Text = "Record Deleted Successfully...";
                                    lblStatus.ForeColor = Color.Red;
                                    
                                }
                                else if ((dsDel.Tables[0].Rows[0][0].ToString()) == "SYSTEM")
                                {
                                    btnList_Click(null, null);
                                    lblStatus.Visible = true;
                                    lblStatus.Text = "System Data can not be deleted...";
                                    lblStatus.ForeColor = Color.Red;
                                  
                                }
                                else
                                {
                                    lblStatus.Visible = true;
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
        private void AddList()
        {
            DataTable dtNewList = new DataTable();
            if (Session["dsAccountTypeMaster"] == null)
            {
                dtNewList = null;
            }
            else
            {
                dtNewList = (DataTable)Session["dsAccountTypeMaster"];
            }
            if (dtNewList == null)
            {
                dtNewList = new DataTable();
                dtNewList.Columns.Add("TypeID");
                dtNewList.Columns.Add("TypeName");
                dtNewList.Columns.Add("TypeDescription");
                dtNewList.Columns.Add("CategoryID");
                dtNewList.Columns.Add("UpdatedBy");
                dtNewList.Columns.Add("UpdatedOn");
                dtNewList.Columns.Add("IsActive", typeof(bool));


            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
                DataRow l_Datarow = dtNewList.NewRow();
                l_Datarow["TypeID"] = "";
                l_Datarow["TypeName"] = "";
                l_Datarow["TypeDescription"] = "";
                l_Datarow["CategoryID"] = "";
                l_Datarow["UpdatedBy"] = "";
                l_Datarow["UpdatedOn"] ="";
                l_Datarow["IsActive"] = 1;
                dtNewList.Rows.Add(l_Datarow);

                grvAccountTypeMaster.DataSource = dtNewList;
                grvAccountTypeMaster.DataBind();
                Session["dsAccountTypeMaster"] = dtNewList;


                for (int j = 0; j < grvAccountTypeMaster.Rows.Count; j++)
                {
                    ((TextBox)(grvAccountTypeMaster.Rows[j].FindControl("grdtxtUpdatedOn"))).Enabled = false;
                    ((TextBox)(grvAccountTypeMaster.Rows[j].FindControl("grdtxtUpdatedBy"))).Enabled = false;

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


        #region Add Click
        private void ADD()
        {
            DataTable dtNewList = new DataTable();
            if (Session["dsAccountTypeMaster"] == null)
            {
                dtNewList = null;
            }
            else
            {
                dtNewList = (DataTable)Session["dsAccountTypeMaster"];
            }
            if (dtNewList == null)
            {
                dtNewList = new DataTable();
                dtNewList.Columns.Add("TypeID");
                dtNewList.Columns.Add("TypeName");
                dtNewList.Columns.Add("TypeDescription");
                dtNewList.Columns.Add("CategoryID");
                dtNewList.Columns.Add("UpdatedBy");
                dtNewList.Columns.Add("UpdatedOn");
                dtNewList.Columns.Add("IsActive", typeof(bool));


            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
               
                   DataRow l_Datarow = dtNewList.NewRow();
                   l_Datarow["TypeID"] = "";
                   l_Datarow["TypeName"] = "";
                   l_Datarow["TypeDescription"] = "";
                   l_Datarow["CategoryID"] = "";
                   l_Datarow["UpdatedBy"] = Session["UserName"].ToString();
                    l_Datarow["UpdatedOn"] = Session["IT"].ToString();
                    l_Datarow["IsActive"] = 1;
                    dtNewList.Rows.Add(l_Datarow);

                    grvAccountTypeMaster.DataSource = dtNewList;
                    grvAccountTypeMaster.DataBind();
                    Session["dsAccountTypeMaster"] = dtNewList;
              
                for (int j = 0; j < grvAccountTypeMaster.Rows.Count; j++)
                {
                    ((TextBox)(grvAccountTypeMaster.Rows[j].FindControl("grdtxtUpdatedOn"))).Enabled = false;
                    ((TextBox)(grvAccountTypeMaster.Rows[j].FindControl("grdtxtUpdatedBy"))).Enabled = false;

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

            Paramsmaster.SetValue("Cost Center Master", count);
            count++;
             
            //2
            string data=null;
            
                for (int j = 0; j < grvAccountTypeMaster.Rows.Count; j++)
                {
                    if (((CheckBox)(grvAccountTypeMaster.Rows[j].FindControl("chkRow"))).Checked==true )
                    
                        data =(((TextBox)(grvAccountTypeMaster.Rows[j].FindControl("grdtxtAccountTypeName"))).Text.ToString());
                    
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

        protected void chkActive_CheckedChanged(object sender, EventArgs e)
        {

        }

    }

  
}

