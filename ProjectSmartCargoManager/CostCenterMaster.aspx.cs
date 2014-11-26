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
    public partial class CostCenterMaster : System.Web.UI.Page
    {
        #region Variables
        BALCostCenterMaster objBAL = new BALCostCenterMaster();
 
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                try
                {
                    Session["dsCostCenterMaster"] = null;
                    chkActive.Checked = true;
                  AddList();
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
                object[] CostCenterListInfo = new object[3];
                int i = 0;

                //0
                CostCenterListInfo.SetValue(txtCostCenterName.Text, i);
                i++;

                CostCenterListInfo.SetValue(txtCostCenterDescription.Text, i);
                i++;

                CostCenterListInfo.SetValue(chkActive.Checked, i);
                i++;
                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.ListCostCenter(CostCenterListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grvCostCenterList.PageIndex = 0;
                                grvCostCenterList.DataSource = ds;
                                grvCostCenterList.DataMember = ds.Tables[0].TableName;
                                grvCostCenterList.DataBind();
                                grvCostCenterList.Visible = true;
                                Session["dsCostCenterMaster"] = ds.Tables[0];
                                
                               txtCostCenterName.Text = string.Empty;
                               txtCostCenterDescription.Text = string.Empty;
                                chkActive.Checked = true;

                                //ds.Clear();

                                for (int j = 0; j < grvCostCenterList.Rows.Count; j++)
                                {
                                    ((TextBox)(grvCostCenterList.Rows[j].FindControl("grdtxtUpdatedBy"))).Enabled = false;
                                    ((TextBox)(grvCostCenterList.Rows[j].FindControl("grdtxtUpdatedOn"))).Enabled = false;
                                
                                }

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Visible = true;
                                lblStatus.Text = "No records found...";
                                grvCostCenterList.PageIndex = 0;
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
            //txtCostCenterName.Text = string.Empty;
            //txtCostCenterDescription.Text = string.Empty;
            //chkActive.Checked = true;
            //btnSave.Text = "Save";
            //btnDel.Enabled = false;
            //lblStatus.Text = string.Empty;
            //grvCostCenterList.DataSource = null;
            //grvCostCenterList.DataBind();
            Response.Redirect("~/CostCenterMaster.aspx");
        }
        #endregion btnClear_Click

        #region Button Save Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool CheckForSelect = false;
            lblStatus.Text = string.Empty;
            int count = 0;
            for (int i = 0; i < grvCostCenterList.Rows.Count; i++)
            { 
                CheckBox chkBox = (CheckBox)grvCostCenterList.Rows[i].FindControl("chkRow");
                if (chkBox.Checked)
                {
                    CheckForSelect = true;
                    count++;
                    TextBox txtCostCenterName = (TextBox)grvCostCenterList.Rows[i].FindControl("grdtxtCostCenterName");
                    TextBox txtCostCenterDescription = (TextBox)grvCostCenterList.Rows[i].FindControl("grdtxtCostCenterDescription");
                    TextBox txtCostCenterID = (TextBox)grvCostCenterList.Rows[i].FindControl("grdtxtCostCenterID");
                    TextBox txtUpdatedBy = (TextBox)grvCostCenterList.Rows[i].FindControl("grdtxtUpdatedBy");
                    TextBox txtUpdatedOn  = (TextBox)grvCostCenterList.Rows[i].FindControl("grdtxtUpdatedOn");
                    CheckBox chkIsActive = (CheckBox)grvCostCenterList.Rows[i].FindControl("chkActive");
                    txtUpdatedBy.Text=Session["UserName"].ToString();
                    txtUpdatedOn.Text=Session["IT"].ToString();
   
                    
                    if (!ValidateCategoryName(txtCostCenterName.Text))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                        lblStatus.Text = "Cost Center name can not be blank !!";
                        return;
                    }
                    #region Prepare Parameters
                    object[] CostCenterListInfo= new object[6];
                    int j = 0;


                    CostCenterListInfo.SetValue(txtCostCenterName.Text, j);
                    j++;
                    CostCenterListInfo.SetValue(txtCostCenterDescription.Text, j);
                    j++;
                    CostCenterListInfo.SetValue(txtCostCenterID.Text, j);
                    j++;
                    CostCenterListInfo.SetValue(txtUpdatedBy.Text, j);
                    j++;


                    CostCenterListInfo.SetValue(txtUpdatedOn.Text, j);
                    j++;

                    CostCenterListInfo.SetValue(chkIsActive.Checked, j);
                    j++;
                    #endregion Prepare Parameters

                    DataSet dsCategory = objBAL.InsertCostCenter(CostCenterListInfo);
                    string value = "SAVE";
                    MasterLog(value);

                    if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                    {
                        if (dsCategory.Tables[0].Rows[0][0].ToString() == "INSERTED")
                        {
                           txtCostCenterName.Text = string.Empty;
                           txtCostCenterDescription.Text = string.Empty;
                            chkActive.Checked = true;
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Cost Center Inserted Successfully !!";
                            
                        }
                        else if (dsCategory.Tables[0].Rows[0][0].ToString() == "UPDATED")
                        {
                            txtCostCenterName.Text = string.Empty;
                            txtCostCenterDescription.Text = string.Empty;
                            chkActive.Checked = true;
                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Cost Center Updated Successfully !!";
                            
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

        # region grvCostCenterList_RowCommand
        protected void grvCostCenterList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //#region EDIT
              //  if (e.CommandName == "Edit")
              //  {
                //    lblStatus.Text = "";
                //    int RowIndex = Convert.ToInt32(e.CommandArgument);
                //    Label lblCatID = (Label)grvCostCenterList.Rows[RowIndex].FindControl("lblCategoryID");
                //    Label lblCatName = (Label)grvCostCenterList.Rows[RowIndex].FindControl("lblCategoryName");
                //    Label lblCatDesc = (Label)grvCostCenterList.Rows[RowIndex].FindControl("lblCategoryDesc");
                //    //Session["srnum"] = int.Parse(((Label)(grvCostCenterList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());

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
                ////    int srno = int.Parse(((Label)(grvCostCenterList.Rows[RowIndex].FindControl("lblSrNo"))).Text.ToString());
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
        # endregion grvCostCenterList_RowCommand

        # region grvCostCenterList_RowEditing
        protected void grvCostCenterList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvCostCenterList_RowEditing

        # region grvCostCenterList_PageIndexChanging
        protected void grvCostCenterList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                lblStatus.Text = "";

                DataTable dsnew = (DataTable)Session["dsCostCenterMaster"];
                grvCostCenterList.PageIndex = e.NewPageIndex;
                grvCostCenterList.DataSource = dsnew; //ds.Copy();
                grvCostCenterList.DataBind();


                for (int j = 0; j < grvCostCenterList.Rows.Count; j++)
                {
                    if (((Label)(grvCostCenterList.Rows[j].FindControl("lblStatus"))).Text.ToString() == "True")
                    {
                        ((Label)(grvCostCenterList.Rows[j].FindControl("lblStatus"))).Text = "Active";
                    }
                    //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                    else if (((Label)(grvCostCenterList.Rows[j].FindControl("lblStatus"))).Text.ToString() == "False")
                    {
                        ((Label)(grvCostCenterList.Rows[j].FindControl("lblStatus"))).Text = "InActive";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion grvCostCenterList_PageIndexChanging

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
                for (int i = 0; i < grvCostCenterList.Rows.Count; i++)
                {

                    if (((CheckBox)grvCostCenterList.Rows[i].FindControl("chkRow")).Checked == true)
                    {
                        chkdel = true;
                        string txtCatName = ((TextBox)(grvCostCenterList.Rows[i].FindControl("grdtxtCostCenterName"))).Text;
                        
                        #region Prepare Parameters
                        object[] CategoryListInfo = new object[1];
                        int j = 0;
                        //0
                        CategoryListInfo.SetValue(txtCatName, j);
                        j++;
                        #endregion Prepare Parameters

                        //DataSet dsDel = objBAL.DeleteCategory(CategoryListInfo);

                        dsDel = objBAL.DeleteCostCenter(CategoryListInfo);

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
        private void AddList()
        {
            DataTable dtNewList = new DataTable();
            if (Session["dsCostCenterMaster"] == null)
            {
                dtNewList = null;
            }
            else
            {
                dtNewList = (DataTable)Session["dsCostCenterMaster"];
            }
            if (dtNewList == null)
            {
                dtNewList = new DataTable();
                dtNewList.Columns.Add("CostCenterID");
                dtNewList.Columns.Add("CostCenterName");
                dtNewList.Columns.Add("CostCenterDescription");
                dtNewList.Columns.Add("UpdatedBy");
                dtNewList.Columns.Add("UpdatedOn");
                dtNewList.Columns.Add("IsActive", typeof(bool));


            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
                DataRow l_Datarow = dtNewList.NewRow();
                l_Datarow["CostCenterID"] = "";
                l_Datarow["CostCenterName"] = "";
                l_Datarow["CostCenterDescription"] = "";
                l_Datarow["UpdatedBy"] = "";
                l_Datarow["UpdatedOn"] ="";
                l_Datarow["IsActive"] = 1;
                dtNewList.Rows.Add(l_Datarow);

                grvCostCenterList.DataSource = dtNewList;
                grvCostCenterList.DataBind();
                Session["dsCostCenterMaster"] = dtNewList;


                for (int j = 0; j < grvCostCenterList.Rows.Count; j++)
                {
                    ((TextBox)(grvCostCenterList.Rows[j].FindControl("grdtxtUpdatedOn"))).Enabled = false;
                    ((TextBox)(grvCostCenterList.Rows[j].FindControl("grdtxtUpdatedBy"))).Enabled = false;

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
            if (Session["dsCostCenterMaster"] == null)
            {
                dtNewList = null;
            }
            else
            {
                dtNewList = (DataTable)Session["dsCostCenterMaster"];
            }
            if (dtNewList == null)
            {
                dtNewList = new DataTable();
                dtNewList.Columns.Add("CostCenterID");
                dtNewList.Columns.Add("CostCenterName");
                dtNewList.Columns.Add("CostCenterDescription");
                dtNewList.Columns.Add("UpdatedBy");
                dtNewList.Columns.Add("UpdatedOn");
                dtNewList.Columns.Add("IsActive", typeof(bool));


            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
               
                   DataRow l_Datarow = dtNewList.NewRow();
                    l_Datarow["CostCenterID"] = "";
                    l_Datarow["CostCenterName"] = "";
                    l_Datarow["CostCenterDescription"] = "";
                    l_Datarow["UpdatedBy"] = Session["UserName"].ToString();
                    l_Datarow["UpdatedOn"] = Session["IT"].ToString();
                    l_Datarow["IsActive"] = 1;
                    dtNewList.Rows.Add(l_Datarow);

                    grvCostCenterList.DataSource = dtNewList;
                    grvCostCenterList.DataBind();
                    Session["dsCostCenterMaster"] = dtNewList;
              
                for (int j = 0; j < grvCostCenterList.Rows.Count; j++)
                {
                    ((TextBox)(grvCostCenterList.Rows[j].FindControl("grdtxtUpdatedOn"))).Enabled = false;
                    ((TextBox)(grvCostCenterList.Rows[j].FindControl("grdtxtUpdatedBy"))).Enabled = false;

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
            
                for (int j = 0; j < grvCostCenterList.Rows.Count; j++)
                {
                    if (((CheckBox)(grvCostCenterList.Rows[j].FindControl("chkRow"))).Checked==true )
                    
                        data =(((TextBox)(grvCostCenterList.Rows[j].FindControl("grdtxtCostCenterName"))).Text.ToString());
                    
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

