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
    public partial class frmProductTypeMaster : System.Web.UI.Page
    {

        #region Variable

        BALProductType objBAL = new BALProductType();
        LoginBL lBal = new LoginBL();
        
        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
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

        }

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
                        if (txtCommodityCode.Text == "")
                        {
                            lblStatus.Text = "Please Enter Product Type";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        #region Prepare Parameters
                        object[] ProductTypeInfo = new object[8];
                        int i = 0;
                        string creationTime = Session["IT"].ToString();
                        string userName = Session["UserName"].ToString();

                        //0
                        ProductTypeInfo.SetValue(txtCommodityCode.Text.Trim(), i);
                        i++;

                        //1
                        if (chkActive.Checked == true)
                        {
                            ProductTypeInfo.SetValue(true, i);
                            i++;
                        }
                        else
                        {
                            ProductTypeInfo.SetValue(false, i);
                            i++;
                        }

                        //2
                        ProductTypeInfo.SetValue(creationTime, i);
                        i++;

                        //3
                        ProductTypeInfo.SetValue(userName, i);
                        i++;

                        //4
                        ProductTypeInfo.SetValue(txtProductDescription.Text, i);
                        i++;

                        //5
                        ProductTypeInfo.SetValue(chkMail.Checked, i);
                        i++;
                        ProductTypeInfo.SetValue(chkShipper.Checked, i);
                      i++;
                      int priority = 0;
                      if (txtPriority.Text == "")
                      {
                          ProductTypeInfo.SetValue(priority, i);

                      }
                      else
                      {
                          ProductTypeInfo.SetValue(txtPriority.Text, i);
                      }
                        #endregion Prepare Parameters

                        int ID = 0;
                        DataSet ds = objBAL.AddProductType(ProductTypeInfo);
                        if (ds.Tables[0].Rows[0][0].ToString().ToUpper()=="INSERT")
                        {
                            #region For Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int j = 0;

                            //1
                            Params.SetValue("Product Type", j);
                            j++;

                            //2
                            Params.SetValue(txtCommodityCode.Text, j);
                            j++;

                            //3

                            Params.SetValue("ADD", j);
                            j++;

                            //4

                            Params.SetValue("", j);
                            j++;


                            //5

                            Params.SetValue("", j);
                            j++;

                            //6

                            Params.SetValue(Session["UserName"], j);
                            j++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                            j++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion

                            txtCommodityCode.Text = string.Empty;
                            txtProductDescription.Text = string.Empty;
                            chkActive.Checked = false;
                            chkMail.Checked = false;
                            btnSave.Text = "Save";
                            lblStatus.Text = string.Empty;

                            btnList_Click(sender, e);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Product Type Added Sucessfully..";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Product Type already exists.";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                # endregion Save

                # region Update
                if (btnSave.Text == "Update")
                {
                    try
                    {
                        #region Prepare Parameters
                        object[] updateProductTypeInfo = new object[9];
                        int i = 0;
                        string creationTime = Session["IT"].ToString();
                        string userName = Session["UserName"].ToString();
                        string serialno = Session["SerialNo"].ToString();
                        //0
                        updateProductTypeInfo.SetValue(txtCommodityCode.Text.Trim(), i);
                        i++;

                        //1
                        if (chkActive.Checked == true)
                        {
                            updateProductTypeInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateProductTypeInfo.SetValue("False", i);
                            i++;
                        }

                        //2
                        updateProductTypeInfo.SetValue(creationTime, i);
                        i++;

                        //3
                        updateProductTypeInfo.SetValue(userName, i);
                        i++;

                        //4
                        updateProductTypeInfo.SetValue(serialno, i);
                        i++;

                        //5
                        updateProductTypeInfo.SetValue(txtProductDescription.Text, i);
                        i++;

                        updateProductTypeInfo.SetValue(chkMail.Checked, i);
                        i++;
                        updateProductTypeInfo.SetValue(chkShipper.Checked, i);
                        i++;

                        int priority = 0;
                        if (txtPriority.Text == "")
                        {
                            updateProductTypeInfo.SetValue(priority, i);

                        }
                        else
                        {
                            updateProductTypeInfo.SetValue(txtPriority.Text, i);
                        }
                        
                       
                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateProductType(updateProductTypeInfo);
                        if (UpdateID >= 0)
                        {
                            #region For Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] Params = new object[7];
                            int j = 0;

                            //1
                            Params.SetValue("Product Type", j);
                            j++;

                            //2
                            Params.SetValue(txtCommodityCode.Text, j);
                            j++;

                            //3

                            Params.SetValue("UPDATE", j);
                            j++;

                            //4

                            Params.SetValue("", j);
                            j++;


                            //5

                            Params.SetValue("", j);
                            j++;

                            //6

                            Params.SetValue(Session["UserName"], j);
                            j++;

                            //7
                            Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                            j++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Params);
                            #endregion

                            txtCommodityCode.Text = string.Empty;
                            txtProductDescription.Text = string.Empty;
                            chkActive.Checked = false;
                            chkMail.Checked = false;
                            chkShipper.Checked = false;
                            btnSave.Text = "Save";
                            lblStatus.Text = string.Empty;
                            txtPriority.Text = string.Empty;
                            btnList_Click(sender, e);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Product Type Updated Sucessfully..";

                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Product Type Updation Failed..";
                        }
                    }
                    catch (Exception ex)
                    {

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
            txtCommodityCode.Text = string.Empty;
            chkActive.Checked = false;
            chkMail.Checked = false;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
            txtProductDescription.Text = "";

            grvCommodityList.DataSource = null;
            grvCommodityList.DataBind();
            txtPriority.Text = string.Empty;
        }
        #endregion btnClear_Click

        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters
                object[] Params = new object[3];
                int i = 0;

                //0
                Params.SetValue(txtCommodityCode.Text, i);
                i++;

                //1
                Params.SetValue(txtProductDescription.Text, i);
                i++;
                //2

                Params.SetValue(txtPriority.Text, i);

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetProductTypeList(Params);

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
                                //ds.Clear();
                                Session["ds"] = ds;

                                txtCommodityCode.Text = string.Empty;
                                chkActive.Checked = false;
                                btnSave.Text = "Save";
                                lblStatus.Text = string.Empty;


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
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Records does not exists..";
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
            try
            {
                #region EDIT
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = string.Empty;
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label lblCommCode = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblCommodityCode");
                    Label lblstat = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblStatus");
                    Session["SerialNo"] = ((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblSerialNo"))).Text.ToString();
                    string Mail = ((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblMail"))).Text.ToString();
                    Label lblShipper = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblShipper");
                    Label lblPriority = (Label)grvCommodityList.Rows[RowIndex].FindControl("lblPriority");
                   
                    txtCommodityCode.Text = lblCommCode.Text;

                    if (lblstat.Text == "Active")
                    {
                        chkActive.Checked = true;
                    }

                    if (lblstat.Text == "InActive")
                    {
                        chkActive.Checked = false;
                    }
                    //Set product description.
                    txtProductDescription.Text = ((Label)grvCommodityList.Rows[RowIndex].FindControl("lblProductDescription")).Text;

                    if (Mail.ToUpper() == "TRUE")
                        chkMail.Checked = true;
                    else
                        chkMail.Checked = false;

                    if (lblShipper.Text == "Active")
                    {
                       chkShipper.Checked = true;
                    }

                    if (lblShipper.Text == "InActive")
                    {
                        chkShipper.Checked = false;
                    }
                    txtPriority.Text = lblPriority.Text;
                    btnSave.Text = "Update";

                }
                #endregion EDIT

                //Added 26th Sept..Unit Testing done
                #region Delete

                if (e.CommandName == "DeleteRecord")
                {
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    int srno = int.Parse(((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblSerialNo"))).Text.ToString());
                    string ProdType = ((Label)(grvCommodityList.Rows[RowIndex].FindControl("lblCommodityCode"))).Text.ToString();
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
                        int res = objBAL.DeleteProdTypeDetail(Params);
                        if (res == 0)
                        {
                            txtCommodityCode.Text = string.Empty;
                            chkActive.Checked = false;
                            btnSave.Text = "Save";
                            lblStatus.Text = string.Empty;

                            btnList_Click(null, null);
                            lblStatus.Text = "Product Type Successfully Deleted...";
                            lblStatus.ForeColor = Color.Red;

                            #region For Master Audit Log
                            MasterAuditBAL ObjMAL = new MasterAuditBAL();
                            #region Prepare Parameters
                            object[] parameter = new object[7];
                            int j = 0;

                            //1
                            parameter.SetValue("Product Type", j);
                            j++;

                            //2
                            parameter.SetValue(ProdType, j);
                            j++;

                            //3

                            parameter.SetValue("DELETE", j);
                            j++;

                            //4

                            parameter.SetValue("", j);
                            j++;


                            //5

                            parameter.SetValue("", j);
                            j++;

                            //6

                            parameter.SetValue(Session["UserName"], j);
                            j++;

                            //7
                            parameter.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), j);
                            j++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(parameter);
                            #endregion

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