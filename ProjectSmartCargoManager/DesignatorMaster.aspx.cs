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
    public partial class DesignatorMaster : System.Web.UI.Page
    {

        #region Variable

        BALDesigMaster objBAL = new BALDesigMaster();

        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {

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
                        if (txtCode.Text == ""||txtPrefix.Text=="")
                        {
                            lblStatus.Text = "Please Enter Both Values";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        #region Prepare Parameters
                        object[] DesigCodeInfo = new object[3];
                        int i = 0;
                        
                        //0
                        DesigCodeInfo.SetValue(txtPrefix.Text, i);
                        i++;

                        //1
                        DesigCodeInfo.SetValue(txtCode.Text, i);
                        i++;

                        if (chkAct.Checked == true)
                        {
                            DesigCodeInfo.SetValue(true, i);
                            i++;
                        }
                        else
                        {
                            DesigCodeInfo.SetValue(false, i);
                            i++;
                        }

                        #endregion Prepare Parameters

                        int ID = 0;
                        ID = objBAL.AddDesigCode(DesigCodeInfo);
                        if (ID >= 0)
                        {
                            btnList_Click(sender, e);
                            lblStatus.Text = "Record Added Sucessfully..";
                            lblStatus.ForeColor = Color.Green;
                            txtCode.Text = txtPrefix.Text = string.Empty;
                            chkAct.Checked = false;
                            btnSave.Text = "Save";
                            
                            
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Record Insertion Failed..";
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
                        object[] updateDesigInfo = new object[4];
                        int i = 0;
                       
                        string serialno = Session["SerialNo"].ToString();
                       
                        //0
                        updateDesigInfo.SetValue(txtPrefix.Text, i);
                        i++;

                        //1
                        updateDesigInfo.SetValue(txtCode.Text, i);
                        i++;

                        //2
                        if (chkAct.Checked == true)
                        {
                            updateDesigInfo.SetValue("True", i);
                            i++;
                        }
                        else
                        {
                            updateDesigInfo.SetValue("False", i);
                            i++;
                        }

                        //3
                        updateDesigInfo.SetValue(serialno, i);
                        i++;

                       
                        #endregion Prepare Parameters

                        int UpdateID = 0;
                        UpdateID = objBAL.UpdateDesigCode(updateDesigInfo);
                        if (UpdateID >= 0)
                        {
                            btnList_Click(sender, e);
                            lblStatus.Text = "Record Updated Sucessfully..";
                            lblStatus.ForeColor = Color.Green;
                            txtCode.Text = txtPrefix.Text = string.Empty;
                            chkAct.Checked = false;
                            btnSave.Text = "Save";
                           
                           
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Record Updation Failed..";
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
            txtCode.Text = txtPrefix.Text = string.Empty;
            chkAct.Checked = false;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
        }
        #endregion btnClear_Click


        # region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                DataSet ds = new DataSet();
                ds = objBAL.GetDesigCodeList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdDesig.PageIndex = 0;
                                grdDesig.DataSource = ds;
                                grdDesig.DataMember = ds.Tables[0].TableName;
                                grdDesig.DataBind();
                                grdDesig.Visible = true;
                                ds.Clear();

                                for (int j = 0; j < grdDesig.Rows.Count; j++)
                                {
                                    if (((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text.ToString() == "True")
                                    {
                                        ((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text = "Active";
                                    }
                                    
                                    else if (((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text.ToString() == "False")
                                    {
                                        ((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text = "InActive";
                                    }
                                }

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

        # region grdDesig_RowCommand
        protected void grdDesig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);
                    Label prefix = (Label)grdDesig.Rows[RowIndex].FindControl("lblPrefix");
                    Label code = (Label)grdDesig.Rows[RowIndex].FindControl("lblCode");
                    Label stat = (Label)grdDesig.Rows[RowIndex].FindControl("lblIsAct");
                    Session["SerialNo"] = ((Label)(grdDesig.Rows[RowIndex].FindControl("lblSerialNo"))).Text.ToString();


                    txtCode.Text = code.Text;
                    txtPrefix.Text = prefix.Text;

                    if (stat.Text == "Active")
                    {
                        chkAct.Checked = true;
                    }

                    if (stat.Text == "InActive")
                    {
                        chkAct.Checked = false;
                    }
                    btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {

            }

        }
        # endregion grdDesig_RowCommand


        # region grdDesig_RowEditing
        protected void grdDesig_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grdDesig_RowEditing


        # region grdDesig_PageIndexChanging
        protected void grdDesig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                lblStatus.Text = "";

                #region Prepare Parameters
                object[] CommodityListInfo = new object[1];
                int i = 0;

                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.GetDesigCodeList();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdDesig.PageIndex = 0;
                                grdDesig.DataSource = ds;
                                grdDesig.DataMember = ds.Tables[0].TableName;
                                grdDesig.DataBind();
                                grdDesig.Visible = true;
                                ds.Clear();
                             }
                        }
                    }
                }

                grdDesig.PageIndex = e.NewPageIndex;
                grdDesig.DataSource = ds.Copy();
                grdDesig.DataBind();


                for (int j = 0; j < grdDesig.Rows.Count; j++)
                {
                    if (((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text.ToString() == "True")
                    {
                        ((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text = "Active";
                    }
                   
                    else if (((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text.ToString() == "False")
                    {
                        ((Label)(grdDesig.Rows[j].FindControl("lblIsAct"))).Text = "InActive";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion grdDesig_PageIndexChanging


    }
}
